using System.Windows.Forms;

namespace VPKSoft.ScintillaNet.AutoComplete.CSharp.GUI
{
    partial class FormCustomCallTipBase
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormCustomCallTipBase));
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.pnTopUp = new System.Windows.Forms.Panel();
            this.pnTopLeft = new System.Windows.Forms.Panel();
            this.pnBottomDown = new System.Windows.Forms.Panel();
            this.pnUpArrow = new System.Windows.Forms.Panel();
            this.tlpXofY = new System.Windows.Forms.TableLayoutPanel();
            this.lbXofY = new System.Windows.Forms.Label();
            this.pnDownArrow = new System.Windows.Forms.Panel();
            this.pnItemText = new System.Windows.Forms.Panel();
            this.pnItemType = new System.Windows.Forms.Panel();
            this.pnBottomRight = new System.Windows.Forms.Panel();
            this.tlpCenterTypeImage = new System.Windows.Forms.TableLayoutPanel();
            this.pnTypeImage = new System.Windows.Forms.Panel();
            this.tlpMain.SuspendLayout();
            this.tlpXofY.SuspendLayout();
            this.tlpCenterTypeImage.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.AutoSize = true;
            this.tlpMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.tlpMain.ColumnCount = 12;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 2F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 3F));
            this.tlpMain.Controls.Add(this.pnTopUp, 0, 0);
            this.tlpMain.Controls.Add(this.pnTopLeft, 0, 1);
            this.tlpMain.Controls.Add(this.pnBottomDown, 0, 2);
            this.tlpMain.Controls.Add(this.pnUpArrow, 2, 1);
            this.tlpMain.Controls.Add(this.tlpXofY, 3, 1);
            this.tlpMain.Controls.Add(this.pnDownArrow, 4, 1);
            this.tlpMain.Controls.Add(this.pnItemText, 8, 1);
            this.tlpMain.Controls.Add(this.pnItemType, 9, 1);
            this.tlpMain.Controls.Add(this.pnBottomRight, 11, 1);
            this.tlpMain.Controls.Add(this.tlpCenterTypeImage, 6, 1);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 3;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 2F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 2F));
            this.tlpMain.Size = new System.Drawing.Size(496, 25);
            this.tlpMain.TabIndex = 0;
            // 
            // pnTopUp
            // 
            this.pnTopUp.BackColor = System.Drawing.Color.Silver;
            this.tlpMain.SetColumnSpan(this.pnTopUp, 11);
            this.pnTopUp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnTopUp.Location = new System.Drawing.Point(0, 0);
            this.pnTopUp.Margin = new System.Windows.Forms.Padding(0);
            this.pnTopUp.Name = "pnTopUp";
            this.pnTopUp.Size = new System.Drawing.Size(493, 2);
            this.pnTopUp.TabIndex = 6;
            // 
            // pnTopLeft
            // 
            this.pnTopLeft.BackColor = System.Drawing.Color.Silver;
            this.pnTopLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnTopLeft.Location = new System.Drawing.Point(0, 2);
            this.pnTopLeft.Margin = new System.Windows.Forms.Padding(0);
            this.pnTopLeft.Name = "pnTopLeft";
            this.pnTopLeft.Size = new System.Drawing.Size(2, 21);
            this.pnTopLeft.TabIndex = 9;
            // 
            // pnBottomDown
            // 
            this.pnBottomDown.BackColor = System.Drawing.Color.Black;
            this.tlpMain.SetColumnSpan(this.pnBottomDown, 11);
            this.pnBottomDown.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnBottomDown.Location = new System.Drawing.Point(0, 23);
            this.pnBottomDown.Margin = new System.Windows.Forms.Padding(0);
            this.pnBottomDown.Name = "pnBottomDown";
            this.pnBottomDown.Size = new System.Drawing.Size(493, 2);
            this.pnBottomDown.TabIndex = 7;
            // 
            // pnUpArrow
            // 
            this.pnUpArrow.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnUpArrow.BackgroundImage")));
            this.pnUpArrow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pnUpArrow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnUpArrow.Location = new System.Drawing.Point(7, 2);
            this.pnUpArrow.Margin = new System.Windows.Forms.Padding(0);
            this.pnUpArrow.Name = "pnUpArrow";
            this.pnUpArrow.Size = new System.Drawing.Size(16, 21);
            this.pnUpArrow.TabIndex = 0;
            // 
            // tlpXofY
            // 
            this.tlpXofY.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpXofY.AutoSize = true;
            this.tlpXofY.ColumnCount = 1;
            this.tlpXofY.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpXofY.Controls.Add(this.lbXofY, 0, 1);
            this.tlpXofY.Location = new System.Drawing.Point(23, 2);
            this.tlpXofY.Margin = new System.Windows.Forms.Padding(0);
            this.tlpXofY.Name = "tlpXofY";
            this.tlpXofY.RowCount = 3;
            this.tlpXofY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpXofY.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpXofY.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpXofY.Size = new System.Drawing.Size(55, 21);
            this.tlpXofY.TabIndex = 5;
            // 
            // lbXofY
            // 
            this.lbXofY.AutoSize = true;
            this.lbXofY.Location = new System.Drawing.Point(3, 3);
            this.lbXofY.Name = "lbXofY";
            this.lbXofY.Size = new System.Drawing.Size(49, 14);
            this.lbXofY.TabIndex = 0;
            this.lbXofY.Text = "0 of 1";
            // 
            // pnDownArrow
            // 
            this.pnDownArrow.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pnDownArrow.BackgroundImage")));
            this.pnDownArrow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pnDownArrow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnDownArrow.Location = new System.Drawing.Point(78, 2);
            this.pnDownArrow.Margin = new System.Windows.Forms.Padding(0);
            this.pnDownArrow.Name = "pnDownArrow";
            this.pnDownArrow.Size = new System.Drawing.Size(16, 21);
            this.pnDownArrow.TabIndex = 3;
            // 
            // pnItemText
            // 
            this.pnItemText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnItemText.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pnItemText.Location = new System.Drawing.Point(120, 2);
            this.pnItemText.Margin = new System.Windows.Forms.Padding(0);
            this.pnItemText.Name = "pnItemText";
            this.pnItemText.Size = new System.Drawing.Size(285, 21);
            this.pnItemText.TabIndex = 1;
            this.pnItemText.Paint += new System.Windows.Forms.PaintEventHandler(this.pnItemText_Paint);
            // 
            // pnItemType
            // 
            this.pnItemType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpMain.SetColumnSpan(this.pnItemType, 2);
            this.pnItemType.Location = new System.Drawing.Point(405, 2);
            this.pnItemType.Margin = new System.Windows.Forms.Padding(0);
            this.pnItemType.Name = "pnItemType";
            this.pnItemType.Size = new System.Drawing.Size(88, 21);
            this.pnItemType.TabIndex = 4;
            this.pnItemType.Paint += new System.Windows.Forms.PaintEventHandler(this.pnItemType_Paint);
            // 
            // pnBottomRight
            // 
            this.pnBottomRight.BackColor = System.Drawing.Color.Black;
            this.pnBottomRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnBottomRight.Location = new System.Drawing.Point(493, 2);
            this.pnBottomRight.Margin = new System.Windows.Forms.Padding(0);
            this.pnBottomRight.Name = "pnBottomRight";
            this.tlpMain.SetRowSpan(this.pnBottomRight, 2);
            this.pnBottomRight.Size = new System.Drawing.Size(3, 23);
            this.pnBottomRight.TabIndex = 8;
            // 
            // tlpCenterTypeImage
            // 
            this.tlpCenterTypeImage.ColumnCount = 3;
            this.tlpCenterTypeImage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpCenterTypeImage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tlpCenterTypeImage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.99999F));
            this.tlpCenterTypeImage.Controls.Add(this.pnTypeImage, 1, 1);
            this.tlpCenterTypeImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpCenterTypeImage.Location = new System.Drawing.Point(99, 2);
            this.tlpCenterTypeImage.Margin = new System.Windows.Forms.Padding(0);
            this.tlpCenterTypeImage.Name = "tlpCenterTypeImage";
            this.tlpCenterTypeImage.RowCount = 3;
            this.tlpCenterTypeImage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.00001F));
            this.tlpCenterTypeImage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tlpCenterTypeImage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpCenterTypeImage.Size = new System.Drawing.Size(16, 21);
            this.tlpCenterTypeImage.TabIndex = 10;
            // 
            // pnTypeImage
            // 
            this.pnTypeImage.BackColor = System.Drawing.Color.White;
            this.pnTypeImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pnTypeImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnTypeImage.Location = new System.Drawing.Point(0, 2);
            this.pnTypeImage.Margin = new System.Windows.Forms.Padding(0);
            this.pnTypeImage.Name = "pnTypeImage";
            this.pnTypeImage.Size = new System.Drawing.Size(16, 16);
            this.pnTypeImage.TabIndex = 3;
            // 
            // FormCustomCallTipBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(66)))), ((int)(((byte)(66)))), ((int)(((byte)(66)))));
            this.ClientSize = new System.Drawing.Size(496, 25);
            this.ControlBox = false;
            this.Controls.Add(this.tlpMain);
            this.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(100000, 25);
            this.MinimizeBox = false;
            this.Name = "FormCustomCallTipBase";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "FormCustomCallTip";
            this.Deactivate += new System.EventHandler(this.FormCustomCallTipBase_Deactivate);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormCustomCallTipBase_KeyDown);
            this.Leave += new System.EventHandler(this.FormCustomCallTipBase_Deactivate);
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            this.tlpXofY.ResumeLayout(false);
            this.tlpXofY.PerformLayout();
            this.tlpCenterTypeImage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.Panel pnUpArrow;
        private System.Windows.Forms.Panel pnItemText;
        private System.Windows.Forms.Panel pnDownArrow;
        private System.Windows.Forms.Panel pnItemType;
        private System.Windows.Forms.TableLayoutPanel tlpXofY;
        private System.Windows.Forms.Label lbXofY;
        private Panel pnTopUp;
        private Panel pnBottomDown;
        private Panel pnBottomRight;
        private Panel pnTopLeft;
        private TableLayoutPanel tlpCenterTypeImage;
        private Panel pnTypeImage;
    }
}

