namespace PeakLims.IntegrationTests.FeatureTests.AccessionComments;

using PeakLims.SharedTestHelpers.Fakes.AccessionComment;
using PeakLims.Domain.AccessionComments.Features;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;
using static TestFixture;
using PeakLims.SharedTestHelpers.Fakes.Accession;
using PeakLims.SharedTestHelpers.Fakes.AccessionComment;

public class DeleteAccessionCommentCommandTests : TestBase
{
    [Test]
    public async Task can_delete_accessioncomment_from_db()
    {
        // Arrange
        var fakeAccessionOne = FakeAccession.Generate(new FakeAccessionForCreationDto().Generate());
        await InsertAsync(fakeAccessionOne);

        var fakeAccessionCommentParentOne = FakeAccessionComment.Generate(new FakeAccessionCommentForCreationDto()
            .RuleFor(a => a.AccessionId, _ => fakeAccessionOne.Id)
            .Generate());
        await InsertAsync(fakeAccessionCommentParentOne);

        var fakeAccessionCommentOne = FakeAccessionComment.Generate(new FakeAccessionCommentForCreationDto()
            .RuleFor(a => a.AccessionId, _ => fakeAccessionOne.Id)
            .RuleFor(a => a.OriginalCommentId, _ => fakeAccessionCommentParentOne.Id).Generate());
        await InsertAsync(fakeAccessionCommentOne);
        var accessionComment = await ExecuteDbContextAsync(db => db.AccessionComments
            .FirstOrDefaultAsync(a => a.Id == fakeAccessionCommentOne.Id));

        // Act
        var command = new DeleteAccessionComment.Command(accessionComment.Id);
        await SendAsync(command);
        var accessionCommentResponse = await ExecuteDbContextAsync(db => db.AccessionComments.CountAsync(a => a.Id == accessionComment.Id));

        // Assert
        accessionCommentResponse.Should().Be(0);
    }

    [Test]
    public async Task delete_accessioncomment_throws_notfoundexception_when_record_does_not_exist()
    {
        // Arrange
        var badId = Guid.NewGuid();

        // Act
        var command = new DeleteAccessionComment.Command(badId);
        Func<Task> act = () => SendAsync(command);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task can_softdelete_accessioncomment_from_db()
    {
        // Arrange
        var fakeAccessionOne = FakeAccession.Generate(new FakeAccessionForCreationDto().Generate());
        await InsertAsync(fakeAccessionOne);

        var fakeAccessionCommentParentOne = FakeAccessionComment.Generate(new FakeAccessionCommentForCreationDto()
            .RuleFor(a => a.AccessionId, _ => fakeAccessionOne.Id)
            .Generate());
        await InsertAsync(fakeAccessionCommentParentOne);

        var fakeAccessionCommentOne = FakeAccessionComment.Generate(new FakeAccessionCommentForCreationDto()
            .RuleFor(a => a.AccessionId, _ => fakeAccessionOne.Id)
            .RuleFor(a => a.OriginalCommentId, _ => fakeAccessionCommentParentOne.Id).Generate());
        await InsertAsync(fakeAccessionCommentOne);
        var accessionComment = await ExecuteDbContextAsync(db => db.AccessionComments
            .FirstOrDefaultAsync(a => a.Id == fakeAccessionCommentOne.Id));

        // Act
        var command = new DeleteAccessionComment.Command(accessionComment.Id);
        await SendAsync(command);
        var deletedAccessionComment = await ExecuteDbContextAsync(db => db.AccessionComments
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(x => x.Id == accessionComment.Id));

        // Assert
        deletedAccessionComment?.IsDeleted.Should().BeTrue();
    }
}