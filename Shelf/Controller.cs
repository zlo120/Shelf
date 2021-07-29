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

namespace Shelf
{
    public class Controller
    {
        public DirectInput directInput;
        public Guid joystickGuid;
        public Joystick joystick;
        public Controller()
        {
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
                //MessageBox.Show("No joystick/Gamepad found.");
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
        }


        /*
         
         
        READ 

         
         */


        public void Read(Joystick joystick)
        {

            // Poll events from joystick
            
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
                            MessageBox.Show("Right arrow was clicked");
                            break;
                        }

                        if (state.ToString().Contains("Value: 18"))
                        {
                            MessageBox.Show("Down arrow was clicked");
                            break;
                        }

                        if (state.ToString().Contains("Value: 27"))
                        {
                            MessageBox.Show("Left arrow was clicked");
                            break;
                        }

                        if (!(state.ToString().Contains("Value: 9") || state.ToString().Contains("Value: 18") || state.ToString().Contains("Value: 27") || state.ToString().Contains("Value: -1")))
                        {
                            MessageBox.Show("Up arrow was clicked");
                            break;
                        }


                    }
                    // END arrows /

                    //START buttons
                    if (state.Offset == JoystickOffset.Buttons0 && !state.ToString().Contains("Value: 0"))
                    {
                        MessageBox.Show("A");
                        break;
                    }
                    if (state.Offset == JoystickOffset.Buttons1 && !state.ToString().Contains("Value: 0"))
                    {
                        MessageBox.Show("B");
                        break;
                    }
                    if (state.Offset == JoystickOffset.Buttons2 && !state.ToString().Contains("Value: 0"))
                    {
                        MessageBox.Show("X");
                        break;
                    }
                    if (state.Offset == JoystickOffset.Buttons3 && !state.ToString().Contains("Value: 0"))
                    {
                        MessageBox.Show("Y");
                        break;
                    }
                }
            }
        }

        private void Selector()
        {

        }


    }
}
