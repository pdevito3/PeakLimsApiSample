namespace PeakLims.Domain.Tests.Features;

using PeakLims.Domain.Tests;
using PeakLims.Domain.Tests.Dtos;
using PeakLims.Domain.Tests.Validators;
using PeakLims.Domain.Tests.Services;
using PeakLims.Services;
using SharedKernel.Exceptions;
using PeakLims.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class UpdateTest
{
    public sealed class Command : IRequest<bool>
    {
        public readonly Guid Id;
        public readonly TestForUpdateDto TestToUpdate;

        public Command(Guid test, TestForUpdateDto newTestData)
        {
            Id = test;
            TestToUpdate = newTestData;
        }
    }

    public sealed class Handler : IRequestHandler<Command, bool>
    {
        private readonly ITestRepository _testRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(ITestRepository testRepository, IUnitOfWork unitOfWork, IHeimGuardClient heimGuard)
        {
            _testRepository = testRepository;
            _unitOfWork = unitOfWork;
            _heimGuard = heimGuard;
        }

        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanUpdateTests);

            var testToUpdate = await _testRepository.GetById(request.Id, cancellationToken: cancellationToken);

            testToUpdate.Update(request.TestToUpdate);
            _testRepository.Update(testToUpdate);
            return await _unitOfWork.CommitChanges(cancellationToken) >= 1;
        }
    }
}