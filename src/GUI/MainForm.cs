using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Tga;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

using DWFont = DoomWriter.Font;
using SixLaborsImage = SixLabors.ImageSharp.Image;

namespace DoomWriter.GUI
{
    public partial class MainForm : Form
    {
        private const string GreetingMessage = 
            @"\a[center]" +
            @"\cJWelcome to \cLD\cIo\cKo\cFm \cDW\cQr\cHi\cTt\cLe\cRr\cJ!" + "\n\n" +
            @"\cJStart \cDtyping\cJ in the \cFbox\cJ below to get started.";

        internal static string DefaultFontPath => Path.Combine(AppContext.BaseDirectory, "Default.dwfont");
        internal static string FontsDirectory = Path.Combine(AppContext.BaseDirectory, "Fonts");

        private static readonly HashSet<Keys> AcceptedNumericKeyCodes = new HashSet<Keys>() {
            Keys.Enter, Keys.Back, Keys.Delete, Keys.Home, Keys.End, Keys.Left, Keys.Right, Keys.Up, Keys.Down,
            Keys.D0, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9,
            Keys.NumPad0, Keys.NumPad1, Keys.NumPad2, Keys.NumPad3, Keys.NumPad4, Keys.NumPad5,
            Keys.NumPad6, Keys.NumPad7, Keys.NumPad8, Keys.NumPad9
        };

        private DWFont DefaultRenderFont;

        private Process currentProcess;
        private TextRenderer renderer;
        private Image renderedImage;

        private Task<Image> renderTask = Task.FromResult<Image>(null);
        private Func<Task<Image>> reRenderTask;
        private readonly Stopwatch renderStopwatch = new Stopwatch();

        private readonly object lockObj = new object();

        private double _renderScaleFactor = 1.0;

        private double RenderScaleFactor
        {
            get {
                return _renderScaleFactor;
            }
            set {
                if(_renderScaleFactor == value)
                    return;

                if(value < 0.1)
                    _renderScaleFactor = 0.1;
                else if(value > 32.0)
                    _renderScaleFactor = 32.0;
                else
                    _renderScaleFactor = value;

                foreach(var menuItem in RenderScaleToolStripSplitButton.DropDownItems.OfType<ToolStripMenuItem>().Where(m => m.Tag != null && m.Tag is double))
                {
                    menuItem.Checked = (double)menuItem.Tag == _renderScaleFactor;
                }

                RenderScaleToolStripSplitButton.Text = $"{Math.Round(_renderScaleFactor * 100.0, 1)}%";

                if(renderedImage == null)
                    return;

                ResultPictureBox.Image?.Dispose();
                ResultPictureBox.Image = ConvertDWImage(renderedImage, _renderScaleFactor);
            }
        }

        public MainForm()
        {
            InitializeComponent();
        }

        internal void Initialize()
        {
            if(renderer != null)
                return;

            DefaultRenderFont = DWFont.Load<DWFont>(DefaultFontPath);

            var fontProvider = new LazyFontProvider(FontsDirectory);
            renderer = new TextRenderer(DefaultRenderFont, fontProvider, ColorTranslator.DefaultTranslations);
            
            try
            {
                renderStopwatch.Restart();
                var image = renderer.Render(GreetingMessage);
                renderStopwatch.Stop();

                ResultPictureBox.Image = ConvertDWImage(image);
                renderedImage = image;

                ToolStripRenderTimeLabel.Text = $"Render time: {renderStopwatch.Elapsed}";
            }
            catch(Exception ex)
            {
                throw new DoomWriterException("Failed to render greeting message", ex);
            }
        }

        private System.Drawing.Image ConvertDWImage(Image image, double scaleFactor = 1.0)
        {
            System.Drawing.Image result = null;

            int width = (int)Math.Ceiling(image.Width * scaleFactor);
            int height = (int)Math.Ceiling(image.Height * scaleFactor);

            try
            {
                result = new System.Drawing.Bitmap(width, height);

                using(var memoryStream = new MemoryStream())
                {
                    image.Save(memoryStream, ImageFormat.PNG);
                    memoryStream.Position = 0;

                    using(var img = System.Drawing.Image.FromStream(memoryStream))
                    using(var g = System.Drawing.Graphics.FromImage(result))
                    {
                        g.InterpolationMode = InterpolationMode.NearestNeighbor;
                        g.PixelOffsetMode = PixelOffsetMode.Half;

                        g.DrawImage(img,
                            new System.Drawing.Rectangle(System.Drawing.Point.Empty, result.Size),
                            new System.Drawing.Rectangle(System.Drawing.Point.Empty, img.Size),
                            System.Drawing.GraphicsUnit.Pixel
                        );

                        return result;
                    }
                }
            }
            catch
            {
                result?.Dispose();
                throw;
            }
        }

