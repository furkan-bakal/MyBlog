using Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Filters
{
    public class ValidationFilter: IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errorMessages = context.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                var responseModel = ResponseModelDto<NoContent>.Failure(errorMessages);
                context.Result = new BadRequestObjectResult(responseModel);
            }
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
