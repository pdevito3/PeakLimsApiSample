namespace PeakLims.Domain.TestOrders.Features;

using PeakLims.Domain.TestOrders.Dtos;
using PeakLims.Domain.TestOrders.Services;
using SharedKernel.Exceptions;
using PeakLims.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class GetTestOrder
{
    public sealed class Query : IRequest<TestOrderDto>
    {
        public readonly Guid Id;

        public Query(Guid id)
        {
            Id = id;
        }
    }

    public sealed class Handler : IRequestHandler<Query, TestOrderDto>
    {
        private readonly ITestOrderRepository _testOrderRepository;
        private readonly IMapper _mapper;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(ITestOrderRepository testOrderRepository, IMapper mapper, IHeimGuardClient heimGuard)
        {
            _mapper = mapper;
            _testOrderRepository = testOrderRepository;
            _heimGuard = heimGuard;
        }

        public async Task<TestOrderDto> Handle(Query request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanReadTestOrders);

            var result = await _testOrderRepository.GetById(request.Id, cancellationToken: cancellationToken);
            return _mapper.Map<TestOrderDto>(result);
        }
    }
}