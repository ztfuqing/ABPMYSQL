using System.Threading.Tasks;
using Abp.Authorization;
using Fq.Authorization;
using Fq.Configuration.Host.Dto;

namespace Fq.Configuration.Host
{
    [AbpAuthorize]
    public class HostSettingsAppService : FqAppServiceBase, IHostSettingsAppService
    {
        public async Task<HostSettingsEditDto> GetAllSettings()
        {
            return new HostSettingsEditDto
            {
                SystemName = await SettingManager.GetSettingValueAsync(AppSettings.SystemName)
            };
        }

        [AbpAuthorize(AppPermissions.Admin_Settings)]
        public async Task UpdateAllSettings(HostSettingsEditDto input)
        {
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.SystemName, input.SystemName);
        }
    }
}
