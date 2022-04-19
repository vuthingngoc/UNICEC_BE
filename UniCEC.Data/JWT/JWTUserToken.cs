
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.RoleRepo;
using UniCEC.Data.ViewModels.Entities.User;

namespace UniCEC.Data.JWT
{
    public class JWTUserToken
    {
        
        public static string GenerateJWTToken(ViewUser user, string roleName)
        {
            JwtSecurityToken tokenUser = new JwtSecurityToken(
                issuer: "",
                audience: "",
                claims: new[] {
                 //email
                 new Claim("email", user.Email),
                 //fullname
                 new Claim("fullname", user.Fullname),
                 //UserId(MSSV)
                 new Claim("userId", user.UserId),
                 //Dob
                 new Claim("DOB", user.Dob),
                 //Role
                 new Claim(ClaimTypes.Role, roleName)
                },
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: new SigningCredentials(
                        key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Key Need To Be Private")),
                        algorithm: SecurityAlgorithms.HmacSha256
                        )
                ); ; ;
            return new JwtSecurityTokenHandler().WriteToken(tokenUser);
        }


    }
}
