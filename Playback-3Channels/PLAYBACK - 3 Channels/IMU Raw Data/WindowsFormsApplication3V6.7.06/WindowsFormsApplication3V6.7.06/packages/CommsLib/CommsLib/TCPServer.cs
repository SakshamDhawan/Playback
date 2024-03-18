using System;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Collections.Generic;

namespace CommsLib
{

    public class TCPServer : ServerBase
    {
        private TcpListener tcpListener;
        private Thread listenThread;

        public TcpClient TheClient;

        bool Listening = true;
        byte[] ClientHandshakeData = new byte[] {21, 08,19,92 };
        public bool ClientHandshakeConfirmed = false;
        public bool EchoMode = false;
        /// <summary>
        /// Overriden
        /// </summary>
        /// <param name="DestinationPort"></param>
        /// <param name="DestinationIP"></param>
        public TCPServer(int DestinationPort, string DestinationIP) : base()
        {
            this.tcpListener = new TcpListener(IPAddress.Parse(DestinationIP), DestinationPort);
            IPAddress test = IPAddress.Parse(DestinationIP);
        }

        /// <summary>
        /// Overridden start server function
        /// Instantiates new listen thread and with ListenForClients function, if everything starts ok
        /// return true, otherwise return false
        /// </summary>
        /// <returns></returns>
        public override bool StartServer()
        {
            try
            {
                this.listenThread = new Thread(new ThreadStart(ListenForClients));
                this.listenThread.Start();
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        private void ListenForClients()
        {
            this.tcpListener.Start();

            while (Listening)
            {
                //blocks until a client has connected to the server
                TheClient = this.tcpListener.AcceptTcpClient();

                //create a thread to handle communication 
                //with connected client
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));
                clientThread.Start(TheClient);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Client"></param>
        private void HandleClientComm(object Client)
        {
            TheClient = (Client as TcpClient);
            NetworkStream clientStream = TheClient.GetStream();

            byte[] message = new byte[4096];
            int bytesRead;

            while (true)
            {
                bytesRead = 0;

                try
                {
                    //blocks until a client sends a message
                    bytesRead = clientStream.Read(message, 0, 4096);                    ProcessPacket(message);
                    message = new byte[4096];

                }
                catch(Exception e)
                {
                    //a socket error has occured
                    break;
                }

                if (bytesRead == 0)
                {
                    //the client has disconnected from the server
                    break;
                }

                //message has successfully been received
                ASCIIEncoding encoder = new ASCIIEncoding();
            }
            TheClient.Close();
        }

        public void ProcessPacket(byte[] message)
        {
            switch (message[0])
            {
                case 1:
                    if (message[1] == ClientHandshakeData[0] && message[2] == ClientHandshakeData[1] && message[3] == ClientHandshakeData[2] && message[4] == ClientHandshakeData[3])
                    {
                        ClientHandshakeConfirmed = true;
                        sendServerHandshake(new byte[] { 17,05,19,91});
                    }
                    break;
                case 2:
                    EchoMode = true;
                    break;
                case 3:
                    EchoMode = false;
                    break;
                case 6:
                    SendPacket(message);
                    break;
                default:
                    _MessageReceivedEvent(message);
                    break;
            }
            
            
        }
        public override bool SendPacket(CommandID Mode, byte[] Data)
        {
            if ((TheClient != null) && (TheClient.Connected))
            {
                NetworkStream clientStream = TheClient.GetStream();
                byte MessageLength = (byte)(Data.Length);
                byte[] message = new byte[Data.Length + 1];
                byte[] messageHeader = { (byte)Mode };
                Array.Copy(messageHeader, 0, message, 0, 1);
                Array.Copy(Data, 0, message, 1, Data.Length);
                clientStream.Write(message, 0, message.Length);
                clientStream.Flush();
            }
            return true;
        }

        void sendServerHandshake(byte[] HandShakeCode)
        {
            SendPacket(CommandID.Handshake, HandShakeCode);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Buffer"></param>
        public override void SendPacket(byte[] Buffer)
        {
            if ((TheClient != null) && (TheClient.Connected))
            {
                NetworkStream clientStream = TheClient.GetStream();

                clientStream.Write(Buffer, 0, Buffer.Length);
                clientStream.Flush();
            }
        }
        public override void SendPacket(string message)
        {
            if ((TheClient != null) && (TheClient.Connected))
            {
                NetworkStream clientStream = TheClient.GetStream();

                clientStream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
                clientStream.Flush();
            }
        }
        public override void SendPacket(CommandID command, string Message)
        {
            if ((TheClient != null) && (TheClient.Connected))
            {
                NetworkStream clientStream = TheClient.GetStream();
                byte[] message = new byte[Message.Length+1];
                message[0] = (byte)command;
                Array.Copy(Encoding.ASCII.GetBytes(Message), 0, message, 1, Message.Length);

                clientStream.Write(message, 0, message.Length);
                clientStream.Flush();
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override bool ClientStatus()
        {
            if (TheClient != null)
                return TheClient.Connected;
            else
                return false;
        }

        /// <summary>
        /// 
        /// </summary>l
        /// <returns></returns>
        public override bool ServerStatus()
        {
            return Listening;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            this.listenThread.Abort();

            this.tcpListener.Stop();
            Listening = false;
        }
    }
}