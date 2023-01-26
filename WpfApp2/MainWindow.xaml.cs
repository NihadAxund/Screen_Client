﻿using System;
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
                var IPAddres = IPAddress.Loopback;
                var port = 27009;
                var op = new IPEndPoint(IPAddres, port);
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
            Task.Run(() => { Connection1(); });
            //Connection1();
            //SendMessage();
        }
    }
}