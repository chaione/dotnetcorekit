// -----------------------------------------------------------------------
// <copyright file="Startup.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Apis
{
    using System;
    using System.IO;

    using Autofac;
    using Autofac.Extensions.DependencyInjection;
    using AutoMapper;
    using DotNetCoreKit.EntityFramework;
    using DotNetCoreKit.Models.Domain;
    using FluentValidation.AspNetCore;
    using FluentValidations;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.PlatformAbstractions;
    using Swashbuckle.AspNetCore.Swagger;

    /// <summary>
    /// The startup.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">
        /// The configuration.
        /// </param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Gets application container.
        /// </summary>
        public IContainer ApplicationContainer { get; private set; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">
        /// The services.
        /// </param>
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

            // Configure the minimum requirements for said user identity.
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 10;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 6;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.Cookie.Expiration = TimeSpan.FromDays(150);
                options.LoginPath = "/Account/Login"; // If the LoginPath is not set here, ASP.NET Core will default to /Account/Login
                options.LogoutPath = "/Account/Logout"; // If the LogoutPath is not set here, ASP.NET Core will default to /Account/Logout
                options.AccessDeniedPath = "/Account/AccessDenied"; // If the AccessDeniedPath is not set here, ASP.NET Core will default to /Account/AccessDenied
                options.SlidingExpiration = true;
            });

            // Add fluent validation and register validator using reflection to discover containing assembly.
            services.AddMvc().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<PersonValidator>());

            // Automapper configurations
            services.AddAutoMapper();
            Mapper.Configuration.AssertConfigurationIsValid();

            // Swagger configurations
            services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new Info { Title = "My Api", Version = "v1" });

                    // Set the comments path for the Swagger JSON and UI.
                    var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                    var xmlPath = Path.Combine(basePath, "DotNetCoreKit.xml");
                    c.IncludeXmlComments(xmlPath);
                    c.DescribeAllEnumsAsStrings();
                    c.DescribeAllParametersInCamelCase();
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
        /// <param name="app">
        /// The app.
        /// </param>
        /// <param name="env">
        /// The env.
        /// </param>
        /// <param name="applicationLifetime">application lifetime indicator</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseAuthentication();
            app.UseStaticFiles();

            // Enable Middle-ware to serve generated swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable Middle-ware to serve swagger-ui
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.InjectStylesheet("/swagger/custom.css");
                c.DocExpansion("none");
                c.ShowJsonEditor();
            });

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