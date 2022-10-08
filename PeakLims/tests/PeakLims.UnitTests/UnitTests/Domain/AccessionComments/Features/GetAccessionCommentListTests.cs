namespace PeakLims.UnitTests.UnitTests.Domain.AccessionComments.Features;

using PeakLims.SharedTestHelpers.Fakes.AccessionComment;
using PeakLims.Domain.AccessionComments;
using PeakLims.Domain.AccessionComments.Dtos;
using PeakLims.Domain.AccessionComments.Mappings;
using PeakLims.Domain.AccessionComments.Features;
using PeakLims.Domain.AccessionComments.Services;
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

public class GetAccessionCommentListTests
{
    
    private readonly SieveProcessor _sieveProcessor;
    private readonly Mapper _mapper = UnitTestUtils.GetApiMapper();
    private readonly Mock<IAccessionCommentRepository> _accessionCommentRepository;
    private readonly Mock<IHeimGuardClient> _heimGuard;

    public GetAccessionCommentListTests()
    {
        _accessionCommentRepository = new Mock<IAccessionCommentRepository>();
        var sieveOptions = Options.Create(new SieveOptions());
        _sieveProcessor = new SieveProcessor(sieveOptions);
        _heimGuard = new Mock<IHeimGuardClient>();
    }
    
    [Test]
    public async Task can_get_paged_list_of_accessionComment()
    {
        //Arrange
        var fakeAccessionCommentOne = FakeAccessionComment.Generate();
        var fakeAccessionCommentTwo = FakeAccessionComment.Generate();
        var fakeAccessionCommentThree = FakeAccessionComment.Generate();
        var accessionComment = new List<AccessionComment>();
        accessionComment.Add(fakeAccessionCommentOne);
        accessionComment.Add(fakeAccessionCommentTwo);
        accessionComment.Add(fakeAccessionCommentThree);
        var mockDbData = accessionComment.AsQueryable().BuildMock();
        
        var queryParameters = new AccessionCommentParametersDto() { PageSize = 1, PageNumber = 2 };

        _accessionCommentRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);
        
        //Act
        var query = new GetAccessionCommentList.Query(queryParameters);
        var handler = new GetAccessionCommentList.Handler(_accessionCommentRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
    }

