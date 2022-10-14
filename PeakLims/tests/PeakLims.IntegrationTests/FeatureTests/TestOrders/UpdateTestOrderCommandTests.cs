namespace PeakLims.IntegrationTests.FeatureTests.TestOrders;

using PeakLims.SharedTestHelpers.Fakes.TestOrder;
using PeakLims.Domain.TestOrders.Dtos;
using SharedKernel.Exceptions;
using PeakLims.Domain.TestOrders.Features;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using static TestFixture;
using PeakLims.SharedTestHelpers.Fakes.Test;

public class UpdateTestOrderCommandTests : TestBase
{
    [Test]
    public async Task can_update_existing_testorder_in_db()
    {
        // Arrange
        var fakeTestOne = FakeTest.Generate(new FakeTestForCreationDto().Generate());
        await InsertAsync(fakeTestOne);

        var fakeTestOrderOne = FakeTestOrder.Generate(new FakeTestOrderForCreationDto()
            .RuleFor(t => t.TestId, _ => fakeTestOne.Id).Generate());
        var updatedTestOrderDto = new FakeTestOrderForUpdateDto()
            .RuleFor(t => t.TestId, _ => fakeTestOne.Id).Generate();
        await InsertAsync(fakeTestOrderOne);

        var testOrder = await ExecuteDbContextAsync(db => db.TestOrders
            .FirstOrDefaultAsync(t => t.Id == fakeTestOrderOne.Id));
        var id = testOrder.Id;

        // Act
        var command = new UpdateTestOrder.Command(id, updatedTestOrderDto);
        await SendAsync(command);
        var updatedTestOrder = await ExecuteDbContextAsync(db => db.TestOrders.FirstOrDefaultAsync(t => t.Id == id));

        // Assert
        updatedTestOrder.Status.Value.Should().Be(fakeTestOrderOne.Status.Value);
        updatedTestOrder.TestId.Should().Be(updatedTestOrderDto.TestId);
    }
}