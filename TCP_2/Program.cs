using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using TCPserverLib;

namespace TCP_2
{
    class Program
    {
        //Example users: [login: piotr, password: sobieraj],[login: kazio, password:kowalski]

        static void Main(string[] args)
        {
            TCPserverAS tcpServer = new TCPserverAS(IPAddress.Parse("127.0.0.1"), 9900);
            tcpServer.Start();

            UserManager um = new UserManager();

            //um.addUser("elo", "siema","test.txt");
           /// um.readUsersFile("test.txt");
          
        }
    }
}
