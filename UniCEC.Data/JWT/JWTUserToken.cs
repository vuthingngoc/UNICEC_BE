﻿
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using UniCEC.Data.Models.DB;
using UniCEC.Data.Repository.ImplRepo.RoleRepo;
using UniCEC.Data.ViewModels.Entities.User;
using UniCEC.Data.ViewModels.Firebase.Auth;

namespace UniCEC.Data.JWT
{
    public class JWTUserToken
    {
        public IConfiguration Configuration { get; }
        public JWTUserToken(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static string GenerateJWTTokenStudent(ViewUser user, string roleName)
        {
            JwtSecurityToken tokenUser = new JwtSecurityToken(
                issuer: "https://securetoken.google.com/unics-e46a4",
                audience: "unics-e46a4",
                claims: new[] {
                 //email
                 new Claim("Email", user.Email),
                 //fullname
                 new Claim("Fullname", user.Fullname),
                 //City-id
                 new Claim("MajorId", user.MajorId.ToString()),
                 //University
                 new Claim("UniversityId", user.UniversityId.ToString()),
                 //UserId(MSSV)
                 new Claim("UserId", user.UserId),
                 //Dob
                 new Claim("DOB", user.Dob),
                 //Role
                 new Claim(ClaimTypes.Role, roleName)
                },
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: new SigningCredentials(
                        key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes("0wPQUnbnoPATU4MJOprB")),
                        algorithm: SecurityAlgorithms.HmacSha256
                        )
                ); ; ;
            return new JwtSecurityTokenHandler().WriteToken(tokenUser);
        }



        public static string GenerateJWTTokenSponsor(ViewUser user, string roleName)
        {
            JwtSecurityToken tokenUser = new JwtSecurityToken(
                issuer: "https://securetoken.google.com/unics-e46a4",
                audience: "unics-e46a4",
                claims: new[] {
                 //email
                 new Claim("Email", user.Email),
                 //fullname
                 new Claim("Fullname", user.Fullname),
                 //Dob
                 new Claim("DOB", user.Dob),
                 //Role
                 new Claim(ClaimTypes.Role, roleName)
                },
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: new SigningCredentials(
                        key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes("0wPQUnbnoPATU4MJOprB")),
                        algorithm: SecurityAlgorithms.HmacSha256
                        )
                ); ; ;
            return new JwtSecurityTokenHandler().WriteToken(tokenUser);
        }

        public static string GenerateJWTTokenAdmin(ViewUser user, string roleName)
        {
            JwtSecurityToken tokenUser = new JwtSecurityToken(
                issuer: "https://securetoken.google.com/unics-e46a4",
                audience: "unics-e46a4",
                claims: new[] {
                 //email
                 new Claim("Email", user.Email),
                 //fullname
                 new Claim("Fullname", user.Fullname),
                 //Dob
                 new Claim("DOB", user.Dob),
                 //Role
                 new Claim(ClaimTypes.Role, roleName)
                },
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: new SigningCredentials(
                        key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes("0wPQUnbnoPATU4MJOprB")),
                        algorithm: SecurityAlgorithms.HmacSha256
                        )
                ); ; ;
            return new JwtSecurityTokenHandler().WriteToken(tokenUser);
        }



        //tạo tạm để trả về FE cho User tiếp tục update
        public static string GenerateJWTTokenUserTemp(ViewUserInfo user)
        {
            JwtSecurityToken tokenUser = new JwtSecurityToken(
                issuer: "https://securetoken.google.com/unics-e46a4",
                audience: "unics-e46a4",
                claims: new[] {
                 //email
                 new Claim("Email", user.Email),
                 //Role
                 new Claim(ClaimTypes.Role, user.RoleName)
                },
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: new SigningCredentials(
                        key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes("0wPQUnbnoPATU4MJOprB")),
                        algorithm: SecurityAlgorithms.HmacSha256
                        )
                ); ; ;
            return new JwtSecurityTokenHandler().WriteToken(tokenUser);
        }



    }
}
