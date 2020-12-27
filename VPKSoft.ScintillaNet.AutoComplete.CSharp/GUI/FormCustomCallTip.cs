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
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ScintillaNET;

namespace VPKSoft.ScintillaNet.AutoComplete.CSharp.GUI
{
    /// <summary>
    /// A form for displaying custom call tips with the <see cref="Scintilla"/> control.
    /// Implements the <see cref="System.Windows.Forms.Form" />
    /// </summary>
    /// <typeparam name="T">The enumeration type for type highlight style indexer for this class.</typeparam>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class FormCustomCallTip<T> : FormCustomCallTipBase where T: Enum
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormCustomCallTip{T}"/> class.
        /// </summary>
        public FormCustomCallTip()
        {
            InitializeComponent();
            FontChanged += formCustomCallTip_FontChanged;
            Disposed += formCustomCallTip_Disposed;
            Shown += formCustomCallTip_Shown;
            StartPosition = FormStartPosition.Manual;
            PaintBodyPanelText += PaintBodyPanelText_Event;
            PaintTypePanelText += PaintTypePanelText_Event;
            PreviousEntry += PreviousEntry_Event;
            NextEntry += NextEntry_Event;
            FilterChanged += formCustomCallTip_FilterChanged;
            SelectionMade += formCustomCallTip_SelectionMade;
            DoClose += DoClose_Event;
        }

        #region PublicEvents        
        /// <summary>
        /// An event that occurs when the user selected an item from the call tip.
        /// </summary>
        public event EventHandler<CallTipSelectedItemEventArgs<T>> ItemSelected;
        #endregion

        #region PublicProperties        
        /// <summary>
        /// Gets or sets the item selected by the user.
        /// </summary>
        /// <value>The item selected by the user.</value>
        public CallTipEntry<T> SelectedItem { get; set; }

        /// <summary>
        /// Gets or sets the index of the current call tip.
        /// </summary>
        /// <value>The index of the current call tip.</value>
        /// <exception cref="IndexOutOfRangeException">CurrentCallTipIndex</exception>
        public int CurrentCallTipIndex
        {
            get => currentCallTipIndex;

            set
            {
                if (value != 0 && (value < 0 || value >= CallTipEntriesFiltered.Count))
                {
                    throw new IndexOutOfRangeException(nameof(CurrentCallTipIndex));
                }

                if (currentCallTipIndex != value)
                {
                    currentCallTipIndex = value;
                    CurrentItemIndex = value;
                    SetImage();
                }
            }
        }

        /// <summary>
        /// Gets the current call tip.
        /// </summary>
        /// <value>The current call tip.</value>
        public CallTipEntry<T> CurrentCallTip =>
            (CallTipEntriesFiltered.Count == 0 || currentCallTipIndex < 0 || currentCallTipIndex >= CallTipEntriesFiltered.Count)
                ? null
                : CallTipEntriesFiltered[currentCallTipIndex];
        #endregion

        #region PublicMethods
        /// <summary>
        /// Adds the <see cref="CallTipEntry{T}"/> instance to the collection.
        /// </summary>
        /// <param name="entry">The entry to add to the collection.</param>
        public void AddEntry(CallTipEntry<T> entry)
        {
            CallTipEntries.Add(entry);
            ItemCount = CallTipEntriesFiltered.Count;
        }

        /// <summary>
        /// Clears the <see cref="CallTipEntry{T}"/> instance collection.
        /// </summary>
        public void Clear()
        {
            CallTipEntries.Clear();
            UserTypedString = string.Empty;
            currentCallTipIndex = 0;
            ItemCount = 0;
        }

        /// <summary>
        /// Moves the current call tip to the first call tip in the <see cref="CallTipEntry{T}"/> instance collection.
        /// </summary>
        public void First()
        {
            SetImage();
            CurrentCallTipIndex = 0;
            CurrentItemIndex = 0;
        }

        /// <summary>
        /// Sets the next call tip to be shown.
        /// </summary>
        public void Next()
        {
            var current = currentCallTipIndex;

            current++;
            if (current >= CallTipEntriesFiltered.Count)
            {
                current = 0;
            }

            if (current != currentCallTipIndex)
            {
                SetImage();
                CurrentCallTipIndex = current;
                CurrentItemIndex = current;
            }
        }

        /// <summary>
        /// Sets the previous call tip to be shown.
        /// </summary>
        public void Previous()
        {
            var current = currentCallTipIndex;

            current--;
            if (current < 0)
            {
                current = CallTipEntriesFiltered.Count > 0 ? CallTipEntriesFiltered.Count - 1 : 0;
            }

            if (current != currentCallTipIndex)
            {
                SetImage();
                CurrentCallTipIndex = current;
                CurrentItemIndex = current;
            }
        }
        #endregion

        #region PrivateFields
        [EditorBrowsable(EditorBrowsableState.Never)]
        private int currentCallTipIndex;
        #endregion

        #region PrivateMethods
        /// <summary>
        /// Sets the image for the current call tip.
        /// </summary>
        public void SetImage()
        {
            var image = CurrentCallTip?[CurrentCallTip.LanguageConstructType];
            LanguageConstructTypeImage = image;
        }
        #endregion

