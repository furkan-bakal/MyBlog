using Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Repository;
using System.Net;

namespace WebApi.Filters
{
    public class NotFoundFilter(IArticleRepository articleRepository): Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var actionMethod = ((ControllerBase)context.Controller).ControllerContext.ActionDescriptor.ActionName;

            var idValue = context.ActionArguments.Values.FirstOrDefault();

            if(idValue == null || !Guid.TryParse(idValue.ToString(), out Guid articleId))
            {
                var errorMessage = "Id değeri geçersiz (Guid olmalıdır)!";
                var responseModel = ResponseModelDto<NoContent>.Failure(errorMessage);
                context.Result = new BadRequestObjectResult(responseModel);
                return;
            }

            var hasArticle = articleRepository.HasExist(articleId).Result;
            if (!hasArticle)
            {
                var errorMessage = $"Article with id {articleId} not found in {actionMethod} method.";
                var responseModel = ResponseModelDto<NoContent>.Failure(errorMessage, HttpStatusCode.NotFound);
                context.Result = new NotFoundObjectResult(responseModel);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
