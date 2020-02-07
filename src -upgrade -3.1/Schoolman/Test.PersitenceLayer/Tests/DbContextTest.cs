using Application.Services;
using Domain;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using System;
using System.Threading.Tasks;
using Test.Shared;
using Xunit;

namespace Test.PersitenceLayer
{
    public class DbContextTest : BasicTest
    {
        public DbContextTest(TestWebAppFactory testWebAppFactory) : base(testWebAppFactory) { }


        [Fact(DisplayName = "SchoolmanContext is registered in IoC")]
        public void SchoolmanContextNotNull()
        {
            var context = factory.Services.GetRequiredService<SchoolmanContext>();

            Assert.NotNull(context);
        }


        [Fact(DisplayName = "IRepository<User> Add and Save new User() entit")]
        public async Task UserRepositoryAddAndSave()
        {
            var userRepository = factory.Services.GetRequiredService<IRepository<User>>();

            await userRepository.Set.AddAsync(new User()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "Steve",
                LastName = "Corney"
            });

            int affected = await userRepository.SaveChangesAsync();
            Assert.True(affected > 0);
        }


        [Fact(DisplayName = "IRepository<User> finds saved user")]
        public async Task UserRepositoryFindUserAfterSaving()
        {
            var userRepository = factory.Services.GetRequiredService<IRepository<User>>();
            string Id = Guid.NewGuid().ToString();

            await userRepository.Set.AddAsync(new User()
            {
                Id = Id,
                FirstName = "Steve",
                LastName = "Corney"
            });

            var user = await userRepository.Set.FindAsync(Id);

            Assert.NotNull(user);
            Assert.Equal(Id, user.Id);
        }


    }
}