using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SnowflakeTestApp.Tests.Infrastructure;

namespace SnowflakeTestApp.Tests.Data
{
    /// <summary>
    /// Integration tests for table data endpoints (items CRUD operations).
    /// These tests document the expected behavior and can be used to verify the endpoints manually.
    /// This test class uses the globally seeded CUSTOMERS test table.
    /// </summary>
    [TestClass]
    public class TableDataEndpointIntegrationTest : BaseIntegrationTest
    {
        private const string TestTable = "CUSTOMERS";
        private const string TestDataset = "default";

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
            EnsureApplicationIsRunning();
        }

        /// <summary>
        /// Test the GET /datasets/{dataset}/tables/{table}/items endpoint with authentication
        /// This test uses the automatically seeded test data and validates against TestDataRecord objects
        /// </summary>
        [TestMethod]
        public async Task GetItemsEndpoint_WithAuth_ReturnsOk()
        {
            var testToken = GetTestToken();
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {testToken}");
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await HttpClient.GetAsync($"{BaseUrl}/datasets('{TestDataset}')/tables('{TestTable}')/items");
            
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.IsFalse(string.IsNullOrEmpty(content), "Response content should not be empty");

            var customers = JsonConvert.DeserializeObject<ODataResponse<TestDataRecord>>(content).Value;
            
            Assert.AreEqual(SeededTestData.Count, customers.Count, "API should return the same number of records as seeded data");

            var expectedInactiveRecord = SeededTestData.FirstOrDefault(r => !r.IsActive && r.Balance > 100.00m);
            Assert.IsNotNull(expectedInactiveRecord, "Should have at least one inactive record in seeded data");
            
            var actualInactiveRecord = customers.FirstOrDefault(r => r.Id == expectedInactiveRecord.Id);
            Assert.IsNotNull(actualInactiveRecord, $"{expectedInactiveRecord.Name} record should exist in API response");
            Assert.IsFalse(actualInactiveRecord.IsActive, $"{expectedInactiveRecord.Name} should be inactive");
            Assert.AreEqual(expectedInactiveRecord.Balance, actualInactiveRecord.Balance, $"{expectedInactiveRecord.Name} should have correct balance");
        }

        /// <summary>
        /// Test the GET items endpoint without authentication
        /// </summary>
        [TestMethod]
        public async Task GetItemsEndpoint_WithoutAuth_ReturnsInternalServerError()
        {
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await HttpClient.GetAsync($"{BaseUrl}/datasets('{TestDataset}')/tables('{TestTable}')/items");
            
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }

                /// <summary>
        /// Test the GET /datasets/{dataset}/tables/{table}/items/{id} endpoint with authentication
        /// This test uses the first active record from seeded data and validates the response
        /// </summary>
        [TestMethod]
        public async Task GetItemEndpoint_WithAuth_ReturnsOk()
        {
            // Get the first active record from seeded data to test with
            var expectedRecord = SeededTestData.FirstOrDefault(r => r.IsActive);
            Assert.IsNotNull(expectedRecord, "Should have at least one active record in seeded data");

            var testToken = GetTestToken();
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {testToken}");
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await HttpClient.GetAsync($"{BaseUrl}/datasets('{TestDataset}')/tables('{TestTable}')/items('{expectedRecord.Id}')");
            
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.IsFalse(string.IsNullOrEmpty(content), "Response content should not be empty");

            var customerItem = JsonConvert.DeserializeObject<TestDataRecord>(content);
            Assert.IsNotNull(customerItem, "Customer item should not be null");

            ValidateRecordMatches(expectedRecord, customerItem, "Single item API response");

            Assert.AreEqual(expectedRecord.Name, customerItem.Name, $"Should be {expectedRecord.Name} record");
            Assert.AreEqual(expectedRecord.Email, customerItem.Email, "Should have correct email from seeded data");
            Assert.AreEqual(expectedRecord.Phone, customerItem.Phone, "Should have correct phone from seeded data");
            Assert.AreEqual(expectedRecord.IsActive, customerItem.IsActive, "Active status should match seeded data");
            Assert.AreEqual(expectedRecord.Balance, customerItem.Balance, "Should have correct balance from seeded data");

            Assert.IsNotNull(customerItem.CreatedDate, "CREATED_DATE should be present");
        }

