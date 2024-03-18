using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Drawing2D;
using AForge;
using AForge.Math;
using System.Windows.Forms;
using MathNet.Numerics.Statistics;
using System.Diagnostics;

using System.Data;
using System.Data.SQLite;
using MLApp;

namespace MMGProcessor
{
    public class MMG
    {
        RollingBuffer MMG_Data;
        RollingBufferf Processed_MMG_Data;
        List<GestureTemplate> Gestures = new List<GestureTemplate>() { };
        bool bufferfull = false;
        public int previous_gesture = -1;
        System.IO.StreamWriter file;
        int WindowLength = 200;
        public bool Training_mode = true;
        public double[] data;
        public int threshold = 50;
        List<double> correlation_Values = new List<double>() { };
        double[] xp = new double[] { 0, 0, 0, 0, 0, 0, 0, 0 };
        double[] zp = new double[] { 0, 0, 0, 0, 0, 0, 0, 0 };
        private double[] a2 = new double[] { 1f, -0.9404f };
        private double[] b2 = new double[] { 0.9702f, -0.9702f };
        private double[] a = new double[] { 1f, -0.7322f };
        private double[] b = new double[] { 0.1339f, 0.1339f };
        public int gestureClock = 0;
        int gestureLength = 0;
        public int currentGesture = 0;
        bool gesture = false;
        double corsum;
        public double sum;
        List<double[]> trainingdata = new List<double[]>() { };
        List<OpenTK.Vector3> trainingdataPositions = new List<OpenTK.Vector3>() { };
        MLApp.MLApp matlab = new MLApp.MLApp();
        public bool store = false;
        public List<double[]> fullRawDataArchive = new List<double[]>() { };
        public int MMGClassifierMode = 0;
        public OpenTK.Vector3 end_position = new OpenTK.Vector3(0, 0, 0);
        public List<KNNTrainingData> knnTrainingData = new List<KNNTrainingData>() { };
        public int UID = 0;
        public int delay_between_gestures = 500;
        public MMG()
        {
            data = new double[WindowLength];
            file = new System.IO.StreamWriter(System.IO.Directory.GetCurrentDirectory() + "..\\..\\..\\MMGDebug.csv");
            MMG_Data = new RollingBuffer(2);
            Processed_MMG_Data = new RollingBufferf(WindowLength);
        }



