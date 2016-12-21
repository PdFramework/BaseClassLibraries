using PeinearyDevelopment.Framework.BaseClassLibraries.Web.WebActivator;
[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(WebApplicationKeepAliveWebActivator), "PreStart")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(WebApplicationKeepAliveWebActivator), "Shutdown")]

namespace PeinearyDevelopment.Framework.BaseClassLibraries.Web.WebActivator
{
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web.Http;

    public static class WebApplicationKeepAliveWebActivator
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "PreStart", Justification = "Named as such to be in line with the casing of the WebActivator and ASP.NET method naming conventions.")]
        public static void PreStart()
        {
            GlobalConfiguration.Configuration.MessageHandlers.Add(new KeepAliveHandler());
        }

        public static void Shutdown()
        {
            //http://weblog.west-wind.com/posts/2013/Oct/02/Use-IIS-Application-Initialization-for-keeping-ASPNET-Apps-alive
            var keepAliveBaseAddress = ConfigurationManager.AppSettings["KeepAlive.Endpoint"];
            var controllerName = GlobalConfiguration.Configuration.Services.GetApiExplorer().ApiDescriptions.First().ActionDescriptor.ControllerDescriptor.ControllerName;
            var address = Path.Combine(keepAliveBaseAddress, controllerName, "keepalive");
            using (var client = new WebClient())
            {
                client.DownloadString(address);
            }
        }
    }
}