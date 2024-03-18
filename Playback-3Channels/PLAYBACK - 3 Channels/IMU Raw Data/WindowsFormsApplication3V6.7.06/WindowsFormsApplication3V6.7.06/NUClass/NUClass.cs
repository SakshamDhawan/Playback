using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows;
using System.IO.Ports;
using System.IO.Pipes;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Permissions;
using Microsoft.Win32;
using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.Widcomm;
using InTheHand.Net.Sockets;
using Communications;
using SerialHandler;
using FTD2XX_NET;
using System.Management;
using System.Diagnostics;
using CommsLib;
using System.Text.RegularExpressions;
using MMGProcessor;
using OpenTK;
using Utilities;
using System.Collections;
using System.Timers;

namespace NUClass
{
    public class Flag : EventArgs
    {
        public readonly int address;
        public readonly string data;
        public readonly byte[] TCPdata;
        public readonly int Class;
        public readonly string name;
        public readonly int result;
        public readonly bool triggered;
        public readonly string StringData;
        public readonly byte[] unityData;
        public readonly bool unity;
        public readonly float[] heliData;
        public readonly Quaternion quat;
        public readonly NU NU;

       

        public Flag(int _address)
        {
            this.Class = 1;
            this.address = _address;
        }   
        public Flag(int _address, string _data, int mode)
        {
            switch (mode)
            {
                case 0:
                    this.Class = 2;
                    this.data = _data;
                    this.address = _address;
                    break;
                case 1:
                    this.Class = 13;
                    this.data = _data;
                    this.address = _address;
                    break;
            }
        }
        public Flag(int _address, byte[] _TCPdata)
        {
            this.Class = 3;
            this.TCPdata = _TCPdata;
            this.address = _address;
        }
        public Flag(string _name, int _result)
        {
            this.Class = 4;
            this.name = _name;
            this.result = _result;
        }
        public Flag(string _StringData, bool diff)
        {
            this.Class = 5;
            this.StringData = _StringData;
        }
        public Flag(bool _triggered,int a)
        {
            switch (a) {
                case 0:
                    this.Class = 6;
                    this.triggered = _triggered;
                    break;
                case 1:
                    Class = 12;
                    break;
            }
        }
        public Flag(bool _unity, byte[] _data, int _address)
        {
            this.Class = 7;
            this.address = _address;
            this.unityData = _data;
        }
        public Flag(float[] _data, int _address, bool helicopter)
        {
            this.Class = 8;
            this.address = _address;
            this.heliData = _data;
        }
        public Flag(string gesture, int _address, bool nine, Quaternion quat)
        {
            this.Class = 9;
            this.address = _address;
            this.data = gesture;
            this.quat = quat;
        }
        public Flag(int gesture, int _address, bool ten, Quaternion quat)
        {
            this.Class = 10;
            this.address = _address;
            this.result = gesture;
            this.quat = quat;
        }
        public Flag(NU _NU)
        {
            this.Class = 11;
            NU = _NU;
        }
    } // Different Formats for sending data to form1

    public class DataStore
    {
        Quaternion Rotation;
        Vector3 Acceleration;
        Vector3 Magnetometer;
        public DataStore(Quaternion _Rotation, Vector3 _Acceleration, Vector3 _Magnetometer)
        {
            Rotation = _Rotation;
            Acceleration = _Acceleration;
            Magnetometer = _Magnetometer;
        }
        public Vector3 GetAccel()
        {
            return Acceleration;
        }
        public Vector3 GetMag()
        {
            return Magnetometer;
        }
        public Quaternion GetRot()
        {
            return Rotation;
        }
    }
    public class NU
    {
        public int AlgorithmMode = 0;
        List<DataStore> dataStore = new List<DataStore>();
        int id;
        int PERIOD;
        int FREQ;
        int ADC;
        int PS;
        int gscale;
        int grate;
        int ascale;
        int arate;
        int mscale;
        int mrate;
        List<float> DataHold = new List<float>() { };
        int freq;
        string IMUType;
        public bool fullMMG = false;
        bool MagTest = false; bool comptest = false;
        bool _docked = true;
        public bool Kat = false;
        int[] divisor = new int[] { 1, 2, 5, 10, 20, 50, 100, 200, 500, 1000 };
        public string name;
        int ignore = 500;
        public int address;
        Form3 Form3;
        Threshold Threshold;
        int gestureCount = 0;
        int thres = 20;
        int firmwareVersion = 0;
        public static Form1 Form1;
        public Form2 Form2;
        Form5 Form5;
        Form6 Form6;
        public Form8 form8;
        GraphForm Form4;
        public int numpad = 0;
        public bool saveData = false;
        public Calibration cali;
        public Quaternion quat = new Quaternion(0, 0, 0, 1);
        public Quaternion quatNew = new Quaternion(0, 0, 0, 1);
        public Quaternion quatSix = new Quaternion(0, 0, 0, 1);
        public Quaternion quatNine = new Quaternion(0, 0, 0, 1);
        public Quaternion zeroQuat = new Quaternion(0, 0, 0, 1);
        public Quaternion MAquatNine = new Quaternion(0, 0, 0, 1);
        public Quaternion SebXquatNine = new Quaternion(0, 0, 0, 1);
        public Quaternion SWquatNine = new Quaternion(0, 0, 0, 1);
        public float samplePeriod = 0.001f;
        float holdSamplePeriod;
        CommunicationsClass Comms;
        Handler PortHandler;
        string COMportName;
        public bool bluetoothConnection = false;
        public bool USBConnection = false;
        int slow = 0;
        int slowcopter = 0;
        byte[] unitydata = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 };
        string CurrentDirectory;
        public bool Helicopter = false;
        public byte openclose = 0;
        public MMG MMGRec;
        float packetCorrection = 1;
        int[,] confusion;
        Quaternion Previous = new Quaternion(0, 0, 0, 1);
        public List<int>[] MMGChannel = { new List<int> { }, new List<int> { }, new List<int> { }, new List<int> { }, new List<int> { }, new List<int> { }, new List<int> { }, new List<int> { } };
        public List<double>[] HandControlProcessorMemory = { new List<double> { }, new List<double> { }, new List<double> { }, new List<double> { }, new List<double> { }, new List<double> { }, new List<double> { }, new List<double> { } };
        public int[] Debouncer = { 0, 0, 0, 0, 0, 0, 0, 0 };
        ManualResetEvent[] doneEvents = new ManualResetEvent[7];
        WorkerClass[] WorkerArray = new WorkerClass[7];
        int yawvar = 0, pitchvar = 0, rollvar = 0;
        DateTime trigger = DateTime.Now;
        SW_Maths sw_Maths = new SW_Maths();
        AHRS ahrs = new AHRS();
        int previousPacketNumber = -1;
        NU SecondAccelHandler;
        List<string> Memory = new List<string>() { };
        public NU Parent;
        public bool baxter = false;
        public int gesture = -1;
        public bool comp = false;
        public bool VisOff = false;
        public Vector3 FakeMag = new Vector3(0, 0, 0);
        public float AX = 0;
        public float AY = 0;
        public float AZ = 0;
        public float MX = 0;
        public float MY = 0;
        public float MZ = 0;
        public bool Southampton_Testing = false;
        public Vector3 EndPosition = new Vector3(0, 0, 0);

        /*public NetworkStream TalkerStream;
        public uint ToTalkerDataNum;
        int TalkerStartCheck;*/

        FilterData MMG_filter_1 = new FilterData();
        FilterData MMG_filter_2 = new FilterData();
        FilterData MMG_filter_3 = new FilterData();
        FilterData MMG_filter_4 = new FilterData();
        FilterData MMG_filter_5 = new FilterData();
        FilterData MMG_filter_6 = new FilterData();
        FilterData MMG_filter_7 = new FilterData();
        FilterData MMG_filter_8 = new FilterData();

        private static System.Timers.Timer aTimer;
        public enum Algorithms
        {
            MadgwickAlgorithm,
            Admiraalgorithm,
            WilsonAlgorithm,
            Madgwick6DofAlgorithm,
            SebX_Update9DoF
        }
        //int AlgorithmSelection = (int)Algorithms.WilsonAlgorithm;
        int AlgorithmSelection = (int)Algorithms.WilsonAlgorithm;
        protected virtual void DataReady()
        {
            if (setflag != null) setflag(new Flag(address));
        }
        protected virtual void DataReady(string data, int mode)
        {
            if (setflag != null) setflag(new Flag(address, data,mode));
        }
        protected virtual void DataReady(byte[] data)
        {
            if (setflag != null) setflag(new Flag(address, data));
        }
        protected virtual void WriteDataToBox(string _StringData)
        {
            if (setflag != null) setflag(new Flag(_StringData, true));
        }
        protected virtual void Trigger(int a)
        {
            if (setflag != null) setflag(new Flag(true,a));
        }
        protected virtual void DataReadyUnity(bool unity, byte[] _data)
        {
            if (setflag != null) setflag(new Flag(unity, _data, address));
        }
        protected virtual void helicopterData(float[] _data, bool helicopterData)
        {
            if (setflag != null) setflag(new Flag(_data, address, true));
        }
        protected virtual void gestureData(string gesture, Quaternion quat)
        {
            if (setflag != null) setflag(new Flag(gesture, address, true, quat));
        }
        protected virtual void IRControl(int gesture)
        {
            if (setflag != null) setflag(new Flag(gesture, address, true, quat));
        }
        protected virtual void AddNU(NU _NU)
        {
            if (setflag != null) setflag(new Flag(_NU));
        }
        public delegate void DataReadyEvent(Flag e);
        public event DataReadyEvent setflag;

