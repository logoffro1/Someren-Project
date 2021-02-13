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
    public class Drink_DAO : Base
    {
        public Drink_DAO() : base()
        {

        }
        // Returns all drinks into a list from database.
        public List<Drink> Db_Get_AllDrinks()
        {
            string query = "SELECT drink_id,drink_type,stock,drink_name,price,decimalPrice FROM Drink;";

            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
        }

        //gets the drink with the specified name
        public Drink GetByName(string name)
        {

            OpenConnection();
            SqlCommand cmd = new SqlCommand("SELECT drink_id,drink_type,stock,drink_name,price,decimalPrice FROM drink WHERE drink_name = @drink_name", conn);
            cmd.Parameters.AddWithValue("@drink_name", name);
            SqlDataReader reader = cmd.ExecuteReader();
            Drink drink = null;

            if (reader.Read())
            {
                drink = ReadDrink(reader);
            }
            reader.Close();
            
            conn.Close();
            return drink;
            
        }

        //gets the drink with the specified ID
        public Drink GetById(int id)
        {

            OpenConnection();
            SqlCommand cmd = new SqlCommand("SELECT drink_id,drink_type,stock,drink_name,price,decimalPrice FROM Drink WHERE drink_id = @drink_id", conn);
            cmd.Parameters.AddWithValue("@drink_id", id);
            SqlDataReader reader = cmd.ExecuteReader();
            Drink drink = null;
            if (reader.Read())
            {
                drink = ReadDrink(reader);
            }
            reader.Close();
            conn.Close();
            return drink;
        }
        private Drink ReadDrink(SqlDataReader reader)
        {

            string priceTemp = $"{reader["price"].ToString()}.{reader["decimalPrice"].ToString()}";

            Drink drink = new Drink()
            {
                Id = (int)reader["drink_id"],
                Price = decimal.Parse(priceTemp),
                drinkType = (bool)reader["drink_type"],
                Stock = (int)reader["stock"],
                Name = reader["drink_name"].ToString()
            };
            return drink;

        }
        //add a new drink into the database
        public void AddDrink(string name, int stock, decimal price, int type)
        {
            string[] numberSplit = new string[2];

            int counter = 0;

            //check if the price has a '.' or a ',' in it, if it does, use that to separate the whole number from the decimal into an array
            for (int i = 0; i < price.ToString().Length; i++)
            {
                if (price.ToString()[i] == '.')
                {
        
                    numberSplit = price.ToString().Split('.');
                    counter++;
                }
              
            }
            if (counter == 0)
                numberSplit = new string[2] { price.ToString(), "0" }; ;
               
                 
            OpenConnection();
            SqlCommand cmd = new SqlCommand("INSERT INTO Drink VALUES(@drink_type,@stock,@drink_name,@price,@decimalPrice); ", conn);
                cmd.Parameters.AddWithValue("@price", int.Parse(numberSplit[0]));
                cmd.Parameters.AddWithValue("@decimalPrice", int.Parse(numberSplit[1]));
                cmd.Parameters.AddWithValue("@drink_type", type);
                cmd.Parameters.AddWithValue("@drink_name", name);
                cmd.Parameters.AddWithValue("@stock", stock);
                SqlDataReader reader = cmd.ExecuteReader();
                reader.Close(); 
         
                conn.Close();            
        }
        //remove a drink from the database
        public void RemoveDrink(Drink drink)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM Drink WHERE drink_id=@Id; ", conn);
            cmd.Parameters.AddWithValue("@Id", drink.Id);
            SqlDataReader reader = cmd.ExecuteReader();
            reader.Close();
            conn.Close();
        }
        //edit an existing drink
        public void UpdateDrink(Drink drink,int stock,string name,decimal price)
        {
            string[] numberSplit = price.ToString().Split('.');
            OpenConnection();
            SqlCommand cmd = new SqlCommand("update [Drink] set stock = @Stock, drink_name = @drink_name,price = @price,decimalPrice = @decimalPrice where drink_id = @Id; ", conn);
        
            cmd.Parameters.AddWithValue("@Stock", stock);
            cmd.Parameters.AddWithValue("@drink_name", name);
            cmd.Parameters.AddWithValue("@Id", drink.Id);
            cmd.Parameters.AddWithValue("@price", int.Parse(numberSplit[0]));
            cmd.Parameters.AddWithValue("@decimalPrice", int.Parse(numberSplit[1]));

            SqlDataReader reader = cmd.ExecuteReader();

            reader.Close();
            conn.Close();

        }
        private List<Drink> ReadTables(DataTable dataTable)
        {
            List<Drink> drinks = new List<Drink>();

            foreach (DataRow dr in dataTable.Rows)
            {
                string priceTemp = $"{dr["price"].ToString()}.{dr["decimalPrice"].ToString()}";

                Drink drink = new Drink()
                {
                    Id = (int)dr["drink_id"],
                    Price = decimal.Parse(priceTemp),
                    drinkType = (bool)(dr["drink_type"]),
                    Stock = (int)(dr["stock"]),
                    Name = (dr["drink_name"].ToString())

                };
            
                drinks.Add(drink);

            }
            return drinks;
        }

    }
}
