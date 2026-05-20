using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Token;

namespace WebApi.Controllers
{
    [Authorize]
    public class TokenController(ITokenService tokenService) : CustomBaseController
    {
        [AllowAnonymous]
        [HttpPost("createclientcredential")]
        public async Task<IActionResult> CreateClientAccessToken(GetAccessTokenRequestDto request, CancellationToken cancellationToken)
        {
            return CreateActionResult(await tokenService.CreateClientAccessTokenAsync(request, cancellationToken));
        }

        [HttpPost("revokerefreshtoken/{code}")]
        public async Task<IActionResult> RevokeRefreshToken(Guid code)
        {
            return CreateActionResult(await tokenService.RevokeRefreshToken(code));
        }
    }
}
