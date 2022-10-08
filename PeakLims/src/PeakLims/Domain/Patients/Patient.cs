namespace PeakLims.Domain.Patients;

using Ethnicities;
using PeakLims.Domain.Patients.Dtos;
using PeakLims.Domain.Patients.Validators;
using PeakLims.Domain.Patients.DomainEvents;
using FluentValidation;
using Lifespans;
using Races;
using Sexes;
using Sieve.Attributes;

public class Patient : BaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string FirstName { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string LastName { get; private set; }
    public virtual Lifespan Lifespan { get; private set; }
    public virtual Sex Sex { get; private set; }
    public virtual Race Race { get; private set; }
    public virtual Ethnicity Ethnicity { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string InternalId { get; }


    public static Patient Create(PatientForCreationDto patientForCreationDto)
    {
        new PatientForCreationDtoValidator().ValidateAndThrow(patientForCreationDto);

        var newPatient = new Patient();

        newPatient.FirstName = patientForCreationDto.FirstName;
        newPatient.LastName = patientForCreationDto.LastName;
        newPatient.Lifespan = new Lifespan(patientForCreationDto.Lifespan.Age, patientForCreationDto.Lifespan.DateOfBirth);
        newPatient.Race = new Race(patientForCreationDto.Race);
        newPatient.Ethnicity = new Ethnicity(patientForCreationDto.Ethnicity);
        newPatient.Sex = new Sex(patientForCreationDto.Sex);

        newPatient.QueueDomainEvent(new PatientCreated(){ Patient = newPatient });
        
        return newPatient;
    }

    public void Update(PatientForUpdateDto patientForUpdateDto)
    {
        new PatientForUpdateDtoValidator().ValidateAndThrow(patientForUpdateDto);

        FirstName = patientForUpdateDto.FirstName;
        LastName = patientForUpdateDto.LastName;
        Lifespan = new Lifespan(patientForUpdateDto.Lifespan.Age, patientForUpdateDto.Lifespan.DateOfBirth);
        Race = new Race(patientForUpdateDto.Race);
        Ethnicity = new Ethnicity(patientForUpdateDto.Ethnicity);
        Sex = new Sex(patientForUpdateDto.Sex);

        QueueDomainEvent(new PatientUpdated(){ Id = Id });
    }
    
    protected Patient() { } // For EF + Mocking
}