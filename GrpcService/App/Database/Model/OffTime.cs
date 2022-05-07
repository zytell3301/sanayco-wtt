using System;
using System.Collections.Generic;

namespace GrpcService1.App.Database.Model
{
    public partial class OffTime
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string? Status { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual User? User { get; set; }
    }
}
