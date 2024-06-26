﻿using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shopping.Business.Core;
using Shopping.Business.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Shopping.Business.Services
{
    public class TokenService : ITokenService
    {
        readonly IConfiguration configuration;

        public TokenService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Task<GenerateTokenResponse> GenerateToken(GenerateTokenRequest request)
        {
            SymmetricSecurityKey symmetricSecurityKey = new(Encoding.ASCII.GetBytes(configuration["AppSettings:Secret"]));

            var dateTimeNow = DateTime.UtcNow;

            JwtSecurityToken jwt = new(
                    issuer: configuration["AppSettings:ValidIssuer"],
                    audience: configuration["AppSettings:ValidAudience"],
                    claims: new List<Claim> {
                    new(ClaimTypes.NameIdentifier, request.UserId.ToString()),
                    new(ClaimTypes.Role,request.Role)
                    },
                    notBefore: dateTimeNow,
                    expires: dateTimeNow.Add(TimeSpan.FromDays(1)),
                    signingCredentials: new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256)
                );

            return Task.FromResult(new GenerateTokenResponse
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwt),
                TokenExpireDate = dateTimeNow.Add(TimeSpan.FromMinutes(500))
            });
        }
    }
}
