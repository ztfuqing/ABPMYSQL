using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Organizations;
using Fq.Authorization.Users;

namespace Fq.Game
{
    [Table("g_tradingrecord")]
    public class TradingRecord : FullAuditedEntity<long, User>
    {
        public TradingRecord()
        {
            IsPay = true;
            IsDelivery = true;
        }

        
        public virtual long TransitonId { get; set; }

        [ForeignKey("TransitonId")]
        public virtual Transition Transiton { get; set; }

        public virtual string UserName { get; set; }

        public virtual TradingType TradingType { get; set; }

        public virtual double Amount { get; set; }

        public virtual double Gold { get; set; }

        //支付
        public virtual bool IsPay { get; set; }

        //交货
        public virtual bool IsDelivery { get; set; }

        public virtual long OrgId { get; set; }

        [ForeignKey("OrgId")]
        public virtual OrganizationUnit Organization { get; set; }

        public static TradingRecord Create(string userName, TradingType type, double amount, long orgId)
        {
            return new TradingRecord
            {
                UserName = userName,
                TradingType = type,
                Amount = amount,
                OrgId = orgId
            };
        }
    }

    public enum TradingType
    {
        ShengDa = 0,
        YiDong = 1,
        DianXin = 2,
        LianTong,
        ZhiFuBao,
        WeiXin,
        CaiFuTong,
        YinHang,
        KaTiXian,
        XianJinTiXian,
        ShouFen,
        SongFen,
        ShouXuFei
    }
}
