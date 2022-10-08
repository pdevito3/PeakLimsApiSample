namespace PeakLims.FunctionalTests.FunctionalTests.AccessionComments;

using PeakLims.SharedTestHelpers.Fakes.AccessionComment;
using PeakLims.FunctionalTests.TestUtilities;
using PeakLims.Domain;
using SharedKernel.Domain;
using PeakLims.SharedTestHelpers.Fakes.Accession;
using PeakLims.SharedTestHelpers.Fakes.AccessionComment;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;

public class CreateAccessionCommentTests : TestBase
{
    [Test]
    public async Task create_accessioncomment_returns_created_using_valid_dto_and_valid_auth_credentials()
    {
        // Arrange
        var fakeAccessionOne = FakeAccession.Generate(new FakeAccessionForCreationDto().Generate());
        await InsertAsync(fakeAccessionOne);

        var fakeAccessionCommentOne = FakeAccessionComment.Generate(new FakeAccessionCommentForCreationDto().Generate());
        await InsertAsync(fakeAccessionCommentOne);

        var fakeAccessionComment = new FakeAccessionCommentForCreationDto()
            .RuleFor(a => a.AccessionId, _ => fakeAccessionOne.Id)
            
            .RuleFor(a => a.OriginalCommentId, _ => fakeAccessionCommentOne.Id)
            .Generate();

        var user = await AddNewSuperAdmin();
        FactoryClient.AddAuth(user.Identifier);

        // Act
        var route = ApiRoutes.AccessionComments.Create;
        var result = await FactoryClient.PostJsonRequestAsync(route, fakeAccessionComment);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);
    }
            
    [Test]
    public async Task create_accessioncomment_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakeAccessionComment = new FakeAccessionCommentForCreationDto { }.Generate();

        // Act
        var route = ApiRoutes.AccessionComments.Create;
        var result = await FactoryClient.PostJsonRequestAsync(route, fakeAccessionComment);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
            
    [Test]
    public async Task create_accessioncomment_returns_forbidden_without_proper_scope()
    {
        // Arrange
        var fakeAccessionComment = new FakeAccessionCommentForCreationDto { }.Generate();
        FactoryClient.AddAuth();

        // Act
        var route = ApiRoutes.AccessionComments.Create;
        var result = await FactoryClient.PostJsonRequestAsync(route, fakeAccessionComment);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}