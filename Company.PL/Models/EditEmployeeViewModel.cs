using System.ComponentModel.DataAnnotations;

namespace Company.PL.Models
{
    public class EditEmployeeViewModel
    {
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Employee name is required.")]
        public string EmployeeName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string EmployeeEmail { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        public string EmployeeRole { get; set; }

        [Required(ErrorMessage = "Department is required.")]
        public int DepartmentId { get; set; }
    }
} 