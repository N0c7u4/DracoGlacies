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
using System.Threading;
using OpenHardwareMonitor.Hardware;
using System.IO.Ports;
using System.Management;
using System.IO;

namespace DracoGlacies
{

    public partial class MainWindow : Window
    {
        public int CPU = 0, GPU = 0, des = 0;
        Boolean On = false;
        public string NameGPU,Config;
        public Boolean conexion = false;
        SerialPort _serialPort;
        public int  Econfig = 0;




        public MainWindow()
        {


            using (StreamReader leer = new StreamReader("config.txt"))
            {
                Config = leer.ReadLine();
            }
            Console.WriteLine("Configuracion Guardada");
            Console.WriteLine(Config);

            Thread t = new Thread(calculo);
            t.Start();

            InitializeComponent();

        }

        



        public void calculo()
        {
            Console.WriteLine("Detectando el Dispositivo...");
            conexion =AutodetectArduinoPort();
            if (!conexion)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Conecte el soporte de la laptop para su optimo funcionamiento", "Dispositivo no Conectado", MessageBoxButton.OK, MessageBoxImage.Error);
                if (!MessageBoxButton.OK.Equals(result))
                {
                    Application.Current.Dispatcher.Invoke(() => { this.Close(); });
                }
            }
            while (conexion)
            {
                
                GetSystemInfo();
                if(Econfig == 1)
                {
                    if(CPU>GPU)
                    {
                        automatic(CPU);
                    }
                    if(GPU>=CPU)
                    {
                        automatic(GPU);
                    }
                }
                else
                {
                    if(Econfig == 2)
                    {
                        if (CPU > GPU)
                        {
                            manual(CPU);
                        }
                        if (GPU >= CPU)
                        {
                            manual(GPU);
                        }
                    }
                    else
                    {
                        _serialPort.WriteLine(Convert.ToString(Convert.ToInt32(On)) + "-" + Convert.ToString(0));
                    }
                }
                Thread.Sleep(1000);
            }
        }


        public class UpdateVisitor : IVisitor
        {
            public void VisitComputer(IComputer computer)
            {
                computer.Traverse(this);
            }
            public void VisitHardware(IHardware hardware)
            {
                hardware.Update();
                foreach (IHardware subHardware in hardware.SubHardware) subHardware.Accept(this);
            }
            public void VisitSensor(ISensor sensor) { }
            public void VisitParameter(IParameter parameter) { }
        }

        private void per(object sender, RoutedEventArgs e)
        {
            Window1 WindowsPersonalizar = new Window1();
            WindowsPersonalizar.Owner = this;
            WindowsPersonalizar.ShowDialog();
        }

        public void manual(int chip)
        {
            String[] acum= {"","","","","","" };
            int cont = 0;

            for (int i = 0; i <= Config.Length - 1; i++)
            {
                if (!(Config[i] == '-'))
                {
                    acum[cont] = acum[cont] + Config[i];
                }
                else
                {
                    cont = cont + 1;
                }
            }



            int pwm;
            if (40 <= chip && chip < 50)
            {
                pwm = (int)(2.55 * Convert.ToDouble(acum[0]));
                _serialPort.WriteLine(Convert.ToInt32(On)+"-" + pwm);
            }
            if (50 <= chip && chip < 60)
            {
                pwm = (int)(2.55 * Convert.ToDouble(acum[1]));
                _serialPort.WriteLine(Convert.ToInt32(On) + "-" + pwm);
            }
            if (60 <= chip && chip < 70)
            {
                pwm = (int)(2.55 * Convert.ToDouble(acum[2]));
                _serialPort.WriteLine(Convert.ToInt32(On) + "-" + pwm);
            }
            if (70 <= chip && chip < 80)
            {
                pwm = (int)(2.55 * Convert.ToDouble(acum[3]));
                _serialPort.WriteLine(Convert.ToInt32(On) + "-" + pwm);
            }
            if (80 <= chip && chip < 90)
            {
                pwm = (int)(2.55 * Convert.ToDouble(acum[4]));
                _serialPort.WriteLine(Convert.ToInt32(On) + "-" + pwm);
            }
            if (90 <= chip && chip < 100)
            {
                pwm = (int)(2.55 * Convert.ToDouble(acum[5]));
                _serialPort.WriteLine(Convert.ToInt32(On) + "-" + pwm);
            }
        }

        private void onoff(object sender, RoutedEventArgs e)
        {
            On = !On;
            if(On)
            {
                on_off.Content = "On";
            }
            else
            {
                on_off.Content = "Off";
            }
        }

