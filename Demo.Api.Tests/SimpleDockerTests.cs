using Demo.Api.DatabaseContainer;
using DockerIntegrationTesting.Tests.Scripts;
using FluentAssertions;
using RestSharp;
using System.Net;

namespace Demo.Api.Tests
{
    public class SimpleDockerTests : IAsyncLifetime
    {
        private readonly SqlContainer _container;
        private readonly RestClient _client;
        private Person _testData;

        public SimpleDockerTests()
        {
            var containerConfiguration = new ContainerConfiguration
            {
                PortNumber = "1433",
                DatabasePassword = "P@assw0rd1"
            };

            _container = new SqlContainer(containerConfiguration);
            _client = new RestClient("http://localhost:8080");

            _testData = new Person
            {
                PersonId = 1,
                LastName = "Cheese",
                FirstName = "Test",
                Address1 = "89 Not an Address",
                Address2 = "AddressLine2",
                City = "Dingleberry".ToUpper(),
            };
        }

        public async Task DisposeAsync()
        {
            await _container.ExecuteNonQuery(await SqlFileReader.GetSqlFile("CleanUpTest"));
        }

        public async Task InitializeAsync()
        {
            await _container.ExecuteNonQuery(await SqlFileReader.GetSqlFile("CreateDatabase"));
            await _container.ExecuteNonQuery(await SqlFileReader.GetSqlFile("CreatePersonTable"));
        }

        [Fact]
        public async Task AddressLine1_AddressLine2_City_ConcatonateUsersAddressLines_ConcatonateSuccess()
        {
            //Arrange
            await SeedDatabase();
            var request = new RestRequest($"/Demo/Address?id={_testData.PersonId}", Method.Get);

            //Act
            var result = await _client.GetAsync(request);

            //Assert
            result.Content.Should().Contain($"{_testData.Address1} | {_testData.Address2} | {_testData.City}");
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task AddressLine1_AddressLine2_IsNull_City_ConcatonateUsersAddressLines_ConcatonateSuccess()
        {
            //Arrange
            _testData.Address2 = null;
            await SeedDatabase();
            var request = new RestRequest($"/Demo/Address?id={_testData.PersonId}", Method.Get);

            //Act
            var result = await _client.GetAsync(request);

            //Assert
            result.Content.Should().Contain($"{_testData.Address1} | {_testData.City}");
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task AddressLine1_AddressLine2_IsEmpty_City_ConcatonateUsersAddressLines_ConcatonateSuccess()
        {
            //Arrange
            _testData.Address2 = string.Empty;
            await SeedDatabase();
            var request = new RestRequest($"/Demo/Address?id={_testData.PersonId}", Method.Get);

            //Act
            var result = await _client.GetAsync(request);

            //Assert
            result.Content.Should().Contain($"{_testData.Address1} | {_testData.City}");
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task AddressLine1_AddressLine2_HasEmptySpace_City_ConcatonateUsersAddressLines_ConcatonateSuccess()
        {
            //Arrange
            _testData.Address2 = " ";
            await SeedDatabase();
            var request = new RestRequest($"/Demo/Address?id={_testData.PersonId}", Method.Get);

            //Act
            var result = await _client.GetAsync(request);

            //Assert
            result.Content.Should().Contain($"{_testData.Address1} | {_testData.Address2} | {_testData.City}");
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task MoreThanOneResultReturnedFromDatabase_ConcatonateUsersAddressLines_ConcatonateFailure()
        {
            //Arrange
            await SeedDatabase();
            await SeedDatabase();
            var request = new RestRequest($"/Demo/Address?id={_testData.PersonId}", Method.Get);

            //Act
            Func<Task> result = async () => { await _client.GetAsync(request); };

            //Assert
            await result.Should().ThrowAsync<HttpRequestException>(); ;
        }

        [Fact]
        public async Task NoResultReturnedFromDatabase_ConcatonateUsersAddressLines_ConcatonateFailure()
        {
            //Arrange
            var request = new RestRequest($"/Demo/Address?id={_testData.PersonId}", Method.Get);

            //Act
            var result = await _client.GetAsync(request);

            //Assert
            result.Content.Should().Contain("No Records Found");
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        private async Task SeedDatabase()
        {
            await _container.ExecuteNonQuery(await SqlFileReader.GetSqlFile("SeedPersonTable"), _testData);
        }
    }
}