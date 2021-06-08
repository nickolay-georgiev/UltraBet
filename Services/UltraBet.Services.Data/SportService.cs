namespace UltraBet.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using UltraBet.Common;
    using UltraBet.Data.Common.Repositories;
    using UltraBet.Data.Models;
    using UltraBet.Data.Models.Enums;
    using MatchType = UltraBet.Data.Models.MatchType;

    public class SportService : ISportService
    {
        private readonly ISportDataService sportDataService;
        private readonly IDeletableEntityRepository<Sport> sportRepository;
        private readonly IDeletableEntityRepository<Match> matchRepository;
        private readonly IRepository<MatchType> typeRepository;

        public SportService(
            ISportDataService sportDataService,
            IDeletableEntityRepository<Sport> sportRepository,
            IDeletableEntityRepository<Match> matchRepository,
            IRepository<MatchType> matchTypeRepository)
        {
            this.sportDataService = sportDataService;
            this.sportRepository = sportRepository;
            this.matchRepository = matchRepository;
            this.typeRepository = matchTypeRepository;
        }

        public async Task StoreDataAsync()
        {
            var data = await this.sportDataService.GetSportDataAsync();

            var sport = new Sport
            {
                Name = data.SportDto.Name,
                Id = data.SportDto.Id,
            };

            foreach (var eventDto in data.SportDto.Events)
            {
                var currentEvent = new Event
                {
                    Name = eventDto.Name,
                    Id = eventDto.Id,
                    IsLive = eventDto.IsLive,
                    CategoryId = eventDto.CategoryId,
                    SportId = sport.Id,
                };

                foreach (var matchDto in eventDto.Matches)
                {
                    if (matchDto.MatchType == GlobalConstants.OutrightMatchType)
                    {
                        continue;
                    }

                    var currentMatch = new Match
                    {
                        Name = matchDto.Name,
                        Id = matchDto.Id,
                        StartDate = matchDto.StartDate,
                        EventId = currentEvent.Id,
                    };

                    var type = this.typeRepository
                        .All()
                        .FirstOrDefault(x => x.Name == matchDto.MatchType);

                    currentMatch.TypeId = type.Id;

                    if (type.Matches.All(x => x.Id != currentMatch.Id))
                    {
                        type.Matches.Add(currentMatch);

                        this.typeRepository.Update(type);
                    }

                    if (matchDto.Bets is not null)
                    {
                        foreach (var betDto in matchDto.Bets)
                        {
                            var currentBet = new Bet
                            {
                                Name = betDto.Name,
                                Id = betDto.Id,
                                IsLive = betDto.IsLive,
                                MatchId = currentMatch.Id,
                            };

                            foreach (var oddDto in betDto.Odds)
                            {
                                var currentOdd = new Odd
                                {
                                    Name = oddDto.Name,
                                    Id = oddDto.Id,
                                    Value = oddDto.Value,
                                    SpecialBetValue = oddDto.SpecialBetValue,
                                    BetId = currentBet.Id,
                                };

                                currentBet.Odds.Add(currentOdd);
                            }

                            currentMatch.Bets.Add(currentBet);
                        }
                    }

                    currentEvent.Matches.Add(currentMatch);
                }

                sport.Events.Add(currentEvent);
            }

            await this.sportRepository.AddAsync(sport);
            await this.sportRepository.SaveChangesAsync();
            await this.typeRepository.SaveChangesAsync();
        }
    }
}
