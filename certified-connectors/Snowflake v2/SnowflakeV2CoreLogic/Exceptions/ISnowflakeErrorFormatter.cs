// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace SnowflakeV2CoreLogic.Exceptions
{
    using SnowflakeV2CoreLogic.Models.SnowflakeAPIModels;

    public interface ISnowflakeErrorFormatter
    {
        public bool CanHandle(SnowflakeErrorResponseModel snowflakeErrorResponseModel);

        public string FormattedError(SnowflakeErrorResponseModel error);
    }
}
