namespace PeakLims.Domain.TestOrders;

using PeakLims.Domain.TestOrders.Dtos;
using PeakLims.Domain.TestOrders.DomainEvents;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using AccessionStatuses;
using PeakLims.Domain.Tests;
using TestOrderStatuses;
using SharedKernel.Exceptions;

public class TestOrder : BaseEntity
{
    public virtual TestOrderStatus Status { get; private set; }

    [JsonIgnore]
    [IgnoreDataMember]
    [ForeignKey("Test")]
    public virtual Guid? TestId { get; private set; }
    public virtual Test Test { get; private set; }


    public static TestOrder Create(TestOrderForCreationDto testOrderForCreationDto)
    {
        var newTestOrder = new TestOrder();

        newTestOrder.Status = TestOrderStatus.Pending();
        newTestOrder.TestId = testOrderForCreationDto.TestId;

        newTestOrder.QueueDomainEvent(new TestOrderCreated(){ TestOrder = newTestOrder });
        
        return newTestOrder;
    }

    public TestOrder Update(TestOrderForUpdateDto testOrderForUpdateDto)
    {
        // TODO unit test
        if (Status == TestOrderStatus.Pending() || Status == TestOrderStatus.ReadyForTesting())
        {
            TestId = testOrderForUpdateDto.TestId;
            QueueDomainEvent(new TestOrderUpdated(){ Id = Id });
        }

        return this;
    }

    public TestOrder SetStatusToReadyForTesting()
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
        QueueDomainEvent(new TestOrderUpdated(){ Id = Id });
        return this;
    }

    public void SetTest(Test test)
    {        
        Test = test;
        TestId = test.Id;
        QueueDomainEvent(new TestOrderUpdated(){ Id = Id });
    }
    
    protected TestOrder() { } // For EF + Mocking
}