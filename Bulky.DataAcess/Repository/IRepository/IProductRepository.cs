using Bulky.Models;
using BulkyAcess.DataAcess.Data;
using BulkyAcess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAcess.Repository.IRepsitory
{
    public interface IProductRepository :  IRepsitory<Product>
    {

        void Update(Product Product);
    }
}
