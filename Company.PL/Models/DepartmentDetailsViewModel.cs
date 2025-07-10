using Company.DAL.Entity;
using System.Collections.Generic;

namespace Company.PL.Models
{
    public class DepartmentDetailsViewModel
    {
        public Department Department { get; set; }
        public List<Employee> Employees { get; set; }
    }
} 