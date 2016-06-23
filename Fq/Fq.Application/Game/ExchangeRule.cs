using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fq.Game
{
    public static class ExchangeRule
    {
        private static Dictionary<TradingType, double> list = new Dictionary<TradingType, double>();
        private static Dictionary<TradingType, string> TypeNames = new Dictionary<TradingType, string>();
        static ExchangeRule()
        {
            list.Add(TradingType.ShengDa, 100);
            list.Add(TradingType.YiDong, 100);
            list.Add(TradingType.DianXin, 100);
            list.Add(TradingType.LianTong, 100);
            list.Add(TradingType.ZhiFuBao, 100);
            list.Add(TradingType.WeiXin, 100);
            list.Add(TradingType.CaiFuTong, 100);
            list.Add(TradingType.YinHang, 100);
            list.Add(TradingType.KaTiXian, 100);
            list.Add(TradingType.XianJinTiXian, 100);
            list.Add(TradingType.ShouFen, 100);
            list.Add(TradingType.SongFen, 100);
            list.Add(TradingType.ShouXuFei, 100);

            TypeNames.Add(TradingType.ShengDa, "盛大");
            TypeNames.Add(TradingType.YiDong, "移动");
            TypeNames.Add(TradingType.DianXin, "电信");
            TypeNames.Add(TradingType.LianTong, "联通");
            TypeNames.Add(TradingType.ZhiFuBao, "支付宝");
            TypeNames.Add(TradingType.WeiXin, "微信");
            TypeNames.Add(TradingType.CaiFuTong, "财付通");
            TypeNames.Add(TradingType.YinHang, "银行");
            TypeNames.Add(TradingType.KaTiXian, "卡提现");
            TypeNames.Add(TradingType.XianJinTiXian, "现金提现");
            TypeNames.Add(TradingType.ShouFen, "收分");
            TypeNames.Add(TradingType.SongFen, "送分");
            TypeNames.Add(TradingType.ShouXuFei, "手续费");
        }

        public static double Get(TradingType type)
        {
            return list[type];
        }

        public static string GetTypeName(TradingType type)
        {
            return TypeNames[type];
        }
    }
}
