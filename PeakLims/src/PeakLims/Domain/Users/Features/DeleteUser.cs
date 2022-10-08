namespace PeakLims.Domain.Users.Features;

using PeakLims.Domain.Users.Services;
using PeakLims.Services;
using SharedKernel.Exceptions;
using PeakLims.Domain;
using HeimGuard;
using MediatR;

public static class DeleteUser
{
    public sealed class Command : IRequest<bool>
    {
        public readonly Guid Id;

        public Command(Guid user)
        {
            Id = user;
        }
    }

    public sealed class Handler : IRequestHandler<Command, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IUserRepository userRepository, IUnitOfWork unitOfWork, IHeimGuardClient heimGuard)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _heimGuard = heimGuard;
        }

        public async Task<bool> Handle(Command request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanDeleteUsers);

            var recordToDelete = await _userRepository.GetById(request.Id, cancellationToken: cancellationToken);

            _userRepository.Remove(recordToDelete);
            return await _unitOfWork.CommitChanges(cancellationToken) >= 1;
        }
    }
}