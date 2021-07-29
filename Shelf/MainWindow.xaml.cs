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
using SharpDX.DirectInput;
using System.Timers;
using System.Diagnostics;

namespace Shelf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public void setPage(int p)
        {
            switch (p)
            {
                case 1:
                    Main.Content = new Page1();
                    break;

                case 2:
                    Main.Content = new Page2();
                    break;
            }
        }

        public MainWindow()
        {
            
            InitializeComponent();
            Main.Content = new Page1();
        }

                    
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Clicked");
        }

    }
}

