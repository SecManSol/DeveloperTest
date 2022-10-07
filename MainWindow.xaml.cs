using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SimpleTCP;
using System.IO;


namespace EchoTested
{

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        SimpleTcpServer server;


        public class DisplayData
        {
            public TimeOnly Started { get; set; }
            public TimeOnly Processed { get; set; }
            public TimeSpan Elapsed { get { return Processed - Started; } set { } }
        }
        private void MainWindow_Load(object sender, EventArgs e)
        {
            try
            {

                DisplayData displayData = new() { Started = TimeOnly.FromDateTime(DateTime.Now) };
                displayData.Processed = TimeOnly.FromDateTime(DateTime.Now);
                server = new SimpleTcpServer();
                server.Delimiter = 0 * 13;
                server.StringEncoder = Encoding.UTF8;
                server.DataReceived += Server_DataReceived;

              


                Thread.Sleep(500);
                dataGrid.Items.Add(displayData);



            }
            catch (Exception ex)
            {
                txtStatus.Text = ex.Message;
            }
        }
        private void Server_DataReceived(object sender, SimpleTCP.Message e)
        {
            txtStatus.Dispatcher.Invoke(new Action(delegate
            {
                txtStatus.Text += e.MessageString;
               
            }));
        }

        public void btnStart_Click(object sender, RoutedEventArgs e) /*Start button*/
        {
            txtStatus.Text += "Server starting ...";
           System.Net.IPAddress ip = new System.Net.IPAddress(long.Parse(txtHost.Text));
           server.Start(ip, Convert.ToInt32(txtPort.Text));
                
            
        
        }

        public void btnStop_Click(object sender, RoutedEventArgs e) /*Stop button*/
        {
            if (server.IsStarted)
                server.Stop();
           
        }

       
            



        }

    }


    

