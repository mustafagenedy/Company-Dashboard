using System;
using System.Collections.Generic;

namespace Company.DAL.Entity
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public ICollection<ProjectDepartment> ProjectDepartments { get; set; }
        public ICollection<Task> Tasks { get; set; }
        public ICollection<MeetingProject> MeetingProjects { get; set; }
    }
} 