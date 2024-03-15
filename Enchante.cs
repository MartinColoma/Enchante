using Guna.UI2.WinForms;
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
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using Paragraph = iTextSharp.text.Paragraph;
using System.Xml.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Reflection;
using System.Windows.Controls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data.SqlClient;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing.Drawing2D;
using static Guna.UI2.WinForms.Helpers.GraphicsHelper;
using System.Collections;
using Mysqlx.Expr;
using System.Security.Policy;

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
        private string[] emplType = { "Admin", "Manager", "Receptionist", "Staff" };
        private string[] emplCategories = { "Not Applicable", "Hair Styling", "Face & Skin", "Nail Care", "Massage", "Spa" };
        private string[] emplCatLevels = { "Not Applicable", "Junior", "Assistant", "Senior" };
        private string[] productType = { "Service Product", "Retail Product" };
        private string[] productStat = { "High Stock", "Low Stock" };
        private string[] SalesDatePeriod = { "Day", "Week", "Month", "Specific Date Range" };
        private string[] SalesCategories = { "Hair Styling", "Face & Skin", "Nail Care", "Massage", "Spa", "All Categories" };
        private string[] BestCategories = { "Hair Styling", "Face & Skin", "Nail Care", "Massage", "Spa", "Top Service Category" };


        // public List<AvailableStaff> filteredbyschedstaff;
        // public Guna.UI2.WinForms.Guna2ToggleSwitch AvailableStaffActiveToggleSwitch;

        public string filterstaffbyservicecategory;
        public bool haschosenacategory = false;
        public bool servicecategorychanged;
        public string selectedStaffID;
        //private bool IsPrefferredTimeSchedComboBoxModified = false;
        public string membercategory;
        public string membertype;


        public Enchante()
        {
            InitializeComponent();

            // Exit MessageBox 
            this.FormClosing += new FormClosingEventHandler(MainForm_FormClosing);

            //Rec Walkin Buy Products
            RecWalkinSelectedProdView();

            //Landing Pages Cardlayout Panel Manager
            ParentPanelShow = new ParentCard(EnchanteHomePage, EnchanteStaffPage, EnchanteReceptionPage, EnchanteMemberPage, EnchanteAdminPage, EnchanteMngrPage);
            Registration = new Registration(MembershipPlanPanel, RegularPlanPanel, PremiumPlanPanel, SVIPPlanPanel);
            Service = new ServiceCard(ServiceType, ServiceHairStyling, ServiceFaceSkin, ServiceNailCare, ServiceSpa, ServiceMassage);
            Transaction = new ReceptionTransactionCard(RecTransactionPanel, RecWalkinPanel, RecAppointmentPanel, RecPayServicePanel, RecQueWinPanel);
            Inventory = new MngrInventoryCard(MngrInventoryTypePanel, MngrServicesPanel, MngrServiceHistoryPanel, MngrInventoryMembershipPanel, 
                                            MngrInventoryProductsPanel, MngrInventoryProductHistoryPanel, MngrSchedPanel, MngrWalkinSalesPanel, MngrIndemandPanel, MngrWalkinProdSalesPanel);



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
            MngrInventoryProductsCatComboText.Items.AddRange(Service_Category);
            MngrInventoryProductsCatComboText.DropDownStyle = ComboBoxStyle.DropDownList;
            MngrInventoryProductsTypeComboText.Items.AddRange(productType);
            MngrInventoryProductsTypeComboText.DropDownStyle = ComboBoxStyle.DropDownList;
            MngrInventoryProductsStatusComboText.Items.AddRange(productStat);
            MngrInventoryProductsStatusComboText.DropDownStyle = ComboBoxStyle.DropDownList;
            //walk-in sales comboboxes
            MngrWalkinSalesPeriod.Items.AddRange(SalesDatePeriod);
            MngrWalkinSalesPeriod.DropDownStyle = ComboBoxStyle.DropDownList;
            MngrWalkinSalesSelectCatBox.Items.AddRange(SalesCategories);
            MngrWalkinSalesSelectCatBox.DropDownStyle = ComboBoxStyle.DropDownList;
            //best employee
            MngrIndemandServiceHistoryPeriod.Items.AddRange(SalesDatePeriod);
            MngrIndemandServiceHistoryPeriod.DropDownStyle = ComboBoxStyle.DropDownList;
            MngrIndemandSelectCatBox.Items.AddRange(BestCategories);
            MngrIndemandSelectCatBox.DropDownStyle = ComboBoxStyle.DropDownList;

            //Receptionist combobox
            RecQueWinStaffCatComboText.Items.AddRange(SalesCategories);
            RecQueWinStaffCatComboText.DropDownStyle = ComboBoxStyle.DropDownList;
            RecQueWinGenCatComboText.Items.AddRange(SalesCategories);
            RecQueWinGenCatComboText.DropDownStyle = ComboBoxStyle.DropDownList;


            RecEditSchedBtn.Click += RecEditSchedBtn_Click;
            MngrStaffAvailabilityComboBox.SelectedIndex = 0;
            MngrStaffSchedComboBox.SelectedIndex = 0;




            //InitializeAvailableStaffFlowLayout();

            //RecAppPrefferedTimeAMComboBox.SelectedIndex = 0;
            //RecAppPrefferedTimePMComboBox.SelectedIndex = 0;

            //RecAppPrefferedTimeAMComboBox.SelectedIndexChanged += RecPrefferedTimeComboBox_SelectedIndexChanged;
            //RecAppPrefferedTimePMComboBox.SelectedIndexChanged += RecPrefferedTimeComboBox_SelectedIndexChanged;

            //RecAppPrefferedTimeAMComboBox.Enabled = false;
            //RecAppPrefferedTimePMComboBox.Enabled = false;

            //InitializePendingCustomersForStaff();

        }

        private void Enchante_Load(object sender, EventArgs e)
        {
            //Reset Panel to Show Default
            HomePanelReset();
            FillRecStaffScheduleViewDataGrid();
            DateTimePickerTimer.Interval = 1000;
            DateTimePickerTimer.Start();
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

        private void RecWalkinSelectedProdView()
        {
            DataGridViewButtonColumn trashColumn = new DataGridViewButtonColumn();
            trashColumn.Name = "Void";
            trashColumn.Text = "x";
            trashColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            trashColumn.Width = 10;
            RecWalkinSelectedProdDGV.Columns.Add(trashColumn);

            DataGridViewTextBoxColumn itemNameColumn = new DataGridViewTextBoxColumn();
            itemNameColumn.Name = "Item Name";
            //itemNameColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            RecWalkinSelectedProdDGV.Columns.Add(itemNameColumn);

            DataGridViewButtonColumn minusColumn = new DataGridViewButtonColumn();
            minusColumn.Name = "-";
            minusColumn.Text = "-";
            minusColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            minusColumn.Width = 10;
            RecWalkinSelectedProdDGV.Columns.Add(minusColumn);

            DataGridViewTextBoxColumn quantityColumn = new DataGridViewTextBoxColumn();
            quantityColumn.Name = "Qty";
            quantityColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            quantityColumn.Width = 15;
            RecWalkinSelectedProdDGV.Columns.Add(quantityColumn);

            DataGridViewButtonColumn plusColumn = new DataGridViewButtonColumn();
            plusColumn.Name = "+";
            plusColumn.Text = "+";
            plusColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            plusColumn.Width = 10;
            RecWalkinSelectedProdDGV.Columns.Add(plusColumn);

            DataGridViewTextBoxColumn itemUnitCostColumn = new DataGridViewTextBoxColumn();
            itemUnitCostColumn.Name = "Unit Price";
            RecWalkinSelectedProdDGV.Columns.Add(itemUnitCostColumn);

            DataGridViewTextBoxColumn itemCostColumn = new DataGridViewTextBoxColumn();
            itemCostColumn.Name = "Total Price";
            RecWalkinSelectedProdDGV.Columns.Add(itemCostColumn);

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
                return;
            }
            else if (e.KeyCode == Keys.Up)
            {
                LoginEmailAddText.Focus();
            }

        }
        private void LoginEmailAddText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                LoginPassText.Focus();
            }

        }
        private void loginchecker()
        {
            if (LoginEmailAddText.Text == "Admin" && LoginPassText.Text == "Admin123")
            {
                //Test Admin
                MessageBox.Show("Welcome back, Admin.", "Login Verified", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AdminHomePanelReset();
                AdminNameLbl.Text = "Admin Tester";
                AdminIDNumLbl.Text = "AT-0000-0000";
                PopulateUserInfoDataGrid();
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
                MngrNameLbl.Text = "Manager Tester";
                MngrIDNumLbl.Text = "MT-0000-0000";
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
                RecWalkinTransactNumRefresh();
                RecNameLbl.Text = "Receptionist Tester";
                RecIDNumLbl.Text = "RT-0000-0000";
                InitializeProducts();
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
                StaffNameLbl.Text = "Staff Tester";
                StaffIDNumLbl.Text = "ST-0000-0000";
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
                //Test Staff
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
                MemberNameLbl.Text = "Member Tester";
                MemberIDNumLbl.Text = "MT-0000-0000";
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
                                        MemberIDNumLbl.Text = ID;
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
                                        MemberIDNumLbl.Text = ID;
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
                                        MemberIDNumLbl.Text = ID;
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

                try //admin, staff, reception and manager login
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
                                        AdminIDNumLbl.Text = ID;
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
                                else if (membertype == "Receptionist")
                                {
                                    // Retrieve the HashedPass column
                                    string hashedPasswordFromDB = readerApproved["HashedPass"].ToString();

                                    // Check if the entered password matches
                                    bool passwordMatches = hashedPasswordFromDB.Equals(passchecker);

                                    if (passwordMatches)
                                    {
                                        MessageBox.Show($"Welcome back, Receptionist {name}.", "Account Verified", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        RecNameLbl.Text = name + " " + lastname;
                                        RecIDNumLbl.Text = ID;
                                        InitializeProducts();
                                        ReceptionHomePanelReset();
                                        RecWalkinTransactNumRefresh();
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
                                        StaffNameLbl.Text = name + " " + lastname;
                                        StaffIDNumLbl.Text = ID;
                                        StaffMemeberCategoryLbl.Text = category;
                                        membercategory = category;
                                        InitializeStaffInventoryDataGrid();
                                        InitializeStaffPersonalInventoryDataGrid();
                                        StaffGeneralCueCurrentCustomersStatusFlowLayoutPanel.Controls.Clear();
                                        StaffPersonalCueCurrentCustomersStatusFlowLayoutPanel.Controls.Clear();
                                        InitializePreferredCuePendingCustomersForStaff();
                                        InitializeGeneralCuePendingCustomersForStaff();
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
                    string errorMessage = "An error occurred: " + ex.Message + "\n\n" + ex.StackTrace;

                    // Copy the error message to the clipboard
                    Clipboard.SetText(errorMessage);

                    // Show a message box indicating the error and informing the user that the error message has been copied to the clipboard
                    MessageBox.Show("An error occurred. The error message has been copied to the clipboard.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            foreach (System.Windows.Forms.Control control in StaffGeneralCueCurrentCustomersStatusFlowLayoutPanel.Controls)
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
                StaffGeneralCueCurrentCustomersStatusFlowLayoutPanel.Controls.Clear();
                membercategory = "";
                StaffIDNumLbl.Text = string.Empty;
                StaffMemeberCategoryLbl.Text = string.Empty;
                StaffInventoryDataGrid.Rows.Clear();

                StaffUserAccPanel.Visible = false;
                MngrUserAccPanel.Visible = false;
                AdminUserAccPanel.Visible = false;
                MemberUserAccPanel.Visible = false;
                ReceptionUserAccPanel.Visible = false;
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
            if (MemberUserAccPanel.Visible == false)
            {
                MemberUserAccPanel.Visible = true;

            }
            else
            {
                MemberUserAccPanel.Visible = false;
            }
        }

        //Reception Panel Starts Here

        private void ReceptionLogoutBtn_Click(object sender, EventArgs e)
        {
            LogoutChecker();

        }

        private void ReceptionAccBtn_Click(object sender, EventArgs e)
        {
            if (ReceptionUserAccPanel.Visible == false)
            {
                ReceptionUserAccPanel.Visible = true;

            }
            else
            {
                ReceptionUserAccPanel.Visible = false;
            }
        }

        private void RecWalkInBtn_Click(object sender, EventArgs e)
        {
            Transaction.PanelShow(RecWalkinPanel);
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

        private void RecWalkinTransactNumRefresh()
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

        }

        private void RecTransactBtn_Click(object sender, EventArgs e)
        {
            //ScrollToCoordinates(0, 0);
            //Change color once clicked

            //Change back to original
            RecHomeBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
        }

        private void RecHomeBtn_MouseHover(object sender, EventArgs e)
        {
            iconToolTip.SetToolTip(RecHomeBtn, "Home");
        }


        private void ReceptionAccBtn_MouseHover(object sender, EventArgs e)
        {
            iconToolTip.SetToolTip(RecAccBtn, "Profile");
        }

        //Reception walk in transaction starts here
        private void RecWalkInExitBtn_Click(object sender, EventArgs e)
        {
            Transaction.PanelShow(RecTransactionPanel);
            RecWalkinTransactionClear();

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
            Inventory.PanelShow(MngrServicesPanel);
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
            filterstaffbyservicecategory = "Hair Styling";
            haschosenacategory = true;
            if (RecWalkinPreferredStaffToggleSwitch.Checked == true)
            {
                RecWalkinAttendingStaffSelectedComboBox.Items.Clear();
                LoadPreferredStaffComboBox();
            }
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
            filterstaffbyservicecategory = "Face & Skin";
            haschosenacategory = true;
            if (RecWalkinPreferredStaffToggleSwitch.Checked == true)
            {
                RecWalkinAttendingStaffSelectedComboBox.Items.Clear();
                LoadPreferredStaffComboBox();
                
            }
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
            filterstaffbyservicecategory = "Nail Care";
            haschosenacategory = true;
            if (RecWalkinPreferredStaffToggleSwitch.Checked == true)
            {
                RecWalkinAttendingStaffSelectedComboBox.Items.Clear();
                LoadPreferredStaffComboBox();
            }
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
            filterstaffbyservicecategory = "Spa";
            haschosenacategory = true;
            if (RecWalkinPreferredStaffToggleSwitch.Checked == true)
            {
                RecWalkinAttendingStaffSelectedComboBox.Items.Clear();
                LoadPreferredStaffComboBox();
            }
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
            filterstaffbyservicecategory = "Massage";
            haschosenacategory = true;
            if (RecWalkinPreferredStaffToggleSwitch.Checked == true)
            {
                RecWalkinAttendingStaffSelectedComboBox.Items.Clear();
                LoadPreferredStaffComboBox();
            }
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
        private void SearchAcrossCategories(string searchText, string category)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                {
                    connection.Open();

                    // Modify the query to search for the specified text in a specific category
                    string sql = "SELECT * FROM `services` WHERE Category = @category AND " +
                                 "(Name LIKE @searchText OR " +
                                 "Description LIKE @searchText OR " +
                                 "Duration LIKE @searchText OR " +
                                 "Price LIKE @searchText)";

                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("@searchText", "%" + searchText + "%");
                    cmd.Parameters.AddWithValue("@category", category);

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
                // Ensure the connection is closed even in case of an exception
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }


        private void RecWalkInSearchServiceTypeText_TextChanged(object sender, EventArgs e)
        {
            RecWalkinSearchServicePerCat();
        }
        private void RecWalkinSearchServicePerCat()
        {
            string searchText = RecWalkinSearchServiceTypeText.Text;
            if (RecWalkinCatHSRB.Checked)
            {
                SearchAcrossCategories(searchText, "Hair Styling");
                return;
            }
            else if (RecWalkinCatFSRB.Checked)
            {
                SearchAcrossCategories(searchText, "Face & Skin");
                return;
            }
            else if (RecWalkinCatNCRB.Checked)
            {
                SearchAcrossCategories(searchText, "Nail Care");
                return;
            }
            else if (RecWalkinCatSpaRB.Checked)
            {
                SearchAcrossCategories(searchText, "Spa");
                return;
            }
            else if (RecWalkinCatMassageRB.Checked)
            {
                SearchAcrossCategories(searchText, "Massage");
                return;
            }
        }
        private void RecWalkInSearchServiceTypeBtn_Click(object sender, EventArgs e)
        {
            RecWalkinSearchServicePerCat();
        }

        private void AdminSignOutBtn_Click_1(object sender, EventArgs e)
        {
            LogoutChecker();

        }

        private void AdminAccUserBtn_Click(object sender, EventArgs e)
        {
            if (AdminUserAccPanel.Visible == false)
            {
                AdminUserAccPanel.Visible = true;

            }
            else
            {
                AdminUserAccPanel.Visible = false;
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

                if (selectedEmpType == "Admin" || selectedEmpType == "Manager" || selectedEmpType == "Receptionist")
                {
                    AdminEmplCatComboText.Text = "Not Applicable";
                    AdminEmplCatLvlComboText.Text = "Not Applicable";
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
            else if (empType == "Receptionist")
            {
                empTypePrefix = "R-";
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

            //InitializeAvailableStaffFlowLayout();
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

        //private void InitializeAvailableStaffFlowLayout()
        //{
        //    List<AvailableStaff> availableStaff = RetrieveAvailableStaffFromDB();


        //    foreach (AvailableStaff staff in availableStaff)
        //    {
        //        if (staff.EmployeeAvailability == "Available")
        //        {
        //            AvailableStaffUserControl addedavailablestaffusercontrol = new AvailableStaffUserControl();
        //            addedavailablestaffusercontrol.AvailableStaffSetData(staff);
        //            AvailableStaffActiveToggleSwitch = addedavailablestaffusercontrol.Controls.OfType<Guna.UI2.WinForms.Guna2ToggleSwitch>().FirstOrDefault();
        //            if (AvailableStaffActiveToggleSwitch != null)
        //            {
        //                AvailableStaffActiveToggleSwitch.CheckedChanged += AvailableStaffToggleSwitch_CheckedChanged;
        //            }
        //            RecAppAvaialableStaffFlowLayout.Controls.Add(addedavailablestaffusercontrol);
        //        }

        //    }
        //}

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

            //if (RecAppPrefferedTimeAMComboBox.SelectedIndex != 0)
            //{
            //    RecAppPrefferedTimePMComboBox.Enabled = false;
            //}
            //else
            //{
            //    RecAppPrefferedTimePMComboBox.Enabled = true;
            //}
            //FilterAvailableStaffInRecFlowLayoutPanelAM();


        }

        private void RecPrefferedTimePMComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

            //if (RecAppPrefferedTimePMComboBox.SelectedIndex != 0)
            //{
            //    RecAppPrefferedTimeAMComboBox.Enabled = false;
            //}
            //else
            //{
            //    RecAppPrefferedTimeAMComboBox.Enabled = true;
            //}
            //FilterAvailableStaffInRecFlowLayoutPanelPM();


        }


        //private void FilterAvailableStaffInRecFlowLayoutPanelAM()
        //{
        //    List<AvailableStaff> availableStaff = RetrieveAvailableStaffFromDB();//DEFAULT STAFF
        //    if (RecAppPrefferedTimeAMComboBox.SelectedIndex != 0)
        //    {
        //        List<AvailableStaff> filterbyschedstaff = new List<AvailableStaff>();
        //        RecAppAvaialableStaffFlowLayout.Controls.Clear();

        //        foreach (AvailableStaff staff in availableStaff)
        //        {
        //            if (staff.EmployeeAvailability == "Available" && staff.EmployeeSchedule == "AM")
        //            {
        //                filterbyschedstaff.Add(staff);
        //                AvailableStaffUserControl addedavailablestaffusercontrol = new AvailableStaffUserControl();
        //                addedavailablestaffusercontrol.AvailableStaffSetData(staff);
        //                AvailableStaffActiveToggleSwitch = addedavailablestaffusercontrol.Controls.OfType<Guna.UI2.WinForms.Guna2ToggleSwitch>().FirstOrDefault();
        //                if (AvailableStaffActiveToggleSwitch != null)
        //                {
        //                    AvailableStaffActiveToggleSwitch.CheckedChanged += AvailableStaffToggleSwitch_CheckedChanged;
        //                }
        //                RecAppAvaialableStaffFlowLayout.Controls.Add(addedavailablestaffusercontrol);
        //            }

        //        }
        //        filteredbyschedstaff = filterbyschedstaff.ToList();
        //    }
        //    else
        //    {
        //        foreach (AvailableStaff staff in availableStaff)
        //        {
        //            if (staff.EmployeeAvailability == "Available")
        //            {
        //                AvailableStaffUserControl addedavailablestaffusercontrol = new AvailableStaffUserControl();
        //                addedavailablestaffusercontrol.AvailableStaffSetData(staff);
        //                AvailableStaffActiveToggleSwitch = addedavailablestaffusercontrol.Controls.OfType<Guna.UI2.WinForms.Guna2ToggleSwitch>().FirstOrDefault();
        //                if (AvailableStaffActiveToggleSwitch != null)
        //                {
        //                    AvailableStaffActiveToggleSwitch.CheckedChanged += AvailableStaffToggleSwitch_CheckedChanged;
        //                }
        //                RecAppAvaialableStaffFlowLayout.Controls.Add(addedavailablestaffusercontrol);
        //            }

        //        }
        //    }

        //}


        //private void FilterAvailableStaffInRecFlowLayoutPanelPM()
        //{
        //    List<AvailableStaff> availableStaff = RetrieveAvailableStaffFromDB(); // DEFAULT AVAILABLE STAFF

        //    if (RecAppPrefferedTimePMComboBox.SelectedIndex != 0)
        //    {
        //        List<AvailableStaff> filterbyschedstaff = new List<AvailableStaff>();
        //        RecAppAvaialableStaffFlowLayout.Controls.Clear();

        //        foreach (AvailableStaff staff in availableStaff)
        //        {
        //            if (staff.EmployeeAvailability == "Available" && staff.EmployeeSchedule == "PM")
        //            {
        //                filterbyschedstaff.Add(staff);
        //                AvailableStaffUserControl addedavailablestaffusercontrol = new AvailableStaffUserControl();
        //                addedavailablestaffusercontrol.AvailableStaffSetData(staff);
        //                AvailableStaffActiveToggleSwitch = addedavailablestaffusercontrol.Controls.OfType<Guna.UI2.WinForms.Guna2ToggleSwitch>().FirstOrDefault();
        //                if (AvailableStaffActiveToggleSwitch != null)
        //                {
        //                    AvailableStaffActiveToggleSwitch.CheckedChanged += AvailableStaffToggleSwitch_CheckedChanged;
        //                }
        //                RecAppAvaialableStaffFlowLayout.Controls.Add(addedavailablestaffusercontrol);
        //            }

        //        }

        //        filteredbyschedstaff = filterbyschedstaff.ToList();
        //    }
        //    else
        //    {
        //        foreach (AvailableStaff staff in availableStaff)
        //        {
        //            if (staff.EmployeeAvailability == "Available")
        //            {
        //                AvailableStaffUserControl addedavailablestaffusercontrol = new AvailableStaffUserControl();
        //                addedavailablestaffusercontrol.AvailableStaffSetData(staff);
        //                AvailableStaffActiveToggleSwitch = addedavailablestaffusercontrol.Controls.OfType<Guna.UI2.WinForms.Guna2ToggleSwitch>().FirstOrDefault();
        //                if (AvailableStaffActiveToggleSwitch != null)
        //                {
        //                    AvailableStaffActiveToggleSwitch.CheckedChanged += AvailableStaffToggleSwitch_CheckedChanged;
        //                }
        //                RecAppAvaialableStaffFlowLayout.Controls.Add(addedavailablestaffusercontrol);
        //            }

        //        }
        //    }

        //}

        //private void FilterAvailableStaffInRecFlowLayoutPanelByHairStyling()
        //{
        //    List<AvailableStaff> filteredbysched = filteredbyschedstaff.ToList();

        //    RecAppAvaialableStaffFlowLayout.Controls.Clear();

        //    foreach (AvailableStaff staff in filteredbysched)
        //    {
        //        if (staff.EmployeeCategory == "Hair Styling")
        //        {
        //            AvailableStaffUserControl addedavailablestaffusercontrol = new AvailableStaffUserControl();
        //            addedavailablestaffusercontrol.AvailableStaffSetData(staff);
        //            AvailableStaffActiveToggleSwitch = addedavailablestaffusercontrol.Controls.OfType<Guna.UI2.WinForms.Guna2ToggleSwitch>().FirstOrDefault();
        //            if (AvailableStaffActiveToggleSwitch != null)
        //            {
        //                AvailableStaffActiveToggleSwitch.CheckedChanged += AvailableStaffToggleSwitch_CheckedChanged;
        //            }
        //            RecAppAvaialableStaffFlowLayout.Controls.Add(addedavailablestaffusercontrol);
        //        }

        //    }

        //}

        //private void FilterAvailableStaffInRecFlowLayoutPanelByFaceandSkin()
        //{
        //    List<AvailableStaff> filteredbysched = filteredbyschedstaff.ToList();

        //    RecAppAvaialableStaffFlowLayout.Controls.Clear();

        //    foreach (AvailableStaff staff in filteredbysched)
        //    {
        //        if (staff.EmployeeCategory == "Face & Skin")
        //        {
        //            AvailableStaffUserControl addedavailablestaffusercontrol = new AvailableStaffUserControl();
        //            addedavailablestaffusercontrol.AvailableStaffSetData(staff);
        //            AvailableStaffActiveToggleSwitch = addedavailablestaffusercontrol.Controls.OfType<Guna.UI2.WinForms.Guna2ToggleSwitch>().FirstOrDefault();
        //            if (AvailableStaffActiveToggleSwitch != null)
        //            {
        //                AvailableStaffActiveToggleSwitch.CheckedChanged += AvailableStaffToggleSwitch_CheckedChanged;
        //            }
        //            RecAppAvaialableStaffFlowLayout.Controls.Add(addedavailablestaffusercontrol);
        //        }

        //    }

        //}

        //private void FilterAvailableStaffInRecFlowLayoutPanelByNailCare()
        //{
        //    List<AvailableStaff> filteredbysched = filteredbyschedstaff.ToList();

        //    RecAppAvaialableStaffFlowLayout.Controls.Clear();

        //    foreach (AvailableStaff staff in filteredbysched)
        //    {
        //        if (staff.EmployeeCategory == "Nail Care")
        //        {
        //            AvailableStaffUserControl addedavailablestaffusercontrol = new AvailableStaffUserControl();
        //            addedavailablestaffusercontrol.AvailableStaffSetData(staff);
        //            AvailableStaffActiveToggleSwitch = addedavailablestaffusercontrol.Controls.OfType<Guna.UI2.WinForms.Guna2ToggleSwitch>().FirstOrDefault();
        //            if (AvailableStaffActiveToggleSwitch != null)
        //            {
        //                AvailableStaffActiveToggleSwitch.CheckedChanged += AvailableStaffToggleSwitch_CheckedChanged;
        //            }
        //            RecAppAvaialableStaffFlowLayout.Controls.Add(addedavailablestaffusercontrol);
        //        }

        //    }

        //}

        //private void FilterAvailableStaffInRecFlowLayoutPanelByMassage()
        //{
        //    List<AvailableStaff> filteredbysched = filteredbyschedstaff.ToList();

        //    RecAppAvaialableStaffFlowLayout.Controls.Clear();

        //    foreach (AvailableStaff staff in filteredbysched)
        //    {
        //        if (staff.EmployeeCategory == "Massage")
        //        {
        //            AvailableStaffUserControl addedavailablestaffusercontrol = new AvailableStaffUserControl();
        //            addedavailablestaffusercontrol.AvailableStaffSetData(staff);
        //            AvailableStaffActiveToggleSwitch = addedavailablestaffusercontrol.Controls.OfType<Guna.UI2.WinForms.Guna2ToggleSwitch>().FirstOrDefault();
        //            if (AvailableStaffActiveToggleSwitch != null)
        //            {
        //                AvailableStaffActiveToggleSwitch.CheckedChanged += AvailableStaffToggleSwitch_CheckedChanged;
        //            }
        //            RecAppAvaialableStaffFlowLayout.Controls.Add(addedavailablestaffusercontrol);
        //        }

        //    }

        //}

        //private void FilterAvailableStaffInRecFlowLayoutPanelBySpa()
        //{
        //    List<AvailableStaff> filteredbysched = filteredbyschedstaff.ToList();

        //    RecAppAvaialableStaffFlowLayout.Controls.Clear();

        //    foreach (AvailableStaff staff in filteredbysched)
        //    {
        //        if (staff.EmployeeCategory == "Spa")
        //        {
        //            AvailableStaffUserControl addedavailablestaffusercontrol = new AvailableStaffUserControl();
        //            addedavailablestaffusercontrol.AvailableStaffSetData(staff);
        //            AvailableStaffActiveToggleSwitch = addedavailablestaffusercontrol.Controls.OfType<Guna.UI2.WinForms.Guna2ToggleSwitch>().FirstOrDefault();
        //            if (AvailableStaffActiveToggleSwitch != null)
        //            {
        //                AvailableStaffActiveToggleSwitch.CheckedChanged += AvailableStaffToggleSwitch_CheckedChanged;
        //            }
        //            RecAppAvaialableStaffFlowLayout.Controls.Add(addedavailablestaffusercontrol);
        //        }

        //    }

        //}

        //private void AvailableStaffToggleSwitch_CheckedChanged(object sender, EventArgs e)
        //{
        //    Guna.UI2.WinForms.Guna2ToggleSwitch toggleSwitch = (Guna.UI2.WinForms.Guna2ToggleSwitch)sender;
        //    System.Windows.Forms.UserControl userControl = (System.Windows.Forms.UserControl)toggleSwitch.Parent;

        //    if (toggleSwitch.Checked)
        //    {
        //        if (AvailableStaffActiveToggleSwitch != null && AvailableStaffActiveToggleSwitch != toggleSwitch)
        //        {
        //            AvailableStaffActiveToggleSwitch.Checked = false;
        //        }
        //        AvailableStaffActiveToggleSwitch = toggleSwitch;
        //    }
        //    else if (AvailableStaffActiveToggleSwitch == toggleSwitch)
        //    {
        //        AvailableStaffActiveToggleSwitch = null;
        //    }
        //}

        //private void RecPrefferedTimeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //    IsPrefferredTimeSchedComboBoxModified = true;
        //}

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
            if (string.IsNullOrEmpty(selectedStaffID))
            {
                MessageBox.Show("Please select a prefered staff or toggle anyone ", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            if (RecWalkinAttendingStaffSelectedComboBox.SelectedItem?.ToString() == "Select a Preferred Staff") // 4942
            {
                MessageBox.Show("Please select a preferred staff or toggle anyone.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (DataGridViewRow row in RecSelectedServiceDataGrid1.Rows)
            {
                string existingServiceID = row.Cells["ServiceID"]?.Value?.ToString(); // Use null-conditional operator

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

                string appointmentDate = DateTime.Now.ToString("MM-dd-yyyy dddd");
                string serviceCategory = SelectedCategory;
                int latestquenumber = GetLargestQueNum(appointmentDate, serviceCategory);

                NewSelectedServiceRow.Cells["ServicePrice"].Value = ServicePrice;
                NewSelectedServiceRow.Cells["ServiceCategory"].Value = SelectedCategory;
                NewSelectedServiceRow.Cells["SelectedService"].Value = ServiceName;
                NewSelectedServiceRow.Cells["ServiceID"].Value = ServiceID;
                NewSelectedServiceRow.Cells["QueNumber"].Value = latestquenumber;
                NewSelectedServiceRow.Cells["StaffSelected"].Value = selectedStaffID;
                QueTypeIdentifier(NewSelectedServiceRow.Cells["QueType"]);

                //RecWalkinPreferredStaffToggleSwitch.Checked = false;
                //RecWalkinAnyStaffToggleSwitch.Checked = false;
                //selectedStaffID = string.Empty;
                //RecWalkinAttendingStaffSelectedComboBox.Items.Clear();
                RecWalkInServiceTypeTable.ClearSelection();
                RecCustomerCustomizationsTextBox.Clear();


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

        private int GetLargestQueNum(string appointmentDate, string serviceCategory)
        {
            using (MySqlConnection connection = new MySqlConnection(mysqlconn))
            {
                connection.Open();

                using (MySqlCommand command = connection.CreateCommand())
                {
                    string query = "SELECT MAX(QueNumber) FROM servicehistory WHERE AppointmentDate = @AppointmentDate AND ServiceCategory = @ServiceCategory";
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@AppointmentDate", appointmentDate);
                    command.Parameters.AddWithValue("@ServiceCategory", serviceCategory);

                    object result = command.ExecuteScalar();
                    int largestquenumber = result != DBNull.Value ? Convert.ToInt32(result) : 0;

                    if (largestquenumber > 0)
                    {
                        largestquenumber++;
                    }
                    else
                    {
                        largestquenumber = 1;
                    }

                    return largestquenumber;
                }
            }
        }
        private void QueTypeIdentifier(DataGridViewCell QueType)
        {
            if (selectedStaffID == "Anyone")
            {
                QueType.Value = "GeneralQue";
            }
            else
            {
                QueType.Value = "Preferred";
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
        }
        private bool IsCardNameValid(string name)
        {
            foreach (char c in name)
            {
                if (!char.IsLetter(c))
                {
                    return false;
                }
            }
            return true;
        }
        private void RecWalkinBookTransactBtn_Click(object sender, EventArgs e)
        {
            
            if (string.IsNullOrWhiteSpace(RecWalkinFNameText.Text))
            {
                MessageBox.Show("Please enter a first name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!IsCardNameValid(RecWalkinFNameText.Text))
            {
                MessageBox.Show("Invalid First Name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (string.IsNullOrWhiteSpace(RecWalkinLNameText.Text))
            {
                MessageBox.Show("Please enter a last name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!IsCardNameValid(RecWalkinLNameText.Text))
            {
                MessageBox.Show("Invalid Last Name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (string.IsNullOrWhiteSpace(RecWalkinCPNumText.Text))
            {
                MessageBox.Show("Please enter a contact number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!IsNumeric(RecWalkinCPNumText.Text))
            {
                MessageBox.Show("Invalid Contact Number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (RecSelectedServiceDataGrid1 != null && RecSelectedServiceDataGrid1.Rows.Count == 0)
            {
                MessageBox.Show("Select a service first to proceed on booking a transaction.", "Ooooops!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                RecWalkinServiceHistoryDB(RecSelectedServiceDataGrid1); //service history db
                ReceptionistWalk_in_AppointmentDB(); //walk-in transaction db
                RecWalkinOrderProdHistoryDB(RecWalkinSelectedProdDGV);
                RecWalkinTransactNumRefresh();
                RecWalkinTransactionClear();
            }
        }

        private void RecWalkinTransactionClear()
        {
            RecWalkinFNameText.Text = "";
            RecWalkinLNameText.Text = "";
            RecWalkinCPNumText.Text = "";
            RecWalkinCatHSRB.Checked = false;
            RecWalkinCatFSRB.Checked = false;
            RecWalkinCatNCRB.Checked = false;
            RecWalkinCatSpaRB.Checked = false;
            RecWalkinCatMassageRB.Checked = false;
            RecSelectedServiceDataGrid1.Rows.Clear();
            RecWalkinSelectedProdDGV.Rows.Clear();

        }
        private void QueueNumReceiptGenerator()
        {
            DateTime currentDate = RecDateTimePicker.Value;
            string datetoday = currentDate.ToString("MM-dd-yyyy dddd");
            string timePrinted = currentDate.ToString("hh:mm tt");
            string timePrintedFile = currentDate.ToString("hh-mm-ss");
            string transactNum = RecPayServiceTransactNumLbl.Text;
            string clientName = RecPayServiceClientNameLbl.Text;
            string receptionName = RecNameLbl.Text;
            string legal = "Thank you for trusting Enchanté Salon for your beauty needs." +
                " This receipt will serve as your sales invoice of any services done in Enchanté Salon." +
                " Any concerns about your services please ask and show this receipt in the frontdesk of Enchanté Salon.";
            // Increment the file name

            // Generate a unique filename for the PDF
            string fileName = $"Enchanté-Receipt-{transactNum}-{timePrintedFile}.pdf";

            // Create a SaveFileDialog to choose the save location
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF Files|*.pdf";
            saveFileDialog.FileName = fileName;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;

                // Create a new document with custom page size (8.5"x4.25" in landscape mode)
                Document doc = new Document(new iTextSharp.text.Rectangle(Utilities.MillimetersToPoints(133f), Utilities.MillimetersToPoints(203f)));

                try
                {
                    // Create a PdfWriter instance
                    PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));

                    // Open the document for writing
                    doc.Open();

                    //string imagePath = "C:\\Users\\Pepper\\source\\repos\\Enchante\\Resources\\Enchante Logo (200 x 200 px) (1).png"; // Replace with the path to your logo image
                    // Load the image from project resources
                    //if (File.Exists(imagePath))
                    //{
                    //    //iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(imagePath);
                    //}

                    // Load the image from project resources
                    byte[] imageBytes = GetImageBytesFromResource("Enchante.Resources.Enchante Logo (200 x 200 px) (1).png");

                    if (imageBytes != null)
                    {
                        iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(imageBytes);
                        logo.ScaleAbsolute(50f, 50f);
                        logo.Alignment = Element.ALIGN_CENTER;
                        doc.Add(logo);
                    }
                    else
                    {
                        MessageBox.Show("Error loading image from resources.", "Manager Receipt Generator Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    };

                    iTextSharp.text.Font headerFont = FontFactory.GetFont("Courier", 16, iTextSharp.text.Font.BOLD);
                    iTextSharp.text.Font boldfont = FontFactory.GetFont("Courier", 10, iTextSharp.text.Font.BOLD);
                    iTextSharp.text.Font font = FontFactory.GetFont("Courier", 10, iTextSharp.text.Font.NORMAL);

                    // Create a centered alignment for text
                    iTextSharp.text.Paragraph centerAligned = new Paragraph();
                    centerAligned.Alignment = Element.ALIGN_CENTER;

                    // Add centered content to the centerAligned Paragraph
                    centerAligned.Add(new Chunk("Enchanté Salon", headerFont));
                    centerAligned.Add(new Chunk("\n69th flr. Enchanté Bldg. Ortigas Extension Ave. \nManggahan, Pasig City 1611 Philippines", font));
                    centerAligned.Add(new Chunk("\nTel. No.: (1101) 111-1010", font));
                    centerAligned.Add(new Chunk($"\nDate: {datetoday} Time: {timePrinted}", font));

                    // Add the centered content to the document
                    doc.Add(centerAligned);
                    doc.Add(new Chunk("\n")); // New line

                    doc.Add(new Paragraph($"Transaction No.: {transactNum}", font));
                    //doc.Add(new Paragraph($"Order Date: {today}", font));
                    doc.Add(new Paragraph($"Reception Name: {receptionName}", font));
                    doc.Add(new Chunk("\n")); // New line

                    doc.Add(new LineSeparator()); // Dotted line
                    PdfPTable itemTable = new PdfPTable(3); // 3 columns for the item table
                    itemTable.SetWidths(new float[] { 5f, 10f, 5f }); // Column widths
                    itemTable.DefaultCell.Border = PdfPCell.NO_BORDER;
                    itemTable.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
                    itemTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    itemTable.AddCell(new Phrase("Staff ID", boldfont));
                    itemTable.AddCell(new Phrase("Service", boldfont));
                    itemTable.AddCell(new Phrase("Price", boldfont));
                    doc.Add(itemTable);
                    doc.Add(new LineSeparator()); // Dotted line
                    // Iterate through the rows of your 
                    foreach (DataGridViewRow row in RecPayServicesAcquiredDGV.Rows)
                    {
                        try
                        {
                            string itemName = row.Cells["SelectedService"].Value?.ToString();
                            if (string.IsNullOrEmpty(itemName))
                            {
                                continue; // Skip empty rows
                            }

                            string staffID = row.Cells["AttendingStaff"].Value?.ToString();
                            string itemTotalcost = row.Cells["ServicePrice"].Value?.ToString();

                            // Add cells to the item table
                            PdfPTable serviceTable = new PdfPTable(3); // 4 columns for the item table
                            serviceTable.SetWidths(new float[] { 3f, 5f, 3f }); // Column widths
                            serviceTable.DefaultCell.Border = PdfPCell.NO_BORDER;
                            serviceTable.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
                            serviceTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            serviceTable.AddCell(new Phrase(staffID, font));
                            serviceTable.AddCell(new Phrase(itemName, font));
                            serviceTable.AddCell(new Phrase(itemTotalcost, font));

                            // Add the item table to the document
                            doc.Add(serviceTable);
                        }
                        catch (Exception ex)
                        {
                            // Handle or log any exceptions that occur while processing DataGridView data
                            MessageBox.Show("An error occurred: " + ex.Message, "Receipt Generator Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    doc.Add(new Chunk("\n")); // New line
                    doc.Add(new LineSeparator()); // Dotted line
                    doc.Add(new Chunk("\n")); // New line

                    // Total from your textboxes as decimal
                    decimal netAmount = decimal.Parse(MngrPayServiceNetAmountBox.Text);
                    decimal discount = decimal.Parse(RecWalkinDiscountBox.Text);
                    decimal vat = decimal.Parse(MngrPayServiceVATBox.Text);
                    decimal grossAmount = decimal.Parse(MngrPayServiceGrossAmountBox.Text);
                    decimal cash = decimal.Parse(MngrPayServiceCashBox.Text);
                    decimal change = decimal.Parse(MngrPayServiceChangeBox.Text);

                    // Create a new table for the "Total" section
                    PdfPTable totalTable = new PdfPTable(2); // 2 columns for the "Total" table
                    totalTable.SetWidths(new float[] { 5f, 3f }); // Column widths
                    totalTable.DefaultCell.Border = PdfPCell.NO_BORDER;

                    // Add cells to the "Total" table
                    totalTable.AddCell(new Phrase($"Total # of Service ({RecPayServicesAcquiredDGV.Rows.Count})", font));
                    totalTable.AddCell(new Phrase($"Php {grossAmount:F2}", font));
                    totalTable.AddCell(new Phrase($"Cash Given", font));
                    totalTable.AddCell(new Phrase($"Php {cash:F2}", font));
                    totalTable.AddCell(new Phrase($"Change", font));
                    totalTable.AddCell(new Phrase($"Php {change:F2}", font));

                    // Add the "Total" table to the document
                    doc.Add(totalTable);
                    doc.Add(new Chunk("\n")); // New line

                    // Create a new table for the "VATable" section
                    PdfPTable vatTable = new PdfPTable(2); // 2 columns for the "VATable" table
                    vatTable.SetWidths(new float[] { 5f, 3f }); // Column widths
                    vatTable.DefaultCell.Border = PdfPCell.NO_BORDER;

                    // Add cells to the "VATable" table
                    vatTable.AddCell(new Phrase("VATable ", font));
                    vatTable.AddCell(new Phrase($"Php {netAmount:F2}", font));
                    vatTable.AddCell(new Phrase("VAT Tax (12%)", font));
                    vatTable.AddCell(new Phrase($"Php {vat:F2}", font));
                    vatTable.AddCell(new Phrase("Discount (20%)", font));
                    vatTable.AddCell(new Phrase($"Php {discount:F2}", font));

                    // Add the "VATable" table to the document
                    doc.Add(vatTable);


                    // Add the "Served To" section
                    doc.Add(new Chunk("\n")); // New line
                    doc.Add(new Paragraph($"Served To: {clientName}", font));
                    doc.Add(new Paragraph("Address:_______________________________", font));
                    doc.Add(new Paragraph("TIN No.:_______________________________", font));

                    // Add the legal string with center alignment
                    Paragraph paragraph_footer = new Paragraph($"\n\n{legal}", font);
                    paragraph_footer.Alignment = Element.ALIGN_CENTER;
                    doc.Add(paragraph_footer);
                }
                catch (DocumentException de)
                {
                    MessageBox.Show("An error occurred: " + de.Message, "Receipt Generator Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (IOException ioe)
                {
                    MessageBox.Show("An error occurred: " + ioe.Message, "Receipt Generator Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // Close the document
                    doc.Close();
                }

                //MessageBox.Show($"Receipt saved as {filePath}", "Receipt Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void RecWalkinOrderProdHistoryDB(DataGridView RecWalkinOrderHistoryView)
        {
            DateTime currentDate = RecDateTimePicker.Value;
            string transactionNum = RecWalkinTransNumText.Text;
            string status = "Not Paid";

            //booked values
            string bookedDate = currentDate.ToString("MM-dd-yyyy dddd"); //bookedDate
            string bookedTime = currentDate.ToString("hh:mm tt"); //bookedTime
            string bookedBy = RecNameLbl.Text; //booked by

            //basic info
            string CustomerName = RecWalkinFNameText.Text + " " + RecWalkinLNameText.Text; //client name
            string yes = "Yes";
            string no = "No";
            if (RecWalkinSelectedProdDGV.Rows.Count > 0)
            {
                try
                {
                    using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                    {
                        connection.Open();

                        foreach (DataGridViewRow row in RecWalkinSelectedProdDGV.Rows)
                        {
                            if (row.Cells["Item Name"].Value != null)
                            {
                                string itemName = row.Cells["Item Name"].Value.ToString();
                                int qty = Convert.ToInt32(row.Cells["Qty"].Value);
                                decimal itemPrice = Convert.ToDecimal(row.Cells["Unit Price"].Value);
                                decimal itemTotalPrice = Convert.ToDecimal(row.Cells["Total Price"].Value);
                                string itemID = row.Cells["OrderProdItemID"].Value.ToString();


                                string query = "INSERT INTO orderproducthistory (TransactionNumber, ProductStatus, CheckedOutDate, CheckedOutTime, CheckedOutBy, ClientName, ItemID, ItemName, Qty, ItemPrice, ItemTotalPrice, CheckedOut, Voided) " +
                                                 "VALUES (@Transact, @status, @date, @time, @OrderedBy, @client, @ID, @ItemName, @Qty, @ItemPrice, @ItemTotalPrice, @Yes, @No)";

                                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                                {
                                    cmd.Parameters.AddWithValue("@Transact", transactionNum);
                                    cmd.Parameters.AddWithValue("@status", status);
                                    cmd.Parameters.AddWithValue("@date", bookedDate);
                                    cmd.Parameters.AddWithValue("@time", bookedTime);
                                    cmd.Parameters.AddWithValue("@OrderedBy", bookedBy);
                                    cmd.Parameters.AddWithValue("@client", CustomerName);
                                    cmd.Parameters.AddWithValue("@ID", itemID);
                                    cmd.Parameters.AddWithValue("@ItemName", itemName);
                                    cmd.Parameters.AddWithValue("@Qty", qty);
                                    cmd.Parameters.AddWithValue("@ItemPrice", itemPrice);
                                    cmd.Parameters.AddWithValue("@ItemTotalPrice", itemTotalPrice);
                                    cmd.Parameters.AddWithValue("@Yes", yes);
                                    cmd.Parameters.AddWithValue("@No", no);

                                    cmd.ExecuteNonQuery();
                                }

                            }
                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    string errorMessage = "An error occurred: " + ex.Message + "\n\n" + ex.StackTrace;
                    MessageBox.Show(errorMessage, "Login Verifier", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
            else
            {
                MessageBox.Show("No products bought.", "Product");
            }

        }


        private void ReceptionistWalk_in_AppointmentDB()
        {
            DateTime currentDate = RecDateTimePicker.Value;
            string transactionNum = RecWalkinTransNumText.Text;
            string serviceStatus = "Pending";

            //basic info
            string CustomerName = RecWalkinFNameText.Text + " " + RecWalkinLNameText.Text; //client name
            string CustomerMobileNumber = RecWalkinCPNumText.Text; //client cp num

            //booked values
            string bookedDate = currentDate.ToString("MM-dd-yyyy dddd"); //bookedDate
            string bookedTime = currentDate.ToString("hh:mm tt"); //bookedTime
            string bookedBy = RecNameLbl.Text; //booked by

            //customize & add notes
            string custom = RecCustomerCustomizationsTextBox.Text;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                {
                    connection.Open();
                    string insertQuery = "INSERT INTO walk_in_appointment (TransactionNumber, ServiceStatus, AppointmentDate, AppointmentTime, " +
                                        "ClientName, CustomerCustomizations, ClientCPNum, ServiceDuration, BookedBy, BookedDate, BookedTime)" +
                                        "VALUES (@Transact, @status, @appointDate, @appointTime, @clientName, @custom, @clientCP, @duration, @bookedBy, @bookedDate, @bookedTime)";

                    MySqlCommand cmd = new MySqlCommand(insertQuery, connection);
                    cmd.Parameters.AddWithValue("@Transact", transactionNum);
                    cmd.Parameters.AddWithValue("@status", serviceStatus);
                    cmd.Parameters.AddWithValue("@appointDate", bookedDate);
                    cmd.Parameters.AddWithValue("@appointTime", bookedTime);
                    cmd.Parameters.AddWithValue("@clientName", CustomerName);
                    cmd.Parameters.AddWithValue("@custom", custom);
                    cmd.Parameters.AddWithValue("@clientCP", CustomerMobileNumber);
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
                                string selectedStaff = row.Cells["StaffSelected"].Value.ToString();
                                string queNumber = row.Cells["QueNumber"].Value.ToString();
                                string queType = row.Cells["QueType"].Value.ToString();

                                string insertQuery = "INSERT INTO servicehistory (TransactionNumber, ServiceStatus, AppointmentDate, AppointmentTime, ClientName, " +
                                                     "ServiceCategory, ServiceID, SelectedService, ServicePrice, Customization, PreferredStaff, QueNumber," +
                                                     "QueType" +
                                                     ") VALUES (@Transact, @status, @appointDate, @appointTime, @name, @serviceCat, @ID, @serviceName, @servicePrice, " +
                                                     "@custom, @preferredstaff, @quenumber, @quetype)";

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
                                cmd.Parameters.AddWithValue("@preferredstaff", selectedStaff);
                                cmd.Parameters.AddWithValue("@quenumber", queNumber);
                                cmd.Parameters.AddWithValue("@quetype", queType);

                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message, "Receptionist Service failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
            else
            {
                MessageBox.Show("No items to insert into the database.", "Service");
            }

        }

        private void ReceptionCalculateTotalPrice()
        {
            decimal total1 = 0;
            decimal total2 = 0;
            decimal total3 = 0;

            // Assuming the "ServicePrice" column is of decimal type
            int servicepriceColumnIndex = RecPayServicesAcquiredDGV.Columns["ServicePrice"].Index;

            foreach (DataGridViewRow row in RecPayServicesAcquiredDGV.Rows)
            {
                if (row.Cells[servicepriceColumnIndex].Value != null)
                {
                    decimal price = decimal.Parse(row.Cells[servicepriceColumnIndex].Value.ToString());
                    total1 += price;
                }
            }
            RecPayServicesAcquiredTotalText.Text = total1.ToString("F2");

            // Assuming the "ItemTotalPrice" column is of decimal type
            int productpriceColumnIndex = RecPayServiceCOProdDGV.Columns["ItemTotalPrice"].Index;

            foreach (DataGridViewRow row in RecPayServiceCOProdDGV.Rows)
            {
                if (row.Cells[productpriceColumnIndex].Value != null)
                {
                    decimal price = decimal.Parse(row.Cells[productpriceColumnIndex].Value.ToString());
                    total2 += price;
                }
            }
            RecPayServicesCOProdTotalText.Text = total2.ToString("F2");


            total3 = total1 + total2;

            // Display the total price in the GrossAmountBox TextBox
            MngrPayServiceGrossAmountBox.Text = total3.ToString("F2"); // Format to two decimal places

            ReceptionCalculateVATAndNetAmount();
        }


        public void ReceptionCalculateVATAndNetAmount()
        {
            // Get the Gross Amount from the TextBox (MngrGrossAmountBox)
            if (decimal.TryParse(MngrPayServiceGrossAmountBox.Text, out decimal grossAmount))
            {
                // Fixed VAT rate of 12%
                decimal rate = 12;

                // Calculate the VAT Amount
                decimal netAmount = grossAmount / ((rate / 100) + 1);

                // Calculate the Net Amount
                decimal vatAmount = grossAmount - netAmount;

                // Display the calculated values in TextBoxes
                MngrPayServiceVATBox.Text = vatAmount.ToString("0.00");
                MngrPayServiceNetAmountBox.Text = netAmount.ToString("0.00");
                MngrPayServiceVATBox.Text = vatAmount.ToString("0.00");
                MngrPayServiceNetAmountBox.Text = netAmount.ToString("0.00");
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
            if (decimal.TryParse(MngrPayServiceGrossAmountBox.Text, out decimal grossAmount))
            {
                if (MngrPayServiceDiscountSenior.Checked && !discountApplied)
                {
                    // Apply the 20% discount if the checkbox is checked and the discount hasn't been applied before
                    originalGrossAmount = grossAmount; // Store the original value
                    decimal discountPercentage = 20m;
                    decimal discountAmount = grossAmount * (discountPercentage / 100); // Calculate the discount amount
                    decimal discountedAmount = grossAmount - discountAmount; // Subtract the discount amount
                    MngrPayServiceGrossAmountBox.Text = discountedAmount.ToString("0.00"); // Format to display as currency
                    discountApplied = true; // Set the flag to indicate that the discount has been applied
                    RecWalkinDiscountBox.Text = discountAmount.ToString("0.00"); // Display the discount amount
                }
                else if (!MngrPayServiceDiscountSenior.Checked && discountApplied)
                {
                    // Unchecked, set MngrGrossAmount to the original value if the discount has been applied before
                    MngrPayServiceGrossAmountBox.Text = originalGrossAmount.ToString("0.00");
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
            if (decimal.TryParse(MngrPayServiceGrossAmountBox.Text, out decimal grossAmount))
            {
                if (MngrPayServiceDiscountPWD.Checked && !discountApplied)
                {
                    // Apply the 20% discount if the checkbox is checked and the discount hasn't been applied before
                    originalGrossAmount = grossAmount; // Store the original value
                    decimal discountPercentage = 20m;
                    decimal discountAmount = grossAmount * (discountPercentage / 100); // Calculate the discount amount
                    decimal discountedAmount = grossAmount - discountAmount; // Subtract the discount amount
                    MngrPayServiceGrossAmountBox.Text = discountedAmount.ToString("0.00"); // Format to display as currency
                    discountApplied = true; // Set the flag to indicate that the discount has been applied
                    RecWalkinDiscountBox.Text = discountAmount.ToString("0.00"); // Display the discount amount
                }
                else if (!MngrPayServiceDiscountPWD.Checked && discountApplied)
                {
                    // Unchecked, set MngrGrossAmount to the original value if the discount has been applied before
                    MngrPayServiceGrossAmountBox.Text = originalGrossAmount.ToString("0.00");
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
            if (decimal.TryParse(MngrPayServiceGrossAmountBox.Text, out decimal grossAmount))
            {
                // Get the Cash Amount from the TextBox (MngrCashBox)
                if (decimal.TryParse(MngrPayServiceCashBox.Text, out decimal cashAmount))
                {
                    // Calculate the Change
                    decimal change = cashAmount - grossAmount;

                    // Display the calculated change value in the MngrChangeBox
                    MngrPayServiceChangeBox.Text = change.ToString("0.00");
                }
                else
                {
                    // Handle invalid input in MngrCashBox, e.g., display an error message
                    MngrPayServiceChangeBox.Text = "0.00";
                }
            }
            else
            {
                // Handle invalid input in MngrGrossAmountBox, e.g., display an error message
                MngrPayServiceChangeBox.Text = "0.00";
            }
        }
        private void RecWalkinGrossAmountBox_TextChanged(object sender, EventArgs e)
        {
            //ReceptionCalculateVATAndNetAmount();
            if (decimal.TryParse(MngrPayServiceGrossAmountBox.Text, out decimal grossAmount))
            {
                // Get the Cash Amount from the TextBox (MngrCashBox)
                if (decimal.TryParse(MngrPayServiceCashBox.Text, out decimal cashAmount))
                {
                    // Calculate the Change
                    decimal change = cashAmount - grossAmount;

                    // Display the calculated change value in the MngrChangeBox
                    MngrPayServiceChangeBox.Text = change.ToString("0.00");
                }
                else
                {
                    // Handle invalid input in MngrCashBox, e.g., display an error message
                    MngrPayServiceChangeBox.Text = "0.00";
                }
            }
            else
            {
                // Handle invalid input in MngrGrossAmountBox, e.g., display an error message
                MngrPayServiceChangeBox.Text = "0.00";
            }
        }
        private void RecWalkinCCPaymentBtn_Click(object sender, EventArgs e)
        {
            if (RecPayServiceCCPaymentRB.Checked == false)
            {
                RecPayServiceCCPaymentRB.Visible = true;
                RecPayServiceCCPaymentRB.Checked = true;
                RecPayServiceTypeText.Text = "Credit Card";
                RecPayServiceBankPaymentPanel.Visible = true;


                //disable other payment panel
                RecPayServiceWalletPaymentPanel.Visible = false;
                MngrPayServiceCashLbl.Visible = false;
                MngrPayServiceCashBox.Visible = false;
                MngrPayServiceCashBox.Text = "0";
                MngrPayServiceChangeBox.Text = "0.00";
                MngrPayServiceCardNameText.Text = "";
                MngrPayServiceCardNumText.Text = "";
                MngrPayServiceCVCText.Text = "";
                MngrPayServiceCardExpText.Text = "MM/YY";
                MngrPayServiceWalletNumText.Text = "";
                MngrPayServiceWalletPINText.Text = "";
                MngrPayServiceWalletOTPText.Text = "";
                MngrPayServiceChangeLbl.Visible = false;
                MngrPayServiceChangeBox.Visible = false;

                //disable radio buttons
                RecPayServicePPPaymentRB.Visible = false;
                RecPayServiceCashPaymentRB.Visible = false;
                RecPayServiceGCPaymentRB.Visible = false;
                MngrPayServicePMPaymentRB.Visible = false;
                RecPayServicePPPaymentRB.Checked = false;
                RecPayServiceCashPaymentRB.Checked = false;
                RecPayServiceGCPaymentRB.Checked = false;
                MngrPayServicePMPaymentRB.Checked = false;
            }
            else
            {
                RecPayServiceCCPaymentRB.Visible = true;
                RecPayServiceCCPaymentRB.Checked = true;
            }
        }

        private void RecWalkinPPPaymentBtn_Click(object sender, EventArgs e)
        {
            if (RecPayServicePPPaymentRB.Checked == false)
            {
                RecPayServicePPPaymentRB.Visible = true;
                RecPayServicePPPaymentRB.Checked = true;
                RecPayServiceTypeText.Text = "Paypal";
                RecPayServiceBankPaymentPanel.Visible = true;


                //disable other payment panel
                RecPayServiceWalletPaymentPanel.Visible = false;
                MngrPayServiceCashLbl.Visible = false;
                MngrPayServiceCashBox.Visible = false;
                MngrPayServiceCashBox.Text = "0";
                MngrPayServiceChangeBox.Text = "0.00";
                MngrPayServiceCardNameText.Text = "";
                MngrPayServiceCardNumText.Text = "";
                MngrPayServiceCVCText.Text = "";
                MngrPayServiceCardExpText.Text = "MM/YY";
                MngrPayServiceWalletNumText.Text = "";
                MngrPayServiceWalletPINText.Text = "";
                MngrPayServiceWalletOTPText.Text = "";
                MngrPayServiceChangeLbl.Visible = false;
                MngrPayServiceChangeBox.Visible = false;

                //disable radio buttons
                RecPayServiceCCPaymentRB.Visible = false;
                RecPayServiceCashPaymentRB.Visible = false;
                RecPayServiceGCPaymentRB.Visible = false;
                MngrPayServicePMPaymentRB.Visible = false;
                RecPayServiceCCPaymentRB.Checked = false;
                RecPayServiceCashPaymentRB.Checked = false;
                RecPayServiceGCPaymentRB.Checked = false;
                MngrPayServicePMPaymentRB.Checked = false;
            }
            else
            {
                RecPayServicePPPaymentRB.Visible = true;
                RecPayServicePPPaymentRB.Checked = true;
            }
        }

        private void RecWalkinCashPaymentBtn_Click(object sender, EventArgs e)
        {
            if (RecPayServiceCashPaymentRB.Checked == false)
            {
                RecPayServiceCashPaymentRB.Visible = true;
                RecPayServiceCashPaymentRB.Checked = true;
                RecPayServiceTypeText.Text = "Cash";
                MngrPayServiceCashLbl.Visible = true;
                MngrPayServiceCashBox.Visible = true;
                MngrPayServiceChangeLbl.Visible = true;
                MngrPayServiceChangeBox.Visible = true;

                //disable other payment panel
                RecPayServiceBankPaymentPanel.Visible = false;
                RecPayServiceWalletPaymentPanel.Visible = false;

                //disable radio buttons
                RecPayServiceCCPaymentRB.Visible = false;
                RecPayServicePPPaymentRB.Visible = false;
                RecPayServiceGCPaymentRB.Visible = false;
                MngrPayServicePMPaymentRB.Visible = false;
                RecPayServiceCCPaymentRB.Checked = false;
                RecPayServicePPPaymentRB.Checked = false;
                RecPayServiceGCPaymentRB.Checked = false;
                MngrPayServicePMPaymentRB.Checked = false;
                MngrPayServiceCardNameText.Text = "";
                MngrPayServiceCardNumText.Text = "";
                MngrPayServiceCVCText.Text = "";
                MngrPayServiceCardExpText.Text = "MM/YY";
                MngrPayServiceWalletNumText.Text = "";
                MngrPayServiceWalletPINText.Text = "";
                MngrPayServiceWalletOTPText.Text = "";
                MngrPayServiceCashBox.Text = "0";
                MngrPayServiceChangeBox.Text = "0.00";
            }
            else
            {
                RecPayServiceCashPaymentRB.Visible = true;
                RecPayServiceCashPaymentRB.Checked = true;
            }
        }

        private void RecWalkinGCPaymentBtn_Click(object sender, EventArgs e)
        {
            if (RecPayServiceGCPaymentRB.Checked == false)
            {
                RecPayServiceGCPaymentRB.Visible = true;
                RecPayServiceGCPaymentRB.Checked = true;
                RecPayServiceTypeText.Text = "Gcash";
                RecPayServiceWalletPaymentPanel.Visible = true;


                //disable other payment panel
                RecPayServiceBankPaymentPanel.Visible = false;
                MngrPayServiceCashLbl.Visible = false;
                MngrPayServiceCashBox.Visible = false;
                MngrPayServiceCashBox.Text = "0";
                MngrPayServiceChangeBox.Text = "0.00";
                MngrPayServiceCardNameText.Text = "";
                MngrPayServiceCardNumText.Text = "";
                MngrPayServiceCVCText.Text = "";
                MngrPayServiceCardExpText.Text = "MM/YY";
                MngrPayServiceWalletNumText.Text = "";
                MngrPayServiceWalletPINText.Text = "";
                MngrPayServiceWalletOTPText.Text = "";
                MngrPayServiceChangeLbl.Visible = false;
                MngrPayServiceChangeBox.Visible = false;

                //disable radio buttons
                RecPayServiceCCPaymentRB.Visible = false;
                RecPayServicePPPaymentRB.Visible = false;
                RecPayServiceCashPaymentRB.Visible = false;
                MngrPayServicePMPaymentRB.Visible = false;
                RecPayServiceCCPaymentRB.Checked = false;
                RecPayServicePPPaymentRB.Checked = false;
                RecPayServiceCashPaymentRB.Checked = false;
                MngrPayServicePMPaymentRB.Checked = false;
            }
            else
            {
                RecPayServiceGCPaymentRB.Visible = true;
                RecPayServiceGCPaymentRB.Checked = true;
            }
        }

        private void RecWalkinPMPaymentBtn_Click(object sender, EventArgs e)
        {
            if (MngrPayServicePMPaymentRB.Checked == false)
            {
                MngrPayServicePMPaymentRB.Visible = true;
                MngrPayServicePMPaymentRB.Checked = true;
                RecPayServiceTypeText.Text = "Paymaya";
                RecPayServiceWalletPaymentPanel.Visible = true;


                //disable other payment panel
                RecPayServiceBankPaymentPanel.Visible = false;
                MngrPayServiceCashLbl.Visible = false;
                MngrPayServiceCashBox.Visible = false;
                MngrPayServiceCashBox.Text = "0";
                MngrPayServiceChangeBox.Text = "0.00";
                MngrPayServiceCardNameText.Text = "";
                MngrPayServiceCardNumText.Text = "";
                MngrPayServiceCVCText.Text = "";
                MngrPayServiceCardExpText.Text = "MM/YY";
                MngrPayServiceWalletNumText.Text = "";
                MngrPayServiceWalletPINText.Text = "";
                MngrPayServiceWalletOTPText.Text = "";
                MngrPayServiceChangeLbl.Visible = false;
                MngrPayServiceChangeBox.Visible = false;

                //disable radio buttons
                RecPayServiceCCPaymentRB.Visible = false;
                RecPayServicePPPaymentRB.Visible = false;
                RecPayServiceCashPaymentRB.Visible = false;
                RecPayServiceGCPaymentRB.Visible = false;
                RecPayServiceCCPaymentRB.Checked = false;
                RecPayServicePPPaymentRB.Checked = false;
                RecPayServiceCashPaymentRB.Checked = false;
                RecPayServiceGCPaymentRB.Checked = false;
            }
            else
            {
                MngrPayServicePMPaymentRB.Visible = true;
                MngrPayServicePMPaymentRB.Checked = true;
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
            public string QueNumber { get; set; }
        }

        protected void InitializeGeneralCuePendingCustomersForStaff()
        {
            List<PendingCustomers> generalquependingcustomers = RetrieveGeneralQuePendingCustomersFromDB();

            if (generalquependingcustomers.Count == 0)
            {
                NoCustomerInQueueUserControl nocustomerusercontrol = new NoCustomerInQueueUserControl();
                StaffGeneralCueCurrentCustomersStatusFlowLayoutPanel.Controls.Add(nocustomerusercontrol);
                return;
            }

            int smallestQueNumber = int.MaxValue;

            foreach (PendingCustomers customer in generalquependingcustomers)
            {
                StaffCurrentAvailableCustomersUserControl availablecustomersusercontrol = new StaffCurrentAvailableCustomersUserControl(this);
                availablecustomersusercontrol.AvailableCustomerSetData(customer);
                availablecustomersusercontrol.ExpandUserControlButtonClicked += AvailableCustomersUserControl_ExpandCollapseButtonClicked;
                availablecustomersusercontrol.StartServiceButtonClicked += AvailableCustomersUserControl_StartServiceButtonClicked;
                availablecustomersusercontrol.StaffEndServiceBtnClicked += AvailableCustomersUserControl_EndServiceButtonClicked;
                StaffGeneralCueCurrentCustomersStatusFlowLayoutPanel.Controls.Add(availablecustomersusercontrol);
                availablecustomersusercontrol.CurrentStaffID = StaffIDNumLbl.Text;

                    string queNumberText = availablecustomersusercontrol.StaffQueNumberTextBox.Text;
                    if (int.TryParse(queNumberText, out int queNumber))
                    {
                        if (queNumber < smallestQueNumber)
                        {
                            smallestQueNumber = queNumber;
                        }
                }
            }

            if (StaffPersonalCueCurrentCustomersStatusFlowLayoutPanel.Controls.Count > 0 && !StaffPersonalCueCurrentCustomersStatusFlowLayoutPanel.Controls.OfType<NoCustomerInQueueUserControl>().Any())
            {
                foreach (StaffCurrentAvailableCustomersUserControl userControl in StaffPersonalCueCurrentCustomersStatusFlowLayoutPanel.Controls)
                {
                    string queNumberText = userControl.StaffQueNumberTextBox.Text;
                    if (int.TryParse(queNumberText, out int queNumber))
                    {
                        if (queNumber < smallestQueNumber)
                        {
                            smallestQueNumber = queNumber;
                        }
                    }
                }
            }
            
            if (StaffGeneralCueCurrentCustomersStatusFlowLayoutPanel.Controls.Count > 0 && !StaffGeneralCueCurrentCustomersStatusFlowLayoutPanel.Controls.OfType<NoCustomerInQueueUserControl>().Any())
            {
                foreach (StaffCurrentAvailableCustomersUserControl userControl in StaffGeneralCueCurrentCustomersStatusFlowLayoutPanel.Controls)
                {
                    string queNumberText = userControl.StaffQueNumberTextBox.Text;
                    if (int.TryParse(queNumberText, out int queNumber))
                    {
                        if (queNumber < smallestQueNumber)
                        {
                            smallestQueNumber = queNumber;
                        }
                    }
                }
            }

            if (generalquependingcustomers.Count > 0 && !StaffGeneralCueCurrentCustomersStatusFlowLayoutPanel.Controls.OfType<NoCustomerInQueueUserControl>().Any())
            {
                foreach (StaffCurrentAvailableCustomersUserControl userControl in StaffGeneralCueCurrentCustomersStatusFlowLayoutPanel.Controls)
                {
                    string queNumberText = userControl.StaffQueNumberTextBox.Text;
                    if (int.TryParse(queNumberText, out int queNumber))
                    {
                        if (queNumber == smallestQueNumber)
                        {
                            userControl.StaffStartServiceBtn.Enabled = true;
                        }
                        else
                        {
                            userControl.StaffStartServiceBtn.Enabled = false;
                        }
                    }
                }
            }
            else
            {
                foreach (System.Windows.Forms.Control control in StaffGeneralCueCurrentCustomersStatusFlowLayoutPanel.Controls)
                {
                    if (control is StaffCurrentAvailableCustomersUserControl userControl)
                    {
                        userControl.StaffStartServiceBtn.Enabled = false;
                    }
                }
            }
        }

        private void AvailableCustomersUserControl_ExpandCollapseButtonClicked(object sender, EventArgs e)
        {
            StaffCurrentAvailableCustomersUserControl availablecustomersusercontrol = (StaffCurrentAvailableCustomersUserControl)sender;

            if (availablecustomersusercontrol != null)
            {
                if (!availablecustomersusercontrol.Viewing)
                {
                    availablecustomersusercontrol.Size = new System.Drawing.Size(875, 350);
                    //StaffCurrentServicesDropDownBtn.IconChar = FontAwesome.Sharp.IconChar.SquareCaretUp;
                }
                else
                {
                    availablecustomersusercontrol.Size = new System.Drawing.Size(875, 200);
                    //StaffCurrentServicesDropDownBtn.IconChar = FontAwesome.Sharp.IconChar.SquareCaretDown;

                }
            }

        }

        private List<PendingCustomers> RetrieveGeneralQuePendingCustomersFromDB()
        {
            DateTime currentDate = DateTime.Today;
            string datetoday = currentDate.ToString("MM-dd-yyyy dddd");
            List<PendingCustomers> result = new List<PendingCustomers>();

            using (MySqlConnection connection = new MySqlConnection(mysqlconn))
            {
                try
                {
                    connection.Open();

                    string generalquependingcustomersquery = $@"SELECT sh.TransactionNumber, sh.ClientName, sh.ServiceStatus, sh.SelectedService, sh.ServiceID, sh.Customization, sh.AddNotes, sh.QueNumber 
                                                     FROM servicehistory sh 
                                                     INNER JOIN walk_in_appointment wa ON sh.TransactionNumber = wa.TransactionNumber 
                                                     WHERE sh.ServiceStatus = 'Pending' 
                                                     AND sh.ServiceCategory = @membercategory 
                                                     AND sh.QueType = 'GeneralQue' 
                                                     AND wa.ServiceStatus = 'Pending' 
                                                     AND sh.AppointmentDate = @datetoday";

                    MySqlCommand command = new MySqlCommand(generalquependingcustomersquery, connection);
                    command.Parameters.AddWithValue("@membercategory", membercategory);
                    command.Parameters.AddWithValue("@datetoday", datetoday);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PendingCustomers generalquependingcustomers = new PendingCustomers
                            {
                                TransactionNumber = reader["TransactionNumber"] as string,
                                ClientName = reader["ClientName"] as string,
                                ServiceStatus = reader["ServiceStatus"] as string,
                                ServiceName = reader["SelectedService"] as string,
                                CustomerCustomizations = reader["Customization"] as string,
                                AdditionalNotes = reader["AddNotes"] as string,
                                ServiceID = reader["ServiceID"] as string,
                                QueNumber = reader["QueNumber"] as string
                            };

                            result.Add(generalquependingcustomers);
                        }
                    }
                    if (result.Count == 0)
                    {
                        MessageBox.Show("No customers in the queue.", "Empty Queue", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
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
            foreach (System.Windows.Forms.Control control in StaffGeneralCueCurrentCustomersStatusFlowLayoutPanel.Controls)
            {
                if (control is StaffCurrentAvailableCustomersUserControl userControl &&
                    userControl.StaffCustomerServiceStatusTextBox.Text == "In Session")
                {
                    return;
                }
            }
            StaffGeneralCueCurrentCustomersStatusFlowLayoutPanel.Controls.Clear();
            StaffPersonalCueCurrentCustomersStatusFlowLayoutPanel.Controls.Clear();
            InitializeGeneralCuePendingCustomersForStaff();
            InitializePreferredCuePendingCustomersForStaff();
        }




        public void RemovePendingUserControls(StaffCurrentAvailableCustomersUserControl selectedControl)
        {
            foreach (System.Windows.Forms.Control control in StaffGeneralCueCurrentCustomersStatusFlowLayoutPanel.Controls.OfType<StaffCurrentAvailableCustomersUserControl>().ToList())
            {
                if (control != selectedControl)
                {
                    StaffGeneralCueCurrentCustomersStatusFlowLayoutPanel.Controls.Remove(control);
                    control.Dispose();
                }
            }
            foreach (System.Windows.Forms.Control control in StaffPersonalCueCurrentCustomersStatusFlowLayoutPanel.Controls.OfType<StaffCurrentAvailableCustomersUserControl>().ToList())
            {
                if (control != selectedControl)
                {
                    StaffPersonalCueCurrentCustomersStatusFlowLayoutPanel.Controls.Remove(control);
                    control.Dispose();
                }
            }
        }



        private void StaffRefreshAvailableCustomersBtn_Click(object sender, EventArgs e)
        {
            RefreshFlowLayoutPanel();
        }


        private List<PendingCustomers> RetrievePreferredQuePendingCustomersFromDB()
        {
            string staffID = StaffIDNumLbl.Text;
            DateTime currentDate = DateTime.Today;
            string datetoday = currentDate.ToString("MM-dd-yyyy dddd");
            List<PendingCustomers> result = new List<PendingCustomers>();

            using (MySqlConnection connection = new MySqlConnection(mysqlconn))
            {
                try
                {
                    connection.Open();

                    string preferredquependingcustomersquery = $@"SELECT sh.TransactionNumber, sh.ClientName, sh.ServiceStatus, sh.SelectedService, sh.ServiceID, sh.Customization, sh.AddNotes, sh.QueNumber
                       FROM servicehistory sh INNER JOIN walk_in_appointment wa ON sh.TransactionNumber = wa.TransactionNumber
                       WHERE sh.ServiceStatus = 'Pending' AND sh.ServiceCategory = @membercategory AND sh.PreferredStaff = @preferredstaff AND wa.ServiceStatus = 'Pending' AND sh.AppointmentDate = @datetoday";

                    MySqlCommand command = new MySqlCommand(preferredquependingcustomersquery, connection);
                    command.Parameters.AddWithValue("@membercategory", membercategory);
                    command.Parameters.AddWithValue("@preferredstaff", staffID);
                    command.Parameters.AddWithValue("@datetoday", datetoday);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PendingCustomers preferredquependingcustomers = new PendingCustomers
                            {
                                TransactionNumber = reader.IsDBNull(reader.GetOrdinal("TransactionNumber")) ? string.Empty : reader.GetString("TransactionNumber"),
                                ClientName = reader.IsDBNull(reader.GetOrdinal("ClientName")) ? string.Empty : reader.GetString("ClientName"),
                                ServiceStatus = reader.IsDBNull(reader.GetOrdinal("ServiceStatus")) ? string.Empty : reader.GetString("ServiceStatus"),
                                ServiceName = reader.IsDBNull(reader.GetOrdinal("SelectedService")) ? string.Empty : reader.GetString("SelectedService"),
                                CustomerCustomizations = reader.IsDBNull(reader.GetOrdinal("Customization")) ? string.Empty : reader.GetString("Customization"),
                                AdditionalNotes = reader.IsDBNull(reader.GetOrdinal("AddNotes")) ? string.Empty : reader.GetString("AddNotes"),
                                ServiceID = reader.IsDBNull(reader.GetOrdinal("ServiceID")) ? string.Empty : reader.GetString("ServiceID"),
                                QueNumber = reader.IsDBNull(reader.GetOrdinal("QueNumber")) ? string.Empty : reader.GetString("QueNumber")
                            };

                            result.Add(preferredquependingcustomers);
                        }
                    }

                    if (result.Count == 0)
                    {
                        MessageBox.Show("No customers in the preferred queue.", "Empty Queue", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }

            return result;
        }


        protected void InitializePreferredCuePendingCustomersForStaff()
        {
        List<PendingCustomers> preferredquependingcustomers = RetrievePreferredQuePendingCustomersFromDB();

            if (preferredquependingcustomers.Count == 0)
            {
                NoCustomerInQueueUserControl nocustomerusercontrol = new NoCustomerInQueueUserControl();
                StaffPersonalCueCurrentCustomersStatusFlowLayoutPanel.Controls.Add(nocustomerusercontrol);
                return;
            }
            int smallestQueNumber = int.MaxValue;

            foreach (PendingCustomers customer in preferredquependingcustomers)
            {
                StaffCurrentAvailableCustomersUserControl availablecustomersusercontrol = new StaffCurrentAvailableCustomersUserControl(this);
                availablecustomersusercontrol.AvailableCustomerSetData(customer);
                availablecustomersusercontrol.ExpandUserControlButtonClicked += AvailableCustomersUserControl_ExpandCollapseButtonClicked;
                availablecustomersusercontrol.StartServiceButtonClicked += AvailableCustomersUserControl_StartServiceButtonClicked;
                availablecustomersusercontrol.StaffEndServiceBtnClicked += AvailableCustomersUserControl_EndServiceButtonClicked;
                StaffPersonalCueCurrentCustomersStatusFlowLayoutPanel.Controls.Add(availablecustomersusercontrol);
                availablecustomersusercontrol.CurrentStaffID = StaffIDNumLbl.Text;

                string queNumberText = availablecustomersusercontrol.StaffQueNumberTextBox.Text;
                if (int.TryParse(queNumberText, out int queNumber))
                {
                    if (queNumber < smallestQueNumber)
                    {
                        smallestQueNumber = queNumber;
                    }
                }
            }
            if (StaffGeneralCueCurrentCustomersStatusFlowLayoutPanel.Controls.Count > 0 && !StaffGeneralCueCurrentCustomersStatusFlowLayoutPanel.Controls.OfType<NoCustomerInQueueUserControl>().Any())
            {
                foreach (StaffCurrentAvailableCustomersUserControl userControl in StaffGeneralCueCurrentCustomersStatusFlowLayoutPanel.Controls)
                {
                    string queNumberText = userControl.StaffQueNumberTextBox.Text;
                    if (int.TryParse(queNumberText, out int queNumber))
                    {
                        if (queNumber < smallestQueNumber)
                        {
                            smallestQueNumber = queNumber;
                        }
                    }
                }
            }
            if (StaffPersonalCueCurrentCustomersStatusFlowLayoutPanel.Controls.Count > 0 && !StaffPersonalCueCurrentCustomersStatusFlowLayoutPanel.Controls.OfType<NoCustomerInQueueUserControl>().Any())
            {
                foreach (StaffCurrentAvailableCustomersUserControl userControl in StaffPersonalCueCurrentCustomersStatusFlowLayoutPanel.Controls)
                {
                    string queNumberText = userControl.StaffQueNumberTextBox.Text;
                    if (int.TryParse(queNumberText, out int queNumber))
                    {
                        if (queNumber < smallestQueNumber)
                        {
                            smallestQueNumber = queNumber;
                        }
                    }
                }
            }

            if (preferredquependingcustomers.Count > 0 && !StaffPersonalCueCurrentCustomersStatusFlowLayoutPanel.Controls.OfType<NoCustomerInQueueUserControl>().Any())
             {
                    foreach (StaffCurrentAvailableCustomersUserControl userControl in StaffPersonalCueCurrentCustomersStatusFlowLayoutPanel.Controls)
                    {
                        string queNumberText = userControl.StaffQueNumberTextBox.Text;
                        if (int.TryParse(queNumberText, out int queNumber))
                        {
                            if (queNumber == smallestQueNumber)
                            {
                                userControl.StaffStartServiceBtn.Enabled = true;
                            }
                            else
                            {
                                userControl.StaffStartServiceBtn.Enabled = false;
                            }
                        }
                    }
            }
            else
            {
                foreach (System.Windows.Forms.Control control in StaffPersonalCueCurrentCustomersStatusFlowLayoutPanel.Controls)
                {
                    if (control is StaffCurrentAvailableCustomersUserControl userControl)
                    {
                        userControl.StaffStartServiceBtn.Enabled = false;
                    }
                }
            }
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
            PDImage.Visible = false;
            ProductImagePictureBox.Visible = false;
            CancelEdit.Visible = false;
            SelectImage.Visible = false;
            MngrInventoryProductsCatComboText.Enabled = true;
            MngrInventoryProductsTypeComboText.Enabled = true;
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

            if (MngrInventoryProductsTypeComboText.SelectedItem.ToString() == "Retail Product")
            {
                if (ProductImagePictureBox.Image == null)
                {
                    MessageBox.Show("Please select an image for the product.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (!IsNumeric(MngrInventoryProductsStockText.Text))
            {
                MessageBox.Show("Invalid Stock Number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MngrInventoryProductsPriceText.Text != "Not Applicable" && !IsNumeric(MngrInventoryProductsPriceText.Text))
            {
                MessageBox.Show("Invalid Price.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Convert.ToInt32(MngrInventoryProductsStockText.Text) > 200)
            {
                MessageBox.Show("Stock cannot exceed 200.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Convert.ToInt32(MngrInventoryProductsStockText.Text) < 40)
            {
                MessageBox.Show("Stock cannot be lower than 40.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string query = "INSERT INTO inventory (ItemID, ProductCategory, ItemName, ItemStock, ItemPrice, ProductType, ItemStatus, ProductPicture) " +
               "VALUES (@ItemID, @ProductCategory, @ItemName, @ItemStock, @ItemPrice, @ProductType, @ItemStatus, @ProductPicture)";

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

                        byte[] imageBytes = null;
                        if (ProductImagePictureBox.Image != null)
                        {
                            using (MemoryStream ms = new MemoryStream())
                            {
                                ProductImagePictureBox.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                                imageBytes = ms.ToArray();
                            }
                        }
                        command.Parameters.AddWithValue("@ProductPicture", imageBytes);
                        command.ExecuteNonQuery();

                    }
                    MessageBox.Show("Item added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    shouldGenerateItemID = false;
                    PDImage.Visible = false;
                    ProductImagePictureBox.Visible = false;
                    SelectImage.Visible = false;
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

                string connectionString = "server=localhost;user=root;database=enchante;password=";
                string query = "SELECT ProductPicture FROM inventory WHERE ItemID = @ItemID";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ItemID", selectedRow.Cells["ItemID"].Value.ToString());

                        try
                        {
                            connection.Open();

                            // ExecuteScalar returns DBNull if the value is null in the database
                            object result = command.ExecuteScalar();

                            if (result != DBNull.Value && result != null)
                            {
                                byte[] imageData = (byte[])result;
                                if (imageData != null && imageData.Length > 0)
                                {
                                    using (MemoryStream ms = new MemoryStream(imageData))
                                    {
                                        ProductImagePictureBox.Image = System.Drawing.Image.FromStream(ms);
                                    }
                                }
                                else
                                {
                                    ProductImagePictureBox.Image = null;
                                }
                            }
                            else
                            {
                                ProductImagePictureBox.Image = null;
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

                shouldGenerateItemID = false;
                MngrInventoryProductsCatComboText.Enabled = false;
                MngrInventoryProductsTypeComboText.Enabled = false;
                MngrInventoryProductsInsertBtn.Visible = false;
                MngrInventoryProductsUpdateBtn.Visible = true;
                CancelEdit.Visible = true;
                MngrInventoryProductsIDText.Text = selectedRow.Cells["ItemID"].Value.ToString();
                MngrInventoryProductsNameText.Text = selectedRow.Cells["ItemName"].Value.ToString();
                MngrInventoryProductsPriceText.Text = selectedRow.Cells["ItemPrice"].Value.ToString();
                MngrInventoryProductsStockText.Text = selectedRow.Cells["ItemStock"].Value.ToString();
                MngrInventoryProductsCatComboText.SelectedItem = selectedRow.Cells["ProductCategory"].Value.ToString();
                MngrInventoryProductsTypeComboText.SelectedItem = selectedRow.Cells["ProductType"].Value.ToString();
                MngrInventoryProductsStatusComboText.SelectedItem = selectedRow.Cells["ItemStatus"].Value.ToString();

            }
            else
            {
                MessageBox.Show("Please select a row to edit.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MngrInventoryProductsUpdateBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(MngrInventoryProductsNameText.Text) || string.IsNullOrWhiteSpace(MngrInventoryProductsPriceText.Text) || string.IsNullOrWhiteSpace(MngrInventoryProductsStockText.Text) || string.IsNullOrWhiteSpace(MngrInventoryProductsIDText.Text) ||
               MngrInventoryProductsCatComboText.SelectedItem == null || MngrInventoryProductsTypeComboText.SelectedItem == null || MngrInventoryProductsStatusComboText.SelectedItem == null || ProductImagePictureBox == null)
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MngrInventoryProductsTypeComboText.SelectedItem.ToString() == "Retail Product")
            {
                if (ProductImagePictureBox.Image == null)
                {
                    MessageBox.Show("Please select an image for the product.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (!IsNumeric(MngrInventoryProductsStockText.Text))
            {
                MessageBox.Show("Invalid Stock.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MngrInventoryProductsPriceText.Text != "Not Applicable" && !IsNumeric(MngrInventoryProductsPriceText.Text))
            {
                MessageBox.Show("Invalid Price.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Convert.ToInt32(MngrInventoryProductsStockText.Text) > 200)
            {
                MessageBox.Show("Stock cannot exceed 200.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Convert.ToInt32(MngrInventoryProductsStockText.Text) < 40)
            {
                MessageBox.Show("Stock cannot be lower than 40.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string connectionString = "server=localhost;user=root;database=enchante;password=";
            string query = @"UPDATE inventory 
                            SET ItemName = @ItemName, 
                                ItemPrice = @ItemPrice, 
                                ItemStock = @ItemStock, 
                                ProductCategory = @ProductCategory, 
                                ProductType = @ProductType, 
                                ItemStatus = @ItemStatus,
                                ProductPicture = @ProductPicture 
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

                    if (MngrInventoryProductsTypeComboText.SelectedItem.ToString() == "Retail Product" && ProductImagePictureBox.Image != null)
                    {
                        MemoryStream ms = new MemoryStream();
                        ProductImagePictureBox.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        byte[] imageData = ms.ToArray();
                        command.Parameters.AddWithValue("@ProductPicture", imageData);
                    }
                    else
                    {
                        // If it's a Service Product or no image is selected, set the parameter to null
                        command.Parameters.AddWithValue("@ProductPicture", DBNull.Value);
                    }

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
                                    // Check if any of the fields have changed
                                    if (reader["ItemName"].ToString() != MngrInventoryProductsNameText.Text ||
                                        reader["ItemPrice"].ToString() != MngrInventoryProductsPriceText.Text ||
                                        reader["ItemStock"].ToString() != MngrInventoryProductsStockText.Text ||
                                        reader["ProductCategory"].ToString() != MngrInventoryProductsCatComboText.SelectedItem.ToString() ||
                                        reader["ProductType"].ToString() != MngrInventoryProductsTypeComboText.SelectedItem.ToString() ||
                                        reader["ItemStatus"].ToString() != MngrInventoryProductsStatusComboText.SelectedItem.ToString())
                                    {
                                        fieldsChanged = true;
                                    }

                                    // Check if the ProductType is "Retail Product" and IMGCheck radio button is checked
                                    if (MngrInventoryProductsTypeComboText.SelectedItem.ToString() == "Retail Product" )
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
                                PDImage.Visible = false;
                                ProductImagePictureBox.Visible = false;
                                SelectImage.Visible = false;
                                CancelEdit.Visible = false;
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
            ProductImagePictureBox.Image = null;
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
            Transaction.PanelShow(RecTransactionPanel);

        }

        private void MngrInventoryWalkinSalesBtn_Click(object sender, EventArgs e)
        {
            Inventory.PanelShow(MngrWalkinSalesPanel);
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
            string staffID = StaffIDNumLbl.Text;
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
            if (StaffInventoryDataGrid.CurrentRow == null)
            {
                MessageBox.Show("Please select a row.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(StaffItemSelectedCountTextBox.Text))
            {
                MessageBox.Show("Please enter the number of items to add.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(StaffItemSelectedCountTextBox.Text, out int itemStockToBeAdded) || itemStockToBeAdded <= 0)
            {
                MessageBox.Show("Invalid number of items to add.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (itemStockToBeAdded > 20)
            {
                MessageBox.Show("You can't take more than 20 items.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string itemID = StaffInventoryDataGrid.SelectedRows[0].Cells["ItemID"].Value.ToString();
            string itemName = StaffInventoryDataGrid.SelectedRows[0].Cells["ItemName"].Value.ToString();
            int itemStock = Convert.ToInt32(StaffInventoryDataGrid.SelectedRows[0].Cells["ItemStock"].Value);
            string itemStatus = StaffInventoryDataGrid.SelectedRows[0].Cells["ItemStatus"].Value.ToString();
            string staffID = StaffIDNumLbl.Text;

            using (MySqlConnection connection = new MySqlConnection(mysqlconn))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM staff_inventory WHERE ItemID = @ItemID AND EmployeeID = @EmployeeID";
                using (MySqlCommand selectCommand = new MySqlCommand(selectQuery, connection))
                {
                    selectCommand.Parameters.AddWithValue("@ItemID", itemID);
                    selectCommand.Parameters.AddWithValue("@EmployeeID", staffID);

                    DataTable dataTable = new DataTable();
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(selectCommand))
                    {
                        adapter.Fill(dataTable);
                    }

                    if (dataTable.Rows.Count > 0)
                    {
                        int currentStock = Convert.ToInt32(dataTable.Rows[0]["ItemStock"]);
                        int newStock = currentStock + itemStockToBeAdded;

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
                }

                string deductQuery = "UPDATE inventory SET ItemStock = ItemStock - @SelectedCount WHERE ItemID = @ItemID AND ItemStock - @SelectedCount >= 40";
                using (MySqlCommand deductCommand = new MySqlCommand(deductQuery, connection))
                {
                    deductCommand.Parameters.AddWithValue("@SelectedCount", itemStockToBeAdded);
                    deductCommand.Parameters.AddWithValue("@ItemID", itemID);
                    int rowsAffected = deductCommand.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        MessageBox.Show("Cannot deduct from this item as it would result in stock below 40. Please refill your inventory to continue operations.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                string updateStatusQuery = "UPDATE inventory SET ItemStatus = 'Low Stock' WHERE ItemID = @ItemID AND ItemStock >= 40 AND ItemStock <= 50";
                using (MySqlCommand updateStatusCommand = new MySqlCommand(updateStatusQuery, connection))
                {
                    updateStatusCommand.Parameters.AddWithValue("@ItemID", itemID);
                    updateStatusCommand.ExecuteNonQuery();
                }

                string checkStockQuery = "SELECT ItemStock FROM inventory WHERE ItemID = @ItemID";
                using (MySqlCommand checkStockCommand = new MySqlCommand(checkStockQuery, connection))
                {
                    checkStockCommand.Parameters.AddWithValue("@ItemID", itemID);
                    int currentStock = Convert.ToInt32(checkStockCommand.ExecuteScalar());
                }

                InitializeStaffPersonalInventoryDataGrid();
                InitializeStaffInventoryDataGrid();
                StaffItemSelectedCountTextBox.Clear();
            }
        }

        public void MngrLoadServiceHistoryDB(string transactNumber)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                {
                    connection.Open();

                    // Modify the SQL query to filter based on TransactNumber and OrderNumber
                    string sql = "SELECT * FROM `servicehistory` WHERE TransactionNumber = @TransactionNumber";
                    MySqlCommand cmd = new MySqlCommand(sql, connection);

                    // Add parameters to the query
                    cmd.Parameters.AddWithValue("@TransactionNumber", transactNumber);

                    System.Data.DataTable dataTable = new System.Data.DataTable();

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);

                        RecPayServicesAcquiredDGV.DataSource = dataTable;

                        RecPayServicesAcquiredDGV.Columns[0].Visible = false; //transact number
                        RecPayServicesAcquiredDGV.Columns[2].Visible = false; //appointment date
                        RecPayServicesAcquiredDGV.Columns[3].Visible = false; //appointment time
                        RecPayServicesAcquiredDGV.Columns[4].Visible = false; //client name
                        RecPayServicesAcquiredDGV.Columns[5].Visible = false; //service category
                        RecPayServicesAcquiredDGV.Columns[7].Visible = false; //service ID
                        RecPayServicesAcquiredDGV.Columns[10].Visible = false; //service start
                        RecPayServicesAcquiredDGV.Columns[11].Visible = false; //service end
                        RecPayServicesAcquiredDGV.Columns[12].Visible = false; //service duration
                        RecPayServicesAcquiredDGV.Columns[13].Visible = false; //customization
                        RecPayServicesAcquiredDGV.Columns[14].Visible = false; // add notes
                        RecPayServicesAcquiredDGV.Columns[15].Visible = false; // preferred staff
                        RecPayServicesAcquiredDGV.Columns[16].Visible = false; // que num
                        RecPayServicesAcquiredDGV.Columns[17].Visible = false; // que type

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Manager Order History List");
            }
            finally
            {
                // Make sure to close the connection (if it's open)
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        public void MngrLoadOrderProdHistoryDB(string transactNumber)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                {
                    connection.Open();

                    // Modify the SQL query to filter based on TransactNumber and OrderNumber
                    string sql = "SELECT * FROM `orderproducthistory` WHERE TransactionNumber = @TransactionNumber";
                    MySqlCommand cmd = new MySqlCommand(sql, connection);

                    // Add parameters to the query
                    cmd.Parameters.AddWithValue("@TransactionNumber", transactNumber);

                    System.Data.DataTable dataTable = new System.Data.DataTable();

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);

                        RecPayServiceCOProdDGV.DataSource = dataTable;

                        RecPayServiceCOProdDGV.Columns[0].Visible = false; //transact number
                        RecPayServiceCOProdDGV.Columns[1].Visible = false; //product stats
                        RecPayServiceCOProdDGV.Columns[2].Visible = false; // date
                        RecPayServiceCOProdDGV.Columns[3].Visible = false; // time 
                        RecPayServiceCOProdDGV.Columns[4].Visible = false; // by
                        RecPayServiceCOProdDGV.Columns[5].Visible = false; //client name
                        RecPayServiceCOProdDGV.Columns[6].Visible = false; // item ID
                        RecPayServiceCOProdDGV.Columns[11].Visible = false; //checkedout
                        RecPayServiceCOProdDGV.Columns[12].Visible = false; //void


                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Manager Order History List");
            }
            finally
            {
                // Make sure to close the connection (if it's open)
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private void MngrPayServiceCompleteTransDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if a valid cell is clicked
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Get TransactNumber and OrderNumber from the clicked cell in MngrSalesTable
                string transactNumber = RecPayServiceCompleteTransDGV.Rows[e.RowIndex].Cells["TransactionNumber"].Value.ToString();
                string clientName = RecPayServiceCompleteTransDGV.Rows[e.RowIndex].Cells["ClientName"].Value.ToString();

                RecPayServiceTransactNumLbl.Text = transactNumber;
                RecPayServiceClientNameLbl.Text = $"Client Name: {clientName}";
                MngrLoadServiceHistoryDB(transactNumber);
                MngrLoadOrderProdHistoryDB(transactNumber);
                ReceptionCalculateTotalPrice();

            }
        }
        public void MngrLoadCompletedTrans()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                {
                    connection.Open();

                    // Filter and sort the data by FoodType
                    string sql = "SELECT * FROM `walk_in_appointment` WHERE ServiceStatus = 'Completed' ORDER BY ServiceStatus";
                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    System.Data.DataTable dataTable = new System.Data.DataTable();

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);

                        RecPayServiceCompleteTransDGV.Columns.Clear();


                        RecPayServiceCompleteTransDGV.DataSource = dataTable;

                        RecPayServiceCompleteTransDGV.Columns[2].Visible = false; //appointment date
                        RecPayServiceCompleteTransDGV.Columns[3].Visible = false; //appointment time
                        RecPayServiceCompleteTransDGV.Columns[5].Visible = false; // customizations
                        RecPayServiceCompleteTransDGV.Columns[6].Visible = false; // add notes
                        RecPayServiceCompleteTransDGV.Columns[7].Visible = false; // client cp num
                        RecPayServiceCompleteTransDGV.Columns[8].Visible = false; // net price
                        RecPayServiceCompleteTransDGV.Columns[9].Visible = false; // vat amount
                        RecPayServiceCompleteTransDGV.Columns[10].Visible = false; // discount amount
                        RecPayServiceCompleteTransDGV.Columns[11].Visible = false; // discount amount
                        RecPayServiceCompleteTransDGV.Columns[12].Visible = false; // cash given
                        RecPayServiceCompleteTransDGV.Columns[13].Visible = false; // due change
                        RecPayServiceCompleteTransDGV.Columns[14].Visible = false; // payment method
                        RecPayServiceCompleteTransDGV.Columns[15].Visible = false; // card name
                        RecPayServiceCompleteTransDGV.Columns[16].Visible = false; // card num
                        RecPayServiceCompleteTransDGV.Columns[17].Visible = false; // cvc
                        RecPayServiceCompleteTransDGV.Columns[18].Visible = false; // card expiration
                        RecPayServiceCompleteTransDGV.Columns[19].Visible = false; // wallet num
                        RecPayServiceCompleteTransDGV.Columns[20].Visible = false; // wallet PIN
                        RecPayServiceCompleteTransDGV.Columns[21].Visible = false; // wallet OTP
                        RecPayServiceCompleteTransDGV.Columns[22].Visible = false; // service duration
                        RecPayServiceCompleteTransDGV.Columns[23].Visible = false; // booked by
                        RecPayServiceCompleteTransDGV.Columns[24].Visible = false; // booked date
                        RecPayServiceCompleteTransDGV.Columns[25].Visible = false; // booked time
                        RecPayServiceCompleteTransDGV.Columns[26].Visible = false; // checked out by [manager]

                        RecPayServiceCompleteTransDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        RecPayServiceCompleteTransDGV.ClearSelection();
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


        private bool UpdateWalk_in_AppointmentDB()
        {
            // cash values
            string netAmount = MngrPayServiceNetAmountBox.Text; // net amount
            string vat = MngrPayServiceVATBox.Text; // vat 
            string discount = RecWalkinDiscountBox.Text; // discount
            string grossAmount = MngrPayServiceGrossAmountBox.Text; // gross amount
            string cash = MngrPayServiceCashBox.Text; // cash given
            string change = MngrPayServiceChangeBox.Text; // due change
            string paymentMethod = RecPayServiceTypeText.Text; // payment method
            string mngr = MngrNameLbl.Text;
            string transactNum = RecPayServiceTransactNumLbl.Text;

            // bank & wallet details
            string cardName = MngrPayServiceCardNameText.Text;
            string cardNum = MngrPayServiceCardNumText.Text;
            string CVC = MngrPayServiceCVCText.Text;
            string expire = MngrPayServiceCardExpText.Text;
            string walletNum = MngrPayServiceWalletNumText.Text;
            string walletPIN = MngrPayServiceWalletPINText.Text;
            string walletOTP = MngrPayServiceWalletOTPText.Text;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                {
                    connection.Open();


                    if (RecPayServiceCashPaymentRB.Checked)
                    {
                        if (grossAmount == "0.00")
                        {
                            MessageBox.Show("Please select a transaction to pay.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        else if (string.IsNullOrWhiteSpace(cash))
                        {
                            MessageBox.Show("Please enter a cash amount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        else if (!IsNumeric(cash))
                        {
                            MessageBox.Show("Cash amount must be in numbers only.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        else if (Convert.ToDecimal(cash) < Convert.ToDecimal(grossAmount))
                        {
                            MessageBox.Show("Insufficient amount. Please provide enough cash to cover the transaction.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }

                    }
                    else if (RecPayServiceCCPaymentRB.Checked || RecPayServicePPPaymentRB.Checked)
                    {
                        if (grossAmount == "0.00")
                        {
                            MessageBox.Show("Please select a transaction to pay.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        else if (string.IsNullOrWhiteSpace(MngrPayServiceCardNameText.Text))
                        {
                            MessageBox.Show("Please enter a cardholder name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        else if (!IsCardNameValid(MngrPayServiceCardNameText.Text))
                        {
                            MessageBox.Show("Please enter a valid name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        else if (string.IsNullOrWhiteSpace(cardNum))
                        {
                            MessageBox.Show("Please enter a card number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        else if (cardNum.Length != 16 || !IsNumeric(cardNum))
                        {
                            MessageBox.Show("Please enter a valid 16-digit card number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        else if (string.IsNullOrWhiteSpace(CVC))
                        {
                            MessageBox.Show("Please enter a CVC code.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        else if (CVC.Length != 3 || !IsNumeric(CVC))
                        {
                            MessageBox.Show("Please enter a valid 3-digit CVC code.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        else if (string.IsNullOrWhiteSpace(expire))
                        {
                            MessageBox.Show("Please enter an expiration date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        if (!Regex.IsMatch(expire, @"^(0[1-9]|1[0-2])\/\d{2}$"))
                        {
                            MessageBox.Show("Please enter the expiration date in MM/YY format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }

                    else if (RecPayServiceGCPaymentRB.Checked || MngrPayServicePMPaymentRB.Checked)
                    {
                        if (grossAmount == "0.00")
                        {
                            MessageBox.Show("Please select a transaction to pay.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        else if (string.IsNullOrWhiteSpace(walletNum))
                        {
                            MessageBox.Show("Please enter your wallet number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        else if (!IsNumeric(walletNum))
                        {
                            MessageBox.Show("Invalid wallet number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        else if (string.IsNullOrWhiteSpace(walletPIN))
                        {
                            MessageBox.Show("Please enter your PIN.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        else if (!IsNumeric(walletPIN) || walletPIN.Length != 6)
                        {
                            MessageBox.Show("Wallet PIN should be a 6-digit numeric code.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        else if (string.IsNullOrWhiteSpace(walletOTP))
                        {
                            MessageBox.Show("Please enter your OTP.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        else if (!IsNumeric(walletOTP) || walletOTP.Length != 6)
                        {
                            MessageBox.Show("OTP should be a 6-digit numeric code.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }
                    string cashPayment = "UPDATE walk_in_appointment SET ServiceStatus = @status, NetPrice = @net, VatAmount = @vat, DiscountAmount = @discount, " +
                                        "GrossAmount = @gross, CashGiven = @cash, DueChange = @change, PaymentMethod = @payment, CheckedOutBy = @mngr " +
                                        "WHERE TransactionNumber = @transactNum"; // cash query
                    string bankPayment = "UPDATE walk_in_appointment SET ServiceStatus = @status, NetPrice = @net, VatAmount = @vat, DiscountAmount = @discount, " +
                                        "GrossAmount = @gross, PaymentMethod = @payment, CardName = @cardname, CardNumber = @cardNum, " +
                                        "CVC = @cvc, CardExpiration = @expiration, CheckedOutBy = @mngr " +
                                        "WHERE TransactionNumber = @transactNum"; // credit card and paypal query
                    string walletPayment = "UPDATE walk_in_appointment SET ServiceStatus = @status, NetPrice = @net, VatAmount = @vat, DiscountAmount = @discount, " +
                                        "GrossAmount = @gross, PaymentMethod = @payment, WalletNumber = @walletNum, WalletPIN = @walletPin, WalletOTP = @walletOTP, CheckedOutBy = @mngr " +
                                        "WHERE TransactionNumber = @transactNum"; // gcash and paymaya query
                    string productPayment = "UPDATE orderproducthistory SET ProductStatus = @status WHERE TransactionNumber = @transactNum";

                    if (RecPayServiceCashPaymentRB.Checked == true)
                    {
                        MySqlCommand cmd = new MySqlCommand(cashPayment,  connection);
                        cmd.Parameters.AddWithValue("@status", "Paid");
                        cmd.Parameters.AddWithValue("@net", netAmount);
                        cmd.Parameters.AddWithValue("@vat", vat);
                        cmd.Parameters.AddWithValue("@discount", discount);
                        cmd.Parameters.AddWithValue("@gross", grossAmount);
                        cmd.Parameters.AddWithValue("@cash", cash);
                        cmd.Parameters.AddWithValue("@change", change);
                        cmd.Parameters.AddWithValue("@payment", paymentMethod);
                        cmd.Parameters.AddWithValue("@mngr", mngr);
                        cmd.Parameters.AddWithValue("@transactNum", transactNum);

                        cmd.ExecuteNonQuery();
                        // Successful update
                        MessageBox.Show("Service successfully been paid through cash.", "Hooray!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Inventory.PanelShow(MngrInventoryTypePanel);
                    }
                    else if (RecPayServiceCCPaymentRB.Checked == true || RecPayServicePPPaymentRB.Checked == true)
                    {
                        MySqlCommand cmd = new MySqlCommand(bankPayment, connection);
                        cmd.Parameters.AddWithValue("@status", "Paid");
                        cmd.Parameters.AddWithValue("@net", netAmount);
                        cmd.Parameters.AddWithValue("@vat", vat);
                        cmd.Parameters.AddWithValue("@discount", discount);
                        cmd.Parameters.AddWithValue("@gross", grossAmount);
                        cmd.Parameters.AddWithValue("@payment", paymentMethod);
                        cmd.Parameters.AddWithValue("@cardname", cardName);
                        cmd.Parameters.AddWithValue("@cardNum", cardNum);
                        cmd.Parameters.AddWithValue("@cvc", CVC);
                        cmd.Parameters.AddWithValue("@expiration", expire);
                        cmd.Parameters.AddWithValue("@mngr", mngr);
                        cmd.Parameters.AddWithValue("@transactNum", transactNum);

                        cmd.ExecuteNonQuery();
                        // Successful update
                        MessageBox.Show("Service successfully been paid through bank.", "Hooray!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Inventory.PanelShow(MngrInventoryTypePanel);
                    }
                    else if (RecPayServiceGCPaymentRB.Checked == true || MngrPayServicePMPaymentRB.Checked == true)
                    {
                        MySqlCommand cmd = new MySqlCommand(walletPayment, connection);
                        cmd.Parameters.AddWithValue("@status", "Paid");
                        cmd.Parameters.AddWithValue("@net", netAmount);
                        cmd.Parameters.AddWithValue("@vat", vat);
                        cmd.Parameters.AddWithValue("@discount", discount);
                        cmd.Parameters.AddWithValue("@gross", grossAmount);
                        cmd.Parameters.AddWithValue("@payment", paymentMethod);
                        cmd.Parameters.AddWithValue("@walletNum", walletNum);
                        cmd.Parameters.AddWithValue("@walletPin", walletPIN);
                        cmd.Parameters.AddWithValue("@walletOTP", walletOTP);
                        cmd.Parameters.AddWithValue("@mngr", mngr);
                        cmd.Parameters.AddWithValue("@transactNum", transactNum);

                        cmd.ExecuteNonQuery();
                        // Successful update
                        MessageBox.Show("Service successfully been paid through online wallet.", "Hooray!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Inventory.PanelShow(MngrInventoryTypePanel);
                    }

                    if (RecPayServiceCashPaymentRB.Checked == true)
                    {
                        MySqlCommand cmd = new MySqlCommand(productPayment, connection);
                        cmd.Parameters.AddWithValue("@status", "Paid");
                        cmd.Parameters.AddWithValue("@transactNum", transactNum);


                        cmd.ExecuteNonQuery();
                        // Successful update
                        MessageBox.Show("Service successfully been paid through cash.", "Hooray!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Inventory.PanelShow(MngrInventoryTypePanel);
                    }
                    else if (RecPayServiceCCPaymentRB.Checked == true || RecPayServicePPPaymentRB.Checked == true)
                    {
                        MySqlCommand cmd = new MySqlCommand(productPayment, connection);
                        cmd.Parameters.AddWithValue("@status", "Paid");
                        cmd.Parameters.AddWithValue("@transactNum", transactNum);

                        cmd.ExecuteNonQuery();
                        // Successful update
                        MessageBox.Show("Service successfully been paid through bank.", "Hooray!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Inventory.PanelShow(MngrInventoryTypePanel);
                    }
                    else if (RecPayServiceGCPaymentRB.Checked == true || MngrPayServicePMPaymentRB.Checked == true)
                    {
                        MySqlCommand cmd = new MySqlCommand(productPayment, connection);
                        cmd.Parameters.AddWithValue("@status", "Paid");
                        cmd.Parameters.AddWithValue("@transactNum", transactNum);

                        cmd.ExecuteNonQuery();
                        // Successful update
                        MessageBox.Show("Service successfully been paid through online wallet.", "Hooray!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Inventory.PanelShow(MngrInventoryTypePanel);
                    }
                }
            }
            catch (MySqlException ex)
            {
                // Handle MySQL database exception
                MessageBox.Show("An error occurred: " + ex.Message, "Manager payment transaction failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false; // Return false in case of an exception
            }
            finally
            {
                // Make sure to close the connection
                connection.Close();
            }
            return true;
        }


        private void MngrPayServicePaymentButton_Click(object sender, EventArgs e)
        {
            if (UpdateWalk_in_AppointmentDB())
            {
                RecPayServiceClientNameLbl.Text = "";
                MngrLoadCompletedTrans();
                InvoiceReceiptGenerator();
            }
        }

        private void RecPayServiceBtn_Click(object sender, EventArgs e)
        {
            Transaction.PanelShow(RecPayServicePanel);
            MngrLoadCompletedTrans();
        }

        private void RecWalkinAttendingStaffComboText_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        // Method to get image bytes from resource
        private byte[] GetImageBytesFromResource(string resourceName)
        {
            try
            {
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                {
                    if (stream != null)
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            stream.CopyTo(memoryStream);
                            return memoryStream.ToArray();
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Resource stream for '{resourceName}' is null.", "Manager Receipt Generator Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Manager Receipt Generator Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private void InvoiceReceiptGenerator()
        {
            DateTime currentDate = MngrDateTimePicker.Value;
            string datetoday = currentDate.ToString("MM-dd-yyyy dddd");
            string timePrinted = currentDate.ToString("hh:mm tt");
            string timePrintedFile = currentDate.ToString("hh-mm-ss");
            string transactNum = RecPayServiceTransactNumLbl.Text;
            string clientName = RecPayServiceClientNameLbl.Text;
            string receptionName = RecNameLbl.Text;
            string legal = "Thank you for trusting Enchanté Salon for your beauty needs." +
                " This receipt will serve as your sales invoice of any services done in Enchanté Salon." +
                " Any concerns about your services please ask and show this receipt in the frontdesk of Enchanté Salon.";
            // Increment the file name

            // Generate a unique filename for the PDF
            string fileName = $"Enchanté-Receipt-{transactNum}-{timePrintedFile}.pdf";

            // Create a SaveFileDialog to choose the save location
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF Files|*.pdf";
            saveFileDialog.FileName = fileName;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;

                // Create a new document with custom page size (8.5"x4.25" in landscape mode)
                Document doc = new Document(new iTextSharp.text.Rectangle(Utilities.MillimetersToPoints(133f), Utilities.MillimetersToPoints(203f)));

                try
                {
                    // Create a PdfWriter instance
                    PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));

                    // Open the document for writing
                    doc.Open();

                    //string imagePath = "C:\\Users\\Pepper\\source\\repos\\Enchante\\Resources\\Enchante Logo (200 x 200 px) (1).png"; // Replace with the path to your logo image
                    // Load the image from project resources
                    //if (File.Exists(imagePath))
                    //{
                    //    //iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(imagePath);
                    //}

                    // Load the image from project resources
                    byte[] imageBytes = GetImageBytesFromResource("Enchante.Resources.Enchante Logo (200 x 200 px) (1).png");

                    if (imageBytes != null)
                    {
                        iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(imageBytes);
                        logo.ScaleAbsolute(50f, 50f);
                        logo.Alignment = Element.ALIGN_CENTER;
                        doc.Add(logo);
                    }
                    else
                    {
                        MessageBox.Show("Error loading image from resources.", "Manager Receipt Generator Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    };

                    iTextSharp.text.Font headerFont = FontFactory.GetFont("Courier", 16, iTextSharp.text.Font.BOLD);
                    iTextSharp.text.Font boldfont = FontFactory.GetFont("Courier", 10, iTextSharp.text.Font.BOLD);
                    iTextSharp.text.Font font = FontFactory.GetFont("Courier", 10, iTextSharp.text.Font.NORMAL);

                    // Create a centered alignment for text
                    iTextSharp.text.Paragraph centerAligned = new Paragraph();
                    centerAligned.Alignment = Element.ALIGN_CENTER;

                    // Add centered content to the centerAligned Paragraph
                    centerAligned.Add(new Chunk("Enchanté Salon", headerFont));
                    centerAligned.Add(new Chunk("\n69th flr. Enchanté Bldg. Ortigas Extension Ave. \nManggahan, Pasig City 1611 Philippines", font));
                    centerAligned.Add(new Chunk("\nTel. No.: (1101) 111-1010", font));
                    centerAligned.Add(new Chunk($"\nDate: {datetoday} Time: {timePrinted}", font));

                    // Add the centered content to the document
                    doc.Add(centerAligned);
                    doc.Add(new Chunk("\n")); // New line

                    doc.Add(new Paragraph($"Transaction No.: {transactNum}", font));
                    //doc.Add(new Paragraph($"Order Date: {today}", font));
                    doc.Add(new Paragraph($"Reception Name: {receptionName}", font));
                    doc.Add(new Chunk("\n")); // New line

                    doc.Add(new LineSeparator()); // Dotted line
                    PdfPTable columnHeaderTable = new PdfPTable(4);
                    columnHeaderTable.SetWidths(new float[] { 10f, 10f, 5f, 5f }); // Column widths
                    columnHeaderTable.DefaultCell.Border = PdfPCell.NO_BORDER;
                    columnHeaderTable.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
                    columnHeaderTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    columnHeaderTable.AddCell(new Phrase("Staff or\nProduct ID", boldfont));
                    columnHeaderTable.AddCell(new Phrase("Services or \nProducts", boldfont));
                    columnHeaderTable.AddCell(new Phrase("Qty.", boldfont));
                    columnHeaderTable.AddCell(new Phrase("Total Price", boldfont));
                    doc.Add(columnHeaderTable);
                    doc.Add(new LineSeparator()); // Dotted line
                    // Iterate through the rows of your 

                    // Add cells to the item table
                    PdfPTable serviceTable = new PdfPTable(4); 
                    serviceTable.SetWidths(new float[] { 5f, 5f, 3f, 3f }); // Column widths
                    serviceTable.DefaultCell.Border = PdfPCell.NO_BORDER;
                    serviceTable.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
                    serviceTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

                    foreach (DataGridViewRow row in RecPayServicesAcquiredDGV.Rows)
                    {
                        try
                        {
                            string serviceName = row.Cells["SelectedService"].Value?.ToString();
                            if (string.IsNullOrEmpty(serviceName))
                            {
                                continue; // Skip empty rows
                            }

                            string staffID = row.Cells["AttendingStaff"].Value?.ToString();
                            string itemTotalcost = row.Cells["ServicePrice"].Value?.ToString();


                            serviceTable.AddCell(new Phrase(staffID, font));
                            serviceTable.AddCell(new Phrase(serviceName, font));
                            serviceTable.AddCell(new Phrase("1", font));
                            serviceTable.AddCell(new Phrase(itemTotalcost, font));
                            doc.Add(serviceTable);

                        }
                        catch (Exception ex)
                        {
                            // Handle or log any exceptions that occur while processing DataGridView data
                            MessageBox.Show("An error occurred: " + ex.Message, "Receipt Generator Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    // Add cells to the item table
                    PdfPTable productTable = new PdfPTable(4);
                    productTable.SetWidths(new float[] { 5f, 5f, 3f, 3f }); // Column widths
                    productTable.DefaultCell.Border = PdfPCell.NO_BORDER;
                    productTable.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
                    productTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

                    foreach (DataGridViewRow row in RecPayServiceCOProdDGV.Rows)
                    {
                        try
                        {
                            string itemName = row.Cells["ItemName"].Value?.ToString();
                            if (string.IsNullOrEmpty(itemName))
                            {
                                continue; // Skip empty rows
                            }
                            string itemID = row.Cells["ItemID"].Value?.ToString();
                            string qty = row.Cells["Qty"].Value?.ToString();
                            string itemTotalcost = row.Cells["ItemTotalPrice"].Value?.ToString();

                            productTable.AddCell(new Phrase(itemID, font));
                            productTable.AddCell(new Phrase(itemName, font));
                            productTable.AddCell(new Phrase(qty, font));
                            productTable.AddCell(new Phrase(itemTotalcost, font));
                            // Add the item table to the document
                            doc.Add(productTable);
                        }
                        catch (Exception ex)
                        {
                            // Handle or log any exceptions that occur while processing DataGridView data
                            MessageBox.Show("An error occurred: " + ex.Message, "Receipt Generator Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }



                    doc.Add(new Chunk("\n")); // New line
                    doc.Add(new LineSeparator()); // Dotted line
                    doc.Add(new Chunk("\n")); // New line

                    // Total from your textboxes as decimal
                    decimal netAmount = decimal.Parse(MngrPayServiceNetAmountBox.Text);
                    decimal discount = decimal.Parse(RecWalkinDiscountBox.Text);
                    decimal vat = decimal.Parse(MngrPayServiceVATBox.Text);
                    decimal grossAmount = decimal.Parse(MngrPayServiceGrossAmountBox.Text);
                    decimal cash = decimal.Parse(MngrPayServiceCashBox.Text);
                    decimal change = decimal.Parse(MngrPayServiceChangeBox.Text);

                    // Create a new table for the "Total" section
                    PdfPTable totalTable = new PdfPTable(2); // 2 columns for the "Total" table
                    totalTable.SetWidths(new float[] { 5f, 3f }); // Column widths
                    totalTable.DefaultCell.Border = PdfPCell.NO_BORDER;

                    int totalRowCount = RecPayServicesAcquiredDGV.Rows.Count + RecPayServiceCOProdDGV.Rows.Count;

                    // Add cells to the "Total" table
                    totalTable.AddCell(new Phrase($"Total # of Service/Products ({totalRowCount})", font));
                    totalTable.AddCell(new Phrase($"Php {grossAmount:F2}", font));
                    totalTable.AddCell(new Phrase($"Cash Given", font));
                    totalTable.AddCell(new Phrase($"Php {cash:F2}", font));
                    totalTable.AddCell(new Phrase($"Change", font));
                    totalTable.AddCell(new Phrase($"Php {change:F2}", font));


                    // Add the "Total" table to the document
                    doc.Add(totalTable);
                    doc.Add(new Chunk("\n")); // New line

                    // Create a new table for the "VATable" section
                    PdfPTable vatTable = new PdfPTable(2); // 2 columns for the "VATable" table
                    vatTable.SetWidths(new float[] { 5f, 3f }); // Column widths
                    vatTable.DefaultCell.Border = PdfPCell.NO_BORDER;

                    // Add cells to the "VATable" table
                    vatTable.AddCell(new Phrase("VATable ", font));
                    vatTable.AddCell(new Phrase($"Php {netAmount:F2}", font));
                    vatTable.AddCell(new Phrase("VAT Tax (12%)", font));
                    vatTable.AddCell(new Phrase($"Php {vat:F2}", font));
                    vatTable.AddCell(new Phrase("Discount (20%)", font));
                    vatTable.AddCell(new Phrase($"Php {discount:F2}", font));

                    // Add the "VATable" table to the document
                    doc.Add(vatTable);


                    // Add the "Served To" section
                    doc.Add(new Chunk("\n")); // New line
                    doc.Add(new Paragraph($"Served To: {clientName}", font));
                    doc.Add(new Paragraph("Address:_______________________________", font));
                    doc.Add(new Paragraph("TIN No.:_______________________________", font));

                    // Add the legal string with center alignment
                    Paragraph paragraph_footer = new Paragraph($"\n\n{legal}", font);
                    paragraph_footer.Alignment = Element.ALIGN_CENTER;
                    doc.Add(paragraph_footer);
                }
                catch (DocumentException de)
                {
                    MessageBox.Show("An error occurred: " + de.Message, "Receipt Generator Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (IOException ioe)
                {
                    MessageBox.Show("An error occurred: " + ioe.Message, "Receipt Generator Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // Close the document
                    doc.Close();
                }

                //MessageBox.Show($"Receipt saved as {filePath}", "Receipt Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void LoadPreferredStaffComboBox()
        {
            using (MySqlConnection connection = new MySqlConnection(mysqlconn))
            {
                connection.Open();

                string query = "SELECT EmployeeID, Gender, LastName, FirstName FROM systemusers WHERE EmployeeCategory = @FilterValue";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@FilterValue", filterstaffbyservicecategory);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    RecWalkinAttendingStaffSelectedComboBox.Items.Clear();
                    RecWalkinAttendingStaffSelectedComboBox.Items.Add("Select a Preferred Staff"); // Kung babaguhin to babaguhin yung line 4942 messagebox

                    while (reader.Read())
                    {
                        string employeeID = reader.GetString("EmployeeID");
                        string gender = reader.GetString("Gender");
                        string lastName = reader.GetString("LastName");
                        string firstName = reader.GetString("FirstName");

                        string comboBoxItem = $"{employeeID}-{gender}-{lastName}, {firstName}";

                        RecWalkinAttendingStaffSelectedComboBox.Items.Add(comboBoxItem);
                    }
                }
            }
            RecWalkinAttendingStaffSelectedComboBox.SelectedIndex = 0;
        }

        private void RecWalkinAnyStaffToggleSwitch_CheckedChanged(object sender, EventArgs e)
        {
            if (haschosenacategory == false)
            {
                ShowNoServiceCategoryChosenWarningMessage();
                RecWalkinAnyStaffToggleSwitch.CheckedChanged -= RecWalkinAnyStaffToggleSwitch_CheckedChanged;
                RecWalkinAnyStaffToggleSwitch.Checked = false;
                RecWalkinAttendingStaffLbl.Visible = false;
                RecWalkinAttendingStaffSelectedComboBox.Visible = false;
                RecWalkinAnyStaffToggleSwitch.CheckedChanged += RecWalkinAnyStaffToggleSwitch_CheckedChanged;
                return;
            }
            else
            {
                if (RecWalkinAnyStaffToggleSwitch.Checked)
                {
                    RecWalkinPreferredStaffToggleSwitch.Checked = false;
                    RecWalkinAttendingStaffSelectedComboBox.Enabled = false;
                    RecWalkinAttendingStaffLbl.Visible = false;
                    RecWalkinAttendingStaffSelectedComboBox.Visible = false;
                    selectedStaffID = "Anyone";
                    RecWalkinAttendingStaffSelectedComboBox.Items.Clear();
                }
            }
        }

        private void RecWalkinPreferredStaffToggleSwitch_CheckedChanged(object sender, EventArgs e)
        {
            if (haschosenacategory == false)
            {
                ShowNoServiceCategoryChosenWarningMessage();
                RecWalkinPreferredStaffToggleSwitch.CheckedChanged -= RecWalkinPreferredStaffToggleSwitch_CheckedChanged;
                RecWalkinPreferredStaffToggleSwitch.Checked = false;
                RecWalkinAttendingStaffLbl.Visible = false;
                RecWalkinAttendingStaffSelectedComboBox.Visible = false;
                RecWalkinPreferredStaffToggleSwitch.CheckedChanged += RecWalkinPreferredStaffToggleSwitch_CheckedChanged;
                return;
            }
            else
            {
                if (RecWalkinPreferredStaffToggleSwitch.Checked && RecWalkinAttendingStaffSelectedComboBox.SelectedText != "Select a Preferred Staff")
                {
                    RecWalkinAnyStaffToggleSwitch.Checked = false;
                    RecWalkinAttendingStaffSelectedComboBox.Enabled = true;
                    RecWalkinAttendingStaffLbl.Visible = true;
                    RecWalkinAttendingStaffSelectedComboBox.Visible = true;
                    LoadPreferredStaffComboBox();
                }
                else
                {
                    selectedStaffID = "Anyone";
                    RecWalkinAttendingStaffSelectedComboBox.Enabled = false;
                    RecWalkinAttendingStaffLbl.Visible = false;
                    RecWalkinAttendingStaffSelectedComboBox.Visible = false;
                    RecWalkinAttendingStaffSelectedComboBox.Items.Clear();
                }
            }

        }

        private void ShowNoServiceCategoryChosenWarningMessage()
        {
            RecWalkinNoServiceCategoryChosenWarningLbl.Visible = true;
            AnimateShakeEffect(RecWalkinNoServiceCategoryChosenWarningLbl);
            Timer timer = new Timer();
            timer.Interval = 1500; // 1 seconds
            timer.Tick += (s, e) =>
            {
                RecWalkinNoServiceCategoryChosenWarningLbl.Visible = false;
                timer.Stop();
            };
            timer.Start();
        }

        private void AnimateShakeEffect(System.Windows.Forms.Control control)
        {
            int originalX = control.Location.X;
            Random rand = new Random();
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 30; // 
            timer.Tick += (s, e) =>
            {
                int newX = originalX + rand.Next(-4, 4);
                control.Location = new System.Drawing.Point(newX, control.Location.Y);
            };
            timer.Start();
        }

        private void RecWalkinAttendingStaffSelectedComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RecWalkinAttendingStaffSelectedComboBox.SelectedItem != null)
            {
                string selectedValue = RecWalkinAttendingStaffSelectedComboBox.SelectedItem.ToString();
                selectedStaffID = selectedValue.Substring(0, 11);
            }
        }

        private void RecAppointmentExitBtn_Click(object sender, EventArgs e)
        {

        }

        //PANEL OF WALK-IN REVENUE
        #region
        private void IncomeBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(MngrWalkinSalesPeriod.Text))
            {
                MessageBox.Show("Please select a sale period.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DateTime toDate;
            string selectedCategory = MngrWalkinSalesSelectCatBox.SelectedItem?.ToString();
            string salePeriod = MngrWalkinSalesPeriod.SelectedItem.ToString();
            DateTime fromDate = DateTime.MinValue;

            switch (salePeriod)
            {
                case "Day":
                    if (!DateTime.TryParseExact(MngrWalkinSalesSelectedPeriodText.Text, "MM-dd-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fromDate))
                    {
                        MessageBox.Show("Please choose a day in the calendar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    toDate = fromDate;
                    break;
                case "Week":
                    if (MngrWalkinSalesSelectedPeriodText.Text.Length < 23 ||
                        !DateTime.TryParseExact(MngrWalkinSalesSelectedPeriodText.Text.Substring(0, 10), "MM-dd-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fromDate) ||
                        !DateTime.TryParseExact(MngrWalkinSalesSelectedPeriodText.Text.Substring(14), "MM-dd-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out toDate))
                    {
                        MessageBox.Show("Please choose a week in the calendar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    break;
                case "Month":
                    if (!DateTime.TryParseExact(MngrWalkinSalesSelectedPeriodText.Text, "MMMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fromDate))
                    {
                        MessageBox.Show("Please choose a month in the calendar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    toDate = fromDate.AddMonths(1).AddDays(-1);
                    break;
                case "Specific Date Range":
                    fromDate = MngrWalkinSalesFromDatePicker.Value;
                    toDate = MngrWalkinSalesToDatePicker.Value;
                    break;
                default:
                    MessageBox.Show("Invalid SalePeriod selection.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }


            if (fromDate > toDate)
            {
                MessageBox.Show("From Date cannot be ahead of To Date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(selectedCategory))
            {
                MessageBox.Show("Please select a category.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string connectionString = "Server=localhost;Database=enchante;Uid=root;Pwd=;";

            List<DateTime> dates = new List<DateTime>();
            Dictionary<string, List<decimal>> categoryRevenues = new Dictionary<string, List<decimal>>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = @"SELECT STR_TO_DATE(AppointmentDate, '%m-%d-%Y') AS AppointmentDay, 
                            ServiceCategory,
                            SUM(CAST(ServicePrice AS DECIMAL(10, 2))) AS TotalRevenue 
                            FROM servicehistory 
                            WHERE ServiceStatus = 'Completed' 
                            AND STR_TO_DATE(AppointmentDate, '%m-%d-%Y') BETWEEN @FromDate AND @ToDate ";

                    if (selectedCategory != "All Categories")
                    {
                        query += " AND ServiceCategory = @SelectedCategory";
                    }

                    query += " GROUP BY AppointmentDay, ServiceCategory";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@FromDate", fromDate.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@ToDate", toDate.ToString("yyyy-MM-dd"));

                    if (selectedCategory != "All Categories")
                    {
                        command.Parameters.AddWithValue("@SelectedCategory", selectedCategory);
                    }

                    MySqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        DateTime appointmentDay = (DateTime)reader["AppointmentDay"];
                        string category = (string)reader["ServiceCategory"];
                        decimal totalRevenue = (decimal)reader["TotalRevenue"];

                        if (!categoryRevenues.ContainsKey(category))
                        {
                            categoryRevenues[category] = new List<decimal>();
                        }

                        categoryRevenues[category].Add(totalRevenue);

                        if (!dates.Contains(appointmentDay))
                        {
                            dates.Add(appointmentDay);
                        }
                    }

                    reader.Close();

                    MngrWalkinSalesGraph.Series.Clear();
                    MngrWalkinSalesGraph.Legends.Clear();

                    foreach (var category in categoryRevenues.Keys)
                    {
                        Series series = MngrWalkinSalesGraph.Series.Add($"{category} Revenue");
                        series.ChartType = SeriesChartType.Line;
                        series.BorderWidth = 3;

                        for (int i = 0; i < dates.Count; i++)
                        {
                            if (categoryRevenues[category].Count > i)
                            {
                                series.Points.AddXY(dates[i].ToShortDateString(), categoryRevenues[category][i]);
                                series.Points[i].MarkerStyle = MarkerStyle.Circle;
                                series.Points[i].MarkerSize = 8;
                            }
                            else
                            {
                                series.Points.AddXY(dates[i].ToShortDateString(), 0);
                            }
                        }
                    }

                    MngrWalkinSalesGraph.ChartAreas[0].AxisX.Title = "Dates";
                    MngrWalkinSalesGraph.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
                    MngrWalkinSalesGraph.ChartAreas[0].AxisY.Title = "Revenue";
                    MngrWalkinSalesGraph.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Arial", 10, FontStyle.Bold);

                    MngrWalkinSalesGraph.Legends.Add("Legend1");
                    MngrWalkinSalesGraph.Legends[0].Enabled = true;
                    MngrWalkinSalesGraph.Legends[0].Docking = Docking.Bottom;

                    DataTable dt = new DataTable();
                    dt.Columns.Add("TransactionNumber");
                    dt.Columns.Add("AppointmentDate");
                    dt.Columns.Add("TotalServicePrice", typeof(decimal));

                    string transNumQuery = @"SELECT TransactionNumber, AppointmentDate, SUM(CAST(ServicePrice AS DECIMAL(10, 2))) AS TotalServicePrice 
                    FROM servicehistory 
                    WHERE ServiceStatus = 'Completed' 
                    AND STR_TO_DATE(AppointmentDate, '%m-%d-%Y %W') BETWEEN @FromDate AND @ToDate ";

                    if (selectedCategory != "All Categories")
                    {
                        transNumQuery += " AND ServiceCategory = @SelectedCategory";
                    }

                    transNumQuery += " GROUP BY TransactionNumber";

                    MySqlCommand transNumCommand = new MySqlCommand(transNumQuery, connection);
                    transNumCommand.Parameters.AddWithValue("@FromDate", fromDate);
                    transNumCommand.Parameters.AddWithValue("@ToDate", toDate);

                    if (selectedCategory != "All Categories")
                    {
                        transNumCommand.Parameters.AddWithValue("@SelectedCategory", selectedCategory);
                    }

                    using (MySqlDataReader transNumReader = transNumCommand.ExecuteReader())
                    {
                        while (transNumReader.Read())
                        {
                            string transactionNumber = transNumReader["TransactionNumber"].ToString();
                            string appointmentDate = transNumReader["AppointmentDate"].ToString();
                            decimal totalServicePrice = (decimal)transNumReader["TotalServicePrice"];

                            dt.Rows.Add(transactionNumber, appointmentDate, totalServicePrice);
                        }
                    }

                    MngrWalkinSalesTransRepDGV.DataSource = dt;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void SalePeriod_SelectedIndexChanged(object sender, EventArgs e)
        {

            MngrWalkinSalesSelectedPeriodText.Text = "";
            string selectedItem = MngrWalkinSalesPeriod.SelectedItem.ToString();

            if (selectedItem == "Day" || selectedItem == "Week" || selectedItem == "Month")
            {
                MngrWalkinSalesPeriodCalendar.Visible = true;
                MngrWalkinSalesFromLbl.Visible = false;
                MngrWalkinSalesToLbl.Visible = false;
                MngrWalkinSalesFromDatePicker.Visible = false;
                MngrWalkinSalesToDatePicker.Visible = false;
                MngrWalkinSalesSelectedPeriodLbl.Visible = true;
                MngrWalkinSalesSelectedPeriodText.Visible = true;
            }

            else if (selectedItem == "Specific Date Range")
            {
                MngrWalkinSalesPeriodCalendar.Visible = false;
                MngrWalkinSalesFromLbl.Visible = true;
                MngrWalkinSalesToLbl.Visible = true;
                MngrWalkinSalesFromDatePicker.Visible = true;
                MngrWalkinSalesToDatePicker.Visible = true;
                MngrWalkinSalesSelectedPeriodLbl.Visible = false;
                MngrWalkinSalesSelectedPeriodText.Visible = false;
            }
        }

        private void SalesPeriodCalendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            DateTime selectedDate = MngrWalkinSalesPeriodCalendar.SelectionStart;
            string selectedPeriod = "";
            string salePeriod = MngrWalkinSalesPeriod.SelectedItem.ToString();

            switch (salePeriod)
            {
                case "Day":
                    selectedPeriod = selectedDate.ToString("MM-dd-yyyy");
                    break;
                case "Week":
                    DateTime monday = selectedDate.AddDays(-(int)selectedDate.DayOfWeek + (int)DayOfWeek.Monday);
                    DateTime sunday = monday.AddDays(6);
                    selectedPeriod = monday.ToString("MM-dd-yyyy") + " to " + sunday.ToString("MM-dd-yyyy");
                    break;
                case "Month":
                    selectedPeriod = selectedDate.ToString("MMMM-yyyy");
                    break;
                default:
                    break;
            }
            MngrWalkinSalesSelectedPeriodText.Text = selectedPeriod;
        }

        private void View_Click(object sender, EventArgs e)
        {
            ViewWalkinSales();
        }

        private void ViewWalkinSales()
        {
            string connectionString = "Server=localhost;Database=enchante;Uid=root;Pwd=;";

            if (MngrWalkinSalesTransRepDGV == null || MngrWalkinSalesTransRepDGV.SelectedRows.Count == 0 || MngrWalkinSalesTransRepDGV.SelectedRows[0].Cells["TransactionNumber"] == null)
            {
                MessageBox.Show("Please select a row to view.", "No Row Selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string transactionNumber = MngrWalkinSalesTransRepDGV.SelectedRows[0].Cells["TransactionNumber"].Value?.ToString();

            if (string.IsNullOrEmpty(transactionNumber))
            {
                MessageBox.Show("TransactionNumber is null or empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string categoryFilter = "";
            if (MngrWalkinSalesSelectCatBox.SelectedItem?.ToString() != "All Categories")
            {
                categoryFilter = "AND ServiceCategory = @ServiceCategory";
            }

            string query = "SELECT ServiceCategory, SelectedService, ServicePrice FROM servicehistory WHERE TransactionNumber = @TransactionNumber AND ServiceStatus = 'Completed' " + categoryFilter;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TransactionNumber", transactionNumber);

                    if (MngrWalkinSalesSelectCatBox.SelectedItem?.ToString() != "All Categories")
                    {
                        command.Parameters.AddWithValue("@ServiceCategory", MngrWalkinSalesSelectCatBox.SelectedItem?.ToString());
                    }

                    DataTable dataTable = new DataTable();

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }

                    MngrWalkinSalesTransServiceHisDGV.DataSource = dataTable;

                    // Display the TransactionNumber in the TextBox
                    MngrWalkinSalesTransIDShow.Text = transactionNumber;
                }
                connection.Close();
            }
        }
        private void TransNum_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ViewWalkinSales();
        }
        private void MngrWalkinSalesExitBtn_Click(object sender, EventArgs e)
        {
            Inventory.PanelShow(MngrInventoryTypePanel);

        }
        #endregion

        private void MngrInventoryInDemandBtn_Click(object sender, EventArgs e)
        {
            Inventory.PanelShow(MngrIndemandPanel);
        }
        //PANEL OF SERVICE DEMAND
        #region
        private void ServiceHistoryBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (MngrIndemandServiceHistoryPeriod.SelectedItem == null || string.IsNullOrEmpty(MngrIndemandServiceHistoryPeriod.SelectedItem.ToString()))
                {
                    MessageBox.Show("Please select a service history period.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                DateTime fromDate, toDate;

                string selectedPeriod = MngrIndemandServiceHistoryPeriod.SelectedItem.ToString();

                if (selectedPeriod == "Day" || selectedPeriod == "Week" || selectedPeriod == "Month")
                {
                    if (string.IsNullOrWhiteSpace(MngrIndemandSelectPeriod.Text))
                    {
                        MessageBox.Show("Please provide a date for the selected period.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                if (selectedPeriod == "Day")
                {
                    fromDate = toDate = DateTime.ParseExact(MngrIndemandSelectPeriod.Text, "MM-dd-yyyy", CultureInfo.InvariantCulture);
                }
                else if (selectedPeriod == "Week")
                {
                    string[] dates = MngrIndemandSelectPeriod.Text.Split(new string[] { " to " }, StringSplitOptions.None);
                    fromDate = DateTime.ParseExact(dates[0], "MM-dd-yyyy", CultureInfo.InvariantCulture);
                    toDate = DateTime.ParseExact(dates[1], "MM-dd-yyyy", CultureInfo.InvariantCulture);
                }
                else if (selectedPeriod == "Month")
                {
                    fromDate = new DateTime(DateTime.ParseExact(MngrIndemandSelectPeriod.Text, "MMMM-yyyy", CultureInfo.InvariantCulture).Year,
                                            DateTime.ParseExact(MngrIndemandSelectPeriod.Text, "MMMM-yyyy", CultureInfo.InvariantCulture).Month,
                                            1);
                    toDate = fromDate.AddMonths(1).AddDays(-1);
                }
                else
                {
                    fromDate = MngrIndemandDatePickerFrom.Value;
                    toDate = MngrIndemandDatePickerTo.Value;
                }

                if (MngrIndemandSelectCatBox.SelectedItem == null)
                {
                    MessageBox.Show("Please select a category.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string selectedCategory = MngrIndemandSelectCatBox.SelectedItem.ToString();

                string query;
                Dictionary<string, int> counts;

                if (selectedCategory == "Top Service Category")
                {
                    query = @"
                        SELECT 
                            ServiceCategory,
                            AttendingStaff,
                        COUNT(*) AS CategoryCount
                        FROM 
                            servicehistory 
                        WHERE 
                            ServiceStatus = 'Completed' 
                            AND STR_TO_DATE(AppointmentDate, '%m-%d-%Y') BETWEEN @FromDate AND @ToDate 
                        GROUP BY
                            ServiceCategory, AttendingStaff";
                    counts = new Dictionary<string, int>();
                }
                else
                {
                    query = @"
                        SELECT 
                            AttendingStaff,
                            STR_TO_DATE(AppointmentDate, '%m-%d-%Y') AS AppointmentDay, 
                            ServiceCategory,
                            SelectedService
                        FROM 
                            servicehistory 
                        WHERE 
                            ServiceStatus = 'Completed' 
                            AND STR_TO_DATE(AppointmentDate, '%m-%d-%Y') BETWEEN @FromDate AND @ToDate 
                            AND ServiceCategory = @SelectedCategory";
                    counts = new Dictionary<string, int>();
                }

                using (MySqlConnection connection = new MySqlConnection("Server=localhost;Database=enchante;Uid=root;Pwd=;"))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FromDate", fromDate.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@ToDate", toDate.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@SelectedCategory", selectedCategory);

                        Dictionary<string, int> serviceCounts = new Dictionary<string, int>();
                        Dictionary<string, int> staffCounts = new Dictionary<string, int>();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                if (selectedCategory == "Top Service Category")
                                {
                                    string serviceCategory = reader.GetString("ServiceCategory");
                                    int categoryCount = reader.GetInt32("CategoryCount");
                                    serviceCounts[serviceCategory] = categoryCount;

                                    string attendingStaff = reader.GetString("AttendingStaff");
                                    if (staffCounts.ContainsKey(attendingStaff))
                                    {
                                        staffCounts[attendingStaff] += categoryCount;
                                    }
                                    else
                                    {
                                        staffCounts[attendingStaff] = categoryCount;
                                    }
                                }
                                else
                                {
                                    string selectedService = reader.GetString("SelectedService");
                                    if (serviceCounts.ContainsKey(selectedService))
                                    {
                                        serviceCounts[selectedService]++;

                                        string attendingStaff = reader.GetString("AttendingStaff");
                                        if (staffCounts.ContainsKey(attendingStaff))
                                        {
                                            staffCounts[attendingStaff]++;
                                        }
                                        else
                                        {
                                            staffCounts[attendingStaff] = 1;
                                        }
                                    }
                                    else
                                    {
                                        serviceCounts[selectedService] = 1;

                                        string attendingStaff = reader.GetString("AttendingStaff");
                                        if (staffCounts.ContainsKey(attendingStaff))
                                        {
                                            staffCounts[attendingStaff]++;
                                        }
                                        else
                                        {
                                            staffCounts[attendingStaff] = 1;
                                        }
                                    }
                                }
                            }
                        }

                        if (selectedCategory == "Top Service Category")
                        {
                            MngrIndemandServiceGraph.Series.Clear();
                            var series = MngrIndemandServiceGraph.Series.Add("ServiceCount");
                            series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;

                            foreach (var kvp in serviceCounts)
                            {
                                string serviceName = kvp.Key;
                                int serviceCount = kvp.Value;

                                var dataPoint = series.Points.Add(serviceCount);
                                series.Points.Last().LegendText = serviceName;
                            }

                            MngrIndemandServiceGraph.Titles.Clear();
                            var title = MngrIndemandServiceGraph.Titles.Add("Top Service");
                            title.Font = new System.Drawing.Font("Arial", 12f, System.Drawing.FontStyle.Bold);

                            DataTable serviceCategoryTable = new DataTable();
                            serviceCategoryTable.Columns.Add("Service Category");
                            serviceCategoryTable.Columns.Add("Top Service Count");

                            foreach (var kvp in serviceCounts)
                            {
                                serviceCategoryTable.Rows.Add(kvp.Key, kvp.Value);
                            }

                            MngrIndemandServiceSelection.DataSource = serviceCategoryTable;

                            DataTable staffTable = new DataTable();
                            staffTable.Columns.Add("Rank");
                            staffTable.Columns.Add("ID");
                            staffTable.Columns.Add("First Name");
                            staffTable.Columns.Add("Last Name");
                            staffTable.Columns.Add("# of Services Done");

                            List<KeyValuePair<string, int>> sortedStaffCounts = staffCounts.ToList();
                            sortedStaffCounts.Sort((x, y) => y.Value.CompareTo(x.Value));

                            int rank = 1;
                            foreach (var kvp in sortedStaffCounts)
                            {
                                string employeeID = kvp.Key;
                                string firstName, lastName;
                                using (MySqlCommand userCommand = new MySqlCommand("SELECT FirstName, LastName FROM systemusers WHERE EmployeeID = @EmployeeID", connection))
                                {
                                    userCommand.Parameters.AddWithValue("@EmployeeID", employeeID);

                                    using (MySqlDataReader userReader = userCommand.ExecuteReader())
                                    {
                                        if (userReader.Read())
                                        {
                                            firstName = userReader.GetString("FirstName");
                                            lastName = userReader.GetString("LastName");

                                            staffTable.Rows.Add(rank, employeeID, firstName, lastName, kvp.Value);
                                            rank++;
                                        }
                                    }
                                }
                            }
                            MngrIndemandBestEmployee.DataSource = staffTable;
                        }
                        else
                        {
                            DataTable serviceTable = new DataTable();
                            serviceTable.Columns.Add("Service Name");
                            serviceTable.Columns.Add("Service Selection Counts");

                            foreach (var kvp in serviceCounts)
                            {
                                serviceTable.Rows.Add(kvp.Key, kvp.Value);
                            }

                            MngrIndemandServiceSelection.DataSource = serviceTable;

                            DataTable staffTable = new DataTable();
                            staffTable.Columns.Add("Rank");
                            staffTable.Columns.Add("ID");
                            staffTable.Columns.Add("First Name");
                            staffTable.Columns.Add("Last Name");
                            staffTable.Columns.Add("# of Services Done");

                            List<KeyValuePair<string, int>> sortedStaffCounts = staffCounts.ToList();
                            sortedStaffCounts.Sort((x, y) => y.Value.CompareTo(x.Value));
                            int rank = 1;

                            foreach (var kvp in sortedStaffCounts)
                            {
                                string employeeID = kvp.Key;
                                string firstName, lastName;
                                using (MySqlCommand userCommand = new MySqlCommand("SELECT FirstName, LastName FROM systemusers WHERE EmployeeID = @EmployeeID", connection))
                                {
                                    userCommand.Parameters.AddWithValue("@EmployeeID", employeeID);

                                    using (MySqlDataReader userReader = userCommand.ExecuteReader())
                                    {
                                        if (userReader.Read())
                                        {
                                            firstName = userReader.GetString("FirstName");
                                            lastName = userReader.GetString("LastName");

                                            staffTable.Rows.Add(rank, employeeID, firstName, lastName, kvp.Value);
                                            rank++;
                                        }
                                    }
                                }
                            }

                            DataView dv = staffTable.DefaultView;
                            dv.Sort = "# of Services Done DESC";
                            MngrIndemandBestEmployee.DataSource = dv.ToTable();

                            MngrIndemandServiceGraph.Series.Clear();
                            var pieSeries = MngrIndemandServiceGraph.Series.Add("ServiceCount");
                            pieSeries.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;

                            foreach (var kvp in serviceCounts)
                            {
                                string serviceName = kvp.Key;
                                int serviceCount = kvp.Value;

                                var dataPoint = pieSeries.Points.Add(serviceCount);
                                pieSeries.Points.Last().LegendText = serviceName;
                            }
                            MngrIndemandServiceGraph.Titles.Clear();
                            var chartTitle = MngrIndemandServiceGraph.Titles.Add("Service Demand");
                            chartTitle.Font = new System.Drawing.Font("Arial", 12f, System.Drawing.FontStyle.Bold);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ServiceHistoryPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            MngrIndemandSelectPeriod.Text = "";
            string selectedItem = MngrIndemandServiceHistoryPeriod.SelectedItem.ToString();

            if (selectedItem == "Day" || selectedItem == "Week" || selectedItem == "Month")
            {
                MngrIndemandServicePeriodCalendar.Visible = true;
                MngrIndemandFromLbl.Visible = false;
                MngrIndemandToLbl.Visible = false;
                MngrIndemandDatePickerFrom.Visible = false;
                MngrIndemandDatePickerTo.Visible = false;
                MngrIndemandSelectPeriodLbl.Visible = true;
                MngrIndemandSelectPeriod.Visible = true;
            }

            else if (selectedItem == "Specific Date Range")
            {
                MngrIndemandServicePeriodCalendar.Visible = false;
                MngrIndemandFromLbl.Visible = true;
                MngrIndemandToLbl.Visible = true;
                MngrIndemandDatePickerFrom.Visible = true;
                MngrIndemandDatePickerTo.Visible = true;
                MngrIndemandSelectPeriodLbl.Visible = false;
                MngrIndemandSelectPeriod.Visible = false;
            }
        }

        private void ServicePeriodCalendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            DateTime selectedDate = MngrIndemandServicePeriodCalendar.SelectionStart;
            string selectedPeriod = "";
            string salePeriod = MngrIndemandServiceHistoryPeriod.SelectedItem.ToString();

            switch (salePeriod)
            {
                case "Day":
                    selectedPeriod = selectedDate.ToString("MM-dd-yyyy");
                    break;
                case "Week":
                    DateTime monday = selectedDate.AddDays(-(int)selectedDate.DayOfWeek + (int)DayOfWeek.Monday);
                    DateTime sunday = monday.AddDays(6);
                    selectedPeriod = monday.ToString("MM-dd-yyyy") + " to " + sunday.ToString("MM-dd-yyyy");
                    break;
                case "Month":
                    selectedPeriod = selectedDate.ToString("MMMM-yyyy");
                    break;
                default:
                    break;
            }
            MngrIndemandSelectPeriod.Text = selectedPeriod;
        }
        private void MngrIndemandExitBtn_Click(object sender, EventArgs e)
        {
            Inventory.PanelShow(MngrInventoryTypePanel);
        }

        #endregion

        private void RecQueWinBtn_Click(object sender, EventArgs e)
        {
            Transaction.PanelShow(RecQueWinPanel);
            RecQuePreferredStaffLoadData();
            RecQueGeneralStaffLoadData();
            RecQueWinNextCustomerLbl.Text = "| NEXT IN LINE [GENERAL QUEUE]";
            RecQueWinGenCatComboText.Visible = true;
            RecQueWinGenCatComboText.SelectedIndex = 5;
            RecQueWinStaffCatComboText.SelectedIndex = 5;

        }

        private void RecQueWinExitBtn_Click(object sender, EventArgs e)
        {
            Transaction.PanelShow(RecTransactionPanel);
            RecQueWinGenCatComboText.SelectedIndex = -1;
            RecQueWinStaffCatComboText.SelectedIndex = -1;

        }
        private void RecQuePreferredStaffLoadData()
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                {
                    connection.Open();

                    string sql = "SELECT * FROM `systemusers` WHERE EmployeeType = 'Staff' ORDER BY EmployeeType";

                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    System.Data.DataTable dataTable = new System.Data.DataTable();

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);


                        RecQueWinStaffListDGV.DataSource = dataTable;
                        RecQueWinStaffListDGV.Columns[2].Visible = false;
                        RecQueWinStaffListDGV.Columns[3].Visible = false;
                        RecQueWinStaffListDGV.Columns[4].Visible = false;
                        RecQueWinStaffListDGV.Columns[5].Visible = false;
                        RecQueWinStaffListDGV.Columns[6].Visible = false;
                        RecQueWinStaffListDGV.Columns[7].Visible = false;
                        RecQueWinStaffListDGV.Columns[8].Visible = false;
                        RecQueWinStaffListDGV.Columns[9].Visible = false;
                        RecQueWinStaffListDGV.Columns[11].Visible = false;
                        RecQueWinStaffListDGV.Columns[15].Visible = false;
                        RecQueWinStaffListDGV.Columns[16].Visible = false;
                        RecQueWinStaffListDGV.Columns[17].Visible = false;


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
        }
        private void RecQuePreferredStaffCatLoadData(string category)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                {
                    connection.Open();

                    string sql = "SELECT * FROM `systemusers` WHERE EmployeeType = 'Staff' AND EmployeeCategory = @category";

                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    System.Data.DataTable dataTable = new System.Data.DataTable();
                    cmd.Parameters.AddWithValue("@category", category);

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);


                        RecQueWinStaffListDGV.DataSource = dataTable;
                        RecQueWinStaffListDGV.Columns[2].Visible = false;
                        RecQueWinStaffListDGV.Columns[3].Visible = false;
                        RecQueWinStaffListDGV.Columns[4].Visible = false;
                        RecQueWinStaffListDGV.Columns[5].Visible = false;
                        RecQueWinStaffListDGV.Columns[6].Visible = false;
                        RecQueWinStaffListDGV.Columns[7].Visible = false;
                        RecQueWinStaffListDGV.Columns[8].Visible = false;
                        RecQueWinStaffListDGV.Columns[9].Visible = false;
                        RecQueWinStaffListDGV.Columns[11].Visible = false;
                        RecQueWinStaffListDGV.Columns[15].Visible = false;
                        RecQueWinStaffListDGV.Columns[16].Visible = false;
                        RecQueWinStaffListDGV.Columns[17].Visible = false;


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
        }
        private void RecQueGeneralStaffLoadData()
        {
            string todayDate = DateTime.Today.ToString("MM-dd-yyyy dddd");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                {
                    connection.Open();

                    string sql = "SELECT * FROM `servicehistory` WHERE QueType = 'GeneralQue' AND ServiceStatus = 'Pending' AND AppointmentDate = @todayDate";
                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    System.Data.DataTable dataTable = new System.Data.DataTable();
                    cmd.Parameters.AddWithValue("@todayDate", todayDate);

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);


                        RecQueWinNextCustomerDGV.DataSource = dataTable;

                        RecQueWinNextCustomerDGV.Columns[0].Visible = false; //transact number
                        RecQueWinNextCustomerDGV.Columns[2].Visible = false; //appointment date
                        RecQueWinNextCustomerDGV.Columns[3].Visible = false; //appointment time
                        RecQueWinNextCustomerDGV.Columns[5].Visible = false; //service category
                        RecQueWinNextCustomerDGV.Columns[6].Visible = false; //attending staff
                        RecQueWinNextCustomerDGV.Columns[7].Visible = false; //service ID
                        RecQueWinNextCustomerDGV.Columns[10].Visible = false; //service start
                        RecQueWinNextCustomerDGV.Columns[11].Visible = false; //service end
                        RecQueWinNextCustomerDGV.Columns[12].Visible = false; //service duration
                        RecQueWinNextCustomerDGV.Columns[13].Visible = false; //customization
                        RecQueWinNextCustomerDGV.Columns[14].Visible = false; // add notes
                        RecQueWinNextCustomerDGV.Columns[15].Visible = false; // preferred staff
                        RecQueWinNextCustomerDGV.Columns[17].Visible = false; // Queue type
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
        }
        private void RecQueGeneralStaffCatLoadData(string category)
        {
            string todayDate = DateTime.Today.ToString("MM-dd-yyyy dddd");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                {
                    connection.Open();

                    string sql = "SELECT * FROM `servicehistory` WHERE QueType = 'GeneralQue' AND ServiceStatus = 'Pending' " +
                        "AND AppointmentDate = @todayDate AND ServiceCategory = @category";
                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    System.Data.DataTable dataTable = new System.Data.DataTable();
                    cmd.Parameters.AddWithValue("@todayDate", todayDate);
                    cmd.Parameters.AddWithValue("@category", category);

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);


                        RecQueWinNextCustomerDGV.DataSource = dataTable;

                        RecQueWinNextCustomerDGV.Columns[0].Visible = false; //transact number
                        RecQueWinNextCustomerDGV.Columns[2].Visible = false; //appointment date
                        RecQueWinNextCustomerDGV.Columns[3].Visible = false; //appointment time
                        RecQueWinNextCustomerDGV.Columns[5].Visible = false; //service category
                        RecQueWinNextCustomerDGV.Columns[6].Visible = false; //attending staff
                        RecQueWinNextCustomerDGV.Columns[7].Visible = false; //service ID
                        RecQueWinNextCustomerDGV.Columns[10].Visible = false; //service start
                        RecQueWinNextCustomerDGV.Columns[11].Visible = false; //service end
                        RecQueWinNextCustomerDGV.Columns[12].Visible = false; //service duration
                        RecQueWinNextCustomerDGV.Columns[13].Visible = false; //customization
                        RecQueWinNextCustomerDGV.Columns[14].Visible = false; // add notes
                        RecQueWinNextCustomerDGV.Columns[15].Visible = false; // preferred staff
                        RecQueWinNextCustomerDGV.Columns[17].Visible = false; // Queue type
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
        }
        private void RecQueWinStaffListDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if a valid cell is clicked
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string ID = RecQueWinStaffListDGV.Rows[e.RowIndex].Cells["EmployeeID"].Value.ToString();
                string emplFName = RecQueWinStaffListDGV.Rows[e.RowIndex].Cells["FirstName"].Value.ToString();
                string emplLName = RecQueWinStaffListDGV.Rows[e.RowIndex].Cells["LastName"].Value.ToString();

                RecQueWinEmplIDLbl.Text = ID;
                RecLoadQueuedClient(ID);
                RecQueWinNextCustomerLbl.Text = $"| NEXT IN LINE [Staff: {emplFName} {emplLName}]";
                RecQueWinGenCatComboText.Visible = false;
            }
            else
            {
                RecQueWinGenCatComboText.Visible = true;
            }
        }
        public void RecLoadQueuedClient(string ID)
        {
            string todayDate = DateTime.Today.ToString("MM-dd-yyyy dddd");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                {
                    connection.Open();

                    string sql = "SELECT * FROM `servicehistory` WHERE PreferredStaff = @emplID AND ServiceStatus = 'Pending' AND AppointmentDate = @todayDate";
                    MySqlCommand cmd = new MySqlCommand(sql, connection);

                    // Add parameters to the query
                    cmd.Parameters.AddWithValue("@emplID", ID);
                    cmd.Parameters.AddWithValue("@todayDate", todayDate);


                    System.Data.DataTable dataTable = new System.Data.DataTable();

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);

                        RecQueWinNextCustomerDGV.DataSource = dataTable;

                        RecQueWinNextCustomerDGV.Columns[0].Visible = false; //transact number
                        RecQueWinNextCustomerDGV.Columns[2].Visible = false; //appointment date
                        RecQueWinNextCustomerDGV.Columns[3].Visible = false; //appointment time
                        RecQueWinNextCustomerDGV.Columns[5].Visible = false; //service category
                        RecQueWinNextCustomerDGV.Columns[6].Visible = false; //attending staff
                        RecQueWinNextCustomerDGV.Columns[7].Visible = false; //service ID
                        RecQueWinNextCustomerDGV.Columns[10].Visible = false; //service start
                        RecQueWinNextCustomerDGV.Columns[11].Visible = false; //service end
                        RecQueWinNextCustomerDGV.Columns[12].Visible = false; //service duration
                        RecQueWinNextCustomerDGV.Columns[13].Visible = false; //customization
                        RecQueWinNextCustomerDGV.Columns[14].Visible = false; // add notes
                        RecQueWinNextCustomerDGV.Columns[15].Visible = false; // preferred staff
                        RecQueWinNextCustomerDGV.Columns[17].Visible = false; // Queue type


                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Manager Order History List");
            }
            finally
            {
                // Make sure to close the connection (if it's open)
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private void RecQueWinStaffCatComboText_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RecQueWinStaffCatComboText.Text == "Hair Styling")
            {
                RecQuePreferredStaffCatLoadData("Hair Styling");
                return;
            }
            else if (RecQueWinStaffCatComboText.Text == "Face & Skin")
            {
                RecQuePreferredStaffCatLoadData("Face & Skin");
                return;
            }
            else if (RecQueWinStaffCatComboText.Text == "Nail Care")
            {
                RecQuePreferredStaffCatLoadData("Nail Care");
                return;
            }
            else if (RecQueWinStaffCatComboText.Text == "Spa")
            {
                RecQuePreferredStaffCatLoadData("Spa");
                return;
            }
            else if (RecQueWinStaffCatComboText.Text == "Massage")
            {
                RecQuePreferredStaffCatLoadData("Massage");
                return;
            }
            else if (RecQueWinStaffCatComboText.Text == "All Categories")
            {
                RecQuePreferredStaffLoadData();
                RecQueWinNextCustomerLbl.Text = "| NEXT IN LINE [GENERAL QUEUE]";
                RecQueWinGenCatComboText.Visible = true;
                RecQueGeneralStaffLoadData();
                ;
                return;
            }

        }

        private void RecQueWinGenCatComboText_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RecQueWinGenCatComboText.Text == "Hair Styling")
            {
                RecQueGeneralStaffCatLoadData("Hair Styling");
                return;
            }
            else if (RecQueWinGenCatComboText.Text == "Face & Skin")
            {
                RecQueGeneralStaffCatLoadData("Face & Skin");
                return;
            }
            else if (RecQueWinGenCatComboText.Text == "Nail Care")
            {
                RecQueGeneralStaffCatLoadData("Nail Care");
                return;
            }
            else if (RecQueWinGenCatComboText.Text == "Spa")
            {
                RecQueGeneralStaffCatLoadData("Spa");
                return;
            }
            else if (RecQueWinGenCatComboText.Text == "Massage")
            {
                RecQueGeneralStaffCatLoadData("Massage");
                return;
            }
            else if (RecQueWinGenCatComboText.Text == "All Categories")
            {
                RecQueGeneralStaffLoadData();
                return;
            }
        }

        private void MngrPayServiceVATExemptChk_CheckedChanged(object sender, EventArgs e)
        {
            if (MngrPayServiceVATExemptChk.Checked)
            {
                ReceptionCalculateVATExemption();
            }
            else
            {
                ReceptionCalculateTotalPrice();
            }
        }
        public void ReceptionCalculateVATExemption()
        {
            // Get the Gross Amount from the TextBox (MngrGrossAmountBox)
            if (decimal.TryParse(MngrPayServiceNetAmountBox.Text, out decimal netAmount))
            {
                // For VAT exemption, set VAT Amount to zero
                decimal vatAmount = 0;

                // Set the Net Amount as the new Gross Amount
                decimal grossAmount = netAmount;

                // Display the calculated values in TextBoxes
                MngrPayServiceVATBox.Text = vatAmount.ToString("0.00");
                MngrPayServiceGrossAmountBox.Text = grossAmount.ToString("0.00");
            }
        }
        private void ProductUserControl_Click(object sender, EventArgs e)
        {
            ProductUserControl clickedControl = sender as ProductUserControl;
            if (clickedControl != null)
            {
                string itemID = clickedControl.ProductItemIDTextBox.Text;
                string itemName = clickedControl.ProductNameTextBox.Text;
                string itemPrice = clickedControl.ProductPriceTextBox.Text;

                bool itemExists = false;



                int existingRowIndex = 0;

                // Check if the item already exists in the order
                foreach (DataGridViewRow row in RecWalkinSelectedProdDGV.Rows)
                {
                    if (row.Cells["Item Name"].Value != null && row.Cells["Item Name"].Value.ToString() == itemName)
                    {
                        itemExists = true;
                        existingRowIndex = row.Index;
                        break;
                    }
                }

                if (itemExists)
                {
                    // The item already exists, increment quantity and update price
                    string quantityString = RecWalkinSelectedProdDGV.Rows[existingRowIndex].Cells["Qty"].Value?.ToString();
                    if (!string.IsNullOrEmpty(quantityString) && int.TryParse(quantityString, out int quantity))
                    {
                        decimal itemCost = decimal.Parse(RecWalkinSelectedProdDGV.Rows[existingRowIndex].Cells["Total Price"].Value?.ToString());

                        // Calculate the cost per item
                        decimal costPerItem = itemCost / quantity;

                        // Increase quantity
                        quantity++;

                        // Calculate updated item cost
                        decimal updatedCost = costPerItem * quantity;

                        // Update Qty and ItemCost in the DataGridView
                        RecWalkinSelectedProdDGV.Rows[existingRowIndex].Cells["Qty"].Value = quantity.ToString();
                        RecWalkinSelectedProdDGV.Rows[existingRowIndex].Cells["Total Price"].Value = updatedCost.ToString("F2"); // Format to two decimal places
                    }

                    else
                    {
                        // Handle the case where quantityString is empty or not a valid integer
                        // For example, show an error message or set a default value
                    }
                }
                else
                {
                    RecWalkinSelectedProdDGV.Rows.Add(itemID, "x", itemName, "-", "1", "+", itemPrice, itemPrice);

                }
            }
        }

        public void InitializeProducts()
        {
            using (MySqlConnection connection = new MySqlConnection(mysqlconn))
            {
                connection.Open();

                string query = "SELECT ItemID, ItemName, ItemStock, ItemPrice, ItemStatus, ProductPicture FROM inventory WHERE ProductType = 'Retail Product'";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string itemID = reader["ItemID"].ToString();
                    string itemName = reader["ItemName"].ToString();
                    string itemStock = reader["ItemStock"].ToString();
                    string itemPrice = reader["ItemPrice"].ToString();
                    string itemStatus = reader["ItemStatus"].ToString();
                    byte[] productPicture = (byte[])reader["ProductPicture"];

                    ProductUserControl productusercontrol = new ProductUserControl();

                    productusercontrol.ProductItemIDTextBox.Text = itemID;
                    productusercontrol.ProductNameTextBox.Text = itemName;
                    productusercontrol.ProductStockTextBox.Text = itemStock;
                    productusercontrol.ProductPriceTextBox.Text = itemPrice;
                    productusercontrol.ProductStatusTextBox.Text = itemStatus;

                    if (itemStatus == "Low Stock")
                    {
                        productusercontrol.ProductOutOfStockPictureBox.Visible = true;
                        productusercontrol.Enabled = false;
                    }
                    else
                    {
                        productusercontrol.ProductOutOfStockPictureBox.Visible = false;
                        productusercontrol.Enabled = true;
                    }

                    if (productPicture != null && productPicture.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream(productPicture))
                        {
                            System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
                            productusercontrol.ProductPicturePictureBox.Image = image;
                        }
                    }
                    else
                    {
                        productusercontrol.ProductPicturePictureBox.Image = null;
                    }

                    foreach (System.Windows.Forms.Control control in productusercontrol.Controls)
                    {
                        control.Click += ProductControlElement_Click;
                    }

                    productusercontrol.Click += ProductUserControl_Click;

                    ProductFlowLayoutPanel.Controls.Add(productusercontrol);
                }
                reader.Close();
            }

        }

        private void ProductControlElement_Click(object sender, EventArgs e)
        {
            ProductUserControl_Click((ProductUserControl)((System.Windows.Forms.Control)sender).Parent, e);
        }

        private void ProductUserControl_ProductClicked(object sender, EventArgs e)
        {
            // MAY GAMIT TO DONT DELETE UwU
            // sigi 
        }

        public string Position { get; set; }


        private void ProductVoidButton_Click(object sender, EventArgs e)
        {
            if (RecWalkinSelectedProdDGV.Rows.Count == 0)
            {
                MessageBox.Show("The product list is empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (RecWalkinSelectedProdDGV.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a product to void.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //input dialog messagebox
            string enteredPassword = GetPasswordWithAsterisks("Enter Manager Password:", "Password Required");

            // Hash the entered password
            string hashedEnteredPassword = HashHelper.HashString(enteredPassword);
            DialogResult result;

            using (MySqlConnection connection = new MySqlConnection(mysqlconn))
            {
                connection.Open();

                string query = "SELECT EmployeeType FROM systemusers WHERE HashedPass = @Password";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Password", hashedEnteredPassword);

                    // Execute the query
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string position = reader["EmployeeType"].ToString();
                            if (position == "Manager")
                            {
                                result = MessageBox.Show("Do you want to remove this item?", "Remove Item", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                                if (result == DialogResult.Yes)
                                {
                                    // Remove the selected row
                                    RecWalkinSelectedProdDGV.Rows.Clear();
                                    MessageBox.Show("Item removed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid password. You need manager permission to remove an item.", "Permission Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                        else
                        {
                            //MessageBox.Show("Invalid password. You need manager permission to remove an item.", "Permission Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            //return;
                        }
                    }
                }
            }
        }

        // Function to get password with asterisks
        private string GetPasswordWithAsterisks(string prompt, string title)
        {
            using (Form passwordForm = new Form())
            {
                System.Windows.Forms.Label label = new System.Windows.Forms.Label()
                {
                    Left = 20,
                    Top = 50,
                    Text = prompt,
                    AutoSize = true,
                    Font = new System.Drawing.Font("Arial Black", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                    ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))))
                }; 

                System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox() { 
                    Left = 20, 
                    Top = 100, 
                    Width = 420,
                    Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                    ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221))))),
                    BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(136)))), ((int)(((byte)(82))))),
                    PasswordChar = '*' 
                };
                System.Windows.Forms.Button button = new System.Windows.Forms.Button() { 
                    Text = "Void Items", 
                    Left = 325, 
                    Width = 120, 
                    Height = 40, 
                    Top = 150,
                    Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                    ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221))))),
                    BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))))
                };
                System.Windows.Forms.Button button1 = new System.Windows.Forms.Button() { 
                    Text = "Show Password", 
                    Left = 120, 
                    Width = 200, 
                    Height = 40, 
                    Top = 150,
                    Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                    ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221))))),
                    BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))))
                };

                button.Click += (sender, e) => { passwordForm.DialogResult = DialogResult.OK; };
                passwordForm.AcceptButton = button;

                // Set the fixed size for the form
                passwordForm.Size = new Size(500, 300);
                passwordForm.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
                // Disable resizing of the form
                passwordForm.FormBorderStyle = FormBorderStyle.FixedDialog;

                passwordForm.Controls.Add(label);
                passwordForm.Controls.Add(textBox);
                passwordForm.Controls.Add(button);
                passwordForm.Controls.Add(button1);

                // Center the form on the screen
                passwordForm.StartPosition = FormStartPosition.CenterScreen;
                button1.Click += (sender, e) =>
                {
                    // Toggle between showing and hiding characters
                    textBox.PasswordChar = (textBox.PasswordChar == '\0') ? '*' : '\0';
                };
                passwordForm.ShowDialog();

                return textBox.Text;
            }
        }

        private void MngrInventoryProductsTypeComboText_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (MngrInventoryProductsTypeComboText.SelectedItem != null)
            {
                if (MngrInventoryProductsTypeComboText.SelectedItem.ToString() == "Service Product")
                {
                    PDImage.Visible = false;
                    ProductImagePictureBox.Visible = false;
                    SelectImage.Visible = false;
                }
                else if (MngrInventoryProductsTypeComboText.SelectedItem.ToString() == "Retail Product")
                {
                    PDImage.Visible = true;
                    ProductImagePictureBox.Visible = true;
                    SelectImage.Visible = true;
                }
            }
        }

        private void CancelEdit_Click(object sender, EventArgs e)
        {
            shouldGenerateItemID = true;
            MngrInventoryProductsInsertBtn.Visible = true;
            MngrInventoryProductsUpdateBtn.Visible = false;       
            CancelEdit.Visible = false;
            PDImage.Visible = false;
            ProductImagePictureBox.Visible = false;
            SelectImage.Visible = false;
            MngrInventoryProductsCatComboText.Enabled = true;
            MngrInventoryProductsTypeComboText.Enabled = true;

            MngrInventoryProductsCatComboText.SelectedIndex = -1;
            MngrInventoryProductsTypeComboText.SelectedIndex = -1;
            MngrInventoryProductsStatusComboText.SelectedIndex = -1;
            MngrInventoryProductsIDText.Text = "";
            MngrInventoryProductsNameText.Text = "";
            MngrInventoryProductsPriceText.Text = "";
            MngrInventoryProductsStockText.Text = "";
        }

        private void SelectImage_Click(object sender, EventArgs e)
        {
            // Open file dialog to select an image
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg; *.jpeg; *.png; *.gif; *.bmp)|*.jpg; *.jpeg; *.png; *.gif; *.bmp|All files (*.*)|*.*";
            openFileDialog.Title = "Select Image";
            openFileDialog.Multiselect = false;

            DialogResult result = openFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                string selectedImagePath = openFileDialog.FileName;

                try
                {
                    // Load the selected image and display it in the PictureBox
                    using (System.Drawing.Image originalImage = System.Drawing.Image.FromFile(selectedImagePath))
                    {
                        System.Drawing.Image resizedImage = ResizeImage(originalImage, ProductImagePictureBox.Width, ProductImagePictureBox.Height);
                        ProductImagePictureBox.Image = resizedImage;


                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private System.Drawing.Image ResizeImage(System.Drawing.Image image, int width, int height)
        {
            float aspectRatio = (float)image.Width / image.Height;
            int targetWidth = width;
            int targetHeight = (int)(width / aspectRatio);

            if (targetHeight > height)
            {
                targetHeight = height;
                targetWidth = (int)(height * aspectRatio);
            }

            Bitmap resizedImage = new Bitmap(width, height);
            int x = (width - targetWidth) / 2;
            int y = (height - targetHeight) / 2;

            using (Graphics graphics = Graphics.FromImage(resizedImage))
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.Clear(Color.Transparent);
                graphics.DrawImage(image, new System.Drawing.Rectangle(x, y, targetWidth, targetHeight));
            }
            return resizedImage;
        }

        private void MngrInventoryProductsStockText_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(MngrInventoryProductsStockText.Text))
            {
                MngrInventoryProductsStatusComboText.SelectedItem = null;
            }

            if (int.TryParse(MngrInventoryProductsStockText.Text, out int inputValue))
            {
                if (inputValue >= 0 && inputValue <= 50)
                {
                    MngrInventoryProductsStatusComboText.SelectedItem = "Low Stock";
                }
                else if (inputValue >= 51)
                {
                    MngrInventoryProductsStatusComboText.SelectedItem = "High Stock";
                }
            }
        }

        private void RecWalkinSelectedProdDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && RecWalkinSelectedProdDGV.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
            {
                // Handle the Bin column
                if (RecWalkinSelectedProdDGV.Columns[e.ColumnIndex].Name == "Void")
                {
                    //input dialog messagebox
                    string enteredPassword = GetPasswordWithAsterisks("Enter Manager Password:", "Password Required");

                    // Hash the entered password
                    string hashedEnteredPassword = HashHelper.HashString(enteredPassword);
                    DialogResult result;

                    using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                    {
                        connection.Open();

                        string query = "SELECT EmployeeType FROM systemusers WHERE HashedPass = @Password";
                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Password", hashedEnteredPassword);

                            // Execute the query
                            using (MySqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    string position = reader["EmployeeType"].ToString();
                                    if (position == "Manager")
                                    {
                                        result = MessageBox.Show("Do you want to remove this item?", "Remove Item", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                                        if (result == DialogResult.Yes)
                                        {
                                            // Remove the selected row
                                            RecWalkinSelectedProdDGV.Rows.RemoveAt(e.RowIndex);
                                            MessageBox.Show("Item removed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Invalid password. You need manager permission to remove an item.", "Permission Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                        return;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Invalid password. You need manager permission to remove an item.", "Permission Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                    return;
                                }
                            }
                        }
                    }
                }
                else if (RecWalkinSelectedProdDGV.Columns[e.ColumnIndex].Name == "-")
                {
                    string quantityString = RecWalkinSelectedProdDGV.Rows[e.RowIndex].Cells["Qty"].Value?.ToString();
                    if (!string.IsNullOrEmpty(quantityString) && int.TryParse(quantityString, out int quantity))
                    {
                        decimal itemCost = decimal.Parse(RecWalkinSelectedProdDGV.Rows[e.RowIndex].Cells["Total Price"].Value?.ToString());

                        // Calculate the cost per item
                        decimal costPerItem = itemCost / quantity;

                        // Decrease quantity
                        if (quantity > 1)
                        {
                            quantity--;

                            // Calculate updated item cost (reset to original price)
                            decimal updatedCost = costPerItem * quantity;

                            // Update Qty and ItemCost in the DataGridView
                            RecWalkinSelectedProdDGV.Rows[e.RowIndex].Cells["Qty"].Value = quantity.ToString();
                            RecWalkinSelectedProdDGV.Rows[e.RowIndex].Cells["Total Price"].Value = updatedCost.ToString("F2"); // Format to two decimal places

                        }
                    }
                    else
                    {
                        // Handle the case where quantityString is empty or not a valid integer
                        // For example, show an error message or set a default value
                    }
                }
                else if (RecWalkinSelectedProdDGV.Columns[e.ColumnIndex].Name == "+")
                {
                    string quantityString = RecWalkinSelectedProdDGV.Rows[e.RowIndex].Cells["Qty"].Value?.ToString();
                    if (!string.IsNullOrEmpty(quantityString) && int.TryParse(quantityString, out int quantity))
                    {
                        decimal itemCost = decimal.Parse(RecWalkinSelectedProdDGV.Rows[e.RowIndex].Cells["Total Price"].Value?.ToString());

                        // Calculate the cost per item
                        decimal costPerItem = itemCost / quantity;

                        // Increase quantity
                        quantity++;

                        // Calculate updated item cost
                        decimal updatedCost = costPerItem * quantity;

                        // Update Qty and ItemCost in the DataGridView
                        RecWalkinSelectedProdDGV.Rows[e.RowIndex].Cells["Qty"].Value = quantity.ToString();
                        RecWalkinSelectedProdDGV.Rows[e.RowIndex].Cells["Total Price"].Value = updatedCost.ToString("F2"); // Format to two decimal places

                    }
                    else
                    {
                        // Handle the case where quantityString is empty or not a valid integer
                        // For example, show an error message or set a default value
                    }
                }
            }
        }

        private void MngrServicesHistoryBtn_Click(object sender, EventArgs e)
        {
            Inventory.PanelShow(MngrServiceHistoryPanel);

        }
        private void MngrServiceHistoryExitBtn_Click(object sender, EventArgs e)
        {
            Inventory.PanelShow(MngrInventoryTypePanel);

        }

        private void MngrWalkinProdSalesBtn_Click(object sender, EventArgs e)
        {
            Inventory.PanelShow(MngrWalkinProdSalesPanel);

        }

        private void MngrWalkinProdSalesExitBtn_Click(object sender, EventArgs e)
        {
            Inventory.PanelShow(MngrInventoryTypePanel);

        }
    }
}
