namespace PeakLims.UnitTests.UnitTests.Domain.Samples.Features;

using PeakLims.SharedTestHelpers.Fakes.Sample;
using PeakLims.Domain.Samples;
using PeakLims.Domain.Samples.Dtos;
using PeakLims.Domain.Samples.Mappings;
using PeakLims.Domain.Samples.Features;
using PeakLims.Domain.Samples.Services;
using MapsterMapper;
using FluentAssertions;
using HeimGuard;
using Microsoft.Extensions.Options;
using MockQueryable.Moq;
using Moq;
using Sieve.Models;
using Sieve.Services;
using TestHelpers;
using NUnit.Framework;

public class GetSampleListTests
{
    
    private readonly SieveProcessor _sieveProcessor;
    private readonly Mapper _mapper = UnitTestUtils.GetApiMapper();
    private readonly Mock<ISampleRepository> _sampleRepository;
    private readonly Mock<IHeimGuardClient> _heimGuard;

    public GetSampleListTests()
    {
        _sampleRepository = new Mock<ISampleRepository>();
        var sieveOptions = Options.Create(new SieveOptions());
        _sieveProcessor = new SieveProcessor(sieveOptions);
        _heimGuard = new Mock<IHeimGuardClient>();
    }
    
    [Test]
    public async Task can_get_paged_list_of_sample()
    {
        //Arrange
        var fakeSampleOne = FakeSample.Generate();
        var fakeSampleTwo = FakeSample.Generate();
        var fakeSampleThree = FakeSample.Generate();
        var sample = new List<Sample>();
        sample.Add(fakeSampleOne);
        sample.Add(fakeSampleTwo);
        sample.Add(fakeSampleThree);
        var mockDbData = sample.AsQueryable().BuildMock();
        
        var queryParameters = new SampleParametersDto() { PageSize = 1, PageNumber = 2 };

        _sampleRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);
        
        //Act
        var query = new GetSampleList.Query(queryParameters);
        var handler = new GetSampleList.Handler(_sampleRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
    }
}