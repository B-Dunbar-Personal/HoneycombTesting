using Demo.Api.DatabaseContainer;
using Demo.Api.Tests.Scripts;
using TechTalk.SpecFlow;
using Microsoft.Extensions.Configuration;

namespace Demo.Api.Tests.Hooks
{
    [Binding]
    [Scope(Tag = "DataSeeder")]
    public class DatabaseDestroyer
    {
        private readonly SqlContainer _container;

        public DatabaseDestroyer()
        {
            var configuration = Configuration().GetSection("AppSettings").Get<AppSettings>();
            var containerConfiguration = new ContainerConfiguration
            {
                Datasource = configuration.Datasource,
                UserId = configuration.UserId,
                Password = configuration.Password
            };
            _container = new SqlContainer(containerConfiguration);
        }

        [AfterScenario]
        public async Task DestroySeededData()
        {
            await _container.ExecuteNonQuery(await SqlFileReader.GetSqlFile("CleanUpTest"));
        }

        private Microsoft.Extensions.Configuration.IConfiguration Configuration()
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                   .AddJsonFile("TestSettings.json")
                   .Build();

            return config;
        }
    }
}
