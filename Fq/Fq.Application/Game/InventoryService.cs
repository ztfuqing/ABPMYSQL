using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Linq.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Fq.Game.Dto;
using System.Data.Entity;
using System.Linq.Dynamic;

namespace Fq.Game
{
    public class InventoryService : FqAppServiceBase, IInventoryService
    {
        private readonly IRepository<Inventory, long> _inventoryRepository;

        public InventoryService(IRepository<Inventory, long> inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<PagedResultOutput<InventoryListDto>> GetInventories(GetInventoriesInput input)
        {
            var query = _inventoryRepository.GetAll()
             .WhereIf(
                 !input.UserName.IsNullOrWhiteSpace(),
                 u => u.CreatorUser != null || u.CreatorUser.UserName.Contains(input.UserName));

            var inventoryCount = await query.CountAsync();
            var inventories = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            var listDtos = inventories.Select(a => new InventoryListDto
            {
                Amount = a.Amount,
                CreationTime = a.CreationTime,
                Id = a.Id,
                InventoryType = a.InventoryType,
                IsChuKu = a.IsChuKu,
                TransitonId = a.TransitonId,
                UserId = a.CreatorUserId.Value,
                UserName = a.CreatorUser.Surname
            }).ToList();

            return new PagedResultOutput<InventoryListDto>(
                inventoryCount,
                listDtos
                );
        }
    }
}
