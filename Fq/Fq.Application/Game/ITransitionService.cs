using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Fq.Game.Dto;

namespace Fq.Game
{
    public interface ITransitionService : IApplicationService
    {
        TransitionStatus GetTransitionStatus();

        Task<JieBanDto> GetPrevTransition();

        Task<CurrentTransitionDto> GetCurrentTransition();

        Task<GetJiaoBanDto> GetCurrentJiaoBanInfo();

        Task JieBan(long transitionId);

        Task JiaoBan(GetJiaoBanDto input);
    }
}
