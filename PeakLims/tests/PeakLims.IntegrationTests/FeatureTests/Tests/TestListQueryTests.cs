namespace PeakLims.IntegrationTests.FeatureTests.Tests;

using PeakLims.Domain.Tests.Dtos;
using PeakLims.SharedTestHelpers.Fakes.Test;
using SharedKernel.Exceptions;
using PeakLims.Domain.Tests.Features;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;
using static TestFixture;

public class TestListQueryTests : TestBase
{
    
    [Test]
    public async Task can_get_test_list()
    {
        // Arrange
        var fakeTestOne = FakeTest.Generate(new FakeTestForCreationDto().Generate());
        var fakeTestTwo = FakeTest.Generate(new FakeTestForCreationDto().Generate());
        var queryParameters = new TestParametersDto();

        await InsertAsync(fakeTestOne, fakeTestTwo);

        // Act
        var query = new GetTestList.Query(queryParameters);
        var tests = await SendAsync(query);

        // Assert
        tests.Count.Should().BeGreaterThanOrEqualTo(2);
    }
}