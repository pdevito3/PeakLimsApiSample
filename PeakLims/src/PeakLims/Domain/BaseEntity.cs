namespace PeakLims.Domain;

using Sieve.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public abstract class BaseEntity
{
    [Key]
    [Sieve(CanFilter = true, CanSort = true)]
    public virtual Guid Id { get; private set; } = Guid.NewGuid();
    
    [Sieve(CanFilter = true, CanSort = true)]
    public virtual DateTime CreatedOn { get; private set; }
    
    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string CreatedBy { get; private set; }
    
    [Sieve(CanFilter = true, CanSort = true)]
    public virtual DateTime? LastModifiedOn { get; private set; }
    
    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string LastModifiedBy { get; private set; }
    public bool IsDeleted { get; private set; }
    
    [NotMapped]
    public List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();

    public void UpdateCreationProperties(DateTime createdOn, string createdBy)
    {
        CreatedOn = createdOn;
        CreatedBy = createdBy;
    }
    
    public void UpdateModifiedProperties(DateTime? lastModifiedOn, string lastModifiedBy)
    {
        LastModifiedOn = lastModifiedOn;
        LastModifiedBy = lastModifiedBy;
    }
    
    public void UpdateIsDeleted(bool isDeleted)
    {
        IsDeleted = isDeleted;
    }
    
    public void QueueDomainEvent(DomainEvent @event)
    {
        if(!DomainEvents.Contains(@event))
            DomainEvents.Add(@event);
    }
}