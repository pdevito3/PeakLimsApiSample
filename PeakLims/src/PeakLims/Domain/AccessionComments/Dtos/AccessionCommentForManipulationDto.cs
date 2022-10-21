namespace PeakLims.Domain.AccessionComments.Dtos;

public abstract class AccessionCommentForManipulationDto 
{
        public string Comment { get; set; }
        public Guid AccessionId { get; set; }
}
