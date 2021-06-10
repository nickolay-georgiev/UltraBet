namespace UltraBet.Web.Hubs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.SignalR;
    using UltraBet.Services.Data;

    public class MatchHub : Hub
    {
        private readonly ISportService sportService;

        public MatchHub(ISportService sportService)
        {
            this.sportService = sportService;
        }

        public async Task GetMatchesInNextTwentyFourHours()
        {
            var matchesData = this.sportService.GetMatchesInNextTwentyFourHours();
            await this.Clients.All.SendAsync("getMatchesInNext24Hours", matchesData);
        }

        public async Task GetMatchById(string id)
        {
            var matchData = this.sportService.GetMatchById(id);
            await this.Clients.Caller.SendAsync("getMatchById", matchData);
        }
    }
}
