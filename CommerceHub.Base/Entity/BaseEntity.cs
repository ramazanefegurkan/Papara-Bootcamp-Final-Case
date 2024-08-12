﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommerceHub.Base.Entity
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime InsertDate { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }
}
