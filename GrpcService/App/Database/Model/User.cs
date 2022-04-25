using System;
using System.Collections.Generic;

namespace GrpcService1.App.Database.Model
{
    public partial class User
    {
        public User()
        {
            OffTimes = new HashSet<OffTime>();
            Presentations = new HashSet<Presentation>();
            Tasks = new HashSet<Task>();
            Tokens = new HashSet<Token>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Lastname { get; set; }
        public string? SkillLevel { get; set; }
        public string? CompanyLevel { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Password { get; set; }
        public string? Username { get; set; }

        public virtual ICollection<OffTime> OffTimes { get; set; }
        public virtual ICollection<Presentation> Presentations { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
        public virtual ICollection<Token> Tokens { get; set; }
    }
}
