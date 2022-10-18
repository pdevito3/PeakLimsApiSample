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
    public virtual Guid ContainerId { get; private set; }
    public virtual Container Container { get; private set; }
    public virtual ICollection<TestOrder> TestOrders { get; private set; } = new List<TestOrder>();


    public static Sample Create(ContainerlessSampleForCreationDto containerlessSampleForCreationDto, Container container)
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
        newSample.SetContainer(container);

        newSample.QueueDomainEvent(new SampleCreated(){ Sample = newSample });
        
        return newSample;
    }

    public void Update(ContainerlessSampleForUpdateDto containerlessSampleForUpdateDto, Container container)
    {
        new SampleForUpdateDtoValidator().ValidateAndThrow(containerlessSampleForUpdateDto);

        Type = new SampleType(containerlessSampleForUpdateDto.Type);
        Quantity = containerlessSampleForUpdateDto.Quantity;
        CollectionDate = containerlessSampleForUpdateDto.CollectionDate;
        ReceivedDate = containerlessSampleForUpdateDto.ReceivedDate;
        CollectionSite = containerlessSampleForUpdateDto.CollectionSite;
        PatientId = containerlessSampleForUpdateDto.PatientId;
        ParentSampleId = containerlessSampleForUpdateDto.ParentSampleId;
        SetContainer(container);

        QueueDomainEvent(new SampleUpdated(){ Id = Id });
    }

    private Sample SetContainer(Container container)
    {
        if (!container.CanStore(Type))
            throw new ValidationException(nameof(Sample),
                $"A {container.Type} container is used to store {container.UsedFor.Value} samples, not {Type.Value}.");
        if (!container.Status.IsActive())
            throw new ValidationException(nameof(Sample),
                $"Only active containers can be added to a sample.");
        
        Container = container;
        ContainerId = container.Id;
        return this;
    }
    
    protected Sample() { } // For EF + Mocking
}