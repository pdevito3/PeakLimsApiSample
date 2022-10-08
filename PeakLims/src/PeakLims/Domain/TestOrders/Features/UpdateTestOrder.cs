namespace PeakLims.Domain.TestOrders.Features;

using PeakLims.Domain.TestOrders;
using PeakLims.Domain.TestOrders.Dtos;
using PeakLims.Domain.TestOrders.Validators;
using PeakLims.Domain.TestOrders.Services;
using PeakLims.Services;
using SharedKernel.Exceptions;
using PeakLims.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class UpdateTestOrder
{
    public sealed class Command : IRequest<bool>
    {
        public readonly Guid Id;
        public readonly TestOrderForUpdateDto TestOrderToUpdate;

        public Command(Guid testOrder, TestOrderForUpdateDto newTestOrderData)
        {
            Id = testOrder;
            TestOrderToUpdate = newTestOrderData;
        }
    }

    public sealed class Handler : IRequestHandler<Command, bool>
    {
        private readonly ITestOrderRepository _testOrderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(ITestOrderRepository testOrderRepository, IUnitOfWork unitOfWork, IHeimGuardClient heimGuard)
        {
            _testOrderRepository = testOrderRepository;
            _unitOfWork = unitOfWork;
            _heimGuard = heimGuard;
        }

        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanUpdateTestOrders);

            var testOrderToUpdate = await _testOrderRepository.GetById(request.Id, cancellationToken: cancellationToken);

            testOrderToUpdate.Update(request.TestOrderToUpdate);
            _testOrderRepository.Update(testOrderToUpdate);
            return await _unitOfWork.CommitChanges(cancellationToken) >= 1;
        }
    }
}