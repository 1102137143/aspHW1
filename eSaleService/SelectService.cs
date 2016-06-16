using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace eSaleService
{
    public class SelectService
    {
        eSaleDao.SelectDao selectDao = new eSaleDao.SelectDao();
        public List<SelectListItem> getShipper()
        {
            return selectDao.getShipper();
        }

        public List<SelectListItem> getEmp()
        {
            return selectDao.getEmp();
        }

        public List<SelectListItem> getCus()
        {
            return selectDao.getCus();
        }

        public List<SelectListItem> getPro()
        {
            return selectDao.getPro();
        }
    }
}
