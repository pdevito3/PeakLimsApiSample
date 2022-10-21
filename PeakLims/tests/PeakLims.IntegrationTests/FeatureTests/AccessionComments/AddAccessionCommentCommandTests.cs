namespace PeakLims.IntegrationTests.FeatureTests.AccessionComments;

using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Threading.Tasks;
using Bogus;
using Domain.AccessionCommentStatuses;
using Domain.Accessions;
using PeakLims.Domain.AccessionComments.Features;
using static TestFixture;

public class AddAccessionCommentCommandTests : TestBase
{
    [Test]
    public async Task can_add_new_accessioncomment_to_db()
    {
        // Arrange
        var accession = Accession.Create();
        await InsertAsync(accession);
        
        var faker = new Faker();
        var comment = faker.Lorem.Sentence();

        // Act
        var command = new AddAccessionComment.Command(accession.Id, comment);
        var accessionCommentReturned = await SendAsync(command);
        var accessionCommentCreated = await ExecuteDbContextAsync(db => db.AccessionComments
            .FirstOrDefaultAsync(a => a.Id == accessionCommentReturned.Id));

        // Assert
        accessionCommentReturned.Id.Should().Be(accessionCommentCreated.Id);
        accessionCommentReturned.Comment.Should().Be(comment);

        accessionCommentCreated.Comment.Should().Be(comment);
        accessionCommentCreated.AccessionId.Should().Be(accession.Id);
        accessionCommentCreated.ParentAccessionComment.Should().BeNull();
        accessionCommentCreated.ParentAccessionCommentId.Should().BeNull();
        accessionCommentCreated.Status.Should().Be(AccessionCommentStatus.Active());
    }
}