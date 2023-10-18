using Bulky.DataAcess.Repository.IRepsitory;
using Bulky.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAcess.Repository.IRepository
{
    public interface IOrderDetailRepository :IRepsitory<OrderDetail>
    {
        void Update(OrderDetail orderDetail);
    }
}