        public static NU NUClass;
        public void initialise(string _name, string _CurrentDirectory, string _IMUType)
        {
            string typeid = "";

        }
        public void IMUinitialise(string _name, string _CurrentDirectory)
        {
            CurrentDirectory = _CurrentDirectory;
            name = _name;
            IMUType = "MMARK";
            string imuBTName = "IMU_BT_" + name;
            string imuUSBName = "IMU_USB_" + name;
            string imuMemory = CurrentDirectory + "\\IMU_Memory\\IMU_BT_" + name + ".txt";
            PortHandler = new Handler();
            Form2 = new Form2();
            NUClass = this;
            bool bluetooth = true;

            try
            {
                Comms = new CommunicationsClass();                      //Attempt to create bluetooth object (Fails if computer does not support bluetooth)
            }
            catch (Exception)
            {
                WriteDataToBox("The computer does not appear to have bluetooth capabilities\n");
                bluetooth = false;
            }
            bluetoothConnection = BluetoothConnect(imuMemory, bluetooth, imuBTName);
            USBConnection = USB(imuUSBName);
            if (bluetoothConnection && USBConnection)
            {
                Form3 = new Form3(name, this);
                Form3.Show();
            }
        }
        public void FakeInit(string _name, string _CurrentDirectory)
        {
            CurrentDirectory = _CurrentDirectory;
            name = _name;
        }
        public void initialise(string _name, string _CurrentDirectory)
        {
            CurrentDirectory = _CurrentDirectory;
            name = _name;
            IMUType = "NU";
            string imuBTName = "NU_BT_" + name;
            string imuUSBName = "NU_USB_" + name;
            string imuMemory = CurrentDirectory + "\\IMU_Memory\\NU_BT_" + name + ".txt";
            PortHandler = new Handler();
            //Form2 = new Form2();
            NUClass = this;
            bool bluetooth = true;
            try
            {
                Comms = new CommunicationsClass();                      //Attempt to create bluetooth object (Fails if computer does not support bluetooth)
            }
            catch (Exception)
            {
                WriteDataToBox("The computer does not appear to have bluetooth capabilities\n");
                bluetooth = false;
            }
            bluetoothConnection = BluetoothConnect(imuMemory, bluetooth, imuBTName);
            //USBConnection = USB(imuUSBName);
            if (bluetoothConnection && USBConnection)
            {
                Form3 = new Form3(name, this);
                Form3.Show();
            }

        }
        public void ChannelSelect(int channel)
        {
            if (channel == 0)
            {
                USBConnection = false;
            }
            else if (channel == 1)
            {
                bluetoothConnection = false;
            }
        }
        public void IRGestureUsed(int gesture)
        {
            IRControl(gesture);
        }
        public void Dispose()
        {
            try
            {
                Comms.bluetoothClose();
            }
            catch (Exception) { };
            //            Comms.Dispose();
        }
        public void setAllReg(int[] Variables)
        {
            if (name.Contains("Fake"))
            {
                cali.WriteToCaliFile(new List<int>() {
                    Variables[16], Variables[16] , Variables[16] ,
                    Variables[13], Variables[14] , Variables[15] ,
                    Variables[10], Variables[11] , Variables[12]
                    });
                return;
            }
            PERIOD = Variables[0];
            FREQ = Variables[1];
            ADC = Variables[2];
            PS = Variables[3];
            gscale = Variables[4];
            grate = Variables[5];
            ascale = Variables[6];
            arate = Variables[7];
            mscale = Variables[8];
            mrate = Variables[9];

            byte p1 = (byte)(PERIOD >> 8);

            for (byte i = 1; i <= Variables.Count(); i++)
            {
                sendClearData(7);
                sendClearData(67);
                sendClearData((byte)((i * 2) + 1));
                sendClearData((byte)(Variables[i - 1] >> 8));
                sendClearData((byte)(Variables[i - 1]));
                sendClearData(11);
            }

            sendClearData(7);
            sendClearData(67);
            sendClearData(221);
            sendClearData(0);
            sendClearData(0);
            sendClearData(11);
            freq = getFrequency() / (FREQ + 1);
            samplePeriod = 1f / (float)freq;

        }
        public void initfreq()
        {
            freq = getFrequency() / (FREQ + 1);
            samplePeriod = 1f / (float)freq;
        }
        private bool USB(string imuUSBName)
        {
            UInt32 ftdiDeviceCount = 0;
            FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_OK;

            // Create new instance of the FTDI device class
            FTDI myFtdiDevice = new FTDI();

            // Determine the number of FTDI devices connected to the machine
            ftStatus = myFtdiDevice.GetNumberOfDevices(ref ftdiDeviceCount);
            // Check status
            if (ftStatus == FTDI.FT_STATUS.FT_OK)
            {
                WriteDataToBox("Number of FTDI devices: " + ftdiDeviceCount.ToString());
                WriteDataToBox("\n");

                // Allocate storage for device info list
                FTDI.FT_DEVICE_INFO_NODE[] ftdiDeviceList = new FTDI.FT_DEVICE_INFO_NODE[ftdiDeviceCount];

                // Populate our device list
                ftStatus = myFtdiDevice.GetDeviceList(ftdiDeviceList);
                bool found = false;
                string deviceID = "";
                if (ftStatus == FTDI.FT_STATUS.FT_OK)
                {
                    for (UInt32 i = 0; i < ftdiDeviceCount; i++)
                    {

                        if (ftdiDeviceList[i].Description.ToString() == imuUSBName)
                        {
                            deviceID = ftdiDeviceList[i].SerialNumber.ToString();
                            found = true;
                        }
                    }
                }
                ManagementObjectCollection ManObjReturn;
                ManagementObjectSearcher ManObjSearch;
                ManObjSearch = new ManagementObjectSearcher("Select * from Win32_PnPEntity");
                ManObjReturn = ManObjSearch.Get();

                foreach (ManagementObject ManObj in ManObjReturn)
                {
                    if (ManObj["DeviceID"].ToString().Contains("FTDI"))
                    {
                        if (found && ManObj["DeviceID"].ToString().Contains(deviceID))
                        {
                            COMportName = ManObj["Name"].ToString();
                            WriteDataToBox("\nNU found on ");
                            WriteDataToBox(COMportName);
                            WriteDataToBox("\n");
                            PortHandler.COMPort = COMportName.Substring((COMportName.IndexOf("(")) + 1, (COMportName.IndexOf(")") - COMportName.IndexOf("(")) - 1); ;

                            return true;
                        }

                    }
                }
            }
            else
            {
                WriteDataToBox("Failed to get number of devices (error " + ftStatus.ToString() + ")");
                return false;
            }
            return false;
        }
        private bool BluetoothConnect(string imuMemory, bool bluetooth, string imuBTName)
        {
            BluetoothDeviceInfo[] devices;
            WriteDataToBox("Connecting to IMU\n");
            String[] deviceaddress = new String[] { "" };
            if (File.Exists(imuMemory))
            {
                deviceaddress = File.ReadAllLines(imuMemory);
            }
            if (deviceaddress[0].Length > 0 && bluetooth == true)
            {

                WriteDataToBox("Attempting Quick Connect\n");
                byte[] address = { 0, 0, 0, 0, 0, 0 };

                for (int i = 0; i < 6; i++)
                {
                    address[i] = (byte)(Convert.ToInt32((deviceaddress[0][10 - (2 * i)].ToString() + deviceaddress[0][11 - (2 * i)].ToString()), 16));
                }
                BluetoothAddress btaddress = new BluetoothAddress(address);


                if (Comms.connectthroughbluetoothaddress(btaddress))
                {
                    WriteDataToBox("Connection Successful\n");
                    Comms.DataIn += new CommunicationsClass.EventHandler(Comms_OnDataIn);
                    return true;
                }
                else
                {


                    if (bluetooth == true)
                    {
                        WriteDataToBox("Searching for Paired Bluetooth Device...\n");
                        BluetoothClient client = new BluetoothClient();
                        devices = client.DiscoverDevices();
                        foreach (BluetoothDeviceInfo d in devices)
                        {
                            if (d.DeviceName == imuBTName)
                            {
                                File.WriteAllText(imuMemory, d.DeviceAddress.ToString());
                                WriteDataToBox("Device Found\nAttempting Connection...\n");
                                if (Comms.connectthroughbluetooth(d))
                                {
                                    WriteDataToBox("Connection Successful\n");
                                    Comms.DataIn += new CommunicationsClass.EventHandler(Comms_OnDataIn);
                                    return true;
                                }
                                else
                                {
                                    WriteDataToBox("Connection Failed\n");
                                    return false;
                                }
                            }
                        }
                    }
                }

            }

            else
            {
                if (bluetooth == true)
                {
                    WriteDataToBox("Searching for Paired Bluetooth Device...\n");
                    BluetoothClient client = new BluetoothClient();
                    devices = client.DiscoverDevices();
                    foreach (BluetoothDeviceInfo d in devices)
                    {
                        if (d.DeviceName == imuBTName)
                        {
                            File.AppendAllText(imuMemory, d.DeviceAddress.ToString());
                            WriteDataToBox("Device Found\nAttempting Connection...\n");
                            if (Comms.connectthroughbluetooth(d))
                            {
                                WriteDataToBox("Connection Successful\n");
                                Comms.DataIn += new CommunicationsClass.EventHandler(Comms_OnDataIn);
                                return true;
                            }
                            else
                            {
                                WriteDataToBox("Connection Failed\n");
                                return false;
                            }
                        }
                    }
                }
            }
            return false;
        }
        void PortHandler_OnMessageReceived(SerialHandler.Message_EventArgs e)
        {
            OnDataIn(e.RawMessage);
            try { e.RawMessage.RemoveRange(0, e.RawMessage.Count); }
            catch (Exception) { }
        }


        void Comms_OnDataIn(Communications.Message_EventArgs e)
        {
            OnDataIn(e.RawMessage);
            try { e.RawMessage.RemoveRange(0, e.RawMessage.Count); }
            catch (Exception)
            {
            }
        }
        public void sendData(string data)
        {
            if (bluetoothConnection)
            {
                Comms.senddata(data);
            }
            if (USBConnection)
            {
                if (!PortHandler.open) { openUSB(); }
                PortHandler.send_char(data);
            }
        }
        public void sendClearData(byte data)
        {
            if (bluetoothConnection)
            {
                Comms.senddata(data);
            }
            if (USBConnection)
            {
                if (!PortHandler.open) { openUSB(); }
                PortHandler.send_clear_data(data);
            }
        }

        public void StopNU()
        {
           /* try { Form3.Close(); } catch (Exception) { };
            try { Form2.Close(); } catch (Exception) { };
            try { Form5.Close(); } catch (Exception) { };
            try { Form4.Close(); } catch (Exception) { };*/ //For Real-time plotting
            /*try { Form6.Close(); } catch (Exception) { };*/

            try
            {
                comp = false; comptest = false;
                cali.caliCountDown = 100;
                cali.calibrated = false;
                sendData("M0dg");
                Comms.getStream();
            }
            catch (Exception)
            {
                WriteDataToBox("Unable to stop IMU");
            }
            try
            {
                //MMGRec.close();
            }
            catch (Exception) { }

        }
        public bool softStopNU()
        {
            bool success = true;
            try
            {
                sendData("M0dg");

                Comms.getStream();
            }
            catch (Exception)
            {
                WriteDataToBox("Unable to stop IMU");
                success = false;

            }
            return success;
        }

        public void initReg()
        {
            sendData("RegR");
        }
        public void editReg()
        {
            sendData("RegR");
            form8 = new Form8(name.Contains("Fake"), this);
            form8.Show();
            if (name.Contains("Fake"))
            {
                if (form8 != null)
                {
                    form8.setdata(PERIOD, FREQ, ADC, PS, gscale, grate, ascale, arate, mscale, mrate, cali.MagX, cali.MagY, cali.MagZ, cali.AccelX, cali.AccelY, cali.AccelZ, cali.gyroX, cali.gyroY, cali.gyroZ, 0);
                }
                form8.setFakeData();
                Calibration c = cali;
            }
        }

        private bool openUSB()
        {
            try { PortHandler.Close(); }
            catch (Exception) { }
            bool success = PortHandler.Open(921600);//1250000
            if (success)
            {
                WriteDataToBox("\nNU Communications Open\n");
                PortHandler.OnMessageReceived += new Handler.MessageReceivedEvent(PortHandler_OnMessageReceived);
            }
            else
            {
                WriteDataToBox("\nUnable To Open NU Communications\n");
                PortHandler.open = false;
            }
            return success;
        }

        public void M1dg()
        {
            OpenForm2();
            sendData("M1dg");
        }

        public void OpenForm2()
        {
            Form2 = new Form2();
            System.Windows.Forms.Application.DoEvents();
            // Show the settings form
            Form2.BackColor = System.Drawing.Color.Black;
            Form2.Show();
            Form2.Init(false);
        }
        public void CloseForm2()
        {
            Form2.Dispose();
            Form2.Close();
        }



