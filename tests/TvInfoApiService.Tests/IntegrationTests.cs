using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TvInfo.Persistence.Models;
using Xunit;

namespace TvInfoApiService.Tests
{
    public class IntegrationTests
    {
        public IntegrationTests()
        {
            using (var context = CreateProvider())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }

        [Fact]
        public async Task GetShowsFromApi_ReturnsExpectedListOfShows()
        {
            ShowTestModel[] testShows = {
                new ShowTestModel{Id = 1, Name = "Show 1"}, 
                new ShowTestModel{Id = 2, Name = "Show 2"} 
            };

            SeedDatabase(testShows);

            using (TestServer server = SetupTestServer())
            {
                HttpClient client = server.CreateClient();

                HttpResponseMessage result = await client.GetAsync("/api/Shows");

                Assert.Equal(HttpStatusCode.OK, result.StatusCode);
                Assert.Equal(
                    "[{\"id\":1,\"name\":\"Show 1\",\"cast\":[]},{\"id\":2,\"name\":\"Show 2\",\"cast\":[]}]",
                    await result.Content.ReadAsStringAsync());
            }
        }

        private static TestServer SetupTestServer()
        {
            var builder = new WebHostBuilder()
                .UseStartup<Startup>()
                .UseEnvironment("Test")
                .ConfigureTestServices(service => service.AddScoped(_ => CreateProvider()));

            return new TestServer(builder);
        }

        private static ShowContext CreateProvider()
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var configuration = configBuilder.Build();
            var connectionStr = configuration.GetConnectionString("DefaultConnection");

            return new ShowContext(new DbContextOptionsBuilder<ShowContext>()
                .UseSqlServer(connectionStr)
                .Options);
        }

        private static void SeedDatabase(params ShowTestModel[] shows)
        {
            using (var context = CreateProvider())
            {
                context.Shows.AddRange(shows.Select(showTestModel => showTestModel.CreateShow()));
                context.SaveChanges();
            }
        }
    }
}
