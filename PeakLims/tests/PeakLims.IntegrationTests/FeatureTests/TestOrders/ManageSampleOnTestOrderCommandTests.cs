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
using SharedTestHelpers.Fakes.Container;
using SharedTestHelpers.Fakes.Sample;

public class ManageSampleOnTestOrderCommandTests : TestBase
{
    private readonly Faker _faker;

    public ManageSampleOnTestOrderCommandTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public async Task can_manage_sample()
    {
        // Arrange
        var container = FakeContainer.Generate();
        var sample = FakeSample.Generate(container);
        await InsertAsync(sample);
        var fakeTestOne = new FakeTestBuilder()
            .WithRepository(GetService<ITestRepository>())
            .Build();
        await InsertAsync(fakeTestOne);
        var fakeTestOrderOne = FakeTestOrder.Generate(fakeTestOne.Id);
        await InsertAsync(fakeTestOrderOne);

        // Act - set
        var command = new SetSampleOnTestOrder.Command(fakeTestOrderOne.Id, sample.Id);
        await SendAsync(command);
        var testOrder = await ExecuteDbContextAsync(db => db.TestOrders
            .FirstOrDefaultAsync(x => x.Id == fakeTestOrderOne.Id));

        // Assert - set
        testOrder.SampleId.Should().Be(sample.Id);

        // Act - remove
        var removeCommand = new RemoveSampleOnTestOrder.Command(fakeTestOrderOne.Id);
        await SendAsync(removeCommand);
        testOrder = await ExecuteDbContextAsync(db => db.TestOrders
            .FirstOrDefaultAsync(x => x.Id == fakeTestOrderOne.Id));

        // Assert - remove
        testOrder.SampleId.Should().BeNull();
    }
}