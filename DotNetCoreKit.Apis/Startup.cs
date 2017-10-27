// -----------------------------------------------------------------------
// <copyright file="Startup.cs" company="ChaiOne">
// Copyright (c) ChaiOne. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace DotNetCoreKit.Apis
{
    using System.IO;
    using AutoMapper;
    using FluentValidation.AspNetCore;
    using FluentValidations;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.PlatformAbstractions;
    using Models;
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
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">
        /// The services.
        /// </param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<PeopleContext>(opt => opt.UseInMemoryDatabase("PeopleList"));

            services.AddMvc().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<PersonValidator>());

            services.AddAutoMapper();
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

            Mapper.Configuration.AssertConfigurationIsValid();
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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            // Enable Middle-ware to serve generated swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable Middle-ware to serve swagger-ui
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.InjectStylesheet("/swagger/custom.css");
            });

            app.UseMvc();
        }
    }
}