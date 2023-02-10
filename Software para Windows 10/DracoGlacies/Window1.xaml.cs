using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Shapes;

namespace DracoGlacies
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        
        String[] Vb = {"","","","","","" };
        
        int cont = 0;

        public Window1()
        {
            InitializeComponent();
            Console.WriteLine(((MainWindow)Application.Current.MainWindow).Config);
            for (int i = 0; i <= ((MainWindow)Application.Current.MainWindow).Config.Length - 1; i++)
            {
                if (!(((MainWindow)Application.Current.MainWindow).Config[i] == '-'))
                {
                    Vb[cont] = Vb[cont] + ((MainWindow)Application.Current.MainWindow).Config[i];
                }
                else
                {
                    cont = cont + 1;
                }
            }
            b1.Value = Convert.ToDouble(Vb[0]);
            b2.Value = Convert.ToDouble(Vb[1]);
            b3.Value = Convert.ToDouble(Vb[2]);
            b4.Value = Convert.ToDouble(Vb[3]);
            b5.Value = Convert.ToDouble(Vb[4]);
            b6.Value = Convert.ToDouble(Vb[5]);
        }

        private void configurar(object sender, RoutedEventArgs e)
        {

            

            ((MainWindow)Application.Current.MainWindow).Econfig = 2;
            ((MainWindow)Application.Current.MainWindow).Config = b1.Value+"-"+ b2.Value + "-" + b3.Value + "-" + b4.Value + "-" + b5.Value + "-" +b6.Value;
            using (StreamWriter outputfile = new StreamWriter("config.txt"))
            {
                outputfile.WriteLine(((MainWindow)Application.Current.MainWindow).Config);
            }
            ((MainWindow)Application.Current.MainWindow).WindowState = WindowState.Minimized;
            this.Close();
            
        }
    }
}
