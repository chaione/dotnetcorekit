// -----------------------------------------------------------------------
// <copyright file="Startup.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Webservices
{
    using System;
    using System.IO;
    using System.Text;

    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using AutoMapper;
    using DotNetCoreKit.EntityFramework;
    using DotNetCoreKit.Models.Domain;
    using DotNetCoreKit.Webservices.FluentValidations;
    using DotNetCoreKit.Webservices.Models;
    using FluentValidation.AspNetCore;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.PlatformAbstractions;
    using Microsoft.IdentityModel.Tokens;
    using Swashbuckle.AspNetCore.Swagger;

    /// <summary>
    /// The startup method that starts the whole api service
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="env">Environment variable for hosted service.</param>
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Environment = env;

            var builder = new ConfigurationBuilder()
                         .SetBasePath(env.ContentRootPath)
                         .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Gets or sets application container.
        /// </summary>
        public IContainer ApplicationContainer { get; set; }

        /// <summary>
        /// Gets or sets hosting Environment variable.
        /// </summary>
        public IHostingEnvironment Environment { get; set; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns>Service Provider to allow dependency injection.</returns>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add Db contexts
            var dbString = Guid.NewGuid().ToString();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase(dbString));

            services.AddDbContext<PeopleContext>(opt => opt.UseInMemoryDatabase(dbString));

            // Add user Identity for authentication.
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            if (!Environment.IsDevelopment())
            {
                services.Configure<MvcOptions>(options => { options.Filters.Add(new RequireHttpsAttribute()); });
            }

            // Add settings from configuration
            services.Configure<AppConfigurationSettings>(Configuration.GetSection("AppConfiguration"));

            // Add JWT token for authentication
            services.AddAuthentication().AddJwtBearer(jwtBearerOptions =>
            {
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidAudience = Configuration.GetSection("AppConfiguration:SiteUrl").Value,
                    ValidIssuer = Configuration.GetSection("AppConfiguration:SiteUrl").Value,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration.GetSection("AppConfiguration:Key").Value)),
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                };

                jwtBearerOptions.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = c =>
                    {
                        c.NoResult();

                        c.Response.StatusCode = 500;
                        c.Response.ContentType = "text/plain";

                        return c.Response.WriteAsync(Environment.IsDevelopment()
                            ? c.Exception.ToString()
                            : "An error occurred processing your authentication.");
                    },
                };
            });

            // Add fluent validation and register validator using reflection to discover containing assembly.
            services.AddMvc().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<PersonValidator>());

            // Automapper configurations
            services.AddAutoMapper();
            Mapper.Configuration.AssertConfigurationIsValid();

            // Swagger configurations
            services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new Info { Title = "My Api", Version = "v1" });

                    // Set the comments path for the Swagger JSON and UI.
                    var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                    var xmlPath = Path.Combine(basePath, "DotNetCoreKit.xml");
                    options.IncludeXmlComments(xmlPath);
                    options.DescribeAllEnumsAsStrings();
                    options.DescribeAllParametersInCamelCase();
                });

            // Add Autofac
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterModule<DefaultModule>();
            containerBuilder.Populate(services);
            ApplicationContainer = containerBuilder.Build();

            return new AutofacServiceProvider(ApplicationContainer);
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The app.</param>
        /// <param name="applicationLifetime">application lifetime indicator</param>
        public void Configure(IApplicationBuilder app, IApplicationLifetime applicationLifetime)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseRequestLocalization();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseStaticFiles();

            // Enable Middle-ware to serve generated swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable Middle-ware to serve swagger-ui
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.InjectStylesheet("/swagger/custom.css");
                c.DocExpansion(DocExpansion.None);
                c.EnableDeepLinking();
            });

            // Enable custom routing here.
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "DefaultApi",
                    template: "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute(
                    name: "Swagger",
                    template: "{swaggerUi=swagger}/#");
            });

            applicationLifetime.ApplicationStopped.Register(() => ApplicationContainer.Dispose());
        }
    }
}