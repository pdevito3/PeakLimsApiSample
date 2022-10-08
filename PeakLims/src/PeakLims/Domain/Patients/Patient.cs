namespace PeakLims.Domain.Patients;

using SharedKernel.Exceptions;
using PeakLims.Domain.Patients.Dtos;
using PeakLims.Domain.Patients.Validators;
using PeakLims.Domain.Patients.DomainEvents;
using FluentValidation;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Sieve.Attributes;
using PeakLims.Domain.Accessions;
using PeakLims.Domain.Samples;


public class Patient : BaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string FirstName { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string LastName { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual DateOnly? DateOfBirth { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual int? Age { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string Sex { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string Race { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string Ethnicity { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string InternalId { get; private set; }

    [JsonIgnore]
    [IgnoreDataMember]
    public virtual ICollection<Accession> Accessions { get; private set; }

    [JsonIgnore]
    [IgnoreDataMember]
    public virtual ICollection<Sample> Samples { get; private set; }


    public static Patient Create(PatientForCreationDto patientForCreationDto)
    {
        new PatientForCreationDtoValidator().ValidateAndThrow(patientForCreationDto);

        var newPatient = new Patient();

        newPatient.FirstName = patientForCreationDto.FirstName;
        newPatient.LastName = patientForCreationDto.LastName;
        newPatient.DateOfBirth = patientForCreationDto.DateOfBirth;
        newPatient.Age = patientForCreationDto.Age;
        newPatient.Sex = patientForCreationDto.Sex;
        newPatient.Race = patientForCreationDto.Race;
        newPatient.Ethnicity = patientForCreationDto.Ethnicity;
        newPatient.InternalId = patientForCreationDto.InternalId;

        newPatient.QueueDomainEvent(new PatientCreated(){ Patient = newPatient });
        
        return newPatient;
    }

    public void Update(PatientForUpdateDto patientForUpdateDto)
    {
        new PatientForUpdateDtoValidator().ValidateAndThrow(patientForUpdateDto);

        FirstName = patientForUpdateDto.FirstName;
        LastName = patientForUpdateDto.LastName;
        DateOfBirth = patientForUpdateDto.DateOfBirth;
        Age = patientForUpdateDto.Age;
        Sex = patientForUpdateDto.Sex;
        Race = patientForUpdateDto.Race;
        Ethnicity = patientForUpdateDto.Ethnicity;
        InternalId = patientForUpdateDto.InternalId;

        QueueDomainEvent(new PatientUpdated(){ Id = Id });
    }
    
    protected Patient() { } // For EF + Mocking
}