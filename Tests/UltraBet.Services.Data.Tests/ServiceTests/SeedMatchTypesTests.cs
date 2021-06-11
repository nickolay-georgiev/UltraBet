namespace UltraBet.Services.Data.Tests.ServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Moq;
    using NUnit.Framework;
    using UltraBet.Data.Seeding;

    [TestFixture]
    public class SeedMatchTypesTests
    {
        [Test]
        public async Task SeedDBFromApiWorksCorrectly()
        {
            //var apiConnection = new Mock<MatchTypesSeeder>();
            //apiConnection
            //    .Setup(x => x.GetCurrentData(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            //var dbSeeder = new SeedDBFromApi(apiConnection.Object);

            //await dbSeeder.Work();

            //apiConnection.Verify(x => x.GetCurrentData(GlobalConstants.StockFunction, GlobalConstants.StockTicker, GlobalConstants.StockInterval), Times.Once);
        }
    }
}
