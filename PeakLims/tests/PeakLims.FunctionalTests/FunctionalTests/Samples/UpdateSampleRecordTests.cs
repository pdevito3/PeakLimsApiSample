namespace PeakLims.FunctionalTests.FunctionalTests.Samples;

using PeakLims.SharedTestHelpers.Fakes.Sample;
using PeakLims.FunctionalTests.TestUtilities;
using PeakLims.Domain;
using SharedKernel.Domain;
using PeakLims.SharedTestHelpers.Fakes.Patient;
using PeakLims.SharedTestHelpers.Fakes.Sample;
using PeakLims.SharedTestHelpers.Fakes.Container;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;
using Domain.Samples.Dtos;
using Services;

public class UpdateSampleRecordTests : TestBase
{
    [Test]
    public async Task put_sample_returns_nocontent_when_entity_exists_and_auth_credentials_are_valid()
    {
        // Arrange
        var container = FakeContainer.Generate();
        var fakePatientOne = FakePatient.Generate(new DateTimeProvider());
        await InsertAsync(fakePatientOne);

        var fakeSample = FakeSample.Generate(new FakeContainerlessSampleForCreationDto()
            .RuleFor(s => s.PatientId, _ => fakePatientOne.Id)
            .Generate(), container);
        var updatedSampleDto = new FakeSampleForUpdateDto(container)
            .RuleFor(s => s.PatientId, _ => fakePatientOne.Id).Generate();

        var user = await AddNewSuperAdmin();
        FactoryClient.AddAuth(user.Identifier);
        await InsertAsync(fakeSample);

        // Act
        var route = ApiRoutes.Samples.Put.Replace(ApiRoutes.Samples.Id, fakeSample.Id.ToString());
        var result = await FactoryClient.PutJsonRequestAsync(route, updatedSampleDto);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
            
    [Test]
    public async Task put_sample_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var container = FakeContainer.Generate();
        var fakePatientOne = FakePatient.Generate(new DateTimeProvider());
        await InsertAsync(fakePatientOne);

        var fakeSample = FakeSample.Generate(new FakeContainerlessSampleForCreationDto()
            .RuleFor(s => s.PatientId, _ => fakePatientOne.Id)
            .Generate(), container);
        var updatedSampleDto = new SampleForUpdateDto();

        await InsertAsync(fakeSample);

        // Act
        var route = ApiRoutes.Samples.Put.Replace(ApiRoutes.Samples.Id, fakeSample.Id.ToString());
        var result = await FactoryClient.PutJsonRequestAsync(route, updatedSampleDto);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task put_sample_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var container = FakeContainer.Generate();
        var fakePatientOne = FakePatient.Generate(new DateTimeProvider());
        await InsertAsync(fakePatientOne);

        var fakeSample = FakeSample.Generate(new FakeContainerlessSampleForCreationDto()
            .RuleFor(s => s.PatientId, _ => fakePatientOne.Id)
            .Generate(), container);
        var updatedSampleDto = new SampleForUpdateDto();
        FactoryClient.AddAuth();

        await InsertAsync(fakeSample);

        // Act
        var route = ApiRoutes.Samples.Put.Replace(ApiRoutes.Samples.Id, fakeSample.Id.ToString());
        var result = await FactoryClient.PutJsonRequestAsync(route, updatedSampleDto);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}