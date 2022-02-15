using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TweetBook2.Domain;
using TweetBook2.Option;

namespace TweetBook2.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        //private readonly RoleManager<IdentityRole> _roleManager;


        public IdentityService(UserManager<IdentityUser> userManager, JwtSettings jwtSettings)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
        }

        public async Task<AuthenticationResult> RegisterAsync(string email, string password)
        {
            var existingUser = await _userManager.FindByNameAsync(email);
            
            if(existingUser != null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User already existed" }
                };
            }
            var newUserId = Guid.NewGuid();
            var newUser = new IdentityUser {Id = newUserId.ToString(), Email = email, UserName = email};
            var createduser = await _userManager.CreateAsync(newUser, password);

            if (!createduser.Succeeded)
            {
                return new AuthenticationResult { Errors = createduser.Errors.Select(x => x.Description) };
            }

            await _userManager.AddToRoleAsync(newUser, "Admin");

            return await GenerateAuthenticationResultForUserAsync(newUser);
        }
        private async Task<AuthenticationResult> GenerateAuthenticationResultForUserAsync(IdentityUser newUser)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, newUser.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, newUser.Email),
                new Claim("id", newUser.Id)
            };

            var userRoles = await _userManager.GetRolesAsync(newUser);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                //var role = await _roleManager.FindByNameAsync(userRole);
                //if (role == null) continue;
                //var roleClaims = await _roleManager.GetClaimsAsync(role);

                //foreach (var roleClaim in roleClaims)
                //{
                //    if (claims.Contains(roleClaim))
                //        continue;

                //    claims.Add(roleClaim);
                //}
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new AuthenticationResult { Success = true, Token = tokenHandler.WriteToken(token) };
        }

        public async Task<AuthenticationResult> Login(string email, string password)
        {
            var user = await _userManager.FindByNameAsync(email);

            if (user == null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User does not exist" }
                };
            }

            var userHasValidPassword = await _userManager.CheckPasswordAsync(user, password);
            if (!userHasValidPassword)
            {
                return new AuthenticationResult { Errors = new[] { "Password is wrong" } };
            }
            return await GenerateAuthenticationResultForUserAsync(user);
        }
    }
}
