using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SomerenDAL;
using SomerenModel;

namespace SomerenLogic
{
    public class CashRegisterService
    {
      private CashRegisterDAO cashregisterDao =  new CashRegisterDAO();

        public List<CashRegister> GetOrderForDates(DateTime startDate, DateTime endDate)
        {
            return cashregisterDao.GetOrderForDates(startDate, endDate);
        }
        public List<CashRegister> getAllOrders()
        {
            return cashregisterDao.Db_Get_AllOrders();
        }
        public void AddOrder(CashRegister order)
        {
            cashregisterDao.AddOrder(order);
        }
    }
}
