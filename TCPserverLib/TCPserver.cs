using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace TCPserverLib
{
    public abstract class TCPserver
    {

        IPAddress iPAddress;
        int port;
        int buffer_size = 1024;
        bool running;
        TcpListener tcpListener;
        TcpClient tcpClient;
        NetworkStream stream;

        public IPAddress IPAddress { get => iPAddress; set { if (!running) iPAddress = value; else throw new Exception("nie można zmienić adresu IP kiedy serwer jest uruchomiony"); } }

        public int Port
        {
            get => port; set
            {
                int tmp = port;
                if (!running) port = value; else throw new Exception("nie można zmienić portu kiedy serwer jest uruchomiony");
                if (!checkPort())
                {
                    port = tmp;
                    throw new Exception("błędna wartość portu");
                }
            }
        }

        public int Buffer_size
        {
            get => buffer_size; set
            {
                if (value < 0 || value > 1024 * 1024 * 64) throw new Exception("błędny rozmiar pakietu");
                if (!running) buffer_size = value; else throw new Exception("nie można zmienić rozmiaru pakietu kiedy serwer jest uruchomiony");
            }

        }

        protected TcpListener TcpListener { get => tcpListener; set => tcpListener = value; }

        protected TcpClient TcpClient { get => tcpClient; set => tcpClient = value; }

        protected NetworkStream Stream { get => stream; set => stream = value; }

        public TCPserver(IPAddress IP, int port)
        {
            running = false;
            IPAddress = IP;
            Port = port;

            if (!checkPort())
            {
                Port = 8000;
                throw new Exception("błędna wartość portu, ustawiam port na 8000");
            }

        }

        protected bool checkPort()
        {
            if (port < 1024 || port > 49151) return false;
            return true;
        }


        protected void StartListening()
        {
            TcpListener = new TcpListener(IPAddress, Port);
            TcpListener.Start();
        }
   
        protected abstract void AcceptClient();   

        protected abstract void BeginDataTransmission(NetworkStream stream);

        public abstract void Start();
    }

}