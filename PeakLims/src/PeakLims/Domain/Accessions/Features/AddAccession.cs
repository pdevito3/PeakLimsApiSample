namespace PeakLims.Domain.Accessions.Features;

using PeakLims.Domain.Accessions.Services;
using PeakLims.Domain.Accessions;
using PeakLims.Domain.Accessions.Dtos;
using PeakLims.Services;
using SharedKernel.Exceptions;
using PeakLims.Domain;
using HeimGuard;
using MapsterMapper;
using MediatR;

public static class AddAccession
{
    public sealed class Command : IRequest<AccessionDto>
    {
    }

    public sealed class Handler : IRequestHandler<Command, AccessionDto>
    {
        private readonly IAccessionRepository _accessionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IAccessionRepository accessionRepository, IUnitOfWork unitOfWork, IMapper mapper, IHeimGuardClient heimGuard)
        {
            _mapper = mapper;
            _accessionRepository = accessionRepository;
            _unitOfWork = unitOfWork;
            _heimGuard = heimGuard;
        }

        public async Task<AccessionDto> Handle(Command request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanAddAccessions);

            var accession = Accession.Create();
            await _accessionRepository.Add(accession, cancellationToken);

            await _unitOfWork.CommitChanges(cancellationToken);

            var accessionAdded = await _accessionRepository.GetById(accession.Id, cancellationToken: cancellationToken);
            return _mapper.Map<AccessionDto>(accessionAdded);
        }
    }
}