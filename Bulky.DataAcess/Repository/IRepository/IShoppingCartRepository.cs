using Bulky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAcess.Repository.IRepsitory
{
    public interface IShoppingCartRepository :IRepsitory<ShoppingCart>
    {
        void Update(ShoppingCart cart);
    }
}
