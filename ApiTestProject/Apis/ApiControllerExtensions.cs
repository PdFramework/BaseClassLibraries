namespace Apis
{
    using System.Web.Http;

    public static class ApiControllerExtensions
    {
        public static NoContentResult NoContent(this ApiController controller)
        {
            return new NoContentResult(controller.Request);
        }
    }
}
