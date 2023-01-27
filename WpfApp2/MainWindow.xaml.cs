using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace WpfApp2
{
    public partial class MainWindow : Window
    {
        private bool isok = true;
        IPAddress IPAd { get; set; }
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public MemoryStream TakeScreenShot()
        {
            Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            MemoryStream test = new MemoryStream();
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(0, 0, 0, 0, Screen.PrimaryScreen.Bounds.Size);
                bmp.Save(test, ImageFormat.Jpeg);

                //bmp.Save(path, ImageFormat.Png);
            }
            return test;
        }

        private void SendMessage()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                      //var bytes = Encoding.ASCII.GetBytes("Aa");
                      //socket.Send(bytes);
                        var bytes = TakeScreenShot().ToArray();
                        socket.Send(bytes);

                    }
                    catch (Exception)
                    {
                       System.Windows.MessageBox.Show("Exit");
                        isok = true;
                        this.Close();
                        //this.Dispatcher.BeginInvoke(new Action(() =>
                        //{
                        //      this.Close();
                        //}));
                        throw;
                       
                    }
                
                }
            });

        }
        private void Connection1()
        {
            try
            {
                IPAd =  IPAddress.Loopback;
                var port = 27009;
                var op = new IPEndPoint(IPAd, port);
                socket.Connect(op);
                if (socket.Connected)
                {
                    SendMessage();
                }
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("Exit");
                Close();
                throw;
            }
        }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isok)
            {
                try
                {
                    if (IP_text_box.Text.Length>0)
                    {
                        if (IP_text_box.Text == "NIHAD")
                            IPAd = IPAddress.Loopback;
                        else IPAd = IPAddress.Parse(IP_text_box.Text);
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Enter IpAdress");
                        return;
                    }
                }
                catch (Exception)
                {
                    System.Windows.MessageBox.Show("Error IpAdress");
                    return;
                }
                Task t1 = new Task(Connection1, TaskCreationOptions.LongRunning);
                t1.Start();
                isok = false;
            }
            
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
