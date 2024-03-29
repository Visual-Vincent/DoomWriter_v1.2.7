using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DoomWriter.Interfaces;

namespace DoomWriter
{
    /// <summary>
    /// An in-memory font provider.
    /// </summary>
    /// <typeparam name="TImage">The type of image used by the font.</typeparam>
    /// <typeparam name="TGlyph">The type of glyphs used by the font.</typeparam>
    public sealed class FontProvider<TImage, TGlyph> : IFontProvider<TImage, TGlyph>
        where TImage : IImage
        where TGlyph : IGlyph
    {
        /// <summary>
        /// Gets the lookup table of fonts provided by this font provider.
        /// </summary>
        public Dictionary<string, Font<TImage, TGlyph>> Fonts { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FontProvider{TImage, TGlyph}"/> class.
        /// </summary>
        public FontProvider()
            : this(true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FontProvider{TImage, TGlyph}"/> class.
        /// </summary>
        /// <param name="caseSensitive">Whether or not the font lookup is case sensitive.</param>
        public FontProvider(bool caseSensitive)
        {
            Fonts = new Dictionary<string, Font<TImage, TGlyph>>(caseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase);
        }

        /// <inheritdoc/>
        public Font<TImage, TGlyph> FromName(string fontName)
        {
            if(!Fonts.TryGetValue(fontName, out var font))
                return null;

            return font;
        }
    }

    /// <summary>
    /// The default in-memory font provider.
    /// </summary>
    public sealed class FontProvider : IFontProvider<Image, Glyph>
    {
        private readonly FontProvider<Image, Glyph> fontProvider;

        /// <summary>
        /// Gets the lookup table of fonts provided by this font provider.
        /// </summary>
        public Dictionary<string, Font<Image, Glyph>> Fonts => fontProvider.Fonts;

        /// <summary>
        /// Initializes a new instance of the <see cref="FontProvider"/> class.
        /// </summary>
        public FontProvider()
        {
            fontProvider = new FontProvider<Image, Glyph>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FontProvider"/> class.
        /// </summary>
        /// <param name="caseSensitive">Whether or not the font lookup is case sensitive.</param>
        public FontProvider(bool caseSensitive)
        {
            fontProvider = new FontProvider<Image, Glyph>(caseSensitive);
        }

        /// <inheritdoc/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Font<Image, Glyph> FromName(string fontName)
        {
            return fontProvider.FromName(fontName);
        }
    }
}
