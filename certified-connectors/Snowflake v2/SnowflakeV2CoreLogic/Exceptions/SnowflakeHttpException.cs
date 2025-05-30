// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.

namespace SnowflakeV2CoreLogic.Exceptions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using SnowflakeV2CoreLogic.Models.SnowflakeAPIModels;

    public class SnowflakeHttpException : System.Exception
    {
        private static readonly List<ISnowflakeErrorFormatter> Formatters = new List<ISnowflakeErrorFormatter>
        {
            new SnowflakeDatabaseError(),
            new SnowflakeWareshouseError(),
        };

        public SnowflakeHttpException(HttpStatusCode httpStatusCode, SnowflakeErrorResponseModel errorData)
            : base(FormatMessage(errorData))
        {
            HTTPStatusCode = httpStatusCode;
            SnowflakeMessage = errorData.Message;
        }

        public string SnowflakeMessage { get; }

        public HttpStatusCode HTTPStatusCode { get; }

        public static HttpResponseMessage CreateHttpResponseMessage(HttpStatusCode httpStatusCode, string message)
        {
            var response = new HttpResponseMessage(httpStatusCode)
            {
                Content = new StringContent(message),
            };
            return response;
        }

        private static string FormatMessage(SnowflakeErrorResponseModel errorData)
        {
            try
            {
                var formatter = Formatters.FirstOrDefault(f => f.CanHandle(errorData));
                return formatter != null ? formatter.FormattedError(errorData) : errorData.Message;
            }
            catch (Exception)
            {
                return errorData.Message;
            }
        }
    }
}