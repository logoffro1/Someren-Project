using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SomerenDAL;
using SomerenModel;

namespace SomerenLogic
{
    public class Timetable_Service
    {
        Timetable_DAO timetable_dao = new Timetable_DAO();
        public List<Timetable> GetTimetableForDates(DateTime startDate, DateTime endDate)
        {
            return timetable_dao.GetTimetableForDates(startDate, endDate);
        }


    }
}
