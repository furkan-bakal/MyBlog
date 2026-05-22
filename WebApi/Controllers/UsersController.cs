using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Token;
using Service.User;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(UserService userService) : CustomBaseController
    {
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(SignUpRequestDto request)
        {
            return CreateActionResult(await userService.SignUpAsync(request));
        }

        [Authorize]
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(SignInRequestDto request)
        {
            return CreateActionResult(await userService.SignInAsync(request));
        }

        [Authorize]
        [HttpPost("signinbyrefreshtoken")]
        public async Task<IActionResult> SignInByRefreshToken(CreateAccessTokenByRefreshTokenRequestDto request)
        {
            return CreateActionResult(await userService.SignInByRefreshTokenAsync(request));
        }
    }
}