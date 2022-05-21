﻿
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UniCEC.Data.ViewModels.Entities.User;

namespace UniCEC.Data.JWT
{
    public class JWTUserToken
    {
        public IConfiguration Configuration { get; }
        
        public JWTUserToken(IConfiguration configuration)
        {
            Configuration = configuration;
            
        }

        //tạo tạm để trả về FE cho User tiếp tục update
        public static string GenerateJWTTokenUserTemp(UserModelTemporary user)
        {
            JwtSecurityToken tokenUser = new JwtSecurityToken(
                issuer: "https://securetoken.google.com/unics-e46a4",
                audience: "unics-e46a4",
                claims: new[] {
                 //Id
                 new Claim("Id", user.Id.ToString()),
                 //fullname
                 new Claim("Fullname", user.Fullname),
                 //University
                 new Claim("UniversityId", user.UniversityId.ToString()),
                 //Avatar
                 new Claim ("Avatar", user.Avatar),
                 //Role Id
                 new Claim("RoleId", user.RoleId.ToString()),
                 //Role
                 new Claim(ClaimTypes.Role, user.RoleName),
                },
                expires: DateTime.UtcNow.AddDays(30),
                signingCredentials: new SigningCredentials(
                        key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes("0wPQUnbnoPATU4MJOprB")),
                        algorithm: SecurityAlgorithms.HmacSha256
                        )
                );
            return new JwtSecurityTokenHandler().WriteToken(tokenUser);
        }

        public static string GenerateJWTTokenStudent(ViewUser user, string roleName)
        {
            JwtSecurityToken tokenUser = new JwtSecurityToken(
                issuer: "https://securetoken.google.com/unics-e46a4",
                audience: "unics-e46a4",
                claims: new[] {
                 //---------- STUDENT INFOMATION
                 //Id
                 new Claim("Id", user.Id.ToString()),
                 //fullname
                 new Claim("Fullname", user.Fullname),
                 //University
                 new Claim("UniversityId", user.UniversityId.ToString()),
                 //Avatar
                 new Claim("Avatar", user.Avatar),
                 //Role Id
                 new Claim("RoleId", user.RoleId.ToString()),
                 //Role
                 new Claim(ClaimTypes.Role, roleName),
                },
                expires: DateTime.UtcNow.AddDays(30),
                signingCredentials: new SigningCredentials(
                        key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes("0wPQUnbnoPATU4MJOprB")),
                        algorithm: SecurityAlgorithms.HmacSha256
                        )
                );
            return new JwtSecurityTokenHandler().WriteToken(tokenUser);
        }



        public static string GenerateJWTTokenSponsor(ViewUser user, string roleName, string sponsorId)
        {
            JwtSecurityToken tokenUser = new JwtSecurityToken(
                issuer: "https://securetoken.google.com/unics-e46a4",
                audience: "unics-e46a4",
                claims: new[] {
                 //Id
                 new Claim("Id", user.Id.ToString()),
                 //fullname
                 new Claim("Fullname", user.Fullname),
                 //Avatar
                 new Claim ("Avatar", user.Avatar),
                 //SponsorId
                 new Claim ("SponsorId", sponsorId),
                 //Role Id
                 new Claim("RoleId", user.RoleId.ToString()),
                 //Role
                 new Claim(ClaimTypes.Role, roleName)
                },
                expires: DateTime.UtcNow.AddDays(30),
                signingCredentials: new SigningCredentials(
                        key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes("0wPQUnbnoPATU4MJOprB")),
                        algorithm: SecurityAlgorithms.HmacSha256
                        )
                );

            return new JwtSecurityTokenHandler().WriteToken(tokenUser);
        }

        public static string GenerateJWTTokenAdmin(ViewUser user, string roleName)
        {
            JwtSecurityToken tokenUser = new JwtSecurityToken(
                issuer: "https://securetoken.google.com/unics-e46a4",
                audience: "unics-e46a4",
                claims: new[] {
                 //Id
                 new Claim("Id", user.Id.ToString()),
                 //fullname
                 new Claim("Fullname", user.Fullname),
                 //Avatar
                 new Claim ("Avatar", user.Avatar),
                 //Role Id
                 new Claim("RoleId", user.RoleId.ToString()),
                 //Role
                 new Claim(ClaimTypes.Role, roleName)
                },
                expires: DateTime.UtcNow.AddDays(30),
                signingCredentials: new SigningCredentials(
                        key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes("0wPQUnbnoPATU4MJOprB")),
                        algorithm: SecurityAlgorithms.HmacSha256
                        )
                ); ; ;
            return new JwtSecurityTokenHandler().WriteToken(tokenUser);
        }
    }
}
