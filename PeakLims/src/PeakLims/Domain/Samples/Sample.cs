namespace PeakLims.Domain.Samples;

using SharedKernel.Exceptions;
using PeakLims.Domain.Samples.Dtos;
using PeakLims.Domain.Samples.Validators;
using PeakLims.Domain.Samples.DomainEvents;
using FluentValidation;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Containers.Services;
using Features;
using Sieve.Attributes;
using PeakLims.Domain.Patients;
using PeakLims.Domain.Samples;
using PeakLims.Domain.Containers;
using SampleTypes;
using TestOrders;
using ValidationException = SharedKernel.Exceptions.ValidationException;

public class Sample : BaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string SampleNumber { get; }
    public virtual SampleType Type { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual decimal? Quantity { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual DateOnly? CollectionDate { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual DateOnly? ReceivedDate { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string CollectionSite { get; private set; }

    [JsonIgnore]
    [IgnoreDataMember]
    [ForeignKey("Patient")]
    public virtual Guid? PatientId { get; private set; }
    public virtual Patient Patient { get; private set; }

    [JsonIgnore]
    [IgnoreDataMember]
    [ForeignKey("Sample")]
    public virtual Guid? ParentSampleId { get; private set; }
    public virtual Sample ParentSample { get; private set; }

    [JsonIgnore]
    [IgnoreDataMember]
    [ForeignKey("Container")]
    public virtual Guid? ContainerId { get; private set; }
    public virtual Container Container { get; private set; }
    public virtual ICollection<TestOrder> TestOrders { get; private set; } = new List<TestOrder>();


    public static Sample Create(ContainerlessSampleForCreationDto containerlessSampleForCreationDto)
    {
        new SampleForCreationDtoValidator().ValidateAndThrow(containerlessSampleForCreationDto);

        var newSample = new Sample();

        newSample.Type = new SampleType(containerlessSampleForCreationDto.Type);
        newSample.Quantity = containerlessSampleForCreationDto.Quantity;
        newSample.CollectionDate = containerlessSampleForCreationDto.CollectionDate;
        newSample.ReceivedDate = containerlessSampleForCreationDto.ReceivedDate;
        newSample.CollectionSite = containerlessSampleForCreationDto.CollectionSite;
        newSample.PatientId = containerlessSampleForCreationDto.PatientId;
        newSample.ParentSampleId = containerlessSampleForCreationDto.ParentSampleId;

        newSample.QueueDomainEvent(new SampleCreated(){ Sample = newSample });
        
        return newSample;
    }

    public void Update(ContainerlessSampleForUpdateDto containerlessSampleForUpdateDto)
    {
        new SampleForUpdateDtoValidator().ValidateAndThrow(containerlessSampleForUpdateDto);

        Type = new SampleType(containerlessSampleForUpdateDto.Type);
        Quantity = containerlessSampleForUpdateDto.Quantity;
        CollectionDate = containerlessSampleForUpdateDto.CollectionDate;
        ReceivedDate = containerlessSampleForUpdateDto.ReceivedDate;
        CollectionSite = containerlessSampleForUpdateDto.CollectionSite;
        PatientId = containerlessSampleForUpdateDto.PatientId;
        ParentSampleId = containerlessSampleForUpdateDto.ParentSampleId;

        QueueDomainEvent(new SampleUpdated(){ Id = Id });
    }

    public Sample SetContainer(Container container)
    {
        if (!container.CanStore(Type))
            throw new ValidationException(nameof(Sample),
                $"A {container.Type} container is used to store {container.UsedFor.Value} samples, not {Type.Value}.");
        
        Container = container;
        ContainerId = container.Id;
        QueueDomainEvent(new SampleUpdated(){ Id = Id });
        return this;
    }

    public Sample RemoveContainer()
    {
        Container = null;
        ContainerId = null;
        QueueDomainEvent(new SampleUpdated(){ Id = Id });
        return this;
    }

    public async Task SetSampleContainer(Guid? givenContainerId, IContainerRepository containerRepository, CancellationToken cancellationToken)
    {
        var passedContainerId = Guid.TryParse(givenContainerId.ToString(), out var containerId);
        if (passedContainerId)
        {
            var container = await containerRepository.GetById(containerId, cancellationToken: cancellationToken);
            SetContainer(container);
        }
        else
        {
            RemoveContainer();
        }
    }
    
    protected Sample() { } // For EF + Mocking
}