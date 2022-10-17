namespace PeakLims.IntegrationTests.FeatureTests.Samples;

using PeakLims.SharedTestHelpers.Fakes.Sample;
using PeakLims.Domain.Samples.Features;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;
using static TestFixture;
using PeakLims.SharedTestHelpers.Fakes.Patient;
using PeakLims.SharedTestHelpers.Fakes.Sample;
using PeakLims.SharedTestHelpers.Fakes.Container;
using Services;

public class SampleQueryTests : TestBase
{
    [Test]
    public async Task can_get_existing_sample_with_accurate_props()
    {
        // Arrange
        var fakePatientOne = FakePatient.Generate(GetService<IDateTimeProvider>());
        await InsertAsync(fakePatientOne);

        var fakeSampleParentOne = FakeSample.Generate(new FakeContainerlessSampleForCreationDto().Generate());
        await InsertAsync(fakeSampleParentOne);

        var fakeContainerOne = FakeContainer.Generate(new FakeContainerForCreationDto().Generate());
        await InsertAsync(fakeContainerOne);

        var fakeSampleOne = FakeSample.Generate(new FakeContainerlessSampleForCreationDto()
            .RuleFor(s => s.PatientId, _ => fakePatientOne.Id)
            .RuleFor(s => s.ParentSampleId, _ => fakeSampleParentOne.Id)
            .Generate());
        await InsertAsync(fakeSampleOne);

        // Act
        var query = new GetSample.Query(fakeSampleOne.Id);
        var sample = await SendAsync(query);

        // Assert
        sample.SampleNumber.Should().Be(fakeSampleOne.SampleNumber);
        sample.Type.Should().Be(fakeSampleOne.Type.Value);
        sample.Quantity.Should().Be(fakeSampleOne.Quantity);
        sample.CollectionDate.Should().Be(fakeSampleOne.CollectionDate);
        sample.ReceivedDate.Should().Be(fakeSampleOne.ReceivedDate);
        sample.CollectionSite.Should().Be(fakeSampleOne.CollectionSite);
        sample.PatientId.Should().Be(fakeSampleOne.PatientId);
        sample.ParentSampleId.Should().Be(fakeSampleOne.ParentSampleId);
        sample.ContainerId.Should().Be(fakeSampleOne.ContainerId);
    }

    [Test]
    public async Task get_sample_throws_notfound_exception_when_record_does_not_exist()
    {
        // Arrange
        var badId = Guid.NewGuid();

        // Act
        var query = new GetSample.Query(badId);
        Func<Task> act = () => SendAsync(query);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}