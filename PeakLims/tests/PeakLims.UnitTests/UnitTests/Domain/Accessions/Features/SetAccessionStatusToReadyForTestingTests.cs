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
using PeakLims.Domain;
using Services;
using SharedKernel.Exceptions;

[Parallelizable]
public class SetAccessionStatusToReadyForTestingTests
{
    private Mock<IUnitOfWork> UnitOfWork { get; set; }
    private Mock<IAccessionRepository> AccessionRepository { get; set; }
    private Mock<IHeimGuardClient> HeimGuard { get; set; }

    [SetUp]
    public void TestSetUp()
    {
        UnitOfWork = new Mock<IUnitOfWork>();
        AccessionRepository = new Mock<IAccessionRepository>();
        HeimGuard = new Mock<IHeimGuardClient>();
    }
    
    [Test]
    public async Task must_have_permission()
    {
        //Arrange
        HeimGuard
            .Setup(x =>
                x.MustHavePermission<ForbiddenAccessException>(Permissions.CanSetAccessionStatusToReadyForTesting))
            .Throws<ForbiddenAccessException>();
        
        //Act
        var query = new SetAccessionStatusToReadyForTesting.Command(Guid.NewGuid());
        var handler = new SetAccessionStatusToReadyForTesting.Handler(AccessionRepository.Object, 
            UnitOfWork.Object, 
            HeimGuard.Object, 
            Mock.Of<IDateTimeProvider>());
        var act = () => handler.Handle(query, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ForbiddenAccessException>();
    }
}