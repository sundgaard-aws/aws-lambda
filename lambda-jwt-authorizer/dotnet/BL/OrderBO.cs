using System;
using System.Threading.Tasks;
using OM.AWS.Demo.DTL;

namespace OM.AWS.Demo.BL
{
    public class OrderBO
    {
        public async Task<OrderDTO> ProcessOrderAsync() {
            var order=new OrderDTO{ OrderID=Guid.NewGuid().ToString(), OrderDate=DateTime.Now, Email="demo@domain.com" };
            Console.WriteLine($"Processing order with ID {order.OrderID} ...");
            return order;
        }
    }
}