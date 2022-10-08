namespace PeakLims.Domain.AccessionComments.Features;

using PeakLims.Domain.AccessionComments.Services;
using PeakLims.Domain.AccessionComments;
using PeakLims.Domain.AccessionComments.Dtos;
using PeakLims.Services;
using SharedKernel.Exceptions;
using PeakLims.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class AddAccessionComment
{
    public sealed class Command : IRequest<AccessionCommentDto>
    {
        public readonly AccessionCommentForCreationDto AccessionCommentToAdd;

        public Command(AccessionCommentForCreationDto accessionCommentToAdd)
        {
            AccessionCommentToAdd = accessionCommentToAdd;
        }
    }

    public sealed class Handler : IRequestHandler<Command, AccessionCommentDto>
    {
        private readonly IAccessionCommentRepository _accessionCommentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IAccessionCommentRepository accessionCommentRepository, IUnitOfWork unitOfWork, IMapper mapper, IHeimGuardClient heimGuard)
        {
            _mapper = mapper;
            _accessionCommentRepository = accessionCommentRepository;
            _unitOfWork = unitOfWork;
            _heimGuard = heimGuard;
        }

        public async Task<AccessionCommentDto> Handle(Command request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanAddAccessionComments);

            var accessionComment = AccessionComment.Create(request.AccessionCommentToAdd);
            await _accessionCommentRepository.Add(accessionComment, cancellationToken);

            await _unitOfWork.CommitChanges(cancellationToken);

            var accessionCommentAdded = await _accessionCommentRepository.GetById(accessionComment.Id, cancellationToken: cancellationToken);
            return _mapper.Map<AccessionCommentDto>(accessionCommentAdded);
        }
    }
}