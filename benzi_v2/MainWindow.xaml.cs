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
using Modbus;
using System.IO;
using System.IO.Ports;
using Modbus.Device;
using System.Windows.Forms;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Media.Animation;

namespace benzi_v2
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Declarare parametri DAQ
        int[] DI2convert = { 0 };
        public bool[] DObits;
        public BitArray DIbits;


        public byte SHJ_digital_slaveID = 10;

        public ushort SHJ_digital_inputReg = 100;
        public ushort SHJ_digital_outputReg = 102;
        public SerialPort port;
        public IModbusSerialMaster modbus_master;
        ///////////


        Timer timer1;
        public static bool info = false;
        public static int b0;
        public static int b1;
        public static int b2;
        public static int b3;
        public static int b4;
        public static int b5;
        public static int b6;
        public static int b7;
        public static int b8;


        public MainWindow()
        {
            InitializeComponent();

            timer1 = new Timer();
            timer1.Tick += new EventHandler(refreshValues);
            timer1.Interval = 100;

            LampaH1.Visibility = Visibility.Visible;
            LampaH2.Visibility = Visibility.Visible;
            LampaH3.Visibility = Visibility.Visible;
            LampaH4.Visibility = Visibility.Visible;

            ///
            // Connect();
            ///
        }

        public void Connect()
        {
            try
            {
                port = new SerialPort("COM1");
                port.BaudRate = 115200;
                port.Parity = Parity.None;
                port.StopBits = StopBits.One;
                port.WriteTimeout = 500;
                port.ReadTimeout = 500;
                port.Handshake = Handshake.None;
                port.Open();

                ushort INIT_OUTPUT = 65535;

                modbus_master = ModbusSerialMaster.CreateRtu(port);
                modbus_master.WriteSingleRegister(SHJ_digital_slaveID, SHJ_digital_outputReg, INIT_OUTPUT); //== all outputs are set LOW MAX = 65535

                timer1.Start();
            }
            catch
            {
                MessageBoxResult result =
                    System.Windows.MessageBox.Show("Eroare conexiune. Redeschidem aplicatia?", "Confirmare",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    System.Windows.Forms.Application.Restart();
                    Close();
                }
                if (result == MessageBoxResult.No)
                {

                }
            }
        }

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
                //_infoWindow.blurEffect.Radius = 10;

                info = true;

            }
        }

        //read Digital Outputs
        public int Get_DO()
        {
            return 0;
            ///
         //  return (int) modbus_master.ReadHoldingRegisters(SHJ_digital_slaveID, SHJ_digital_outputReg, 1)[0];
            ///
        }

        //read Digital Inputs
        public int Get_DI()
        {
            int DI = modbus_master.ReadHoldingRegisters(SHJ_digital_slaveID, SHJ_digital_inputReg, 1)[0];
            DI2convert[0] = DI;

            DIbits = new BitArray(DI2convert);
            DIbits.Not();

            return DI;
        }

        private void setDOBit(int bit)
        {
            int DO = Get_DO();

            int[] bits = Utils.decimalToBits(DO);

            bits[15 - bit] = 0;

            int newDO = Utils.bitsToDecimal(bits);
            ///
           // modbus_master.WriteSingleRegister(SHJ_digital_slaveID, SHJ_digital_outputReg, (ushort)(newDO));
            ///
        }

        private void resetDOBit(int bit)
        {
            int DO = Get_DO();

            int[] bits = Utils.decimalToBits(DO);

            bits[15 - bit] = 1;

            int newDO = Utils.bitsToDecimal(bits);
            ///
         //  modbus_master.WriteSingleRegister(SHJ_digital_slaveID, SHJ_digital_outputReg, (ushort)(newDO));
            ///
        }

        public void CreateAPath()
        {

            // Create a blue and a black Brush

            SolidColorBrush blueBrush = new SolidColorBrush();

            blueBrush.Color = Colors.Blue;

            SolidColorBrush blackBrush = new SolidColorBrush();

            blackBrush.Color = Colors.Black;
        }

        private void S0_button_TouchDown(object sender, TouchEventArgs e)
        {
            if (b0 == 0)
            {
                b0 = 1;
            }
            else
            {
                b0 = 0;
            }
        }

        private void S0_button_TouchUp(object sender, TouchEventArgs e)
        {
            Get_DO();
        }

        private void S1_button_TouchDown(object sender, TouchEventArgs e)
        {
            if (b1 == 0)
            {
                b1 = 1;
            }
            else
            {
                b1 = 0;
            }
        }

        private void S1_button_TouchUp(object sender, TouchEventArgs e)
        {
            Get_DO();
        }

        private void S2_button_TouchDown(object sender, TouchEventArgs e)
        {
            if (b2 == 0)
            {
                b2 = 1;
            }
            else
            {
                b2 = 0;
            }
        }

        private void S2_button_TouchUp(object sender, TouchEventArgs e)
        {
            Get_DO();
        }

        private void S3_button_TouchDown(object sender, TouchEventArgs e)
        {
            if (b3 == 0)
            {
                b3 = 1;
            }
            else
            {
                b3 = 0;
            }
        }

        private void S3_button_TouchUp(object sender, TouchEventArgs e)
        {
            Get_DO();
        }

        private void S4_button_TouchDown(object sender, TouchEventArgs e)
        {
            if (b4 == 0)
            {
                b4 = 1;
            }
            else
            {
                b4 = 0;
            }
        }

        private void S4_button_TouchUp(object sender, TouchEventArgs e)
        {
            Get_DO();
        }

        private void S5_button_TouchDown(object sender, TouchEventArgs e)
        {
            Random r = new Random();
            int rInt = r.Next(1, 3);
            if (b5 == 0)
            {
                b5 = rInt;
            }
            else
            {
                b5 = 0;
            }

        }

        private void S5_button_TouchUp(object sender, TouchEventArgs e)
        {
            Get_DO();
        }

        private void S6_button_TouchDown(object sender, TouchEventArgs e)
        {
            if (b6 == 0)
            {
                b6 = 1;
            }
            else
            {
                b6 = 0;
            }
        }

        private void S6_button_TouchUp(object sender, TouchEventArgs e)
        {
            Get_DO();
        }

        private void S7_button_TouchDown(object sender, TouchEventArgs e)
        {
            if (b7 == 0)
            {
                b7 = 1;
            }
            else
            {
                b7 = 0;
            }
        }

        private void S7_button_TouchUp(object sender, TouchEventArgs e)
        {
            Get_DO();
        }

        private void S8_button_TouchDown(object sender, TouchEventArgs e)
        {
            if (b8 == 0)
            {
                b8 = 1;
            }
            else
            {
                b8 = 0;
            }
        }

        private void S8_button_TouchUp(object sender, TouchEventArgs e)
        {
            Get_DO();
        }

        private void refreshValues(object sender, EventArgs e)
        {
            Get_DI();
            Get_DO();

            if (b0 == 0)
            {
                resetDOBit(0);
            }
            if (b0 == 1)
            {
                setDOBit(0);
            }

            if (b1 == 0)
            {
                resetDOBit(1);
            }
            if (b1 == 1)
            {
                setDOBit(1);
            }

            if (b2 == 0)
            {
                resetDOBit(2);
            }
            if (b2 == 1)
            {
                setDOBit(2);
            }

            if (b3 == 0)
            {
                resetDOBit(3);
            }
            if (b3 == 1)
            {
                setDOBit(3);
            }

            if (b4 == 0)
            {
                resetDOBit(4);
            }
            if (b4 == 1)
            {
                setDOBit(4);
            }

            if (b5 == 0)
            {
                resetDOBit(5);
            }
            if (b5 == 1)
            {
                setDOBit(5);
            }

            if (b6 == 0)
            {
                resetDOBit(6);
            }
            if (b6 == 1)
            {
                setDOBit(6);
            }

            if (b7 == 0)
            {
                resetDOBit(7);
            }
            if (b7 == 1)
            {
                setDOBit(7);
            }

            if (b8 == 0)
            {
                resetDOBit(8);
            }
            if (b8 == 1)
            {
                setDOBit(8);
            }

            if (DIbits[1])
            {
                LampaH1.Fill = Brushes.Green;
            }
            if (!DIbits[1])
            {
                LampaH1.Fill = Brushes.Red;
            }

            if (DIbits[2])
            {
                LampaH2.Fill = Brushes.Green;
            }
            if (!DIbits[2])
            {
                LampaH2.Fill = Brushes.Red;
            }

            if (DIbits[3])
            {
                LampaH3.Fill = Brushes.Green;
            }
            if (!DIbits[3])
            {
                LampaH3.Fill = Brushes.Red;
            }

            if (DIbits[0])
            {
                LampaH4.Fill = Brushes.Green;
            }
            if (!DIbits[0])
            {
                LampaH4.Fill = Brushes.Red;
            }

        }


    }
}