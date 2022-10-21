namespace PeakLims.UnitTests.UnitTests.Domain.AccessionComments;

using PeakLims.Domain.AccessionComments;
using PeakLims.Domain.AccessionComments.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using NUnit.Framework;
using PeakLims.Domain.AccessionCommentStatuses;
using SharedTestHelpers.Fakes.Accession;
using ValidationException = SharedKernel.Exceptions.ValidationException;

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
        // Arrange
        var accession = FakeAccessionBuilder
            .Initialize()
            .WithMockTestRepository()
            .Build();
        var comment = _faker.Lorem.Sentence();
        
        // Act
        var accessionComment = AccessionComment.Create(accession, comment);

        // Assert
        accessionComment.Comment.Should().Be(comment);
        accessionComment.AccessionId.Should().Be(accession.Id);
        accessionComment.ParentAccessionComment.Should().BeNull();
        accessionComment.ParentAccessionCommentId.Should().BeNull();
        accessionComment.Status.Should().Be(AccessionCommentStatus.Active());
    }
    
    [Test]
    public void must_have_a_comment()
    {
        // Arrange
        var accession = FakeAccessionBuilder
            .Initialize()
            .WithMockTestRepository()
            .Build();
        
        // Act
        var act = () => AccessionComment.Create(accession, null);

        // Assert
        act.Should().Throw<ValidationException>()
            .WithMessage("Please provide a valid comment.");
    }

    [Test]
    public void queue_domain_event_on_create()
    {
        // Arrange
        var accession = FakeAccessionBuilder
            .Initialize()
            .WithMockTestRepository()
            .Build();
        var comment = _faker.Lorem.Sentence();
        
        // Act
        var accessionComment = AccessionComment.Create(accession, comment);

        // Assert
        accessionComment.DomainEvents.Count.Should().Be(1);
        accessionComment.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(AccessionCommentCreated));
    }
}