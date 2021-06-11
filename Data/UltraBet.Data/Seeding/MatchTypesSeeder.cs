namespace UltraBet.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using UltraBet.Common;

    using MatchType = UltraBet.Data.Models.MatchType;

    public class MatchTypesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var initialMatchTypes = new List<MatchType>();

            var liveType = new MatchType
            {
                Name = GlobalConstants.LiveMatchType,
            };

            var prematchType = new MatchType
            {
                Name = GlobalConstants.PrematchMatchType,
            };

            initialMatchTypes.Add(liveType);
            initialMatchTypes.Add(prematchType);

            await SeedTypesAsync(initialMatchTypes, dbContext);
        }

        private static async Task SeedTypesAsync(List<MatchType> initialMatchTypes, ApplicationDbContext dbContext)
        {
            foreach (var type in initialMatchTypes)
            {
                var currentType = dbContext.MatchTypes.FirstOrDefault(x => x.Name == type.Name);

                if (currentType is null)
                {
                    await dbContext.MatchTypes.AddAsync(type);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
