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
    public class SelectDao
    {
        private string GetDBConnectionString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString.ToString();
        }

        /// <summary>
        /// 取得出貨公司
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> getShipper()
        {
            DataTable dt = new DataTable();
            string sql = @"select ShipperID as SelectId,CompanyName as SelectName
                           from sales.Shippers";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapCodeData(dt);
        }

        /// <summary>
        /// 取得員工
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> getEmp()
        {
            DataTable dt = new DataTable();
            string sql = @"select EmployeeID as selectId,LastName+FirstName as selectName 
                           from hr.Employees";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapCodeData(dt);
        }

        /// <summary>
        /// 取得客戶
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> getCus()
        {
            DataTable dt = new DataTable();
            string sql = @"select CustomerID as SelectId,CompanyName as SelectName
                        from Sales.Customers";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapCodeData(dt);
        }

        /// <summary>
        /// 取得商品
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> getPro()
        {
            DataTable dt = new DataTable();
            string sql = @"select ProductID as SelectId ,ProductName as SelectName
                            from Production.Products";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapCodeData(dt);
        }

        private List<SelectListItem> MapCodeData(DataTable dt)
        {
            List<SelectListItem> result = new List<SelectListItem>();

            foreach (DataRow row in dt.Rows)
            {
                result.Add(new SelectListItem()
                {
                    Text = row["SelectName"].ToString(),
                    Value = row["SelectId"].ToString()
                });
            }
            return result;
        }

    }
}
