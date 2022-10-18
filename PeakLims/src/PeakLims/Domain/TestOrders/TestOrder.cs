namespace PeakLims.Domain.TestOrders;

using PeakLims.Domain.TestOrders.DomainEvents;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Accessions;
using Panels;
using PeakLims.Domain.Tests;
using PeakLims.Services;
using Samples;
using TestOrderStatuses;
using SharedKernel.Exceptions;
using TestOrderCancellationReasons;

public class TestOrder : BaseEntity
{
    public virtual TestOrderStatus Status { get; private set; }
    public virtual DateOnly? DueDate { get; private set; }
    public virtual int? TatSnapshot { get; private set; }
    public virtual TestOrderCancellationReason CancellationReason { get; private set; }
    public virtual string CancellationComments { get; private set; }
    
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

    [JsonIgnore]
    [IgnoreDataMember]
    [ForeignKey("Accession")]
    public virtual Guid? AccessionId { get; private set; }
    public virtual Accession Accession { get; private set; }

    [JsonIgnore]
    [IgnoreDataMember]
    [ForeignKey("Sample")]
    public virtual Guid? SampleId { get; private set; }
    public virtual Sample Sample { get; private set; }

    public bool IsPartOfPanel() => AssociatedPanelId.HasValue;


    public static TestOrder Create(Test test)
    {
        new ValidationException(nameof(TestOrder), $"Invalid Test.").ThrowWhenNull(test);
        var newTestOrder = new TestOrder();

        newTestOrder.Status = TestOrderStatus.Pending();
        newTestOrder.Test = test;
        newTestOrder.TestId = test?.Id;

        newTestOrder.QueueDomainEvent(new TestOrderCreated(){ TestOrder = newTestOrder });
        
        return newTestOrder;
    }
    
    public static TestOrder Create(Guid testId)
    {
        new ValidationException(nameof(TestOrder), $"Invalid Test Id.").ThrowWhenNullOrEmpty(testId);
        var newTestOrder = new TestOrder();

        newTestOrder.Status = TestOrderStatus.Pending();
        newTestOrder.TestId = testId;

        newTestOrder.QueueDomainEvent(new TestOrderCreated(){ TestOrder = newTestOrder });
        
        return newTestOrder;
    }
    
    public static TestOrder Create(Guid testId, Guid associatedPanelId)
    {
        new ValidationException(nameof(TestOrder), $"Invalid Test Id.").ThrowWhenNullOrEmpty(testId);
        new ValidationException(nameof(TestOrder), $"Invalid Panel Id.").ThrowWhenNullOrEmpty(associatedPanelId);
        
        var newTestOrder = new TestOrder();

        newTestOrder.Status = TestOrderStatus.Pending();
        newTestOrder.TestId = testId;
        newTestOrder.AssociatedPanelId = associatedPanelId;

        newTestOrder.QueueDomainEvent(new TestOrderCreated(){ TestOrder = newTestOrder });
        
        return newTestOrder;
    }
    
    public static TestOrder Create(Test test, Panel associatedPanel)
    {
        new ValidationException(nameof(TestOrder), $"Invalid Test.").ThrowWhenNull(test);
        new ValidationException(nameof(TestOrder), $"Invalid Panel.").ThrowWhenNull(associatedPanel);
        
        var newTestOrder = new TestOrder();

        newTestOrder.Status = TestOrderStatus.Pending();
        newTestOrder.Test = test;
        newTestOrder.TestId = test?.Id;
        newTestOrder.AssociatedPanel = associatedPanel;
        newTestOrder.AssociatedPanelId = associatedPanel?.Id;

        newTestOrder.QueueDomainEvent(new TestOrderCreated(){ TestOrder = newTestOrder });
        
        return newTestOrder;
    }

    public TestOrder SetStatusToReadyForTesting(IDateTimeProvider dateTimeProvider)
    {
        if (Status.IsProcessing())
            throw new ValidationException(nameof(TestOrder),
                $"Test orders in a {Status.Value} state can not be set to {TestOrderStatus.ReadyForTesting().Value}.");
        new ValidationException(nameof(TestOrder),
                $"A test is required in order to set a test order to {TestOrderStatus.ReadyForTesting().Value}.")
            .ThrowWhenNullOrEmpty(TestId);
        new ValidationException(nameof(TestOrder),
                $"A sample is required in order to set a test order to {TestOrderStatus.ReadyForTesting().Value}.")
            .ThrowWhenNullOrEmpty(SampleId);

        Status = TestOrderStatus.ReadyForTesting();
        TatSnapshot = Test.TurnAroundTime;
        DueDate = dateTimeProvider.DateOnlyUtcNow.AddDays(Test.TurnAroundTime);
        QueueDomainEvent(new TestOrderUpdated(){ Id = Id });
        return this;
    }

    public TestOrder Cancel(TestOrderCancellationReason reason, string comments)
    {
        new ValidationException(nameof(TestOrder),
            $"A comment must be provided detailing why the test order was cancelled.")
            .ThrowWhenNullOrEmpty(comments);
        
        // TODO unit test
        if (Status.IsFinalState())
            throw new ValidationException(nameof(TestOrder),
                $"This test order is already in a final state and can not be cancelled.");
        
        Status = TestOrderStatus.Cancelled();
        CancellationReason = reason;
        CancellationComments = comments;
        QueueDomainEvent(new TestOrderUpdated(){ Id = Id });
        return this;
    }

    public TestOrder SetSample(Sample sample)
    {
        GuardSampleIfTestOrderIsProcessing();

        Sample = sample;
        SampleId = sample.Id;
        QueueDomainEvent(new TestOrderUpdated(){ Id = Id });
        return this;
    }

    public TestOrder RemoveSample()
    {
        GuardSampleIfTestOrderIsProcessing();

        Sample = null;
        SampleId = null;
        QueueDomainEvent(new TestOrderUpdated(){ Id = Id });
        return this;
    }

    private void GuardSampleIfTestOrderIsProcessing()
    {
        if (Status.IsProcessing())
            throw new ValidationException(nameof(TestOrder),
                $"The assigned sample can not be updated once a test order has started processing.");
    }

    protected TestOrder() { } // For EF + Mocking
}