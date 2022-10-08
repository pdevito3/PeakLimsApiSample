namespace PeakLims.Domain.HealthcareOrganizationContacts;

using SharedKernel.Exceptions;
using PeakLims.Domain.HealthcareOrganizationContacts.Dtos;
using PeakLims.Domain.HealthcareOrganizationContacts.Validators;
using PeakLims.Domain.HealthcareOrganizationContacts.DomainEvents;
using FluentValidation;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Emails;
using Sieve.Attributes;
using PeakLims.Domain.HealthcareOrganizations;


public class HealthcareOrganizationContact : BaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string Name { get; private set; }

    public virtual Email Email { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string Npi { get; private set; }

    [Required]
    [JsonIgnore]
    [IgnoreDataMember]
    [ForeignKey("HealthcareOrganization")]
    public virtual Guid HealthcareOrganizationId { get; private set; }
    public virtual HealthcareOrganization HealthcareOrganization { get; private set; }


    public static HealthcareOrganizationContact Create(HealthcareOrganizationContactForCreationDto healthcareOrganizationContactForCreationDto)
    {
        new HealthcareOrganizationContactForCreationDtoValidator().ValidateAndThrow(healthcareOrganizationContactForCreationDto);

        var newHealthcareOrganizationContact = new HealthcareOrganizationContact();

        newHealthcareOrganizationContact.Name = healthcareOrganizationContactForCreationDto.Name;
        newHealthcareOrganizationContact.Email = new Email(healthcareOrganizationContactForCreationDto.Email);
        newHealthcareOrganizationContact.Npi = healthcareOrganizationContactForCreationDto.Npi;
        newHealthcareOrganizationContact.HealthcareOrganizationId = healthcareOrganizationContactForCreationDto.HealthcareOrganizationId;

        newHealthcareOrganizationContact.QueueDomainEvent(new HealthcareOrganizationContactCreated(){ HealthcareOrganizationContact = newHealthcareOrganizationContact });
        
        return newHealthcareOrganizationContact;
    }

    public void Update(HealthcareOrganizationContactForUpdateDto healthcareOrganizationContactForUpdateDto)
    {
        new HealthcareOrganizationContactForUpdateDtoValidator().ValidateAndThrow(healthcareOrganizationContactForUpdateDto);

        Name = healthcareOrganizationContactForUpdateDto.Name;
        Email = new Email(healthcareOrganizationContactForUpdateDto.Email);
        Npi = healthcareOrganizationContactForUpdateDto.Npi;
        HealthcareOrganizationId = healthcareOrganizationContactForUpdateDto.HealthcareOrganizationId;

        QueueDomainEvent(new HealthcareOrganizationContactUpdated(){ Id = Id });
    }
    
    protected HealthcareOrganizationContact() { } // For EF + Mocking
}