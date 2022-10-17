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
using PeakLims.Domain.Containers.Services;
using Services;
using SharedTestHelpers.Fakes.Container;

public class AddSampleCommandTests
{
    [Test]
    public async Task can_add_container_when_passed_to_handler()
    {
        //Arrange
        var mapper = UnitTestUtils.GetApiMapper();
        var sampleRepository = new Mock<ISampleRepository>();
        var containerRepository = new Mock<IContainerRepository>();
        var heimGuard = new Mock<IHeimGuardClient>();
        var uow = new Mock<IUnitOfWork>();
        
        var container = FakeContainer.Generate();
        var sampleToCreate = new FakeSampleForCreationDto().Generate();
        sampleToCreate.ContainerId = container.Id;
        sampleToCreate.Type = container.UsedFor.Value;

        containerRepository
            .Setup(x => x.GetById(container.Id, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(container);
        
        //Act
        var query = new AddSample.Command(sampleToCreate);
        var handler = new AddSample.Handler(sampleRepository.Object, uow.Object, mapper, heimGuard.Object, containerRepository.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.ContainerId.Should().Be(container.Id);
    }
    
    [Test]
    public async Task container_is_null_when_not_passed()
    {
        //Arrange
        var mapper = UnitTestUtils.GetApiMapper();
        var sampleRepository = new Mock<ISampleRepository>();
        var containerRepository = new Mock<IContainerRepository>();
        var heimGuard = new Mock<IHeimGuardClient>();
        var uow = new Mock<IUnitOfWork>();
        
        var sampleToCreate = new FakeSampleForCreationDto().Generate();
        sampleToCreate.ContainerId = null;
        
        //Act
        var query = new AddSample.Command(sampleToCreate);
        var handler = new AddSample.Handler(sampleRepository.Object, uow.Object, mapper, heimGuard.Object, containerRepository.Object);
        var response = await handler.Handle(query, CancellationToken.None);

        // Assert
        response.ContainerId.Should().BeNull();
    }
}