    [Test]
    public async Task can_filter_accessioncomment_list_using_Comment()
    {
        //Arrange
        var fakeAccessionCommentOne = FakeAccessionComment.Generate(new FakeAccessionCommentForCreationDto()
            .RuleFor(a => a.Comment, _ => "alpha")
            .Generate());
        var fakeAccessionCommentTwo = FakeAccessionComment.Generate(new FakeAccessionCommentForCreationDto()
            .RuleFor(a => a.Comment, _ => "bravo")
            .Generate());
        var queryParameters = new AccessionCommentParametersDto() { Filters = $"Comment == {fakeAccessionCommentTwo.Comment}" };

        var accessionCommentList = new List<AccessionComment>() { fakeAccessionCommentOne, fakeAccessionCommentTwo };
        var mockDbData = accessionCommentList.AsQueryable().BuildMock();

        _accessionCommentRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetAccessionCommentList.Query(queryParameters);
        var handler = new GetAccessionCommentList.Handler(_accessionCommentRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeAccessionCommentTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_filter_accessioncomment_list_using_InitialAccessionState()
    {
        //Arrange
        var fakeAccessionCommentOne = FakeAccessionComment.Generate(new FakeAccessionCommentForCreationDto()
            .RuleFor(a => a.InitialAccessionState, _ => "alpha")
            .Generate());
        var fakeAccessionCommentTwo = FakeAccessionComment.Generate(new FakeAccessionCommentForCreationDto()
            .RuleFor(a => a.InitialAccessionState, _ => "bravo")
            .Generate());
        var queryParameters = new AccessionCommentParametersDto() { Filters = $"InitialAccessionState == {fakeAccessionCommentTwo.InitialAccessionState}" };

        var accessionCommentList = new List<AccessionComment>() { fakeAccessionCommentOne, fakeAccessionCommentTwo };
        var mockDbData = accessionCommentList.AsQueryable().BuildMock();

        _accessionCommentRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetAccessionCommentList.Query(queryParameters);
        var handler = new GetAccessionCommentList.Handler(_accessionCommentRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeAccessionCommentTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_filter_accessioncomment_list_using_EndingAccessionState()
    {
        //Arrange
        var fakeAccessionCommentOne = FakeAccessionComment.Generate(new FakeAccessionCommentForCreationDto()
            .RuleFor(a => a.EndingAccessionState, _ => "alpha")
            .Generate());
        var fakeAccessionCommentTwo = FakeAccessionComment.Generate(new FakeAccessionCommentForCreationDto()
            .RuleFor(a => a.EndingAccessionState, _ => "bravo")
            .Generate());
        var queryParameters = new AccessionCommentParametersDto() { Filters = $"EndingAccessionState == {fakeAccessionCommentTwo.EndingAccessionState}" };

        var accessionCommentList = new List<AccessionComment>() { fakeAccessionCommentOne, fakeAccessionCommentTwo };
        var mockDbData = accessionCommentList.AsQueryable().BuildMock();

        _accessionCommentRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetAccessionCommentList.Query(queryParameters);
        var handler = new GetAccessionCommentList.Handler(_accessionCommentRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.Should().HaveCount(1);
        response
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeAccessionCommentTwo, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_accessioncomment_by_Comment()
    {
        //Arrange
        var fakeAccessionCommentOne = FakeAccessionComment.Generate(new FakeAccessionCommentForCreationDto()
            .RuleFor(a => a.Comment, _ => "alpha")
            .Generate());
        var fakeAccessionCommentTwo = FakeAccessionComment.Generate(new FakeAccessionCommentForCreationDto()
            .RuleFor(a => a.Comment, _ => "bravo")
            .Generate());
        var queryParameters = new AccessionCommentParametersDto() { SortOrder = "-Comment" };

        var AccessionCommentList = new List<AccessionComment>() { fakeAccessionCommentOne, fakeAccessionCommentTwo };
        var mockDbData = AccessionCommentList.AsQueryable().BuildMock();

        _accessionCommentRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetAccessionCommentList.Query(queryParameters);
        var handler = new GetAccessionCommentList.Handler(_accessionCommentRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakeAccessionCommentTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeAccessionCommentOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_accessioncomment_by_InitialAccessionState()
    {
        //Arrange
        var fakeAccessionCommentOne = FakeAccessionComment.Generate(new FakeAccessionCommentForCreationDto()
            .RuleFor(a => a.InitialAccessionState, _ => "alpha")
            .Generate());
        var fakeAccessionCommentTwo = FakeAccessionComment.Generate(new FakeAccessionCommentForCreationDto()
            .RuleFor(a => a.InitialAccessionState, _ => "bravo")
            .Generate());
        var queryParameters = new AccessionCommentParametersDto() { SortOrder = "-InitialAccessionState" };

        var AccessionCommentList = new List<AccessionComment>() { fakeAccessionCommentOne, fakeAccessionCommentTwo };
        var mockDbData = AccessionCommentList.AsQueryable().BuildMock();

        _accessionCommentRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetAccessionCommentList.Query(queryParameters);
        var handler = new GetAccessionCommentList.Handler(_accessionCommentRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakeAccessionCommentTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeAccessionCommentOne, options =>
                options.ExcludingMissingMembers());
    }

    [Test]
    public async Task can_get_sorted_list_of_accessioncomment_by_EndingAccessionState()
    {
        //Arrange
        var fakeAccessionCommentOne = FakeAccessionComment.Generate(new FakeAccessionCommentForCreationDto()
            .RuleFor(a => a.EndingAccessionState, _ => "alpha")
            .Generate());
        var fakeAccessionCommentTwo = FakeAccessionComment.Generate(new FakeAccessionCommentForCreationDto()
            .RuleFor(a => a.EndingAccessionState, _ => "bravo")
            .Generate());
        var queryParameters = new AccessionCommentParametersDto() { SortOrder = "-EndingAccessionState" };

        var AccessionCommentList = new List<AccessionComment>() { fakeAccessionCommentOne, fakeAccessionCommentTwo };
        var mockDbData = AccessionCommentList.AsQueryable().BuildMock();

        _accessionCommentRepository
            .Setup(x => x.Query())
            .Returns(mockDbData);

        //Act
        var query = new GetAccessionCommentList.Query(queryParameters);
        var handler = new GetAccessionCommentList.Handler(_accessionCommentRepository.Object, _mapper, _sieveProcessor, _heimGuard.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.FirstOrDefault()
            .Should().BeEquivalentTo(fakeAccessionCommentTwo, options =>
                options.ExcludingMissingMembers());
        response.Skip(1)
            .FirstOrDefault()
            .Should().BeEquivalentTo(fakeAccessionCommentOne, options =>
                options.ExcludingMissingMembers());
    }
}