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
   public class RoomDAO : Base
    {
       
        public RoomDAO():base()
        {
           
        }
        // Return all rooms into a list from database.
        public List<Room> Db_Get_All_Rooms()
        {
            string query = "SELECT [room_id],[capacity],[residenttype] FROM [Room];";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            return ReadTables(ExecuteSelectQuery(query, sqlParameters));
        }

        private List<Room> ReadTables(DataTable dataTable)
        {
            List<Room> rooms = new List<Room>();

            foreach (DataRow dr in dataTable.Rows)
            {
               Room Room= new Room()
                {
                    Number = (int)dr["room_id"],
                    Capacity = (int)(dr["capacity"]),
                    Type = (bool)(dr["residenttype"]),
                  
                };
                rooms.Add(Room);
            }
            return rooms;
        }
    }
}
