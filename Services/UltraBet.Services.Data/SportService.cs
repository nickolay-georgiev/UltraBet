namespace UltraBet.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Configuration;
    using UltraBet.Common;
    using UltraBet.Data.Common.Repositories;
    using UltraBet.Data.Models;
    using UltraBet.Services.Mapping;
    using UltraBet.Services.Models;
    using UltraBet.Web.ViewModels;

    public class SportService : ISportService
    {
        private readonly IConfiguration configuration;
        private readonly IDeletableEntityRepository<Market> marketRepository;
        private readonly IDeletableEntityRepository<Odd> oddRepository;
        private readonly IDeletableEntityRepository<Team> teamRepository;
        private readonly IDeletableEntityRepository<Event> eventRepository;
        private readonly IDeletableEntityRepository<Sport> sportRepository;
        private readonly IDeletableEntityRepository<Match> matchRepository;
        private readonly IRepository<MarketName> marketNameRepository;
        private readonly IRepository<OddName> oddNameRepository;
        private readonly IRepository<MatchType> matchTypeRepository;
        private readonly IRepository<EventCategory> eventCategoryRepository;

        public SportService(
            IConfiguration configuration,
            IDeletableEntityRepository<Market> betRepository,
            IDeletableEntityRepository<Odd> oddRepository,
            IDeletableEntityRepository<Team> teamRepository,
            IDeletableEntityRepository<Event> eventRepository,
            IDeletableEntityRepository<Sport> sportRepository,
            IDeletableEntityRepository<Match> matchRepository,
            IRepository<MarketName> betNameRepository,
            IRepository<OddName> oddNameRepository,
            IRepository<MatchType> matchTypeRepository,
            IRepository<EventCategory> eventCategoryRepository)
        {
            this.configuration = configuration;
            this.marketRepository = betRepository;
            this.oddRepository = oddRepository;
            this.teamRepository = teamRepository;
            this.eventRepository = eventRepository;
            this.sportRepository = sportRepository;
            this.matchRepository = matchRepository;
            this.marketNameRepository = betNameRepository;
            this.oddNameRepository = oddNameRepository;
            this.matchTypeRepository = matchTypeRepository;
            this.eventCategoryRepository = eventCategoryRepository;
        }

        public async Task StoreDataAsync(XmlSportsDto data)
        {
            // To start receiving update notifications for odd entity uncomment this part of code and
            // listener.Stop(); at the end of the method
            /*
            var connectionString = this.configuration.GetConnectionString("DefaultConnection");
            var listener = new SQLBrokerService(
                connectionString, GlobalConstants.SystemName, GlobalConstants.MonitoredTableOdds, listenerType: SQLBrokerService.NotificationTypes.Update);

            listener.TableChanged += (obj, entity) =>
            {
                var updatedEntityAsXml = entity.Data;
            };

            listener.Start();
            */
            var watch = new Stopwatch();
            watch.Start();

            // 32 total
            var oddNamesWithIds = this.oddNameRepository
                .AllAsNoTracking()
                .ToList()
                .GroupBy(x => x.Name)
                .ToDictionary(x => x.Key, x => x.Select(x => x.Id).ToList()[0]);

            // 2 total
            var matchTypesWithIds = this.matchTypeRepository
                .AllAsNoTracking()
                .ToList()
                .GroupBy(x => x.Name)
                .ToDictionary(x => x.Key, x => x.Select(x => x.Id).ToList()[0]);

            // 11 total
            var eventCategoriesWithIds = this.eventCategoryRepository
               .AllAsNoTracking()
               .ToList()
               .GroupBy(x => x.Name)
               .ToDictionary(x => x.Key, x => x.Select(x => x.Id).ToList()[0]);

            var sport = this.sportRepository
                .All()
                .FirstOrDefault(x => x.Id == data.SportDto.Id);

            if (sport is null)
            {
                sport = new Sport
                {
                    Name = data.SportDto.Name,
                    Id = data.SportDto.Id,
                };

                await this.sportRepository.AddAsync(sport);
                await this.sportRepository.SaveChangesAsync();
            }

            foreach (var eventDto in data.SportDto.Events)
            {
                var currentEvent = this.eventRepository
                    .All()
                    .FirstOrDefault(x => x.Id == eventDto.Id);

                if (currentEvent is null)
                {
                    if (!eventCategoriesWithIds.ContainsKey(eventDto.CategoryId))
                    {
                        var eventCategory = new EventCategory { Name = eventDto.CategoryId };

                        await this.eventCategoryRepository.AddAsync(eventCategory);
                        await this.eventCategoryRepository.SaveChangesAsync();

                        eventCategoriesWithIds.Add(eventCategory.Name, eventCategory.Id);
                    }

                    var eventCategoryId = eventCategoriesWithIds[eventDto.CategoryId];

                    currentEvent = new Event
                    {
                        Name = eventDto.Name,
                        Id = eventDto.Id,
                        IsLive = eventDto.IsLive,
                        SportId = sport.Id,
                        EventCategoryId = eventCategoryId,
                    };

                    sport.Events.Add(currentEvent);

                    await this.eventRepository.AddAsync(currentEvent);
                }

                foreach (var matchDto in eventDto.Matches)
                {
                    var matchType = matchDto.MatchType.ToUpper();
                    if (matchType == GlobalConstants.OutRightMatchType)
                    {
                        continue;
                    }

                    var matchTypeId = matchTypesWithIds[matchType];

                    var currentMatch = this.matchRepository
                        .All()
                        .FirstOrDefault(x => x.Id == matchDto.Id);

                    if (currentMatch is null)
                    {
                        currentMatch = new Match
                        {
                            Name = matchDto.Name,
                            Id = matchDto.Id,
                            StartDate = matchDto.StartDate,
                            EventId = currentEvent.Id,
                            TypeId = matchTypeId,
                        };

                        var teams = currentMatch.Name.Split(" - ");
                        foreach (var teamName in teams)
                        {
                            var currentTeam = this.teamRepository
                                .All()
                                .FirstOrDefault(x => x.Name == teamName);

                            if (currentTeam is null)
                            {
                                currentTeam = new Team { Name = teamName };

                                await this.teamRepository.AddAsync(currentTeam);
                                await this.teamRepository.SaveChangesAsync();
                            }

                            currentMatch.Teams.Add(new MatchesTeams
                            {
                                TeamId = currentTeam.Id,
                                MatchId = currentMatch.Id,
                            });
                        }

                        currentEvent.Matches.Add(currentMatch);
                    }
                    else if (currentMatch.TypeId != matchTypeId)
                    {
                        currentMatch.TypeId = matchTypeId;
                    }

                    if (currentMatch.StartDate != matchDto.StartDate)
                    {
                        currentMatch.StartDate = matchDto.StartDate;
                    }

                    if (matchDto.Bets is not null)
                    {
                        foreach (var marketDto in matchDto.Bets)
                        {
                            var currentMarket = this.marketRepository
                                 .All()
                                 .FirstOrDefault(x => x.Id == marketDto.Id);

                            if (currentMarket is null)
                            {
                                var marketName = this.marketNameRepository
                                    .All()
                                    .FirstOrDefault(x => x.Name == marketDto.Name);

                                if (marketName is null)
                                {
                                    marketName = new MarketName { Name = marketDto.Name };

                                    await this.marketNameRepository.AddAsync(marketName);
                                    await this.marketNameRepository.SaveChangesAsync();
                                }

                                currentMarket = new Market
                                {
                                    Id = marketDto.Id,
                                    MarketNameId = marketName.Id,
                                    IsLive = marketDto.IsLive,
                                    MatchId = currentMatch.Id,
                                };

                                currentMatch.Markets.Add(currentMarket);
                            }

                            int counter = 1;
                            string previousSpecialBetValue = null;
                            foreach (var oddDto in marketDto.Odds)
                            {
                                var currentOdd = this.oddRepository
                                     .All()
                                     .FirstOrDefault(x => x.Id == oddDto.Id);

                                if (currentOdd is null)
                                {
                                    if (!oddNamesWithIds.ContainsKey(oddDto.Name))
                                    {
                                        var oddName = new OddName { Name = oddDto.Name };

                                        await this.oddNameRepository.AddAsync(oddName);
                                        await this.oddNameRepository.SaveChangesAsync();

                                        oddNamesWithIds.Add(oddDto.Name, oddName.Id);
                                    }

                                    var oddNameId = oddNamesWithIds[oddDto.Name];

                                    currentOdd = new Odd
                                    {
                                        Id = oddDto.Id,
                                        Value = oddDto.Value,
                                        SpecialBetValue = oddDto.SpecialBetValue,
                                        MarketId = currentMarket.Id,
                                        OddNameId = oddNameId,
                                    };

                                    currentMarket.Odds.Add(currentOdd);
                                }
                                else if (currentOdd.Value != oddDto.Value)
                                {
                                    currentOdd.Value = oddDto.Value;
                                }

                                if (currentOdd.SpecialBetValue is not null)
                                {
                                    if (previousSpecialBetValue is null)
                                    {
                                        previousSpecialBetValue = currentOdd.SpecialBetValue;
                                    }

                                    if (previousSpecialBetValue == currentOdd.SpecialBetValue)
                                    {
                                        currentOdd.GroupNumber = counter;
                                    }
                                    else
                                    {
                                        previousSpecialBetValue = currentOdd.SpecialBetValue;
                                        currentOdd.GroupNumber = ++counter;
                                    }
                                }
                                else
                                {
                                    currentOdd.GroupNumber = GlobalConstants.DefaultGroupNumber;
                                }
                            }
                        }
                    }
                }
            }

            await this.eventRepository.SaveChangesAsync();
            await this.sportRepository.SaveChangesAsync();

            var time = watch.Elapsed;

            // listener.Stop();
        }

        public IEnumerable<MatchInNext24HoursViewModel> GetMatchesInNextTwentyFourHours() => this.matchRepository
                 .AllAsNoTracking()
                 .Where(x => x.StartDate >= DateTime.UtcNow && x.StartDate <= DateTime.UtcNow.AddHours(24))
                 .To<MatchInNext24HoursViewModel>()
                 .ToList();

        public MatchSearchByIdViewModel GetMatchById(string id) => this.matchRepository
                 .AllAsNoTracking()
                 .To<MatchSearchByIdViewModel>()
                 .FirstOrDefault(x => x.Id == id);

        // public IEnumerable<MatchViewModel> GetMatchesInNextTwentyFourHours()
        // {
        //    var allowedBetNames = new List<string> { "Match Winner", "Map Advantage", "Total Maps Played" };

        // return this.matchRepository
        //        .AllAsNoTracking()
        //        .Where(x => x.StartDate >= DateTime.UtcNow && x.StartDate <= DateTime.UtcNow.AddHours(24))
        //        .Select(x => new MatchViewModel
        //        {
        //            Id = x.Id,
        //            Name = x.Name,
        //            StartDate = x.StartDate,
        //            Bets = x.Bets
        //                   .Where(b => allowedBetNames.Contains(b.BetName.Name))
        //                   .Select(b => new BetViewModel
        //                   {
        //                       Id = b.Id,
        //                       Name = b.BetName.Name,
        //                       IsLive = b.IsLive,
        //                       Odds = b.Odds
        //                              .Where(o => o.GroupNumber == 1)
        //                              .Select(o => new OddViewModel
        //                              {
        //                                  Id = o.Id,
        //                                  Name = o.Name,
        //                                  Value = o.Value,
        //                                  SpecialBetValue = o.SpecialBetValue,
        //                              }).ToList(),
        //                   }).ToList(),
        //        }).ToList();
        // }
    }
}
