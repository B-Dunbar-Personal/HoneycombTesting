using Demo.Api.DatabaseContainer;
using Demo.Api.Tests.Scripts;
using FluentAssertions;
using RestSharp;
using System.Net;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Demo.Api.Tests.Steps
{
    [Binding]
    public class SimpleDockerTestsStepDefinitions
    {
        private ScenarioContext _scenarioContext;
        private readonly SqlContainer _container;
        private readonly RestClient _client;

        public SimpleDockerTestsStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _container = scenarioContext[ScenarioContextTags.SqlContainer] as SqlContainer;
            _client = new RestClient("http://localhost:8080");
        }

        [Given(@"we have a user in our database:")]
        public async Task GivenWeHaveAUserInOurDatabase(Table table)
        {
            await GivenWeHaveTheSameUserInOurDatabaseTimes(0, table);
        }


        [Given(@"we have the same user in our database (.*) times:")]
        public async Task GivenWeHaveTheSameUserInOurDatabaseTimes(int seedCount, Table table)
        {
            var person = table.CreateInstance<Person>();
            _scenarioContext[ScenarioContextTags.Person] = person;

            if (seedCount == 0)
                await _container.ExecuteNonQuery(await SqlFileReader.GetSqlFile("SeedPersonTable"), person);
            else
                for (int i = 0; i < seedCount; i++)
                    await _container.ExecuteNonQuery(await SqlFileReader.GetSqlFile("SeedPersonTable"), person);
        }

        [When(@"a request is made to '([^']*)'")]
        public async Task WhenARequestIsMadeTo(string requestUrl)
        {
            var request = new RestRequest(requestUrl, Method.Get);
            _scenarioContext[ScenarioContextTags.RestResponse] = await _client.GetAsync(request);
        }

        [Then(@"address1, address2, and city are in a single line")]
        public void ThenAddress1Address2AndCityAreInASingleLine()
        {
            var response = _scenarioContext[ScenarioContextTags.RestResponse] as RestResponse;
            var testData = _scenarioContext[ScenarioContextTags.Person] as Person;
            response.Content.Should().Contain($"{testData.Address1} | {testData.Address2} | {testData.City.ToUpper()}");
        }

        [Then(@"address1, and city are in a single line")]
        public void ThenAddress1AndCityAreInASingleLine()
        {
            var response = _scenarioContext[ScenarioContextTags.RestResponse] as RestResponse;
            var testData = _scenarioContext[ScenarioContextTags.Person] as Person;
            response.Content.Should().Contain($"{testData.Address1} | {testData.City.ToUpper()}");
        }

        [Then(@"a http exception is thrown when '([^']*)' is called")]
        public async Task ThenAHttpExceptionIsThrownWhenIsCalled(string requestUrl)
        {
            var client = new RestClient("http://localhost:8080");
            var request = new RestRequest(requestUrl, Method.Get);
            Func<Task> result = async () => { await client.GetAsync(request); };

            await result.Should().ThrowAsync<HttpRequestException>();
        }

        [Then(@"a (.*) result is returned")]
        public void ThenAResultIsReturned(int statusCode)
        {
            var response = _scenarioContext[ScenarioContextTags.RestResponse] as RestResponse;
            response.StatusCode.Should().Be((HttpStatusCode)statusCode);
        }
    }
}