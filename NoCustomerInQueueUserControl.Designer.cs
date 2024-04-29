namespace Enchante
{
    partial class NoCustomerInQueueUserControl
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
            this.StaffQueNumberTextBox = new Guna.UI2.WinForms.Guna2Button();
            this.SuspendLayout();
            // 
            // StaffQueNumberTextBox
            // 
            this.StaffQueNumberTextBox.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(183)))), ((int)(((byte)(97)))));
            this.StaffQueNumberTextBox.BorderRadius = 20;
            this.StaffQueNumberTextBox.BorderThickness = 2;
            this.StaffQueNumberTextBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.StaffQueNumberTextBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            this.StaffQueNumberTextBox.DisabledState.CustomBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            this.StaffQueNumberTextBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            this.StaffQueNumberTextBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            this.StaffQueNumberTextBox.Enabled = false;
            this.StaffQueNumberTextBox.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            this.StaffQueNumberTextBox.Font = new System.Drawing.Font("Arial Narrow", 30F, System.Drawing.FontStyle.Bold);
            this.StaffQueNumberTextBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(105)))), ((int)(((byte)(44)))));
            this.StaffQueNumberTextBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(183)))), ((int)(((byte)(97)))));
            this.StaffQueNumberTextBox.HoverState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            this.StaffQueNumberTextBox.HoverState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(105)))), ((int)(((byte)(44)))));
            this.StaffQueNumberTextBox.Location = new System.Drawing.Point(12, 3);
            this.StaffQueNumberTextBox.Name = "StaffQueNumberTextBox";
            this.StaffQueNumberTextBox.Size = new System.Drawing.Size(1269, 198);
            this.StaffQueNumberTextBox.TabIndex = 12;
            this.StaffQueNumberTextBox.Text = "No customers in queue";
            // 
            // NoCustomerInQueueUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.StaffQueNumberTextBox);
            this.Name = "NoCustomerInQueueUserControl";
            this.Size = new System.Drawing.Size(1284, 207);
            this.ResumeLayout(false);

        }

        #endregion

        public Guna.UI2.WinForms.Guna2Button StaffQueNumberTextBox;
    }
}
