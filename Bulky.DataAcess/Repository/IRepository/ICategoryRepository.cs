using BulkyAcess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAcess.Repository.IRepsitory
{
    public interface ICategoryRepository : IRepsitory<Category>
    {
        void Update(Category category);

    }
}
