using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Organizations;
using Fq.Authorization;
using Fq.Authorization.Users;
using Fq.Dto;
using Fq.Organizations.Dto;

namespace Fq.Organizations
{
    public class OrganizationUnitAppService : FqAppServiceBase, IOrganizationUnitAppService
    {
        private readonly OrganizationUnitManager _organizationUnitManager;
        private readonly IRepository<OrganizationUnit, long> _organizationUnitRepository;
        private readonly IRepository<User, long> _userRepository;
        public OrganizationUnitAppService(
            OrganizationUnitManager organizationUnitManager,
            IRepository<OrganizationUnit, long> organizationUnitRepository,
            IRepository<User, long> userRepository)
        {
            _organizationUnitManager = organizationUnitManager;
            _organizationUnitRepository = organizationUnitRepository;
            _userRepository = userRepository;
        }

        public async Task<OrganizationTreeDto> OrganizationForTree()
        {
            var result = (await _organizationUnitRepository.GetAllListAsync()).MapTo<List<FlatOrganizationDto>>();

            return new OrganizationTreeDto
            {
                Organizations = result
            };
        }

        public async Task<ListResultOutput<OrganizationUnitDto>> GetOrganizationUnits()
        {
            var query =
                from ou in _organizationUnitRepository.GetAll()
                join uou in _userRepository.GetAll() on ou.Id equals uou.OrgId into g
                select new { ou, memberCount = g.Count() };

            var items = await query.ToListAsync();

            return new ListResultOutput<OrganizationUnitDto>(
                items.Select(item =>
                {
                    var dto = item.ou.MapTo<OrganizationUnitDto>();
                    dto.MemberCount = item.memberCount;
                    return dto;
                }).ToList());
        }

        [AbpAuthorize(AppPermissions.Admin_OrganizationUnits)]
        public async Task<OrganizationUnitDto> CreateOrganizationUnit(CreateOrganizationUnitInput input)
        {
            var organizationUnit = new OrganizationUnit(AbpSession.TenantId, input.DisplayName, input.ParentId);

            await _organizationUnitManager.CreateAsync(organizationUnit);
            await CurrentUnitOfWork.SaveChangesAsync();

            return organizationUnit.MapTo<OrganizationUnitDto>();
        }

        [AbpAuthorize(AppPermissions.Admin_OrganizationUnits)]
        public async Task<OrganizationUnitDto> UpdateOrganizationUnit(UpdateOrganizationUnitInput input)
        {
            var organizationUnit = await _organizationUnitRepository.GetAsync(input.Id);

            organizationUnit.DisplayName = input.DisplayName;

            await _organizationUnitManager.UpdateAsync(organizationUnit);

            return await CreateOrganizationUnitDto(organizationUnit);
        }

        [AbpAuthorize(AppPermissions.Admin_OrganizationUnits)]
        public async Task<OrganizationUnitDto> MoveOrganizationUnit(MoveOrganizationUnitInput input)
        {
            await _organizationUnitManager.MoveAsync(input.Id, input.NewParentId);

            return await CreateOrganizationUnitDto(
                await _organizationUnitRepository.GetAsync(input.Id)
                );
        }

        [AbpAuthorize(AppPermissions.Admin_OrganizationUnits)]
        public async Task DeleteOrganizationUnit(IdInput<long> input)
        {
            await _organizationUnitManager.DeleteAsync(input.Id);
        }


        private async Task<OrganizationUnitDto> CreateOrganizationUnitDto(OrganizationUnit organizationUnit)
        {
            var dto = organizationUnit.MapTo<OrganizationUnitDto>();
            dto.MemberCount = await _userRepository.CountAsync(u => u.OrgId == organizationUnit.Id);
            return dto;
        }
    }
}
