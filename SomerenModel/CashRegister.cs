using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomerenModel
{
  public  class CashRegister
    {
        public int order_id { get; set; } // order's primary key
        public Student student { get; set; } // the student that placed the order
        public Drink drink { get; set; } // the drink that is in that order
        public decimal totalPrice { get; set; } // the total price of the order
        public int quantity { get; set; } // the total quantity of the order
        public DateTime orderDate { get; set; } // the date when the order was placed
    }
}
