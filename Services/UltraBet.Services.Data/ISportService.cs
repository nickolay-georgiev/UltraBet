using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UltraBet.Services.Data
{
    public interface ISportService
    {
        Task StoreDataAsync();
    }
}
