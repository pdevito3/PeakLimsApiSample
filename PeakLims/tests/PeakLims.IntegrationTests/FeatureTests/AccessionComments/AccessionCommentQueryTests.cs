namespace PeakLims.IntegrationTests.FeatureTests.AccessionComments;

using PeakLims.SharedTestHelpers.Fakes.AccessionComment;
using PeakLims.Domain.AccessionComments.Features;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharedKernel.Exceptions;
using System.Threading.Tasks;
using Domain.Accessions;
using static TestFixture;
using PeakLims.SharedTestHelpers.Fakes.Accession;
using PeakLims.SharedTestHelpers.Fakes.AccessionComment;

public class AccessionCommentQueryTests : TestBase
{
    [Test]
    public async Task can_get_existing_accessioncomment_with_accurate_props()
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
        await InsertAsync(fakeAccessionCommentOne);

        // Act
        var query = new GetAccessionComment.Query(fakeAccessionCommentOne.Id);
        var accessionComment = await SendAsync(query);

        // Assert
        accessionComment.Comment.Should().Be(fakeAccessionCommentOne.Comment);
        accessionComment.InitialAccessionState.Should().Be(fakeAccessionCommentOne.InitialAccessionState);
        accessionComment.EndingAccessionState.Should().Be(fakeAccessionCommentOne.EndingAccessionState);
        accessionComment.AccessionId.Should().Be(fakeAccessionCommentOne.AccessionId);
        accessionComment.OriginalCommentId.Should().Be(fakeAccessionCommentOne.OriginalCommentId);
    }

    [Test]
    public async Task get_accessioncomment_throws_notfound_exception_when_record_does_not_exist()
    {
        // Arrange
        var badId = Guid.NewGuid();

        // Act
        var query = new GetAccessionComment.Query(badId);
        Func<Task> act = () => SendAsync(query);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>();
    }
}