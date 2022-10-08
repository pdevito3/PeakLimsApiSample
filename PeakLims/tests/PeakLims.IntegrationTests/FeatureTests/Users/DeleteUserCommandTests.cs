namespace PeakLims.IntegrationTests.FeatureTests.Users;

using PeakLims.SharedTestHelpers.Fakes.User;
using PeakLims.Domain.Users.Features;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;
using static TestFixture;

public class DeleteUserCommandTests : TestBase
{
    [Test]
    public async Task can_delete_user_from_db()
    {
        // Arrange
        var fakeUserOne = FakeUser.Generate(new FakeUserForCreationDto().Generate());
        await InsertAsync(fakeUserOne);
        var user = await ExecuteDbContextAsync(db => db.Users
            .FirstOrDefaultAsync(u => u.Id == fakeUserOne.Id));

        // Act
        var command = new DeleteUser.Command(user.Id);
        await SendAsync(command);
        var userResponse = await ExecuteDbContextAsync(db => db.Users.CountAsync(u => u.Id == user.Id));

        // Assert
        userResponse.Should().Be(0);
    }

    [Test]
    public async Task delete_user_throws_notfoundexception_when_record_does_not_exist()
    {
        // Arrange
        var badId = Guid.NewGuid();

        // Act
        var command = new DeleteUser.Command(badId);
        Func<Task> act = () => SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task can_softdelete_user_from_db()
    {
        // Arrange
        var fakeUserOne = FakeUser.Generate(new FakeUserForCreationDto().Generate());
        await InsertAsync(fakeUserOne);
        var user = await ExecuteDbContextAsync(db => db.Users
            .FirstOrDefaultAsync(u => u.Id == fakeUserOne.Id));

        // Act
        var command = new DeleteUser.Command(user.Id);
        await SendAsync(command);
        var deletedUser = await ExecuteDbContextAsync(db => db.Users
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == user.Id));

        // Assert
        deletedUser?.IsDeleted.Should().BeTrue();
    }
}