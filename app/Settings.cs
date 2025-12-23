using System.Diagnostics;
using System.Timers;
using RogMouse.Helpers;
using RogMouse.Peripherals;
using RogMouse.Peripherals.Mouse;
using RogMouse.UI;

namespace RogMouse
{
    public partial class SettingsForm : RForm
    {
        ContextMenuStrip contextMenuStrip = new CustomContextMenu();

        AsusMouseSettings? mouseSettings;

        public static System.Timers.Timer sensorTimer = default!;

        static long lastRefresh;
        static long lastLostFocus;

        bool activateCheck = false;

        public SettingsForm()
        {
            InitializeComponent();
            InitTheme(true);
            checkStartup.Text = Properties.Strings.RunOnStartup;

            buttonQuit.Text = Properties.Strings.Quit;

            // Accessible Labels
            buttonQuit.AccessibleName = Properties.Strings.Quit;

            FormClosing += SettingsForm_FormClosing;
            Deactivate += SettingsForm_LostFocus;
            Activated += SettingsForm_Focused;

            VisibleChanged += SettingsForm_VisibleChanged;

            buttonQuit.Click += ButtonQuit_Click;

            checkStartup.Checked = Startup.IsScheduled();
            checkStartup.CheckedChanged += CheckStartup_CheckedChanged;

            sensorTimer = new System.Timers.Timer(AppConfig.Get("sensor_timer", 1000));
            sensorTimer.Elapsed += OnTimedEvent;
            sensorTimer.Enabled = true;

            buttonPeripheral.Click += ButtonPeripheral_Click;

            buttonPeripheral.MouseEnter += ButtonPeripheral_MouseEnter;

            Text = "";
            TopMost = AppConfig.Is("topmost");

            //This will auto position the window again when it resizes. Might mess with position if people drag the window somewhere else.
            this.Resize += SettingsForm_Resize;
        }

        private void SettingsForm_Focused(object? sender, EventArgs e)
        {
            if (activateCheck)
            {
                activateCheck = false;
            }
        }

        private void SettingsForm_LostFocus(object? sender, EventArgs e)
        {
            lastLostFocus = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }

        private void SettingsForm_Resize(object? sender, EventArgs e)
        {
            Left = Screen.FromControl(this).WorkingArea.Width - 10 - Width;
            Top = Screen.FromControl(this).WorkingArea.Height - 10 - Height;
        }

        private void SettingsForm_VisibleChanged(object? sender, EventArgs e)
        {
            sensorTimer.Enabled = this.Visible;
            if (this.Visible)
            {
                Task.Run((Action)RefreshPeripheralsBattery);
            }
        }

        private void RefreshPeripheralsBattery()
        {
            PeripheralsProvider.RefreshBatteryForAllDevices(true);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Program.WM_TASKBARCREATED)
            {
                Logger.WriteLine("Taskbar created, re-creating tray icon");
                if (Program.trayIcon is not null) Program.trayIcon.Visible = true;
            }

            try
            {
                base.WndProc(ref m);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        public void SetContextMenu()
        {
            foreach (ToolStripItem item in contextMenuStrip.Items.Cast<ToolStripItem>().ToList())
            {
                if (item is ToolStripMenuItem menuItem) menuItem.Dispose();
            }

            contextMenuStrip.Items.Clear();
            contextMenuStrip.ShowCheckMargin = true;
            contextMenuStrip.ImageScalingSize = new Size(16, 16);
            contextMenuStrip.ShowImageMargin = false;
            Padding padding = new Padding(15, 5, 5, 5);

            // var title = new ToolStripMenuItem("");
            // title.Margin = padding;
            // title.Enabled = false;
            // contextMenuStrip.Items.Add(title);
            //
            // contextMenuStrip.Items.Add("-");


            var quit = new ToolStripMenuItem(Properties.Strings.Quit);
            quit.Click += ButtonQuit_Click;
            quit.Margin = padding;
            contextMenuStrip.Items.Add(quit);

            //contextMenuStrip.ShowCheckMargin = true;
            contextMenuStrip.RenderMode = ToolStripRenderMode.System;

            if (darkTheme)
            {
                contextMenuStrip.BackColor = this.BackColor;
                contextMenuStrip.ForeColor = this.ForeColor;
            }

            if (Program.trayIcon is not null) Program.trayIcon.ContextMenuStrip = contextMenuStrip;
        }

        private static void OnTimedEvent(Object? source, ElapsedEventArgs? e)
        {
            Program.settingsForm.RefreshSensors();
        }

        private void CheckStartup_CheckedChanged(object? sender, EventArgs e)
        {
            if (sender is null) return;
            CheckBox chk = (CheckBox)sender;

            if (chk.Checked)
                Startup.Schedule();
            else
                Startup.UnSchedule();
        }

        private void ButtonQuit_Click(object? sender, EventArgs e)
        {
            Close();
            if (Program.trayIcon != null) Program.trayIcon.Visible = false;
            Application.Exit();
        }

        /// <summary>
        /// Closes all forms except the settings. Hides the settings
        /// </summary>
        public void HideAll()
        {
            this.Hide();
            if (mouseSettings != null && mouseSettings.Text != "") mouseSettings.Close();
        }

        /// <summary>
        /// Brings all visible windows to the top, with settings being the focus
        /// </summary>
        public void ShowAll()
        {
            this.Activate();
            this.TopMost = true;
            this.TopMost = AppConfig.Is("topmost");
        }

        /// <summary>
        /// Check if any of fans, keyboard, update, or itself has focus
        /// </summary>
        /// <returns>Focus state</returns>
        public bool HasAnyFocus(bool lostFocusCheck = false)
        {
            return this.ContainsFocus ||
                   (lostFocusCheck && Math.Abs(DateTimeOffset.Now.ToUnixTimeMilliseconds() - lastLostFocus) < 300);
        }

        private void SettingsForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing) return;
            e.Cancel = true;
            HideAll();
        }

