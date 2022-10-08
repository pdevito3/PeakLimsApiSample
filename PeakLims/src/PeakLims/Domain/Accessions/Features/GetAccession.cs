namespace PeakLims.Domain.Accessions.Features;

using PeakLims.Domain.Accessions.Dtos;
using PeakLims.Domain.Accessions.Services;
using SharedKernel.Exceptions;
using PeakLims.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class GetAccession
{
    public sealed class Query : IRequest<AccessionDto>
    {
        public readonly Guid Id;

        public Query(Guid id)
        {
            Id = id;
        }
    }

    public sealed class Handler : IRequestHandler<Query, AccessionDto>
    {
        private readonly IAccessionRepository _accessionRepository;
        private readonly IMapper _mapper;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IAccessionRepository accessionRepository, IMapper mapper, IHeimGuardClient heimGuard)
        {
            _mapper = mapper;
            _accessionRepository = accessionRepository;
            _heimGuard = heimGuard;
        }

        public async Task<AccessionDto> Handle(Query request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanReadAccessions);

            var result = await _accessionRepository.GetById(request.Id, cancellationToken: cancellationToken);
            return _mapper.Map<AccessionDto>(result);
        }
    }
}