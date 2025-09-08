using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SnowflakeTestApp.Tests.Infrastructure
{
    /// <summary>
    /// Represents a test data record for use in integration tests
    /// </summary>
    public class TestDataRecord
    {
        [JsonProperty("ID")]
        public int Id { get; set; }
        [JsonProperty("NAME")]
        public string Name { get; set; }
        [JsonProperty("EMAIL")]
        public string Email { get; set; }
        [JsonProperty("PHONE")]
        public string Phone { get; set; }
        [JsonProperty("IS_ACTIVE")]
        public bool IsActive { get; set; }
        [JsonProperty("BALANCE")]
        public decimal Balance { get; set; }
        [JsonProperty("CREATED_DATE")]
        public DateTime? CreatedDate { get; set; }

        public TestDataRecord() { }

        public TestDataRecord(int id, string name, string email, string phone, bool isActive, decimal balance)
        {
            Id = id;
            Name = name;
            Email = email;
            Phone = phone;
            IsActive = isActive;
            Balance = balance;
        }

        public override bool Equals(object obj)
        {
            var other = obj as TestDataRecord;
            if (other == null) return false;
            
            return Id == other.Id &&
                   Name == other.Name &&
                   Email == other.Email &&
                   Phone == other.Phone &&
                   IsActive == other.IsActive &&
                   Balance == other.Balance;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + Id.GetHashCode();
                hash = hash * 23 + (Name?.GetHashCode() ?? 0);
                hash = hash * 23 + (Email?.GetHashCode() ?? 0);
                hash = hash * 23 + (Phone?.GetHashCode() ?? 0);
                hash = hash * 23 + IsActive.GetHashCode();
                hash = hash * 23 + Balance.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return $"TestDataRecord {{ Id={Id}, Name='{Name}', Email='{Email}', Phone='{Phone}', IsActive={IsActive}, Balance={Balance} }}";
        }
    }

    /// <summary>
    /// Provides sample test data for integration tests
    /// </summary>
    public static class SampleTestData
    {
        /// <summary>
        /// Gets the default set of test records used for seeding
        /// </summary>
        public static List<TestDataRecord> GetDefaultTestRecords()
        {
            return new List<TestDataRecord>
            {
                new TestDataRecord(1, "John Doe", "john.doe@example.com", "+1-555-0101", true, 1500.50m),
                new TestDataRecord(2, "Jane Smith", "jane.smith@example.com", "+1-555-0102", true, 2750.00m),
                new TestDataRecord(3, "Bob Johnson", "bob.johnson@example.com", "+1-555-0103", true, 890.25m),
                new TestDataRecord(4, "Alice Brown", "alice.brown@example.com", "+1-555-0104", false, 0.00m),
                new TestDataRecord(5, "Charlie Wilson", "charlie.wilson@example.com", "+1-555-0105", true, 3200.75m),
                new TestDataRecord(6, "Diana Davis", "diana.davis@example.com", "+1-555-0106", true, 1100.00m),
                new TestDataRecord(7, "Eve Miller", "eve.miller@example.com", "+1-555-0107", true, 4500.25m),
                new TestDataRecord(8, "Frank Garcia", "frank.garcia@example.com", "+1-555-0108", false, 250.00m),
                new TestDataRecord(9, "Grace Lee", "grace.lee@example.com", "+1-555-0109", true, 1875.50m),
                new TestDataRecord(10, "Henry Taylor", "henry.taylor@example.com", "+1-555-0110", true, 3350.00m)
            };
        }

        /// <summary>
        /// Gets active test records only
        /// </summary>
        public static List<TestDataRecord> GetActiveTestRecords()
        {
            return GetDefaultTestRecords().FindAll(r => r.IsActive);
        }

        /// <summary>
        /// Gets inactive test records only
        /// </summary>
        public static List<TestDataRecord> GetInactiveTestRecords()
        {
            return GetDefaultTestRecords().FindAll(r => !r.IsActive);
        }

        /// <summary>
        /// Gets a test record by ID
        /// </summary>
        public static TestDataRecord GetTestRecordById(int id)
        {
            return GetDefaultTestRecords().Find(r => r.Id == id);
        }

        /// <summary>
        /// Gets test records with balance greater than the specified amount
        /// </summary>
        public static List<TestDataRecord> GetTestRecordsWithBalanceGreaterThan(decimal amount)
        {
            return GetDefaultTestRecords().FindAll(r => r.Balance > amount);
        }
    }
} 