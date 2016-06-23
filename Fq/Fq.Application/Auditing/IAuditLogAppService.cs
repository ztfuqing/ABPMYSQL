using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Fq.Auditing.Dto;
using Fq.Dto;

namespace Fq.Auditing
{
    public interface IAuditLogAppService : IApplicationService
    {
        Task<PagedResultOutput<AuditLogListDto>> GetAuditLogs(GetAuditLogsInput input);

        Task<FileDto> GetAuditLogsToExcel(GetAuditLogsInput input);
    }
}