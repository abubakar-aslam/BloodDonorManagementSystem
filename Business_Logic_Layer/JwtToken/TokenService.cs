using Business_Logic_Layer.Roles;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business_Logic_Layer.JwtToken
{
    public class TokenService
    {
        public string GenerateJwtToken(string username)
        {
            var issuer = "https://localhost:7124/"; // Replace with your issuer
            var audience = "https://localhost:7124/"; // Replace with your audience
            var key = Encoding.ASCII.GetBytes("jdhfhdhfkfhdsh798987bhkjahkhsfkjhtext"); // Replace with your secret key


            /////////
            ///

            bool userHasAdminPrivileges = false;  //CheckIfUserHasAdminPrivileges(); // Implement this based on your own logic

            var roles = new List<string>();

            if (userHasAdminPrivileges)
            {
                roles.Add(CustomRoles.Admin);
            }

            else
            {
                roles.Add(CustomRoles.User);
            }


            ///////


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, username),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                    new Claim(ClaimTypes.Role, string.Join(",", roles))
                    
                    // new Claim(ClaimTypes.Role, CustomRoles.Admin),
                    // new Claim(ClaimTypes.Role, CustomRoles.User)
                }),
                Expires = DateTime.UtcNow.AddMinutes(50),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)


            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            //////////
            ///



            return jwtToken;
        }
    }
}
