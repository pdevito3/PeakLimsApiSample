namespace PeakLims.IntegrationTests.FeatureTests.Samples;

using PeakLims.SharedTestHelpers.Fakes.Sample;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using PeakLims.Domain.Samples.Features;
using static TestFixture;
using SharedKernel.Exceptions;
using PeakLims.SharedTestHelpers.Fakes.Patient;
using PeakLims.SharedTestHelpers.Fakes.Sample;
using PeakLims.SharedTestHelpers.Fakes.Container;
using Services;

public class AddSampleCommandTests : TestBase
{
    [Test]
    public async Task can_add_new_sample_to_db()
    {
        // Arrange
        var fakePatientOne = FakePatient.Generate(GetService<IDateTimeProvider>());
        await InsertAsync(fakePatientOne);

        var fakeSampleParentOne = FakeSample.Generate(new FakeSampleForCreationDto().Generate());
        await InsertAsync(fakeSampleParentOne);

        var fakeContainerOne = FakeContainer.Generate(new FakeContainerForCreationDto().Generate());
        await InsertAsync(fakeContainerOne);

        var fakeSampleOne = new FakeSampleForCreationDto()
            .RuleFor(s => s.PatientId, _ => fakePatientOne.Id)
            // .RuleFor(s => s.ParentSampleId, _ => fakeSampleParentOne.Id)
            .RuleFor(s => s.ContainerId, _ => fakeContainerOne.Id).Generate();

        // Act
        var command = new AddSample.Command(fakeSampleOne);
        var sampleReturned = await SendAsync(command);
        var sampleCreated = await ExecuteDbContextAsync(db => db.Samples
            .FirstOrDefaultAsync(s => s.Id == sampleReturned.Id));

        // Assert
        sampleReturned.Type.Should().Be(fakeSampleOne.Type);
        sampleReturned.Quantity.Should().Be(fakeSampleOne.Quantity);
        sampleReturned.CollectionDate.Should().Be(fakeSampleOne.CollectionDate);
        sampleReturned.ReceivedDate.Should().Be(fakeSampleOne.ReceivedDate);
        sampleReturned.CollectionSite.Should().Be(fakeSampleOne.CollectionSite);
        sampleReturned.PatientId.Should().Be(fakeSampleOne.PatientId);
        sampleReturned.ParentSampleId.Should().Be(fakeSampleOne.ParentSampleId);
        sampleReturned.ContainerId.Should().Be(fakeSampleOne.ContainerId);

        sampleCreated.Type.Value.Should().Be(fakeSampleOne.Type);
        sampleCreated.Quantity.Should().Be(fakeSampleOne.Quantity);
        sampleCreated.CollectionDate.Should().Be(fakeSampleOne.CollectionDate);
        sampleCreated.ReceivedDate.Should().Be(fakeSampleOne.ReceivedDate);
        sampleCreated.CollectionSite.Should().Be(fakeSampleOne.CollectionSite);
        sampleCreated.PatientId.Should().Be(fakeSampleOne.PatientId);
        sampleCreated.ParentSampleId.Should().Be(fakeSampleOne.ParentSampleId);
        sampleCreated.ContainerId.Should().Be(fakeSampleOne.ContainerId);
    }
}