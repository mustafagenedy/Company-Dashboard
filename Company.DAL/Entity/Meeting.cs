using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Company.DAL.Entity
{
    public class Meeting
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string OnlineLink { get; set; }

        public bool IsRecurring { get; set; }
        public string? RecurrencePattern { get; set; } // e.g., "Weekly", "Monthly", null for one-time

        [Required]
        public int CreatedById { get; set; }
        public Employee CreatedBy { get; set; }
        public ICollection<MeetingParticipant> Participants { get; set; }
        public ICollection<MeetingDepartment> MeetingDepartments { get; set; }
        public ICollection<MeetingProject> MeetingProjects { get; set; }
    }
} 