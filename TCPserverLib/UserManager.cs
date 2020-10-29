using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Security.Policy;
using System.Runtime.CompilerServices;

namespace TCPserverLib
{
    public class UserManager
    {
        private Dictionary<string,string> usersMap;
        private string loginToCheck;
        private string passToCheck;

        public string LoginToCheck   // property
        {
            get { return loginToCheck; }   // get method
            set { loginToCheck = value; }  // set method
        }

        public string PassToCheck   // property
        {
            get { return passToCheck; }   // get method
            set { passToCheck = value; }  // set method
        }

        public UserManager()
        {
            this.usersMap = new Dictionary<string, string>();
        }

        public void readUsersFile(String fileName)
        {       
            string line;
            int counter = 0;
            ArrayList infoArr = new ArrayList();

            // Read the file and display it line by line.  
            System.IO.StreamReader file =
                new System.IO.StreamReader(fileName);
            while ((line = file.ReadLine()) != null)
            {
                infoArr.Add(line);
                counter++;
            }
            file.Close();
          /*  
            foreach(string l in infoArr)
            {
                Console.WriteLine(l);
            }
        */
            int itLogin = 0;
            int itPassword = 1;
            for(int i = 0; i < infoArr.Count/2; i++)
            {
                string tmp = (String)infoArr[itLogin];
                string tmp2 = (String)infoArr[itPassword];
                if (usersMap.ContainsKey(tmp) == false)
                {
                    usersMap.Add(tmp, tmp2);
                }
                itLogin += 2;
                itPassword += 2;
            }
         /*
            foreach (KeyValuePair<string, string> a in usersMap)
            {
               Console.WriteLine("Key: {0}, Value: {1}",
                    a.Key, a.Value);
            }
         */

        }
        public void addUser(String login, String password, String fileName)
        {
            string[] lines = { login, password };
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(fileName,true))
            {
                foreach (string line in lines)
                {   
                    file.WriteLine(line);
                }
            }
        }

        private bool check()
        {
            string tmp;
            if (this.usersMap.ContainsKey(this.loginToCheck))
            {
                if (this.usersMap.TryGetValue(this.loginToCheck, out tmp))
                {
                    if (tmp.Equals(this.passToCheck))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public string checkText()
        {
            if (check())
            {
                return "Correct! You are logged!";
            }
            else
            {
                return "Incorrect! Wrong login or password!";
            }
        }
    }
}
