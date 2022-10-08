namespace PeakLims.IntegrationTests.FeatureTests.Users;

using PeakLims.Domain.Roles;
using PeakLims.SharedTestHelpers.Fakes.User;
using PeakLims.Domain.Users.Features;
using Bogus;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using static TestFixture;

public class AddRemoveUserRoleTests : TestBase
{    
    [Test]
    public async Task can_add_and_remove_role()
    {
        // Arrange
        var faker = new Faker();
        var fakeUserOne = FakeUser.Generate(new FakeUserForCreationDto().Generate());
        await InsertAsync(fakeUserOne);

        var user = await ExecuteDbContextAsync(db => db.Users
            .FirstOrDefaultAsync(u => u.Id == fakeUserOne.Id));
        var id = user.Id;
        var role = faker.PickRandom<RoleEnum>(RoleEnum.List).Name;

        // Act - Add
        var command = new AddUserRole.Command(id, role);
        await SendAsync(command);
        var updatedUser = await ExecuteDbContextAsync(db => db.Users
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Id == id));

        // Assert - Add
        updatedUser.Roles.Count.Should().Be(1);
        updatedUser.Roles.FirstOrDefault().Role.Value.Should().Be(role);
        
        // Act - Remove
        var removeCommand = new RemoveUserRole.Command(id, role);
        await SendAsync(removeCommand);
        
        // Assert - Remove
        updatedUser = await ExecuteDbContextAsync(db => db.Users
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.Id == id));
        updatedUser.Roles.Count.Should().Be(0);
    }
}