using Demo.Api.DatabaseContainer;
using Demo.Api.Tests.Scripts;
using TechTalk.SpecFlow;

namespace Demo.Api.Tests.Hooks
{
    [Binding]
    [Scope(Tag = "DataSeeder")]
    public class DatabaseDestroyer
    {
        private readonly SqlContainer _container;

        public DatabaseDestroyer()
        {
            var containerConfiguration = new ContainerConfiguration
            {
                PortNumber = "1433",
                DatabasePassword = "P@assw0rd1"
            };
            _container = new SqlContainer(containerConfiguration);
        }

        [AfterScenario]
        public async Task DestroySeededData()
        {
            await _container.ExecuteNonQuery(await SqlFileReader.GetSqlFile("CleanUpTest"));
        }
    }
}
