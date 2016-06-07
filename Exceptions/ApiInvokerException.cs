namespace PeinearyDevelopment.Framework.BaseClassLibraries.Exceptions
{
    using System;
    using System.Net.Http;
    using System.Runtime.Serialization;

    [Serializable]
    public class ApiInvokerException : Exception
    {
        public ApiInvokerException()
        {
        }

        public ApiInvokerException(HttpResponseMessage message, string httpResponseMessageContent = null) : base(CreateMessage(message, httpResponseMessageContent))
        {
        }

        protected ApiInvokerException(SerializationInfo info, StreamingContext context):base(info, context)
        {
        }

        private static string CreateMessage(HttpResponseMessage message, string httpResponseMessageContent = null)
        {
            if (httpResponseMessageContent == null) httpResponseMessageContent = message.Content.ReadAsStringAsync().Result;

            return $"{{ statusCode: {message.StatusCode}, content: {httpResponseMessageContent} }}";
        }
    }
}
