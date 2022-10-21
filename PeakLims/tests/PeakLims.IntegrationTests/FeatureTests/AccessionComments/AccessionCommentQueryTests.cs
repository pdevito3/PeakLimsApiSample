namespace PeakLims.IntegrationTests.FeatureTests.AccessionComments;

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
using SharedTestHelpers.Fakes.AccessionComments;

public class AccessionCommentQueryTests : TestBase
{
    [Test]
    public async Task can_get_existing_accessioncomment_with_accurate_props()
    {
        // Arrange
        var fakeAccessionCommentOne = FakeAccessionCommentBuilder.Initialize()
            .WithMockAccession()
            .Build();
        await InsertAsync(fakeAccessionCommentOne);

        // Act
        var query = new GetAccessionComment.Query(fakeAccessionCommentOne.Id);
        var accessionComment = await SendAsync(query);

        // Assert
        accessionComment.Comment.Should().Be(fakeAccessionCommentOne.Comment);
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