using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
        /// Test that $count=true returns the total record count in the response alongside data.
        /// With partition-based pagination, COUNT(*) is only executed when $count=true is requested.
        /// </summary>
        [TestMethod]
        public async Task GetItemsEndpoint_WithCountTrue_ReturnsODataCount()
        {
            var testToken = GetTestToken();
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {testToken}");
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await HttpClient.GetAsync($"{BaseUrl}/datasets('{TestDataset}')/tables('{TestTable}')/items?$count=true");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.IsFalse(string.IsNullOrEmpty(content), "Response content should not be empty");

            var json = JObject.Parse(content);
            var count = json["@odata.count"];
            Assert.IsNotNull(count, "Response should include @odata.count when $count=true");
            Assert.AreEqual(SeededTestData.Count, (int) count, "@odata.count should equal total seeded records");
        }

        /// <summary>
        /// Test that a response without $count=true does not include @odata.count.
        /// This verifies the optimization that COUNT(*) is skipped when not requested.
        /// </summary>
        [TestMethod]
        public async Task GetItemsEndpoint_WithoutCount_DoesNotIncludeODataCount()
        {
            var testToken = GetTestToken();
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {testToken}");
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await HttpClient.GetAsync($"{BaseUrl}/datasets('{TestDataset}')/tables('{TestTable}')/items");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();

            var json = JObject.Parse(content);
            var count = json["@odata.count"];
            Assert.IsNull(count, "Response should NOT include @odata.count when $count is not requested");
        }

        /// <summary>
        /// Test that fetching items with $top returns the correct number of items
        /// and that the response includes partition-based NextLink when more data is available.
        /// Note: For small datasets, Snowflake may return all results in a single partition,
        /// so NextLink with sfStatementHandle may not appear. This test validates the basic $top behavior.
        /// </summary>
        [TestMethod]
        public async Task GetItemsEndpoint_WithTop_ReturnsLimitedResults()
        {
            var testToken = GetTestToken();
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {testToken}");
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await HttpClient.GetAsync($"{BaseUrl}/datasets('{TestDataset}')/tables('{TestTable}')/items?$top=2");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.IsFalse(string.IsNullOrEmpty(content), "Response content should not be empty");

            var json = JObject.Parse(content);
            var items = json["value"] as JArray;
            Assert.IsNotNull(items, "Response should contain a 'value' array");
            Assert.AreEqual(2, items.Count, "$top=2 should return exactly 2 items");
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
        /// Test that tolower() enables case-insensitive filtering.
        /// Seeded data has "John Doe" (mixed case). A plain eq with lowercase should return nothing
        /// because Snowflake is case-sensitive by default, but tolower(NAME) eq 'john doe' should match.
        /// </summary>
        [TestMethod]
        public async Task GetItemsEndpoint_FilterWithToLower_ReturnsCaseInsensitiveMatch()
        {
            var testToken = GetTestToken();
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {testToken}");
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var caseSensitiveResponse = await HttpClient.GetAsync(
                $"{BaseUrl}/datasets('{TestDataset}')/tables('{TestTable}')/items?$filter=NAME eq 'john doe'");
            Assert.AreEqual(HttpStatusCode.OK, caseSensitiveResponse.StatusCode, "Case-sensitive filter request should succeed");

            var caseSensitiveContent = await caseSensitiveResponse.Content.ReadAsStringAsync();
            var caseSensitiveData = JsonConvert.DeserializeObject<ODataResponse<TestDataRecord>>(caseSensitiveContent);
            Assert.IsNotNull(caseSensitiveData?.Value, "Case-sensitive filter response should be parseable");
            Assert.AreEqual(0, caseSensitiveData.Value.Count,
                "Plain eq with lowercase 'john doe' should return no results because Snowflake is case-sensitive");

            var toLowerResponse = await HttpClient.GetAsync(
                $"{BaseUrl}/datasets('{TestDataset}')/tables('{TestTable}')/items?$filter=tolower(NAME) eq 'john doe'");
            Assert.AreEqual(HttpStatusCode.OK, toLowerResponse.StatusCode, "tolower filter request should succeed");

            var toLowerContent = await toLowerResponse.Content.ReadAsStringAsync();
            var toLowerData = JsonConvert.DeserializeObject<ODataResponse<TestDataRecord>>(toLowerContent);
            Assert.IsNotNull(toLowerData?.Value, "tolower filter response should contain data");
            Assert.IsTrue(toLowerData.Value.Count > 0, "tolower(NAME) eq 'john doe' should return at least one record");
            Assert.IsTrue(
                toLowerData.Value.All(item => item.Name.Equals("John Doe")),
                "All returned records should have NAME matching 'John Doe'");

            var bothSidesResponse = await HttpClient.GetAsync(
                $"{BaseUrl}/datasets('{TestDataset}')/tables('{TestTable}')/items?$filter=tolower(NAME) eq tolower('jOhN DOe')");
            Assert.AreEqual(HttpStatusCode.OK, bothSidesResponse.StatusCode, "tolower on both sides should succeed");

            var bothSidesContent = await bothSidesResponse.Content.ReadAsStringAsync();
            var bothSidesData = JsonConvert.DeserializeObject<ODataResponse<TestDataRecord>>(bothSidesContent);
            Assert.IsNotNull(bothSidesData?.Value, "tolower both-sides response should contain data");
            Assert.IsTrue(bothSidesData.Value.Count > 0,
                "tolower(NAME) eq tolower('jOhN DOe') should return at least one record");
            Assert.IsTrue(
                bothSidesData.Value.All(item => item.Name.Equals("John Doe")),
                "All returned records should have NAME matching 'John Doe'");
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
        /// Test inserting a record with null column values via POST.
        /// </summary>
        [TestMethod]
        public async Task CreateItemEndpoint_WithNullValues_ReturnsCreated()
        {
            string createTableSQL = "CREATE OR REPLACE TABLE NULL_INSERT_TEST (" +
                                    "  ID NUMBER(38,0) NOT NULL," +
                                    "  NAME VARCHAR(16777216) NOT NULL," +
                                    "  DESCRIPTION VARCHAR(16777216)," +
                                    "  SCORE NUMBER(38,0)," +
                                    "  PRIMARY KEY (ID)" +
                                    ");";
            DataSeeder.ExecuteSqlStatement(createTableSQL).GetAwaiter().GetResult();

            var testToken = GetTestToken();
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {testToken}");
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var newItem = new
            {
                ID = 1,
                NAME = "NullInsertTest",
                DESCRIPTION = (string) null,
                SCORE = (int?) null
            };

            var response = await HttpClient.PostAsync(
                $"{BaseUrl}/datasets('{TestDataset}')/tables('NULL_INSERT_TEST')/items",
                CreateJsonContent(newItem));

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode,
                "POST with null column values should succeed");

            var readResponse = await HttpClient.GetAsync(
                $"{BaseUrl}/datasets('{TestDataset}')/tables('NULL_INSERT_TEST')/items('1')");
            Assert.AreEqual(HttpStatusCode.OK, readResponse.StatusCode);

            var content = await readResponse.Content.ReadAsStringAsync();
            var record = JObject.Parse(content);

            Assert.AreEqual(1, (int) record["ID"]);
            Assert.AreEqual("NullInsertTest", (string) record["NAME"]);
            Assert.IsTrue(record["DESCRIPTION"].Type == JTokenType.Null,
                "DESCRIPTION should be null in the created record");
            Assert.IsTrue(record["SCORE"].Type == JTokenType.Null,
                "SCORE should be null in the created record");
        }

        /// <summary>
        /// Test updating an existing column to null via PUT.
        /// </summary>
        [TestMethod]
        public async Task UpdateItemEndpoint_WithNullValue_ReturnsOk()
        {
            string createTableSQL = "CREATE OR REPLACE TABLE NULL_UPDATE_TEST (" +
                                    "  ID NUMBER(38,0) NOT NULL," +
                                    "  NAME VARCHAR(16777216) NOT NULL," +
                                    "  DESCRIPTION VARCHAR(16777216)," +
                                    "  SCORE NUMBER(38,0)," +
                                    "  PRIMARY KEY (ID)" +
                                    ");";
            DataSeeder.ExecuteSqlStatement(createTableSQL).GetAwaiter().GetResult();

            string insertSQL = "INSERT INTO DATAVERSE.PUBLIC.NULL_UPDATE_TEST(ID, NAME, DESCRIPTION, SCORE) " +
                               "VALUES(1, 'Original', 'Has a description', 42);";
            DataSeeder.ExecuteSqlStatement(insertSQL).GetAwaiter().GetResult();

            var testToken = GetTestToken();
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {testToken}");
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Verify the record exists with non-null values first
            var beforeResponse = await HttpClient.GetAsync(
                $"{BaseUrl}/datasets('{TestDataset}')/tables('NULL_UPDATE_TEST')/items('1')");
            Assert.AreEqual(HttpStatusCode.OK, beforeResponse.StatusCode);

            var beforeContent = await beforeResponse.Content.ReadAsStringAsync();
            var beforeRecord = JObject.Parse(beforeContent);
            Assert.AreEqual("Has a description", (string) beforeRecord["DESCRIPTION"],
                "DESCRIPTION should initially be non-null");
            Assert.AreEqual(42, (int) beforeRecord["SCORE"],
                "SCORE should initially be 42");

            // Update: set DESCRIPTION and SCORE to null
            var updatedItem = new
            {
                ID = 1,
                NAME = "Original",
                DESCRIPTION = (string) null,
                SCORE = (int?) null
            };

            var updateResponse = await HttpClient.PutAsync(
                $"{BaseUrl}/datasets('{TestDataset}')/tables('NULL_UPDATE_TEST')/items('1')",
                CreateJsonContent(updatedItem));

            Assert.AreEqual(HttpStatusCode.OK, updateResponse.StatusCode,
                "PUT with null column values should succeed");

            // Verify the columns are now null
            var afterResponse = await HttpClient.GetAsync(
                $"{BaseUrl}/datasets('{TestDataset}')/tables('NULL_UPDATE_TEST')/items('1')");
            Assert.AreEqual(HttpStatusCode.OK, afterResponse.StatusCode);

            var afterContent = await afterResponse.Content.ReadAsStringAsync();
            var afterRecord = JObject.Parse(afterContent);

            Assert.AreEqual(1, (int) afterRecord["ID"]);
            Assert.AreEqual("Original", (string) afterRecord["NAME"]);
            Assert.IsTrue(afterRecord["DESCRIPTION"].Type == JTokenType.Null,
                "DESCRIPTION should be null after update");
            Assert.IsTrue(afterRecord["SCORE"].Type == JTokenType.Null,
                "SCORE should be null after update");
        }

        /// <summary>
        /// Creates a table with 1000 rows containing large padding data to trigger multiple
        /// Snowflake result partitions, then iterates all pages via @odata.nextLink.
        /// Validates:
        ///   - NextLink contains $skiptoken in handle~partition~total format
        ///   - $top decreases as rows are consumed across pages
        ///   - All 1000 rows are retrieved with unique, contiguous IDs
        /// </summary>
        [TestMethod]
        public async Task GetItemsEndpoint_LargeDataset_IteratesPartitionsViaNextLink()
        {
            const string tableName = "PAGINATION_TEST";
            const int totalRows = 1000;

            string createTableSQL = "CREATE OR REPLACE TABLE " + tableName + " (" +
                                    "  ID NUMBER(38,0) NOT NULL," +
                                    "  LABEL VARCHAR(255) NOT NULL," +
                                    "  PADDING VARCHAR(16777216)," +
                                    "  PRIMARY KEY (ID)" +
                                    ");";
            DataSeeder.ExecuteSqlStatement(createTableSQL).GetAwaiter().GetResult();

            string insertSQL = "INSERT INTO " + tableName + " (ID, LABEL, PADDING) " +
                               "SELECT " +
                               "  ROW_NUMBER() OVER (ORDER BY SEQ4()) AS ID, " +
                               "  'Row_' || ROW_NUMBER() OVER (ORDER BY SEQ4()) AS LABEL, " +
                               "  REPEAT(MD5(RANDOM()::VARCHAR), 1563) AS PADDING " +
                               "FROM TABLE(GENERATOR(ROWCOUNT => " + totalRows + "));";
            DataSeeder.ExecuteSqlStatement(insertSQL).GetAwaiter().GetResult();

            var testToken = GetTestToken();
            HttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {testToken}");
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var allItems = new List<JObject>();
            int pageCount = 0;

            string url = $"{BaseUrl}/datasets('{TestDataset}')/tables('{tableName}')/items?$top={totalRows}";

            while (url != null)
            {
                pageCount++;
                Assert.IsTrue(pageCount <= 50, "Safety limit: too many pages, possible infinite loop");

                var response = await HttpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode,
                    $"Page {pageCount} should return OK. Response: {content}");
                var json = JObject.Parse(content);

                var items = json["value"] as JArray;
                Assert.IsNotNull(items, $"Page {pageCount} should contain a 'value' array");
                Assert.IsTrue(items.Count > 0, $"Page {pageCount} should have at least one item");

                foreach (var item in items)
                {
                    allItems.Add((JObject)item);
                }

                var nextLink = json["@odata.nextLink"]?.ToString();

                if (nextLink != null)
                {
                    ValidateNextLinkFormat(nextLink, pageCount, totalRows);
                }

                url = nextLink;
            }

            var distinctIds = allItems.Select(i => (int)i["ID"]).Distinct().OrderBy(id => id).ToList();

            Assert.AreEqual(totalRows, allItems.Count,
                $"Should retrieve all {totalRows} rows across {pageCount} page(s)");
            Assert.AreEqual(totalRows, distinctIds.Count, "All IDs should be unique");
            CollectionAssert.AreEqual(
                Enumerable.Range(1, totalRows).ToList(),
                distinctIds,
                "IDs should be contiguous from 1 to " + totalRows);

            Assert.IsTrue(pageCount > 1,
                "Expected multiple pages (partitions) for the large dataset. " +
                "If this fails, increase PADDING size to trigger Snowflake partitioning.");
        }

        private static void ValidateNextLinkFormat(string nextLink, int pageNumber, int originalTop)
        {
            var nextUri = new Uri(nextLink);
            var queryParams = HttpUtility.ParseQueryString(nextUri.Query);

            var skipToken = queryParams["$skiptoken"];
            Assert.IsNotNull(skipToken, $"NextLink on page {pageNumber} should contain $skiptoken");

            var parts = skipToken.Split('~');
            Assert.AreEqual(3, parts.Length,
                $"$skiptoken should have format 'handle~partition~total', got: {skipToken}");

            Assert.IsFalse(string.IsNullOrEmpty(parts[0]),
                "Statement handle in $skiptoken should not be empty");
            Assert.IsTrue(int.TryParse(parts[1], out int partitionIndex),
                $"Partition index should be an integer, got: {parts[1]}");
            Assert.IsTrue(int.TryParse(parts[2], out int totalPartitions),
                $"Total partitions should be an integer, got: {parts[2]}");
            Assert.IsTrue(partitionIndex > 0 && partitionIndex < totalPartitions,
                $"Partition index ({partitionIndex}) should be between 1 and {totalPartitions - 1}");

            var topParam = queryParams["$top"];
            Assert.IsNotNull(topParam, $"NextLink on page {pageNumber} should contain $top");
            Assert.IsTrue(int.TryParse(topParam, out int topValue), "$top should be a valid integer");
            Assert.IsTrue(topValue > 0 && topValue < originalTop,
                $"$top ({topValue}) should be between 1 and {originalTop - 1}");
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
