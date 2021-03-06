﻿namespace PeinearyDevelopment.Framework.BaseClassLibraries.Contracts
{
    using System;
    using System.Globalization;
    using System.Net.Http;
    using System.Runtime.Serialization;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors", Justification = "This is only meant for use in one specific case. 'Standard Exception Constructors' aren't applicable.")]
    [Serializable]
    public class ApiInvokerException : Exception
    {
        public ApiInvokerException()
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        public ApiInvokerException(HttpResponseMessage message, string httpResponseMessageContent = null) : base(CreateMessage(message, httpResponseMessageContent))
        {
        }

        protected ApiInvokerException(SerializationInfo info, StreamingContext context):base(info, context)
        {
        }

        private static string CreateMessage(HttpResponseMessage message, string httpResponseMessageContent = null)
        {
            if (httpResponseMessageContent == null) httpResponseMessageContent = message.Content.ReadAsStringAsync().Result;

            return string.Format(CultureInfo.InvariantCulture, "{{ statusCode: {0}, content: {1} }}", message.StatusCode, httpResponseMessageContent);
        }
    }
}