        /// <summary>
        /// Test the GET single item endpoint without authentication
        /// </summary>
        [TestMethod]
        public async Task GetItemEndpoint_WithoutAuth_ReturnsInternalServerError()
        {
            var testRecord = SeededTestData.Skip(1).FirstOrDefault();
            Assert.IsNotNull(testRecord, "Should have at least 2 records in seeded data");

            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await HttpClient.GetAsync($"{BaseUrl}/datasets('{TestDataset}')/tables('{TestTable}')/items('{testRecord.Id}')");
            
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        /// <summary>
        /// Test the POST /datasets/{dataset}/tables/{table}/items endpoint with authentication
        /// This test creates a new item based on a template from seeded data
        /// </summary>
        [TestMethod]
        public async Task CreateItemEndpoint_WithAuth_ReturnsCreated()
        {
            var templateRecord = SeededTestData.OrderByDescending(r => r.Balance).FirstOrDefault();
            Assert.IsNotNull(templateRecord, "Should have seeded data to use as template");

            var testToken = GetTestToken();
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {testToken}");
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var newId = SeededTestData.Max(r => r.Id) + 50;
            var newItem = new TestDataRecord(
                newId, 
                $"New {templateRecord.Name}", 
                templateRecord.Email.Replace("@", "+create@"), 
                templateRecord.Phone.Replace("555", "777"), 
                templateRecord.IsActive, 
                templateRecord.Balance * 0.8m
            );

            var response = await HttpClient.PostAsync($"{BaseUrl}/datasets('{TestDataset}')/tables('{TestTable}')/items", 
                CreateJsonContent(newItem));

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode, 
                $"Should successfully create record based on template: {templateRecord.Name}");
        }

