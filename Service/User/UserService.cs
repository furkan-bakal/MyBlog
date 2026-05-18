using Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Repository.Identity;
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
        IOptions<CustomTokenOptions> tokenOptions
        )
    {
        public async Task<ResponseModelDto<string>> SignUpAsync(SignUpRequestDto request)
        {
            var user = new AppUser
            {
                UserName = request.UserName,
                Email = request.Email,
                Id = Guid.NewGuid().ToString()
            };
            var result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return ResponseModelDto<string>.Failure(result.Errors.Select(e => e.Description).ToList());
            }

            return ResponseModelDto<string>.Success(user.Id, HttpStatusCode.Created);
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

            var tokenExpire = DateTime.Now.AddHours(tokenOptions.Value.ExpireByHour);
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.Value.Signature));

            var jwtToken = new JwtSecurityToken(
                claims: userClaimList,
                expires: tokenExpire,
                issuer: tokenOptions.Value.Issuer,
                audience: tokenOptions.Value.Audience,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            var handler = new JwtSecurityTokenHandler();

            var token = handler.WriteToken(jwtToken);

            return ResponseModelDto<TokenResponseDto>.Success(new TokenResponseDto(token), HttpStatusCode.OK);

        }

    }
}
