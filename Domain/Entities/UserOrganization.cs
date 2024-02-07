using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserOrganization : AuditableBaseEntity
    {
        public int UserId { get; set; }
        public int OrganizationId { get; set; }
        public virtual User User { get; set; }
        public virtual Organization Organization { get; set; }  
    }
}
