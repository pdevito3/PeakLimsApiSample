namespace PeakLims.UnitTests.UnitTests.Domain.Patients.Features;

using PeakLims.SharedTestHelpers.Fakes.Patient;
using PeakLims.Domain.Patients;
using PeakLims.Domain.Patients.Dtos;
using PeakLims.Domain.Patients.Mappings;
using PeakLims.Domain.Patients.Features;
using PeakLims.Domain.Patients.Services;
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

public class GetPatientListTests
{
    
    private readonly SieveProcessor _sieveProcessor;
    private readonly Mapper _mapper = UnitTestUtils.GetApiMapper();
    private readonly Mock<IPatientRepository> _patientRepository;
    private readonly Mock<IHeimGuardClient> _heimGuard;

    public GetPatientListTests()
    {
        _patientRepository = new Mock<IPatientRepository>();
        var sieveOptions = Options.Create(new SieveOptions());
        _sieveProcessor = new SieveProcessor(sieveOptions);
        _heimGuard = new Mock<IHeimGuardClient>();
    }
    
    [Test]
    public async Task can_get_paged_list_of_patient()
    {
        //Arrange
        var fakePatientOne = FakePatient.Generate();
        var fakePatientTwo = FakePatient.Generate();
        var fakePatientThree = FakePatient.Generate();
        var patient = new List<Patient>();
        patient.Add(fakePatientOne);
        patient.Add(fakePatientTwo);
        patient.Add(fakePatientThree);
        var mockDbData = patient.AsQueryable().BuildMock();
        
        var queryParameters = new PatientParametersDto() { PageSize = 1, PageNumber = 2 };

        _patientRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);
        
        //Act
        var query = new GetPatientList.Query(queryParameters);
        var handler = new GetPatientList.Handler(_patientRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
    }

