using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using SixLabors.ImageSharp.PixelFormats;
using DoomWriter;

using Image = System.Drawing.Image;
using Point = System.Drawing.Point;
using Size = System.Drawing.Size;
using Rectangle = System.Drawing.Rectangle;
using SixLaborsImage = SixLabors.ImageSharp.Image;
using SixLaborsImageRgba32 = SixLabors.ImageSharp.Image<SixLabors.ImageSharp.PixelFormats.Rgba32>;
using DWImage = DoomWriter.Image;
using DWFont = DoomWriter.Font;
using FontConverter = DoomWriter.FontConverter;

#pragma warning disable IDE0003 // Remove 'this'

namespace FontEditor.Forms
{
    public partial class MainForm : Form
    {
        private static readonly Pen SelectionPen = new Pen(Color.Black, 1.0f) { DashStyle = DashStyle.Dash };
        private static readonly Brush CharacterBrush = new SolidBrush(Color.FromArgb(127, Color.Yellow));
        private static readonly Brush DescenderBrush = new SolidBrush(Color.FromArgb(127, Color.Lime));

        private static readonly HashSet<Keys> AcceptedNumericKeyCodes = new HashSet<Keys>() {
            Keys.Enter, Keys.Back, Keys.Delete, Keys.Home, Keys.End, Keys.Left, Keys.Right, Keys.Up, Keys.Down,
            Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9,
            Keys.NumPad0, Keys.NumPad1, Keys.NumPad2, Keys.NumPad3, Keys.NumPad4, Keys.NumPad5,
            Keys.NumPad6, Keys.NumPad7, Keys.NumPad8, Keys.NumPad9
        };

        private MutableFont editedFont = new MutableFont();
        private string editedFontPath = null;

        private readonly PaletteViewer paletteViewer = new PaletteViewer() { Dock = DockStyle.Fill };
        private CharacterMappingControl characterMappingsControl;

        private Point panMouseLastPosition;
        private Glyph selectedCharacterMapping;

        private bool unsavedChanges = false;

        /// <summary>
        /// Gets whether or not the form has unsaved changes.
        /// </summary>
        public bool UnsavedChanges
        {
            get {
                return unsavedChanges;
            }
            private set {
                if(unsavedChanges == value)
                    return;

                unsavedChanges = value;

                this.Text = this.Text.TrimEnd('*') + (unsavedChanges ? "*" : "");
            }
        }

        public MainForm()
        {
            InitializeComponent();
            StatusLabel.Text = "";
            MainPictureBox_ImageChanged(MainPictureBox, EventArgs.Empty);
        }

        private void NewFont()
        {
            editedFont.Dispose();
            editedFont = new MutableFont();
            editedFontPath = null;
            selectedCharacterMapping = null;

            UnsavedChanges = false;

            SetImageZoom(1.0);
            MainPictureBox.Image?.Dispose();
            MainPictureBox.Image = null;

            if(characterMappingsControl != null)
            {
                characterMappingsControl.MappingsChanged -= CharacterMappingsControl_MappingsChanged;
                characterMappingsControl.SelectionChanged -= CharacterMappingsControl_SelectionChanged;
                characterMappingsControl.Dispose();
            }

            characterMappingsControl = new CharacterMappingControl() { Dock = DockStyle.Fill };
            characterMappingsControl.MappingsChanged += CharacterMappingsControl_MappingsChanged;
            characterMappingsControl.SelectionChanged += CharacterMappingsControl_SelectionChanged;
        }

        private void LoadFont(MutableFont font)
        {
            NewFont();

            editedFont.Dispose();
            editedFont = font;

            MainPictureBox.Image?.Dispose();
            MainPictureBox.Image = editedFont.Image.ToSystemDrawingImage();

            characterMappingsControl.LoadCharacterMappings(editedFont.Glyphs);
            UnsavedChanges = false;
        }

