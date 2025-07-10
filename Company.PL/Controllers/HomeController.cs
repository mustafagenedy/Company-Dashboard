using System.Diagnostics;
using Company.PL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Company.DAL.Data.DbContexts;

namespace Company.PL.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CompanyDbContext _context;

        public HomeController(ILogger<HomeController> logger, CompanyDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var model = new DashboardOverviewViewModel
            {
                DepartmentCount = _context.Departments.Count(),
                EmployeeCount = _context.Employees.Count(),
                ActiveProjectCount = _context.Projects.Count(p => p.IsActive),
                PendingTaskCount = _context.Tasks.Count(t => t.Status == "Pending")
            };
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
