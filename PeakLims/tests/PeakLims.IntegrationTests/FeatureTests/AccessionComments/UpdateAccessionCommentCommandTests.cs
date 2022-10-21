namespace PeakLims.IntegrationTests.FeatureTests.AccessionComments;

using PeakLims.Domain.AccessionComments.Dtos;
using SharedKernel.Exceptions;
using PeakLims.Domain.AccessionComments.Features;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using Bogus;
using Domain.AccessionCommentStatuses;
using Domain.Accessions;
using static TestFixture;
using PeakLims.SharedTestHelpers.Fakes.Accession;
using SharedTestHelpers.Fakes.AccessionComments;

public class UpdateAccessionCommentCommandTests : TestBase
{
    [Test]
    public async Task can_update_existing_accessioncomment_in_db()
    {
        // Arrange
        var originalAccessionComment = FakeAccessionCommentBuilder.Initialize()
            .WithMockAccession()
            .Build();
        await InsertAsync(originalAccessionComment);
        
        var faker = new Faker();
        var comment = faker.Lorem.Sentence();

        // Act
        var command = new UpdateAccessionComment.Command(originalAccessionComment.Id, comment);
        await SendAsync(command);
        
        var accessionComments = await ExecuteDbContextAsync(db => db.AccessionComments
            .Where(a => a.AccessionId == originalAccessionComment.AccessionId)
            .ToListAsync());
        var newComment = accessionComments.FirstOrDefault(a => a.Status == AccessionCommentStatus.Active());
        var archivedComment = accessionComments.FirstOrDefault(a => a.Status == AccessionCommentStatus.Archived());

        // Assert
        accessionComments.Count.Should().Be(2);
        
        newComment.AccessionId.Should().Be(originalAccessionComment.AccessionId);
        newComment.Comment.Should().Be(comment);
        newComment.ParentAccessionCommentId.Should().Be(originalAccessionComment.Id);
        newComment.Status.Should().Be(AccessionCommentStatus.Active());
        
        archivedComment.Id.Should().Be(originalAccessionComment.Id);
        archivedComment.AccessionId.Should().Be(originalAccessionComment.AccessionId);
        archivedComment.ParentAccessionCommentId.Should().BeNull();
        archivedComment.Comment.Should().Be(originalAccessionComment.Comment);
        archivedComment.Status.Should().Be(AccessionCommentStatus.Archived());
    }
}