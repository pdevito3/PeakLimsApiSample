namespace PeakLims.UnitTests.UnitTests.Domain.Accessions.Features;

using PeakLims.SharedTestHelpers.Fakes.Accession;
using PeakLims.Domain.Accessions;
using PeakLims.Domain.Accessions.Dtos;
using PeakLims.Domain.Accessions.Mappings;
using PeakLims.Domain.Accessions.Features;
using PeakLims.Domain.Accessions.Services;
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

public class GetAccessionListTests
{
    
    private readonly SieveProcessor _sieveProcessor;
    private readonly Mapper _mapper = UnitTestUtils.GetApiMapper();
    private readonly Mock<IAccessionRepository> _accessionRepository;
    private readonly Mock<IHeimGuardClient> _heimGuard;

    public GetAccessionListTests()
    {
        _accessionRepository = new Mock<IAccessionRepository>();
        var sieveOptions = Options.Create(new SieveOptions());
        _sieveProcessor = new SieveProcessor(sieveOptions);
        _heimGuard = new Mock<IHeimGuardClient>();
    }
    
    [Test]
    public async Task can_get_paged_list_of_accession()
    {
        //Arrange
        var fakeAccessionOne = FakeAccession.Generate();
        var fakeAccessionTwo = FakeAccession.Generate();
        var fakeAccessionThree = FakeAccession.Generate();
        var accession = new List<Accession>();
        accession.Add(fakeAccessionOne);
        accession.Add(fakeAccessionTwo);
        accession.Add(fakeAccessionThree);
        var mockDbData = accession.AsQueryable().BuildMock();
        
        var queryParameters = new AccessionParametersDto() { PageSize = 1, PageNumber = 2 };

        _accessionRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);
        
        //Act
        var query = new GetAccessionList.Query(queryParameters);
        var handler = new GetAccessionList.Handler(_accessionRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
    }

    [Test]
    public async Task can_filter_accession_list_using_AccessionNumber()
    {
        //Arrange
        var fakeAccessionOne = FakeAccession.Generate(new FakeAccessionForCreationDto()
            .RuleFor(a => a.AccessionNumber, _ => "alpha")
            .Generate());
        var fakeAccessionTwo = FakeAccession.Generate(new FakeAccessionForCreationDto()
            .RuleFor(a => a.AccessionNumber, _ => "bravo")
            .Generate());
        var queryParameters = new AccessionParametersDto() { Filters = $"AccessionNumber == {fakeAccessionTwo.AccessionNumber}" };

        var accessionList = new List<Accession>() { fakeAccessionOne, fakeAccessionTwo };
        var mockDbData = accessionList.AsQueryable().BuildMock();

        _accessionRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetAccessionList.Query(queryParameters);
        var handler = new GetAccessionList.Handler(_accessionRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeAccessionTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_filter_accession_list_using_State()
    {
        //Arrange
        var fakeAccessionOne = FakeAccession.Generate(new FakeAccessionForCreationDto()
            .RuleFor(a => a.State, _ => "alpha")
            .Generate());
        var fakeAccessionTwo = FakeAccession.Generate(new FakeAccessionForCreationDto()
            .RuleFor(a => a.State, _ => "bravo")
            .Generate());
        var queryParameters = new AccessionParametersDto() { Filters = $"State == {fakeAccessionTwo.State}" };

        var accessionList = new List<Accession>() { fakeAccessionOne, fakeAccessionTwo };
        var mockDbData = accessionList.AsQueryable().BuildMock();

        _accessionRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetAccessionList.Query(queryParameters);
        var handler = new GetAccessionList.Handler(_accessionRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeAccessionTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_accession_by_AccessionNumber()
    {
        //Arrange
        var fakeAccessionOne = FakeAccession.Generate(new FakeAccessionForCreationDto()
            .RuleFor(a => a.AccessionNumber, _ => "alpha")
            .Generate());
        var fakeAccessionTwo = FakeAccession.Generate(new FakeAccessionForCreationDto()
            .RuleFor(a => a.AccessionNumber, _ => "bravo")
            .Generate());
        var queryParameters = new AccessionParametersDto() { SortOrder = "-AccessionNumber" };

        var AccessionList = new List<Accession>() { fakeAccessionOne, fakeAccessionTwo };
        var mockDbData = AccessionList.AsQueryable().BuildMock();

        _accessionRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetAccessionList.Query(queryParameters);
        var handler = new GetAccessionList.Handler(_accessionRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakeAccessionTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeAccessionOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_accession_by_State()
    {
        //Arrange
        var fakeAccessionOne = FakeAccession.Generate(new FakeAccessionForCreationDto()
            .RuleFor(a => a.State, _ => "alpha")
            .Generate());
        var fakeAccessionTwo = FakeAccession.Generate(new FakeAccessionForCreationDto()
            .RuleFor(a => a.State, _ => "bravo")
            .Generate());
        var queryParameters = new AccessionParametersDto() { SortOrder = "-State" };

        var AccessionList = new List<Accession>() { fakeAccessionOne, fakeAccessionTwo };
        var mockDbData = AccessionList.AsQueryable().BuildMock();

        _accessionRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetAccessionList.Query(queryParameters);
        var handler = new GetAccessionList.Handler(_accessionRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakeAccessionTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeAccessionOne, options =>
                options.ExcludingMissingMembers());
    }
}