namespace Apis.Config
{
    using PeinearyDevelopment.Framework.BaseClassLibraries.Web.Http;
    using System.Web.Http;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes(new InheritanceDirectRouteProvider());
        }
    }
}
