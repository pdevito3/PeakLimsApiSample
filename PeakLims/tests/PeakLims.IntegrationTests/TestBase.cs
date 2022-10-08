namespace PeakLims.IntegrationTests;

using NUnit.Framework;
using System.Threading.Tasks;
using AutoBogus;

    using HeimGuard;
    using Moq;
using static TestFixture;

[Parallelizable]
public class TestBase
{
    [SetUp]
    public Task TestSetUp()
    {
        var userPolicyHandler = GetService<IHeimGuardClient>();
        Mock.Get(userPolicyHandler)
            .Setup(x => x.HasPermissionAsync(It.IsAny<string>()))
            .ReturnsAsync(true);

        AutoFaker.Configure(builder =>
        {
            // configure global autobogus settings here
            builder.WithDateTimeKind(DateTimeKind.Utc)
                .WithRecursiveDepth(3)
                .WithTreeDepth(1)
                .WithRepeatCount(1);
        });
        
        return Task.CompletedTask;
    }
}