using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTools
{
    public interface ISpecifiRequester<T>
    {
        List<T> GetAll();
        T GetOne(int id);

        T Save(T obj);

        bool Delete(int id);

        bool Modify(T obj);
    }
}
