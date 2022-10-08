namespace PeakLims.UnitTests.Patients.Features;

using Domain.Patients;
using Domain.Patients.Dtos;
using Domain.Patients.Features;
using Domain.Patients.Services;
using FluentAssertions;
using HeimGuard;
using MapsterMapper;
using Microsoft.Extensions.Options;
using MockQueryable.Moq;
using Moq;
using NUnit.Framework;
using SharedTestHelpers.Fakes.Patient;
using Sieve.Models;
using Sieve.Services;
using UnitTests.TestHelpers;

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
}