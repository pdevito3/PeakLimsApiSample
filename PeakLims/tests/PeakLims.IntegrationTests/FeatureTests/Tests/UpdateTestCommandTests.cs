namespace PeakLims.IntegrationTests.FeatureTests.Tests;

using PeakLims.SharedTestHelpers.Fakes.Test;
using SharedKernel.Exceptions;
using PeakLims.Domain.Tests.Features;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using Domain.Tests.Services;
using static TestFixture;

public class UpdateTestCommandTests : TestBase
{
    [Test]
    public async Task can_update_existing_test_in_db()
    {
        // Arrange
        var fakeTestOne = new FakeTestBuilder()
            .WithRepository(GetService<ITestRepository>())
            .Build();
        var updatedTestDto = new FakeTestForUpdateDto().Generate();
        await InsertAsync(fakeTestOne);

        var test = await ExecuteDbContextAsync(db => db.Tests
            .FirstOrDefaultAsync(t => t.Id == fakeTestOne.Id));
        var id = test.Id;

        // Act
        var command = new UpdateTest.Command(id, updatedTestDto);
        await SendAsync(command);
        var updatedTest = await ExecuteDbContextAsync(db => db.Tests.FirstOrDefaultAsync(t => t.Id == id));

        // Assert
        updatedTest?.TestCode.Should().Be(fakeTestOne.TestCode);
        updatedTest?.TestName.Should().Be(updatedTestDto.TestName);
        updatedTest?.Methodology.Should().Be(updatedTestDto.Methodology);
        updatedTest?.Platform.Should().Be(updatedTestDto.Platform);
        updatedTest?.Version.Should().Be(updatedTestDto.Version);
    }
    
    [Test]
    public async Task can_not_update_test_with_same_code_and_version()
    {
        // Arrange
        var fakeTestOne = new FakeTestBuilder()
            .WithRepository(GetService<ITestRepository>())
            .Build();
        await InsertAsync(fakeTestOne);
        var fakeTestTwo = new FakeTestForUpdateDto().Generate();
        fakeTestTwo.Version = fakeTestOne.Version;

        // Act
        var command = new UpdateTest.Command(fakeTestOne.Id, fakeTestTwo);
        var act = () => SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }
}