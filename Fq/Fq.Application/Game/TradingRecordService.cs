using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Linq.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.UI;
using Fq.Game.Dto;
using System.Data.Entity;

namespace Fq.Game
{
    public class TradingRecordService : FqAppServiceBase, ITradingRecordService
    {
        private readonly IRepository<TradingRecord, long> _tradingRecordRepository;

        public TradingRecordService(IRepository<TradingRecord, long> tradingRecordRepository)
        {

            _tradingRecordRepository = tradingRecordRepository;
        }

        public async Task CreateTradingRecord(TradingRecordDto input)
        {
            var list = new List<TradingRecord>();
            var user = await UserManager.GetUserByIdAsync(UserManager.AbpSession.UserId.Value);
            if (input.UserName.IsNullOrEmpty())
            {
                if (input.ShouXuFei.HasValue)
                {
                    list.Add(TradingRecord.Create(input.UserName, TradingType.ShouXuFei, input.ShouXuFei.Value, user.OrgId));
                }
            }
            else
            {
                if (input.KaTiXian.HasValue)
                {
                    list.Add(TradingRecord.Create(input.UserName, TradingType.KaTiXian, input.KaTiXian.Value, user.OrgId));
                }
                if (input.XianJinTiXian.HasValue)
                {
                    list.Add(TradingRecord.Create(input.UserName, TradingType.XianJinTiXian, input.XianJinTiXian.Value, user.OrgId));
                }
                if (input.ShouFen.HasValue)
                {
                    list.Add(TradingRecord.Create(input.UserName, TradingType.ShouFen, input.ShouFen.Value, user.OrgId));
                }
                if (input.SongFen.HasValue)
                {
                    list.Add(TradingRecord.Create(input.UserName, TradingType.SongFen, input.SongFen.Value, user.OrgId));
                }
                if (input.ShouXuFei.HasValue)
                {
                    list.Add(TradingRecord.Create(input.UserName, TradingType.ShouXuFei, input.ShouXuFei.Value, user.OrgId));
                }
                if (input.ShengDa.HasValue)
                {
                    list.Add(TradingRecord.Create(input.UserName, TradingType.ShengDa, input.ShengDa.Value, user.OrgId));
                }
                if (input.YiDong.HasValue)
                {
                    list.Add(TradingRecord.Create(input.UserName, TradingType.YiDong, input.YiDong.Value, user.OrgId));
                }
                if (input.DianXin.HasValue)
                {
                    list.Add(TradingRecord.Create(input.UserName, TradingType.DianXin, input.DianXin.Value, user.OrgId));
                }
                if (input.LianTong.HasValue)
                {
                    list.Add(TradingRecord.Create(input.UserName, TradingType.LianTong, input.LianTong.Value, user.OrgId));
                }
                if (input.ZhiFuBao.HasValue)
                {
                    list.Add(TradingRecord.Create(input.UserName, TradingType.ZhiFuBao, input.ZhiFuBao.Value, user.OrgId));
                }
                if (input.WeiXin.HasValue)
                {
                    list.Add(TradingRecord.Create(input.UserName, TradingType.WeiXin, input.WeiXin.Value, user.OrgId));
                }
                if (input.CaiFuTong.HasValue)
                {
                    list.Add(TradingRecord.Create(input.UserName, TradingType.CaiFuTong, input.CaiFuTong.Value, user.OrgId));
                }
                if (input.YinHang.HasValue)
                {
                    list.Add(TradingRecord.Create(input.UserName, TradingType.YinHang, input.YinHang.Value, user.OrgId));
                }
            }
            if (list.Count == 0)
            {
                throw new UserFriendlyException("请正确填写信息");
            }

            foreach (var item in list)
            {
                item.Gold = item.Amount * ExchangeRule.Get(item.TradingType);
                item.TransitonId = input.TransitionId;
                await _tradingRecordRepository.InsertAsync(item);
            }
        }

        public async Task<List<TradingRecordListDto>> GetCurrentTransitionTradingRecords(GetTradingRecordsInput input)
        {
            var result = await _tradingRecordRepository.GetAll()
                .AsNoTracking()
                .Where(a => a.CreatorUserId == UserManager.AbpSession.UserId.Value)
                .WhereIf(input.TransitionId.HasValue, a => a.TransitonId == input.TransitionId.Value)
                .WhereIf(!input.UserName.IsNullOrEmpty(), a => a.UserName.Contains(input.UserName))
                .OrderByDescending(a => a.Id)
                .ToListAsync();

            return result.Select(a => new TradingRecordListDto
            {
                Amount = a.Amount,
                CreateTime = a.CreationTime,
                Gold = a.Gold,
                Id = a.Id,
                IsDelivery = a.IsDelivery,
                IsPay = a.IsPay,
                TradingType = ExchangeRule.GetTypeName(a.TradingType),
                TransitionId = a.TransitonId,
                UserName = a.UserName
            }).ToList();

        }

        public Task<TradingRecordDto> GetNewTradingRecord(IdInput<long> input)
        {
            return Task.FromResult(new TradingRecordDto()
            {
                TransitionId = input.Id
            });
        }
    }
}
