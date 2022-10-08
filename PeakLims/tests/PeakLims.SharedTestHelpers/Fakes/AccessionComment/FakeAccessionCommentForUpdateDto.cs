namespace PeakLims.SharedTestHelpers.Fakes.AccessionComment;

using AutoBogus;
using PeakLims.Domain.AccessionComments;
using PeakLims.Domain.AccessionComments.Dtos;

// or replace 'AutoFaker' with 'Faker' along with your own rules if you don't want all fields to be auto faked
public class FakeAccessionCommentForUpdateDto : AutoFaker<AccessionCommentForUpdateDto>
{
    public FakeAccessionCommentForUpdateDto()
    {
        // if you want default values on any of your properties (e.g. an int between a certain range or a date always in the past), you can add `RuleFor` lines describing those defaults
        //RuleFor(a => a.ExampleIntProperty, a => a.Random.Number(50, 100000));
        //RuleFor(a => a.ExampleDateProperty, a => a.Date.Past());
    }
}