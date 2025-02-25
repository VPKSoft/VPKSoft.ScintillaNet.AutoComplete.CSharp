﻿#region License
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
using System.Drawing;
using System.Windows.Forms;
using ScintillaNET;
using VPKSoft.ScintillaNet.AutoComplete.CSharp.Interfaces;

namespace VPKSoft.ScintillaNet.AutoComplete.CSharp.GUI
{
    /// <summary>
    /// A form for displaying custom call tips with the <see cref="Scintilla"/> control.
    /// Implements the <see cref="System.Windows.Forms.Form" />
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class FormCustomCallTipBase: Form, IAutoCompleteColors
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormCustomCallTip{T}"/> class.
        /// </summary>
        public FormCustomCallTipBase()
        {
            InitializeComponent();
            pnItemText.Paint += PaintBodyPanelText;
            base.DoubleBuffered = true;
        }

        #region PublicEvents
        /// <summary>
        /// An event which occurs when the panel containing the body of a language construct should be painted.
        /// </summary>
        internal event PaintEventHandler PaintBodyPanelText;

        /// <summary>
        /// An event which occurs when the panel containing the type of a language construct should be painted.
        /// </summary>
        internal event PaintEventHandler PaintTypePanelText;

        /// <summary>
        /// An event that occurs when the selects the previous entry to be displayed from the call tip.
        /// </summary>
        internal event EventHandler PreviousEntry;

        /// <summary>
        /// An event that occurs when the selects the next entry to be displayed from the call tip.
        /// </summary>
        internal event EventHandler NextEntry;

        /// <summary>
        /// An event that occurs when the user action indicates a wish to close the call tip.
        /// </summary>
        internal event EventHandler DoClose;

        /// <summary>
        /// An event that occurs when the search filter has changed.
        /// </summary>
        internal event EventHandler FilterChanged;

        /// <summary>
        /// An event which occurs when the user selects an item in the call tip.
        /// </summary>
        internal event EventHandler SelectionMade;
        #endregion

        #region PublicProperties        
        /// <summary>
        /// Gets or sets the comparison filter call tips.
        /// </summary>
        /// <value>The comparison filter call tips.</value>
        public StringComparison ComparisonFilterCallTips { get; set; } = StringComparison.OrdinalIgnoreCase;

