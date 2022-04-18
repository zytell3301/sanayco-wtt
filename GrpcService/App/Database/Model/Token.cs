using System;
using System.Collections.Generic;

namespace GrpcService1.App.Database.Model
{
    public partial class Token
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string? Token1 { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual User? User { get; set; }
    }
}
