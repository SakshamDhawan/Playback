using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace SerialHandler
{
    //Arguments class for message received event
    public class Message_EventArgs : EventArgs
    {
        public readonly List<int> RawMessage;

        public Message_EventArgs(List<int> RawMessage)
        {
            this.RawMessage = RawMessage;
        }
    }

    public class Handler
    {
        /// <summary>
        /// Event virtual function for firing
        /// </summary>
        protected virtual void _MessageReceivedEvent(List<int> MessageData)
        {
            if (OnMessageReceived != null) OnMessageReceived(new Message_EventArgs(MessageData));
        }

        public delegate void MessageReceivedEvent(Message_EventArgs e);
        public event MessageReceivedEvent OnMessageReceived;
        public string COMPort = "";
        public bool open = false;
        SerialPort _port;
        List<int> data;
        List<int> incomingData;
        int expectedBytes = 0;
        public Handler()
        {
            data = new List<int>();
            incomingData = new List<int>();
        }

        public bool Open(string portName, int baudRate)
        {
            bool sucess = true;
            if (portName != null)
            {
                _port = new SerialPort(portName, baudRate);
            }
            try
            {
                _port.Open();
            }

            catch (UnauthorizedAccessException)
            {
                sucess = false;
            }
            catch (System.IO.IOException)
            {
                sucess = false;
            }
            catch (NullReferenceException)
            {
                sucess = false;
            }
            _port.DataReceived += new SerialDataReceivedEventHandler(_port_DataReceived);
            open = true;
            return sucess;

        }
        public bool Open(int baudRate)
        {
            bool sucess = true;
            if (COMPort != "")
            {
                _port = new SerialPort(COMPort, baudRate);
            }
            try
            {
                _port.Open();
            }

            catch (UnauthorizedAccessException)
            {
                sucess = false;
                return sucess;
            }
            catch (System.IO.IOException)
            {
                sucess = false;
                return sucess;
            }
            catch (NullReferenceException)
            {
                sucess = false;
                return sucess;
            }
            byte[] bytesToSend = new byte[1] { (byte)11 };
            _port.Write(bytesToSend, 0,1);
            _port.DataReceived += new SerialDataReceivedEventHandler(_port_DataReceived);
            open = true;
            return sucess;

        }

        public bool send_data(string data)
        {

            try
            {
                _port.WriteLine(data);
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            return true;
        }
        public bool send_clear_data(byte data)
        {
            try
            {
                byte[] bytesToSend = new byte[1] { (byte)data };
                _port.Write(bytesToSend,0,1);
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            return true;
        }
        public bool send_char(string data)
        {
            byte[] bytesToSend = new byte[6] { (byte)'\a', (byte)data[0], (byte)data[1], (byte)data[2], (byte)data[3], (byte)'\v' };
            string sent = Encoding.UTF8.GetString(bytesToSend);
            try
            {
                _port.Write(sent);
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            return true;
        }


        public bool Close()
        {
            bool sucess = true;
            try
            {
                if (_port != null)
                {
                    _port.Close();
                }
            }

            catch (UnauthorizedAccessException)
            {
                sucess = false;
            }

            open = false;
            return sucess;

        }

        void _port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try {
                while (_port.BytesToRead != 0)
                {



                    incomingData.Add(_port.ReadByte());

                }

                if (incomingData.Count > 5)
                {
                    buildPacket(incomingData);
                }
                //_MessageReceivedEvent(incomingData);
                //incomingData.RemoveRange(0, count);
            }
            catch (Exception) { }
            }
        

        int count = 0;

        void buildPacket(List<int> incomingData)
        {
            bool flag = false;
            data.Clear();
            count = 0;
            while ((incomingData.Count > 5) && (count < incomingData.Count - 3))
            {
                if ((incomingData[count] == 0xDD) && (incomingData[count + 1] == 0xAA) && (incomingData[count + 2] == 0x55))
                {
                    expectedBytes = incomingData[count + 3];
                    count = count + 4;
                    flag = true;
                    incomingData.RemoveRange(0, count);
                    count = 0;
                    data.Add(expectedBytes);
                }
                else
                {
                    count++;
                }
                while ((incomingData.Count >= (expectedBytes)) && (flag == true) && (count < incomingData.Count))
                {
                    try
                    {
                        data.Add(incomingData[count++]);
                    }
                    catch
                    {
                        incomingData.Clear();
                        data.Clear();
                        flag = false;
                    }
                        if ((data.Count == expectedBytes + 1) && (data[0] == data[expectedBytes]))
                        {
                        if ((data[expectedBytes - 1] != 85)) {
                            if (data[1] == 6)
                            {
                                int brea = 0;
                            }
                            data[0] = data[0] - 1;
                            data.RemoveAt(expectedBytes);
                            _MessageReceivedEvent(data);
                        }

                            flag = false;
                    }
                    if (flag == false)
                    {
                        incomingData.RemoveRange(0, count);
                        data.Clear();
                        count = 0;
                    }
                }
            }
        }
    }
}
