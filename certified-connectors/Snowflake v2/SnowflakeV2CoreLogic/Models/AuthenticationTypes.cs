// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace SnowflakeV2CoreLogic.Models
{
    /// <summary>
    /// Enum representing types of authentications
    /// </summary>
    public enum AuthenticationType
    {
        /// <summary>
        /// Microsoft Entra ID
        /// </summary>
        AAD,

        /// <summary>
        /// User Delegated
        /// </summary>
        AADUserDelegated,
    }
}
