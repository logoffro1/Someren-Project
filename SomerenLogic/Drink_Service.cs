using SomerenDAL;
using SomerenModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomerenLogic
{
    public class Drink_Service
    {
        Drink_DAO drink_dao = new Drink_DAO();

        public Drink GetByName(string name)
        {

            Drink drink = drink_dao.GetByName(name);

            return drink;
        }
        public void AddDrink(string name, int stock, decimal price, int type)
        {
            drink_dao.AddDrink(name, stock, price, type);
        }
        public void RemoveDrink(Drink drink)
        {
            drink_dao.RemoveDrink(drink);

        }
        public void UpdateDrink(Drink drink, int stock, string name, decimal price)
        {
            drink_dao.UpdateDrink(drink, stock, name, price);
        }
        public List<Drink> GetDrinks()
        {
            List<Drink> drinkList = new List<Drink>();
            try
            {
                drinkList = drink_dao.Db_Get_AllDrinks();
                return drinkList;
            }
            catch
            {
                string exp = "SomerenApp couldn't get the drink list";
                ErrorDAO error = new ErrorDAO(exp);
                return null;
            }

        }
    }
}
