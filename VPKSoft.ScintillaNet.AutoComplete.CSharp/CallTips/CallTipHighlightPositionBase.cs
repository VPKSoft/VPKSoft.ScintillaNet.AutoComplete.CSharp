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
using VPKSoft.ScintillaNet.AutoComplete.CSharp.Interfaces;

namespace VPKSoft.ScintillaNet.AutoComplete.CSharp.CallTips
{
    /// <summary>
    /// A base class for the <see cref="ICallTipHighlightPosition{T}"/> interface.
    /// Implements the <see cref="ICallTipHighlightPosition{T}" />
    /// </summary>
    /// <typeparam name="T">An enumeration for the type of the highlight.</typeparam>
    /// <seealso cref="ICallTipHighlightPosition{T}" />
    public class CallTipHighlightPositionBase<T>: ICallTipHighlightPosition<T> where T: Enum
    {
        /// <summary>
        /// Gets or sets the starting highlight position.
        /// </summary>
        /// <value>The starting highlight position.</value>
        public int Start { get; set; }

        /// <summary>
        /// Gets or sets the length of the highlight position.
        /// </summary>
        /// <value>The length of the highlight position.</value>
        public int Length { get; set; }

        /// <summary>
        /// Gets the end of the highlight position.
        /// </summary>
        /// <value>The end of the highlight position.</value>
        public int End => Start + Length;

        /// <summary>
        /// Gets or sets the type of the highlight.
        /// </summary>
        /// <value>The type of the highlight.</value>
        public T HighlightType { get; set; }
    }
}