using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using System.Drawing.Imaging;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System.Diagnostics;
using System.IO;


namespace NUClass
{
    public partial class Form2 : Form
    {
        bool loaded = false;
        Bitmap Texture;
        QuatFunc QuatFunc = new QuatFunc();
        int textureID;
        Quaternion quat;
        Vector3 axis;
        float angle;
        int counter = 0;
        bool _docked = false;
        public Form2()
        {
            InitializeComponent();
            BackColor = Color.SkyBlue;
        }
        protected override void OnResize(EventArgs e)
        {
            if (!_docked)
            {
                glControl1.Height = this.Height;
                glControl1.Width = this.Width;
                this.label1.Location = new System.Drawing.Point(6, (this.Height - 60));
            }
        }

        int[] offest = { -25,-25,-25,-25,-25,-25 };
        float AXhold = 0, AYhold = 0, AZhold = 0;
        float AXshold = 0, AYshold = 0, AZshold = 0;
        float MXhold = -1, MYhold = 1, MZhold = 0;

        int[,] background_images =
        {
            {0,0,351,117 },
            {0,118,351,235},
            {0,236,351,470 },
            {0,471,351,705 },
            {0,706,234,823 },
            {0,824,234,942 }
        };

        int[,] cubeColors = {
            { 255, 0, 0, 255 },
            { 255, 0, 0, 255 },
            { 0, 255, 0, 255 },
            { 0, 255, 0, 255 },
            { 0, 0, 255, 255 },
            { 0, 0, 255, 255 },
            { 0, 255, 255, 255 },
            { 0, 255, 255, 255 },
            { 255,0, 255, 255 },
            { 255,0, 255, 255 },
            {255, 255,0,  255 },
            { 255,255,0, 255 },
                    };

        float[,] triangles =
        {
            { 1, 0, 2 ,1,0.5f,1,0.2502651113467656415694591728526f,0,0.5f}, // bottom
            { 3, 2, 0 ,0,0.2502651113467656415694591728526f,0,0.5f,1,0.2502651113467656415694591728526f},
            { 6, 4, 5 ,0,0.5f,1,0.74761399787910922587486744432662f,1,0.5f}, // top
            { 4, 6, 7 ,1,0.74761399787910922587486744432662f,0,0.5f,0,0.74761399787910922587486744432662f},
            { 4, 7, 0 ,1,0.12526539278131634819532908704883f,0,0.12526539278131634819532908704883f,1,0.2502651113467656415694591728526f}, // front
            { 7, 3, 0 ,0,0.12526539278131634819532908704883f,0,0.2502651113467656415694591728526f,1,0.2502651113467656415694591728526f},
            { 1, 2, 5 ,0,0.12526539278131634819532908704883f,1,0.12526539278131634819532908704883f,0,0}, // back
            { 2, 6, 5 ,1,0.12526539278131634819532908704883f,1,0,0,0},
            { 0, 1, 5 ,0,1,0.66666666666666666666666666666667f,1,0.66666666666666666666666666666667f,0.87473460721868365180467091295117f}, // right
            { 0, 5, 4 ,0,1,0.66666666666666666666666666666667f,0.87473460721868365180467091295117f,0,0.87473460721868365180467091295117f},
            { 2, 3, 6 ,0,0.87473460721868365180467091295117f,0.66666666666666666666666666666667f,0.87473460721868365180467091295117f,0,0.74946921443736730360934182590234f}, // left
            { 3, 7, 6 ,0.66666666666666666666666666666667f,0.87473460721868365180467091295117f,0.66666666666666666666666666666667f,0.74946921443736730360934182590234f,0,0.74946921443736730360934182590234f},
        };

