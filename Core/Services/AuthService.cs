using Domain.Exceptions;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Services.Abstractions;
using Shared;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services
    {
    public class AuthService(UserManager<AppUser> userManager, IOptions<JwtOptions> options) : IAuthService
        {
        public async Task<UserResultDto> LoginAsync(LoginDto loginDto)
            {
            var user = await userManager.FindByEmailAsync(loginDto.Email);
            if ( user is null ) throw new UnAuthorizedException();

            var flag = await userManager.CheckPasswordAsync(user, loginDto.Password);
            if ( !flag ) throw new UnAuthorizedException();

            return new UserResultDto
                {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await GenerateJwtToken(user)
                };

            }

        public async Task<UserResultDto> RegisterAsync(RegisterDto registerDto)
            {
            var user = new AppUser
                {
                UserName = registerDto.UserName,
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                };

            var result = await userManager.CreateAsync(user, registerDto.Password);

            if ( !result.Succeeded )
                {
                var errors = result.Errors.Select(e => e.Description);
                throw new ValidationException(errors);
                }


            return new UserResultDto
                {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await GenerateJwtToken(user)
                };
            }

        private async Task<string> GenerateJwtToken(AppUser user)
            {
            var jwtOptions = options.Value;


            var authClaim = new List<Claim>
                {
                new Claim (ClaimTypes.Name, user.UserName),
                new Claim (ClaimTypes.Email, user.Email),
                };

            var roles = await userManager.GetRolesAsync(user);
            foreach ( var role in roles )
                {
                authClaim.Add(new Claim(ClaimTypes.Role, role));
                }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey));

            var token = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                claims: authClaim,
                expires: DateTime.UtcNow.AddDays(jwtOptions.ExpireTimeDays),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);

            }

        }
    }
