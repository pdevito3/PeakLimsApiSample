namespace PeakLims.Domain.RolePermissions.Features;

using PeakLims.Domain.RolePermissions.Dtos;
using PeakLims.Domain.RolePermissions.Services;
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

public static class GetRolePermissionList
{
    public sealed class Query : IRequest<PagedList<RolePermissionDto>>
    {
        public readonly RolePermissionParametersDto QueryParameters;

        public Query(RolePermissionParametersDto queryParameters)
        {
            QueryParameters = queryParameters;
        }
    }

    public sealed class Handler : IRequestHandler<Query, PagedList<RolePermissionDto>>
    {
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly SieveProcessor _sieveProcessor;
        private readonly IMapper _mapper;
        private readonly IHeimGuardClient _heimGuard;

        public Handler(IRolePermissionRepository rolePermissionRepository, IMapper mapper, SieveProcessor sieveProcessor, IHeimGuardClient heimGuard)
        {
            _mapper = mapper;
            _rolePermissionRepository = rolePermissionRepository;
            _sieveProcessor = sieveProcessor;
            _heimGuard = heimGuard;
        }

        public async Task<PagedList<RolePermissionDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            await _heimGuard.MustHavePermission<ForbiddenAccessException>(Permissions.CanReadRolePermissions);

            var collection = _rolePermissionRepository.Query().AsNoTracking();

            var sieveModel = new SieveModel
            {
                Sorts = request.QueryParameters.SortOrder ?? "-CreatedOn",
                Filters = request.QueryParameters.Filters
            };

            var appliedCollection = _sieveProcessor.Apply(sieveModel, collection);
            var dtoCollection = appliedCollection
                .ProjectToType<RolePermissionDto>();

            return await PagedList<RolePermissionDto>.CreateAsync(dtoCollection,
                request.QueryParameters.PageNumber,
                request.QueryParameters.PageSize,
                cancellationToken);
        }
    }
}