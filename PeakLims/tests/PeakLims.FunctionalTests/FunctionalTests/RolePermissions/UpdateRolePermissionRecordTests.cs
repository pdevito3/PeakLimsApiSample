namespace PeakLims.FunctionalTests.FunctionalTests.RolePermissions;

using PeakLims.SharedTestHelpers.Fakes.RolePermission;
using PeakLims.FunctionalTests.TestUtilities;
using PeakLims.Domain;
using SharedKernel.Domain;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class UpdateRolePermissionRecordTests : TestBase
{
    [Test]
    public async Task put_rolepermission_returns_nocontent_when_entity_exists_and_auth_credentials_are_valid()
    {
        // Arrange
        var fakeRolePermission = FakeRolePermission.Generate(new FakeRolePermissionForCreationDto().Generate());
        var updatedRolePermissionDto = new FakeRolePermissionForUpdateDto().Generate();

        var user = await AddNewSuperAdmin();
        FactoryClient.AddAuth(user.Identifier);
        await InsertAsync(fakeRolePermission);

        // Act
        var route = ApiRoutes.RolePermissions.Put.Replace(ApiRoutes.RolePermissions.Id, fakeRolePermission.Id.ToString());
        var result = await FactoryClient.PutJsonRequestAsync(route, updatedRolePermissionDto);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
            
    [Test]
    public async Task put_rolepermission_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakeRolePermission = FakeRolePermission.Generate(new FakeRolePermissionForCreationDto().Generate());
        var updatedRolePermissionDto = new FakeRolePermissionForUpdateDto { }.Generate();

        await InsertAsync(fakeRolePermission);

        // Act
        var route = ApiRoutes.RolePermissions.Put.Replace(ApiRoutes.RolePermissions.Id, fakeRolePermission.Id.ToString());
        var result = await FactoryClient.PutJsonRequestAsync(route, updatedRolePermissionDto);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task put_rolepermission_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakeRolePermission = FakeRolePermission.Generate(new FakeRolePermissionForCreationDto().Generate());
        var updatedRolePermissionDto = new FakeRolePermissionForUpdateDto { }.Generate();
        FactoryClient.AddAuth();

        await InsertAsync(fakeRolePermission);

        // Act
        var route = ApiRoutes.RolePermissions.Put.Replace(ApiRoutes.RolePermissions.Id, fakeRolePermission.Id.ToString());
        var result = await FactoryClient.PutJsonRequestAsync(route, updatedRolePermissionDto);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}