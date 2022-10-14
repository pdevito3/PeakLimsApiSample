namespace PeakLims.IntegrationTests.FeatureTests.TestOrders;

using PeakLims.SharedTestHelpers.Fakes.TestOrder;
using PeakLims.Domain.TestOrders.Features;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;
using static TestFixture;
using PeakLims.SharedTestHelpers.Fakes.Test;

public class TestOrderQueryTests : TestBase
{
    [Test]
    public async Task can_get_existing_testorder_with_accurate_props()
    {
        // Arrange
        var fakeTestOne = FakeTest.Generate(new FakeTestForCreationDto().Generate());
        await InsertAsync(fakeTestOne);

        var fakeTestOrderOne = FakeTestOrder.Generate(fakeTestOne.Id);
        await InsertAsync(fakeTestOrderOne);

        // Act
        var query = new GetTestOrder.Query(fakeTestOrderOne.Id);
        var testOrder = await SendAsync(query);

        // Assert
        testOrder.Status.Should().Be(fakeTestOrderOne.Status);
        testOrder.TestId.Should().Be(fakeTestOrderOne.TestId);
    }

    [Test]
    public async Task get_testorder_throws_notfound_exception_when_record_does_not_exist()
    {
        // Arrange
        var badId = Guid.NewGuid();

        // Act
        var query = new GetTestOrder.Query(badId);
        Func<Task> act = () => SendAsync(query);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}