using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace Fq.Game.Dto
{
    public class TradingRecordDto : IDoubleWayDto
    {
        public long TransitionId { get; set; }

        public string UserName { get; set; }

        public double? ShengDa { get; set; }
        public double? YiDong { get; set; }
        public double? DianXin { get; set; }
        public double? LianTong { get; set; }
        public double? KaTiXian { get; set; }
        public double? ZhiFuBao { get; set; }
        public double? WeiXin { get; set; }
        public double? CaiFuTong { get; set; }
        public double? YinHang { get; set; }
        public double? XianJinTiXian { get; set; }
        public double? ShouFen { get; set; }
        public double? SongFen { get; set; }
        public double? ShouXuFei { get; set; }
    }
}
