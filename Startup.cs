using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LiturgieMakerAPI.Data;
using LiturgieMakerAPI.Liedbundels.Context;
using LiturgieMakerAPI.Liedbundels.Repositories;
using LiturgieMakerAPI.LiturgieMaker.Context;
using LiturgieMakerAPI.LiturgieMaker.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace LiturgieMakerAPI
{
    public class Startup
    {
        private readonly string LITURGIEMAKERDBNAME = "Liturgie";
        private IHostingEnvironment CurrentEnvironment { get; set; }

        public Startup(IHostingEnvironment env)
        {
            CurrentEnvironment = env;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("LiturgieMaker", new Info { Title = "LiturgieMaker API" });
                // Set the comments path for the Swagger JSON and UI.
                var basePath = AppContext.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "LiturgieMakerAPI.xml");
                c.IncludeXmlComments(xmlPath);
                c.DescribeAllEnumsAsStrings();
            });

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

            ConfigureLiedbundels(services);
            ConfigureLiturgieMaker(services);

            services.AddMvc()
                .AddJsonOptions(
                    options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
                );
            services.AddAutoMapper();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (CurrentEnvironment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // Setup testdata
                using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
                {
                    var liedbundelsContext = serviceScope.ServiceProvider.GetRequiredService<LiedbundelsContext>();
                    var liturgieMakerContext = serviceScope.ServiceProvider.GetRequiredService<LiturgieMakerContext>();
                    LiedbundelInitializer.Initialize(liedbundelsContext);
                    LiturgieMakerInitializer.Initialize(liturgieMakerContext, liedbundelsContext);
                }
            }
            app.UseSwagger(c => { c.RouteTemplate = "api/swagger/{documentName}/swagger.json"; });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("LiturgieMaker/swagger.json", "LiturgieMaker API");
                c.RoutePrefix = "api/swagger";
            });

            app.UseMvc();
        }

        private void ConfigureLiedbundels(IServiceCollection services)
        {
            services.AddDbContext<LiedbundelsContext>(opt => SelectDb(opt));
            services.AddScoped<LiedbundelRepository>();
        }

        private void ConfigureLiturgieMaker(IServiceCollection services)
        {
            services.AddDbContext<LiturgieMakerContext>(opt => SelectDb(opt));
            services.AddScoped<LiturgieRepository>();
        }

        private void SelectDb(DbContextOptionsBuilder optionsBuilder)
        {
            if (CurrentEnvironment.IsDevelopment())
            {
                optionsBuilder.UseInMemoryDatabase(LITURGIEMAKERDBNAME);
            }
            else
            {
                // TODO DB voor andere envs
            }
        }
    }
}
