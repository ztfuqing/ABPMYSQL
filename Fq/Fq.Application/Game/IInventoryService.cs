using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Fq.Game.Dto;

namespace Fq.Game
{
    public interface IInventoryService : IApplicationService
    {
        Task<PagedResultOutput<InventoryListDto>> GetInventories(GetInventoriesInput input);
    }
}
