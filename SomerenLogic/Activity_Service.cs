using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SomerenDAL;
using SomerenModel;
namespace SomerenLogic
{
   public class Activity_Service
    {
        Activity_DAO activity_dao = new Activity_DAO();
        public List<Activity> GetAllActivities()
        {
            return activity_dao.Db_Get_AllActivities();

        }
        public void AddActivity(Activity activity)
        {
            activity_dao.AddActivity(activity);
        }
        public void AddTimetable(Timetable timetable)
        {
            activity_dao.AddTimetable(timetable);
        }
        public Activity GetBiggestId()
        {
            return activity_dao.GetBiggestId();
        }
        public void AddActivityType(string activityType)
        {
            activity_dao.AddActivityType(activityType);
        }
        public void UpdateActivity(Activity activity, string activity_date)
        {
            activity_dao.UpdateActivity(activity, activity_date);
        }
        public List<string> GetActivityTypes()
        {
            return activity_dao.GetActivityTypes();
        }
        public void RemoveActivity(Activity activity)
        {
            activity_dao.RemoveActivity(activity);
        }
        public Activity GetById(int id)
        {

            return activity_dao.GetById(id);
        }
    }
}
