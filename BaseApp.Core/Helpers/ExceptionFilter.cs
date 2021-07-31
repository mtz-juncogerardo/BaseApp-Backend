using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BaseApp.Core.Helpers
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly IModelMetadataProvider _modelMetadataProvider;

        public ExceptionFilter(IModelMetadataProvider modelMetadataProvider)
        {
            _modelMetadataProvider = modelMetadataProvider;
        }

        public void OnException(ExceptionContext ex)
        {
            var httpCode = IsValidHttpCode(ex.Exception.HResult) ? ex.Exception.HResult : 500;
            ex.HttpContext.Response.StatusCode = httpCode;
            ex.Result = new JsonResult(ex.Exception.Message);
        }

        private static bool IsValidHttpCode(int code)
        {
            return code.Equals(400) 
                || code.Equals(401)
                || code.Equals(404)
                || code.Equals(405)
                || code.Equals(500)
                || code.Equals(200);
        }
    }
}