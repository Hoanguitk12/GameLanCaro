using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCaroLan
{
    [Serializable]
    public class SocketData
    {
        private int command;

        public int Command { get => command; set => command = value; }
        public Point Point { get => point; set => point = value; }
        public string Message { get => message; set => message = value; }

        private Point point;
        public SocketData(int command,string message,Point point)
        {
            this.Command = command;
            this.Point = point;
            this.Message = message;
           
        }
        private string message;
       
        public enum SocketCommand
        {
            SEND_POINT,
            NOTIFY,
            NEW_GAME,
            UNDO,
            END_GAME,
            TIME_OUT,
            QUIT 

        }
    }
}
