using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

using SixLaborsImage = SixLabors.ImageSharp.Image;

namespace DoomWriter
{
    /// <summary>
    /// A static class containing methods for converting Doom Writer fonts between different formats.
    /// </summary>
    public static class FontConverter
    {
        private const int DefaultEmptyLineHeight = 12;
        private const int DefaultLineHeight = 4;
        private const int DefaultTabWidth = 4;

        private static readonly char[] LegacyChartGlyphs =
            new char[] {'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
                        'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
                        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
                        'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
                        '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '(', ')', '[',
                        ']', '%', '+', '?', '"', ':', ';', '/', '\\', '_', '-', '=', '$',
                        '!', '<', '>', '^', '\'', '.', ',', '*', '#', '@', '&'};

        private static string DecryptLegacyChartData(byte[] chartData)
        {
            chartData = Convert.FromBase64String(Encoding.UTF8.GetString(chartData));

            using(var aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                aes.Key = Encoding.ASCII.GetBytes("4FG68FTY1RME95IW");
                aes.IV = Encoding.ASCII.GetBytes("WP47GUZANDJQFK3E");

                using(var decryptor = aes.CreateDecryptor())
                using(var encryptedStream = new MemoryStream(chartData))
                using(var cryptoStream = new CryptoStream(encryptedStream, decryptor, CryptoStreamMode.Read))
                using(var streamReader = new StreamReader(cryptoStream, Encoding.UTF8, true))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Converts a chart from Doom Writer v2.0.0 into the new font format.
        /// </summary>
        /// <param name="chartData">The raw chart data to convert.</param>
        public static Font ConvertLegacyChart(byte[] chartData)
        {
            Font font = new Font();
            Image<Rgba32> chartImage = null;

            try
            {
                string[] sections = DecryptLegacyChartData(chartData).Split('·');
                ThrowHelper.Assert(sections.Length >= 3, new FormatException("Data is not a valid legacy Doom Writer chart"));

                chartImage = SixLaborsImage.Load<Rgba32>(Convert.FromBase64String(sections[0]));
                font.Image = new Image(chartImage);

                for(int i = 1; i < sections.Length; i++)
                    sections[i] = DecryptLegacyChartData(Encoding.UTF8.GetBytes(sections[i]));

                string[] settings = sections[2].Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                ThrowHelper.Assert(settings.Length >= 4, new FormatException("Data is not a valid legacy Doom Writer chart"));

                if(!ushort.TryParse(settings[0], out var spaceWidth))
                    throw new FormatException("Data is not a valid legacy Doom Writer chart");

                if(!short.TryParse(settings[1], out var letterSpacing))
                    throw new FormatException("Data is not a valid legacy Doom Writer chart");

                font.EmptyLineHeight = DefaultEmptyLineHeight;
                font.LineHeight = DefaultLineHeight;
                font.TabWidth = DefaultTabWidth;
                font.SpaceWidth = spaceWidth;
                font.LetterSpacing = (short)-letterSpacing; // In Doom Writer v2.0.0, letter spacing is inverted

                // In Doom Writer v2.0.0, the rendered space width was affected by the letter spacing
                // In v3.0.0 and onward this is no longer the case, thus subtract it from the space width
                if(font.SpaceWidth >= letterSpacing)
                    font.SpaceWidth = (ushort)(font.SpaceWidth - letterSpacing);

                char[] descenderGlyphs = settings[2].Split(':').Select(s => {
                    ThrowHelper.Assert(s.Length == 1, new FormatException("Data is not a valid legacy Doom Writer chart"));
                    return s[0];
                }).ToArray();

                int[] descenderValues = settings[3].Split(':').Select(s => {
                    ThrowHelper.Assert(int.TryParse(s, out var d), new FormatException("Data is not a valid legacy Doom Writer chart"));
                    return d;
                }).ToArray();

                Dictionary<char, int> descenders = new Dictionary<char, int>();

                for(int i = 0; i < descenderGlyphs.Length && i < descenderValues.Length; i++)
                {
                    descenders[descenderGlyphs[i]] = descenderValues[i];
                }

                string[] glyphDefs = sections[1].Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                for(int i = 0; i < glyphDefs.Length && i < LegacyChartGlyphs.Length; i++)
                {
                    string definition = glyphDefs[i]
                        .Trim()
                        .TrimStart('{')
                        .TrimEnd('}');

                    string[] bounds = definition.Split(':');

                    if(bounds.Length != 4)
                        continue;

                    if(
                        !int.TryParse(bounds[0], out var x) ||
                        !int.TryParse(bounds[1], out var y) ||
                        !int.TryParse(bounds[2], out var width) ||
                        !int.TryParse(bounds[3], out var height)
                    )
                        continue;

                    char c = LegacyChartGlyphs[i];

                    descenders.TryGetValue(c, out var descender);
                    font.Glyphs[c] = new Glyph(x, y, width, height, descender);
                }

                return font;
            }
            catch(FormatException)
            {
                font?.Dispose();
                throw;
            }
            catch(Exception ex)
            {
                font?.Dispose();
                throw new FormatException("Data is not a valid legacy Doom Writer chart", ex);
            }
        }
    }
}
