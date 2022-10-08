namespace PeakLims.UnitTests.UnitTests.Domain.HealthcareOrganizationContacts.Features;

using PeakLims.SharedTestHelpers.Fakes.HealthcareOrganizationContact;
using PeakLims.Domain.HealthcareOrganizationContacts;
using PeakLims.Domain.HealthcareOrganizationContacts.Dtos;
using PeakLims.Domain.HealthcareOrganizationContacts.Mappings;
using PeakLims.Domain.HealthcareOrganizationContacts.Features;
using PeakLims.Domain.HealthcareOrganizationContacts.Services;
using MapsterMapper;
using FluentAssertions;
using HeimGuard;
using Microsoft.Extensions.Options;
using MockQueryable.Moq;
using Moq;
using Sieve.Models;
using Sieve.Services;
using TestHelpers;
using NUnit.Framework;

public class GetHealthcareOrganizationContactListTests
{
    
    private readonly SieveProcessor _sieveProcessor;
    private readonly Mapper _mapper = UnitTestUtils.GetApiMapper();
    private readonly Mock<IHealthcareOrganizationContactRepository> _healthcareOrganizationContactRepository;
    private readonly Mock<IHeimGuardClient> _heimGuard;

    public GetHealthcareOrganizationContactListTests()
    {
        _healthcareOrganizationContactRepository = new Mock<IHealthcareOrganizationContactRepository>();
        var sieveOptions = Options.Create(new SieveOptions());
        _sieveProcessor = new SieveProcessor(sieveOptions);
        _heimGuard = new Mock<IHeimGuardClient>();
    }
    
    [Test]
    public async Task can_get_paged_list_of_healthcareOrganizationContact()
    {
        //Arrange
        var fakeHealthcareOrganizationContactOne = FakeHealthcareOrganizationContact.Generate();
        var fakeHealthcareOrganizationContactTwo = FakeHealthcareOrganizationContact.Generate();
        var fakeHealthcareOrganizationContactThree = FakeHealthcareOrganizationContact.Generate();
        var healthcareOrganizationContact = new List<HealthcareOrganizationContact>();
        healthcareOrganizationContact.Add(fakeHealthcareOrganizationContactOne);
        healthcareOrganizationContact.Add(fakeHealthcareOrganizationContactTwo);
        healthcareOrganizationContact.Add(fakeHealthcareOrganizationContactThree);
        var mockDbData = healthcareOrganizationContact.AsQueryable().BuildMock();
        
        var queryParameters = new HealthcareOrganizationContactParametersDto() { PageSize = 1, PageNumber = 2 };

        _healthcareOrganizationContactRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);
        
        //Act
        var query = new GetHealthcareOrganizationContactList.Query(queryParameters);
        var handler = new GetHealthcareOrganizationContactList.Handler(_healthcareOrganizationContactRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
    }
}