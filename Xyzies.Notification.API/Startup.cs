using IdentityServiceClient;
using IdentityServiceClient.Middlewares;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureADB2C.UI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Xyzies.Notification.Data;
using Xyzies.Notification.Data.Repository;
using Xyzies.Notification.Data.Repository.Behaviour;
using Xyzies.Notification.Services.Common.Interfaces;
using Xyzies.Notification.Services.Helpers;
using Xyzies.Notification.Services.Services;

namespace Xyzies.Notification.API
{
    public class Startup
    {
        private readonly ILogger _logger = null;

        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IConfiguration Configuration { get; }

        public static string ServiceBaseUrlPrefix { get; set; } = "/api/notification-management-api"; // Default

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(AzureADB2CDefaults.BearerAuthenticationScheme)
                .AddAzureADB2CBearer(options => Configuration.Bind("AzureAdB2C", options));
            services.AddCors();
            services.AddDataProtection();
            services.AddHealthChecks();
            services.AddHttpContextAccessor();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
           
            services.AddIdentityClient(options =>
            {
                options.ServiceUrl = options.ServiceUrl = Configuration.GetSection("Services")["IdentityServiceUrl"];
            });

            services.Configure<MailerOptions>(options => Configuration.Bind("MailServiceOptions", options));

            services.AddSwaggerGen(options =>
            {
                options.SwaggerGeneratorOptions.IgnoreObsoleteActions = true;

                options.SwaggerDoc("v1", new Info
                {
                    Title = "Xyzies.Notification",
                    Version = $"v1.0.0",
                    Description = ""
                });

                options.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    In = "header",
                    Name = "Authorization",
                    Description = "Please enter JWT with Bearer into field",
                    Type = "apiKey"
                });

                options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", Enumerable.Empty<string>() }
                });

                options.CustomSchemaIds(x => x.FullName);
                options.EnableAnnotations();
                options.DescribeAllEnumsAsStrings();
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
                   string.Concat(Assembly.GetExecutingAssembly().GetName().Name, ".xml")));
            });

            services.AddDbContext<NotificationContext>(ctxOptions =>
                    ctxOptions.UseSqlServer(Configuration.GetConnectionString("db")));

            #region DI settings

            services.AddScoped<IMailerService, MailerService>();
            services.AddScoped<IMessageTemplateRepository, MessageTemplateRepository>();
            #endregion

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseHttpsRedirection();
            }


            app.UseCors("dev")
               .UseAuthentication()
               .UseClientMiddleware()
               .UseMvc()
               .UseHealthChecks("/api/health")
               .UseSwagger(options =>
               {
                   //options.PreSerializeFilters.Add((swaggerDoc, httpReq) => swaggerDoc.BasePath = $"{ServiceBaseUrlPrefix}");

                   options.RouteTemplate = "/swagger/{documentName}/swagger.json";

               })
               .UseSwaggerUI(uiOptions =>
               {
                   uiOptions.SwaggerEndpoint("v1/swagger.json", $"v1.0.0");
                   uiOptions.DisplayRequestDuration();
               });

            _logger.LogDebug("Startup configured successfully.");
        }
    }
}
