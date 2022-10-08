namespace PeakLims.IntegrationTests.FeatureTests.AccessionComments;

using PeakLims.Domain.AccessionComments.Dtos;
using PeakLims.SharedTestHelpers.Fakes.AccessionComment;
using SharedKernel.Exceptions;
using PeakLims.Domain.AccessionComments.Features;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;
using static TestFixture;
using PeakLims.SharedTestHelpers.Fakes.Accession;
using PeakLims.SharedTestHelpers.Fakes.AccessionComment;

public class AccessionCommentListQueryTests : TestBase
{
    
    [Test]
    public async Task can_get_accessioncomment_list()
    {
        // Arrange
        var fakeAccessionOne = FakeAccession.Generate(new FakeAccessionForCreationDto().Generate());
        var fakeAccessionTwo = FakeAccession.Generate(new FakeAccessionForCreationDto().Generate());
        await InsertAsync(fakeAccessionOne, fakeAccessionTwo);

        var fakeAccessionCommentParentOne = FakeAccessionComment.Generate(new FakeAccessionCommentForCreationDto().Generate());
        var fakeAccessionCommentParentTwo = FakeAccessionComment.Generate(new FakeAccessionCommentForCreationDto().Generate());
        await InsertAsync(fakeAccessionCommentParentOne, fakeAccessionCommentParentTwo);

        var fakeAccessionCommentOne = FakeAccessionComment.Generate(new FakeAccessionCommentForCreationDto()
            .RuleFor(a => a.AccessionId, _ => fakeAccessionOne.Id)
            .RuleFor(a => a.OriginalCommentId, _ => fakeAccessionCommentParentOne.Id).Generate());
        var fakeAccessionCommentTwo = FakeAccessionComment.Generate(new FakeAccessionCommentForCreationDto()
            .RuleFor(a => a.AccessionId, _ => fakeAccessionTwo.Id)
            .RuleFor(a => a.OriginalCommentId, _ => fakeAccessionCommentParentTwo.Id).Generate());
        var queryParameters = new AccessionCommentParametersDto();

        await InsertAsync(fakeAccessionCommentOne, fakeAccessionCommentTwo);

        // Act
        var query = new GetAccessionCommentList.Query(queryParameters);
        var accessionComments = await SendAsync(query);

        // Assert
        accessionComments.Count.Should().BeGreaterThanOrEqualTo(2);
    }
}