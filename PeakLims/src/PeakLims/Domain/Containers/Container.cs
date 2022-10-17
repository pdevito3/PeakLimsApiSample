namespace PeakLims.Domain.Containers;

using SharedKernel.Exceptions;
using PeakLims.Domain.Containers.Dtos;
using PeakLims.Domain.Containers.Validators;
using PeakLims.Domain.Containers.DomainEvents;
using FluentValidation;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using ContainerStatuses;
using SampleTypes;
using Sieve.Attributes;


public class Container : BaseEntity
{
    public virtual string Type { get; private set; }
    public virtual ContainerStatus Status { get; private set; }
    public virtual SampleType UsedFor { get; private set; }
    public bool CanStore(SampleType sampleType) => UsedFor == sampleType;


    public static Container Create(ContainerForCreationDto containerForCreationDto)
    {
        new ContainerForCreationDtoValidator().ValidateAndThrow(containerForCreationDto);

        var newContainer = new Container();

        newContainer.Status = ContainerStatus.Active();
        newContainer.UsedFor = new SampleType(containerForCreationDto.UsedFor);
        newContainer.Type = containerForCreationDto.Type;

        newContainer.QueueDomainEvent(new ContainerCreated(){ Container = newContainer });
        
        return newContainer;
    }

    public void Update(ContainerForUpdateDto containerForUpdateDto)
    {
        new ContainerForUpdateDtoValidator().ValidateAndThrow(containerForUpdateDto);

        Status = ContainerStatus.Active();
        UsedFor = new SampleType(containerForUpdateDto.UsedFor);
        Type = containerForUpdateDto.Type;

        QueueDomainEvent(new ContainerUpdated(){ Id = Id });
    }

    public Container Activate()
    {
        if (Status == ContainerStatus.Active())
            return this;
        
        Status = ContainerStatus.Active();
        QueueDomainEvent(new ContainerUpdated(){ Id = Id });
        return this;
    }

    public Container Deactivate()
    {
        if (Status == ContainerStatus.Inactive())
            return this;
        
        Status = ContainerStatus.Inactive();
        QueueDomainEvent(new ContainerUpdated(){ Id = Id });
        return this;
    }
    
    protected Container() { } // For EF + Mocking
}