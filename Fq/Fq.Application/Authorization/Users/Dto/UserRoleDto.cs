﻿using Abp.Application.Services.Dto;

namespace Fq.Authorization.Users.Dto
{
    public class UserRoleDto : IDto
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public string RoleDisplayName { get; set; }

        public bool IsAssigned { get; set; }
    }
}