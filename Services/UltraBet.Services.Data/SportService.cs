namespace UltraBet.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Caching.Memory;
    using UltraBet.Common;
    using UltraBet.Data.Common.Repositories;
    using UltraBet.Data.Models;
    using UltraBet.Services.Mapping;
    using UltraBet.Services.Models;
    using UltraBet.Web.ViewModels;

    public class SportService : ISportService
    {
        private readonly IDeletableEntityRepository<Bet> betRepository;
        private readonly IDeletableEntityRepository<Odd> oddRepository;
        private readonly IDeletableEntityRepository<Team> teamRepository;
        private readonly IDeletableEntityRepository<Event> eventRepository;
        private readonly IDeletableEntityRepository<Sport> sportRepository;
        private readonly IDeletableEntityRepository<Match> matchRepository;
        private readonly IRepository<BetName> betNameRepository;
        private readonly IRepository<OddName> oddNameRepository;
        private readonly IRepository<MatchType> matchTypeRepository;

        public SportService(
            IDeletableEntityRepository<Bet> betRepository,
            IDeletableEntityRepository<Odd> oddRepository,
            IDeletableEntityRepository<Team> teamRepository,
            IDeletableEntityRepository<Event> eventRepository,
            IDeletableEntityRepository<Sport> sportRepository,
            IDeletableEntityRepository<Match> matchRepository,
            IRepository<BetName> betNameRepository,
            IRepository<OddName> oddNameRepository,
            IRepository<MatchType> matchTypeRepository)
        {
            this.betRepository = betRepository;
            this.oddRepository = oddRepository;
            this.teamRepository = teamRepository;
            this.eventRepository = eventRepository;
            this.sportRepository = sportRepository;
            this.matchRepository = matchRepository;
            this.betNameRepository = betNameRepository;
            this.oddNameRepository = oddNameRepository;
            this.matchTypeRepository = matchTypeRepository;
        }

        public async Task StoreDataAsync(XmlSportsDto data)
        {
            var oddNamesWithIds = this.oddNameRepository
                .AllAsNoTracking()
                .ToList()
                .GroupBy(x => x.Name)
                .ToDictionary(x => x.Key, x => x.Select(x => x.Id).ToList()[0]);

            var matchTypesWithIds = this.matchTypeRepository
                .AllAsNoTracking()
                .ToList()
                .GroupBy(x => x.Name)
                .ToDictionary(x => x.Key, x => x.Select(x => x.Id).ToList()[0]);

            var watch = new Stopwatch();
            watch.Start();

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
                                        BetId = currentBet.Id,
                                        OddNameId = oddNameId,
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
            var matchViewModel = this.matchRepository
                                  .AllAsNoTracking()
                                  .Where(x => x.StartDate >= DateTime.UtcNow &&
                                              x.StartDate <= DateTime.UtcNow.AddHours(24))
                                  .To<MatchViewModel>()
                                  .ToList();

            return matchViewModel;
        }

        public MatchSearchByIdViewModel GetMatchById(string id)
        {
            var matchViewModel = this.matchRepository
                                   .AllAsNoTracking()
                                   .To<MatchSearchByIdViewModel>()
                                   .FirstOrDefault(x => x.Id == id);

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
