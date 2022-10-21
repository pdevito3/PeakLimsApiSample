namespace PeakLims.FunctionalTests.FunctionalTests.AccessionComments;

using PeakLims.FunctionalTests.TestUtilities;
using PeakLims.Domain;
using SharedKernel.Domain;
using PeakLims.SharedTestHelpers.Fakes.Accession;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;
using SharedTestHelpers.Fakes.AccessionComments;

public class GetAccessionCommentTests : TestBase
{
    [Test]
    public async Task get_accessioncomment_returns_success_when_entity_exists_using_valid_auth_credentials()
    {
        // Arrange
        var accessionComment = FakeAccessionCommentBuilder.Initialize()
            .WithMockAccession()
            .Build();
        await InsertAsync(accessionComment);

        var user = await AddNewSuperAdmin();
        FactoryClient.AddAuth(user.Identifier);

        // Act
        var route = ApiRoutes.AccessionComments.GetRecord.Replace(ApiRoutes.AccessionComments.Id, accessionComment.Id.ToString());
        var result = await FactoryClient.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }
            
    [Test]
    public async Task get_accessioncomment_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var accessionComment = FakeAccessionCommentBuilder.Initialize()
            .WithMockAccession()
            .Build();

        // Act
        var route = ApiRoutes.AccessionComments.GetRecord.Replace(ApiRoutes.AccessionComments.Id, accessionComment.Id.ToString());
        var result = await FactoryClient.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task get_accessioncomment_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var accessionComment = FakeAccessionCommentBuilder.Initialize()
            .WithMockAccession()
            .Build();
        FactoryClient.AddAuth();

        // Act
        var route = ApiRoutes.AccessionComments.GetRecord.Replace(ApiRoutes.AccessionComments.Id, accessionComment.Id.ToString());
        var result = await FactoryClient.GetRequestAsync(route);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}