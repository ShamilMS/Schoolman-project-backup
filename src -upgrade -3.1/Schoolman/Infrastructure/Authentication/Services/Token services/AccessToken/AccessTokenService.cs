﻿using Application.Common.Models;
using Application.Services.Token;
using Application.Services.Token.Validators.Access_Token_Validator;
using Authentication.Options;
using Domain;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Services.New_services
{
    public class AccessTokenService : IAccessTokenService
    {
        private readonly IAuthTokenClaimService claimsService;
        private readonly JwtOptions jwtOptions;

        public AccessTokenService(IAuthTokenClaimService claimsBuilder,
                                  IOptionsMonitor<JwtOptions> jwtOptions)
        {
            claimsService = claimsBuilder;
            this.jwtOptions = jwtOptions.CurrentValue;
        }


        public string GetUserIdFromClaims(ClaimsPrincipal tokenClaims)
        {
            return claimsService.GetUserIdFromClaims(tokenClaims.Claims);
        }


        public Task<Result<string>> GenerateTokenAsync(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();


            Claim[] claims = claimsService.BuildClaims(user);
            byte[] secretKeyBytes = Encoding.UTF8.GetBytes(jwtOptions.SecretKey);

            var tokenDesciptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(jwtOptions.ExpirationTime),
                Audience = jwtOptions.Audience,
                Issuer = jwtOptions.Issuer,
                SigningCredentials = new SigningCredentials(key: new SymmetricSecurityKey(secretKeyBytes),
                                                         algorithm: SecurityAlgorithms.HmacSha256)
            };

            SecurityToken securityToken = jwtTokenHandler.CreateToken(tokenDesciptor);
            string accessToken = jwtTokenHandler.WriteToken(securityToken);
            return Task.FromResult(Result<string>.Success(accessToken));
        }

      
        // todo: Add jwt time validation
        public async Task<Result<ClaimsPrincipal>> ValidateTokenAsync(string accessToken)
        {

            // No worries. Just explicit conversion operator
            TokenValidationParameters validationParams = (TokenValidationParameters) jwtOptions;


            validationParams.ValidateLifetime = false;

            var jwtHandler = new JwtSecurityTokenHandler();
            if (!jwtHandler.CanReadToken(accessToken))
                return Result<ClaimsPrincipal>.Failure("Access token is not valid");

            try
            {
                var principal = jwtHandler.ValidateToken(token: accessToken,
                                         validationParameters: validationParams, out SecurityToken token);



                if (principal == null)
                    return Result<ClaimsPrincipal>.Failure("Access token is not valid");

                return Result<ClaimsPrincipal>.Success(principal);
            }
            catch (ArgumentException)
            {
                return Result<ClaimsPrincipal>.Failure("Access token is invalid");
            }

        }


    }
}
