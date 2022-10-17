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

public class UpdateSampleRecordTests : TestBase
{
    [Test]
    public async Task put_sample_returns_nocontent_when_entity_exists_and_auth_credentials_are_valid()
    {
        // Arrange
        var fakePatientOne = FakePatient.Generate(new FakePatientForCreationDto().Generate());
        await InsertAsync(fakePatientOne);

        var fakeSampleParentOne = FakeSample.Generate(new FakeContainerlessSampleForCreationDto().Generate());
        await InsertAsync(fakeSampleParentOne);

        var fakeContainerOne = FakeContainer.Generate(new FakeContainerForCreationDto().Generate());
        await InsertAsync(fakeContainerOne);

        var fakeSample = FakeSample.Generate(new FakeContainerlessSampleForCreationDto()
            .RuleFor(s => s.PatientId, _ => fakePatientOne.Id)
            .RuleFor(s => s.ParentSampleId, _ => fakeSampleParentOne.Id)
            .RuleFor(s => s.ContainerId, _ => fakeContainerOne.Id).Generate());
        var updatedSampleDto = new FakeSampleForUpdateDto()
            .RuleFor(s => s.PatientId, _ => fakePatientOne.Id)
            .RuleFor(s => s.ParentSampleId, _ => fakeSampleParentOne.Id)
            .RuleFor(s => s.ContainerId, _ => fakeContainerOne.Id).Generate();

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
        var fakeSample = FakeSample.Generate(new FakeContainerlessSampleForCreationDto().Generate());
        var updatedSampleDto = new FakeSampleForUpdateDto { }.Generate();

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
        var fakeSample = FakeSample.Generate(new FakeContainerlessSampleForCreationDto().Generate());
        var updatedSampleDto = new FakeSampleForUpdateDto { }.Generate();
        FactoryClient.AddAuth();

        await InsertAsync(fakeSample);

        // Act
        var route = ApiRoutes.Samples.Put.Replace(ApiRoutes.Samples.Id, fakeSample.Id.ToString());
        var result = await FactoryClient.PutJsonRequestAsync(route, updatedSampleDto);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}