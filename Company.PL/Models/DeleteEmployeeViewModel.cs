using System.ComponentModel.DataAnnotations;

namespace Company.PL.Models
{
    public class DeleteEmployeeViewModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeEmail { get; set; }
        public string EmployeeRole { get; set; }
    }
} 