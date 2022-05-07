using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GrpcService1.App.Database.Model
{
    public partial class wttContext : DbContext
    {
        public wttContext()
        {
        }

        public wttContext(DbContextOptions<wttContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Food> Foods { get; set; } = null!;
        public virtual DbSet<FoodOrder> FoodOrders { get; set; } = null!;
        public virtual DbSet<Mission> Missions { get; set; } = null!;
        public virtual DbSet<OffTime> OffTimes { get; set; } = null!;
        public virtual DbSet<Permission> Permissions { get; set; } = null!;
        public virtual DbSet<Presentation> Presentations { get; set; } = null!;
        public virtual DbSet<Project> Projects { get; set; } = null!;
        public virtual DbSet<ProjectMember> ProjectMembers { get; set; } = null!;
        public virtual DbSet<Task> Tasks { get; set; } = null!;
        public virtual DbSet<Token> Tokens { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost,50296;Database=wtt;Trusted_Connection=True;MultipleActiveResultSets=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Food>(entity =>
            {
                entity.ToTable("foods");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.IsAvailable).HasColumnName("is_available");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.Title)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("title");
            });

            modelBuilder.Entity<FoodOrder>(entity =>
            {
                entity.ToTable("food_orders");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.FoodId).HasColumnName("food_id");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Food)
                    .WithMany(p => p.FoodOrders)
                    .HasForeignKey(d => d.FoodId)
                    .HasConstraintName("FK_FOOD_ORDERS_FOODS_FOOD_ID_ID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.FoodOrders)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_FOODS_ORDER_USERS_USER_ID_ID");
            });

            modelBuilder.Entity<Mission>(entity =>
            {
                entity.ToTable("missions");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Description)
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.FromDate)
                    .HasColumnType("datetime")
                    .HasColumnName("from_date");

                entity.Property(e => e.IsVerified).HasColumnName("is_verified");

                entity.Property(e => e.Location)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("location");

                entity.Property(e => e.MemberId).HasColumnName("member_id");

                entity.Property(e => e.ProjectId).HasColumnName("project_id");

                entity.Property(e => e.Title)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("title");

                entity.Property(e => e.ToDate)
                    .HasColumnType("datetime")
                    .HasColumnName("to_date");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Missions)
                    .HasForeignKey(d => d.MemberId)
                    .HasConstraintName("FK_MISSIONS_MEMBER_ID_ID");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Missions)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_MISSIONS_PROJECT_ID_ID");
            });

            modelBuilder.Entity<OffTime>(entity =>
            {
                entity.ToTable("off_time");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.Description)
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.FromDate)
                    .HasColumnType("datetime")
                    .HasColumnName("from_date");

                entity.Property(e => e.Status)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("status");

                entity.Property(e => e.ToDate)
                    .HasColumnType("datetime")
                    .HasColumnName("to_date");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.OffTimes)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_OFF_TIME_USERS_USER_ID_ID");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.ToTable("permissions");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.GrantedBy).HasColumnName("granted_by");

                entity.Property(e => e.Title)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("title");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.GrantedByNavigation)
                    .WithMany(p => p.PermissionGrantedByNavigations)
                    .HasForeignKey(d => d.GrantedBy)
                    .HasConstraintName("FK_PERMISSIONS_USERS_GRANTED_BY_ID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.PermissionUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_PERMISSIONS_USERS_USER_ID_ID");
            });

            modelBuilder.Entity<Presentation>(entity =>
            {
                entity.ToTable("presentations");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.End)
                    .HasColumnType("datetime")
                    .HasColumnName("end");

                entity.Property(e => e.Start)
                    .HasColumnType("datetime")
                    .HasColumnName("start");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Presentations)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("PRESENTATIONS_USERS_USER_ID_ID");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.ToTable("projects");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.Description)
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<ProjectMember>(entity =>
            {
                entity.ToTable("project_members");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.Level)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("level");

                entity.Property(e => e.ProjectId).HasColumnName("project_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.ProjectMembers)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_PROJECT_MEMBERS_PROJECTS_PROJECT_ID_ID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ProjectMembers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_PROJECT_MEMBERS_USERS_USER_ID_ID");
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.ToTable("tasks");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.Description)
                    .HasMaxLength(1024)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.EndTime)
                    .HasColumnType("datetime")
                    .HasColumnName("end_time");

                entity.Property(e => e.ProjectId).HasColumnName("project_id");

                entity.Property(e => e.Status)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("status");

                entity.Property(e => e.Title)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("title");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.WorkLocation)
                    .HasMaxLength(16)
                    .IsUnicode(false)
                    .HasColumnName("work_location");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_TASKS_PROJECTS_PROJECT_ID_ID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_TASKS_USERS_USER_ID_ID");
            });

            modelBuilder.Entity<Token>(entity =>
            {
                entity.ToTable("tokens");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.ExpirationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("expiration_date");

                entity.Property(e => e.Token1)
                    .HasMaxLength(64)
                    .IsUnicode(false)
                    .HasColumnName("token");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Tokens)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_TOKENS_USERS_USER_ID_ID");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasIndex(e => e.Username, "USERS_USERNAME")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CompanyLevel)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("company_level");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.Lastname)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("lastname");

                entity.Property(e => e.Name)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.SkillLevel)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("skill_level");

                entity.Property(e => e.Username)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("username");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
