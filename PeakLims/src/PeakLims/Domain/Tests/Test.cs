namespace PeakLims.Domain.Tests;

using SharedKernel.Exceptions;
using PeakLims.Domain.Tests.Dtos;
using PeakLims.Domain.Tests.Validators;
using PeakLims.Domain.Tests.DomainEvents;
using FluentValidation;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Sieve.Attributes;
using PeakLims.Domain.Panels;
using Services;
using TestStatuses;

public class Test : BaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string TestCode { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string TestName { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string Methodology { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string Platform { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual int Version { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual TestStatus Status { get; private set; }

    [JsonIgnore]
    [IgnoreDataMember]
    public virtual ICollection<Panel> Panels { get; private set; }


    public static Test Create(TestForCreationDto testForCreationDto)
    {
        new TestForCreationDtoValidator().ValidateAndThrow(testForCreationDto);

        var newTest = new Test();
        
        newTest.TestCode = testForCreationDto.TestCode;
        newTest.TestName = testForCreationDto.TestName;
        newTest.Methodology = testForCreationDto.Methodology;
        newTest.Platform = testForCreationDto.Platform;
        newTest.Version = testForCreationDto.Version;
        newTest.Status = TestStatus.Draft();

        newTest.QueueDomainEvent(new TestCreated(){ Test = newTest });
        
        return newTest;
    }

    public void Update(TestForUpdateDto testForUpdateDto)
    {
        new TestForUpdateDtoValidator().ValidateAndThrow(testForUpdateDto);
        
        TestCode = testForUpdateDto.TestCode;
        TestName = testForUpdateDto.TestName;
        Methodology = testForUpdateDto.Methodology;
        Platform = testForUpdateDto.Platform;
        Version = testForUpdateDto.Version;

        QueueDomainEvent(new TestUpdated(){ Id = Id });
    }

    public void Activate()
    {
        Status = TestStatus.Active();
        QueueDomainEvent(new TestUpdated(){ Id = Id });
    }

    public void Deactivate()
    {
        Status = TestStatus.Inactive();
        QueueDomainEvent(new TestUpdated(){ Id = Id });
    }
    
    protected Test() { } // For EF + Mocking
}