using Company.BLL.Repostery;
using Company.DAL.Entity;
using Company.PL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Company.PL.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        public readonly DepartmentRepostery repostery;
        public readonly EmployeeRepostery employeeRepostery;
        public DepartmentController(DepartmentRepostery repostery, EmployeeRepostery employeeRepostery) {
            this.repostery = repostery;
            this.employeeRepostery = employeeRepostery;
        }


        public IActionResult Index() 
        {
           var department = repostery.GetAll();
            return View(department);
        }

        [HttpGet]

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]

        public IActionResult Create(CreateDepartmentDto departmentDto)
        {
            if (ModelState.IsValid)
            {
                var department = new Department()
                {
                    Code = departmentDto.Code,
                    Name = departmentDto.Name,
                    CreatedAt = departmentDto.CreatedAt,

                };
                var count = repostery.ADD(department);
                if (count > 0)
                {
                    return RedirectToAction("index");   
                }
            }
            return View(departmentDto);
        }  


        public IActionResult Details(int id)
        {
            var department = repostery.GetById(id);
            if (department == null)
                return NotFound();
            var employees = employeeRepostery
                .GetAll()
                .Where(e => e.DepartmentId == id)
                .ToList();
            var viewModel = new DepartmentDetailsViewModel
            {
                Department = department,
                Employees = employees
            };
            return View(viewModel);
        }

        public IActionResult Edit(int id) { 
        
            var department = repostery.GetById(id);
            return View(department);
        }
        [HttpPost]
        public IActionResult Edit(Department department)
        {
            var count =repostery.UPDATE(department);
            if (count > 0) { 
                return RedirectToAction("index");
            }
            return View(department);
        }



        public IActionResult Delete(int id) {

            var department = repostery.GetById(id);
            return View(department);
        }
        [HttpPost]
        public IActionResult Delete(Department department)
        
        { 
           var count =  repostery.DELETE(department);
            if (count > 0) { 
                return RedirectToAction("index");
            }
            return View(department);
        }
            

        }
    }
