using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.CompilerServices;
using DoomWriter.Interfaces;

namespace DoomWriter
{
    /// <summary>
    /// A font provider that loads a font from disk only when the font is requested.
    /// </summary>
    /// <remarks>Once a font has been loaded from disk, it's cached in memory. Lookup is either case sensitive or case insensitive depending on the current file system.</remarks>
    public sealed class LazyFontProvider : IFontProvider<Image, Glyph>, IDisposable
    {
        private readonly Dictionary<string, Font> fontCache = new Dictionary<string, Font>(FileSystem.IsCaseSensitive ? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase);
        private readonly string fontsDirectory;

        /// <summary>
        /// Gets the collection of fonts currently loaded into memory.
        /// </summary>
        public IReadOnlyDictionary<string, Font> LoadedFonts => fontCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyFontProvider"/> class.
        /// </summary>
        /// <param name="fontsDirectory">The path to the directory from which to load font files.</param>
        public LazyFontProvider(string fontsDirectory)
        {
            if(string.IsNullOrWhiteSpace(fontsDirectory))
                throw new ArgumentNullException(nameof(fontsDirectory));

            this.fontsDirectory = fontsDirectory;
        }

        /// <inheritdoc/>
        public Font<Image, Glyph> FromName(string fontName)
        {
            ThrowIfDisposed();

            if(fontCache.TryGetValue(fontName, out var font))
                return font;

            if(!Directory.Exists(fontsDirectory))
                return null;

            string filePath = Path.Combine(fontsDirectory, $"{fontName}.dwfont");

            if(!File.Exists(filePath))
                return null;
            
            font = Font.Load<Font>(filePath);
            fontCache.Add(fontName, font);

            return font;
        }

        #region IDisposable Support
        private bool disposedValue;

        private void Dispose(bool disposing)
        {
            if(!disposedValue)
            {
                if(disposing)
                {
                    // Dispose managed resources
                    foreach(var kvp in fontCache)
                    {
                        kvp.Value.Dispose();
                    }

                    fontCache.Clear();
                }

                // Free unmanaged resources

                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~LazyFontProvider()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Throws an <see cref="ObjectDisposedException"/> if the current object is disposed.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ThrowIfDisposed()
        {
            if(disposedValue)
                throw new ObjectDisposedException(typeof(LazyFontProvider).FullName);
        }
        #endregion
    }
}