        float[,] cube = {
            {-100, 150, 50}, // vertex[0]
            { 100, 150, 50 }, // vertex[1]
            { 100,-150, 50 }, // vertex[2]
            {-100,-150, 50 }, // vertex[3]
            {-100, 150,-50 }, // vertex[4]
            { 100, 150,-50 }, // vertex[5]
            { 100,-150,-50 }, // vertex[6]
            {-100,-150,-50 }, // vertex[7]
		};

        Stopwatch sw = new Stopwatch(); // available to all event handlers
        private void glControl1_Load(object sender, EventArgs e)
        {
            loaded = true;
            GL.ClearColor(Color.SkyBlue); // Yey! .NET Colors can be used directly!
            SetupViewport();
            Application.Idle += Application_Idle; // press TAB twice after +=
            sw.Start(); // start at application boot

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);

            load_texture();
            // Accept fragment if it closer to the camera than the former one

        }
        void Application_Idle(object sender, EventArgs e)
        {
            double milliseconds = ComputeTimeSlice();
            Accumulate(milliseconds);
            Animate(milliseconds);
        }

        float rotation = 0;
        private void Animate(double milliseconds)
        {
            float deltaRotation = (float)milliseconds / 20.0f;
            rotation += deltaRotation;
            glControl1.Invalidate();
        }

        double accumulator = 0;
        int idleCounter = 0;
        private void Accumulate(double milliseconds)
        {
            idleCounter++;
            accumulator += milliseconds;
            if (accumulator > 1000)
            {
                label1.Text = idleCounter.ToString();
                accumulator -= 1000;
                idleCounter = 0; // don't forget to reset the counter!
            }
        }

        private double ComputeTimeSlice()
        {
            sw.Stop();
            double timeslice = sw.Elapsed.TotalMilliseconds;
            sw.Reset();
            sw.Start();
            return timeslice;
        }

        private void SetupViewport()
        {
            int w = glControl1.Width;
            int h = glControl1.Height;
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            GL.Ortho(0, w, 0, h, -2000, 2000); // Bottom-left corner pixel has coordinate (0, 0)
            GL.Viewport(0, 0, w, h); // Use all of the glControl painting area
        }

