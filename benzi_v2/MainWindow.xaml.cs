using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using EasyModbus;
using System.Windows.Forms;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Media.Animation;
using Modbus.Device;
using System.IO.Ports;

namespace benzi_v2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static bool info = false;//Functional Variables >TCP Modbus

        SolidColorBrush blak = new SolidColorBrush(Colors.Black);
        SolidColorBrush blu = new SolidColorBrush(Color.FromRgb(42, 172, 166));
        SolidColorBrush whit = new SolidColorBrush(Colors.White);
        SolidColorBrush darkgry = new SolidColorBrush(Color.FromRgb(68, 67, 67));

        ushort U1Val = 0;


        public int[] AI;
        public int[] DI;
        public int DO = 0;
        public bool[] DObits;
        public BitArray DIbits;
        public ModbusClient modbusClient;
        public Timer timer;
        public bool Connected = false;//Functional Variables <


        bool _force = false;//Functional Variables >RTU Modbus 

        //public ushort[] AI;
        //public ushort[] AO;
        //public ushort DI;
        //int[] DI2convert = { 0 };
        //public ushort DO;
        //public bool[] DObits;
        //public BitArray DIbits;
        //public SerialPort port;
        //public Timer timer;
        //public bool Connected = true;
        public byte SHJ_digital_slaveID = 10;
        public byte SHJ_analog_slaveID = 11;
        public ushort SHJ_digital_inputReg = 100;
        public ushort SHJ_digital_outputReg = 102;
        public ushort SHJ_analog_outputReg = 110;
        public ushort SHJ_analog_inputReg = 100;//MAX = 4095 = 10 V
        public IModbusSerialMaster modbus_master;//Functional Variables <


        public MainWindow()
        {
            InitializeComponent();
            //Conn();//!!!!!!!!!!!!!!!!!!!!!!!!!!!!COMMENT FOR TEST ONLY!!!!!!!!!!

            timer = new Timer();
            timer.Tick += new EventHandler(refresh_values);
            timer.Interval = 10;


        }
        public void Conn()
        {
            modbusClient = new ModbusClient("172.16.17.2", 502);
            modbusClient.Connect();

            modbusClient.WriteSingleRegister(0x1120, 0);
            modbusClient.WriteSingleRegister(2080, 0);
            AI = modbusClient.ReadHoldingRegisters(1, 1);
            DObits = new bool[] { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };


        }

        //public void refresh_analogTEXT()
        //{
        //    //U1 Text BlockS
        //    modbus_master.WriteSingleRegister(SHJ_analog_slaveID, SHJ_analog_outputReg, U1Val);



        //    if (U1Val == 10000 | U1Val > 10000)
        //    {
        //        HighVValue.Text = "10";
        //        LowVValue.Text = "0";
        //    }
        //    else
        //    {
        //        HighVValue.Text = (U1Val / 1000).ToString();
        //        LowVValue.Text = (U1Val % 1000).ToString();

        //    }

        //}


        private void Window_Closing(object sender, CancelEventArgs e) //MUST be MANUALLY linked
        {
            MessageBoxResult result =
                System.Windows.MessageBox.Show("Iesire din aplicatie?", "Confirmare iesire",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                string shortcutName = string.Concat(Environment.GetFolderPath(Environment.SpecialFolder.Programs),
                "\\Menu\\", "Menu", ".appref-ms");
                ProcessStartInfo openMenu = new ProcessStartInfo(shortcutName);
                openMenu.WindowStyle = ProcessWindowStyle.Maximized;
                Process.Start(openMenu);
                e.Cancel = false;
            }
            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }
        //read Digital Outputs
        public void Get_DO()
        {
            DO = modbusClient.ReadHoldingRegisters(2080, 1)[0];

        }

        //read Digital Inputs
        public void Get_DI()
        {
            DI = modbusClient.ReadHoldingRegisters(32, 2);
            DIbits = new BitArray(DI);
        }

        //setting Digital Outputs to HIGH
        public void Set_DO(int bit)
        {

            Get_DO();

            modbusClient.WriteSingleRegister(2080, DO + (2 ^ bit) - 2);

        }

        //setting Digital Outputs to LOW
        public void Reset_DO(int bit_set)
        {
            Get_DO();
            if (DO >= 0)
            {
                modbusClient.WriteSingleRegister(2080, DO - (2 ^ (bit_set - 1)));
            }
        }

        public void refresh_values(object sender, EventArgs e)
        {
            //Show the current status of the PLC OUTPUTS(inputs for the program)
            //No effect on the physical side if the OUTPUTS !!!only visual

            Get_DI();
            Get_DO();



            AnimationControl();

        }

        public void AnimationControl() { }

        private void button_Click(object sender, RoutedEventArgs e)//HomeButtonClick
        {
            this.Close();
        }

        private void INFO_button_Click(object sender, RoutedEventArgs e)
        {
            if (!info)
            {
                var _infoWindow = new benzi_v2.InfoWindow();
                _infoWindow.Show();
                blurEffect.Radius = 10;

                info = true;

            }
        }

        private void S0_button_TouchDown(object sender, TouchEventArgs e)
        {
            Get_DO();
            modbus_master_serial.WriteSingleRegister(SHJ_digital_slaveID, SHJ_digital_outputReg, (ushort)(DO - 2 ^ 0));
        }

        private void S0_button_TouchUp(object sender, TouchEventArgs e)
        {
            Get_DO();
            modbus_master_serial.WriteSingleRegister(SHJ_digital_slaveID, SHJ_digital_outputReg, (ushort)(DO - 2 ^ 0));
        }

        private void S1_button_TouchDown(object sender, TouchEventArgs e)
        {
            Get_DO();
            modbus_master_serial.WriteSingleRegister(SHJ_digital_slaveID, SHJ_digital_outputReg, (ushort)(DO - 2 ^ 1));
        }

        private void S1_button_TouchUp(object sender, TouchEventArgs e)
        {
            Get_DO();
            modbus_master_serial.WriteSingleRegister(SHJ_digital_slaveID, SHJ_digital_outputReg, (ushort)(DO - 2 ^ 1));
        }

        private void S2_button_TouchDown(object sender, TouchEventArgs e)
        {
            Get_DO();
            modbus_master_serial.WriteSingleRegister(SHJ_digital_slaveID, SHJ_digital_outputReg, (ushort)(DO - 2 ^ 2));
        }

        private void S2_button_TouchUp(object sender, TouchEventArgs e)
        {
            Get_DO();
            modbus_master_serial.WriteSingleRegister(SHJ_digital_slaveID, SHJ_digital_outputReg, (ushort)(DO - 2 ^ 2));
        }

        private void S3_button_TouchDown(object sender, TouchEventArgs e)
        {
            Get_DO();
            modbus_master_serial.WriteSingleRegister(SHJ_digital_slaveID, SHJ_digital_outputReg, (ushort)(DO - 2 ^ 2));
        }

        private void S3_button_TouchUp(object sender, TouchEventArgs e)
        {
            Get_DO();
            modbus_master_serial.WriteSingleRegister(SHJ_digital_slaveID, SHJ_digital_outputReg, (ushort)(DO - 2 ^ 2));
        }
    }
}
