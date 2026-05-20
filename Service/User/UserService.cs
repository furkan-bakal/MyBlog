using Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Repository;
using Repository.Identity;
using Repository.Tokens;
using Service.Token;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;


namespace Service.User
{
    public class UserService(
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        IOptions<CustomTokenOptions> tokenOptions,
        IGenericRepository<RefreshToken> refreshTokenRepository,
        IUnitOfWork unitOfWork
        )
    {
        public async Task<ResponseModelDto<string>> SignUpAsync(SignUpRequestDto request)
        {

            if(IsUserMailExist(request.Email))
            {
                return ResponseModelDto<string>.Failure("Email is already exist!");
            }

            var user = new AppUser
            {
                UserName = request.UserName,
                Email = request.Email,
                Id = Guid.NewGuid()
            };
            var result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return ResponseModelDto<string>.Failure(result.Errors.Select(e => e.Description).ToList());
            }

            return ResponseModelDto<string>.Success(user.Id.ToString(), HttpStatusCode.Created);
        }

        public async Task<ResponseModelDto<TokenResponseDto>> SignInAsync(SignInRequestDto request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return ResponseModelDto<TokenResponseDto>.Failure(new List<string> { "Invalid email or password!" });
            }
            var result = await userManager.CheckPasswordAsync(user, request.Password);
            if (!result)
            {
                return ResponseModelDto<TokenResponseDto>.Failure(new List<string> { "Invalid email or password!" });
            }

            var userClaimList = await CreateUserClaim(user, new List<string>());

            var accessToken = CreateAccessToken(userClaimList, tokenOptions.Value);

            var refreshToken = await CreateOrUpdateRefreshToken(user.Id);

            await unitOfWork.CommitAsync();
            return ResponseModelDto<TokenResponseDto>.Success(new TokenResponseDto(accessToken, refreshToken));

        }

        public async Task<ResponseModelDto<TokenResponseDto>> SignInByRefreshTokenAsync(CreateAccessTokenByRefreshTokenRequestDto request)
        {
            var hasRefrehToken = await refreshTokenRepository.Where(x => x.Code == request.RefreshToken).SingleOrDefaultAsync();
            if (hasRefrehToken is null)
            {
                return ResponseModelDto<TokenResponseDto>.Failure("Refresh token not found!", HttpStatusCode.NotFound);
            }
            if (hasRefrehToken.ExpireDate < DateTime.Now)
            {
                return ResponseModelDto<TokenResponseDto>.Failure("Refresh token expired!", HttpStatusCode.BadRequest);
            }
            var user = await userManager.FindByIdAsync(hasRefrehToken.UserId.ToString());
            if (user is null)
            {
                return ResponseModelDto<TokenResponseDto>.Failure("User not found!", HttpStatusCode.NotFound);
            }
            var userClaimList = await CreateUserClaim(user, new List<string>());
            var accessToken = CreateAccessToken(userClaimList, tokenOptions.Value);
            var refreshToken = await CreateOrUpdateRefreshToken(user.Id);
            await unitOfWork.CommitAsync();
            return ResponseModelDto<TokenResponseDto>.Success(new TokenResponseDto(accessToken, refreshToken));
        }


        private async Task<string> CreateOrUpdateRefreshToken(Guid userId)
        {
            var hasRefreshToken = await refreshTokenRepository.Where(x => x.UserId == userId).SingleOrDefaultAsync();

            if (hasRefreshToken is null)
            {
                hasRefreshToken = new RefreshToken
                {
                    UserId = userId,
                    Code = Guid.NewGuid(),
                    ExpireDate = DateTime.Now.AddDays(tokenOptions.Value.RefreshTokenByExpireDay)
                };

                await refreshTokenRepository.Add(hasRefreshToken);
            }
            else
            {
                hasRefreshToken.Code = Guid.NewGuid();
                hasRefreshToken.ExpireDate = DateTime.Now.AddDays(tokenOptions.Value.RefreshTokenByExpireDay);
                await refreshTokenRepository.Update(hasRefreshToken);
            }

            return hasRefreshToken.Code.ToString();
        }

        private string CreateAccessToken(List<Claim> claimList, CustomTokenOptions tokenOptions)
        {

            var tokenExpire = DateTime.Now.AddHours(tokenOptions.ExpireByHour);
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.Signature));

            var jwtToken = new JwtSecurityToken(
                claims: claimList,
                expires: tokenExpire,
                issuer: tokenOptions.Issuer,
                audience: tokenOptions.Audience,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            var handler = new JwtSecurityTokenHandler();

            return handler.WriteToken(jwtToken);
        }

        private async Task<List<Claim>> CreateUserClaim(AppUser user, List<string> roles)
        {
            var userClaimList = new List<Claim>();

            userClaimList.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            userClaimList.Add(new Claim(ClaimTypes.Name, user.UserName!));

            var userRoles = await userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                userClaimList.Add(new Claim(ClaimTypes.Role, role));
            }

            var userClaims = await userManager.GetClaimsAsync(user);

            foreach (var claim in userClaims)
            {
                userClaimList.Add(new Claim(claim.Type, claim.Value));
            }

            foreach (var role in userRoles)
            {
                var appRole = await roleManager.FindByNameAsync(role);
                if (appRole is null)
                {
                    continue;
                }

                var roleClaims = await roleManager.GetClaimsAsync(appRole);
                foreach (var roleClaim in roleClaims)
                {
                    userClaimList.Add(roleClaim);
                }

            }
            return userClaimList;
        }

        public bool IsUserMailExist(string email)
        {
            var user = userManager.FindByEmailAsync(email).Result;
            return user is not null;
        }
    }
}