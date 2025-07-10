using Company.BLL.Repostery;
using Company.DAL.Entity;
using Company.PL.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using Microsoft.AspNetCore.Authorization;
using Company.DAL.Data.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Company.PL.Controllers
{
    [Authorize]
    public class EmployeeController: Controller
    {
        public readonly EmployeeRepostery repostery;
        public readonly DepartmentRepostery departmentRepostery;
        private readonly CompanyDbContext _context;
        public EmployeeController(EmployeeRepostery repostery, DepartmentRepostery departmentRepostery, CompanyDbContext context)
        {
            this.repostery = repostery;
            this.departmentRepostery = departmentRepostery;
            _context = context;
        }
        public IActionResult Index()
        {
            var employee = repostery.GetAll();
            return View(employee);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Departments = departmentRepostery.GetAll().ToList();
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateEmployeeDto employeeDto)
        {
            if (!departmentRepostery.GetAll().Any(d => d.Id == employeeDto.DepartmentId))
            {
                ModelState.AddModelError("DepartmentId", "Selected department does not exist.");
            }
            if (ModelState.IsValid)
            {
                var employee = new Employee()
                {
                    EmployeeEmail = employeeDto.EmployeeEmail,
                    EmployeeName = employeeDto.EmployeeName,
                    EmployeeRole = employeeDto.EmployeeRole,
                    DepartmentId = employeeDto.DepartmentId
                };
                var count = repostery.ADD(employee);
                if (count > 0)
                {
                    return RedirectToAction("index");
                }
            }
            ViewBag.Departments = departmentRepostery.GetAll().ToList();
            return View(employeeDto);
        }
        public IActionResult Details(int id)
        {

            var employee = repostery.GetById(id);
            return View(employee);
        }



        public IActionResult Edit(int id)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.EmployeeId == id);
            if (employee == null) return NotFound();
            var viewModel = new EditEmployeeViewModel
            {
                EmployeeId = employee.EmployeeId,
                EmployeeName = employee.EmployeeName,
                EmployeeEmail = employee.EmployeeEmail,
                EmployeeRole = employee.EmployeeRole,
                DepartmentId = employee.DepartmentId
            };
            ViewBag.Departments = departmentRepostery.GetAll().ToList();
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult Edit(EditEmployeeViewModel model)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            System.Diagnostics.Debug.WriteLine("Employee Edit ModelState Errors: " + string.Join(", ", errors));
            System.Diagnostics.Debug.WriteLine("Employee ID: " + model.EmployeeId);
            
            if (!departmentRepostery.GetAll().Any(d => d.Id == model.DepartmentId))
            {
                ModelState.AddModelError("DepartmentId", "Selected department does not exist.");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    var dbEmployee = _context.Employees.FirstOrDefault(e => e.EmployeeId == model.EmployeeId);
                    if (dbEmployee == null) return NotFound();
                    dbEmployee.EmployeeName = model.EmployeeName;
                    dbEmployee.EmployeeEmail = model.EmployeeEmail;
                    dbEmployee.EmployeeRole = model.EmployeeRole;
                    dbEmployee.DepartmentId = model.DepartmentId;
                    _context.SaveChanges();
                    return RedirectToAction("index");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error updating employee: " + ex.Message);
                    ModelState.AddModelError("", "An error occurred while updating the employee. Please try again.");
                }
            }
            ViewBag.Departments = departmentRepostery.GetAll().ToList();
            return View(model);
        }






        [HttpGet]
        public IActionResult Delete(int id)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.EmployeeId == id);
            if (employee == null) return NotFound();
            var viewModel = new DeleteEmployeeViewModel
            {
                EmployeeId = employee.EmployeeId,
                EmployeeName = employee.EmployeeName,
                EmployeeEmail = employee.EmployeeEmail,
                EmployeeRole = employee.EmployeeRole
            };
            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int employeeId)
        {
            try
            {
                var employee = _context.Employees.FirstOrDefault(e => e.EmployeeId == employeeId);
                System.Diagnostics.Debug.WriteLine("Deleting employee: " + (employee != null ? employee.EmployeeId.ToString() : "null"));
                if (employee == null) return NotFound();

                // Remove related entities first
                var meetingParticipants = _context.MeetingParticipants.Where(mp => mp.EmployeeId == employeeId).ToList();
                if (meetingParticipants.Any())
                {
                    _context.MeetingParticipants.RemoveRange(meetingParticipants);
                }

                var tasks = _context.Tasks.Where(t => t.EmployeeId == employeeId).ToList();
                if (tasks.Any())
                {
                    _context.Tasks.RemoveRange(tasks);
                }

                _context.Employees.Remove(employee);
                _context.SaveChanges();
                return RedirectToAction("index");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error deleting employee: " + ex.Message);
                return RedirectToAction("Index");
            }
        }
    }
}