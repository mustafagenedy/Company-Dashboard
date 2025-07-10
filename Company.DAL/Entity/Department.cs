using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.DAL.Entity
{
    public class Department
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Employee> Employees { get; set; }
        public ICollection<ProjectDepartment> ProjectDepartments { get; set; }
        public ICollection<MeetingDepartment> MeetingDepartments { get; set; }
    
        
    }
}
