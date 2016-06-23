using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Organizations;

namespace Fq.Authorization.Users.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserListDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string Mobile { get; set; }

        public string FullName { get; set; }

        public string EmailAddress { get; set; }

        public OrganizationDto Organization { get; set; }

        public List<UserListRoleDto> Roles { get; set; }

        public bool IsEmailConfirmed { get; set; }

        public DateTime? LastLoginTime { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreationTime { get; set; }

        [AutoMapFrom(typeof(OrganizationUnit))]
        public class OrganizationDto
        {
            public int Id { get; set; }

            public string DisplayName { get; set; }
        }

        [AutoMapFrom(typeof(UserRole))]
        public class UserListRoleDto
        {
            public int RoleId { get; set; }

            public string RoleName { get; set; }
        }
    }
}