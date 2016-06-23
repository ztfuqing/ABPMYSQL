using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Organizations;

namespace Fq.Dto
{
    [AutoMapFrom(typeof(OrganizationUnit))]
    public class FlatOrganizationDto : IDto
    {
        public long Id { get; set; }

        public long ParentId { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }
    }
}
