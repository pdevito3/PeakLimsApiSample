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

    [Test]
    public async Task can_filter_sample_list_using_SampleNumber()
    {
        //Arrange
        var fakeSampleOne = FakeSample.Generate(new FakeSampleForCreationDto()
            .RuleFor(s => s.SampleNumber, _ => "alpha")
            .Generate());
        var fakeSampleTwo = FakeSample.Generate(new FakeSampleForCreationDto()
            .RuleFor(s => s.SampleNumber, _ => "bravo")
            .Generate());
        var queryParameters = new SampleParametersDto() { Filters = $"SampleNumber == {fakeSampleTwo.SampleNumber}" };

        var sampleList = new List<Sample>() { fakeSampleOne, fakeSampleTwo };
        var mockDbData = sampleList.AsQueryable().BuildMock();

        _sampleRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetSampleList.Query(queryParameters);
        var handler = new GetSampleList.Handler(_sampleRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeSampleTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_filter_sample_list_using_Status()
    {
        //Arrange
        var fakeSampleOne = FakeSample.Generate(new FakeSampleForCreationDto()
            .RuleFor(s => s.Status, _ => "alpha")
            .Generate());
        var fakeSampleTwo = FakeSample.Generate(new FakeSampleForCreationDto()
            .RuleFor(s => s.Status, _ => "bravo")
            .Generate());
        var queryParameters = new SampleParametersDto() { Filters = $"Status == {fakeSampleTwo.Status}" };

        var sampleList = new List<Sample>() { fakeSampleOne, fakeSampleTwo };
        var mockDbData = sampleList.AsQueryable().BuildMock();

        _sampleRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetSampleList.Query(queryParameters);
        var handler = new GetSampleList.Handler(_sampleRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeSampleTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_filter_sample_list_using_Type()
    {
        //Arrange
        var fakeSampleOne = FakeSample.Generate(new FakeSampleForCreationDto()
            .RuleFor(s => s.Type, _ => "alpha")
            .Generate());
        var fakeSampleTwo = FakeSample.Generate(new FakeSampleForCreationDto()
            .RuleFor(s => s.Type, _ => "bravo")
            .Generate());
        var queryParameters = new SampleParametersDto() { Filters = $"Type == {fakeSampleTwo.Type}" };

        var sampleList = new List<Sample>() { fakeSampleOne, fakeSampleTwo };
        var mockDbData = sampleList.AsQueryable().BuildMock();

        _sampleRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetSampleList.Query(queryParameters);
        var handler = new GetSampleList.Handler(_sampleRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeSampleTwo, options =>
                options.ExcludingMissingMembers());
    }







    [Test]
    public async Task can_filter_sample_list_using_CollectionSite()
    {
        //Arrange
        var fakeSampleOne = FakeSample.Generate(new FakeSampleForCreationDto()
            .RuleFor(s => s.CollectionSite, _ => "alpha")
            .Generate());
        var fakeSampleTwo = FakeSample.Generate(new FakeSampleForCreationDto()
            .RuleFor(s => s.CollectionSite, _ => "bravo")
            .Generate());
        var queryParameters = new SampleParametersDto() { Filters = $"CollectionSite == {fakeSampleTwo.CollectionSite}" };

        var sampleList = new List<Sample>() { fakeSampleOne, fakeSampleTwo };
        var mockDbData = sampleList.AsQueryable().BuildMock();

        _sampleRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetSampleList.Query(queryParameters);
        var handler = new GetSampleList.Handler(_sampleRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeSampleTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_sample_by_SampleNumber()
    {
        //Arrange
        var fakeSampleOne = FakeSample.Generate(new FakeSampleForCreationDto()
            .RuleFor(s => s.SampleNumber, _ => "alpha")
            .Generate());
        var fakeSampleTwo = FakeSample.Generate(new FakeSampleForCreationDto()
            .RuleFor(s => s.SampleNumber, _ => "bravo")
            .Generate());
        var queryParameters = new SampleParametersDto() { SortOrder = "-SampleNumber" };

        var SampleList = new List<Sample>() { fakeSampleOne, fakeSampleTwo };
        var mockDbData = SampleList.AsQueryable().BuildMock();

        _sampleRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetSampleList.Query(queryParameters);
        var handler = new GetSampleList.Handler(_sampleRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakeSampleTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeSampleOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_sample_by_Status()
    {
        //Arrange
        var fakeSampleOne = FakeSample.Generate(new FakeSampleForCreationDto()
            .RuleFor(s => s.Status, _ => "alpha")
            .Generate());
        var fakeSampleTwo = FakeSample.Generate(new FakeSampleForCreationDto()
            .RuleFor(s => s.Status, _ => "bravo")
            .Generate());
        var queryParameters = new SampleParametersDto() { SortOrder = "-Status" };

        var SampleList = new List<Sample>() { fakeSampleOne, fakeSampleTwo };
        var mockDbData = SampleList.AsQueryable().BuildMock();

        _sampleRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetSampleList.Query(queryParameters);
        var handler = new GetSampleList.Handler(_sampleRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakeSampleTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeSampleOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_sample_by_Type()
    {
        //Arrange
        var fakeSampleOne = FakeSample.Generate(new FakeSampleForCreationDto()
            .RuleFor(s => s.Type, _ => "alpha")
            .Generate());
        var fakeSampleTwo = FakeSample.Generate(new FakeSampleForCreationDto()
            .RuleFor(s => s.Type, _ => "bravo")
            .Generate());
        var queryParameters = new SampleParametersDto() { SortOrder = "-Type" };

        var SampleList = new List<Sample>() { fakeSampleOne, fakeSampleTwo };
        var mockDbData = SampleList.AsQueryable().BuildMock();

        _sampleRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetSampleList.Query(queryParameters);
        var handler = new GetSampleList.Handler(_sampleRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakeSampleTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeSampleOne, options =>
                options.ExcludingMissingMembers());
    }







    [Test]
    public async Task can_get_sorted_list_of_sample_by_CollectionSite()
    {
        //Arrange
        var fakeSampleOne = FakeSample.Generate(new FakeSampleForCreationDto()
            .RuleFor(s => s.CollectionSite, _ => "alpha")
            .Generate());
        var fakeSampleTwo = FakeSample.Generate(new FakeSampleForCreationDto()
            .RuleFor(s => s.CollectionSite, _ => "bravo")
            .Generate());
        var queryParameters = new SampleParametersDto() { SortOrder = "-CollectionSite" };

        var SampleList = new List<Sample>() { fakeSampleOne, fakeSampleTwo };
        var mockDbData = SampleList.AsQueryable().BuildMock();

        _sampleRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetSampleList.Query(queryParameters);
        var handler = new GetSampleList.Handler(_sampleRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakeSampleTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeSampleOne, options =>
                options.ExcludingMissingMembers());
    }
}