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

            var id = (int)context.ActionArguments.Values.First()!;

            if(!int.TryParse(id.ToString(), out int articleId))
            {
                var errorMessage = "Id değeri sayısal olmalıdır!";
                var responseModel = ResponseModelDto<NoContent>.Failure(errorMessage);
                context.Result = new NotFoundObjectResult(responseModel);
            }

            var hasArticle = articleRepository.GetById(articleId).Result;
            if (hasArticle is null)
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
