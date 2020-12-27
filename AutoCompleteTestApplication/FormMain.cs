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

using System.Drawing;
using System.Windows.Forms;
using ScintillaNET;
using VPKSoft.ScintillaLexers;
using VPKSoft.ScintillaNet.AutoComplete.CSharp.Cs;

namespace AutoCompleteTestApplication
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();

            ScintillaLexers.CreateLexer(sctMain, LexerEnumerations.LexerType.Cs);
            ScintillaLexers.CreateLexer(sctTest, LexerEnumerations.LexerType.Cs);

            AutoCompleteCs = new AutoCompleteCs(sctMain);

            sctTest.Styles[Style.CallTip].BackColor = Color.FromArgb(66, 66, 66);
            sctTest.Styles[Style.CallTip].ForeColor = Color.FromArgb(255, 255, 255);
            sctTest.Styles[Style.CallTip].Bold = true;
            sctTest.CallTipSetForeHlt(Color.FromArgb(86, 156, 214));
        }

        private AutoCompleteCs AutoCompleteCs { get; }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            AutoCompleteCs.Dispose();
        }
    }
}
