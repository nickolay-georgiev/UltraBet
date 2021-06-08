namespace UltraBet.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using UltraBet.Services.Models;

    public interface ISportDataService
    {
        Task<XmlSportsDto> GetSportDataAsync();
    }
}
