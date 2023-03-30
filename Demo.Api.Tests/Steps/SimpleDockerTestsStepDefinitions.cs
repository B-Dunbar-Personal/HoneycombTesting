using Demo.Api.DatabaseContainer;
using Demo.Api.Tests.Scripts;
using FluentAssertions;
using RestSharp;
using System;
using System.Net;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.CommonModels;

namespace Demo.Api.Tests.Steps
{
    [Binding]
    public class SimpleDockerTestsStepDefinitions
    {
        private ScenarioContext _scenarioContext;
        private readonly SqlContainer _container;

        public SimpleDockerTestsStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _container = scenarioContext[ScenarioContextTags.SqlContainer] as SqlContainer;
        }

        [Given(@"data is seeded with an id of (.*)")]
        public async Task GivenDataIsSeededWithAnIdOf(int personId)
        {
            var testData = _scenarioContext[ScenarioContextTags.Person] as Person;
            testData.PersonId = personId;
            await _container.ExecuteNonQuery(await SqlFileReader.GetSqlFile("SeedPersonTable"), testData);
        }

        [When(@"a request is made to '([^']*)'")]
        public async Task WhenARequestIsMadeTo(string requestUrl)
        {
            var client = new RestClient("http://localhost:8080");
            var request = new RestRequest(requestUrl, Method.Get);
            _scenarioContext[ScenarioContextTags.RestResponse] = await client.GetAsync(request);
        }

        [Then(@"a (.*) is returned and concatonation should be a success")]
        public async Task ThenAIsReturnedAndConcatonationShouldBeASuccess(int statusCode)
        {
            var response = _scenarioContext[ScenarioContextTags.RestResponse] as RestResponse;
            var testData = _scenarioContext[ScenarioContextTags.Person] as Person;
            response.Content.Should().Contain($"{testData.Address1} | {testData.Address2} | {testData.City}");
            response.StatusCode.Should().Be((HttpStatusCode)statusCode);
        }
    }
}
