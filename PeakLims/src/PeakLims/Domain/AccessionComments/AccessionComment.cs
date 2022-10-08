namespace PeakLims.Domain.AccessionComments;

using SharedKernel.Exceptions;
using PeakLims.Domain.AccessionComments.Dtos;
using PeakLims.Domain.AccessionComments.Validators;
using PeakLims.Domain.AccessionComments.DomainEvents;
using FluentValidation;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Sieve.Attributes;
using PeakLims.Domain.Accessions;
using PeakLims.Domain.AccessionComments;


public class AccessionComment : BaseEntity
{
    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string Comment { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string InitialAccessionState { get; private set; }

    [Sieve(CanFilter = true, CanSort = true)]
    public virtual string EndingAccessionState { get; private set; }

    [JsonIgnore]
    [IgnoreDataMember]
    [ForeignKey("Accession")]
    public virtual Guid AccessionId { get; private set; }
    public virtual Accession Accession { get; private set; }

    [JsonIgnore]
    [IgnoreDataMember]
    [ForeignKey("AccessionComment")]
    public virtual Guid? OriginalCommentId { get; private set; }
    public virtual AccessionComment ParentAccessionComment { get; private set; }


    public static AccessionComment Create(AccessionCommentForCreationDto accessionCommentForCreationDto)
    {
        new AccessionCommentForCreationDtoValidator().ValidateAndThrow(accessionCommentForCreationDto);

        var newAccessionComment = new AccessionComment();

        newAccessionComment.Comment = accessionCommentForCreationDto.Comment;
        newAccessionComment.InitialAccessionState = accessionCommentForCreationDto.InitialAccessionState;
        newAccessionComment.EndingAccessionState = accessionCommentForCreationDto.EndingAccessionState;
        newAccessionComment.AccessionId = accessionCommentForCreationDto.AccessionId;
        newAccessionComment.OriginalCommentId = accessionCommentForCreationDto.OriginalCommentId;

        newAccessionComment.QueueDomainEvent(new AccessionCommentCreated(){ AccessionComment = newAccessionComment });
        
        return newAccessionComment;
    }

    public void Update(AccessionCommentForUpdateDto accessionCommentForUpdateDto)
    {
        new AccessionCommentForUpdateDtoValidator().ValidateAndThrow(accessionCommentForUpdateDto);

        Comment = accessionCommentForUpdateDto.Comment;
        InitialAccessionState = accessionCommentForUpdateDto.InitialAccessionState;
        EndingAccessionState = accessionCommentForUpdateDto.EndingAccessionState;
        AccessionId = accessionCommentForUpdateDto.AccessionId;
        OriginalCommentId = accessionCommentForUpdateDto.OriginalCommentId;

        QueueDomainEvent(new AccessionCommentUpdated(){ Id = Id });
    }
    
    protected AccessionComment() { } // For EF + Mocking
}