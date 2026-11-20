// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace SnowflakeV2CoreLogic.Tests.Utilities
{
    using System;
    using SnowflakeV2CoreLogic.Utilities;

    /// <summary>
    /// Unit tests covering the input-validation/escaping used to prevent SQL injection through
    /// identifiers, the OData <c>$select</c>/<c>$orderby</c> clauses, and quoted identifiers.
    /// These tests run in-process and do not require the application or a live Snowflake instance.
    /// </summary>
    [TestClass]
    public sealed class EnsureExtensionsInjectionTest
    {
        // ---- EnsureValidSnowflakeIdentifier ----

        [DataTestMethod]
        [DataRow("CUSTOMERS")]
        [DataRow("_underscore")]
        [DataRow("My_Table$1")]
        [DataRow("a")]
        [DataRow("\"Quoted Identifier\"")]
        [DataRow("\"with a , comma\"")]
        [DataRow("\"with \"\"escaped\"\" quotes\"")]
        [DataRow("\"Multiple   Spaces\"")]
        public void EnsureValidSnowflakeIdentifier_AcceptsValidIdentifiers(string identifier)
        {
            Assert.AreEqual(identifier, identifier.EnsureValidSnowflakeIdentifier("identifier"));
        }

        [DataTestMethod]
        [DataRow("CUSTOMERS; DROP TABLE SECRETS")]
        [DataRow("CUSTOMERS UNION SELECT * FROM SECRETS")]
        [DataRow("CUSTOMERS--comment")]
        [DataRow("CUSTOMERS WHERE 1=1")]
        [DataRow("1nvalidStart")]
        [DataRow("has space")]
        [DataRow("\"breakout\"; DROP TABLE X--\"")]
        [DataRow("\"unterminated")]
        [DataRow("'single'")]
        public void EnsureValidSnowflakeIdentifier_RejectsInjectionPayloads(string identifier)
        {
            Assert.ThrowsException<ArgumentException>(() => identifier.EnsureValidSnowflakeIdentifier("identifier"));
        }

        // ---- EnsureValidQualifiedSnowflakeIdentifier ----

        [DataTestMethod]
        [DataRow("CUSTOMERS")]
        [DataRow("SCHEMA.TABLE")]
        [DataRow("DB.SCHEMA.TABLE")]
        [DataRow("\"My DB\".\"My Schema\".\"My Table\"")]
        [DataRow("DB.\"my.schema\".TABLE")]
        public void EnsureValidQualifiedSnowflakeIdentifier_AcceptsQualifiedNames(string identifier)
        {
            Assert.AreEqual(identifier, identifier.EnsureValidQualifiedSnowflakeIdentifier("Table Name"));
        }

        [DataTestMethod]
        [DataRow("DB..TABLE")]
        [DataRow(".TABLE")]
        [DataRow("TABLE.")]
        [DataRow("DB.SCHEMA.TABLE; DROP TABLE SECRETS")]
        [DataRow("DB.SCHEMA.TABLE UNION SELECT * FROM SECRETS")]
        [DataRow("DB.1nvalid")]
        [DataRow("DB.\"unterminated")]
        [DataRow("(SELECT CURRENT_USER()) t")]
        public void EnsureValidQualifiedSnowflakeIdentifier_RejectsInjectionPayloads(string identifier)
        {
            Assert.ThrowsException<ArgumentException>(() => identifier.EnsureValidQualifiedSnowflakeIdentifier("Table Name"));
        }

        /// <summary>
        /// Regression test for the live-reproduced SQL injection: passing
        /// <c>(SELECT CURRENT_USER()) t</c> as the "table name" to the <c>.../tables/{table}/items</c>
        /// operation caused the unpatched backend to inline it as
        /// <c>SELECT * FROM (SELECT CURRENT_USER()) t ...</c> and leak the Snowflake session user.
        /// This is the exact guard <c>ListAllItemsAsync</c> applies to the table name, so it must reject the payload.
        /// </summary>
        [TestMethod]
        public void EnsureValidQualifiedSnowflakeIdentifier_RejectsCurrentUserSubqueryInjection()
        {
            const string payload = "(SELECT CURRENT_USER()) t";

            Assert.ThrowsException<ArgumentException>(
                () => payload.EnsureValidQualifiedSnowflakeIdentifier("Table Name"));
        }

        // ---- EnsureValidSelectClause ----

        [DataTestMethod]
        [DataRow("*")]
        [DataRow(" * ")]
        [DataRow("NAME")]
        [DataRow("NAME,EMAIL")]
        [DataRow("NAME, EMAIL , PHONE")]
        [DataRow("NAME ,   EMAIL")]
        [DataRow(" NAME , EMAIL ")]
        [DataRow("\"First Name\",\"Last Name\"")]
        [DataRow("\"Last, First\",EMAIL")]
        public void EnsureValidSelectClause_AcceptsValidProjections(string select)
        {
            Assert.AreEqual(select, select.EnsureValidSelectClause("Select"));
        }

        [DataTestMethod]
        [DataRow("NAME, (SELECT secret FROM admin)")]
        [DataRow("NAME; DROP TABLE SECRETS")]
        [DataRow("(CASE WHEN 1=1 THEN NAME ELSE EMAIL END)")]
        [DataRow("NAME UNION SELECT password FROM users")]
        [DataRow("*, (SELECT 1)")]
        [DataRow("NAME,")]
        [DataRow("NAME,,EMAIL")]
        [DataRow(",NAME")]
        public void EnsureValidSelectClause_RejectsInjectionPayloads(string select)
        {
            Assert.ThrowsException<ArgumentException>(() => select.EnsureValidSelectClause("Select"));
        }

        // ---- EnsureValidOrderByClause ----

        [DataTestMethod]
        [DataRow("NAME")]
        [DataRow("NAME asc")]
        [DataRow("NAME DESC")]
        [DataRow("NAME asc, EMAIL desc")]
        [DataRow("NAME, EMAIL")]
        [DataRow("NAME, EMAIL desc")]
        [DataRow("NAME   asc")]
        [DataRow("NAME asc,    EMAIL desc")]
        [DataRow(" NAME asc ")]
        [DataRow("\"Quoted\" desc")]
        [DataRow("\"First Name\"")]
        [DataRow("\"First Name\" asc")]
        [DataRow("\"First Name\"    desc")]
        [DataRow("\"First Name\" desc, \"Last Name\"")]
        [DataRow("\"Last, First\" asc")]
        public void EnsureValidOrderByClause_AcceptsValidClauses(string orderBy)
        {
            Assert.AreEqual(orderBy, orderBy.EnsureValidOrderByClause("Order By"));
        }

        [DataTestMethod]
        [DataRow("NAME; DROP TABLE SECRETS")]
        [DataRow("NAME asc desc")]
        [DataRow("(SELECT 1)")]
        [DataRow("NAME UNION SELECT 1")]
        [DataRow("CASE WHEN (SELECT COUNT(*) FROM secrets) > 0 THEN 1 ELSE 2 END")]
        [DataRow("1; DELETE FROM customers")]
        [DataRow("NAME asc,")]
        [DataRow("NAME,,EMAIL")]
        [DataRow(",NAME desc")]
        public void EnsureValidOrderByClause_RejectsInjectionPayloads(string orderBy)
        {
            Assert.ThrowsException<ArgumentException>(() => orderBy.EnsureValidOrderByClause("Order By"));
        }

        // ---- ToSafeSnowflakeIdentifier ----

        [DataTestMethod]
        [DataRow("IS_ACTIVE", "IS_ACTIVE")]
        [DataRow("FIRST_NAME", "FIRST_NAME")]
        [DataRow("_x$1", "_x$1")]
        public void ToSafeSnowflakeIdentifier_LeavesValidUnquotedIdentifiersUnchanged(string input, string expected)
        {
            Assert.AreEqual(expected, input.ToSafeSnowflakeIdentifier("Filter property"));
        }

        [DataTestMethod]
        [DataRow("first-name", "\"first-name\"")]
        [DataRow("my.col", "\"my.col\"")]
        [DataRow("1starts_with_digit", "\"1starts_with_digit\"")]
        [DataRow("first name", "\"first name\"")]                       // space
        [DataRow("o'brien", "\"o'brien\"")]                            // single quote (not doubled - it is harmless inside a double-quoted id)
        [DataRow("a\"b", "\"a\"\"b\"")]                                // embedded double quote -> doubled
        [DataRow("x;y--", "\"x;y--\"")]                               // semicolon + comment
        [DataRow("DROP TABLE x", "\"DROP TABLE x\"")]                 // spaces + keywords
        [DataRow("col\"; DROP TABLE y--", "\"col\"\"; DROP TABLE y--\"")] // breakout attempt
        [DataRow("x\" OR \"1\"=\"1", "\"x\"\" OR \"\"1\"\"=\"\"1\"")]
        public void ToSafeSnowflakeIdentifier_QuotesAndEscapesEverythingElse(string input, string expected)
        {
            Assert.AreEqual(expected, input.ToSafeSnowflakeIdentifier("Filter property"));
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow("   ")]
        public void ToSafeSnowflakeIdentifier_RejectsNullOrWhitespace(string input)
        {
            Assert.ThrowsException<ArgumentNullException>(() => input.ToSafeSnowflakeIdentifier("Filter property"));
        }

        // ---- EscapeSnowflakeQuotedIdentifier ----

        [DataTestMethod]
        [DataRow("plain", "plain")]
        [DataRow("a\"b", "a\"\"b")]
        [DataRow("end\"", "end\"\"")]
        [DataRow("x\";DROP TABLE y--", "x\"\";DROP TABLE y--")]
        public void EscapeSnowflakeQuotedIdentifier_DoublesEmbeddedQuotes(string input, string expected)
        {
            Assert.AreEqual(expected, input.EscapeSnowflakeQuotedIdentifier());
        }
    }
}
