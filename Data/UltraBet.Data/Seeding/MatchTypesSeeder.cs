using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltraBet.Common;
using MatchType = UltraBet.Data.Models.MatchType;

namespace UltraBet.Data.Seeding
{
    public class MatchTypesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var initialTypes = new List<MatchType>();

            var liveType = new MatchType
            {
                Name = GlobalConstants.LiveMatchType,
            };

            var prematchType = new MatchType
            {
                Name = GlobalConstants.PrematchMatchType,
            };

            initialTypes.Add(liveType);
            initialTypes.Add(prematchType);

            await SeedTypesAsync(initialTypes, dbContext);
        }

        private static async Task SeedTypesAsync(List<MatchType> initialTypes, ApplicationDbContext dbContext)
        {
            foreach (var item in initialTypes)
            {
                var currentType = dbContext.MatchTypes.FirstOrDefault(x => x.Name == item.Name);

                if (currentType is null)
                {
                    await dbContext.MatchTypes.AddAsync(item);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
