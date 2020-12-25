#region License
/*
MIT License

Copyright(c) 2020 Petteri Kautonen

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
#endregion

using System;
using System.Collections.Generic;
using System.Drawing;
using ScintillaNET;

namespace VPKSoft.ScintillaNet.AutoComplete.CSharp.Interfaces
{
    /// <summary>
    /// An interface for the <see cref="Scintilla"/> to help with the call tip styles.
    /// </summary>
    public interface ICallTipStyling<T> where T: Enum
    {
        /// <summary>
        /// Gets or sets the background color for a call tip.
        /// </summary>
        /// <value>The the background color for the call tip.</value>
        Color CallTipBackColor { get; set; }

        /// <summary>
        /// Gets or sets the foreground color for a call tip.
        /// </summary>
        /// <value>The the foreground color for the call tip.</value>
        Color CallTipForeColor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the call tip style is bold.
        /// </summary>
        /// <value><c>true</c> if the call tip style is bold; otherwise, <c>false</c>.</value>
        bool CallTipStyleBold { get; set; }

        /// <summary>
        /// Gets the call tip style for a <see cref="Scintilla"/> control.
        /// </summary>
        /// <value>The call tip style for the <see cref="Scintilla"/> control.</value>
        Style CallTipStyle { get; }

        /// <summary>
        /// Gets or sets the highlight color for a call tip.
        /// </summary>
        /// <value>The the highlight color for a call tip.</value>
        Color CallTipHighlightColor { get; set; }

        /// <summary>
        /// Gets or sets the call tip text format (i.e. <c>'{0} of {1}'</c>).
        /// </summary>
        /// <value>The call tip text format.</value>
        string CallTipTextFormat { get; set; }

        /// <summary>
        /// Shows the call tip with specified type and text string and specified highlight style positions.
        /// </summary>
        /// <param name="type">The type text.</param>
        /// <param name="text">The body display text.</param>
        /// <param name="stylings">The collection of <see cref="ICallTipHighlightPosition{T}"/> values to be highlighted within the call tip.</param>
        /// <param name="current">The index of the current item.</param>
        /// <param name="amount">The amount of call tips to display.</param>
        void ShowCallTip(string type, string text, IList<ICallTipHighlightPosition<T>> stylings, int current, int amount);
    }
}
