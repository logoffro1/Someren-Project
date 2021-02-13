using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SomerenModel;
using SomerenLogic;

namespace SomerenUI
{
    public partial class OrderForm : Form
    {
         public Student_Service studentService = new Student_Service();
         public Drink_Service drinkService = new Drink_Service();
        public CashRegisterService cashRegisterService = new CashRegisterService();

        public OrderForm()
        {
            InitializeComponent();
        }
        private void OrderForm_Load(object sender, EventArgs e)
        {
            //fill the combo boxes
            List<Student> students = studentService.GetStudents();
            foreach (Student S in students)
            {
                cmbOrdStudents.Items.Add(S.Name);
            }
            
            List<Drink> drinks = drinkService.GetDrinks();
            foreach(Drink D in drinks)
            {
                cmbOrdDrinks.Items.Add(D.Name);
            }

        }

        private void btnCheckout_Click(object sender, EventArgs e)
        {
            Student student = null;
            bool error = false;
            //get the student and the drink by the name in the combo box
            if (cmbOrdStudents.SelectedIndex>=0)
            student = studentService.GetByName(cmbOrdStudents.SelectedItem.ToString());
            else
            {
                MessageBox.Show("You need to select a student", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                error = true;
            }

            Drink drink = null;
            if (cmbOrdDrinks.SelectedIndex>=0)
            drink = drinkService.GetByName(cmbOrdDrinks.SelectedItem.ToString());
            else
            {
                MessageBox.Show("You need to select a drink", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                error = true;
            }
           
            if (!System.Text.RegularExpressions.Regex.IsMatch(txtQuantity.Text, "[0-9]"))
                MessageBox.Show("Wrong input format for quantity!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
            {
                int quantityT = int.Parse(txtQuantity.Text);
                if (quantityT <= 0)
                    MessageBox.Show("Quantity can't be equal or lower than 0!", "Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                else
                {
                    if (!error)
                    {
                        //check if there are enough drinks available
                        if (drink.Stock - quantityT >= 0)
                        {
                            drink.Stock -= quantityT;
                            drinkService.UpdateDrink(drink, drink.Stock, drink.Name, drink.Price);
                            decimal totalPriceT = (decimal)quantityT * drink.Price;

                            //create new order
                            CashRegister order = new CashRegister()
                            {
                                student = student,
                                drink = drink,
                                quantity = quantityT,
                                totalPrice = totalPriceT,
                                orderDate = DateTime.Now
                            };

                            //get the years difference from now to the birth date
                            int age = DateTime.Now.Year - order.student.BirthDate.Year;
                            //if his birthdate was already this year, add 1 year to the age
                            if (DateTime.Now.Month > order.student.BirthDate.Month)
                                age++;

                            if (age < 18 && drink.drinkType)
                                MessageBox.Show("You can't buy alcohol if you are underage!", "Underage", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                            else
                            {
                              
                                    DialogResult Result = MessageBox.Show("Total Price: " + totalPriceT + "\nAre you sure?", "Proceed", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                                    if (Result == DialogResult.Yes)
                                        cashRegisterService.AddOrder(order);
        

                            }
                        }
                    }
                  
                    else
                      MessageBox.Show("Out of Stock ", "Out of Stock", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