    [Test]
    public async Task can_filter_patient_list_using_FirstName()
    {
        //Arrange
        var fakePatientOne = FakePatient.Generate(new FakePatientForCreationDto()
            .RuleFor(p => p.FirstName, _ => "alpha")
            .Generate());
        var fakePatientTwo = FakePatient.Generate(new FakePatientForCreationDto()
            .RuleFor(p => p.FirstName, _ => "bravo")
            .Generate());
        var queryParameters = new PatientParametersDto() { Filters = $"FirstName == {fakePatientTwo.FirstName}" };

        var patientList = new List<Patient>() { fakePatientOne, fakePatientTwo };
        var mockDbData = patientList.AsQueryable().BuildMock();

        _patientRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetPatientList.Query(queryParameters);
        var handler = new GetPatientList.Handler(_patientRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakePatientTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_filter_patient_list_using_LastName()
    {
        //Arrange
        var fakePatientOne = FakePatient.Generate(new FakePatientForCreationDto()
            .RuleFor(p => p.LastName, _ => "alpha")
            .Generate());
        var fakePatientTwo = FakePatient.Generate(new FakePatientForCreationDto()
            .RuleFor(p => p.LastName, _ => "bravo")
            .Generate());
        var queryParameters = new PatientParametersDto() { Filters = $"LastName == {fakePatientTwo.LastName}" };

        var patientList = new List<Patient>() { fakePatientOne, fakePatientTwo };
        var mockDbData = patientList.AsQueryable().BuildMock();

        _patientRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetPatientList.Query(queryParameters);
        var handler = new GetPatientList.Handler(_patientRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakePatientTwo, options =>
                options.ExcludingMissingMembers());
    }



    [Test]
    public async Task can_filter_patient_list_using_Age()
    {
        //Arrange
        var fakePatientOne = FakePatient.Generate(new FakePatientForCreationDto()
            .RuleFor(p => p.Age, _ => 1)
            .Generate());
        var fakePatientTwo = FakePatient.Generate(new FakePatientForCreationDto()
            .RuleFor(p => p.Age, _ => 2)
            .Generate());
        var queryParameters = new PatientParametersDto() { Filters = $"Age == {fakePatientTwo.Age}" };

        var patientList = new List<Patient>() { fakePatientOne, fakePatientTwo };
        var mockDbData = patientList.AsQueryable().BuildMock();

        _patientRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetPatientList.Query(queryParameters);
        var handler = new GetPatientList.Handler(_patientRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakePatientTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_filter_patient_list_using_Sex()
    {
        //Arrange
        var fakePatientOne = FakePatient.Generate(new FakePatientForCreationDto()
            .RuleFor(p => p.Sex, _ => "alpha")
            .Generate());
        var fakePatientTwo = FakePatient.Generate(new FakePatientForCreationDto()
            .RuleFor(p => p.Sex, _ => "bravo")
            .Generate());
        var queryParameters = new PatientParametersDto() { Filters = $"Sex == {fakePatientTwo.Sex}" };

        var patientList = new List<Patient>() { fakePatientOne, fakePatientTwo };
        var mockDbData = patientList.AsQueryable().BuildMock();

        _patientRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetPatientList.Query(queryParameters);
        var handler = new GetPatientList.Handler(_patientRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakePatientTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_filter_patient_list_using_Race()
    {
        //Arrange
        var fakePatientOne = FakePatient.Generate(new FakePatientForCreationDto()
            .RuleFor(p => p.Race, _ => "alpha")
            .Generate());
        var fakePatientTwo = FakePatient.Generate(new FakePatientForCreationDto()
            .RuleFor(p => p.Race, _ => "bravo")
            .Generate());
        var queryParameters = new PatientParametersDto() { Filters = $"Race == {fakePatientTwo.Race}" };

        var patientList = new List<Patient>() { fakePatientOne, fakePatientTwo };
        var mockDbData = patientList.AsQueryable().BuildMock();

        _patientRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetPatientList.Query(queryParameters);
        var handler = new GetPatientList.Handler(_patientRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakePatientTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_filter_patient_list_using_Ethnicity()
    {
        //Arrange
        var fakePatientOne = FakePatient.Generate(new FakePatientForCreationDto()
            .RuleFor(p => p.Ethnicity, _ => "alpha")
            .Generate());
        var fakePatientTwo = FakePatient.Generate(new FakePatientForCreationDto()
            .RuleFor(p => p.Ethnicity, _ => "bravo")
            .Generate());
        var queryParameters = new PatientParametersDto() { Filters = $"Ethnicity == {fakePatientTwo.Ethnicity}" };

        var patientList = new List<Patient>() { fakePatientOne, fakePatientTwo };
        var mockDbData = patientList.AsQueryable().BuildMock();

        _patientRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetPatientList.Query(queryParameters);
        var handler = new GetPatientList.Handler(_patientRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakePatientTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_filter_patient_list_using_InternalId()
    {
        //Arrange
        var fakePatientOne = FakePatient.Generate(new FakePatientForCreationDto()
            .RuleFor(p => p.InternalId, _ => "alpha")
            .Generate());
        var fakePatientTwo = FakePatient.Generate(new FakePatientForCreationDto()
            .RuleFor(p => p.InternalId, _ => "bravo")
            .Generate());
        var queryParameters = new PatientParametersDto() { Filters = $"InternalId == {fakePatientTwo.InternalId}" };

        var patientList = new List<Patient>() { fakePatientOne, fakePatientTwo };
        var mockDbData = patientList.AsQueryable().BuildMock();

        _patientRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetPatientList.Query(queryParameters);
        var handler = new GetPatientList.Handler(_patientRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakePatientTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_patient_by_FirstName()
    {
        //Arrange
        var fakePatientOne = FakePatient.Generate(new FakePatientForCreationDto()
            .RuleFor(p => p.FirstName, _ => "alpha")
            .Generate());
        var fakePatientTwo = FakePatient.Generate(new FakePatientForCreationDto()
            .RuleFor(p => p.FirstName, _ => "bravo")
            .Generate());
        var queryParameters = new PatientParametersDto() { SortOrder = "-FirstName" };

        var PatientList = new List<Patient>() { fakePatientOne, fakePatientTwo };
        var mockDbData = PatientList.AsQueryable().BuildMock();

        _patientRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetPatientList.Query(queryParameters);
        var handler = new GetPatientList.Handler(_patientRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakePatientTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakePatientOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_patient_by_LastName()
    {
        //Arrange
        var fakePatientOne = FakePatient.Generate(new FakePatientForCreationDto()
            .RuleFor(p => p.LastName, _ => "alpha")
            .Generate());
        var fakePatientTwo = FakePatient.Generate(new FakePatientForCreationDto()
            .RuleFor(p => p.LastName, _ => "bravo")
            .Generate());
        var queryParameters = new PatientParametersDto() { SortOrder = "-LastName" };

        var PatientList = new List<Patient>() { fakePatientOne, fakePatientTwo };
        var mockDbData = PatientList.AsQueryable().BuildMock();

        _patientRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetPatientList.Query(queryParameters);
        var handler = new GetPatientList.Handler(_patientRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakePatientTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakePatientOne, options =>
                options.ExcludingMissingMembers());
    }



    [Test]
    public async Task can_get_sorted_list_of_patient_by_Age()
    {
        //Arrange
        var fakePatientOne = FakePatient.Generate(new FakePatientForCreationDto()
            .RuleFor(p => p.Age, _ => 1)
            .Generate());
        var fakePatientTwo = FakePatient.Generate(new FakePatientForCreationDto()
            .RuleFor(p => p.Age, _ => 2)
            .Generate());
        var queryParameters = new PatientParametersDto() { SortOrder = "-Age" };

        var PatientList = new List<Patient>() { fakePatientOne, fakePatientTwo };
        var mockDbData = PatientList.AsQueryable().BuildMock();

        _patientRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetPatientList.Query(queryParameters);
        var handler = new GetPatientList.Handler(_patientRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakePatientTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakePatientOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_patient_by_Sex()
    {
        //Arrange
        var fakePatientOne = FakePatient.Generate(new FakePatientForCreationDto()
            .RuleFor(p => p.Sex, _ => "alpha")
            .Generate());
        var fakePatientTwo = FakePatient.Generate(new FakePatientForCreationDto()
            .RuleFor(p => p.Sex, _ => "bravo")
            .Generate());
        var queryParameters = new PatientParametersDto() { SortOrder = "-Sex" };

        var PatientList = new List<Patient>() { fakePatientOne, fakePatientTwo };
        var mockDbData = PatientList.AsQueryable().BuildMock();

        _patientRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetPatientList.Query(queryParameters);
        var handler = new GetPatientList.Handler(_patientRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakePatientTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakePatientOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_patient_by_Race()
    {
        //Arrange
        var fakePatientOne = FakePatient.Generate(new FakePatientForCreationDto()
            .RuleFor(p => p.Race, _ => "alpha")
            .Generate());
        var fakePatientTwo = FakePatient.Generate(new FakePatientForCreationDto()
            .RuleFor(p => p.Race, _ => "bravo")
            .Generate());
        var queryParameters = new PatientParametersDto() { SortOrder = "-Race" };

        var PatientList = new List<Patient>() { fakePatientOne, fakePatientTwo };
        var mockDbData = PatientList.AsQueryable().BuildMock();

        _patientRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetPatientList.Query(queryParameters);
        var handler = new GetPatientList.Handler(_patientRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakePatientTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakePatientOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_patient_by_Ethnicity()
    {
        //Arrange
        var fakePatientOne = FakePatient.Generate(new FakePatientForCreationDto()
            .RuleFor(p => p.Ethnicity, _ => "alpha")
            .Generate());
        var fakePatientTwo = FakePatient.Generate(new FakePatientForCreationDto()
            .RuleFor(p => p.Ethnicity, _ => "bravo")
            .Generate());
        var queryParameters = new PatientParametersDto() { SortOrder = "-Ethnicity" };

        var PatientList = new List<Patient>() { fakePatientOne, fakePatientTwo };
        var mockDbData = PatientList.AsQueryable().BuildMock();

        _patientRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetPatientList.Query(queryParameters);
        var handler = new GetPatientList.Handler(_patientRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakePatientTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakePatientOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_patient_by_InternalId()
    {
        //Arrange
        var fakePatientOne = FakePatient.Generate(new FakePatientForCreationDto()
            .RuleFor(p => p.InternalId, _ => "alpha")
            .Generate());
        var fakePatientTwo = FakePatient.Generate(new FakePatientForCreationDto()
            .RuleFor(p => p.InternalId, _ => "bravo")
            .Generate());
        var queryParameters = new PatientParametersDto() { SortOrder = "-InternalId" };

        var PatientList = new List<Patient>() { fakePatientOne, fakePatientTwo };
        var mockDbData = PatientList.AsQueryable().BuildMock();

        _patientRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetPatientList.Query(queryParameters);
        var handler = new GetPatientList.Handler(_patientRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakePatientTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakePatientOne, options =>
                options.ExcludingMissingMembers());
    }
}