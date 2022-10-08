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
using Sieve.Attributes;


public class Container : BaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string ContainerNumber { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string State { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string Type { get; private set; }


    public static Container Create(ContainerForCreationDto containerForCreationDto)
    {
        new ContainerForCreationDtoValidator().ValidateAndThrow(containerForCreationDto);

        var newContainer = new Container();

        newContainer.ContainerNumber = containerForCreationDto.ContainerNumber;
        newContainer.State = containerForCreationDto.State;
        newContainer.Type = containerForCreationDto.Type;

        newContainer.QueueDomainEvent(new ContainerCreated(){ Container = newContainer });
        
        return newContainer;
    }

    public void Update(ContainerForUpdateDto containerForUpdateDto)
    {
        new ContainerForUpdateDtoValidator().ValidateAndThrow(containerForUpdateDto);

        ContainerNumber = containerForUpdateDto.ContainerNumber;
        State = containerForUpdateDto.State;
        Type = containerForUpdateDto.Type;

        QueueDomainEvent(new ContainerUpdated(){ Id = Id });
    }
    
    protected Container() { } // For EF + Mocking
}