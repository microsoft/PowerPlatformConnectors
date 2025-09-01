// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace SnowflakeV2CoreLogic.Models.SnowflakeAPIModels
{
    using System.Collections.Generic;
    using Microsoft.Azure.Connectors.SnowflakeV2Contracts.Models;
    using SnowflakeV2CoreLogic.Utilities;

    public class SnowflakeTableData : SnowflakeAPIResponseModel
    {
        public List<Item> ToListOfItems()
        {
            List<Item> items = new List<Item>();

            // Loop through the metadata and create a list of indexed headers
            var columnData = ResultSetMetaData.RowType.ToArray();

            // Loop through each row in the data block
            foreach (var row in Data)
            {
                // Create a new item
                Item item = new Item();

                // Loop through the columns and add the data to the item
                for (int i = 0; i < columnData.Length; i++)
                {
                    var dataType = columnData[i].Type;

                    // Scale = Precision to the right of the decimal place
                    int? precisionRightOfDecimal = columnData[i].Scale;

                    // Cast the datatype to the correct type
                    item.DynamicProperties.Add(columnData[i].Name, SnowflakeToODataHelper.CastSnowflakeDataToCorrectType(dataType, precisionRightOfDecimal, row[i]));
                }

                item.EntityTag = null;

                items.Add(item);
            }

            return items;
        }

        public List<Dictionary<string, object>> ToGenericDictionaryList()
        {
            List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();

            // Loop through the metadata and create a list of indexed headers
            var columnData = ResultSetMetaData.RowType.ToArray();

            // Loop through each row in the data block
            foreach (var row in Data)
            {
                // Create a new item
                Dictionary<string, object> item = new Dictionary<string, object>();

                // Loop through the columns and add the data to the item
                for (int i = 0; i < columnData.Length; i++)
                {
                    var dataType = columnData[i].Type;

                    // Scale = Precision to the right of the decimal place
                    int? precisionRightOfDecimal = columnData[i].Scale;

                    // Convert the data to the correct type
                    item.Add(columnData[i].Name, SnowflakeToODataHelper.CastSnowflakeDataToCorrectType(dataType, precisionRightOfDecimal, row[i]));
                }

                items.Add(item);
            }

            return items;
        }
    }
}