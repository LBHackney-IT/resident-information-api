using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using ResidentInformationApi.V1.Boundary.Requests;
using ResidentInformationApi.V1.Boundary.Responses;
using ResidentInformationApi.V1.Controllers;
using ResidentInformationApi.V1.Domain;
using ResidentInformationApi.V1.UseCase;
using AcademyClaimantInformation = ResidentInformationApi.V1.Boundary.Responses.AcademyClaimantInformation;

namespace ResidentInformationApi.Tests.V1.Controllers
{
    [TestFixture]
    public class ResidentInformationControllerTests
    {
        private ResidentInformationController _classUnderTest;
        private Mock<IListContactsUseCase> _mockListContactsUseCase;
        private readonly Fixture _fixture = new Fixture();

        [SetUp]
        public void SetUp()
        {
            _mockListContactsUseCase = new Mock<IListContactsUseCase>();
            _classUnderTest = new ResidentInformationController(_mockListContactsUseCase.Object);
        }

        [Test]
        public async Task ListContactsReturnsAResponse()
        {
            var stubbedContactInformation = new ResidentInformationResponse()
            {
                Results = _fixture.Build<ResidentInformationResult>().With(x => x.Data, _fixture.Create<AcademyClaimantInformation>()).CreateMany().ToList()
            };

            var rqp = new ResidentQueryParam()
            {
                FirstName = "ciasom",
                LastName = "tessellate",
                Postcode = "E8 1DY",
                Address = "1 Montage Street"
            };

            _mockListContactsUseCase.Setup(x => x.Execute(rqp)).Returns(Task.FromResult(stubbedContactInformation));
            var response = await _classUnderTest.ListContactsAsync(rqp).ConfigureAwait(true) as OkObjectResult;

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(200);
            response.Value.Should().BeEquivalentTo(stubbedContactInformation);
        }

        [Test]
        public async Task ListContactsReturns400IfNoQueryParametersAreProvided()
        {
            _mockListContactsUseCase.Setup(x => x.Execute(It.IsAny<ResidentQueryParam>()))
                .Throws(new InvalidQueryParameterException("Please provide one or more search terms"));

            var response = await _classUnderTest.ListContactsAsync(new ResidentQueryParam()).ConfigureAwait(true) as BadRequestObjectResult;

            response.Should().NotBeNull();
            response.StatusCode.Should().Be(400);
            response.Value.Should().Be("Please provide one or more search terms");
        }
    }
}
