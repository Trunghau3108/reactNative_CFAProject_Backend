﻿using System;
using System.Collections.Generic;

namespace CFAProject_Backend.Models
{
    public partial class WebAction
    {
        public WebAction()
        {
            RoleActions = new HashSet<RoleAction>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        public virtual ICollection<RoleAction> RoleActions { get; set; }
    }
}
