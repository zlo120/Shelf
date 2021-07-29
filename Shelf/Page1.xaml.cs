using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {

        public DirectInput directInput;
        public Guid joystickGuid;
        public Joystick joystick;
        bool DS4 = false;
        System.Timers.Timer aTimer = new System.Timers.Timer();
        Window main = (MainWindow)Application.Current.MainWindow;

        public Page1()
        {
            InitializeComponent();
            this.Cursor = System.Windows.Input.Cursors.None;
            Right.Visibility = Visibility.Visible;
            // Initialize DirectInput
            directInput = new DirectInput();

            // Find a Joystick Guid
            joystickGuid = Guid.Empty;

            foreach (var deviceInstance in directInput.GetDevices(DeviceType.Gamepad,
                        DeviceEnumerationFlags.AllDevices))
                joystickGuid = deviceInstance.InstanceGuid;

            // If Gamepad not found, look for a Joystick
            if (joystickGuid == Guid.Empty)
                foreach (var deviceInstance in directInput.GetDevices(DeviceType.Joystick,
                        DeviceEnumerationFlags.AllDevices))
                    joystickGuid = deviceInstance.InstanceGuid;

            // If Joystick not found, throws an error
            if (joystickGuid == Guid.Empty)
            {
                Process.Start("C:\\Users\\Zac Lo\\3D Objects\\DS4Windows\\DS4Windows.exe");
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                DS4 = true;
                Environment.Exit(1);
            }

            // Instantiate the joystick
            joystick = new Joystick(directInput, joystickGuid);

            // Query all suported ForceFeedback effects
            var allEffects = joystick.GetEffects();


            // Set BufferSize in order to use buffered data.
            joystick.Properties.BufferSize = 128;

            // Acquire the joystick
            joystick.Acquire();

            _1B.StrokeThickness = 4;
            SolidColorBrush b = new SolidColorBrush();
            b.Color = Color.FromRgb(51, 255, 51);
            _1B.Stroke = b;
            _1B.Visibility = Visibility.Visible;
            _2B.Visibility = Visibility.Visible;
            _3B.Visibility = Visibility.Visible;
            A1.Visibility = Visibility.Visible;


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

                        switch (uInput)
                        {
                            // BEGIN LOGIC FOR NAVIGATION
                            // Reminder: 0 RIGHT, 1 DOWN, 2 LEFT, 3 UP

                            //RIGHT
                            case 0:

                                switch (current)
                                {
                                    case 1:
                                        current = 2;


                                        outline.Color = Color.FromRgb(0, 0, 0);
                                        _1B.Stroke = outline;
                                        _1B.StrokeThickness = 1;

                                        A1.Visibility = Visibility.Hidden;
                                        A2.Visibility = Visibility.Visible;

                                        color.Color = Color.FromRgb(51, 255, 51);
                                        _2B.Stroke = color;
                                        _2B.StrokeThickness = 4;

                                        break;

                                    case 2:
                                        current = 3;

                                        outline.Color = Color.FromRgb(0, 0, 0);
                                        _2B.Stroke = outline;
                                        _2B.StrokeThickness = 1;

                                        A2.Visibility = Visibility.Hidden;
                                        A3.Visibility = Visibility.Visible;

                                        color.Color = Color.FromRgb(51, 255, 51);
                                        _3B.Stroke = color;
                                        _3B.StrokeThickness = 4;

                                        break;

                                    case 3:
                                        /*
                                        current = 4;

                                        outline.Color = Color.FromRgb(0, 0, 0);
                                        _3B.Stroke = outline;
                                        _3B.StrokeThickness = 1;

                                        A3.Visibility = Visibility.Hidden;
                                        A4.Visibility = Visibility.Visible;

                                        color.Color = Color.FromRgb(51, 255, 51);
                                        _4B.Stroke = color;
                                        _4B.StrokeThickness = 4;
                                        */
                                        aTimer.Close();
                                        main.Content = new Page2();
                                        Thread.Sleep(1000);
                                        break;

                                    case 4:
                                        current = 5;

                                        outline.Color = Color.FromRgb(0, 0, 0);
                                        _4B.Stroke = outline;
                                        _4B.StrokeThickness = 1;

                                        A4.Visibility = Visibility.Hidden;
                                        A5.Visibility = Visibility.Visible;

                                        color.Color = Color.FromRgb(51, 255, 51);
                                        _5B.Stroke = color;
                                        _5B.StrokeThickness = 4;

                                        break;

                                    case 5:
                                        current = 6;

                                        outline.Color = Color.FromRgb(0, 0, 0);
                                        _5B.Stroke = outline;
                                        _5B.StrokeThickness = 1;

                                        A5.Visibility = Visibility.Hidden;
                                        A6.Visibility = Visibility.Visible;

                                        color.Color = Color.FromRgb(51, 255, 51);
                                        _6B.Stroke = color;
                                        _6B.StrokeThickness = 4;

                                        break;

                                    case 6:
                                        /*
                                        outline.Color = Color.FromRgb(0, 0, 0);
                                        _6B.Stroke = outline;
                                        _6B.StrokeThickness = 1;

                                        A6.Visibility = Visibility.Hidden;
                                        A1.Visibility = Visibility.Visible;

                                        color.Color = Color.FromRgb(51, 255, 51);
                                        _1B.Stroke = color;
                                        _1B.StrokeThickness = 4;
                                        */
                                        aTimer.Close();
                                        main.Content = new Page2();
                                        Thread.Sleep(1000);

                                        break;

                                }
                                break;

                            //DOWN
                            case 1:
                                switch (current)
                                {
                                    case 1:
                                        current = 4;


                                        outline.Color = Color.FromRgb(0, 0, 0);
                                        _1B.Stroke = outline;
                                        _1B.StrokeThickness = 1;

                                        A1.Visibility = Visibility.Hidden;
                                        A4.Visibility = Visibility.Visible;

                                        color.Color = Color.FromRgb(51, 255, 51);
                                        _4B.Stroke = color;
                                        _4B.StrokeThickness = 4;

                                        break;

                                    case 2:
                                        current = 5;

                                        outline.Color = Color.FromRgb(0, 0, 0);
                                        _2B.Stroke = outline;
                                        _2B.StrokeThickness = 1;

                                        A2.Visibility = Visibility.Hidden;
                                        A5.Visibility = Visibility.Visible;

                                        color.Color = Color.FromRgb(51, 255, 51);
                                        _5B.Stroke = color;
                                        _5B.StrokeThickness = 4;

                                        break;

                                    case 3:
                                        current = 6;

                                        outline.Color = Color.FromRgb(0, 0, 0);
                                        _3B.Stroke = outline;
                                        _3B.StrokeThickness = 1;

                                        A3.Visibility = Visibility.Hidden;
                                        A6.Visibility = Visibility.Visible;

                                        color.Color = Color.FromRgb(51, 255, 51);
                                        _6B.Stroke = color;
                                        _6B.StrokeThickness = 4;

                                        break;
                                }

                                break;

                            //LEFT
                            case 2:

                                switch (current)
                                {
                                    case 1:
                                        current = 6;


                                        outline.Color = Color.FromRgb(0, 0, 0);

                                        _1B.Stroke = outline;
                                        _1B.StrokeThickness = 1;

                                        A1.Visibility = Visibility.Hidden;
                                        A6.Visibility = Visibility.Visible;

                                        color.Color = Color.FromRgb(51, 255, 51);
                                        _6B.Stroke = color;
                                        _6B.StrokeThickness = 4;

                                        break;

                                    case 2:
                                        current = 1;

                                        outline.Color = Color.FromRgb(0, 0, 0);
                                        _2B.Stroke = outline;
                                        _2B.StrokeThickness = 1;

                                        A2.Visibility = Visibility.Hidden;
                                        A1.Visibility = Visibility.Visible;

                                        color.Color = Color.FromRgb(51, 255, 51);
                                        _1B.Stroke = color;
                                        _1B.StrokeThickness = 4;

                                        break;

                                    case 3:
                                        current = 2;

                                        outline.Color = Color.FromRgb(0, 0, 0);
                                        _3B.Stroke = outline;
                                        _3B.StrokeThickness = 1;

                                        A3.Visibility = Visibility.Hidden;
                                        A2.Visibility = Visibility.Visible;

                                        color.Color = Color.FromRgb(51, 255, 51);
                                        _2B.Stroke = color;
                                        _2B.StrokeThickness = 4;

                                        break;

                                    case 4:
                                        current = 3;

                                        outline.Color = Color.FromRgb(0, 0, 0);
                                        _4B.Stroke = outline;
                                        _4B.StrokeThickness = 1;

                                        A4.Visibility = Visibility.Hidden;
                                        A3.Visibility = Visibility.Visible;

                                        color.Color = Color.FromRgb(51, 255, 51);
                                        _3B.Stroke = color;
                                        _3B.StrokeThickness = 4;

                                        break;

                                    case 5:
                                        current = 4;

                                        outline.Color = Color.FromRgb(0, 0, 0);
                                        _5B.Stroke = outline;
                                        _5B.StrokeThickness = 1;

                                        A5.Visibility = Visibility.Hidden;
                                        A4.Visibility = Visibility.Visible;

                                        color.Color = Color.FromRgb(51, 255, 51);
                                        _4B.Stroke = color;
                                        _4B.StrokeThickness = 4;

                                        break;

                                    case 6:
                                        current = 5;

                                        outline.Color = Color.FromRgb(0, 0, 0);
                                        _6B.Stroke = outline;
                                        _6B.StrokeThickness = 1;

                                        A6.Visibility = Visibility.Hidden;
                                        A5.Visibility = Visibility.Visible;

                                        color.Color = Color.FromRgb(51, 255, 51);
                                        _5B.Stroke = color;
                                        _5B.StrokeThickness = 4;

                                        break;


                                }

                                break;

                            //UP
                            case 3:

                                switch (current)
                                {

                                    case 4:
                                        current = 1;

                                        outline.Color = Color.FromRgb(0, 0, 0);
                                        _4B.Stroke = outline;
                                        _4B.StrokeThickness = 1;

                                        A4.Visibility = Visibility.Hidden;
                                        A1.Visibility = Visibility.Visible;

                                        color.Color = Color.FromRgb(51, 255, 51);
                                        _1B.Stroke = color;
                                        _1B.StrokeThickness = 4;

                                        break;

                                    case 5:
                                        current = 2;

                                        outline.Color = Color.FromRgb(0, 0, 0);
                                        _5B.Stroke = outline;
                                        _5B.StrokeThickness = 1;

                                        A5.Visibility = Visibility.Hidden;
                                        A2.Visibility = Visibility.Visible;

                                        color.Color = Color.FromRgb(51, 255, 51);
                                        _2B.Stroke = color;
                                        _2B.StrokeThickness = 4;

                                        break;

                                    case 6:
                                        current = 3;

                                        outline.Color = Color.FromRgb(0, 0, 0);
                                        _6B.Stroke = outline;
                                        _6B.StrokeThickness = 1;

                                        A6.Visibility = Visibility.Hidden;
                                        A3.Visibility = Visibility.Visible;

                                        color.Color = Color.FromRgb(51, 255, 51);
                                        _3B.Stroke = color;
                                        _3B.StrokeThickness = 4;

                                        break;


                                }

                                break;

                            //A or DS4 X
                            case 10:


                                switch (current)
                                {
                                    case 1:


                                        Process.Start("C:\\Users\\Zac Lo\\Documents\\CloseDS4\\FO.pyw");
                                        System.Windows.Application.Current.Shutdown();
                                        break;

                                    case 2:

                                        Process.Start("C:\\Users\\Zac Lo\\Documents\\CloseDS4\\Dauntless.pyw");
                                        System.Windows.Application.Current.Shutdown();

                                        break;

                                    case 3:

                                        Process.Start("C:\\Users\\Zac Lo\\Documents\\CloseDS4\\Batman.pyw");
                                        System.Windows.Application.Current.Shutdown();

                                        break;

                                    case 4:

                                        Process.Start("C:\\Users\\Zac Lo\\Documents\\CloseDS4\\Avengers.pyw");
                                        System.Windows.Application.Current.Shutdown();

                                        break;

                                    case 5:

                                        Process.Start("C:\\Users\\Zac Lo\\Documents\\CloseDS4\\Naruto.pyw");
                                        System.Windows.Application.Current.Shutdown();

                                        break;

                                    case 6:

                                        Process.Start("C:\\Users\\Zac Lo\\Documents\\CloseDS4\\LGSW.pyw");
                                        System.Windows.Application.Current.Shutdown();
                                        break;

                                }

                                break;

                            //B or DS4 O
                            case 20:
                                Process.Start("C:\\Users\\Zac Lo\\Documents\\CloseDS4\\close.pyw");
                                System.Windows.Application.Current.Shutdown();
                                break;
                        }
                    }));

            };


            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }
    }

}

