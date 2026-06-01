using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SnowflakeTestApp.Tests.Infrastructure
{
    /// <summary>
    /// Integration tests for validating TestDataRecord mapping and database operations.
    /// </summary>
    [TestClass]
    public class TestDataValidationIntegrationTest : BaseIntegrationTest
    {
        private const decimal FRANK_GARCIA_EXPECTED_BALANCE = 250.00m;
        private const decimal HIGH_BALANCE_THRESHOLD = 2000m;
        private const int NON_EXISTENT_RECORD_ID = 99999;
        private const int EXPECTED_FIRST_RECORD_ID = 1;
        private const int EXPECTED_SECOND_RECORD_ID = 2;
        private const string FRANK_GARCIA_NAME = "Frank Garcia";
        private const string ALICE_BROWN_NAME = "Alice Brown";

        /// <summary>
        /// Seeds fresh test data before any tests in this class run
        /// This ensures clean and predictable data for all test methods
        /// </summary>
        [ClassInitialize]
        public static async Task ClassInitialize(TestContext context)
        {
            if (DataSeeder != null)
            {
                await DataSeeder.EnsureTestTableExistsAndSeed(TestData.DefaultTable, TestData.DefaultDataset);
            }
        }

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
            EnsureApplicationIsRunning();
        }

        /// <summary>
        /// Test that demonstrates basic data fetching and mapping functionality
        /// </summary>
        [TestMethod]
        public async Task FetchTestDataFromDatabase_ShouldReturnMappedRecords()
        {
            var actualRecords = await FetchActualDataFromDatabase();
            
            AssertRecordsAreNotNullOrEmpty(actualRecords);
            AssertFirstRecordHasValidStructure(actualRecords.First());
        }

        /// <summary>
        /// Test that demonstrates fetching a specific record by ID
        /// </summary>
        [TestMethod]
        public async Task FetchTestRecordById_ShouldReturnCorrectRecord()
        {
            var actualRecord = await FetchActualRecordById(EXPECTED_FIRST_RECORD_ID);
            
            Assert.IsNotNull(actualRecord, "Record with ID 1 should exist");
            Assert.AreEqual(EXPECTED_FIRST_RECORD_ID, actualRecord.Id, "Retrieved record should have correct ID");
            
            var expectedRecord = SeededTestData.FirstOrDefault(r => r.Id == EXPECTED_FIRST_RECORD_ID);
            ValidateRecordMatches(expectedRecord, actualRecord, "Fetched record should match seeded data");
        }

        /// <summary>
        /// Test that demonstrates full dataset validation
        /// </summary>
        [TestMethod]
        public async Task ValidateCompleteDataset_ShouldMatchSeededData()
        {
            var actualRecords = await FetchActualDataFromDatabase();
            
            ValidateDataMatches(SeededTestData, actualRecords, "Complete dataset validation");
            
            Assert.AreEqual(SeededTestData.Count, actualRecords.Count, "Record counts should match");
            
            ValidateSpecificKnownRecords(actualRecords);
        }

        /// <summary>
        /// Test that demonstrates validating active vs inactive records
        /// </summary>
        [TestMethod]
        public async Task ValidateActiveInactiveRecords_ShouldMatchExpectedCounts()
        {
            var actualRecords = await FetchActualDataFromDatabase();
            
            var actualActiveRecords = GetActiveRecords(actualRecords);
            var actualInactiveRecords = GetInactiveRecords(actualRecords);
            
            var expectedActiveRecords = SampleTestData.GetActiveTestRecords();
            var expectedInactiveRecords = SampleTestData.GetInactiveTestRecords();
            
            ValidateActiveInactiveRecordCounts(expectedActiveRecords, actualActiveRecords, expectedInactiveRecords, actualInactiveRecords);
            ValidateActiveRecordsMatch(expectedActiveRecords, actualActiveRecords);
        }

        /// <summary>
        /// Test that demonstrates using the helper methods from SampleTestData
        /// </summary>
        [TestMethod]
        public async Task ValidateHelperMethods_ShouldProvideCorrectData()
        {
            var actualRecords = await FetchActualDataFromDatabase();
            
            ValidateGetTestRecordByIdHelper(actualRecords);
            ValidateGetTestRecordsWithBalanceGreaterThanHelper(actualRecords);
        }

        /// <summary>
        /// Test that demonstrates error handling when record is not found
        /// </summary>
        [TestMethod]
        public async Task FetchNonExistentRecord_ShouldReturnNull()
        {
            var nonExistentRecord = await FetchActualRecordById(NON_EXISTENT_RECORD_ID);
            
            Assert.IsNull(nonExistentRecord, "Non-existent record should return null");
        }

        /// <summary>
        /// Test that demonstrates the TestDataRecord equality functionality
        /// </summary>
        [TestMethod]
        public void TestDataRecordEquality_ShouldWorkCorrectly()
        {
            var record1 = CreateTestRecord(1, "John Doe", "john@example.com", "+1-555-0101", true, 1500.50m);
            var record2 = CreateTestRecord(1, "John Doe", "john@example.com", "+1-555-0101", true, 1500.50m);
            var record3 = CreateTestRecord(2, "Jane Smith", "jane@example.com", "+1-555-0102", true, 2750.00m);
            
            ValidateRecordEquality(record1, record2, record3);
            ValidateRecordStringRepresentation(record1);
        }

        private static void AssertRecordsAreNotNullOrEmpty(List<TestDataRecord> actualRecords)
        {
            Assert.IsNotNull(actualRecords, "Fetched records should not be null");
            Assert.IsTrue(actualRecords.Count > 0, "Should have fetched some records");
        }

        private static void AssertFirstRecordHasValidStructure(TestDataRecord firstRecord)
        {
            Assert.IsTrue(firstRecord.Id > 0, "ID should be positive");
            Assert.IsFalse(string.IsNullOrEmpty(firstRecord.Name), "Name should not be empty");
            Assert.IsFalse(string.IsNullOrEmpty(firstRecord.Email), "Email should not be empty");
            Assert.IsTrue(firstRecord.Balance >= 0, "Balance should be non-negative");
        }

        private static void ValidateSpecificKnownRecords(List<TestDataRecord> actualRecords)
        {
            var frankGarcia = actualRecords.FirstOrDefault(r => r.Name == FRANK_GARCIA_NAME);
            Assert.IsNotNull(frankGarcia, "Frank Garcia should exist in database");
            Assert.AreEqual(FRANK_GARCIA_EXPECTED_BALANCE, frankGarcia.Balance, "Frank Garcia should have correct balance");
            
            var aliceBrown = actualRecords.FirstOrDefault(r => r.Name == ALICE_BROWN_NAME);
            Assert.IsNotNull(aliceBrown, "Alice Brown should exist in database");
            Assert.IsFalse(aliceBrown.IsActive, "Alice Brown should be inactive");
        }

        private static List<TestDataRecord> GetActiveRecords(List<TestDataRecord> records)
        {
            return records.Where(r => r.IsActive).ToList();
        }

        private static List<TestDataRecord> GetInactiveRecords(List<TestDataRecord> records)
        {
            return records.Where(r => !r.IsActive).ToList();
        }

        private void ValidateActiveInactiveRecordCounts(List<TestDataRecord> expectedActiveRecords, List<TestDataRecord> actualActiveRecords,
            List<TestDataRecord> expectedInactiveRecords, List<TestDataRecord> actualInactiveRecords)
        {
            Assert.AreEqual(expectedActiveRecords.Count, actualActiveRecords.Count, "Active record count should match");
            Assert.AreEqual(expectedInactiveRecords.Count, actualInactiveRecords.Count, "Inactive record count should match");
        }

        private void ValidateActiveRecordsMatch(List<TestDataRecord> expectedActiveRecords, List<TestDataRecord> actualActiveRecords)
        {
            foreach (var expectedActive in expectedActiveRecords)
            {
                var actualActive = actualActiveRecords.FirstOrDefault(r => r.Id == expectedActive.Id);
                ValidateRecordMatches(expectedActive, actualActive, $"Active record with ID {expectedActive.Id}");
            }
        }

        private void ValidateGetTestRecordByIdHelper(List<TestDataRecord> actualRecords)
        {
            var expectedRecord = SampleTestData.GetTestRecordById(EXPECTED_SECOND_RECORD_ID);
            var actualRecord = actualRecords.FirstOrDefault(r => r.Id == EXPECTED_SECOND_RECORD_ID);
            ValidateRecordMatches(expectedRecord, actualRecord, "Helper method should return correct record");
        }

        private void ValidateGetTestRecordsWithBalanceGreaterThanHelper(List<TestDataRecord> actualRecords)
        {
            var expectedHighBalanceRecords = SampleTestData.GetTestRecordsWithBalanceGreaterThan(HIGH_BALANCE_THRESHOLD);
            var actualHighBalanceRecords = actualRecords.Where(r => r.Balance > HIGH_BALANCE_THRESHOLD).ToList();
            
            Assert.AreEqual(expectedHighBalanceRecords.Count, actualHighBalanceRecords.Count, 
                "High balance record counts should match");
            
            foreach (var expected in expectedHighBalanceRecords)
            {
                var actual = actualHighBalanceRecords.FirstOrDefault(r => r.Id == expected.Id);
                ValidateRecordMatches(expected, actual, $"High balance record with ID {expected.Id}");
            }
        }

        private static TestDataRecord CreateTestRecord(int id, string name, string email, string phone, bool isActive, decimal balance)
        {
            return new TestDataRecord(id, name, email, phone, isActive, balance);
        }

        private static void ValidateRecordEquality(TestDataRecord record1, TestDataRecord record2, TestDataRecord record3)
        {
            Assert.AreEqual(record1, record2, "Identical records should be equal");
            Assert.AreNotEqual(record1, record3, "Different records should not be equal");
            Assert.AreEqual(record1.GetHashCode(), record2.GetHashCode(), "Equal records should have same hash code");
        }

        private static void ValidateRecordStringRepresentation(TestDataRecord record)
        {
            var stringRepresentation = record.ToString();
            Assert.IsTrue(stringRepresentation.Contains("John Doe"), "ToString should contain record data");
            Assert.IsTrue(stringRepresentation.Contains("1500.50"), "ToString should contain balance");
        }
    }
} 