namespace UltraBet.Services.Data.Tests.ServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Configuration;
    using Moq;
    using NUnit.Framework;
    using UltraBet.Common;
    using UltraBet.Data.Common.Repositories;
    using UltraBet.Data.Models;
    using UltraBet.Data.Repositories;
    using UltraBet.Services.Data.Tests.ServiceTests.Helpers;

    using Match = UltraBet.Data.Models.Match;

    [TestFixture]
    public class SportServiceTests
    {
        [SetUp]
        public void Setup()
        {
            AutoMapperInitializer.Init();
        }

        [Test]
        public void GetMatchesInNext24HoursShouldReturnOnlyMatchesInNext24Hours()
        {
            var testMatches = new TestDataHelper().GetTestMatchesModels().AsQueryable();

            var mockMatchRepository = new Mock<IDeletableEntityRepository<Match>>();
            mockMatchRepository.Setup(x => x.AllAsNoTracking()).Returns(testMatches);

            var sportService = new SportService(
               new Mock<IConfiguration>().Object,
               new Mock<IDeletableEntityRepository<Market>>().Object,
               new Mock<IDeletableEntityRepository<Odd>>().Object,
               new Mock<IDeletableEntityRepository<Team>>().Object,
               new Mock<IDeletableEntityRepository<Event>>().Object,
               new Mock<IDeletableEntityRepository<Sport>>().Object,
               mockMatchRepository.Object,
               new Mock<IRepository<MarketName>>().Object,
               new Mock<IRepository<OddName>>().Object,
               new Mock<IRepository<MatchType>>().Object,
               new Mock<IRepository<EventCategory>>().Object);

            var matches = sportService.GetMatchesInNextTwentyFourHours();

            var matchesStartDates = matches
               .Select(x => x.StartDate)
               .ToList();

            Assert.That(matchesStartDates, Is.All.GreaterThan(DateTime.Now).Or.All.LessThan(DateTime.Now.AddHours(24)));
        }

        [Test]
        public void GetMatchesInNext24HoursReturnCorrectPreviewMarkets()
        {
            var testMatches = new TestDataHelper().GetTestMatchesModels().AsQueryable();

            var mockMatchRepository = new Mock<IDeletableEntityRepository<Match>>();
            mockMatchRepository.Setup(x => x.AllAsNoTracking()).Returns(testMatches);

            var sportService = new SportService(
               new Mock<IConfiguration>().Object,
               new Mock<IDeletableEntityRepository<Market>>().Object,
               new Mock<IDeletableEntityRepository<Odd>>().Object,
               new Mock<IDeletableEntityRepository<Team>>().Object,
               new Mock<IDeletableEntityRepository<Event>>().Object,
               new Mock<IDeletableEntityRepository<Sport>>().Object,
               mockMatchRepository.Object,
               new Mock<IRepository<MarketName>>().Object,
               new Mock<IRepository<OddName>>().Object,
               new Mock<IRepository<MatchType>>().Object,
               new Mock<IRepository<EventCategory>>().Object);

            var matches = sportService.GetMatchesInNextTwentyFourHours();
            var marketTypes = matches.SelectMany(x => x.Markets.Select(x => x.Name)).ToList();

            var allowedPreviewMarkets = new List<string>
            {
                GlobalConstants.MarketMatchWinner,
                GlobalConstants.MarketMapAdvantage,
                GlobalConstants.MarketTotalMapsPlayed,
            };

            Assert.That(marketTypes, Is.EquivalentTo(allowedPreviewMarkets));
        }

        [Test]
        public void GetMatchBuyIdWorksCorrectly()
        {
            var testMatches = new TestDataHelper().GetTestMatchesModels().AsQueryable();

            var mockMatchRepository = new Mock<IDeletableEntityRepository<Match>>();
            mockMatchRepository.Setup(x => x.AllAsNoTracking()).Returns(testMatches);

            var sportService = new SportService(
               new Mock<IConfiguration>().Object,
               new Mock<IDeletableEntityRepository<Market>>().Object,
               new Mock<IDeletableEntityRepository<Odd>>().Object,
               new Mock<IDeletableEntityRepository<Team>>().Object,
               new Mock<IDeletableEntityRepository<Event>>().Object,
               new Mock<IDeletableEntityRepository<Sport>>().Object,
               mockMatchRepository.Object,
               new Mock<IRepository<MarketName>>().Object,
               new Mock<IRepository<OddName>>().Object,
               new Mock<IRepository<MatchType>>().Object,
               new Mock<IRepository<EventCategory>>().Object);

            var match = sportService.GetMatchById("1");

            Assert.That(match.Id == "1");
            Assert.That(match.Name == "Navi - Nigma");
            Assert.That(match.Teams[0] == "Navi");
            Assert.That(match.Teams[1] == "Nigma");
            Assert.That(match.Markets.Count == 1);
        }

        [Test]
        public async Task GetMatchesInNext24HoursReturnCorrectOdds()
        {
            //var listener = new SQLBrokerService("test", "test", "test");
            //listener.Start();

            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var mockMatchTypeRepository = new EfRepository<MatchType>(context);

            var liveType = new MatchType
            {
                Name = GlobalConstants.LiveMatchType,
            };

            var prematchType = new MatchType
            {
                Name = GlobalConstants.PrematchMatchType,
            };

            await mockMatchTypeRepository.AddAsync(liveType);
            await mockMatchTypeRepository.AddAsync(prematchType);
            await mockMatchTypeRepository.SaveChangesAsync();

            var xmlSportsDto = new TestDataHelper().GetXmlSportsDto();

            var sportService = new SportService(
                  new Mock<IConfiguration>().Object,
                  new EfDeletableEntityRepository<Market>(context),
                  new EfDeletableEntityRepository<Odd>(context),
                  new EfDeletableEntityRepository<Team>(context),
                  new EfDeletableEntityRepository<Event>(context),
                  new EfDeletableEntityRepository<Sport>(context),
                  new EfDeletableEntityRepository<Match>(context),
                  new EfRepository<MarketName>(context),
                  new EfRepository<OddName>(context),
                  mockMatchTypeRepository,
                  new EfRepository<EventCategory>(context));

            await sportService.StoreDataAsync(xmlSportsDto);

            var matches = sportService.GetMatchesInNextTwentyFourHours();

            var oddsSpecialBetValues = matches
                .Where(x => x.Id == "1")
                .SelectMany(x => x.Markets
                .SelectMany(x => x.Odds))
                .Select(x => x.SpecialBetValue)
                .ToList();

            var validMatchesCount = 3;
            var valueOfFirstGroupOddsWithSpecialBetValue = "7";

            Assert.That(matches.Count() == validMatchesCount);
            Assert.That(oddsSpecialBetValues, Is.All.EqualTo(valueOfFirstGroupOddsWithSpecialBetValue));
        }

        [Test]
        public async Task ChangeOfMachTypeWorksCorrectly()
        {
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var testSportRepository = new EfDeletableEntityRepository<Sport>(context);

            var testSport = new TestDataHelper().GetTestSportModel();
            var testMatches = new TestDataHelper().GetTestMatchesModels();

            foreach (var match in testMatches.ToList())
            {
                testSport.Events.FirstOrDefault().Matches.Add(match);
            }

            await testSportRepository.AddAsync(testSport);
            await testSportRepository.SaveChangesAsync();

            var insertedMatches = testSportRepository
                .All()
                .Select(x => x.Events.SelectMany(x => x.Matches))
                .SelectMany(x => x)
                .ToList();

            foreach (var match in insertedMatches)
            {
                match.Type = new MatchType { Name = GlobalConstants.PrematchMatchType };
            }

            testSportRepository.Update(testSport);
            await testSportRepository.SaveChangesAsync();

            var updatedMatchTypes = testSportRepository
                .AllAsNoTracking()
                .Select(x => x.Events.SelectMany(x => x.Matches.Select(x => x.Type.Name)))
                .SelectMany(x => x)
                .ToList();

            Assert.That(updatedMatchTypes, Is.All.EqualTo(GlobalConstants.PrematchMatchType));
        }

        [Test]
        public async Task ChangeOfMachStartDateWorksCorrectly()
        {
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var testSportRepository = new EfDeletableEntityRepository<Sport>(context);

            var testSport = new TestDataHelper().GetTestSportModel();
            var testMatches = new TestDataHelper().GetTestMatchesModels();

            foreach (var match in testMatches)
            {
                testSport.Events.FirstOrDefault().Matches.Add(match);
            }

            await testSportRepository.AddAsync(testSport);
            await testSportRepository.SaveChangesAsync();

            var insertedMatches = testSportRepository
                .All()
                .Select(x => x.Events.SelectMany(x => x.Matches))
                .SelectMany(x => x)
                .ToList();

            var date = new DateTime(2021, 06, 11);
            foreach (var match in insertedMatches)
            {
                match.StartDate = date;
            }

            testSportRepository.Update(testSport);
            await testSportRepository.SaveChangesAsync();

            var updatedMatchDates = testSportRepository
                .AllAsNoTracking()
                .Select(x => x.Events.SelectMany(x => x.Matches.Select(x => x.StartDate)))
                .SelectMany(x => x)
                .ToList();

            Assert.That(updatedMatchDates, Is.All.EqualTo(date));
        }

        [Test]
        public async Task ChangeOfMarketValueWorksCorrectly()
        {
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var testSportRepository = new EfDeletableEntityRepository<Sport>(context);

            var testSport = new TestDataHelper().GetTestSportModel();
            var testMatches = new TestDataHelper().GetTestMatchesModels();

            foreach (var match in testMatches)
            {
                testSport.Events.FirstOrDefault().Matches.Add(match);
            }

            await testSportRepository.AddAsync(testSport);
            await testSportRepository.SaveChangesAsync();

            var insertedMatches = testSportRepository
              .All()
              .Select(x => x.Events.SelectMany(x => x.Matches))
              .SelectMany(x => x)
              .ToList();

            foreach (var match in insertedMatches)
            {
                foreach (var bet in match.Markets)
                {
                    foreach (var odd in bet.Odds)
                    {
                        odd.Value = 7;
                    }
                }
            }

            testSportRepository.Update(testSport);
            await testSportRepository.SaveChangesAsync();

            var updatedOddValuesDates = testSportRepository
                .AllAsNoTracking()
                .Select(x => x.Events
                .SelectMany(x => x.Matches
                .SelectMany(x => x.Markets
                .SelectMany(x => x.Odds
                .Select(x => x.Value)))))
                .SelectMany(x => x)
                .ToList();

            Assert.That(updatedOddValuesDates, Is.All.EqualTo(7));
        }
    }
}
