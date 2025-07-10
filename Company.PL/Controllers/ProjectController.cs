using Company.DAL.Data.DbContexts;
using Company.DAL.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Collections.Generic;
using Company.PL.Models; // Added for CreateProjectViewModel
using Microsoft.EntityFrameworkCore;

namespace Company.PL.Controllers
{
    [Authorize]
    public class ProjectController : Controller
    {
        private readonly CompanyDbContext _context;
        public ProjectController(CompanyDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var projects = _context.Projects
                .Include(p => p.ProjectDepartments)
                .ThenInclude(pd => pd.Department)
                .ToList();
            return View(projects);
        }

        public IActionResult Details(int id)
        {
            var project = _context.Projects
                .Include(p => p.ProjectDepartments)
                .ThenInclude(pd => pd.Department)
                .FirstOrDefault(p => p.Id == id);
            if (project == null) return NotFound();
            return View(project);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Departments = _context.Departments.ToList();
            return View(new CreateProjectViewModel());
        }

        [HttpPost]
        public IActionResult Create(CreateProjectViewModel model)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            System.Diagnostics.Debug.WriteLine("ModelState Errors: " + string.Join(", ", errors));
            System.Diagnostics.Debug.WriteLine($"Project Name: {model.Name}, Departments: {(model.SelectedDepartments != null ? string.Join(",", model.SelectedDepartments) : "null")}");

            if (ModelState.IsValid)
            {
                var project = new Project
                {
                    Name = model.Name,
                    Description = model.Description,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    IsActive = model.IsActive,
                    ProjectDepartments = new List<ProjectDepartment>()
                };
                if (model.SelectedDepartments != null)
                {
                    foreach (var deptId in model.SelectedDepartments)
                    {
                        project.ProjectDepartments.Add(new ProjectDepartment
                        {
                            DepartmentId = deptId
                        });
                    }
                }
                _context.Projects.Add(project);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Departments = _context.Departments.ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var project = _context.Projects
                .Include(p => p.ProjectDepartments)
                .FirstOrDefault(p => p.Id == id);
            if (project == null) return NotFound();
            var model = new CreateProjectViewModel
            {
                Name = project.Name,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                IsActive = project.IsActive,
                SelectedDepartments = project.ProjectDepartments.Select(pd => pd.DepartmentId).ToArray()
            };
            ViewBag.Departments = _context.Departments.ToList();
            ViewBag.ProjectId = id;
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(int id, CreateProjectViewModel model)
        {
            var project = _context.Projects
                .Include(p => p.ProjectDepartments)
                .FirstOrDefault(p => p.Id == id);
            if (project == null) return NotFound();
            if (ModelState.IsValid)
            {
                project.Name = model.Name;
                project.Description = model.Description;
                project.StartDate = model.StartDate;
                project.EndDate = model.EndDate;
                project.IsActive = model.IsActive;
                // Update departments
                project.ProjectDepartments.Clear();
                if (model.SelectedDepartments != null)
                {
                    foreach (var deptId in model.SelectedDepartments)
                    {
                        project.ProjectDepartments.Add(new ProjectDepartment
                        {
                            ProjectId = id,
                            DepartmentId = deptId
                        });
                    }
                }
                _context.SaveChanges();
                return RedirectToAction("Details", new { id });
            }
            ViewBag.Departments = _context.Departments.ToList();
            ViewBag.ProjectId = id;
            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var project = _context.Projects
                .Include(p => p.ProjectDepartments)
                .FirstOrDefault(p => p.Id == id);
            if (project == null) return NotFound();
            return View(project);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var project = _context.Projects
                .Include(p => p.ProjectDepartments)
                .FirstOrDefault(p => p.Id == id);
            if (project == null) return NotFound();
            // Remove related ProjectDepartments
            _context.ProjectDepartments.RemoveRange(project.ProjectDepartments);
            _context.Projects.Remove(project);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
} 