        /// <summary>
        /// Gets or sets a value indicating whether to filter the call tips also in the middle of the text.
        /// </summary>
        /// <value><c>true</c> if to filter the call tips also in the middle of the text; otherwise, <c>false</c>.</value>
        public bool FilterCallTipsMiddle { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether the call tip sets it width automatically to
        /// fit the description of the currently displayed language construct.
        /// </summary>
        /// <value><c>true</c> if the automatic width is in use; otherwise, <c>false</c>.</value>
        public static bool AutoWidth { get; set; } = true;

        /// <summary>
        /// Gets or sets the space between the body text and the type text.
        /// </summary>
        /// <value>The space between the body text and the type text..</value>
        public int PanelTypeTextSpan 
        { 
            get => panelTypeTextSpan;

            set
            {
                if (value != panelTypeTextSpan)
                {
                    panelTypeTextSpan = value;
                    InvalidatePanels();
                }
            }
        }

        /// <summary>
        /// Gets or sets the call tip text format (i.e. <c>'{0} of {1}'</c>).
        /// </summary>
        /// <value>The call tip text format.</value>
        public static string CallTipTextFormat { get; set; } = @"{0} of {1}";

        /// <summary>
        /// Gets or sets the up arrow image.
        /// </summary>
        /// <value>The up arrow image.</value>
        public Image UpArrowImage
        {
            get => pnUpArrow.BackgroundImage;
            set => pnUpArrow.BackgroundImage = value;
        }

        /// <summary>
        /// Gets or sets down arrow image.
        /// </summary>
        /// <value>Down arrow image.</value>
        public Image DownArrowImage
        {
            get => pnDownArrow.BackgroundImage;
            set => pnDownArrow.BackgroundImage = value;
        }

        /// <summary>
        /// Gets or sets the language construct type image.
        /// </summary>
        /// <value>The language construct type image.</value>
        public Image LanguageConstructTypeImage
        {
            get => pnTypeImage.BackgroundImage;
            set => pnTypeImage.BackgroundImage = value;
        }

        /// <summary>
        /// Gets or sets the default highlight color.
        /// </summary>
        /// <value>The default highlight color.</value>
        public Color DefaultHighlightColor
        {
            get => defaultHighlightColor;
            set
            {
                if (value != defaultHighlightColor)
                {
                    defaultHighlightColor = value;
                    Invalidate();
                }
            }
        }
        #endregion

        #region InternalProperties
        /// <summary>
        /// Gets or sets the user typed string while the call tip was visible.
        /// </summary>
        /// <value>The user typed string while the call tip was visible.</value>
        internal string UserTypedString { get; set; }

        /// <summary>
        /// Gets or sets the width of the panel containing the body text of the language construct.
        /// </summary>
        /// <value>The width of the panel containing the body text of the language construct.</value>
        internal int PanelBodyTextWidth
        {
            get => pnItemText.Width;

            set
            {
                if (value != pnItemText.Width)
                {
                    pnItemText.Width = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the width of the panel containing the member type text.
        /// </summary>
        /// <value>The width of the panel containing the member type text.</value>
        internal int PanelTypeTextWidth
        {
            get => pnItemType.Width;

            set
            {
                if (value != pnItemType.Width)
                {
                    pnItemType.Width = value;
                    pnItemType.Invalidate();
                }
            }
        }

        /// <summary>
        /// Gets or sets the index of the current item.
        /// </summary>
        /// <value>The index of the current item.</value>
        internal int CurrentItemIndex
        {
            get => currentItemIndex;

            set
            {
                if (value != currentItemIndex)
                {
                    currentItemIndex = value;
                    lbXofY.Text = string.Format(CallTipTextFormat, currentItemIndex + 1, itemCount);
                    InvalidatePanels();
                }
            }
        }

        /// <summary>
        /// Gets or sets the item count.
        /// </summary>
        /// <value>The item count.</value>
        internal int ItemCount
        {
            get => itemCount;

            set
            {
                if (value != itemCount)
                {
                    itemCount = value;
                    lbXofY.Text = string.Format(CallTipTextFormat, currentItemIndex + 1, itemCount);
                    Invalidate();
                }
            }
        }
        #endregion

        #region InternalMethods        
        /// <summary>
        /// Invalidates the entire surface of the multi-colored panel controls and causes them to be redrawn.
        /// </summary>
        internal void InvalidatePanels()
        {
            pnItemText.Invalidate();
            pnItemType.Invalidate();
        }
        #endregion

        #region PublicMethods
        /// <summary>
        /// Displays the call tip at the current cursor position of the specified <see cref="Scintilla"/> control.
        /// </summary>
        /// <param name="scintilla">The scintilla on which to display the call tip.</param>
        public void CallTipShow(Scintilla scintilla)
        {
            UserTypedString = string.Empty;

            if (ItemCount == 0)
            {
                return;
            }

            Height = 16;
            var x = scintilla.PointXFromPosition(scintilla.CurrentPosition);
            var y = scintilla.PointYFromPosition(scintilla.CurrentPosition);
            var location = new Point(x, y);
            Location = scintilla.PointToScreen(location);

            Show();
        }
        #endregion

        #region PrivateFields
        private Color defaultHighlightColor = Color.FromArgb(86, 156, 214);
        private int itemCount;
        private int currentItemIndex;
        private int panelTypeTextSpan = 40;
        #endregion

        #region PrivateMethods
        #endregion

        #region PrivateProperties        
        /// <summary>
        /// Gets or sets a value indicating whether to handle a key event as character input.
        /// </summary>
        /// <value><c>true</c> if to handle a key event as character input; otherwise, <c>false</c>.</value>
        private bool HandleKeyAsChar { get; set; }        
        #endregion

        #region InternalEvents
        private void pnItemText_Paint(object sender, PaintEventArgs e)
        {
            PaintBodyPanelText?.Invoke(sender, e);
        }

        private void pnItemType_Paint(object sender, PaintEventArgs e)
        {
            PaintTypePanelText?.Invoke(sender, e);
        }

        private void FormCustomCallTipBase_Deactivate(object sender, EventArgs e)
        {
            Hide();
        }

        private void FormCustomCallTipBase_KeyDown(object sender, KeyEventArgs e)
        {
            HandleKeyAsChar = false;
            
            var noModifiers = !e.Alt && !e.Control && !e.Shift;
            if (e.KeyCode == Keys.Up && noModifiers)
            {
                e.SuppressKeyPress = true;
                PreviousEntry?.Invoke(this, new EventArgs());
                return;
            }

            if (e.KeyCode == Keys.Return && noModifiers)
            {
                e.SuppressKeyPress = true;
                Hide();
                SelectionMade?.Invoke(this, new EventArgs());
                UserTypedString = string.Empty;
                return;
            }

            if (e.KeyCode == Keys.Down && noModifiers)
            {
                e.SuppressKeyPress = true;
                NextEntry?.Invoke(this, new EventArgs());
                return;
            }

            if (e.KeyCode == Keys.Escape && noModifiers)
            {
                e.SuppressKeyPress = true;
                DoClose?.Invoke(this, new EventArgs());
                return;
            }

            if (e.KeyCode == Keys.Back && !string.IsNullOrEmpty(UserTypedString) && noModifiers)
            {
                e.SuppressKeyPress = true;
                UserTypedString = UserTypedString.Substring(0, UserTypedString.Length - 1);
                FilterChanged?.Invoke(this, new EventArgs());
                return;
            }

            if (e.KeyCode == Keys.Back && string.IsNullOrEmpty(UserTypedString) && noModifiers)
            {
                e.SuppressKeyPress = true;
                DoClose?.Invoke(this, new EventArgs());
                return;
            }

            HandleKeyAsChar = true;
        }

        private void FormCustomCallTipBase_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (HandleKeyAsChar)
            {
                UserTypedString += e.KeyChar.ToString();
                FilterChanged?.Invoke(this, new EventArgs());
                HandleKeyAsChar = false;
            }
        }
        #endregion

        #region IFaceColors
        /// <summary>
        /// Gets or sets the call tip color.
        /// </summary>
        /// <value>The call tip color.</value>
        public Color ColorCallTip
        {
            get => tlpMain.BackColor;
            set
            {
                tlpMain.BackColor = value;
                pnItemText.BackColor = value;
                pnItemType.BackColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the background color of the up and down arrows.
        /// </summary>
        /// <value>The background color of the up and down arrows.</value>
        public Color ColorBackgroundUpDownArrow
        {
            get => pnDownArrow.BackColor;
            set
            {
                pnDownArrow.BackColor = value;
                pnUpArrow.BackColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the background color for the type image.
        /// </summary>
        /// <value>The background color for the type image.</value>
        public Color ColorBackgroundTypeImage { get => pnTypeImage.BackColor; set => pnTypeImage.BackColor = value; }

        /// <summary>
        /// Gets or sets the background color of the "X of Y" text in the call tip.
        /// </summary>
        /// <value>The background color of the "X of Y" text in the call tip.</value>
        public Color ColorBackgroundNumOfNum { get => lbXofY.BackColor; set => lbXofY.BackColor = value; }

        /// <summary>
        /// Gets or sets the foreground color of the "X of Y" text in the call tip.
        /// </summary>
        /// <value>The foreground color of the "X of Y" text in the call tip.</value>
        public Color ColorForegroundNumOfNum { get => lbXofY.ForeColor; set => lbXofY.ForeColor = value; }

        /// <summary>
        /// Gets or sets the up left border color of the call tip.
        /// </summary>
        /// <value>The up left border color of the call tip.</value>
        public Color ColorUpLeftBorder
        {
            get => pnTopLeft.BackColor;
            set
            {
                pnTopLeft.BackColor = value;
                pnTopUp.BackColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the bottom right border color of the call tip.
        /// </summary>
        /// <value>The bottom right border color of the call tip.</value>
        public Color ColorBottomRightBorder
        {
            get => pnBottomDown.BackColor;
            set
            {
                pnBottomDown.BackColor = value;
                pnBottomRight.BackColor = value;
            }
        }
        #endregion
    }
}
