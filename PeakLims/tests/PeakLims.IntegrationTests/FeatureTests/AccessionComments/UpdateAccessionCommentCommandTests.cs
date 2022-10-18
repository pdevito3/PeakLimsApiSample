namespace PeakLims.IntegrationTests.FeatureTests.AccessionComments;

using PeakLims.SharedTestHelpers.Fakes.AccessionComment;
using PeakLims.Domain.AccessionComments.Dtos;
using SharedKernel.Exceptions;
using PeakLims.Domain.AccessionComments.Features;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using Domain.Accessions;
using static TestFixture;
using PeakLims.SharedTestHelpers.Fakes.Accession;
using PeakLims.SharedTestHelpers.Fakes.AccessionComment;

public class UpdateAccessionCommentCommandTests : TestBase
{
    [Test]
    public async Task can_update_existing_accessioncomment_in_db()
    {
        // Arrange
        var fakeAccessionOne = Accession.Create();
        await InsertAsync(fakeAccessionOne);

        var fakeAccessionCommentParentOne = FakeAccessionComment.Generate(new FakeAccessionCommentForCreationDto()
            .RuleFor(a => a.AccessionId, _ => fakeAccessionOne.Id)
            .Generate());
        await InsertAsync(fakeAccessionCommentParentOne);

        var fakeAccessionCommentOne = FakeAccessionComment.Generate(new FakeAccessionCommentForCreationDto()
            .RuleFor(a => a.AccessionId, _ => fakeAccessionOne.Id)
            .RuleFor(a => a.OriginalCommentId, _ => fakeAccessionCommentParentOne.Id).Generate());
        var updatedAccessionCommentDto = new FakeAccessionCommentForUpdateDto()
            .RuleFor(a => a.AccessionId, _ => fakeAccessionOne.Id)
            .RuleFor(a => a.OriginalCommentId, _ => fakeAccessionCommentParentOne.Id).Generate();
        await InsertAsync(fakeAccessionCommentOne);

        var accessionComment = await ExecuteDbContextAsync(db => db.AccessionComments
            .FirstOrDefaultAsync(a => a.Id == fakeAccessionCommentOne.Id));
        var id = accessionComment.Id;

        // Act
        var command = new UpdateAccessionComment.Command(id, updatedAccessionCommentDto);
        await SendAsync(command);
        var updatedAccessionComment = await ExecuteDbContextAsync(db => db.AccessionComments.FirstOrDefaultAsync(a => a.Id == id));

        // Assert
        updatedAccessionComment.Comment.Should().Be(updatedAccessionCommentDto.Comment);
        updatedAccessionComment.InitialAccessionState.Should().Be(updatedAccessionCommentDto.InitialAccessionState);
        updatedAccessionComment.EndingAccessionState.Should().Be(updatedAccessionCommentDto.EndingAccessionState);
        updatedAccessionComment.AccessionId.Should().Be(updatedAccessionCommentDto.AccessionId);
        updatedAccessionComment.OriginalCommentId.Should().Be(updatedAccessionCommentDto.OriginalCommentId);
    }
}