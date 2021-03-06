﻿using AutoMapper;
using LiturgieMakerAPI.Config;
using LiturgieMakerAPI.Data;
using LiturgieMakerAPI.Liedbundels.Repositories;
using LiturgieMakerAPI.LiturgieMaker.Context;
using LiturgieMakerAPI.LiturgieMaker.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;

namespace LiturgieMakerAPI
{
    public class Startup
    {
        private readonly string LITURGIEMAKERDBNAME = "LiturgieMaker";

        private readonly IConfiguration Configuration;
        private readonly IHostingEnvironment CurrentEnvironment;
        private readonly StartupSettings Settings;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
            Settings = configuration.GetSection("StartupSettings").Get<StartupSettings>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            if (Settings.UseSwagger)
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
            }

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
                services.AddCors(options =>
                {
                    options.AddPolicy("AllowProd", builder =>
                    {
                        builder.WithOrigins(Settings.CorsOrigins)
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
                });
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
                // Setup testdata
                using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetRequiredService<LiturgieMakerContext>();
                    LiturgieMakerInitializer.Initialize(context);
                }

                app.UseCors("AllowAll");
            }
            else
            {
                // Migrate
                using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
                {
                    var context = serviceScope.ServiceProvider.GetRequiredService<LiturgieMakerContext>();
                    context.Database.Migrate();
                }


                if (Settings.UseReverseProxyServer)
                {
                    app.UseForwardedHeaders(new ForwardedHeadersOptions
                    {
                        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                    });
                }

                app.UseCors("AllowProd");
            }

            if (Settings.UseSwagger)
            {
                app.UseSwagger(c => { c.RouteTemplate = "api/swagger/{documentName}/swagger.json"; });
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("LiturgieMaker/swagger.json", "LiturgieMaker API");
                    c.RoutePrefix = "api/swagger";
                });
            }

            app.UseMvc();
        }

        private void ConfigureLiedbundels(IServiceCollection services)
        {
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
                optionsBuilder.UseMySql(Configuration.GetConnectionString(LITURGIEMAKERDBNAME));
            }
        }
    }
}
