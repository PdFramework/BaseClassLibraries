namespace PeinearyDevelopment.Framework.BaseClassLibraries.Web.Http
{
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;

    public class NoContentResult : IHttpActionResult
    {
        private readonly HttpRequestMessage _request;

        public NoContentResult()
        {
        }

        public NoContentResult(HttpRequestMessage request)
        {
            _request = request;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = _request.CreateResponse(HttpStatusCode.NoContent);
            return Task.FromResult(response);
        }
    }
}