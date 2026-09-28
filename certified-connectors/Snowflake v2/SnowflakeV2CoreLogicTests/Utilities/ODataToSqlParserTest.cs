namespace SnowflakeV2CoreLogic.Tests.Utilities
{
    using System;
    using Microsoft.OData.Core.UriParser;
    using Microsoft.OData.Core.UriParser.Semantic;
    using Microsoft.OData.Core.UriParser.TreeNodeKinds;
    using Microsoft.OData.Edm;
    using Microsoft.OData.Edm.Library;
    using SnowflakeV2CoreLogic.Utilities;

    [TestClass]
    public sealed class ODataToSqlParserTest
    {
        private ODataToSqlParser parser = null!;

        [TestInitialize]
        public void TestInitialize()
        {
            parser = new ODataToSqlParser();
        }

        #region Helpers

        private static readonly EdmEntityType ItemEntityType;
        private static readonly EdmModel SharedModel;

        static ODataToSqlParserTest()
        {
            SharedModel = new EdmModel();
            ItemEntityType = new EdmEntityType("NS", "Item", null, false, isOpen: true);
            ItemEntityType.AddKeys(ItemEntityType.AddStructuralProperty("Id", EdmPrimitiveTypeKind.Int32));
            var container = new EdmEntityContainer("NS", "Default");
            container.AddEntitySet("Items", ItemEntityType);
            SharedModel.AddElement(ItemEntityType);
            SharedModel.AddElement(container);
        }

        private static FilterClause ParseFilter(string filterString)
        {
            var serviceRoot = new Uri("http://test/");
            var fullUri = new Uri($"http://test/Items?$filter={Uri.EscapeDataString(filterString)}");
            var odataParser = new ODataUriParser(SharedModel, serviceRoot, fullUri);
            return odataParser.ParseFilter();
        }

        private static FilterClause MakeFilterClause(SingleValueNode expression)
        {
            var rangeVariable = new EntityRangeVariable(
                "$it",
                new EdmEntityTypeReference(ItemEntityType, false),
                SharedModel.FindDeclaredEntitySet("Items"));
            return new FilterClause(expression, rangeVariable);
        }

        #endregion

        #region 1. Null input

        [TestMethod]
        public void ParseFilterToSql_NullFilterClause_ReturnsEmpty()
        {
            var result = parser.ParseFilterToSql(null);
            Assert.AreEqual(string.Empty, result);
        }

        #endregion

        #region 2. Comparison operators

        [TestMethod]
        public void ParseFilterToSql_Eq_StringComparison()
        {
            var filter = ParseFilter("Name eq 'Alice'");
            var result = parser.ParseFilterToSql(filter);
            Assert.AreEqual("Name = 'Alice'", result);
        }

        [TestMethod]
        public void ParseFilterToSql_Ne_StringComparison()
        {
            var filter = ParseFilter("Name ne 'Alice'");
            var result = parser.ParseFilterToSql(filter);
            Assert.AreEqual("Name <> 'Alice'", result);
        }

        [TestMethod]
        public void ParseFilterToSql_Gt_NumericComparison()
        {
            var filter = ParseFilter("Age gt 25");
            var result = parser.ParseFilterToSql(filter);
            Assert.AreEqual("Age > 25", result);
        }

        [TestMethod]
        public void ParseFilterToSql_Ge_NumericComparison()
        {
            var filter = ParseFilter("Age ge 25");
            var result = parser.ParseFilterToSql(filter);
            Assert.AreEqual("Age >= 25", result);
        }

        [TestMethod]
        public void ParseFilterToSql_Lt_NumericComparison()
        {
            var filter = ParseFilter("Age lt 25");
            var result = parser.ParseFilterToSql(filter);
            Assert.AreEqual("Age < 25", result);
        }

        [TestMethod]
        public void ParseFilterToSql_Le_NumericComparison()
        {
            var filter = ParseFilter("Age le 25");
            var result = parser.ParseFilterToSql(filter);
            Assert.AreEqual("Age <= 25", result);
        }

        #endregion

        #region 3. Logical operators

        [TestMethod]
        public void ParseFilterToSql_And_TwoConditions()
        {
            var filter = ParseFilter("Name eq 'Alice' and Age gt 25");
            var result = parser.ParseFilterToSql(filter);
            Assert.AreEqual("(Name = 'Alice') AND (Age > 25)", result);
        }

        [TestMethod]
        public void ParseFilterToSql_Or_TwoConditions()
        {
            var filter = ParseFilter("Name eq 'Alice' or Age gt 25");
            var result = parser.ParseFilterToSql(filter);
            Assert.AreEqual("(Name = 'Alice') OR (Age > 25)", result);
        }

        [TestMethod]
        public void ParseFilterToSql_Not_SingleCondition()
        {
            var filter = ParseFilter("not (Age gt 25)");
            var result = parser.ParseFilterToSql(filter);
            Assert.AreEqual("NOT (Age > 25)", result);
        }

        #endregion

        #region 4. String functions

        [TestMethod]
        public void ParseFilterToSql_Contains()
        {
            var filter = ParseFilter("contains(Name, 'Ali')");
            var result = parser.ParseFilterToSql(filter);
            Assert.AreEqual("Name LIKE '%Ali%'", result);
        }

        [TestMethod]
        public void ParseFilterToSql_StartsWith()
        {
            var filter = ParseFilter("startswith(Name, 'Ali')");
            var result = parser.ParseFilterToSql(filter);
            Assert.AreEqual("Name LIKE 'Ali%'", result);
        }

        [TestMethod]
        public void ParseFilterToSql_EndsWith()
        {
            var filter = ParseFilter("endswith(Name, 'ce')");
            var result = parser.ParseFilterToSql(filter);
            Assert.AreEqual("Name LIKE '%ce'", result);
        }

        #endregion

        #region 5. Constant / leaf node types

        [TestMethod]
        public void ParseFilterToSql_NullConstant()
        {
            var filter = ParseFilter("Name eq null");
            var result = parser.ParseFilterToSql(filter);
            Assert.AreEqual("Name = NULL", result);
        }

        [TestMethod]
        public void ParseFilterToSql_IntegerConstant()
        {
            var filter = ParseFilter("Age eq 42");
            var result = parser.ParseFilterToSql(filter);
            Assert.AreEqual("Age = 42", result);
        }

        [TestMethod]
        public void ParseFilterToSql_StringConstant()
        {
            var filter = ParseFilter("Name eq 'Bob'");
            var result = parser.ParseFilterToSql(filter);
            Assert.AreEqual("Name = 'Bob'", result);
        }

        #endregion

        #region 6. Compound / nested expressions

        [TestMethod]
        public void ParseFilterToSql_NestedAndOr()
        {
            var filter = ParseFilter("Name eq 'A' and (Age gt 20 or Age lt 10)");
            var result = parser.ParseFilterToSql(filter);
            Assert.AreEqual("(Name = 'A') AND ((Age > 20) OR (Age < 10))", result);
        }

        [TestMethod]
        public void ParseFilterToSql_NotWithComparison()
        {
            var filter = ParseFilter("not (Name eq 'X')");
            var result = parser.ParseFilterToSql(filter);
            Assert.AreEqual("NOT (Name = 'X')", result);
        }

        [TestMethod]
        public void ParseFilterToSql_ContainsWithAnd()
        {
            var filter = ParseFilter("contains(Name, 'Al') and Age gt 25");
            var result = parser.ParseFilterToSql(filter);
            Assert.AreEqual("(Name LIKE '%Al%') AND (Age > 25)", result);
        }

        #endregion

        #region 7. Error / unsupported paths

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ParseFilterToSql_UnsupportedFunction_Throws()
        {
            var propertyNode = new SingleValueOpenPropertyAccessNode(
                new ConstantNode("dummy"), "Name");
            var functionNode = new SingleValueFunctionCallNode(
                "trim",
                new QueryNode[] { propertyNode },
                EdmCoreModel.Instance.GetString(false));
            var filterClause = MakeFilterClause(functionNode);

            parser.ParseFilterToSql(filterClause);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ParseFilterToSql_UnsupportedBinaryOperator_Throws()
        {
            var left = new ConstantNode(1);
            var right = new ConstantNode(2);
            var addNode = new BinaryOperatorNode(BinaryOperatorKind.Add, left, right);
            var filterClause = MakeFilterClause(addNode);

            parser.ParseFilterToSql(filterClause);
        }

        #endregion

        #region 8. ConvertNode handling

        [TestMethod]
        public void ParseFilterToSql_ConvertNode_Unwraps()
        {
            var innerNode = new ConstantNode(42);
            var convertNode = new ConvertNode(innerNode, EdmCoreModel.Instance.GetInt32(false));
            var eqNode = new BinaryOperatorNode(
                BinaryOperatorKind.Equal,
                new SingleValueOpenPropertyAccessNode(new ConstantNode("dummy"), "Age"),
                convertNode);
            var filterClause = MakeFilterClause(eqNode);

            var result = parser.ParseFilterToSql(filterClause);
            Assert.AreEqual("Age = 42", result);
        }

        #endregion

        #region 9. tolower / toupper

        [TestMethod]
        public void ParseFilterToSql_ToLower_OnColumn()
        {
            var filter = ParseFilter("tolower(Name) eq 'alice'");
            var result = parser.ParseFilterToSql(filter);
            Assert.AreEqual("LOWER(Name) = 'alice'", result);
        }

        [TestMethod]
        public void ParseFilterToSql_ToUpper_OnColumn()
        {
            var filter = ParseFilter("toupper(Name) eq 'ALICE'");
            var result = parser.ParseFilterToSql(filter);
            Assert.AreEqual("UPPER(Name) = 'ALICE'", result);
        }

        [TestMethod]
        public void ParseFilterToSql_ToLower_BothSides()
        {
            var filter = ParseFilter("tolower(Name) eq tolower('Alice')");
            var result = parser.ParseFilterToSql(filter);
            Assert.AreEqual("LOWER(Name) = LOWER('Alice')", result);
        }

        [TestMethod]
        public void ParseFilterToSql_ToUpper_BothSides()
        {
            var filter = ParseFilter("toupper(Name) eq toupper('Alice')");
            var result = parser.ParseFilterToSql(filter);
            Assert.AreEqual("UPPER(Name) = UPPER('Alice')", result);
        }

        [TestMethod]
        public void ParseFilterToSql_ToLower_WithContains()
        {
            var filter = ParseFilter("contains(tolower(Name), 'ali')");
            var result = parser.ParseFilterToSql(filter);
            Assert.AreEqual("LOWER(Name) LIKE '%ali%'", result);
        }

        [TestMethod]
        public void ParseFilterToSql_ToLower_CombinedWithAnd()
        {
            var filter = ParseFilter("tolower(Name) eq 'alice' and Age gt 25");
            var result = parser.ParseFilterToSql(filter);
            Assert.AreEqual("(LOWER(Name) = 'alice') AND (Age > 25)", result);
        }

        #endregion

        #region 10. Case-insensitive mode (ILIKE)

        [TestMethod]
        public void ParseFilterToSql_CaseInsensitive_Contains()
        {
            var ciParser = new ODataToSqlParser(useCaseInsensitiveFilters: true);
            var filter = ParseFilter("contains(Name, 'Ali')");
            var result = ciParser.ParseFilterToSql(filter);
            Assert.AreEqual("Name ILIKE '%Ali%'", result);
        }

        [TestMethod]
        public void ParseFilterToSql_CaseInsensitive_StartsWith()
        {
            var ciParser = new ODataToSqlParser(useCaseInsensitiveFilters: true);
            var filter = ParseFilter("startswith(Name, 'Ali')");
            var result = ciParser.ParseFilterToSql(filter);
            Assert.AreEqual("Name ILIKE 'Ali%'", result);
        }

        [TestMethod]
        public void ParseFilterToSql_CaseInsensitive_EndsWith()
        {
            var ciParser = new ODataToSqlParser(useCaseInsensitiveFilters: true);
            var filter = ParseFilter("endswith(Name, 'ce')");
            var result = ciParser.ParseFilterToSql(filter);
            Assert.AreEqual("Name ILIKE '%ce'", result);
        }

        [TestMethod]
        public void ParseFilterToSql_CaseInsensitive_ContainsWithAnd()
        {
            var ciParser = new ODataToSqlParser(useCaseInsensitiveFilters: true);
            var filter = ParseFilter("contains(Name, 'Al') and Age gt 25");
            var result = ciParser.ParseFilterToSql(filter);
            Assert.AreEqual("(Name ILIKE '%Al%') AND (Age > 25)", result);
        }

        [TestMethod]
        public void ParseFilterToSql_CaseInsensitive_DefaultFalse_UsesLike()
        {
            var defaultParser = new ODataToSqlParser();
            var filter = ParseFilter("contains(Name, 'Ali')");
            var result = defaultParser.ParseFilterToSql(filter);
            Assert.AreEqual("Name LIKE '%Ali%'", result);
        }

        [TestMethod]
        public void ParseFilterToSql_CaseInsensitive_ExplicitFalse_UsesLike()
        {
            var csParser = new ODataToSqlParser(useCaseInsensitiveFilters: false);
            var filter = ParseFilter("contains(Name, 'Ali')");
            var result = csParser.ParseFilterToSql(filter);
            Assert.AreEqual("Name LIKE '%Ali%'", result);
        }

        [TestMethod]
        public void ParseFilterToSql_CaseInsensitive_EqUnaffected()
        {
            var ciParser = new ODataToSqlParser(useCaseInsensitiveFilters: true);
            var filter = ParseFilter("Name eq 'Alice'");
            var result = ciParser.ParseFilterToSql(filter);
            Assert.AreEqual("Name = 'Alice'", result);
        }

        #endregion
    }
}
