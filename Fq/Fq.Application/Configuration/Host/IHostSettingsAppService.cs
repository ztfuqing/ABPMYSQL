using System.Threading.Tasks;
using Abp.Application.Services;
using Fq.Configuration.Host.Dto;

namespace Fq.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);
    }
}
