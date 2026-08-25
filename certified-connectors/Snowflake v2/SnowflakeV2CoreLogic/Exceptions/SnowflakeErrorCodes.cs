// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace SnowflakeV2CoreLogic.Exceptions
{
    public static class SnowflakeErrorCodes
    {
        public const string DatabaseNotFound = "391918";
        public const string DatabaseErrorMessage = " Please check the Database name and case, and make sure it has same name/case as in Snowflake account.";

        public const string WarehouseNotFound = "391920";
        public const string WarehouseErrorMessage = " Please check the Warehouse name and case, and make sure it has same name/case as in Snowflake account.";
    }
}