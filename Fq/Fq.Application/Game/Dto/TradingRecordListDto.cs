using System;
using Abp.Application.Services.Dto;

namespace Fq.Game.Dto
{
    public class TradingRecordListDto : EntityDto<long>
    {
        public long TransitionId { get; set; }

        public string UserName { get; set; }

        public DateTime CreateTime { get; set; }

        public string TradingType { get; set; }

        public double Amount { get; set; }

        public double Gold { get; set; }

        //支付
        public bool IsPay { get; set; }

        //交货
        public bool IsDelivery { get; set; }
    }
}
