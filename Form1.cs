using AForge.Video;
using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Camera_Upload
{
    public partial class Form1 : Form
    {
        private string Path = @"D:\Captura\";
        private bool Devices = false;
        private FilterInfoCollection MyDevice;
        private VideoCaptureDevice MyCam;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CargaDevice();
        }
        public void CargaDevice()
        {
            MyDevice = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (MyDevice.Count > 0)
            {
                Devices = true;
                for (int i = 0; i < MyDevice.Count; i++)
                {
                    comboBox1.Items.Add(MyDevice[i].Name);
                    comboBox1.Text = MyDevice[0].ToString();
                }
            }
            else
            {
                Devices = false;
            }
        }
        public void CloseCam()
        {
            if (MyCam != null && MyCam.IsRunning)
            {
                MyCam.SignalToStop();
                MyCam = null;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            CloseCam();
            int i = comboBox1.SelectedIndex;
            string NombreVideo = MyDevice[i].MonikerString;
            MyCam = new VideoCaptureDevice(NombreVideo);
            MyCam.NewFrame += new NewFrameEventHandler(Captura);
            MyCam.Start();
        }

        private void Captura(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap Imagen = (Bitmap)eventArgs.Frame.Clone();
            pictureBox1.Image = Imagen;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            CloseCam();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
                Console.WriteLine(Path);

            }
            if (MyCam != null && MyCam.IsRunning)
            {
                pictureBox2.Image = pictureBox1.Image;
                pictureBox2.Image.Save(Path + "csns.jpg", ImageFormat.Jpeg);
            }
        }
    }
}
