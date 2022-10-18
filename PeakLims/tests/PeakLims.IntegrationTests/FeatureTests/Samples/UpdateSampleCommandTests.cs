namespace PeakLims.IntegrationTests.FeatureTests.Samples;

using PeakLims.SharedTestHelpers.Fakes.Sample;
using PeakLims.Domain.Samples.Features;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using Domain.Samples;
using PeakLims.SharedTestHelpers.Fakes.Container;
using static TestFixture;

public class UpdateSampleCommandTests : TestBase
{
    [Test]
    public async Task can_update_existing_sample_in_db()
    {
        // Arrange
        var container = FakeContainer.Generate();
        var sampleToCreate = new FakeContainerlessSampleForCreationDto().Generate();
        sampleToCreate.Type = container.UsedFor.Value;
        var sample = Sample.Create(sampleToCreate, container);
        await InsertAsync(sample);
        
        var updatedSampleData = new FakeSampleForUpdateDto(container).Generate();
        updatedSampleData.ContainerId = container.Id;
        updatedSampleData.Type = container.UsedFor.Value;

        // Act
        var command = new UpdateSample.Command(sample.Id, updatedSampleData);
        await SendAsync(command);
        var updatedSample = await ExecuteDbContextAsync(db => db.Samples.FirstOrDefaultAsync(s => s.Id == sample.Id));

        // Assert
        updatedSample.Type.Value.Should().Be(updatedSampleData.Type);
        updatedSample.Quantity.Should().Be(updatedSampleData.Quantity);
        updatedSample.CollectionDate.Should().Be(updatedSampleData.CollectionDate);
        updatedSample.ReceivedDate.Should().Be(updatedSampleData.ReceivedDate);
        updatedSample.CollectionSite.Should().Be(updatedSampleData.CollectionSite);
        updatedSample.PatientId.Should().Be(updatedSampleData.PatientId);
        updatedSample.ParentSampleId.Should().Be(updatedSampleData.ParentSampleId);
        updatedSample.ContainerId.Should().Be(updatedSampleData.ContainerId);
    }
}