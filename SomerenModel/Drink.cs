using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomerenModel
{
    public class Drink
    {
        public int Id { get; set; } // Drink ID
        public string Name { get; set;  } // Drink Name
        public decimal Price { get; set; } // Drink Price
        public int Stock { get; set; } // Available STOCK
        public bool drinkType { get; set; } // Alcoholic = true, Non-Alcoholic = false
       
 
    }
}
