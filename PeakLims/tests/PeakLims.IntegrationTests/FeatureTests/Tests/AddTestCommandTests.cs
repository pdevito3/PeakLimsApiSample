namespace PeakLims.IntegrationTests.FeatureTests.Tests;

using PeakLims.SharedTestHelpers.Fakes.Test;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using PeakLims.Domain.Tests.Features;
using static TestFixture;
using SharedKernel.Exceptions;

public class AddTestCommandTests : TestBase
{
    [Test]
    public async Task can_add_new_test_to_db()
    {
        // Arrange
        var fakeTestOne = new FakeTestForCreationDto().Generate();

        // Act
        var command = new AddTest.Command(fakeTestOne);
        var testReturned = await SendAsync(command);
        var testCreated = await ExecuteDbContextAsync(db => db.Tests
            .FirstOrDefaultAsync(t => t.Id == testReturned.Id));

        // Assert
        testReturned.TestCode.Should().Be(fakeTestOne.TestCode);
        testReturned.TestName.Should().Be(fakeTestOne.TestName);
        testReturned.Methodology.Should().Be(fakeTestOne.Methodology);
        testReturned.Platform.Should().Be(fakeTestOne.Platform);
        testReturned.Version.Should().Be(fakeTestOne.Version);
        testReturned.TurnAroundTime.Should().Be(fakeTestOne.TurnAroundTime);
        
        testCreated.TestCode.Should().Be(fakeTestOne.TestCode);
        testCreated.TestName.Should().Be(fakeTestOne.TestName);
        testCreated.Methodology.Should().Be(fakeTestOne.Methodology);
        testCreated.Platform.Should().Be(fakeTestOne.Platform);
        testCreated.Version.Should().Be(fakeTestOne.Version);
        testCreated.TurnAroundTime.Should().Be(fakeTestOne.TurnAroundTime);
    }
    
    [Test]
    public async Task can_not_add_test_with_same_code_and_version()
    {
        // Arrange
        var fakeTestOne = new FakeTestForCreationDto().Generate();
        var fakeTestTwo = new FakeTestForCreationDto().Generate();
        fakeTestTwo.TestCode = fakeTestOne.TestCode;
        fakeTestTwo.Version = fakeTestOne.Version;

        // Act
        var commandOne = new AddTest.Command(fakeTestOne);
        await SendAsync(commandOne);
        var commandTwo = new AddTest.Command(fakeTestTwo);
        var act = () => SendAsync(commandTwo);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }
}