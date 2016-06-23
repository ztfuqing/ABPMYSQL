using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;

namespace Fq.Configuration.Host.Dto
{
    public class HostSettingsEditDto : IDoubleWayDto
    {
        [Required]
        public string SystemName { get; set; }
    }
}
