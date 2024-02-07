using Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Organization : AuditableBaseEntity
    {
        [MaxLength(500)]
        public required string Name { get; set; }

        [MaxLength(100)]
        public required string SlugTenant { get; set; }

        public virtual ICollection<UserOrganization> UserOrganizations { get; } = new List<UserOrganization>();

    }
}
