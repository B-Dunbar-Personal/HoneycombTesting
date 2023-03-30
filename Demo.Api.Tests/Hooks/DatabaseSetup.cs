using Castle.Core.Configuration;
using Demo.Api.DatabaseContainer;
using Demo.Api.Tests.Scripts;
using Microsoft.Extensions.Configuration;
using TechTalk.SpecFlow;

namespace Demo.Api.Tests.Hooks
{
    [Binding]
    [Scope(Tag = "DataSeeder")]
    public class DatabaseSetup
    {
        private ScenarioContext _scenarioContext;
        private readonly SqlContainer _container;

        public DatabaseSetup(ScenarioContext scenarioContext)
        {
            var configuration = Configuration().GetSection("AppSettings").Get<AppSettings>();
            var containerConfiguration = new ContainerConfiguration
            {
                PortNumber = "1433",
                DatabasePassword = configuration.Password
            };
            _container = new SqlContainer(containerConfiguration);
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario]
        public async Task SetupDatabase()
        {
            _scenarioContext[ScenarioContextTags.SqlContainer] = _container;
            await _container.ExecuteNonQuery(await SqlFileReader.GetSqlFile("CreateDatabase"));
            await _container.ExecuteNonQuery(await SqlFileReader.GetSqlFile("CreatePersonTable"));
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
