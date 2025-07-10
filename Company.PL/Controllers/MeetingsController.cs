using Company.DAL.Data.DbContexts;
using Company.DAL.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Company.PL.Models;

namespace Company.PL.Controllers
{
    [Authorize]
    public class MeetingsController : Controller
    {
        private readonly CompanyDbContext _context;
        public MeetingsController(CompanyDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var meetings = _context.Meetings
                .Include(m => m.MeetingDepartments).ThenInclude(md => md.Department)
                .Include(m => m.MeetingProjects).ThenInclude(mp => mp.Project)
                .Include(m => m.Participants).ThenInclude(p => p.Employee)
                .Include(m => m.CreatedBy)
                .OrderByDescending(m => m.StartTime)
                .ToList();
            return View(meetings);
        }

        [Authorize(Roles = "Manager,Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Departments = _context.Departments.ToList();
            ViewBag.Projects = _context.Projects.ToList();
            ViewBag.Employees = _context.Employees.ToList();
            return View(new CreateMeetingViewModel());
        }

        [Authorize(Roles = "Manager,Admin")]
        [HttpPost]
        public IActionResult Create(CreateMeetingViewModel model)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            System.Diagnostics.Debug.WriteLine("ModelState Errors: " + string.Join(", ", errors));

            if (ModelState.IsValid)
            {
                var meeting = new Meeting
                {
                    Title = model.Title,
                    Description = model.Description,
                    StartTime = model.StartTime,
                    EndTime = model.EndTime,
                    Location = model.Location,
                    OnlineLink = model.OnlineLink,
                    IsRecurring = model.IsRecurring,
                    RecurrencePattern = model.RecurrencePattern,
                    MeetingDepartments = new List<MeetingDepartment>(),
                    MeetingProjects = new List<MeetingProject>(),
                    Participants = new List<MeetingParticipant>()
                };
                if (model.SelectedDepartments != null)
                {
                    foreach (var deptId in model.SelectedDepartments)
                    {
                        meeting.MeetingDepartments.Add(new MeetingDepartment { DepartmentId = deptId });
                    }
                }
                if (model.SelectedProjects != null)
                {
                    foreach (var projId in model.SelectedProjects)
                    {
                        meeting.MeetingProjects.Add(new MeetingProject { ProjectId = projId });
                    }
                }
                if (model.SelectedEmployees != null)
                {
                    foreach (var empId in model.SelectedEmployees)
                    {
                        meeting.Participants.Add(new MeetingParticipant { EmployeeId = empId });
                    }
                }
                // Set CreatedById to the currently logged-in user's EmployeeId
                var userEmail = User.Identity.Name;
                var employee = _context.Employees.FirstOrDefault(e => e.EmployeeEmail == userEmail);
                if (employee != null)
                {
                    meeting.CreatedById = employee.EmployeeId;
                }
                else
                {
                    ModelState.AddModelError("", "Could not determine the meeting creator.");
                    ViewBag.Departments = _context.Departments.ToList();
                    ViewBag.Projects = _context.Projects.ToList();
                    ViewBag.Employees = _context.Employees.ToList();
                    return View(model);
                }
                _context.Meetings.Add(meeting);
                try
                {
                    var result = _context.SaveChanges();
                    System.Diagnostics.Debug.WriteLine("SaveChanges result: " + result);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Database error: " + ex.Message);
                    ViewBag.Departments = _context.Departments.ToList();
                    ViewBag.Projects = _context.Projects.ToList();
                    ViewBag.Employees = _context.Employees.ToList();
                    return View(model);
                }
                return RedirectToAction("Index");
            }
            ViewBag.Departments = _context.Departments.ToList();
            ViewBag.Projects = _context.Projects.ToList();
            ViewBag.Employees = _context.Employees.ToList();
            return View(model);
        }

        public IActionResult Details(int id)
        {
            var meeting = _context.Meetings
                .Include(m => m.MeetingDepartments).ThenInclude(md => md.Department)
                .Include(m => m.MeetingProjects).ThenInclude(mp => mp.Project)
                .Include(m => m.Participants).ThenInclude(p => p.Employee)
                .Include(m => m.CreatedBy)
                .FirstOrDefault(m => m.Id == id);
            if (meeting == null) return NotFound();
            return View(meeting);
        }

