// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace SnowflakeV2CoreLogic.Exceptions
{
    using SnowflakeV2CoreLogic.Models.SnowflakeAPIModels;

    public class SnowflakeDatabaseError : ISnowflakeErrorFormatter
    {
        public bool CanHandle(SnowflakeErrorResponseModel snowflakeErrorResponseModel)
        {
            return snowflakeErrorResponseModel.Code == SnowflakeErrorCodes.DatabaseNotFound;
        }

        public string FormattedError(SnowflakeErrorResponseModel error)
        {
            return error.Message + SnowflakeErrorCodes.DatabaseErrorMessage;
        }
    }
}