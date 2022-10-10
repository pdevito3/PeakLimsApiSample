namespace PeakLims.IntegrationTests.FeatureTests.Tests;

using PeakLims.SharedTestHelpers.Fakes.Test;
using PeakLims.Domain.Tests.Dtos;
using SharedKernel.Exceptions;
using PeakLims.Domain.Tests.Features;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using static TestFixture;

public class UpdateTestCommandTests : TestBase
{
    [Test]
    public async Task can_update_existing_test_in_db()
    {
        // Arrange
        var fakeTestOne = FakeTest.Generate(new FakeTestForCreationDto().Generate());
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
        updatedTest.TestCode.Should().Be(updatedTestDto.TestCode);
        updatedTest.TestName.Should().Be(updatedTestDto.TestName);
        updatedTest.Methodology.Should().Be(updatedTestDto.Methodology);
        updatedTest.Platform.Should().Be(updatedTestDto.Platform);
        updatedTest.Version.Should().Be(updatedTestDto.Version);
    }
}