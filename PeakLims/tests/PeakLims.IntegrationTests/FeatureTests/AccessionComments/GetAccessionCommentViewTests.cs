namespace PeakLims.IntegrationTests.FeatureTests.AccessionComments;

using Bogus;
using Domain.AccessionComments.Features;
using Domain.Tests.Services;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SharedTestHelpers.Fakes.Accession;
using SharedTestHelpers.Fakes.AccessionComments;
using static TestFixture;

public class GetAccessionCommentViewTests
{
    [Test]
    public async Task can_get_view_with_basic_history()
    {
        // Arrange
        
        // summary
        var accession = FakeAccessionBuilder
            .Initialize()
            .WithTestRepository(GetService<ITestRepository>())
            .Build();

        // input
        var tumorboardDiscussionCommentItem = FakeAccessionCommentBuilder.Initialize()
            .WithAccession(accession)
            .Build();
        await InsertAsync(tumorboardDiscussionCommentItem);
        var newComment = new Faker().Lorem.Sentence();
        var command = new UpdateAccessionComment.Command(tumorboardDiscussionCommentItem.Id, newComment);
        await SendAsync(command);

        // Act
        var query = new GetAccessionCommentView.Query(accession.Id);
        var discussionView = await SendAsync(query);

        // Assert
        discussionView.AccessionComments.Count.Should().Be(1);
        var viewCommentItem = discussionView.AccessionComments[0];
        viewCommentItem.Comment.Should().Be(newComment);
        viewCommentItem.History.Count.Should().Be(1);
        
        var viewCommentItemHistory = viewCommentItem.History[0];
        viewCommentItemHistory.Comment.Should().Be(tumorboardDiscussionCommentItem.Comment);
    }
    
    [Test]
    public async Task can_handle_complex_chat_history()
    {
        // Arrange
        
        // summary
        var accession = FakeAccessionBuilder
            .Initialize()
            .WithTestRepository(GetService<ITestRepository>())
            .Build();

        // standalone input
        var standaloneCommentItem = FakeAccessionCommentBuilder.Initialize()
            .WithAccession(accession)
            .Build();
        
        // input with history
        var rootCommentItem = FakeAccessionCommentBuilder.Initialize()
            .WithAccession(accession)
            .Build();
        await InsertAsync(rootCommentItem, standaloneCommentItem);
        
        var firstUpdatedCommentText = new Faker().Lorem.Sentence();
        var command = new UpdateAccessionComment.Command(rootCommentItem.Id, firstUpdatedCommentText);
        await SendAsync(command);
        
        var updatedCommentItem = await ExecuteDbContextAsync(db => db.AccessionComments
            .FirstOrDefaultAsync(x => x.Comment == firstUpdatedCommentText));
        var finalCommentText = new Faker().Lorem.Sentence();
        command = new UpdateAccessionComment.Command(updatedCommentItem.Id, finalCommentText);
        await SendAsync(command);

        // Act
        var query = new GetAccessionCommentView.Query(accession.Id);
        var discussionView = await SendAsync(query);

        // Assert
        discussionView.AccessionComments.Count.Should().Be(2);
        var standaloneCommentItemView = discussionView.AccessionComments.FirstOrDefault(x => x.Id == standaloneCommentItem.Id);
        standaloneCommentItemView.Comment.Should().Be(standaloneCommentItem.Comment);
        
        var editedCommentItemView = discussionView.AccessionComments.FirstOrDefault(x => x.Id != standaloneCommentItem.Id);
        editedCommentItemView.Comment.Should().Be(finalCommentText);
 
        editedCommentItemView.History.Count.Should().Be(2);
        editedCommentItemView.History.Select(x => x.Comment).Should().Contain(rootCommentItem.Comment);
        editedCommentItemView.History.Select(x => x.Comment).Should().Contain(firstUpdatedCommentText);
    }
}