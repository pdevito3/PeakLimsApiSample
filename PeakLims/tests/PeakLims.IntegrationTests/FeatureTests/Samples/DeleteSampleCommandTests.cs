namespace PeakLims.IntegrationTests.FeatureTests.Samples;

using PeakLims.SharedTestHelpers.Fakes.Sample;
using PeakLims.Domain.Samples.Features;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;
using static TestFixture;
using PeakLims.SharedTestHelpers.Fakes.Patient;
using PeakLims.SharedTestHelpers.Fakes.Sample;
using PeakLims.SharedTestHelpers.Fakes.Container;
using Services;

public class DeleteSampleCommandTests : TestBase
{
    [Test]
    public async Task can_delete_sample_from_db()
    {
        // Arrange
        var fakePatientOne = FakePatient.Generate(GetService<IDateTimeProvider>());
        await InsertAsync(fakePatientOne);

        var fakeSampleParentOne = FakeSample.Generate();
        await InsertAsync(fakeSampleParentOne);

        var fakeSampleOne = FakeSample.Generate(new FakeContainerlessSampleForCreationDto()
            .RuleFor(s => s.PatientId, _ => fakePatientOne.Id)
            .RuleFor(s => s.ParentSampleId, _ => fakeSampleParentOne.Id)
            .Generate());
        await InsertAsync(fakeSampleOne);
        var sample = await ExecuteDbContextAsync(db => db.Samples
            .FirstOrDefaultAsync(s => s.Id == fakeSampleOne.Id));

        // Act
        var command = new DeleteSample.Command(sample.Id);
        await SendAsync(command);
        var sampleResponse = await ExecuteDbContextAsync(db => db.Samples.CountAsync(s => s.Id == sample.Id));

        // Assert
        sampleResponse.Should().Be(0);
    }

    [Test]
    public async Task delete_sample_throws_notfoundexception_when_record_does_not_exist()
    {
        // Arrange
        var badId = Guid.NewGuid();

        // Act
        var command = new DeleteSample.Command(badId);
        Func<Task> act = () => SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task can_softdelete_sample_from_db()
    {
        // Arrange
        var fakePatientOne = FakePatient.Generate(GetService<IDateTimeProvider>());
        await InsertAsync(fakePatientOne);

        var fakeSampleParentOne = FakeSample.Generate();
        await InsertAsync(fakeSampleParentOne);

        var fakeContainerOne = FakeContainer.Generate(new FakeContainerForCreationDto().Generate());
        await InsertAsync(fakeContainerOne);

        var fakeSampleOne = FakeSample.Generate(new FakeContainerlessSampleForCreationDto()
            .RuleFor(s => s.PatientId, _ => fakePatientOne.Id)
            .RuleFor(s => s.ParentSampleId, _ => fakeSampleParentOne.Id)
            .Generate());
        await InsertAsync(fakeSampleOne);
        var sample = await ExecuteDbContextAsync(db => db.Samples
            .FirstOrDefaultAsync(s => s.Id == fakeSampleOne.Id));

        // Act
        var command = new DeleteSample.Command(sample.Id);
        await SendAsync(command);
        var deletedSample = await ExecuteDbContextAsync(db => db.Samples
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == sample.Id));

        // Assert
        deletedSample?.IsDeleted.Should().BeTrue();
    }
}