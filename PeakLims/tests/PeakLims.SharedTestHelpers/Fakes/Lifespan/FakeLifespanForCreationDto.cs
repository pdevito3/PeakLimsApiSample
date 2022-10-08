namespace PeakLims.SharedTestHelpers.Fakes.Lifespan;

using AutoBogus;
using Domain.Lifespans.Dtos;

// or replace 'AutoFaker' with 'Faker' along with your own rules if you don't want all fields to be auto faked
public class FakeLifespanForCreationDto : AutoFaker<LifespanForCreationDto>
{
    public FakeLifespanForCreationDto()
    {
        RuleFor(x => x.DateOfBirth, f=> f.Date.PastDateOnly());
    }
}