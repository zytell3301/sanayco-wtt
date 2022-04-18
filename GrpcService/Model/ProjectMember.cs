using System;
using System.Collections.Generic;

namespace GrpcService1.Model
{
    public partial class ProjectMember
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? ProjectId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Level { get; set; }
    }
}
