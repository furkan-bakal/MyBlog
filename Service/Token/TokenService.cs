using Core;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;


namespace Service.Token
{
    public class TokenService(IOptions<TokenOptions> tokenOptions, IOptions<Clients> clients): ITokenService
    {
        public Task<ResponseModelDto<TokenResponseDto>> GetAccessTokenAsync(GetAccessTokenRequestDto request, CancellationToken cancellationToken)
        {
            if(!clients.Value.Items.Any(x => x.Id == request.ClientId && x.Secret == request.ClientSecret))
            {
                return Task.FromResult(ResponseModelDto<TokenResponseDto>.Failure("Invalid client credentials", HttpStatusCode.BadRequest));
            }

            var claimAsClientId = new Claim("clientId", request.ClientId);
            var claims = new List<Claim>()
            {
                new Claim("clientId", request.ClientId)
            };
            var tokenExpire = DateTime.Now.AddHours(tokenOptions.Value.ExpireByHour);
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.Value.Signature));

            var jwtToken = new JwtSecurityToken(
                claims: claims,
                expires: tokenExpire,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            var handler = new JwtSecurityTokenHandler();

            var token = handler.WriteToken(jwtToken);

            return Task.FromResult(ResponseModelDto<TokenResponseDto>.Success(new TokenResponseDto(token), HttpStatusCode.OK));

        }
    }
}