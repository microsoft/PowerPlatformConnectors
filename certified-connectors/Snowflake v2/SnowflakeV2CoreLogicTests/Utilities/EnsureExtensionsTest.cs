// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace SnowflakeV2CoreLogic.Tests.Utilities
{
    using System;
    using SnowflakeV2CoreLogic.Utilities;

    [TestClass]
    public sealed class EnsureExtensionsTest
    {
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
