namespace Company.DAL.Entity
{
    public class ProjectDepartment
    {
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }
} 