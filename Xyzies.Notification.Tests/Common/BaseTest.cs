using AutoFixture;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using Xyzies.Notification.API;
using Xyzies.Notification.Data;

namespace Xyzies.Notification.Tests.Common
{
    public class BaseTest : IDisposable
    {
        private object _lock = new object();
        public TestServer TestServer;
        public HttpClient HttpClient;
        public NotificationContext DbContext;
        public Fixture Fixture;

        public BaseTest()
        {
            lock (_lock)
            {
                var builder = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json", optional: true);

                var configuration = builder.Build();

                IWebHostBuilder webHostBuild =
                        WebHost.CreateDefaultBuilder()
                               .UseStartup<Startup>()
                               .UseWebRoot(Directory.GetCurrentDirectory())
                               .UseContentRoot(Directory.GetCurrentDirectory());

                var dbConnectionString = configuration.GetConnectionString("db");
                if (string.IsNullOrEmpty(dbConnectionString))
                {
                    throw new ApplicationException("Missing the connection string to database");
                };
                webHostBuild.ConfigureServices(service =>
                {
                    service.AddDbContextPool<NotificationContext>(ctxOptions => ctxOptions.UseInMemoryDatabase(dbConnectionString).EnableSensitiveDataLogging());
                });
                TestServer = new TestServer(webHostBuild);
                DbContext = TestServer.Host.Services.GetRequiredService<NotificationContext>();
                HttpClient = TestServer.CreateClient();
                Fixture = new Fixture();
            }
        }

        public void Dispose()
        {
            DbContext.Dispose();
            HttpClient.Dispose();
            TestServer.Dispose();
        }
    }
}
