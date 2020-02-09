﻿using Application.Services.Token.Validators.Access_Token_Validator;
using Domain;
using Schoolman.Student.Core.Application.Models;
using System.Security.Claims;

namespace Application.Services.Token
{
    public interface IAccessTokenService:
        ITokenValidator<string, Result<ClaimsPrincipal>>,
        ITokenFactory<User, Result<string>>
    {
        string GetUserIdFromClaims(ClaimsPrincipal tokenClaims);
    }
}
