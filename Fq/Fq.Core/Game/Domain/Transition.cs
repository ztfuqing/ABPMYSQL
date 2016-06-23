using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Fq.Authorization.Users;

namespace Fq.Game
{
    //交接班记录，每次接班时创建一条记录
    [Table("g_transition")]
    public class Transition : CreationAuditedEntity<long, User>
    {
        public Transition()
        {
            IsComplete = false;
            IsFinish = false;
        }

        public virtual long? ParentId { get; set; }

        [ForeignKey("ParentId")]
        public virtual Transition Parent { get; set; }

        public virtual double JieBanGold { get; set; }

        public virtual double JieBanCard { get; set; }

        public virtual double JieBanCash { get; set; }

        //流水号
        public virtual string Code { get; set; }

        //是否交班
        public virtual bool IsComplete { get; set; }

        public virtual DateTime? JiaoBanDateTime { get; set; }

        public virtual double JiaoBanGold { get; set; }

        public virtual double JiaoBanCard { get; set; }

        public virtual double JiaoBanCash { get; set; }

        //是否有人接班
        public virtual bool IsFinish { get; set; }
    }
}
