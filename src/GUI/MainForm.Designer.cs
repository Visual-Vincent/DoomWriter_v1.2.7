using System;
using System.Windows.Forms;

namespace DoomWriter.GUI
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if(disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.MainFormMenu = new System.Windows.Forms.MainMenu(this.components);
            this.FileMenuItem = new System.Windows.Forms.MenuItem();
            this.SaveAsMenuItem = new System.Windows.Forms.MenuItem();
            this.ExitMenuItem = new System.Windows.Forms.MenuItem();
            this.HelpMenuItem = new System.Windows.Forms.MenuItem();
            this.HelpOnlineMenuItem = new System.Windows.Forms.MenuItem();
            this.AboutMenuItem = new System.Windows.Forms.MenuItem();
            this.MainStatusStrip = new System.Windows.Forms.StatusStrip();
            this.ToolStripLabelStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripMemoryUsageLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripSeparator2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripRenderTimeLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.MemoryUsageTimer = new System.Windows.Forms.Timer(this.components);
            this.ImageSaveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.MainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.ResultPanel = new System.Windows.Forms.Panel();
            this.ResultPictureBox = new System.Windows.Forms.PictureBox();
            this.InputTextBox = new System.Windows.Forms.TextBox();
            this.MainToolStrip = new System.Windows.Forms.ToolStrip();
            this.SaveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.RenderScaleToolStripSplitButton = new System.Windows.Forms.ToolStripSplitButton();
            this.RenderScaleToolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.MainStatusStrip.SuspendLayout();
            this.MainPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainSplitContainer)).BeginInit();
            this.MainSplitContainer.Panel1.SuspendLayout();
            this.MainSplitContainer.Panel2.SuspendLayout();
            this.MainSplitContainer.SuspendLayout();
            this.ResultPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ResultPictureBox)).BeginInit();
            this.MainToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainFormMenu
            // 
            this.MainFormMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.FileMenuItem,
            this.HelpMenuItem});
            // 
            // FileMenuItem
            // 
            this.FileMenuItem.Index = 0;
            this.FileMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.SaveAsMenuItem,
            this.ExitMenuItem});
            this.FileMenuItem.Text = "&File";
            // 
            // SaveAsMenuItem
            // 
            this.SaveAsMenuItem.Index = 0;
            this.SaveAsMenuItem.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
            this.SaveAsMenuItem.Text = "&Save as...";
            this.SaveAsMenuItem.Click += new System.EventHandler(this.SaveAsMenuItem_Click);
            // 
            // ExitMenuItem
            // 
            this.ExitMenuItem.Index = 1;
            this.ExitMenuItem.Text = "E&xit";
            this.ExitMenuItem.Click += new System.EventHandler(this.ExitMenuItem_Click);
            // 
            // HelpMenuItem
            // 
            this.HelpMenuItem.Index = 1;
            this.HelpMenuItem.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.HelpOnlineMenuItem,
            this.AboutMenuItem});
            this.HelpMenuItem.Text = "&Help";
            // 
            // HelpOnlineMenuItem
            // 
            this.HelpOnlineMenuItem.Index = 0;
            this.HelpOnlineMenuItem.Text = "&Help (online)";
            this.HelpOnlineMenuItem.Click += new System.EventHandler(this.HelpOnlineMenuItem_Click);
            // 
            // AboutMenuItem
            // 
            this.AboutMenuItem.Index = 1;
            this.AboutMenuItem.Text = "&About";
            this.AboutMenuItem.Click += new System.EventHandler(this.AboutMenuItem_Click);
            // 
            // MainStatusStrip
            // 
            this.MainStatusStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.MainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripLabelStatus,
            this.ToolStripSeparator1,
            this.ToolStripMemoryUsageLabel,
            this.ToolStripSeparator2,
            this.ToolStripRenderTimeLabel});
            this.MainStatusStrip.Location = new System.Drawing.Point(0, 519);
            this.MainStatusStrip.Name = "MainStatusStrip";
            this.MainStatusStrip.Size = new System.Drawing.Size(884, 22);
            this.MainStatusStrip.TabIndex = 2;
            // 
            // ToolStripLabelStatus
            // 
            this.ToolStripLabelStatus.Name = "ToolStripLabelStatus";
            this.ToolStripLabelStatus.Size = new System.Drawing.Size(39, 17);
            this.ToolStripLabelStatus.Text = "Ready";
            // 
            // ToolStripSeparator1
            // 
            this.ToolStripSeparator1.Name = "ToolStripSeparator1";
            this.ToolStripSeparator1.Size = new System.Drawing.Size(16, 17);
            this.ToolStripSeparator1.Text = " | ";
            // 
            // ToolStripMemoryUsageLabel
            // 
            this.ToolStripMemoryUsageLabel.Name = "ToolStripMemoryUsageLabel";
            this.ToolStripMemoryUsageLabel.Size = new System.Drawing.Size(101, 17);
            this.ToolStripMemoryUsageLabel.Text = "Memory usage: ...";
            // 
            // ToolStripSeparator2
            // 
            this.ToolStripSeparator2.Name = "ToolStripSeparator2";
            this.ToolStripSeparator2.Size = new System.Drawing.Size(16, 17);
            this.ToolStripSeparator2.Text = " | ";
            // 
            // ToolStripRenderTimeLabel
            // 
            this.ToolStripRenderTimeLabel.Name = "ToolStripRenderTimeLabel";
            this.ToolStripRenderTimeLabel.Size = new System.Drawing.Size(86, 17);
            this.ToolStripRenderTimeLabel.Text = "Render time: ...";
            // 
            // MemoryUsageTimer
            // 
            this.MemoryUsageTimer.Enabled = true;
            this.MemoryUsageTimer.Interval = 1000;
            this.MemoryUsageTimer.Tick += new System.EventHandler(this.MemoryUsageTimer_Tick);
            // 
            // ImageSaveFileDialog
            // 
            this.ImageSaveFileDialog.Filter = "PNG images (*.png)|*.png|BMP images (*.bmp)|*.bmp|JPEG images (*.jpg; *.jpeg)|*.j" +
    "pg;*.jpeg|GIF images (*.gif)|*.gif|TIFF images (*.tif; *.tiff)|*.tif;*.tiff|TGA " +
    "images (*.tga)|*.tga";
            this.ImageSaveFileDialog.RestoreDirectory = true;
            // 
            // MainPanel
            // 
            this.MainPanel.BackgroundImage = global::DoomWriter.GUI.Properties.Resources.WindowBackground;
            this.MainPanel.Controls.Add(this.MainSplitContainer);
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.MainPanel.Location = new System.Drawing.Point(0, 27);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Padding = new System.Windows.Forms.Padding(32);
            this.MainPanel.Size = new System.Drawing.Size(884, 492);
            this.MainPanel.TabIndex = 0;
            // 
            // MainSplitContainer
            // 
            this.MainSplitContainer.BackColor = System.Drawing.Color.DimGray;
            this.MainSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.MainSplitContainer.Location = new System.Drawing.Point(32, 32);
            this.MainSplitContainer.Name = "MainSplitContainer";
            this.MainSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // MainSplitContainer.Panel1
            // 
            this.MainSplitContainer.Panel1.Controls.Add(this.ResultPanel);
            // 
            // MainSplitContainer.Panel2
            // 
            this.MainSplitContainer.Panel2.Controls.Add(this.InputTextBox);
            this.MainSplitContainer.Size = new System.Drawing.Size(820, 428);
            this.MainSplitContainer.SplitterDistance = 287;
            this.MainSplitContainer.TabIndex = 0;
            // 
            // ResultPanel
            // 
            this.ResultPanel.AutoScroll = true;
            this.ResultPanel.BackColor = System.Drawing.Color.Cyan;
            this.ResultPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ResultPanel.Controls.Add(this.ResultPictureBox);
            this.ResultPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResultPanel.Location = new System.Drawing.Point(0, 0);
            this.ResultPanel.Name = "ResultPanel";
            this.ResultPanel.Size = new System.Drawing.Size(820, 287);
            this.ResultPanel.TabIndex = 0;
            // 
            // ResultPictureBox
            // 
            this.ResultPictureBox.BackColor = System.Drawing.Color.Cyan;
            this.ResultPictureBox.Location = new System.Drawing.Point(0, 0);
            this.ResultPictureBox.Name = "ResultPictureBox";
            this.ResultPictureBox.Size = new System.Drawing.Size(16, 16);
            this.ResultPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.ResultPictureBox.TabIndex = 0;
            this.ResultPictureBox.TabStop = false;
            // 
            // InputTextBox
            // 
            this.InputTextBox.AcceptsReturn = true;
            this.InputTextBox.AcceptsTab = true;
            this.InputTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InputTextBox.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.InputTextBox.Location = new System.Drawing.Point(0, 0);
            this.InputTextBox.Multiline = true;
            this.InputTextBox.Name = "InputTextBox";
            this.InputTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.InputTextBox.Size = new System.Drawing.Size(820, 137);
            this.InputTextBox.TabIndex = 1;
            this.InputTextBox.Text = "Type your text here...";
            this.InputTextBox.Enter += new System.EventHandler(this.InputTextBox_Enter);
            // 
            // MainToolStrip
            // 
            this.MainToolStrip.BackgroundImage = global::DoomWriter.GUI.Properties.Resources.ToolBarBackground;
            this.MainToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.MainToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveToolStripButton,
            this.ToolStripSeparator3,
            this.RenderScaleToolStripSplitButton});
            this.MainToolStrip.Location = new System.Drawing.Point(0, 0);
            this.MainToolStrip.Name = "MainToolStrip";
            this.MainToolStrip.Padding = new System.Windows.Forms.Padding(4, 0, 1, 0);
            this.MainToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.MainToolStrip.Size = new System.Drawing.Size(884, 27);
            this.MainToolStrip.TabIndex = 3;
            // 
            // SaveToolStripButton
            // 
            this.SaveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SaveToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("SaveToolStripButton.Image")));
            this.SaveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SaveToolStripButton.Name = "SaveToolStripButton";
            this.SaveToolStripButton.Padding = new System.Windows.Forms.Padding(2);
            this.SaveToolStripButton.Size = new System.Drawing.Size(24, 24);
            this.SaveToolStripButton.Text = "&Save";
            this.SaveToolStripButton.Click += new System.EventHandler(this.SaveToolStripButton_Click);
            // 
            // ToolStripSeparator3
            // 
            this.ToolStripSeparator3.Name = "ToolStripSeparator3";
            this.ToolStripSeparator3.Size = new System.Drawing.Size(6, 27);
            // 
            // RenderScaleToolStripSplitButton
            // 
            this.RenderScaleToolStripSplitButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RenderScaleToolStripTextBox});
            this.RenderScaleToolStripSplitButton.Image = global::DoomWriter.GUI.Properties.Resources.Resize;
            this.RenderScaleToolStripSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RenderScaleToolStripSplitButton.Name = "RenderScaleToolStripSplitButton";
            this.RenderScaleToolStripSplitButton.Padding = new System.Windows.Forms.Padding(2);
            this.RenderScaleToolStripSplitButton.Size = new System.Drawing.Size(71, 24);
            this.RenderScaleToolStripSplitButton.Text = "100%";
            this.RenderScaleToolStripSplitButton.ToolTipText = "Render scale";
            this.RenderScaleToolStripSplitButton.ButtonClick += new System.EventHandler(this.RenderScaleToolStripSplitButton_ButtonClick);
            // 
            // RenderScaleToolStripTextBox
            // 
            this.RenderScaleToolStripTextBox.MaxLength = 4;
            this.RenderScaleToolStripTextBox.Name = "RenderScaleToolStripTextBox";
            this.RenderScaleToolStripTextBox.Size = new System.Drawing.Size(100, 23);
            this.RenderScaleToolStripTextBox.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.RenderScaleToolStripTextBox.LostFocus += new System.EventHandler(this.RenderScaleToolStripTextBox_LostFocus);
            this.RenderScaleToolStripTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RenderScaleToolStripTextBox_KeyDown);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 541);
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.MainStatusStrip);
            this.Controls.Add(this.MainToolStrip);
            this.ForeColor = System.Drawing.Color.White;
            this.Menu = this.MainFormMenu;
            this.MinimumSize = new System.Drawing.Size(640, 480);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Doom Writer";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.MainStatusStrip.ResumeLayout(false);
            this.MainStatusStrip.PerformLayout();
            this.MainPanel.ResumeLayout(false);
            this.MainSplitContainer.Panel1.ResumeLayout(false);
            this.MainSplitContainer.Panel2.ResumeLayout(false);
            this.MainSplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainSplitContainer)).EndInit();
            this.MainSplitContainer.ResumeLayout(false);
            this.ResultPanel.ResumeLayout(false);
            this.ResultPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ResultPictureBox)).EndInit();
            this.MainToolStrip.ResumeLayout(false);
            this.MainToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MainMenu MainFormMenu;
        private MenuItem FileMenuItem;
        private MenuItem ExitMenuItem;
        private StatusStrip MainStatusStrip;
        private SplitContainer MainSplitContainer;
        private ToolStripStatusLabel ToolStripLabelStatus;
        private ToolStripStatusLabel ToolStripSeparator1;
        private ToolStripStatusLabel ToolStripMemoryUsageLabel;
        private ToolStripStatusLabel ToolStripSeparator2;
        private ToolStripStatusLabel ToolStripRenderTimeLabel;
        private PictureBox ResultPictureBox;
        private TextBox InputTextBox;
        private Timer MemoryUsageTimer;
        private Panel ResultPanel;
        private Panel MainPanel;
        private MenuItem SaveAsMenuItem;
        private MenuItem HelpMenuItem;
        private MenuItem AboutMenuItem;
        private SaveFileDialog ImageSaveFileDialog;
        private MenuItem HelpOnlineMenuItem;
        private ToolStrip MainToolStrip;
        private ToolStripButton SaveToolStripButton;
        private ToolStripSplitButton RenderScaleToolStripSplitButton;
        private ToolStripTextBox RenderScaleToolStripTextBox;
        private ToolStripSeparator ToolStripSeparator3;
    }
}