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
    /// Interaction logic for Exit.xaml
    /// </summary>
    public partial class Exit : Window
    {
        public DirectInput directInput;
        public Guid joystickGuid;
        public Joystick joystick;
        System.Timers.Timer aTimer = new System.Timers.Timer();
        public Exit()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            SetTimer();
        }

        public void SetTimer()
        {

            int uInput = -1;
            int current = 1;
            
            aTimer.Interval = 500;
            // Hook up the Elapsed event for the timer. 
            //aTimer.Elapsed += Read
            aTimer.Elapsed += delegate
            {
                //READING
                while (true)
                {
                    joystick.Poll();
                    var datas = joystick.GetBufferedData();
                    foreach (var state in datas)
                    {
                        //START arrows  WORKING: Checks which arrow was inputted
                        if (state.Offset == JoystickOffset.PointOfViewControllers0)
                        {
                            if (state.ToString().Contains("Value: 9"))
                            {
                                uInput = 0;
                                goto Selected;
                            }

                            if (state.ToString().Contains("Value: 18"))
                            {
                                uInput = 1;
                                goto Selected;
                            }

                            if (state.ToString().Contains("Value: 27"))
                            {
                                uInput = 2;
                                goto Selected;
                            }

                            if (!(state.ToString().Contains("Value: 9") || state.ToString().Contains("Value: 18") || state.ToString().Contains("Value: 27") || state.ToString().Contains("Value: -1")))
                            {
                                uInput = 3;
                                goto Selected;
                            }


                        }
                        // END arrows /

                        //START buttons
                        if (state.Offset == JoystickOffset.Buttons0 && !state.ToString().Contains("Value: 0"))
                        {

                            //A;
                            uInput = 10;
                            goto Selected;
                        }
                        if (state.Offset == JoystickOffset.Buttons1 && !state.ToString().Contains("Value: 0"))
                        {

                            //B;
                            uInput = 20;
                            goto Selected;
                        }
                        if (state.Offset == JoystickOffset.Buttons2 && !state.ToString().Contains("Value: 0"))
                        {

                            //X;
                            uInput = 30;
                            goto Selected;
                        }
                        if (state.Offset == JoystickOffset.Buttons3 && !state.ToString().Contains("Value: 0"))
                        {

                            //Y;
                            uInput = 40;
                            goto Selected;
                        }
                    }
                };

            Selected:
                Dispatcher.BeginInvoke(
                    new ThreadStart(() =>
                    {

                        string path;

                        SolidColorBrush color = new SolidColorBrush();
                        SolidColorBrush outline = new SolidColorBrush();
                        SolidColorBrush fill = new SolidColorBrush();

                        switch (uInput)
                        {
                            // BEGIN LOGIC FOR NAVIGATION
                            // Reminder: 0 RIGHT, 1 DOWN, 2 LEFT, 3 UP
                            case 10:

                                aTimer.Stop();
                                System.Windows.Application.Current.Shutdown();
                                break;

                            case 20:

                                this.Close();
                                break;

                                MainWindow main = new MainWindow();
                                main.Show();

                        }

                        //_1B.Fill = color;
                    }));

            };


            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }
    }
}
