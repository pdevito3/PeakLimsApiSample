namespace PeakLims.IntegrationTests.FeatureTests.TestOrders;

using PeakLims.SharedTestHelpers.Fakes.TestOrder;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using PeakLims.Domain.TestOrders.Features;
using static TestFixture;
using SharedKernel.Exceptions;
using PeakLims.SharedTestHelpers.Fakes.Test;

public class AddTestOrderCommandTests : TestBase
{
    [Test]
    public async Task can_add_new_testorder_to_db()
    {
        // Arrange
        var fakeTestOne = FakeTest.Generate(new FakeTestForCreationDto().Generate());
        await InsertAsync(fakeTestOne);

        var fakeTestOrderOne = new FakeTestOrderForCreationDto()
            .RuleFor(t => t.TestId, _ => fakeTestOne.Id).Generate();

        // Act
        var command = new AddTestOrder.Command(fakeTestOrderOne);
        var testOrderReturned = await SendAsync(command);
        var testOrderCreated = await ExecuteDbContextAsync(db => db.TestOrders
            .FirstOrDefaultAsync(t => t.Id == testOrderReturned.Id));

        // Assert
        testOrderReturned.State.Should().Be(fakeTestOrderOne.State);
        testOrderReturned.TestId.Should().Be(fakeTestOrderOne.TestId);

        testOrderCreated.State.Should().Be(fakeTestOrderOne.State);
        testOrderCreated.TestId.Should().Be(fakeTestOrderOne.TestId);
    }
}