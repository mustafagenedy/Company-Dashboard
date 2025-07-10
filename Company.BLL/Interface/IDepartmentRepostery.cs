using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.DAL.Entity;

namespace Company.BLL.Interface
{
    public interface IDepartmentRepostery
    {
        IEnumerable<Department> GetAll();
        Department? GetById(int id);
        int  ADD(Department model);
        int UPDATE(Department model);
        int DELETE(Department model); 
         

    }
}