        public async void RefreshSensors(bool force = false)
        {
            if (!force && Math.Abs(DateTimeOffset.Now.ToUnixTimeMilliseconds() - lastRefresh) < 2000) return;
            lastRefresh = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            await Task.Run((Action)PeripheralsProvider.RefreshBatteryForAllDevices);

            string trayTip = "TODO device list";

            if (Program.trayIcon is not null) Program.trayIcon.Text = trayTip;
        }

        public void VisualiseIcon()
        {
            if (Program.trayIcon is null) return;
            bool isDark = CheckSystemDarkModeStatus();
            Program.trayIcon.Icon = AppConfig.IsBWIcon()
                ? (!isDark ? Properties.Resources.dark_standard : Properties.Resources.light_standard)
                : Properties.Resources.standard;
        }

        public void VisualizePeripherals()
        {
            if (!PeripheralsProvider.IsAnyPeripheralConnect())
            {
                panelPeripherals.Visible = false;
                return;
            }

            Button[] buttons = [buttonPeripheral];
            
            List<IPeripheral> lp = PeripheralsProvider.AllPeripherals();

            for (int i = 0; i < lp.Count && i < buttons.Length; ++i)
            {
                IPeripheral m = lp.ElementAt(i);
                Button b = buttons[i];

                if (m.IsDeviceReady)
                {
                    if (m.HasBattery())
                    {
                        b.Text = m.GetDisplayName() + "\n" + m.Battery + "%"
                                 + (m.Charging ? "(" + Properties.Strings.Charging + ")" : "");
                    }
                    else
                    {
                        b.Text = m.GetDisplayName();
                    }
                }
                else
                {
                    //Mouse is either not connected or in standby
                    b.Text = m.GetDisplayName() + "\n(" + Properties.Strings.NotConnected + ")";
                }

                b.Image = m.DeviceType() switch
                {
                    PeripheralType.Mouse => ControlHelper.TintImage(Properties.Resources.icons8_maus_32, b.ForeColor),
                    PeripheralType.Keyboard => ControlHelper.TintImage(Properties.Resources.icons8_keyboard_32, b.ForeColor),
                    _ => b.Image
                };

                b.Visible = true;
            }

            for (int i = lp.Count; i < buttons.Length; ++i)
            {
                buttons[i].Visible = false;
            }

            panelPeripherals.Visible = true;
        }

        private void ButtonPeripheral_MouseEnter(object? sender, EventArgs e)
        {
            IPeripheral? iph = PeripheralsProvider.AllPeripherals().FirstOrDefault();

            if (iph is null)
            {
                return;
            }

            if (!iph.IsDeviceReady)
            {
                //Refresh battery on hover if the device is marked as "Not Ready"
                iph.ReadBattery();
            }
        }

        private void ButtonPeripheral_Click(object? sender, EventArgs e)
        {
            if (mouseSettings is not null)
            {
                mouseSettings.Close();
                return;
            }

            IPeripheral? iph = PeripheralsProvider.AllPeripherals().FirstOrDefault();

            if (iph is null)
            {
                //Can only happen when the user hits the button in the exact moment a device is disconnected.
                return;
            }

            if (iph.DeviceType() == PeripheralType.Mouse)
            {
                AsusMouse? am = iph as AsusMouse;
                if (am is null || !am.IsDeviceReady)
                {
                    //Should not happen if all device classes are implemented correctly. But better safe than sorry.
                    return;
                }

                mouseSettings = new AsusMouseSettings(am);
                mouseSettings.TopMost = AppConfig.Is("topmost");
                mouseSettings.FormClosed += MouseSettings_FormClosed;
                mouseSettings.Disposed += MouseSettings_Disposed;
                if (!mouseSettings.IsDisposed)
                {
                    mouseSettings.Show();
                }
                else
                {
                    mouseSettings = null;
                }
            }
        }

        private void MouseSettings_Disposed(object? sender, EventArgs e)
        {
            mouseSettings = null;
        }

        private void MouseSettings_FormClosed(object? sender, FormClosedEventArgs e)
        {
            mouseSettings = null;
        }
    }
}