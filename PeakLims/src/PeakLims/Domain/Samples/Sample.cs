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
using Sieve.Attributes;
using PeakLims.Domain.Patients;
using PeakLims.Domain.Samples;
using PeakLims.Domain.Containers;


public class Sample : BaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string SampleNumber { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string State { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string Type { get; private set; }

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


    public static Sample Create(SampleForCreationDto sampleForCreationDto)
    {
        new SampleForCreationDtoValidator().ValidateAndThrow(sampleForCreationDto);

        var newSample = new Sample();

        newSample.SampleNumber = sampleForCreationDto.SampleNumber;
        newSample.State = sampleForCreationDto.State;
        newSample.Type = sampleForCreationDto.Type;
        newSample.Quantity = sampleForCreationDto.Quantity;
        newSample.CollectionDate = sampleForCreationDto.CollectionDate;
        newSample.ReceivedDate = sampleForCreationDto.ReceivedDate;
        newSample.CollectionSite = sampleForCreationDto.CollectionSite;
        newSample.PatientId = sampleForCreationDto.PatientId;
        newSample.ParentSampleId = sampleForCreationDto.ParentSampleId;
        newSample.ContainerId = sampleForCreationDto.ContainerId;

        newSample.QueueDomainEvent(new SampleCreated(){ Sample = newSample });
        
        return newSample;
    }

    public void Update(SampleForUpdateDto sampleForUpdateDto)
    {
        new SampleForUpdateDtoValidator().ValidateAndThrow(sampleForUpdateDto);

        SampleNumber = sampleForUpdateDto.SampleNumber;
        State = sampleForUpdateDto.State;
        Type = sampleForUpdateDto.Type;
        Quantity = sampleForUpdateDto.Quantity;
        CollectionDate = sampleForUpdateDto.CollectionDate;
        ReceivedDate = sampleForUpdateDto.ReceivedDate;
        CollectionSite = sampleForUpdateDto.CollectionSite;
        PatientId = sampleForUpdateDto.PatientId;
        ParentSampleId = sampleForUpdateDto.ParentSampleId;
        ContainerId = sampleForUpdateDto.ContainerId;

        QueueDomainEvent(new SampleUpdated(){ Id = Id });
    }
    
    protected Sample() { } // For EF + Mocking
}