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
        var fakeAccessionOne = Accession.Create();
        var fakeAccessionTwo = Accession.Create();
        var fakeAccessionThree = Accession.Create();
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
}