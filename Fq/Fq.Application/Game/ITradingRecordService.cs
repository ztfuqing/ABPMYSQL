using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Fq.Game.Dto;

namespace Fq.Game
{
    public interface ITradingRecordService : IApplicationService
    {
        Task<TradingRecordDto> GetNewTradingRecord(IdInput<long> input);

        Task CreateTradingRecord(TradingRecordDto input);

        Task<List<TradingRecordListDto>> GetCurrentTransitionTradingRecords(GetTradingRecordsInput input);
    }
}
