using Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Repository;
using Repository.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;


namespace Service.Token
{
    public class TokenService(IOptions<CustomTokenOptions> tokenOptions, IOptions<Clients> clients, IGenericRepository<RefreshToken> refreshTokenRepository) : ITokenService
    {
        public Task<ResponseModelDto<TokenResponseDto>> CreateClientAccessTokenAsync(GetAccessTokenRequestDto request, CancellationToken cancellationToken)
        {
            if (!clients.Value.Items.Any(x => x.Id == request.ClientId && x.Secret == request.ClientSecret))
            {
                return Task.FromResult(ResponseModelDto<TokenResponseDto>.Failure("Invalid client credentials", HttpStatusCode.BadRequest));
            }

            var claimAsClientId = new Claim("clientId", request.ClientId);
            var claims = new List<Claim>()
            {
                new Claim("clientId", request.ClientId)
            };
            var tokenExpire = DateTime.UtcNow.AddHours(tokenOptions.Value.ExpireByHour);
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.Value.Signature));

            var jwtToken = new JwtSecurityToken(
                claims: claims,
                expires: tokenExpire,
                issuer: tokenOptions.Value.Issuer,
                audience: tokenOptions.Value.Audience,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            var handler = new JwtSecurityTokenHandler();

            var token = handler.WriteToken(jwtToken);

            return Task.FromResult(ResponseModelDto<TokenResponseDto>.Success(new TokenResponseDto(token, string.Empty), HttpStatusCode.OK));

        }

        public async Task<ResponseModelDto<NoContent>> RevokeRefreshToken(Guid code)
        {
            var hasRefreshToken = await refreshTokenRepository.Where(x => x.Code == code).SingleOrDefaultAsync();
            if (hasRefreshToken is null)
            {
                return ResponseModelDto<NoContent>.Failure("Refresh token not found!", HttpStatusCode.NotFound);
            }

            await refreshTokenRepository.Remove(hasRefreshToken);
            return ResponseModelDto<NoContent>.Success(HttpStatusCode.NoContent);
        }
    }
}