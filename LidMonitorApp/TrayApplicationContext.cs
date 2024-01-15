using System;
using System.Windows.Forms;
using LidMonitorApp.Properties;
using Microsoft.Win32;

namespace LidMonitorApp
{
    public class TrayApplicationContext : ApplicationContext
    {
        private readonly NotifyIcon _trayIcon;

        public TrayApplicationContext()
        {
            // Initialize Tray Icon
            _trayIcon = new NotifyIcon()
            {
                Icon = Resources.TrayIcon,
                ContextMenu = new ContextMenu(new MenuItem[] {
                    new MenuItem("Exit", Exit)
                }),
                Visible = true
            };

            SystemEventsOnDisplaySettingsChanged(default, default);
            SystemEvents.DisplaySettingsChanged += SystemEventsOnDisplaySettingsChanged;
            SystemEvents.SessionSwitch += SystemEventsOnDisplaySettingsChanged;
        }

        private void SystemEventsOnDisplaySettingsChanged(object sender, EventArgs e)
        {
            PowerCfg.SetLidAction(Screen.AllScreens.Length >= 2
                ? PowerCfg.LidAction.Nothing
                : PowerCfg.LidAction.Sleep);

            // if (Screen.AllScreens.Length >= 2) 
            DisplaySwitch.Switch(DisplaySwitch.DisplayMode.Extend);
        }

        private void Exit(object sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            _trayIcon.Visible = false;
            Application.Exit();
        }
    }
}