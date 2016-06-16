using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace eSaleDao
{
    public class OrderDao
    {
        private string GetDBConnectionString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString.ToString();
        }

        /*@"select *
                        from Sales.Orders
                        where Sales.Orders.OrderID=@orderId";*/
        /// <summary>
        /// 依照ID取得訂單
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public eSaleModel.Order GetOrderById(int orderId)
        {
            DataTable dt = new DataTable();
            string sql = @"select *
                from Sales.Orders as a
                INNER JOIN Sales.Customers as b on a.CustomerID=b.CustomerID
                INNER JOIN HR.Employees as c on a.EmployeeID=c.EmployeeID
                INNER JOIN Sales.Shippers as d on a.ShipperID=d.ShipperID
                where a.OrderID=@orderId";


            using (SqlConnection conn=new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@OrderId", orderId));

                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapOrderDataToList(dt).FirstOrDefault();
        }

        /// <summary>
        /// 依照條件取得訂單資料
        /// </summary>
        /// <returns></returns>
        public List<eSaleModel.Order> GetOrderByCondtioin(eSaleModel.Order order)
        {
            DataTable dt = new DataTable();
            string sql = @"select *
                from Sales.Orders as a
                INNER JOIN Sales.Customers as b on a.CustomerID=b.CustomerID
                INNER JOIN HR.Employees as c on a.EmployeeID=c.EmployeeID
                INNER JOIN Sales.Shippers as d on a.ShipperID=d.ShipperID
				WHERE (a.OrderID=@OrderId OR @OrderId=0) AND (b.Companyname LIKE '%'+@CustName+'%' OR @CustName='')
                AND (c.EmployeeID=@EmpId OR @EmpId=0)
                AND (d.ShipperID=@ShipperId OR @ShipperId=0)
                AND (a.OrderDate=@OrderDate OR @OrderDate='2019-01-01')
                AND (a.ShippedDate=@ShippedDate OR @ShippedDate='2019-01-01')
                AND (a.RequiredDate=@RequiredDate OR @RequiredDate='2019-01-01')";


            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@OrderId", order.OrderId));
                cmd.Parameters.Add(new SqlParameter("@CustName", order.CustName == null ? string.Empty : order.CustName));
                cmd.Parameters.Add(new SqlParameter("@EmpId", order.EmpId));
                cmd.Parameters.Add(new SqlParameter("@ShipperId", order.ShipperId));
                cmd.Parameters.Add(new SqlParameter("@Orderdate", order.Orderdate == null ? Convert.ToDateTime("2019-01-01") : order.Orderdate));
                cmd.Parameters.Add(new SqlParameter("@ShippedDate", order.ShippedDate == null ? Convert.ToDateTime("2019-01-01") : order.ShippedDate));
                cmd.Parameters.Add(new SqlParameter("@RequiredDate", order.RequiredDate == null ? Convert.ToDateTime("2019-01-01") : order.RequiredDate));

                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }


            return this.MapOrderDataToList(dt);
        }

        /// <summary>
        /// 取得所有訂單
        /// </summary>
        /// <returns></returns>
        public List<eSaleModel.Order> GetOrder()
        {
            DataTable dt = new DataTable();
            string sql = @"select *
                from Sales.Orders as a
                INNER JOIN Sales.Customers as b on a.CustomerID=b.CustomerID
                INNER JOIN HR.Employees as c on a.EmployeeID=c.EmployeeID
                INNER JOIN Sales.Shippers as d on a.ShipperID=d.ShipperID
                Order By a.OrderID";
            
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapOrderDataToList(dt);
        }

        /// <summary>
        /// 刪除訂單
        /// </summary>
        public void DeleteOrderById(string orderId)
        {
            try
            {
                string sql = "Delete FROM Sales.Orders Where orderid=@orderid";
                using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add(new SqlParameter("@orderid", orderId));
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 新增訂單
        /// </summary>
        /// <param name="order"></param>
        /// <returns>訂單編號</returns>
        public int InsertOrder(eSaleModel.Order order)
        {
            string sql = @"Insert INTO Sales.Orders
						 (
							CustomerID,EmployeeID,OrderDate,RequiredDate,ShippedDate,ShipperID,Freight,
							ShipName,ShipAddress,ShipCity,ShipRegion,ShipPostalCode,ShipCountry
						)
						VALUES
						(
							@CustId,@EmpId,@Orderdate,@RequiredDate,@ShippedDate,@ShipperId,@Freight,
							@ShipName,@ShipAddress,@ShipCity,@ShipRegion,@ShipPostalCode,@ShipCountry
						)
                        Select SCOPE_IDENTITY()
						";
            int orderId;
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@CustId", order.CustId));
                cmd.Parameters.Add(new SqlParameter("@EmpId", order.EmpId));
                cmd.Parameters.Add(new SqlParameter("@Orderdate", order.Orderdate));
                cmd.Parameters.Add(new SqlParameter("@RequiredDate", order.RequiredDate));
                cmd.Parameters.Add(new SqlParameter("@ShippedDate", order.ShippedDate == null ? null : order.ShippedDate));
                cmd.Parameters.Add(new SqlParameter("@ShipperId", order.ShipperId));
                cmd.Parameters.Add(new SqlParameter("@Freight", order.Freight));
                cmd.Parameters.Add(new SqlParameter("@ShipName", order.ShipName));
                cmd.Parameters.Add(new SqlParameter("@ShipAddress", order.ShipAddress));
                cmd.Parameters.Add(new SqlParameter("@ShipCity", order.ShipCity));
                cmd.Parameters.Add(new SqlParameter("@ShipRegion", order.ShipRegion == null ? string.Empty : order.ShipRegion));
                cmd.Parameters.Add(new SqlParameter("@ShipPostalCode", order.ShipPostalCode == null ? string.Empty : order.ShipPostalCode));
                cmd.Parameters.Add(new SqlParameter("@ShipCountry", order.ShipCountry));

                orderId = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
            }
            return orderId;
        }

        /// <summary>
        /// 更新訂單
        /// </summary>
        /// <param name="order"></param>
        public void UpdateOrder(eSaleModel.Order order)
        {
            string sql = @"update sales.orders
                            set CustomerID=@CustId,EmployeeID=@EmpId,OrderDate=@Orderdate,RequiredDate=@RequiredDate,ShippedDate=@ShippedDate,ShipperID=@ShipperId,Freight=@Freight,
							ShipName=@ShipName,ShipAddress=@ShipAddress,ShipCity=@ShipCity,ShipRegion=@ShipRegion,ShipPostalCode=@ShipPostalCode,ShipCountry=@ShipCountry
                            where orderid=@orderid
                            
						";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@orderid", order.OrderId));
                cmd.Parameters.Add(new SqlParameter("@CustId", order.CustId));
                cmd.Parameters.Add(new SqlParameter("@EmpId", order.EmpId));
                cmd.Parameters.Add(new SqlParameter("@Orderdate", order.Orderdate));
                cmd.Parameters.Add(new SqlParameter("@RequiredDate", order.RequiredDate));
                cmd.Parameters.Add(new SqlParameter("@ShippedDate", order.ShippedDate));
                cmd.Parameters.Add(new SqlParameter("@ShipperId", order.ShipperId));
                cmd.Parameters.Add(new SqlParameter("@Freight", order.Freight));
                cmd.Parameters.Add(new SqlParameter("@ShipName", order.ShipName));
                cmd.Parameters.Add(new SqlParameter("@ShipAddress", order.ShipAddress));
                cmd.Parameters.Add(new SqlParameter("@ShipCity", order.ShipCity));
                cmd.Parameters.Add(new SqlParameter("@ShipRegion", order.ShipRegion == null ? string.Empty : order.ShipRegion));
                cmd.Parameters.Add(new SqlParameter("@ShipPostalCode", order.ShipPostalCode == null ? string.Empty : order.ShipPostalCode));
                cmd.Parameters.Add(new SqlParameter("@ShipCountry", order.ShipCountry));

                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        /// <summary>
        /// 新增訂單明細
        /// </summary>
        /// <param name="order"></param>
        public void InsertOrderDetial(List<eSaleModel.OrderDetails> orderDetial,int orderId)
        {
            foreach (eSaleModel.OrderDetails row in orderDetial)
            {
                string sql = @"Insert INTO Sales.OrderDetails
						(
							OrderID,ProductID,UnitPrice,Qty
						)
						VALUES
						(
							@OrderID,@ProductID,@UnitPrice,@Qty
						)
                        Select SCOPE_IDENTITY()
						";
                using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
                {

                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add(new SqlParameter("@OrderID", orderId));
                    cmd.Parameters.Add(new SqlParameter("@ProductID", row.ProductId));
                    cmd.Parameters.Add(new SqlParameter("@UnitPrice", row.UnitPrice));
                    cmd.Parameters.Add(new SqlParameter("@Qty", row.Qty));

                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            } 
        }

        private List<eSaleModel.Order> MapOrderDataToList(DataTable orderData)
        {
            List<eSaleModel.Order> result = new List<eSaleModel.Order>();


            foreach (DataRow row in orderData.Rows)
            {
                result.Add(new eSaleModel.Order()
                {
                    CustId = row["CustomerId"].ToString(),
                    CustName = row["Companyname"].ToString(),
                    EmpId = (int)row["EmployeeId"],
                    EmpName = (row["lastname"].ToString()) + (row["firstname"].ToString()),
                    Freight = Convert.ToDouble(row["Freight"]),
                    Orderdate = row["Orderdate"] == DBNull.Value ? (DateTime?)null : (DateTime)row["Orderdate"],
                    OrderId = (int)row["OrderId"],
                    RequiredDate = row["RequiredDate"] == DBNull.Value ? (DateTime?)null : (DateTime)row["RequiredDate"],
                    ShipAddress = row["ShipAddress"].ToString(),
                    ShipCity = row["ShipCity"].ToString(),
                    ShipCountry = row["ShipCountry"].ToString(),
                    ShipName = row["ShipName"].ToString(),
                    ShippedDate = row["ShippedDate"] == DBNull.Value ? (DateTime?)null : (DateTime)row["ShippedDate"],
                    ShipperId = (int)row["ShipperId"],
                    ShipperName = row["CompanyName"].ToString(),
                    ShipPostalCode = row["ShipPostalCode"].ToString(),
                    ShipRegion = row["ShipRegion"].ToString()
                });
            }
            return result;
        }

        
    }
}
