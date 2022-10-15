namespace PeakLims.SharedTestHelpers.Fakes.Test;

using AutoBogus;
using Domain.Tests.Services;
using PeakLims.Domain.Tests;
using PeakLims.Domain.Tests.Dtos;

public class FakeTest
{
    public static Test Generate(TestForCreationDto panelForCreationDto, ITestRepository testRepository)
    {
        return Test.Create(panelForCreationDto, testRepository);
    }
    public static Test GenerateActivated(TestForCreationDto panelForCreationDto, ITestRepository testRepository)
    {
        var panel = Test.Create(panelForCreationDto, testRepository);
        panel.Activate();
        return panel;
    }

    public static Test Generate(ITestRepository testRepository)
    {
        return Generate(new FakeTestForCreationDto().Generate(), testRepository);
    }

    public static Test GenerateActivated(ITestRepository testRepository)
    {
        var panel = Generate(new FakeTestForCreationDto().Generate(), testRepository);
        panel.Activate();
        return panel;
    }
}