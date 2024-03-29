using System;

namespace DoomWriter
{
    /// <summary>
    /// A render modifier that changes the horizontal alignment of the rendered text.
    /// </summary>
    public sealed class TextAlignmentModifier : TextRenderModifier
    {
        /// <summary>
        /// Gets the horizontal text alignment that will be used by the text renderer.
        /// </summary>
        public TextAlignment Alignment { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TextAlignmentModifier"/> class.
        /// </summary>
        /// <param name="alignment">The horizontal text alignment that will be used by the text renderer.</param>
        /// <remarks>The character position is always 0 since this needs to be applied at the start of the line.</remarks>
        public TextAlignmentModifier(TextAlignment alignment)
            : base(0)
        {
            // Since this is .NET Standard, Enum.IsDefined<T>() is not supported
            // Workaround to support reflection-free NativeAOT compilation
            switch(alignment)
            {
                case TextAlignment.Left:
                case TextAlignment.Center:
                case TextAlignment.Right:
                    Alignment = alignment;
                    break;

                default:
                    throw new ArgumentException($"'{alignment}' is not a valid text alignment", nameof(alignment));
            }
        }
    }

    /// <summary>
    /// Specifies the horizontal alignment of text.
    /// </summary>
    public enum TextAlignment : int
    {
        /// <summary>
        /// The text should be aligned to the left.
        /// </summary>
        Left = 0,

        /// <summary>
        /// The text should be aligned to the center.
        /// </summary>
        Center,

        /// <summary>
        /// The text should be aligned to the right.
        /// </summary>
        Right
    }
}
