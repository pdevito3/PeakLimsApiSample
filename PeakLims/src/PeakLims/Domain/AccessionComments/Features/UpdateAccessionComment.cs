namespace PeakLims.Domain.AccessionComments.Features;

using PeakLims.Domain.AccessionComments;
using PeakLims.Domain.AccessionComments.Dtos;
using PeakLims.Domain.AccessionComments.Services;
using PeakLims.Services;
using SharedKernel.Exceptions;
using PeakLims.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class UpdateAccessionComment
{
    public sealed class Command : IRequest<bool>
    {
        public readonly Guid AccessionCommentId;
        public readonly string Comment;

        public Command(Guid accessionCommentId, string comment)
        {
            AccessionCommentId = accessionCommentId;
            Comment = comment;
        }
    }

    public sealed class Handler : IRequestHandler<Command, bool>
    {
        private readonly IAccessionCommentRepository _accessionCommentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IAccessionCommentRepository accessionCommentRepository, IUnitOfWork unitOfWork, IHeimGuardClient heimGuard)
        {
            _accessionCommentRepository = accessionCommentRepository;
            _unitOfWork = unitOfWork;
            _heimGuard = heimGuard;
        }

        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanUpdateAccessionComments);

            var accessionCommentToUpdate = await _accessionCommentRepository.GetById(request.AccessionCommentId, cancellationToken: cancellationToken);

            accessionCommentToUpdate.Update(request.Comment, out var newComment, out var archivedComment);
            await _accessionCommentRepository.Add(newComment, cancellationToken);
            _accessionCommentRepository.Update(archivedComment);
            
            return await _unitOfWork.CommitChanges(cancellationToken) >= 1;
        }
    }
}