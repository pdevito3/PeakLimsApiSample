namespace PeakLims.SharedTestHelpers.Fakes.Test;

using AutoBogus;
using PeakLims.Domain.Tests;
using PeakLims.Domain.Tests.Dtos;

public class FakeTest
{
    public static Test Generate(TestForCreationDto testForCreationDto)
    {
        var test = Test.Create(testForCreationDto);
        return test;
    }
    
    public static Test GenerateActivated(TestForCreationDto testForCreationDto)
    {
        var test = Test.Create(testForCreationDto);
        test.Activate();
        return test;
    }

    public static Test Generate()
    {
        return Generate(new FakeTestForCreationDto().Generate());
    }
    
    public static Test GenerateActivated()
    {
        return GenerateActivated(new FakeTestForCreationDto().Generate());
    }
}