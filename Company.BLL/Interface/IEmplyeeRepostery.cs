using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.DAL.Data.DbContexts;
using Company.DAL.Entity;

namespace Company.BLL.Interface
{
    public interface IEmplyeeRepostery
    {

        IEnumerable<Employee> GetAll();
        Employee? GetById(int id);
        int ADD(Employee model);
        int UPDATE(Employee model);
        int DELETE(Employee model);

    }
}
