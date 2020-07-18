namespace tymbot
{
    using System;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
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
            services.AddDbContextPool<TymDbContext>(
                options => options.UseMySql(this.configuration.GetConnectionString("default"),
                    mySqlOptions => mySqlOptions.ServerVersion(new Version(10, 1, 36), ServerType.MariaDb)
            ));
            services.AddSingleton<BotService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(BotService botService)
        {
            botService.Initialize();
        }
    }
}