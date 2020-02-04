﻿using Application.Common.Helpers;
using Application.Common.Models;
using Application.Models;
using Application.Users;
using AutoMapper;
using Domain;
using Schoolman.Student.Core.Application.Interfaces;
using Schoolman.Student.Core.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Services
{
    /// <summary>
    /// Service for user authentication
    /// </summary>
    public class AuthenticationService : IAuthService
    {
        private readonly IUserService userService;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;

        public AuthenticationService(IUserService userService,
                                     ITokenService tokenService,
                                     IMapper mapper)
        {
            this.userService = userService;
            this.tokenService = tokenService;
            this.mapper = mapper;
        }


        /// <summary>
        /// Registers user and return registration result
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<Result<User>> RegisterUserAsync(UserRegistrationRequest model)
        {
            // password and Id are empty
            // will be added by userService
            var newUser = mapper.Map<User>(model);

            // User creation
            Result<User> userCreationResult = await userService.CreateUser(newUser, model.Password);
            if (!userCreationResult.Succeeded)
                return userCreationResult;

            // Send email to created User
            Result emailSendingResult = await userService.SendConfirmationEmail(userCreationResult.Response);
            if (!emailSendingResult.Succeeded)
                return Result<User>.Failure(emailSendingResult.Errors);


            return Result<User>.Success(userCreationResult.Response);
        }


        /// <summary>
        /// Generates token for registered and email-confirmed user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<Result<AuthenticationCredential>> LoginUserAsync(string email, string password)
        {
            if (Assert.Is.NullOrWhiteSpace(email, password))
                return Result<AuthenticationCredential>.Failure("User credentials invalid");

            var user = await userService.FindUserAsync(u => u.Email == email);

            if (user == null)
                return Result<AuthenticationCredential>.Failure("User credentials invalid");

            var result = await userService.CheckUserAsync(user, ops => ops.ConfirmPassword(password)
                                                                          .ConfirmedEmail(true));
            if (!result.Succeeded)
                return Result<AuthenticationCredential>.Failure(result.Errors);

            return await tokenService.GenerateTokensAsync(user.Id);
        }

    }
}