        private void LoadFontFromFile(string file)
        {
            MutableFont font = null;

            try
            {
                font = DWFont.Load<MutableFont>(file);
            }
            catch(Exception ex)
            {
                font?.Dispose();
                MessageBox.Show("Failed to open font:" + Environment.NewLine + $"{ex.GetType().FullName}: {ex.Message}", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LoadFont(font);
            editedFontPath = file;
        }

        private void LoadLegacyChartFromFile(string file)
        {
            byte[] chartData;
            MutableFont font = null;
            DWFont convertedFont = null;

            try
            {
                chartData = File.ReadAllBytes(file);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Failed to open legacy chart:" + Environment.NewLine + $"{ex.GetType().FullName}: {ex.Message}", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                convertedFont = FontConverter.ConvertLegacyChart(chartData);
                font = new MutableFont(convertedFont);
            }
            catch(Exception ex)
            {
                font?.Dispose();
                MessageBox.Show("Failed to convert legacy chart:" + Environment.NewLine + $"{ex.GetType().FullName}: {ex.Message}", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                convertedFont?.Dispose();
            }

            LoadFont(font);

            editedFontPath = null;
            UnsavedChanges = true;
        }

        /// <summary>
        /// Prompts the user to save their unsaved changes, if there are any. Returns a boolean value indicating whether the user took an action or chose to cancel the dialog.
        /// </summary>
        /// <param name="message">The message to show the user.</param>
        /// <returns><see langword="true"/> if there were no unsaved changes or the user chose to save/not save them; otherwise <see langword="false"/> (such as if the user pressed "Cancel", or the operation resulted in an error).</returns>
        private bool PromptUnsavedChanges(string message)
        {
            if(!UnsavedChanges)
                return true;

            var result = MessageBox.Show(message, Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            switch(result)
            {
                case DialogResult.Yes:
                    if(SaveChanges() != DialogResult.OK)
                    {
                        return false;
                    }
                    break;

                case DialogResult.Cancel:
                    return false;
            }

            return true;
        }

        private DialogResult SaveChanges()
        {
            var result = DialogResult.OK;

            // If we don't have a path, or if the file including its parent directory cannot be found, show save dialog
            if(string.IsNullOrEmpty(editedFontPath) || (!File.Exists(editedFontPath) && !Directory.Exists(Path.GetDirectoryName(editedFontPath))))
            {
                result = SaveFontFileDialog.ShowDialog();

                if(result != DialogResult.OK)
                    return result;

                editedFontPath = SaveFontFileDialog.FileName;
                SaveFontFileDialog.FileName = "";
            }

            UpdateCharacterMappings();

            try
            {
                DWFont.Save(editedFontPath, editedFont);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Failed to save font:" + Environment.NewLine + $"{ex.GetType().FullName}: {ex.Message}", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return DialogResult.Abort;
            }

            UnsavedChanges = false;

            return result;
        }

        private void SetImageZoom(double ratio)
        {
            if(MainPictureBox.Zoom == ratio)
                return;

            ImageContainerTableLayoutPanel.AutoScroll = false;
            MainPictureBox.Zoom = ratio;
            ImageContainerTableLayoutPanel.AutoScroll = true;
        }

        private void UpdateCharacterMappings()
        {
            editedFont.Glyphs.Clear();

            var mappings = characterMappingsControl.GetCharacterMappings();

            foreach(var kvp in mappings)
            {
                editedFont.Glyphs.Add(kvp);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if(keyData == Keys.Escape && MainPictureBox.IsSelecting)
            {
                MainPictureBox.CancelSelection();
                StatusLabel.Text = "";

                return true;
            }
            
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Menu = MainFormMenu;

            ToolStripManager.RenderMode = ToolStripManagerRenderMode.System;
            ToolStripManager.LoadSettings(this);

            NewFont();

            foreach(int zoomLevel in new int[] { 25, 50, 75, 100, 150, 200, 300, 400, 800, 1600 })
            {
                var button = new ToolStripMenuItem($"{zoomLevel}%") { Name = $"Zoom{zoomLevel}ToolStripMenuItem" };
                
                button.Click += ZoomToolStripMenuItem_Click;
                button.Tag = zoomLevel / 100.0;

                if(zoomLevel == 100)
                    button.Checked = true;

                ZoomToolStripDropDownButton.DropDownItems.Insert(0, button);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(UnsavedChanges && e.CloseReason != CloseReason.ApplicationExitCall && e.CloseReason != CloseReason.WindowsShutDown)
            {
                if(!PromptUnsavedChanges("Would you like to save your changes before exiting?"))
                {
                    e.Cancel = true;
                    return;
                }
            }

            editedFont.Dispose();
            ToolStripManager.SaveSettings(this);
        }

        private void MainPictureBox_EditModeChanged(object sender, EventArgs e)
        {
            foreach(var item in EditingToolStrip.Items.OfType<ToolStripButton>())
            {
                item.Checked = false;
            }

            if(WindowState != FormWindowState.Maximized)
                Width -= MainSplitContainer.Panel2.Width;

            MainSplitContainer.Panel2Collapsed = true;
            MainSplitContainer.Panel2.Controls.Clear();

            switch(MainPictureBox.EditMode)
            {
                case EditMode.None:
                    break;

                case EditMode.CharacterSelect:
                    CharacterMappingToolStripButton.Checked = true;

                    MainSplitContainer.SplitterDistance = ClientSize.Width - characterMappingsControl.Width - MainSplitContainer.SplitterWidth;
                    MainSplitContainer.Panel2Collapsed = false;
                    MainSplitContainer.Panel2.Controls.Add(characterMappingsControl);
                    MainSplitContainer.Refresh();

                    if(WindowState != FormWindowState.Maximized)
                        Width += MainSplitContainer.Panel2.Width;
                    break;

                case EditMode.PaletteView:
                    ColorPaletteToolStripButton.Checked = true;

                    MainSplitContainer.SplitterDistance = ClientSize.Width - paletteViewer.Width - MainSplitContainer.SplitterWidth;
                    MainSplitContainer.Panel2Collapsed = false;
                    MainSplitContainer.Panel2.Controls.Add(paletteViewer);
                    MainSplitContainer.Refresh();

                    var palette = MainPictureBox.Image.GetColorPalette(true);
                    paletteViewer.SetPalette(palette, true);

                    if(WindowState != FormWindowState.Maximized)
                        Width += MainSplitContainer.Panel2.Width;
                    break;

                case EditMode.Pan:
                default:
                    PanToolStripButton.Checked = true;
                    break;
            }
        }

        private void MainPictureBox_ImageChanged(object sender, EventArgs e)
        {
            bool hasImage = MainPictureBox.Image != null;

            ImageContainerTableLayoutPanel.Visible = hasImage;
            InfoLabel.Visible = !hasImage;
            ZoomToolStripDropDownButton.Enabled = hasImage;

            SaveToolStripButton.Enabled = hasImage;
            SaveMenuItem.Enabled = hasImage;
            SaveAsMenuItem.Enabled = hasImage;
            FontPropertiesMenuItem.Enabled = hasImage;

            foreach(var item in EditingToolStrip.Items.OfType<ToolStripItem>())
            {
                item.Enabled = hasImage;
            }

            MainPictureBox.EditMode = EditMode.Pan;
        }

        private void MainPictureBox_BeginPan(object sender, PanEventArgs e)
        {
            panMouseLastPosition = ImageContainerTableLayoutPanel.AutoScrollPosition;
        }

        private void MainPictureBox_Panning(object sender, PanEventArgs e)
        {
            ImageContainerTableLayoutPanel.AutoScrollPosition = new Point(
                e.StartLocation.X - e.Location.X - panMouseLastPosition.X,
                e.StartLocation.Y - e.Location.Y - panMouseLastPosition.Y
            );

            panMouseLastPosition = ImageContainerTableLayoutPanel.AutoScrollPosition;
        }

        private void MainPictureBox_EndPan(object sender, PanEventArgs e)
        {
            // No-op
        }

        private void MainPictureBox_BeginSelection(object sender, EventArgs e)
        {
            // No-op
        }

        private void MainPictureBox_SelectionChanged(object sender, SelectionEventArgs e)
        {
            StatusLabel.Text = $"Origin: ({e.Selection.X}, {e.Selection.Y}). Size: {e.Selection.Width} x {e.Selection.Height}";
            MainStatusStrip.Refresh();
        }

        private void MainPictureBox_EndSelection(object sender, SelectionEventArgs e)
        {
            StatusLabel.Text = "";
            characterMappingsControl.SetCurrentCharacterBounds(e.Selection, true);
        }

        private void MainPictureBox_Paint(object sender, PaintEventArgs e)
        {
            if(selectedCharacterMapping != null && MainPictureBox.EditMode == EditMode.CharacterSelect)
            {
                var rect = new Rectangle(
                    selectedCharacterMapping.X, selectedCharacterMapping.Y,
                    selectedCharacterMapping.Width, selectedCharacterMapping.Height
                );
                var descender = selectedCharacterMapping.Descender;
                var zoomLevel = (float)MainPictureBox.Zoom;

                e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;
                e.Graphics.FillRectangle(CharacterBrush,
                    zoomLevel * rect.X + 0.5f,
                    zoomLevel * rect.Y + 0.5f,
                    zoomLevel * rect.Width - 0.5f,
                    zoomLevel * (rect.Height - descender) - 0.5f
                );

                if(descender > 0)
                {
                    e.Graphics.FillRectangle(DescenderBrush,
                        zoomLevel * rect.X + 0.5f,
                        zoomLevel * (rect.Y + rect.Height - descender) + 0.5f,
                        zoomLevel * rect.Width - 0.5f,
                        zoomLevel * descender - 0.5f
                    );
                }

                e.Graphics.DrawRectangle(SelectionPen,
                    zoomLevel * rect.X + 0.5f,
                    zoomLevel * rect.Y + 0.5f,
                    zoomLevel * rect.Width - 0.5f,
                    zoomLevel * rect.Height - 0.5f
                );
            }
        }

        private void MainPictureBox_ZoomChanged(object sender, EventArgs e)
        {
            var zoomLevel = MainPictureBox.Zoom;

            foreach(var menuItem in ZoomToolStripDropDownButton.DropDownItems.OfType<ToolStripMenuItem>().Where(m => m.Tag != null && m.Tag is double))
            {
                menuItem.Checked = (double)menuItem.Tag == zoomLevel;
            }

            ZoomToolStripDropDownButton.Text = $"{Math.Round(zoomLevel * 100.0, 1)}%";
        }

        private void ZoomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(!(sender is ToolStripMenuItem menuItem) || menuItem.Tag == null || !(menuItem.Tag is double))
                return;

            SetImageZoom((double)menuItem.Tag);
        }

        private void ZoomToolStripDropDownButton_ButtonClick(object sender, EventArgs e)
        {
            var menuItems = ZoomToolStripDropDownButton.DropDownItems
                .OfType<ToolStripMenuItem>()
                .Where(m => m.Tag != null && m.Tag is double)
                .ToArray();

            ToolStripMenuItem selectedItem = menuItems.Where(m => m.Checked).FirstOrDefault();
            ToolStripMenuItem nextItem = null;

        retry:
            if(selectedItem == null)
            {
                nextItem = menuItems.Single(m => (double)m.Tag == 1.0);
            }
            else
            {
                int index = Array.IndexOf(menuItems, selectedItem);

                if(index < 1)
                {
                    selectedItem = null;
                    goto retry;
                }

                nextItem = menuItems[index - 1];
            }
            
            SetImageZoom((double)nextItem.Tag);
        }

        private void ZoomLevelToolStripTextBox_LostFocus(object sender, EventArgs e)
        {
            string text = ZoomLevelToolStripTextBox.Text;

            if(string.IsNullOrWhiteSpace(text))
            {
                ZoomLevelToolStripTextBox.Text = "";
                return;
            }

            if(!ushort.TryParse(text, out var zoomLevel) || zoomLevel <= 0)
            {
                ZoomLevelToolStripTextBox.Text = "";
                MessageBox.Show($"'{text}' is not a valid zoom level.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SetImageZoom(zoomLevel / 100.0);
        }

        private void ZoomLevelToolStripTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(!AcceptedNumericKeyCodes.Contains(e.KeyCode))
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
            }

            if(e.KeyCode != Keys.Enter)
                return;

            e.SuppressKeyPress = true;
            e.Handled = true;

            ZoomLevelToolStripTextBox_LostFocus(sender, e);
        }

        private void CharacterMappingsControl_MappingsChanged(object sender, EventArgs e)
        {
            UnsavedChanges = true;
        }

        private void CharacterMappingsControl_SelectionChanged(object sender, EventArgs e)
        {
            selectedCharacterMapping = characterMappingsControl.SelectedCharacterMapping;
            MainPictureBox.Invalidate();
        }

        private void NewMenuItem_Click(object sender, EventArgs e)
        {
            if(!PromptUnsavedChanges("Would you like to save your changes before creating a new font?"))
                return;

            NewFont();
        }

        private void OpenMenuItem_Click(object sender, EventArgs e)
        {
            if(!PromptUnsavedChanges("Would you like to save your changes before opening another font?"))
                return;

            if(OpenFontFileDialog.ShowDialog() != DialogResult.OK)
                return;

            string file = OpenFontFileDialog.FileName;
            OpenFontFileDialog.FileName = "";

            switch(OpenFontFileDialog.FilterIndex)
            {
                case 1: // Standard Doom Writer font
                    LoadFontFromFile(file);
                    break;

                case 2: // Legacy Doom Writer v2.0.0 chart
                    LoadLegacyChartFromFile(file);
                    break;
            }
        }

        private void SaveMenuItem_Click(object sender, EventArgs e)
        {
            SaveChanges();
        }

        private void SaveAsMenuItem_Click(object sender, EventArgs e)
        {
            if(SaveFontFileDialog.ShowDialog() != DialogResult.OK)
                return;

            editedFontPath = SaveFontFileDialog.FileName;
            SaveFontFileDialog.FileName = "";

            SaveChanges();
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SetFontImageMenuItem_Click(object sender, EventArgs e)
        {
            if(MainPictureBox.Image != null)
            {
                if(MessageBox.Show("Are you sure you want to replace the current font image?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                    return;
            }

            if(ImageImportFileDialog.ShowDialog() != DialogResult.OK)
                return;

            SixLaborsImageRgba32 sixLaborsImage = null;
            DWImage dwImage = null;
            Image image = null;

            try
            {
                sixLaborsImage = SixLaborsImage.Load<Rgba32>(ImageImportFileDialog.FileName);
                dwImage = new DWImage(sixLaborsImage);
                image = dwImage.ToSystemDrawingImage();
            }
            catch(Exception ex)
            {
                dwImage.Dispose();
                sixLaborsImage.Dispose();
                image.Dispose();

                MessageBox.Show("Failed to import new font image:" + Environment.NewLine + $"{ex.GetType().FullName}: {ex.Message}", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            editedFont.Image?.Dispose();
            editedFont.Image = dwImage;

            MainPictureBox.Image?.Dispose();
            MainPictureBox.Image = image;

            UnsavedChanges = true;
        }

        private void FontPropertiesMenuItem_Click(object sender, EventArgs e)
        {
            UpdateCharacterMappings();

            using(var propertiesForm = new FontPropertiesForm())
            {
                propertiesForm.SavedChanges += (fsender, fe) => UnsavedChanges = true;
                propertiesForm.EditedFont = editedFont;
                propertiesForm.ShowDialog(this);
            }
        }

        private void HelpOnlineMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/Visual-Vincent/DoomWriter/wiki");
        }

        private void AboutMenuItem_Click(object sender, EventArgs e)
        {
            using(var aboutBox = new MainAboutBox())
            {
                aboutBox.ShowDialog();
            }
        }

        private void NewToolStripButton_Click(object sender, EventArgs e)
        {
            NewMenuItem_Click(NewMenuItem, EventArgs.Empty);
        }

        private void OpenToolStripButton_Click(object sender, EventArgs e)
        {
            OpenMenuItem_Click(OpenMenuItem, EventArgs.Empty);
        }

        private void SaveToolStripButton_Click(object sender, EventArgs e)
        {
            SaveMenuItem_Click(SaveMenuItem, EventArgs.Empty);
        }

        private void HelpToolStripButton_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/Visual-Vincent/DoomWriter/wiki");
        }

        private void PanToolStripButton_Click(object sender, EventArgs e)
        {
            MainPictureBox.EditMode = EditMode.Pan;
        }

        private void CharacterMappingToolStripButton_Click(object sender, EventArgs e)
        {
            MainPictureBox.EditMode = EditMode.CharacterSelect;
        }

        private void ColorPaletteToolStripButton_Click(object sender, EventArgs e)
        {
            MainPictureBox.EditMode = EditMode.PaletteView;
        }
    }
}

#pragma warning restore IDE0003 // Remove 'this'
