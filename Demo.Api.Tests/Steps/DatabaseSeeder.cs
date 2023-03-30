using Demo.Api.DatabaseContainer;
using Demo.Api.Tests.Scripts;
using TechTalk.SpecFlow;

namespace Demo.Api.Tests.Steps
{
    [Binding]
    [Scope(Tag = "DataSeeder")]
    public class DatabaseSeeder
    {
        private ScenarioContext _scenarioContext;
        private readonly SqlContainer _container;

        public DatabaseSeeder(ScenarioContext scenarioContext)
        {
            var containerConfiguration = new ContainerConfiguration
            {
                PortNumber = "1433",
                DatabasePassword = "P@assw0rd1"
            };
            _container = new SqlContainer(containerConfiguration);
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario]
        public async Task SetupDatabase()
        {
            _scenarioContext[ScenarioContextTags.Person] = new Person
            {
                PersonId = 1,
                LastName = "Cheese",
                FirstName = "Test",
                Address1 = "89 Not an Address",
                Address2 = "AddressLine2",
                City = "Dingleberry".ToUpper(),
            };
            _scenarioContext[ScenarioContextTags.SqlContainer] = _container;
            await _container.ExecuteNonQuery(await SqlFileReader.GetSqlFile("CreateDatabase"));
            await _container.ExecuteNonQuery(await SqlFileReader.GetSqlFile("CreatePersonTable"));
        }

        [AfterScenario]
        public async Task DestroySeededData()
        {
            await _container.ExecuteNonQuery(await SqlFileReader.GetSqlFile("CleanUpTest"));
        }
    }
}
