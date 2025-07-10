using System;

namespace Company.DAL.Entity
{
    public class MeetingParticipant
    {
        public int MeetingId { get; set; }
        public Meeting Meeting { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
} 