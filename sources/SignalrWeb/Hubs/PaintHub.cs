using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using SignalrWeb.Models;

namespace SignalrWeb.Hubs
{
    public class PaintHub : Hub
    {
        private static Dictionary<string, List<string>> groups = new Dictionary<string, List<string>>();
 
        public static object GroupLock = new object();

        public Room AddUserInRoom(string connectionId, string roomName)
        {
            Room room;
            if (!groups.ContainsKey(roomName))
            {
                lock (GroupLock)
                {
                    if (!groups.ContainsKey(roomName))
                    {
                        List<string> users = new List<string>();
                        users.Add(connectionId);
                        groups.Add(roomName, users);
                        //first user
                        room = new Room();
                        room.Name = roomName;
                        room.Count = 1;
                    }
                    else
                    {
                        List<string> users = groups[roomName];
                        users.Add(connectionId);
                        room = new Room();
                        room.Name = roomName;
                        room.Count = users.Count;

                    }
                }
            }
            else
            {
                lock (GroupLock)
                {
                    //remove duplicated code
                    List<string> users = groups[roomName];
                    users.Add(connectionId);
                    room = new Room();
                    room.Name = roomName;
                    room.Count = users.Count;
                }
            }
            return room;
        }

        public Room DisconnectUser(string connectionId)
        {
            Room room = null;
            foreach (KeyValuePair<string, List<string>> group in groups)
            {
                List<string> users = group.Value;
                if (users.Contains(connectionId))
                {
                    lock (GroupLock)
                    {
                        if (users.Contains(connectionId))
                        {
                            users.Remove(connectionId);
                            room = new Room();
                            room.Name = group.Key;
                            room.Count = group.Value.Count;

                        }
                    }
                }
            }
            return room;
        }

        public void RoomCountChanged(Room room)
        {
             if (room != null)
             {
                 Clients.Group(room.Name).RoomCountChanged(room.Count);
             }
        }

        public void JoinRoom(string roomName)
        {
            Groups.Add(Context.ConnectionId, roomName);
            Room room = AddUserInRoom(Context.ConnectionId, roomName);
            RoomCountChanged(room);
        }

        public void GetRoomCount(string roomName)
        {
            if (groups.ContainsKey(roomName))
            {
                List<string> users = groups[roomName];
                if ((users != null) && (users.Count > 0))
                {
                    Clients.Caller.RoomCountChanged(users.Count);
                }
                
            }
        }

        public void SendLine(Line line)
        {
            if (line != null)
            {
                if (line.WidthBrush > 50)
                {
                    line.WidthBrush = 50;
                }
                else if (line.WidthBrush < 1)
                {
                    line.WidthBrush = 1;
                }
                Clients.Group(line.GroupName).AddNewLine(line);
            }
        }

        public override Task OnDisconnected()
        {
            Room room = DisconnectUser(Context.ConnectionId);
            RoomCountChanged(room);
            return base.OnDisconnected();
        }

    }
}