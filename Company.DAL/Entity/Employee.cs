using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Company.DAL.Entity
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        
        [Required]
        public string EmployeeName { get; set; }
        
        [Required]
        [EmailAddress]
        public string EmployeeEmail { get; set; }
        
        [Required]
        public string EmployeeRole { get; set; }
        
        public int DepartmentId { get; set; }

        // Navigation property should be nullable and not required
        public Department? Department { get; set; }

        // Initialize collection and do not require it for model binding
        public ICollection<MeetingParticipant> MeetingParticipants { get; set; } = new List<MeetingParticipant>();
    }
}
