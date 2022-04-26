﻿using System;
using System.Collections.Generic;

namespace GrpcService1.App.Database.Model
{
    public partial class Mission
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int ProjectId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Description { get; set; } = null!;

        public virtual User Member { get; set; } = null!;
        public virtual Project Project { get; set; } = null!;
    }
}