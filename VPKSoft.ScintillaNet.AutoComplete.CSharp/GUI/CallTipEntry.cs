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
using System.Linq;
using VPKSoft.ScintillaNet.AutoComplete.CSharp.CallTips;
using VPKSoft.ScintillaNet.AutoComplete.CSharp.Enumerations;
using VPKSoft.ScintillaNet.AutoComplete.CSharp.Interfaces;

namespace VPKSoft.ScintillaNet.AutoComplete.CSharp.GUI
{
    /// <summary>
    /// A call tip style and data holder class for the <see cref="FormCustomCallTip{T}"/> form.
    /// </summary>
    /// <typeparam name="T">The enumeration type for type highlight style indexer for this class.</typeparam>
    public class CallTipEntry<T> where T: Enum
    {
        /// <summary>
        /// Gets or sets the call tip body text.
        /// </summary>
        /// <value>The call tip body text.</value>
        public string CallTipBodyText { get; set; }

        /// <summary>
        /// Gets or sets the call tip body text without the parameter specifications.
        /// </summary>
        /// <value>The call call tip body text without the parameter specifications.</value>
        public string CallTipBodyTextNoParameters { get; set; }

        /// <summary>
        /// Gets or sets the call tip type text.
        /// </summary>
        /// <value>The call tip type text.</value>
        public string CallTipTypeText { get; set; }

        /// <summary>
        /// Gets or sets the type of the call tip style.
        /// </summary>
        /// <value>The type of the call tip style.</value>
        public T Style { get; set; }

        /// <summary>
        /// Gets or sets the type of the language construct.
        /// </summary>
        /// <value>The type of the language construct.</value>
        public LanguageConstructType LanguageConstructType { get; set; }

        /// <summary>
        /// Gets the highlight positions.
        /// </summary>
        /// <value>The highlight positions.</value>
        private List<CallTipHighlightPositionBase<T>> HighlightPositionsReadWrite { get; set; } =
            new List<CallTipHighlightPositionBase<T>>();

        /// <summary>
        /// Gets the highlight positions.
        /// </summary>
        /// <value>The highlight positions.</value>
        public IReadOnlyList<ICallTipHighlightPosition<T>> HighlightPositions => HighlightPositionsReadWrite;

        /// <summary>
        /// Gets or sets default style to fill the gaps without style.
        /// </summary>
        /// <value>The default style to fill the gaps without style.</value>
        public static T StyleGap { get; set; } = default;

        /// <summary>
        /// Gets the <see cref="StyleContainer{T}"/> for the specified highlight type.
        /// </summary>
        /// <param name="type">The highlight type.</param>
        /// <returns>An instance to a <see cref="StyleContainer{T}"/> if on was found; otherwise null.</returns>
        public StyleContainer<T> this[T type] => Styles.FirstOrDefault(f => f.Type.Equals(type));

        /// <summary>
        /// Adds a new highlight position to the collection.
        /// </summary>
        /// <param name="position">The new position to add to the collection.</param>
        public void AddHighlightPosition(CallTipHighlightPositionBase<T> position)
        {
            HighlightPositionsReadWrite.Add(position);
            FillGaps();
        }

        /// <summary>
        /// Clears the highlight positions from the collection.
        /// </summary>
        public void ClearHighlightPositions()
        {
            HighlightPositionsReadWrite.Clear();
            FillGaps();
        }

