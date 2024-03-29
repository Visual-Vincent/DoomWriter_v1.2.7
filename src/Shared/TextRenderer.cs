using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DoomWriter.Interfaces;
using SixLabors.ImageSharp.PixelFormats;

using FontModifier = DoomWriter.FontModifier<DoomWriter.Font<DoomWriter.Image, DoomWriter.Glyph>>;

namespace DoomWriter
{
    /// <summary>
    /// The default Doom Writer text renderer.
    /// </summary>
    public class TextRenderer : TextRendererBase, ITextRenderer<Image, Glyph>
    {
        private readonly Font<Image, Glyph> DefaultFont;

        /// <summary>
        /// Gets or sets the font provider used to lookup fonts when the text renderer encounters a font escape sequence.
        /// </summary>
        public IFontProvider<Image, Glyph> FontProvider { get; set; }

        /// <summary>
        /// Gets the translation table used by the text renderer.
        /// </summary>
        public Dictionary<string, ColorTranslation> Translations { get; } = new Dictionary<string, ColorTranslation>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Initializes a new instance of the <see cref="TextRenderer"/> class.
        /// </summary>
        /// <param name="defaultFont">The default font to use when rendering text.</param>
        public TextRenderer(Font<Image, Glyph> defaultFont)
        {
            if(defaultFont == null)
                throw new ArgumentNullException(nameof(defaultFont));

            DefaultFont = defaultFont;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextRenderer"/> class.
        /// </summary>
        /// <param name="defaultFont">The default font to use when rendering text.</param>
        /// <param name="fontProvider">A font provider used to lookup fonts when the text renderer encounters a font escape sequence.</param>
        public TextRenderer(Font<Image, Glyph> defaultFont, IFontProvider<Image, Glyph> fontProvider)
            : this(defaultFont)
        {
            FontProvider = fontProvider;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextRenderer"/> class.
        /// </summary>
        /// <param name="defaultFont">The default font to use when rendering text.</param>
        /// <param name="translationsTable">A table containing all color translations that will be made available to the renderer.</param>
        public TextRenderer(Font<Image, Glyph> defaultFont, IDictionary<string, ColorTranslation> translationsTable)
            : this(defaultFont)
        {
            foreach(var kvp in translationsTable)
            {
                Translations.Add(kvp.Key, kvp.Value.Clone());
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextRenderer"/> class.
        /// </summary>
        /// <param name="defaultFont">The default font to use when rendering text.</param>
        /// <param name="fontProvider">A font provider used to lookup fonts when the text renderer encounters a font escape sequence.</param>
        /// <param name="translationsTable">A table containing all color translations that will be made available to the renderer.</param>
        public TextRenderer(Font<Image, Glyph> defaultFont, IFontProvider<Image, Glyph> fontProvider, IDictionary<string, ColorTranslation> translationsTable)
            : this(defaultFont, translationsTable)
        {
            FontProvider = fontProvider;
        }

        /// <inheritdoc/>
        public override Image Render(string text)
        {
            return Render(text, DefaultFont);
        }

        /// <inheritdoc/>
        public Image Render(string text, Font<Image, Glyph> font)
        {
            var measurement = Measure(text, font);
            int surfaceWidth = measurement.Width.Clamp(1, int.MaxValue);
            int surfaceHeight = measurement.Height.Clamp(1, int.MaxValue);
            var surface = new ImageSurface<Rgba32>(surfaceWidth, surfaceHeight);

            ColorTranslation currentTranslation = null;
            Font<Image, Glyph> currentFont = font;
            TextAlignment currentAlignment = TextAlignment.Left;

            int y = 0;

            foreach(var line in measurement.Lines)
            {
                int glyphIndex = 0;
                int offsetX = 0;

                var modifier = line.RenderModifiers.GetEnumerator();
                modifier.MoveNext();

                void ProcessModifiers()
                {
                    while(modifier.Current != null && modifier.Current.Position == glyphIndex)
                    {
                        switch(modifier.Current)
                        {
                            case ColorTranslationModifier renderModifier:
                                currentTranslation = renderModifier.Translation;
                                break;

                            case FontModifier renderModifier:
                                currentFont = renderModifier.Font ?? font;
                                break;

                            case TextAlignmentModifier renderModifier:
                                currentAlignment = renderModifier.Alignment;
                                break;
                        }

                        // Cache translations for the built-in font type
                        if(currentFont is Font builtInFont && currentTranslation != null && !builtInFont.HasTranslation(currentTranslation))
                        {
                            builtInFont.AddTranslation(currentTranslation);
                        }

                        modifier.MoveNext();
                    }
                }
                
                // Process any modifiers at the beginning of the line
                ProcessModifiers();
                glyphIndex = -1;

                switch(currentAlignment)
                {
                    case TextAlignment.Center:
                        offsetX = (int)Math.Floor(surfaceWidth / 2.0 - line.Width / 2.0);
                        break;

                    case TextAlignment.Right:
                        offsetX = surfaceWidth - line.Width;
                        break;
                }

                foreach(var g in line.Glyphs)
                {
                    glyphIndex++;
                    ProcessModifiers();

                    currentFont.DrawGlyph((Glyph)g.Glyph, surface, offsetX + g.X, y + (line.Height - line.TallestDescender - g.Glyph.Height + g.Glyph.Descender), currentTranslation);
                }

                // Process any modifiers at the end of the line
                glyphIndex++;
                ProcessModifiers();

                y += line.Height + line.LineHeight;
            }

            return new Image(surface.GetImage());
        }

        /// <inheritdoc/>
        public async Task<Image> RenderAsync(string text, Font<Image, Glyph> font)
        {
            return await Task.Run(() => Render(text, font));
        }

        private TextMeasurementResult Measure(string text, Font<Image, Glyph> font)
        {
            if(text == null)
                throw new ArgumentNullException(nameof(text));

            if(font == null)
                throw new ArgumentNullException(nameof(font));

            if(text.Length <= 0)
                throw new ArgumentException("No text specified", nameof(text));

            int width = 0;
            int height = 0;

            var lines = new List<TextMeasuredLine>();
            var currentFont = font;

            using(StringReader reader = new StringReader(text))
            {
                string line;
                while((line = reader.ReadLine()) != null)
                {
                    int x = 0;
                    int lineHeight = line.Length <= 0 ? currentFont.EmptyLineHeight : 0;
                    int tallestDescender = 0;
                    int backslashCount = 0;
                    int glyphIndex = 0;

                    var fontLineHeight = currentFont.LineHeight;
                    var letterSpacing = currentFont.LetterSpacing;
                    var spaceWidth = currentFont.SpaceWidth + currentFont.LetterSpacing;
                    var tabWidth = currentFont.TabWidth;

                    var glyphs = new List<RenderedGlyph>();
                    var renderModifiers = new List<TextRenderModifier>();

                    // Ensure spaces are always the same size
                    if(line.Length > 0 && (line[0] == ' ' || line[0] == '\t'))
                    {
                        x += letterSpacing;
                    }

                    char c = (char)0;
                    char pc = (char)0;

                    for(int i = 0; i < line.Length; i++)
                    {
                        c = line[i];

                        // Ensure spaces are always the same size
                        if((pc == ' ' || pc == '\t') && (c != ' ' && c != '\t'))
                        {
                            x -= letterSpacing;
                        }

                        backslashCount = (c == '\\') ? backslashCount + 1 : 0;

                        switch(c)
                        {
                            case ' ':
                                x += spaceWidth;
                                continue;

                            case '\t':
                                x += tabWidth * spaceWidth;
                                continue;

                            case '\\':
                                if(backslashCount % 2 == 0)
                                    continue;

                                var renderModifier = ParseEscapeSequence(ref i, line, glyphIndex);

                                if(renderModifier != null)
                                {
                                    if(renderModifier is FontModifier fontModifier)
                                        currentFont = fontModifier.Font ?? font;

                                    renderModifiers.Add(renderModifier);
                                    backslashCount = 0;

                                    continue;
                                }

                                break;
                        }

                        if(!currentFont.Glyphs.TryGetValue(c, out var glyph))
                        {
                            x += spaceWidth;
                            continue;
                        }

                        if(glyphIndex > 0)
                            x += currentFont.KernTable[pc, c];

                        glyphIndex++;
                        glyphs.Add(new RenderedGlyph(c, glyph, x, 0)); // y is calculated when rendering

                        x += glyph.Width + letterSpacing;

                        if(glyph.Height > lineHeight)
                            lineHeight = glyph.Height;

                        if(glyph.Descender > tallestDescender)
                            tallestDescender = glyph.Descender;

                        if(currentFont.LineHeight > fontLineHeight)
                            fontLineHeight = currentFont.LineHeight;

                        pc = c;
                    }

                    if(x - letterSpacing > width)
                        width = x - letterSpacing;

                    height += lineHeight + fontLineHeight;

                    lines.Add(new TextMeasuredLine(glyphs, x - letterSpacing, lineHeight, fontLineHeight, tallestDescender, renderModifiers));
                }

                height -= lines.Last().LineHeight;
            }

            return new TextMeasurementResult(lines, width, height);
        }

        private TextRenderModifier ParseEscapeSequence(ref int index, string text, int glyphIndex)
        {
            if(index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), index, "Index must be greater than or equal to zero");

            if(text == null)
                throw new ArgumentNullException(nameof(text));

            if(glyphIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(glyphIndex), glyphIndex, "Glyph index must be greater than or equal to zero");

            int start = index + 1;

            if(start + 1 >= text.Length)
                return null;

            var character = char.ToLower(text[start++]);

            switch(character)
            {
                case 'c':
                    char colorCode = char.ToUpperInvariant(text[start]);

                    if(colorCode >= 'A' && colorCode <= 'Z')
                    {
                        if(!Translations.TryGetValue(colorCode.ToString(), out var translation))
                            break;

                        index += 2;
                        return new ColorTranslationModifier(glyphIndex, translation);
                    }
                    else if(colorCode == '-')
                    {
                        index += 2;
                        return new ColorTranslationModifier(glyphIndex, null);
                    }

                    string colorName = ParseBracketedName(text, start);

                    if(string.IsNullOrEmpty(colorName))
                        break;

                    if(!Translations.TryGetValue(colorName, out var namedTranslation))
                        break;

                    index = start + colorName.Length + 1;

                    return new ColorTranslationModifier(glyphIndex, namedTranslation);

                case 'f':
                    if(FontProvider == null)
                        break;

                    if(text[start] == '-')
                    {
                        index += 2;
                        return new FontModifier<Font<Image, Glyph>>(glyphIndex, null);
                    }

                    string fontName = ParseBracketedName(text, start);

                    if(string.IsNullOrEmpty(fontName))
                        break;

                    var font = FontProvider.FromName(fontName);

                    if(font == null)
                        break;

                    index = start + fontName.Length + 1;

                    return new FontModifier<Font<Image, Glyph>>(glyphIndex, font);

                case 'a':
                    if(glyphIndex != 0)
                        break;

                    string alignment = ParseBracketedName(text, start);

                    if(string.IsNullOrEmpty(alignment))
                        break;

                    if(!Enum.TryParse<TextAlignment>(alignment, true, out var textAlignment))
                        break;

                    index = start + alignment.Length + 1;

                    return new TextAlignmentModifier(textAlignment);
            }

            return null;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private string ParseBracketedName(string text, int index)
        {
            if(text == null)
                throw new ArgumentNullException(nameof(text));

            if(index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), index, "Index must be greater than or equal to zero");

            if(text[index] != '[' || index + 1 >= text.Length || text[++index] == ']')
                return null;

            int end = text.IndexOf(']', index);

            if(end < 0)
                return null;

            return text.Substring(index, end - index);
        }
    }
}
