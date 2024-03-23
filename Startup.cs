using System;
using CodeRacerBackend.Hubs;
using CodeRacerBackend.Utils;
using Coravel;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace CodeRacerBackend;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        var cors = Configuration.GetValue<string>("CorsAllow");
        services.AddCors(options => options.AddPolicy("CorsPolicy",
            builder =>
            {
                builder.AllowAnyMethod().AllowAnyHeader()
                    .WithOrigins(cors)
                    .AllowCredentials().SetIsOriginAllowed(host => true);
            }));

        services.AddSignalR();
        services.AddScheduler();
        services.AddControllers();
        
        services.AddSingleton<ISnippetFinder>(provider => new OctoSnippetFinder(Configuration));
        services.AddSingleton<ILobbyManager>(provider => new LobbyManager());
        services.AddTransient<LobbyCleardownTask>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {

        if (env.IsDevelopment()) app.UseDeveloperExceptionPage();


        if (!env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            // app.UseHttpsRedirection();
        }

        app.ApplicationServices.UseScheduler((scheduler) => {
            scheduler.Schedule<LobbyCleardownTask>().DailyAtHour(1);
        });
        app.UseRouting();
        app.UseCors("CorsPolicy");
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHub<LobbyHub>("/app/lobbyHub");
        });
    }
}