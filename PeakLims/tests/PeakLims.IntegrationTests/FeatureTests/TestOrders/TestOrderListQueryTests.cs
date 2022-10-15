namespace PeakLims.IntegrationTests.FeatureTests.TestOrders;

using PeakLims.Domain.TestOrders.Dtos;
using PeakLims.SharedTestHelpers.Fakes.TestOrder;
using SharedKernel.Exceptions;
using PeakLims.Domain.TestOrders.Features;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;
using Domain.Tests.Services;
using static TestFixture;
using PeakLims.SharedTestHelpers.Fakes.Test;

public class TestOrderListQueryTests : TestBase
{
    
    [Test]
    public async Task can_get_testorder_list()
    {
        // Arrange
        var fakeTestOne = new FakeTestBuilder()
            .WithRepository(GetService<ITestRepository>())
            .Build();
        var fakeTestTwo = new FakeTestBuilder()
            .WithRepository(GetService<ITestRepository>())
            .Build();
        await InsertAsync(fakeTestOne, fakeTestTwo);

        var fakeTestOrderOne = FakeTestOrder.Generate(fakeTestOne.Id);
        var fakeTestOrderTwo = FakeTestOrder.Generate(fakeTestTwo.Id);
        var queryParameters = new TestOrderParametersDto();

        await InsertAsync(fakeTestOrderOne, fakeTestOrderTwo);

        // Act
        var query = new GetTestOrderList.Query(queryParameters);
        var testOrders = await SendAsync(query);

        // Assert
        testOrders.Count.Should().BeGreaterThanOrEqualTo(2);
    }
}