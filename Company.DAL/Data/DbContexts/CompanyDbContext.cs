using Company.DAL.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.DAL.Data.DbContexts
{
    public class CompanyDbContext : DbContext
    {
        public CompanyDbContext(DbContextOptions<CompanyDbContext> options) : base(options)
        {
        }

        /*
        //connect Db
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=Company;Trusted_Connection=True;TrustServerCertificate=True");
        }
        */

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Company.DAL.Entity.Task> Tasks { get; set; }
        public DbSet<ProjectDepartment> ProjectDepartments { get; set; }
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<MeetingParticipant> MeetingParticipants { get; set; }
        public DbSet<MeetingDepartment> MeetingDepartments { get; set; }
        public DbSet<MeetingProject> MeetingProjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ProjectDepartment>()
                .HasKey(pd => new { pd.ProjectId, pd.DepartmentId });
            modelBuilder.Entity<ProjectDepartment>()
                .HasOne(pd => pd.Project)
                .WithMany(p => p.ProjectDepartments)
                .HasForeignKey(pd => pd.ProjectId);
            modelBuilder.Entity<ProjectDepartment>()
                .HasOne(pd => pd.Department)
                .WithMany(d => d.ProjectDepartments)
                .HasForeignKey(pd => pd.DepartmentId);
            modelBuilder.Entity<MeetingParticipant>()
                .HasKey(mp => new { mp.MeetingId, mp.EmployeeId });
            modelBuilder.Entity<MeetingParticipant>()
                .HasOne(mp => mp.Meeting)
                .WithMany(m => m.Participants)
                .HasForeignKey(mp => mp.MeetingId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<MeetingParticipant>()
                .HasOne(mp => mp.Employee)
                .WithMany(e => e.MeetingParticipants)
                .HasForeignKey(mp => mp.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MeetingDepartment>()
                .HasKey(md => new { md.MeetingId, md.DepartmentId });
            modelBuilder.Entity<MeetingDepartment>()
                .HasOne(md => md.Meeting)
                .WithMany(m => m.MeetingDepartments)
                .HasForeignKey(md => md.MeetingId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<MeetingDepartment>()
                .HasOne(md => md.Department)
                .WithMany(d => d.MeetingDepartments)
                .HasForeignKey(md => md.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MeetingProject>()
                .HasKey(mp => new { mp.MeetingId, mp.ProjectId });
            modelBuilder.Entity<MeetingProject>()
                .HasOne(mp => mp.Meeting)
                .WithMany(m => m.MeetingProjects)
                .HasForeignKey(mp => mp.MeetingId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<MeetingProject>()
                .HasOne(mp => mp.Project)
                .WithMany(p => p.MeetingProjects)
                .HasForeignKey(mp => mp.ProjectId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
