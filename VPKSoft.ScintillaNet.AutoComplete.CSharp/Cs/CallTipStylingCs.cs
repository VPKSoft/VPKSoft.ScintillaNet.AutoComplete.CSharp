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

using System.Collections.Generic;
using System.Drawing;
using ScintillaNET;
using VPKSoft.ScintillaNet.AutoComplete.CSharp.CallTips;
using VPKSoft.ScintillaNet.AutoComplete.CSharp.Interfaces;

namespace VPKSoft.ScintillaNet.AutoComplete.CSharp.Cs
{
    /// <summary>
    /// A class for formatting <see cref="Scintilla"/> call tips for the C# programming language.
    /// Implements the <see cref="HighLightStyleCs" />
    /// </summary>
    /// <seealso cref="HighLightStyleCs" />
    public class CallTipStylingCs : CallTipStylingBase<HighLightStyleCs>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CallTipStylingCs"/> class.
        /// </summary>
        /// <param name="scintilla">The <see cref="Scintilla" /> control.</param>
        public CallTipStylingCs(Scintilla scintilla) : base(scintilla)
        {
            CallTipBackColor = Color.FromArgb(66, 66, 66);
            CallTipForeColor = Color.FromArgb(255, 255, 255);
            CallTipStyleBold = true;
        }

        /// <summary>
        /// Shows the call tip with specified type and text string and specified highlight style positions.
        /// </summary>
        /// <param name="type">The type text.</param>
        /// <param name="text">The body display text.</param>
        /// <param name="stylings">The collection of <see cref="IList{T}" /> values to be highlighted within the call tip.</param>
        /// <param name="current">The index of the current item.</param>
        /// <param name="amount">The amount of call tips to display.</param>
        public void ShowCallTip(string type, string text, IList<HighLightPositionCs> stylings, int current, int amount)
        {
            List<ICallTipHighlightPosition<HighLightStyleCs>> list = new List<ICallTipHighlightPosition<HighLightStyleCs>>();

            foreach (var styling in stylings)
            {
                list.Add(styling);
            }

            base.ShowCallTip(type, text, list, current, amount);
        }
    }
}
