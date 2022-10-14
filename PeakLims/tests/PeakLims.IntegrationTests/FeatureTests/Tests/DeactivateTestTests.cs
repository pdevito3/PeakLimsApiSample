namespace PeakLims.IntegrationTests.FeatureTests.Tests;

using System.Threading.Tasks;
using Domain.Tests.Features;
using Domain.TestStatuses;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using PeakLims.Domain.AccessionStatuses;
using SharedTestHelpers.Fakes.Test;
using static TestFixture;

public class DeactivateTestTests : TestBase
{
    [Test]
    public async Task can_deactivate_test()
    {
        // Arrange
        var fakeTest = FakeTest.Generate();
        await InsertAsync(fakeTest);

        // Act
        var command = new DeactivateTest.Command(fakeTest.Id);
        await SendAsync(command);
        var updatedTest = await ExecuteDbContextAsync(db => db.Tests.FirstOrDefaultAsync(a => a.Id == fakeTest.Id));

        // Assert
        updatedTest.Status.Should().Be(TestStatus.Inactive());
    }
}