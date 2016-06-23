using System.Collections.Generic;
using Abp.Extensions;
using Fq.Auditing.Dto;
using Fq.DataExporting.Excel.EpPlus;
using Fq.Dto;

namespace Fq.Auditing.Exporting
{
    public class AuditLogListExcelExporter : EpPlusExcelExporterBase, IAuditLogListExcelExporter
    {
        public FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos)
        {
            return CreateExcelPackage(
                "AuditLogs.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("AuditLogs"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("时间"),
                        L("用户名"),
                        L("服务"),
                        L("动作"),
                        L("参数"),
                        L("持续时间"),
                        L("Ip地址"),
                        L("客户端"),
                        L("浏览器"),
                        L("状态")
                    );

                    AddObjects(
                        sheet, 2, auditLogListDtos,
                        _ => _.ExecutionTime,
                        _ => _.UserName,
                        _ => _.ServiceName,
                        _ => _.MethodName,
                        _ => _.Parameters,
                        _ => _.ExecutionDuration,
                        _ => _.ClientIpAddress,
                        _ => _.ClientName,
                        _ => _.BrowserInfo,
                        _ => _.Exception.IsNullOrEmpty() ? L("Success") : _.Exception
                        );

                    //Formatting cells

                    var timeColumn = sheet.Column(1);
                    timeColumn.Style.Numberformat.Format = "mm-dd-yy hh:mm:ss";

                    for (var i = 1; i <= 10; i++)
                    {
                        if (i.IsIn(5, 10)) //Don't AutoFit Parameters and Exception
                        {
                            continue;
                        }

                        sheet.Column(i).AutoFit();
                    }
                });
        }
    }
}