        public void Init(bool docked)
        {
            glControl1.MakeCurrent();
            _docked = docked;
            if (docked)
            {
                this.Height = 182;
                this.Width = 323;
                glControl1.Height = 182;
                glControl1.Width = 323;
            }
        }
        public void Init(bool docked, int height, int width)
        {
            glControl1.MakeCurrent();
            _docked = docked;
            if (docked)
            {
                this.Height = height;
                this.Width = width;
                glControl1.Height = height;
                glControl1.Width = width;
            }
        }
        public void resize(int height, int width)
        {
            glControl1.MakeCurrent();
            this.Height = height;
            this.Width = width;
            glControl1.Height = height;
            glControl1.Width = width;
        }
        private void load_texture()
        {
            Texture = new Bitmap("../.../Images.bmp");
            GL.GenTextures(1, out textureID);
            GL.BindTexture(TextureTarget.Texture2D, textureID);
            BitmapData bmp_data = Texture.LockBits(new Rectangle(0, 0, Texture.Width, Texture.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);
            Texture.UnlockBits(bmp_data);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

        }
        public static Quaternion QuaternionFromYawPitchRoll(float yaw, float pitch, float roll)
        {
            Quaternion result = Quaternion.Identity;
            float num9 = roll * 0.5f;
            float num6 = (float)Math.Sin((double)num9);
            float num5 = (float)Math.Cos((double)num9);
            float num8 = pitch * 0.5f;
            float num4 = (float)Math.Sin((double)num8);
            float num3 = (float)Math.Cos((double)num8);
            float num7 = yaw * 0.5f;
            float num2 = (float)Math.Sin((double)num7);
            float num = (float)Math.Cos((double)num7);
            result.X = ((num * num4) * num5) + ((num2 * num3) * num6);
            result.Y = ((num2 * num3) * num5) - ((num * num4) * num6);
            result.Z = ((num * num3) * num6) - ((num2 * num4) * num5);
            result.W = ((num * num3) * num5) + ((num2 * num4) * num6);
            return result;
        }

        public void setAllValues(Quaternion quat, Vector3 Accel, Vector3 Mag)
        {
            setValue(quat);
            setAccelerometerValue(Accel.X, Accel.Y, Accel.Z);
            setMagnetometerValue(Mag.X, Mag.Y, Mag.Z);
        }
        public void setValue(Quaternion _quat)
        {
            quat.W = _quat.W;
            quat.X = -_quat.X;
            quat.Y = -_quat.Y;
            quat.Z = -_quat.Z;
        }
        public void setAccelerometerValue(float AX, float AY, float AZ)
        {
            AXshold = AX; AYshold = AY; AZshold = AZ;
            counter++;
            AXhold += AX; AYhold += AY; AZhold += AZ;
            if (counter >= 5)
            {
                AccelerometerValue(AXhold / 5, AYhold / 5, AZhold / 5);
                counter = 0;
                AXhold = 0; AYhold = 0; AZhold = 0;
            }

        }
        public void setMagnetometerValue(float _MX, float _MY, float _MZ)
        {
            MXhold = _MX;
            MYhold = _MY;
            MZhold = _MZ;
        }
        public void AccelerometerValue(float AX, float AY, float AZ)
        {
            if (AZ > 0)
            {
                offest[1] = Math.Abs(((int)AZ / 75));
                offest[0] = 0;
            }
            else
            {
                offest[0] = Math.Abs(((int)AZ / 75));
                offest[1] = 0;
            }
            if (AX > 0)
            {
                offest[5] = Math.Abs(((int)AX / 75));
                offest[4] = 0;
            }
            else
            {
                offest[4] = Math.Abs(((int)AX / 75));
                offest[5] = 0;
            }
            if (AY > 0)
            {
                offest[3] = Math.Abs(((int)AY / 75));
                offest[2] = 0;
            }
            else
            {
                offest[2] = Math.Abs(((int)AY / 75));
                offest[3] = 0;
            }
        }
        private void glControl1_Paint(object sender, PaintEventArgs e)
        {
            if (!loaded)
                return;
            // Enable depth test

            glControl1.MakeCurrent();

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView((float)((Math.PI) / 3), glControl1.AspectRatio, 1, 10000);


            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.MultMatrix(ref perspective);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            GL.Translate(30, 35, -500);


            Quaternion quat2 = Quaternion.Invert(quat);
            Quaternion quat3 = QuaternionFromYawPitchRoll(0, -(((float)Math.PI) / 2), 0) * quat2;
            //Quaternion quat6 = QuaternionFromYawPitchRoll(0, -(((float)Math.PI) / 2), 0);
            //Quaternion quat4 = Quaternion.Invert(QuaternionFromYawPitchRoll(0, 0, 0));
            //Quaternion quat5 = quat3 * quat4;
            quat3.ToAxisAngle(out axis, out angle);
            angle = ((angle* 180 / (float)Math.PI) );

            


            int length = 250;
            Vector3 vectorRef = Vector3.Normalize(new Vector3(1, 0, 0));
            Vector3 vectorCurrent = Vector3.Normalize(new Vector3(MXhold, MYhold, MZhold));
            Vector3 Cross = Vector3.Normalize(Vector3.Cross(vectorRef, vectorCurrent));
            float anglea = (float)Math.Acos(Vector3.Dot(vectorRef, vectorCurrent));
            Quaternion RotQuat = Quaternion.Normalize(new Quaternion(Cross.X * (float)Math.Sin((anglea) / 2), Cross.Y * (float)Math.Sin((anglea) / 2), Cross.Z * (float)Math.Sin((anglea) / 2), (float)Math.Cos((anglea) / 2)));
            if (float.IsNaN(Cross.X) || float.IsNaN(Cross.Y) || float.IsNaN(Cross.Z)) { RotQuat = Quaternion.Normalize(new Quaternion(0, 0, 0, 1)); }
            Matrix3 m = Matrix3.CreateFromQuaternion(RotQuat);

            length = 250;
            vectorRef = Vector3.Normalize(new Vector3(1, 0, 0));
            vectorCurrent = Vector3.Normalize(new Vector3(AXshold, AYshold, AZshold));
            Cross = Vector3.Normalize(Vector3.Cross(vectorRef, vectorCurrent));
            anglea = (float)Math.Acos(Vector3.Dot(vectorRef, vectorCurrent));
            RotQuat = Quaternion.Normalize(new Quaternion(Cross.X * (float)Math.Sin((anglea) / 2), Cross.Y * (float)Math.Sin((anglea) / 2), Cross.Z * (float)Math.Sin((anglea) / 2), (float)Math.Cos((anglea) / 2)));
            if (float.IsNaN(Cross.X) || float.IsNaN(Cross.Y) || float.IsNaN(Cross.Z)) { RotQuat = Quaternion.Normalize(new Quaternion(0, 0, 0, 1)); }
            Matrix3 ma = Matrix3.CreateFromQuaternion(RotQuat);


            GL.Rotate(angle, axis); // OpenTK has this nice Vector3 class!

            GL.Begin(BeginMode.Triangles);
            //GL.Begin(BeginMode.Quads);//Alternative

            GL.Color3(Color.White);

            for (int a = 0; a < 12; a++)
            {
                //   GL.Color3(Color.FromArgb(cubeColors[a, 3], cubeColors[a, 0], cubeColors[a, 1], cubeColors[a, 2]));
                
                GL.BindTexture(TextureTarget.Texture2D, textureID);

                GL.TexCoord2(triangles[a, 3], triangles[a, 4]);
                GL.Vertex3(cube[(int)triangles[a, 0], 0], cube[(int)triangles[a, 0], 1], cube[(int)triangles[a, 0], 2]);
                GL.TexCoord2(triangles[a, 5], triangles[a, 6]);
                GL.Vertex3(cube[(int)triangles[a, 1], 0], cube[(int)triangles[a, 1], 1], cube[(int)triangles[a, 1], 2]);
                GL.TexCoord2(triangles[a, 7], triangles[a, 8]);
                GL.Vertex3(cube[(int)triangles[a, 2], 0], cube[(int)triangles[a, 2], 1], cube[(int)triangles[a, 2], 2]);
            }

            for (int k = 0; k < 360; k += 1)
            {
                GL.Color3(Color.Red);
                GL.Vertex3(0, 0, 75 + offest[0]);
                GL.Vertex3(Math.Cos(k) * 25f, Math.Sin(k) * 25f, 50 + offest[0]);
                GL.Vertex3((Math.Cos(k + 1)) * 25f, (Math.Sin(k + 1)) * 25f, 50 + offest[0]);

                GL.Color3(Color.Blue);
                GL.Vertex3(0, 0, -75 - offest[1]);
                GL.Vertex3(Math.Cos(k) * 25f, Math.Sin(k) * 25f, -50 - offest[1]);
                GL.Vertex3((Math.Cos(k + 1)) * 25f, (Math.Sin(k + 1)) * 25f, -50 - offest[1]);

                GL.Color3(Color.Yellow);
                GL.Vertex3(0, 175 + offest[2], 0);
                GL.Vertex3(Math.Cos(k) * 25f, 150 + offest[2], Math.Sin(k) * 25f);
                GL.Vertex3((Math.Cos(k + 1)) * 25f, 150 + offest[2], (Math.Sin(k + 1)) * 25f);

                GL.Color3(Color.Green);
                GL.Vertex3(0, -175 - offest[3], 0);
                GL.Vertex3(Math.Cos(k) * 25f, -150 - offest[3], Math.Sin(k) * 25f);
                GL.Vertex3((Math.Cos(k + 1)) * 25f, -150 - offest[3], (Math.Sin(k + 1)) * 25f);

                GL.Color3(Color.Purple);
                GL.Vertex3(125 + offest[4], 0, 0);
                GL.Vertex3(100 + offest[4], Math.Cos(k) * 25f, Math.Sin(k) * 25f);
                GL.Vertex3(100 + offest[4], (Math.Cos(k + 1)) * 25f, (Math.Sin(k + 1)) * 25f);

                GL.Color3(Color.Orange);
                GL.Vertex3(-125 - offest[5], 0, 0);
                GL.Vertex3(-100 - offest[5], Math.Cos(k) * 25f, Math.Sin(k) * 25f);
                GL.Vertex3(-100 - offest[5], (Math.Cos(k + 1)) * 25f, (Math.Sin(k + 1)) * 25f);

                GL.Color3(Color.Silver);
                GL.Vertex3(QuatFunc.MultiplyByQuat(m, new Vector3(0 - length - 25, 0, 0)));
                GL.Vertex3(QuatFunc.MultiplyByQuat(m, new Vector3(0 - length, (float)Math.Cos(k) * 25f, (float)Math.Sin(k) * 25f)));
                GL.Vertex3(QuatFunc.MultiplyByQuat(m, new Vector3(0 - length, (float)(Math.Cos(k + 1)) * 25f, (float)(Math.Sin(k + 1)) * 25f)));
                GL.Color3(Color.Gold);
                GL.Vertex3(QuatFunc.MultiplyByQuat(ma, new Vector3(0 - length - 25, 0, 0)));
                GL.Vertex3(QuatFunc.MultiplyByQuat(ma, new Vector3(0 - length, (float)Math.Cos(k) * 25f, (float)Math.Sin(k) * 25f)));
                GL.Vertex3(QuatFunc.MultiplyByQuat(ma, new Vector3(0 - length, (float)(Math.Cos(k + 1)) * 25f, (float)(Math.Sin(k + 1)) * 25f)));
            }
            GL.End();
                GL.Begin(BeginMode.Quads);
            for (int k = 0; k <= 360; k += 1)
            {
                GL.Color3(Color.Red);
                GL.Vertex3(Math.Cos(k) * 5f, Math.Sin(k) * 5f, 50);
                GL.Vertex3((Math.Cos(k + 1)) * 5f, (Math.Sin(k + 1)) * 5f, 50);
                GL.Vertex3(Math.Cos(k) * 5f, Math.Sin(k) * 5f, 50 + offest[0]);
                GL.Vertex3((Math.Cos(k + 1)) * 5f, (Math.Sin(k + 1)) * 5f, 50 + offest[0]);
                GL.Color3(Color.Blue);
                GL.Vertex3(Math.Cos(k) * 5f, Math.Sin(k) * 5f, -50);
                GL.Vertex3((Math.Cos(k + 1)) * 5f, (Math.Sin(k + 1)) * 5f, -50);
                GL.Vertex3(Math.Cos(k) * 5f, Math.Sin(k) * 5f, -50 - offest[1]);
                GL.Vertex3((Math.Cos(k + 1)) * 5f, (Math.Sin(k + 1)) * 5f, -50 - offest[1]);
                GL.Color3(Color.Yellow);
                GL.Vertex3(Math.Cos(k) * 5f, 150, Math.Sin(k) * 5f);
                GL.Vertex3((Math.Cos(k + 1)) * 5f, 150, (Math.Sin(k + 1)) * 5f);
                GL.Vertex3(Math.Cos(k) * 5f, 150 + offest[2], Math.Sin(k) * 5f);
                GL.Vertex3((Math.Cos(k + 1)) * 5f, 150 + offest[2], (Math.Sin(k + 1)) * 5f);
                GL.Color3(Color.Green);
                GL.Vertex3(Math.Cos(k) * 5f, -150, Math.Sin(k) * 5f);
                GL.Vertex3((Math.Cos(k + 1)) * 5f, -150, (Math.Sin(k + 1)) * 5f);
                GL.Vertex3(Math.Cos(k) * 5f, -150 - offest[3], Math.Sin(k) * 5f);
                GL.Vertex3((Math.Cos(k + 1)) * 5f, -150 - offest[3], (Math.Sin(k + 1)) * 5f);
                GL.Color3(Color.Purple);
                GL.Vertex3(100, Math.Cos(k) * 5f, Math.Sin(k) * 5f);
                GL.Vertex3(100, (Math.Cos(k + 1)) * 5f, (Math.Sin(k + 1)) * 5f);
                GL.Vertex3(100 + offest[4], Math.Cos(k) * 5f, Math.Sin(k) * 5f);
                GL.Vertex3(100 + offest[4], (Math.Cos(k + 1)) * 5f, (Math.Sin(k + 1)) * 5f);
                GL.Color3(Color.Orange);
                GL.Vertex3(-100, Math.Cos(k) * 5f, Math.Sin(k) * 5f);
                GL.Vertex3(-100, (Math.Cos(k + 1)) * 5f, (Math.Sin(k + 1)) * 5f);
                GL.Vertex3((-100 - offest[5]), Math.Cos(k) * 5f, Math.Sin(k) * 5f);
                GL.Vertex3((-100 - offest[5]), (Math.Cos(k + 1)) * 5f, (Math.Sin(k + 1)) * 5f);
                GL.Color3(Color.Silver);
                GL.Vertex3(QuatFunc.MultiplyByQuat(m, new Vector3(0, (float)Math.Cos(k) * 5f, (float)Math.Sin(k) * 5f)));
                GL.Vertex3(QuatFunc.MultiplyByQuat(m, new Vector3(0, (float)Math.Cos(k) * 5f, (float)Math.Sin(k) * 5f)));
                GL.Vertex3(QuatFunc.MultiplyByQuat(m, new Vector3((0 - length), (float)Math.Cos(k) * 5f, (float)Math.Sin(k) * 5f)));
                GL.Vertex3(QuatFunc.MultiplyByQuat(m, new Vector3((0 - length), (float)(Math.Cos(k + 1)) * 5f, (float)(Math.Sin(k + 1)) * 5f)));
                GL.Color3(Color.Gold);
                GL.Vertex3(QuatFunc.MultiplyByQuat(ma, new Vector3(0, (float)Math.Cos(k) * 5f, (float)Math.Sin(k) * 5f)));
                GL.Vertex3(QuatFunc.MultiplyByQuat(ma, new Vector3(0, (float)Math.Cos(k) * 5f, (float)Math.Sin(k) * 5f)));
                GL.Vertex3(QuatFunc.MultiplyByQuat(ma, new Vector3((0 - length), (float)Math.Cos(k) * 5f, (float)Math.Sin(k) * 5f)));
                GL.Vertex3(QuatFunc.MultiplyByQuat(ma, new Vector3((0 - length), (float)(Math.Cos(k + 1)) * 5f, (float)(Math.Sin(k + 1)) * 5f)));
            }
            GL.End();

            glControl1.SwapBuffers();
        }

        private void glControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                glControl1.Invalidate();
            }

        }

        private void glControl1_Resize(object sender, EventArgs e)
        {
            SetupViewport();
            glControl1.Invalidate();
        }
    }
    public class QuatFunc
    {
        public QuatFunc() { }
        public Vector3 MultiplyByQuat(Matrix3 mat, Vector3 vec)
        {
            Vector3 result = new Vector3(0, 0, 0);
            for (int i = 0; i < 3; i++)
            {
                for (int j=0; j < 3; j++)
                {
                    result[i] += mat[j, i] * vec[j];
                }
            }
            return result;
        }
    }
}