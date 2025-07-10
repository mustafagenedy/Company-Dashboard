using System;

namespace Company.DAL.Entity
{
    public class MeetingDepartment
    {
        public int MeetingId { get; set; }
        public Meeting Meeting { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
    }
} 