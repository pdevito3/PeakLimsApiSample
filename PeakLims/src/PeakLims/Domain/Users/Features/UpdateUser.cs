namespace PeakLims.Domain.Users.Features;

using PeakLims.Domain.Users;
using PeakLims.Domain.Users.Dtos;
using PeakLims.Domain.Users.Validators;
using PeakLims.Domain.Users.Services;
using PeakLims.Services;
using SharedKernel.Exceptions;
using PeakLims.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class UpdateUser
{
    public sealed class Command : IRequest<bool>
    {
        public readonly Guid Id;
        public readonly UserForUpdateDto UserToUpdate;

        public Command(Guid user, UserForUpdateDto newUserData)
        {
            Id = user;
            UserToUpdate = newUserData;
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
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanUpdateUsers);

            var userToUpdate = await _userRepository.GetById(request.Id, cancellationToken: cancellationToken);

            userToUpdate.Update(request.UserToUpdate);
            _userRepository.Update(userToUpdate);
            return await _unitOfWork.CommitChanges(cancellationToken) >= 1;
        }
    }
}