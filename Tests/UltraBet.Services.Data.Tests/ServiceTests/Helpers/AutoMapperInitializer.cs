namespace UltraBet.Services.Data.Tests.ServiceTests.Helpers
{
    using System.Reflection;

    using UltraBet.Services.Mapping;
    using UltraBet.Web.ViewModels;

    public class AutoMapperInitializer
    {
        public static void Init()
        {
            AutoMapperConfig.RegisterMappings(typeof(MatchViewModel).GetTypeInfo().Assembly);
        }
    }
}
