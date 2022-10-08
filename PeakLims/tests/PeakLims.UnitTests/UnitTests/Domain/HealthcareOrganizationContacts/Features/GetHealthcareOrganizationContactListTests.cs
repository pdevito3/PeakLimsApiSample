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

    [Test]
    public async Task can_filter_healthcareorganizationcontact_list_using_Name()
    {
        //Arrange
        var fakeHealthcareOrganizationContactOne = FakeHealthcareOrganizationContact.Generate(new FakeHealthcareOrganizationContactForCreationDto()
            .RuleFor(h => h.Name, _ => "alpha")
            .Generate());
        var fakeHealthcareOrganizationContactTwo = FakeHealthcareOrganizationContact.Generate(new FakeHealthcareOrganizationContactForCreationDto()
            .RuleFor(h => h.Name, _ => "bravo")
            .Generate());
        var queryParameters = new HealthcareOrganizationContactParametersDto() { Filters = $"Name == {fakeHealthcareOrganizationContactTwo.Name}" };

        var healthcareOrganizationContactList = new List<HealthcareOrganizationContact>() { fakeHealthcareOrganizationContactOne, fakeHealthcareOrganizationContactTwo };
        var mockDbData = healthcareOrganizationContactList.AsQueryable().BuildMock();

        _healthcareOrganizationContactRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetHealthcareOrganizationContactList.Query(queryParameters);
        var handler = new GetHealthcareOrganizationContactList.Handler(_healthcareOrganizationContactRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeHealthcareOrganizationContactTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_filter_healthcareorganizationcontact_list_using_Email()
    {
        //Arrange
        var fakeHealthcareOrganizationContactOne = FakeHealthcareOrganizationContact.Generate(new FakeHealthcareOrganizationContactForCreationDto()
            .RuleFor(h => h.Email, _ => "alpha")
            .Generate());
        var fakeHealthcareOrganizationContactTwo = FakeHealthcareOrganizationContact.Generate(new FakeHealthcareOrganizationContactForCreationDto()
            .RuleFor(h => h.Email, _ => "bravo")
            .Generate());
        var queryParameters = new HealthcareOrganizationContactParametersDto() { Filters = $"Email == {fakeHealthcareOrganizationContactTwo.Email}" };

        var healthcareOrganizationContactList = new List<HealthcareOrganizationContact>() { fakeHealthcareOrganizationContactOne, fakeHealthcareOrganizationContactTwo };
        var mockDbData = healthcareOrganizationContactList.AsQueryable().BuildMock();

        _healthcareOrganizationContactRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetHealthcareOrganizationContactList.Query(queryParameters);
        var handler = new GetHealthcareOrganizationContactList.Handler(_healthcareOrganizationContactRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeHealthcareOrganizationContactTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_filter_healthcareorganizationcontact_list_using_Npi()
    {
        //Arrange
        var fakeHealthcareOrganizationContactOne = FakeHealthcareOrganizationContact.Generate(new FakeHealthcareOrganizationContactForCreationDto()
            .RuleFor(h => h.Npi, _ => "alpha")
            .Generate());
        var fakeHealthcareOrganizationContactTwo = FakeHealthcareOrganizationContact.Generate(new FakeHealthcareOrganizationContactForCreationDto()
            .RuleFor(h => h.Npi, _ => "bravo")
            .Generate());
        var queryParameters = new HealthcareOrganizationContactParametersDto() { Filters = $"Npi == {fakeHealthcareOrganizationContactTwo.Npi}" };

        var healthcareOrganizationContactList = new List<HealthcareOrganizationContact>() { fakeHealthcareOrganizationContactOne, fakeHealthcareOrganizationContactTwo };
        var mockDbData = healthcareOrganizationContactList.AsQueryable().BuildMock();

        _healthcareOrganizationContactRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetHealthcareOrganizationContactList.Query(queryParameters);
        var handler = new GetHealthcareOrganizationContactList.Handler(_healthcareOrganizationContactRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeHealthcareOrganizationContactTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_healthcareorganizationcontact_by_Name()
    {
        //Arrange
        var fakeHealthcareOrganizationContactOne = FakeHealthcareOrganizationContact.Generate(new FakeHealthcareOrganizationContactForCreationDto()
            .RuleFor(h => h.Name, _ => "alpha")
            .Generate());
        var fakeHealthcareOrganizationContactTwo = FakeHealthcareOrganizationContact.Generate(new FakeHealthcareOrganizationContactForCreationDto()
            .RuleFor(h => h.Name, _ => "bravo")
            .Generate());
        var queryParameters = new HealthcareOrganizationContactParametersDto() { SortOrder = "-Name" };

        var HealthcareOrganizationContactList = new List<HealthcareOrganizationContact>() { fakeHealthcareOrganizationContactOne, fakeHealthcareOrganizationContactTwo };
        var mockDbData = HealthcareOrganizationContactList.AsQueryable().BuildMock();

        _healthcareOrganizationContactRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetHealthcareOrganizationContactList.Query(queryParameters);
        var handler = new GetHealthcareOrganizationContactList.Handler(_healthcareOrganizationContactRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakeHealthcareOrganizationContactTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeHealthcareOrganizationContactOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_healthcareorganizationcontact_by_Email()
    {
        //Arrange
        var fakeHealthcareOrganizationContactOne = FakeHealthcareOrganizationContact.Generate(new FakeHealthcareOrganizationContactForCreationDto()
            .RuleFor(h => h.Email, _ => "alpha")
            .Generate());
        var fakeHealthcareOrganizationContactTwo = FakeHealthcareOrganizationContact.Generate(new FakeHealthcareOrganizationContactForCreationDto()
            .RuleFor(h => h.Email, _ => "bravo")
            .Generate());
        var queryParameters = new HealthcareOrganizationContactParametersDto() { SortOrder = "-Email" };

        var HealthcareOrganizationContactList = new List<HealthcareOrganizationContact>() { fakeHealthcareOrganizationContactOne, fakeHealthcareOrganizationContactTwo };
        var mockDbData = HealthcareOrganizationContactList.AsQueryable().BuildMock();

        _healthcareOrganizationContactRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetHealthcareOrganizationContactList.Query(queryParameters);
        var handler = new GetHealthcareOrganizationContactList.Handler(_healthcareOrganizationContactRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakeHealthcareOrganizationContactTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeHealthcareOrganizationContactOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_healthcareorganizationcontact_by_Npi()
    {
        //Arrange
        var fakeHealthcareOrganizationContactOne = FakeHealthcareOrganizationContact.Generate(new FakeHealthcareOrganizationContactForCreationDto()
            .RuleFor(h => h.Npi, _ => "alpha")
            .Generate());
        var fakeHealthcareOrganizationContactTwo = FakeHealthcareOrganizationContact.Generate(new FakeHealthcareOrganizationContactForCreationDto()
            .RuleFor(h => h.Npi, _ => "bravo")
            .Generate());
        var queryParameters = new HealthcareOrganizationContactParametersDto() { SortOrder = "-Npi" };

        var HealthcareOrganizationContactList = new List<HealthcareOrganizationContact>() { fakeHealthcareOrganizationContactOne, fakeHealthcareOrganizationContactTwo };
        var mockDbData = HealthcareOrganizationContactList.AsQueryable().BuildMock();

        _healthcareOrganizationContactRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetHealthcareOrganizationContactList.Query(queryParameters);
        var handler = new GetHealthcareOrganizationContactList.Handler(_healthcareOrganizationContactRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakeHealthcareOrganizationContactTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeHealthcareOrganizationContactOne, options =>
                options.ExcludingMissingMembers());
    }
}