        public void M3dg()
        {
            Form4 = new GraphForm();
            Form4.Graph(8, false);
            Form4.Show();
            sendData("M3dg");
        }
        public void M4dgold()
        {
            _docked = false;
            Form2 = new Form2();
            System.Windows.Forms.Application.DoEvents();

            Form2.Show();
            Form2.Init(_docked);



            sendData("M4dg");
        }
        public void M12d()
        {
            try { Form1 = new Form1(this); }
            catch (Exception)
            {
                Form1 = new Form1(this);
            }
            Form1.OpenForms();
            Form1.Show();
            comp = true;
            sendData("M4dg");
        }
        public void M13d()
        {
            try { Form1 = new Form1(this); }
            catch (Exception)
            {
                Form1 = new Form1(this);
            }
            Form1.OpenForms();
            Form1.Show();
            comptest = true;
            M4dg(true);
        }
        public void M4dg(bool _VisOff)
        {
            VisOff = _VisOff;
            _docked = true;
            if (!VisOff)
            {
                try { Form5 = new Form5(Int32.Parse(name), this); }
                catch (Exception)
                {
                    Form5 = new Form5(000, this);
                }
                Form5.OpenForms();
                Form5.Show();
            }
            fullMMG = false;
            if (SecondAccelHandler == null)
            {
                try
                {
                    sendData("M4dg");
                }
                catch (Exception) { }
            }
            else
            {
                try
                {
                    sendData("M9dg");
                }
                catch (Exception) { }
            }
        }
        public void Playback(bool _VisOff)
        {
            VisOff = _VisOff;
            _docked = true;
            if (!VisOff)
            {
                try { Form6 = new Form6(Int32.Parse(name), this); }
                catch (Exception)
                {
                    Form6 = new Form6(000, this);
                }
                //Form6.OpenForms();
                Form6.Show();
            }
            fullMMG = false;
            if (SecondAccelHandler == null)
            {
                try
                {
                    sendData("M4dg");
                }
                catch (Exception) { }
            }
            else
            {
                try
                {
                    sendData("M9dg");
                }
                catch (Exception) { }
            }
        }
        public void HDMMG()
        {
            try { Form5 = new Form5(Int32.Parse(name), this); }
            catch (Exception)
            {
                Form5 = new Form5(000, this);
            }
            Form5.OpenForms();
            Form5.Show();
            try
            {
                sendData("M8dg");
            }
            catch (Exception) { }
        }
        public void HDMMG2()
        {
            try { Form5 = new Form5(Int32.Parse(name), this); }
            catch (Exception)
            {
                Form5 = new Form5(000, this);
            }
            Form5.OpenForms();
            Form5.Show();
            try
            {
                sendData("M10d");
            }
            catch (Exception) { }
        }

        public void M9dg(bool _VisOff)
        {
            VisOff = _VisOff;
            _docked = true;
            if (!VisOff)
            {
                try { Form5 = new Form5(Int32.Parse(name), this); }
                catch (Exception)
                {
                    Form5 = new Form5(000, this);
                }
                Form5.OpenForms();
                Form5.Show();
            }
            fullMMG = false;
            if (SecondAccelHandler == null)
            {
                SecondAccelHandler = new NU();
                SecondAccelHandler.FakeInit("Fake1", CurrentDirectory);
                SecondAccelHandler.ShowConfig(new List<int>()
            {
                (id>>8)&0xFF,

                (id&0xFF),
                (PERIOD>>8)&0xFF,
                (PERIOD&0xFF),
                (FREQ>>8)&0xFF,
                (FREQ&0xFF),
                (ADC>>8)&0xFF,
                (ADC&0xFF),
                (PS>>8)&0xFF,
                (PS&0xFF),
                gscale,
                grate ,
                ascale,
                arate,
                mscale,
                mrate
        });
                AddNU(SecondAccelHandler);
                SecondAccelHandler.saveData = saveData;
                SecondAccelHandler.Parent = this;
            }
            SecondAccelHandler.M4dg(VisOff);
            sendData("M9dg");
        }
        public void init2ndAccel()
        {
            
            SecondAccelHandler = new NU();
            SecondAccelHandler.FakeInit("Fake1", CurrentDirectory);
            SecondAccelHandler.ShowConfig(new List<int>()
            {
                (id>>8)&0xFF,
                (id&0xFF),
                (PERIOD>>8)&0xFF,
                (PERIOD&0xFF),
                (FREQ>>8)&0xFF,
                (FREQ&0xFF),
                (ADC>>8)&0xFF,
                (ADC&0xFF),
                (PS>>8)&0xFF,
                (PS&0xFF),
                gscale,
                grate ,
                ascale,
                arate,
                mscale,
                mrate
        });
            AddNU(SecondAccelHandler);
            SecondAccelHandler.saveData = saveData;
            SecondAccelHandler.Parent = this;
        }

        public void MMGButtonAdd(string newDescription)
        {
            MMGRec.addGesture(newDescription);
            List<string> GestureNames = MMGRec.returnGestureNames();
            Form5.WriteText("");
            foreach (string GN in GestureNames)
            {
                Form5.appendText(GN + ", ");
            }
            Form5.appendText("\nCurrent Gesture: " + GestureNames[Form5.CurrentGestures]);
            WriteDataToBox("\nCurrent Gesture: " + GestureNames[Form5.CurrentGestures]);
        }
        public void Form5Record(bool button, float yaw)
        {

            if (button)
            {
                Form5.CurrentGestures = 0;
                MMGRec.Training_mode = true;
                List<string> GestureNames = MMGRec.returnGestureNames();
                Form5.WriteText("");
                foreach (string GN in GestureNames)
                {
                    Form5.appendText(GN + ", ");
                }
                Form5.appendText("\nCurrent Gesture: " + GestureNames[Form5.CurrentGestures]);
                gestureCount = 0;
                sendData("M4dg");
            }
            else
            {
                if (Form5.test)
                {
                    Form5.add2conf(MMGRec.previous_gesture);
                }
                else
                {
                    if (MMGRec.Training_mode)
                    {
                        gestureCount++;
                        Form5.Form4b.addfullarray(MMGRec.returnArray());
                        if (gestureCount >= 5)
                        {

                            if (Form5.CurrentGestures+1 == MMGRec.returnGestureNames().Count)
                            {
                                MMGRec.Training_mode = false;
                                Form5.WriteButton4("Record");
                               
                                sendData("M0dg");
                            }

    

                            Form5Next();
                            gestureCount = 0;
                        }
                    }
                    else
                    {
                        Form5.WriteText("");
                        foreach (string GN in MMGRec.returnGestureNames())
                        {
                            Form5.appendText(GN + ", ");
                        }
                        Form5.appendText("\nPredicted Gesture: " + MMGRec.returnGestureNames()[MMGRec.previous_gesture]);

                        WriteDataToBox("\nPredicted Gesture: " + MMGRec.returnGestureNames()[MMGRec.previous_gesture]);
                        if (Form5.Computer_control)
                        {
                            gestureData(MMGRec.returnGestureNames(MMGRec.previous_gesture), quat);
                        }
                        if (Form5.Use_IR.Checked)
                        {
                            IRControl(MMGRec.previous_gesture + 3 + '0');
                        }
                        if (Form5.Sky_control.Checked)
                        {
                            switch (MMGRec.previous_gesture)
                            {

                                case 0:
                                    IRControl(0 + 1 + '0');
                                    System.Threading.Thread.Sleep(1000);
                                    IRControl(0 + 3 + '0');
                                    System.Threading.Thread.Sleep(1000);
                                    IRControl(0 + 3 + '0');
                                    System.Threading.Thread.Sleep(1000);
                                    break;
                                default:
                                    IRControl(MMGRec.previous_gesture + 1 + '0');
                                    break;
                            }
                            Form5.timerReset();

                        }
                    }
                }
            }
        }
        public void select()
        {
            IRControl(0 + 3 + '0');
        }
        public void Form5RecordCurrentTemplate(string userName, int _GestureNo)
        {
            MMGRec.SaveGestureToDatabase(userName, _GestureNo);
        }
        public void Form5LoadPreviousTemplate(string userName, int _GestureNo)
        {
            MMGRec.ReadGestureFromDatabase(userName, _GestureNo);

            Form5.Form4b.addfullarray(MMGRec.returnArray());
        }
        public void Form5Next()
        {
            Form5.CurrentGestures = (Form5.CurrentGestures + 1) % MMGRec.returnGestureNames().Count;
            MMGRec.currentGesture = Form5.CurrentGestures;
            Form5.WriteText("");
            foreach (string GN in MMGRec.returnGestureNames())
            {
                Form5.appendText(GN + ", ");
            }

            Form5.appendText("\nCurrent Gesture: " + MMGRec.returnGestureNames()[Form5.CurrentGestures]);
        }
        public void Form5Test()
        {
            MMGRec.Training_mode = false;
            Form5.beginTest(MMGRec.returnGestureNames());
            gestureCount = 0;
            sendData("M4dg");
        }
        public void Form5End()
        {
            sendData("M0dg");
        }
        public void Form5Training()
        {
            if (Form5.button3.Text == "Free Run")
            {
                MMGRec.Training_mode = false;
                Form5.button3.Text = "Training";
                sendData("M4dg");
            }
            else
            {
                MMGRec.Training_mode = true;
                Form5.button3.Text = "Free Run";
                sendData("M0dg");
            }
        }
        public void Form5ComputerControl()
        {
            if (Form5.ComputerContrl.Text == "Comp Control")
            {
                MMGRec.Training_mode = false;
                Form5.ComputerContrl.Text = "Running";
                sendData("M4dg");
            }
            else
            {
                MMGRec.Training_mode = true;
                Form5.ComputerContrl.Text = "Comp Control";
                sendData("M0dg");
            }
        }
        public void SaveClassifierData(string UserName,string Position)
        {
            if (UserName == "") { UserName = DateTime.Now.ToString("yyyyMMdd"); }
            if (Position == "") { Position = DateTime.Now.ToString("HHmmss"); }
            string path = CurrentDirectory + "..\\..\\..\\DataFiles\\"+ UserName + "\\" + Position + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss");
            if (File.Exists(path+".csv"))
            {
            }
            else
            {
                System.IO.Directory.CreateDirectory(CurrentDirectory + "..\\..\\..\\DataFiles\\" + UserName + "\\" + Position + "\\");
                System.IO.File.WriteAllText(path+".csv", string.Empty);
            }
            StreamWriter file = new System.IO.StreamWriter(path + ".csv", true);
            foreach (double[] line in MMGRec.fullRawDataArchive)
            {
                file.WriteLine(String.Join(",", line.Select(p => p.ToString()).ToArray()));
            }
            file.Close();
            DataReady(path, 1);

        }
        public void MMGstore()
        {
            MMGRec.store = true;
        }
        public void MMG()
        {
            // OpenForm2();
            _docked = true;
            Form5 = new Form5(Int32.Parse(name),this);
            Form5.addButtons(this);
            Form5.OpenForms();
            Form5.setModeVis(true);
            Form5.Show();
            fullMMG = true;
            MMGRec = new MMG();
            MMGRec.addGesture("Open");
            MMGRec.addGesture("Close");
            MMGRec.addGesture("Point");
            MMGRec.addGesture("1 Finger Pinch");
            //MMGRec.addGesture("2 Finger Pinch");
            //MMGRec.addGesture("Thumb Up");
            //MMGRec.addGesture("Key");
            //MMGRec.addGesture("FingerRoll");
            //           MMGRec.addGesture("Close");
            //           MMGRec.addGesture("Right Rotate");
            //           MMGRec.addGesture("Left Rotate");
            //            MMGRec.addGesture("Forefinger Down");
            //           MMGRec.addGesture("FingerRoll");

            //MMGRec.addGesture("Thumb Tap");
            //MMGRec.addGesture("Thumb Tap");
            // MMGRec.addGesture("1 Finger Tap");
            // MMGRec.addGesture("2 Finger Tap");
            //MMGRec.addGesture("3 Finger Tap");
            //MMGRec.addGesture("4 Finger Tap");
            //MMGRec.addGesture("4 Finger");
            List<string> GestureNames = MMGRec.returnGestureNames();
            foreach (string GN in GestureNames)
            {
                Form5.appendText(GN + ", ");
            }
            Form5.appendText("\nCurrent Gesture: " + GestureNames[Form5.CurrentGestures]);
        }
        public void M7dg()
        {
            // OpenForm2();
            Form5 = new Form5(Int32.Parse(name),this);
            Form5.OpenForms();
            Form5.Show();

            sendData("M7dg");
        }


