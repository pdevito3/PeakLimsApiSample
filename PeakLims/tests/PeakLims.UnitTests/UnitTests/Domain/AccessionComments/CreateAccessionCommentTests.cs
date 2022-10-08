namespace PeakLims.UnitTests.UnitTests.Domain.AccessionComments;

using PeakLims.SharedTestHelpers.Fakes.AccessionComment;
using PeakLims.Domain.AccessionComments;
using PeakLims.Domain.AccessionComments.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;

[Parallelizable]
public class CreateAccessionCommentTests
{
    private readonly Faker _faker;

    public CreateAccessionCommentTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_create_valid_accessionComment()
    {
        // Arrange + Act
        var accessionCommentToCreate = new FakeAccessionCommentForCreationDto().Generate();
        var fakeAccessionComment = FakeAccessionComment.Generate(accessionCommentToCreate);

        // Assert
        fakeAccessionComment.Comment.Should().Be(accessionCommentToCreate.Comment);
        fakeAccessionComment.InitialAccessionState.Should().Be(accessionCommentToCreate.InitialAccessionState);
        fakeAccessionComment.EndingAccessionState.Should().Be(accessionCommentToCreate.EndingAccessionState);
        fakeAccessionComment.AccessionId.Should().Be(accessionCommentToCreate.AccessionId);
        fakeAccessionComment.OriginalCommentId.Should().Be(accessionCommentToCreate.OriginalCommentId);
    }

    [Test]
    public void queue_domain_event_on_create()
    {
        // Arrange + Act
        var fakeAccessionComment = FakeAccessionComment.Generate();

        // Assert
        fakeAccessionComment.DomainEvents.Count.Should().Be(1);
        fakeAccessionComment.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(AccessionCommentCreated));
    }
}