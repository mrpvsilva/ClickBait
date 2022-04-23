using ClickBait.Infra.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ClickBait.Infra.Extensions
{
    public static class IServiceProviderExtensions
    {
        public static void ConfigureApplication(this IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                scope.DatabaseMigrate();
            }
        }


        private static void DatabaseMigrate(this IServiceScope scope)
        {
            var dataContext = scope.ServiceProvider.GetRequiredService<ClickBaitContext>();
            dataContext.Database.Migrate();
        }
    }
}
