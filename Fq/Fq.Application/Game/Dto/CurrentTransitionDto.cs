namespace Fq.Game.Dto
{
    public class CurrentTransitionDto
    {
        public long Id { get; set; }

        public virtual double JieBanGold { get; set; }

        public virtual double JieBanCard { get; set; }

        public virtual double JieBanCash { get; set; }

        //流水号
        public virtual string Code { get; set; }

        //是否交班
        public virtual bool IsComplete { get; set; }
    }
}
