using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPserverLib
{
    //*****************************************
    //At this moment this class is not used
    //*****************************************

    public class User 
    {
        private String login;
        private String password;

        public  User(String login, String password)
        {
            this.login = login;
            this.password = password;
        }
        public string Login   // property
        {
            get { return login; }   // get method
            set { login = value; }  // set method
        }

        public string Password   // property
        {
            get { return password; }   // get method
            set { password = value; }  // set method
        }
    }
}
