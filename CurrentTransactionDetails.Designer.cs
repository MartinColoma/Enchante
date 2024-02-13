namespace Enchante
{
    partial class currenttransactiondetails
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TransactionNumberLbl = new System.Windows.Forms.Label();
            this.CurrentNameTxtBox = new System.Windows.Forms.TextBox();
            this.CurrentStatusLbl = new System.Windows.Forms.Label();
            this.CurrentServiceTypeLbl = new System.Windows.Forms.Label();
            this.CurrentStartBtn = new FontAwesome.Sharp.IconButton();
            this.CurrentCompeleteBtn = new FontAwesome.Sharp.IconButton();
            this.SuspendLayout();
            // 
            // TransactionNumberLbl
            // 
            this.TransactionNumberLbl.AutoSize = true;
            this.TransactionNumberLbl.Location = new System.Drawing.Point(340, 47);
            this.TransactionNumberLbl.Name = "TransactionNumberLbl";
            this.TransactionNumberLbl.Size = new System.Drawing.Size(63, 13);
            this.TransactionNumberLbl.TabIndex = 0;
            this.TransactionNumberLbl.Text = "Transaction";
            // 
            // CurrentNameTxtBox
            // 
            this.CurrentNameTxtBox.Location = new System.Drawing.Point(52, 44);
            this.CurrentNameTxtBox.Name = "CurrentNameTxtBox";
            this.CurrentNameTxtBox.Size = new System.Drawing.Size(232, 20);
            this.CurrentNameTxtBox.TabIndex = 1;
            // 
            // CurrentStatusLbl
            // 
            this.CurrentStatusLbl.AutoSize = true;
            this.CurrentStatusLbl.Location = new System.Drawing.Point(443, 47);
            this.CurrentStatusLbl.Name = "CurrentStatusLbl";
            this.CurrentStatusLbl.Size = new System.Drawing.Size(37, 13);
            this.CurrentStatusLbl.TabIndex = 2;
            this.CurrentStatusLbl.Text = "Status";
            this.CurrentStatusLbl.Click += new System.EventHandler(this.CurrentStatusLbl_Click);
            // 
            // CurrentServiceTypeLbl
            // 
            this.CurrentServiceTypeLbl.AutoSize = true;
            this.CurrentServiceTypeLbl.Location = new System.Drawing.Point(49, 88);
            this.CurrentServiceTypeLbl.Name = "CurrentServiceTypeLbl";
            this.CurrentServiceTypeLbl.Size = new System.Drawing.Size(63, 13);
            this.CurrentServiceTypeLbl.TabIndex = 3;
            this.CurrentServiceTypeLbl.Text = "Transaction";
            // 
            // CurrentStartBtn
            // 
            this.CurrentStartBtn.IconChar = FontAwesome.Sharp.IconChar.None;
            this.CurrentStartBtn.IconColor = System.Drawing.Color.Black;
            this.CurrentStartBtn.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.CurrentStartBtn.Location = new System.Drawing.Point(549, 56);
            this.CurrentStartBtn.Name = "CurrentStartBtn";
            this.CurrentStartBtn.Size = new System.Drawing.Size(75, 23);
            this.CurrentStartBtn.TabIndex = 4;
            this.CurrentStartBtn.Text = "Start";
            this.CurrentStartBtn.UseVisualStyleBackColor = true;
            // 
            // CurrentCompeleteBtn
            // 
            this.CurrentCompeleteBtn.IconChar = FontAwesome.Sharp.IconChar.None;
            this.CurrentCompeleteBtn.IconColor = System.Drawing.Color.Black;
            this.CurrentCompeleteBtn.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.CurrentCompeleteBtn.Location = new System.Drawing.Point(661, 56);
            this.CurrentCompeleteBtn.Name = "CurrentCompeleteBtn";
            this.CurrentCompeleteBtn.Size = new System.Drawing.Size(75, 23);
            this.CurrentCompeleteBtn.TabIndex = 5;
            this.CurrentCompeleteBtn.Text = "Complete";
            this.CurrentCompeleteBtn.UseVisualStyleBackColor = true;
            // 
            // currenttransactiondetails
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.CurrentCompeleteBtn);
            this.Controls.Add(this.CurrentStartBtn);
            this.Controls.Add(this.CurrentServiceTypeLbl);
            this.Controls.Add(this.CurrentStatusLbl);
            this.Controls.Add(this.CurrentNameTxtBox);
            this.Controls.Add(this.TransactionNumberLbl);
            this.Name = "currenttransactiondetails";
            this.Size = new System.Drawing.Size(759, 138);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label TransactionNumberLbl;
        private System.Windows.Forms.TextBox CurrentNameTxtBox;
        private System.Windows.Forms.Label CurrentStatusLbl;
        private System.Windows.Forms.Label CurrentServiceTypeLbl;
        private FontAwesome.Sharp.IconButton CurrentStartBtn;
        private FontAwesome.Sharp.IconButton CurrentCompeleteBtn;
    }
}
