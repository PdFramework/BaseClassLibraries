namespace PeinearyDevelopment.Framework.BaseClassLibraries.Web.WebActivator
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    public class KeepAliveHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            if (request.RequestUri.ToString().Contains("keepalive"))
            {
                // Create the response.
                var response = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent("Staying alive!")
                };

                // Note: TaskCompletionSource creates a task that does not contain a delegate.
                var tsc = new TaskCompletionSource<HttpResponseMessage>();
                tsc.SetResult(response);   // Also sets the task state to "RanToCompletion"
                return tsc.Task;
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