        [Authorize(Roles = "Manager,Admin")]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var meeting = _context.Meetings
                .Include(m => m.MeetingDepartments)
                .Include(m => m.MeetingProjects)
                .Include(m => m.Participants)
                .FirstOrDefault(m => m.Id == id);
            if (meeting == null) return NotFound();
            var model = new CreateMeetingViewModel
            {
                Title = meeting.Title,
                Description = meeting.Description,
                StartTime = meeting.StartTime,
                EndTime = meeting.EndTime,
                Location = meeting.Location,
                OnlineLink = meeting.OnlineLink,
                IsRecurring = meeting.IsRecurring,
                RecurrencePattern = meeting.RecurrencePattern,
                SelectedDepartments = meeting.MeetingDepartments?.Select(md => md.DepartmentId).ToArray(),
                SelectedProjects = meeting.MeetingProjects?.Select(mp => mp.ProjectId).ToArray(),
                SelectedEmployees = meeting.Participants?.Select(p => p.EmployeeId).ToArray()
            };
            ViewBag.Departments = _context.Departments.ToList();
            ViewBag.Projects = _context.Projects.ToList();
            ViewBag.Employees = _context.Employees.ToList();
            ViewBag.MeetingId = id;
            return View(model);
        }

        [Authorize(Roles = "Manager,Admin")]
        [HttpPost]
        public IActionResult Edit(int id, CreateMeetingViewModel model)
        {
            var meeting = _context.Meetings
                .Include(m => m.MeetingDepartments)
                .Include(m => m.MeetingProjects)
                .Include(m => m.Participants)
                .FirstOrDefault(m => m.Id == id);
            if (meeting == null) return NotFound();
            if (ModelState.IsValid)
            {
                meeting.Title = model.Title;
                meeting.Description = model.Description;
                meeting.StartTime = model.StartTime;
                meeting.EndTime = model.EndTime;
                meeting.Location = model.Location;
                meeting.OnlineLink = model.OnlineLink;
                meeting.IsRecurring = model.IsRecurring;
                meeting.RecurrencePattern = model.RecurrencePattern;
                // Update Departments
                meeting.MeetingDepartments.Clear();
                if (model.SelectedDepartments != null)
                {
                    foreach (var deptId in model.SelectedDepartments)
                    {
                        meeting.MeetingDepartments.Add(new MeetingDepartment { MeetingId = id, DepartmentId = deptId });
                    }
                }
                // Update Projects
                meeting.MeetingProjects.Clear();
                if (model.SelectedProjects != null)
                {
                    foreach (var projId in model.SelectedProjects)
                    {
                        meeting.MeetingProjects.Add(new MeetingProject { MeetingId = id, ProjectId = projId });
                    }
                }
                // Update Participants
                meeting.Participants.Clear();
                if (model.SelectedEmployees != null)
                {
                    foreach (var empId in model.SelectedEmployees)
                    {
                        meeting.Participants.Add(new MeetingParticipant { MeetingId = id, EmployeeId = empId });
                    }
                }
                _context.SaveChanges();
                return RedirectToAction("Details", new { id });
            }
            ViewBag.Departments = _context.Departments.ToList();
            ViewBag.Projects = _context.Projects.ToList();
            ViewBag.Employees = _context.Employees.ToList();
            ViewBag.MeetingId = id;
            return View(model);
        }

        [Authorize(Roles = "Manager,Admin")]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var meeting = _context.Meetings
                .Include(m => m.MeetingDepartments).ThenInclude(md => md.Department)
                .Include(m => m.MeetingProjects).ThenInclude(mp => mp.Project)
                .Include(m => m.Participants).ThenInclude(p => p.Employee)
                .Include(m => m.CreatedBy)
                .FirstOrDefault(m => m.Id == id);
            if (meeting == null) return NotFound();
            return View(meeting);
        }

        [Authorize(Roles = "Manager,Admin")]
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var meeting = _context.Meetings
                .Include(m => m.MeetingDepartments)
                .Include(m => m.MeetingProjects)
                .Include(m => m.Participants)
                .FirstOrDefault(m => m.Id == id);
            if (meeting == null) return NotFound();
            _context.MeetingDepartments.RemoveRange(meeting.MeetingDepartments);
            _context.MeetingProjects.RemoveRange(meeting.MeetingProjects);
            _context.MeetingParticipants.RemoveRange(meeting.Participants);
            _context.Meetings.Remove(meeting);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
} 