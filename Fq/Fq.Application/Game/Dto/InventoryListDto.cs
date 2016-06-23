using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;

namespace Fq.Game.Dto
{
    public class InventoryListDto : EntityDto<long>, IHasCreationTime
    {
        public bool IsChuKu { get; set; }

        public InventoryType InventoryType { get; set; }

        public double Amount { get; set; }

        public long? TransitonId { get; set; }

        public DateTime CreationTime { get; set; }

        public long UserId { get; set; }

        public string UserName { get; set; }

        public string LeiXing
        {
            get { return IsChuKu ? "出库" : "入库"; }
        }

        public string Product
        {
            get
            {
                switch (InventoryType)
                {
                    case InventoryType.Card:
                        return "卡";
                    case InventoryType.Cash:
                        return "现金";
                    default:
                        return "金币";
                }
            }
        }
    }
}
