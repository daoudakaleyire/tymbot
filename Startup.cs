namespace tymbot
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
    using tymbot.Data;

    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configure dbcontext
            string connectionString = this.configuration
                .GetConnectionString("default")
                .Replace("__PASSWORD__", Environment.GetEnvironmentVariable("DATABASE_ROOT_PASSWORD"));
            services.AddDbContextPool<TymDbContext>(
                options => options.UseMySql(connectionString,
                    mySqlOptions => mySqlOptions.ServerVersion(new Version(10, 1, 36), ServerType.MariaDb)
            ));

            // Register Service
            services.AddSingleton<BotService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(BotService botService, TymDbContext db, ILogger<Startup> logger)
        {
            while (true)
            {
                try
                {
                    db.Database.Migrate();
                    logger.LogInformation("Database migration successful.");
                    break;
                }
                catch (Exception ex)
                {
                    logger.LogError($"Database not ready: {ex.Message}");
                    Task.Delay(1000).Wait();
                }
            }
            
            botService.Initialize();
        }
    }
}