using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Ports;
using System.IO;
using System.Security.Permissions;
using Microsoft.Win32;
using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.Factory;
using InTheHand.Net.Bluetooth.Widcomm;
using InTheHand.Net.Sockets;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Diagnostics;

namespace Communications
{
    public class Message_EventArgs : EventArgs
    {
        public readonly List<int> RawMessage;

        public Message_EventArgs(List<int> RawMessage)
        {
            this.RawMessage = RawMessage;
        }
    }

    public class CommunicationsClass : INotifyPropertyChanged
    {
        Stream peerStream;
        Thread oThread;
        public List<int> data;
        BluetoothClient client;
        byte[] sendBuf;
        List<byte> datalist = new List<Byte>();
        List<byte> holdlist = new List<Byte>();
        List<int> message = new List<int>();
        public CommunicationsClass()
        {
            client = new BluetoothClient();
            byte[] sendBuf = new byte[30];
            List<byte> datalist = new List<Byte>();
            List<byte> holdlist = new List<Byte>();
            List<int> message = new List<int>();

        }
        public Stream getStream()
        {
            oThread.Abort();
            while (client.Available != 0)
            {
                int bytes = peerStream.ReadByte();

            }
                oThread = new Thread(() => waitfordata(true));
            oThread.Start();
            return peerStream;
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        public delegate void EventHandler(Message_EventArgs e);
        public event EventHandler DataIn;

        protected void OnPropertyChanged(Message_EventArgs e)
        {
            EventHandler handler = DataIn;
            if (handler != null)
                handler(new Message_EventArgs(message));
        }

        protected void OnPropertyChanged(List <int>data)
        {
            OnPropertyChanged(new Message_EventArgs(data));
        }

        public List<int> Data
        {
            get { return data; }
            set
            {
                if (value != data)
                {
                    data = value;
                    OnPropertyChanged(data);
                    data = null;
                }
            }
        }

        public int waitfordata(bool run)
        {
            while (true)
            {
                while (client.Available != 0)
                {
                    int bytes = peerStream.ReadByte();
                    if (bytes >= 0)
                    {

                        datalist.Add((byte)bytes);

                        if (datalist.Count > 5)
                        {
                            buildPacket(datalist);
                        }


                    }
                }
            }
            return 0;

        }
        public int waitfordata(bool run, bool test)
        {
            
            while (true)
            {
                while (client.Available != 0)
                {
                    int bytes = peerStream.ReadByte();
                    if (bytes >= 0)
                    {
                        
                        char[] a = bytes.ToString("X2").ToCharArray();
                        message.Add((byte)a[0]);
                        message.Add((byte)a[1]);
                        OnPropertyChanged(message);
                        message.RemoveAt(0);
                        message.RemoveAt(0);
                    }
                }
            }
            return 0;

        }

        public void senddata(byte newdata)
        {
            try
            {
                peerStream.WriteByte(newdata);
            }
            catch (IOException e)
            {
                String warning = "IOException - Normally means the Device you are connected too has been unplugged";
                Debugger.Break();
                warning = warning+" - Broken";
            }
        }
        public void senddata(string newdata)
         {

            peerStream.WriteByte(0x07);
            foreach (char c in newdata)
            {
                peerStream.WriteByte((byte)c);
            }



            peerStream.WriteByte(0x0B);

        }

        public bool QuickConnect(BluetoothDeviceInfo d)
        {
            oThread = new Thread(() => waitfordata(true));
            var addr = d.DeviceAddress;
            Guid serviceClass = BluetoothService.SerialPort;
            var ep = new BluetoothEndPoint(addr, serviceClass);
            try { client.Connect(ep); }
            catch (Exception)
            {
                return false;
            }
            peerStream = client.GetStream();

            oThread.Start();
            return true;
        }
        public bool QuickConnect(BluetoothDeviceInfo d, bool IR)
        {
            oThread = new Thread(() => waitfordata(true, IR));
            var addr = d.DeviceAddress;
            Guid serviceClass = BluetoothService.SerialPort;
            var ep = new BluetoothEndPoint(addr, serviceClass);
            try { client.Connect(ep); }
            catch (Exception)
            {
                return false;
            }
            peerStream = client.GetStream();

            oThread.Start();
            return true;
        }

        public bool quickconnectthroughbluetoothaddress(BluetoothAddress d)
        {
            oThread = new Thread(() => waitfordata(true));
            var addr = d;
            Guid serviceClass = BluetoothService.SerialPort;
            var ep = new BluetoothEndPoint(addr, serviceClass);

            try { client.Connect(ep); }
            catch (Exception)
            {
                return false;
            }
            peerStream = client.GetStream();
            oThread.Start();

            return true;


        }

        public bool connectthroughbluetooth(BluetoothDeviceInfo d)
        {
            oThread = new Thread(() => waitfordata(true));
            var addr = d.DeviceAddress;
            Guid serviceClass = BluetoothService.SerialPort;
            var ep = new BluetoothEndPoint(addr, serviceClass);
            try { client.Connect(ep); }
            catch (Exception)
            {
                return false;
            }
            peerStream = client.GetStream();

            byte[] buf = new byte[50];
            int readLen = peerStream.Read(buf, 0, 15);
            var bths = "";
            bths = Encoding.ASCII.GetString(buf);
            if (bths.Contains("Connection Open"))
            {
                peerStream.WriteByte(0x0B);
                oThread.Start();
                return true;
            }
            else
            {
                return false;
            }


        }
        public bool quickconnectthroughbluetoothaddress(BluetoothAddress d, bool IR)
        {
            oThread = new Thread(() => waitfordata(true, IR));
            var addr = d;
            Guid serviceClass = BluetoothService.SerialPort;
            var ep = new BluetoothEndPoint(addr, serviceClass);

            try { client.Connect(ep); }
            catch (Exception)
            {
                return false;
            }
            peerStream = client.GetStream();
            oThread.Start();

            return true;


        }

        public bool connectthroughbluetooth(BluetoothDeviceInfo d, bool IR)
        {
            oThread = new Thread(() => waitfordata(true,IR));
            var addr = d.DeviceAddress;
            Guid serviceClass = BluetoothService.SerialPort;
            var ep = new BluetoothEndPoint(addr, serviceClass);
            try { client.Connect(ep); }
            catch (Exception)
            {
                return false;
            }
            peerStream = client.GetStream();

            byte[] buf = new byte[50];
            int readLen = peerStream.Read(buf, 0, 15);
            var bths = "";
            bths = Encoding.ASCII.GetString(buf);
            if (bths.Contains("Connection Open"))
            {
                peerStream.WriteByte(0x0B);
                oThread.Start();
                return true;
            }
            else
            {
                return false;
            }


        }

        public void Dispose()
        {
                        oThread.Abort();
        }
        public void Class1logger(string input)
        {
            System.IO.StreamWriter logfile;
            if (!File.Exists(System.IO.Directory.GetCurrentDirectory() + "..\\..\\..\\DataFiles\\BTLOG.txt"))
            {
                System.IO.File.WriteAllText(System.IO.Directory.GetCurrentDirectory() + "..\\..\\..\\DataFiles\\BTLOG.txt", string.Empty);
            }
            else
            {

                logfile = new System.IO.StreamWriter(System.IO.Directory.GetCurrentDirectory() + "..\\..\\..\\DataFiles\\BTLOG.txt", true);
                logfile.WriteLine(DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + input);
                logfile.Close();
            }
        }
        public bool connectthroughbluetoothaddress(BluetoothAddress d)
        {
            oThread = new Thread(() => waitfordata(true));
            var addr = d;
            Guid serviceClass = BluetoothService.SerialPort;
            var ep = new BluetoothEndPoint(addr, serviceClass);
            Class1logger("ep = " + ep.ToString());
            Class1logger("serviceClass = " + serviceClass.ToString());
            client.Client.ReceiveBufferSize = 16384;
            client.Client.SendBufferSize = 255;
            try { client.Connect(ep); }
            catch (Exception e)
            {
                if(e.HResult == -2147467259)
                {
                    try { client.Connect(ep); }
                    catch (Exception) {
                        return false;
                    }
                }
            }

            peerStream = client.GetStream();
            byte[] buf = new byte[50];
            int readLen = peerStream.Read(buf, 0, 15);
            var bths = "";
            bths = Encoding.ASCII.GetString(buf);
            if (bths.Contains("Connection Open"))
            {
                oThread.Start();
                return true;
            }
            else
            {
                return false;
            }


        }
        public bool connectthroughbluetoothaddress(BluetoothAddress d,bool IR)
        {
            oThread = new Thread(() => waitfordata(true, IR));
            var addr = d;
            Guid serviceClass = BluetoothService.SerialPort;
            var ep = new BluetoothEndPoint(addr, serviceClass);

            try { client.Connect(ep); }
            catch (Exception)
            {
                return false;
            }
            peerStream = client.GetStream();

            byte[] buf = new byte[50];
            int readLen = peerStream.Read(buf, 0, 15);
            var bths = "";
            bths = Encoding.ASCII.GetString(buf);
            if (bths.Contains("Connection Open"))
            {
                oThread.Start();
                return true;
            }
            else
            {
                return false;
            }


        }
        public void bluetoothClose()
        {
            oThread.Abort();
            client.Dispose();
        }
        int count = 0;
        int expectedBytes = 0;
        void buildPacket(List<byte> datalist)
        {
            bool flag = false;
            bool notEnoughData = false;
            message.Clear();
            count = 0;
            while ((datalist.Count > 5) && (count < datalist.Count - 3) && notEnoughData == false)
            {
                if ((datalist[count] == 0xDD) && (datalist[count + 1] == 0xAA) && (datalist[count + 2] == 0x55))
                {
                    expectedBytes = datalist[count + 3];
                    if (datalist.Count <= (expectedBytes + count + 3))
                    {
                        notEnoughData = true;
                    }
                    else
                    {
                        expectedBytes = datalist[count + 3];
                        count = count + 4;
                        flag = true;
                        datalist.RemoveRange(0, count);
                        count = 0;
                        message.Add(expectedBytes);
                    }
                }
                else
                {
                    count++;
                }
                

                    while ((datalist.Count >= (expectedBytes)) && (flag == true) && (count < datalist.Count))
                {
                    try
                    {
                        message.Add(datalist[count++]);
                        if (message.Count() >= 4)
                        {
                            if ((message[count-2] == 0xDD) && (message[count - 1] == 0xAA) && (message[count] == 0x55))
                            {
                                datalist.RemoveRange(0, count - 3);
                                message.RemoveRange(0, count - 3);
                                flag = false;
                                count = 0;
                            }
                        }
                    }
                    catch
                    {
                        datalist.Clear();
                        message.Clear();
                        flag = false;
                    }
                    if (expectedBytes >= 3)
                    {
                        if ((message.Count == expectedBytes + 1) && (message[0] == message[expectedBytes]) && !((message[expectedBytes - 1] == 0x55) && (message[expectedBytes - 2] == 0xAA) && (message[expectedBytes - 3] == 0xDD)))
                        {

                            message[0] = message[0] - 1;
                            message.RemoveAt(expectedBytes);

                            OnPropertyChanged(message);
                            flag = false;
                        }
                    }
                    if (flag == false)
                    {
                        datalist.RemoveRange(0, count);
                        message.Clear();
                        count = 0;
                    }
                }
            }
        }





    }
}
