﻿using Application.Services;
using Domain;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using System;
using System.Threading.Tasks;
using Test.Shared;
using Xunit;

namespace Test.PersitenceLayer
{
    public class RepositoryTest : TestBase
    {
        public RepositoryTest(TestWebAppFactory testWebAppFactory) : base(testWebAppFactory) { }

        [Fact(DisplayName = "IRepository<User> Add and Save new User")]
        public async Task UserRepositoryAddAndSave()
        {
            var userRepository = factory.Services.GetRequiredService<IRepository<User>>();

            string Id = Guid.NewGuid().ToString();

            await userRepository.AddOrUpdateAsync(new User()
            {
                Id = Id,
                FirstName = "Steve",
                LastName = "Corney"
            });

            var user = await  userRepository.FindAsync(Id);

            Assert.NotNull(user);
        }


        [Fact(DisplayName = "IRepository<User> finds saved user")]
        public async Task UserRepositoryFindUserAfterSaving()
        {
            var userRepository = factory.Services.GetRequiredService<IRepository<User>>();
            string Id = Guid.NewGuid().ToString();

            await userRepository.AddOrUpdateAsync(new User()
            {
                Id = Id,
                FirstName = "Steve",
                LastName = "Corney"
            });

            var user = await userRepository.FindAsync(Id);

            Assert.NotNull(user);
            Assert.Equal(Id, user.Id);
        }
    }
}
