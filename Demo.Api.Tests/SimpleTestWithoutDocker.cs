using Demo.Api.Controllers;
using Demo.Api.Database;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Demo.Api.Tests
{
    public class SimpleTestWithoutDocker
    {
        private List<Person> _testData;

        public SimpleTestWithoutDocker()
        {
            _testData = new List<Person>
            {
                {
                    new Person {
                        PersonId = 1,
                        LastName = "Cheese",
                        FirstName = "Test",
                        Address1 = "89 Not an Address",
                        Address2 = "AddressLine2",
                        City = "Dingleberry".ToUpper(),
                    }
                }
            };

        }

        [Fact]
        public async Task AddressLine1_AddressLine2_City_ConcatonateUsersAddressLines_ConcatonateSuccess()
        {
            //Arrange
            var mock = new Mock<IDemoDatabase>();
            mock.Setup(p => p.GetIndividualsAddressLines(1)).ReturnsAsync(_testData);
            var sut = new DemoController(mock.Object);

            //Act
            var result = await sut.GetAddress(_testData[0].PersonId) as OkObjectResult;

            //Assert
            result.Value.Should().Be($"{_testData[0].Address1} | {_testData[0].Address2} | {_testData[0].City}");
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task AddressLine1_AddressLine2IEmpty_City_ConcatonateUsersAddressLines_ConcatonateSuccess()
        {
            //Arrange
            _testData[0].Address2 = "";
            var mock = new Mock<IDemoDatabase>();
            mock.Setup(p => p.GetIndividualsAddressLines(_testData[0].PersonId)).ReturnsAsync(_testData);
            var sut = new DemoController(mock.Object);

            //Act
            var result = await sut.GetAddress(_testData[0].PersonId) as OkObjectResult;

            //Assert
            result.Value.Should().Be($"{_testData[0].Address1} | {_testData[0].City}");
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task AddressLine1_AddressLine2IsNull_City_ConcatonateUsersAddressLines_ConcatonateSuccess()
        {
            //Arrange
            _testData[0].Address2 = null;
            var mock = new Mock<IDemoDatabase>();
            mock.Setup(p => p.GetIndividualsAddressLines(_testData[0].PersonId)).ReturnsAsync(_testData);
            var sut = new DemoController(mock.Object);

            //Act
            var result = await sut.GetAddress(_testData[0].PersonId) as OkObjectResult;

            //Assert
            result.Value.Should().Be($"{_testData[0].Address1} | {_testData[0].City}");
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task AddressLine1_AddressLine2_IsNull_City_ConcatonateUsersAddressLines_ConcatonateSuccess()
        {
            //Arrange
            _testData[0].Address2 = " ";
            var mock = new Mock<IDemoDatabase>();
            mock.Setup(p => p.GetIndividualsAddressLines(_testData[0].PersonId)).ReturnsAsync(_testData);
            var sut = new DemoController(mock.Object);

            //Act
            var result = await sut.GetAddress(_testData[0].PersonId) as OkObjectResult;

            //Assert
            result.Value.Should().Be($"{_testData[0].Address1} | {_testData[0].Address2} | {_testData[0].City}");
            result.StatusCode.Should().Be(200);
        }
    }
}
