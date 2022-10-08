namespace PeakLims.Domain.Samples.Features;

using PeakLims.Domain.Samples.Dtos;
using PeakLims.Domain.Samples.Services;
using SharedKernel.Exceptions;
using PeakLims.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class GetSample
{
    public sealed class Query : IRequest<SampleDto>
    {
        public readonly Guid Id;

        public Query(Guid id)
        {
            Id = id;
        }
    }

    public sealed class Handler : IRequestHandler<Query, SampleDto>
    {
        private readonly ISampleRepository _sampleRepository;
        private readonly IMapper _mapper;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(ISampleRepository sampleRepository, IMapper mapper, IHeimGuardClient heimGuard)
        {
            _mapper = mapper;
            _sampleRepository = sampleRepository;
            _heimGuard = heimGuard;
        }

        public async Task<SampleDto> Handle(Query request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanReadSamples);

            var result = await _sampleRepository.GetById(request.Id, cancellationToken: cancellationToken);
            return _mapper.Map<SampleDto>(result);
        }
    }
}