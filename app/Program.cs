using GHelper.Helpers;
using GHelper.Peripherals;
using GHelper.USB;
using Microsoft.Win32;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text;
using static NativeMethods;

namespace GHelper
{
    static class Program
    {
        public static NotifyIcon? trayIcon;

        public static SettingsForm settingsForm = new SettingsForm();

        public static IntPtr unRegPowerNotify, unRegPowerNotifyLid;
        public static int WM_TASKBARCREATED = 0;

        private static long lastAuto;
        private static long lastTheme;

        private static PowerLineStatus isPlugged = SystemInformation.PowerStatus.PowerLineStatus;

        // The main entry point for the application
        public static void Main(string[] args)
        {
            string action = "";
            if (args.Length > 0) action = args[0];

            string language = AppConfig.GetString("language");

            if (language != null && language.Length > 0)
                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(language);
            else
            {
                var culture = CultureInfo.CurrentUICulture;
                if (culture.ToString() == "kr") culture = CultureInfo.GetCultureInfo("ko");
                Thread.CurrentThread.CurrentUICulture = culture;
            }

            ProcessHelper.CheckAlreadyRunning();
            ProcessHelper.SetPriority();

            Logger.WriteLine("------------");
            Logger.WriteLine("App launched: " + AppConfig.GetModel() + " :" +
                             Assembly.GetExecutingAssembly().GetName().Version.ToString() +
                             CultureInfo.CurrentUICulture + (ProcessHelper.IsUserAdministrator() ? "." : ""));

            var startCount = AppConfig.Get("start_count") + 1;
            AppConfig.Set("start_count", startCount);
            Logger.WriteLine("Start Count: " + startCount);


            ProcessHelper.KillByName("ASUSSmartDisplayControl");

            Application.EnableVisualStyles();

            trayIcon = new NotifyIcon
            {
                Text = "G-Helper",
                Icon = Properties.Resources.standard,
                Visible = true
            };

            WM_TASKBARCREATED = RegisterWindowMessage("TaskbarCreated");
            Logger.WriteLine($"Tray Icon: {trayIcon.Visible} | {WM_TASKBARCREATED}");

            settingsForm.SetContextMenu();
            trayIcon.MouseClick += TrayIcon_MouseClick;
            trayIcon.MouseMove += TrayIcon_MouseMove;

            SetAutoModes(init: true);

            // Subscribing for system power change events
            SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;

            // Subscribing for monitor power on events
            unRegPowerNotify = NativeMethods.RegisterPowerSettingNotification(settingsForm.Handle,
                PowerSettingGuid.ConsoleDisplayState, NativeMethods.DEVICE_NOTIFY_WINDOW_HANDLE);
            unRegPowerNotifyLid = NativeMethods.RegisterPowerSettingNotification(settingsForm.Handle,
                PowerSettingGuid.LIDSWITCH_STATE_CHANGE, NativeMethods.DEVICE_NOTIFY_WINDOW_HANDLE);


            Task task = Task.Run((Action)PeripheralsProvider.DetectAllAsusMice);
            PeripheralsProvider.RegisterForDeviceEvents();

            if (Environment.CurrentDirectory.Trim('\\') == Application.StartupPath.Trim('\\') || action.Length > 0)
            {
                SettingsToggle(false);
            }

            switch (action)
            {
                default:
                    Task.Run(Startup.StartupCheck);
                    break;
            }

            Application.Run();
        }

        static void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            if (Math.Abs(DateTimeOffset.Now.ToUnixTimeMilliseconds() - lastTheme) < 2000) return;

            switch (e.Category)
            {
                case UserPreferenceCategory.General:
                    bool changed = settingsForm.InitTheme();
                    settingsForm.VisualiseIcon();

                    if (changed)
                    {
                        Debug.WriteLine("Theme Changed");
                        lastTheme = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                    }

                    break;
            }
        }

        public static bool SetAutoModes(bool powerChanged = false, bool init = false, bool wakeup = false)
        {
            int skipDelay = wakeup ? 10000 : 3000;

            if (Math.Abs(DateTimeOffset.Now.ToUnixTimeMilliseconds() - lastAuto) < skipDelay) return false;
            lastAuto = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            isPlugged = SystemInformation.PowerStatus.PowerLineStatus;
            Logger.WriteLine("AutoSetting for " + isPlugged.ToString());

            return true;
        }

        public static void SettingsToggle(bool checkForFocus = true, bool trayClick = false)
        {
            if (settingsForm.Visible)
            {
                // If helper window is not on top, this just focuses on the app again
                // Pressing the ghelper button again will hide the app
                if (checkForFocus && !settingsForm.HasAnyFocus(trayClick) && !AppConfig.Is("topmost"))
                {
                    settingsForm.ShowAll();
                }
                else
                {
                    settingsForm.HideAll();
                }
            }
            else
            {
                var screen = Screen.PrimaryScreen;
                if (screen is null) screen = Screen.FromControl(settingsForm);

                settingsForm.Location = screen.WorkingArea.Location;
                settingsForm.Left = screen.WorkingArea.Width - 10 - settingsForm.Width;
                settingsForm.Top = screen.WorkingArea.Height - 10 - settingsForm.Height;

                settingsForm.Show();
                settingsForm.ShowAll();

                settingsForm.Left = screen.WorkingArea.Width - 10 - settingsForm.Width;
                settingsForm.Top = screen.WorkingArea.Height - 10 - settingsForm.Height;
            }
        }

        static void TrayIcon_MouseClick(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                SettingsToggle(trayClick: true);
        }

        static void TrayIcon_MouseMove(object? sender, MouseEventArgs e)
        {
            settingsForm.RefreshSensors();
        }

        static void OnExit(object sender, EventArgs e)
        {
            if (trayIcon is not null)
            {
                trayIcon.Visible = false;
                trayIcon.Dispose();
            }

            PeripheralsProvider.UnregisterForDeviceEvents();
            NativeMethods.UnregisterPowerSettingNotification(unRegPowerNotify);
            NativeMethods.UnregisterPowerSettingNotification(unRegPowerNotifyLid);
            Application.Exit();
        }
    }
}