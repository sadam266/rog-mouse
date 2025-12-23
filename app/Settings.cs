using GHelper.Peripherals;
using GHelper.UI;
using GHelper.Peripherals.Mouse;

namespace GHelper
{
    public partial class SettingsForm : RForm
    {
        private Dictionary<AsusMouse, AsusMouseSettings> mouseSettingsWindows = new Dictionary<AsusMouse, AsusMouseSettings>();

        public SettingsForm()
        {
            InitializeComponent();

            InitTheme();

            VisualizePeripherals();

            FormClosing += SettingsForm_FormClosing;
            buttonQuit.Click += (s, e) => Application.Exit();

            InitStartup();
        }

        private void InitStartup()
        {
            checkStartup.Checked = IsStartupEnabled();
            checkStartup.CheckedChanged += CheckStartup_CheckedChanged;
        }

        private bool IsStartupEnabled()
        {
            using (var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", false))
            {
                return key?.GetValue(Application.ProductName) != null;
            }
        }

        private void CheckStartup_CheckedChanged(object? sender, EventArgs e)
        {
            using (var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
            {
                if (checkStartup.Checked)
                {
                    key?.SetValue(Application.ProductName, "\"" + Application.ExecutablePath + "\"");
                }
                else
                {
                    key?.DeleteValue(Application.ProductName, false);
                }
            }
        }

        private void SettingsForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        public void VisualizePeripherals()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(VisualizePeripherals));
                return;
            }

            panelPeripherals.Controls.Clear();
            panelPeripherals.Padding = new Padding(20, 10, 20, 10);

            panelPeripherals.Controls.Add(panelPeripheralsTitle);
            panelPeripheralsTitle.Dock = DockStyle.Top;
            panelPeripheralsTitle.SendToBack();

            picturePeripherals.BackgroundImage = ControlHelper.TintImage(Properties.Resources.icons8_maus_48, RForm.foreMain);
            panelPeripheralsTitle.BackColor = RForm.formBack;
            picturePeripherals.BackColor = RForm.formBack;

            labelPeripherals.Text = Properties.Strings.Peripherals;
            labelPeripherals.ForeColor = RForm.foreMain;
            panelPeripherals.BackColor = RForm.formBack;

            panelPeripheralsTitle.Controls.Add(picturePeripherals);
            panelPeripheralsTitle.Controls.Add(labelPeripherals);

            panelFooter.BackColor = RForm.formBack;
            buttonQuit.BackColor = RForm.buttonSecond;
            buttonQuit.ForeColor = RForm.foreMain;

            this.BackColor = RForm.formBack;

            var devices = PeripheralsProvider.AllPeripherals();
            foreach (var device in devices)
            {
                if (device is AsusMouse mouse)
                {
                    RButton button = new RButton();
                    string batteryText = mouse.HasBattery() ? $" ({mouse.Battery}%)" : "";
                    button.Text = mouse.GetDisplayName() + batteryText;
                    button.Height = 100;
                    button.Margin = new Padding(0, 5, 0, 5);
                    button.Dock = DockStyle.Top;
                    button.Click += (s, e) => {
                        if (mouseSettingsWindows.ContainsKey(mouse))
                        {
                            var existingWindow = mouseSettingsWindows[mouse];
                            if (existingWindow.Visible)
                            {
                                existingWindow.Hide();
                            }
                            else
                            {
                                existingWindow.Show();
                                existingWindow.Activate();
                            }
                        }
                        else
                        {
                            var settings = new AsusMouseSettings(mouse);
                            mouseSettingsWindows[mouse] = settings;
                            settings.FormClosed += (sender, args) => mouseSettingsWindows.Remove(mouse);
                            settings.Show();
                            settings.Activate();
                        }
                    };
                    button.BackColor = RForm.buttonMain;
                    button.ForeColor = RForm.foreMain;

                    if (mouse.HasBattery())
                    {
                        button.Image = Properties.Resources.icons8_maus_48;
                        button.ImageAlign = ContentAlignment.MiddleLeft;
                        button.TextImageRelation = TextImageRelation.ImageBeforeText;
                        button.TextAlign = ContentAlignment.MiddleLeft;
                        button.Padding = new Padding(10, 0, 0, 0);
                    }

                    panelPeripherals.Controls.Add(button);
                    button.BringToFront();
                }
            }

            panelPeripherals.Visible = true;
        }

        private void PositionSelf()
        {
            var screen = Screen.FromPoint(Cursor.Position);
            this.Left = screen.WorkingArea.Right - this.Width - 10;
            this.Top = screen.WorkingArea.Bottom - this.Height - 10;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            PositionSelf();
        }
    }
}
