using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Nicepet_API.Models;
using System.Security.Claims;
using System.Linq;

namespace Nicepet_API.Helpers.JWT
{
    public class JwtToMail
    {
        private JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

        private byte[] key = Encoding.ASCII.GetBytes("[SECRET USED TO SIGN AND VERIFY JWT TOKENS, IT CAN BE ANY STRING]");



        public SignUp ValidateJwtToken(string token)
        {
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = (jwtToken.Claims.First(x => x.Type == "userid").Value).ToString();
                var tokenRole = (jwtToken.Claims.First(x => x.Type == "Role").Value).ToString();

                // return user id from JWT token if validation successful
                return new SignUp { UserId = Convert.ToInt32(userId), Token = tokenRole };
            }
            catch (Exception ex)
            {
                UserInformation userInformation = new UserInformation
                {
                    Error = "ValidateToken "+ex.Message.ToString()
                };
                // return null if validation fails
                return new SignUp();
            }
        }


        public string GenerateJwtToken(int userId, string tokenRole)
        {
            try
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { new Claim("userid", userId.ToString()), new Claim("Role", tokenRole) }),
                    Expires = DateTime.UtcNow.AddMinutes(90),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                                 SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                UserInformation userInformation = new UserInformation
                {
                    Error = "GenerateJwtToken " + ex.Message.ToString()
                };
                return "GenerateJwtToken " + ex.Message.ToString();
            }
        }


    }
}
