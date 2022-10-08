namespace PeakLims.Domain.AccessionComments.Features;

using PeakLims.Domain.AccessionComments.Dtos;
using PeakLims.Domain.AccessionComments.Services;
using PeakLims.Wrappers;
using SharedKernel.Exceptions;
using PeakLims.Domain;
using HeimGuard;
using Microsoft.EntityFrameworkCore;
using MapsterMapper;
using Mapster;
using MediatR;
using Sieve.Models;
using Sieve.Services;

public static class GetAccessionCommentList
{
    public sealed class Query : IRequest<PagedList<AccessionCommentDto>>
    {
        public readonly AccessionCommentParametersDto QueryParameters;

        public Query(AccessionCommentParametersDto queryParameters)
        {
            QueryParameters = queryParameters;
        }
    }

    public sealed class Handler : IRequestHandler<Query, PagedList<AccessionCommentDto>>
    {
        private readonly IAccessionCommentRepository _accessionCommentRepository;
        private readonly SieveProcessor _sieveProcessor;
        private readonly IMapper _mapper;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IAccessionCommentRepository accessionCommentRepository, IMapper mapper, SieveProcessor sieveProcessor, IHeimGuardClient heimGuard)
        {
            _mapper = mapper;
            _accessionCommentRepository = accessionCommentRepository;
            _sieveProcessor = sieveProcessor;
            _heimGuard = heimGuard;
        }

        public async Task<PagedList<AccessionCommentDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanReadAccessionComments);

            var collection = _accessionCommentRepository.Query().AsNoTracking();

            var sieveModel = new SieveModel
            {
                Sorts = request.QueryParameters.SortOrder ?? "-CreatedOn",
                Filters = request.QueryParameters.Filters
            };

            var appliedCollection = _sieveProcessor.Apply(sieveModel, collection);
            var dtoCollection = appliedCollection
                .ProjectToType<AccessionCommentDto>();

            return await PagedList<AccessionCommentDto>.CreateAsync(dtoCollection,
                request.QueryParameters.PageNumber,
                request.QueryParameters.PageSize,
                cancellationToken);
        }
    }
}