namespace Enchante
{
    partial class Enchante
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
            this.components = new System.ComponentModel.Container();
            this.EnchanteParentContainer = new System.Windows.Forms.Panel();
            this.EnchanteVIPPage = new System.Windows.Forms.Panel();
            this.EnchanteMngrPage = new System.Windows.Forms.Panel();
            this.EnchanteStaffPage = new System.Windows.Forms.Panel();
            this.EnchanteHomePage = new System.Windows.Forms.FlowLayoutPanel();
            this.EnchanteParentCard = new Syncfusion.Windows.Forms.Tools.CardLayout(this.components);
            this.EnchanteParentContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.EnchanteParentCard)).BeginInit();
            this.SuspendLayout();
            // 
            // EnchanteParentContainer
            // 
            this.EnchanteParentContainer.BackColor = System.Drawing.SystemColors.ControlText;
            this.EnchanteParentContainer.Controls.Add(this.EnchanteVIPPage);
            this.EnchanteParentContainer.Controls.Add(this.EnchanteMngrPage);
            this.EnchanteParentContainer.Controls.Add(this.EnchanteStaffPage);
            this.EnchanteParentContainer.Controls.Add(this.EnchanteHomePage);
            this.EnchanteParentContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EnchanteParentContainer.Location = new System.Drawing.Point(0, 0);
            this.EnchanteParentContainer.Name = "EnchanteParentContainer";
            this.EnchanteParentContainer.Size = new System.Drawing.Size(1077, 575);
            this.EnchanteParentContainer.TabIndex = 0;
            // 
            // EnchanteVIPPage
            // 
            this.EnchanteVIPPage.BackColor = System.Drawing.Color.Coral;
            this.EnchanteParentCard.SetCardName(this.EnchanteVIPPage, "Card4");
            this.EnchanteVIPPage.Location = new System.Drawing.Point(0, 0);
            this.EnchanteParentCard.SetMinimumSize(this.EnchanteVIPPage, new System.Drawing.Size(200, 100));
            this.EnchanteVIPPage.Name = "EnchanteVIPPage";
            this.EnchanteParentCard.SetPreferredSize(this.EnchanteVIPPage, new System.Drawing.Size(200, 100));
            this.EnchanteVIPPage.Size = new System.Drawing.Size(1077, 575);
            this.EnchanteVIPPage.TabIndex = 3;
            // 
            // EnchanteMngrPage
            // 
            this.EnchanteMngrPage.BackColor = System.Drawing.SystemColors.HotTrack;
            this.EnchanteParentCard.SetCardName(this.EnchanteMngrPage, "Card3");
            this.EnchanteMngrPage.Location = new System.Drawing.Point(0, 0);
            this.EnchanteParentCard.SetMinimumSize(this.EnchanteMngrPage, new System.Drawing.Size(200, 100));
            this.EnchanteMngrPage.Name = "EnchanteMngrPage";
            this.EnchanteParentCard.SetPreferredSize(this.EnchanteMngrPage, new System.Drawing.Size(200, 100));
            this.EnchanteMngrPage.Size = new System.Drawing.Size(862, 460);
            this.EnchanteMngrPage.TabIndex = 2;
            // 
            // EnchanteStaffPage
            // 
            this.EnchanteStaffPage.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.EnchanteParentCard.SetCardName(this.EnchanteStaffPage, "StaffDashboard");
            this.EnchanteStaffPage.Location = new System.Drawing.Point(0, 0);
            this.EnchanteParentCard.SetMinimumSize(this.EnchanteStaffPage, new System.Drawing.Size(200, 100));
            this.EnchanteStaffPage.Name = "EnchanteStaffPage";
            this.EnchanteParentCard.SetPreferredSize(this.EnchanteStaffPage, new System.Drawing.Size(200, 100));
            this.EnchanteStaffPage.Size = new System.Drawing.Size(1077, 575);
            this.EnchanteStaffPage.TabIndex = 1;
            // 
            // EnchanteHomePage
            // 
            this.EnchanteHomePage.BackColor = System.Drawing.SystemColors.ControlDark;
            this.EnchanteParentCard.SetCardName(this.EnchanteHomePage, "HomePage");
            this.EnchanteHomePage.Location = new System.Drawing.Point(0, 0);
            this.EnchanteParentCard.SetMinimumSize(this.EnchanteHomePage, new System.Drawing.Size(200, 100));
            this.EnchanteHomePage.Name = "EnchanteHomePage";
            this.EnchanteParentCard.SetPreferredSize(this.EnchanteHomePage, new System.Drawing.Size(200, 100));
            this.EnchanteHomePage.Size = new System.Drawing.Size(1077, 575);
            this.EnchanteHomePage.TabIndex = 0;
            // 
            // EnchanteParentCard
            // 
            this.EnchanteParentCard.ContainerControl = this.EnchanteParentContainer;
            this.EnchanteParentCard.LayoutMode = Syncfusion.Windows.Forms.Tools.CardLayoutMode.Fill;
            this.EnchanteParentCard.SelectedCard = "Card3";
            // 
            // Enchante
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1077, 575);
            this.Controls.Add(this.EnchanteParentContainer);
            this.Name = "Enchante";
            this.Text = "Enchante";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Enchante_Load);
            this.EnchanteParentContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.EnchanteParentCard)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel EnchanteParentContainer;
        private System.Windows.Forms.FlowLayoutPanel EnchanteHomePage;
        private Syncfusion.Windows.Forms.Tools.CardLayout EnchanteParentCard;
        private System.Windows.Forms.Panel EnchanteStaffPage;
        private System.Windows.Forms.Panel EnchanteMngrPage;
        private System.Windows.Forms.Panel EnchanteVIPPage;
    }
}

