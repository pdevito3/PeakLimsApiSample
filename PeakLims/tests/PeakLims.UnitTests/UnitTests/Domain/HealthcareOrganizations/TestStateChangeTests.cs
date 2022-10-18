namespace PeakLims.UnitTests.UnitTests.Domain.Tests;

using Bogus;
using FluentAssertions;
using NUnit.Framework;
using PeakLims.Domain.AccessionStatuses;
using PeakLims.Domain.HealthcareOrganizations.DomainEvents;
using PeakLims.Domain.HealthcareOrganizationStatuses;
using PeakLims.Domain.Tests.DomainEvents;
using PeakLims.Domain.TestStatuses;
using PeakLims.SharedTestHelpers.Fakes.Test;
using SharedTestHelpers.Fakes.HealthcareOrganization;

[Parallelizable]
public class TestStateChangeTests
{
    private readonly Faker _faker;

    public TestStateChangeTests()
    {
        _faker = new Faker();
    }
    
    [Test]
    public void can_activate_test()
    {
        // Arrange
        var fakeHealthcareOrganization = FakeHealthcareOrganization.Generate();
        fakeHealthcareOrganization.DomainEvents.Clear();
        
        // Act
        fakeHealthcareOrganization.Activate();

        // Assert
        fakeHealthcareOrganization.Status.Should().Be(HealthcareOrganizationStatus.Active());
        fakeHealthcareOrganization.DomainEvents.Count.Should().Be(1);
        fakeHealthcareOrganization.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(HealthcareOrganizationUpdated));
    }
    
    [Test]
    public void can_deactivate_test()
    {
        // Arrange
        var fakeHealthcareOrganization = FakeHealthcareOrganization.Generate();
        fakeHealthcareOrganization.DomainEvents.Clear();
        
        // Act
        fakeHealthcareOrganization.Deactivate();

        // Assert
        fakeHealthcareOrganization.Status.Should().Be(HealthcareOrganizationStatus.Inactive());
        fakeHealthcareOrganization.DomainEvents.Count.Should().Be(1);
        fakeHealthcareOrganization.DomainEvents.FirstOrDefault().Should().BeOfType(typeof(HealthcareOrganizationUpdated));
    }
}