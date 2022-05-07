using System;
using System.Collections.Generic;

namespace GrpcService1.App.Database.Model
{
    public partial class User
    {
        public User()
        {
            FoodOrders = new HashSet<FoodOrder>();
            Missions = new HashSet<Mission>();
            OffTimes = new HashSet<OffTime>();
            PermissionGrantedByNavigations = new HashSet<Permission>();
            PermissionUsers = new HashSet<Permission>();
            Presentations = new HashSet<Presentation>();
            ProjectMembers = new HashSet<ProjectMember>();
            Tasks = new HashSet<Task>();
            Tokens = new HashSet<Token>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Lastname { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Password { get; set; }
        public string? Username { get; set; }
        public string? SkillLevel { get; set; }
        public string? CompanyLevel { get; set; }

        public virtual ICollection<FoodOrder> FoodOrders { get; set; }
        public virtual ICollection<Mission> Missions { get; set; }
        public virtual ICollection<OffTime> OffTimes { get; set; }
        public virtual ICollection<Permission> PermissionGrantedByNavigations { get; set; }
        public virtual ICollection<Permission> PermissionUsers { get; set; }
        public virtual ICollection<Presentation> Presentations { get; set; }
        public virtual ICollection<ProjectMember> ProjectMembers { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }
        public virtual ICollection<Token> Tokens { get; set; }
    }
}
