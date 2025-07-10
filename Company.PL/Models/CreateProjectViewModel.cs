using System;

namespace Company.PL.Models
{
    public class CreateProjectViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public int[] SelectedDepartments { get; set; }
    }
} 