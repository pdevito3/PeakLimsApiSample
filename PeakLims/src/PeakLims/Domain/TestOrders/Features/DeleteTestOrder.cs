namespace PeakLims.Domain.TestOrders.Features;

using PeakLims.Domain.TestOrders.Services;
using PeakLims.Services;
using SharedKernel.Exceptions;
using PeakLims.Domain;
using HeimGuard;
using MediatR;

public static class DeleteTestOrder
{
    public sealed class Command : IRequest<bool>
    {
        public readonly Guid Id;

        public Command(Guid testOrder)
        {
            Id = testOrder;
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
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanDeleteTestOrders);

            var recordToDelete = await _testOrderRepository.GetById(request.Id, cancellationToken: cancellationToken);

            _testOrderRepository.Remove(recordToDelete);
            return await _unitOfWork.CommitChanges(cancellationToken) >= 1;
        }
    }
}