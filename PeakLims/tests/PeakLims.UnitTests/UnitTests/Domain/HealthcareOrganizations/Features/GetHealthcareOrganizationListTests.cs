namespace PeakLims.UnitTests.UnitTests.Domain.HealthcareOrganizations.Features;

using PeakLims.SharedTestHelpers.Fakes.HealthcareOrganization;
using PeakLims.Domain.HealthcareOrganizations;
using PeakLims.Domain.HealthcareOrganizations.Dtos;
using PeakLims.Domain.HealthcareOrganizations.Mappings;
using PeakLims.Domain.HealthcareOrganizations.Features;
using PeakLims.Domain.HealthcareOrganizations.Services;
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

public class GetHealthcareOrganizationListTests
{
    
    private readonly SieveProcessor _sieveProcessor;
    private readonly Mapper _mapper = UnitTestUtils.GetApiMapper();
    private readonly Mock<IHealthcareOrganizationRepository> _healthcareOrganizationRepository;
    private readonly Mock<IHeimGuardClient> _heimGuard;

    public GetHealthcareOrganizationListTests()
    {
        _healthcareOrganizationRepository = new Mock<IHealthcareOrganizationRepository>();
        var sieveOptions = Options.Create(new SieveOptions());
        _sieveProcessor = new SieveProcessor(sieveOptions);
        _heimGuard = new Mock<IHeimGuardClient>();
    }
    
    [Test]
    public async Task can_get_paged_list_of_healthcareOrganization()
    {
        //Arrange
        var fakeHealthcareOrganizationOne = FakeHealthcareOrganization.Generate();
        var fakeHealthcareOrganizationTwo = FakeHealthcareOrganization.Generate();
        var fakeHealthcareOrganizationThree = FakeHealthcareOrganization.Generate();
        var healthcareOrganization = new List<HealthcareOrganization>();
        healthcareOrganization.Add(fakeHealthcareOrganizationOne);
        healthcareOrganization.Add(fakeHealthcareOrganizationTwo);
        healthcareOrganization.Add(fakeHealthcareOrganizationThree);
        var mockDbData = healthcareOrganization.AsQueryable().BuildMock();
        
        var queryParameters = new HealthcareOrganizationParametersDto() { PageSize = 1, PageNumber = 2 };

        _healthcareOrganizationRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);
        
        //Act
        var query = new GetHealthcareOrganizationList.Query(queryParameters);
        var handler = new GetHealthcareOrganizationList.Handler(_healthcareOrganizationRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
    }

    [Test]
    public async Task can_filter_healthcareorganization_list_using_Name()
    {
        //Arrange
        var fakeHealthcareOrganizationOne = FakeHealthcareOrganization.Generate(new FakeHealthcareOrganizationForCreationDto()
            .RuleFor(h => h.Name, _ => "alpha")
            .Generate());
        var fakeHealthcareOrganizationTwo = FakeHealthcareOrganization.Generate(new FakeHealthcareOrganizationForCreationDto()
            .RuleFor(h => h.Name, _ => "bravo")
            .Generate());
        var queryParameters = new HealthcareOrganizationParametersDto() { Filters = $"Name == {fakeHealthcareOrganizationTwo.Name}" };

        var healthcareOrganizationList = new List<HealthcareOrganization>() { fakeHealthcareOrganizationOne, fakeHealthcareOrganizationTwo };
        var mockDbData = healthcareOrganizationList.AsQueryable().BuildMock();

        _healthcareOrganizationRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetHealthcareOrganizationList.Query(queryParameters);
        var handler = new GetHealthcareOrganizationList.Handler(_healthcareOrganizationRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeHealthcareOrganizationTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_filter_healthcareorganization_list_using_Email()
    {
        //Arrange
        var fakeHealthcareOrganizationOne = FakeHealthcareOrganization.Generate(new FakeHealthcareOrganizationForCreationDto()
            .RuleFor(h => h.Email, _ => "alpha")
            .Generate());
        var fakeHealthcareOrganizationTwo = FakeHealthcareOrganization.Generate(new FakeHealthcareOrganizationForCreationDto()
            .RuleFor(h => h.Email, _ => "bravo")
            .Generate());
        var queryParameters = new HealthcareOrganizationParametersDto() { Filters = $"Email == {fakeHealthcareOrganizationTwo.Email}" };

        var healthcareOrganizationList = new List<HealthcareOrganization>() { fakeHealthcareOrganizationOne, fakeHealthcareOrganizationTwo };
        var mockDbData = healthcareOrganizationList.AsQueryable().BuildMock();

        _healthcareOrganizationRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetHealthcareOrganizationList.Query(queryParameters);
        var handler = new GetHealthcareOrganizationList.Handler(_healthcareOrganizationRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeHealthcareOrganizationTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_healthcareorganization_by_Name()
    {
        //Arrange
        var fakeHealthcareOrganizationOne = FakeHealthcareOrganization.Generate(new FakeHealthcareOrganizationForCreationDto()
            .RuleFor(h => h.Name, _ => "alpha")
            .Generate());
        var fakeHealthcareOrganizationTwo = FakeHealthcareOrganization.Generate(new FakeHealthcareOrganizationForCreationDto()
            .RuleFor(h => h.Name, _ => "bravo")
            .Generate());
        var queryParameters = new HealthcareOrganizationParametersDto() { SortOrder = "-Name" };

        var HealthcareOrganizationList = new List<HealthcareOrganization>() { fakeHealthcareOrganizationOne, fakeHealthcareOrganizationTwo };
        var mockDbData = HealthcareOrganizationList.AsQueryable().BuildMock();

        _healthcareOrganizationRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetHealthcareOrganizationList.Query(queryParameters);
        var handler = new GetHealthcareOrganizationList.Handler(_healthcareOrganizationRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakeHealthcareOrganizationTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeHealthcareOrganizationOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_healthcareorganization_by_Email()
    {
        //Arrange
        var fakeHealthcareOrganizationOne = FakeHealthcareOrganization.Generate(new FakeHealthcareOrganizationForCreationDto()
            .RuleFor(h => h.Email, _ => "alpha")
            .Generate());
        var fakeHealthcareOrganizationTwo = FakeHealthcareOrganization.Generate(new FakeHealthcareOrganizationForCreationDto()
            .RuleFor(h => h.Email, _ => "bravo")
            .Generate());
        var queryParameters = new HealthcareOrganizationParametersDto() { SortOrder = "-Email" };

        var HealthcareOrganizationList = new List<HealthcareOrganization>() { fakeHealthcareOrganizationOne, fakeHealthcareOrganizationTwo };
        var mockDbData = HealthcareOrganizationList.AsQueryable().BuildMock();

        _healthcareOrganizationRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetHealthcareOrganizationList.Query(queryParameters);
        var handler = new GetHealthcareOrganizationList.Handler(_healthcareOrganizationRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakeHealthcareOrganizationTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeHealthcareOrganizationOne, options =>
                options.ExcludingMissingMembers());
    }
}