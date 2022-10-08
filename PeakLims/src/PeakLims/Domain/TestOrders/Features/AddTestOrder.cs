namespace PeakLims.Domain.TestOrders.Features;

using PeakLims.Domain.TestOrders.Services;
using PeakLims.Domain.TestOrders;
using PeakLims.Domain.TestOrders.Dtos;
using PeakLims.Services;
using SharedKernel.Exceptions;
using PeakLims.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class AddTestOrder
{
    public sealed class Command : IRequest<TestOrderDto>
    {
        public readonly TestOrderForCreationDto TestOrderToAdd;

        public Command(TestOrderForCreationDto testOrderToAdd)
        {
            TestOrderToAdd = testOrderToAdd;
        }
    }

    public sealed class Handler : IRequestHandler<Command, TestOrderDto>
    {
        private readonly ITestOrderRepository _testOrderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(ITestOrderRepository testOrderRepository, IUnitOfWork unitOfWork, IMapper mapper, IHeimGuardClient heimGuard)
        {
            _mapper = mapper;
            _testOrderRepository = testOrderRepository;
            _unitOfWork = unitOfWork;
            _heimGuard = heimGuard;
        }

        public async Task<TestOrderDto> Handle(Command request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanAddTestOrders);

            var testOrder = TestOrder.Create(request.TestOrderToAdd);
            await _testOrderRepository.Add(testOrder, cancellationToken);

            await _unitOfWork.CommitChanges(cancellationToken);

            var testOrderAdded = await _testOrderRepository.GetById(testOrder.Id, cancellationToken: cancellationToken);
            return _mapper.Map<TestOrderDto>(testOrderAdded);
        }
    }
}