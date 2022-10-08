namespace PeakLims.Domain.Users;

using PeakLims.Domain.Users.Dtos;
using PeakLims.Domain.Users.Validators;
using PeakLims.Domain.Users.DomainEvents;
using PeakLims.Domain.Emails;
using Roles;
using FluentValidation;
using System.Text.Json.Serialization;
using System.Runtime.Serialization;
using Sieve.Attributes;

public class User : BaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string Identifier { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string FirstName { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string LastName { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual Email Email { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string Username { get; private set; }

    [JsonIgnore]
    [IgnoreDataMember]
    public virtual ICollection<UserRole> Roles { get; private set; } = new List<UserRole>();


    public static User Create(UserForCreationDto userForCreationDto)
    {
        new UserForCreationDtoValidator().ValidateAndThrow(userForCreationDto);

        var newUser = new User();

        newUser.Identifier = userForCreationDto.Identifier;
        newUser.FirstName = userForCreationDto.FirstName;
        newUser.LastName = userForCreationDto.LastName;
        newUser.Email = new Email(userForCreationDto.Email);
        newUser.Username = userForCreationDto.Username;

        newUser.QueueDomainEvent(new UserCreated(){ User = newUser });
        
        return newUser;
    }

    public void Update(UserForUpdateDto userForUpdateDto)
    {
        new UserForUpdateDtoValidator().ValidateAndThrow(userForUpdateDto);

        Identifier = userForUpdateDto.Identifier;
        FirstName = userForUpdateDto.FirstName;
        LastName = userForUpdateDto.LastName;
        Email = new Email(userForUpdateDto.Email);
        Username = userForUpdateDto.Username;

        QueueDomainEvent(new UserUpdated(){ Id = Id });
    }

    public UserRole AddRole(Role role)
    {
        var newList = Roles.ToList();
        var userRole = UserRole.Create(Id, role);
        newList.Add(userRole);
        UpdateRoles(newList);
        return userRole;
    }

    public UserRole RemoveRole(Role role)
    {
        var newList = Roles.ToList();
        var roleToRemove = Roles.FirstOrDefault(x => x.Role == role);
        newList.Remove(roleToRemove);
        UpdateRoles(newList);
        return roleToRemove;
    }

    private void UpdateRoles(IList<UserRole> updates)
    {
        var additions = updates.Where(userRole => Roles.All(x => x.Role != userRole.Role)).ToList();
        var removals = Roles.Where(userRole => updates.All(x => x.Role != userRole.Role)).ToList();
    
        var newList = Roles.ToList();
        removals.ForEach(toRemove => newList.Remove(toRemove));
        additions.ForEach(newRole => newList.Add(newRole));
        Roles = newList;
        QueueDomainEvent(new UserRolesUpdated(){ UserId = Id });
    }
    
    protected User() { } // For EF + Mocking
}