﻿namespace UltraBet.Web.ViewModels
{
    using System.Linq;

    using AutoMapper;
    using UltraBet.Common;
    using UltraBet.Data.Models;
    using UltraBet.Services.Mapping;

    public class BetViewModel : BaseBetViewModel, IHaveCustomMappings
    {
        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Bet, BetViewModel>()
                .ForMember(x => x.Odds, opt =>
                    opt.MapFrom(x => x.Odds
                       .Where(x => x.GroupNumber == GlobalConstants.DefaultGroupNumber)))
                .ForMember(x => x.Name, opt =>
                    opt.MapFrom(x => x.BetName.Name));
        }
    }
}