        /// <summary>
        /// Test the POST create item endpoint without authentication
        /// 
        /// 
        /// </summary>
        [TestMethod]
        public async Task CreateItemEndpoint_WithoutAuth_ReturnsInternalServerError()
        {
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            var testItem = new { NAME = "Test Customer" };
            var json = JsonConvert.SerializeObject(testItem);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await HttpClient.PostAsync($"{BaseUrl}/datasets('{TestDataset}')/tables('{TestTable}')/items", content);
            
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        /// <summary>
        /// Test the PUT /datasets/{dataset}/tables/{table}/items({id}) endpoint with authentication
        /// This test updates an existing item using data from the third seeded record
        /// </summary>
        [TestMethod]
        public async Task UpdateItemEndpoint_WithAuth_ReturnsOk()
        {
            var recordToUpdate = SeededTestData.Skip(2).FirstOrDefault();
            Assert.IsNotNull(recordToUpdate, "Should have at least 3 records in seeded data");

            var testToken = GetTestToken();
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {testToken}");
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var updatedItem = new
            {
                ID = recordToUpdate.Id,
                NAME = $"Updated {recordToUpdate.Name}",
                EMAIL = recordToUpdate.Email.Replace("@", "+updated@"),
                PHONE = recordToUpdate.Phone,
                IS_ACTIVE = !recordToUpdate.IsActive, 
                BALANCE = recordToUpdate.Balance + 500.00m
            };

            var json = JsonConvert.SerializeObject(updatedItem);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await HttpClient.PutAsync($"{BaseUrl}/datasets('{TestDataset}')/tables('{TestTable}')/items('{recordToUpdate.Id}')", content);
            
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        /// <summary>
        /// Test the PUT update item endpoint without authentication
        /// </summary>
        [TestMethod]
        public async Task UpdateItemEndpoint_WithoutAuth_ReturnsInternalServerError()
        {
            var testRecord = SeededTestData.LastOrDefault();
            Assert.IsNotNull(testRecord, "Should have seeded data available");

            var updateData = new { NAME = $"Updated {testRecord.Name}" };
            var json = JsonConvert.SerializeObject(updateData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await HttpClient.PutAsync($"{BaseUrl}/datasets('{TestDataset}')/tables('{TestTable}')/items('{testRecord.Id}')", content);
            
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        /// <summary>
        /// Test the DELETE /datasets/{dataset}/tables/{table}/items({id}) endpoint with authentication
        /// This test deletes the lowest balance record from the seeded test table
        /// </summary>
        [TestMethod]
        public async Task DeleteItemEndpoint_WithAuth_ReturnsOk()
        {
            var recordToDelete = SeededTestData.OrderBy(r => r.Balance).FirstOrDefault();
            Assert.IsNotNull(recordToDelete, "Should have seeded data with balance information");

            var testToken = GetTestToken();
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {testToken}");
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await HttpClient.DeleteAsync($"{BaseUrl}/datasets('{TestDataset}')/tables('{TestTable}')/items('{recordToDelete.Id}')");
            
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        /// <summary>
        /// Test the DELETE item endpoint without authentication
        /// </summary>
        [TestMethod]
        public async Task DeleteItemEndpoint_WithoutAuth_ReturnsInternalServerError()
        {
            var testRecord = SeededTestData.Skip(SeededTestData.Count / 2).FirstOrDefault();
            Assert.IsNotNull(testRecord, "Should have seeded data available");

            var response = await HttpClient.DeleteAsync($"{BaseUrl}/datasets('{TestDataset}')/tables('{TestTable}')/items('{testRecord.Id}')");
            
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        /// <summary>
        /// Test items endpoint with missing dataset parameter
        /// </summary>
        [TestMethod]
        public async Task GetItemsEndpoint_WithMissingDataset_ReturnsBadRequest()
        {
            var testToken = GetTestToken();
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {testToken}");

            var response = await HttpClient.GetAsync($"{BaseUrl}/datasets('')/tables('{TestTable}')/items");
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, "Expected HTTP 400 Bad Request");
        }

        /// <summary>
        /// Test items endpoint with missing table parameter
        /// </summary>
        [TestMethod]
        public async Task GetItemsEndpoint_WithMissingTable_ReturnsBadRequest()
        {
            var testToken = GetTestToken();
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {testToken}");

            var response = await HttpClient.GetAsync($"{BaseUrl}/datasets('{TestDataset}')/tables('')/items");
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode, "Expected HTTP 400 Bad Request");
        }



        /// <summary>
        /// Test OData filtering functionality by validating active records count
        /// </summary>
        [TestMethod]
        public async Task GetItemsEndpoint_FilterActiveRecords_ReturnsCorrectCount()
        {
            var testToken = GetTestToken();
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {testToken}");
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            
            var activeResponse = await HttpClient.GetAsync($"{BaseUrl}/datasets('{TestDataset}')/tables('{TestTable}')/items?$filter=IS_ACTIVE eq true");
            Assert.AreEqual(HttpStatusCode.OK, activeResponse.StatusCode, "Active filter should succeed");
            
            var activeContent = await activeResponse.Content.ReadAsStringAsync();
            var activeData = JsonConvert.DeserializeObject<ODataResponse<TestDataRecord>>(activeContent);
            Assert.IsNotNull(activeData?.Value, "Active filter response should contain data");
            
            Assert.IsTrue(activeData.Value.All(item => item.IsActive), "All filtered records should be active");

            var inactiveResponse = await HttpClient.GetAsync($"{BaseUrl}/datasets('{TestDataset}')/tables('{TestTable}')/items?$filter=IS_ACTIVE eq false");
            Assert.AreEqual(HttpStatusCode.OK, inactiveResponse.StatusCode, "Inactive filter should succeed");
            
            var inactiveContent = await inactiveResponse.Content.ReadAsStringAsync();
            var inactiveData = JsonConvert.DeserializeObject<ODataResponse<TestDataRecord>>(inactiveContent);
            Assert.IsNotNull(inactiveData?.Value, "Inactive filter response should contain data");
            Assert.IsTrue(inactiveData.Value.Count > 0, $"Should return some inactive records");
            
            Assert.IsTrue(inactiveData.Value.All(item => !item.IsActive), "All filtered records should be inactive");
        }

        /// <summary>
        /// End-to-end test validating complete CRUD operation lifecycle with database verification
        /// </summary>
        [TestMethod]
        public async Task CrudOperations_EndToEndValidation()
        {
            var testToken = GetTestToken();
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {testToken}");
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // 1. Create a new record based on seeded template
            var templateRecord = SeededTestData.OrderBy(r => r.Balance).FirstOrDefault();
            var newId = SeededTestData.Max(r => r.Id) + 200;
            
            var newRecord = new TestDataRecord(newId, $"Test {templateRecord.Name}", 
                templateRecord.Email.Replace("@", "+test@"), templateRecord.Phone.Replace("555", "999"), 
                true, templateRecord.Balance * 2);

            var createResponse = await HttpClient.PostAsync($"{BaseUrl}/datasets('{TestDataset}')/tables('{TestTable}')/items", 
                CreateJsonContent(newRecord));
            Assert.AreEqual(HttpStatusCode.Created, createResponse.StatusCode, "Create should succeed");

            // Verify creation in database
            var createdRecord = await FetchActualRecordById(newId);
            ValidateRecordMatches(newRecord, createdRecord, "Created record should match input");

            // 2. Verify we can retrieve the created record via API
            var readResponse = await HttpClient.GetAsync($"{BaseUrl}/datasets('{TestDataset}')/tables('{TestTable}')/items('{newId}')");
            Assert.AreEqual(HttpStatusCode.OK, readResponse.StatusCode, "Read should succeed");
            
            var readContent = await readResponse.Content.ReadAsStringAsync();
            var apiRecord = JsonConvert.DeserializeObject<TestDataRecord>(readContent);
            ValidateRecordMatches(newRecord, apiRecord, "API read should return correct data");

            // 3. Modify the record
            var updatedRecord = new TestDataRecord(newId, $"Modified {newRecord.Name}", 
                newRecord.Email.Replace("+test@", "+updated@"), newRecord.Phone, 
                !newRecord.IsActive, newRecord.Balance + 500m);

            var updateResponse = await HttpClient.PutAsync($"{BaseUrl}/datasets('{TestDataset}')/tables('{TestTable}')/items('{newId}')", 
                CreateJsonContent(updatedRecord));
            Assert.AreEqual(HttpStatusCode.OK, updateResponse.StatusCode, "Update should succeed");

            // Verify update in database
            var modifiedRecord = await FetchActualRecordById(newId);
            ValidateRecordMatches(updatedRecord, modifiedRecord, "Updated record should match new values");

            // 4. Remove the record
            var deleteResponse = await HttpClient.DeleteAsync($"{BaseUrl}/datasets('{TestDataset}')/tables('{TestTable}')/items('{newId}')");
            Assert.AreEqual(HttpStatusCode.OK, deleteResponse.StatusCode, "Delete should succeed");

            // Verify deletion in database
            var deletedRecord = await FetchActualRecordById(newId);
            Assert.IsNull(deletedRecord, "Record should be deleted from database");

            // Verify read after delete returns appropriate response
            var readAfterDeleteResponse = await HttpClient.GetAsync($"{BaseUrl}/datasets('{TestDataset}')/tables('{TestTable}')/items('{newId}')");
            Assert.AreEqual(HttpStatusCode.OK, readAfterDeleteResponse.StatusCode, "Read after delete should return 200 OK");
            
            // Validate that no test record values are present in the response
            var responseContent = await readAfterDeleteResponse.Content.ReadAsStringAsync();
            Assert.IsFalse(responseContent.Contains(updatedRecord.Name), "Response should not contain the deleted record's name");
            Assert.IsFalse(responseContent.Contains(updatedRecord.Email), "Response should not contain the deleted record's email");
            Assert.IsFalse(responseContent.Contains(updatedRecord.Phone), "Response should not contain the deleted record's phone");
            Assert.IsFalse(responseContent.Contains(updatedRecord.Balance.ToString()), "Response should not contain the deleted record's balance");
        }

        /// <summary>
        /// Test the GET /datasets/{dataset}/tables/{table}/items and /datasets/{dataset}/tables/{table}/items/{id} returning rows with null values
        /// </summary>
        [TestMethod]
        public async Task GetItemsEndpoints_WithNullValues_ReturnsRows()
        {
            // Crete table with null values
            string createTableSQL = "create or replace TABLE ANIMALS (" +
                                       "  ID NUMBER(38,0) NOT NULL autoincrement start 1 increment 1 noorder, " +
                                       "  NAME VARCHAR(16777216) NOT NULL," +
                                       "  AGE NUMBER(38,0)," +
                                       "  DATEADDED DATE," +
                                       "  DATEUPDATED TIMESTAMP_NTZ," +
                                       "  primary key (ID)" +
                                       " );";


            DataSeeder.ExecuteSqlStatement(createTableSQL).GetAwaiter().GetResult();

            // Insert record with null values
            string insertSQL = "insert into  DATAVERSE.PUBLIC.ANIMALS(ID, NAME, AGE, DATEADDED, DATEUPDATED) " +
                               " VALUES(1, 'Cat', 12, null, null)";

            DataSeeder.ExecuteSqlStatement(insertSQL).GetAwaiter().GetResult();


            var testToken = GetTestToken();
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {testToken}");
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await HttpClient.GetAsync($"{BaseUrl}/datasets('{TestDataset}')/tables('ANIMALS')/items");


            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.IsFalse(string.IsNullOrEmpty(content), "Response content should not be empty");

            var animals = JsonConvert.DeserializeObject<ODataResponse<TestDataRecord>>(content).Value;

            Assert.AreEqual(1, animals.Count, "API should return the same number of records as seeded data");


            response = await HttpClient.GetAsync($"{BaseUrl}/datasets('{TestDataset}')/tables('ANIMALS')/items('1')");
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Read after delete should return 200 OK");

            // Validate that no test record values are present in the response
            string responseContent = await response.Content.ReadAsStringAsync();
            
            var responseMap = JObject.Parse(responseContent);
            Assert.AreEqual(1, responseMap["ID"]);
            Assert.AreEqual("Cat", responseMap["NAME"]);
            Assert.IsTrue(responseMap.ContainsKey("DATEADDED"));
            Assert.IsTrue(responseMap.ContainsKey("DATEUPDATED"));
            Assert.IsTrue(responseMap["DATEADDED"].IsNullOrEmpty());
            Assert.IsTrue(responseMap["DATEUPDATED"].IsNullOrEmpty());
        }

        /// <summary>
        /// Helper method to create JSON content from TestDataRecord
        /// </summary>
        private StringContent CreateJsonContent(TestDataRecord record)
        {
            var json = JsonConvert.SerializeObject(new
            {
                ID = record.Id,
                NAME = record.Name,
                EMAIL = record.Email,
                PHONE = record.Phone,
                IS_ACTIVE = record.IsActive,
                BALANCE = record.Balance
            });
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
} 
