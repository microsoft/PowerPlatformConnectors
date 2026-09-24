namespace SnowflakeV2CoreLogic.Tests.Utilities
{
    using SnowflakeV2CoreLogic;
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

    }
}
