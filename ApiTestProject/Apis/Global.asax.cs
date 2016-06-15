namespace Apis
{
    using PeinearyDevelopment.Framework.BaseClassLibraries.Web.Http;

    using System;
    using Config;

    using System.Web;
    using System.Web.Http;

    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AutoMapperConfig.Configure();
            UnityConfig.RegisterComponents();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            HttpContext.Current.User = new IdPrincipal(string.Empty);
        }
    }
}
