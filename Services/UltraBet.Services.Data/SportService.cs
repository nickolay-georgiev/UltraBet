namespace UltraBet.Services.Data
{
    using Microsoft.Extensions.Caching.Memory;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using UltraBet.Common;
    using UltraBet.Data.Common.Repositories;
    using UltraBet.Data.Models;
    using UltraBet.Services.Mapping;
    using UltraBet.Web.ViewModels;

    public class SportService : ISportService
    {
        private readonly IMemoryCache memoryCache;
        private readonly ISportDataService sportDataService;
        private readonly IDeletableEntityRepository<Bet> betRepository;
        private readonly IDeletableEntityRepository<Odd> oddRepository;
        private readonly IDeletableEntityRepository<Team> teamRepository;
        private readonly IDeletableEntityRepository<Event> eventRepository;
        private readonly IDeletableEntityRepository<Sport> sportRepository;
        private readonly IDeletableEntityRepository<Match> matchRepository;
        private readonly IRepository<BetName> betNameRepository;
        private readonly IRepository<MatchType> matchTypeRepository;

        public SportService(
            IMemoryCache memoryCache,
            ISportDataService sportDataService,
            IDeletableEntityRepository<Bet> betRepository,
            IDeletableEntityRepository<Odd> oddRepository,
            IDeletableEntityRepository<Team> teamRepository,
            IDeletableEntityRepository<Event> eventRepository,
            IDeletableEntityRepository<Sport> sportRepository,
            IDeletableEntityRepository<Match> matchRepository,
            IRepository<BetName> betNameRepository,
            IRepository<MatchType> matchTypeRepository)
        {
            this.memoryCache = memoryCache;
            this.sportDataService = sportDataService;
            this.betRepository = betRepository;
            this.oddRepository = oddRepository;
            this.teamRepository = teamRepository;
            this.eventRepository = eventRepository;
            this.sportRepository = sportRepository;
            this.matchRepository = matchRepository;
            this.betNameRepository = betNameRepository;
            this.matchTypeRepository = matchTypeRepository;
        }

        public async Task StoreDataAsync()
        {
            var watch = new Stopwatch();
            watch.Start();

            var data = await this.sportDataService.GetSportDataAsync();

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
                    currentEvent = new Event
                    {
                        Name = eventDto.Name,
                        Id = eventDto.Id,
                        IsLive = eventDto.IsLive,
                        CategoryId = eventDto.CategoryId,
                        SportId = sport.Id,
                    };

                    sport.Events.Add(currentEvent);
                }

                foreach (var matchDto in eventDto.Matches)
                {
                    if (matchDto.MatchType.ToUpper() == GlobalConstants.OutRightMatchType)
                    {
                        continue;
                    }

                    var matchType = this.matchTypeRepository
                        .All()
                        .FirstOrDefault(x => x.Name == matchDto.MatchType.ToUpper());

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
                            TypeId = matchType.Id,
                        };

                        currentEvent.Matches.Add(currentMatch);
                    }
                    else if (currentMatch.TypeId != matchType.Id)
                    {
                        currentMatch.TypeId = matchType.Id;
                    }

                    if (currentMatch.StartDate != matchDto.StartDate)
                    {
                        currentMatch.StartDate = matchDto.StartDate;
                    }

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

                        var te = this.matchRepository
                         .All()
                         .SelectMany(x => x.Teams)
                         .Where(x => x.MatchId == matchDto.Id)
                         .ToList();

                        if (te.All(x => x.TeamId != currentTeam.Id && x.MatchId != currentMatch.Id))
                        {
                            currentMatch.Teams.Add(new MatchesTeams { TeamId = currentTeam.Id });
                        }
                    }

                    if (matchDto.Bets is not null)
                    {
                        foreach (var betDto in matchDto.Bets)
                        {
                            var betName = this.betNameRepository
                                .All()
                                .FirstOrDefault(x => x.Name == betDto.Name);

                            if (betName is null)
                            {
                                betName = new BetName { Name = betDto.Name };

                                await this.betNameRepository.AddAsync(betName);
                                await this.betNameRepository.SaveChangesAsync();
                            }

                            var currentBet = this.betRepository
                                 .All()
                                 .FirstOrDefault(x => x.Id == betDto.Id);

                            if (currentBet is null)
                            {
                                currentBet = new Bet
                                {
                                    Id = betDto.Id,
                                    BetNameId = betName.Id,
                                    IsLive = betDto.IsLive,
                                    MatchId = currentMatch.Id,
                                };

                                currentMatch.Bets.Add(currentBet);
                            }

                            int counter = 1;
                            string previousSpecialBetValue = null;
                            foreach (var oddDto in betDto.Odds)
                            {
                                var currentOdd = this.oddRepository
                                     .All()
                                     .FirstOrDefault(x => x.Id == oddDto.Id);

                                if (currentOdd is null)
                                {
                                    currentOdd = new Odd
                                    {
                                        Name = oddDto.Name,
                                        Id = oddDto.Id,
                                        Value = oddDto.Value,
                                        SpecialBetValue = oddDto.SpecialBetValue,
                                        BetId = currentBet.Id,
                                    };

                                    currentBet.Odds.Add(currentOdd);
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

            this.sportRepository.Update(sport);
            await this.sportRepository.SaveChangesAsync();

            // ~25sek
            var time = watch.Elapsed;
        }

        public IEnumerable<MatchViewModel> GetMatchesInNextTwentyFourHours()
        {
            IEnumerable<MatchViewModel> matchViewModel;

            if (!this.memoryCache.TryGetValue<IEnumerable<MatchViewModel>>(
                nameof(this.GetMatchesInNextTwentyFourHours), out matchViewModel))
            {
                matchViewModel = this.matchRepository
                                     .AllAsNoTracking()
                                     .Where(x => x.StartDate >= DateTime.UtcNow &&
                                                 x.StartDate <= DateTime.UtcNow.AddHours(24))
                                     .To<MatchViewModel>()
                                     .ToList();

                this.memoryCache.Set(
                    nameof(this.GetMatchesInNextTwentyFourHours), matchViewModel, TimeSpan.FromSeconds(60));
            }

            return matchViewModel;
        }

        public MatchSearchByIdViewModel GetMatchById(string id)
        {
            MatchSearchByIdViewModel matchViewModel;

            if (!this.memoryCache.TryGetValue<MatchSearchByIdViewModel>(
                nameof(this.GetMatchById), out matchViewModel))
            {
                matchViewModel = this.matchRepository
                                     .AllAsNoTracking()
                                     .To<MatchSearchByIdViewModel>()
                                     .FirstOrDefault(x => x.Id == id);

                this.memoryCache.Set(
                    nameof(this.GetMatchById), matchViewModel, TimeSpan.FromSeconds(60));
            }

            return matchViewModel;
        }

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
