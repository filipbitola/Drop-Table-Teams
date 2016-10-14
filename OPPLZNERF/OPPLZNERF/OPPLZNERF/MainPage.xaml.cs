using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Gaming.Input;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Net.Sockets;
using System.Net.Http;
using Windows.Networking.Sockets;
using Windows.Networking;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace OPPLZNERF
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        GamepadHandler gamepadHandler;
        Gamepad controller;
        DispatcherTimer dispatcherTimer;
        TimeSpan period = TimeSpan.FromMilliseconds(100);


        public MainPage()
        {
            this.InitializeComponent();

            GamepadHandler gamepadHandler = new GamepadHandler();

            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Start();

            Gamepad.GamepadAdded += Gamepad_GamepadAdded;
            Gamepad.GamepadRemoved += Gamepad_GamepadRemoved;
        }

        #region EventHandlers

        private async void Gamepad_GamepadAdded(object sender, Gamepad e)
        {
            e.HeadsetConnected += E_HeadsetConnected;
            e.HeadsetDisconnected += E_HeadsetDisconnected;
            e.UserChanged += E_UserChanged;

            var socket = new StreamSocket();
            string hostname = "";
            string port = "";
            await socket.ConnectAsync(new HostName(hostname), port);

            var cache = new AppCache();
            cache["socket"] = socket;

            await Log("Gamepad Added");
        }

        private async void Gamepad_GamepadRemoved(object sender, Gamepad e)
        {
            var cache = new AppCache();
            var socket = cache["socket"];
            if(socket != null)
            {
                await ((StreamSocket)socket).CancelIOAsync();
            }
            
            await Log("Gamepad Removed");
        }

        private async void E_UserChanged(IGameController sender, Windows.System.UserChangedEventArgs args)

        {
            await Log("User changed");
        }

        private async void E_HeadsetDisconnected(IGameController sender, Headset args)
        {
            await Log("HeadsetDisconnected");
        }

        private async void E_HeadsetConnected(IGameController sender, Headset args)
        {
            await Log("HeadsetConnected");
        }

        private void dispatcherTimer_Tick(object sender, object e)
        {

            if (Gamepad.Gamepads.Count > 0)
            {
                controller = Gamepad.Gamepads.First();
                var reading = controller.GetCurrentReading();

                gamepadHandler.ProcessReading(reading);

                pbLeftThumbstickX.Value = reading.LeftThumbstickX;
                pbLeftThumbstickY.Value = reading.LeftThumbstickY;
                pbRightThumbstickX.Value = reading.RightThumbstickX;
                pbRightThumbstickY.Value = reading.RightThumbstickY;
                pbLeftTrigger.Value = reading.LeftTrigger;
                pbRightTrigger.Value = reading.RightTrigger;

                Log($"Left Thumbstick X: {pbLeftThumbstickX.Value}");
                Log($"Left Thumbstick Y: {pbLeftThumbstickY.Value}");
                Log($"Right Thumbstick X: {pbRightThumbstickX.Value}");
                Log($"Right Thumbstick Y: {pbRightThumbstickY.Value}");
                Log($"Left Trigger: {pbLeftTrigger.Value}");
                Log($"Right Trigger: {pbLeftTrigger.Value}");



                //https://msdn.microsoft.com/en-us/library/windows/apps/windows.gaming.input.gamepadbuttons.aspx

                var mapping = new Dictionary<GamepadButtons, UIElement>
                {
                    {GamepadButtons.A, lblA },
                    {GamepadButtons.B, lblB },
                    {GamepadButtons.X, lblX },
                    {GamepadButtons.Y, lblY },
                    {GamepadButtons.Menu, lblMenu },
                    {GamepadButtons.DPadLeft, lblDPadLeft },
                    {GamepadButtons.DPadRight, lblDPadRight },
                    {GamepadButtons.DPadUp, lblDPadUp },
                    {GamepadButtons.DPadDown, lblDPadDown },
                    {GamepadButtons.View, lblView },
                    {GamepadButtons.RightThumbstick, ellRightThumbstick },
                    { GamepadButtons.LeftThumbstick, ellLeftThumbstick },
                    { GamepadButtons.LeftShoulder, rectLeftShoulder},
                    { GamepadButtons.RightShoulder, recRightShoulder}
                };

                foreach(var kvp in mapping)
                {
                    ChangeVisibility(reading, kvp.Key, kvp.Value);
                }
            }
        }

        #endregion
        #region Helper methods

        private void ChangeVisibility(GamepadReading reading, GamepadButtons button, UIElement elem)
        {
            if (reading.Buttons.HasFlag(button))
            {
                elem.Visibility = Visibility.Visible;
                Log(Enum.GetName(typeof(GamepadButtons), button));
                SendActionToListener(button);
            }
            else
            { elem.Visibility = Visibility.Collapsed; }

        }

        private void SendActionToListener(GamepadButtons button)
        {
            

            throw new NotImplementedException();
        }

        private async Task Log(String txt)
        {

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                txtEvents.Text = DateTime.Now.ToString("hh:mm:ss.fff ") + txt + "\n" + txtEvents.Text;
                Debug.WriteLine(DateTime.Now.ToString("hh:mm:ss.fff : ") + txt);
            }
            );
        }

        #endregion
    }
}
