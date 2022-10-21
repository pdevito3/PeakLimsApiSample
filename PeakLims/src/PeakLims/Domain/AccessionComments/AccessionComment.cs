namespace PeakLims.Domain.AccessionComments;

using SharedKernel.Exceptions;
using PeakLims.Domain.AccessionComments.Dtos;
using PeakLims.Domain.AccessionComments.DomainEvents;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using AccessionCommentStatuses;
using PeakLims.Domain.Accessions;
using ValidationException = SharedKernel.Exceptions.ValidationException;

public class AccessionComment : BaseEntity
{
    public virtual string Comment { get; private set; }
    public virtual AccessionCommentStatus Status { get; private set; }

    [JsonIgnore]
    [IgnoreDataMember]
    [ForeignKey("Accession")]
    public virtual Guid AccessionId { get; private set; }
    public virtual Accession Accession { get; private set; }

    [JsonIgnore]
    [IgnoreDataMember]
    [ForeignKey("AccessionComment")]
    public virtual Guid? ParentAccessionCommentId { get; private set; }
    public virtual AccessionComment ParentAccessionComment { get; private set; }


    public static AccessionComment Create(Accession accession, string commentText)
    {
        GuardCommentNotEmptyOrNull(commentText);
        
        var newAccessionComment = new AccessionComment
        {
            Comment = commentText,
            Accession = accession,
            AccessionId = accession.Id,
            ParentAccessionCommentId = null,
            ParentAccessionComment = null,
            Status = AccessionCommentStatus.Active()
        };

        newAccessionComment.QueueDomainEvent(new AccessionCommentCreated(){ AccessionComment = newAccessionComment });
        
        return newAccessionComment;
    }

    public void Update(string commentText, out AccessionComment newComment, out AccessionComment archivedComment)
    {
        GuardCommentNotEmptyOrNull(commentText);
        newComment = new AccessionComment
        {
            Comment = commentText,
            AccessionId = AccessionId,
            Accession = Accession,
            ParentAccessionCommentId = Id,
            ParentAccessionComment = this,
            Status = AccessionCommentStatus.Active()
        };

        Status = AccessionCommentStatus.Archived();
        archivedComment = this;
        
        QueueDomainEvent(new AccessionCommentUpdated(){ Id = Id });
        newComment.QueueDomainEvent(new AccessionCommentCreated(){ AccessionComment = newComment });
    }

    private static void GuardCommentNotEmptyOrNull(string commentText)
    {
        new ValidationException("Please provide a valid comment.").ThrowWhenNullOrEmpty(commentText);
    }
    
    protected AccessionComment() { } // For EF + Mocking
}