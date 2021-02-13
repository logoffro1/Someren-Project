using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Collections.ObjectModel;
using SomerenModel;
using System.Configuration;


namespace SomerenDAL
{
  public  class CashRegisterDAO : Base
    {
        private Student_DAO studentDAO = new Student_DAO();
        private Drink_DAO drinkDAO = new Drink_DAO();
    
        public CashRegisterDAO() : base()
        {

        }
        // Returns all orders into a list from database.
        public List<CashRegister> Db_Get_AllOrders()
        {
            string query1 = "select order_id, student.student_id ,student.[name],drink.drink_id, drink.drink_name,drink.price,drink.decimalPrice,";
            string query2 = "drink.stock, [order].Quantity,[order].total_price,";
            string query3 = "[order].OrderDate from [order] inner join  Drink on[order].drink_id = drink.drink_id inner join student on [order].student_id = student.student_id;";
            string query = query1 + query2 + query3;
            SqlParameter[] sqlParameters = new SqlParameter[0];
            
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
           
        }
        //gets all the orders placed in that time interval
        public List<CashRegister> GetOrderForDates(DateTime startDate, DateTime endDate)
        {
            List<CashRegister> tempOrders = Db_Get_AllOrders();
            List<CashRegister> orders = new List<CashRegister>();
            foreach(CashRegister order in tempOrders)
            {
                if (order.orderDate.Day >= startDate.Day && order.orderDate.Day <= endDate.Day)
                    orders.Add(order);
            }
            return orders;
        }
        
        public void AddOrder(CashRegister order)
        {
           //insert the values into the database
         
            SqlCommand cmd = new SqlCommand("INSERT INTO [Order] VALUES(@student_id,@drink_id,@Quantity,@OrderDate,@total_price); ", conn);
            cmd.Parameters.AddWithValue("@student_id", order.student.Number);
            cmd.Parameters.AddWithValue("@drink_id", order.drink.Id);
            cmd.Parameters.AddWithValue("@Quantity",order.quantity);
            cmd.Parameters.AddWithValue("@total_price", order.totalPrice.ToString());
            cmd.Parameters.AddWithValue("@OrderDate", order.orderDate);
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Close();
    
        }

        private List<CashRegister> ReadTables(DataTable dataTable)
        {
            List<CashRegister> orders = new List<CashRegister>();

            foreach (DataRow dr in dataTable.Rows)
            {
                int stid = (int)dr["student_id"];
                int drid = (int)dr["drink_id"];
                int quantityT=(int)dr["Quantity"];
                Drink drinkk = drinkDAO.GetById(drid);
                decimal total_price = drinkk.Price * quantityT;

                CashRegister order = new CashRegister()
                {
                    order_id = (int)dr["order_id"],
                    student = studentDAO.GetById(stid),
                    drink = drinkk,
                    quantity = quantityT,
                    totalPrice = decimal.Parse(dr["total_price"].ToString()),
                    orderDate = (DateTime)dr["OrderDate"]

                  
                };

                orders.Add(order);

            }
            return orders;

        }

    }
}

