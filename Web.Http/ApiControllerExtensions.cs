namespace PeinearyDevelopment.Framework.BaseClassLibraries.Web.Http
{
    using System.Web.Http;

    public static class ApiControllerExtensions
    {
        public static NoContentResult NoContent(this ApiController controller)
        {
            return controller == null ? new NoContentResult() : new NoContentResult(controller.Request);
        }
    }
}
