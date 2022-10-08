namespace PeakLims.SharedTestHelpers.Fakes.AccessionComment;

using AutoBogus;
using PeakLims.Domain.AccessionComments;
using PeakLims.Domain.AccessionComments.Dtos;

public class FakeAccessionComment
{
    public static AccessionComment Generate(AccessionCommentForCreationDto accessionCommentForCreationDto)
    {
        return AccessionComment.Create(accessionCommentForCreationDto);
    }

    public static AccessionComment Generate()
    {
        return AccessionComment.Create(new FakeAccessionCommentForCreationDto().Generate());
    }
}