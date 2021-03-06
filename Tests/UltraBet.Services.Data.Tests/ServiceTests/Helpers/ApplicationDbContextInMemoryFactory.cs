namespace UltraBet.Services.Data.Tests.ServiceTests.Helpers
{
    using System;

    using Microsoft.EntityFrameworkCore;
    using UltraBet.Data;

    public class ApplicationDbContextInMemoryFactory
    {
        public static ApplicationDbContext InitializeContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }
    }
}
