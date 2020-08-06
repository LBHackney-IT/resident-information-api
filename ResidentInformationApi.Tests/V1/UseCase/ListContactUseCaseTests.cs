using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using ResidentInformationApi.V1.Boundary.Requests;
using ResidentInformationApi.V1.Boundary.Responses;
using ResidentInformationApi.V1.Factories;
using ResidentInformationApi.V1.Gateways;
using ResidentInformationApi.V1.UseCase;
using AcademyClaimantInformation = ResidentInformationApi.V1.Domain.AcademyClaimantInformation;
using HousingResidentInformation = ResidentInformationApi.V1.Domain.HousingResidentInformation;
using MosaicResidentInformation = ResidentInformationApi.V1.Domain.MosaicResidentInformation;

namespace ResidentInformationApi.Tests.V1.UseCase
{
    [TestFixture]
    public class ListContactUseCaseTests
    {
        private Mock<IAcademyInformationGateway> _mockAcademyGateway;
        private Mock<IHousingInformationGateway> _mockHousingGateway;
        private Mock<IMosaicInformationGateway> _mockMosaicGateway;
        private ListContactsUseCase _classUnderTest;
        private string _academyUrl;
        private string _housingUrl;
        private string _mosaicUrl;
        private readonly Fixture _fixture = new Fixture();

        [SetUp]
        public void SetUp()
        {
            _mockAcademyGateway = new Mock<IAcademyInformationGateway>();
            _mockHousingGateway = new Mock<IHousingInformationGateway>();
            _mockMosaicGateway = new Mock<IMosaicInformationGateway>();
            _classUnderTest = new ListContactsUseCase(
                _mockAcademyGateway.Object,
                _mockHousingGateway.Object,
                _mockMosaicGateway.Object
                );
            _academyUrl = "http://ACADEMY_API_URL/";
            _housingUrl = "http://HOUSING_API_URL/";
            _mosaicUrl = "http://MOSAIC_API_URL/";

            Environment.SetEnvironmentVariable("ACADEMY_API_URL", _academyUrl);
            Environment.SetEnvironmentVariable("HOUSING_API_URL", _housingUrl);
            Environment.SetEnvironmentVariable("MOSAIC_API_URL", _mosaicUrl);
        }

        [Test]
        public async Task ExecuteReturnsContactInformationResponse()
        {
            var stubbedAcademyClaimant = _fixture.Create<AcademyClaimantInformation>();
            var stubbedHousingResident = _fixture.Create<HousingResidentInformation>();
            var stubbedMosaicResident = _fixture.Create<MosaicResidentInformation>();

            var expectedResult = new List<ResidentInformationResult>();

            expectedResult.Add(new ResidentInformationResult
            {
                System = "Academy",
                SystemId = stubbedAcademyClaimant.ClaimId.ToString(),
                SystemUrl = new Uri(_academyUrl + $"api/v1/claimants/claim/{stubbedAcademyClaimant.ClaimId}/person/{stubbedAcademyClaimant.PersonRef}"),
                Data = stubbedAcademyClaimant.ToResponse()
            });
            expectedResult.Add(new ResidentInformationResult
            {
                System = "Housing",
                SystemId = stubbedHousingResident.HouseReference,
                SystemUrl = new Uri(_housingUrl + $"api/v1/households/{stubbedHousingResident.HouseReference}/people/{stubbedHousingResident.PersonNumber}"),
                Data = stubbedHousingResident.ToResponse()
            });
            expectedResult.Add(new ResidentInformationResult
            {
                System = "Mosaic",
                SystemId = stubbedMosaicResident.MosaicId,
                SystemUrl = new Uri(_mosaicUrl + $"api/v1/residents/{stubbedMosaicResident.MosaicId}"),
                Data = stubbedMosaicResident.ToResponse()
            });

            var rqp = new ResidentQueryParam()
            {
                FirstName = "ciasom",
                LastName = "tessellate",
                Postcode = "E8 1DY",
                Address = "1 Montage Street"
            };

            _mockAcademyGateway.Setup(x =>
                x.GetClaimantInformation(rqp))
                .Returns(Task.FromResult(new List<AcademyClaimantInformation> { stubbedAcademyClaimant }));
            _mockHousingGateway.Setup(x =>
                x.GetResidentInformation(rqp))
                .Returns(Task.FromResult(new List<HousingResidentInformation> { stubbedHousingResident }));
            _mockMosaicGateway.Setup(x =>
                x.GetResidentInformation(rqp))
                .Returns(Task.FromResult(new List<MosaicResidentInformation> { stubbedMosaicResident }));

            var response = await _classUnderTest.Execute(rqp).ConfigureAwait(true);

            response.Should().NotBeNull();
            response.Results.Should().BeEquivalentTo(expectedResult);
        }
    }
}
