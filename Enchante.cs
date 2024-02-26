﻿using Guna.UI2.WinForms;
using Guna.UI2.WinForms.Suite;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.X509;
using Syncfusion.Styles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Xml;
using static Enchante.Enchante;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace Enchante
{
    public partial class Enchante : Form
    {
        //local db connection
        public static string mysqlconn = "server=localhost;user=root;database=enchante;password=";
        public MySqlConnection connection = new MySqlConnection(mysqlconn);

        //cardlayout panel classes
        private ParentCard ParentPanelShow; //Parent Card
        private Registration Registration; //Membership Type Card
        private ServiceCard Service; //Service Card
        private ReceptionTransactionCard Transaction;
        private MngrInventoryCard Inventory;

        //tool tip
        private System.Windows.Forms.ToolTip iconToolTip;


        //gender combo box
        private string[] genders = { "Male", "Female", "Prefer Not to Say" };

        // service category combo box
        private string[] Service_Category = { "Hair Styling", "Nail Care", "Face & Skin", "Massage", "Spa" };
        //service type combo box
        private string[] Service_type = { "Hair Cut", "Hair Blowout", "Hair Color", "Hair Extension", "Manicure",
        "Pedicure", "Nail Extension", "Nail Repair", "Package", "Skin Whitening", "Exfoliation Treatment", "Chemical Peel",
        "Hydration Treatment", "Acne Treatment", "Anti-aging Treatment", "Soft Massage", "Moderate Massage", "Hard Massage",
        "Herbal Pool", "Sauna"};
        //admin employee combobox
        private string[] emplType = { "Admin", "Manager", "Staff" };
        private string[] emplCategories = { "Hair Styling", "Face & Skin", "Nail Care", "Massage", "Spa" };
        private string[] emplCatLevels = { "Junior", "Assistant", "Senior" };
        private string[] productType = { "Service Product", "Retail Product" };
        private string[] productStat = { "High Stock", "Low Stock" };

        public List<AvailableStaff> filteredbyschedstaff;
        public Guna.UI2.WinForms.Guna2ToggleSwitch AvailableStaffActiveToggleSwitch;
        private bool IsPrefferredTimeSchedComboBoxModified = false;
        public string membercategory;
        public string membertype;

        public Enchante()
        {
            InitializeComponent();

            // Exit MessageBox 
            this.FormClosing += new FormClosingEventHandler(MainForm_FormClosing);



            //Landing Pages Cardlayout Panel Manager
            ParentPanelShow = new ParentCard(EnchanteHomePage, EnchanteStaffPage, EnchanteReceptionPage, EnchanteMemberPage, EnchanteAdminPage, EnchanteMngrPage);
            Registration = new Registration(MembershipPlanPanel, RegularPlanPanel, PremiumPlanPanel, SVIPPlanPanel);
            Service = new ServiceCard(ServiceType, ServiceHairStyling, ServiceFaceSkin, ServiceNailCare, ServiceSpa, ServiceMassage);
            Transaction = new ReceptionTransactionCard(RecTransactionPanel, RecWalkinPanel, RecAppointmentPanel);
            Inventory = new MngrInventoryCard(MngrInventoryTypePanel, MngrInventoryServicesPanel, MngrInventoryMembershipPanel, MngrInventoryProductsPanel, MngrPayServicePanel, MngrInventoryProductHistoryPanel, MngrSchedPanel);



            //icon tool tip
            iconToolTip = new System.Windows.Forms.ToolTip();
            iconToolTip.IsBalloon = true;

            //gender combobox
            RegularGenderComboText.Items.AddRange(genders);
            RegularGenderComboText.DropDownStyle = ComboBoxStyle.DropDownList;
            SVIPGenderComboText.Items.AddRange(genders);
            SVIPGenderComboText.DropDownStyle = ComboBoxStyle.DropDownList;
            PremGenderComboText.Items.AddRange(genders);
            PremGenderComboText.DropDownStyle = ComboBoxStyle.DropDownList;

            //Mngr inventory comboboxes
            RecServicesCategoryComboText.Items.AddRange(Service_Category);
            RecServicesCategoryComboText.DropDownStyle = ComboBoxStyle.DropDownList;
            RecServicesTypeComboText.Items.AddRange(Service_type);
            RecServicesTypeComboText.DropDownStyle = ComboBoxStyle.DropDownList;
            
            MngrInventoryProductsCatComboText.Items.AddRange(Service_Category);
            MngrInventoryProductsCatComboText.DropDownStyle = ComboBoxStyle.DropDownList;
            MngrInventoryProductsTypeComboText.Items.AddRange(productType);
            MngrInventoryProductsTypeComboText.DropDownStyle = ComboBoxStyle.DropDownList;
            MngrInventoryProductsStatusComboText.Items.AddRange(productStat);
            MngrInventoryProductsStatusComboText.DropDownStyle = ComboBoxStyle.DropDownList;

            //admin combobox
            AdminGenderComboText.Items.AddRange(genders);
            AdminGenderComboText.DropDownStyle = ComboBoxStyle.DropDownList;
            AdminEmplTypeComboText.Items.AddRange(emplType);
            AdminEmplTypeComboText.DropDownStyle = ComboBoxStyle.DropDownList;
            AdminEmplCatComboText.Items.AddRange(emplCategories);
            AdminEmplCatComboText.DropDownStyle = ComboBoxStyle.DropDownList;
            AdminEmplCatLvlComboText.Items.AddRange(emplCatLevels);
            AdminEmplCatLvlComboText.DropDownStyle = ComboBoxStyle.DropDownList;

            //mngr combobox


            RecEditSchedBtn.Click += RecEditSchedBtn_Click;
            MngrStaffAvailabilityComboBox.SelectedIndex = 0;
            MngrStaffSchedComboBox.SelectedIndex = 0;

            InitializeAvailableStaffFlowLayout();

            RecAppPrefferedTimeAMComboBox.SelectedIndex = 0;
            RecAppPrefferedTimePMComboBox.SelectedIndex = 0;

            RecAppPrefferedTimeAMComboBox.SelectedIndexChanged += RecPrefferedTimeComboBox_SelectedIndexChanged;
            RecAppPrefferedTimePMComboBox.SelectedIndexChanged += RecPrefferedTimeComboBox_SelectedIndexChanged;

            RecAppPrefferedTimeAMComboBox.Enabled = false;
            RecAppPrefferedTimePMComboBox.Enabled = false;

            InitializePendingCustomersForStaff();
            
        }

        private void Enchante_Load(object sender, EventArgs e)
        {
            //Reset Panel to Show Default
            HomePanelReset();
            DB_Loader();
            FillRecStaffScheduleViewDataGrid();
            DateTimePickerTimer.Interval = 1000;
            DateTimePickerTimer.Start();
        }

        private void DB_Loader()
        {
            ReceptionLoadServices();
        }
        public void ReceptionLoadServices()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                {
                    connection.Open();
                    string sql = "SELECT * FROM `services`";
                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    System.Data.DataTable dataTable = new System.Data.DataTable();

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);


                        RecInventoryServicesTable.DataSource = dataTable;

                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred: " + e.Message, "Inventory Service List");
            }
            finally
            {
                // Make sure to close the connection (if it's open)
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            // Rest of your code for configuring DataGridView to display images without distortion
        }

        private void EnchanteHomeScrollPanel_Click(object sender, EventArgs e)
        {
            //Reset Panel to Show Default
            HomePanelReset();
        }

        private void HomePanelReset()
        {
            ParentPanelShow.PanelShow(EnchanteHomePage);
            Service.PanelShow(ServiceType);
            Registration.PanelShow(MembershipPlanPanel);

        }
        private void MngrHomePanelReset()
        {
            ParentPanelShow.PanelShow(EnchanteMngrPage);
            Inventory.PanelShow(MngrInventoryTypePanel);
        }
        private void ReceptionHomePanelReset()
        {
            ParentPanelShow.PanelShow(EnchanteReceptionPage);
            Transaction.PanelShow(RecTransactionPanel);
        }

        private void StaffHomePanelReset()
        {
            ParentPanelShow.PanelShow(EnchanteStaffPage);

        }
        private void AdminHomePanelReset()
        {
            ParentPanelShow.PanelShow(EnchanteAdminPage);

        }
        private void MemberHomePanelReset()
        {
            ParentPanelShow.PanelShow(EnchanteMemberPage);

        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (e.CloseReason == CloseReason.UserClosing)
            {
                // Prevent the form from closing.
                e.Cancel = true;

                DialogResult result = MessageBox.Show("Do you want to close the application?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    this.Dispose();

                }


            }
        }
        public class HashHelper
        {
            public static string HashString(string input)
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                    byte[] hashBytes = sha256.ComputeHash(inputBytes);
                    string hashedString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
                    return hashedString;
                }
            }
        }
        public class HashHelper_Salt
        {
            public static string HashString_Salt(string input_Salt)
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] inputBytes_Salt = Encoding.UTF8.GetBytes(input_Salt);
                    byte[] hashBytes_Salt = sha256.ComputeHash(inputBytes_Salt);
                    string hashedString_Salt = BitConverter.ToString(hashBytes_Salt).Replace("-", "").ToLower();
                    return hashedString_Salt;
                }
            }
        }
        public class HashHelper_SaltperUser
        {
            public static string HashString_SaltperUser(string input_SaltperUser)
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] inputBytes_SaltperUser = Encoding.UTF8.GetBytes(input_SaltperUser);
                    byte[] hashBytes_SaltperUser = sha256.ComputeHash(inputBytes_SaltperUser);
                    string hashedString_SaltperUser = BitConverter.ToString(hashBytes_SaltperUser).Replace("-", "").ToLower();
                    return hashedString_SaltperUser;
                }
            }
        }


        private void ScrollToCoordinates(int x, int y)
        {
            // Set the AutoScrollPosition to the desired coordinates
            EnchanteHomeScrollPanel.AutoScrollPosition = new Point(x, y);
        }

        private void EnchanteAppointBtn_Click(object sender, EventArgs e)
        {
            MemberLocationAndColor();
        }

        private void EnchanteHLoginBtn_Click(object sender, EventArgs e)
        {


            if (EnchanteLoginForm.Visible == false)
            {


                HomeLocationAndColor();


                EnchanteLoginForm.Visible = true;
                return;
            }
            else
            {



                HomeLocationAndColor();



                EnchanteLoginForm.Visible = false;

            }

        }

        private void EnchanteHomeBtn_Click(object sender, EventArgs e)
        {
            //Reset Panel to Show Default
            HomePanelReset();
            HomeLocationAndColor();
        }
        private void EnchanteHeaderLogo_Click(object sender, EventArgs e)
        {
            //Reset Panel to Show Default
            HomePanelReset();
            HomeLocationAndColor();
        }
        private void HomeLocationAndColor()
        {
            // Scroll to the Home position (0, 0)
            ScrollToCoordinates(0, 0);
            //Change color once clicked
            EnchanteHomeBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(183)))), ((int)(((byte)(97)))));

            //Change back to original
            EnchanteServiceBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            EnchanteMemberBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            EnchanteReviewBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            EnchanteTeamBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            EnchanteAbtUsBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
        }

        private void EnchanteServiceBtn_Click(object sender, EventArgs e)
        {
            ServiceLocationAndColor();
        }

        private void ServiceLocationAndColor()
        {
            //Reset Panel to Show Default
            Service.PanelShow(ServiceType);

            int serviceSectionY = 1000;
            ScrollToCoordinates(0, serviceSectionY);
            //Change color once clicked
            EnchanteServiceBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(183)))), ((int)(((byte)(97)))));
            //Change back to original
            EnchanteHomeBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            EnchanteMemberBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            EnchanteReviewBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            EnchanteTeamBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            EnchanteAbtUsBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
        }

        private void EnchanteMemberBtn_Click(object sender, EventArgs e)
        {
            MemberLocationAndColor();
        }

        private void MemberLocationAndColor()
        {
            //Reset Panel to Show Default
            Registration.PanelShow(MembershipPlanPanel);

            //location scroll
            int serviceSectionY = 1800;
            ScrollToCoordinates(0, serviceSectionY);

            //Change color once clicked
            EnchanteMemberBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(183)))), ((int)(((byte)(97)))));
            //Change back to original
            EnchanteHomeBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            EnchanteServiceBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            EnchanteReviewBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            EnchanteTeamBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            EnchanteAbtUsBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
        }
        private void EnchanteReviewBtn_Click(object sender, EventArgs e)
        {
            ReviewLocationAndColor();
        }

        private void ReviewLocationAndColor()
        {
            //Reset Panel to Show Default
            HomePanelReset();

            ////location scroll
            //int serviceSectionY = 1800;
            //ScrollToCoordinates(0, serviceSectionY);

            //Change color once clicked
            EnchanteReviewBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(183)))), ((int)(((byte)(97)))));
            //Change back to original
            EnchanteHomeBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            EnchanteServiceBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            EnchanteMemberBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            EnchanteTeamBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            EnchanteAbtUsBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
        }
        private void EnchanteTeamBtn_Click(object sender, EventArgs e)
        {
            TeamLocationAndColor();
        }

        private void TeamLocationAndColor()
        {
            //Reset Panel to Show Default
            HomePanelReset();

            ////location scroll
            //int serviceSectionY = 1800;
            //ScrollToCoordinates(0, serviceSectionY);

            //Change color once clicked
            EnchanteTeamBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(183)))), ((int)(((byte)(97)))));
            //Change back to original
            EnchanteHomeBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            EnchanteServiceBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            EnchanteMemberBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            EnchanteReviewBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            EnchanteAbtUsBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
        }
        private void EnchanteAbtUsBtn_Click(object sender, EventArgs e)
        {
            AboutUsLocatonAndColor();
        }

        private void AboutUsLocatonAndColor()
        {
            //Reset Panel to Show Default
            HomePanelReset();

            ////location scroll
            //int serviceSectionY = 1800;
            //ScrollToCoordinates(0, serviceSectionY);

            //Change color once clicked
            EnchanteAbtUsBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(183)))), ((int)(((byte)(97)))));
            //Change back to original
            EnchanteHomeBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            EnchanteServiceBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            EnchanteMemberBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            EnchanteReviewBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            EnchanteTeamBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
        }
        private void EnchanteHomeBtn_MouseHover(object sender, EventArgs e)
        {
            iconToolTip.SetToolTip(EnchanteHomeBtn, "Home");
        }

        private void EnchanteServiceBtn_MouseHover(object sender, EventArgs e)
        {
            iconToolTip.SetToolTip(EnchanteServiceBtn, "Service");
        }


        private void EnchanteMemberBtn_MouseHover(object sender, EventArgs e)
        {
            iconToolTip.SetToolTip(EnchanteMemberBtn, "Membership");
        }

        private void EnchanteReviewBtn_MouseHover(object sender, EventArgs e)
        {
            iconToolTip.SetToolTip(EnchanteReviewBtn, "Reviews");
        }

        private void EnchanteTeamBtn_MouseHover(object sender, EventArgs e)
        {
            iconToolTip.SetToolTip(EnchanteTeamBtn, "Our Team");
        }
        private void EnchanteAbtUsBtn_MouseHover(object sender, EventArgs e)
        {
            iconToolTip.SetToolTip(EnchanteAbtUsBtn, "About Us");
        }
        private void EnchanteHLoginBtn_MouseHover(object sender, EventArgs e)
        {
            iconToolTip.SetToolTip(EnchanteHLoginBtn, "Login");
        }

        //services part

        private void ServiceHSBtn_Click(object sender, EventArgs e)
        {
            Service.PanelShow(ServiceHairStyling);

        }

        private void ServiceFSBtn_Click(object sender, EventArgs e)
        {
            Service.PanelShow(ServiceFaceSkin);

        }

        private void ServiceNCBtn_Click(object sender, EventArgs e)
        {
            Service.PanelShow(ServiceNailCare);

        }

        private void ServiceSpaBtn_Click(object sender, EventArgs e)
        {
            Service.PanelShow(ServiceSpa);

        }

        private void ServiceMBtn_Click(object sender, EventArgs e)
        {
            Service.PanelShow(ServiceMassage);

        }



        private void ShowHidePassBtn_Click(object sender, EventArgs e)
        {
            if (LoginPassText.UseSystemPasswordChar == true)
            {
                LoginPassText.UseSystemPasswordChar = false;
                ShowHidePassBtn.IconChar = FontAwesome.Sharp.IconChar.EyeSlash;
            }
            else if (LoginPassText.UseSystemPasswordChar == false)
            {
                LoginPassText.UseSystemPasswordChar = true;
                ShowHidePassBtn.IconChar = FontAwesome.Sharp.IconChar.Eye;





            }
        }
        private void ShowHidePassBtn_MouseHover(object sender, EventArgs e)
        {
            if (LoginPassText.UseSystemPasswordChar == true)
            {
                iconToolTip.SetToolTip(ShowHidePassBtn, "Show Password");
            }
            else if (LoginPassText.UseSystemPasswordChar == false)
            {
                iconToolTip.SetToolTip(ShowHidePassBtn, "Hide Password");
            }
        }
        private void LoginPassReqBtn_MouseHover(object sender, EventArgs e)
        {
            string message = "Must be at least 8 character long.\n";
            message += "First character must be capital.\n";
            message += "Must include a special character and a number.";

            iconToolTip.SetToolTip(LoginPassReqBtn, message);
        }
        private void LoginBtn_Click(object sender, EventArgs e)
        {
            loginchecker();
        }
        private void LoginPassText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                loginchecker();

                e.SuppressKeyPress = true;
            }
        }
        private void loginchecker()
        {
            if (LoginEmailAddText.Text == "Admin" && LoginPassText.Text == "Admin123")
            {
                //Test Admin
                MessageBox.Show("Welcome back, Admin.", "Login Verified", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AdminHomePanelReset();
                PopulateUserInfoDataGrid();
                LoginEmailAddErrorLbl.Visible = false;
                LoginPassErrorLbl.Visible = false;

                logincredclear();
                return;
            }
            else if (LoginEmailAddText.Text != "Admin" && LoginPassText.Text == "Admin123")
            {
                //Test Admin
                LoginEmailAddErrorLbl.Visible = true;
                LoginPassErrorLbl.Visible = false;

                LoginEmailAddErrorLbl.Text = "EMAIL ADDRESS DOES NOT EXIST";

                return;
            }
            else if (LoginEmailAddText.Text == "Admin" && LoginPassText.Text != "Admin123")
            {
                //Test Admin
                LoginEmailAddErrorLbl.Visible = false;
                LoginPassErrorLbl.Visible = true;

                LoginPassErrorLbl.Text = "INCORRECT PASSWORD";

                return;
            }
            else if (LoginEmailAddText.Text == "Manager" && LoginPassText.Text == "Manager123")
            {
                //Test Mngr
                MessageBox.Show("Welcome back, Manager.", "Login Verified", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MngrHomePanelReset();
                MngrNameLbl.Text = "Test Manager";
                MngrIDNumLbl.Text = "TMngr-0000-0000";
                LoginEmailAddErrorLbl.Visible = false;
                LoginPassErrorLbl.Visible = false;
                logincredclear();



                return;
            }
            else if (LoginEmailAddText.Text != "Manager" && LoginPassText.Text == "Manager123")
            {
                //Test Mngr
                LoginEmailAddErrorLbl.Visible = true;
                LoginPassErrorLbl.Visible = false;
                LoginEmailAddErrorLbl.Text = "EMAIL ADDRESS DOES NOT EXIST";
                return;
            }
            else if (LoginEmailAddText.Text == "Manager" && LoginPassText.Text != "Manager123")
            {
                //Test Mngr
                LoginEmailAddErrorLbl.Visible = false;
                LoginPassErrorLbl.Visible = true;
                LoginPassErrorLbl.Text = "INCORRECT PASSWORD";
                return;
            }
            else if (LoginEmailAddText.Text == "Recept" && LoginPassText.Text == "Recept123")
            {
                //Test Recept
                MessageBox.Show("Welcome back, Receptionist.", "Login Verified", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ReceptionHomePanelReset();
                RecNameLbl.Text = "Test Receptionist";
                RecIDNumLbl.Text = "TRec-0000-0000";
                LoginEmailAddErrorLbl.Visible = false;
                LoginPassErrorLbl.Visible = false;
                logincredclear();



                return;
            }
            else if (LoginEmailAddText.Text != "Recept" && LoginPassText.Text == "Recept123")
            {
                //Test Recept
                LoginEmailAddErrorLbl.Visible = true;
                LoginPassErrorLbl.Visible = false;
                LoginEmailAddErrorLbl.Text = "EMAIL ADDRESS DOES NOT EXIST";
                return;
            }
            else if (LoginEmailAddText.Text == "Recept" && LoginPassText.Text != "Recept123")
            {
                //Test Recept
                LoginEmailAddErrorLbl.Visible = false;
                LoginPassErrorLbl.Visible = true;
                LoginPassErrorLbl.Text = "INCORRECT PASSWORD";
                return;
            }
            else if (LoginEmailAddText.Text == "Staff" && LoginPassText.Text == "Staff123")
            {
                //Test Staff
                MessageBox.Show("Welcome back, Staff.", "Login Verified", MessageBoxButtons.OK, MessageBoxIcon.Information);
                StaffHomePanelReset();
                LoginEmailAddErrorLbl.Visible = false;
                LoginPassErrorLbl.Visible = false;
                logincredclear();
                Service.PanelShow(ServiceType);

                return;
            }
            else if (LoginEmailAddText.Text != "Staff" && LoginPassText.Text == "Staff123")
            {
                //Test Staff
                LoginEmailAddErrorLbl.Visible = true;
                LoginPassErrorLbl.Visible = false;
                LoginEmailAddErrorLbl.Text = "EMAIL ADDRESS DOES NOT EXIST";
                return;
            }
            else if (LoginEmailAddText.Text == "Staff" && LoginPassText.Text != "Staff123")
            {
                //Test Mngr
                LoginEmailAddErrorLbl.Visible = false;
                LoginPassErrorLbl.Visible = true;
                LoginPassErrorLbl.Text = "INCORRECT PASSWORD";
                return;
            }
            else if (LoginEmailAddText.Text == "Member" && LoginPassText.Text == "Member123")
            {
                //Test Member
                LoginEmailAddErrorLbl.Visible = false;
                LoginPassErrorLbl.Visible = true;
                MessageBox.Show("Welcome back, Member.", "Login Verified", MessageBoxButtons.OK, MessageBoxIcon.Information);
                MemberHomePanelReset();
                logincredclear();

                return;
            }
            else if (LoginEmailAddText.Text != "Member" && LoginPassText.Text == "Member123")
            {
                //Test Member
                LoginEmailAddErrorLbl.Visible = true;
                LoginPassErrorLbl.Visible = false;
                LoginEmailAddErrorLbl.Text = "EMAIL ADDRESS DOES NOT EXIST";

                return;
            }
            else if (LoginEmailAddText.Text == "Member" && LoginPassText.Text != "Member123")
            {
                //Test Member
                LoginEmailAddErrorLbl.Visible = false;
                LoginPassErrorLbl.Visible = true;
                LoginPassErrorLbl.Text = "INCORRECT PASSWORD";
                return;
            }

            else if (string.IsNullOrEmpty(LoginEmailAddText.Text) && string.IsNullOrEmpty(LoginPassText.Text))
            {
                //MessageBox.Show("Missing text on required fields.", "Ooooops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LoginEmailAddErrorLbl.Visible = true;
                LoginPassErrorLbl.Visible = true;
                LoginEmailAddErrorLbl.Text = "Missing Field";
                LoginPassErrorLbl.Text = "Missing Field";
                return;
            }
            else if (string.IsNullOrEmpty(LoginEmailAddText.Text))
            {
                LoginEmailAddErrorLbl.Visible = true;
                LoginPassErrorLbl.Visible = false;

                LoginEmailAddErrorLbl.Text = "Missing Field";
                return;
            }
            else if (string.IsNullOrEmpty(LoginPassText.Text))
            {
                LoginEmailAddErrorLbl.Visible = false;
                LoginPassErrorLbl.Visible = true;
                LoginPassErrorLbl.Text = "Missing Field";
                return;
            }
            else
            {
                //db connection query
                string email = LoginEmailAddText.Text;
                string password = LoginPassText.Text;
                string passchecker = HashHelper.HashString(password); // Assuming "enteredPassword" is supposed to be "LoginPassText"

                try //user member login
                {
                    connection.Open();

                    string queryApproved = "SELECT FirstName, LastName, MemberIDNumber, MembershipType, HashedPass FROM membershipaccount WHERE EmailAdd = @email";

                    using (MySqlCommand cmdApproved = new MySqlCommand(queryApproved, connection))
                    {
                        cmdApproved.Parameters.AddWithValue("@email", email);

                        using (MySqlDataReader readerApproved = cmdApproved.ExecuteReader())
                        {
                            if (readerApproved.Read())
                            {
                                string name = readerApproved["FirstName"].ToString();
                                string lastname = readerApproved["LastName"].ToString();
                                string ID = readerApproved["MemberIDNumber"].ToString();
                                membertype = readerApproved["MembershipType"].ToString();

                                if (membertype == "Regular")
                                {
                                    // Retrieve the HashedPass column
                                    string hashedPasswordFromDB = readerApproved["HashedPass"].ToString();

                                    // Check if the entered password matches
                                    bool passwordMatches = hashedPasswordFromDB.Equals(passchecker);

                                    if (passwordMatches)
                                    {
                                        MessageBox.Show($"Welcome back, Regular Client {name}.", "Account Verified", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        MemberSubAccUserBtn.Visible = false;
                                        MemberNameLbl.Text = name + " " + lastname;
                                        MemberIDLbl.Text = ID;
                                        MemberHomePanelReset();
                                        logincredclear();

                                    }
                                    else
                                    {
                                        LoginEmailAddErrorLbl.Visible = false;
                                        LoginPassErrorLbl.Visible = true;
                                        LoginPassErrorLbl.Text = "INCORRECT PASSWORD";
                                    }
                                    return;
                                }
                                else if (membertype == "PREMIUM")
                                {
                                    // Retrieve the HashedPass column
                                    string hashedPasswordFromDB = readerApproved["HashedPass"].ToString();

                                    // Check if the entered password matches
                                    bool passwordMatches = hashedPasswordFromDB.Equals(passchecker);

                                    if (passwordMatches)
                                    {
                                        MessageBox.Show($"Welcome back, Premium Client {name}.", "Account Verified", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        MemberNameLbl.Text = name + " " + lastname;
                                        MemberIDLbl.Text = ID;
                                        MemberHomePanelReset();
                                        logincredclear();

                                    }
                                    else
                                    {
                                        LoginEmailAddErrorLbl.Visible = false;
                                        LoginPassErrorLbl.Visible = true;
                                        LoginPassErrorLbl.Text = "INCORRECT PASSWORD";
                                    }
                                    return;
                                }
                                else if (membertype == "SVIP")
                                {
                                    // Retrieve the HashedPass column
                                    string hashedPasswordFromDB = readerApproved["HashedPass"].ToString();

                                    // Check if the entered password matches
                                    bool passwordMatches = hashedPasswordFromDB.Equals(passchecker);

                                    if (passwordMatches)
                                    {
                                        MessageBox.Show($"Welcome back, SVIP Client {name}.", "Account Verified", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        MemberNameLbl.Text = name + " " + lastname;
                                        MemberIDLbl.Text = ID;
                                        MemberHomePanelReset();
                                        logincredclear();

                                    }
                                    else
                                    {
                                        LoginEmailAddErrorLbl.Visible = false;
                                        LoginPassErrorLbl.Visible = true;
                                        LoginPassErrorLbl.Text = "INCORRECT PASSWORD";
                                    }
                                    return;
                                }
                            }

                        }


                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Login Verifier", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection?.Close();
                }

                try //addmin, staff, and manager login
                {
                    connection.Open();

                    string queryApproved = "SELECT FirstName, LastName, EmployeeID, EmployeeType, EmployeeCategory, HashedPass FROM systemusers WHERE Email = @email";

                    using (MySqlCommand cmdApproved = new MySqlCommand(queryApproved, connection))
                    {
                        cmdApproved.Parameters.AddWithValue("@email", email);

                        using (MySqlDataReader readerApproved = cmdApproved.ExecuteReader())
                        {
                            if (readerApproved.Read())
                            {
                                string name = readerApproved["FirstName"].ToString();
                                string lastname = readerApproved["LastName"].ToString();
                                string ID = readerApproved["EmployeeID"].ToString();
                                string membertype = readerApproved["EmployeeType"].ToString();
                                string category = readerApproved["EmployeeCategory"].ToString();

                                if (membertype == "Admin")
                                {
                                    // Retrieve the HashedPass column
                                    string hashedPasswordFromDB = readerApproved["HashedPass"].ToString();

                                    // Check if the entered password matches
                                    bool passwordMatches = hashedPasswordFromDB.Equals(passchecker);

                                    if (passwordMatches)
                                    {
                                        MessageBox.Show($"Welcome back, Admin {name}.", "System User Verified", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        AdminNameLbl.Text = name + " " + lastname;
                                        AdminIDNumlbl.Text = ID;
                                        AdminHomePanelReset();
                                        PopulateUserInfoDataGrid();
                                        logincredclear();

                                    }
                                    else
                                    {
                                        LoginEmailAddErrorLbl.Visible = false;
                                        LoginPassErrorLbl.Visible = true;
                                        LoginPassErrorLbl.Text = "INCORRECT PASSWORD";
                                    }
                                    return;
                                }
                                else if (membertype == "Manager")
                                {
                                    // Retrieve the HashedPass column
                                    string hashedPasswordFromDB = readerApproved["HashedPass"].ToString();

                                    // Check if the entered password matches
                                    bool passwordMatches = hashedPasswordFromDB.Equals(passchecker);

                                    if (passwordMatches)
                                    {
                                        MessageBox.Show($"Welcome back, Manager {name}.", "System User Verified", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        RecNameLbl.Text = name + " " + lastname;
                                        RecIDNumLbl.Text = ID;
                                        ReceptionHomePanelReset();
                                        logincredclear();

                                    }
                                    else
                                    {
                                        LoginEmailAddErrorLbl.Visible = false;
                                        LoginPassErrorLbl.Visible = true;
                                        LoginPassErrorLbl.Text = "INCORRECT PASSWORD";
                                    }
                                    return;
                                }
                                else if (membertype == "Staff")
                                {
                                    // Retrieve the HashedPass column
                                    string hashedPasswordFromDB = readerApproved["HashedPass"].ToString();

                                    // Check if the entered password matches
                                    bool passwordMatches = hashedPasswordFromDB.Equals(passchecker);

                                    if (passwordMatches)
                                    {
                                        MessageBox.Show($"Welcome back, Staff {name}.", "Account Verified", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        //MemberNameLbl.Text = name + " " + lastname;
                                        //MemberIDLbl.Text = ID;
                                        
                                        StaffIDLbl.Text = ID;
                                        StaffMemeberCategoryLbl.Text = category;
                                        membercategory = category;
                                        InitializeStaffInventoryDataGrid();
                                        InitializeStaffPersonalInventoryDataGrid();
                                        StaffCurrentCustomersStatusFlowLayoutPanel.Controls.Clear();
                                        InitializePendingCustomersForStaff();
                                        StaffHomePanelReset();
                                        logincredclear();

                                    }
                                    else
                                    {
                                        LoginEmailAddErrorLbl.Visible = false;
                                        LoginPassErrorLbl.Visible = true;
                                        LoginPassErrorLbl.Text = "INCORRECT PASSWORD";
                                    }
                                    return;
                                }
                            }

                        }


                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Login Verifier", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection?.Close();
                }
            }
        }

        private void logincredclear()
        {
            LoginEmailAddText.Text = "";
            LoginPassText.Text = "";
            LoginEmailAddErrorLbl.Visible = false;
            LoginPassErrorLbl.Visible = false;

        }

        private void MemberSignOut_Click(object sender, EventArgs e)
        {
            LogoutChecker();
        }

        private void MngrSignOutBtn_Click(object sender, EventArgs e)
        {
            LogoutChecker();
        }

        private void StaffSignOutBtn_Click(object sender, EventArgs e)
        {
            foreach (Control control in StaffCurrentCustomersStatusFlowLayoutPanel.Controls)
            {
                if (control is StaffCurrentAvailableCustomersUserControl userControl &&
                    userControl.StaffCustomerServiceStatusTextBox.Text == "In Session")
                {
                    MessageBox.Show("Service Currently In Session");
                    return;
                }
                
            }
            LogoutChecker();
        }

        private void AdminSignOutBtn_Click(object sender, EventArgs e)
        {
            LogoutChecker();
        }

        private void LogoutChecker()
        {
            DialogResult result = MessageBox.Show("Do you want to logout user?", "Logout Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                EnchanteLoginForm.Visible = false;
                ParentPanelShow.PanelShow(EnchanteHomePage);
                StaffCurrentCustomersStatusFlowLayoutPanel.Controls.Clear();
                membercategory = "";
                StaffIDLbl.Text = string.Empty;
                StaffMemeberCategoryLbl.Text = string.Empty;
                StaffInventoryDataGrid.Rows.Clear();
            }
        }

        private void LoginRegisterHereLbl_Click(object sender, EventArgs e)
        {
            MemberLocationAndColor();

        }

        private void SM_FBBtn_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.facebook.com/enchantesalon2024");
        }

        private void SM_TwitterBtn_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://twitter.com/Enchante2024");
        }

        private void SM_IGBtn_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.instagram.com/enchantesalon2024/");
        }

        private void SM_GmailBtn_Click(object sender, EventArgs e)
        {
            string emailAddress = "enchantesalon2024@gmail.com";
            string subject = "Subject of your email";
            string body = "Body of your email";

            string mailtoLink = $"mailto:{emailAddress}?subject={Uri.EscapeDataString(subject)}&body={Uri.EscapeDataString(body)}";

            System.Diagnostics.Process.Start(mailtoLink);
        }

        private void SM_FBBtn_MouseHover(object sender, EventArgs e)
        {
            iconToolTip.SetToolTip(SM_FBBtn, "Facebook");

        }

        private void SM_TwitterBtn_MouseHover(object sender, EventArgs e)
        {
            iconToolTip.SetToolTip(SM_TwitterBtn, "Twitter");
        }

        private void SM_IGBtn_MouseHover(object sender, EventArgs e)
        {
            iconToolTip.SetToolTip(SM_IGBtn, "Instagram");

        }

        private void SM_GmailBtn_MouseHover(object sender, EventArgs e)
        {
            iconToolTip.SetToolTip(SM_GmailBtn, "Email Us Here");
        }

        private void RMemberCreateAccBtn_Click(object sender, EventArgs e)
        {
            Registration.PanelShow(RegularPlanPanel);
            RegularAccIDGenerator();
        }

        private void PMemberCreateAccBtn_Click(object sender, EventArgs e)
        {
            Registration.PanelShow(PremiumPlanPanel);
            PremAccIDGenerator();
            SetExpirationDate("monthly");
            PremMonthly();
        }

        private void SVIPMemberCreateAccBtn_Click(object sender, EventArgs e)
        {
            Registration.PanelShow(SVIPPlanPanel);
            SVIPAccIDGenerator();
            SetExpirationDate("monthly");
            SVIPMonthly();

        }


        //Regular Member Registration
        private void RegularExitBtn_Click(object sender, EventArgs e)
        {
            Registration.PanelShow(MembershipPlanPanel);

        }

        private void RegularBdayPicker_ValueChanged(object sender, EventArgs e)
        {
            DateTime selectedDate = RegularBdayPicker.Value;
            int age = DateTime.Now.Year - selectedDate.Year;

            if (DateTime.Now < selectedDate.AddYears(age))
            {
                age--; // Subtract 1 if the birthday hasn't occurred yet this year
            }

            RegularAgeText.Text = age.ToString();
            if (age < 18)
            {
                RegularAgeErrorLbl.Visible = true;
                RegularAgeErrorLbl.Text = "Must be 18 years old and above";
                return;
            }
            else
            {
                RegularAgeErrorLbl.Visible = false;

            }
        }

        private void RegularGenderComboText_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RegularGenderComboText.SelectedItem != null)
            {
                RegularGenderComboText.Text = RegularGenderComboText.SelectedItem.ToString();
            }
        }

        private void RegularPassReqBtn_MouseHover(object sender, EventArgs e)
        {
            string message = "Must be at least 8 character long.\n";
            message += "First character must be capital.\n";
            message += "Must include a special character and a number.";

            iconToolTip.SetToolTip(RegularPassReqBtn, message);

        }
        private void RegularShowHidePassBtn_Click(object sender, EventArgs e)
        {
            if (RegularPassText.UseSystemPasswordChar == true)
            {
                RegularPassText.UseSystemPasswordChar = false;
                RegularShowHidePassBtn.IconChar = FontAwesome.Sharp.IconChar.EyeSlash;
            }
            else if (RegularPassText.UseSystemPasswordChar == false)
            {
                RegularPassText.UseSystemPasswordChar = true;
                RegularShowHidePassBtn.IconChar = FontAwesome.Sharp.IconChar.Eye;

            }
        }
        private void RegularConfirmPassText_TextChanged(object sender, EventArgs e)
        {
            if (RegularConfirmPassText.Text != RegularPassText.Text)
            {
                RegularConfirmPassErrorLbl.Visible = true;
                RegularConfirmPassErrorLbl.Text = "PASSWORD DOES NOT MATCH";
            }
            else
            {
                RegularConfirmPassErrorLbl.Visible = false;
            }
        }
        private void RegularConfirmShowHidePassBtn_Click(object sender, EventArgs e)
        {
            if (RegularConfirmPassText.UseSystemPasswordChar == true)
            {
                RegularConfirmPassText.UseSystemPasswordChar = false;
                RegularConfirmShowHidePassBtn.IconChar = FontAwesome.Sharp.IconChar.EyeSlash;
            }
            else if (RegularPassText.UseSystemPasswordChar == false)
            {
                RegularConfirmPassText.UseSystemPasswordChar = true;
                RegularConfirmShowHidePassBtn.IconChar = FontAwesome.Sharp.IconChar.Eye;

            }
        }
        public class RegularClientIDGenerator
        {
            private static Random random = new Random();

            public static string GenerateClientID()
            {
                // Get the current year and extract the last digit
                int currentYear = DateTime.Now.Year;
                int lastDigitOfYear = currentYear % 100;

                // Generate a random 6-digit number
                string randomPart = GenerateRandomNumber();

                // Format the ClientID
                string clientID = $"R-{lastDigitOfYear:D2}-{randomPart:D6}";

                return clientID;
            }

            private static string GenerateRandomNumber()
            {
                // Generate a random 6-digit number
                int randomNumber = random.Next(100000, 999999);

                return randomNumber.ToString();
            }
        }
        private void RegularAccIDGenerator()
        {
            RegularMemberIDText.Text = "";

            // Call the GenerateClientID method using the type name
            string generatedClientID = RegularClientIDGenerator.GenerateClientID();

            RegularMemberIDText.Text = generatedClientID;
        }
        private void RegularMemberIDCopyBtn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(RegularMemberIDText.Text))
            {
                RegularMemberIDCopyLbl.Visible = true;
                RegularMemberIDCopyLbl.Text = "ID Number Copied Successfully";
                Clipboard.SetText(RegularMemberIDText.Text);

            }
        }
        private void RegularCreateAccBtn_Click(object sender, EventArgs e)
        {
            DateTime selectedDate = RegularBdayPicker.Value;
            DateTime currentDate = DateTime.Now;

            string rCreated = currentDate.ToString("MM-dd-yyyy");
            string rStatus = "Active";
            string rType = "Regular";
            string rPlanPeriod = "None";
            string rFirstname = RegularFirstNameText.Text;
            string rLastname = RegularLastNameText.Text;
            string rBday = selectedDate.ToString("MM-dd-yyyy");
            string rAge = RegularAgeText.Text;
            string rGender = RegularGenderComboText.Text;
            string rNumber = RegularMobileNumText.Text;
            string rEmailAdd = RegularEmailText.Text;
            string rMemberID = RegularMemberIDText.Text;
            string rPass = RegularPassText.Text;
            string rConfirmPass = RegularConfirmPassText.Text;

            Regex nameRegex = new Regex("^[A-Z][a-zA-Z]+(?: [a-zA-Z]+)*$");
            Regex gmailRegex = new Regex(@"^[A-Za-z0-9._%+-]*\d*@gmail\.com$");
            Regex passwordRegex = new Regex("^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*[!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?])[A-Za-z\\d!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?]{8,}$");

            string hashedPassword = HashHelper.HashString(rPass);    // Password hashed
            string fixedSalt = HashHelper_Salt.HashString_Salt("Enchante" + rPass + "2024");    //Fixed Salt
            string perUserSalt = HashHelper_SaltperUser.HashString_SaltperUser(rPass + rMemberID);    //Per User salt

            int age = DateTime.Now.Year - selectedDate.Year;
            if (DateTime.Now < selectedDate.AddYears(age))
            {
                age--; // Subtract 1 if the birthday hasn't occurred yet this year
            }

            if (string.IsNullOrEmpty(rFirstname) || string.IsNullOrEmpty(rLastname) || string.IsNullOrEmpty(rAge) ||
                string.IsNullOrEmpty(rGender) || string.IsNullOrEmpty(rNumber) || string.IsNullOrEmpty(rEmailAdd) ||
                string.IsNullOrEmpty(rNumber) || string.IsNullOrEmpty(rPass) || string.IsNullOrEmpty(rConfirmPass))
            {
                RegularFirstNameErrorLbl.Visible = true;
                RegularGenderErrorLbl.Visible = true;
                RegularMobileNumErrorLbl.Visible = true;
                RegularEmailErrorLbl.Visible = true;
                RegularPassErrorLbl.Visible = true;
                RegularConfirmPassErrorLbl.Visible = true;
                RegularLastNameErrorLbl.Visible = true;
                RegularAgeErrorLbl.Visible = true;

                RegularFirstNameErrorLbl.Text = "Missing Field";
                RegularGenderErrorLbl.Text = "Missing Field";
                RegularMobileNumErrorLbl.Text = "Missing Field";
                RegularEmailErrorLbl.Text = "Missing Field";
                RegularPassErrorLbl.Text = "Missing Field";
                RegularConfirmPassErrorLbl.Text = "Missing Field";
                RegularLastNameErrorLbl.Text = "Missing Field";
                RegularAgeErrorLbl.Text = "Missing Field";

            }
            else if (age < 18)
            {
                RegularAgeErrorLbl.Visible = true;
                RegularAgeErrorLbl.Text = "Must be 18 years old and above";
                return;
            }
            else if (!nameRegex.IsMatch(rFirstname) && !nameRegex.IsMatch(rLastname))
            {
                RegularFirstNameErrorLbl.Visible = true;
                RegularLastNameErrorLbl.Visible = true;

                RegularFirstNameErrorLbl.Text = "First Letter Must Be Capital";
                RegularLastNameErrorLbl.Text = "First Letter Must Be Capital";

                return;
            }
            else if (!gmailRegex.IsMatch(rEmailAdd))
            {
                RegularEmailErrorLbl.Visible = true;
                RegularEmailErrorLbl.Text = "Invalid Email Format";
                return;
            }
            else if (!passwordRegex.IsMatch(rPass))
            {
                RegularPassErrorLbl.Visible = true;
                RegularPassErrorLbl.Text = "Invalid Password Format";
                return;
            }
            else if (rPass != rConfirmPass)
            {
                RegularConfirmPassErrorLbl.Visible = true;
                RegularPassErrorLbl.Text = "PASSWORD DOES NOT MATCH";
                return;
            }
            else
            {
                try
                {
                    using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                    {
                        connection.Open();
                        // Check if email already exists
                        string checkEmailQuery = "SELECT COUNT(*) FROM membershipaccount WHERE EmailAdd = @email";
                        MySqlCommand checkEmailCmd = new MySqlCommand(checkEmailQuery, connection);
                        checkEmailCmd.Parameters.AddWithValue("@email", rEmailAdd);

                        int emailCount = Convert.ToInt32(checkEmailCmd.ExecuteScalar());

                        if (emailCount > 0)
                        {
                            // Email already exists, show a message or take appropriate action
                            MessageBox.Show("Email already exists. Please use a different email.", "Email Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return; // Exit the method without inserting the new account
                        }
                        string insertQuery = "INSERT INTO membershipaccount (MembershipType, MemberIDNumber, AccountStatus, FirstName, " +
                            "LastName, Birthday, Age, CPNumber, EmailAdd, HashedPass, SaltedPass, UserSaltedPass, PlanPeriod, AccountCreated) " +
                            "VALUES (@type, @ID, @status, @firstName, @lastName, @bday, @age, @cpnum, @email, @hashedpass, @saltedpass, @usersaltedpass, @period, @created)";

                        MySqlCommand cmd = new MySqlCommand(insertQuery, connection);
                        cmd.Parameters.AddWithValue("@type", rType);
                        cmd.Parameters.AddWithValue("@ID", rMemberID);
                        cmd.Parameters.AddWithValue("@status", rStatus);
                        cmd.Parameters.AddWithValue("@firstName", rFirstname);
                        cmd.Parameters.AddWithValue("@lastName", rLastname);
                        cmd.Parameters.AddWithValue("@bday", rBday);
                        cmd.Parameters.AddWithValue("@age", rAge);
                        cmd.Parameters.AddWithValue("@cpnum", rNumber);
                        cmd.Parameters.AddWithValue("@email", rEmailAdd);
                        cmd.Parameters.AddWithValue("@hashedpass", hashedPassword);
                        cmd.Parameters.AddWithValue("@saltedpass", fixedSalt);
                        cmd.Parameters.AddWithValue("@usersaltedpass", perUserSalt);
                        cmd.Parameters.AddWithValue("@period", rPlanPeriod);
                        cmd.Parameters.AddWithValue("@created", rCreated);

                        cmd.ExecuteNonQuery();
                    }

                    // Successful insertion
                    MessageBox.Show("Regular Account is successfully created.", "Welcome to Enchanté", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RegularAccIDGenerator();
                    RegularMembershipBoxClear();
                    MemberLocationAndColor();

                }
                catch (MySqlException ex)
                {
                    // Handle MySQL database exception
                    MessageBox.Show("MySQL Error: " + ex.Message, "Creating Regular Account Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // Make sure to close the connection
                    connection.Close();
                }
            }


        }
        private void RegularMembershipBoxClear()
        {
            RegularFirstNameText.Text = "";
            RegularLastNameText.Text = "";
            RegularAgeText.Text = "";
            RegularGenderComboText.SelectedIndex = -1;
            RegularMobileNumText.Text = "";
            RegularEmailText.Text = "";
            RegularMemberIDText.Text = "";
            RegularPassText.Text = "";
            RegularConfirmPassText.Text = "";


        }

        //Super VIP Plan Membership
        private void SVIPExitBtn_Click(object sender, EventArgs e)
        {
            Registration.PanelShow(MembershipPlanPanel);
        }
        private void SetExpirationDate(string planType)
        {
            DateTime registrationDate = DateTime.Now; // Replace with your actual registration date

            switch (planType.ToLower())
            {
                case "monthly":
                    SVIPPlanExpirationText.Text = CalculateMonthlyExpirationDate(registrationDate);
                    PremPlanExpirationText.Text = CalculateMonthlyExpirationDate(registrationDate);

                    break;

                case "yearly":
                    SVIPPlanExpirationText.Text = CalculateYearlyExpirationDate(registrationDate);
                    PremPlanExpirationText.Text = CalculateMonthlyExpirationDate(registrationDate);

                    break;

                case "biyearly":
                    SVIPPlanExpirationText.Text = CalculateBiyearlyExpirationDate(registrationDate);
                    PremPlanExpirationText.Text = CalculateMonthlyExpirationDate(registrationDate);

                    break;

                default:
                    // Handle invalid plan type
                    break;
            }
        }

        private string CalculateMonthlyExpirationDate(DateTime registrationDate)
        {
            DateTime expirationDate = registrationDate.AddMonths(1);
            return expirationDate.ToString("MM-dd-yyyy");
        }

        private string CalculateYearlyExpirationDate(DateTime registrationDate)
        {
            DateTime expirationDate = registrationDate.AddYears(1);
            return expirationDate.ToString("MM-dd-yyyy");
        }

        private string CalculateBiyearlyExpirationDate(DateTime registrationDate)
        {
            DateTime expirationDate = registrationDate.AddYears(2);
            return expirationDate.ToString("MM-dd-yyyy");
        }
        private void SVIPMonthlyPlanBtn_Click(object sender, EventArgs e)
        {
            SVIPMonthly();
        }
        private void SVIPYearlyPlanBtn_Click(object sender, EventArgs e)
        {
            SVIPYearly();
        }

        private void SVIPBiyearlyPlanBtn_Click(object sender, EventArgs e)
        {
            SVIPBiyearly();
        }

        private void SVIPMonthlyPlanRB_CheckedChanged(object sender, EventArgs e)
        {
            //SVIPMonthly();
        }

        private void SVIPYearlyPlanRB_CheckedChanged(object sender, EventArgs e)
        {
            //SVIPYearly();
        }

        private void SVIPBiyearlyPlanRB_CheckedChanged(object sender, EventArgs e)
        {
            //SVIPBiyearly();
        }
        private void SVIPMonthly()
        {
            SetExpirationDate("monthly");

            if (SVIPMonthlyPlanRB.Checked == false)
            {
                SVIPMonthlyPlanRB.Visible = true;
                SVIPMonthlyPlanRB.Checked = true;
                SVIPPlanPeriodText.Text = "Super VIP Plan - Monthly";

                SVIPOrigPriceText.Visible = false;
                SVIPOrigPriceText.Text = "Php. 4999.00";
                SVIPNewPriceText.Text = "Php. 4999.00";

                SVIPYearlyPlanRB.Visible = false;
                SVIPBiyearlyPlanRB.Visible = false;
                SVIPYearlyPlanRB.Checked = false;
                SVIPBiyearlyPlanRB.Checked = false;
                return;
            }
            else if (SVIPMonthlyPlanRB.Checked == true)
            {
                SVIPPlanPeriodText.Text = "Super VIP Plan - Monthly";

                SVIPOrigPriceText.Visible = false;
                SVIPOrigPriceText.Text = "Php. 4999.00";
                SVIPNewPriceText.Text = "Php. 4999.00";

                SVIPYearlyPlanRB.Visible = false;
                SVIPBiyearlyPlanRB.Visible = false;
                SVIPYearlyPlanRB.Checked = false;
                SVIPBiyearlyPlanRB.Checked = false;
            }
        }
        private void SVIPYearly()
        {
            SetExpirationDate("yearly");

            if (SVIPYearlyPlanRB.Checked == false)
            {
                SVIPYearlyPlanRB.Visible = true;
                SVIPYearlyPlanRB.Checked = true;
                SVIPPlanPeriodText.Text = "Super VIP Plan - 12 Months";

                SVIPOrigPriceText.Visible = true;
                SVIPOrigPriceText.Text = "Php. 4999.00";
                SVIPNewPriceText.Text = "Php. 3499.00";

                SVIPMonthlyPlanRB.Visible = false;
                SVIPBiyearlyPlanRB.Visible = false;
                SVIPMonthlyPlanRB.Checked = false;
                SVIPBiyearlyPlanRB.Checked = false;
            }
            else
            {
                SVIPYearlyPlanRB.Visible = true;
                SVIPYearlyPlanRB.Checked = true;
            }
        }
        private void SVIPBiyearly()
        {
            SetExpirationDate("biyearly");

            if (SVIPBiyearlyPlanRB.Checked == false)
            {
                SVIPBiyearlyPlanRB.Visible = true;
                SVIPBiyearlyPlanRB.Checked = true;
                SVIPPlanPeriodText.Text = "Super VIP Plan - 24 Months";

                SVIPOrigPriceText.Visible = true;
                SVIPOrigPriceText.Text = "Php. 4999.00";
                SVIPNewPriceText.Text = "Php. 2999.00";

                SVIPMonthlyPlanRB.Visible = false;
                SVIPYearlyPlanRB.Visible = false;
                SVIPMonthlyPlanRB.Checked = false;
                SVIPYearlyPlanRB.Checked = false;
            }
            else
            {
                SVIPBiyearlyPlanRB.Visible = true;
                SVIPBiyearlyPlanRB.Checked = true;
            }
        }
        public class SVIPClientIDGenerator
        {
            private static Random random = new Random();

            public static string GenerateClientID()
            {
                // Get the current year and extract the last digit
                int currentYear = DateTime.Now.Year;
                int lastDigitOfYear = currentYear % 100;

                // Generate a random 6-digit number
                string randomPart = GenerateRandomNumber();

                // Format the ClientID
                string clientID = $"SVIP-{lastDigitOfYear:D2}-{randomPart:D6}";

                return clientID;
            }

            private static string GenerateRandomNumber()
            {
                // Generate a random 6-digit number
                int randomNumber = random.Next(100000, 999999);

                return randomNumber.ToString();
            }
        }
        private void SVIPAccIDGenerator()
        {
            SVIPMemberIDText.Text = "";

            // Call the GenerateClientID method using the type name
            string generatedClientID = SVIPClientIDGenerator.GenerateClientID();

            SVIPMemberIDText.Text = generatedClientID;
        }
        private void SVIPMemberCopyBtn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(SVIPMemberIDText.Text))
            {
                SVIPMemberIDCopyLbl.Visible = true;
                SVIPMemberIDCopyLbl.Text = "ID Number Copied Successfully";
                Clipboard.SetText(SVIPMemberIDText.Text);

            }
        }

        private void SVIPPassReqBtn_MouseHover(object sender, EventArgs e)
        {
            string message = "Must be at least 8 character long.\n";
            message += "First character must be capital.\n";
            message += "Must include a special character and a number.";

            iconToolTip.SetToolTip(SVIPPassReqBtn, message);
        }

        private void SVIPShowHidePassBtn_Click(object sender, EventArgs e)
        {
            if (SVIPPassText.UseSystemPasswordChar == true)
            {
                SVIPPassText.UseSystemPasswordChar = false;
                SVIPShowHidePassBtn.IconChar = FontAwesome.Sharp.IconChar.EyeSlash;
            }
            else if (SVIPPassText.UseSystemPasswordChar == false)
            {
                SVIPPassText.UseSystemPasswordChar = true;
                SVIPShowHidePassBtn.IconChar = FontAwesome.Sharp.IconChar.Eye;

            }
        }

        private void SVIPShowHideConfirmPassBtn_Click(object sender, EventArgs e)
        {
            if (SVIPConfirmPassText.UseSystemPasswordChar == true)
            {
                SVIPConfirmPassText.UseSystemPasswordChar = false;
                SVIPShowHideConfirmPassBtn.IconChar = FontAwesome.Sharp.IconChar.EyeSlash;
            }
            else if (SVIPConfirmPassText.UseSystemPasswordChar == false)
            {
                SVIPConfirmPassText.UseSystemPasswordChar = true;
                SVIPShowHideConfirmPassBtn.IconChar = FontAwesome.Sharp.IconChar.Eye;

            }
        }

        private void SVIPBdayPicker_ValueChanged(object sender, EventArgs e)
        {
            DateTime selectedDate = SVIPBdayPicker.Value;
            int age = DateTime.Now.Year - selectedDate.Year;

            if (DateTime.Now < selectedDate.AddYears(age))
            {
                age--; // Subtract 1 if the birthday hasn't occurred yet this year
            }
            SVIPAgeText.Text = age.ToString();
            if (age < 18)
            {
                SVIPAgeErrorLbl.Visible = true;
                SVIPAgeErrorLbl.Text = "Must be 18 years old and above";
                return;
            }
            else
            {
                SVIPAgeErrorLbl.Visible = false;

            }
        }

        private void SVIPGenderComboText_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SVIPGenderComboText.SelectedItem != null)
            {
                SVIPGenderComboText.Text = SVIPGenderComboText.SelectedItem.ToString();
            }
        }

        private void SVIPCCPaymentBtn_Click(object sender, EventArgs e)
        {
            if (SVIPCCPaymentRB.Checked == false)
            {
                SVIPCCPaymentRB.Visible = true;
                SVIPCCPaymentRB.Checked = true;
                SVIPPaymentTypeText.Text = "Credit Card";

                SVIPPayPPaymentRB.Visible = false;
                SVIPGCPaymentRB.Visible = false;
                SVIPPayMPaymentRB.Visible = false;
                SVIPPayPPaymentRB.Checked = false;
                SVIPGCPaymentRB.Checked = false;
                SVIPPayMPaymentRB.Checked = false;
            }
            else
            {
                SVIPCCPaymentRB.Visible = true;
                SVIPCCPaymentRB.Checked = true;
            }
        }

        private void SVIPPayPPaymentBtn_Click(object sender, EventArgs e)
        {
            if (SVIPPayPPaymentRB.Checked == false)
            {
                SVIPPayPPaymentRB.Visible = true;
                SVIPPayPPaymentRB.Checked = true;
                SVIPPaymentTypeText.Text = "Paypal";

                SVIPCCPaymentRB.Visible = false;
                SVIPGCPaymentRB.Visible = false;
                SVIPPayMPaymentRB.Visible = false;
                SVIPCCPaymentRB.Checked = false;
                SVIPGCPaymentRB.Checked = false;
                SVIPPayMPaymentRB.Checked = false;
            }
            else
            {
                SVIPPayPPaymentRB.Visible = true;
                SVIPPayPPaymentRB.Checked = true;
            }
        }

        private void SVIPGCPaymentBtn_Click(object sender, EventArgs e)
        {
            if (SVIPGCPaymentRB.Checked == false)
            {
                SVIPGCPaymentRB.Visible = true;
                SVIPGCPaymentRB.Checked = true;
                SVIPPaymentTypeText.Text = "GCash";

                SVIPCCPaymentRB.Visible = false;
                SVIPPayPPaymentRB.Visible = false;
                SVIPPayMPaymentRB.Visible = false;
                SVIPCCPaymentRB.Checked = false;
                SVIPPayPPaymentRB.Checked = false;
                SVIPPayMPaymentRB.Checked = false;
            }
            else
            {
                SVIPGCPaymentRB.Visible = true;
                SVIPGCPaymentRB.Checked = true;
            }
        }

        private void SVIPPayMPaymentBtn_Click(object sender, EventArgs e)
        {
            if (SVIPPayMPaymentRB.Checked == false)
            {
                SVIPPayMPaymentRB.Visible = true;
                SVIPPayMPaymentRB.Checked = true;
                SVIPPaymentTypeText.Text = "Paymaya";


                SVIPCCPaymentRB.Visible = false;
                SVIPPayPPaymentRB.Visible = false;
                SVIPGCPaymentRB.Visible = false;
                SVIPCCPaymentRB.Checked = false;
                SVIPPayPPaymentRB.Checked = false;
                SVIPGCPaymentRB.Checked = false;
            }
            else
            {
                SVIPPayMPaymentRB.Checked = true;
            }
        }
        private void SVIPConfirmPassText_TextChanged(object sender, EventArgs e)
        {
            if (SVIPConfirmPassText.Text != SVIPPassText.Text)
            {
                SVIPConfirmPassErrorLbl.Visible = true;
                SVIPConfirmPassErrorLbl.Text = "PASSWORD DOES NOT MATCH";
            }
            else
            {
                SVIPConfirmPassErrorLbl.Visible = false;
            }
        }

        private void SVIPCreateAccBtn_Click(object sender, EventArgs e)
        {
            DateTime selectedDate = SVIPBdayPicker.Value;
            DateTime currentDate = DateTime.Now;

            string SVCreated = currentDate.ToString("MM-dd-yyyy");
            string SVStatus = "Active";
            string SVType = "SVIP";
            string SVFirstname = SVIPFirstNameText.Text;
            string SVLastname = SVIPLastNameText.Text;
            string SVBday = selectedDate.ToString("MM-dd-yyyy");
            string SVAge = SVIPAgeText.Text;
            string SVGender = SVIPGenderComboText.Text;
            string SVNumber = SVIPCPNumText.Text;
            string SVEmailAdd = SVIPEmailText.Text;
            string SVMemberID = SVIPMemberIDText.Text;
            string SVPass = SVIPPassText.Text;
            string SVConfirmPass = SVIPConfirmPassText.Text;
            string SVPeriod = SVIPPlanPeriodText.Text;
            string SVPayment = SVIPPaymentTypeText.Text;
            string SVCardName = SVIPCardNameText.Text;
            string SVCardNum = SVIPCardNumText.Text;
            string SVCardExpire = SVIPCardExpireText.Text;
            string SVcvc = SVIPCardCVCText.Text;
            string SVPlanExpire = SVIPPlanExpirationText.Text;
            string SVPlanRenew = "";
            string SVAmount = SVIPNewPriceText.Text;


            Regex nameRegex = new Regex("^[A-Z][a-zA-Z]+(?: [a-zA-Z]+)*$");
            Regex gmailRegex = new Regex(@"^[A-Za-z0-9._%+-]*\d*@gmail\.com$");
            Regex passwordRegex = new Regex("^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*[!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?])[A-Za-z\\d!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?]{8,}$");

            string hashedPassword = HashHelper.HashString(SVPass);    // Password hashed
            string fixedSalt = HashHelper_Salt.HashString_Salt("Enchante" + SVPass + "2024");    //Fixed Salt
            string perUserSalt = HashHelper_SaltperUser.HashString_SaltperUser(SVPass + SVMemberID);    //Per User salt

            int age = DateTime.Now.Year - selectedDate.Year;
            if (DateTime.Now < selectedDate.AddYears(age))
            {
                age--; // Subtract 1 if the birthday hasn't occurred yet this year
            }

            if (string.IsNullOrEmpty(SVFirstname) || string.IsNullOrEmpty(SVLastname) || string.IsNullOrEmpty(SVAge) ||
                string.IsNullOrEmpty(SVGender) || string.IsNullOrEmpty(SVNumber) || string.IsNullOrEmpty(SVEmailAdd) ||
                string.IsNullOrEmpty(SVNumber) || string.IsNullOrEmpty(SVPass) || string.IsNullOrEmpty(SVConfirmPass) ||
                string.IsNullOrEmpty(SVPeriod) || string.IsNullOrEmpty(SVPayment) || string.IsNullOrEmpty(SVCardName) ||
                string.IsNullOrEmpty(SVCardNum) || string.IsNullOrEmpty(SVCardExpire) || string.IsNullOrEmpty(SVcvc) || string.IsNullOrEmpty(SVAmount))
            {
                SVIPFirstNameErrorLbl.Visible = true;
                SVIPGenderErrorLbl.Visible = true;
                SVIPCPNumErrorLbl.Visible = true;
                SVIPEmailErrorLbl.Visible = true;
                SVIPPassErrorLbl.Visible = true;
                SVIPConfirmPassErrorLbl.Visible = true;
                SVIPLastNameErrorLbl.Visible = true;
                SVIPAgeErrorLbl.Visible = true;


                SVIPFirstNameErrorLbl.Text = "Missing Field";
                SVIPGenderErrorLbl.Text = "Missing Field";
                SVIPCPNumErrorLbl.Text = "Missing Field";
                SVIPEmailErrorLbl.Text = "Missing Field";
                SVIPPassErrorLbl.Text = "Missing Field";
                SVIPConfirmPassErrorLbl.Text = "Missing Field";
                SVIPLastNameErrorLbl.Text = "Missing Field";
                SVIPAgeErrorLbl.Text = "Missing Field";

            }
            else if (age < 18)
            {
                SVIPAgeErrorLbl.Visible = true;
                SVIPAgeErrorLbl.Text = "Must be 18 years old and above";
                return;
            }
            else if (!nameRegex.IsMatch(SVFirstname) && !nameRegex.IsMatch(SVLastname))
            {
                SVIPFirstNameErrorLbl.Visible = true;
                SVIPLastNameErrorLbl.Visible = true;

                SVIPFirstNameErrorLbl.Text = "First Letter Must Be Capital";
                SVIPLastNameErrorLbl.Text = "First Letter Must Be Capital";

                return;
            }
            else if (!gmailRegex.IsMatch(SVEmailAdd))
            {
                SVIPEmailErrorLbl.Visible = true;
                SVIPEmailErrorLbl.Text = "Invalid Email Format";
                return;
            }
            else if (!passwordRegex.IsMatch(SVPass))
            {
                SVIPPassErrorLbl.Visible = true;
                SVIPPassErrorLbl.Text = "Invalid Password Format";
                return;
            }
            else if (SVPass != SVConfirmPass)
            {
                SVIPConfirmPassErrorLbl.Visible = true;
                SVIPPassErrorLbl.Text = "PASSWORD DOES NOT MATCH";
                return;
            }
            else
            {
                try
                {
                    using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                    {
                        connection.Open();

                        // Check if email already exists
                        string checkEmailQuery = "SELECT COUNT(*) FROM membershipaccount WHERE EmailAdd = @email";
                        MySqlCommand checkEmailCmd = new MySqlCommand(checkEmailQuery, connection);
                        checkEmailCmd.Parameters.AddWithValue("@email", SVEmailAdd);

                        int emailCount = Convert.ToInt32(checkEmailCmd.ExecuteScalar());

                        if (emailCount > 0)
                        {
                            // Email already exists, show a message or take appropriate action
                            MessageBox.Show("Email already exists. Please use a different email.", "Email Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return; // Exit the method without inserting the new account
                        }

                        // Email doesn't exist, proceed with insertion
                        string insertQuery = "INSERT INTO membershipaccount (MembershipType, MemberIDNumber, AccountStatus, FirstName, " +
                            "LastName, Birthday, Age, CPNumber, EmailAdd, HashedPass, SaltedPass, UserSaltedPass, PlanPeriod, " +
                            "PaymentType, CardholderName, CardNumber, CardExpiration, CVCCode, AccountCreated, PlanExpiration, PlanRenewal, AmountPaid) " +
                            "VALUES (@type, @ID, @status, @firstName, @lastName, @bday, @age, @cpnum, @email, @hashedpass, @saltedpass, @usersaltedpass, " +
                            "@period, @payment, @cardname, @cardnumber, @cardexpiration, @cvc, @created, @planExpiration, @planRenew, @amount)";

                        MySqlCommand cmd = new MySqlCommand(insertQuery, connection);
                        cmd.Parameters.AddWithValue("@type", SVType);
                        cmd.Parameters.AddWithValue("@ID", SVMemberID);
                        cmd.Parameters.AddWithValue("@status", SVStatus);
                        cmd.Parameters.AddWithValue("@firstName", SVFirstname);
                        cmd.Parameters.AddWithValue("@lastName", SVLastname);
                        cmd.Parameters.AddWithValue("@bday", SVBday);
                        cmd.Parameters.AddWithValue("@age", SVAge);
                        cmd.Parameters.AddWithValue("@cpnum", SVNumber);
                        cmd.Parameters.AddWithValue("@email", SVEmailAdd);
                        cmd.Parameters.AddWithValue("@hashedpass", hashedPassword);
                        cmd.Parameters.AddWithValue("@saltedpass", fixedSalt);
                        cmd.Parameters.AddWithValue("@usersaltedpass", perUserSalt);
                        cmd.Parameters.AddWithValue("@period", SVPeriod);
                        cmd.Parameters.AddWithValue("@payment", SVPayment);
                        cmd.Parameters.AddWithValue("@cardname", SVCardName);
                        cmd.Parameters.AddWithValue("@cardnumber", SVCardNum);
                        cmd.Parameters.AddWithValue("@cardexpiration", SVCardExpire);
                        cmd.Parameters.AddWithValue("@cvc", SVcvc);
                        cmd.Parameters.AddWithValue("@created", SVCreated);
                        cmd.Parameters.AddWithValue("@planExpiration", SVPlanExpire);
                        cmd.Parameters.AddWithValue("@planRenew", SVPlanRenew);
                        cmd.Parameters.AddWithValue("@amount", SVAmount);

                        cmd.ExecuteNonQuery();
                    }

                    // Successful insertion
                    MessageBox.Show("SVIP Account is successfully created.", "Welcome to Enchanté", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    SVIPAccIDGenerator();
                    SVIPMembershipBoxClear();
                    MemberLocationAndColor();
                }
                catch (MySqlException ex)
                {
                    // Handle MySQL database exception
                    MessageBox.Show("MySQL Error: " + ex.Message, "Creating SVIP Account Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // No need to close the connection here as it is in a using statement
                }



            }
        }
        private void SVIPMembershipBoxClear()
        {
            SVIPFirstNameText.Text = "";
            SVIPLastNameText.Text = "";
            SVIPAgeText.Text = "";
            SVIPGenderComboText.SelectedIndex = -1;
            SVIPCPNumText.Text = "";
            SVIPEmailText.Text = "";
            SVIPMemberIDText.Text = "";
            SVIPPassText.Text = "";
            SVIPConfirmPassText.Text = "";
            SVIPBdayPicker.Value = DateTime.Now;
            SVIPCardNameText.Text = "";
            SVIPPlanPeriodText.Text = "";
            SVIPPaymentTypeText.Text = "";
            SVIPCardNumText.Text = "";
            SVIPCardExpireText.Text = "";
            SVIPCardCVCText.Text = "";
            SVIPPlanExpirationText.Text = "";
            SVIPNewPriceText.Text = "";

        }
        //PREMIUM REGISTRATION
        private void PremiumExitBtn_Click(object sender, EventArgs e)
        {
            Registration.PanelShow(MembershipPlanPanel);
        }

        private void PremMonthlyPlanBtn_Click(object sender, EventArgs e)
        {
            PremMonthly();
        }

        private void PremYearlyPlanBtn_Click(object sender, EventArgs e)
        {
            PremYearly();
        }

        private void PremBiyearlyPlanBtn_Click(object sender, EventArgs e)
        {
            PremBiyearly();
        }

        private void PremPassReqBtn_MouseHover(object sender, EventArgs e)
        {
            string message = "Must be at least 8 character long.\n";
            message += "First character must be capital.\n";
            message += "Must include a special character and a number.";

            iconToolTip.SetToolTip(PremPassReqBtn, message);
        }

        private void PremShowHidePassBtn_MouseHover(object sender, EventArgs e)
        {
            if (PremPassText.UseSystemPasswordChar == true)
            {
                iconToolTip.SetToolTip(PremShowHidePassBtn, "Show Password");
            }
            else if (PremPassText.UseSystemPasswordChar == false)
            {
                iconToolTip.SetToolTip(PremShowHidePassBtn, "Hide Password");
            }
        }

        private void PremShowHidePassBtn_Click(object sender, EventArgs e)
        {
            if (PremPassText.UseSystemPasswordChar == true)
            {
                PremPassText.UseSystemPasswordChar = false;
                PremShowHidePassBtn.IconChar = FontAwesome.Sharp.IconChar.EyeSlash;
            }
            else if (SVIPPassText.UseSystemPasswordChar == false)
            {
                PremPassText.UseSystemPasswordChar = true;
                PremShowHidePassBtn.IconChar = FontAwesome.Sharp.IconChar.Eye;

            }
        }

        private void PremShowHideConfirmPassBtn_MouseHover(object sender, EventArgs e)
        {
            if (PremConfirmPassText.UseSystemPasswordChar == true)
            {
                iconToolTip.SetToolTip(PremShowHideConfirmPassBtn, "Show Password");
            }
            else if (PremConfirmPassText.UseSystemPasswordChar == false)
            {
                iconToolTip.SetToolTip(PremShowHideConfirmPassBtn, "Hide Password");
            }
        }

        private void PremShowHideConfirmPassBtn_Click(object sender, EventArgs e)
        {
            if (PremConfirmPassText.UseSystemPasswordChar == true)
            {
                PremConfirmPassText.UseSystemPasswordChar = false;
                PremShowHideConfirmPassBtn.IconChar = FontAwesome.Sharp.IconChar.EyeSlash;
            }
            else if (SVIPConfirmPassText.UseSystemPasswordChar == false)
            {
                PremConfirmPassText.UseSystemPasswordChar = true;
                PremShowHideConfirmPassBtn.IconChar = FontAwesome.Sharp.IconChar.Eye;

            }
        }
        private void PremConfirmPassText_TextChanged(object sender, EventArgs e)
        {
            if (PremConfirmPassText.Text != PremPassText.Text)
            {
                PremConfirmPassErrorLbl.Visible = true;
                PremConfirmPassErrorLbl.Text = "PASSWORD DOES NOT MATCH";
            }
            else
            {
                PremConfirmPassErrorLbl.Visible = false;
            }
        }
        private void PremMemberIDCopyBtn_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(SVIPMemberIDText.Text))
            {
                PremMemberIDCopyLbl.Visible = true;
                PremMemberIDCopyLbl.Text = "ID Number Copied Successfully";
                Clipboard.SetText(PremMemberIDText.Text);

            }
        }

        private void PremBdayPicker_ValueChanged(object sender, EventArgs e)
        {
            DateTime selectedDate = PremBdayPicker.Value;
            int age = DateTime.Now.Year - selectedDate.Year;

            if (DateTime.Now < selectedDate.AddYears(age))
            {
                age--; // Subtract 1 if the birthday hasn't occurred yet this year
            }
            PremAgeText.Text = age.ToString();
            if (age < 18)
            {
                PremAgeErrorLbl.Visible = true;
                PremAgeErrorLbl.Text = "Must be 18 years old and above";
                return;
            }
            else
            {
                PremAgeErrorLbl.Visible = false;

            }
        }

        private void PremGenderComboText_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PremGenderComboText.SelectedItem != null)
            {
                PremGenderComboText.Text = PremGenderComboText.SelectedItem.ToString();
            }
        }
        private void PremMonthly()
        {
            SetExpirationDate("monthly");

            if (PremMonthlyPlanRB.Checked == false)
            {
                PremMonthlyPlanRB.Visible = true;
                PremMonthlyPlanRB.Checked = true;
                PremPlanPeriodText.Text = "Premium Plan - Monthly";

                PremOrigPriceText.Visible = false;
                PremOrigPriceText.Text = "Php. 1499.00";
                PremNewPriceText.Text = "Php. 1499.00";

                PremYearlyPlanRB.Visible = false;
                PremBiyearlyPlanRB.Visible = false;
                PremYearlyPlanRB.Checked = false;
                PremBiyearlyPlanRB.Checked = false;
                return;
            }
            else if (PremMonthlyPlanRB.Checked == true)
            {
                PremPlanPeriodText.Text = "Premium Plan - Monthly";

                PremOrigPriceText.Visible = false;
                PremOrigPriceText.Text = "Php. 1499.00";
                PremNewPriceText.Text = "Php. 1499.00";

                PremYearlyPlanRB.Visible = false;
                PremBiyearlyPlanRB.Visible = false;
                PremYearlyPlanRB.Checked = false;
                PremBiyearlyPlanRB.Checked = false;
            }
        }
        private void PremYearly()
        {
            SetExpirationDate("yearly");

            if (PremYearlyPlanRB.Checked == false)
            {
                PremYearlyPlanRB.Visible = true;
                PremYearlyPlanRB.Checked = true;
                PremPlanPeriodText.Text = "Premium Plan - 12 Months";

                PremOrigPriceText.Visible = true;
                PremOrigPriceText.Text = "Php. 1499.00";
                PremNewPriceText.Text = "Php. 1299.00";

                PremMonthlyPlanRB.Visible = false;
                PremBiyearlyPlanRB.Visible = false;
                PremMonthlyPlanRB.Checked = false;
                PremBiyearlyPlanRB.Checked = false;
            }
            else
            {
                PremYearlyPlanRB.Visible = true;
                PremYearlyPlanRB.Checked = true;
            }
        }
        private void PremBiyearly()
        {
            SetExpirationDate("biyearly");

            if (PremBiyearlyPlanRB.Checked == false)
            {
                PremBiyearlyPlanRB.Visible = true;
                PremBiyearlyPlanRB.Checked = true;
                PremPlanPeriodText.Text = "Premium Plan - 24 Months";

                PremOrigPriceText.Visible = true;
                PremOrigPriceText.Text = "Php. 1499.00";
                PremNewPriceText.Text = "Php. 999.00";

                PremMonthlyPlanRB.Visible = false;
                PremYearlyPlanRB.Visible = false;
                PremMonthlyPlanRB.Checked = false;
                PremYearlyPlanRB.Checked = false;
            }
            else
            {
                PremBiyearlyPlanRB.Visible = true;
                PremBiyearlyPlanRB.Checked = true;
            }
        }
        public class PremClientIDGenerator
        {
            private static Random random = new Random();

            public static string GenerateClientID()
            {
                // Get the current year and extract the last digit
                int currentYear = DateTime.Now.Year;
                int lastDigitOfYear = currentYear % 100;

                // Generate a random 6-digit number
                string randomPart = GenerateRandomNumber();

                // Format the ClientID
                string clientID = $"PREM-{lastDigitOfYear:D2}-{randomPart:D6}";

                return clientID;
            }

            private static string GenerateRandomNumber()
            {
                // Generate a random 6-digit number
                int randomNumber = random.Next(100000, 999999);

                return randomNumber.ToString();
            }
        }
        private void PremAccIDGenerator()
        {
            PremMemberIDText.Text = "";

            // Call the GenerateClientID method using the type name
            string generatedClientID = PremClientIDGenerator.GenerateClientID();

            PremMemberIDText.Text = generatedClientID;
        }

        private void PremCCPaymentBtn_Click(object sender, EventArgs e)
        {
            if (PremCCPaymentRB.Checked == false)
            {
                PremCCPaymentRB.Visible = true;
                PremCCPaymentRB.Checked = true;
                PremPaymentTypeText.Text = "Credit Card";

                PremPayPPaymentRB.Visible = false;
                PremGCPaymentRB.Visible = false;
                PremPayMPaymentRB.Visible = false;
                PremPayPPaymentRB.Checked = false;
                PremGCPaymentRB.Checked = false;
                PremPayMPaymentRB.Checked = false;
            }
            else
            {
                PremCCPaymentRB.Visible = true;
                PremCCPaymentRB.Checked = true;
            }
        }

        private void PremPayPPaymentBtn_Click(object sender, EventArgs e)
        {
            if (PremPayPPaymentRB.Checked == false)
            {
                PremPayPPaymentRB.Visible = true;
                PremPayPPaymentRB.Checked = true;
                PremPaymentTypeText.Text = "Paypal";

                PremCCPaymentRB.Visible = false;
                PremGCPaymentRB.Visible = false;
                PremPayMPaymentRB.Visible = false;
                PremCCPaymentRB.Checked = false;
                PremGCPaymentRB.Checked = false;
                PremPayMPaymentRB.Checked = false;
            }
            else
            {
                PremPayPPaymentRB.Visible = true;
                PremPayPPaymentRB.Checked = true;
            }
        }

        private void PremGCPaymentBtn_Click(object sender, EventArgs e)
        {
            if (PremGCPaymentRB.Checked == false)
            {
                PremGCPaymentRB.Visible = true;
                PremGCPaymentRB.Checked = true;
                PremPaymentTypeText.Text = "GCash";

                PremCCPaymentRB.Visible = false;
                PremPayPPaymentRB.Visible = false;
                PremPayMPaymentRB.Visible = false;
                PremCCPaymentRB.Checked = false;
                PremPayPPaymentRB.Checked = false;
                PremPayMPaymentRB.Checked = false;
            }
            else
            {
                PremGCPaymentRB.Visible = true;
                PremGCPaymentRB.Checked = true;
            }
        }

        private void PremPayMPaymentBtn_Click(object sender, EventArgs e)
        {
            if (PremPayMPaymentRB.Checked == false)
            {
                PremPayMPaymentRB.Visible = true;
                PremPayMPaymentRB.Checked = true;
                PremPaymentTypeText.Text = "Paymaya";

                PremCCPaymentRB.Visible = false;
                PremPayPPaymentRB.Visible = false;
                PremGCPaymentRB.Visible = false;
                PremCCPaymentRB.Checked = false;
                PremPayPPaymentRB.Checked = false;
                PremGCPaymentRB.Checked = false;
            }
            else
            {
                PremPayMPaymentRB.Checked = true;
            }
        }

        private void PremCreateAccBtn_Click(object sender, EventArgs e)
        {
            DateTime selectedDate = PremBdayPicker.Value;
            DateTime currentDate = DateTime.Now;

            string PremCreated = currentDate.ToString("MM-dd-yyyy");
            string PremStatus = "Active";
            string PremType = "PREMIUM";
            string PremFirstname = PremFirstNameText.Text;
            string PremLastname = PremLastNameText.Text;
            string PremBday = selectedDate.ToString("MM-dd-yyyy");
            string PremAge = PremAgeText.Text;
            string PremGender = PremGenderComboText.Text;
            string PremNumber = PremCPNumText.Text;
            string PremEmailAdd = PremEmailText.Text;
            string PremMemberID = PremMemberIDText.Text;
            string PremPass = PremPassText.Text;
            string PremConfirmPass = PremConfirmPassText.Text;
            string PremPeriod = PremPlanPeriodText.Text;
            string PremPayment = PremPaymentTypeText.Text;
            string PremCardName = PremCardNameText.Text;
            string PremCardNum = PremCardNumText.Text;
            string PremCardExpire = PremCardExpireText.Text;
            string Premcvc = PremCardCVCText.Text;
            string PremPlanExpire = PremPlanExpirationText.Text;
            string PremPlanRenew = "";
            string PremAmount = PremNewPriceText.Text;


            Regex nameRegex = new Regex("^[A-Z][a-zA-Z]+(?: [a-zA-Z]+)*$");
            Regex gmailRegex = new Regex(@"^[A-Za-z0-9._%+-]*\d*@gmail\.com$");
            Regex passwordRegex = new Regex("^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*[!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?])[A-Za-z\\d!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?]{8,}$");

            string hashedPassword = HashHelper.HashString(PremPass);    // Password hashed
            string fixedSalt = HashHelper_Salt.HashString_Salt("Enchante" + PremPass + "2024");    //Fixed Salt
            string perUserSalt = HashHelper_SaltperUser.HashString_SaltperUser(PremPass + PremMemberID);    //Per User salt

            int age = DateTime.Now.Year - selectedDate.Year;
            if (DateTime.Now < selectedDate.AddYears(age))
            {
                age--; // Subtract 1 if the birthday hasn't occurred yet this year
            }

            if (string.IsNullOrEmpty(PremFirstname) || string.IsNullOrEmpty(PremLastname) || string.IsNullOrEmpty(PremAge) ||
                string.IsNullOrEmpty(PremGender) || string.IsNullOrEmpty(PremNumber) || string.IsNullOrEmpty(PremEmailAdd) ||
                string.IsNullOrEmpty(PremNumber) || string.IsNullOrEmpty(PremPass) || string.IsNullOrEmpty(PremConfirmPass) ||
                string.IsNullOrEmpty(PremPeriod) || string.IsNullOrEmpty(PremPayment) || string.IsNullOrEmpty(PremCardName) ||
                string.IsNullOrEmpty(PremCardNum) || string.IsNullOrEmpty(PremCardExpire) || string.IsNullOrEmpty(Premcvc) || string.IsNullOrEmpty(PremAmount))
            {
                SVIPFirstNameErrorLbl.Visible = true;
                SVIPGenderErrorLbl.Visible = true;
                SVIPCPNumErrorLbl.Visible = true;
                SVIPEmailErrorLbl.Visible = true;
                SVIPPassErrorLbl.Visible = true;
                SVIPConfirmPassErrorLbl.Visible = true;
                SVIPLastNameErrorLbl.Visible = true;
                SVIPAgeErrorLbl.Visible = true;


                SVIPFirstNameErrorLbl.Text = "Missing Field";
                SVIPGenderErrorLbl.Text = "Missing Field";
                SVIPCPNumErrorLbl.Text = "Missing Field";
                SVIPEmailErrorLbl.Text = "Missing Field";
                SVIPPassErrorLbl.Text = "Missing Field";
                SVIPConfirmPassErrorLbl.Text = "Missing Field";
                SVIPLastNameErrorLbl.Text = "Missing Field";
                SVIPAgeErrorLbl.Text = "Missing Field";

            }
            else if (age < 18)
            {
                SVIPAgeErrorLbl.Visible = true;
                SVIPAgeErrorLbl.Text = "Must be 18 years old and above";
                return;
            }
            else if (!nameRegex.IsMatch(PremFirstname) && !nameRegex.IsMatch(PremLastname))
            {
                PremFirstNameErrorLbl.Visible = true;
                PremLastNameErrorLbl.Visible = true;

                PremFirstNameErrorLbl.Text = "First Letter Must Be Capital";
                PremLastNameErrorLbl.Text = "First Letter Must Be Capital";

                return;
            }
            else if (!gmailRegex.IsMatch(PremEmailAdd))
            {
                PremEmailErrorLbl.Visible = true;
                PremEmailErrorLbl.Text = "Invalid Email Format";
                return;
            }
            else if (!passwordRegex.IsMatch(PremPass))
            {
                PremPassErrorLbl.Visible = true;
                PremPassErrorLbl.Text = "Invalid Password Format";
                return;
            }
            else if (PremPass != PremConfirmPass)
            {
                PremConfirmPassErrorLbl.Visible = true;
                PremConfirmPassErrorLbl.Text = "PASSWORD DOES NOT MATCH";
                return;
            }
            else
            {
                try
                {
                    using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                    {
                        connection.Open();

                        // Check if email already exists
                        string checkEmailQuery = "SELECT COUNT(*) FROM membershipaccount WHERE EmailAdd = @email";
                        MySqlCommand checkEmailCmd = new MySqlCommand(checkEmailQuery, connection);
                        checkEmailCmd.Parameters.AddWithValue("@email", PremEmailAdd);

                        int emailCount = Convert.ToInt32(checkEmailCmd.ExecuteScalar());

                        if (emailCount > 0)
                        {
                            // Email already exists, show a message or take appropriate action
                            MessageBox.Show("Email already exists. Please use a different email.", "Email Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return; // Exit the method without inserting the new account
                        }

                        // Email doesn't exist, proceed with insertion
                        string insertQuery = "INSERT INTO membershipaccount (MembershipType, MemberIDNumber, AccountStatus, FirstName, " +
                            "LastName, Birthday, Age, CPNumber, EmailAdd, HashedPass, SaltedPass, UserSaltedPass, PlanPeriod, " +
                            "PaymentType, CardholderName, CardNumber, CardExpiration, CVCCode, AccountCreated, PlanExpiration, PlanRenewal, AmountPaid) " +
                            "VALUES (@type, @ID, @status, @firstName, @lastName, @bday, @age, @cpnum, @email, @hashedpass, @saltedpass, @usersaltedpass, " +
                            "@period, @payment, @cardname, @cardnumber, @cardexpiration, @cvc, @created, @planExpiration, @planRenew, @amount)";

                        MySqlCommand cmd = new MySqlCommand(insertQuery, connection);
                        cmd.Parameters.AddWithValue("@type", PremType);
                        cmd.Parameters.AddWithValue("@ID", PremMemberID);
                        cmd.Parameters.AddWithValue("@status", PremStatus);
                        cmd.Parameters.AddWithValue("@firstName", PremFirstname);
                        cmd.Parameters.AddWithValue("@lastName", PremLastname);
                        cmd.Parameters.AddWithValue("@bday", PremBday);
                        cmd.Parameters.AddWithValue("@age", PremAge);
                        cmd.Parameters.AddWithValue("@cpnum", PremNumber);
                        cmd.Parameters.AddWithValue("@email", PremEmailAdd);
                        cmd.Parameters.AddWithValue("@hashedpass", hashedPassword);
                        cmd.Parameters.AddWithValue("@saltedpass", fixedSalt);
                        cmd.Parameters.AddWithValue("@usersaltedpass", perUserSalt);
                        cmd.Parameters.AddWithValue("@period", PremPeriod);
                        cmd.Parameters.AddWithValue("@payment", PremPayment);
                        cmd.Parameters.AddWithValue("@cardname", PremCardName);
                        cmd.Parameters.AddWithValue("@cardnumber", PremCardNum);
                        cmd.Parameters.AddWithValue("@cardexpiration", PremCardExpire);
                        cmd.Parameters.AddWithValue("@cvc", Premcvc);
                        cmd.Parameters.AddWithValue("@created", PremCreated);
                        cmd.Parameters.AddWithValue("@planExpiration", PremPlanExpire);
                        cmd.Parameters.AddWithValue("@planRenew", PremPlanRenew);
                        cmd.Parameters.AddWithValue("@amount", PremAmount);

                        cmd.ExecuteNonQuery();
                    }

                    // Successful insertion
                    MessageBox.Show("Premium Account is successfully created.", "Welcome to Enchanté", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    PremAccIDGenerator();
                    PremMembershipBoxClear();
                    MemberLocationAndColor();
                }
                catch (MySqlException ex)
                {
                    // Handle MySQL database exception
                    MessageBox.Show("MySQL Error: " + ex.Message, "Creating Premium Account Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // No need to close the connection here as it is in a using statement
                }



            }
        }
        private void PremMembershipBoxClear()
        {
            PremFirstNameText.Text = "";
            PremLastNameText.Text = "";
            PremAgeText.Text = "";
            PremGenderComboText.SelectedIndex = -1;
            PremCPNumText.Text = "";
            PremEmailText.Text = "";
            PremMemberIDText.Text = "";
            PremPassText.Text = "";
            PremConfirmPassText.Text = "";
            PremBdayPicker.Value = DateTime.Now;
            PremCardNameText.Text = "";
            PremPlanPeriodText.Text = "";
            PremPaymentTypeText.Text = "";
            PremCardNumText.Text = "";
            PremCardExpireText.Text = "";
            PremCardCVCText.Text = "";
            PremPlanExpirationText.Text = "";
            PremNewPriceText.Text = "";

        }

        //Member Panel Starts Here
        private void MemberAccUserBtn_Click(object sender, EventArgs e)
        {
            if (MemberAccountPanel.Visible == false)
            {
                MemberAccountPanel.Visible = true;

            }
            else
            {
                MemberAccountPanel.Visible = false;
            }
        }

        //Reception Panel Starts Here

        private void ReceptionLogoutBtn_Click(object sender, EventArgs e)
        {
            LogoutChecker();

        }

        private void ReceptionAccBtn_Click(object sender, EventArgs e)
        {
            if (ReceptionAccPanel.Visible == false)
            {
                ReceptionAccPanel.Visible = true;

            }
            else
            {
                ReceptionAccPanel.Visible = false;
            }
        }

        private void RecWalkInBtn_Click(object sender, EventArgs e)
        {
            Transaction.PanelShow(RecWalkinPanel);
            MngrTransactNumRefresh();
        }

        private void RecAppointmentBtn_Click(object sender, EventArgs e)
        {
            Transaction.PanelShow(RecAppointmentPanel);
        }
        public class CashierSessionOrderNumberGenerator
        {
            private static int transactNumber = 1; // Starting order number


            public static string GenerateOrderNumber()
            {
                string datePart = DateTime.Now.ToString("MMddhhmm");

                // Use only the order number
                string orderPart = transactNumber.ToString("D3");

                // Increment the order number for the next order
                transactNumber++;
                string ordersessionNumber = $"W-{datePart}-{orderPart}";

                return ordersessionNumber;
            }
        }

        private void MngrTransactNumRefresh()
        {
            RecWalkinTransNumText.Text = CashierSessionOrderNumberGenerator.GenerateOrderNumber();

        }
        private void RecHomeBtn_Click(object sender, EventArgs e)
        {
            // Scroll to the Home position (0, 0)
            ScrollToCoordinates(0, 0);
            ReceptionHomePanelReset();

            //Change color once clicked
            RecHomeBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(183)))), ((int)(((byte)(97)))));

            //Change back to original
            RecTransactBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));

        }

        private void RecTransactBtn_Click(object sender, EventArgs e)
        {
            //ScrollToCoordinates(0, 0);
            //Change color once clicked
            RecTransactBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(183)))), ((int)(((byte)(97)))));

            //Change back to original
            RecHomeBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
        }

        private void RecHomeBtn_MouseHover(object sender, EventArgs e)
        {
            iconToolTip.SetToolTip(RecHomeBtn, "Home");
        }

        private void RecTransactBtn_MouseHover(object sender, EventArgs e)
        {
            iconToolTip.SetToolTip(RecTransactBtn, "Transaction");
        }

        private void ReceptionAccBtn_MouseHover(object sender, EventArgs e)
        {
            iconToolTip.SetToolTip(ReceptionAccBtn, "Profile");
        }

        //Reception walk in transaction starts here
        private void RecWalkInExitBtn_Click(object sender, EventArgs e)
        {
            Transaction.PanelShow(RecTransactionPanel);
        }

        private void RecInventoryMembershipBtn_Click(object sender, EventArgs e)
        {
            Inventory.PanelShow(MngrInventoryMembershipPanel);

        }

        private void RecInventoryProductsBtn_Click(object sender, EventArgs e)
        {
            Inventory.PanelShow(MngrInventoryProductsPanel);
            MngrInventoryProductData();
        }

        private void RecInventoryServicesBtn_Click_1(object sender, EventArgs e)
        {
            Inventory.PanelShow(MngrInventoryServicesPanel);
            ReceptionLoadServices();

        }

        private void RecInventoryServicesExitBtn_Click(object sender, EventArgs e)
        {
            Inventory.PanelShow(MngrInventoryTypePanel);
            ServiceBoxClear();

        }

        private void RecServicesCategoryComboText_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RecServicesCategoryComboText.SelectedItem != null)
            {
                RecServicesCategoryComboText.Text = RecServicesCategoryComboText.SelectedItem.ToString();
                UpdateServiceTypeComboBox();
                GenerateServiceID();
            }
        }

        private void UpdateServiceTypeComboBox()
        {
            RecServicesTypeComboText.Items.Clear();

            // Get the selected category
            string selectedCategory = RecServicesCategoryComboText.SelectedItem.ToString();

            // Filter and add the relevant service types based on the selected category
            switch (selectedCategory)
            {
                case "Hair Styling":
                    RecServicesTypeComboText.Items.AddRange(new string[] { "Hair Cut", "Hair Blowout", "Hair Color", "Hair Extension", "Package" });
                    break;
                case "Nail Care":
                    RecServicesTypeComboText.Items.AddRange(new string[] { "Manicure", "Pedicure", "Nail Extension", "Nail Art", "Nail Treatment", "Nail Repair", "Package" });
                    break;
                case "Face & Skin":
                    // Add relevant face and skin service types here
                    RecServicesTypeComboText.Items.AddRange(new string[] { "Skin Whitening", "Exfoliation Treatment", "Chemical Peel", "Hydration Treatment", "Acne Treatment", "Anti-Aging Treatment", "Package" });
                    break;
                case "Massage":
                    // Add relevant massage service types here
                    RecServicesTypeComboText.Items.AddRange(new string[] { "Soft Massage", "Moderate Massage", "Hard Massage", "Package" });

                    break;
                case "Spa":
                    // Add relevant spa service types here
                    RecServicesTypeComboText.Items.AddRange(new string[] { "Herbal Pool", "Sauna", "Package" });
                    break;
                default:
                    break;
            }

            // Select the first item in the list
            if (RecServicesTypeComboText.Items.Count > 0)
            {
                RecServicesTypeComboText.SelectedIndex = 0;
            }
        }

        private void RecServicesTypeComboText_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RecServicesTypeComboText.SelectedItem != null)
            {
                RecServicesTypeComboText.Text = RecServicesTypeComboText.SelectedItem.ToString();
                GenerateServiceID();

            }
        }
        public class DynamicIDGenerator
        {
            private static Random random = new Random();

            public static string GenerateServiceID(string selectedCategory, string selectedType)
            {
                // Get the first two characters of the service category
                string categoryCode = selectedCategory.Substring(0, 2).ToUpper();

                // Get the first character of the service type
                char typeCode = selectedType[0];

                // Generate a random 6-digit number
                string randomPart = GenerateRandomNumber();

                // Format the ServiceID
                string serviceID = $"{categoryCode}-{typeCode}-{randomPart:D6}";

                return serviceID;
            }

            private static string GenerateRandomNumber()
            {
                // Generate a random 6-digit number
                int randomNumber = random.Next(100000, 999999);

                return randomNumber.ToString();
            }
        }


        private void GenerateServiceID()
        {
            if (RecServicesCategoryComboText.SelectedIndex >= 0 && RecServicesTypeComboText.SelectedIndex >= 0)
            {
                // Get the selected items from both combo boxes
                string selectedCategory = RecServicesCategoryComboText.SelectedItem.ToString();
                string selectedType = RecServicesTypeComboText.SelectedItem.ToString();

                // Call the GenerateServiceID method
                string generatedServiceID = DynamicIDGenerator.GenerateServiceID(selectedCategory, selectedType);

                // Update your UI element with the generated ID
                RecServicesIDNumText.Text = generatedServiceID;
            }
        }

        private void RecServicesCreateBtn_Click(object sender, EventArgs e)
        {
            string category = RecServicesCategoryComboText.Text;
            string type = RecServicesTypeComboText.Text;
            string name = RecServicesNameText.Text;
            string describe = RecServicesDescriptionText.Text;
            string duration = RecServicesDurationText.Text;
            string price = RecServicesPriceText.Text;
            string ID = RecServicesIDNumText.Text;

            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(type) && string.IsNullOrEmpty(category) && string.IsNullOrEmpty(describe)
                && string.IsNullOrEmpty(duration) && string.IsNullOrEmpty(price))
            {
                MessageBox.Show("Missing text on required fields.", "Missing Text", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(type) || string.IsNullOrEmpty(category) || string.IsNullOrEmpty(describe)
                || string.IsNullOrEmpty(duration) || string.IsNullOrEmpty(price))
            {
                MessageBox.Show("Missing text on required fields.", "Missing Text", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {

                try
                {
                    using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                    {
                        connection.Open();
                        // Check if email already exists
                        string checkIDQuery = "SELECT COUNT(*) FROM services WHERE ServiceID = @ID";
                        MySqlCommand checkIDCmd = new MySqlCommand(checkIDQuery, connection);
                        checkIDCmd.Parameters.AddWithValue("@ID", ID);

                        int ID_Count = Convert.ToInt32(checkIDCmd.ExecuteScalar());

                        if (ID_Count > 0)
                        {
                            // Email already exists, show a message or take appropriate action
                            MessageBox.Show("Service ID already exists. Please use a different ID Number.", "Salon Service Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return; // Exit the method without inserting the new account
                        }
                        string insertQuery = "INSERT INTO services (Category, Type, ServiceID, Name, Description, Duration, Price)" +
                            "VALUES (@category, @type, @ID, @name, @describe, @duration, @price)";

                        MySqlCommand cmd = new MySqlCommand(insertQuery, connection);
                        cmd.Parameters.AddWithValue("@category", category);
                        cmd.Parameters.AddWithValue("@type", type);
                        cmd.Parameters.AddWithValue("@ID", ID);
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@describe", describe);
                        cmd.Parameters.AddWithValue("@duration", duration);
                        cmd.Parameters.AddWithValue("@price", price);

                        cmd.ExecuteNonQuery();
                    }

                    // Successful insertion
                    MessageBox.Show("Salon service is successfully created.", "Enchanté Service", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ServiceBoxClear();
                    ReceptionLoadServices();
                    GenerateServiceID();


                }
                catch (MySqlException ex)
                {
                    // Handle MySQL database exception
                    MessageBox.Show("MySQL Error: " + ex.Message, "Creating Enchanté Service Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // Make sure to close the connection
                    connection.Close();
                }
            }

        }
        private void ServiceBoxClear()
        {
            RecServicesCreateBtn.Visible = true;
            RecServicesUpdateBtn.Visible = false;
            RecServicesCategoryComboText.Enabled = true;
            RecServicesTypeComboText.Enabled = true;
            RecServicesCategoryComboText.SelectedIndex = -1;
            RecServicesTypeComboText.SelectedIndex = -1;
            RecServicesCategoryComboText.Text = "";
            RecServicesTypeComboText.Text = "";
            RecServicesNameText.Text = "";
            RecServicesDescriptionText.Text = "";
            RecServicesDurationText.Text = "";
            RecServicesPriceText.Text = "";
            RecServicesIDNumText.Text = "";

        }

        private void RecServicesUpdateInfoBtn_Click(object sender, EventArgs e)
        {
            if (RecInventoryServicesTable.SelectedRows.Count > 0)
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to edit the selected data?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialogResult == DialogResult.Yes)
                {
                    // Iterate through selected rows in PendingTable
                    foreach (DataGridViewRow selectedRow in RecInventoryServicesTable.SelectedRows)
                    {
                        try
                        {
                            //// Re data into the database
                            RetrieveServiceDataFromDB(selectedRow);
                            RecServicesUpdateBtn.Visible = true;
                            RecServicesCreateBtn.Visible = false;
                            RecServicesCategoryComboText.Enabled = false;
                            RecServicesTypeComboText.Enabled = false;
                        }
                        catch (Exception ex)
                        {
                            // Handle any database-related errors here
                            MessageBox.Show("Error: " + ex.Message);
                        }
                    }


                }
                else if (dialogResult == DialogResult.No)
                {

                }
            }
            else
            {
                MessageBox.Show("Select a table row first.", "Ooooops!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
        }
        private void RetrieveServiceDataFromDB(DataGridViewRow selectedRow)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                {
                    connection.Open();

                    string ID = selectedRow.Cells[2].Value.ToString();

                    string selectQuery = "SELECT * FROM services WHERE ServiceID = @ID";
                    MySqlCommand selectCmd = new MySqlCommand(selectQuery, connection);
                    selectCmd.Parameters.AddWithValue("@ID", ID);

                    using (MySqlDataReader reader = selectCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string serviceCategory = reader["Category"].ToString();
                            string serviceType = reader["Type"].ToString();
                            string serviceID = reader["ServiceID"].ToString();
                            string serviceName = reader["Name"].ToString();
                            string serviceDescribe = reader["Description"].ToString();
                            string serviceDuration = reader["Duration"].ToString();
                            string servicePrice = reader["Price"].ToString();

                            RecServicesCategoryComboText.Text = serviceCategory;
                            RecServicesTypeComboText.Text = serviceType;
                            RecServicesIDNumText.Text = serviceID;
                            RecServicesNameText.Text = serviceName;
                            RecServicesDescriptionText.Text = serviceDescribe;
                            RecServicesDurationText.Text = serviceDuration;
                            RecServicesPriceText.Text = servicePrice;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Retrieving Food Item Data Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            finally
            {
                connection.Close();
            }
        }


        private void RecServicesUpdateBtn_Click(object sender, EventArgs e)
        {
            string category = RecServicesCategoryComboText.Text;
            string type = RecServicesTypeComboText.Text;
            string name = RecServicesNameText.Text;
            string describe = RecServicesDescriptionText.Text;
            string duration = RecServicesDurationText.Text;
            string price = RecServicesPriceText.Text;
            string ID = RecServicesIDNumText.Text;

            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(type) && string.IsNullOrEmpty(category) && string.IsNullOrEmpty(describe)
                && string.IsNullOrEmpty(duration) && string.IsNullOrEmpty(price))
            {
                MessageBox.Show("Missing text on required fields.", "Missing Text", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(type) || string.IsNullOrEmpty(category) || string.IsNullOrEmpty(describe)
                || string.IsNullOrEmpty(duration) || string.IsNullOrEmpty(price))
            {
                MessageBox.Show("Missing text on required fields.", "Missing Text", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                try
                {
                    using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                    {
                        connection.Open();

                        // Check if the employee with the given Employee ID exists
                        string checkExistQuery = "SELECT COUNT(*) FROM services WHERE ServiceID = @ID";
                        MySqlCommand checkExistCmd = new MySqlCommand(checkExistQuery, connection);
                        checkExistCmd.Parameters.AddWithValue("@ID", ID);
                        int serviceCount = Convert.ToInt32(checkExistCmd.ExecuteScalar());

                        if (serviceCount == 0)
                        {
                            MessageBox.Show("Service with the provided ID does not exist in the database.", "Service Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }


                        // Update without image
                        string updateQuery = "UPDATE services SET Category = @category, Type = @type, Name = @name, Description = @describe, Duration = @duration, Price = @price " +
                            "WHERE ServiceID = @ID";
                        MySqlCommand updateCmd = new MySqlCommand(updateQuery, connection);
                        updateCmd.Parameters.AddWithValue("@category", category);
                        updateCmd.Parameters.AddWithValue("@type", type);
                        updateCmd.Parameters.AddWithValue("@ID", ID);
                        updateCmd.Parameters.AddWithValue("@name", name);
                        updateCmd.Parameters.AddWithValue("@describe", describe);
                        updateCmd.Parameters.AddWithValue("@duration", duration);
                        updateCmd.Parameters.AddWithValue("@price", price);

                        updateCmd.ExecuteNonQuery();

                    }

                    // Successful update
                    MessageBox.Show("Service information has been successfully updated.", "Service Info Update", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    ServiceBoxClear();
                    ReceptionLoadServices();


                }
                catch (MySqlException ex)
                {
                    // Handle MySQL database exception
                    MessageBox.Show("MySQL Error: " + ex.Message, "Updating Service Information Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }

        }

        private void RecWalkInCatHSBtn_Click(object sender, EventArgs e)
        {
            //if (!IsPrefferredTimeSchedComboBoxModified && RecAppPrefferedTimePMComboBox.SelectedIndex == 0 && RecAppPrefferedTimeAMComboBox.SelectedIndex == 0)
            //{
            //    return;
            //}
            //if (RecAppPrefferedTimePMComboBox.SelectedIndex == 0 && RecAppPrefferedTimeAMComboBox.SelectedIndex == 0)
            //{
            //    MessageBox.Show("Please Select a prefferred time first");
            //}
            //else
            //{
            //    FilterAvailableStaffInRecFlowLayoutPanelByHairStyling();
            //}
            HairStyle();

        }

        private void RecWalkInCatFSBtn_Click(object sender, EventArgs e)
        {
            //if (!IsPrefferredTimeSchedComboBoxModified && RecAppPrefferedTimePMComboBox.SelectedIndex == 0 && RecAppPrefferedTimeAMComboBox.SelectedIndex == 0)
            //{
            //    return;
            //}
            //if (RecAppPrefferedTimePMComboBox.SelectedIndex == 0 && RecAppPrefferedTimeAMComboBox.SelectedIndex == 0)
            //{
            //    MessageBox.Show("Please Select a prefferred time first");
            //}
            //else
            //{
            //    FilterAvailableStaffInRecFlowLayoutPanelByFaceandSkin();
            //}
            Face();

        }

        private void RecWalkInCatNCBtn_Click(object sender, EventArgs e)
        {
            //if (!IsPrefferredTimeSchedComboBoxModified && RecAppPrefferedTimePMComboBox.SelectedIndex == 0 && RecAppPrefferedTimeAMComboBox.SelectedIndex == 0)
            //{
            //    return;
            //}
            //if (RecAppPrefferedTimePMComboBox.SelectedIndex == 0 && RecAppPrefferedTimeAMComboBox.SelectedIndex == 0)
            //{
            //    MessageBox.Show("Please Select a prefferred time first");
            //}
            //else
            //{
            //    FilterAvailableStaffInRecFlowLayoutPanelByNailCare();
            //}
            Nail();

        }

        private void RecWalkInCatSpaBtn_Click(object sender, EventArgs e)
        {
            //if (!IsPrefferredTimeSchedComboBoxModified && RecAppPrefferedTimePMComboBox.SelectedIndex == 0 && RecAppPrefferedTimeAMComboBox.SelectedIndex == 0)
            //{
            //    return;
            //}
            //if (RecAppPrefferedTimePMComboBox.SelectedIndex == 0 && RecAppPrefferedTimeAMComboBox.SelectedIndex == 0)
            //{
            //    MessageBox.Show("Please Select a prefferred time first");
            //}
            //else
            //{
            //    FilterAvailableStaffInRecFlowLayoutPanelBySpa();
            //}
            Spa();

        }

        private void RecWalkInCatMassageBtn_Click(object sender, EventArgs e)
        {
            //if (!IsPrefferredTimeSchedComboBoxModified && RecAppPrefferedTimePMComboBox.SelectedIndex == 0 && RecAppPrefferedTimeAMComboBox.SelectedIndex == 0)
            //{
            //    return;
            //}
            //if (RecAppPrefferedTimePMComboBox.SelectedIndex == 0 && RecAppPrefferedTimeAMComboBox.SelectedIndex == 0)
            //{
            //    MessageBox.Show("Please Select a prefferred time first");
            //}
            //else
            //{
            //    FilterAvailableStaffInRecFlowLayoutPanelByMassage();
            //}
            Massage();

        }
        private void HairStyle()
        {
            if (RecWalkinCatHSRB.Checked == false)
            {
                RecWalkinCatHSRB.Visible = true;
                RecWalkinCatHSRB.Checked = true;
                LoadServiceTypeComboBox("Hair Styling");

                RecWalkinCatFSRB.Visible = false;
                RecWalkinCatNCRB.Visible = false;
                RecWalkinCatSpaRB.Visible = false;
                RecWalkinCatMassageRB.Visible = false;
                RecWalkinCatFSRB.Checked = false;
                RecWalkinCatNCRB.Checked = false;
                RecWalkinCatSpaRB.Checked = false;
                RecWalkinCatMassageRB.Checked = false;
                return;
            }
            else if (RecWalkinCatHSRB.Checked == true)
            {
                RecWalkinCatHSRB.Visible = true;
                RecWalkinCatHSRB.Checked = true;
                LoadServiceTypeComboBox("Hair Styling");

                RecWalkinCatFSRB.Visible = false;
                RecWalkinCatNCRB.Visible = false;
                RecWalkinCatSpaRB.Visible = false;
                RecWalkinCatMassageRB.Visible = false;
                RecWalkinCatFSRB.Checked = false;
                RecWalkinCatNCRB.Checked = false;
                RecWalkinCatSpaRB.Checked = false;
                RecWalkinCatMassageRB.Checked = false;
            }
        }
        private void Face()
        {
            if (RecWalkinCatFSRB.Checked == false)
            {
                RecWalkinCatFSRB.Visible = true;
                RecWalkinCatFSRB.Checked = true;
                LoadServiceTypeComboBox("Face & Skin");

                RecWalkinCatHSRB.Visible = false;
                RecWalkinCatNCRB.Visible = false;
                RecWalkinCatSpaRB.Visible = false;
                RecWalkinCatMassageRB.Visible = false;
                RecWalkinCatHSRB.Checked = false;
                RecWalkinCatNCRB.Checked = false;
                RecWalkinCatSpaRB.Checked = false;
                RecWalkinCatMassageRB.Checked = false;
                return;
            }
            else if (RecWalkinCatFSRB.Checked == true)
            {
                RecWalkinCatFSRB.Visible = true;
                RecWalkinCatFSRB.Checked = true;
            }
        }
        private void Nail()
        {
            if (RecWalkinCatNCRB.Checked == false)
            {
                RecWalkinCatNCRB.Visible = true;
                RecWalkinCatNCRB.Checked = true;
                LoadServiceTypeComboBox("Nail Care");

                RecWalkinCatHSRB.Visible = false;
                RecWalkinCatFSRB.Visible = false;
                RecWalkinCatSpaRB.Visible = false;
                RecWalkinCatMassageRB.Visible = false;
                RecWalkinCatHSRB.Checked = false;
                RecWalkinCatFSRB.Checked = false;
                RecWalkinCatSpaRB.Checked = false;
                RecWalkinCatMassageRB.Checked = false;
                return;
            }
            else if (RecWalkinCatNCRB.Checked == true)
            {
                RecWalkinCatNCRB.Visible = true;
                RecWalkinCatNCRB.Checked = true;
            }
        }
        private void Spa()
        {
            if (RecWalkinCatSpaRB.Checked == false)
            {
                RecWalkinCatSpaRB.Visible = true;
                RecWalkinCatSpaRB.Checked = true;
                LoadServiceTypeComboBox("Spa");

                RecWalkinCatHSRB.Visible = false;
                RecWalkinCatFSRB.Visible = false;
                RecWalkinCatNCRB.Visible = false;
                RecWalkinCatMassageRB.Visible = false;
                RecWalkinCatHSRB.Checked = false;
                RecWalkinCatFSRB.Checked = false;
                RecWalkinCatNCRB.Checked = false;
                RecWalkinCatMassageRB.Checked = false;
                return;
            }
            else if (RecWalkinCatSpaRB.Checked == true)
            {
                RecWalkinCatSpaRB.Visible = true;
                RecWalkinCatSpaRB.Checked = true;
            }
        }
        private void Massage()
        {
            if (RecWalkinCatMassageRB.Checked == false)
            {
                RecWalkinCatMassageRB.Visible = true;
                RecWalkinCatMassageRB.Checked = true;
                LoadServiceTypeComboBox("Massage");

                RecWalkinCatHSRB.Visible = false;
                RecWalkinCatFSRB.Visible = false;
                RecWalkinCatNCRB.Visible = false;
                RecWalkinCatSpaRB.Visible = false;
                RecWalkinCatHSRB.Checked = false;
                RecWalkinCatFSRB.Checked = false;
                RecWalkinCatNCRB.Checked = false;
                RecWalkinCatSpaRB.Checked = false;
                return;
            }
            else if (RecWalkinCatMassageRB.Checked == true)
            {
                RecWalkinCatMassageRB.Visible = true;
                RecWalkinCatMassageRB.Checked = true;
            }
        }
        public void ReceptionLoadHairStyleType()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                {
                    connection.Open();

                    // Filter and sort the data by FoodType
                    string sql = "SELECT * FROM `services` WHERE Category = 'Hair Styling' ORDER BY Category";
                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    System.Data.DataTable dataTable = new System.Data.DataTable();

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);

                        RecWalkInServiceTypeTable.Columns.Clear();


                        RecWalkInServiceTypeTable.DataSource = dataTable;

                        RecWalkInServiceTypeTable.Columns[0].Visible = false; //service category
                        RecWalkInServiceTypeTable.Columns[1].Visible = false; // service type
                        RecWalkInServiceTypeTable.Columns[2].Visible = false; // service ID
                        RecWalkInServiceTypeTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        RecWalkInServiceTypeTable.ClearSelection();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred: " + e.Message, "Cashier Burger Item List");
            }
            finally
            {
                connection.Close();
            }
        }
        public void ReceptionFaceSkinType()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                {
                    connection.Open();

                    // Filter and sort the data by FoodType
                    string sql = "SELECT * FROM `services` WHERE Category = 'Face & Skin' ORDER BY Category";
                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    System.Data.DataTable dataTable = new System.Data.DataTable();

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);

                        RecWalkInServiceTypeTable.Columns.Clear();


                        RecWalkInServiceTypeTable.DataSource = dataTable;

                        RecWalkInServiceTypeTable.Columns[0].Visible = false;
                        RecWalkInServiceTypeTable.Columns[1].Visible = false;
                        RecWalkInServiceTypeTable.Columns[2].Visible = false;
                        RecWalkInServiceTypeTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        RecWalkInServiceTypeTable.ClearSelection();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred: " + e.Message, "Cashier Burger Item List");
            }
            finally
            {
                connection.Close();
            }
        }
        public void ReceptionNailCareType()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                {
                    connection.Open();

                    // Filter and sort the data by FoodType
                    string sql = "SELECT * FROM `services` WHERE Category = 'Nail Care' ORDER BY Category";
                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    System.Data.DataTable dataTable = new System.Data.DataTable();

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);

                        RecWalkInServiceTypeTable.Columns.Clear();


                        RecWalkInServiceTypeTable.DataSource = dataTable;

                        RecWalkInServiceTypeTable.Columns[0].Visible = false;
                        RecWalkInServiceTypeTable.Columns[1].Visible = false;
                        RecWalkInServiceTypeTable.Columns[2].Visible = false;
                        RecWalkInServiceTypeTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        RecWalkInServiceTypeTable.ClearSelection();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred: " + e.Message, "Cashier Burger Item List");
            }
            finally
            {
                connection.Close();
            }
        }
        public void ReceptionSpaType()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                {
                    connection.Open();

                    // Filter and sort the data by FoodType
                    string sql = "SELECT * FROM `services` WHERE Category = 'Spa' ORDER BY Category";
                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    System.Data.DataTable dataTable = new System.Data.DataTable();

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);

                        RecWalkInServiceTypeTable.Columns.Clear();


                        RecWalkInServiceTypeTable.DataSource = dataTable;

                        RecWalkInServiceTypeTable.Columns[0].Visible = false;
                        RecWalkInServiceTypeTable.Columns[1].Visible = false;
                        RecWalkInServiceTypeTable.Columns[2].Visible = false;
                        RecWalkInServiceTypeTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        RecWalkInServiceTypeTable.ClearSelection();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred: " + e.Message, "Cashier Burger Item List");
            }
            finally
            {
                connection.Close();
            }
        }
        public void ReceptionMassageType()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                {
                    connection.Open();

                    // Filter and sort the data by FoodType
                    string sql = "SELECT * FROM `services` WHERE Category = 'Massage' ORDER BY Category";
                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    System.Data.DataTable dataTable = new System.Data.DataTable();

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);

                        RecWalkInServiceTypeTable.Columns.Clear();


                        RecWalkInServiceTypeTable.DataSource = dataTable;

                        RecWalkInServiceTypeTable.Columns[0].Visible = false;
                        RecWalkInServiceTypeTable.Columns[1].Visible = false;
                        RecWalkInServiceTypeTable.Columns[2].Visible = false;
                        RecWalkInServiceTypeTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        RecWalkInServiceTypeTable.ClearSelection();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred: " + e.Message, "Cashier Burger Item List");
            }
            finally
            {
                connection.Close();
            }
        }
        private void LoadServiceTypeComboBox(string selectedCategory)
        {
            // Filter and add the relevant service types based on the selected category
            switch (selectedCategory)
            {
                case "Hair Styling":
                    ReceptionLoadHairStyleType();
                    break;
                case "Nail Care":
                    ReceptionNailCareType();
                    break;
                case "Face & Skin":
                    ReceptionFaceSkinType();
                    break;
                case "Massage":
                    ReceptionMassageType();
                    break;
                case "Spa":
                    ReceptionSpaType();
                    break;
                default:
                    break;
            }

        }
        private void SearchAcrossCategories(string searchText)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                {
                    connection.Open();

                    // Modify the query to search for the specified text in all categories
                    string sql = "SELECT * FROM `services` WHERE " +
                                 "(Name LIKE @searchText OR " +
                                 "Description LIKE @searchText OR " +
                                 "Duration LIKE @searchText OR " +
                                 "Price LIKE @searchText)";

                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("@searchText", "%" + searchText + "%");

                    System.Data.DataTable dataTable = new System.Data.DataTable();

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);

                        RecWalkInServiceTypeTable.Columns.Clear();

                        RecWalkInServiceTypeTable.DataSource = dataTable;

                        // Adjust column visibility and sizing as needed
                        RecWalkInServiceTypeTable.Columns[0].Visible = false;
                        RecWalkInServiceTypeTable.Columns[1].Visible = false;
                        RecWalkInServiceTypeTable.Columns[2].Visible = false;
                        RecWalkInServiceTypeTable.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        RecWalkInServiceTypeTable.ClearSelection();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred: " + e.Message, "Error");
            }
            finally
            {
                connection.Close();
            }
        }


        private void RecWalkInSearchServiceTypeText_TextChanged(object sender, EventArgs e)
        {
            string searchText = RecWalkinSearchServiceTypeText.Text;
            SearchAcrossCategories(searchText);
        }

        private void RecWalkInSearchServiceTypeBtn_Click(object sender, EventArgs e)
        {
            string searchText = RecWalkinSearchServiceTypeText.Text;
            SearchAcrossCategories(searchText);
        }

        private void AdminSignOutBtn_Click_1(object sender, EventArgs e)
        {
            LogoutChecker();

        }

        private void AdminAccUserBtn_Click(object sender, EventArgs e)
        {
            if (AdminAccUserPanel.Visible == false)
            {
                AdminAccUserPanel.Visible = true;

            }
            else
            {
                AdminAccUserPanel.Visible = false;
            }
        }

        private void AdminBdayPicker_ValueChanged(object sender, EventArgs e)
        {
            DateTime selectedDate = AdminBdayPicker.Value;
            int age = DateTime.Now.Year - selectedDate.Year;

            if (DateTime.Now < selectedDate.AddYears(age))
            {
                age--; // Subtract 1 if the birthday hasn't occurred yet this year
            }
            AdminAgeText.Text = age.ToString();
            if (age < 18)
            {
                AdminAgeErrorLbl.Visible = true;
                AdminAgeErrorLbl.Text = "Must be 18 years old and above";
                return;
            }
            else
            {
                AdminAgeErrorLbl.Visible = false;

            }
        }
        private string selectedHashedPerUser;

        private void AdminEditAccBtn_Click(object sender, EventArgs e)
        {
            DateTime selectedDate = RegularBdayPicker.Value;
            DateTime currentDate = DateTime.Now;

            string fname = AdminFirstNameText.Text;
            string lname = AdminLastNameText.Text;
            string bday = selectedDate.ToString("MM-dd-yyyy");
            string age = AdminAgeText.Text;
            string gender = AdminGenderComboText.Text;
            string cpnum = AdminCPNumText.Text;
            string emplType = AdminEmplTypeComboText.Text;
            string emplCat = AdminEmplCatComboText.Text;
            string emplCatLvl = AdminEmplCatLvlComboText.Text;
            string emplID = AdminEmplIDText.Text;
            string email = AdminEmailText.Text;
            string pass = AdminPassText.Text;
            string confirm = AdminConfirmPassText.Text;

            string hashedPassword = HashHelper.HashString(pass);    // Password hashed
            string fixedSalt = HashHelper_Salt.HashString_Salt("Enchante" + pass + "2024");    //Fixed Salt
            string perUserSalt = HashHelper_SaltperUser.HashString_SaltperUser(pass + emplID);    //Per User salt

            if (AdminAccountTable.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = AdminAccountTable.SelectedRows[0];

                bool rowIsEmpty = true;
                foreach (DataGridViewCell cell in selectedRow.Cells)
                {
                    if (!string.IsNullOrEmpty(cell.Value?.ToString()))
                    {
                        rowIsEmpty = false;
                        break;
                    }
                }

                if (rowIsEmpty)
                {
                    MessageBox.Show("The selected row is empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                AdminFirstNameText.Text = selectedRow.Cells["FirstName"].Value?.ToString();
                AdminLastNameText.Text = selectedRow.Cells["LastName"].Value?.ToString();
                AdminEmailText.Text = selectedRow.Cells["Email"].Value?.ToString();
                AdminAgeText.Text = selectedRow.Cells["Age"].Value?.ToString();
                AdminGenderComboText.SelectedItem = selectedRow.Cells["Gender"].Value?.ToString();
                AdminCPNumText.Text = selectedRow.Cells["PhoneNumber"].Value?.ToString();
                AdminEmplTypeComboText.SelectedItem = selectedRow.Cells["EmployeeType"].Value?.ToString();
                AdminEmplCatComboText.SelectedItem = selectedRow.Cells["EmployeeCategory"].Value?.ToString();
                AdminEmplCatLvlComboText.SelectedItem = selectedRow.Cells["EmployeeCategoryLevel"].Value?.ToString();
                AdminEmplIDText.Text = selectedRow.Cells["EmployeeID"].Value?.ToString();

                string birthdayString = selectedRow.Cells["Birthday"].Value?.ToString() ?? string.Empty;
                DateTime birthday;
                if (!string.IsNullOrEmpty(birthdayString) && DateTime.TryParseExact(birthdayString, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out birthday))
                {
                    AdminBdayPicker.Value = birthday.Date;
                }
                else if (string.IsNullOrEmpty(birthdayString))
                {
                    AdminBdayPicker.Value = DateTime.Today;
                }
                else
                {
                    MessageBox.Show("Invalid date format in the 'Birthday' column.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                selectedHashedPerUser = selectedRow.Cells["HashedPerUser"].Value?.ToString();
                AdminEmplTypeComboText.Enabled = false;
                AdminEmplCatComboText.Enabled = false;
                AdminCreateAccBtn.Visible = false;
                AdminUpdateAccBtn.Visible = true;

            }
            else
            {
                MessageBox.Show("Please select a row first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AdminEmplTypeComboText_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AdminEmplTypeComboText.SelectedItem != null)
            {
                string selectedEmpType = AdminEmplTypeComboText.SelectedItem?.ToString() ?? string.Empty;

                if (selectedEmpType == "Admin" || selectedEmpType == "Manager")
                {
                    AdminEmplCatComboText.SelectedIndex = AdminEmplCatComboText.Items.IndexOf("Not Applicable");
                    AdminEmplCatLvlComboText.SelectedIndex = AdminEmplCatLvlComboText.Items.IndexOf("Not Applicable");
                    AdminEmplCatComboText.Enabled = false;
                    AdminEmplCatLvlComboText.Enabled = false;
                    AdminGenerateID();
                }
                else if (selectedEmpType == "Staff")
                {
                    AdminEmplCatComboText.SelectedIndex = -1;
                    AdminEmplCatLvlComboText.SelectedIndex = -1;
                    AdminEmplCatComboText.Enabled = true;
                    AdminEmplCatLvlComboText.Enabled = true;
                    AdminGenerateID();
                }
            }
        }

        private void AdminEmplCatLvlComboText_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AdminEmplCatLvlComboText.SelectedItem != null)
            {
                AdminEmplCatLvlComboText.Text = AdminEmplCatLvlComboText.SelectedItem.ToString();
                AdminGenerateID();
            }
        }

        private void AdminEmplCatComboText_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AdminEmplCatComboText.SelectedItem != null)
            {
                AdminEmplCatComboText.Text = AdminEmplCatComboText.SelectedItem.ToString();
                AdminGenerateID();
            }
        }

        private void AdminShowHidePassBtn_Click(object sender, EventArgs e)
        {
            if (AdminPassText.UseSystemPasswordChar == true)
            {
                AdminPassText.UseSystemPasswordChar = false;
                AdminShowHidePassBtn.IconChar = FontAwesome.Sharp.IconChar.EyeSlash;
            }
            else if (AdminPassText.UseSystemPasswordChar == false)
            {
                AdminPassText.UseSystemPasswordChar = true;
                AdminShowHidePassBtn.IconChar = FontAwesome.Sharp.IconChar.Eye;

            }
        }

        private void AdminShowHideConfirmPassBtn_Click(object sender, EventArgs e)
        {
            if (AdminConfirmPassText.UseSystemPasswordChar == true)
            {
                AdminConfirmPassText.UseSystemPasswordChar = false;
                AdminShowHideConfirmPassBtn.IconChar = FontAwesome.Sharp.IconChar.EyeSlash;
            }
            else if (AdminConfirmPassText.UseSystemPasswordChar == false)
            {
                AdminConfirmPassText.UseSystemPasswordChar = true;
                AdminShowHideConfirmPassBtn.IconChar = FontAwesome.Sharp.IconChar.Eye;

            }
        }

        private void AdminConfirmPassText_TextChanged(object sender, EventArgs e)
        {
            if (AdminConfirmPassText.Text != AdminPassText.Text)
            {
                AdminConfirmPassErrorLbl.Visible = true;
                AdminConfirmPassErrorLbl.Text = "PASSWORD DOES NOT MATCH";
            }
            else
            {
                AdminConfirmPassErrorLbl.Visible = false;
            }
        }
        private bool ContainsNumbers(string input)
        {
            return input.Any(char.IsDigit);
        }
        private bool IsNumeric(string input)
        {
            foreach (char c in input)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }
        private void AdminCreateAccBtn_Click(object sender, EventArgs e)
        {
            DateTime selectedDate = RegularBdayPicker.Value;
            DateTime currentDate = DateTime.Now;

            string fname = AdminFirstNameText.Text;
            string lname = AdminLastNameText.Text;
            string bday = selectedDate.ToString("MM-dd-yyyy");
            string age = AdminAgeText.Text;
            string gender = AdminGenderComboText.Text;
            string cpnum = AdminCPNumText.Text;
            string emplType = AdminEmplTypeComboText.Text;
            string emplCat = AdminEmplCatComboText.Text;
            string emplCatLvl = AdminEmplCatLvlComboText.Text;
            string emplID = AdminEmplIDText.Text;
            string email = AdminEmailText.Text;
            string pass = AdminPassText.Text;
            string confirm = AdminConfirmPassText.Text;

            string hashedPassword = HashHelper.HashString(pass);    // Password hashed
            string fixedSalt = HashHelper_Salt.HashString_Salt("Enchante" + pass + "2024");    //Fixed Salt
            string perUserSalt = HashHelper_SaltperUser.HashString_SaltperUser(pass + emplID);    //Per User salt


            if (string.IsNullOrWhiteSpace(fname) || string.IsNullOrWhiteSpace(lname) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(age) ||
               string.IsNullOrWhiteSpace(cpnum) || string.IsNullOrWhiteSpace(emplID) || string.IsNullOrWhiteSpace(pass) || string.IsNullOrWhiteSpace(confirm) ||
               AdminBdayPicker.Value == null || AdminGenderComboText.SelectedItem == null || AdminEmplTypeComboText.SelectedItem == null || AdminEmplCatComboText.SelectedItem == null || AdminEmplCatLvlComboText.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (ContainsNumbers(fname))
            {
                MessageBox.Show("First Name should not contain numbers.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (ContainsNumbers(lname))
            {
                MessageBox.Show("Last Name should not contain numbers.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!email.Contains("@") || !email.Contains(".com"))
            {
                MessageBox.Show("Invalid email format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!IsNumeric(age))
            {
                MessageBox.Show("Invalid Age.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!IsNumeric(cpnum))
            {
                MessageBox.Show("Invalid Phone Number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (pass != confirm)
            {
                MessageBox.Show("Passwords do not match.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                try
                {
                    using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                    {
                        connection.Open();

                        string query = "INSERT INTO systemusers (FirstName, LastName, Email, Birthday, Age, Gender, PhoneNumber, EmployeeType, EmployeeCategory, EmployeeCategoryLevel, EmployeeID, HashedPass, HashedFixedSalt, HashedPerUser) " +
                                       "VALUES (@FirstName, @LastName, @Email, @Birthday, @Age, @Gender, @PhoneNumber, @EmployeeType, @EmployeeCategory, @EmployeeCategoryLevel, @EmployeeID, @HashedPass, @HashedFixedSalt, @HashedPerUser)";

                        MySqlCommand command = new MySqlCommand(query, connection);
                        command.Parameters.AddWithValue("@FirstName", fname);
                        command.Parameters.AddWithValue("@LastName", lname);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Birthday", bday);
                        command.Parameters.AddWithValue("@Age", int.Parse(age));
                        command.Parameters.AddWithValue("@Gender", gender);
                        command.Parameters.AddWithValue("@PhoneNumber", cpnum);
                        command.Parameters.AddWithValue("@EmployeeType", emplType);
                        command.Parameters.AddWithValue("@EmployeeCategory", emplCat);
                        command.Parameters.AddWithValue("@EmployeeCategoryLevel", emplCatLvl);
                        command.Parameters.AddWithValue("@EmployeeID", emplID);
                        command.Parameters.AddWithValue("@HashedPass", hashedPassword);
                        command.Parameters.AddWithValue("@HashedFixedSalt", fixedSalt);
                        command.Parameters.AddWithValue("@HashedPerUser", perUserSalt);

                        command.ExecuteNonQuery();

                        MessageBox.Show("Registered Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        PopulateUserInfoDataGrid();
                        AdminClearFields();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void AdminClearFields()
        {
            AdminFirstNameText.Text = "";
            AdminLastNameText.Text = "";
            AdminBdayPicker.Value = DateTime.Now;
            AdminAgeText.Text = "";
            AdminGenderComboText.Text = "";
            AdminCPNumText.Text = "";
            AdminEmplTypeComboText.Text = "";
            AdminEmplCatComboText.Text = "";
            AdminEmplCatLvlComboText.Text = "";
            AdminEmplIDText.Text = "";
            AdminPassText.Text = "";
            AdminConfirmPassText.Text = "";
        }
        private void AdminUpdateAccBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(AdminFirstNameText.Text) || string.IsNullOrWhiteSpace(AdminLastNameText.Text) || string.IsNullOrWhiteSpace(AdminEmailText.Text) || string.IsNullOrWhiteSpace(AdminAgeText.Text) ||
    string.IsNullOrWhiteSpace(AdminCPNumText.Text) || string.IsNullOrWhiteSpace(AdminEmplIDText.Text) || AdminBdayPicker.Value == null || AdminGenderComboText.SelectedItem == null || AdminEmplTypeComboText.SelectedItem == null ||
    AdminEmplCatComboText.SelectedItem == null || AdminEmplCatLvlComboText.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (ContainsNumbers(AdminFirstNameText.Text))
            {
                MessageBox.Show("First Name should not contain numbers.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (ContainsNumbers(AdminLastNameText.Text))
            {
                MessageBox.Show("Last Name should not contain numbers.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!AdminEmailText.Text.Contains("@") || !AdminEmailText.Text.Contains(".com"))
            {
                MessageBox.Show("Invalid email format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!IsNumeric(AdminAgeText.Text))
            {
                MessageBox.Show("Invalid Age.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!IsNumeric(AdminCPNumText.Text))
            {
                MessageBox.Show("Invalid Phone Number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (selectedHashedPerUser != null)
            {
                try
                {
                    bool fieldsChanged = false;
                    string selectQuery = "SELECT FirstName, LastName, Email, Birthday, Age, Gender, PhoneNumber, EmployeeType, EmployeeCategory, EmployeeCategoryLevel, EmployeeID FROM systemusers WHERE HashedPerUser = @HashedPerUser";
                    string query = @"UPDATE systemusers 
                 SET FirstName = @FirstName, 
                     LastName = @LastName, 
                     Email = @Email, 
                     Birthday = @Birthday, 
                     Age = @Age, 
                     Gender = @Gender, 
                     PhoneNumber = @PhoneNumber, 
                     EmployeeType = @EmployeeType, 
                     EmployeeCategory = @EmployeeCategory, 
                     EmployeeCategoryLevel = @EmployeeCategoryLevel, 
                     EmployeeID = @EmployeeID 
                 WHERE HashedPerUser = @HashedPerUser";
                    using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                    {
                        connection.Open();

                        using (MySqlCommand selectCommand = new MySqlCommand(selectQuery, connection))
                        {
                            selectCommand.Parameters.AddWithValue("@HashedPerUser", selectedHashedPerUser);

                            using (MySqlDataReader reader = selectCommand.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    if (reader["FirstName"].ToString() != AdminFirstNameText.Text ||
                                        reader["LastName"].ToString() != AdminLastNameText.Text ||
                                        reader["Email"].ToString() != AdminEmailText.Text ||
                                        !DateTime.TryParse(reader["Birthday"].ToString(), out DateTime birthday) || birthday != AdminBdayPicker.Value ||
                                        Convert.ToInt32(reader["Age"]) != int.Parse(AdminAgeText.Text) ||
                                        reader["Gender"].ToString() != AdminGenderComboText.SelectedItem.ToString() ||
                                        reader["PhoneNumber"].ToString() != AdminCPNumText.Text ||
                                        reader["EmployeeType"].ToString() != AdminEmplTypeComboText.SelectedItem.ToString() ||
                                        reader["EmployeeCategory"].ToString() != AdminEmplCatComboText.SelectedItem.ToString() ||
                                        reader["EmployeeCategoryLevel"].ToString() != AdminEmplCatLvlComboText.SelectedItem.ToString() ||
                                        reader["EmployeeID"].ToString() != AdminEmplIDText.Text)
                                    {
                                        fieldsChanged = true;
                                    }
                                }
                            }
                        }
                    }

                    if (fieldsChanged)
                    {
                        using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                        {
                            connection.Open();

                            using (MySqlCommand command = new MySqlCommand(query, connection))
                            {
                                command.Parameters.AddWithValue("@FirstName", AdminFirstNameText.Text);
                                command.Parameters.AddWithValue("@LastName", AdminLastNameText.Text);
                                command.Parameters.AddWithValue("@Email", AdminEmailText.Text);
                                command.Parameters.AddWithValue("@Birthday", AdminBdayPicker.Value);
                                command.Parameters.AddWithValue("@Age", int.Parse(AdminAgeText.Text));
                                command.Parameters.AddWithValue("@Gender", AdminGenderComboText.SelectedItem.ToString());
                                command.Parameters.AddWithValue("@PhoneNumber", AdminCPNumText.Text);
                                command.Parameters.AddWithValue("@EmployeeType", AdminEmplTypeComboText.SelectedItem.ToString());
                                command.Parameters.AddWithValue("@EmployeeCategory", AdminEmplCatComboText.SelectedItem.ToString());
                                command.Parameters.AddWithValue("@EmployeeCategoryLevel", AdminEmplCatLvlComboText.SelectedItem.ToString());
                                command.Parameters.AddWithValue("@EmployeeID", AdminEmplIDText.Text);
                                command.Parameters.AddWithValue("@HashedPerUser", selectedHashedPerUser);

                                int rowsAffected = command.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Data updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    PopulateUserInfoDataGrid();
                                    AdminEmplTypeComboText.Enabled = true;
                                    AdminEmplCatComboText.Enabled = true;
                                    AdminCreateAccBtn.Visible = true;
                                    AdminUpdateAccBtn.Visible = false;
                                    AdminClearFields();
                                }
                                else
                                {
                                    MessageBox.Show("No rows updated.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("No changes have been made.", "Information", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a row first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AdminGenderComboText_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (AdminGenderComboText.SelectedItem != null)
            {
                AdminGenderComboText.Text = AdminGenderComboText.SelectedItem.ToString();
            }
        }
        private void PopulateUserInfoDataGrid()
        {
            string connectionString = "Server=localhost;Database=enchante;User=root;Password=;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT FirstName, LastName, Email, Birthday, Age, Gender, PhoneNumber, EmployeeType, EmployeeCategory, EmployeeCategoryLevel, EmployeeID, HashedPass, HashedFixedSalt, HashedPerUser FROM systemusers";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            // Bind the DataTable to the DataGridView
                            AdminAccountTable.DataSource = dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
        private void AdminGenerateID()
        {
            string empType = AdminEmplTypeComboText.SelectedItem?.ToString() ?? string.Empty;
            string empCategory = AdminEmplCatComboText.SelectedItem?.ToString() ?? string.Empty;

            string empTypePrefix = "";
            string empCategoryPrefix = "";

            if (empType == "Admin")
            {
                empTypePrefix = "A-";
            }
            else if (empType == "Manager")
            {
                empTypePrefix = "M-";
            }
            else if (empType == "Staff")
            {
                empTypePrefix = "S-";
            }

            if (empCategory == "Hair Styling")
            {
                empCategoryPrefix = "HS-";
            }
            else if (empCategory == "Face & Skin")
            {
                empCategoryPrefix = "FS-";
            }
            else if (empCategory == "Nail Care")
            {
                empCategoryPrefix = "NC-";
            }
            else if (empCategory == "Massage")
            {
                empCategoryPrefix = "MS-";
            }
            else if (empCategory == "Spa")
            {
                empCategoryPrefix = "SP-";
            }

            Random random = new Random();
            int randomNumber = random.Next(100000, 999999);
            string randomNumberString = randomNumber.ToString("D6");
            string finalID = empTypePrefix + empCategoryPrefix + randomNumberString;
            AdminEmplIDText.Text = finalID;
        }

        //reception dashboard continues here
        private void RecCalendar_DateSelected(object sender, DateRangeEventArgs e)
        {
            // Get the selected date and set your text based on it
            DateTime selectedDate = RecAppCalendar.SelectionStart;

            // Example: Set a label text based on the selected date
            RecAppSelectedDateText.Text = selectedDate.ToString("MM-dd-yyyy");
        }

        private void RecEditSchedBtn_Click(object sender, EventArgs e)
        {


        }

        private void FillRecStaffScheduleViewDataGrid()
        {
            using (MySqlConnection connection = new MySqlConnection(mysqlconn))
            {
                string query = "SELECT EmployeeID, FirstName, LastName, EmployeeCategory, EmployeeCategoryLevel, Schedule, Availability FROM systemusers WHERE EmployeeType = 'Staff'";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                MngrStaffSchedViewDataGrid.Rows.Clear(); // Clear existing rows

                foreach (DataRow row in dataTable.Rows)
                {
                    int index = MngrStaffSchedViewDataGrid.Rows.Add(); // Add a new row to the DataGridView

                    // Fill the existing columns with data
                    MngrStaffSchedViewDataGrid.Rows[index].Cells["EmployeeID"].Value = row["EmployeeID"];
                    MngrStaffSchedViewDataGrid.Rows[index].Cells["FirstName"].Value = row["FirstName"];
                    MngrStaffSchedViewDataGrid.Rows[index].Cells["LastName"].Value = row["LastName"];
                    MngrStaffSchedViewDataGrid.Rows[index].Cells["EmployeeCategory"].Value = row["EmployeeCategory"];
                    MngrStaffSchedViewDataGrid.Rows[index].Cells["CategoryLevel"].Value = row["EmployeeCategoryLevel"];
                    MngrStaffSchedViewDataGrid.Rows[index].Cells["Schedule"].Value = row["Schedule"];
                    MngrStaffSchedViewDataGrid.Rows[index].Cells["Availability"].Value = row["Availability"];
                }
            }
        }

        private void RecEditStaffSchedBtn_Click(object sender, EventArgs e)
        {
            if (MngrStaffSchedViewDataGrid.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = MngrStaffSchedViewDataGrid.SelectedRows[0];

                string EmployeeIDValue = selectedRow.Cells["EmployeeID"].Value.ToString();
                string FirstNameValue = selectedRow.Cells["FirstName"].Value.ToString();
                string LastNameValue = selectedRow.Cells["LastName"].Value.ToString();
                string EmployeeCategoryValue = selectedRow.Cells["EmployeeCategory"].Value.ToString();
                string CategoryLevelValue = selectedRow.Cells["CategoryLevel"].Value.ToString();
                string ScheduleValue = selectedRow.Cells["Schedule"].Value.ToString();
                string AvailabilityValue = selectedRow.Cells["Availability"].Value.ToString();

                MngrEmployeeIDText.Text = EmployeeIDValue;
                MngrEmployeeFirstNameLbl.Text = FirstNameValue;
                MngrEmployeeLastNameLbl.Text = LastNameValue;
                MngrEmployeeCategoryText.Text = EmployeeCategoryValue;
                MngrEmployeeCategoryLevelText.Text = CategoryLevelValue;
                MngrCurrentSchedText.Text = ScheduleValue;
                MngrCurrentAvailabilityText.Text = AvailabilityValue;
            }
        }

        private void RecChangeStaffSchedBtn_Click(object sender, EventArgs e)
        {
            string EmployeeIDValue = MngrEmployeeIDText.Text;
            string EmployeeAvailabilityValue = MngrStaffAvailabilityComboBox.SelectedItem.ToString();
            string EmployeeTimeScheduleValue = MngrStaffSchedComboBox.SelectedItem.ToString();

            using (MySqlConnection connection = new MySqlConnection(mysqlconn))
            {
                connection.Open();

                bool availabilityUpdated = false;
                bool scheduleUpdated = false;

                if (MngrStaffAvailabilityComboBox.SelectedIndex != 0)
                {
                    string updateAvailabilityQuery = "UPDATE systemusers SET Availability = @Availability WHERE EmployeeID = @EmployeeID";
                    MySqlCommand availabilityCommand = new MySqlCommand(updateAvailabilityQuery, connection);
                    availabilityCommand.Parameters.AddWithValue("@Availability", EmployeeAvailabilityValue);
                    availabilityCommand.Parameters.AddWithValue("@EmployeeID", EmployeeIDValue);
                    int availabilityRowsAffected = availabilityCommand.ExecuteNonQuery();

                    if (availabilityRowsAffected > 0)
                    {
                        availabilityUpdated = true;
                    }
                }

                if (MngrStaffSchedComboBox.SelectedIndex != 0)
                {
                    string updateScheduleQuery = "UPDATE systemusers SET Schedule = @Schedule WHERE EmployeeID = @EmployeeID";
                    MySqlCommand scheduleCommand = new MySqlCommand(updateScheduleQuery, connection);
                    scheduleCommand.Parameters.AddWithValue("@Schedule", EmployeeTimeScheduleValue);
                    scheduleCommand.Parameters.AddWithValue("@EmployeeID", EmployeeIDValue);
                    int scheduleRowsAffected = scheduleCommand.ExecuteNonQuery();

                    if (scheduleRowsAffected > 0)
                    {
                        scheduleUpdated = true;
                    }
                }

                if (availabilityUpdated && scheduleUpdated)
                {
                    MessageBox.Show("Availability and schedule updated.");
                }
                else if (availabilityUpdated || scheduleUpdated)
                {
                    MessageBox.Show("Availability or schedule was updated.");
                }
                else if (!availabilityUpdated && !scheduleUpdated)
                {
                    MessageBox.Show("No updates were made.");
                }
            }

            InitializeAvailableStaffFlowLayout();
            FillRecStaffScheduleViewDataGrid();
        }


        public class AvailableStaff
        {
            public string EmployeeAvailability { get; set; }
            public string EmployeeSchedule { get; set; }
            public string EmployeeID { get; set; }
            public string EmployeeFirstName { get; set; }
            public string EmployeeLastName { get; set; }
            public string EmployeeName { get; set; }
            public string EmployeeCategory { get; set; }
            public string EmployeeCategoryLevel { get; set; }
        }

        private void InitializeAvailableStaffFlowLayout()
        {
            List<AvailableStaff> availableStaff = RetrieveAvailableStaffFromDB();


            foreach (AvailableStaff staff in availableStaff)
            {
                if (staff.EmployeeAvailability == "Available")
                {
                    AvailableStaffUserControl addedavailablestaffusercontrol = new AvailableStaffUserControl();
                    addedavailablestaffusercontrol.AvailableStaffSetData(staff);
                    AvailableStaffActiveToggleSwitch = addedavailablestaffusercontrol.Controls.OfType<Guna.UI2.WinForms.Guna2ToggleSwitch>().FirstOrDefault();
                    if (AvailableStaffActiveToggleSwitch != null)
                    {
                        AvailableStaffActiveToggleSwitch.CheckedChanged += AvailableStaffToggleSwitch_CheckedChanged;
                    }
                    RecAvaialableStaffFlowLayout.Controls.Add(addedavailablestaffusercontrol);
                }

            }
        }

        private List<AvailableStaff> RetrieveAvailableStaffFromDB()
        {
            List<AvailableStaff> result = new List<AvailableStaff>();

            using (MySqlConnection connection = new MySqlConnection(mysqlconn))
            {
                connection.Open();

                string availablestaffquery = "SELECT Availability, Schedule, EmployeeID, FirstName, LastName, EmployeeCategory, EmployeeCategoryLevel FROM systemusers WHERE Availability = 'Available' ";
                MySqlCommand command = new MySqlCommand(availablestaffquery, connection);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    // Check if there are any results
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            AvailableStaff availableStaff = new AvailableStaff
                            {
                                EmployeeAvailability = reader.GetString("Availability"),
                                EmployeeSchedule = reader.GetString("Schedule"),
                                EmployeeID = reader.GetString("EmployeeID"),
                                EmployeeFirstName = reader.GetString("FirstName"),
                                EmployeeLastName = reader.GetString("LastName"),
                                EmployeeCategory = reader.GetString("EmployeeCategory"),
                                EmployeeCategoryLevel = reader.GetString("EmployeeCategoryLevel")
                            };

                            result.Add(availableStaff);
                        }

                    }
                }
            }

            return result;
        }

        private void DisbaleTimeSchedIfNoDateIsSelected()
        {
            if (string.IsNullOrEmpty(RecAppSelectedDateText.Text))
            {
                RecAppPrefferedTimeAMComboBox.Enabled = false;
                RecAppPrefferedTimePMComboBox.Enabled = false;
            }
            else
            {
                RecAppPrefferedTimeAMComboBox.Enabled = true;
                RecAppPrefferedTimePMComboBox.Enabled = true;
            }
        }
        private void RecPrefferedTimeAMComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (RecAppPrefferedTimeAMComboBox.SelectedIndex != 0)
            {
                RecAppPrefferedTimePMComboBox.Enabled = false;
            }
            else
            {
                RecAppPrefferedTimePMComboBox.Enabled = true;
            }
            FilterAvailableStaffInRecFlowLayoutPanelAM();


        }

        private void RecPrefferedTimePMComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (RecAppPrefferedTimePMComboBox.SelectedIndex != 0)
            {
                RecAppPrefferedTimeAMComboBox.Enabled = false;
            }
            else
            {
                RecAppPrefferedTimeAMComboBox.Enabled = true;
            }
            FilterAvailableStaffInRecFlowLayoutPanelPM();


        }


        private void FilterAvailableStaffInRecFlowLayoutPanelAM()
        {
            List<AvailableStaff> availableStaff = RetrieveAvailableStaffFromDB();//DEFAULT STAFF
            if (RecAppPrefferedTimeAMComboBox.SelectedIndex != 0)
            {
                List<AvailableStaff> filterbyschedstaff = new List<AvailableStaff>();
                RecAvaialableStaffFlowLayout.Controls.Clear();

                foreach (AvailableStaff staff in availableStaff)
                {
                    if (staff.EmployeeAvailability == "Available" && staff.EmployeeSchedule == "AM")
                    {
                        filterbyschedstaff.Add(staff);
                        AvailableStaffUserControl addedavailablestaffusercontrol = new AvailableStaffUserControl();
                        addedavailablestaffusercontrol.AvailableStaffSetData(staff);
                        AvailableStaffActiveToggleSwitch = addedavailablestaffusercontrol.Controls.OfType<Guna.UI2.WinForms.Guna2ToggleSwitch>().FirstOrDefault();
                        if (AvailableStaffActiveToggleSwitch != null)
                        {
                            AvailableStaffActiveToggleSwitch.CheckedChanged += AvailableStaffToggleSwitch_CheckedChanged;
                        }
                        RecAvaialableStaffFlowLayout.Controls.Add(addedavailablestaffusercontrol);
                    }

                }
                filteredbyschedstaff = filterbyschedstaff.ToList();
            }
            else
            {
                foreach (AvailableStaff staff in availableStaff)
                {
                    if (staff.EmployeeAvailability == "Available")
                    {
                        AvailableStaffUserControl addedavailablestaffusercontrol = new AvailableStaffUserControl();
                        addedavailablestaffusercontrol.AvailableStaffSetData(staff);
                        AvailableStaffActiveToggleSwitch = addedavailablestaffusercontrol.Controls.OfType<Guna.UI2.WinForms.Guna2ToggleSwitch>().FirstOrDefault();
                        if (AvailableStaffActiveToggleSwitch != null)
                        {
                            AvailableStaffActiveToggleSwitch.CheckedChanged += AvailableStaffToggleSwitch_CheckedChanged;
                        }
                        RecAvaialableStaffFlowLayout.Controls.Add(addedavailablestaffusercontrol);
                    }

                }
            }

        }


        private void FilterAvailableStaffInRecFlowLayoutPanelPM()
        {
            List<AvailableStaff> availableStaff = RetrieveAvailableStaffFromDB(); // DEFAULT AVAILABLE STAFF

            if (RecAppPrefferedTimePMComboBox.SelectedIndex != 0)
            {
                List<AvailableStaff> filterbyschedstaff = new List<AvailableStaff>();
                RecAvaialableStaffFlowLayout.Controls.Clear();

                foreach (AvailableStaff staff in availableStaff)
                {
                    if (staff.EmployeeAvailability == "Available" && staff.EmployeeSchedule == "PM")
                    {
                        filterbyschedstaff.Add(staff);
                        AvailableStaffUserControl addedavailablestaffusercontrol = new AvailableStaffUserControl();
                        addedavailablestaffusercontrol.AvailableStaffSetData(staff);
                        AvailableStaffActiveToggleSwitch = addedavailablestaffusercontrol.Controls.OfType<Guna.UI2.WinForms.Guna2ToggleSwitch>().FirstOrDefault();
                        if (AvailableStaffActiveToggleSwitch != null)
                        {
                            AvailableStaffActiveToggleSwitch.CheckedChanged += AvailableStaffToggleSwitch_CheckedChanged;
                        }
                        RecAvaialableStaffFlowLayout.Controls.Add(addedavailablestaffusercontrol);
                    }

                }

                filteredbyschedstaff = filterbyschedstaff.ToList();
            }
            else
            {
                foreach (AvailableStaff staff in availableStaff)
                {
                    if (staff.EmployeeAvailability == "Available")
                    {
                        AvailableStaffUserControl addedavailablestaffusercontrol = new AvailableStaffUserControl();
                        addedavailablestaffusercontrol.AvailableStaffSetData(staff);
                        AvailableStaffActiveToggleSwitch = addedavailablestaffusercontrol.Controls.OfType<Guna.UI2.WinForms.Guna2ToggleSwitch>().FirstOrDefault();
                        if (AvailableStaffActiveToggleSwitch != null)
                        {
                            AvailableStaffActiveToggleSwitch.CheckedChanged += AvailableStaffToggleSwitch_CheckedChanged;
                        }
                        RecAvaialableStaffFlowLayout.Controls.Add(addedavailablestaffusercontrol);
                    }

                }
            }

        }

        private void FilterAvailableStaffInRecFlowLayoutPanelByHairStyling()
        {
            List<AvailableStaff> filteredbysched = filteredbyschedstaff.ToList();

            RecAvaialableStaffFlowLayout.Controls.Clear();

            foreach (AvailableStaff staff in filteredbysched)
            {
                if (staff.EmployeeCategory == "Hair Styling")
                {
                    AvailableStaffUserControl addedavailablestaffusercontrol = new AvailableStaffUserControl();
                    addedavailablestaffusercontrol.AvailableStaffSetData(staff);
                    AvailableStaffActiveToggleSwitch = addedavailablestaffusercontrol.Controls.OfType<Guna.UI2.WinForms.Guna2ToggleSwitch>().FirstOrDefault();
                    if (AvailableStaffActiveToggleSwitch != null)
                    {
                        AvailableStaffActiveToggleSwitch.CheckedChanged += AvailableStaffToggleSwitch_CheckedChanged;
                    }
                    RecAvaialableStaffFlowLayout.Controls.Add(addedavailablestaffusercontrol);
                }

            }

        }

        private void FilterAvailableStaffInRecFlowLayoutPanelByFaceandSkin()
        {
            List<AvailableStaff> filteredbysched = filteredbyschedstaff.ToList();

            RecAvaialableStaffFlowLayout.Controls.Clear();

            foreach (AvailableStaff staff in filteredbysched)
            {
                if (staff.EmployeeCategory == "Face & Skin")
                {
                    AvailableStaffUserControl addedavailablestaffusercontrol = new AvailableStaffUserControl();
                    addedavailablestaffusercontrol.AvailableStaffSetData(staff);
                    AvailableStaffActiveToggleSwitch = addedavailablestaffusercontrol.Controls.OfType<Guna.UI2.WinForms.Guna2ToggleSwitch>().FirstOrDefault();
                    if (AvailableStaffActiveToggleSwitch != null)
                    {
                        AvailableStaffActiveToggleSwitch.CheckedChanged += AvailableStaffToggleSwitch_CheckedChanged;
                    }
                    RecAvaialableStaffFlowLayout.Controls.Add(addedavailablestaffusercontrol);
                }

            }

        }

        private void FilterAvailableStaffInRecFlowLayoutPanelByNailCare()
        {
            List<AvailableStaff> filteredbysched = filteredbyschedstaff.ToList();

            RecAvaialableStaffFlowLayout.Controls.Clear();

            foreach (AvailableStaff staff in filteredbysched)
            {
                if (staff.EmployeeCategory == "Nail Care")
                {
                    AvailableStaffUserControl addedavailablestaffusercontrol = new AvailableStaffUserControl();
                    addedavailablestaffusercontrol.AvailableStaffSetData(staff);
                    AvailableStaffActiveToggleSwitch = addedavailablestaffusercontrol.Controls.OfType<Guna.UI2.WinForms.Guna2ToggleSwitch>().FirstOrDefault();
                    if (AvailableStaffActiveToggleSwitch != null)
                    {
                        AvailableStaffActiveToggleSwitch.CheckedChanged += AvailableStaffToggleSwitch_CheckedChanged;
                    }
                    RecAvaialableStaffFlowLayout.Controls.Add(addedavailablestaffusercontrol);
                }

            }

        }

        private void FilterAvailableStaffInRecFlowLayoutPanelByMassage()
        {
            List<AvailableStaff> filteredbysched = filteredbyschedstaff.ToList();

            RecAvaialableStaffFlowLayout.Controls.Clear();

            foreach (AvailableStaff staff in filteredbysched)
            {
                if (staff.EmployeeCategory == "Massage")
                {
                    AvailableStaffUserControl addedavailablestaffusercontrol = new AvailableStaffUserControl();
                    addedavailablestaffusercontrol.AvailableStaffSetData(staff);
                    AvailableStaffActiveToggleSwitch = addedavailablestaffusercontrol.Controls.OfType<Guna.UI2.WinForms.Guna2ToggleSwitch>().FirstOrDefault();
                    if (AvailableStaffActiveToggleSwitch != null)
                    {
                        AvailableStaffActiveToggleSwitch.CheckedChanged += AvailableStaffToggleSwitch_CheckedChanged;
                    }
                    RecAvaialableStaffFlowLayout.Controls.Add(addedavailablestaffusercontrol);
                }

            }

        }

        private void FilterAvailableStaffInRecFlowLayoutPanelBySpa()
        {
            List<AvailableStaff> filteredbysched = filteredbyschedstaff.ToList();

            RecAvaialableStaffFlowLayout.Controls.Clear();

            foreach (AvailableStaff staff in filteredbysched)
            {
                if (staff.EmployeeCategory == "Spa")
                {
                    AvailableStaffUserControl addedavailablestaffusercontrol = new AvailableStaffUserControl();
                    addedavailablestaffusercontrol.AvailableStaffSetData(staff);
                    AvailableStaffActiveToggleSwitch = addedavailablestaffusercontrol.Controls.OfType<Guna.UI2.WinForms.Guna2ToggleSwitch>().FirstOrDefault();
                    if (AvailableStaffActiveToggleSwitch != null)
                    {
                        AvailableStaffActiveToggleSwitch.CheckedChanged += AvailableStaffToggleSwitch_CheckedChanged;
                    }
                    RecAvaialableStaffFlowLayout.Controls.Add(addedavailablestaffusercontrol);
                }

            }

        }

        private void AvailableStaffToggleSwitch_CheckedChanged(object sender, EventArgs e)
        {
            Guna.UI2.WinForms.Guna2ToggleSwitch toggleSwitch = (Guna.UI2.WinForms.Guna2ToggleSwitch)sender;
            UserControl userControl = (UserControl)toggleSwitch.Parent;

            if (toggleSwitch.Checked)
            {
                if (AvailableStaffActiveToggleSwitch != null && AvailableStaffActiveToggleSwitch != toggleSwitch)
                {
                    AvailableStaffActiveToggleSwitch.Checked = false;
                }
                AvailableStaffActiveToggleSwitch = toggleSwitch;
            }
            else if (AvailableStaffActiveToggleSwitch == toggleSwitch)
            {
                AvailableStaffActiveToggleSwitch = null;
            }
        }

        private void RecPrefferedTimeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

            IsPrefferredTimeSchedComboBoxModified = true;
        }

        private void RecSelectServiceAndStaffBtn_Click(object sender, EventArgs e)
        {
            AddService();
        }
        private void AddService()
        {
            //bool IsStaffSelectedToggleSwitch = false;
            //AvailableStaff selectedStaff = null;

            //foreach (AvailableStaffUserControl availabelstaffusercontrol in RecAvaialableStaffFlowLayout.Controls)
            //{
            //    Guna.UI2.WinForms.Guna2ToggleSwitch availabelstaffusercontroltoggleswitch = availabelstaffusercontrol.Controls.OfType<Guna.UI2.WinForms.Guna2ToggleSwitch>().FirstOrDefault();

            //    if (availabelstaffusercontroltoggleswitch != null && availabelstaffusercontroltoggleswitch.Checked)
            //    {
            //        IsStaffSelectedToggleSwitch = true;
            //        selectedStaff = availabelstaffusercontrol.GetAvailableStaffData();

            //        break;
            //    }
            //}

            //if (!IsStaffSelectedToggleSwitch)
            //{
            //    MessageBox.Show("Please select a staff member.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}

            if (RecWalkInServiceTypeTable.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a service.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataGridViewRow selectedRow = RecWalkInServiceTypeTable.SelectedRows[0];

            string SelectedDateValue = RecAppSelectedDateText.Text;
            //string TimePickedValue = CustomerTimePicked();
            //string TimeSchedPickedValue = TimeSchedPicked();
            string TimePickedValue = CustomerTimePicked();
            string TimeSchedPickedValue = TimeSchedPicked();
            string CustomerName = RecWalkinLNameText.Text + ", " + RecWalkinFNameText.Text;
            string CustomerMobileNumber = RecWalkinCPNumText.Text;
            string SelectedCategory = selectedRow.Cells[0].Value.ToString();
            string ServiceID = selectedRow.Cells[2].Value.ToString();
            string ServiceName = selectedRow.Cells[3].Value.ToString();
            string ServiceDuration = selectedRow.Cells[5].Value.ToString();
            string ServicePrice = selectedRow.Cells[6].Value.ToString();
            string CustomerCustomizations = RecCustomerCustomizationsTextBox.Text;
            string CustomerAdditionalNotes = RecCustomerCustomerAdditionalNotesTextBox.Text;
            //string EmployeeID = selectedStaff.EmployeeID;
            //string EmployeeName = selectedStaff.EmployeeName;
            //string EmployeeCategory = selectedStaff.EmployeeCategory;
            //string EmployeeCategoryLevel = selectedStaff.EmployeeCategoryLevel;
            //string EmployeeSchedule = selectedStaff.EmployeeSchedule;

            string serviceID = selectedRow.Cells[2]?.Value?.ToString(); // Use null-conditional operator to avoid NullReferenceException

            // ... (existing code)

            if (RecWalkInServiceTypeTable.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a service.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (selectedRow == null)
            {
                MessageBox.Show("Selected row is null.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(serviceID))
            {
                MessageBox.Show("Service ID is null or empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check if the service is already in the RecSelectedServiceDataGrid
            foreach (DataGridViewRow row in RecSelectedServiceDataGrid1.Rows)
            {
                string existingServiceID = row.Cells["ServiceCategory"]?.Value?.ToString(); // Use null-conditional operator

                if (serviceID == existingServiceID)
                {
                    MessageBox.Show("This service is already selected.", "Duplicate Service", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }

            // ... (existing code)


            DialogResult result = MessageBox.Show("Are you sure you want to add this service?", "Confirm Service Selection", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Add the row
                DataGridViewRow NewSelectedServiceRow = RecSelectedServiceDataGrid1.Rows[RecSelectedServiceDataGrid1.Rows.Add()];

                // Set the cell values
                //NewSelectedServiceRow.Cells["SelectedDate"].Value = SelectedDateValue;
                //NewSelectedServiceRow.Cells["TimePicked"].Value = TimePickedValue;
                //NewSelectedServiceRow.Cells["TimeSched"].Value = TimeSchedPickedValue;
                //NewSelectedServiceRow.Cells["CustomerName"].Value = CustomerName;
                //NewSelectedServiceRow.Cells["MobileNumber"].Value = CustomerMobileNumber;
                //NewSelectedServiceRow.Cells["ServiceID"].Value = ServiceID;
                //NewSelectedServiceRow.Cells["ServiceDuration"].Value = ServiceDuration;
                //NewSelectedServiceRow.Cells["CustomerAdditionalNotes"].Value = CustomerAdditionalNotes;
                //NewSelectedServiceRow.Cells["StaffSelectedID"].Value = EmployeeID;
                //NewSelectedServiceRow.Cells["StaffName"].Value = EmployeeName;
                //NewSelectedServiceRow.Cells["StaffCategory"].Value = EmployeeCategory;
                //NewSelectedServiceRow.Cells["StaffCategoryLevel"].Value = EmployeeCategoryLevel;
                //NewSelectedServiceRow.Cells["StaffTimeSched"].Value = EmployeeSchedule;
                
                NewSelectedServiceRow.Cells["ServicePrice"].Value = ServicePrice;
                NewSelectedServiceRow.Cells["ServiceCategory"].Value = SelectedCategory; 
                NewSelectedServiceRow.Cells["SelectedService"].Value = ServiceName;
                NewSelectedServiceRow.Cells["ServiceID"].Value = ServiceID;

                RecWalkInServiceTypeTable.ClearSelection();
                RecCustomerCustomizationsTextBox.Clear();
                RecCustomerCustomerAdditionalNotesTextBox.Clear();

                //foreach (AvailableStaffUserControl availabelstaffusercontrol in RecAvaialableStaffFlowLayout.Controls)
                //{
                //    Guna.UI2.WinForms.Guna2ToggleSwitch availabelstaffusercontroltoggleswitch = availabelstaffusercontrol.Controls.OfType<Guna.UI2.WinForms.Guna2ToggleSwitch>().FirstOrDefault();

                //    if (availabelstaffusercontroltoggleswitch != null)
                //    {
                //        availabelstaffusercontroltoggleswitch.Checked = false;
                //    }
                //}
            }
        }
        private string CustomerTimePicked()
        {
            string TimePicked = string.Empty;

            if (RecAppPrefferedTimeAMComboBox.SelectedIndex == 0)
            {
                TimePicked = RecAppPrefferedTimePMComboBox.SelectedItem.ToString();
            }
            else if (RecAppPrefferedTimePMComboBox.SelectedIndex == 0)
            {
                TimePicked = RecAppPrefferedTimeAMComboBox.SelectedItem.ToString();
            }

            return TimePicked;
        }

        private string TimeSchedPicked()
        {
            string TimeSched = string.Empty;

            if (RecAppPrefferedTimeAMComboBox.SelectedIndex == 0)
            {
                TimeSched = "PM";
            }
            else if (RecAppPrefferedTimePMComboBox.SelectedIndex == 0)
            {
                TimeSched = "AM";
            }

            return TimeSched;
        }
        private void RecWalkinSelectedDateText_TextChanged(object sender, EventArgs e)
        {
            DisbaleTimeSchedIfNoDateIsSelected();
        }

        private void RecDeleteSelectedServiceAndStaffBtn_Click(object sender, EventArgs e)
        {
            if (RecSelectedServiceDataGrid1.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this row?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    DataGridViewRow selectedRow = RecSelectedServiceDataGrid1.SelectedRows[0];
                    RecSelectedServiceDataGrid1.Rows.Remove(selectedRow);
                }
            }
        }

        private void RecSelectedServiceDataGrid1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void RecWalkInServiceTypeTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            AddService();
            ReceptionCalculateTotalPrice();
        }

        private void RecWalkinBookTransactBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(RecWalkinCashBox.Text))
            {
                MessageBox.Show("Please add a valid amount of cash.", "Ooooops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (decimal.TryParse(RecWalkinChangeBox.Text, out decimal cash) && cash < 0)
            {
                MessageBox.Show("Please add a valid amount of cash.", "Ooooops!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                RecWalkinServiceHistoryDB(RecSelectedServiceDataGrid1);
                ReceptionistWalk_in_AppointmentDB();
            }

        }

        private void ReceptionistWalk_in_AppointmentDB()
        {
            //bool IsStaffSelectedToggleSwitch = false;
            //AvailableStaff selectedStaff = null;

            //foreach (AvailableStaffUserControl availabelstaffusercontrol in RecAvaialableStaffFlowLayout.Controls)
            //{
            //    Guna.UI2.WinForms.Guna2ToggleSwitch availabelstaffusercontroltoggleswitch = availabelstaffusercontrol.Controls.OfType<Guna.UI2.WinForms.Guna2ToggleSwitch>().FirstOrDefault();

            //    if (availabelstaffusercontroltoggleswitch != null && availabelstaffusercontroltoggleswitch.Checked)
            //    {
            //        IsStaffSelectedToggleSwitch = true;
            //        selectedStaff = availabelstaffusercontrol.GetAvailableStaffData();

            //        break;
            //    }
            //}
            //string EmployeeName = selectedStaff.EmployeeName;//attending staff

            DateTime currentDate = RecDateTimePicker.Value;
            string transactionNum = RecWalkinTransNumText.Text;
            string serviceStatus = "Pending";

            //basic info
            string CustomerName = RecWalkinFNameText.Text + " " + RecWalkinLNameText.Text; //client name
            string CustomerMobileNumber = RecWalkinCPNumText.Text; //client cp num

            //cash values
            string netAmount = RecWalkinNetAmountBox.Text; //net amount
            string vat = RecWalkinVATBox.Text; //vat 
            string discount = RecWalkinDiscountBox.Text;//discount
            string grossAmount = RecWalkinGrossAmountBox.Text; //gross amount
            string cash = RecWalkinCashBox.Text; //cashgiven
            string change = RecWalkinChangeBox.Text; //due change
            string paymentMethod = RecWalkinTypeText.Text; //payment method
            
            //booked values
            string bookedDate = currentDate.ToString("MM-dd-yyyy dddd"); //bookedDate
            string bookedTime = currentDate.ToString("hh:mm tt"); //bookedTime
            string bookedBy = RecNameLbl.Text; //booked by
            
            //customize & add notes
            string custom = RecCustomerCustomizationsTextBox.Text;
            string notes = RecCustomerCustomerAdditionalNotesTextBox.Text;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                {
                    connection.Open();
                    string insertQuery = "INSERT INTO walk_in_appointment (TransactionNumber, ServiceStatus, AppointmentDate, AppointmentTime, " +
                                        "ClientName, CustomerCustomizations, AdditionalNotes, ClientCPNum, NetPrice, VatAmount, DiscountAmount, GrossAmount, CashGiven, " +
                                        "DueChange, PaymentMethod, ServiceDuration, BookedBy, BookedDate, BookedTime)" +
                                        "VALUES (@Transact, @status, @appointDate, @appointTime, @clientName, @custom, @addNotes, @clientCP, @net, @vat, " +
                                        "@discount, @gross, @cash, @change, @payment, @duration, @bookedBy, @bookedDate, @bookedTime)";

                    MySqlCommand cmd = new MySqlCommand(insertQuery, connection);
                    cmd.Parameters.AddWithValue("@Transact", transactionNum);
                    cmd.Parameters.AddWithValue("@status", serviceStatus);
                    cmd.Parameters.AddWithValue("@appointDate", bookedDate);
                    cmd.Parameters.AddWithValue("@appointTime", bookedTime);
                    //cmd.Parameters.AddWithValue("@staff", EmployeeName);
                    cmd.Parameters.AddWithValue("@clientName", CustomerName);
                    cmd.Parameters.AddWithValue("@custom", custom);
                    cmd.Parameters.AddWithValue("@addNotes", notes);
                    cmd.Parameters.AddWithValue("@clientCP", CustomerMobileNumber);
                    cmd.Parameters.AddWithValue("@net", netAmount);
                    cmd.Parameters.AddWithValue("@vat", vat);
                    cmd.Parameters.AddWithValue("@discount", discount);
                    cmd.Parameters.AddWithValue("@gross", grossAmount);
                    cmd.Parameters.AddWithValue("@cash", cash);
                    cmd.Parameters.AddWithValue("@change", change);
                    cmd.Parameters.AddWithValue("@payment", paymentMethod);
                    cmd.Parameters.AddWithValue("@duration", "00:00:00");
                    cmd.Parameters.AddWithValue("@bookedBy", bookedBy);
                    cmd.Parameters.AddWithValue("@bookedDate", bookedDate);
                    cmd.Parameters.AddWithValue("@bookedTime", bookedTime);


                    cmd.ExecuteNonQuery();
                }

                // Successful insertion
                MessageBox.Show("Service successfully booked.", "Hooray!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Transaction.PanelShow(RecTransactionPanel);
                //RecWalkinServiceHistoryDB();
            }
            catch (MySqlException ex)
            {
                // Handle MySQL database exception
                MessageBox.Show("An error occurred: " + ex.Message, "Manager booked transaction failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Make sure to close the connection
                connection.Close();
            }
        }

        private void RecWalkinServiceHistoryDB(DataGridView RecWalkinServiceHistoryView)
        {
            DateTime currentDate = RecDateTimePicker.Value;
            string transactionNum = RecWalkinTransNumText.Text;
            string serviceStatus = "Pending";

            //booked values
            string bookedDate = currentDate.ToString("MM-dd-yyyy dddd"); //bookedDate
            string bookedTime = currentDate.ToString("hh:mm tt"); //bookedTime
            
            //basic info
            string CustomerName = RecWalkinFNameText.Text + " " + RecWalkinLNameText.Text; //client name

            //customize & add notes
            string custom = RecCustomerCustomizationsTextBox.Text;
            string notes = RecCustomerCustomerAdditionalNotesTextBox.Text;
            if (RecSelectedServiceDataGrid1.Rows.Count > 0)
            {
                try
                {
                    using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                    {
                        connection.Open();

                        foreach (DataGridViewRow row in RecSelectedServiceDataGrid1.Rows)
                        {
                            if (row.Cells["SelectedService"].Value != null)
                            {
                                string serviceName = row.Cells["SelectedService"].Value.ToString();
                                string serviceCat = row.Cells["ServiceCategory"].Value.ToString();
                                string serviceID = row.Cells["ServiceID"].Value.ToString();
                                decimal servicePrice = Convert.ToDecimal(row.Cells["ServicePrice"].Value);

                                string insertQuery = "INSERT INTO servicehistory (TransactionNumber, ServiceStatus, AppointmentDate, AppointmentTime, ClientName, " +
                                                    "ServiceCategory, ServiceID, SelectedService, ServicePrice, Customization, AddNotes)" +
                                                    "VALUES (@Transact, @status, @appointDate, @appointTime, @name, @serviceCat, @ID, @serviceName, @servicePrice, " +
                                                    "@custom, @notes)";

                                MySqlCommand cmd = new MySqlCommand(insertQuery, connection);
                                cmd.Parameters.AddWithValue("@Transact", transactionNum);
                                cmd.Parameters.AddWithValue("@status", serviceStatus);
                                cmd.Parameters.AddWithValue("@appointDate", bookedDate);
                                cmd.Parameters.AddWithValue("@appointTime", bookedTime);
                                cmd.Parameters.AddWithValue("@name", CustomerName);
                                cmd.Parameters.AddWithValue("@serviceCat", serviceCat);
                                cmd.Parameters.AddWithValue("@ID", serviceID);
                                cmd.Parameters.AddWithValue("@serviceName", serviceName);
                                cmd.Parameters.AddWithValue("@servicePrice", servicePrice);
                                cmd.Parameters.AddWithValue("@custom", custom);
                                cmd.Parameters.AddWithValue("@notes", notes);

                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Manager booked service failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
            else
            {
                MessageBox.Show("No items to insert into the database.");
            }
            
        }

        private void ReceptionCalculateTotalPrice()
        {
            decimal total = 0;

            // Assuming the "Price" column is of decimal type
            int priceColumnIndex = RecSelectedServiceDataGrid1.Columns["ServicePrice"].Index;

            foreach (DataGridViewRow row in RecSelectedServiceDataGrid1.Rows)
            {
                if (row.Cells[priceColumnIndex].Value != null)
                {
                    decimal price = decimal.Parse(row.Cells[priceColumnIndex].Value.ToString());
                    total += price;
                }
            }

            // Display the total price in the GrossAmountBox TextBox
            RecWalkinGrossAmountBox.Text = total.ToString("F2"); // Format to two decimal places

            ReceptionCalculateVATAndNetAmount();
        }

        public void ReceptionCalculateVATAndNetAmount()
        {
            // Get the Gross Amount from the TextBox (MngrGrossAmountBox)
            if (decimal.TryParse(RecWalkinGrossAmountBox.Text, out decimal grossAmount))
            {
                // Fixed VAT rate of 12%
                decimal rate = 12;

                // Calculate the VAT Amount
                decimal netAmount = grossAmount / ((rate / 100) + 1);

                // Calculate the Net Amount
                decimal vatAmount = grossAmount - netAmount;

                // Display the calculated values in TextBoxes
                RecWalkinVATBox.Text = vatAmount.ToString("0.00");
                RecWalkinNetAmountBox.Text = netAmount.ToString("0.00");
                RecWalkinVATBox.Text = vatAmount.ToString("0.00");
                RecWalkinNetAmountBox.Text = netAmount.ToString("0.00");
            }

        }

        private void DateTimePickerTimer_Tick(object sender, EventArgs e)
        {
            RecDateTimePicker.Value = DateTime.Now;
            DateTime cashierrcurrentDate = RecDateTimePicker.Value;
            string Cashiertoday = cashierrcurrentDate.ToString("MM-dd-yyyy dddd hh:mm tt");
            RecDateTimeText.Text = Cashiertoday;
        }
        private bool discountApplied = false; // Flag to track if the discount has been applied
        private decimal originalGrossAmount; // Store the original value

        private void RecWalkinDiscountSenior_CheckedChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(RecWalkinGrossAmountBox.Text, out decimal grossAmount))
            {
                if (RecWalkinDiscountSenior.Checked && !discountApplied)
                {
                    // Apply the 20% discount if the checkbox is checked and the discount hasn't been applied before
                    originalGrossAmount = grossAmount; // Store the original value
                    decimal discountPercentage = 20m;
                    decimal discountAmount = grossAmount * (discountPercentage / 100); // Calculate the discount amount
                    decimal discountedAmount = grossAmount - discountAmount; // Subtract the discount amount
                    RecWalkinGrossAmountBox.Text = discountedAmount.ToString("0.00"); // Format to display as currency
                    discountApplied = true; // Set the flag to indicate that the discount has been applied
                    RecWalkinDiscountBox.Text = discountAmount.ToString("0.00"); // Display the discount amount
                }
                else if (!RecWalkinDiscountSenior.Checked && discountApplied)
                {
                    // Unchecked, set MngrGrossAmount to the original value if the discount has been applied before
                    RecWalkinGrossAmountBox.Text = originalGrossAmount.ToString("0.00");
                    discountApplied = false; // Reset the flag
                    RecWalkinDiscountBox.Text = "0.00"; // Reset the discount amount display
                }
                else
                {
                    // If the checkbox is checked but the discount has already been applied, update the discount amount display
                    decimal discountPercentage = 20m;
                    decimal discountAmount = originalGrossAmount * (discountPercentage / 100);
                    RecWalkinDiscountBox.Text = discountAmount.ToString("0.00");
                }
            }
        }

        private void RecWalkinDiscountPWD_CheckedChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(RecWalkinGrossAmountBox.Text, out decimal grossAmount))
            {
                if (RecWalkinDiscountPWD.Checked && !discountApplied)
                {
                    // Apply the 20% discount if the checkbox is checked and the discount hasn't been applied before
                    originalGrossAmount = grossAmount; // Store the original value
                    decimal discountPercentage = 20m;
                    decimal discountAmount = grossAmount * (discountPercentage / 100); // Calculate the discount amount
                    decimal discountedAmount = grossAmount - discountAmount; // Subtract the discount amount
                    RecWalkinGrossAmountBox.Text = discountedAmount.ToString("0.00"); // Format to display as currency
                    discountApplied = true; // Set the flag to indicate that the discount has been applied
                    RecWalkinDiscountBox.Text = discountAmount.ToString("0.00"); // Display the discount amount
                }
                else if (!RecWalkinDiscountPWD.Checked && discountApplied)
                {
                    // Unchecked, set MngrGrossAmount to the original value if the discount has been applied before
                    RecWalkinGrossAmountBox.Text = originalGrossAmount.ToString("0.00");
                    discountApplied = false; // Reset the flag
                    RecWalkinDiscountBox.Text = "0.00"; // Reset the discount amount display
                }
                else
                {
                    // If the checkbox is checked but the discount has already been applied, update the discount amount display
                    decimal discountPercentage = 20m;
                    decimal discountAmount = originalGrossAmount * (discountPercentage / 100);
                    RecWalkinDiscountBox.Text = discountAmount.ToString("0.00");
                }
            }
        }

        private void RecWalkinCashBox_TextChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(RecWalkinGrossAmountBox.Text, out decimal grossAmount))
            {
                // Get the Cash Amount from the TextBox (MngrCashBox)
                if (decimal.TryParse(RecWalkinCashBox.Text, out decimal cashAmount))
                {
                    // Calculate the Change
                    decimal change = cashAmount - grossAmount;

                    // Display the calculated change value in the MngrChangeBox
                    RecWalkinChangeBox.Text = change.ToString("0.00");
                }
                else
                {
                    // Handle invalid input in MngrCashBox, e.g., display an error message
                    RecWalkinChangeBox.Text = "Invalid Input";
                }
            }
            else
            {
                // Handle invalid input in MngrGrossAmountBox, e.g., display an error message
                RecWalkinChangeBox.Text = "Invalid Input";
            }
        }
        private void RecWalkinGrossAmountBox_TextChanged(object sender, EventArgs e)
        {
            ReceptionCalculateVATAndNetAmount();
            if (decimal.TryParse(RecWalkinGrossAmountBox.Text, out decimal grossAmount))
            {
                // Get the Cash Amount from the TextBox (MngrCashBox)
                if (decimal.TryParse(RecWalkinCashBox.Text, out decimal cashAmount))
                {
                    // Calculate the Change
                    decimal change = cashAmount - grossAmount;

                    // Display the calculated change value in the MngrChangeBox
                    RecWalkinChangeBox.Text = change.ToString("0.00");
                }
                else
                {
                    // Handle invalid input in MngrCashBox, e.g., display an error message
                    RecWalkinChangeBox.Text = "Invalid Input";
                }
            }
            else
            {
                // Handle invalid input in MngrGrossAmountBox, e.g., display an error message
                RecWalkinChangeBox.Text = "Invalid Input";
            }
        }
        private void RecWalkinCCPaymentBtn_Click(object sender, EventArgs e)
        {
            if (RecWalkinCCPaymentRB.Checked == false)
            {
                RecWalkinCCPaymentRB.Visible = true;
                RecWalkinCCPaymentRB.Checked = true;
                RecWalkinTypeText.Text = "Credit Card";

                RecWalkinPPPaymentRB.Visible = false;
                RecWalkinCashPaymentRB.Visible = false;
                RecWalkinGCPaymentRB.Visible = false;
                RecWalkinPMPaymentRB.Visible = false;

                RecWalkinPPPaymentRB.Checked = false;
                RecWalkinCashPaymentRB.Checked = false;
                RecWalkinGCPaymentRB.Checked = false;
                RecWalkinPMPaymentRB.Checked = false;
            }
            else
            {
                RecWalkinCCPaymentRB.Visible = true;
                RecWalkinCCPaymentRB.Checked = true;
            }
        }

        private void RecWalkinPPPaymentBtn_Click(object sender, EventArgs e)
        {
            if (RecWalkinPPPaymentRB.Checked == false)
            {
                RecWalkinPPPaymentRB.Visible = true;
                RecWalkinPPPaymentRB.Checked = true;
                RecWalkinTypeText.Text = "Paypal";

                RecWalkinCCPaymentRB.Visible = false;
                RecWalkinCashPaymentRB.Visible = false;
                RecWalkinGCPaymentRB.Visible = false;
                RecWalkinPMPaymentRB.Visible = false;

                RecWalkinCCPaymentRB.Checked = false;
                RecWalkinCashPaymentRB.Checked = false;
                RecWalkinGCPaymentRB.Checked = false;
                RecWalkinPMPaymentRB.Checked = false;
            }
            else
            {
                RecWalkinPPPaymentRB.Visible = true;
                RecWalkinPPPaymentRB.Checked = true;
            }
        }

        private void RecWalkinCashPaymentBtn_Click(object sender, EventArgs e)
        {
            if (RecWalkinCashPaymentRB.Checked == false)
            {
                RecWalkinCashPaymentRB.Visible = true;
                RecWalkinCashPaymentRB.Checked = true;
                RecWalkinTypeText.Text = "Cash";

                RecWalkinCCPaymentRB.Visible = false;
                RecWalkinPPPaymentRB.Visible = false;
                RecWalkinGCPaymentRB.Visible = false;
                RecWalkinPMPaymentRB.Visible = false;

                RecWalkinCCPaymentRB.Checked = false;
                RecWalkinPPPaymentRB.Checked = false;
                RecWalkinGCPaymentRB.Checked = false;
                RecWalkinPMPaymentRB.Checked = false;
            }
            else
            {
                RecWalkinCashPaymentRB.Visible = true;
                RecWalkinCashPaymentRB.Checked = true;
            }
        }

        private void RecWalkinGCPaymentBtn_Click(object sender, EventArgs e)
        {
            if (RecWalkinGCPaymentRB.Checked == false)
            {
                RecWalkinGCPaymentRB.Visible = true;
                RecWalkinGCPaymentRB.Checked = true;
                RecWalkinTypeText.Text = "Gcash";

                RecWalkinCCPaymentRB.Visible = false;
                RecWalkinPPPaymentRB.Visible = false;
                RecWalkinCashPaymentRB.Visible = false;
                RecWalkinPMPaymentRB.Visible = false;

                RecWalkinCCPaymentRB.Checked = false;
                RecWalkinPPPaymentRB.Checked = false;
                RecWalkinCashPaymentRB.Checked = false;
                RecWalkinPMPaymentRB.Checked = false;
            }
            else
            {
                RecWalkinGCPaymentRB.Visible = true;
                RecWalkinGCPaymentRB.Checked = true;
            }
        }

        private void RecWalkinPMPaymentBtn_Click(object sender, EventArgs e)
        {
            if (RecWalkinPMPaymentRB.Checked == false)
            {
                RecWalkinPMPaymentRB.Visible = true;
                RecWalkinPMPaymentRB.Checked = true;
                RecWalkinTypeText.Text = "Paymaya";

                RecWalkinCCPaymentRB.Visible = false;
                RecWalkinPPPaymentRB.Visible = false;
                RecWalkinCashPaymentRB.Visible = false;
                RecWalkinGCPaymentRB.Visible = false;

                RecWalkinCCPaymentRB.Checked = false;
                RecWalkinPPPaymentRB.Checked = false;
                RecWalkinCashPaymentRB.Checked = false;
                RecWalkinGCPaymentRB.Checked = false;
            }
            else
            {
                RecWalkinPMPaymentRB.Visible = true;
                RecWalkinPMPaymentRB.Checked = true;
            }
        }

        public class PendingCustomers
        {
            public string TransactionNumber { get; set; }
            public string ClientName { get; set; }
            public string ServiceID { get; set; }
            public string ServiceName { get; set; }
            public string ServiceStatus { get; set; }
            public string CustomerCustomizations { get; set; }
            public string AdditionalNotes { get; set; }
        }

        protected void InitializePendingCustomersForStaff()
        {
            List<PendingCustomers> pendingcustomers = RetrievePendingCustomersFromDB();

            foreach (PendingCustomers customer in pendingcustomers)
            {
                StaffCurrentAvailableCustomersUserControl availablecustomersusercontrol = new StaffCurrentAvailableCustomersUserControl(this);
                availablecustomersusercontrol.AvailableCustomerSetData(customer);
                availablecustomersusercontrol.ExpandUserControlButtonClicked += AvailableCustomersUserControl_ExpandCollapseButtonClicked;
                availablecustomersusercontrol.StartServiceButtonClicked += AvailableCustomersUserControl_StartServiceButtonClicked;
                availablecustomersusercontrol.StaffEndServiceBtnClicked += AvailableCustomersUserControl_EndServiceButtonClicked;
                StaffCurrentCustomersStatusFlowLayoutPanel.Controls.Add(availablecustomersusercontrol);
                availablecustomersusercontrol.CurrentStaffID = StaffIDLbl.Text;
            }

        }

        private void AvailableCustomersUserControl_ExpandCollapseButtonClicked(object sender, EventArgs e)
        {
            StaffCurrentAvailableCustomersUserControl availablecustomersusercontrol = (StaffCurrentAvailableCustomersUserControl)sender;

            if (availablecustomersusercontrol != null)
            {
                if (!availablecustomersusercontrol.Viewing)
                {
                    availablecustomersusercontrol.Size = new System.Drawing.Size(900, 350);
                    //StaffCurrentServicesDropDownBtn.IconChar = FontAwesome.Sharp.IconChar.SquareCaretUp;
                }
                else
                {
                    availablecustomersusercontrol.Size = new System.Drawing.Size(900, 200);
                    //StaffCurrentServicesDropDownBtn.IconChar = FontAwesome.Sharp.IconChar.SquareCaretDown;

                }
            }

        }

        private List<PendingCustomers> RetrievePendingCustomersFromDB()
        {

            List<PendingCustomers> result = new List<PendingCustomers>();

            using (MySqlConnection connection = new MySqlConnection(mysqlconn))
            {
                connection.Open();

                string pendingcustomersquery = "SELECT TransactionNumber, ClientName, ServiceStatus, SelectedService, ServiceID, Customization, AddNotes FROM servicehistory WHERE ServiceStatus = 'Pending' AND ServiceCategory = @membercategory";
                MySqlCommand command = new MySqlCommand(pendingcustomersquery, connection);
                command.Parameters.AddWithValue("@membercategory", membercategory);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            PendingCustomers pendingcustomers = new PendingCustomers
                            {
                                TransactionNumber = reader.GetString("TransactionNumber"),
                                ClientName = reader.GetString("ClientName"),
                                ServiceStatus = reader.GetString("ServiceStatus"),
                                ServiceName = reader.GetString("SelectedService"),
                                CustomerCustomizations = reader.GetString("Customization"),
                                AdditionalNotes = reader.GetString("AddNotes"),
                                ServiceID = reader.GetString("ServiceID")
                            };

                            result.Add(pendingcustomers);
                        }

                    }
                }
            }

            return result;
        }



        private void AvailableCustomersUserControl_StartServiceButtonClicked(object sender, EventArgs e)
        {
            StaffCurrentAvailableCustomersUserControl insessioncustomerusercontrol = (StaffCurrentAvailableCustomersUserControl)sender;

            if (insessioncustomerusercontrol != null)
            {
                insessioncustomerusercontrol.StartTimer();
            }

        }
        private void AvailableCustomersUserControl_EndServiceButtonClicked(object sender, EventArgs e)
        {
            StaffCurrentAvailableCustomersUserControl clickedUserControl = (StaffCurrentAvailableCustomersUserControl)sender;
            TimeSpan elapsedTime = clickedUserControl.GetElapsedTime();
        }

        public void RefreshFlowLayoutPanel()
        {
            foreach (Control control in StaffCurrentCustomersStatusFlowLayoutPanel.Controls)
            {
                if (control is StaffCurrentAvailableCustomersUserControl userControl &&
                    userControl.StaffCustomerServiceStatusTextBox.Text == "In Session")
                {
                    return;
                }
            }
            StaffCurrentCustomersStatusFlowLayoutPanel.Controls.Clear();
            InitializePendingCustomersForStaff();
        }

        public void RemovePendingUserControls(StaffCurrentAvailableCustomersUserControl selectedControl)
        {
            foreach (Control control in StaffCurrentCustomersStatusFlowLayoutPanel.Controls.OfType<StaffCurrentAvailableCustomersUserControl>().ToList())
            {
                if (control != selectedControl)
                {
                    StaffCurrentCustomersStatusFlowLayoutPanel.Controls.Remove(control);
                    control.Dispose();
                }
            }
        }

        private void StaffRefreshAvailableCustomersBtn_Click(object sender, EventArgs e)
        {

            RefreshFlowLayoutPanel();
        }

        private void StaffUserAccBtn_Click(object sender, EventArgs e)
        {
            if (StaffUserAccPanel.Visible == false)
            {
                StaffUserAccPanel.Visible = true;
            }
            else
            {
                StaffUserAccPanel.Visible = false; 
            }
        }

        private void MngrSignOutBtn_Click_1(object sender, EventArgs e)
        {
            LogoutChecker();
        }

        private void MngrUserAccBtn_Click(object sender, EventArgs e)
        {
            if (MngrUserAccPanel.Visible == false)
            {
                MngrUserAccPanel.Visible = true;
            }
            else
            {
                MngrUserAccPanel.Visible = false;
            }
        }

        private void MngrInventoryProductsExitBtn_Click(object sender, EventArgs e)
        {
            Inventory.PanelShow(MngrInventoryTypePanel);
            MngrProductClearFields();
        }

        private void StaffCurrentCustomerText_TextChanged(object sender, EventArgs e)
        {

        }

        private void MngrInventoryProductData()
        {
            string connectionString = "Server=localhost;Database=enchante;User=root;Password=;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT ItemID, ProductCategory, ItemName, ItemStock, ItemPrice, ProductType, ItemStatus FROM inventory";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            // Bind the DataTable to the DataGridView
                            MngrInventoryProductsTable.DataSource = dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void MngrHomeBtn_Click(object sender, EventArgs e)
        {
            MngrHomePanelReset();
        }
        private bool shouldGenerateItemID = true;

        private void MngrInventoryProductsCatComboText_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (shouldGenerateItemID)
            {
                ProductGenerateItemID();

            }
        }
        private void ProductGenerateItemID()
        {
            string empType = AdminEmplTypeComboText.SelectedItem?.ToString() ?? string.Empty;

            string productcat = null;
            if (MngrInventoryProductsCatComboText.SelectedItem != null)
            {
                productcat = MngrInventoryProductsCatComboText.SelectedItem.ToString();
            }
            string pdcat = "";

            if (productcat == "Hair Styling")
            {
                pdcat = "HS-";
            }
            else if (productcat == "Face & Skin")
            {
                pdcat = "FS-";
            }
            else if (productcat == "Nail Care")
            {
                pdcat = "NC-";
            }
            else if (productcat == "Massage")
            {
                pdcat = "MS-";
            }
            else if (productcat == "Spa")
            {
                pdcat = "SP-";
            }

            Random random = new Random();
            int randomNumber = random.Next(100000, 999999);
            string randomNumberString = randomNumber.ToString("D5");
            string itemID = pdcat + randomNumberString;
            MngrInventoryProductsIDText.Text = itemID;
        }

        private void MngrInventoryProductsInsertBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(MngrInventoryProductsNameText.Text) || string.IsNullOrWhiteSpace(MngrInventoryProductsPriceText.Text) || string.IsNullOrWhiteSpace(MngrInventoryProductsStockText.Text) || string.IsNullOrWhiteSpace(MngrInventoryProductsIDText.Text) ||
    MngrInventoryProductsCatComboText.SelectedItem == null || MngrInventoryProductsTypeComboText.SelectedItem == null || MngrInventoryProductsStatusComboText.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!IsNumeric(MngrInventoryProductsStockText.Text))
            {
                MessageBox.Show("Invalid Number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MngrInventoryProductsPriceText.Text != "Not Applicable" && !IsNumeric(MngrInventoryProductsPriceText.Text))
            {
                MessageBox.Show("Invalid Price.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            string query = "INSERT INTO inventory (ItemID, ProductCategory, ItemName, ItemStock, ItemPrice, ProductType, ItemStatus) " +
                           "VALUES (@ItemID, @ProductCategory, @ItemName, @ItemStock, @ItemPrice, @ProductType, @ItemStatus)";

            using (MySqlConnection connection = new MySqlConnection(mysqlconn))
            {
                try
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ItemName", MngrInventoryProductsNameText.Text);
                        command.Parameters.AddWithValue("@ItemPrice", MngrInventoryProductsPriceText.Text);
                        command.Parameters.AddWithValue("@ItemStock", MngrInventoryProductsStockText.Text);
                        command.Parameters.AddWithValue("@ItemID", MngrInventoryProductsIDText.Text);
                        command.Parameters.AddWithValue("@ProductCategory", MngrInventoryProductsCatComboText.SelectedItem.ToString());
                        command.Parameters.AddWithValue("@ProductType", MngrInventoryProductsTypeComboText.SelectedItem.ToString());
                        command.Parameters.AddWithValue("@ItemStatus", MngrInventoryProductsStatusComboText.SelectedItem.ToString());

                        command.ExecuteNonQuery();

                    }
                    MessageBox.Show("Item added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    shouldGenerateItemID = false;
                    MngrInventoryProductData();
                    MngrProductClearFields();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        private void MngrInventoryProductsInfoEditBtn_Click(object sender, EventArgs e)
        {
            if (MngrInventoryProductsTable.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = MngrInventoryProductsTable.SelectedRows[0];

                bool rowIsEmpty = true;
                foreach (DataGridViewCell cell in selectedRow.Cells)
                {
                    if (!string.IsNullOrEmpty(cell.Value?.ToString()))
                    {
                        rowIsEmpty = false;
                        break;
                    }
                }

                if (rowIsEmpty)
                {
                    MessageBox.Show("The selected row is empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                shouldGenerateItemID = false;
                MngrInventoryProductsCatComboText.Enabled = false;
                MngrInventoryProductsTypeComboText.Enabled = false;
                MngrInventoryProductsIDText.Text = selectedRow.Cells["ItemID"].Value.ToString();
                MngrInventoryProductsNameText.Text = selectedRow.Cells["ItemName"].Value.ToString();
                MngrInventoryProductsPriceText.Text = selectedRow.Cells["ItemPrice"].Value.ToString();
                MngrInventoryProductsStockText.Text = selectedRow.Cells["ItemStock"].Value.ToString();
                MngrInventoryProductsCatComboText.SelectedItem = selectedRow.Cells["ProductCategory"].Value.ToString();
                MngrInventoryProductsTypeComboText.SelectedItem = selectedRow.Cells["ProductType"].Value.ToString();
                MngrInventoryProductsStatusComboText.SelectedItem = selectedRow.Cells["ItemStatus"].Value.ToString();

                MngrInventoryProductsUpdateBtn.Visible = true;
                MngrInventoryProductsInsertBtn.Visible = false;
            }
            else
            {
                MessageBox.Show("Please select a row to edit.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MngrInventoryProductsUpdateBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(MngrInventoryProductsNameText.Text) || string.IsNullOrWhiteSpace(MngrInventoryProductsPriceText.Text) || string.IsNullOrWhiteSpace(MngrInventoryProductsStockText.Text) || string.IsNullOrWhiteSpace(MngrInventoryProductsIDText.Text) ||
               MngrInventoryProductsCatComboText.SelectedItem == null || MngrInventoryProductsTypeComboText.SelectedItem == null || MngrInventoryProductsStatusComboText.SelectedItem == null)
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!IsNumeric(MngrInventoryProductsStockText.Text))
            {
                MessageBox.Show("Invalid Number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MngrInventoryProductsPriceText.Text != "Not Applicable" && !IsNumeric(MngrInventoryProductsPriceText.Text))
            {
                MessageBox.Show("Invalid Price.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string connectionString = "server=localhost;user=root;database=enchante;password=";
            string query = @"UPDATE inventory 
             SET ItemName = @ItemName, 
                 ItemPrice = @ItemPrice, 
                 ItemStock = @ItemStock, 
                 ProductCategory = @ProductCategory, 
                 ProductType = @ProductType, 
                 ItemStatus = @ItemStatus 
             WHERE ItemID = @ItemID";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ItemName", MngrInventoryProductsNameText.Text);
                    command.Parameters.AddWithValue("@ItemPrice", MngrInventoryProductsPriceText.Text);
                    command.Parameters.AddWithValue("@ItemStock", MngrInventoryProductsStockText.Text);
                    command.Parameters.AddWithValue("@ItemID", MngrInventoryProductsIDText.Text);
                    command.Parameters.AddWithValue("@ProductCategory", MngrInventoryProductsCatComboText.SelectedItem.ToString());
                    command.Parameters.AddWithValue("@ProductType", MngrInventoryProductsTypeComboText.SelectedItem.ToString());
                    command.Parameters.AddWithValue("@ItemStatus", MngrInventoryProductsStatusComboText.SelectedItem.ToString());

                    try
                    {
                        connection.Open();
                        bool fieldsChanged = false;

                        string selectQuery = "SELECT ItemName, ItemPrice, ItemStock, ProductCategory, ProductType, ItemStatus FROM inventory WHERE ItemID = @ItemID";
                        using (MySqlCommand selectCommand = new MySqlCommand(selectQuery, connection))
                        {
                            selectCommand.Parameters.AddWithValue("@ItemID", MngrInventoryProductsIDText.Text);

                            using (MySqlDataReader reader = selectCommand.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    if (reader["ItemName"].ToString() != MngrInventoryProductsNameText.Text ||
                                        reader["ItemPrice"].ToString() != MngrInventoryProductsPriceText.Text ||
                                        reader["ItemStock"].ToString() != MngrInventoryProductsStockText.Text ||
                                        reader["ProductCategory"].ToString() != MngrInventoryProductsCatComboText.SelectedItem.ToString() ||
                                        reader["ProductType"].ToString() != MngrInventoryProductsTypeComboText.SelectedItem.ToString() ||
                                        reader["ItemStatus"].ToString() != MngrInventoryProductsStatusComboText.SelectedItem.ToString())
                                    {
                                        fieldsChanged = true;
                                    }
                                }
                            }
                        }

                        if (fieldsChanged)
                        {
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Item updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                MngrInventoryProductData();
                                MngrProductClearFields();
                                MngrInventoryProductsCatComboText.Enabled = true;
                                MngrInventoryProductsTypeComboText.Enabled = true;
                                MngrInventoryProductsUpdateBtn.Visible = false;
                                MngrInventoryProductsInsertBtn.Visible = true;
                            }
                            else
                            {
                                MessageBox.Show("No rows updated.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            MessageBox.Show("No changes have been made.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void MngrProductClearFields()
        {
            MngrInventoryProductsIDText.Text = "";
            MngrInventoryProductsNameText.Text = "";
            MngrInventoryProductsPriceText.Text = "";
            MngrInventoryProductsStockText.Text = "";
            MngrInventoryProductsCatComboText.SelectedIndex = -1;
            MngrInventoryProductsTypeComboText.SelectedIndex = -1;
            MngrInventoryProductsStatusComboText.SelectedIndex = -1;
            shouldGenerateItemID = true;

        }

        private void MngrSchedExitBtn_Click(object sender, EventArgs e)
        {
            Inventory.PanelShow(MngrInventoryTypePanel);
        }

        private void MngrInventoryProductHistoryExitBtn_Click(object sender, EventArgs e)
        {
            Inventory.PanelShow(MngrInventoryTypePanel);
        }

        private void MngrPayServiceExitBtn_Click(object sender, EventArgs e)
        {
            Inventory.PanelShow(MngrInventoryTypePanel);

        }

        private void MngrInventoryServicesHistoryBtn_Click(object sender, EventArgs e)
        {
            Inventory.PanelShow(MngrPayServicePanel);

        }

        private void MngrInventoryProductsHistoryBtn_Click(object sender, EventArgs e)
        {
            Inventory.PanelShow(MngrInventoryProductHistoryPanel);

        }

        private void MngrInventoryStaffSchedBtn_Click(object sender, EventArgs e)
        {
            Inventory.PanelShow(MngrSchedPanel);

        }

        private void MngrInventoryMembershipExitBtn_Click(object sender, EventArgs e)
        {
            Inventory.PanelShow(MngrInventoryTypePanel);
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        public void InitializeStaffInventoryDataGrid()
        {
            StaffInventoryDataGrid.Rows.Clear();
           
            using (MySqlConnection connection = new MySqlConnection(mysqlconn))
            {
                connection.Open();

                string inventoryquery = "SELECT ItemID, ItemName, ItemStock, ItemPrice, ItemStatus FROM inventory WHERE ProductType = 'Service Product' AND ProductCategory = @ProductCategory";

                using (MySqlCommand command = new MySqlCommand(inventoryquery, connection))
                {
                    command.Parameters.AddWithValue("@ProductCategory", membercategory);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            object[] rowData = new object[4];
                            rowData[0] = reader["ItemID"];
                            rowData[1] = reader["ItemName"];
                            rowData[2] = reader["ItemStock"];
                            rowData[3] = reader["ItemStatus"];

                            StaffInventoryDataGrid.Rows.Add(rowData);
                        }
                    }
                }

            }

        }

        public void InitializeStaffPersonalInventoryDataGrid()
        {
            StaffPersonalInventoryDataGrid.Rows.Clear();
            string staffID = StaffIDLbl.Text;
            using (MySqlConnection connection = new MySqlConnection(mysqlconn))
            {
                connection.Open();

                string personalinventoryquery = "SELECT ItemID, ItemName, ItemStock, ItemStatus FROM staff_inventory WHERE EmployeeID = @EmployeeID";

                using (MySqlCommand command = new MySqlCommand(personalinventoryquery, connection))
                {
                    command.Parameters.AddWithValue("@EmployeeID", staffID);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            object[] rowData = new object[4];
                            rowData[0] = reader["ItemID"];
                            rowData[1] = reader["ItemName"];
                            rowData[2] = reader["ItemStock"];
                            rowData[3] = reader["ItemStatus"];

                            StaffPersonalInventoryDataGrid.Rows.Add(rowData);
                        }
                    }
                }

            }

        }


        private void StaffAddToInventoryButton_Click(object sender, EventArgs e)
        {
            if (StaffInventoryDataGrid.SelectedRows.Count > 0 && !string.IsNullOrEmpty(StaffItemSelectedCountTextBox.Text))
            {
                string itemID = StaffInventoryDataGrid.SelectedRows[0].Cells["ItemID"].Value.ToString();
                string itemName = StaffInventoryDataGrid.SelectedRows[0].Cells["ItemName"].Value.ToString();
                string itemStock = StaffInventoryDataGrid.SelectedRows[0].Cells["ItemStock"].Value.ToString();
                string itemStatus = StaffInventoryDataGrid.SelectedRows[0].Cells["ItemStatus"].Value.ToString();
                string itemStockToBeAdded = StaffItemSelectedCountTextBox.Text;
                string staffID = StaffIDLbl.Text;

                DataTable dataTable = new DataTable();
                using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                {
                    connection.Open();

                    string selectQuery = "SELECT * FROM staff_inventory WHERE ItemID = @ItemID AND EmployeeID = @EmployeeID";
                    using (MySqlCommand selectCommand = new MySqlCommand(selectQuery, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@ItemID", itemID);
                        selectCommand.Parameters.AddWithValue("@EmployeeID", staffID);

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(selectCommand))
                        {
                            adapter.Fill(dataTable);
                        }
                    }

                    if (dataTable.Rows.Count > 0)
                    {
                        int currentStock = int.Parse(dataTable.Rows[0]["ItemStock"].ToString());
                        int newStock = currentStock + int.Parse(itemStockToBeAdded);

                        string updateQuery = "UPDATE staff_inventory SET ItemStock = @NewStock WHERE ItemID = @ItemID AND EmployeeID = @EmployeeID";
                        using (MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection))
                        {
                            updateCommand.Parameters.AddWithValue("@NewStock", newStock);
                            updateCommand.Parameters.AddWithValue("@ItemID", itemID);
                            updateCommand.Parameters.AddWithValue("@EmployeeID", staffID);
                            updateCommand.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string insertQuery = "INSERT INTO staff_inventory (ItemID, ItemName, ItemStock, ItemStatus, EmployeeID) " +
                                             "VALUES (@ItemID, @ItemName, @ItemStock, @ItemStatus, @EmployeeID)";
                        using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@ItemID", itemID);
                            insertCommand.Parameters.AddWithValue("@ItemName", itemName);
                            insertCommand.Parameters.AddWithValue("@ItemStock", itemStockToBeAdded);
                            insertCommand.Parameters.AddWithValue("@ItemStatus", itemStatus);
                            insertCommand.Parameters.AddWithValue("@EmployeeID", staffID);
                            insertCommand.ExecuteNonQuery();
                        }
                    }

                    string deductQuery = "UPDATE inventory SET ItemStock = ItemStock - @SelectedCount WHERE ItemID = @ItemID";
                    using (MySqlCommand deductCommand = new MySqlCommand(deductQuery, connection))
                    {
                        deductCommand.Parameters.AddWithValue("@SelectedCount", itemStockToBeAdded);
                        deductCommand.Parameters.AddWithValue("@ItemID", itemID);
                        deductCommand.ExecuteNonQuery();
                    }
                    InitializeStaffPersonalInventoryDataGrid();
                    InitializeStaffInventoryDataGrid();
                }
            }
            else
            {
                MessageBox.Show("Please select a row in the inventory and enter a value for Selected Count.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }

}
