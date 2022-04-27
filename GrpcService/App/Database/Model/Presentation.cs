using System;
using System.Collections.Generic;

namespace GrpcService1.App.Database.Model
{
    public partial class Presentation
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }

        public virtual User? User { get; set; }
    }
}
