namespace PeakLims.IntegrationTests.FeatureTests.RolePermissions;

using PeakLims.SharedTestHelpers.Fakes.RolePermission;
using PeakLims.Domain.RolePermissions.Features;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;
using static TestFixture;

public class DeleteRolePermissionCommandTests : TestBase
{
    [Test]
    public async Task can_delete_rolepermission_from_db()
    {
        // Arrange
        var fakeRolePermissionOne = FakeRolePermission.Generate(new FakeRolePermissionForCreationDto().Generate());
        await InsertAsync(fakeRolePermissionOne);
        var rolePermission = await ExecuteDbContextAsync(db => db.RolePermissions
            .FirstOrDefaultAsync(r => r.Id == fakeRolePermissionOne.Id));

        // Act
        var command = new DeleteRolePermission.Command(rolePermission.Id);
        await SendAsync(command);
        var rolePermissionResponse = await ExecuteDbContextAsync(db => db.RolePermissions.CountAsync(r => r.Id == rolePermission.Id));

        // Assert
        rolePermissionResponse.Should().Be(0);
    }

    [Test]
    public async Task delete_rolepermission_throws_notfoundexception_when_record_does_not_exist()
    {
        // Arrange
        var badId = Guid.NewGuid();

        // Act
        var command = new DeleteRolePermission.Command(badId);
        Func<Task> act = () => SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task can_softdelete_rolepermission_from_db()
    {
        // Arrange
        var fakeRolePermissionOne = FakeRolePermission.Generate(new FakeRolePermissionForCreationDto().Generate());
        await InsertAsync(fakeRolePermissionOne);
        var rolePermission = await ExecuteDbContextAsync(db => db.RolePermissions
            .FirstOrDefaultAsync(r => r.Id == fakeRolePermissionOne.Id));

        // Act
        var command = new DeleteRolePermission.Command(rolePermission.Id);
        await SendAsync(command);
        var deletedRolePermission = await ExecuteDbContextAsync(db => db.RolePermissions
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == rolePermission.Id));

        // Assert
        deletedRolePermission?.IsDeleted.Should().BeTrue();
    }
}