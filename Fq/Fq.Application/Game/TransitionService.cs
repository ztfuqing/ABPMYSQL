using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.UI;
using Fq.Game.Dto;

namespace Fq.Game
{
    public class TransitionService : FqAppServiceBase, ITransitionService
    {
        private readonly IRepository<Transition, long> _transitionRepository;
        private readonly IRepository<TradingRecord, long> _tradingRecordRepository;
        private readonly IRepository<Inventory, long> _inventoryRepository;

        public TransitionService(IRepository<Transition, long> transitionRepository, IRepository<Inventory, long> inventoryRepository, IRepository<TradingRecord, long> tradingRecordRepository)
        {
            _transitionRepository = transitionRepository;
            _tradingRecordRepository = tradingRecordRepository;
            _inventoryRepository = inventoryRepository;
        }

        public async Task<GetJiaoBanDto> GetCurrentJiaoBanInfo()
        {
            var trans = await _transitionRepository.FirstOrDefaultAsync(a => a.CreatorUserId == UserManager.AbpSession.UserId && a.IsComplete == false);

            if (trans == null)
            {
                throw new UserFriendlyException("没有当前用户的值班信息");
            }

            var result = trans.MapTo<GetJiaoBanDto>();

            var cardAmount = _tradingRecordRepository.GetAll().Where(a => a.TransitonId == trans.Id)
                .Where(a => a.TradingType == TradingType.ShengDa ||
                        a.TradingType == TradingType.YiDong ||
                        a.TradingType == TradingType.DianXin ||
                        a.TradingType == TradingType.LianTong)
                .Select(a => (double?)a.Amount).Sum() ?? 0;


            var cashAmount = _tradingRecordRepository.GetAll().Where(a => a.TransitonId == trans.Id)
                .Where(a => a.TradingType == TradingType.CaiFuTong ||
                        a.TradingType == TradingType.YinHang ||
                        a.TradingType == TradingType.ZhiFuBao ||
                        a.TradingType == TradingType.WeiXin)
                .Select(a => (double?)a.Amount).Sum() ?? 0;

            var goldAmount = _tradingRecordRepository.GetAll().Where(a => a.TransitonId == trans.Id).Select(a => (double?)a.Gold).Sum() ?? 0; ;

            var inventories = await _inventoryRepository.GetAllListAsync(a => a.TransitonId == trans.Id);

            var inventorCard = inventories.Where(a => a.InventoryType == InventoryType.Card && a.IsChuKu == true).Sum(b => b.Amount) -
                               inventories.Where(a => a.InventoryType == InventoryType.Card && a.IsChuKu == false).Sum(b => b.Amount);
            var inventorCash = inventories.Where(a => a.InventoryType == InventoryType.Cash && a.IsChuKu == true).Sum(b => b.Amount) -
                               inventories.Where(a => a.InventoryType == InventoryType.Cash && a.IsChuKu == false).Sum(b => b.Amount);

            var inventorGold = inventories.Where(a => a.InventoryType == InventoryType.Gold && a.IsChuKu == true).Sum(b => b.Amount) -
                               inventories.Where(a => a.InventoryType == InventoryType.Gold && a.IsChuKu == false).Sum(b => b.Amount);

            result.JiaoBanCard = result.JieBanCard + cardAmount + inventorCard;
            result.JiaoBanCash = result.JieBanCash + cashAmount + inventorCash;
            result.JiaoBanGold = result.JieBanGold - goldAmount + inventorGold;

            return result;

        }

        public async Task<CurrentTransitionDto> GetCurrentTransition()
        {
            var trans = await _transitionRepository.FirstOrDefaultAsync(a => a.CreatorUserId == UserManager.AbpSession.UserId && a.IsComplete == false);

            if (trans == null)
            {
                throw new UserFriendlyException("没有当前用户的值班信息");
            }

            return new CurrentTransitionDto
            {
                Code = trans.Code,
                Id = trans.Id,
                IsComplete = trans.IsComplete,
                JieBanCard = trans.JieBanCard,
                JieBanCash = trans.JieBanCash,
                JieBanGold = trans.JieBanGold
            };
        }

        public async Task<JieBanDto> GetPrevTransition()
        {
            var user = GetCurrentUser();
            var orgId = user.OrgId;
            var trans = await _transitionRepository.FirstOrDefaultAsync(a => a.CreatorUser.OrgId == orgId && a.IsComplete == true && a.IsFinish == false);

            if (trans == null)
                throw new UserFriendlyException("上个班次未完成，无法接班");

            return new JieBanDto
            {
                JiaoBanCard = trans.JiaoBanCard,
                JiaoBanCash = trans.JiaoBanCash,
                JiaoBanGold = trans.JiaoBanGold,
                Id = trans.Id,
                UserName = trans.CreatorUser.Surname,
                JiaoBanDateTime = trans.JiaoBanDateTime.Value
            };
        }

        public TransitionStatus GetTransitionStatus()
        {
            var user = GetCurrentUser();
            var orgId = user.OrgId;
            if (_transitionRepository.Count(a => a.CreatorUserId == user.Id && a.IsComplete == false) == 1)
            {
                return TransitionStatus.Running;
            }
            if (_transitionRepository.Count(a => a.CreatorUser.OrgId == orgId && a.IsComplete == true && a.IsFinish == false) == 1)
            {
                return TransitionStatus.Success;
            }

            return TransitionStatus.Failed;
        }

        public async Task JieBan(long transitionId)
        {
            var transition = await _transitionRepository.GetAsync(transitionId);

            transition.IsFinish = true;
            await _transitionRepository.UpdateAsync(transition);

            var newTran = new Transition
            {
                JieBanCard = transition.JiaoBanCard,
                JieBanCash = transition.JiaoBanCash,
                JieBanGold = transition.JiaoBanGold,
                ParentId = transition.Id,
                Code = Guid.NewGuid().ToString("N")
            };

            await _transitionRepository.InsertAsync(newTran);
        }

        public async Task JiaoBan(GetJiaoBanDto input)
        {
            var tran = await _transitionRepository.GetAsync(input.Id);

            tran.JiaoBanCard = input.JiaoBanCard;
            tran.JiaoBanCash = input.JiaoBanCash;
            tran.JiaoBanGold = input.JiaoBanGold;
            tran.JiaoBanDateTime = DateTime.Now;
            tran.IsComplete = true;

            await _transitionRepository.UpdateAsync(tran);
        }
    }
}
