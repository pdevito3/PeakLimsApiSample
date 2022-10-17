namespace PeakLims.UnitTests.UnitTests.Domain.TestOrders;

using PeakLims.SharedTestHelpers.Fakes.TestOrder;
using PeakLims.Domain.TestOrders;
using PeakLims.Domain.TestOrders.DomainEvents;
using Bogus;
using FluentAssertions;
using FluentAssertions.Extensions;
using Moq;
using NUnit.Framework;
using PeakLims.Domain.TestOrderStatuses;
using Services;
using SharedTestHelpers.Fakes.Sample;
using SharedTestHelpers.Fakes.Test;

[Parallelizable]
public class ManageTestOrderSampleTests
{
    private readonly Faker _faker;

    public ManageTestOrderSampleTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_manage_sample()
    {
        // Arrange
        var sample = FakeSample.Generate();
        var test = new FakeTestBuilder()
            .WithMockRepository()
            .Activate()
            .Build();
        var fakeTestOrder = TestOrder.Create(test);
        
        // Act -- add
        fakeTestOrder.SetSample(sample);

        // Assert -- add
        fakeTestOrder.SampleId.Should().Be(sample.Id);
        fakeTestOrder.Sample.Should().Be(sample);
        
        // Act -- remove
        fakeTestOrder.RemoveSample();

        // Assert -- remove
        fakeTestOrder.SampleId.Should().BeNull();
        fakeTestOrder.Sample.Should().BeNull();
    }
}