using System;
using System.ComponentModel.DataAnnotations;

namespace Company.PL.Models
{
    public class CreateMeetingViewModel
    {
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
        public string? RecurrencePattern { get; set; }
        public int[] SelectedDepartments { get; set; }
        public int[] SelectedProjects { get; set; }
        public int[] SelectedEmployees { get; set; }
    }
} 