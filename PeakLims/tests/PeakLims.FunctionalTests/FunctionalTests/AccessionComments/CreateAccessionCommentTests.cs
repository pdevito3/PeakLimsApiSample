namespace PeakLims.FunctionalTests.FunctionalTests.AccessionComments;

using PeakLims.FunctionalTests.TestUtilities;
using PeakLims.Domain;
using SharedKernel.Domain;
using PeakLims.SharedTestHelpers.Fakes.Accession;
using FluentAssertions;
using NUnit.Framework;
using System.Net;
using System.Threading.Tasks;
using Bogus;
using Domain.AccessionComments.Dtos;
using Domain.Accessions.Dtos;

public class CreateAccessionCommentTests : TestBase
{
    [Test]
    public async Task create_accessioncomment_returns_created_using_valid_dto_and_valid_auth_credentials()
    {
        // Arrange
        var faker = new Faker();
        var fakeAccession = FakeAccessionBuilder
            .Initialize()
            .WithMockTestRepository()
            .Build();
        await InsertAsync(fakeAccession);

        var user = await AddNewSuperAdmin();
        FactoryClient.AddAuth(user.Identifier);

        // Act
        var route = ApiRoutes.AccessionComments.Create;
        var result = await FactoryClient.PostJsonRequestAsync(route, new AccessionCommentForCreationDto()
        {
            AccessionId = fakeAccession.Id,
            Comment = faker.Lorem.Sentence()
        });

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Created);
    }
            
    [Test]
    public async Task create_accessioncomment_returns_unauthorized_without_valid_token()
    {
        // Arrange
        var fakeAccessionComment = new AccessionCommentForCreationDto();

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
        var fakeAccessionComment = new AccessionCommentForCreationDto();
        FactoryClient.AddAuth();

        // Act
        var route = ApiRoutes.AccessionComments.Create;
        var result = await FactoryClient.PostJsonRequestAsync(route, fakeAccessionComment);

        // Assert
        result.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }
}