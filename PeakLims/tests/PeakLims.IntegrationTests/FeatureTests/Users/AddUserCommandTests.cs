namespace PeakLims.IntegrationTests.FeatureTests.Users;

using PeakLims.SharedTestHelpers.Fakes.User;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using PeakLims.Domain.Users.Features;
using static TestFixture;
using SharedKernel.Exceptions;

public class AddUserCommandTests : TestBase
{
    [Test]
    public async Task can_add_new_user_to_db()
    {
        // Arrange
        var fakeUserOne = new FakeUserForCreationDto().Generate();

        // Act
        var command = new AddUser.Command(fakeUserOne);
        var userReturned = await SendAsync(command);
        var userCreated = await ExecuteDbContextAsync(db => db.Users
            .FirstOrDefaultAsync(u => u.Id == userReturned.Id));

        // Assert
        userReturned.FirstName.Should().Be(fakeUserOne.FirstName);
        userReturned.LastName.Should().Be(fakeUserOne.LastName);
        userReturned.Username.Should().Be(fakeUserOne.Username);
        userReturned.Identifier.Should().Be(fakeUserOne.Identifier);
        userReturned.Email.Should().Be(fakeUserOne.Email);

        userCreated?.FirstName.Should().Be(fakeUserOne.FirstName);
        userCreated?.LastName.Should().Be(fakeUserOne.LastName);
        userCreated?.Username.Should().Be(fakeUserOne.Username);
        userCreated?.Identifier.Should().Be(fakeUserOne.Identifier);
        userCreated?.Email.Value.Should().Be(fakeUserOne.Email);
    }
}