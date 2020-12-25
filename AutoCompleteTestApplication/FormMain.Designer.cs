namespace AutoCompleteTestApplication
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.sctMain = new ScintillaNET.Scintilla();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabLibraryTest = new System.Windows.Forms.TabPage();
            this.tabTestOther = new System.Windows.Forms.TabPage();
            this.sctTest = new ScintillaNET.Scintilla();
            this.tabControl1.SuspendLayout();
            this.tabLibraryTest.SuspendLayout();
            this.tabTestOther.SuspendLayout();
            this.SuspendLayout();
            // 
            // sctMain
            // 
            this.sctMain.AutoCMaxHeight = 9;
            this.sctMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sctMain.Location = new System.Drawing.Point(3, 3);
            this.sctMain.Name = "sctMain";
            this.sctMain.Size = new System.Drawing.Size(762, 394);
            this.sctMain.TabIndents = true;
            this.sctMain.TabIndex = 0;
            this.sctMain.Text = resources.GetString("sctMain.Text");
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabLibraryTest);
            this.tabControl1.Controls.Add(this.tabTestOther);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(776, 426);
            this.tabControl1.TabIndex = 1;
            // 
            // tabLibraryTest
            // 
            this.tabLibraryTest.Controls.Add(this.sctMain);
            this.tabLibraryTest.Location = new System.Drawing.Point(4, 22);
            this.tabLibraryTest.Name = "tabLibraryTest";
            this.tabLibraryTest.Padding = new System.Windows.Forms.Padding(3);
            this.tabLibraryTest.Size = new System.Drawing.Size(768, 400);
            this.tabLibraryTest.TabIndex = 0;
            this.tabLibraryTest.Text = "Test the VPKSoft.ScintillaNETAutoComplete class library";
            this.tabLibraryTest.UseVisualStyleBackColor = true;
            // 
            // tabTestOther
            // 
            this.tabTestOther.Controls.Add(this.sctTest);
            this.tabTestOther.Location = new System.Drawing.Point(4, 22);
            this.tabTestOther.Name = "tabTestOther";
            this.tabTestOther.Padding = new System.Windows.Forms.Padding(3);
            this.tabTestOther.Size = new System.Drawing.Size(768, 400);
            this.tabTestOther.TabIndex = 1;
            this.tabTestOther.Text = "Other tests";
            this.tabTestOther.UseVisualStyleBackColor = true;
            // 
            // sctTest
            // 
            this.sctTest.AutoCMaxHeight = 9;
            this.sctTest.Dock = System.Windows.Forms.DockStyle.Left;
            this.sctTest.Location = new System.Drawing.Point(3, 3);
            this.sctTest.Name = "sctTest";
            this.sctTest.Size = new System.Drawing.Size(616, 394);
            this.sctTest.TabIndents = true;
            this.sctTest.TabIndex = 0;
            this.sctTest.Text = resources.GetString("sctTest.Text");
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.Text = "Auto-complete test application (ScintillaNET)";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
            this.tabControl1.ResumeLayout(false);
            this.tabLibraryTest.ResumeLayout(false);
            this.tabTestOther.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ScintillaNET.Scintilla sctMain;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabLibraryTest;
        private System.Windows.Forms.TabPage tabTestOther;
        private ScintillaNET.Scintilla sctTest;
    }
}

