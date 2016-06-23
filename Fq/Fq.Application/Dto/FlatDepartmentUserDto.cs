using Abp.Application.Services.Dto;

namespace Fq.Dto
{
    public class FlatDepartmentUserDto : IDto
    {
        public string ParentId { get; set; }

        public string Id { get; set; }

        public string DisplayName { get; set; }
    }
}
