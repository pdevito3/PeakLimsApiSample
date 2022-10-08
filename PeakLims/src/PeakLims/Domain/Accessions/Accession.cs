namespace PeakLims.Domain.Accessions;

using SharedKernel.Exceptions;
using PeakLims.Domain.Accessions.Dtos;
using PeakLims.Domain.Accessions.Validators;
using PeakLims.Domain.Accessions.DomainEvents;
using FluentValidation;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Sieve.Attributes;
using PeakLims.Domain.Patients;
using PeakLims.Domain.HealthcareOrganizations;
using PeakLims.Domain.HealthcareOrganizationContacts;
using PeakLims.Domain.PanelOrders;
using PeakLims.Domain.TestOrders;
using PeakLims.Domain.AccessionComments;


public class Accession : BaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string AccessionNumber { get; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string Status { get; private set; }

    [JsonIgnore]
    [IgnoreDataMember]
    [ForeignKey("Patient")]
    public virtual Guid? PatientId { get; private set; }
    public virtual Patient Patient { get; private set; }

    [JsonIgnore]
    [IgnoreDataMember]
    [ForeignKey("HealthcareOrganization")]
    public virtual Guid? HealthcareOrganizationId { get; private set; }
    public virtual HealthcareOrganization HealthcareOrganization { get; private set; }

    [JsonIgnore]
    [IgnoreDataMember]
    public virtual ICollection<HealthcareOrganizationContact> Contacts { get; private set; }

    [JsonIgnore]
    [IgnoreDataMember]
    public virtual ICollection<PanelOrder> PanelOrders { get; private set; }

    [JsonIgnore]
    [IgnoreDataMember]
    public virtual ICollection<TestOrder> TestOrders { get; private set; }

    [JsonIgnore]
    [IgnoreDataMember]
    public virtual ICollection<AccessionComment> Comments { get; private set; }


    public static Accession Create(AccessionForCreationDto accessionForCreationDto)
    {
        new AccessionForCreationDtoValidator().ValidateAndThrow(accessionForCreationDto);

        var newAccession = new Accession();

        newAccession.Status = accessionForCreationDto.Status;
        newAccession.PatientId = accessionForCreationDto.PatientId;
        newAccession.HealthcareOrganizationId = accessionForCreationDto.HealthcareOrganizationId;

        newAccession.QueueDomainEvent(new AccessionCreated(){ Accession = newAccession });
        
        return newAccession;
    }

    public void Update(AccessionForUpdateDto accessionForUpdateDto)
    {
        new AccessionForUpdateDtoValidator().ValidateAndThrow(accessionForUpdateDto);

        Status = accessionForUpdateDto.Status;
        PatientId = accessionForUpdateDto.PatientId;
        HealthcareOrganizationId = accessionForUpdateDto.HealthcareOrganizationId;

        QueueDomainEvent(new AccessionUpdated(){ Id = Id });
    }
    
    protected Accession() { } // For EF + Mocking
}