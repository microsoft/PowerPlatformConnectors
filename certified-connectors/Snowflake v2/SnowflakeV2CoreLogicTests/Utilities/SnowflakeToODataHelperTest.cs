namespace SnowflakeV2CoreLogic.Tests.Utilities
{
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Constants;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models;
    using Newtonsoft.Json.Linq;
    using SnowflakeV2CoreLogic;
    using SnowflakeV2CoreLogic.Models.SnowflakeAPIModels;
    using SnowflakeV2CoreLogic.Tests.TestDoubles;
    using SnowflakeV2CoreLogic.Utilities;

    [TestClass]
    public sealed class SnowflakeToODataHelperTest
    {

        [DataRow("1700000000", "2023-11-14 22:13:20.0000000")]
        [DataRow("1700000000.5", "2023-11-14 22:13:20.5000000")]
        [DataRow("1700000000.05", "2023-11-14 22:13:20.0500000")]
        [DataRow("1700000000.123", "2023-11-14 22:13:20.1230000")]
        [DataRow("1700000000.123456", "2023-11-14 22:13:20.1234560")]
        [DataRow("1700000000.123456789", "2023-11-14 22:13:20.1234567")]
        [DataTestMethod]
        public void CastSnowflakeDataToCorrectType_TimestampNtz_SupportsAllScales(string snowflakeValue, string expected)
        {
            var result = SnowflakeToODataHelper.CastSnowflakeDataToCorrectType(
                Constants.SFDataTypeTimestampNoTimeZone,
                precision: null,
                data: snowflakeValue);

            Assert.AreEqual(expected, result);
        }

        [DataRow("1700000000", "2023-11-14 22:13:20.0000000")]
        [DataRow("1700000000.1", "2023-11-14 22:13:20.1000000")]
        [DataRow("1700000000.12", "2023-11-14 22:13:20.1200000")]
        [DataRow("1700000000.123", "2023-11-14 22:13:20.1230000")]
        [DataRow("1700000000.123456", "2023-11-14 22:13:20.1234560")]
        [DataRow("1700000000.123456789", "2023-11-14 22:13:20.1234567")]
        [DataTestMethod]
        public void CastSnowflakeDataToCorrectType_TimestampLtz_SupportsAllScales(string snowflakeValue, string expected)
        {
            var result = SnowflakeToODataHelper.CastSnowflakeDataToCorrectType(
                Constants.SFDataTypeTimestampLocalTimeZone,
                precision: null,
                data: snowflakeValue);

            Assert.AreEqual(expected, result);
        }

        [DataRow("1700000000 +0000", "2023-11-14 22:13:20.0000000")]
        [DataRow("1700000000.5 -0800", "2023-11-14 22:13:20.5000000")]
        [DataRow("1700000000.05 +0530", "2023-11-14 22:13:20.0500000")]
        [DataRow("1700000000.123 +0000", "2023-11-14 22:13:20.1230000")]
        [DataRow("1700000000.123456 +0000", "2023-11-14 22:13:20.1234560")]
        [DataRow("1700000000.123456789 +0000", "2023-11-14 22:13:20.1234567")]
        [DataTestMethod]
        public void CastSnowflakeDataToCorrectType_TimestampTz_SupportsAllScales(string snowflakeValue, string expected)
        {
            var result = SnowflakeToODataHelper.CastSnowflakeDataToCorrectType(
                Constants.SFDataTypeTimestampWithTimezone,
                precision: null,
                data: snowflakeValue);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TableMetadataToOdata_SingleColumnPrimaryKey_MarksKeyColumn()
        {
            var metadata = SnowflakeToODataHelper.TableMetadataToOdata(OrderLinesColumns(), SnowflakeResponses.PrimaryKeys(("ORDER_ID", 1)), "T");

            var properties = Properties(metadata);
            AssertColumn(properties, "ORDER_ID", KeyType.Primary, 1);
            AssertColumn(properties, "LINE_NO", KeyType.None, null);
            AssertColumn(properties, "NOTE", KeyType.None, null);
            Assert.AreEqual(PermissionType.ReadWrite, metadata.Permission);
        }

        [TestMethod]
        public void TableMetadataToOdata_CompositePrimaryKey_MarksEveryKeyColumnInKeyOrder()
        {
            var metadata = SnowflakeToODataHelper.TableMetadataToOdata(
                OrderLinesColumns(),
                SnowflakeResponses.PrimaryKeys(("LINE_NO", 2), ("ORDER_ID", 1)),
                "T");

            var properties = Properties(metadata);
            AssertColumn(properties, "ORDER_ID", KeyType.Primary, 1);
            AssertColumn(properties, "LINE_NO", KeyType.Primary, 2);
            AssertColumn(properties, "NOTE", KeyType.None, null);
            Assert.AreEqual(PermissionType.ReadWrite, metadata.Permission);
        }

        [TestMethod]
        public void TableMetadataToOdata_NoPrimaryKey_MarksNoKeyColumn()
        {
            var metadata = SnowflakeToODataHelper.TableMetadataToOdata(OrderLinesColumns(), SnowflakeResponses.NoPrimaryKey(), "T");

            var properties = Properties(metadata);
            AssertColumn(properties, "ORDER_ID", KeyType.None, null);
            AssertColumn(properties, "LINE_NO", KeyType.None, null);
            AssertColumn(properties, "NOTE", KeyType.None, null);
        }

        [TestMethod]
        public void TableMetadataToOdata_UserProvidedKey_IsWritableAndRequired()
        {
            var metadata = SnowflakeToODataHelper.TableMetadataToOdata(OrderLinesColumns(), SnowflakeResponses.PrimaryKeys(("ORDER_ID", 1), ("LINE_NO", 2)), "T");

            var properties = Properties(metadata);
            AssertWritability(metadata, properties, "ORDER_ID", "read-write", required: true);
            AssertWritability(metadata, properties, "LINE_NO", "read-write", required: true);
            AssertWritability(metadata, properties, "NOTE", "read-write", required: false);
        }

        [TestMethod]
        public void TableMetadataToOdata_IdentityPrimaryKey_IsReadOnlyAndNotRequired()
        {
            var metadata = SnowflakeToODataHelper.TableMetadataToOdata(IdentityColumns(), SnowflakeResponses.PrimaryKeys(("ID", 1)), "T");

            var properties = Properties(metadata);
            AssertColumn(properties, "ID", KeyType.Primary, 1);
            AssertWritability(metadata, properties, "ID", "read-only", required: false);
            AssertWritability(metadata, properties, "SEQ_NO", "read-write", required: false);
            AssertWritability(metadata, properties, "NAME", "read-write", required: true);
        }

        [TestMethod]
        public void TableMetadataToOdata_CompositeKeyWithIdentityColumn_OnlyIdentityColumnIsServerGenerated()
        {
            var metadata = SnowflakeToODataHelper.TableMetadataToOdata(IdentityColumns(), SnowflakeResponses.PrimaryKeys(("NAME", 1), ("ID", 2)), "T");

            var properties = Properties(metadata);
            AssertColumn(properties, "NAME", KeyType.Primary, 1);
            AssertColumn(properties, "ID", KeyType.Primary, 2);
            AssertWritability(metadata, properties, "NAME", "read-write", required: true);
            AssertWritability(metadata, properties, "ID", "read-only", required: false);
        }

        [TestMethod]
        public void TableMetadataToOdata_IdentityColumnNotInKey_IsWritableAndNotRequired()
        {
            var metadata = SnowflakeToODataHelper.TableMetadataToOdata(IdentityColumns(), SnowflakeResponses.NoPrimaryKey(), "T");

            var properties = Properties(metadata);
            AssertWritability(metadata, properties, "ID", "read-write", required: false);
            AssertWritability(metadata, properties, "SEQ_NO", "read-write", required: false);
            AssertWritability(metadata, properties, "NAME", "read-write", required: true);
        }

        /// <summary>
        /// An information_schema.columns response for ORDER_ID NUMBER NOT NULL, LINE_NO NUMBER NOT NULL, NOTE TEXT,
        /// without the IS_IDENTITY column.
        /// </summary>
        private static SnowflakeTableData OrderLinesColumns()
        {
            return SnowflakeResponses.Table(
                new[] { ("COLUMN_NAME", "text", (int?)null), ("DATA_TYPE", "text", (int?)null), ("IS_NULLABLE", "text", (int?)null), ("NUMERIC_SCALE", "fixed", (int?)0) },
                new string?[] { "ORDER_ID", Constants.SFDataTypeNumber, "NO", "0" },
                new string?[] { "LINE_NO", Constants.SFDataTypeNumber, "NO", "0" },
                new string?[] { "NOTE", Constants.SFDataTypeText, "YES", null });
        }

        /// <summary>
        /// An information_schema.columns response for ID NUMBER AUTOINCREMENT, SEQ_NO NUMBER IDENTITY, NAME TEXT NOT NULL.
        /// </summary>
        private static SnowflakeTableData IdentityColumns()
        {
            return SnowflakeResponses.Table(
                new[] { ("COLUMN_NAME", "text", (int?)null), ("DATA_TYPE", "text", (int?)null), ("IS_NULLABLE", "text", (int?)null), ("NUMERIC_SCALE", "fixed", (int?)0), ("IS_IDENTITY", "text", (int?)null) },
                new string?[] { "ID", Constants.SFDataTypeNumber, "NO", "0", "YES" },
                new string?[] { "SEQ_NO", Constants.SFDataTypeNumber, "NO", "0", "YES" },
                new string?[] { "NAME", Constants.SFDataTypeText, "NO", null, "NO" });
        }

        private static JObject Properties(TableMetadata metadata)
        {
            return metadata.Schema[SchemaPropertyConstants.Items]?[SchemaPropertyConstants.Properties] as JObject
                ?? throw new AssertFailedException("Metadata should contain schema.items.properties");
        }

        private static void AssertColumn(JObject properties, string column, string keyType, int? keyOrder)
        {
            var property = properties[column] as JObject ?? throw new AssertFailedException($"Missing column {column}");
            Assert.AreEqual(keyType, (string?)property[SchemaPropertyConstants.KeyType], $"{column} key type");
            if (keyOrder == null)
            {
                Assert.IsFalse(property.ContainsKey(SchemaPropertyConstants.KeyOrder), $"{column} must not have a key order");
            }
            else
            {
                Assert.AreEqual(keyOrder, (int?)property[SchemaPropertyConstants.KeyOrder], $"{column} key order");
            }
        }

        private static void AssertWritability(TableMetadata metadata, JObject properties, string column, string permission, bool required)
        {
            var property = properties[column] ?? throw new AssertFailedException($"Missing column {column}");
            Assert.AreEqual(permission, (string?)property[SchemaPropertyConstants.Permission], $"{column} permission");
            Assert.AreEqual(required, (bool?)property[SchemaPropertyConstants.Required], $"{column} required flag");

            var requiredColumns = metadata.Schema[SchemaPropertyConstants.Items]?[SchemaPropertyConstants.Required] as JArray
                ?? throw new AssertFailedException("Metadata should contain schema.items.required");
            Assert.AreEqual(required, requiredColumns.Any(c => (string?)c == column), $"{column} in schema.items.required");
        }
    }
}
