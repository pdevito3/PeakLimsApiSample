namespace PeakLims.Domain.TestOrders;

using SharedKernel.Exceptions;
using PeakLims.Domain.TestOrders.Dtos;
using PeakLims.Domain.TestOrders.Validators;
using PeakLims.Domain.TestOrders.DomainEvents;
using FluentValidation;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Sieve.Attributes;
using PeakLims.Domain.Tests;


public class TestOrder : BaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string Status { get; private set; }

    [JsonIgnore]
    [IgnoreDataMember]
    [ForeignKey("Test")]
    public virtual Guid? TestId { get; private set; }
    public virtual Test Test { get; private set; }


    public static TestOrder Create(TestOrderForCreationDto testOrderForCreationDto)
    {
        new TestOrderForCreationDtoValidator().ValidateAndThrow(testOrderForCreationDto);

        var newTestOrder = new TestOrder();

        newTestOrder.Status = testOrderForCreationDto.Status;
        newTestOrder.TestId = testOrderForCreationDto.TestId;

        newTestOrder.QueueDomainEvent(new TestOrderCreated(){ TestOrder = newTestOrder });
        
        return newTestOrder;
    }

    public void Update(TestOrderForUpdateDto testOrderForUpdateDto)
    {
        new TestOrderForUpdateDtoValidator().ValidateAndThrow(testOrderForUpdateDto);

        Status = testOrderForUpdateDto.Status;
        TestId = testOrderForUpdateDto.TestId;

        QueueDomainEvent(new TestOrderUpdated(){ Id = Id });
    }

    public void SetTest(Test panel)
    {        
        Test = panel;
        QueueDomainEvent(new TestOrderUpdated(){ Id = Id });
    }
    
    protected TestOrder() { } // For EF + Mocking
}