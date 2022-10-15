namespace PeakLims.Domain.TestOrders;

using PeakLims.Domain.TestOrders.DomainEvents;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Panels;
using PeakLims.Domain.Tests;
using PeakLims.Services;
using TestOrderStatuses;
using SharedKernel.Exceptions;

public class TestOrder : BaseEntity
{
    public virtual TestOrderStatus Status { get; private set; }
    public virtual DateOnly? DueDate { get; private set; }
    public virtual int? TatSnapshot { get; private set; }
    
    [JsonIgnore]
    [IgnoreDataMember]
    [ForeignKey("Panel")]
    public virtual Guid? AssociatedPanelId { get; private set; }
    public virtual Panel AssociatedPanel { get; private set; }

    [JsonIgnore]
    [IgnoreDataMember]
    [ForeignKey("Test")]
    public virtual Guid? TestId { get; private set; }
    public virtual Test Test { get; private set; }


    public static TestOrder Create(Test test)
    {
        var newTestOrder = new TestOrder();

        newTestOrder.Status = TestOrderStatus.Pending();
        newTestOrder.Test = test;
        newTestOrder.TestId = test.Id;

        newTestOrder.QueueDomainEvent(new TestOrderCreated(){ TestOrder = newTestOrder });
        
        return newTestOrder;
    }
    
    public static TestOrder Create(Guid testId)
    {
        var newTestOrder = new TestOrder();

        newTestOrder.Status = TestOrderStatus.Pending();
        newTestOrder.TestId = testId;

        newTestOrder.QueueDomainEvent(new TestOrderCreated(){ TestOrder = newTestOrder });
        
        return newTestOrder;
    }
    
    public static TestOrder Create(Guid testId, Guid associatedPanelId)
    {
        var newTestOrder = new TestOrder();

        newTestOrder.Status = TestOrderStatus.Pending();
        newTestOrder.TestId = testId;
        newTestOrder.AssociatedPanelId = associatedPanelId;

        newTestOrder.QueueDomainEvent(new TestOrderCreated(){ TestOrder = newTestOrder });
        
        return newTestOrder;
    }
    
    public static TestOrder Create(Test test, Panel associatedPanel)
    {
        var newTestOrder = new TestOrder();

        newTestOrder.Status = TestOrderStatus.Pending();
        newTestOrder.Test = test;
        newTestOrder.TestId = test.Id;
        newTestOrder.AssociatedPanel = associatedPanel;
        newTestOrder.AssociatedPanelId = associatedPanel.Id;

        newTestOrder.QueueDomainEvent(new TestOrderCreated(){ TestOrder = newTestOrder });
        
        return newTestOrder;
    }

    public TestOrder SetStatusToReadyForTesting(IDateTimeProvider dateTimeProvider)
    {
        // TODO unit test
        new ValidationException(nameof(TestOrder),
                $"A test is required in order to set a test order to {TestOrderStatus.ReadyForTesting().Value}")
            .ThrowWhenNullOrEmpty(TestId);
        
        // TODO unit test
        if (Status != TestOrderStatus.Pending())
            throw new ValidationException(nameof(TestOrder),
                $"Test orders in a {Status.Value} state can not be set to {TestOrderStatus.ReadyForTesting().Value}");
        
        Status = TestOrderStatus.ReadyForTesting();
        TatSnapshot = Test.TurnAroundTime;
        DueDate = dateTimeProvider.DateOnlyUtcNow.AddDays(Test.TurnAroundTime);
        QueueDomainEvent(new TestOrderUpdated(){ Id = Id });
        return this;
    }
    
    protected TestOrder() { } // For EF + Mocking
}