        public void ReadGestureFromDatabase(string username, int _GestureNo)
        {
            List<string> ImportedFiles = new List<string>();
            string Gesture = "";
            byte[] hold = { };
            int counter = 0;
            double[][] DatabaseData = new double[6][];
            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=../../DataFiles/MMG_Database.sqlite;Version=3;");
            m_dbConnection.Open();
            using (SQLiteCommand fmd = m_dbConnection.CreateCommand())
            {
                fmd.CommandText = @"SELECT user, gesture, count, date, channel1, channel2, channel3, channel4, channel5, channel6 FROM Gestures WHERE gesture = '" + returnGestureNames(_GestureNo) + "' ORDER BY date DESC";
                fmd.CommandType = CommandType.Text;
                SQLiteDataReader r = fmd.ExecuteReader();
                r.Read();
                Gesture = Convert.ToString(r["gesture"]);
                counter = Convert.ToInt32(r["count"]);
                DatabaseData[0] = StringToDouble(Convert.ToString(r["channel1"]));
                DatabaseData[1] = StringToDouble(Convert.ToString(r["channel2"]));
                DatabaseData[2] = StringToDouble(Convert.ToString(r["channel3"]));
                DatabaseData[3] = StringToDouble(Convert.ToString(r["channel4"]));
                DatabaseData[4] = StringToDouble(Convert.ToString(r["channel5"]));
                DatabaseData[5] = StringToDouble(Convert.ToString(r["channel6"]));
                while (r.Read()) ;
                Gestures[_GestureNo].updateFullGestureArray(DatabaseData, counter);
            }
        }
        double[] StringToDouble(string dataString)
        {
            double[] returnarray = new double[dataString.Length / 32];
            int returnarrayAddress = 0;
            string line = "";
            int n = 0;
            byte[] holdarray = { 0, 0, 0, 0, 0, 0, 0, 0 };
            foreach (char character in dataString)
            {
                if (character == ',' || character == ';')
                {
                    if (character == ',')
                    {
                        byte.TryParse(line, out holdarray[n++]);
                        line = "";
                    }
                    else
                    {
                        byte.TryParse(line, out holdarray[n++]);
                        n = 0; line = "";
                        returnarray[returnarrayAddress++] = BitConverter.ToDouble(holdarray, 0);
                    }
                }
                else
                {
                    line += character;
                }
            }
            return returnarray;
        }
        public void SaveGestureToDatabase(string username, int _GestureNo)
        {
            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=../../DataFiles/MMG_Database.sqlite;Version=3;");
            m_dbConnection.Open();
            string sql = "insert into Gestures (" +
                "user, gesture,count,date," +
                "channel1,channel2,channel3,channel4,channel5,channel6)" +
                "values ('" +
                username + "','" +
                returnGestureNames(_GestureNo) + "','" +
                Gestures[_GestureNo].currentGestureCount + "','" +
                System.DateTime.Now.ToString("yyyyMMddHHmmss") + "','" +
                Gestures[_GestureNo].returnGestureChannelToString(0) + "','" +
                Gestures[_GestureNo].returnGestureChannelToString(1) + "','" +
                Gestures[_GestureNo].returnGestureChannelToString(2) + "','" +
                Gestures[_GestureNo].returnGestureChannelToString(3) + "','" +
                Gestures[_GestureNo].returnGestureChannelToString(4) + "','" +
                Gestures[_GestureNo].returnGestureChannelToString(5) + "')";

            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            m_dbConnection.Close();
        }
        public List<string> returnGestureNames()
        {
            List<string> GestureNames = new List<string>() { };
            foreach (GestureTemplate GestureInstance in Gestures)
            {
                GestureNames.Add(GestureInstance.description);
            }
            return GestureNames;
        }
        public string returnGestureNames(int gestureNo)
        {
            List<string> GestureNames = new List<string>() { };
            foreach (GestureTemplate GestureInstance in Gestures)
            {
                GestureNames.Add(GestureInstance.description);
            }
            return GestureNames[gestureNo];
        }
        public void addGesture(string _description)
        {
            Gestures.Add(new GestureTemplate(_description));
            correlation_Values.Add(correlation_Values.Count + 1);
        }
        private static bool isZero(int i)
        {
            return (i == 0);
        }

