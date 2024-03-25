namespace Enchante
{
    partial class RateMyService
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RateMyService));
            this.RateMeStarBox = new System.Windows.Forms.ComboBox();
            this.RateMeSubmitBtn = new Guna.UI2.WinForms.Guna2Button();
            this.RateMeOne = new FontAwesome.Sharp.IconButton();
            this.RateMeTwo = new FontAwesome.Sharp.IconButton();
            this.RateMeThree = new FontAwesome.Sharp.IconButton();
            this.RateMeFive = new FontAwesome.Sharp.IconButton();
            this.RateMeFour = new FontAwesome.Sharp.IconButton();
            this.RateMeLbl = new System.Windows.Forms.Label();
            this.RateMeNumStarsLbl = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // RateMeStarBox
            // 
            this.RateMeStarBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            this.RateMeStarBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RateMeStarBox.DropDownWidth = 150;
            this.RateMeStarBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RateMeStarBox.Font = new System.Drawing.Font("Arial Black", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RateMeStarBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            this.RateMeStarBox.FormattingEnabled = true;
            this.RateMeStarBox.Location = new System.Drawing.Point(40, 136);
            this.RateMeStarBox.Name = "RateMeStarBox";
            this.RateMeStarBox.Size = new System.Drawing.Size(404, 35);
            this.RateMeStarBox.TabIndex = 27;
            this.RateMeStarBox.SelectedIndexChanged += new System.EventHandler(this.RateMeStarBox_SelectedIndexChanged);
            // 
            // RateMeSubmitBtn
            // 
            this.RateMeSubmitBtn.AutoRoundedCorners = true;
            this.RateMeSubmitBtn.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(183)))), ((int)(((byte)(97)))));
            this.RateMeSubmitBtn.BorderRadius = 20;
            this.RateMeSubmitBtn.BorderThickness = 2;
            this.RateMeSubmitBtn.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.RateMeSubmitBtn.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.RateMeSubmitBtn.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.RateMeSubmitBtn.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.RateMeSubmitBtn.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            this.RateMeSubmitBtn.Font = new System.Drawing.Font("Arial Black", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RateMeSubmitBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            this.RateMeSubmitBtn.Location = new System.Drawing.Point(161, 194);
            this.RateMeSubmitBtn.Name = "RateMeSubmitBtn";
            this.RateMeSubmitBtn.Size = new System.Drawing.Size(150, 42);
            this.RateMeSubmitBtn.TabIndex = 182;
            this.RateMeSubmitBtn.Text = "Submit";
            this.RateMeSubmitBtn.Click += new System.EventHandler(this.RateMeSubmitBtn_Click);
            // 
            // RateMeOne
            // 
            this.RateMeOne.AutoSize = true;
            this.RateMeOne.BackColor = System.Drawing.Color.Transparent;
            this.RateMeOne.Cursor = System.Windows.Forms.Cursors.Hand;
            this.RateMeOne.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(183)))), ((int)(((byte)(97)))));
            this.RateMeOne.FlatAppearance.BorderSize = 0;
            this.RateMeOne.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RateMeOne.Font = new System.Drawing.Font("TechnicBold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.RateMeOne.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            this.RateMeOne.IconChar = FontAwesome.Sharp.IconChar.Star;
            this.RateMeOne.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            this.RateMeOne.IconFont = FontAwesome.Sharp.IconFont.Regular;
            this.RateMeOne.Location = new System.Drawing.Point(43, 61);
            this.RateMeOne.Name = "RateMeOne";
            this.RateMeOne.Size = new System.Drawing.Size(54, 54);
            this.RateMeOne.TabIndex = 183;
            this.RateMeOne.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.RateMeOne.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.RateMeOne.UseVisualStyleBackColor = false;
            this.RateMeOne.Click += new System.EventHandler(this.RateMeOne_Click);
            // 
            // RateMeTwo
            // 
            this.RateMeTwo.AutoSize = true;
            this.RateMeTwo.BackColor = System.Drawing.Color.Transparent;
            this.RateMeTwo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.RateMeTwo.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(183)))), ((int)(((byte)(97)))));
            this.RateMeTwo.FlatAppearance.BorderSize = 0;
            this.RateMeTwo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RateMeTwo.Font = new System.Drawing.Font("TechnicBold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.RateMeTwo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            this.RateMeTwo.IconChar = FontAwesome.Sharp.IconChar.Star;
            this.RateMeTwo.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            this.RateMeTwo.IconFont = FontAwesome.Sharp.IconFont.Regular;
            this.RateMeTwo.Location = new System.Drawing.Point(129, 61);
            this.RateMeTwo.Name = "RateMeTwo";
            this.RateMeTwo.Size = new System.Drawing.Size(54, 54);
            this.RateMeTwo.TabIndex = 184;
            this.RateMeTwo.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.RateMeTwo.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.RateMeTwo.UseVisualStyleBackColor = false;
            this.RateMeTwo.Click += new System.EventHandler(this.RateMeTwo_Click);
            // 
            // RateMeThree
            // 
            this.RateMeThree.AutoSize = true;
            this.RateMeThree.BackColor = System.Drawing.Color.Transparent;
            this.RateMeThree.Cursor = System.Windows.Forms.Cursors.Hand;
            this.RateMeThree.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(183)))), ((int)(((byte)(97)))));
            this.RateMeThree.FlatAppearance.BorderSize = 0;
            this.RateMeThree.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RateMeThree.Font = new System.Drawing.Font("TechnicBold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.RateMeThree.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            this.RateMeThree.IconChar = FontAwesome.Sharp.IconChar.Star;
            this.RateMeThree.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            this.RateMeThree.IconFont = FontAwesome.Sharp.IconFont.Regular;
            this.RateMeThree.Location = new System.Drawing.Point(215, 61);
            this.RateMeThree.Name = "RateMeThree";
            this.RateMeThree.Size = new System.Drawing.Size(54, 54);
            this.RateMeThree.TabIndex = 185;
            this.RateMeThree.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.RateMeThree.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.RateMeThree.UseVisualStyleBackColor = false;
            this.RateMeThree.Click += new System.EventHandler(this.RateMeThree_Click);
            // 
            // RateMeFive
            // 
            this.RateMeFive.AutoSize = true;
            this.RateMeFive.BackColor = System.Drawing.Color.Transparent;
            this.RateMeFive.Cursor = System.Windows.Forms.Cursors.Hand;
            this.RateMeFive.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(183)))), ((int)(((byte)(97)))));
            this.RateMeFive.FlatAppearance.BorderSize = 0;
            this.RateMeFive.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RateMeFive.Font = new System.Drawing.Font("TechnicBold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.RateMeFive.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            this.RateMeFive.IconChar = FontAwesome.Sharp.IconChar.Star;
            this.RateMeFive.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            this.RateMeFive.IconFont = FontAwesome.Sharp.IconFont.Regular;
            this.RateMeFive.Location = new System.Drawing.Point(387, 61);
            this.RateMeFive.Name = "RateMeFive";
            this.RateMeFive.Size = new System.Drawing.Size(54, 54);
            this.RateMeFive.TabIndex = 187;
            this.RateMeFive.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.RateMeFive.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.RateMeFive.UseVisualStyleBackColor = false;
            this.RateMeFive.Click += new System.EventHandler(this.RateMeFive_Click);
            // 
            // RateMeFour
            // 
            this.RateMeFour.AutoSize = true;
            this.RateMeFour.BackColor = System.Drawing.Color.Transparent;
            this.RateMeFour.Cursor = System.Windows.Forms.Cursors.Hand;
            this.RateMeFour.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(183)))), ((int)(((byte)(97)))));
            this.RateMeFour.FlatAppearance.BorderSize = 0;
            this.RateMeFour.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RateMeFour.Font = new System.Drawing.Font("TechnicBold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.RateMeFour.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            this.RateMeFour.IconChar = FontAwesome.Sharp.IconChar.Star;
            this.RateMeFour.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            this.RateMeFour.IconFont = FontAwesome.Sharp.IconFont.Regular;
            this.RateMeFour.Location = new System.Drawing.Point(301, 61);
            this.RateMeFour.Name = "RateMeFour";
            this.RateMeFour.Size = new System.Drawing.Size(54, 54);
            this.RateMeFour.TabIndex = 186;
            this.RateMeFour.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.RateMeFour.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.RateMeFour.UseVisualStyleBackColor = false;
            this.RateMeFour.Click += new System.EventHandler(this.RateMeFour_Click);
            // 
            // RateMeLbl
            // 
            this.RateMeLbl.AutoEllipsis = true;
            this.RateMeLbl.AutoSize = true;
            this.RateMeLbl.Font = new System.Drawing.Font("TechnicBold", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.RateMeLbl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            this.RateMeLbl.Location = new System.Drawing.Point(15, 19);
            this.RateMeLbl.Name = "RateMeLbl";
            this.RateMeLbl.Size = new System.Drawing.Size(357, 29);
            this.RateMeLbl.TabIndex = 188;
            this.RateMeLbl.Text = "Give us your honest review.";
            // 
            // RateMeNumStarsLbl
            // 
            this.RateMeNumStarsLbl.AutoEllipsis = true;
            this.RateMeNumStarsLbl.AutoSize = true;
            this.RateMeNumStarsLbl.Font = new System.Drawing.Font("TechnicBold", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.RateMeNumStarsLbl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            this.RateMeNumStarsLbl.Location = new System.Drawing.Point(364, 194);
            this.RateMeNumStarsLbl.Name = "RateMeNumStarsLbl";
            this.RateMeNumStarsLbl.Size = new System.Drawing.Size(101, 18);
            this.RateMeNumStarsLbl.TabIndex = 189;
            this.RateMeNumStarsLbl.Text = "No. of stars";
            // 
            // RateMyService
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            this.ClientSize = new System.Drawing.Size(484, 261);
            this.Controls.Add(this.RateMeNumStarsLbl);
            this.Controls.Add(this.RateMeLbl);
            this.Controls.Add(this.RateMeFive);
            this.Controls.Add(this.RateMeFour);
            this.Controls.Add(this.RateMeThree);
            this.Controls.Add(this.RateMeTwo);
            this.Controls.Add(this.RateMeOne);
            this.Controls.Add(this.RateMeSubmitBtn);
            this.Controls.Add(this.RateMeStarBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "RateMyService";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Rate My Service";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox RateMeStarBox;
        private Guna.UI2.WinForms.Guna2Button RateMeSubmitBtn;
        private FontAwesome.Sharp.IconButton RateMeOne;
        private FontAwesome.Sharp.IconButton RateMeTwo;
        private FontAwesome.Sharp.IconButton RateMeThree;
        private FontAwesome.Sharp.IconButton RateMeFive;
        private FontAwesome.Sharp.IconButton RateMeFour;
        private System.Windows.Forms.Label RateMeLbl;
        public System.Windows.Forms.Label RateMeNumStarsLbl;
    }
}