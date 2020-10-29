using System;

using System.Collections.Generic;

using System.IO;

using System.Linq;

using System.Net;

using System.Net.Sockets;

using System.Text;

namespace TCPserverLib
{
    public class TCPserverAS : TCPserver
    {
        private int bufferLen = 0;
        public delegate void TransmissionDataDelegate(NetworkStream stream);

        public TCPserverAS(IPAddress IP, int port) : base(IP, port)
        {

        }

        protected override void AcceptClient()
        {

            while (true)
            { 
                TcpClient = TcpListener.AcceptTcpClient();
                String connectionParamsText = "Connected!" + "\r\n" + "Connection parameters - ip: " + IPAddress.ToString() + " port: " + Port + "\r";
                
                if (TcpClient != null)
                {     
                    Console.WriteLine(connectionParamsText);
                }

                Stream = TcpClient.GetStream();           
                TransmissionDataDelegate transmissionDelegate = new TransmissionDataDelegate(BeginDataTransmission);
                transmissionDelegate.BeginInvoke(Stream, TransmissionCallback, TcpClient);
            }
          
        }

        private void TransmissionCallback(IAsyncResult ar) { }

        private void send(NetworkStream stream,String text)
        {
            stream.Write(castStringToByte(text), 0, castStringToByte(text).Length);
        }

        private int read(NetworkStream stream, byte[] buffer)
        {
            int i = i = stream.Read(buffer, 0, buffer.Length);
            int message_size = 0;
            string tmp;

            tmp = Encoding.ASCII.GetString(buffer,0,i);
            char[] trim = { '\r', '\n' };
            tmp.Trim(trim);
            message_size = Encoding.ASCII.GetBytes(tmp).Length;

            return message_size;
        }

        private void checkLen(byte[] buffer)
        {
            this.bufferLen = Stream.Read(buffer, 0, buffer.Length);
            if(bufferLen > 2)
            {
                if ((Encoding.ASCII.GetString(buffer, 0, this.bufferLen) == "\r\n") || (Encoding.ASCII.GetString(buffer, 0, this.bufferLen) == "\n\r"))
                {
                    Stream.Read(buffer, 0, buffer.Length);
                }
            }
           
        }

        protected override void BeginDataTransmission(NetworkStream stream)
        {
            bool start = true;
            bool login = true;
            bool password = false;

            byte[] buffer = new byte[Buffer_size];
            String connectionParamsText = "Connected!" + "\r\n" + "Connection parameters - ip: " + IPAddress.ToString() + " port: " + Port + "\r\n\n";
            UserManager us = new UserManager();
            us.readUsersFile("test.txt");

            while (true)
            {           
                try
                {
                    if (start == true)
                    {
                        send(stream, connectionParamsText);
                        start = false;
                    }
                    else if((login == true) && (password == false) && (start == false))
                    {
                        send(stream, "Type login: ");
                        int message_size = read(stream, buffer);
                        string readed = Encoding.ASCII.GetString(buffer,0,message_size);
                        checkLen(buffer);
                       
                        us.LoginToCheck = readed;                       
                            
                        login = false;
                        password = true;
                    }
                    else if ((login == false) && (password == true) && (start == false))
                    {

                        send(stream, "Type password: ");
                        int message_size = read(stream, buffer);
                        string readed = Encoding.ASCII.GetString(buffer, 0, message_size);

                        checkLen(buffer);

                        us.PassToCheck = readed;                     

                        send(Stream, us.checkText());
                        checkLen(buffer);

                        send(stream, "\r\n");
                        login = true;
                        password = false;
                    }
                   
                }
                catch (IOException e)
                {
                    break;
                }
            }
        }

        private Byte[] castStringToByte(String text)
        {
            return System.Text.Encoding.ASCII.GetBytes(text);
        }
        private String castByteToString(Byte[] buffer, Int32 offset, int i)
        {
            return System.Text.Encoding.ASCII.GetString(buffer, 0, i);
        }

        public override void Start()
        {
            Console.Write("Waiting for a connection... ");
            StartListening();
            AcceptClient();
        }
        static void Main(string[] args) { }
    }
   
}