        public int addData(int[] _data, OpenTK.Vector3 _endPostion, int GestureTestValue, bool GyroLimitExceeded)
        {
            end_position = _endPostion;
            previous_gesture = -1;
            MMG_Data.add(_data);
            List<int> test = MMG_Data.getChannel(1);
            if (MMG_Data.full)
            {
                Processed_MMG_Data.add(new double[] {
                processData(MMG_Data.getprev(0), MMG_Data.getcurrent(0),0),
                processData(MMG_Data.getprev(1), MMG_Data.getcurrent(1),1),
                processData(MMG_Data.getprev(2), MMG_Data.getcurrent(2),2),
                processData(MMG_Data.getprev(3), MMG_Data.getcurrent(3),3),
                processData(MMG_Data.getprev(4), MMG_Data.getcurrent(4),4),
                processData(MMG_Data.getprev(5), MMG_Data.getcurrent(5),5),
                processData(MMG_Data.getprev(6), MMG_Data.getcurrent(6),6),
                processData(MMG_Data.getprev(7), MMG_Data.getcurrent(7),7),
            });

                if (Processed_MMG_Data.full)
                {
                    sum =
                        Math.Abs(Processed_MMG_Data.getcurrent(0)) + Math.Abs(Processed_MMG_Data.getcurrent(1)) +
                        Math.Abs(Processed_MMG_Data.getcurrent(2)) + Math.Abs(Processed_MMG_Data.getcurrent(3)) +
                        Math.Abs(Processed_MMG_Data.getcurrent(4)) + Math.Abs(Processed_MMG_Data.getcurrent(5));
                    sum /= 6;
                    if (sum > threshold)
                    {
                        if (gestureClock > 500)
                        {
                            gesture = true;
                        }
                        gestureClock = 0;
                    }
                    else
                    {
                        gestureClock++;
                    }
                    int marker = 0;
                    if (gesture)
                    {
                        marker = -100;
                        gestureLength++;
                        if (gestureLength > (WindowLength - 50))
                        {
                            if (GyroLimitExceeded)
                            {
                                gestureLength = 0;
                                gesture = false;
                                return -1;
                            }
                            gestureLength = 0;
                            gesture = false;
                        
                            switch (MMGClassifierMode)
                            {
                                case 2:
                                    double[] line1 = new double[1208]; line1[0] = currentGesture; line1[1201] = end_position.X; line1[1202] = end_position.Y; line1[1203] = end_position.Z;
                                    Convert2Dto1D(Processed_MMG_Data.getDataArray(), 200, 6).CopyTo(line1, 1);
                                    Gestures[currentGesture].add(Processed_MMG_Data.returnData());

                                    List<KNNTrainingData> CurrentTrainingData = new List<KNNTrainingData>() { };
                                    List<KNNTrainingData> BackupTrainingData = new List<KNNTrainingData>(knnTrainingData);
                                    int number_of_training_instances = 40;
                                    int NumofGestures = 5;
                                    List<int> CurrentNumbers = new List<int>(); for (int i = 0; i < NumofGestures; i++) { CurrentNumbers.Add(0); }
                                    foreach (KNNTrainingData ktd in knnTrainingData)
                                    {
                                        CurrentNumbers[ktd.getClass()] = CurrentNumbers[ktd.getClass()] + 1;
                                    }
                                    CurrentNumbers.RemoveAll(isZero);
                                    if (CurrentNumbers.Count() > 2)
                                    {
                                        for (int Gesture = 0; Gesture < CurrentNumbers.Count(); Gesture++)
                                        {
                                            List<KNNTrainingData> LocalCurrentTrainingData = new List<KNNTrainingData>() { };
                                            while (LocalCurrentTrainingData.Count < number_of_training_instances && CurrentNumbers.Min() >= number_of_training_instances)
                                            {
                                                int Min = 0;
                                                double minDistance = 1000000;
                                                for (int i = 0; i < BackupTrainingData.Count; i++)
                                                {
                                                    if (minDistance > (BackupTrainingData[i].getGesturePosition() - end_position).Length && BackupTrainingData[i].getClass() == Gesture)
                                                    {
                                                        Min = i;
                                                        minDistance = (BackupTrainingData[i].getGesturePosition() - end_position).Length;
                                                    }
                                                }
                                                LocalCurrentTrainingData.Add(BackupTrainingData[Min]);
                                                BackupTrainingData.RemoveAt(Min);
                                            }
                                            CurrentTrainingData.AddRange(LocalCurrentTrainingData);
                                        }

                                        int[] UIDs = new int[CurrentTrainingData.Count + 1];
                                        UIDs[0] = UID;
                                        for (int UIDnumber = 0; UIDnumber < CurrentTrainingData.Count; UIDnumber++)
                                        {
                                            UIDs[UIDnumber + 1] = CurrentTrainingData[UIDnumber].getUID();
                                        }
                                        object result2 = null;
                                        object result3 = null;
                                        double[,] KNNFeatures = KNNListToArray(knnTrainingData);
                                        double[,] CurrentTrainingDataFeatures = KNNListToArray(CurrentTrainingData);

                                        matlab.Feval("SaveArray", 0, out result3, "UIDArchive", "UIDArchive" + UID + ".mat", UIDs);
                                        int numGest = CurrentNumbers.Count();
                                        bool not_enough_gestures = false;
                                        if (CurrentNumbers.Count == 1) { not_enough_gestures = true; }

                                        if (CurrentNumbers.Min() > 1 && !not_enough_gestures && CurrentTrainingData.Count >1)
                                        {
                                            try
                                            {
                                                matlab.Feval("CubicSVM_UntrainedFeaturesx2", 1, out result2, CurrentTrainingDataFeatures, KNNFeatures, (double)(numGest));
                                            }
                                            catch (Exception e)
                                            {
                                                e = null;
                                            }
                                            // Define the output 
                                            result2 = null;
                                            // Call the MATLAB function myfunc
                                            //   matlab.Feval("testClassifier", 1, out result, testFeatures);
                                        }
                                    }
                                    if (Training_mode)
                                    {
                                        previous_gesture = -2;
                                        matlab.Execute(@"cd " + System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\Tools\\");
                                        object resulttr = null;
                                        matlab.Feval("FeatureExtraction_Training", 1, out resulttr, Processed_MMG_Data.getDataArray(), (double)currentGesture);
                                        object[] restr = resulttr as object[];
                                        resulttr = null;
                                        knnTrainingData.Add(new KNNTrainingData(end_position, currentGesture, MATLAB2Dto1D((double[,])restr[0]), UID));
                                        line1[1204] = GestureTestValue; line1[1205] = UID;
                                    }
                                    else
                                    {
                                        matlab.Execute(@"cd " + System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\Tools\\");
                                        object result = null;
                                        matlab.Feval("FeatureExtraction_Test", 1, out result, Processed_MMG_Data.getDataArray());
                                        object[] res = result as object[];
                                        result = null;
                                        double[] testFeatures = (MATLAB2Dto1D((double[,])res[0]));
                                        result = null;
                                        matlab.Feval("testCSVMClassifierx2", 1, out result, testFeatures);
                                        res = result as object[];
                                        double[,] resarray = (double[,])res[0];
                                        try
                                        {
                                            previous_gesture = (int)Math.Round(resarray[0,0]);
                                        }
                                        catch (Exception e) {
                                            int a = 1;
                                        }
                                        line1[1204] = GestureTestValue; line1[1205] = UID; line1[1206] = resarray[0, 0]; line1[1207] = resarray[0, 1];
                                    }
                                    UID++;
                                    fullRawDataArchive.Add(line1);

                                    break;
                                case 1:
                                    previous_gesture = Classifier1(GestureTestValue);
                                    break;
                                case 0:
                                    previous_gesture = Classifier0();
                                    break;
                            }
                        }
                    }
                }
            }
            return previous_gesture;
        }

        int Classifier1(int GestureTestValue)
        {
            double[] line1 = new double[1206]; line1[0] = currentGesture; line1[1201] = end_position.X; line1[1202] = end_position.Y; line1[1203] = end_position.Z;
            Convert2Dto1D(Processed_MMG_Data.getDataArray(), 200, 6).CopyTo(line1, 1);
            Gestures[currentGesture].add(Processed_MMG_Data.returnData());

            List<KNNTrainingData> CurrentTrainingData = new List<KNNTrainingData>() { };
            List<KNNTrainingData> BackupTrainingData = new List<KNNTrainingData>(knnTrainingData);
            int number_of_training_instances = 30;
            int NumofGestures = Gestures.Count();
            List<int> CurrentNumbers = new List<int>(); for (int i = 0; i < NumofGestures; i++) { CurrentNumbers.Add(0); }
            foreach (KNNTrainingData ktd in knnTrainingData)
            {
                CurrentNumbers[ktd.getClass()] = CurrentNumbers[ktd.getClass()] + 1;
            }
            CurrentNumbers.RemoveAll(isZero);
            if (CurrentNumbers.Count() >= 2)
            {
                for (int Gesture = 0; Gesture < CurrentNumbers.Count(); Gesture++)
                {
                    List<KNNTrainingData> LocalCurrentTrainingData = new List<KNNTrainingData>() { };
                    while (LocalCurrentTrainingData.Count < number_of_training_instances && CurrentNumbers.Min() >= number_of_training_instances)
                    {
                        int Min = 0;
                        double minDistance = 1000000;
                        for (int i = 0; i < BackupTrainingData.Count; i++)
                        {
                            if (minDistance > (BackupTrainingData[i].getGesturePosition() - end_position).Length && BackupTrainingData[i].getClass() == Gesture)
                            {
                                Min = i;
                                minDistance = (BackupTrainingData[i].getGesturePosition() - end_position).Length;
                            }
                        }
                        LocalCurrentTrainingData.Add(BackupTrainingData[Min]);
                        BackupTrainingData.RemoveAt(Min);
                    }
                    CurrentTrainingData.AddRange(LocalCurrentTrainingData);
                }
                if(CurrentNumbers.Min() < number_of_training_instances)
                {
                    CurrentTrainingData = knnTrainingData;
                }
                int[] UIDs = new int[CurrentTrainingData.Count + 1];
                UIDs[0] = UID;
                for (int UIDnumber = 0; UIDnumber < CurrentTrainingData.Count; UIDnumber++)
                {
                    UIDs[UIDnumber + 1] = CurrentTrainingData[UIDnumber].getUID();
                }
                object result2 = null;
                object result3 = null;
                double[,] KNNFeatures = KNNListToArray(CurrentTrainingData);

                matlab.Feval("SaveArray", 0, out result3, "UIDArchive", "UIDArchive" + UID + ".mat", UIDs);
                int numGest = CurrentNumbers.Count();
                bool not_enough_gestures = false;
                if (CurrentNumbers.Count == 1) { not_enough_gestures = true; }

                if (CurrentNumbers.Min() > 1 && !not_enough_gestures)
                {
                    try
                    {
                        matlab.Feval("CubicSVM_UntrainedFeatures", 1, out result2, KNNFeatures, (double)(numGest));
                    }
                    catch (Exception e)
                    {
                        e = null;
                    }
                    // Define the output 
                    result2 = null;
                    // Call the MATLAB function myfunc
                    //   matlab.Feval("testClassifier", 1, out result, testFeatures);
                }
            }
            if (Training_mode)
            {
                previous_gesture = -2;
                matlab.Execute(@"cd " + System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\Tools\\");
                object resulttr = null;
                matlab.Feval("FeatureExtraction_Training", 1, out resulttr, Processed_MMG_Data.getDataArray(), (double)currentGesture);
                object[] restr = resulttr as object[];
                resulttr = null;
                knnTrainingData.Add(new KNNTrainingData(end_position, currentGesture, MATLAB2Dto1D((double[,])restr[0]), UID));
                line1[1204] = GestureTestValue; line1[1205] = UID;
            }
            else
            {
                matlab.Execute(@"cd " + System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\Tools\\");
                object result = null;
                matlab.Feval("FeatureExtraction_Test", 1, out result, Processed_MMG_Data.getDataArray());
                object[] res = result as object[];
                result = null;
                double[] testFeatures = (MATLAB2Dto1D((double[,])res[0]));
                result = null;
                matlab.Feval("testCSVMClassifier", 1, out result, testFeatures);
                res = result as object[];
                previous_gesture = (int)Math.Round((double)res[0]);

                line1[1204] = GestureTestValue; line1[1205] = UID;
            }
            UID++;
            fullRawDataArchive.Add(line1);
            return previous_gesture;
        }

        int Classifier0()
        {
            if (Training_mode)
            {
                double[] line = new double[1201]; line[0] = currentGesture;
                Convert2Dto1D(Processed_MMG_Data.getDataArray(), 200, 6).CopyTo(line, 1);
                fullRawDataArchive.Add(line);
                Gestures[currentGesture].add(Processed_MMG_Data.returnData());
                matlab.Execute(@"cd " + System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\Tools\\");
                object result = null;
                matlab.Feval("FeatureExtraction_Training", 1, out result, Processed_MMG_Data.getDataArray(), (double)currentGesture + 1);
                object[] res = result as object[];
                result = null;
                trainingdata.Add(MATLAB2Dto1D((double[,])res[0]));
                double[,] Features = ListToArray(trainingdata);
                previous_gesture = -2;
                List<double> classnumbers = new List<double>() { };
                for (int i = 0; i <= (Features.Length / 79) - 1; i++)
                {
                    classnumbers.Add(Features[i, 0]);
                }
                float[] numofclasses = new float[(int)classnumbers.Max()];
                for (int i = 0; i <= (Features.Length / 79) - 1; i++)
                {
                    numofclasses[(int)Features[i, 0] - 1] = numofclasses[(int)Features[i, 0] - 1] + 1;
                }
                int numGest = (int)(classnumbers.Max() - classnumbers.Min()) + 1;
                bool not_enough_gestures = false;
                foreach (float classes in numofclasses)
                {
                    if (classes == 1) { not_enough_gestures = true; }
                }
                if (numGest > 1 && !not_enough_gestures)
                {
                    try
                    {
                        matlab.Feval("LDA_Features", 1, out result, Features, (double)(numGest));
                    }
                    catch (Exception e)
                    {
                        e = null;
                    }
                    // Define the output 
                    result = null;
                    // Call the MATLAB function myfunc
                    //   matlab.Feval("testClassifier", 1, out result, testFeatures);
                }
            }
            else
            {
                matlab.Execute(@"cd " + System.IO.Directory.GetCurrentDirectory() + "\\..\\..\\Tools\\");
                object result = null;
                matlab.Feval("FeatureExtraction_Test", 1, out result, Processed_MMG_Data.getDataArray());
                object[] res = result as object[];
                result = null;
                double[] testFeatures = (MATLAB2Dto1D((double[,])res[0]));
                result = null;
                matlab.Feval("testClassifier", 1, out result, testFeatures);
                res = result as object[];
                previous_gesture = (int)Math.Round((double)res[0]) - 1;
            }
            return previous_gesture;
        }

        public double[,] ListToArray(List<double[]> input)
        {
            int a = input.Count();
            double[,] r = new double[a, 79]; int n1c = 0, n2c = 0;
            foreach (double[] n1 in input)
            {
                foreach (double n2 in n1)
                {
                    r[n1c, n2c++] = n2;
                }
                n1c++;
                n2c = 0;
            }
            return r;
        }
        public double[,] KNNListToArray(List<KNNTrainingData> input)
        {
            int a = input.Count();
            double[,] r = new double[a, 79]; int n1c = 0, n2c = 0;
            foreach (KNNTrainingData n1 in input)
            {
                foreach (double n2 in n1.getData())
                {
                    r[n1c, n2c++] = n2;
                }
                n1c++;
                n2c = 0;
            }
            return r;
        }
        public float[] extractFeatures()
        {
            float[] returnarray = { };
            return returnarray;
        }
        public double[] Convert2Dto1D(double[,] input, int x_size, int y_size)
        {
            double[] r = new double[x_size * y_size]; int n1c = 0;
            foreach (double n1 in input)
            {
                r[n1c] = n1;
                n1c++;
            }
            return r;
        }
        public double[] MATLAB2Dto1D(double[,] input)
        {
            int b = input.GetLength(1);
            double[] output = new double[b]; int i, j, k = 0; ;
            for (i = 0; i <= b - 1; i++)
            {
                output[i] = input[0, i];
            }

            return output;
        }
        public int TemplateClassifier()
        {

            for (int i = 0; i < Gestures.Count; i++)
            {
                correlation_Values[i] =
                    Correlation.Pearson(Processed_MMG_Data.getChannel(0), Gestures[i].returnGestureChannel(0)) +
                    Correlation.Pearson(Processed_MMG_Data.getChannel(1), Gestures[i].returnGestureChannel(1)) +
                    Correlation.Pearson(Processed_MMG_Data.getChannel(2), Gestures[i].returnGestureChannel(2)) +
                    Correlation.Pearson(Processed_MMG_Data.getChannel(3), Gestures[i].returnGestureChannel(3)) +
                    Correlation.Pearson(Processed_MMG_Data.getChannel(4), Gestures[i].returnGestureChannel(4)) +
                    Correlation.Pearson(Processed_MMG_Data.getChannel(5), Gestures[i].returnGestureChannel(5));

            }
            previous_gesture = correlation_Values.IndexOf(correlation_Values.Max());
            if (correlation_Values.Max() < 0)
            {
                previous_gesture = -1;
            }
            return previous_gesture;
        }


        public double[][] returnArray()
        {
            return Gestures[currentGesture].returnFullTemplate();
        }
        public double processData(int prev, int current, int channel)
        {
            double x = ((current - prev) * 0.9969f + 0.9937f * xp[channel]);
            double z1 = 0.07296f * (xp[channel] + x) + 0.8541f * zp[channel];
            xp[channel] = x;
            zp[channel] = z1;
            return z1;
        }
        public void close()
        {
            try
            {
                file.Close();
            }
            catch (Exception) { }
        }
    }
    public class GestureTemplate
    {
        double[][] gesture = new double[6][];
        public string description;
        public int currentGestureCount = 0;
        public GestureTemplate(string _desc)
        {
            description = _desc;
            for (int i = 0; i < 6; i++)
            {
                gesture[i] = new double[200];
            }
        }
        public void updateFullGestureArray(double[][] _gesture, int count)
        {
            gesture = _gesture;
            currentGestureCount = count;
        }
        public void add(List<List<double>> data)
        {
            if (currentGestureCount > 0)
            {
                double[][] hold = new double[6][];
                for (int i = 0; i < 6; i++)
                {
                    hold[i] = data[i].ToArray();
                    for (int j = 0; j < hold[i].Length; j++)
                    {
                        gesture[i][j] = ((gesture[i][j] * currentGestureCount) + hold[i][j]) / (currentGestureCount + 1);
                    }
                }
            }
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    gesture[i] = data[i].ToArray();
                }
            }
            currentGestureCount++;
        }
        public double[] returnGestureChannel(int channel)
        {
            return gesture[channel];
        }
        public string returnGestureChannelToString(int channel)
        {
            string returnstring = "";
            double[] dataarray = returnGestureChannel(channel);
            byte[] byteArray = { };
            foreach (double data in dataarray)
            {
                byteArray = BitConverter.GetBytes(data);
                returnstring +=
                    byteArray[0].ToString("D3") + "," +
                    byteArray[1].ToString("D3") + "," +
                    byteArray[2].ToString("D3") + "," +
                    byteArray[3].ToString("D3") + "," +
                    byteArray[4].ToString("D3") + "," +
                    byteArray[5].ToString("D3") + "," +
                    byteArray[6].ToString("D3") + "," +
                    byteArray[7].ToString("D3") + ";"
                    ;
            }
            return returnstring;
        }
        public double[][] returnFullTemplate()
        {
            return gesture;
        }
    }
    public class KNNTrainingData
    {
        OpenTK.Vector3 GesturePosition;
        int Class;
        double[] data;
        int UID;
        public KNNTrainingData(OpenTK.Vector3 _GesturePosition, int _Class, double[] _data,int _UID)
        {
            UID = _UID;
            GesturePosition = _GesturePosition;
            Class = _Class;
            data = _data;
        }
        public int getUID()
        {
            return UID;
        }
        public OpenTK.Vector3 getGesturePosition()
        {
            return GesturePosition;
        }
        public int getClass()
        {
            return Class;
        }
        public double[] getData()
        {
            return data;
        }

    }
    public class RollingBuffer
    {
        List<List<int>> array;
        int size;
        public bool full = false;
        public RollingBuffer(int _size)
        {
            size = _size;
            array = new List<List<int>> { new List<int> { 0}, new List<int> { 0}, new List<int> { 0}, new List<int> { 0}, new List<int> { 0}, new List<int> { 0}, new List<int> { 0}, new List<int> { 0} };
        }

        public void add(int[] values)
        {
            for (int i = 0; i < values.Count(); i++)
            {
                array[i].Add(values[i]);
                if (array[i].Count() > size)
                {
                    full = true;
                    array[i].RemoveAt(0);
                }
            }
        }
        public List<int> getChannel(int channel)
        {
            return array[channel];
        }
        public int getprev(int channel)
        {
            return array[channel][size-2];
        }
        public int getcurrent(int channel)
        {
            return array[channel][size-1];
        }
    }

    public class RollingBufferf
    {
        List<List<double>> array;
        int size;
        public bool full = false;
        public RollingBufferf(int _size)
        {
            size = _size;
            array = new List<List<double>> { new List<double> { 0}, new List<double> { 0}, new List<double> { 0}, new List<double> { 0}, new List<double> { 0}, new List<double> { 0}, new List<double> { 0}, new List<double> { 0} };
        }

        public void add(double[] values)
        {
            for (int i = 0; i < values.Count(); i++)
            {
                array[i].Add(values[i]);
                if (array[i].Count() > size)
                {
                    full = true;
                    array[i].RemoveAt(0);
                }
            }
        }
        public List<List<double>> returnData()
        {
            return array;
        }
        public List<double> getChannel(int channel)
        {
            return array[channel];
        }

        public double[,] getDataArray()
        {
             double[,] r = new double[6, 200]; int n1c = 0, n2c = 0;
             foreach (List<double> n1 in array)
             {
                 foreach (double n2 in n1)
                 {
                     r[n1c, n2c++] = n2;
                 }
                 n1c++;
                 n2c = 0;
                if (n1c == 6) break;
             }
             return r;
         }
        public double getprev(int channel)
        {
            return array[channel][array[channel].Count-2];
        }
        public double getcurrent(int channel)
        {
            return array[channel][array[channel].Count-1];
        }
    }
}
