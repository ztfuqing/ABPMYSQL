using Abp.Application.Services.Dto;

namespace Fq.Game.Dto
{
    public class GetTradingRecordsInput : IInputDto
    {
        public long? TransitionId { get; set; }

        public string CreateUser { get; set; }

        public string UserName { get; set; }

        public double Amount { get; set; }
    }
}
