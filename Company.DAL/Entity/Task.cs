using System;

namespace Company.DAL.Entity
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; } // e.g., Pending, Completed
        public DateTime DueDate { get; set; }
        public int ProjectId { get; set; }
        public Project? Project { get; set; }
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
        public int? EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }
} 