        /// <summary>
        /// Fills the style gaps.
        /// </summary>
        private void FillGaps()
        {
            CallTipHighlightPositionBase<T> EmptyGapFill(int start, int end)
            {
                var gapFill = (CallTipHighlightPositionBase<T>)Activator.CreateInstance(typeof(CallTipHighlightPositionBase<T>));
                if (gapFill != null)
                {
                    gapFill.Start = start;
                    gapFill.Length = end - start;
                    gapFill.HighlightType = StyleGap;
                }
                else
                {
                    throw new NotSupportedException(nameof(FillGaps));
                }

                return gapFill;
            }

            HighlightPositionsReadWrite.RemoveAll(f => f.HighlightType.Equals(StyleGap));

            if (string.IsNullOrEmpty(CallTipBodyText))
            {
                return;
            }

            HighlightPositionsReadWrite = HighlightPositionsReadWrite
                .OrderBy(f => f.Start)
                .ThenByDescending(f => f.Length).ToList();
            if (HighlightPositionsReadWrite.Count == 0)
            {
                HighlightPositionsReadWrite.Add(EmptyGapFill(0, CallTipBodyText.Length));
                return;
            }

            for (var i = 0; i < CallTipBodyText.Length; i++)
            {
                if (HighlightPositionsReadWrite.Exists(f => i >= f.Start && i < f.End))
                {
                    continue;
                }

                var hasElements = HighlightPositionsReadWrite.Any(f => i < f.Start);

                var end = hasElements ? HighlightPositionsReadWrite.Where(f => i < f.Start).Min(f => f.Start) : CallTipBodyText.Length;
                var addGap = EmptyGapFill(i, end);
                HighlightPositionsReadWrite.Add(addGap);
                i += addGap.Length;
            }

            HighlightPositionsReadWrite = HighlightPositionsReadWrite
                .OrderBy(f => f.Start)
                .ThenByDescending(f => f.Length).ToList();
        }

        /// <summary>
        /// Adds a style for a specified type.
        /// </summary>
        /// <param name="type">The type of the style.</param>
        /// <param name="foreColor">The foreground color for the style.</param>
        /// <param name="backColor">The background color for the style.</param>
        /// <param name="font">The font for the style.</param>
        public static void AddStyle(T type, Color foreColor, Color backColor, Font font)
        {
            Styles.RemoveWhere(f => f.Type.Equals(type));
            Styles.Add(new StyleContainer<T> {Type = type, ForeColor = foreColor, BackColor = backColor, Font = font});
        }

        /// <summary>
        /// Adds a specified style to the collection.
        /// </summary>
        /// <param name="style">The style to add.</param>
        public static void AddStyle(StyleContainer<T> style)
        {
            Styles.RemoveWhere(f => f.Type.Equals(style.Type));
            Styles.Add(style);
        }

        /// <summary>
        /// Adds a specified style to the collection.
        /// </summary>
        /// <param name="type">The type of the style.</param>
        /// <param name="style">The style to add.</param>
        public static void AddStyle(T type, StyleContainer<T> style)
        {
            style.Type = type;
            AddStyle(style);
        }

        /// <summary>
        /// Gets a style for a specified type.
        /// </summary>
        /// <param name="type">The type of the style.</param>
        /// <returns>An instance to a <see cref="StyleContainer{T}"/> class.</returns>
        public static StyleContainer<T> GetStyle(T type)
        {
            return Styles.FirstOrDefault(f => f.Type.Equals(type));
        }

        /// <summary>
        /// Adds a type image for a specified <see cref="LanguageConstructType"/> type.
        /// </summary>
        /// <param name="constructType">Type of the language construct.</param>
        /// <param name="image">The image for the specified <see cref="LanguageConstructType"/> type.</param>
        public static void AddTypeImage(LanguageConstructType constructType, Image image)
        {
            TypeImages.RemoveWhere(f => f.Key == constructType);
            TypeImages.Add(new KeyValuePair<LanguageConstructType, Image>(constructType, image));
        }

        /// <summary>
        /// Gets a type image for a specified <see cref="LanguageConstructType"/> type.
        /// </summary>
        /// <param name="constructType">Type of the language construct.</param>
        /// <returns>An instance to a <see cref="Image"/> class.</returns>
        public static Image GetTypeImage(LanguageConstructType constructType)
        {
            return TypeImages.FirstOrDefault(f => f.Key == constructType).Value;
        }

        /// <summary>
        /// Gets the styles.
        /// </summary>
        /// <value>The styles.</value>
        private static HashSet<StyleContainer<T>> Styles { get; } = new HashSet<StyleContainer<T>>();

        /// <summary>
        /// Gets the <see cref="Image"/> for the specified language construct type.
        /// </summary>
        /// <param name="constructType">Type of the language construct.</param>
        /// <returns>An image associated for the specified language construct type if found; otherwise null.</returns>
        public Image this[LanguageConstructType constructType] => TypeImages.FirstOrDefault(f => f.Key.Equals(constructType)).Value;

        // ReSharper disable once StaticMemberInGenericType, Intentional..
        private static HashSet<KeyValuePair<LanguageConstructType, Image>> TypeImages { get; } =
            new HashSet<KeyValuePair<LanguageConstructType, Image>>();
    }
}