        public void Save()
        {
            sendData("M4dg");
        }
        public void MATLAB()
        {
            sendData("M2dg");
        }
        void MMGin(List<int> message, int GyroValue, bool docked,Vector3 _endPosition)
        {
            int[] testval = new int[8];
            testval[0] = (message[0] * 256) + message[1];
            testval[1] = (message[2] * 256) + message[3];
            testval[2] = (message[4] * 256) + message[5];
            testval[3] = (message[6] * 256) + message[7];
            testval[4] = (message[8] * 256) + message[9];
            testval[5] = (message[10] * 256) + message[11];
            Stopwatch watch = Stopwatch.StartNew();
            int _gesture = -1 ;
            bool GyroLimitExceeded = false;
            if (MMGRec != null)
            {
                if (GyroValue > 1000)
                {
                    Form5.R2G2 = 0;
                    GyroLimitExceeded = true;
                }
                else
                {
                    Form5.R2G2 = 255;
                }
                if (MMGRec.gestureClock > MMGRec.delay_between_gestures)
                {
                    Form5.R2G = 255;
                }
                else
                {
                    Form5.R2G = 0;
                }
                if (Form5.currentGesture == -1|| Form5.currentGesture > 24)
                {
                    _gesture = MMGRec.addData(testval, _endPosition, Form5.currentGesture, GyroLimitExceeded);
                }
                else
                {
                    try
                    {
                        _gesture = MMGRec.addData(testval, _endPosition, Form5.testNumbers[Form5.currentGesture], GyroLimitExceeded);
                    }
                    catch(Exception e) {}
                }
            }
            else { _gesture = -1; }
            if (_gesture != -1&& _gesture != -2)
            {
                if (baxter)
                {
                    gesture = _gesture;
                    if (MMGRec.returnGestureNames().Count > MMGRec.previous_gesture)
                    {
                        WriteDataToBox("\nPredicted Gesture: " + MMGRec.returnGestureNames()[MMGRec.previous_gesture] + "\n");
                    }
                    else
                    {
                        WriteDataToBox("\nGesture Doesn't Exsist");
                    }
                    return;
                }
                float yaw = (float)Math.Atan2(2 * (quat.W * quat.Z + quat.X * quat.Y), 1 - 2 * (quat.Y * quat.Y + quat.Z * quat.Z));
                Form5Record(false, yaw);
            }
            if(_gesture==-2)
            {
                float yaw = (float)Math.Atan2(2 * (quat.W * quat.Z + quat.X * quat.Y), 1 - 2 * (quat.Y * quat.Y + quat.Z * quat.Z));
                Form5Record(false, yaw);
            }

            watch.Stop();
            long a = watch.ElapsedTicks;
            if (VisOff)
            {
                return;
            }

            try
            {

                Form5.Form4.addValue((testval[0] * 3.3f) / 1024, (testval[1] * 3.3f) / 1024, (testval[2] * 3.3f) / 1024, (testval[3] * 3.3f) / 1024, (testval[4] * 3.3f) / 1024, (testval[5] * 3.3f) / 1024, (testval[6] * 3.3f) / 1024, (testval[7] * 3.3f) / 1024);
            }
            catch (Exception)
            {

            }
        }
        void MMGdatain(List<int> message, int GyroValue, bool docked)
        {
            if(comp == true) { return; }
            if (saveData == true)
            {
            }
            else
            {
                int[] testval = new int[8];
                if (slow < -10) { slow = 10; }
                float val1 = (message[0] * 256) + message[1];
                testval[0] = (int)val1;
                val1 = (val1 * 3.3f) / 1024;
                float val2 = (message[2] * 256) + message[3];
                testval[1] = (int)val2;
                val2 = (val2 * 3.3f) / 1024;
                float val3 = (message[4] * 256) + message[5];
                testval[2] = (int)val3;
                val3 = (val3 * 3.3f) / 1024;
                float val4 = (message[6] * 256) + message[7];
                testval[3] = (int)val4;
                val4 = (val4 * 3.3f) / 1024;
                float val5 = (message[8] * 256) + message[9];
                testval[4] = (int)val5;
                val5 = (val5 * 3.3f) / 1024;
                float val6 = (message[10] * 256) + message[11];
                testval[5] = (int)val6;
                val6 = (val6 * 3.3f) / 1024;
                float val7 = (message[12] * 256) + message[13];
                testval[6] = (int)val7;
                val7 = (val7 * 3.3f) / 1024;
                float val8 = (message[14] * 256) + message[15];
                testval[7] = (int)val8;
                val8 = (val8 * 3.3f) / 1024;
                slow--;

                if (Parent == null)
                {
                    DataReady(new byte[] { 12,

                    (byte)message[4],
                    (byte)message[5]

                });
                }
                else
                {
                    Parent.DataReady(new byte[] { 12,

                    (byte)message[4],
                    (byte)message[5]

                });
                }


                for (int i = 0; i < 7; i++)
                {
                    doneEvents[i] = new ManualResetEvent(false);
                    WorkerClass f = new WorkerClass(doneEvents[i], testval[i], MMGChannel[i], HandControlProcessorMemory[i], Debouncer[i], thres);
                    WorkerArray[i] = f;
                    ThreadPool.QueueUserWorkItem(f.ThreadPoolCallback, i);

                }


                WaitHandle.WaitAll(doneEvents);
                for (int i = 0; i < 7; i++)
                {
                    Debouncer[i] = WorkerArray[i].MMGProcessor.current;
                    HandControlProcessorMemory[i] = WorkerArray[i].MMGProcessor.memory;
                    MMGChannel[i] = WorkerArray[i].MMGProcessor.MMGdata;

                    if (WorkerArray[i].MMGProcessor.saveReturn)
                    {
                        if (GyroValue <= 1500)
                        {
                            // Form1._Form1.WriteToBoxXThread("Triggered by channel" + i.ToString());
                            if (i == 3)
                            {
                                try { SendKeys.SendWait(" "); } catch (Exception) { };
                                if (Kat == false)
                                {
                                    Trigger(0); // hand triggered - class 6
                                }
                                else
                                {
                                    if (address == 1)
                                    {
                                        Trigger(0); // hand triggered - class 6
                                    }
                                }
                            }
                            else
                            {
                            }
                        }
                    }
                }
                if (VisOff) { return; }
                if (WorkerArray[3].saveValue != 0)
                {
                    // Form1._Form1.WriteToBoxXThread("Current = " + WorkerArray[3].MMGProcessor.current + "\n");
                    openclose = (byte)WorkerArray[3].MMGProcessor.current;
                    try
                    {
                         //Form5.Form4b.addValue((float)WorkerArray[3].saveValue / 25, ((float)thres) / 25, 0);

                        //Form5.Form4.addValue(val1, val2, val3, val4, val5, val6, val7, val8);
                    }
                    catch (Exception)
                    {

                    }
                }
                if (slow == 0)
                {


                    try
                    {
                        MMG_filter_1.Update(val1);
                        float data_filtered_1 = 2*MMG_filter_1.output_lowpass;
                   
                        MMG_filter_2.Update(val2);
                        float data_filtered_2 = 2*MMG_filter_2.output_lowpass;
                        MMG_filter_3.Update(val3);
                        float data_filtered_3 = 2*MMG_filter_3.output_lowpass;
                        MMG_filter_4.Update(val4);
                        float data_filtered_4 = 2*MMG_filter_4.output_lowpass;
                        MMG_filter_5.Update(val5);
                        float data_filtered_5 = 2*MMG_filter_5.output_lowpass;
                        MMG_filter_6.Update(val6);
                        float data_filtered_6 = 2*MMG_filter_6.output_lowpass;
                        MMG_filter_7.Update(val7);
                        float data_filtered_7 = 2*MMG_filter_7.output_lowpass;
                        MMG_filter_8.Update(val8);
                        float data_filtered_8 = 2*MMG_filter_8.output_lowpass;

                        //float data_filtered_2 = 2*MMG_filter.output_lowpass;
                        if (Form5 != null)
                        {
                            Form5.Form4.addValue(data_filtered_1*5, data_filtered_2*10, data_filtered_3*2, data_filtered_4, data_filtered_5, data_filtered_6, data_filtered_7, data_filtered_8);
                            // Used for real-time plotting
                        }
                        if (Form6 != null)
                        { 

                            Form6.Update_MMGValue((int)(data_filtered_1*150*15), (int)(data_filtered_2 * 150*15), (int)(data_filtered_3 * 150*10)); 
                        }

                        //Form5.Form4.addValue(data_filtered_1 + 2, data_filtered_2 +2, data_filtered_2+2);
                        // Form5.Form4.addValue(val1, val2, val3, val4, val5, val6, val7, val8);
                        //Form5.Form4.addValue(val1, val2, val8);
                        slow = 10;

                    }

                    catch (Exception)
                    {
                        if (!Kat)
                        {
                            sendData("M0dg");
                        }
                    }
                }
            }
            if (Kat)
            {
                if (Parent == null)
                {
                    DataReadyUnity(true, unitydata);
                }
                else
                {
                    Parent.DataReadyUnity(true, unitydata);
                }
            }
        }

        void UpdateSD()
        {
            sendClearData(7);
            sendClearData(67);
            sendClearData(12);
            sendClearData((byte)(cali.MagX >> 8));
            sendClearData((byte)(cali.MagX));
            sendClearData(11);
            System.Threading.Thread.Sleep(100);
            sendClearData(7);
            sendClearData(67);
            sendClearData(13);
            sendClearData((byte)(cali.MagY >> 8));
            sendClearData((byte)(cali.MagY));
            sendClearData(11);
            System.Threading.Thread.Sleep(100);
            sendClearData(7);
            sendClearData(67);
            sendClearData(14);
            sendClearData((byte)(cali.MagZ >> 8));
            sendClearData((byte)(cali.MagZ));
            sendClearData(11);
            System.Threading.Thread.Sleep(100);
            sendClearData(7);
            sendClearData(67);
            sendClearData(11);
            sendClearData(0);
            sendClearData(0);
            sendClearData(11);
        }

