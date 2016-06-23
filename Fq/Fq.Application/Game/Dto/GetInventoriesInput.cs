using System;
using Abp.Extensions;
using Abp.Runtime.Validation;
using Fq.Dto;

namespace Fq.Game.Dto
{
    public class GetInventoriesInput : PagedAndSortedInputDto, IShouldNormalize
    {
        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string UserName { get; set; }

        public int Type { get; set; }

        public int IsChuKu { get; set; }

        public void Normalize()
        {
            if (Sorting.IsNullOrWhiteSpace())
            {
                Sorting = "Id DESC";
            }
        }
    }
}
