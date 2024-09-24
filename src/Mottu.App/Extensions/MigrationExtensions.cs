using Microsoft.EntityFrameworkCore;

namespace Mottu.App.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            int retries = 5;
            int delay = 2000;

            for (int i = 0; i < retries; i++)
            {
                try
                {
                    dbContext.Database.Migrate();
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Try {i + 1} failed: {ex.Message}");
                    Thread.Sleep(delay);
                }
            }

            throw new Exception("Failed to connect in database, please stop and run again.");
        }
    }
}
