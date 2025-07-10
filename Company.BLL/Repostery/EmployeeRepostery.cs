using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.BLL.Interface;
using Company.DAL.Data.DbContexts;
using Company.DAL.Entity;
using Microsoft.EntityFrameworkCore;

namespace Company.BLL.Repostery
{
    public class EmployeeRepostery : IEmplyeeRepostery      // lma tnwr ahmr e3ml ctrl. 3lshan t implement
    {
        private readonly CompanyDbContext dBContext;
        public EmployeeRepostery(CompanyDbContext dBContext)
        {
            this.dBContext = dBContext;
        }


        public IEnumerable<Employee> GetAll()
        {
            return dBContext.Employees.ToList();
        }

        public Employee? GetById(int id)
        {
            return dBContext.Employees.Find(id);

        }

      
        public int ADD(Employee model)
        {
             dBContext.Employees.Add(model);
            return dBContext.SaveChanges();

        }

        public int UPDATE(Employee model)
        {
             dBContext.Employees.Update(model); 
            return dBContext.SaveChanges();
        }

        public int DELETE(Employee model)
        {
            dBContext.Employees.Remove(model);
            return dBContext.SaveChanges();
        }


       
    }
}
