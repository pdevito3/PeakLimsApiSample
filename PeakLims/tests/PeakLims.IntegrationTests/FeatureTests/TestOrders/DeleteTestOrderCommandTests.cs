namespace PeakLims.IntegrationTests.FeatureTests.TestOrders;

using PeakLims.SharedTestHelpers.Fakes.TestOrder;
using PeakLims.Domain.TestOrders.Features;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;
using static TestFixture;
using PeakLims.SharedTestHelpers.Fakes.Test;

public class DeleteTestOrderCommandTests : TestBase
{
    [Test]
    public async Task can_delete_testorder_from_db()
    {
        // Arrange
        var fakeTestOne = FakeTest.Generate(new FakeTestForCreationDto().Generate());
        await InsertAsync(fakeTestOne);

        var fakeTestOrderOne = FakeTestOrder.Generate(new FakeTestOrderForCreationDto()
            .RuleFor(t => t.TestId, _ => fakeTestOne.Id).Generate());
        await InsertAsync(fakeTestOrderOne);
        var testOrder = await ExecuteDbContextAsync(db => db.TestOrders
            .FirstOrDefaultAsync(t => t.Id == fakeTestOrderOne.Id));

        // Act
        var command = new DeleteTestOrder.Command(testOrder.Id);
        await SendAsync(command);
        var testOrderResponse = await ExecuteDbContextAsync(db => db.TestOrders.CountAsync(t => t.Id == testOrder.Id));

        // Assert
        testOrderResponse.Should().Be(0);
    }

    [Test]
    public async Task delete_testorder_throws_notfoundexception_when_record_does_not_exist()
    {
        // Arrange
        var badId = Guid.NewGuid();

        // Act
        var command = new DeleteTestOrder.Command(badId);
        Func<Task> act = () => SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task can_softdelete_testorder_from_db()
    {
        // Arrange
        var fakeTestOne = FakeTest.Generate(new FakeTestForCreationDto().Generate());
        await InsertAsync(fakeTestOne);

        var fakeTestOrderOne = FakeTestOrder.Generate(new FakeTestOrderForCreationDto()
            .RuleFor(t => t.TestId, _ => fakeTestOne.Id).Generate());
        await InsertAsync(fakeTestOrderOne);
        var testOrder = await ExecuteDbContextAsync(db => db.TestOrders
            .FirstOrDefaultAsync(t => t.Id == fakeTestOrderOne.Id));

        // Act
        var command = new DeleteTestOrder.Command(testOrder.Id);
        await SendAsync(command);
        var deletedTestOrder = await ExecuteDbContextAsync(db => db.TestOrders
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == testOrder.Id));

        // Assert
        deletedTestOrder?.IsDeleted.Should().BeTrue();
    }
}