        public void automatic(int cpu)
        {
            int pwm;
            if(40<=cpu && cpu<50)
            {
                pwm = (int)(255.0 * 0.17);
                Console.WriteLine(Convert.ToString(Convert.ToInt32(On)) + "-" + Convert.ToString(pwm));
                _serialPort.WriteLine(Convert.ToString(Convert.ToInt32(On)) + "-" + Convert.ToString(pwm));
            }
            if (50 <= cpu && cpu < 60)
            {
                pwm = (int)(255.0 * 0.34);
                Console.WriteLine(Convert.ToString(Convert.ToInt32(On)) + "-" + Convert.ToString(pwm));
                _serialPort.WriteLine(Convert.ToString(Convert.ToInt32(On)) + "-" + Convert.ToString(pwm));
            }
            if (60 <= cpu && cpu < 70)
            {
                pwm = (int)(255.0 * 0.51);
                Console.WriteLine(Convert.ToString(Convert.ToInt32(On)) + "-" + Convert.ToString(pwm));
                _serialPort.WriteLine(Convert.ToString(Convert.ToInt32(On)) + "-" + Convert.ToString(pwm));
            }
            if (70 <= cpu && cpu < 80)
            {
                pwm = (int)(255.0 * 0.68);
                Console.WriteLine(Convert.ToString(Convert.ToInt32(On)) + "-" + Convert.ToString(pwm));
                _serialPort.WriteLine(Convert.ToString(Convert.ToInt32(On)) + "-" + Convert.ToString(pwm));
            }
            if (80 <= cpu && cpu < 90)
            {
                pwm = (int)(255.0 * 0.85);
                Console.WriteLine(Convert.ToString(Convert.ToInt32(On)) + "-" + Convert.ToString(pwm));
                _serialPort.WriteLine(Convert.ToString(Convert.ToInt32(On)) + "-" + Convert.ToString(pwm));
            }
            if (90 <= cpu && cpu < 100)
            {
                pwm = (int)(255.0 * 1);
                Console.WriteLine(Convert.ToString(Convert.ToInt32(On)) + "-" + Convert.ToString(pwm));
                _serialPort.WriteLine(Convert.ToString(Convert.ToInt32(On)) + "-" + Convert.ToString(pwm));
            }
        }

        private void auto(object sender, RoutedEventArgs e)
        {
            Econfig = 1;
            MessageBoxResult result = System.Windows.MessageBox.Show("Configurado Exitosamente", "Configuracion Automatica", MessageBoxButton.OK);
            WindowState = WindowState.Minimized;
        }

        public void GetSystemInfo()
        {
            UpdateVisitor updateVisitor = new UpdateVisitor();
            Computer computer = new Computer();
            computer.Open();
            computer.CPUEnabled = true;
            computer.GPUEnabled = true;
            computer.Accept(updateVisitor);
            for (int i = 0; i < computer.Hardware.Length; i++)
            {
                if (computer.Hardware[i].HardwareType == HardwareType.CPU || computer.Hardware[i].HardwareType == HardwareType.GpuNvidia)
                {
                    for (int j = 0; j < computer.Hardware[i].Sensors.Length; j++)
                    {
                        if (computer.Hardware[i].Sensors[j].SensorType == SensorType.Temperature)
                        {
                            if (computer.Hardware[i].Sensors[j].Name == "CPU Package")
                            {
                                CPU = (int)computer.Hardware[i].Sensors[j].Value;
                                Application.Current.Dispatcher.Invoke(() => { CPU_T.Text = "CPU ºC =  " + CPU; });
                            }
                            if (computer.Hardware[i].Sensors[j].Name == "GPU Core")
                            {
                                GPU = (int)computer.Hardware[i].Sensors[j].Value;
                                Application.Current.Dispatcher.Invoke(() => { GPU_T.Text = "GPU ºC =  " + GPU; });
                            }
                            if (computer.Hardware[i].HardwareType == HardwareType.GpuNvidia && des == 0)
                            {
                                des = 1;
                                NameGPU = (string)computer.Hardware[i].Name;
                                Application.Current.Dispatcher.Invoke(() => { TargCPU.Text = NameGPU; });
                            }
                        }

                    }

                }

            }

            computer.Close();
        }


        public Boolean AutodetectArduinoPort()
        {
            

            try
            {
                for (int i=1;i<256;i++)
                {

                    string deviceId = "COM"+i;
                    Console.WriteLine("Dispositivos listados...");
                    Console.WriteLine(deviceId);
                    try
                    {
                        _serialPort = new SerialPort(); 
                        _serialPort.PortName = deviceId;
                        _serialPort.BaudRate = 9600;
                        _serialPort.DtrEnable = true;
                        _serialPort.Open();

                        _serialPort.WriteLine("1210717");
                        string serialLine = _serialPort.ReadLine();
                        Console.WriteLine(serialLine);
                        if (serialLine == "1210717")
                        {
                            Console.WriteLine("Arduino Encontrado");
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Dispositivo no Compatible");
                            _serialPort.Close();
                        }

                    }
                    catch
                    {
                        Console.WriteLine("Error En la Busqueda del Dispositivo... -> COM Buscado: "+deviceId);
                    }
                    

                    
                }
            }
            catch (ManagementException e)
            {
                /* Do Nothing */
            }

            return false;
        }


       

        

    }

}
