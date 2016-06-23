using Abp.AutoMapper;

namespace Fq.Game.Dto
{
    [AutoMapFrom(typeof(Transition))]
    public class GetJiaoBanDto
    {
        public long Id { get; set; }

        public virtual double JieBanGold { get; set; }

        public virtual double JieBanCard { get; set; }

        public virtual double JieBanCash { get; set; }

        //流水号
        public virtual string Code { get; set; }


        public virtual double JiaoBanGold { get; set; }

        public virtual double JiaoBanCard { get; set; }

        public virtual double JiaoBanCash { get; set; }
    }
}
