using System;
using System.Drawing;
using System.Windows.Forms;

namespace DoomWriter.GUI
{
    /// <summary>
    /// Renders a <see cref="ToolStrip"/> using the system colors, surrounded by a one pixel wide border.
    /// </summary>
    public class BorderedToolStripRenderer : ToolStripSystemRenderer
    {
        public ToolStripStatusLabelBorderSides Borders { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BorderedToolStripRenderer"/> class.
        /// </summary>
        /// <param name="borders">The borders that the <see cref="ToolStrip"/> should have.</param>
        public BorderedToolStripRenderer(ToolStripStatusLabelBorderSides borders)
        {
            if(!Enum.IsDefined(typeof(ToolStripStatusLabelBorderSides), borders))
                throw new ArgumentException($"'{borders}' is not a valid border side", nameof(borders));

            Borders = borders;
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            if(e.ToolStrip.GetType() != typeof(ToolStrip) && e.ToolStrip.GetType() != typeof(StatusStrip))
            {
                base.OnRenderToolStripBorder(e);
                return;
            }

            if(Borders.HasFlag(ToolStripStatusLabelBorderSides.Left))
                e.Graphics.DrawLine(Pens.Black, 0, 0, 0, e.ToolStrip.Height - 1);

            if(Borders.HasFlag(ToolStripStatusLabelBorderSides.Right))
                e.Graphics.DrawLine(Pens.Black, e.ToolStrip.Width - 1, 0, e.ToolStrip.Width - 1, e.ToolStrip.Height - 1);

            if(Borders.HasFlag(ToolStripStatusLabelBorderSides.Top))
                e.Graphics.DrawLine(Pens.Black, 0, 0, e.ToolStrip.Width - 1, 0);

            if(Borders.HasFlag(ToolStripStatusLabelBorderSides.Bottom))
                e.Graphics.DrawLine(Pens.Black, 0, e.ToolStrip.Height - 1, e.ToolStrip.Width - 1, e.ToolStrip.Height - 1);
        }
    }
}
