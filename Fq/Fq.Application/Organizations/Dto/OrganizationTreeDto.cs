using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Fq.Dto;

namespace Fq.Organizations.Dto
{
    public class OrganizationTreeDto : IOutputDto
    {
        public List<FlatOrganizationDto> Organizations { get; set; }

        public long SelectedId { get; set; }

        public long SelectedName { get; set; }
    }
}
