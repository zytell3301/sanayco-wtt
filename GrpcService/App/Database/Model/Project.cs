using System;
using System.Collections.Generic;

namespace GrpcService1.App.Database.Model
{
    public partial class Project
    {
        public Project()
        {
            Missions = new HashSet<Mission>();
            ProjectMembers = new HashSet<ProjectMember>();
            Tasks = new HashSet<Task>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual ICollection<Mission> Missions { get; set; }
        public virtual ICollection<ProjectMember> ProjectMembers { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
    }
}
