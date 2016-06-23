using System.Collections.Generic;
using Fq.Auditing.Dto;
using Fq.Dto;

namespace Fq.Auditing.Exporting
{
    public interface IAuditLogListExcelExporter
    {
        FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos);
    }
}
