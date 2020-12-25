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
using VPKSoft.ScintillaNet.AutoComplete.CSharp.Interfaces;

namespace VPKSoft.ScintillaNet.AutoComplete.CSharp.CallTips
{
    /// <summary>
    /// A class to help with the <see cref="Scintilla"/> call tip style.
    /// Implements the <see cref="ICallTipStyling{T}" />
    /// </summary>
    /// <seealso cref="ICallTipStyling{T}" />
    public class CallTipStylingBase<T>: ICallTipStyling<T> where T: Enum
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CallTipStylingBase{T}"/> class.
        /// </summary>
        /// <param name="scintilla">The <see cref="Scintilla"/> control.</param>
        public CallTipStylingBase(Scintilla scintilla)
        {
            Scintilla = scintilla;
        }

        /// <summary>
        /// Gets or sets the <see cref="Scintilla"/> control for call tip styling.
        /// </summary>
        /// <value>The <see cref="Scintilla"/> control for call tip styling.</value>
        private Scintilla Scintilla { get; }

        /// <summary>
        /// Gets or sets the background color for a call tip.
        /// </summary>
        /// <value>The the background color for the call tip.</value>
        public Color CallTipBackColor 
        { 
            get => Scintilla.Styles[Style.CallTip].BackColor;
            set => Scintilla.Styles[Style.CallTip].BackColor = value;
        }

        /// <summary>
        /// Gets or sets the foreground color for a call tip.
        /// </summary>
        /// <value>The the foreground color for the call tip.</value>
        public Color CallTipForeColor
        {
            get => Scintilla.Styles[Style.CallTip].ForeColor;
            set => Scintilla.Styles[Style.CallTip].ForeColor = value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the call tip style is bold.
        /// </summary>
        /// <value><c>true</c> if the call tip style is bold; otherwise, <c>false</c>.</value>
        public bool CallTipStyleBold
        {
            get => Scintilla.Styles[Style.CallTip].Bold;
            set => Scintilla.Styles[Style.CallTip].Bold = value;
        }

        /// <summary>
        /// Gets the call tip style for a <see cref="Scintilla" /> control.
        /// </summary>
        /// <value>The call tip style for the <see cref="Scintilla" /> control.</value>
        public Style CallTipStyle => Scintilla.Styles[Style.CallTip];

        /// <summary>
        /// Gets or sets the highlight color for a call tip.
        /// </summary>
        /// <value>The the highlight color for a call tip.</value>
        public Color CallTipHighlightColor { get; set; } = Color.FromArgb(86, 156, 214);

        /// <summary>
        /// Gets or sets the call tip text format (i.e. <c>'{0} of {1}'</c>).
        /// </summary>
        /// <value>The call tip text format.</value>
        public string CallTipTextFormat { get; set; } = @"{0} of {1}";

        /// <summary>
        /// Shows the call tip with specified type and text string and specified highlight style positions.
        /// </summary>
        /// <param name="type">The type text.</param>
        /// <param name="text">The body display text.</param>
        /// <param name="stylings">The collection of <see cref="ICallTipHighlightPosition{T}" /> values to be highlighted within the call tip.</param>
        /// <param name="current">The index of the current item.</param>
        /// <param name="amount">The amount of call tips to display.</param>
        public virtual void ShowCallTip(string type, string text, IList<ICallTipHighlightPosition<T>> stylings, int current, int amount)
        {
            var callTip = FormulateCallTipArrowText(current, amount);

            int locationIncrement = callTip.Length;

            callTip += text;

            Scintilla.CallTipShow(Scintilla.CurrentPosition, callTip);
            foreach (var styling in stylings)
            {
                Scintilla.CallTipSetHlt(styling.Start + locationIncrement, styling.End + locationIncrement);
                break; // only one highlight is possible..
            }
        }

        /// <summary>
        /// Formulates the call tip arrow text with the specified current entry and the specified entry amount using the <see cref="CallTipTextFormat"/> property value.
        /// </summary>
        /// <param name="currentEntry">The current entry from 1 to <paramref name="entries"/>.</param>
        /// <param name="entries">The amount of entries to be displayed in the call tip.</param>
        /// <returns>A string for the <see cref="Scintilla"/> call tip.</returns>
        public virtual string FormulateCallTipArrowText(int currentEntry, int entries)
        {
            const char upArrow = (char) 1;
            const char downArrow = (char) 2;
            return upArrow + string.Format(CallTipTextFormat, currentEntry, entries) + downArrow;
        }
    }
}
