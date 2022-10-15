namespace PeakLims.IntegrationTests.FeatureTests.Tests;

using System.Threading.Tasks;
using Domain.Tests.Features;
using Domain.Tests.Services;
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
        var fakeTest = new FakeTestBuilder()
            .WithRepository(GetService<ITestRepository>())
            .Build();
        await InsertAsync(fakeTest);

        // Act
        var command = new DeactivateTest.Command(fakeTest.Id);
        await SendAsync(command);
        var updatedTest = await ExecuteDbContextAsync(db => db.Tests.FirstOrDefaultAsync(a => a.Id == fakeTest.Id));

        // Assert
        updatedTest.Status.Should().Be(TestStatus.Inactive());
    }
}