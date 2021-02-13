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
    public class Room_Service
    {
        RoomDAO roomservice = new RoomDAO();

        public List<Room> GetRooms()
        {
            List<Room> Rooms = new List<Room>();
            try
            {
                Rooms = roomservice.Db_Get_All_Rooms();
                return Rooms;
            }
            catch
            {
                string exp = "SomerenApp couldn't open the room list";
                ErrorDAO error = new ErrorDAO(exp);
                return null;

            }

        }
    }
}
