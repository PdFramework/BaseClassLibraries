namespace PeinearyDevelopment.Framework.BaseClassLibraries.Web.Http
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http.Controllers;
    using System.Web.Http.Routing;

    // http://www.strathweb.com/2016/06/inheriting-route-attributes-in-asp-net-web-api/
    public class InheritanceDirectRouteProvider : DefaultDirectRouteProvider
    {
        protected override IReadOnlyList<IDirectRouteFactory> GetActionRouteFactories(HttpActionDescriptor actionDescriptor)
        {
            if (actionDescriptor == null) throw new ArgumentNullException(nameof(actionDescriptor));

            return actionDescriptor.GetCustomAttributes<IDirectRouteFactory>(true);
        }
    }
}
