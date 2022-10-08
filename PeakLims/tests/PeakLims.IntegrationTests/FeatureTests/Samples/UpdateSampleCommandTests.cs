namespace PeakLims.IntegrationTests.FeatureTests.Samples;

using PeakLims.SharedTestHelpers.Fakes.Sample;
using PeakLims.Domain.Samples.Dtos;
using SharedKernel.Exceptions;
using PeakLims.Domain.Samples.Features;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using static TestFixture;
using PeakLims.SharedTestHelpers.Fakes.Patient;
using PeakLims.SharedTestHelpers.Fakes.Sample;
using PeakLims.SharedTestHelpers.Fakes.Container;

public class UpdateSampleCommandTests : TestBase
{
    [Test]
    public async Task can_update_existing_sample_in_db()
    {
        // Arrange
        var fakePatientOne = FakePatient.Generate(new FakePatientForCreationDto().Generate());
        await InsertAsync(fakePatientOne);

        var fakeSampleParentOne = FakeSample.Generate(new FakeSampleForCreationDto().Generate());
        await InsertAsync(fakeSampleParentOne);

        var fakeContainerOne = FakeContainer.Generate(new FakeContainerForCreationDto().Generate());
        await InsertAsync(fakeContainerOne);

        var fakeSampleOne = FakeSample.Generate(new FakeSampleForCreationDto()
            .RuleFor(s => s.PatientId, _ => fakePatientOne.Id)
            .RuleFor(s => s.ParentSampleId, _ => fakeSampleParentOne.Id)
            .RuleFor(s => s.ContainerId, _ => fakeContainerOne.Id).Generate());
        var updatedSampleDto = new FakeSampleForUpdateDto()
            .RuleFor(s => s.PatientId, _ => fakePatientOne.Id)
            .RuleFor(s => s.ParentSampleId, _ => fakeSampleParentOne.Id)
            .RuleFor(s => s.ContainerId, _ => fakeContainerOne.Id).Generate();
        await InsertAsync(fakeSampleOne);

        var sample = await ExecuteDbContextAsync(db => db.Samples
            .FirstOrDefaultAsync(s => s.Id == fakeSampleOne.Id));
        var id = sample.Id;

        // Act
        var command = new UpdateSample.Command(id, updatedSampleDto);
        await SendAsync(command);
        var updatedSample = await ExecuteDbContextAsync(db => db.Samples.FirstOrDefaultAsync(s => s.Id == id));

        // Assert
        updatedSample.SampleNumber.Should().Be(updatedSampleDto.SampleNumber);
        updatedSample.Status.Should().Be(updatedSampleDto.Status);
        updatedSample.Type.Should().Be(updatedSampleDto.Type);
        updatedSample.Quantity.Should().Be(updatedSampleDto.Quantity);
        updatedSample.CollectionDate.Should().Be(updatedSampleDto.CollectionDate);
        updatedSample.ReceivedDate.Should().Be(updatedSampleDto.ReceivedDate);
        updatedSample.CollectionSite.Should().Be(updatedSampleDto.CollectionSite);
        updatedSample.PatientId.Should().Be(updatedSampleDto.PatientId);
        updatedSample.ParentSampleId.Should().Be(updatedSampleDto.ParentSampleId);
        updatedSample.ContainerId.Should().Be(updatedSampleDto.ContainerId);
    }
}