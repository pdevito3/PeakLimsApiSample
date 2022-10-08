namespace PeakLims.IntegrationTests.FeatureTests.Tests;

using PeakLims.SharedTestHelpers.Fakes.Test;
using PeakLims.Domain.Tests.Features;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;
using static TestFixture;

public class DeleteTestCommandTests : TestBase
{
    [Test]
    public async Task can_delete_test_from_db()
    {
        // Arrange
        var fakeTestOne = FakeTest.Generate(new FakeTestForCreationDto().Generate());
        await InsertAsync(fakeTestOne);
        var test = await ExecuteDbContextAsync(db => db.Tests
            .FirstOrDefaultAsync(t => t.Id == fakeTestOne.Id));

        // Act
        var command = new DeleteTest.Command(test.Id);
        await SendAsync(command);
        var testResponse = await ExecuteDbContextAsync(db => db.Tests.CountAsync(t => t.Id == test.Id));

        // Assert
        testResponse.Should().Be(0);
    }

    [Test]
    public async Task delete_test_throws_notfoundexception_when_record_does_not_exist()
    {
        // Arrange
        var badId = Guid.NewGuid();

        // Act
        var command = new DeleteTest.Command(badId);
        Func<Task> act = () => SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task can_softdelete_test_from_db()
    {
        // Arrange
        var fakeTestOne = FakeTest.Generate(new FakeTestForCreationDto().Generate());
        await InsertAsync(fakeTestOne);
        var test = await ExecuteDbContextAsync(db => db.Tests
            .FirstOrDefaultAsync(t => t.Id == fakeTestOne.Id));

        // Act
        var command = new DeleteTest.Command(test.Id);
        await SendAsync(command);
        var deletedTest = await ExecuteDbContextAsync(db => db.Tests
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == test.Id));

        // Assert
        deletedTest?.IsDeleted.Should().BeTrue();
    }
}