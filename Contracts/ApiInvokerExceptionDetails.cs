namespace PeinearyDevelopment.Framework.BaseClassLibraries.Contracts
{
    using System.Net;

    public class ApiInvokerExceptionDetails
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public string StackTrace { get; set; }
    }
}
