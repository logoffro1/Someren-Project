using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SomerenLogic;
using System.Text.RegularExpressions;
namespace SomerenUI
{
    public partial class AddDrinkForm : Form
    {
        public AddDrinkForm()
        {
            InitializeComponent();
        }

        private void applyBtn_Click(object sender, EventArgs e)
        {
            int drinkType = 4;   //1 = alcoholic ; 0 = non-alcoholic

            //if it's true, there is an error, if it's false, there is no error
            bool error = false;


            //set drinkType based on the selected index
            if (typeCmb.SelectedIndex == 0)
                drinkType = 0;
            else if (typeCmb.SelectedIndex == 1)
                drinkType = 1;
            else
            {
                MessageBox.Show("You need to select a drink type!", "Select drink type", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                error = true;
            }
            if (!Regex.IsMatch(priceTxt.Text, @"[\d]{1,4}([.,][\d]{1,2})?"))
            {
                MessageBox.Show("Wrong price format", "Price error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                error = true;
            }
            //check if the name TextBox is empty or it has less than 4 characters
            if (nameTxt.Text == "" || nameTxt.Text.Length <= 3)
            {
                MessageBox.Show("Your drink name must NOT be empty and it has to have a minimum length of 3 characters!",
                    "Warning",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                error = true;
            }
            if (!Regex.IsMatch(stockTxt.Text, "[0-9]"))
            {
                MessageBox.Show("The stock has to be a number!", "Stock error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                error = true;
            }
            if (!error)
            {
                if (decimal.Parse(priceTxt.Text) < 0 || decimal.Parse(stockTxt.Text) < 0)
                {
                    MessageBox.Show("The stock and the price can't be less than 0!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    error = true;
                }
            }
          
            if (!error) //if there are no errors, add a new drink
            {
                for (int i = 0; i < priceTxt.Text.Length; i++)
                {
                    //if there are commas in the price, replace them with a period
                    if (priceTxt.Text[i].ToString() == ",")
                        priceTxt.Text = priceTxt.Text.Replace(",", ".");
                }
                Drink_Service drinkService = new Drink_Service();
                drinkService.AddDrink(nameTxt.Text, int.Parse((stockTxt.Text)), decimal.Parse(priceTxt.Text), drinkType);
                MessageBox.Show("Drink added succesfully!", "Drink Added", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();

            }
        }

        private void AddDrinkForm_Load(object sender, EventArgs e)
        {
            typeCmb.Items.Add("Non-Alcoholic");
            typeCmb.Items.Add("Alcoholic");
            typeCmb.SelectedIndex = -1;
        }
    }
}