        #region PrivateProperties
        /// <summary>
        /// Gets the list of call tip entries.
        /// </summary>
        /// <value>The list of call tip entries.</value>
        private List<CallTipEntry<T>> CallTipEntries { get; } = new List<CallTipEntry<T>>();

        /// <summary>
        /// Gets the call tip entries filtered by the user input.
        /// </summary>
        /// <value>The call tip entries filtered by the user input.</value>
        private List<CallTipEntry<T>> CallTipEntriesFiltered =>
            (CallTipEntries?.Where(f =>
                f.CallTipBodyText.IndexOf(UserTypedString ?? string.Empty, ComparisonFilterCallTips) != (FilterCallTipsMiddle ? -1 : 0) ||
                string.IsNullOrWhiteSpace(UserTypedString)) ?? new List<CallTipEntry<T>>()).ToList();
        #endregion

        #region InternalEvents
        private void formCustomCallTip_Disposed(object sender, EventArgs e)
        {
            FontChanged -= formCustomCallTip_FontChanged;
            Shown -= formCustomCallTip_Shown;
            PaintBodyPanelText -= PaintBodyPanelText_Event;
            PaintTypePanelText -= PaintTypePanelText_Event;
            PreviousEntry -= PreviousEntry_Event;
            NextEntry -= NextEntry_Event;
            FilterChanged -= formCustomCallTip_FilterChanged;
            SelectionMade -= formCustomCallTip_SelectionMade;
            DoClose -= DoClose_Event;            
            Disposed -= formCustomCallTip_Disposed;
        }

        private void formCustomCallTip_FontChanged(object sender, EventArgs e)
        {
            InvalidatePanels();
        }

        private void formCustomCallTip_Shown(object sender, EventArgs e)
        {
            SelectedItem = default;
            currentCallTipIndex = 0;
            Height = 16;
            SetImage();
        }

        private void PaintBodyPanelText_Event(object sender, PaintEventArgs e)
        {
            var panel = (Panel) sender;

            using var backgroundBrush = new SolidBrush(panel.BackColor);
            e.Graphics.FillRectangle(backgroundBrush, e.ClipRectangle);

            if (CurrentCallTip != null)
            {
                var left = 4f;

                var panelWidth = 0f;

                foreach (var highlightPosition in CurrentCallTip?.HighlightPositions)
                {
                    var style = CurrentCallTip?[highlightPosition.HighlightType] ?? new StyleContainer<T>
                        {BackColor = BackColor, ForeColor = DefaultHighlightColor, Font = Font, Type = default};

                    var textPart =
                        CurrentCallTip.CallTipBodyText.Substring(highlightPosition.Start, highlightPosition.Length);

                    var measure = e.Graphics.MeasureString(textPart, style.Font);
                    var y = (e.ClipRectangle.Height - measure.Height) / 2f;

                    using var brush = new SolidBrush(style.ForeColor);
                    e.Graphics.DrawString(textPart, style.Font, brush, new PointF(left, y));

                    left += measure.Width;

                    panelWidth = left;
                }

                if (AutoWidth)
                {
                    PanelBodyTextWidth = (int)(panelWidth + 0.5f);
                }
            }
        }

        private void PaintTypePanelText_Event(object sender, PaintEventArgs e)
        {
            var panel = (Panel) sender;

            using var backgroundBrush = new SolidBrush(panel.BackColor);
            e.Graphics.FillRectangle(backgroundBrush, e.ClipRectangle);

            var panelWidth = (float)PanelTypeTextSpan;

            if (CurrentCallTip != null)
            {
                var style = CurrentCallTip?[default(T)] ?? new StyleContainer<T>
                    {BackColor = BackColor, ForeColor = DefaultHighlightColor, Font = Font, Type = default};

                var measure = e.Graphics.MeasureString(CurrentCallTip.CallTipTypeText, style.Font);
                var y = (e.ClipRectangle.Height - measure.Height) / 2f;
                var x = e.ClipRectangle.Right - measure.Width - 4f;

                panelWidth += measure.Width + 4f;


                using var brush = new SolidBrush(style.ForeColor);
                e.Graphics.DrawString(CurrentCallTip.CallTipTypeText, style.Font, brush, new PointF(x, y));
            }

            if (AutoWidth)
            {
                PanelTypeTextWidth = (int)(panelWidth + 0.5f);
            }
        }

        private void DoClose_Event(object sender, EventArgs e)
        {
            Hide();
        }

        private void NextEntry_Event(object sender, EventArgs e)
        {
            Next();
        }

        private void PreviousEntry_Event(object sender, EventArgs e)
        {
            Previous();
        }

        private void formCustomCallTip_FilterChanged(object sender, EventArgs e)
        {
            ItemCount = CallTipEntriesFiltered.Count;

            if (ItemCount == 0)
            {
                Hide();
                return;
            }

            First();
            InvalidatePanels();
        }

        private void formCustomCallTip_SelectionMade(object sender, EventArgs e)
        {
            SelectedItem = CallTipEntriesFiltered[CurrentCallTipIndex];
            ItemSelected?.Invoke(this, new CallTipSelectedItemEventArgs<T> {SelectedItem = SelectedItem});
        }
        #endregion
    }
}
