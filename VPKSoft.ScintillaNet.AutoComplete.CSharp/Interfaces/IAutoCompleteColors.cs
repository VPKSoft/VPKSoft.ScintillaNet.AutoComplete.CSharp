using System.Drawing;

namespace VPKSoft.ScintillaNet.AutoComplete.CSharp.Interfaces
{
    /// <summary>
    /// An interface for common colors for the call tip.
    /// </summary>
    public interface IAutoCompleteColors
    {
        /// <summary>
        /// Gets or sets the call tip color.
        /// </summary>
        /// <value>The call tip color.</value>
        Color ColorCallTip { get; set; }

        /// <summary>
        /// Gets or sets the background color of the up and down arrows.
        /// </summary>
        /// <value>The background color of the up and down arrows.</value>
        Color ColorBackgroundUpDownArrow { get; set; }

        /// <summary>
        /// Gets or sets the background color for the type image.
        /// </summary>
        /// <value>The background color for the type image.</value>
        Color ColorBackgroundTypeImage { get; set; }

        /// <summary>
        /// Gets or sets the background color of the "X of Y" text in the call tip.
        /// </summary>
        /// <value>The background color of the "X of Y" text in the call tip.</value>
        Color ColorBackgroundNumOfNum { get; set; }

        /// <summary>
        /// Gets or sets the foreground color of the "X of Y" text in the call tip.
        /// </summary>
        /// <value>The foreground color of the "X of Y" text in the call tip.</value>
        Color ColorForegroundNumOfNum { get; set; }

        /// <summary>
        /// Gets or sets the up left border color of the call tip.
        /// </summary>
        /// <value>The up left border color of the call tip.</value>
        Color ColorUpLeftBorder { get; set; }

        /// <summary>
        /// Gets or sets the bottom right border color of the call tip.
        /// </summary>
        /// <value>The bottom right border color of the call tip.</value>
        Color ColorBottomRightBorder { get; set; }
    }
}
