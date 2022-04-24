using System;
using System.Collections.Generic;

namespace GrpcService1.App.Database.Model
{
    public partial class Permission
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string? Title { get; set; }
        public int? GrantedBy { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual User? GrantedByNavigation { get; set; }
        public virtual User? User { get; set; }
    }
}
