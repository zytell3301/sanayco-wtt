using System;
using System.Collections.Generic;

namespace GrpcService1.App.Database.Model
{
    public partial class ProjectMember
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? ProjectId { get; set; }
        public string? Level { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual Project? Project { get; set; }
        public virtual User? User { get; set; }
    }
}
