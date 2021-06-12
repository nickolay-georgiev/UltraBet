namespace UltraBet.Services.Data.Tests.ServiceTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Moq;
    using NUnit.Framework;
    using UltraBet.Data.Common.Repositories;
    using UltraBet.Data.Models;
    using UltraBet.Data.Repositories;
    using UltraBet.Services.Data.Tests.ServiceTests.Helpers;

    using Match = UltraBet.Data.Models.Match;

    [TestFixture]
    public class SportServiceTests
    {
        private ISportService sportService;
        private IQueryable<Match> testMatches;
        private Mock<IDeletableEntityRepository<Market>> mockMarketRepository;
        private Mock<IDeletableEntityRepository<Odd>> mockOddRepository;
        private Mock<IDeletableEntityRepository<Team>> mockTeamRepository;
        private Mock<IDeletableEntityRepository<Event>> mockEventRepository;
        private Mock<IDeletableEntityRepository<Sport>> mockSportRepository;
        private Mock<IDeletableEntityRepository<Match>> mockMatchRepository;
        private Mock<IRepository<MarketName>> mockMarketNameRepository;
        private Mock<IRepository<OddName>> mockOddNameRepository;
        private Mock<IRepository<MatchType>> mockMatchTypeRepository;
        private Mock<IRepository<EventCategory>> mockEventCategoryRepository;

        [SetUp]
        public void Setup()
        {
            AutoMapperInitializer.Init();
            this.mockMarketRepository = new Mock<IDeletableEntityRepository<Market>>();
            this.mockOddRepository = new Mock<IDeletableEntityRepository<Odd>>();
            this.mockTeamRepository = new Mock<IDeletableEntityRepository<Team>>();
            this.mockEventRepository = new Mock<IDeletableEntityRepository<Event>>();
            this.mockSportRepository = new Mock<IDeletableEntityRepository<Sport>>();
            this.mockMatchRepository = new Mock<IDeletableEntityRepository<Match>>();
            this.mockMarketNameRepository = new Mock<IRepository<MarketName>>();
            this.mockOddNameRepository = new Mock<IRepository<OddName>>();
            this.mockMatchTypeRepository = new Mock<IRepository<MatchType>>();
            this.mockEventCategoryRepository = new Mock<IRepository<EventCategory>>();

            this.testMatches = new TestDataHelper().GetTestMatchesModels().AsQueryable();
            this.mockMatchRepository.Setup(x => x.AllAsNoTracking()).Returns(this.testMatches);

            this.sportService = new SportService(
               this.mockMarketRepository.Object,
               this.mockOddRepository.Object,
               this.mockTeamRepository.Object,
               this.mockEventRepository.Object,
               this.mockSportRepository.Object,
               this.mockMatchRepository.Object,
               this.mockMarketNameRepository.Object,
               this.mockOddNameRepository.Object,
               this.mockMatchTypeRepository.Object,
               this.mockEventCategoryRepository.Object);
        }

        [Test]
        public void GetMatchesInNext24HoursShouldReturnOnlyMatchesInNext24Hours()
        {
            var matches = this.sportService.GetMatchesInNextTwentyFourHours();

            var matchesStartDates = matches
               .Select(x => x.StartDate)
               .ToList();

            Assert.That(matchesStartDates, Is.All.GreaterThan(DateTime.Now).Or.All.LessThan(DateTime.Now.AddHours(24)));
        }

        [Test]
        public void GetMatchesInNext24HoursReturnCorrectPreviewMarkets()
        {
            var matches = this.sportService.GetMatchesInNextTwentyFourHours();
            var betTypes = matches.SelectMany(x => x.Markets.Select(x => x.Name)).ToList();

            var allowedPreviewMarkets = new List<string>
            { "Total Maps Played", "Match Winner", "Map Advantage" };

            Assert.That(betTypes, Is.EquivalentTo(allowedPreviewMarkets));
        }

        [Test]
        public void GetMatchBuyIdWorksCorrectly()
        {
            var match = this.sportService.GetMatchById("1");

            Assert.That(match.Id == "1");
            Assert.That(match.Name == "Navi - Nigma");
            Assert.That(match.Teams[0] == "Navi");
            Assert.That(match.Teams[1] == "Nigma");
            Assert.That(match.Markets.Count == 1);
        }

        [Test]
        public async Task InsertSportEntityInDbWorksCorrectly()
        {
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var testSportRepository = new EfDeletableEntityRepository<Sport>(context);

            var testSport = new TestDataHelper().GetTestSportModel();

            foreach (var match in this.testMatches.ToList())
            {
                testSport.Events.FirstOrDefault().Matches.Add(match);
            }

            await testSportRepository.AddAsync(testSport);
            await testSportRepository.SaveChangesAsync();

            var insertedSportEntity = testSportRepository.All().FirstOrDefault();

            Assert.AreEqual(insertedSportEntity, testSport);
        }

        [Test]
        public async Task ChangeOfMachTypeWorksCorrectly()
        {
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var testSportRepository = new EfDeletableEntityRepository<Sport>(context);

            var testSport = new TestDataHelper().GetTestSportModel();

            foreach (var match in this.testMatches.ToList())
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
                match.Type = new MatchType { Name = "Prematch" };
            }

            testSportRepository.Update(testSport);
            await testSportRepository.SaveChangesAsync();

            var updatedMatchTypes = testSportRepository
                .AllAsNoTracking()
                .Select(x => x.Events.SelectMany(x => x.Matches.Select(x => x.Type.Name)))
                .SelectMany(x => x)
                .ToList();

            Assert.That(updatedMatchTypes, Is.All.EqualTo("Prematch"));
        }

        [Test]
        public async Task ChangeOfMachStartDateWorksCorrectly()
        {
            var context = ApplicationDbContextInMemoryFactory.InitializeContext();
            var testSportRepository = new EfDeletableEntityRepository<Sport>(context);

            var testSport = new TestDataHelper().GetTestSportModel();

            foreach (var match in this.testMatches.ToList())
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

            foreach (var match in this.testMatches.ToList())
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
