using System;
using System.Diagnostics;
using System.IO;
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

        private DWFont DefaultRenderFont;

        private Process currentProcess;
        private TextRenderer renderer;

        private Task<Image> renderTask = Task.FromResult<Image>(null);
        private Func<Task<Image>> reRenderTask;
        private readonly Stopwatch renderStopwatch = new Stopwatch();

        private readonly object lockObj = new object();

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
                image.Dispose();

                ToolStripRenderTimeLabel.Text = $"Render time: {renderStopwatch.Elapsed}";
            }
            catch(Exception ex)
            {
                throw new DoomWriterException("Failed to render greeting message", ex);
            }
        }

        private System.Drawing.Image ConvertDWImage(Image image)
        {
            System.Drawing.Image result = null;

            try
            {
                result = new System.Drawing.Bitmap(image.Width, image.Height);

                using(var memoryStream = new MemoryStream())
                {
                    image.Save(memoryStream, ImageFormat.PNG);
                    memoryStream.Position = 0;

                    using(var img = System.Drawing.Image.FromStream(memoryStream))
                    using(var g = System.Drawing.Graphics.FromImage(result))
                    {
                        g.DrawImage(img, new System.Drawing.Rectangle(System.Drawing.Point.Empty, img.Size));
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
            renderStopwatch.Stop();

            ResultPictureBox.Image?.Dispose();
            ResultPictureBox.Image = image != null ? ConvertDWImage(image) : null;
            image?.Dispose();

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