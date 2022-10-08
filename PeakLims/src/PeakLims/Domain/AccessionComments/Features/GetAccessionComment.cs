namespace PeakLims.Domain.AccessionComments.Features;

using PeakLims.Domain.AccessionComments.Dtos;
using PeakLims.Domain.AccessionComments.Services;
using SharedKernel.Exceptions;
using PeakLims.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class GetAccessionComment
{
    public sealed class Query : IRequest<AccessionCommentDto>
    {
        public readonly Guid Id;

        public Query(Guid id)
        {
            Id = id;
        }
    }

    public sealed class Handler : IRequestHandler<Query, AccessionCommentDto>
    {
        private readonly IAccessionCommentRepository _accessionCommentRepository;
        private readonly IMapper _mapper;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IAccessionCommentRepository accessionCommentRepository, IMapper mapper, IHeimGuardClient heimGuard)
        {
            _mapper = mapper;
            _accessionCommentRepository = accessionCommentRepository;
            _heimGuard = heimGuard;
        }

        public async Task<AccessionCommentDto> Handle(Query request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanReadAccessionComments);

            var result = await _accessionCommentRepository.GetById(request.Id, cancellationToken: cancellationToken);
            return _mapper.Map<AccessionCommentDto>(result);
        }
    }
}