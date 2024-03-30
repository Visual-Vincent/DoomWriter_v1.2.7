#if FONTEDITOR
namespace FontEditor.Forms
#else
namespace DoomWriter.GUI
#endif
{
    partial class MainAboutBox
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainAboutBox));
            this.DoomWriterTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.LogoPictureBox = new System.Windows.Forms.PictureBox();
            this.ProductNameLabel = new System.Windows.Forms.Label();
            this.VersionLabel = new System.Windows.Forms.Label();
            this.CopyrightLabel = new System.Windows.Forms.Label();
            this.CompanyNameLabel = new System.Windows.Forms.Label();
            this.CreditsTextBox = new System.Windows.Forms.TextBox();
            this.OKButton = new System.Windows.Forms.Button();
            this.MainTabControl = new System.Windows.Forms.TabControl();
            this.DoomWriterTabPage = new System.Windows.Forms.TabPage();
            this.ThirdPartyTabPage = new System.Windows.Forms.TabPage();
            this.ThirdPartyTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.ThirdPartyTextBox = new System.Windows.Forms.TextBox();
            this.ThirdPartyLinkLabel = new System.Windows.Forms.LinkLabel();
            this.ThirdPartyListBox = new System.Windows.Forms.ListBox();
            this.MainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.DoomWriterTableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).BeginInit();
            this.MainTabControl.SuspendLayout();
            this.DoomWriterTabPage.SuspendLayout();
            this.ThirdPartyTabPage.SuspendLayout();
            this.ThirdPartyTableLayoutPanel.SuspendLayout();
            this.MainTableLayoutPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // DoomWriterTableLayoutPanel
            // 
            this.DoomWriterTableLayoutPanel.ColumnCount = 2;
            this.DoomWriterTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 138F));
            this.DoomWriterTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.DoomWriterTableLayoutPanel.Controls.Add(this.LogoPictureBox, 0, 0);
            this.DoomWriterTableLayoutPanel.Controls.Add(this.ProductNameLabel, 1, 0);
            this.DoomWriterTableLayoutPanel.Controls.Add(this.VersionLabel, 1, 1);
            this.DoomWriterTableLayoutPanel.Controls.Add(this.CopyrightLabel, 1, 2);
            this.DoomWriterTableLayoutPanel.Controls.Add(this.CompanyNameLabel, 1, 3);
            this.DoomWriterTableLayoutPanel.Controls.Add(this.CreditsTextBox, 1, 4);
            this.DoomWriterTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DoomWriterTableLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.DoomWriterTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.DoomWriterTableLayoutPanel.Name = "DoomWriterTableLayoutPanel";
            this.DoomWriterTableLayoutPanel.RowCount = 5;
            this.DoomWriterTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.DoomWriterTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.DoomWriterTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.DoomWriterTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.DoomWriterTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.DoomWriterTableLayoutPanel.Size = new System.Drawing.Size(570, 315);
            this.DoomWriterTableLayoutPanel.TabIndex = 0;
            // 
            // LogoPictureBox
            // 
            this.LogoPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LogoPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("LogoPictureBox.Image")));
            this.LogoPictureBox.Location = new System.Drawing.Point(0, 0);
            this.LogoPictureBox.Margin = new System.Windows.Forms.Padding(0);
            this.LogoPictureBox.Name = "LogoPictureBox";
            this.DoomWriterTableLayoutPanel.SetRowSpan(this.LogoPictureBox, 5);
            this.LogoPictureBox.Size = new System.Drawing.Size(138, 315);
            this.LogoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.LogoPictureBox.TabIndex = 12;
            this.LogoPictureBox.TabStop = false;
            // 
            // ProductNameLabel
            // 
            this.ProductNameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProductNameLabel.Location = new System.Drawing.Point(144, 3);
            this.ProductNameLabel.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.ProductNameLabel.MaximumSize = new System.Drawing.Size(0, 17);
            this.ProductNameLabel.Name = "ProductNameLabel";
            this.ProductNameLabel.Size = new System.Drawing.Size(423, 17);
            this.ProductNameLabel.TabIndex = 19;
            this.ProductNameLabel.Text = "Product Name";
            this.ProductNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // VersionLabel
            // 
            this.VersionLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.VersionLabel.Location = new System.Drawing.Point(144, 26);
            this.VersionLabel.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.VersionLabel.MaximumSize = new System.Drawing.Size(0, 17);
            this.VersionLabel.Name = "VersionLabel";
            this.VersionLabel.Size = new System.Drawing.Size(423, 17);
            this.VersionLabel.TabIndex = 0;
            this.VersionLabel.Text = "Version";
            this.VersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CopyrightLabel
            // 
            this.CopyrightLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CopyrightLabel.Location = new System.Drawing.Point(144, 49);
            this.CopyrightLabel.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.CopyrightLabel.MaximumSize = new System.Drawing.Size(0, 17);
            this.CopyrightLabel.Name = "CopyrightLabel";
            this.CopyrightLabel.Size = new System.Drawing.Size(423, 17);
            this.CopyrightLabel.TabIndex = 21;
            this.CopyrightLabel.Text = "Copyright";
            this.CopyrightLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CompanyNameLabel
            // 
            this.CompanyNameLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CompanyNameLabel.Location = new System.Drawing.Point(144, 72);
            this.CompanyNameLabel.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.CompanyNameLabel.MaximumSize = new System.Drawing.Size(0, 17);
            this.CompanyNameLabel.Name = "CompanyNameLabel";
            this.CompanyNameLabel.Size = new System.Drawing.Size(423, 17);
            this.CompanyNameLabel.TabIndex = 22;
            this.CompanyNameLabel.Text = "Company Name";
            this.CompanyNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CreditsTextBox
            // 
            this.CreditsTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.CreditsTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CreditsTextBox.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CreditsTextBox.Location = new System.Drawing.Point(144, 95);
            this.CreditsTextBox.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.CreditsTextBox.Multiline = true;
            this.CreditsTextBox.Name = "CreditsTextBox";
            this.CreditsTextBox.ReadOnly = true;
            this.CreditsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.CreditsTextBox.Size = new System.Drawing.Size(423, 217);
            this.CreditsTextBox.TabIndex = 23;
            this.CreditsTextBox.TabStop = false;
            this.CreditsTextBox.Text = "Credits";
            // 
            // OKButton
            // 
            this.OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OKButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OKButton.Location = new System.Drawing.Point(503, 353);
            this.OKButton.Margin = new System.Windows.Forms.Padding(6);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 24;
            this.OKButton.Text = "&OK";
            // 
            // MainTabControl
            // 
            this.MainTabControl.Controls.Add(this.DoomWriterTabPage);
            this.MainTabControl.Controls.Add(this.ThirdPartyTabPage);
            this.MainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTabControl.Location = new System.Drawing.Point(0, 0);
            this.MainTabControl.Margin = new System.Windows.Forms.Padding(0);
            this.MainTabControl.Name = "MainTabControl";
            this.MainTabControl.SelectedIndex = 0;
            this.MainTabControl.Size = new System.Drawing.Size(584, 347);
            this.MainTabControl.TabIndex = 1;
            // 
            // DoomWriterTabPage
            // 
            this.DoomWriterTabPage.Controls.Add(this.DoomWriterTableLayoutPanel);
            this.DoomWriterTabPage.Location = new System.Drawing.Point(4, 22);
            this.DoomWriterTabPage.Name = "DoomWriterTabPage";
            this.DoomWriterTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.DoomWriterTabPage.Size = new System.Drawing.Size(576, 321);
            this.DoomWriterTabPage.TabIndex = 0;
            this.DoomWriterTabPage.Text = "Doom Writer";
            this.DoomWriterTabPage.UseVisualStyleBackColor = true;
            // 
            // ThirdPartyTabPage
            // 
            this.ThirdPartyTabPage.Controls.Add(this.ThirdPartyTableLayoutPanel);
            this.ThirdPartyTabPage.Location = new System.Drawing.Point(4, 22);
            this.ThirdPartyTabPage.Name = "ThirdPartyTabPage";
            this.ThirdPartyTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.ThirdPartyTabPage.Size = new System.Drawing.Size(576, 326);
            this.ThirdPartyTabPage.TabIndex = 1;
            this.ThirdPartyTabPage.Text = "3rd party";
            this.ThirdPartyTabPage.UseVisualStyleBackColor = true;
            // 
            // ThirdPartyTableLayoutPanel
            // 
            this.ThirdPartyTableLayoutPanel.ColumnCount = 2;
            this.ThirdPartyTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 138F));
            this.ThirdPartyTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ThirdPartyTableLayoutPanel.Controls.Add(this.ThirdPartyTextBox, 1, 1);
            this.ThirdPartyTableLayoutPanel.Controls.Add(this.ThirdPartyLinkLabel, 1, 0);
            this.ThirdPartyTableLayoutPanel.Controls.Add(this.ThirdPartyListBox, 0, 0);
            this.ThirdPartyTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ThirdPartyTableLayoutPanel.Location = new System.Drawing.Point(3, 3);
            this.ThirdPartyTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ThirdPartyTableLayoutPanel.Name = "ThirdPartyTableLayoutPanel";
            this.ThirdPartyTableLayoutPanel.RowCount = 2;
            this.ThirdPartyTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.ThirdPartyTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ThirdPartyTableLayoutPanel.Size = new System.Drawing.Size(570, 320);
            this.ThirdPartyTableLayoutPanel.TabIndex = 1;
            // 
            // ThirdPartyTextBox
            // 
            this.ThirdPartyTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.ThirdPartyTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ThirdPartyTextBox.Location = new System.Drawing.Point(141, 22);
            this.ThirdPartyTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.ThirdPartyTextBox.MaxLength = 65536;
            this.ThirdPartyTextBox.Multiline = true;
            this.ThirdPartyTextBox.Name = "ThirdPartyTextBox";
            this.ThirdPartyTextBox.ReadOnly = true;
            this.ThirdPartyTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.ThirdPartyTextBox.Size = new System.Drawing.Size(429, 295);
            this.ThirdPartyTextBox.TabIndex = 23;
            this.ThirdPartyTextBox.TabStop = false;
            // 
            // ThirdPartyLinkLabel
            // 
            this.ThirdPartyLinkLabel.AutoSize = true;
            this.ThirdPartyLinkLabel.Location = new System.Drawing.Point(141, 3);
            this.ThirdPartyLinkLabel.Margin = new System.Windows.Forms.Padding(3);
            this.ThirdPartyLinkLabel.Name = "ThirdPartyLinkLabel";
            this.ThirdPartyLinkLabel.Size = new System.Drawing.Size(27, 13);
            this.ThirdPartyLinkLabel.TabIndex = 24;
            this.ThirdPartyLinkLabel.TabStop = true;
            this.ThirdPartyLinkLabel.Text = "Link";
            this.ThirdPartyLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ThirdPartyLinkLabel_LinkClicked);
            // 
            // ThirdPartyListBox
            // 
            this.ThirdPartyListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ThirdPartyListBox.FormattingEnabled = true;
            this.ThirdPartyListBox.Location = new System.Drawing.Point(0, 0);
            this.ThirdPartyListBox.Margin = new System.Windows.Forms.Padding(0);
            this.ThirdPartyListBox.Name = "ThirdPartyListBox";
            this.ThirdPartyTableLayoutPanel.SetRowSpan(this.ThirdPartyListBox, 2);
            this.ThirdPartyListBox.Size = new System.Drawing.Size(138, 320);
            this.ThirdPartyListBox.TabIndex = 25;
            this.ThirdPartyListBox.SelectedIndexChanged += new System.EventHandler(this.ThirdPartyListBox_SelectedIndexChanged);
            // 
            // MainTableLayoutPanel
            // 
            this.MainTableLayoutPanel.ColumnCount = 1;
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTableLayoutPanel.Controls.Add(this.MainTabControl, 0, 0);
            this.MainTableLayoutPanel.Controls.Add(this.OKButton, 0, 1);
            this.MainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.MainTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.MainTableLayoutPanel.Name = "MainTableLayoutPanel";
            this.MainTableLayoutPanel.RowCount = 2;
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.MainTableLayoutPanel.Size = new System.Drawing.Size(584, 382);
            this.MainTableLayoutPanel.TabIndex = 2;
            // 
            // MainAboutBox
            // 
            this.AcceptButton = this.OKButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 382);
            this.Controls.Add(this.MainTableLayoutPanel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(450, 320);
            this.Name = "MainAboutBox";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MainAboutBox";
            this.DoomWriterTableLayoutPanel.ResumeLayout(false);
            this.DoomWriterTableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LogoPictureBox)).EndInit();
            this.MainTabControl.ResumeLayout(false);
            this.DoomWriterTabPage.ResumeLayout(false);
            this.ThirdPartyTabPage.ResumeLayout(false);
            this.ThirdPartyTableLayoutPanel.ResumeLayout(false);
            this.ThirdPartyTableLayoutPanel.PerformLayout();
            this.MainTableLayoutPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

#endregion

        private System.Windows.Forms.TableLayoutPanel DoomWriterTableLayoutPanel;
        private System.Windows.Forms.PictureBox LogoPictureBox;
        private System.Windows.Forms.Label ProductNameLabel;
        private System.Windows.Forms.Label VersionLabel;
        private System.Windows.Forms.Label CopyrightLabel;
        private System.Windows.Forms.Label CompanyNameLabel;
        private System.Windows.Forms.TextBox CreditsTextBox;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.TabControl MainTabControl;
        private System.Windows.Forms.TabPage DoomWriterTabPage;
        private System.Windows.Forms.TabPage ThirdPartyTabPage;
        private System.Windows.Forms.TableLayoutPanel MainTableLayoutPanel;
        private System.Windows.Forms.TableLayoutPanel ThirdPartyTableLayoutPanel;
        private System.Windows.Forms.TextBox ThirdPartyTextBox;
        private System.Windows.Forms.LinkLabel ThirdPartyLinkLabel;
        private System.Windows.Forms.ListBox ThirdPartyListBox;
    }
}
