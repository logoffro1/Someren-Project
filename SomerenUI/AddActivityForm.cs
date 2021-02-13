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
    public partial class AddActivityForm : Form
    {
        public AddActivityForm()
        {
            InitializeComponent();
        }

        private void AddActivityForm_Load(object sender, EventArgs e)
        {
            calendarActivity.MinDate = DateTime.Today;

            Activity_Service activity_service = new Activity_Service();
            List<string> activityTypes = activity_service.GetActivityTypes();
            cmbActivityTypes.Items.Add("Create own activity type");

            foreach (string s in activityTypes) //fill the combo box with the activity types
                cmbActivityTypes.Items.Add(s);

            for (int i = 1; i <= 24; i++)
                cmbHour.Items.Add(i.ToString());
           
            for (int i = 0; i <= 55; i+=5)
                cmbMins.Items.Add(i.ToString());

            cmbHour.SelectedIndex = 0;
            cmbMins.SelectedIndex = 0;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            Activity_Service activityService = new Activity_Service();

            if (cmbActivityTypes.SelectedIndex <= 0 && txtCreateActivity.Text.Length == 0)
                MessageBox.Show("You need to select an activity type!", "Select activity", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else
            {
                bool error = false;
                string activityType;
                List<string> activityTypes = activityService.GetActivityTypes();

                // if there is nothing selected in the combo box or the index 0 is selected, use the text box as the activity type
                if (cmbActivityTypes.SelectedIndex <= 0)
                {
                     activityType = txtCreateActivity.Text;

                    foreach (string s in activityTypes) //check if that activity type already exists
                        if (s.ToLower() == activityType.ToLower())
                        {
                            MessageBox.Show("Activity type already exists!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            error = true;
                        }
                        
                    if(!error) // if there are no errors, add the new activity type
                    activityService.AddActivityType(activityType);
                }
                   
                else
                    activityType = cmbActivityTypes.SelectedItem.ToString();

         //format the activity date into a string
                string activityDate = calendarActivity.SelectionStart.Year.ToString() +"-"
                    + calendarActivity.SelectionStart.Month.ToString("00") + "-"
                    + calendarActivity.SelectionStart.Day.ToString("00");

                if (cmbHour.SelectedItem.ToString().Length < 2)
                    activityDate += " 0" + cmbHour.SelectedItem.ToString();
                else
                    activityDate += " "+cmbHour.SelectedItem.ToString();

                if (cmbMins.SelectedItem.ToString().Length < 2)
                    activityDate += ":0" + cmbMins.SelectedItem.ToString();
                else
                    activityDate += ":" + cmbMins.SelectedItem.ToString();


                if (!error)
                {
                    Activity _activity = new Activity(activityType, activityDate);
                    _activity.activity_Date = DateTime.ParseExact(activityDate, "yyyy-MM-dd HH:mm", null);
                
                    DateTime _startTime = _activity.activity_Date;                 
                    //create a random duration for the activity, between 30 minutes and 120 minutes(only divisible by 5)
                    Random rnd = new Random();
                    int activityMinutes = rnd.Next(6, 25) * 5;
                    //set the activity end time to the start time + the activity minutes
                    DateTime _endTime = _startTime.AddMinutes(activityMinutes);

                    activityService.AddActivity(_activity);
                    Activity activityTime = activityService.GetBiggestId();
                    Timetable timetable = new Timetable()
                    {
                        activity = activityTime,
                        startTime = _startTime,
                        endTime = _endTime
                    };
                    activityService.AddTimetable(timetable);
                   
                    MessageBox.Show("New activity added!\nRefresh to see changes!", "Succes", MessageBoxButtons.OK);
                }
            }    
        }
        private void cmbActivityTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            //enable or disable the textbox depending on the combo box selection
            if (cmbActivityTypes.SelectedIndex > 0)
                txtCreateActivity.Enabled = false;
            else
                txtCreateActivity.Enabled = true;
        }
    }
}