        public void ShowConfig(List<int> message)
        {

            id = (message[0] << 8) | message[1];
            PERIOD = (message[2] << 8) | message[3];
            FREQ = (message[4] << 8) | message[5];
            ADC = (message[6] << 8) | message[7];
            PS = (message[8] << 8) | message[9];
            gscale = message[10];
            grate = message[11];
            ascale = message[12];
            arate = message[13];
            mscale = message[14];
            mrate = message[15];
            packetCorrection = 1f / (float)(FREQ+1f);
            int firmware = 5;
            int MagX = 0;
            int MagY = 0;
            int MagZ = 0;
            int AccX = 0;
            int AccY = 0;
            int AccZ = 0;
            int GyroX = 0;
            int GyroY = 0;
            int GyroZ = 0;
            if (message.Count > 25)
            {
                MagX = sw_Maths.getsignedvalue((message[16] << 8) | message[17]);
                MagY = sw_Maths.getsignedvalue((message[18] << 8) | message[19]);
                MagZ = sw_Maths.getsignedvalue((message[20] << 8) | message[21]);
                AccX = sw_Maths.getsignedvalue((message[22] << 8) | message[23]);
                AccY = sw_Maths.getsignedvalue((message[24] << 8) | message[25]);
                AccZ = sw_Maths.getsignedvalue((message[26] << 8) | message[27]);
                GyroX = sw_Maths.getsignedvalue((message[28] << 8) | message[29]);
                GyroY = sw_Maths.getsignedvalue((message[30] << 8) | message[31]);
                GyroZ = sw_Maths.getsignedvalue((message[32] << 8) | message[33]);
                firmware = (message[34] << 8) | message[35];
            }
            firmwareVersion = firmware;

            if (firmwareVersion >= 6 && IMUType != "MMARK")
            {
                cali = new Calibration(name, CurrentDirectory, MagX, MagY, MagZ, AccX, AccY, AccZ, MagX, MagY, MagZ, false);
            }
            else
            {
                cali = new Calibration(name, CurrentDirectory, MagX, MagY, MagZ, AccX, AccY, AccZ, MagX, MagY, MagZ, true);
            }
            initfreq();
            if (form8 != null && firmware == 5)
            {
                form8.setdata(PERIOD, FREQ, ADC, PS, gscale, grate, ascale, arate, mscale, mrate, firmware);
            }
            if (form8 != null && firmware >= 6)
            {
                form8.setdata(PERIOD, FREQ, ADC, PS, gscale, grate, ascale, arate, mscale, mrate, MagX, MagY, MagZ, AccX, AccY, AccZ, GyroX, GyroY, GyroZ, firmware);
            }

        }
        int getFrequency()
        {
            if (PERIOD == 0)
            {
                //Data retrieval failed
                sendData("RegR");
                return 1000;
            }
            int frequency;
            int[] prescaler = { 1, 8, 64, 256 };
            int clock = 30000000;
            frequency = (clock) / (PERIOD * prescaler[PS] * 2);

            return frequency;
        }
        int getFrequency(int ps, int period)
        {
            int frequency;
            int[] prescaler = { 1, 8, 64, 256 };
            int clock = 30000000;
            frequency = (clock) / (period * prescaler[ps] * 2);

            return frequency;
        }
        int[] setFrequency(int frequency)
        {
            int[] variables = new int[2] { 65536, 0 };
            int ps = 0;

            int[] prescaler = { 1, 8, 64, 256 };
            int clock = 30000000;
            while ((variables[0] >= 65536) && (variables[1] <= 4))
            {
                variables[0] = (clock) / (frequency * prescaler[variables[1]] * 2);
                variables[1]++;
            }
            if (ps == 5)
            {
                return null;
            }
            return variables;
        }

        public void battery()
        {
            sendData("Batt");
        }


