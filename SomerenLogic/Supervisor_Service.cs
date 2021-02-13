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
   public class Supervisor_Service
    {
        Supervisor_DAO supervisor_dao = new Supervisor_DAO();

        public List<Activity> GetTeacherActivities(int teacherId)
        {
            return supervisor_dao.GetTeacherActivities(teacherId);
        }
        public Supervisor GetById(int id)
        {
            return supervisor_dao.GetById(id);
        }
        public void AddSupervisor(Supervisor supervisor)
        {
            supervisor_dao.AddSupervisor(supervisor);
        }
        public void RemoveSupervisor(Supervisor supervisor)
        {
            supervisor_dao.RemoveSupervisor(supervisor);

        }
        public void UpdateSupervisor(Supervisor supervisor,int activityId)
        {
            supervisor_dao.UpdateSupervisor(supervisor,activityId);
        }
        public List<Supervisor> GetAllSupervisors()
        {
            List<Supervisor> supervisorList = new List<Supervisor>();
            try
            {
                supervisorList = supervisor_dao.Db_Get_AllSupervisors();
                return supervisorList;
            }
            catch
            {
                string exp = "SomerenApp couldn't get the supervisor list";
                ErrorDAO error = new ErrorDAO(exp);
                return null;
            }

        }
    }
}
