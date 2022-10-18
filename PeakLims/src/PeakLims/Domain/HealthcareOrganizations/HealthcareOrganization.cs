namespace PeakLims.Domain.HealthcareOrganizations;

using SharedKernel.Exceptions;
using PeakLims.Domain.HealthcareOrganizations.Dtos;
using PeakLims.Domain.HealthcareOrganizations.Validators;
using PeakLims.Domain.HealthcareOrganizations.DomainEvents;
using FluentValidation;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Addresses;
using Emails;
using HealthcareOrganizationStatuses;
using Sieve.Attributes;
using PeakLims.Domain.HealthcareOrganizationContacts;


public class HealthcareOrganization : BaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string Name { get; private set; }
    public virtual Email Email { get; private set; }
    public virtual HealthcareOrganizationStatus Status { get; private set; }
    public virtual Address PrimaryAddress { get; private set; }

    [JsonIgnore]
    [IgnoreDataMember]
    public virtual ICollection<HealthcareOrganizationContact> Contacts { get; private set; }


    public static HealthcareOrganization Create(HealthcareOrganizationForCreationDto healthcareOrganizationForCreationDto)
    {
        new HealthcareOrganizationForCreationDtoValidator().ValidateAndThrow(healthcareOrganizationForCreationDto);

        var newHealthcareOrganization = new HealthcareOrganization();

        newHealthcareOrganization.Name = healthcareOrganizationForCreationDto.Name;
        newHealthcareOrganization.Email = new Email(healthcareOrganizationForCreationDto.Email);
        newHealthcareOrganization.Status = HealthcareOrganizationStatus.Active();
        newHealthcareOrganization.PrimaryAddress = new Address(healthcareOrganizationForCreationDto.PrimaryAddress.Line1,
            healthcareOrganizationForCreationDto.PrimaryAddress.Line2,
            healthcareOrganizationForCreationDto.PrimaryAddress.City,
            healthcareOrganizationForCreationDto.PrimaryAddress.State,
            healthcareOrganizationForCreationDto.PrimaryAddress.PostalCode,
            healthcareOrganizationForCreationDto.PrimaryAddress.Country);

        newHealthcareOrganization.QueueDomainEvent(new HealthcareOrganizationCreated(){ HealthcareOrganization = newHealthcareOrganization });
        
        return newHealthcareOrganization;
    }

    public void Update(HealthcareOrganizationForUpdateDto healthcareOrganizationForUpdateDto)
    {
        new HealthcareOrganizationForUpdateDtoValidator().ValidateAndThrow(healthcareOrganizationForUpdateDto);

        Name = healthcareOrganizationForUpdateDto.Name;
        Email = new Email(healthcareOrganizationForUpdateDto.Email);
        PrimaryAddress = new Address(healthcareOrganizationForUpdateDto.PrimaryAddress.Line1,
            healthcareOrganizationForUpdateDto.PrimaryAddress.Line2,
            healthcareOrganizationForUpdateDto.PrimaryAddress.City,
            healthcareOrganizationForUpdateDto.PrimaryAddress.State,
            healthcareOrganizationForUpdateDto.PrimaryAddress.PostalCode,
            healthcareOrganizationForUpdateDto.PrimaryAddress.Country);

        QueueDomainEvent(new HealthcareOrganizationUpdated(){ Id = Id });
    }

    public HealthcareOrganization Activate()
    {
        if (Status == HealthcareOrganizationStatus.Active())
            return this;
        
        Status = HealthcareOrganizationStatus.Active();
        QueueDomainEvent(new HealthcareOrganizationUpdated(){ Id = Id });
        return this;
    }

    public HealthcareOrganization Deactivate()
    {
        if (Status == HealthcareOrganizationStatus.Inactive())
            return this;
        
        Status = HealthcareOrganizationStatus.Inactive();
        QueueDomainEvent(new HealthcareOrganizationUpdated(){ Id = Id });
        return this;
    }
    
    protected HealthcareOrganization() { } // For EF + Mocking
}