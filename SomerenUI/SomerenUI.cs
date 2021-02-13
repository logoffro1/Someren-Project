using SomerenLogic;
using SomerenModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace SomerenUI
{
    public partial class SomerenUI : Form
    {
        //the current selected drink from the Combo Box
        private Drink selectedDrink;
        private bool buttonSelected; // this is for the drinks panel - false = edit button, true = remove button
        private bool buttonSelectedActivities; //this is for the activities panel - false = edit button, true = remove button
        private int buttonSelectedSupervisor = 0; //this is for the supervisor panel - 0 = none, 1 = add, 2 = remove, 3 = edit

        private DateTime startDate; // startTime for the timetables panel
        private DateTime endDate; // endTime for the timetables panel
        public SomerenUI()
        {
            InitializeComponent();
       
          //  this.weekCalendar.SelectionChanged += weekCalendar_SelectionChanged;
        }
     
        private void SomerenUI_Load(object sender, EventArgs e)
        {
            showPanel("Dashboard");
            weekCalendar.MaxSelectionCount = 1;
        }
        //hide all the panels except the one passed as a parameter
        private void HidePanels(Panel pnl)
        {
            //fill the list with the panels
            List<Panel> panels = new List<Panel>();
            panels.Add(pnl_Dashboard);
            panels.Add(pnl_lecturers);
            panels.Add(pnl_room);
            panels.Add(pnl_Drinks);
            panels.Add(pnl_Students);
            panels.Add(pnl_Register);
            panels.Add(pnl_revenue);
            panels.Add(pnl_Activities);
            panels.Add(pnl_supervisors);
            panels.Add(pnl_timetable);

            //loop through the list, hide all except the one passed as a parameter
            foreach (Panel p in panels)
            {
                if (p != pnl)
                    p.Hide();
            }
            if (pnl != pnl_Dashboard)
                img_Dashboard.Hide();
            else
                img_Dashboard.Show();

            pnl.Show();
        }
        private void showPanel(string panelName)
        {

            //show all panels
            switch (panelName)
            {
                case "Dashboard":
                    // hide all other panels
                    HidePanels(pnl_Dashboard);
                    break;
                #region students
                case "Students":
                    HidePanels(pnl_Students);
                    // clear the listview before filling it again
                    listViewStudents.Clear();

                    // fill the students listview within the students panel with a list of students
                    Student_Service studService = new Student_Service();
                    List<Student> studentList = studService.GetStudents();

                    ColumnHeader name = new ColumnHeader();
                    name.Text = "Name";
                    ColumnHeader ID = new ColumnHeader();
                    ID.Text = "Student_ID";
                    ColumnHeader DOB = new ColumnHeader();
                    DOB.Text = "Birth_Date";
                    ColumnHeader Room_Number = new ColumnHeader();
                    Room_Number.Text = "Room_Number";
                    listViewStudents.Columns.AddRange(new ColumnHeader[] { ID, name, DOB, Room_Number });

                    //Saves the required data for each student in a row in a correct format.
                    foreach (SomerenModel.Student s in studentList)
                    {
                        ListViewItem li = new ListViewItem(s.Number.ToString(), 0);
                        li.SubItems.Add(s.Name);
                        li.SubItems.Add(s.BirthDate.ToString("dd/MM/yyyy"));
                        li.SubItems.Add(s.RoomID.ToString());
                        listViewStudents.Items.Add(li);

                    }
                    //expands the header size.
                    listViewStudents.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                    break;

                #endregion
                #region lecturers
                case "Lecturers":
                    //hides everything except lecturer panel.
                    HidePanels(pnl_lecturers);
                    listViewLecturers.Clear();


                    Lecturer_Service Lecturers = new Lecturer_Service();
                    List<Teacher> lecturerList = Lecturers.GetLecturers();

                    // creates and matches columns
                    ColumnHeader lecturerName = new ColumnHeader();
                    lecturerName.Text = "Name";

                    ColumnHeader lecturerId = new ColumnHeader();
                    lecturerId.Text = "Teacher_ID";

                    ColumnHeader lecturerRoom_Number = new ColumnHeader();
                    lecturerRoom_Number.Text = "Room_Number";
                    listViewLecturers.Columns.AddRange(new ColumnHeader[] { lecturerId, lecturerName, lecturerRoom_Number });
                    //Saves the required data for each teacher in a row in a correct format.
                    foreach (SomerenModel.Teacher t in lecturerList)
                    {
                        ListViewItem li = new ListViewItem(t.Number.ToString(), 0);
                        li.SubItems.Add(t.Name);
                        li.SubItems.Add(t.RoomID.ToString());
                        listViewLecturers.Items.Add(li);
                    }
                    listViewLecturers.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                    break;
                #endregion
                #region rooms
                case "Rooms":
                    // hides other panels
                    HidePanels(pnl_room);
                    // clears room list view for upcoming data.
                    listViewRooms.Clear();


                    ColumnHeader room_id = new ColumnHeader();
                    room_id.Text = "room_id";
                    ColumnHeader capacity = new ColumnHeader();
                    capacity.Text = "Capacity";
                    ColumnHeader ResidentType = new ColumnHeader();
                    ResidentType.Text = "Resident Type";

                    listViewRooms.Columns.AddRange(new ColumnHeader[] { room_id, capacity, ResidentType });

                    Room_Service roomservice = new Room_Service();
                    List<Room> Roomlist = roomservice.GetRooms();
                    //Saves the required data for each room in a row in a correct format.
                    foreach (SomerenModel.Room r in Roomlist)
                    {
                        ListViewItem li = new ListViewItem(r.Number.ToString(), 0);
                        li.SubItems.Add(r.Capacity.ToString());
                        if (r.Type == false)
                        {
                            li.SubItems.Add("Teacher");
                        }
                        else
                        {
                            li.SubItems.Add("Student");
                        }

                        listViewRooms.Items.Add(li);
                    }
                    listViewRooms.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

                    break;
                #endregion
                #region drinks
                case "Drinks":

                    selectedDrink = null; //set selected drink to null whenever you open the panel

                    //change the buttons/text boxes visibility
                    ChangeVisibility(false);
                    ChangeButtonsVis(true);

                    // hides other panels
                    cmbDrinks.Hide();
                    HidePanels(pnl_Drinks);

                    //clears the drink list view
                    listViewDrinks.Clear();


                    ColumnHeader drinkId = new ColumnHeader();
                    drinkId.Text = "drink_id";
                    ColumnHeader drinkName = new ColumnHeader();
                    drinkName.Text = "Name";
                    ColumnHeader price = new ColumnHeader();
                    price.Text = "Price";
                    ColumnHeader stock = new ColumnHeader();
                    stock.Text = "Stock";
                    ColumnHeader type = new ColumnHeader();
                    type.Text = "Type";
                    ColumnHeader available = new ColumnHeader();
                    available.Text = "";

                    listViewDrinks.Columns.AddRange(new ColumnHeader[] { drinkId, drinkName, price, stock, type, available });

                    Drink_Service drinkService = new Drink_Service();
                    List<Drink> drinkList = drinkService.GetDrinks();

                    //Saves the required data for each room in a row in a correct format.
                    foreach (Drink d in drinkList)
                    {
                        ListViewItem li = new ListViewItem(d.Id.ToString(), 0);
                        li.SubItems.Add(d.Name.ToString());
                        li.SubItems.Add(d.Price.ToString("0.00"));
                        li.SubItems.Add(d.Stock.ToString());

                        //if the drinkType is true, then that means the drink is alcoholic, show "alcoholic" in the column, otherwise, show "Non-Alcoholic"
                        if (d.drinkType == true)
                            li.SubItems.Add("Alcoholic");
                        else
                            li.SubItems.Add("Non-Alcoholic");

                        //show the drink availability based on the stock
                        if (!(d.Name == "Water" || d.Name == "Orangeade" || d.Name == "Cherry Juice"))
                        {
                            if (d.Stock < 10)
                                li.SubItems.Add("Stock nearly depleted");
                            else
                                li.SubItems.Add("Stock sufficient");
                        }
                        listViewDrinks.Items.Add(li);
                    }
                    listViewDrinks.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                    break;
                #endregion
                #region orders
                case "Orders":
                    //Hide other panels
                    HidePanels(pnl_Register);

                    listViewOrders.Clear();

                    ColumnHeader order_id = new ColumnHeader();
                    order_id.Text = "order_id";
                    ColumnHeader id = new ColumnHeader();
                    id.Text = "drink_id";
                    ColumnHeader drinkNameO = new ColumnHeader();
                    drinkNameO.Text = "Drink Name";
                    ColumnHeader drinkPrice = new ColumnHeader();
                    drinkPrice.Text = "Drink Price";
                    ColumnHeader drinkStock = new ColumnHeader();
                    drinkStock.Text = "Stock";
                    ColumnHeader StudentName = new ColumnHeader();
                    StudentName.Text = "Student Name";
                    ColumnHeader StudentID = new ColumnHeader();
                    StudentID.Text = "student_id";
                    ColumnHeader Quantity = new ColumnHeader();
                    Quantity.Text = "Quantity";
                    ColumnHeader TotalAmount = new ColumnHeader();
                    TotalAmount.Text = "Total Price";
                    ColumnHeader orderDate = new ColumnHeader();
                    orderDate.Text = "Order Date";

                    listViewOrders.Columns.AddRange(new ColumnHeader[] { order_id, StudentID, StudentName, id, drinkNameO, drinkPrice, drinkStock, Quantity, TotalAmount, orderDate });

                    CashRegisterService cashRegisterService = new CashRegisterService();
                    List<CashRegister> orders = cashRegisterService.getAllOrders();

                    foreach (CashRegister order in orders)
                    {
                        ListViewItem li = new ListViewItem(order.order_id.ToString());
                        li.SubItems.Add(order.student.Number.ToString());
                        li.SubItems.Add(order.student.Name);
                        li.SubItems.Add(order.drink.Id.ToString());
                        li.SubItems.Add(order.drink.Name);
                        li.SubItems.Add(order.drink.Price.ToString());
                        li.SubItems.Add(order.drink.Stock.ToString());
                        li.SubItems.Add(order.quantity.ToString());
                        li.SubItems.Add(order.totalPrice.ToString());
                        li.SubItems.Add(order.orderDate.ToString("dd/MM/yyyy"));
                        listViewOrders.Items.Add(li);
                    }
                    listViewOrders.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

                    break;
                #endregion
                case "report":
                    HidePanels(pnl_revenue);
                    break;
                #region Activities
                case "Activities":
                    //hide all other panels
                    HidePanels(pnl_Activities);

                    listViewActivities.Clear();

                    btnAddActivity.Show();
                    btnEditActivity.Show();
                    btnRemoveActivity.Show();
                    cmbActivities.Hide();
                    btnApplyActivities.Hide();

                    //set the date boxes from the edit sections to be hidden
                    VisibilityDateBoxes(false);


                    Activity_Service activity_service = new Activity_Service();
                    List<Activity> activities = activity_service.GetAllActivities();
                    cmbActivities.Items.Clear();

                    //fill the combo box with the activities
                    foreach (Activity activity in activities)
                        cmbActivities.Items.Add(activity.activity_name + " ID:" + activity.activity_id.ToString());

                    cmbActivities.SelectedIndex = -1;

                    ColumnHeader activity_id = new ColumnHeader();
                    activity_id.Text = "Activity ID";
                    ColumnHeader activity_name = new ColumnHeader();
                    activity_name.Text = "Activity Name";
                    ColumnHeader nrOfStudents = new ColumnHeader();
                    nrOfStudents.Text = "Number Students";
                    ColumnHeader nrOfSupervisors = new ColumnHeader();
                    nrOfSupervisors.Text = "Number Supervisors";
                    ColumnHeader activity_date = new ColumnHeader();
                    activity_date.Text = "Activity Date";

                    listViewActivities.Columns.AddRange(new ColumnHeader[] { activity_id, activity_name, nrOfStudents, nrOfSupervisors, activity_date });

                    Activity_Service activity_Service = new Activity_Service();
                    foreach (Activity activity in activities)
                    {
                        ListViewItem li = new ListViewItem(activity.activity_id.ToString());
                        li.SubItems.Add(activity.activity_name);
                        li.SubItems.Add(activity.nrOfStudents.ToString());

                        if (activity.nrOfSupervisors <= 0)
                            li.SubItems.Add("Unsupervised");
                        else
                            li.SubItems.Add(activity.nrOfSupervisors.ToString());
                        li.SubItems.Add(activity.activity_Date.ToString("dd/MM/yyyy hh:mm"));
                        listViewActivities.Items.Add(li);
                    }
                    listViewActivities.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                    break;
                #endregion
                #region supervisor
                case "supervisors":
                    //hide all other panels
                    HidePanels(pnl_supervisors);

                    //hide or show the different buttons/labels/combo boxes
                    listViewSupervisors.Clear();
                    cmbSupervisors.Hide();
                    cmbSelectActivity.Hide();
                    cmbSupervisors.Items.Clear();
                    cmbSelectActivity.Items.Clear();
                    btnApplySupervisor.Hide();
                    btnAddSupervisor.Show();
                    btnRemoveSupervisor.Show();
                    btnEditSupervisor.Show();

                    Supervisor_Service supervisor_service = new Supervisor_Service();
                    List<Supervisor> supervisors = supervisor_service.GetAllSupervisors();
                    Activity_Service acService = new Activity_Service();
                    Lecturer_Service lecturerService = new Lecturer_Service();
                    List<Teacher> teacherList = lecturerService.GetLecturers();
                    List<Activity> activitiesList = acService.GetAllActivities();

                    cmbSelectActivity.Items.Clear();

                    cmbSelectActivity.Items.Add("Select activity");
                    //fill the combo box with all the available activities
                    foreach (Activity activity in activitiesList)
                        cmbSelectActivity.Items.Add($"{activity.activity_name}ID:{activity.activity_id}");


                    cmbSupervisors.ResetText();
                 //   cmbSupervisors.SelectedIndex = 0;

                    cmbSelectActivity.ResetText();
                    cmbSelectActivity.SelectedIndex = 0;
                    ColumnHeader supervisors_id = new ColumnHeader();
                    supervisors_id.Text = "Supervisor ID";
                    ColumnHeader supervisor_name = new ColumnHeader();
                    supervisor_name.Text = "Supervisor Name";
                    ColumnHeader activity_nameId = new ColumnHeader();
                    activity_nameId.Text = "Activity Name(id)";

                    listViewSupervisors.Columns.AddRange(new ColumnHeader[] { supervisors_id, supervisor_name, activity_nameId });

                    foreach (Supervisor supervisor in supervisors)
                    {
                        ListViewItem li = new ListViewItem(supervisor.supervisor_id.ToString());
                        li.SubItems.Add(supervisor.Name);
                        Activity activity = acService.GetById(supervisor.activity_id);
                        li.SubItems.Add($"{activity.activity_name}({activity.activity_id.ToString()})");
                        listViewSupervisors.Items.Add(li);
                    }
                    listViewSupervisors.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                    break;
                #endregion
                #region timetable
                case "timetable":
                //    //hide all other panels
                 HidePanels(pnl_timetable);

                  
                   listViewTimetable.Clear();
                    cmbActivitiesType.Hide();
                    label11.Hide();
                    Activity_Service actService = new Activity_Service();
                    List<string> activitiesListt = actService.GetActivityTypes();
                    cmbActivitiesType.Items.Clear();
                    cmbActivitiesType.Items.Add("Select activity");
                    foreach (string s in activitiesListt)
                        cmbActivitiesType.Items.Add(s);

                    cmbActivitiesType.SelectedIndex = 0;

                    ColumnHeader timetable_id = new ColumnHeader();
                    timetable_id.Text = "Timetable ID";
                    ColumnHeader activityName = new ColumnHeader();
                    activityName.Text = "Activity Name(id)";
                    ColumnHeader nrOfSupervisorsTime = new ColumnHeader();
                    nrOfSupervisorsTime.Text = "Number Supervisors";
                    ColumnHeader activityDate = new ColumnHeader();
                    activityDate.Text = "Activity Date";
                    ColumnHeader activityStartTime = new ColumnHeader();
                    activityStartTime.Text = "Start Time";
                    ColumnHeader activityEndTime = new ColumnHeader();
                    activityEndTime.Text = "End Time";

                    listViewTimetable.Columns.AddRange(new ColumnHeader[] { timetable_id, activityName, nrOfSupervisorsTime, activityDate, activityStartTime, activityEndTime });
                    listViewTimetable.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                    break;
                    #endregion
            }
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void dashboardToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            showPanel("Dashboard");
        }

        private void img_Dashboard_Click(object sender, EventArgs e)
        {
            MessageBox.Show("What happens in Someren, stays in Someren!");
        }

        private void studentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showPanel("Students");
        }

        private void lecturersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showPanel("Lecturers");
        }
        private void roomsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            showPanel("Rooms");
        }
        private void drinkSuppliesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showPanel("Drinks");
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            buttonSelected = false;
            //show the combo box with the drink
            FillComboBox();
            ChangeButtonsVis(false);
            ChangeVisibility(false);

        }

        private void FillComboBox()
        {
            cmbDrinks.Show();
            cmbDrinks.Items.Clear();
            cmbDrinks.ResetText();
            lblSelectDrink.Visible = true;
            //to reset selected value
            cmbDrinks.SelectedIndex = -1;
            //fill the drink combo box
            Drink_Service drinkService = new Drink_Service();
            List<Drink> drinkList = drinkService.GetDrinks();
            foreach (Drink drink in drinkList)
                cmbDrinks.Items.Add(drink.Name.ToString());
        }
        private void ChangeButtonsVis(bool change)
        {
            //change the visible / enabled values for the Add/Remove/Edit buttons
            btnEdit.Visible = change;
            btnEdit.Enabled = change;

            btnAdd.Visible = change;
            btnAdd.Enabled = change;

            btnRemove.Visible = change;
            btnRemove.Enabled = change;
        }
        private void ChangeVisibility(bool change)
        {

            //Change the text boxes visibility
            txtNewName.Visible = change;
            txtNewName.Enabled = change;
            txtNewStock.Visible = change;
            txtNewStock.Enabled = change;
            txtNewPrice.Visible = change;
            txtNewPrice.Enabled = change;
            //change the buttons visibility
            btnApply.Enabled = change;
            btnApply.Visible = change;


            txtNewName.Text = "New Name";

            txtNewStock.Text = "New Stock";

            if (buttonSelected)
            {
                btnApply.Visible = !change;
                btnApply.Enabled = !change;
            }

        }
        //the apply button for the drink panel
        private void btnApply_Click(object sender, EventArgs e)
        {
            Drink_Service drinkService = new Drink_Service();
            // txtNewName.Text = selectedDrink.Id.ToString();
            //error handling
            if (!buttonSelected)
            {
                if (txtNewName.Text == "New Name")
                {
                    txtNewName.Text = selectedDrink.Name;
                }
                if (txtNewPrice.Text == "New Price")
                {
                    txtNewPrice.Text = selectedDrink.Price.ToString();
                }
                if (txtNewStock.Text == "New Stock")
                {
                    txtNewStock.Text = selectedDrink.Stock.ToString();
                }
                if (!System.Text.RegularExpressions.Regex.IsMatch(txtNewStock.Text, "[0-9]"))
                {
                    MessageBox.Show("The stock has to be a number!", "Stock", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else if (!Regex.IsMatch(txtNewPrice.Text, @"[\d]{1,4}([.,][\d]{1,2})?"))
                {
                    MessageBox.Show("The price was in the wrong format!", "Price", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (cmbDrinks.SelectedIndex == -1)
                    MessageBox.Show("You need to select a drink!", "Select Drink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else if (txtNewName.Text == "")
                    MessageBox.Show("You need to enter a new drink name!", "Select Name", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                {

                    string[] nrTemp = txtNewPrice.Text.Split('.');
                    if (nrTemp.Length == 1)
                        txtNewPrice.Text += ".00";
                    if (int.Parse(txtNewStock.Text) < 0 || double.Parse(txtNewPrice.Text) < 0)
                        MessageBox.Show("The price or the stock can't be less than 0", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    else
                    {
                        lblSelectDrink.Hide();
                        drinkService.UpdateDrink(selectedDrink, int.Parse(txtNewStock.Text), txtNewName.Text, decimal.Parse(txtNewPrice.Text));
                        MessageBox.Show("Drink Updated!", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        showPanel("Drinks");
                    }
                }
            }

            else
            {
                if (selectedDrink == null)
                    MessageBox.Show("You must select a drink!", "Select Drink", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                {
                    try
                    {
                        lblSelectDrink.Visible = false;
                        drinkService.RemoveDrink(selectedDrink);
                        MessageBox.Show("Drink removed!", "Removed", MessageBoxButtons.OK);
                        showPanel("Drinks");
                    }
                    catch
                    {
                        Error_Service error = new Error_Service("Could not remove the drink!");
                    }
                }
            }
        }
        private void cmbDrinks_SelectedIndexChanged(object sender, EventArgs e)
        {
            //when the user pressed on a new drink, add that drink in the selected drink
            Drink_Service drinkService = new Drink_Service();

            selectedDrink = drinkService.GetByName(cmbDrinks.SelectedItem.ToString());
            if (!buttonSelected)
                ChangeVisibility(true);
            else
                ChangeVisibility(false);
        }
        //the remove button for the drinks panel
        private void btnRemove_Click(object sender, EventArgs e)
        {
            buttonSelected = true;
            lblSelectDrink.Visible = false;
            FillComboBox();
            ChangeButtonsVis(false);
        }
        //the add button for the drinks panel
        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddDrinkForm addDrinkForm = new AddDrinkForm();
            addDrinkForm.ShowDialog();
        }
        //the refresh button for the drinks panel
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            lblSelectDrink.Visible = false;
            showPanel("Drinks");
        }
        private void cashRegisterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showPanel("Orders");
        }
        private void btnAddOrder_Click(object sender, EventArgs e)
        {
            OrderForm order = new OrderForm();
            order.ShowDialog();
        }
        //the refresh button for the CashRegister panel
        private void btnRefresh2_Click(object sender, EventArgs e)
        {
            showPanel("Orders");
        }
        private void pnl_revenue_Paint(object sender, PaintEventArgs e)
        {
            calendarStart.MaxDate = DateTime.Today;
            calendarEnd.MaxDate = DateTime.Today;

            startSelectedLbl.Text = calendarStart.SelectionStart.ToString("dd/MM/yyyy");
            endSelectedLbl.Text = calendarEnd.SelectionStart.ToString("dd/MM/yyyy");
        }
        private void revenueReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showPanel("report");
        }

        //the calculate button for the revenue report panel
        private void btnCalculate_Click(object sender, EventArgs e)
        {
            //save the selected dates
            DateTime startDate = calendarStart.SelectionStart;
            DateTime endDate = calendarEnd.SelectionStart;

            if (endDate < startDate)
                MessageBox.Show("The end date can't be older than the start date!", "Date Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                //get all the total sales
                CashRegisterService orderService = new CashRegisterService();
                List<CashRegister> allOrders = orderService.GetOrderForDates(startDate, endDate);
                int totalSales = 0;
                foreach (CashRegister order in allOrders)
                {
                    if (order.orderDate.Day >= startDate.Day && order.orderDate.Day <= endDate.Day)
                        totalSales += order.quantity;
                }
                soldLbl.Text = $"Total number of drinks sold: {totalSales.ToString()}";

                //get all the distinct customers
                List<int> studentIds = new List<int>();
                int totalCustomers = 0;
                foreach (CashRegister order in allOrders)
                {

                    if (!studentIds.Contains(order.student.Number))
                    {
                        studentIds.Add(order.student.Number);
                        totalCustomers++;
                    }
                }
                customersLbl.Text = $"Number of customers: {totalCustomers.ToString()}";

                //get the turnover
                decimal turnover = 0;
                foreach (CashRegister order in allOrders)
                {
                    turnover += order.totalPrice;
                }
                turnoverLbl.Text = $"Turnover: {turnover}€";

                //get the most sold drink
                Drink_Service drinkService = new Drink_Service();
                List<Drink> drinks = drinkService.GetDrinks();

                Drink mvpDrink = new Drink();
                int timesOrdered = 0;
                Dictionary<Drink, int> soldDrinks = new Dictionary<Drink, int>();
                //add the drinks and the quantity to the dictionary 
                foreach (CashRegister order in allOrders)
                {
                    soldDrinks.Add(order.drink, order.quantity);
                }
                //loop through all drinks, if a drink has a more timesOrdered, put it as the MVP drink
                foreach (Drink drink in drinks)
                {
                    Drink tempDrink = new Drink();
                    int tempCounter = 0;
                    //loop through the dictionary
                    foreach (KeyValuePair<Drink, int> kvp in soldDrinks)
                    {
                        if (kvp.Key.Id == drink.Id)
                        {
                            tempCounter += kvp.Value;
                            tempDrink = kvp.Key;
                        }
                    }

                    if (tempCounter >= timesOrdered)
                    {
                        timesOrdered = tempCounter;
                        mvpDrink = tempDrink;

                    }
                }
                mvpDrinkLbl.Text = $"Most sold drink: {mvpDrink.Name}({timesOrdered.ToString()})";
            }
        }
        private void calendarEnd_DateChanged(object sender, DateRangeEventArgs e)
        {
            endSelectedLbl.Text = calendarEnd.SelectionStart.ToString("dd/MM/yyyy");
        }
        private void calendarStart_DateChanged(object sender, DateRangeEventArgs e)
        {
            startSelectedLbl.Text = calendarStart.SelectionStart.ToString("dd/MM/yyyy");
        }
        private void activitiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showPanel("Activities");
        }
        private void btnRemoveActivity_Click(object sender, EventArgs e)
        {
            buttonSelectedActivities = true;
            cmbActivities.ResetText();
            Activity_Service activityService = new Activity_Service();
            btnAddActivity.Hide();
            btnEditActivity.Hide();
            btnRemoveActivity.Hide();
            cmbActivities.Show();
        }

        private void btnApplyActivities_Click(object sender, EventArgs e)
        {
            Activity_Service activityService = new Activity_Service();
            if (buttonSelectedActivities)
            {

                DialogResult result = MessageBox.Show("Are you sure you want to remove this activity?", "Remove", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
              
                if (result == DialogResult.Yes)
                {
                    string[] divideNameId = cmbActivities.SelectedItem.ToString().Split(':'); //get the activity id from the combo box
                    Activity activity = activityService.GetById(int.Parse(divideNameId[1]));
                    activityService.RemoveActivity(activity);
                    MessageBox.Show("Activity deleted!", "Deleted", MessageBoxButtons.OK);
                    showPanel("Activities");
                }
                else
                    MessageBox.Show("No changes have been made");
            }
            else
            {
                //if the time is invalid, set error to true, show error to the user
                bool error = false;
                if (cmbDay.SelectedIndex <= 0 || cmbMonth.SelectedIndex <= 0 || cmbYear.SelectedIndex <= 0 || cmbHour.SelectedIndex <= 0 || cmbMins.SelectedIndex <= 0)
                    error = true;

                string[] divideNameId = cmbActivities.SelectedItem.ToString().Split(':');
                Activity activity = activityService.GetById(int.Parse(divideNameId[1]));
                string activity_date = cmbYear.SelectedItem.ToString();

                //if the selected item is only 1 character, add a '0' to make it in the "00" format
                if (cmbMonth.SelectedItem.ToString().Length < 2)
                    activity_date += "0" + cmbMonth.SelectedItem.ToString();
                else
                    activity_date += cmbMonth.SelectedItem.ToString();
                if (cmbDay.SelectedItem.ToString().Length < 2)
                    activity_date += "0" + cmbMonth.SelectedItem.ToString();
                else
                    activity_date += cmbDay.SelectedItem.ToString();

                if (cmbHour.SelectedItem.ToString().Length < 2)
                    activity_date += " 0" + cmbHour.SelectedItem.ToString();
                else
                    activity_date += " " + cmbHour.SelectedItem.ToString();

                if (cmbMins.SelectedItem.ToString().Length < 2)
                    activity_date += ":0" + cmbMins.SelectedItem.ToString();
                else
                    activity_date += ":" + cmbMins.SelectedItem.ToString();


                if (!error)
                {
                    activityService.UpdateActivity(activity, activity_date);
                    MessageBox.Show("Activity Updated", "updated", MessageBoxButtons.OK);
                    showPanel("Activities");
                }
                else
                    MessageBox.Show("Please select a proper time", "Time error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void btnEditActivity_Click(object sender, EventArgs e)
        {
            buttonSelectedActivities = false;
            //set the date boxes from the edit section to be visible
            VisibilityDateBoxes(true);
            cmbActivities.ResetText();
            cmbActivities.SelectedIndex = -1;
            btnAddActivity.Hide();
            btnEditActivity.Hide();
            btnRemoveActivity.Hide();
            cmbActivities.Show();


            FillTimeBoxes();
        }
        private void VisibilityDateBoxes(bool show)
        {
            //show or hide the time combo boxes
            if (show)
            {
                cmbDay.Show();
                cmbMonth.Show();
                cmbYear.Show();
                cmbHour.Show();
                cmbMins.Show();
            }
            else
            {
                cmbDay.Hide();
                cmbMonth.Hide();
                cmbYear.Hide();
                cmbHour.Hide();
                cmbMins.Hide();
            }
            //clear the combo boxes to avoid errors
            cmbDay.Items.Clear();
            cmbMins.Items.Clear();
            cmbMonth.Items.Clear();
            cmbYear.Items.Clear();
            cmbHour.Items.Clear();
        }
        private void FillTimeBoxes()
        {
            //fill the time boxes
            cmbDay.Items.Add("Day");
            cmbMonth.Items.Add("Month");
            cmbYear.Items.Add("Year");
            cmbHour.Items.Add("Hour");
            cmbMins.Items.Add("Min");

            cmbDay.SelectedIndex = 0;
            cmbMonth.SelectedIndex = 0;
            cmbYear.SelectedIndex = 0;
            cmbHour.SelectedIndex = 0;
            cmbMins.SelectedIndex = 0;

            //fill the day combo box with 31 days, remove days later depending on the month
            for (int i = 1; i <= 31; i++)
                cmbDay.Items.Add(i.ToString());

            for (int i = 1; i <= 12; i++)
            {
                cmbMonth.Items.Add(i.ToString());
            }
            for (int i = DateTime.Now.Year; i <= DateTime.Now.AddYears(5).Year; i++)
            {
                cmbYear.Items.Add(i.ToString());
            }
            for (int i = 1; i <= 24; i++)
                cmbHour.Items.Add(i.ToString());
            for (int i = 0; i <= 55; i += 5)
                cmbMins.Items.Add(i.ToString());

        }
        private void cmbActivities_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnApplyActivities.Show();
        }
        private void btnAddActivity_Click(object sender, EventArgs e)
        {
            AddActivityForm form = new AddActivityForm();
            form.ShowDialog();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            showPanel("Activities");
        }
        private void cmbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            // cmbDay.Items.Clear();
            string month = cmbMonth.SelectedItem.ToString();

            //if the month is february, remove the last 3 days to make it 28 days
            if (month == "2")
            {
                cmbDay.Items.RemoveAt(31);
                cmbDay.Items.RemoveAt(30);
                cmbDay.Items.RemoveAt(29);
            }
            //if the month is april, june, september or november, remove the last day to make it 30 days, for the rest of the days, leave it at 31
            else if (month == "4" || month == "6" || month == "9" || month == "11")
                cmbDay.Items.RemoveAt(31);
        }
        private void supervisorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showPanel("supervisors");
        }
        private void btnApplySupervisor_Click(object sender, EventArgs e)
        {
            Supervisor_Service supervisor_service = new Supervisor_Service();
            Activity_Service activityService = new Activity_Service();

            List<Supervisor> supervisorsList = supervisor_service.GetAllSupervisors(); // add all supervisors to the list
          
            if (buttonSelectedSupervisor == 1) // if the add button is selected
            {
             
                if (cmbSupervisors.SelectedIndex >= 1 && cmbSelectActivity.SelectedIndex >= 1) // continue if both boxes are selected, give error msg if not
                {
                    string[] supervisorSplit = cmbSupervisors.SelectedItem.ToString().Split(':'); // get the ID of the supervisor from the combo box
                    string[] activitySplit = cmbSelectActivity.SelectedItem.ToString().Split(':'); // get the activity ID from the combo box
                    bool proceed = true;

                    //create new supervisor
                    Supervisor supervisor = new Supervisor()
                    {
                        Number = int.Parse(supervisorSplit[1]),
                        activity_id = int.Parse(activitySplit[1])

                    };
                    List<Activity> activities = supervisor_service.GetTeacherActivities(supervisor.Number); // get all the activities for that supervisor
                    Activity activity = activityService.GetById(int.Parse(activitySplit[1]));

                    foreach (Activity act in activities)
                    {
                        //if the supervisor already supervises an activity on that day, give an error
                        if ((act.activity_Date.Year == activity.activity_Date.Year) && (act.activity_Date.Month == activity.activity_Date.Month) && (act.activity_Date.Day == activity.activity_Date.Day))
                        {
                            proceed = false;
                        }
                    }
                    if (proceed)
                    {
                        //if there are no errors, add a new supervisor
                        supervisor_service.AddSupervisor(supervisor);
                        showPanel("supervisors");
                        MessageBox.Show("Supervisor added","Added succesfully");
                    }
                    else
                        MessageBox.Show("This teacher is already a supervisor!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                    MessageBox.Show("You need to select a supervisor and an activity!","Error",MessageBoxButtons.OK,MessageBoxIcon.Warning);

            }
            else if (buttonSelectedSupervisor == 2) // if the remove button is selected
            {
                if (cmbSupervisors.SelectedIndex <= 0)
                    MessageBox.Show("You need to select a supervisor!","Error",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                else
                {
                    string[] supervisorSplit = cmbSupervisors.SelectedItem.ToString().Split(':'); // get the ID of the supervisor from the combo box          
                    Supervisor supervisor = supervisor_service.GetById(int.Parse(supervisorSplit[1]));
                    DialogResult result = MessageBox.Show("Are you sure you wanna delete this supervisor?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                    {
                        supervisor_service.RemoveSupervisor(supervisor);
                        showPanel("supervisors");
                        MessageBox.Show("Supervisor deleted!", "Deleted", MessageBoxButtons.OK);
                        cmbSupervisors.Hide();
                        btnApplySupervisor.Hide();
                    }
                }             
            } else if (buttonSelectedSupervisor == 3)
            {
                if (cmbSupervisors.SelectedIndex <= 0 || cmbSupervisors.SelectedIndex <=0)
                    MessageBox.Show("You need to select a supervisor and a new activity!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                else
                {
                    bool proceed = true;

                    string[] activitySplit = cmbSelectActivity.SelectedItem.ToString().Split(':'); // get the activity ID from the combo box
                    string[] supervisorSplit = cmbSupervisors.SelectedItem.ToString().Split(':'); // get the ID of the supervisor from the combo box  

                    Activity activity = activityService.GetById(int.Parse(activitySplit[1]));

                    Supervisor supervisor = supervisor_service.GetById(int.Parse(supervisorSplit[1]));

                    List<Activity> activities = supervisor_service.GetTeacherActivities(supervisor.Number); // get all the activities for that supervisor
                    
                    foreach (Activity act in activities)
                    {
                        //if the supervisor already supervises an activity on that day, give an error
                        if (DateTime.Compare(act.activity_Date,activity.activity_Date)==0)
                        {
                            MessageBox.Show("test");
                            proceed = false;
                        }
                    }
                    if (proceed)
                    {
                        supervisor_service.UpdateSupervisor(supervisor, int.Parse(activitySplit[1]));
                        showPanel("supervisors");
                        MessageBox.Show("Supervisor updated!", "Updated", MessageBoxButtons.OK);
                        cmbSupervisors.Hide();
                        cmbActivities.Hide();
                        btnApplySupervisor.Hide();
                    }
                    else
                        MessageBox.Show("The supervisor is busy on that date","Error",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                }
            }
        }
        private void btnRemoveSupervisor_Click(object sender, EventArgs e)
        {
            Supervisor_Service supervisor_service = new Supervisor_Service();
            List<Supervisor> supervisors = supervisor_service.GetAllSupervisors();

            cmbSupervisors.Items.Clear();
            cmbSelectActivity.Items.Clear();

            cmbSupervisors.Items.Add("Select supervisor");

            foreach (Supervisor supervisor in supervisors)
                cmbSupervisors.Items.Add($"{supervisor.Name} ID:{supervisor.supervisor_id}");

            cmbSupervisors.SelectedIndex = 0;

            cmbSupervisors.Show();
            btnAddSupervisor.Hide();
            btnRemoveSupervisor.Hide();
            btnEditSupervisor.Hide();
            btnApplySupervisor.Show();

            buttonSelectedSupervisor = 2;


        }

        private void btnAddSupervisor_Click(object sender, EventArgs e)
        {
            Lecturer_Service lecturerService = new Lecturer_Service();
            List<Teacher> teacherList = lecturerService.GetLecturers();

            cmbSupervisors.Items.Clear();
            cmbSupervisors.Items.Add("Select supervisor");

            foreach (Teacher teacher in teacherList)
                cmbSupervisors.Items.Add($"{teacher.Name} ID:{teacher.Number}");

            cmbSelectActivity.SelectedIndex = 0;
            cmbSupervisors.SelectedIndex = 0;


            cmbSupervisors.Show();
            cmbSelectActivity.Show();
            btnAddSupervisor.Hide();
            btnRemoveSupervisor.Hide();
            btnEditSupervisor.Hide();
            btnApplySupervisor.Show();

            buttonSelectedSupervisor = 1;

        }

        private void btnEditSupervisor_Click(object sender, EventArgs e)
        {
            Supervisor_Service supervisorSerivce = new Supervisor_Service();
            List<Supervisor> supervisors = supervisorSerivce.GetAllSupervisors();

            cmbSupervisors.Items.Clear();
            cmbSupervisors.Items.Add("Select supervisor");
           

            foreach (Supervisor s in supervisors)
                cmbSupervisors.Items.Add($"{s.Name} ID:{s.supervisor_id}");

            cmbSelectActivity.SelectedIndex = 0;
            cmbSupervisors.SelectedIndex = 0;


            cmbSupervisors.Show();
            cmbSelectActivity.Show();
            btnAddSupervisor.Hide();
            btnRemoveSupervisor.Hide();
            btnEditSupervisor.Hide();
            btnApplySupervisor.Show();

            buttonSelectedSupervisor = 3;
        }
        public void weekCalendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            List<DateTime> selectedDates = new List<DateTime>();
            cmbActivitiesType.Show();
            label11.Show();

            int counter = 0;
            //while the selected day + counter is not saturday, add to the selectedDays list the date
            while (weekCalendar.SelectionRange.Start.AddDays(counter).DayOfWeek != DayOfWeek.Saturday)
            {
                selectedDates.Add(weekCalendar.SelectionRange.Start.AddDays(counter));

                counter++;

            }
            counter = -1;

            //while the selected day - counter is not sunday, add to the selectedDays list the date
            while (weekCalendar.SelectionRange.Start.AddDays(counter).DayOfWeek != DayOfWeek.Sunday)
            {
                selectedDates.Add(weekCalendar.SelectionRange.Start.AddDays(counter));
                counter--;
            }


            startDate = selectedDates[0];
            endDate = selectedDates[0];


            weekCalendar.BoldedDates = selectedDates.ToArray(); //bold all the dates(it's going to be the days from monday to friday, no sunday or saturday)

            for(int i = 0; i < selectedDates.Count; i++)
            {
                if (startDate >= selectedDates[i])
                    startDate = selectedDates[i];
            }
            for (int i = 0; i < selectedDates.Count; i++)
            {
                if (endDate <= selectedDates[i])
                    endDate = selectedDates[i];
            }
            Timetable_Service timetableService = new Timetable_Service();

           List<Timetable> timetables = timetableService.GetTimetableForDates(startDate,endDate);
            showTimetables(timetables);

  
           
        }
        private void showTimetables(List<Timetable> timetables)
        {
            listViewTimetable.Clear();
            ColumnHeader timetable_id = new ColumnHeader();
            timetable_id.Text = "Timetable ID";
            ColumnHeader activityName = new ColumnHeader();
            activityName.Text = "Activity Name(id)";
            ColumnHeader nrOfSupervisors = new ColumnHeader();
            nrOfSupervisors.Text = "Number Supervisors";
            ColumnHeader activityDate = new ColumnHeader();
            activityDate.Text = "Activity Date";
            ColumnHeader activityStartTime = new ColumnHeader();
            activityStartTime.Text = "Start Time";
            ColumnHeader activityEndTime = new ColumnHeader();
            activityEndTime.Text = "End Time";

            listViewTimetable.Columns.AddRange(new ColumnHeader[] { timetable_id, activityName, nrOfSupervisors, activityDate, activityStartTime, activityEndTime });
            foreach (Timetable time in timetables)
            {
                ListViewItem li = new ListViewItem(time.timetable_id.ToString());
                li.SubItems.Add($"{time.activity.activity_name}({time.activity.activity_id})");
                li.SubItems.Add(time.nrOfSupervisors.ToString());
                li.SubItems.Add(time.activity.activity_Date.ToString("dd/MM/yyyy"));
                li.SubItems.Add(time.startTime.ToString("hh:mm"));
                li.SubItems.Add(time.endTime.ToString("hh:mm"));
                listViewTimetable.Items.Add(li);
            }
            listViewTimetable.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }
        private void timetableToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showPanel("timetable");
        }

        private void cmbActivitiesType_MouseDown(object sender, MouseEventArgs e)
        {
            if (this.cmbActivitiesType.DroppedDown == false)
                this.cmbActivitiesType.DoDragDrop(this.cmbActivitiesType.Text, DragDropEffects.Copy);
            else
                this.cmbActivitiesType.DroppedDown = true;
        }

        private void listViewTimetable_DragEnter(object sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.Text))
            e.Effect = DragDropEffects.All;

        }

        private void listViewTimetable_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listViewTimetable_DragDrop(object sender, DragEventArgs e)
        {
            //  MessageBox.Show(e.Data.GetData(DataFormats.Text).ToString());
            Activity_Service activityService = new Activity_Service();
             List<string> activityTypes = activityService.GetActivityTypes();
            string draggedData = e.Data.GetData(DataFormats.Text).ToString();
            bool isCorrect = false;
            //check if the text dragged is a valid activity type
            foreach(string s in activityTypes)
            {
                if (s == draggedData)
                {
                    isCorrect = true;
                    break;
                }                  
            }

            if (isCorrect)
            {
                string activityDate = weekCalendar.SelectionStart.Year.ToString() +"-"
                    + weekCalendar.SelectionStart.Month.ToString("00")+"-"
                    + weekCalendar.SelectionStart.Day.ToString("00")+" "+"03"+":"+"30";
          
                
                Activity _activity = new Activity() //create new activity
                {
                    activity_name = draggedData,
                    nrOfStudents = 0,
                    nrOfSupervisors = 0,
                    activityDateString = activityDate
                   
                };

                activityService.AddActivity(_activity);
                Activity activityTime = activityService.GetBiggestId();
                _activity.activity_Date = DateTime.ParseExact(activityDate, "yyyy-MM-dd HH:mm", null);
                DateTime _startTime = _activity.activity_Date;
                //create a random duration for the activity, between 30 minutes and 120 minutes(only divisible by 5)
                Random rnd = new Random();
                int activityMinutes = rnd.Next(6, 25) * 5;
                //set the activity end time to the start time + the activity minutes
                DateTime _endTime = _startTime.AddMinutes(activityMinutes);

                Timetable timetable = new Timetable() //create new timetable
                {
                    activity = activityTime,
                    nrOfSupervisors = 0,
                    startTime = _startTime,
                    endTime = _endTime

                };
                activityService.AddTimetable(timetable);

                //refresh the page
                Timetable_Service timetableService = new Timetable_Service();
                List<Timetable> timetables = timetableService.GetTimetableForDates(startDate, endDate);
                showTimetables(timetables);
            }
        }
    }
}

