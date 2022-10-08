namespace PeakLims.IntegrationTests.FeatureTests.Users;

using PeakLims.SharedTestHelpers.Fakes.User;
using PeakLims.Domain.Users.Dtos;
using SharedKernel.Exceptions;
using PeakLims.Domain.Users.Features;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using static TestFixture;

public class UpdateUserCommandTests : TestBase
{
    [Test]
    public async Task can_update_existing_user_in_db()
    {
        // Arrange
        var fakeUserOne = FakeUser.Generate(new FakeUserForCreationDto().Generate());
        var updatedUserDto = new FakeUserForUpdateDto().Generate();
        await InsertAsync(fakeUserOne);

        var user = await ExecuteDbContextAsync(db => db.Users
            .FirstOrDefaultAsync(u => u.Id == fakeUserOne.Id));
        var id = user.Id;

        // Act
        var command = new UpdateUser.Command(id, updatedUserDto);
        await SendAsync(command);
        var updatedUser = await ExecuteDbContextAsync(db => db.Users.FirstOrDefaultAsync(u => u.Id == id));

        // Assert
        updatedUser?.FirstName.Should().Be(updatedUserDto.FirstName);
        updatedUser?.LastName.Should().Be(updatedUserDto.LastName);
        updatedUser?.Username.Should().Be(updatedUserDto.Username);
        updatedUser?.Identifier.Should().Be(updatedUserDto.Identifier);
        updatedUser?.Email.Value.Should().Be(updatedUserDto.Email);
    }
}