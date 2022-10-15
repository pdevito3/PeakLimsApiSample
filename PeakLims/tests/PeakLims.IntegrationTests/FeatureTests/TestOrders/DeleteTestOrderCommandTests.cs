namespace PeakLims.IntegrationTests.FeatureTests.TestOrders;

using PeakLims.SharedTestHelpers.Fakes.TestOrder;
using PeakLims.Domain.TestOrders.Features;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;
using Domain.Tests.Services;
using static TestFixture;
using PeakLims.SharedTestHelpers.Fakes.Test;

public class DeleteTestOrderCommandTests : TestBase
{
    [Test]
    public async Task can_delete_testorder_from_db()
    {
        // Arrange
        var fakeTestOne = new FakeTestBuilder()
            .WithRepository(GetService<ITestRepository>())
            .Build();
        await InsertAsync(fakeTestOne);

        var fakeTestOrderOne = FakeTestOrder.Generate(fakeTestOne.Id);
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
    public async Task can_softdelete_testorder_from_db()
    {
        // Arrange
        var fakeTestOne = new FakeTestBuilder()
            .WithRepository(GetService<ITestRepository>())
            .Build();
        await InsertAsync(fakeTestOne);

        var fakeTestOrderOne = FakeTestOrder.Generate(fakeTestOne.Id);
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