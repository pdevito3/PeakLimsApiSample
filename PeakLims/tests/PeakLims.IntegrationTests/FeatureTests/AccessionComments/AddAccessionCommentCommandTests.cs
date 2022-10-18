namespace PeakLims.IntegrationTests.FeatureTests.AccessionComments;

using PeakLims.SharedTestHelpers.Fakes.AccessionComment;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using Domain.Accessions;
using PeakLims.Domain.AccessionComments.Features;
using static TestFixture;
using SharedKernel.Exceptions;
using PeakLims.SharedTestHelpers.Fakes.Accession;
using PeakLims.SharedTestHelpers.Fakes.AccessionComment;

public class AddAccessionCommentCommandTests : TestBase
{
    [Test]
    public async Task can_add_new_accessioncomment_to_db()
    {
        // Arrange
        var fakeAccessionOne = Accession.Create();
        await InsertAsync(fakeAccessionOne);

        var fakeAccessionCommentParentOne = FakeAccessionComment.Generate(new FakeAccessionCommentForCreationDto()
            .RuleFor(a => a.AccessionId, _ => fakeAccessionOne.Id)
            .Generate());
        await InsertAsync(fakeAccessionCommentParentOne);

        var fakeAccessionCommentOne = new FakeAccessionCommentForCreationDto()
            .RuleFor(a => a.AccessionId, _ => fakeAccessionOne.Id)
            .RuleFor(a => a.OriginalCommentId, _ => fakeAccessionCommentParentOne.Id).Generate();

        // Act
        var command = new AddAccessionComment.Command(fakeAccessionCommentOne);
        var accessionCommentReturned = await SendAsync(command);
        var accessionCommentCreated = await ExecuteDbContextAsync(db => db.AccessionComments
            .FirstOrDefaultAsync(a => a.Id == accessionCommentReturned.Id));

        // Assert
        accessionCommentReturned.Comment.Should().Be(fakeAccessionCommentOne.Comment);
        accessionCommentReturned.InitialAccessionState.Should().Be(fakeAccessionCommentOne.InitialAccessionState);
        accessionCommentReturned.EndingAccessionState.Should().Be(fakeAccessionCommentOne.EndingAccessionState);
        accessionCommentReturned.AccessionId.Should().Be(fakeAccessionCommentOne.AccessionId);
        accessionCommentReturned.OriginalCommentId.Should().Be(fakeAccessionCommentOne.OriginalCommentId);

        accessionCommentCreated.Comment.Should().Be(fakeAccessionCommentOne.Comment);
        accessionCommentCreated.InitialAccessionState.Should().Be(fakeAccessionCommentOne.InitialAccessionState);
        accessionCommentCreated.EndingAccessionState.Should().Be(fakeAccessionCommentOne.EndingAccessionState);
        accessionCommentCreated.AccessionId.Should().Be(fakeAccessionCommentOne.AccessionId);
        accessionCommentCreated.OriginalCommentId.Should().Be(fakeAccessionCommentOne.OriginalCommentId);
    }
}