        private async Task<Image> RenderFunc(string text)
        {
            if(string.IsNullOrEmpty(text))
                return null;

            return await renderer.RenderAsync(text);
        }

        private string ToHumanReadableFileSize(long size, byte decimals)
        {
            if(size < 1024)
                return $"{size} B";

            double result = size / 1024;
            string format = $"0.{new string('0', decimals)}";

            if(size < Math.Pow(1024, 2))
                return $"{Math.Round(result, decimals).ToString(format)} KB";

            result /= 1024;

            if(size < Math.Pow(1024, 3))
                return $"{Math.Round(result, decimals).ToString(format)} MB";

            result /= 1024;

            if(size < Math.Pow(1024, 4))
                return $"{Math.Round(result, decimals).ToString(format)} GB";

            result /= 1024;

            return $"{Math.Round(result, decimals).ToString(format)} TB";
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if(renderer == null)
            {
                MessageBox.Show("Form wasn't initialized properly!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }

            currentProcess = Process.GetCurrentProcess();

            MainToolStrip.Renderer = new BorderedToolStripRenderer(ToolStripStatusLabelBorderSides.Bottom);
            MainStatusStrip.Renderer = new BorderedToolStripRenderer(ToolStripStatusLabelBorderSides.Top);

            foreach(int scalePercent in new int[] { 25, 50, 75, 100, 125, 150, 175, 200, 250, 300, 400, 800, 1600, 3200 })
            {
                var button = new ToolStripMenuItem($"{scalePercent}%") { Name = $"RenderScale{scalePercent}ToolStripMenuItem" };

                button.Click += RenderScaleToolStripMenuItem_Click;
                button.Tag = scalePercent / 100.0;

                if(scalePercent == 100)
                    button.Checked = true;

                RenderScaleToolStripSplitButton.DropDownItems.Add(button);
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            currentProcess?.Dispose();
            DefaultRenderFont?.Dispose();
        }

        private void MemoryUsageTimer_Tick(object sender, EventArgs e)
        {
            if(currentProcess == null)
                return;

            currentProcess.Refresh();

            ToolStripMemoryUsageLabel.Text = $"Memory usage: {ToHumanReadableFileSize(currentProcess.PrivateMemorySize64, 2)}";
        }

        private void InputTextBox_Enter(object sender, EventArgs e)
        {
            InputTextBox.Enter -= InputTextBox_Enter;
            InputTextBox.Text = "";
            InputTextBox.ForeColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.WindowText);
            InputTextBox.TextChanged += InputTextBox_TextChanged;
        }

        private async void InputTextBox_TextChanged(object sender, EventArgs e)
        {
            Image image = null;
            string text = InputTextBox.Text;

            ToolStripLabelStatus.Text = "Working...";

            lock(lockObj)
            {
                if(!renderTask.IsCompleted)
                {
                    reRenderTask = async () => await RenderFunc(text);
                    return;
                }

                renderStopwatch.Restart();
                renderTask = Task.Run(async () => await RenderFunc(text));
            }

            while(true)
            {
                try
                {
                    image = await renderTask;
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Rendering failed:" + Environment.NewLine + Environment.NewLine + ex.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    goto complete;
                }

                lock(lockObj)
                {
                    if(reRenderTask == null)
                        break;

                    image?.Dispose();

                    renderStopwatch.Restart();
                    renderTask = Task.Run(reRenderTask);
                    reRenderTask = null;
                }
            }

        complete:
            var displayImage = image != null
                ? ConvertDWImage(image, RenderScaleFactor)
                : null;

            renderStopwatch.Stop();

            renderedImage?.Dispose();
            ResultPictureBox.Image?.Dispose();
            ResultPictureBox.Image = displayImage;
            renderedImage = image;

            ToolStripRenderTimeLabel.Text = $"Render time: {renderStopwatch.Elapsed}";
            ToolStripLabelStatus.Text = "Ready";
        }

        private void SaveAsMenuItem_Click(object sender, EventArgs e)
        {
            if(ResultPictureBox.Image == null)
                return;

            if(ImageSaveFileDialog.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                using(var memoryStream = new MemoryStream())
                {
                    ResultPictureBox.Image.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                    memoryStream.Position = 0;

                    using(var image = SixLaborsImage.Load<Rgba32>(memoryStream))
                    {
                        switch((ImageFilters)ImageSaveFileDialog.FilterIndex)
                        {
                            case ImageFilters.PNG:
                                image.SaveAsPng(ImageSaveFileDialog.FileName);
                                break;

                            case ImageFilters.BMP:
                                image.SaveAsBmp(ImageSaveFileDialog.FileName, new BmpEncoder() { BitsPerPixel = BmpBitsPerPixel.Pixel32, SupportTransparency = true });
                                break;

                            case ImageFilters.GIF:
                                image.SaveAsGif(ImageSaveFileDialog.FileName);
                                break;

                            case ImageFilters.TGA:
                                image.SaveAsTga(ImageSaveFileDialog.FileName, new TgaEncoder() { BitsPerPixel = TgaBitsPerPixel.Pixel32 });
                                break;

                            // Image formats that don't support transparency
                            case ImageFilters.JPEG:
                            case ImageFilters.TIFF:
                                using(var newImage = new Image<Rgba32>(image.Width, image.Height, Color.Cyan))
                                {
                                    newImage.Mutate(c => { c.DrawImage(image, 1.0f); });

                                    switch((ImageFilters)ImageSaveFileDialog.FilterIndex)
                                    {
                                        case ImageFilters.JPEG:
                                            newImage.SaveAsJpeg(ImageSaveFileDialog.FileName, new JpegEncoder() { Quality = 100 });
                                            break;

                                        case ImageFilters.TIFF:
                                            newImage.SaveAsTiff(ImageSaveFileDialog.FileName);
                                            break;
                                    }
                                }
                                break;

                            default:
                                MessageBox.Show("Failed to save image:" + Environment.NewLine + "No file type selected.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                                break;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Failed to save image:" + Environment.NewLine + Environment.NewLine + ex.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
#pragma warning disable IDE0003 // Remove 'this'
            this.Close();
#pragma warning restore IDE0003
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

        private void SaveToolStripButton_Click(object sender, EventArgs e)
        {
            SaveAsMenuItem_Click(SaveAsMenuItem, EventArgs.Empty);
        }

        private void RenderScaleToolStripSplitButton_ButtonClick(object sender, EventArgs e)
        {
            var menuItems = RenderScaleToolStripSplitButton.DropDownItems
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

                if(index >= menuItems.Length - 1)
                {
                    selectedItem = null;
                    goto retry;
                }

                nextItem = menuItems[index + 1];
            }
            
            try
            {
                RenderScaleFactor = (double)nextItem.Tag;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Failed to set render scale:" + Environment.NewLine + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RenderScaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(!(sender is ToolStripMenuItem menuItem) || menuItem.Tag == null || !(menuItem.Tag is double))
                return;

            try
            {
                RenderScaleFactor = (double)menuItem.Tag;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Failed to set render scale:" + Environment.NewLine + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RenderScaleToolStripTextBox_LostFocus(object sender, EventArgs e)
        {
            string text = RenderScaleToolStripTextBox.Text;
            
            if(string.IsNullOrWhiteSpace(text))
            {
                RenderScaleToolStripTextBox.Text = "";
                return;
            }

            if(!ushort.TryParse(text, out var renderScale) || renderScale <= 0)
            {
                RenderScaleToolStripTextBox.Text = "";
                MessageBox.Show($"'{text}' is not a valid render scale.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                RenderScaleFactor = renderScale / 100.0;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Failed to set render scale:" + Environment.NewLine + ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RenderScaleToolStripTextBox_KeyDown(object sender, KeyEventArgs e)
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

            RenderScaleToolStripTextBox_LostFocus(sender, e);
        }

        private enum ImageFilters
        {
            PNG = 1,
            BMP,
            JPEG,
            GIF,
            TIFF,
            TGA
        }
    }
}