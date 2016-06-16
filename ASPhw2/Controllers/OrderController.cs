using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ASPhw2.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        eSaleService.OrderService orderService = new eSaleService.OrderService();
        eSaleService.SelectService selectServuce = new eSaleService.SelectService();
        public ActionResult Index()
        {
            //var data = orderService.GetOrderById("10249");
            //ViewBag.data = data.CustName;
            var Orderdata = orderService.GetOrder();
            ViewBag.Orderdata = Orderdata;

            var Shipdata = selectServuce.getShipper();
            ViewBag.Ship = Shipdata;

            var Empdata = selectServuce.getEmp();
            ViewBag.Emp = Empdata;

            return View();
        }

        [HttpPost()]
        public ActionResult Index(eSaleModel.Order postOrder)
        {
            var getOrderByCondtioin = orderService.GetOrderByCondtioin(postOrder);
            ViewBag.Orderdata = getOrderByCondtioin;

            var Shipdata = selectServuce.getShipper();
            ViewBag.Ship = Shipdata;

            var Empdata = selectServuce.getEmp();
            ViewBag.Emp = Empdata;
            return View();
        }

        /// <summary>
        /// 刪除訂單
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPost()]
        public JsonResult DeleteOrder(string orderId)
        {
            try
            {
                eSaleService.OrderService orderService = new eSaleService.OrderService();
                orderService.DeleteOrderById(orderId);
                return this.Json(true);
            }
            catch (Exception)
            {
                return this.Json(false);
            }
        }

        /// <summary>
        /// 新增訂單
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var Empdata = selectServuce.getEmp();
            ViewBag.Emp = Empdata;

            var Cusdata = selectServuce.getCus();
            ViewBag.Cus = Cusdata;

            var Shipdata = selectServuce.getShipper();
            ViewBag.Ship = Shipdata;

            var Prodata = selectServuce.getPro();
            ViewBag.Pro = Prodata;

            return View();
        }

        [HttpPost()]
        public ActionResult Create(eSaleModel.Order postOrder)
        {
            orderService.InsertOrder(postOrder);

            Index();
            return View("Index");
        }

        /// <summary>
        /// 修改訂單
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Update(int id)
        {
            ViewBag.Id = id;

            Create();

            var Orderdata = orderService.GetOrderById(id);
            ViewBag.Order = Orderdata;

            ViewBag.Orderdate = string.Format("{0:yyyy-MM-dd}", Orderdata.Orderdate);
            ViewBag.RequiredDate = string.Format("{0:yyyy-MM-dd}", Orderdata.RequiredDate);
            ViewBag.ShippedDate = string.Format("{0:yyyy-MM-dd}", Orderdata.ShippedDate);

            return View();
        }

        [HttpPost]
        public ActionResult Update(eSaleModel.Order postOrder)
        {
            orderService.UpdateOrder(postOrder);
            Index();
            return View("Index");
        }

    }
}