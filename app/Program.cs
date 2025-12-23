using GHelper.Helpers;
using GHelper.Peripherals;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;

namespace GHelper
{
    static class Program
    {
        public static SettingsForm settingsForm;
        public static NotifyIcon trayIcon;

        [DllImport("user32.dll")]
        public static extern int RegisterWindowMessage(string message);

        public static int WM_TASKBARCREATED;

        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            settingsForm = new SettingsForm();

            PeripheralsProvider.DetectAllAsusMice();

            trayIcon = new NotifyIcon
            {
                Text = "ROG Mouse Helper",
                Icon = Properties.Resources.standard,
                Visible = true
            };

            WM_TASKBARCREATED = RegisterWindowMessage("TaskbarCreated");

            trayIcon.Click += (sender, e) => SettingsToggle();

            PeripheralsProvider.RegisterForDeviceEvents();

            Application.Run();
        }

        public static void SettingsToggle()
        {
            if (settingsForm == null || settingsForm.IsDisposed)
            {
                settingsForm = new SettingsForm();
            }

            if (settingsForm.Visible)
            {
                settingsForm.Hide();
            }
            else
            {
                settingsForm.Show();
                settingsForm.Activate();
            }
        }
    }
}
