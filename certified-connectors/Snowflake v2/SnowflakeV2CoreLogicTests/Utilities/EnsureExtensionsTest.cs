// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace SnowflakeV2CoreLogic.Tests.Utilities
{
    using System;
    using SnowflakeV2CoreLogic.Utilities;

    [TestClass]
    public sealed class EnsureExtensionsTest
    {
        #region EnsureValidSnowflakeUrl — valid hostnames

        [TestMethod]
        public void EnsureValidSnowflakeUrl_ValidStandardHostname_Returns()
        {
            var result = "myaccount.snowflakecomputing.com".EnsureValidSnowflakeUrl("Instance");
            Assert.AreEqual("myaccount.snowflakecomputing.com", result);
        }

        [TestMethod]
        public void EnsureValidSnowflakeUrl_ValidPrivateLinkHostname_Returns()
        {
            var result = "myaccount.privatelink.snowflakecomputing.com".EnsureValidSnowflakeUrl("Instance");
            Assert.AreEqual("myaccount.privatelink.snowflakecomputing.com", result);
        }

        [TestMethod]
        public void EnsureValidSnowflakeUrl_ValidHostnameWithDashes_Returns()
        {
            var result = "my-org-account.snowflakecomputing.com".EnsureValidSnowflakeUrl("Instance");
            Assert.AreEqual("my-org-account.snowflakecomputing.com", result);
        }

        [TestMethod]
        public void EnsureValidSnowflakeUrl_ValidHostnameWithDots_Returns()
        {
            var result = "org.account.snowflakecomputing.com".EnsureValidSnowflakeUrl("Instance");
            Assert.AreEqual("org.account.snowflakecomputing.com", result);
        }

        [TestMethod]
        public void EnsureValidSnowflakeUrl_ValidChinaHostname_Returns()
        {
            var result = "myaccount.cn-north-1.snowflakecomputing.cn".EnsureValidSnowflakeUrl("Instance");
            Assert.AreEqual("myaccount.cn-north-1.snowflakecomputing.cn", result);
        }

        [TestMethod]
        public void EnsureValidSnowflakeUrl_ValidChinaPrivateLinkHostname_Returns()
        {
            var result = "myaccount.privatelink.snowflakecomputing.cn".EnsureValidSnowflakeUrl("Instance");
            Assert.AreEqual("myaccount.privatelink.snowflakecomputing.cn", result);
        }

        #endregion

        #region EnsureValidSnowflakeUrl — invalid hostnames

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EnsureValidSnowflakeUrl_ArbitraryHostname_Throws()
        {
            "evil-server.com".EnsureValidSnowflakeUrl("Instance");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EnsureValidSnowflakeUrl_SubstringBypassWithSuffix_Throws()
        {
            "foo.snowflakecomputing.com.evil.com".EnsureValidSnowflakeUrl("Instance");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EnsureValidSnowflakeUrl_ChinaSubstringBypassWithSuffix_Throws()
        {
            "foo.snowflakecomputing.cn.evil.com".EnsureValidSnowflakeUrl("Instance");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EnsureValidSnowflakeUrl_WrongTld_Throws()
        {
            "myaccount.snowflakecomputing.cm".EnsureValidSnowflakeUrl("Instance");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EnsureValidSnowflakeUrl_SubstringBypassWithPrefix_Throws()
        {
            "evil.com.snowflakecomputing.com.attacker.com".EnsureValidSnowflakeUrl("Instance");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EnsureValidSnowflakeUrl_SnowflakeDomainAsSubdomain_Throws()
        {
            "attacker.com/foo.snowflakecomputing.com".EnsureValidSnowflakeUrl("Instance");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EnsureValidSnowflakeUrl_NullValue_Throws()
        {
            ((string)null!).EnsureValidSnowflakeUrl("Instance");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EnsureValidSnowflakeUrl_EmptyString_Throws()
        {
            "".EnsureValidSnowflakeUrl("Instance");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EnsureValidSnowflakeUrl_WhitespaceOnly_Throws()
        {
            "   ".EnsureValidSnowflakeUrl("Instance");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EnsureValidSnowflakeUrl_IpAddress_Throws()
        {
            "192.168.1.1".EnsureValidSnowflakeUrl("Instance");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void EnsureValidSnowflakeUrl_Localhost_Throws()
        {
            "localhost".EnsureValidSnowflakeUrl("Instance");
        }

        #endregion

        #region EnsureQualifiedIdentifierWithinScope

        [DataTestMethod]
        [DataRow("CUSTOMERS", "MYDB", "PUBLIC")]
        [DataRow("\"Mixed Case Table\"", "MYDB", "PUBLIC")]
        [DataRow("PUBLIC.CUSTOMERS", "MYDB", "PUBLIC")]
        [DataRow("public.CUSTOMERS", "MYDB", "PUBLIC")]
        [DataRow("\"PUBLIC\".CUSTOMERS", "MYDB", "PUBLIC")]
        [DataRow("MYDB.PUBLIC.CUSTOMERS", "MYDB", "PUBLIC")]
        [DataRow("mydb.public.customers", "MYDB", "PUBLIC")]
        [DataRow("\"MyDb\".\"MySchema\".CUSTOMERS", "MyDb", "MySchema")]
        [DataRow("\"my.db\".\"my.schema\".CUSTOMERS", "my.db", "my.schema")]
        [DataRow("\"a\"\"b\".CUSTOMERS", "MYDB", "a\"b")]
        [DataRow("OTHERDB.OTHER.CUSTOMERS", null, null)]
        [DataRow("OTHERDB.OTHER.CUSTOMERS", "", "")]
        [DataRow("OTHERDB.PUBLIC.CUSTOMERS", null, "PUBLIC")]
        [DataRow("MYDB.OTHER.CUSTOMERS", "MYDB", null)]
        public void EnsureQualifiedIdentifierWithinScope_WithinScope_Returns(string identifier, string? database, string? schema)
        {
            Assert.AreEqual(identifier, identifier.EnsureQualifiedIdentifierWithinScope(database, schema, "Table Name"));
        }

        [DataTestMethod]
        [DataRow("OTHER.CUSTOMERS", "MYDB", "PUBLIC")]
        [DataRow("OTHERDB.PUBLIC.CUSTOMERS", "MYDB", "PUBLIC")]
        [DataRow("MYDB.OTHER.CUSTOMERS", "MYDB", "PUBLIC")]
        [DataRow("\"public\".CUSTOMERS", "MYDB", "PUBLIC")]
        [DataRow("\"mydb\".PUBLIC.CUSTOMERS", "MYDB", "PUBLIC")]
        [DataRow("MyDb.PUBLIC.CUSTOMERS", "MyDb", "PUBLIC")]
        [DataRow("MYDB.PUBLIC.EXTRA.CUSTOMERS", "MYDB", "PUBLIC")]
        [DataRow("MYDB..CUSTOMERS", "MYDB", "PUBLIC")]
        [DataRow("MYDB.OTHER.CUSTOMERS", null, "PUBLIC")]
        [DataRow("OTHERDB.PUBLIC.CUSTOMERS", "MYDB", null)]
        [DataRow("A.B.C.D", null, null)]
        [DataRow("A..C", null, null)]
        public void EnsureQualifiedIdentifierWithinScope_OutsideScopeOrInvalid_Throws(string identifier, string? database, string? schema)
        {
            Assert.ThrowsException<ArgumentException>(() => identifier.EnsureQualifiedIdentifierWithinScope(database, schema, "Table Name"));
        }

        #endregion
    }
}
