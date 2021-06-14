namespace UltraBet.Services.Data.Tests.ServiceTests.Helpers
{
    using System;
    using System.Collections.Generic;

    using UltraBet.Data.Models;
    using UltraBet.Services.Models;

    public class TestDataHelper
    {
        public List<Match> GetTestMatchesModels()
        {
            return new List<Match>
            {
                new Match
                {
                    Id = "1",
                    Name = "Navi - Nigma",
                    Teams = new List<MatchesTeams>
                    {
                        new MatchesTeams
                        {
                            TeamId = 1,
                            Team = new Team { Name = "Navi", Id = 1 },
                        },
                        new MatchesTeams
                        {
                            TeamId = 2,
                            Team = new Team { Name = "Nigma", Id = 2 },
                        },
                    },
                    StartDate = DateTime.UtcNow.AddSeconds(1),
                    TypeId = 1,
                    Type = new MatchType { Name = "Live" },
                    EventId = "1",
                    Event = new Event {Id = "1", Name = "test" },
                    Markets = new List<Market>
                    {
                        new Market
                        {
                            Id = "1",
                            IsLive = true,
                            MatchId = "1",
                            MarketNameId = 1,
                            MarketName = new MarketName { Name = "Match Winner" },
                            Odds = new List<Odd>
                            {
                                new Odd
                                {
                                    Id = "1",
                                    Value = 1,
                                    SpecialBetValue = "7",
                                    OddName = new OddName { Name = "2:0"},
                                },
                                new Odd
                                {
                                    Id = "2",
                                    Value = 2,
                                    SpecialBetValue = "7",
                                    OddName = new OddName { Name = "2:2"},
                                },
                                new Odd
                                {
                                    Id = "13",
                                    Value = 2,
                                    SpecialBetValue = "10",
                                    OddName = new OddName { Name = "5:2"},
                                },
                                new Odd
                                {
                                    Id = "14",
                                    Value = 2,
                                    SpecialBetValue = "10",
                                    OddName = new OddName { Name = "6:2"},
                                },
                            },
                        },
                    },
                },
                new Match
                {
                    Id = "2",
                    Name = "Alliance - Brame",
                    Teams = new List<MatchesTeams>
                    {
                        new MatchesTeams
                        {
                            TeamId = 3,
                            Team = new Team { Name = "Alliance", Id = 3 },
                        },
                        new MatchesTeams
                        {
                            TeamId = 4,
                            Team = new Team { Name = "Brame", Id = 4 },
                        },
                    },
                    StartDate = DateTime.UtcNow.AddHours(23).AddMinutes(59),
                    TypeId = 1,
                    Type = new MatchType { Name = "Live" },
                    EventId = "1",
                    Event = new Event {Id = "1", Name = "test" },
                    Markets = new List<Market>
                    {
                        new Market
                        {
                            Id = "2",
                            IsLive = true,
                            MatchId = "2",
                            MarketNameId = 2,
                            MarketName = new MarketName { Name = "Total Maps Played" },
                            Odds = new List<Odd>
                            {
                                new Odd
                                {
                                    Id = "3",
                                    Value = 1,
                                    SpecialBetValue = null,
                                    OddName = new OddName {Name = "1:0"},
                                },
                                new Odd
                                {
                                    Id = "4",
                                    Value = 2,
                                    SpecialBetValue = null,
                                    OddName = new OddName { Name = "2:1"},
                                },
                            },
                        },
                    },
                },
                new Match
                {
                    Id = "3",
                    Name = "Hellbear Smashers - Hippomaniacs",
                    Teams = new List<MatchesTeams>
                    {
                        new MatchesTeams
                        {
                            TeamId = 5,
                            Team = new Team { Name = "Hellbear Smashers", Id = 5 },
                        },
                        new MatchesTeams
                        {
                            TeamId = 6,
                            Team = new Team { Name = "Hippomaniacs", Id = 6 },
                        },
                    },
                    StartDate = DateTime.UtcNow.AddHours(15),
                    TypeId = 1,
                    Type = new MatchType { Name = "Live" },
                    EventId = "1",
                    Event = new Event {Id = "1", Name = "test" },
                    Markets = new List<Market>
                    {
                        new Market
                        {
                            Id = "3",
                            IsLive = true,
                            MatchId = "3",
                            MarketNameId = 3,
                            MarketName = new MarketName { Name = "Map Advantage" },
                            Odds = new List<Odd>
                            {
                                new Odd
                                {
                                    Id = "5",
                                    Value = 1,
                                    SpecialBetValue = null,
                                    OddName = new OddName {Name = "2:3"},
                                },
                                new Odd
                                {
                                    Id = "6",
                                    Value = 2,
                                    SpecialBetValue = null,
                                    OddName = new OddName { Name = "2:4"},
                                },
                            },
                        },
                    },
                },
                new Match
                {
                    Id = "4",
                    Name = "Chicken Fighters - Nigma",
                    Teams = new List<MatchesTeams>
                    {
                        new MatchesTeams
                        {
                            TeamId = 7,
                            Team = new Team { Name = "Chicken Fighters", Id = 7 },
                        },
                        new MatchesTeams
                        {
                            TeamId = 8,
                            Team = new Team { Name = "eSuba", Id = 8 },
                        },
                    },
                    StartDate = DateTime.UtcNow.AddDays(3),
                    TypeId = 1,
                    Type = new MatchType { Name = "Live" },
                    EventId = "1",
                    Event = new Event {Id = "1", Name = "test" },
                    Markets = new List<Market>
                    {
                        new Market
                        {
                            Id = "4",
                            IsLive = true,
                            MatchId = "4",
                            MarketNameId = 4,
                            MarketName = new MarketName { Name = "Invalid" },
                            Odds = new List<Odd>
                            {
                                new Odd
                                {
                                    Id = "7",
                                    Value = 1,
                                    SpecialBetValue = null,
                                    OddName = new OddName {Name = "2:5"},
                                },
                                new Odd
                                {
                                    Id = "8",
                                    Value = 2,
                                    SpecialBetValue = null,
                                    OddName = new OddName { Name = "2:6"},
                                },
                            },
                        },
                    },
                },
            };
        }

        public Sport GetTestSportModel()
        {
            return new Sport
            {
                Name = "Test Sport",
                Id = "1",
                Events = new List<Event>
                {
                    new Event
                    {
                        Id = "1",
                        Name = "Dota 2",
                        IsLive = false,
                        EventCategory = new EventCategory { Name = "123" },
                    },
                },
            };
        }

        public XmlSportsDto GetXmlSportsDto()
        {
            return new XmlSportsDto
            {
                SportDto = new SportDto
                {
                    Id = "1",
                    Name = "First",
                    Events = new EventDto[]
                    {
                        new EventDto
                        {
                            Id = "1",
                            Name = "1",
                            IsLive = false,
                            CategoryId = "1",
                            Matches = new MatchDto[]
                            {
                                 new MatchDto
                            {
                                 Id = "1",
                                 Name = "Navi - Nigma",
                                 StartDate = DateTime.UtcNow.AddMinutes(1),
                                 MatchType = "Live",
                                 Bets = new BetDto[]
                                 {
                                     new BetDto
                                     {
                                         Id = "1",
                                         IsLive = true,
                                         Name = "Match Winner",
                                         Odds = new OddDto[]
                                         {
                                             new OddDto
                                             {
                                                 Id = "1",
                                                 Value = 1,
                                                 SpecialBetValue = "7",
                                                 Name = "2:0",
                                             },
                                             new OddDto
                                             {
                                                 Id = "2",
                                                 Value = 2,
                                                 SpecialBetValue = "7",
                                                 Name = "2:2",
                                             },
                                             new OddDto
                                             {
                                                 Id = "13",
                                                 Value = 2,
                                                 SpecialBetValue = "10",
                                                 Name = "5:2",
                                             },
                                             new OddDto
                                             {
                                                 Id = "14",
                                                 Value = 2,
                                                 SpecialBetValue = "10",
                                                 Name = "6:2",
                                             },
                                         },
                                     },
                                 },
                            },
                                 new MatchDto
                                 {
                                     Id = "2",
                                     Name = "Alliance - Brame",
                                     StartDate = DateTime.UtcNow.AddHours(23).AddMinutes(59),
                                     MatchType = "Live",
                                     Bets = new BetDto[]
                                     {
                                         new BetDto
                                         {
                                             Id = "2",
                                             IsLive = true,
                                             Name = "Total Maps Played",
                                             Odds = new OddDto[]
                                             {
                                                 new OddDto
                                                 {
                                                     Id = "3",
                                                     Value = 1,
                                                     SpecialBetValue = null,
                                                     Name = "1:0",
                                                 },
                                                 new OddDto
                                                 {
                                                     Id = "4",
                                                     Value = 2,
                                                     SpecialBetValue = null,
                                                     Name = "2:1",
                                                 },
                                             },
                                         },
                                     },
                                 },
                                 new MatchDto
                                 {
                                     Id = "3",
                                     Name = "Hellbear Smashers - Hippomaniacs",
                                     StartDate = DateTime.UtcNow.AddHours(15),
                                     MatchType = "Live",
                                     Bets = new BetDto[]
                                     {
                                         new BetDto
                                         {
                                             Id = "3",
                                             IsLive = true,
                                             Name = "Map Advantage",
                                             Odds = new OddDto[]
                                             {
                                                 new OddDto
                                                 {
                                                     Id = "5",
                                                     Value = 1,
                                                     SpecialBetValue = null,
                                                     Name = "2:3",
                                                 },
                                                 new OddDto
                                                 {
                                                     Id = "6",
                                                     Value = 2,
                                                     SpecialBetValue = null,
                                                     Name = "2:4",
                                                 },
                                             },
                                         },
                                     },
                                 },
                                 new MatchDto
                                 {
                                     Id = "4",
                                     Name = "Chicken Fighters - Nigma",
                                     StartDate = DateTime.UtcNow.AddDays(3),
                                     MatchType= "Live",
                                     Bets = new BetDto[]
                                     {
                                         new BetDto
                                         {
                                             Id = "4",
                                             IsLive = true,
                                             Name = "Invalid",
                                             Odds = new OddDto[]
                                             {
                                                 new OddDto
                                                 {
                                                     Id = "7",
                                                     Value = 1,
                                                     SpecialBetValue = null,
                                                     Name = "2:5",
                                                 },
                                                 new OddDto
                                                 {
                                                     Id = "8",
                                                     Value = 2,
                                                     SpecialBetValue = null,
                                                     Name = "2:6",
                                                 },
                                             },
                                         },
                                     },
                                 },
                            },
                        },
                    },
                },
            };
        }
    }
}
