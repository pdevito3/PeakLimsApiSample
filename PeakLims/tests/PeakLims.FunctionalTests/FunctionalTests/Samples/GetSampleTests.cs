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
using Services;

public class GetSampleTests : TestBase
{
    [Test]
    public async Task get_sample_returns_success_when_entity_exists_using_valid_auth_credentials()
    {
        // Arrange
        var container = FakeContainer.Generate();
        var fakePatientOne = FakePatient.Generate(new DateTimeProvider());
        await InsertAsync(fakePatientOne);

        var fakeSample = FakeSample.Generate(new FakeContainerlessSampleForCreationDto()
            .RuleFor(s => s.PatientId, _ => fakePatientOne.Id)
            .Generate(), container);

        var user = await AddNewSuperAdmin();
        FactoryClient.AddAuth(user.Identifier);
        await InsertAsync(fakeSample);

        // Act
        var route = ApiRoutes.Samples.GetRecord.Replace(ApiRoutes.Samples.Id, fakeSample.Id.ToString());
        var result = await FactoryClient.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
            
    [Test]
    public async Task get_sample_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var container = FakeContainer.Generate();
        var fakePatientOne = FakePatient.Generate(new DateTimeProvider());
        await InsertAsync(fakePatientOne);

        var fakeSample = FakeSample.Generate(new FakeContainerlessSampleForCreationDto()
            .RuleFor(s => s.PatientId, _ => fakePatientOne.Id)
            .Generate(), container);

        await InsertAsync(fakeSample);

        // Act
        var route = ApiRoutes.Samples.GetRecord.Replace(ApiRoutes.Samples.Id, fakeSample.Id.ToString());
        var result = await FactoryClient.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task get_sample_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var container = FakeContainer.Generate();
        var fakePatientOne = FakePatient.Generate(new DateTimeProvider());
        await InsertAsync(fakePatientOne);

        var fakeSample = FakeSample.Generate(new FakeContainerlessSampleForCreationDto()
            .RuleFor(s => s.PatientId, _ => fakePatientOne.Id)
            .Generate(), container);
        FactoryClient.AddAuth();

        await InsertAsync(fakeSample);

        // Act
        var route = ApiRoutes.Samples.GetRecord.Replace(ApiRoutes.Samples.Id, fakeSample.Id.ToString());
        var result = await FactoryClient.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}