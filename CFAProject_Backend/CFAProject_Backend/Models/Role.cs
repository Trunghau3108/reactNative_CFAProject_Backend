using System;
using System.Collections.Generic;

namespace CFAProject_Backend.Models
{
    public partial class Role
    {
        public Role()
        {
            RoleActions = new HashSet<RoleAction>();
            UserRoles = new HashSet<UserRole>();
        }

        public string Id { get; set; } = null!;
        public string Name { get; set; } = null!;

        public virtual ICollection<RoleAction> RoleActions { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
