/** This Code was Written By : 
 ** Lindokuhle .E Magagula
 ** 23 September 2022
 ** 18:00 PM
 ** In South Africa
 */

using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace WPF_Application
{
    
    public partial class MainWindow : Window
    {
        //Port number of the remote server.
        private readonly int _Port = 12345;
        //IPAddress of the remote server.
        private readonly string _IPAdrress = "127.0.0.1";

        TcpClient _TcpClient = new TcpClient(AddressFamily.InterNetwork);
        NetworkStream _networkStream;
        
        string _Started; 
        double _Elapsed, _Processed;

        Guid _guidID;
        Stopwatch _Stopwatch = new Stopwatch();

        public MainWindow()
        {
            InitializeComponent();
            StopButton.IsEnabled = false;
        }

        private async void StartButton_Click(object sender, RoutedEventArgs e)
        {
            sendData:
            try
            {
                //check connection
                if (_TcpClient.Connected)
                {
                    //Enable and Disable button 
                    StopButton.IsEnabled = true;
                    StartButton.IsEnabled = false;

                    //Stopwatch 
                    _Stopwatch.Start(); // start stopwatch

                    //Started process At
                    _Started = (DateTime.Now).ToLongTimeString();
                    
                    //** => Write a message to the server and convert it in a byte
                    //Get a client Networkstream for reading and writing
                    _networkStream = _TcpClient.GetStream();

                    /**-Send the json string using a TCP socket to the echotool(The echotool will echo back all received data)*/
                    var json = SerialiserMethod();
                    //Transalate the passed message into UTF8 and store it as a byte array
                    byte[] sendBuffer = Encoding.UTF8.GetBytes(json);
                    //send the message to the server
                    _networkStream.Write(sendBuffer, 0, sendBuffer.Length);


                    /** => Read the server message into a byte buffer*/
                    //byte to store the response message byte from the server
                    byte[] receivedBuffer = new byte[1024];
                    //Read the response bytes that was sent by the server 
                    _networkStream.Read(receivedBuffer, 0, receivedBuffer.Length);
                    //Converting the bytes into string
                    string data = Encoding.UTF8.GetString(receivedBuffer);

                    // Parse and assemble the echo'ed json data packet(s) and deserialize it back into an object
                    var _objectResult = DeSerialiserMethod(data);
                    
                    _Processed = _Stopwatch.Elapsed.TotalSeconds;
                    _Elapsed = _Stopwatch.Elapsed.TotalMilliseconds;
                    
                    

                    if (_objectResult != null)
                    {
                        //Add this echo result to the GUI Grid into the relevant columns
                        //(Started = timestamp when this echo process started , Processed = timestamp after deserialization into an object ,
                        //Elapsed = milliseconds elapsed from Started to Processed)
                        DisplayResult[] result = new DisplayResult[] { new DisplayResult(_Started,
                            _Processed, _Elapsed, _guidID)};

                        //ResultWindow.ItemsSource = result;
                        ResultWindow.Items.Add(result);
                    }

                    // Repeat the below exactly every 500 ms
                    await Task.Delay(millisecondsDelay: 5000);

                    //reset Stopwatch 
                    _Stopwatch.Reset();
                    //repeat the process
                    goto sendData;
                }   
            }
            catch (Exception ex) when (ex is ArgumentOutOfRangeException || ex is ArgumentNullException)
            {
                //if there is Error while establishing the connection to the server 
                MessageBox.Show($"Connection Closed, There was an Error: {ex.Message}",
                    "Error",MessageBoxButton.YesNo);
            }
        }
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_TcpClient.Connected)
                {

                    //closing the connection
                    _networkStream.Close();
                    _TcpClient.Close();
                    DisplayStatus.Text = "Disconnected";
                    StopButton.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"The is an Error closing the connection :{ex.Message}",
                    "Error", MessageBoxButton.YesNo);
            }

        }
        private void Window_Activated(object sender, EventArgs e)
        {
            //Establish connection to the server
            try
            {
                //The array of IP Address of the remote host
                IPAddress[] _IPAddresses = Dns.GetHostAddresses(_IPAdrress);
                //Connect to the server
                _TcpClient.Connect(_IPAddresses , _Port);
                //Send connection message to indicate the connection
                if (_TcpClient.Connected)
                {
                    DisplayStatus.Text = $"Connected";
                }
               
            }
            //Catch the Excepption when the host is NULL or when the port is not between minPort and Maxport
            catch (Exception ex) when (ex is SocketException || ex is ArgumentNullException)
            {
                //if there is Error while establishing the connection to the server 
               MessageBox.Show($"Connection didn't happen, Fail to Connect: {ex.Message}",
                   "Error",MessageBoxButton.YesNo);
            }
            
        }
        private string SerialiserMethod()
        {
            //-Create object that contains an initialized GUID
           _guidID = Guid.NewGuid();  //Create a new Random guid value

            // -Serialize the object using a Json serializer
            var _jsonSerializer = JsonConvert.SerializeObject(_guidID);
            
            //client.SendAsync(_jsonSerializer);
            return _jsonSerializer;
            
        }
        private Object DeSerialiserMethod(string data)
        {
            var _jsonDeserializer = JsonConvert.DeserializeObject(data);
            _Stopwatch.Stop();

            return _jsonDeserializer;
         }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (_TcpClient.Connected)
            {
                //closing the connection
                _networkStream = null ;
                _TcpClient.Close();
            }
        }


        
    }
}
