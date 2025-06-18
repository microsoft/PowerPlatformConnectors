// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace Microsoft.Azure.Connectors.SnowflakeV2Contracts.Constants
{
    /// <summary>
    /// string constants
    /// </summary>
    public static class StringConstants
    {
        /// <summary>
        /// datasets entity set
        /// </summary>
        public const string DataSets = "datasets";

        /// <summary>
        /// tables entity set
        /// </summary>
        public const string Tables = "tables";

        /// <summary>
        /// items entity set
        /// </summary>
        public const string Items = "items";

        /// <summary>
        /// procedures entity set
        /// </summary>
        public const string Procedures = "procedures";

        /// <summary>
        /// pass-through native query entity set
        /// </summary>
        public const string Query = "query";

        /// <summary>
        /// New item trigger
        /// </summary>
        public const string NewItemTrigger = "onnewitems";

        /// <summary>
        /// Updated item trigger
        /// </summary>
        public const string UpdatedItemTrigger = "onupdateditems";

        /// <summary>
        /// Changed item trigger
        /// </summary>
        public const string ChangedItemTrigger = "onchangeditems";

        /// <summary>
        /// Deleted item trigger
        /// </summary>
        public const string DeletedItemTrigger = "ondeleteditems";

        /// <summary>
        /// Default data set
        /// </summary>
        public const string DefaultDataSet = "default";

        /// <summary>
        /// Location header string constant
        /// </summary>
        public const string LocationHeaderString = "Location";

        /// <summary>
        /// The next page marker query parameter name
        /// </summary>
        public const string NextPageMarkerQueryParameter = "nextPageMarker";

        /// <summary>
        /// Read metadata from server
        /// </summary>
        public const string ReadFileMetadataFromServer = "ReadFileMetadataFromServer";

        /// <summary>
        /// Skip delete if file is not found on the server
        /// </summary>
        public const string SkipDeleteIfFileNotFoundOnServer = "SkipDeleteIfFileNotFoundOnServer";

        /// <summary>
        /// Key to represent the timeout
        /// (app config)
        /// </summary>
        public const string Timeout = "TimeoutSeconds";
    }
}
