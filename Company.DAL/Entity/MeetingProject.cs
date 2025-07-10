using System;

namespace Company.DAL.Entity
{
    public class MeetingProject
    {
        public int MeetingId { get; set; }
        public Meeting Meeting { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
    }
} 