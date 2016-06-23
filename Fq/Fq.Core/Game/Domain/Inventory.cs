using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Fq.Authorization.Users;

namespace Fq.Game
{
    //库存信息表,入库包含（卡、现金、金币），把卡折算成现金
    //出库只有现金和金币
    [Table("g_inventory")]
    public class Inventory : CreationAuditedEntity<long, User>
    {
        public virtual bool IsChuKu { get; set; }

        public virtual InventoryType InventoryType { get; set; }

        public virtual double Amount { get; set; }

        public virtual long? TransitonId { get; set; }

        [ForeignKey("TransitonId")]
        public virtual Transition Transiton { get; set; }
    }

    public enum InventoryType
    {
        Card,
        Cash,
        Gold
    }
}
