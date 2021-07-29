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
    /// Interaction logic for Page2.xaml
    /// </summary>
    public partial class Page2 : Page
    {
        bool DS4 = false;
        public DirectInput directInput;
        public Guid joystickGuid;
        public Joystick joystick;
        System.Timers.Timer aTimer = new System.Timers.Timer();
        Window main = (MainWindow)Application.Current.MainWindow;

        public Page2()
        {
            InitializeComponent();
            this.Cursor = System.Windows.Input.Cursors.None;

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

            _7B.StrokeThickness = 4;
            SolidColorBrush b = new SolidColorBrush();
            b.Color = Color.FromRgb(51, 255, 51);
            _7B.Stroke = b;
            _7B.Visibility = Visibility.Visible;
            _8B.Visibility = Visibility.Visible;
            _9B.Visibility = Visibility.Visible;
            A7.Visibility = Visibility.Visible;

            SetTimer();
        }

        public void SetTimer()
        {

            int uInput = -1;
            int current2 = 7;

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

                                switch (current2)
                                {
                                    case 7:
                                        current2 = 8;

                                        outline.Color = Color.FromRgb(0, 0, 0);
                                        _7B.Stroke = outline;
                                        _7B.StrokeThickness = 1;

                                        A7.Visibility = Visibility.Hidden;
                                        A8.Visibility = Visibility.Visible;

                                        color.Color = Color.FromRgb(51, 255, 51);
                                        _8B.Stroke = color;
                                        _8B.StrokeThickness = 4;

                                        break;

                                    case 8:
                                        current2 = 9;

                                        outline.Color = Color.FromRgb(0, 0, 0);
                                        _8B.Stroke = outline;
                                        _8B.StrokeThickness = 1;

                                        A8.Visibility = Visibility.Hidden;
                                        A9.Visibility = Visibility.Visible;

                                        color.Color = Color.FromRgb(51, 255, 51);
                                        _9B.Stroke = color;
                                        _9B.StrokeThickness = 4;

                                        break;

                                    case 9:
                                        
                                        current2= 10;

                                        outline.Color = Color.FromRgb(0, 0, 0);
                                        _9B.Stroke = outline;
                                        _9B.StrokeThickness = 1;

                                        A9.Visibility = Visibility.Hidden;
                                        A10.Visibility = Visibility.Visible;

                                        color.Color = Color.FromRgb(51, 255, 51);
                                        _10B.Stroke = color;
                                        _10B.StrokeThickness = 4;
                                        
                                        break;

                                    case 10:
                                        current2= 11;

                                        outline.Color = Color.FromRgb(0, 0, 0);
                                        _10B.Stroke = outline;
                                        _10B.StrokeThickness = 1;

                                        A10.Visibility = Visibility.Hidden;
                                        A11.Visibility = Visibility.Visible;

                                        color.Color = Color.FromRgb(51, 255, 51);
                                        _11B.Stroke = color;
                                        _11B.StrokeThickness = 4;

                                        break;

                                    case 11:
                                        current2= 12;

                                        outline.Color = Color.FromRgb(0, 0, 0);
                                        _11B.Stroke = outline;
                                        _11B.StrokeThickness = 1;

                                        A11.Visibility = Visibility.Hidden;
                                        A12.Visibility = Visibility.Visible;

                                        color.Color = Color.FromRgb(51, 255, 51);
                                        _12B.Stroke = color;
                                        _12B.StrokeThickness = 4;

                                        break;

                                    case 12:

                                        current2= 7;
                                        
                                        outline.Color = Color.FromRgb(0, 0, 0);
                                        _12B.Stroke = outline;
                                        _12B.StrokeThickness = 1;

                                        A12.Visibility = Visibility.Hidden;
                                        A7.Visibility = Visibility.Visible;

                                        color.Color = Color.FromRgb(51, 255, 51);
                                        _7B.Stroke = color;
                                        _7B.StrokeThickness = 4;
                                        

                                        break;
                                }
                                break;

                            //DOWN
                            case 1:
                                switch (current2)
                                {
                                    case 7:
                                        current2= 10;


                                        outline.Color = Color.FromRgb(0, 0, 0);
                                        _7B.Stroke = outline;
                                        _7B.StrokeThickness = 1;

                                        A7.Visibility = Visibility.Hidden;
                                        A10.Visibility = Visibility.Visible;

                                        color.Color = Color.FromRgb(51, 255, 51);
                                        _10B.Stroke = color;
                                        _10B.StrokeThickness = 4;

                                        break;

                                    case 8:
                                        current2= 11;

                                        outline.Color = Color.FromRgb(0, 0, 0);
                                        _8B.Stroke = outline;
                                        _8B.StrokeThickness = 1;

                                        A8.Visibility = Visibility.Hidden;
                                        A11.Visibility = Visibility.Visible;

                                        color.Color = Color.FromRgb(51, 255, 51);
                                        _11B.Stroke = color;
                                        _11B.StrokeThickness = 4;

                                        break;

                                    case 9:
                                        current2= 12;

                                        outline.Color = Color.FromRgb(0, 0, 0);
                                        _9B.Stroke = outline;
                                        _9B.StrokeThickness = 1;

                                        A9.Visibility = Visibility.Hidden;
                                        A12.Visibility = Visibility.Visible;

                                        color.Color = Color.FromRgb(51, 255, 51);
                                        _12B.Stroke = color;
                                        _12B.StrokeThickness = 4;

                                        break;
                                }

                                break;

                            //LEFT
                            case 2:

                                switch (current2)
                                {
                                    case 7:
                                        /*
                                        current2= 6;


                                        outline.Color = Color.FromRgb(0, 0, 0);

                                        _1B.Stroke = outline;
                                        _1B.StrokeThickness = 1;

                                        A1.Visibility = Visibility.Hidden;
                                        A6.Visibility = Visibility.Visible;

                                        color.Color = Color.FromRgb(51, 255, 51);
                                        _6B.Stroke = color;
                                        _6B.StrokeThickness = 4;
                                        */
                                        aTimer.Close();
                                        main.Content = new Page1();
                                        Thread.Sleep(1000);
                                        break;

                                    case 8:
                                        current2= 7;

                                        outline.Color = Color.FromRgb(0, 0, 0);
                                        _8B.Stroke = outline;
                                        _8B.StrokeThickness = 1;

                                        A8.Visibility = Visibility.Hidden;
                                        A7.Visibility = Visibility.Visible;

                                        color.Color = Color.FromRgb(51, 255, 51);
                                        _7B.Stroke = color;
                                        _7B.StrokeThickness = 4;

                                        break;

                                    case 9:
                                        current2= 8;

                                        outline.Color = Color.FromRgb(0, 0, 0);
                                        _9B.Stroke = outline;
                                        _9B.StrokeThickness = 1;

                                        A9.Visibility = Visibility.Hidden;
                                        A8.Visibility = Visibility.Visible;

                                        color.Color = Color.FromRgb(51, 255, 51);
                                        _8B.Stroke = color;
                                        _8B.StrokeThickness = 4;

                                        break;

                                    case 10:
                                        /*
                                        current2= 3;

                                        outline.Color = Color.FromRgb(0, 0, 0);
                                        _4B.Stroke = outline;
                                        _4B.StrokeThickness = 1;

                                        A4.Visibility = Visibility.Hidden;
                                        A3.Visibility = Visibility.Visible;

                                        color.Color = Color.FromRgb(51, 255, 51);
                                        _3B.Stroke = color;
                                        _3B.StrokeThickness = 4;
                                        */
                                        aTimer.Close();
                                        main.Content = new Page1();
                                        Thread.Sleep(1000);
                                        break;

                                    case 11:
                                        current2= 10;

                                        outline.Color = Color.FromRgb(0, 0, 0);
                                        _11B.Stroke = outline;
                                        _11B.StrokeThickness = 1;

                                        A11.Visibility = Visibility.Hidden;
                                        A10.Visibility = Visibility.Visible;

                                        color.Color = Color.FromRgb(51, 255, 51);
                                        _10B.Stroke = color;
                                        _10B.StrokeThickness = 4;

                                        break;

                                    case 12:
                                        current2= 11;

                                        outline.Color = Color.FromRgb(0, 0, 0);
                                        _12B.Stroke = outline;
                                        _12B.StrokeThickness = 1;

                                        A12.Visibility = Visibility.Hidden;
                                        A11.Visibility = Visibility.Visible;

                                        color.Color = Color.FromRgb(51, 255, 51);
                                        _11B.Stroke = color;
                                        _11B.StrokeThickness = 4;

                                        break;


                                }

                                break;

                            //UP
                            case 3:

                                switch (current2)
                                {

                                    case 10:
                                        current2= 7;

                                        outline.Color = Color.FromRgb(0, 0, 0);
                                        _10B.Stroke = outline;
                                        _10B.StrokeThickness = 1;

                                        A10.Visibility = Visibility.Hidden;
                                        A7.Visibility = Visibility.Visible;

                                        color.Color = Color.FromRgb(51, 255, 51);
                                        _7B.Stroke = color;
                                        _7B.StrokeThickness = 4;

                                        break;

                                    case 11:
                                        current2= 8;

                                        outline.Color = Color.FromRgb(0, 0, 0);
                                        _11B.Stroke = outline;
                                        _11B.StrokeThickness = 1;

                                        A11.Visibility = Visibility.Hidden;
                                        A8.Visibility = Visibility.Visible;

                                        color.Color = Color.FromRgb(51, 255, 51);
                                        _8B.Stroke = color;
                                        _8B.StrokeThickness = 4;

                                        break;

                                    case 12:
                                        current2= 9;

                                        outline.Color = Color.FromRgb(0, 0, 0);
                                        _12B.Stroke = outline;
                                        _12B.StrokeThickness = 1;

                                        A12.Visibility = Visibility.Hidden;
                                        A9.Visibility = Visibility.Visible;

                                        color.Color = Color.FromRgb(51, 255, 51);
                                        _9B.Stroke = color;
                                        _9B.StrokeThickness = 4;

                                        break;


                                }

                                break;

                            //A
                            case 10:

                                switch (current2)
                                {
                                    case 7:

                                        
                                        Process.Start("C:\\Users\\Zac Lo\\Documents\\CloseDS4\\P2\\FarCry5.pyw");
                                        System.Windows.Application.Current.Shutdown();
                                        break;

                                    case 8:

                                        Process.Start("C:\\Users\\Zac Lo\\Documents\\CloseDS4\\P2\\Mcc.pyw");
                                        System.Windows.Application.Current.Shutdown();

                                        break;

                                    case 9:

                                        Process.Start("C:\\Users\\Zac Lo\\Documents\\CloseDS4\\P2\\Horizon.pyw");
                                        System.Windows.Application.Current.Shutdown();

                                        break;

                                    case 10:

                                        Process.Start("C:\\Users\\Zac Lo\\Documents\\CloseDS4\\P2\\MWR.pyw"); 
                                        System.Windows.Application.Current.Shutdown();

                                        break;

                                    case 11:

                                        Process.Start("C:\\Users\\Zac Lo\\Documents\\CloseDS4\\P2\\WWZ.pyw");
                                        System.Windows.Application.Current.Shutdown();

                                        break;

                                    case 12:

                                        Process.Start("C:\\Users\\Zac Lo\\Documents\\CloseDS4\\P2\\SWTFU.pyw");
                                        System.Windows.Application.Current.Shutdown();
                                        break;

                                }

                                break;

                            //B
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
