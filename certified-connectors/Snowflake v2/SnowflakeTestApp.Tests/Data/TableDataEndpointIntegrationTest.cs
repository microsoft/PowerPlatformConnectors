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
