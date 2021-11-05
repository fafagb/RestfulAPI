using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using API.Extensions;
using API.Core.Interfaces;
using API.Repository.Database;
using API.Repository.Repositories;
using API.Repository.Resources;
using API.Repository.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using API.Core.Entities;
using Swashbuckle.AspNetCore.Swagger;
using System.Diagnostics;

namespace API
{
    public class Startup
    {

        public static IConfiguration Configuration { get; set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940




        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }



        public void ConfigureServices(IServiceCollection services)
        {

            services.AddMvc(options =>
            {

                options.ReturnHttpNotAcceptable = true;//内容协商
                //options.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());

                var inputFormatter = options.InputFormatters.OfType<SystemTextJsonInputFormatter>().FirstOrDefault();
                if (inputFormatter != null)
                {
                    //供应商mediatype
                    inputFormatter.SupportedMediaTypes.Add("application/vnd..post.create+json");
                    inputFormatter.SupportedMediaTypes.Add("application/vnd..put.update+json");
                }
                var outputFormatter = options.OutputFormatters.OfType<SystemTextJsonInputFormatter>().FirstOrDefault();
                if (outputFormatter != null)
                {
                    outputFormatter.SupportedMediaTypes.Add("application/vnd..hateoas+json");
                }
            }).AddJsonOptions(options =>
            {
              //  options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }).AddFluentValidation();
            services.AddDbContext<mydbContext>(options =>
            {
                //var connectionString = Configuration["ConnectionStrings:DefaultConnection"];
                var connectionString = Configuration.GetConnectionString("DefaultConnection");
                options.UseMySQL(connectionString);
            });


            // services.AddHttpsRedirection(options =>
            // {
            //     options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
            //   //  options.HttpsPort = 6001;

            // });

  

            //services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
            //    .AddIdentityServerAuthentication(options => {
            //        options.Authority = "https://localhost:5001";
            //        options.ApiName = "restapi";

            //    }


            //    );

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Interface Online Documentation", Version = "v1" });
            });



            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //services.AddHsts();生产建议启用Hsts
            services.AddAutoMapper();
            services.AddTransient<IValidator<PersonAddResource>, PersonAddOrUpdateResourceValidator<PersonAddResource>>();
            services.AddTransient<IValidator<PersonUpdateResource>, PersonAddOrUpdateResourceValidator<PersonUpdateResource>>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(factory =>
            {
                var actionContext = factory.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });

            var propertyMappingContainer = new PropertyMappingContainer();
            propertyMappingContainer.Register<PersonPropertyMapping>();
            services.AddSingleton<IPropertyMappingContainer>(propertyMappingContainer);

            services.AddTransient<ITypeHelperService, TypeHelperService>();

            //授权策略,针对所有的controller 已认证用户才可访问
            //services.Configure<MvcOptions>( options =>
            //{
            //    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
            //    options.Filters.Add(new AuthorizeFilter(policy));

            //});
        }



        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            app.UseExceptionHandler(loggerFactory);


            //if (env.IsDevelopment())
            //{
            //    
            //    app.UseDeveloperExceptionPage();
            //}


            //app.UseHsts();
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API");
            });


            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Hello World!");
            //});


        }
    }
}
