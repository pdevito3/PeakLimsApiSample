namespace PeakLims.SharedTestHelpers.Fakes.Test;

using AutoBogus;
using PeakLims.Domain.Tests;
using PeakLims.Domain.Tests.Dtos;

// or replace 'AutoFaker' with 'Faker' along with your own rules if you don't want all fields to be auto faked
public class FakeTestForCreationDto : AutoFaker<TestForCreationDto>
{
    public FakeTestForCreationDto()
    {
        RuleFor(x => x.Version, f => f.Random.Int(0));
    }
}