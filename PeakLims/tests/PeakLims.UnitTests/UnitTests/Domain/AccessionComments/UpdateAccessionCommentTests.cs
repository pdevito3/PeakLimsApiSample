namespace PeakLims.UnitTests.UnitTests.Domain.AccessionComments;

using PeakLims.SharedTestHelpers.Fakes.AccessionComment;
using PeakLims.Domain.AccessionComments;
using PeakLims.Domain.AccessionComments.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;

[Parallelizable]
public class UpdateAccessionCommentTests
{
    private readonly Faker _faker;

    public UpdateAccessionCommentTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_update_accessionComment()
    {
        // Arrange
        var fakeAccessionComment = FakeAccessionComment.Generate();
        var updatedAccessionComment = new FakeAccessionCommentForUpdateDto().Generate();
        
        // Act
        fakeAccessionComment.Update(updatedAccessionComment);

        // Assert
        fakeAccessionComment.Comment.Should().Be(updatedAccessionComment.Comment);
        fakeAccessionComment.InitialAccessionState.Should().Be(updatedAccessionComment.InitialAccessionState);
        fakeAccessionComment.EndingAccessionState.Should().Be(updatedAccessionComment.EndingAccessionState);
        fakeAccessionComment.AccessionId.Should().Be(updatedAccessionComment.AccessionId);
        fakeAccessionComment.OriginalCommentId.Should().Be(updatedAccessionComment.OriginalCommentId);
    }
    
    [Test]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var fakeAccessionComment = FakeAccessionComment.Generate();
        var updatedAccessionComment = new FakeAccessionCommentForUpdateDto().Generate();
        fakeAccessionComment.DomainEvents.Clear();
        
        // Act
        fakeAccessionComment.Update(updatedAccessionComment);

        // Assert
        fakeAccessionComment.DomainEvents.Count.Should().Be(1);
        fakeAccessionComment.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(AccessionCommentUpdated));
    }
}