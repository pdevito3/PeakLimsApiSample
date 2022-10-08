namespace PeakLims.Domain.AccessionComments.Features;

using PeakLims.Domain.AccessionComments;
using PeakLims.Domain.AccessionComments.Dtos;
using PeakLims.Domain.AccessionComments.Validators;
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
        public readonly Guid Id;
        public readonly AccessionCommentForUpdateDto AccessionCommentToUpdate;

        public Command(Guid accessionComment, AccessionCommentForUpdateDto newAccessionCommentData)
        {
            Id = accessionComment;
            AccessionCommentToUpdate = newAccessionCommentData;
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

            var accessionCommentToUpdate = await _accessionCommentRepository.GetById(request.Id, cancellationToken: cancellationToken);

            accessionCommentToUpdate.Update(request.AccessionCommentToUpdate);
            _accessionCommentRepository.Update(accessionCommentToUpdate);
            return await _unitOfWork.CommitChanges(cancellationToken) >= 1;
        }
    }
}