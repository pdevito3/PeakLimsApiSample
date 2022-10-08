namespace PeakLims.SharedTestHelpers.Fakes.Test;

using AutoBogus;
using PeakLims.Domain.Tests;
using PeakLims.Domain.Tests.Dtos;

public class FakeTest
{
    public static Test Generate(TestForCreationDto testForCreationDto)
    {
        return Test.Create(testForCreationDto);
    }

    public static Test Generate()
    {
        return Test.Create(new FakeTestForCreationDto().Generate());
    }
}