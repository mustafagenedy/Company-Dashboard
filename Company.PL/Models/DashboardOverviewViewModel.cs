namespace Company.PL.Models
{
    public class DashboardOverviewViewModel
    {
        public int DepartmentCount { get; set; }
        public int EmployeeCount { get; set; }
        public int ActiveProjectCount { get; set; }
        public int PendingTaskCount { get; set; }
        // Add more properties for projects and tasks if needed
    }
} 