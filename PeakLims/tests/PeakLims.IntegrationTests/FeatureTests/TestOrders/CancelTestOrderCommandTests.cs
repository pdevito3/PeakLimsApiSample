namespace PeakLims.IntegrationTests.FeatureTests.TestOrders;

using PeakLims.SharedTestHelpers.Fakes.TestOrder;
using PeakLims.Domain.TestOrders.Features;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;
using Bogus;
using Domain.TestOrderCancellationReasons;
using Domain.TestOrderStatuses;
using Domain.Tests.Services;
using static TestFixture;
using PeakLims.SharedTestHelpers.Fakes.Test;

public class CancelTestOrderCommandTests : TestBase
{
    private readonly Faker _faker;

    public CancelTestOrderCommandTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public async Task can_get_existing_testorder_with_accurate_props()
    {
        // Arrange
        var fakeTestOne = new FakeTestBuilder()
            .WithRepository(GetService<ITestRepository>())
            .Build();
        await InsertAsync(fakeTestOne);
        var fakeTestOrderOne = FakeTestOrder.Generate(fakeTestOne.Id);
        await InsertAsync(fakeTestOrderOne);
        
        var reason = _faker.PickRandom(TestOrderCancellationReason.ListNames());
        var comments = _faker.Lorem.Sentence(); 

        // Act
        var query = new CancelTestOrder.Command(fakeTestOrderOne.Id, reason, comments);
        await SendAsync(query);
        var testOrder = await ExecuteDbContextAsync(db => db.TestOrders
            .FirstOrDefaultAsync(x => x.Id == fakeTestOrderOne.Id));

        // Assert
        testOrder.Status.Should().Be(TestOrderStatus.Cancelled());
        testOrder.CancellationReason.Value.Should().Be(reason);
        testOrder.CancellationComments.Should().Be(comments);
    }
}