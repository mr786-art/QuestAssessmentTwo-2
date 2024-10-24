using Patient_and_DoctorManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient_and_DoctorManagementSystem.Repositories
{
    public interface IRepository<T>
    {
        GenericResponse<T> Create(T entity); 
        GenericResponse<List<T>> Read(); 
        GenericResponse<T> Update(T entity); 
        GenericResponse<bool> Delete(int id);
    }
}
