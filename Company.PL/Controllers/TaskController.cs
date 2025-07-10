using Company.DAL.Data.DbContexts;
using Company.DAL.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;

namespace Company.PL.Controllers
{
    [Authorize]
    public class TaskController : Controller
    {
        private readonly CompanyDbContext _context;
        public TaskController(CompanyDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var tasks = _context.Tasks
                .Select(t => new
                {
                    Task = t,
                    Project = t.Project,
                    Department = t.Department,
                    Employee = t.Employee
                })
                .ToList()
                .Select(x => {
                    x.Task.Project = x.Project;
                    x.Task.Department = x.Department;
                    x.Task.Employee = x.Employee;
                    return x.Task;
                });
            return View(tasks);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Projects = _context.Projects.ToList();
            ViewBag.Departments = _context.Departments.ToList();
            ViewBag.Employees = _context.Employees.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Company.DAL.Entity.Task task)
        {
            if (!ModelState.IsValid)
            {
                // Log errors for debugging
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                ViewBag.Projects = _context.Projects.ToList();
                ViewBag.Departments = _context.Departments.ToList();
                ViewBag.Employees = _context.Employees.ToList();
                return View(task);
            }
            _context.Tasks.Add(task);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            var task = _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.Department)
                .Include(t => t.Employee)
                .FirstOrDefault(t => t.Id == id);
            if (task == null) return NotFound();
            return View(task);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var task = _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.Department)
                .Include(t => t.Employee)
                .FirstOrDefault(t => t.Id == id);
            if (task == null) return NotFound();
            ViewBag.Projects = _context.Projects.ToList();
            ViewBag.Departments = _context.Departments.ToList();
            ViewBag.Employees = _context.Employees.ToList();
            return View(task);
        }

        [HttpPost]
        public IActionResult Edit(int id, Company.DAL.Entity.Task task)
        {
            var existing = _context.Tasks.FirstOrDefault(t => t.Id == id);
            if (existing == null) return NotFound();
            
            if (ModelState.IsValid)
            {
                existing.Title = task.Title;
                existing.Description = task.Description;
                existing.Status = task.Status;
                existing.DueDate = task.DueDate;
                existing.ProjectId = task.ProjectId;
                existing.DepartmentId = task.DepartmentId;
                existing.EmployeeId = task.EmployeeId;
                _context.SaveChanges();
                return RedirectToAction("Details", new { id });
            }
            ViewBag.Projects = _context.Projects.ToList();
            ViewBag.Departments = _context.Departments.ToList();
            ViewBag.Employees = _context.Employees.ToList();
            return View(task);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var task = _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.Department)
                .Include(t => t.Employee)
                .FirstOrDefault(t => t.Id == id);
            if (task == null) return NotFound();
            return View(task);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.Id == id);
            if (task == null) return NotFound();
            _context.Tasks.Remove(task);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
} 