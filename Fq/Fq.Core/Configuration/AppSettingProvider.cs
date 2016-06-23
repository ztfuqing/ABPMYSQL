using System.Collections.Generic;
using Abp.Configuration;

namespace Fq.Configuration
{
    public class AppSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
                   {
                       new SettingDefinition(AppSettings.SystemName, "测试系统"),
                   };
        }
    }
}
