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
    public class DepartmentRepostery : IDepartmentRepostery
    {
        private readonly CompanyDbContext dbContext;
        public DepartmentRepostery(CompanyDbContext dbContext) {
            this.dbContext = dbContext;
        }
        public IEnumerable<Department> GetAll()
        {
           // using CompanyDbContext dbContext = new CompanyDbContext();
            return dbContext.Departments.ToList();
        }

        public Department? GetById(int id)
        {
           // using CompanyDbContext dbContext = new CompanyDbContext();
            return dbContext.Departments.Find(id);
        }

        public int ADD(Department model)
        {
           // using CompanyDbContext dbContext = new CompanyDbContext();
            dbContext.Departments.Add(model);
            return dbContext.SaveChanges();
        }

        public int UPDATE(Department model)
        {
           // using CompanyDbContext dbContext = new CompanyDbContext();
            dbContext.Departments.Update(model);
            return dbContext.SaveChanges();
        }
        public int DELETE(Department model)
        {
          //  using CompanyDbContext dbContext = new CompanyDbContext();
            dbContext.Departments.Remove(model);
            return dbContext.SaveChanges();
        }

               
    }
}
