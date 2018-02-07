using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiturgieMakerAPI.Data;
using LiturgieMakerAPI.LiedBundels.Context;
using LiturgieMakerAPI.LiedBundels.Repositories;
using LiturgieMakerAPI.LiturgieMaker.Context;
using LiturgieMakerAPI.LiturgieMaker.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LiturgieMakerAPI
{
    public class Startup
    {
        private readonly string LITURGIEMAKERDBNAME = "Liturgie";
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        private IHostingEnvironment CurrentEnvironment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureLiedBundels(services);
            ConfigureLiturgieMaker(services);

            if (CurrentEnvironment.IsDevelopment())
            {
                services.AddCors(options =>
                {
                    options.AddPolicy("AllowAll", builder =>
                        {
                            builder
                                .AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader()
                                .AllowCredentials();
                        });
                });
            }
            else
            {
                // TODO Cors voor prod
                services.AddCors();
            }

            services.AddMvc()
                .AddJsonOptions(
                    options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // Setup testdata
                using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
                {
                    var liedBundelsContext = serviceScope.ServiceProvider.GetRequiredService<LiedBundelsContext>();
                    var liturgieMakerContext = serviceScope.ServiceProvider.GetRequiredService<LiturgieMakerContext>();
                    LiedBundelInitializer.Initialize(liedBundelsContext);
                    LiturgieMakerInitializer.Initialize(liturgieMakerContext, liedBundelsContext);
                }
            }

            app.UseMvc();
        }

        public void ConfigureLiedBundels(IServiceCollection services)
        {
            services.AddDbContext<LiedBundelsContext>(opt => opt.UseInMemoryDatabase(LITURGIEMAKERDBNAME));
            services.AddScoped<LiedBundelRepository>();
        }

        public void ConfigureLiturgieMaker(IServiceCollection services)
        {
            services.AddDbContext<LiturgieMakerContext>(opt => opt.UseInMemoryDatabase(LITURGIEMAKERDBNAME));
            services.AddScoped<LiturgieRepository>();
        }
    }
}