        void OnDataIn(List<int> message)
        {
            if (message.Count > 2)
            {
                int data = message[0];
                int mode = message[1];
                if (mode == 19)
                {
                    message[1] = 6;
                    if (SecondAccelHandler != null)
                    {
                        SecondAccelHandler.OnDataIn(message);
                    }
                    else
                    {
                        mode = -1;
                    }
                }
                message.RemoveRange(0, 2);
                if (data - 1 != message.Count())
                {
                    return;
                }
                if (mode == 0)
                {
                    foreach (byte i in message)
                    {
                        WriteDataToBox((Convert.ToChar(i)).ToString());
                    }
                }
                if (mode == 1)
                {
                    WriteDataToBox(string.Join(",", message.ToArray()));
                }

                if (mode == 2 || mode == 3)
                {
                    int PacketNumber = message[message.Count - 1];
                    processData(message, mode, false, PacketNumber);
                    return;
                }
                if (mode == 4)
                {
                    return;
                }
                if (mode == 5)
                {
                    MMGdatainShort(message, 0);
                    return;
                }
                if (mode == 12)
                {
                    List<int> fakedata = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, message[0], message[1], message[2], message[3], message[4], message[5] };
                    processData(fakedata, 2, true, previousPacketNumber+1);
                    List<int> fakedata2 = new List<int>() { message[6], message[7], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,0  };

                    MMGdatain(fakedata2, 0, true);

                    DataReady(((short)sw_Maths.int32toint16((message[0] + ((message[1]) << 8)))).ToString() + ',' +
                    ((short)sw_Maths.int32toint16((message[2] + ((message[3]) << 8)))).ToString() + ',' +
                    ((short)sw_Maths.int32toint16((message[4] + ((message[5]) << 8)))).ToString() + ',' +
                    ((short)sw_Maths.int32toint16((message[7] + ((message[6]) << 8)))).ToString(),0 
                    );
                }
                if (mode == 21)
                {
                    int ch =numpad*6;
                    List<int> fakedata = new List<int>() { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, message[0+ ch], message[1+ ch], message[2+ ch], message[3+ ch], message[4+ ch], message[5+ ch] };
                    processData(fakedata, 2, true, previousPacketNumber + 1);
                    List<int> fakedata2 = new List<int>() { message[48], message[49], 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                    MMGdatain(fakedata2, 0, true);

                    DataReady(((short)sw_Maths.int32toint16((message[0] + ((message[1]) << 8)))).ToString() + ',' +
                    ((short)sw_Maths.int32toint16((message[2] + ((message[3]) << 8)))).ToString() + ',' +
                    ((short)sw_Maths.int32toint16((message[4] + ((message[5]) << 8)))).ToString() + ',' +
                    ((short)sw_Maths.int32toint16((message[7] + ((message[6]) << 8)))).ToString() + ',' +
                    ((short)sw_Maths.int32toint16((message[9] + ((message[8]) << 8)))).ToString() + ',' +
                    ((short)sw_Maths.int32toint16((message[11] + ((message[10]) << 8)))).ToString() + ',' +
                    ((short)sw_Maths.int32toint16((message[13] + ((message[12]) << 8)))).ToString() + ',' +
                    ((short)sw_Maths.int32toint16((message[15] + ((message[14]) << 8)))).ToString() + ',' +
                    ((short)sw_Maths.int32toint16((message[17] + ((message[16]) << 8)))).ToString() + ',' +
                    ((short)sw_Maths.int32toint16((message[19] + ((message[18]) << 8)))).ToString() + ',' +
                    ((short)sw_Maths.int32toint16((message[21] + ((message[20]) << 8)))).ToString() + ',' +
                    ((short)sw_Maths.int32toint16((message[23] + ((message[22]) << 8)))).ToString() + ',' +
                    ((short)sw_Maths.int32toint16((message[25] + ((message[24]) << 8)))).ToString() + ',' +
                    ((short)sw_Maths.int32toint16((message[27] + ((message[26]) << 8)))).ToString() + ',' +
                    ((short)sw_Maths.int32toint16((message[29] + ((message[28]) << 8)))).ToString() + ',' +
                    ((short)sw_Maths.int32toint16((message[31] + ((message[30]) << 8)))).ToString() + ',' +
                    ((short)sw_Maths.int32toint16((message[33] + ((message[32]) << 8)))).ToString() + ',' +
                    ((short)sw_Maths.int32toint16((message[35] + ((message[34]) << 8)))).ToString() + ',' +
                    ((short)sw_Maths.int32toint16((message[37] + ((message[36]) << 8)))).ToString() + ',' +
                    ((short)sw_Maths.int32toint16((message[39] + ((message[38]) << 8)))).ToString() + ',' +
                    ((short)sw_Maths.int32toint16((message[41] + ((message[40]) << 8)))).ToString() + ',' +
                    ((short)sw_Maths.int32toint16((message[43] + ((message[42]) << 8)))).ToString() + ',' +
                    ((short)sw_Maths.int32toint16((message[45] + ((message[44]) << 8)))).ToString() + ',' +
                    ((short)sw_Maths.int32toint16((message[47] + ((message[46]) << 8)))).ToString() + ',' +
                    ((short)sw_Maths.int32toint16((message[49] + ((message[48]) << 8)))).ToString(), 0
                    );
                }

                if (mode == 6)
                {
                    int PacketNumber = message[message.Count-1];

                    if (saveData == true)
                    {
                        if (!Control.ModifierKeys.HasFlag(Keys.Control))
                        {
                            StringBuilder messageString = new StringBuilder();
                            messageString.Append(name.ToString()).Append(',');
                            int count = 0;
                            foreach (int Data in message)
                            {
                                if (count != message.Count() - 1)
                                {
                                    messageString.Append(Data).Append(',');
                                    count++;
                                }
                                else
                                {
                                    messageString.Append(Data);
                                }
                            }
                            if (message.Count >= 18)
                            {
                                processData(message.GetRange(0, 18), 2, false, PacketNumber);
                                messageString.Append(',').Append(quat.W).Append(',').Append(quat.X).Append(',').Append(quat.Y).Append(',').Append(quat.Z).Append(',');
                            }

                            if (Parent == null)
                            {
                                try
                                {
                                    DataReady(messageString.ToString(), 0);
                                    //LoggingExtensions.WriteDebug(messageString.ToString());
                                }

                                catch { }
                            }
                            else
                            {
                                Parent.DataReady(messageString.ToString(), 0);
                                //LoggingExtensions.WriteDebug(messageString.ToString());
                            }

                        }
                        else
                        {
                            bool test = true;
                        }
                    }

                    else
                    {
                        if (message.Count <= 16)
                        {
                            while (message.Count != 16)
                            {
                                message.Add(0);
                            }
                            if (!fullMMG)
                            {
                                MMGdatain(message, 0, true);
                            }
                            else
                            {
                                MMGin(message, 0, true,EndPosition);
                            }
                        }
                        else
                        {
                            while (message.Count <= 34)
                            {
                                message.Add(0);
                            }
                            if (MagTest)
                            {
                                processData(message.GetRange(0, 18), 2, true, PacketNumber);
                            }
                            else
                            {
                                if (_docked)
                                {
                                    if (!fullMMG) { MMGdatain(message.GetRange(18, 16), processData(message.GetRange(0, 18), 2, true, PacketNumber), true); }
                                    else
                                    {
                                        if (MMGRec != null)
                                        {
                                            int thres = processData(message.GetRange(0, 18), 2, true, PacketNumber);
                                            MMGin(message.GetRange(18, 16), thres, true,EndPosition);
                                        }
                                    }
                                }                             else
                                {
                                    processData(message.GetRange(0, 18), 2, false, PacketNumber);
                                }
                            }
                        }
                    }
                    if (!Control.ModifierKeys.HasFlag(Keys.Control))
                    {
                        StringBuilder messageString = new StringBuilder();
                        messageString.Append(name.ToString()).Append(',');
                        int count = 0;
                        foreach (int Data in message)
                        {
                            if (count != message.Count() - 1)
                            {
                                messageString.Append(Data).Append(',');
                                count++;
                            }
                            else
                            {
                                messageString.Append(Data);
                            }
                        }
                        if (message.Count >= 18)
                        {
                            Vector3 YPR = QuaternionExtensions.YawPitchRollFromQuaternion(quatNew);
                            messageString.Append(',').Append(quat.W).Append(',').Append(quat.X).Append(',').Append(quat.Y).Append(',').Append(quat.Z).Append(',').Append(YPR.X).Append(',').Append(YPR.Y).Append(',').Append(YPR.Z).Append(',').Append(DateTime.Now.Ticks.ToString()).Append(',');
                        }

                        /*ToTalkerDataNum += 1;
                        string Talkernum = Convert.ToString(ToTalkerDataNum);
                        file.WriteLine(e.data + Talkernum);*/

                        string MMGTimeStampDate = DateTime.Now.ToString("yyyy.MM.dd.hh.mm.ss.ffffff");
                        int ActiveRest_State = 0;
                        if (Form6 != null)
                        {
                            ActiveRest_State = 1-Form6.active_rest;
                        }

                        messageString.Append(ActiveRest_State).Append(',').Append(MMGTimeStampDate);

                        Memory.Add(messageString.ToString());
                        //LoggingExtensions.WriteDebug(messageString.ToString());

                    }
                    else
                    {
                        bool test = true;
                    }

                    return;
                }
                if (mode == 9 || mode == 13)
                {
                    ShowConfig(message);
                }
                if (mode == 20)
                {
                    int batteryValue = (message[0] << 8) + message[1];
                    float batteryValuef = ((float)batteryValue / 512) * 3.3f;
                    WriteDataToBox("Battery Value = " + batteryValuef.ToString() + "V");
                }
                if (mode == 8)
                {

                    if (saveData == true)
                    {
                        StringBuilder messageString = new StringBuilder();
                        messageString.Append(name.ToString()).Append(',');
                        foreach (int Data in message)
                        {
                            messageString.Append(Data).Append(',');
                        }
                        try
                        {
                            DataReady(messageString.ToString(), 0);
                        }

                        catch { }
                    }

                    else
                    {
                        if (message.Count <= 8)
                        {
                            while (message.Count != 16)
                            {
                                message.Add(0);
                            }
                            if (!fullMMG) { MMGdatain(message, 0, true); }
                        }
                        else
                        {
                            while (message.Count != 24)
                            {
                                message.Add(0);
                            }
                            float W = (float)((message[0] * 256) + message[1]);
                            if (W > 16384) { W -= 65536; }
                            float X = (float)((message[2] * 256) + message[3]);
                            if (X > 16384) { X -= 65536; }
                            float Y = (float)((message[4] * 256) + message[5]);
                            if (Y > 16384) { Y -= 65536; }
                            float Z = (float)((message[6] * 256) + message[7]);
                            if (Z > 16384) { Z -= 65536; }


                            quat.W = W;
                            quat.X = X;
                            quat.Y = Y;
                            quat.Z = Z;
                            Form5.Form2.setValue(Quaternion.Invert(quat));
                            if (!fullMMG) { MMGdatain(message.GetRange(8, 16), 0, true); }
                            //processData(message.GetRange(0, 18), 2, false);
                        }

                    }


                    return;







                }


             }
        }
        void MMGdatainShort(List<int> message, int GyroValue)
        {
            int[] testval = new int[8];
            if (slow < -10) { slow = 10; }
            float val1 = (message[0] * 256) + message[1];
            testval[0] = (int)val1;
            val1 = (val1 * 3.3f) / 1024;
            slow--;
            if (slow == 0)
            {
                try
                {
                    Form4.addValue(val1);
                    slow = 10;
                }
                catch (Exception)
                {
                    sendData("M0dg");
                }
            }
        }
        public void SaveLog(string path)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            if (File.Exists(path))
            {

            }
            else
            {
                System.IO.File.WriteAllText(path, string.Empty);
            }
            StreamWriter file = new System.IO.StreamWriter(path,true);
            for (int i = 0; i < Memory.Count; i++)
            {
                file.WriteLine(Memory[i]);
            }
            file.Close();
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
        }
        public static void Swap<T>(ref T GX, ref T GY, ref T GZ, ref T AX, ref T AY, ref T AZ, ref T MX, ref T MY, ref T MZ)
        {
            /*T temp = GX;
            GX = GY;
            GY = temp;

            temp = AX;
            AX = AY;
            AY = temp;

            temp = MX;
            MX = MY;
            MY = temp;*/
        }
        public static void Swap<T>( ref T AX, ref T AY, ref T AZ, ref T MX, ref T MY, ref T MZ)
        {

            T temp = AX;
            AX = AY;
            AY = temp;

            temp = MX;
            MX = MY;
            MY = temp;
        }
       
        public void createfakeIMUdata()
        {
            try { Form1 = new Form1(this); }
            catch (Exception)
            {
                Form1 = new Form1(this);
            }
            Form1.OpenForms();
            Form1.Show();
            comptest = true;
            

        }

    public int processData(List<int> message, int mode, bool docked, int currentPacketNumber)
        {
            if (cali == null)
            {
                sendData("M0dg");
                return 0;
            }
            // accel_data is array of the data from the accelerometer in the following format: G_X, G_Y, G_Z, M_X, M_Y, M_Z, A_X, A_Y, A_Z
            short[] accel_data_short = new short[]{
            (short)sw_Maths.int32toint16(message[0] + ((message[1]) << 8)),
            (short)sw_Maths.int32toint16(message[2] + ((message[3]) << 8)),
            (short)sw_Maths.int32toint16((message[4] + ((message[5]) << 8))),
            (short)sw_Maths.int32toint16(message[6] + ((message[7]) << 8)),
            (short)sw_Maths.int32toint16(message[8] + ((message[9]) << 8)),
            (short)sw_Maths.int32toint16((message[10] + ((message[11]) << 8))),
            (short)sw_Maths.int32toint16(message[12] + ((message[13]) << 8)),
            (short)sw_Maths.int32toint16(message[14] + ((message[15]) << 8)),
            (short)sw_Maths.int32toint16((message[16] + ((message[17]) << 8)))
        };
            long GyroValue = 0; 
            if (message[1] != 128 && message[3] != 128 && message[5] != 128)
            {
                GyroValue = ((Math.Abs(accel_data_short[0]) + Math.Abs(accel_data_short[1]) + Math.Abs(accel_data_short[2])) / 3);
            }
             if (mode == 2)
            {
                AX = accel_data_short[6] - cali.AccelX;
                AY = accel_data_short[7] - cali.AccelY;
                AZ = accel_data_short[8] - cali.AccelZ;
                MX = (accel_data_short[3] - cali.MagX);
                MY = (accel_data_short[4] - cali.MagY);
                MZ = -(accel_data_short[5] - cali.MagZ);
                float packets = 1;
                if (currentPacketNumber != -1)
                {
                    packets = ((currentPacketNumber + 512) - (previousPacketNumber + 256)) % 256;
                }
                packets *= packetCorrection;
                previousPacketNumber = currentPacketNumber;
                if (!cali.calibrated)
                {
                    cali.caliCountDown--;
                    cali.gyroX += accel_data_short[0];
                    cali.gyroY += accel_data_short[1];
                    cali.gyroZ += accel_data_short[2];
                    if (firmwareVersion >= 10)
                    {
                        float a=0, b=0, c=0;
                         AX *= -1; AY *= 1; MX *= 1; MY *= 1; MZ *= -1;
                        Swap<float>(ref a,ref b, ref c, ref AX, ref AY, ref AZ, ref MX, ref MY, ref MZ);
                    }
                    //quatSix = ahrs.Update6DoF(0, 0, 0, AX, AY, AZ,quatSix, 0.1f); // longer sample period to speed up convergence during calibration
                    //quatNine = ahrs.Update9DoF(0, 0, 0, AX, AY, AZ, MX, MY, MZ, quat, 0.1f);
                    //MAquatNine = ahrs.MA_Update9DoF(0, 0, 0, AX, AY, AZ, MX, MY, MZ, MAquatNine, 0.1f);
                    //SWquatNine = ahrs.SW_Update9DoF(0, 0, 0, AX, AY, AZ, MX, MY, MZ, SWquatNine, 0.1f);
                    //quat = SWquatNine;
                    //quat = ahrs.MA_Update9DoF(0, 0, 0, AX, AY, AZ, MX, MY, MZ, quat, 0.1f);
                    //quat = ahrs.Update6DoF(0, 0, 0, AX, AY, AZ, quat, 0.1f);
                    for (int i = 0; i < cali.caliCountDown; i++)
                    {
                        quat = ahrs.Update(0, 0, 0, AX, AY, AZ, MX, MY, MZ, quat, samplePeriod * packets, (int)Algorithms.MadgwickAlgorithm);
                        quatNew = ahrs.Update(0, 0, 0, AX, AY, AZ, MX, MY, MZ, quatNew, samplePeriod * packets, (int)Algorithms.MadgwickAlgorithm);
                    }
                    if (cali.caliCountDown == 0)
                    {
                        zeroQuat = quat;
                        cali.calibrated = true;
                        cali.gyroX /= 100;
                        cali.gyroY /= 100;
                        cali.gyroZ /= 100;
                        previousPacketNumber = currentPacketNumber;

                    }
                }
                else
                {
                    float GX = ((((float)(accel_data_short[0]) - (float)cali.gyroX) * 500f * (float)Math.PI) / (32758f * 180f));
                    float GY = ((((float)(accel_data_short[1]) - (float)cali.gyroY) * 500f * (float)Math.PI) / (32758f * 180f));
                    float GZ = ((((float)(accel_data_short[2]) - (float)cali.gyroZ) * 500f * (float)Math.PI) / (32758f * 180f));
                    if (firmwareVersion >= 10)
                    {
                        GX *= -1; GY *= 1; AX *= -1; AY *= 1; MX *= 1; MY *=1; MZ *= -1;
                        Swap<float>(ref GX, ref GY, ref GZ, ref AX, ref AY, ref AZ, ref MX, ref MY, ref MZ);
                    }


                     //   quat = ahrs.Update(0, 0, 0, AX, AY, AZ, 1, 0, 0, quat, samplePeriod * packets, (int)Algorithms.SebX_Update9DoF);
                        quatNew = ahrs.Update(GX, GY, GZ, AX, AY, AZ, MX, MY, MZ, quatNew, samplePeriod * packets, (int)Algorithms.SebX_Update9DoF);
                    Vector3 acceleration = quatNew.Inverted() * new Vector3(0,0, 1);
                    Vector3 magnetometer = quatNew.Inverted() * new Vector3(10000, 0,0);
                    if(AlgorithmMode==2)
                    {
                        quat = ahrs.Update(GX, GY, GZ, acceleration.X, acceleration.Y, acceleration.Z, FakeMag.X, FakeMag.Y, FakeMag.Z, quat, samplePeriod * packets, (int)Algorithms.SebX_Update9DoF);
                        int break1 = 0;
                    }
                    else
                    {
                        quat = ahrs.Update(GX, GY, GZ, acceleration.X, acceleration.Y, acceleration.Z, MX, MY, MZ, quat, samplePeriod * packets, (int)Algorithms.SebX_Update9DoF);
                    }
                    /*Vector3 HYPR = QuaternionExtensions.YawPitchRollFromQuaternion(quatNew.Inverted());
                    Quaternion correction = Quaternion.Identity;

                    if (HYPR.Z < ((float)((Math.PI / 180)* - 40)))
                    {
                        correction = Quaternion.FromAxisAngle((quatNew.Inverted() * new Vector3(0, 0, -1)), (float)((Math.PI / 180) * (-40)) - HYPR.Z);
                    }
                    if (HYPR.Z > ((float)((Math.PI / 180) * 90)))
                    {
                        correction = Quaternion.FromAxisAngle((quatNew.Inverted() * new Vector3(0, 0, -1)), (float)((Math.PI / 180) * (90)) - HYPR.Z);
                    }
                    quatNew = quatNew * correction;
//                      Debug.WriteLine((HYPR.X * (180 / (float)Math.PI)).ToString() + ","+(HYPR.Y * (180 / (float)Math.PI)).ToString() + ","+(HYPR.Z * (180 / (float)Math.PI)).ToString());
*/

                }

                if (comp)
                {
                    Form1.Form2a.setValue( SebXquatNine);
                    //Form1.Form2a.setValue(quatNine);
                    Form1.Form2a.setAccelerometerValue(AX, AY, AZ);
                    Form1.Form2a.setMagnetometerValue(MX, MY, MZ);
                    //Form1.Form2b.setValue(new Quaternion(1, 0, 0, 0) * MAquatNine);
                    Form1.Form2b.setValue(new Quaternion(1, 0, 0, 0) * quat);
                    Form1.Form2b.setAccelerometerValue(AX, AY, AZ);
                    Form1.Form2b.setMagnetometerValue(MX, MY, MZ);
                    return (int)GyroValue;
                }

                if (comptest)
                {
                    Form1.Form2a.setValue(new Quaternion(1, 0, 0, 0) * quat);
                    //Form1.Form2a.setValue(quatNine);
                    Form1.Form2a.setAccelerometerValue(AX, AY, AZ);
                    Form1.Form2a.setMagnetometerValue(MX, MY, MZ);
                    //Form1.Form2b.setValue(new Quaternion(1, 0, 0, 0) * MAquatNine);
                    Form1.Form2b.setValue(new Quaternion(1, 0, 0, 0) * Quaternion.Invert(Quaternion.Invert(quat)*new Quaternion(0,0,1,1)));
                    Form1.Form2b.setAccelerometerValue(AX, AY, AZ);
                    Form1.Form2b.setMagnetometerValue(MX, MY, MZ);
                    return (int)GyroValue;
                }

                if (VisOff)
                {
                    return (int)GyroValue;
                }
                if (Helicopter)
                {


                    try
                    {
                        if (cali.calibrated == true)
                        {
                            Quaternion q1 = Quaternion.Invert(quat);
                            Vector3 axis; float angle;
                            q1.ToAxisAngle(out axis, out angle);
                            q1 = quat;
                            float yaw = (float)Math.Atan2(2 * (q1.W * q1.Z + q1.X * q1.Y), 1 - 2 * (q1.Y * q1.Y + q1.Z * q1.Z));


                            float pitch = (float)Math.Asin(-2 * (q1.X * q1.Z - q1.W * q1.Y));
                            float roll = ((float)Math.Atan2(2 * (q1.Y * q1.Z + q1.W * q1.X), ((q1.W * q1.W) - (q1.X * q1.X) - (q1.Y * q1.Y) + (q1.Z * q1.Z))));

                            

                            Vector3 zerovalue = new Vector3 ( 0, 1, 0 );
                            if ((Math.Abs(roll) > 2.75f)  &&((DateTime.Now - trigger).TotalSeconds>5))
                            {
                                trigger = DateTime.Now;
                                Process q;
                                q = new Process();
                                q.StartInfo.FileName = Path.GetFullPath("../../Tools/AutoItScripts/Enter.exe");
                                q.Start();
                            }
                           // Form5.WriteText(roll + "\n" + pitch + "\n" + yaw);

                            helicopterData(new float[] { roll, pitch, yaw }, true);
                        }
                    }
                    catch (Exception e)
                    {
                        if (e.HResult == -2147467261) { }
                    }

                }
                if (Kat && cali.calibrated)  ///FIND
                {
                    Quaternion quat5 = Quaternion.Invert(quat*new Quaternion(0,-1,0,1)) * (zeroQuat * new Quaternion(0, -1, 0, 1));
                    quat5 = Quaternion.Invert(quat5);
                    Quaternion current = new Quaternion(0, 0, -1, -1);
                    Quaternion result = Quaternion.Normalize(quat5 * current);

                    byte W = (byte)((result.W * 125) + 125);
                    byte X = (byte)((result.X * 125) + 125);
                    byte Y = (byte)((result.Y * 125) + 125);
                    byte Z = (byte)((result.Z * 125) + 125);
                    //Z = Z * (byte)-1;
                    byte M = 0;
                    unitydata = new byte[] { 255, (byte)(address), X, Y, Z, W, M, (byte)(address) };
                }
                if (docked)
                {
                    if (cali.calibrated == true && Form5 != null && Form5.Form2 !=null)
                    {

                        Form5.Form2.setValue(new Quaternion(1, 0, 0, 0) * quatNew);
                        Form5.Form2.setAccelerometerValue(AX, AY, AZ);
                        Form5.Form2.setMagnetometerValue(MX, MY, MZ);
                    }
                    return (int)GyroValue;
                }
                else
                {
                    if (cali != null && Form2 != null)
                    {

                        Form2.setValue(new Quaternion(1, 0, 0, 0) * quat);
                        Form2.setAccelerometerValue(AX, AY, AZ);
                        Form2.setMagnetometerValue(MX, MY, MZ);
                    }
                }
            }
            if (mode == 3)
            {
                float GX = ((((float)accel_data_short[0] - (float)cali.gyroX) * 500f * (float)Math.PI) / (32758f * 180f));
                float GY = ((((float)accel_data_short[1] - (float)cali.gyroY) * 500f * (float)Math.PI) / (32758f * 180f));
                float GZ = ((((float)accel_data_short[2] - (float)cali.gyroZ) * 500f * (float)Math.PI) / (32758f * 180f));
                AX = (int)accel_data_short[6] - cali.AccelX;
                AY = (int)accel_data_short[7] - cali.AccelY;
                AZ = accel_data_short[8] - cali.AccelZ;
                MX = ((float)accel_data_short[3] - (float)cali.MagX);
                MY = ((float)accel_data_short[4] - (float)cali.MagY);
                MZ = -((float)accel_data_short[5] - (float)cali.MagZ);


                quat = ahrs.Update9DoF(GX, GY, GZ, AX, AY, AZ, MX, MY, MZ,quat,samplePeriod);
                Vector3 mag = new Vector3(accel_data_short[3], accel_data_short[4], accel_data_short[5]);
                Vector3 accel = new Vector3(accel_data_short[6], accel_data_short[7], accel_data_short[8]);
                //mag = Vector3.Normalize(mag);
                accel = Vector3.Normalize(accel);
                float angle = (float)MathExtensions.Deg2Rad*(Vector3.Dot(Vector3.Normalize(mag), Vector3.Normalize(accel)));
                byte[] byteArrayW = BitConverter.GetBytes(quat.W);
                byte[] byteArrayX = BitConverter.GetBytes(quat.X);
                byte[] byteArrayY = BitConverter.GetBytes(quat.Y);
                byte[] byteArrayZ = BitConverter.GetBytes(quat.Z);
                byte[] byteArrayMX = BitConverter.GetBytes(MX);
                byte[] byteArrayMY = BitConverter.GetBytes(MY);
                byte[] byteArrayMZ = BitConverter.GetBytes(MZ);

                WriteDataToBox(string.Join(",", accel_data_short.ToArray()));
                WriteDataToBox("\n");
                DataReady(new byte[] { 6,
                   (byte)message[0],
                   (byte)message[1],
                   (byte)message[2],
                   (byte)message[3],
                   (byte)message[4],
                   (byte)message[5],
                    (byte)message[6],
                    (byte)message[7],
                    (byte)message[8],
                    (byte)message[9],
                    (byte)message[10],
                    (byte)message[11],
                    //(byte)byteArrayMX[0],
                    //(byte)byteArrayMX[1],
                    //(byte)byteArrayMY[0],
                    //(byte)byteArrayMY[1],
                    //(byte)byteArrayMZ[0],
                    //(byte)byteArrayMZ[1],

                    (byte)message[12],
                   (byte)message[13],
                   (byte)message[14],
                   (byte)message[15],
                   (byte)message[16],
                   (byte)message[17],
                    byteArrayW[0],
                    byteArrayW[1],
                    byteArrayW[2],
                    byteArrayW[3],
                    byteArrayX[0],
                    byteArrayX[1],
                    byteArrayX[2],
                    byteArrayX[3],
                    byteArrayY[0],
                    byteArrayY[1],
                    byteArrayY[2],
                    byteArrayY[3],
                    byteArrayZ[0],
                    byteArrayZ[1],
                    byteArrayZ[2],
                    byteArrayZ[3],

                });
            }
            return (int)GyroValue;
        }

        public void RotationLimits(Quaternion LocalReference, Vector3 reference, float minangle, float maxangle, out bool exceeded, out Quaternion correction)
        {
            float angle;
            Vector3 rotresult = LocalReference * reference;
            correction = Quaternion.Identity;
            exceeded = false;
            Vector2 XZRef = new Vector2(rotresult.X, rotresult.Z);
            if (XZRef.Length < 0.2)
            {
                return;
            }
            Vector2 straightOut = new Vector2(0, 1);
            angle = angleBetweenVector2D(XZRef, straightOut);
            angle = angle * (XZRef.X / Math.Abs(XZRef.X));
            if (angle < -40)
            {
                correction = Quaternion.FromAxisAngle((LocalReference.Inverted() * new Vector3(0, 1, 0)), (float)((Math.PI / 180) * (-40 - angle)));
                exceeded = true;
            }
            if (angle > 90)
            {
                correction = Quaternion.FromAxisAngle((LocalReference.Inverted() * new Vector3(0, 1, 0)), (float)((Math.PI / 180) * (90 - angle)));
                exceeded = true;
            }
        }
        public float angleBetweenVector2D(Vector2 v1, Vector2 v2)
        {
            return (float)((180 / Math.PI) * (float)Math.Acos(Vector2.Dot(v1, v2) / (v1.Length * v2.Length)));
        }
        public float angleBetweenVector3D(Vector3 v1, Vector3 v2)
        {
            return (float)((180 / Math.PI) * (float)Math.Acos(Vector3.Dot(v1, v2) / (v1.Length * v2.Length)));
        }
        public void loadCalifiles(string name, string _CurrentDirectory)
        {
            cali = new Calibration(name, _CurrentDirectory, 0, 0, 0, 0, 0, 0, 0, 0, 0, true);
        }

    }
    /// <summary>
    /// Class Containing calibration variables
    /// </summary>

    public class Calibration
    {
        public bool calibrated = false;
        public int caliCountDown = 200;
        public int gyroX = 0;
        public int gyroY = 0;
        public int gyroZ = 0;
        public int AccelX = 0;
        public int AccelY = 0;
        public int AccelZ = 0;
        public int MagX = 0;
        public int MagY = 0;
        public int MagZ = 0;
        private string name;
        private string path;
        SW_Maths sw_maths = new SW_Maths();
        public Calibration(string _name, string _CurrentDirectory, int _gyroX, int _gyroY, int _gyroZ, int _AccelX, int _AccelY, int _AccelZ, int _MagX, int _MagY, int _MagZ, bool read)
        {
            if (read)
            {
                name = _name;
                path = _CurrentDirectory;
                if (path.Substring(path.Length - 10) == "\\bin\\Debug")
                {
                    path += "\\IMU_Memory\\MagCali\\" + name + ".txt";
                }
                else
                {
                    path += "\\bin\\Debug\\IMU_Memory\\MagCali\\" + name + ".txt";
                }

                if (File.Exists(path))
                {
                    string[] CalibrationValues = File.ReadAllLines(path);
                    float result;
                    /*                    float.TryParse(CalibrationValues[0], out result);
                                        gyroX = (int)Math.Round(result);
                                        float.TryParse(CalibrationValues[1], out result);
                                        gyroY = (int)Math.Round(result);
                                        float.TryParse(CalibrationValues[2], out result);
                                        gyroZ = (int)Math.Round(result);*/
                    float.TryParse(CalibrationValues[3], out result);
                    AccelX = (int)Math.Round(result);
                    float.TryParse(CalibrationValues[4], out result);
                    AccelY = (int)Math.Round(result);
                    float.TryParse(CalibrationValues[5], out result);
                    AccelZ = (int)Math.Round(result);
                    float.TryParse(CalibrationValues[6], out result);
                    MagX = (int)Math.Round(result);
                    float.TryParse(CalibrationValues[7], out result);
                    MagY = (int)Math.Round(result);
                    float.TryParse(CalibrationValues[8], out result);
                    MagZ = (int)Math.Round(result);
                }


                return;
            }




            gyroX = _gyroX;
            gyroY = _gyroY;
            gyroZ = _gyroZ;
            AccelX = _AccelX;
            AccelY = _AccelY;
            AccelZ = _AccelZ;
            MagX = _MagX;
            MagY = _MagY;
            MagZ = _MagZ;

            name = _name;
            path = _CurrentDirectory;
            if (path.Substring(path.Length - 10) == "\\bin\\Debug")
            {
                path += "\\IMU_Memory\\MagCali\\" + name + ".txt";
            }
            else
            {
                path += "\\bin\\Debug\\IMU_Memory\\MagCali\\" + name + ".txt";
            }
            if (!File.Exists(path))
            {
                File.WriteAllLines(path, new string[] { "0\n", "0\n", "0\n", "0\n", "0\n", "0\n", "0\n", "0\n", "0\n" });
            }
            if (Math.Abs(AccelX) + Math.Abs(AccelY) + Math.Abs(AccelZ) +
                Math.Abs(MagX) + Math.Abs(MagY) + Math.Abs(MagZ) == 0)
            {
                int AccelXsaved = 0;
                int AccelYsaved = 0;
                int AccelZsaved = 0;
                int MagXsaved = 0;
                int MagYsaved = 0;
                int MagZsaved = 0;
                string[] CalibrationValues = File.ReadAllLines(path);
                float result;
                float.TryParse(CalibrationValues[3], out result);
                AccelXsaved = (int)Math.Round(result);
                float.TryParse(CalibrationValues[4], out result);
                AccelYsaved = (int)Math.Round(result);
                float.TryParse(CalibrationValues[5], out result);
                AccelZsaved = (int)Math.Round(result);
                float.TryParse(CalibrationValues[6], out result);
                MagXsaved = (int)Math.Round(result);
                float.TryParse(CalibrationValues[7], out result);
                MagYsaved = (int)Math.Round(result);
                float.TryParse(CalibrationValues[8], out result);
                MagZsaved = (int)Math.Round(result);

                if (Math.Abs(AccelXsaved) + Math.Abs(AccelYsaved) + Math.Abs(AccelZsaved) +
               Math.Abs(MagXsaved) + Math.Abs(MagYsaved) + Math.Abs(MagZsaved) != 0)
                {

                    //MessageBox.Show("This IMU backs up its calibration values whenever they are changed. It appears that you are trying to write Zero values to a calibration file that previously contained valid calibration information. This usually occurs because the IMU onboard calibration has been overwritten accidently. If this was not an accident and you intended to overwrite the calibration file, please correct the file manually. Otherwise, please ensure that the IMU has the expected calibration values using the 'editreg' function", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }


            }

            if (File.Exists(path))
            {
                File.Delete(path);
                string[] data = {gyroX.ToString(),
                gyroY.ToString(),
                gyroZ.ToString(),
                AccelX.ToString(),
                AccelY.ToString(),
                AccelZ.ToString(),
                MagX.ToString(),
                MagY.ToString(),
                MagZ.ToString()};
                File.WriteAllLines(path, data);
            }
            else
            {
                string[] data = {gyroX.ToString(),
                gyroY.ToString(),
                gyroZ.ToString(),
                AccelX.ToString(),
                AccelY.ToString(),
                AccelZ.ToString(),
                MagX.ToString(),
                MagY.ToString(),
                MagZ.ToString()};
                File.WriteAllLines(path, data);
            }
        }
        public bool setData(List<int> caliData)
        {
            if (caliData.Max() != 0)
            {
                MagX = sw_maths.int32toint16(caliData[1] + ((caliData[0]) << 8));
                MagY = sw_maths.int32toint16(caliData[3] + ((caliData[2]) << 8));
                MagZ = sw_maths.int32toint16(caliData[5] + ((caliData[4]) << 8));
                string[] CalibrationValues = File.ReadAllLines(path);
                if (CalibrationValues.Count() == 10)
                {
                    int.TryParse(CalibrationValues[6], out MagX);
                    int.TryParse(CalibrationValues[7], out MagY);
                    int.TryParse(CalibrationValues[8], out MagZ);
                    string[] data = {gyroX.ToString(),
                gyroY.ToString(),
                gyroZ.ToString(),
                AccelX.ToString(),
                AccelY.ToString(),
                AccelZ.ToString(),
                MagX.ToString(),
                MagY.ToString(),
                MagZ.ToString()};
                    File.WriteAllLines((path), data);
                    return true;

                }
                else
                {
                    string[] data = {gyroX.ToString(),
                gyroY.ToString(),
                gyroZ.ToString(),
                AccelX.ToString(),
                AccelY.ToString(),
                AccelZ.ToString(),
                MagX.ToString(),
                MagY.ToString(),
                MagZ.ToString()};
                    File.WriteAllLines((path), data);
                    return false;
                }

            }
            return false;
        }
        public void WriteToCaliFile(List<int> Data)
        {
            gyroX = Data[0];
            gyroY = Data[1];
            gyroZ = Data[2];
            AccelX = Data[3];
            AccelY = Data[4];
            AccelZ = Data[5];
            MagX = Data[6];
            MagY = Data[7];
            MagZ = Data[8];
            {
                string[] data = {gyroX.ToString(),
                gyroY.ToString(),
                gyroZ.ToString(),
                AccelX.ToString(),
                AccelY.ToString(),
                AccelZ.ToString(),
                MagX.ToString(),
                MagY.ToString(),
                MagZ.ToString()};
                File.WriteAllLines((path), data);
            }
        }
    }
    
    public class WorkerClass
    {

        public MMGProcessorClass MMGProcessor = new MMGProcessorClass();
        private ManualResetEvent _doneEvent;
        public int value;
        public float saveValue;
        public WorkerClass(ManualResetEvent doneEvent, int _value, List<int> Data, List<double> Memory, int Current, int threshold)
        {
            MMGProcessor.initialise(500, 4, threshold, 1f, 40, 1, Data, Memory, Current);
            value = _value;
            _doneEvent = doneEvent;
        }
        public void ThreadPoolCallback(Object threadContext)
        {
            bool trigger = MMGProcessor.AddData(value);
            if (trigger)
            {
                trigger = !trigger;
            }
            if (MMGProcessor.val != 0 && threadContext.Equals(3))
            {

                saveValue = (float)MMGProcessor.val;
            }
            _doneEvent.Set();

        }

    }
    public static class LoggingExtensions
    {
        static ReaderWriterLock locker = new ReaderWriterLock();
        static string filename = "../../DataFiles/"+DateTime.Now.ToString("yyyyMMddHHmmss") +".csv";
        public static void WriteDebug(this string text)
        {
            try
            {
                locker.AcquireWriterLock(int.MaxValue); //You might wanna change timeout value 
                System.IO.File.AppendAllLines(Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Replace("file:\\", ""), filename), new[] { text });
            }
            finally
            {
                locker.ReleaseWriterLock();
            }
        }
    }

    public class FilterData
    {

        IIRFilter Lowpass10;
        IIRFilter Highpass10;
        IIRFilter Lowpass100;

        public float output_lowpass;
        public float output_highpass;

        public FilterData()
        {
            Start();
        }
        // Use this for initialization
        public void Start()
        {
            Lowpass10 = new IIRFilter(50, 1);
            Lowpass100 = new IIRFilter(50, 25);


            float Gain = (float)0.91496914411308261083632942245458252728;
            float b1 = (float)1.822694925196308046722037943254690617323;
            float b2 = (float)-0.83718165125602250764558220907929353416;

            Highpass10 = new IIRFilter(Gain, -2 * Gain, Gain, b1, b2);
        }

        // Update is called once per frame
        public void Update(float RawData)
        {
            float input = RawData;
            float output_lowpass100;

            output_lowpass100 = filter(input, Lowpass100);
            output_highpass = filter(input, Highpass10);

            output_lowpass = filter(Math.Abs(output_highpass), Lowpass10);
        }

        public class IIRFilter
        {
            public float a0;
            public float a1;
            public float a2;
            public float b1;
            public float b2;

            public float x1;
            public float x2;
            public float y1;
            public float y2;

            // three parameters indicates a notch filter
            // equation obtained here http://dspguide.com/ch19/3.htm
            public IIRFilter(float samplingrate, float frequency, float Bandwidth)
            {
                float Pi = 3.141592f;
                float BW = Bandwidth / samplingrate;
                float f = frequency / samplingrate;
                float R = 1 - 3 * BW;
                float K = (float)((1 - 2 * R * Math.Cos(2 * Pi * f) + R * R) / (2 - 2 * Math.Cos(2 * Pi * f)));
                a0 = K;
                a1 = (float)(-2 * K * Math.Cos(2 * Pi * f));
                a2 = K;
                b1 = (float)(2 * R * Math.Cos(2 * Pi * f));
                b2 = -R * R;

                x1 = x2 = y1 = y2 = 0;
            }

            // two parameters indicates a 2nd order Butterworth low-pass filter
            // equation obtained here: https://www.codeproject.com/Tips/1092012/A-Butterworth-Filter-in-Csharp
            public IIRFilter(float samplingrate, float frequency)
            {
                const float pi = 3.14159265358979f;
                float wc = (float)(Math.Tan(frequency * pi / samplingrate));
                float k1 = 1.414213562f * wc;
                float k2 = wc * wc;
                a0 = k2 / (1 + k1 + k2);
                a1 = 2 * a0;
                a2 = a0;
                float k3 = a1 / k2;
                b1 = -2 * a0 + k3;
                b2 = 1 - (2 * a0) - k3;

                x1 = x2 = y1 = y2 = 0;
            }

            // for Butterworth high-pass filters or other IIR filters, you need to insert parameters manually.
            // Easy way to find these parameters is using R's "signal" package butter function, and convert parameters like this: 
            // a0 = $b[0]; a1 = $b[1]; a2 = $b[2]; b1 = -$a[1]; b2 = -$a[2]
            public IIRFilter(float a0in, float a1in, float a2in, float b1in, float b2in)
            {
                a0 = a0in;
                a1 = a1in;
                a2 = a2in;
                b1 = b1in;
                b2 = b2in;

                x1 = x2 = y1 = y2 = 0;
            }
        }

        // filter data. Each IIRFilter stores two data points of filtered and unfiltered data. Therefore, filtering should be continuous and not be switched on and off. 
        // Furthermore, each IIRFilter may only process one data stream. If you intend to filter two data streams with the same kind of filter, you need to initialize 
        // two IIRFilters accordingly (e.g. "Notch50_1" and "Notch50_2"), each filtering only one data stream. 
        public float filter(float x0, IIRFilter iirfilter)
        {
            float y = iirfilter.a0 * x0 + iirfilter.a1 * iirfilter.x1 + iirfilter.a2 * iirfilter.x2 + iirfilter.b1 * iirfilter.y1 + iirfilter.b2 * iirfilter.y2;

            iirfilter.x2 = iirfilter.x1;
            iirfilter.x1 = x0;
            iirfilter.y2 = iirfilter.y1;
            iirfilter.y1 = y;

            return y;
        }
    }

}