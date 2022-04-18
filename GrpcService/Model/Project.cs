using System;
using System.Collections.Generic;

namespace GrpcService1.Model
{
    public partial class Project
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Description { get; set; }
    }
}
