using Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomBaseController : ControllerBase
    {
        public IActionResult CreateActionResult<T>(ResponseModelDto<T> response)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return new ObjectResult(null)
                {
                    StatusCode = 204
                };
            }

            return new ObjectResult(response.Data)
            {
                StatusCode = (int)response.StatusCode
            };
        }

        public IActionResult CreateActionResult<T>(ResponseModelDto<T> response, string methodName, Object routeValues)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return CreatedAtAction(methodName, routeValues, null);
            }

            return new ObjectResult(response.Data)
            {
                StatusCode = (int)response.StatusCode
            };
        }
    }
}