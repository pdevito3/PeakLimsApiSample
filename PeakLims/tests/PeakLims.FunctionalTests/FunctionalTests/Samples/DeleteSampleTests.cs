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

public class DeleteSampleTests : TestBase
{
    [Test]
    public async Task delete_sample_returns_nocontent_when_entity_exists_and_auth_credentials_are_valid()
    {
        // Arrange
        var fakePatientOne = FakePatient.Generate(new FakePatientForCreationDto().Generate());
        await InsertAsync(fakePatientOne);

        var fakeSampleParentOne = FakeSample.Generate(new FakeSampleForCreationDto().Generate());
        await InsertAsync(fakeSampleParentOne);

        var fakeContainerOne = FakeContainer.Generate(new FakeContainerForCreationDto().Generate());
        await InsertAsync(fakeContainerOne);

        var fakeSample = FakeSample.Generate(new FakeSampleForCreationDto()
            .RuleFor(s => s.PatientId, _ => fakePatientOne.Id)
            .RuleFor(s => s.ParentSampleId, _ => fakeSampleParentOne.Id)
            .RuleFor(s => s.ContainerId, _ => fakeContainerOne.Id).Generate());

        var user = await AddNewSuperAdmin();
        FactoryClient.AddAuth(user.Identifier);
        await InsertAsync(fakeSample);

        // Act
        var route = ApiRoutes.Samples.Delete.Replace(ApiRoutes.Samples.Id, fakeSample.Id.ToString());
        var result = await FactoryClient.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
            
    [Test]
    public async Task delete_sample_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakeSample = FakeSample.Generate(new FakeSampleForCreationDto().Generate());

        await InsertAsync(fakeSample);

        // Act
        var route = ApiRoutes.Samples.Delete.Replace(ApiRoutes.Samples.Id, fakeSample.Id.ToString());
        var result = await FactoryClient.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task delete_sample_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakeSample = FakeSample.Generate(new FakeSampleForCreationDto().Generate());
        FactoryClient.AddAuth();

        await InsertAsync(fakeSample);

        // Act
        var route = ApiRoutes.Samples.Delete.Replace(ApiRoutes.Samples.Id, fakeSample.Id.ToString());
        var result = await FactoryClient.DeleteRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}