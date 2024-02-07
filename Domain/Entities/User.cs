using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class User : AuditableBaseEntity
    {
        [MaxLength(100)]
        public required string Name { get; set; }

        [MaxLength(100)]
        public required string Email { get; set; }

        [MaxLength(100)]
        public required string Password { get; set; }

        public int RoleId { get; set; }

        public virtual  Role Role { get; set; }

        public virtual ICollection<UserOrganization> UserOrganizations { get; set; } = new List<UserOrganization>();

    }
}
