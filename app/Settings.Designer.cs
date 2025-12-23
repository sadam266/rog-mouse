using RogMouse.UI;

namespace RogMouse
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            checkStartup = new System.Windows.Forms.CheckBox();
            panelStartup = new System.Windows.Forms.Panel();
            panelPeripherals = new System.Windows.Forms.Panel();
            tableLayoutPeripherals = new System.Windows.Forms.TableLayoutPanel();
            buttonPeripheral = new RButton();
            tableButtons = new System.Windows.Forms.TableLayoutPanel();
            buttonQuit = new RButton();
            panelFooter = new System.Windows.Forms.Panel();
            panelStartup.SuspendLayout();
            panelPeripherals.SuspendLayout();
            tableLayoutPeripherals.SuspendLayout();
            tableButtons.SuspendLayout();
            panelFooter.SuspendLayout();
            SuspendLayout();
            // 
            // checkStartup
            // 
            checkStartup.AutoSize = true;
            checkStartup.Dock = System.Windows.Forms.DockStyle.Left;
            checkStartup.Location = new System.Drawing.Point(20, 0);
            checkStartup.Margin = new System.Windows.Forms.Padding(11, 5, 11, 5);
            checkStartup.Name = "checkStartup";
            checkStartup.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            checkStartup.Size = new System.Drawing.Size(167, 50);
            checkStartup.TabIndex = 21;
            checkStartup.Text = global::RogMouse.Properties.Strings.RunOnStartup;
            checkStartup.UseVisualStyleBackColor = true;
            // 
            // panelStartup
            // 
            panelStartup.Controls.Add(checkStartup);
            panelStartup.Dock = System.Windows.Forms.DockStyle.Top;
            panelStartup.Location = new System.Drawing.Point(11, 154);
            panelStartup.Margin = new System.Windows.Forms.Padding(0);
            panelStartup.Name = "panelStartup";
            panelStartup.Padding = new System.Windows.Forms.Padding(20, 0, 20, 0);
            panelStartup.Size = new System.Drawing.Size(302, 50);
            panelStartup.TabIndex = 6;
            // 
            // panelPeripherals
            // 
            panelPeripherals.AutoSize = true;
            panelPeripherals.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            panelPeripherals.Controls.Add(tableLayoutPeripherals);
            panelPeripherals.Dock = System.Windows.Forms.DockStyle.Top;
            panelPeripherals.Location = new System.Drawing.Point(11, 11);
            panelPeripherals.Margin = new System.Windows.Forms.Padding(0);
            panelPeripherals.Name = "panelPeripherals";
            panelPeripherals.Padding = new System.Windows.Forms.Padding(20, 5, 20, 10);
            panelPeripherals.Size = new System.Drawing.Size(302, 143);
            panelPeripherals.TabIndex = 4;
            panelPeripherals.Visible = false;
            // 
            // tableLayoutPeripherals
            // 
            tableLayoutPeripherals.AutoSize = true;
            tableLayoutPeripherals.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            tableLayoutPeripherals.ColumnCount = 1;
            tableLayoutPeripherals.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tableLayoutPeripherals.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tableLayoutPeripherals.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tableLayoutPeripherals.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tableLayoutPeripherals.Controls.Add(buttonPeripheral, 0, 0);
            tableLayoutPeripherals.Dock = System.Windows.Forms.DockStyle.Top;
            tableLayoutPeripherals.Location = new System.Drawing.Point(20, 5);
            tableLayoutPeripherals.Margin = new System.Windows.Forms.Padding(8, 4, 8, 4);
            tableLayoutPeripherals.Name = "tableLayoutPeripherals";
            tableLayoutPeripherals.RowCount = 1;
            tableLayoutPeripherals.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutPeripherals.Size = new System.Drawing.Size(262, 128);
            tableLayoutPeripherals.TabIndex = 43;
            // 
            // buttonPeripheral
            // 
            buttonPeripheral.Activated = false;
            buttonPeripheral.BackColor = System.Drawing.SystemColors.ControlLightLight;
            buttonPeripheral.BorderColor = System.Drawing.Color.Transparent;
            buttonPeripheral.BorderRadius = 5;
            buttonPeripheral.CausesValidation = false;
            buttonPeripheral.Dock = System.Windows.Forms.DockStyle.Top;
            buttonPeripheral.FlatAppearance.BorderSize = 0;
            buttonPeripheral.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonPeripheral.Font = new System.Drawing.Font("Segoe UI", 8F);
            buttonPeripheral.ForeColor = System.Drawing.SystemColors.ControlText;
            buttonPeripheral.Image = global::RogMouse.Properties.Resources.icons8_maus_48;
            buttonPeripheral.Location = new System.Drawing.Point(4, 4);
            buttonPeripheral.Margin = new System.Windows.Forms.Padding(4);
            buttonPeripheral.Name = "buttonPeripheral";
            buttonPeripheral.Secondary = false;
            buttonPeripheral.Size = new System.Drawing.Size(254, 120);
            buttonPeripheral.TabIndex = 20;
            buttonPeripheral.Text = "Mouse 1";
            buttonPeripheral.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            buttonPeripheral.UseVisualStyleBackColor = false;
            // 
            // tableButtons
            // 
            tableButtons.AutoSize = true;
            tableButtons.ColumnCount = 1;
            tableButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.333332F));
            tableButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.333332F));
            tableButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.333332F));
            tableButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            tableButtons.Controls.Add(buttonQuit, 1, 0);
            tableButtons.Dock = System.Windows.Forms.DockStyle.Top;
            tableButtons.Location = new System.Drawing.Point(20, 10);
            tableButtons.Margin = new System.Windows.Forms.Padding(8, 4, 8, 4);
            tableButtons.Name = "tableButtons";
            tableButtons.RowCount = 1;
            tableButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableButtons.Size = new System.Drawing.Size(262, 58);
            tableButtons.TabIndex = 25;
            // 
            // buttonQuit
            // 
            buttonQuit.Activated = false;
            buttonQuit.BackColor = System.Drawing.SystemColors.ControlLight;
            buttonQuit.BorderColor = System.Drawing.Color.Transparent;
            buttonQuit.BorderRadius = 2;
            buttonQuit.Dock = System.Windows.Forms.DockStyle.Top;
            buttonQuit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            buttonQuit.Location = new System.Drawing.Point(4, 5);
            buttonQuit.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            buttonQuit.Name = "buttonQuit";
            buttonQuit.Secondary = true;
            buttonQuit.Size = new System.Drawing.Size(254, 48);
            buttonQuit.TabIndex = 2;
            buttonQuit.Text = "&Quit";
            buttonQuit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            buttonQuit.UseVisualStyleBackColor = false;
            // 
            // panelFooter
            // 
            panelFooter.AutoSize = true;
            panelFooter.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            panelFooter.Controls.Add(tableButtons);
            panelFooter.Dock = System.Windows.Forms.DockStyle.Top;
            panelFooter.Location = new System.Drawing.Point(11, 204);
            panelFooter.Margin = new System.Windows.Forms.Padding(0);
            panelFooter.Name = "panelFooter";
            panelFooter.Padding = new System.Windows.Forms.Padding(20, 10, 20, 20);
            panelFooter.Size = new System.Drawing.Size(302, 88);
            panelFooter.TabIndex = 7;
            // 
            // SettingsForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(144F, 144F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            AutoSize = true;
            AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            ClientSize = new System.Drawing.Size(324, 305);
            Controls.Add(panelFooter);
            Controls.Add(panelStartup);
            Controls.Add(panelPeripherals);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Margin = new System.Windows.Forms.Padding(8, 4, 8, 4);
            MaximizeBox = false;
            MdiChildrenMinimizedAnchorBottom = false;
            MinimizeBox = false;
            MinimumSize = new System.Drawing.Size(324, 162);
            Padding = new System.Windows.Forms.Padding(11);
            ShowIcon = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "ROG Peripherals";
            panelStartup.ResumeLayout(false);
            panelStartup.PerformLayout();
            panelPeripherals.ResumeLayout(false);
            panelPeripherals.PerformLayout();
            tableLayoutPeripherals.ResumeLayout(false);
            tableButtons.ResumeLayout(false);
            panelFooter.ResumeLayout(false);
            panelFooter.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Panel panelFooter;
        private RButton buttonQuit;
        private System.Windows.Forms.CheckBox checkStartup;
        private System.Windows.Forms.Panel panelStartup;
        private System.Windows.Forms.TableLayoutPanel tableButtons;
        private System.Windows.Forms.Panel panelPeripherals;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPeripherals;
        private RButton buttonPeripheral;
    }
}
