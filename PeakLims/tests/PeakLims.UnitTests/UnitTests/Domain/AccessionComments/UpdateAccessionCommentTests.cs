namespace PeakLims.UnitTests.UnitTests.Domain.AccessionComments;

using PeakLims.Domain.AccessionComments;
using PeakLims.Domain.AccessionComments.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;
using PeakLims.Domain.AccessionCommentStatuses;
using SharedTestHelpers.Fakes.AccessionComments;
using ValidationException = SharedKernel.Exceptions.ValidationException;

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
        var originalAccessionComment = FakeAccessionCommentBuilder.Initialize()
            .WithMockAccession()
            .Build();
        var comment = _faker.Lorem.Sentence();
        
        // Act
        originalAccessionComment.Update(comment, out var newComment, out var archivedComment);

        // Assert
        newComment.AccessionId.Should().Be(originalAccessionComment.AccessionId);
        newComment.Comment.Should().Be(comment);
        newComment.ParentAccessionCommentId.Should().BeNull();
        newComment.Status.Should().Be(AccessionCommentStatus.Active());
        
        archivedComment.Id.Should().Be(originalAccessionComment.Id);
        archivedComment.AccessionId.Should().Be(originalAccessionComment.AccessionId);
        archivedComment.ParentAccessionCommentId.Should().Be(newComment.Id);
        archivedComment.Comment.Should().Be(originalAccessionComment.Comment);
        archivedComment.Status.Should().Be(AccessionCommentStatus.Archived());
    }
    
    [Test]
    public void must_have_a_comment()
    {
        // Arrange
        var originalAccessionComment = FakeAccessionCommentBuilder.Initialize()
            .WithMockAccession()
            .Build();
        
        // Act
        var act = () => originalAccessionComment.Update( null, out _ , out _);

        // Assert
        act.Should().Throw<ValidationException>()
            .WithMessage("Please provide a valid comment.");
    }
    
    [Test]
    public void queue_domain_event_on_update()
    {
        // Arrange
        var originalAccessionComment = FakeAccessionCommentBuilder.Initialize()
            .WithMockAccession()
            .Build();
        var comment = _faker.Lorem.Sentence();
        originalAccessionComment.DomainEvents.Clear();
        
        // Act
        originalAccessionComment.Update(comment, out var newComment, out var archivedComment);

        // Assert
        archivedComment.DomainEvents.Count.Should().Be(1);
        archivedComment.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(AccessionCommentUpdated));
        
        newComment.DomainEvents.Count.Should().Be(1);
        newComment.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(AccessionCommentCreated));
    }
}