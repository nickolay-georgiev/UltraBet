namespace UltraBet.Services.Data.Tests.ServiceTests.Helpers
{
    using System;
    using System.Collections.Generic;

    using UltraBet.Data.Models;

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
                                    GroupNumber = 1,
                                    SpecialBetValue = null,
                                    OddName = new OddName { Name = "2:0"},
                                },
                                new Odd
                                {
                                    Id = "2",
                                    Value = 2,
                                    GroupNumber = 2,
                                    SpecialBetValue = null,
                                    OddName = new OddName { Name = "2:2"},
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
                                    GroupNumber = 3,
                                    SpecialBetValue = null,
                                    OddName = new OddName {Name = "1:0"},
                                },
                                new Odd
                                {
                                    Id = "4",
                                    Value = 2,
                                    GroupNumber = 4,
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
                                    GroupNumber = 5,
                                    SpecialBetValue = null,
                                    OddName = new OddName {Name = "2:3"},
                                },
                                new Odd
                                {
                                    Id = "6",
                                    Value = 2,
                                    GroupNumber = 6,
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
                                    GroupNumber = 9,
                                    SpecialBetValue = null,
                                    OddName = new OddName {Name = "2:5"},
                                },
                                new Odd
                                {
                                    Id = "8",
                                    Value = 2,
                                    GroupNumber = 10,
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
    }
}
