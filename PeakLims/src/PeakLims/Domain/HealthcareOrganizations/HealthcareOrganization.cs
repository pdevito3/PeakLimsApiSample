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
using Sieve.Attributes;
using PeakLims.Domain.HealthcareOrganizationContacts;


public class HealthcareOrganization : BaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string Name { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string Email { get; private set; }

    [JsonIgnore]
    [IgnoreDataMember]
    public virtual ICollection<HealthcareOrganizationContact> Contacts { get; private set; }


    public static HealthcareOrganization Create(HealthcareOrganizationForCreationDto healthcareOrganizationForCreationDto)
    {
        new HealthcareOrganizationForCreationDtoValidator().ValidateAndThrow(healthcareOrganizationForCreationDto);

        var newHealthcareOrganization = new HealthcareOrganization();

        newHealthcareOrganization.Name = healthcareOrganizationForCreationDto.Name;
        newHealthcareOrganization.Email = healthcareOrganizationForCreationDto.Email;

        newHealthcareOrganization.QueueDomainEvent(new HealthcareOrganizationCreated(){ HealthcareOrganization = newHealthcareOrganization });
        
        return newHealthcareOrganization;
    }

    public void Update(HealthcareOrganizationForUpdateDto healthcareOrganizationForUpdateDto)
    {
        new HealthcareOrganizationForUpdateDtoValidator().ValidateAndThrow(healthcareOrganizationForUpdateDto);

        Name = healthcareOrganizationForUpdateDto.Name;
        Email = healthcareOrganizationForUpdateDto.Email;

        QueueDomainEvent(new HealthcareOrganizationUpdated(){ Id = Id });
    }
    
    protected HealthcareOrganization() { } // For EF + Mocking
}