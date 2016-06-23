using System;

namespace Fq.Game.Dto
{
    public class JieBanDto
    {
        public long Id { get; set; }

        public string UserName { get; set; }

        public double JiaoBanGold { get; set; }

        public double JiaoBanCard { get; set; }

        public double JiaoBanCash { get; set; }

        public DateTime JiaoBanDateTime { get; set; }
    }
}
