namespace PeakLims.FunctionalTests.FunctionalTests.RolePermissions;

using PeakLims.SharedTestHelpers.Fakes.RolePermission;
using PeakLims.FunctionalTests.TestUtilities;
using PeakLims.Domain;
using SharedKernel.Domain;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class GetRolePermissionTests : TestBase
{
    [Test]
    public async Task get_rolepermission_returns_success_when_entity_exists_using_valid_auth_credentials()
    {
        // Arrange
        var fakeRolePermission = FakeRolePermission.Generate(new FakeRolePermissionForCreationDto().Generate());

        var user = await AddNewSuperAdmin();
        FactoryClient.AddAuth(user.Identifier);
        await InsertAsync(fakeRolePermission);

        // Act
        var route = ApiRoutes.RolePermissions.GetRecord.Replace(ApiRoutes.RolePermissions.Id, fakeRolePermission.Id.ToString());
        var result = await FactoryClient.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
            
    [Test]
    public async Task get_rolepermission_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakeRolePermission = FakeRolePermission.Generate(new FakeRolePermissionForCreationDto().Generate());

        await InsertAsync(fakeRolePermission);

        // Act
        var route = ApiRoutes.RolePermissions.GetRecord.Replace(ApiRoutes.RolePermissions.Id, fakeRolePermission.Id.ToString());
        var result = await FactoryClient.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task get_rolepermission_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakeRolePermission = FakeRolePermission.Generate(new FakeRolePermissionForCreationDto().Generate());
        FactoryClient.AddAuth();

        await InsertAsync(fakeRolePermission);

        // Act
        var route = ApiRoutes.RolePermissions.GetRecord.Replace(ApiRoutes.RolePermissions.Id, fakeRolePermission.Id.ToString());
        var result = await FactoryClient.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}