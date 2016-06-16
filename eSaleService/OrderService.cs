using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace eSaleService
{
    public class OrderService
    {
        eSaleDao.OrderDao oderDao = new eSaleDao.OrderDao();
        public eSaleModel.Order GetOrderById(int id)
        {
            return oderDao.GetOrderById(id);
        }

        public List<eSaleModel.Order> GetOrder()
        {
            return oderDao.GetOrder();
        }

        public List<eSaleModel.Order> GetOrderByCondtioin(eSaleModel.Order order)
        {
            return oderDao.GetOrderByCondtioin(order);
        }

        public void DeleteOrderById(string orderId)
        {
            oderDao.DeleteOrderById(orderId);
        }

        public void InsertOrder(eSaleModel.Order order)
        {
            int id=oderDao.InsertOrder(order);
            oderDao.InsertOrderDetial(order.orderDetails,id);
        }

        public void UpdateOrder(eSaleModel.Order order)
        {
            oderDao.UpdateOrder(order);
        }
    }
}
