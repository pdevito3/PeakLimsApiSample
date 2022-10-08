namespace PeakLims.Domain.AccessionComments.Dtos;

public sealed class AccessionCommentDto 
{
        public Guid Id { get; set; }
        public string Comment { get; set; }
        public string InitialAccessionState { get; set; }
        public string EndingAccessionState { get; set; }
        public Guid AccessionId { get; set; }
        public Guid? OriginalCommentId { get; set; }
}
