using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Net.Http;

namespace SchoolLibrary.Tests.Integration
{
    public class TestServerProvider : IDisposable
    {
        private TestServer server;
        public HttpClient Client { get; private set; }

        public TestServerProvider()
        {
            var webHostBuilder = new WebHostBuilder()
                       .UseEnvironment("Development")
                       .UseStartup<Startup>();  // Uses Start up class from your API Host project to configure the test server

            server = new TestServer(webHostBuilder);
            Client = server.CreateClient();
        }

        public void Dispose()
        {
            server?.Dispose();
            Client?.Dispose();
        }
    }
}
