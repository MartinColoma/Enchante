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
using Org.BouncyCastle.Math;
using Mysqlx.Crud;

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
            RecShopProdSelectedProdView();

            //Landing Pages Cardlayout Panel Manager
            ParentPanelShow = new ParentCard(EnchanteHomePage, EnchanteStaffPage, EnchanteReceptionPage, EnchanteMemberPage, EnchanteAdminPage, EnchanteMngrPage);
            Registration = new Registration(MembershipPlanPanel, RegularPlanPanel, PremiumPlanPanel, SVIPPlanPanel);
            Service = new ServiceCard(ServiceType, ServiceHairStyling, ServiceFaceSkin, ServiceNailCare, ServiceSpa, ServiceMassage);
            Transaction = new ReceptionTransactionCard(RecTransactionPanel, RecWalkinPanel, RecApptPanel, RecPayServicePanel, RecQueWinPanel, RecShopProdPanel, RecApptConfirmPanel);
            Inventory = new MngrInventoryCard(MngrInventoryTypePanel, MngrServicesPanel, MngrServiceHistoryPanel, MngrInventoryMembershipPanel,
                                            MngrInventoryProductsPanel, MngrInventoryProductHistoryPanel, MngrSchedPanel, MngrWalkinSalesPanel, MngrIndemandPanel, MngrWalkinProdSalesPanel, MngrApptServicePanel);




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
            MngrServicesCategoryComboText.Items.AddRange(Service_Category);
            MngrServicesCategoryComboText.DropDownStyle = ComboBoxStyle.DropDownList;
            MngrServicesTypeComboText.Items.AddRange(Service_type);
            MngrServicesTypeComboText.DropDownStyle = ComboBoxStyle.DropDownList;

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
            RecApptBookingTimeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

            RecEditSchedBtn.Click += RecEditSchedBtn_Click;
            MngrStaffAvailabilityComboBox.SelectedIndex = 0;
            MngrStaffSchedComboBox.SelectedIndex = 0;

            MngrProductSalesPeriod.Items.Add("Day");
            MngrProductSalesPeriod.Items.Add("Week");
            MngrProductSalesPeriod.Items.Add("Month");
            MngrProductSalesPeriod.Items.Add("Specific Date Range");
            MngrProductSalesSelectCatBox.Items.Add("Hair Styling");
            MngrProductSalesSelectCatBox.Items.Add("Face & Skin");
            MngrProductSalesSelectCatBox.Items.Add("Nail Care");
            MngrProductSalesSelectCatBox.Items.Add("Massage");
            MngrProductSalesSelectCatBox.Items.Add("Spa");
            MngrProductSalesSelectCatBox.Items.Add("All Categories");

            MngrAppSalesPeriod.Items.Add("Day");
            MngrAppSalesPeriod.Items.Add("Week");
            MngrAppSalesPeriod.Items.Add("Month");
            MngrAppSalesPeriod.Items.Add("Specific Date Range");
            MngrAppSalesSelectCatBox.Items.Add("Hair Styling");
            MngrAppSalesSelectCatBox.Items.Add("Face & Skin");
            MngrAppSalesSelectCatBox.Items.Add("Nail Care");
            MngrAppSalesSelectCatBox.Items.Add("Massage");
            MngrAppSalesSelectCatBox.Items.Add("Spa");
            MngrAppSalesSelectCatBox.Items.Add("All Categories");

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

        //database-related methods
        #region
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


                        MngrInventoryServicesTable.DataSource = dataTable;

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
        #endregion

        // ID Generator Methods
        #region
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
        #endregion

        //password hashers
        #region
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
        #endregion

        //customized dgv on receptionist dashboard
        #region
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
            itemNameColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            itemNameColumn.ReadOnly = true;
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
            quantityColumn.ReadOnly = true;
            RecWalkinSelectedProdDGV.Columns.Add(quantityColumn);

            DataGridViewButtonColumn plusColumn = new DataGridViewButtonColumn();
            plusColumn.Name = "+";
            plusColumn.Text = "+";
            plusColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            plusColumn.Width = 10;
            RecWalkinSelectedProdDGV.Columns.Add(plusColumn);

            DataGridViewTextBoxColumn itemUnitCostColumn = new DataGridViewTextBoxColumn();
            itemUnitCostColumn.Name = "Unit Price";
            itemUnitCostColumn.ReadOnly = true;
            RecWalkinSelectedProdDGV.Columns.Add(itemUnitCostColumn);

            DataGridViewTextBoxColumn itemCostColumn = new DataGridViewTextBoxColumn();
            itemCostColumn.Name = "Total Price";
            itemCostColumn.ReadOnly = true;
            RecWalkinSelectedProdDGV.Columns.Add(itemCostColumn);


        }

        private void RecShopProdSelectedProdView()
        {
            DataGridViewButtonColumn trashColumn = new DataGridViewButtonColumn();
            trashColumn.Name = "Void";
            trashColumn.Text = "x";
            trashColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            trashColumn.Width = 10;
            RecShopProdSelectedProdDGV.Columns.Add(trashColumn);

            DataGridViewTextBoxColumn itemNameColumn = new DataGridViewTextBoxColumn();
            itemNameColumn.Name = "Item Name";
            itemNameColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            itemNameColumn.ReadOnly = true;
            RecShopProdSelectedProdDGV.Columns.Add(itemNameColumn);

            DataGridViewButtonColumn minusColumn = new DataGridViewButtonColumn();
            minusColumn.Name = "-";
            minusColumn.Text = "-";
            minusColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;

            minusColumn.Width = 10;
            RecShopProdSelectedProdDGV.Columns.Add(minusColumn);

            DataGridViewTextBoxColumn quantityColumn = new DataGridViewTextBoxColumn();
            quantityColumn.Name = "Qty";
            quantityColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            quantityColumn.Width = 15;
            quantityColumn.ReadOnly = true;
            RecShopProdSelectedProdDGV.Columns.Add(quantityColumn);

            DataGridViewButtonColumn plusColumn = new DataGridViewButtonColumn();
            plusColumn.Name = "+";
            plusColumn.Text = "+";
            plusColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            plusColumn.Width = 10;
            RecShopProdSelectedProdDGV.Columns.Add(plusColumn);

            DataGridViewTextBoxColumn itemUnitCostColumn = new DataGridViewTextBoxColumn();
            itemUnitCostColumn.Name = "Unit Price";
            itemUnitCostColumn.ReadOnly = true;
            RecShopProdSelectedProdDGV.Columns.Add(itemUnitCostColumn);

            DataGridViewTextBoxColumn itemCostColumn = new DataGridViewTextBoxColumn();
            itemCostColumn.Name = "Total Price";
            itemCostColumn.ReadOnly = true;
            RecShopProdSelectedProdDGV.Columns.Add(itemCostColumn);

            DataGridViewCheckBoxColumn checkboxColumn = new DataGridViewCheckBoxColumn();
            checkboxColumn.HeaderText = "Senior\nPWD\nDiscount";
            checkboxColumn.Name = "CheckBoxColumn";
            checkboxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            checkboxColumn.Width = 15;
            RecShopProdSelectedProdDGV.Columns.Add(checkboxColumn);

        }
        #endregion

        // Enchante Home Landing Page Starts Here
        #region
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
                RecNameLbl.Text = "Receptionist Tester";
                RecIDNumLbl.Text = "RT-0000-0000";
                InitializeProducts();
                logincredclear();
                InitializeAppointmentDataGrid();
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
                                        logincredclear();
                                        InitializeAppointmentDataGrid();

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
                                        StaffPriorityQueueCurrentCustomersStatusFlowLayoutPanel.Controls.Clear();
                                        InitializePreferredCuePendingCustomersForStaff();
                                        InitializeGeneralCuePendingCustomersForStaff();
                                        InitializePriorityPendingCustomersForStaff();
                                        RefreshFlowLayoutPanel();
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
            LoginPassText.UseSystemPasswordChar = true;

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
                StaffPersonalCueCurrentCustomersStatusFlowLayoutPanel.Controls.Clear();
                StaffPriorityQueueCurrentCustomersStatusFlowLayoutPanel.Controls.Clear();
                RecApptAcceptLateDeclineDGV.Rows.Clear();
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
            RegularPassText.UseSystemPasswordChar = true;
            RegularConfirmPassText.UseSystemPasswordChar = true;

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
            SVIPPassText.UseSystemPasswordChar = true;
            SVIPConfirmPassText.UseSystemPasswordChar = true;
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
                PremFirstNameErrorLbl.Visible = true;
                PremGenderErrorLbl.Visible = true;
                PremCPNumErrorLbl.Visible = true;
                PremEmailErrorLbl.Visible = true;
                PremPassErrorLbl.Visible = true;
                PremConfirmPassErrorLbl.Visible = true;
                PremLastNameErrorLbl.Visible = true;
                PremAgeErrorLbl.Visible = true;


                PremFirstNameErrorLbl.Text = "Missing Field";
                PremGenderErrorLbl.Text = "Missing Field";
                PremCPNumErrorLbl.Text = "Missing Field";
                PremEmailErrorLbl.Text = "Missing Field";
                PremPassErrorLbl.Text = "Missing Field";
                PremConfirmPassErrorLbl.Text = "Missing Field";
                PremLastNameErrorLbl.Text = "Missing Field";
                PremAgeErrorLbl.Text = "Missing Field";

            }
            else if (age < 18)
            {
                PremAgeErrorLbl.Visible = true;
                PremAgeErrorLbl.Text = "Must be 18 years old and above";
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
            PremPassText.UseSystemPasswordChar = true;
            PremConfirmPassText.UseSystemPasswordChar = true;
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
        #endregion

        //Customer Member Dashboard Starts Here
        #region
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
        #endregion


        #region Receptionist Dashboard Starts Here

        #region Receptionist Misc. Functions
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
            RecWalkinTransNumText.Text = TransactionNumberGenerator.WalkinGenerateTransNumberDefault();
        }

        //ApptMember
        private void RecAppointmentBtn_Click(object sender, EventArgs e)
        {
            Transaction.PanelShow(RecApptPanel);
            RecApptBookingTimeComboBox.Items.Clear();
            LoadBookingTimes();
            RecApptTransNumText.Text = TransactionNumberGenerator.AppointGenerateTransNumberDefault();
            RecApptBookingDatePicker.MinDate = DateTime.Today;
            RecApptClientBdayPicker.MaxDate = DateTime.Today;
            isappointment = true;
        }
        public class TransactionNumberGenerator
        {
            private static int transactNumber = 1; // Starting order number

            public static string WalkinGenerateTransNumberDefault()
            {
                string datePart = DateTime.Now.ToString("MMddhhmm");

                string orderPart = transactNumber.ToString("D3");

                string ordersessionNumber = $"W-{datePart}-{orderPart}";

                return ordersessionNumber;
            }
            public static string WalkinGenerateTransNumberInc()
            {
                string datePart = DateTime.Now.ToString("MMddhhmm");

                // Use only the order number
                string orderPart = transactNumber.ToString("D3");

                // Increment the order number for the next order
                transactNumber++;
                string ordersessionNumber = $"W-{datePart}-{orderPart}";

                return ordersessionNumber;
            }
            public static string AppointGenerateTransNumberDefault()
            {
                string datePart = DateTime.Now.ToString("MMddhhmm");

                string orderPart = transactNumber.ToString("D3");

                string ordersessionNumber = $"A-{datePart}-{orderPart}";

                return ordersessionNumber;
            }
            //ApptMember
            public static string AppointGenerateTransNumberInc()
            {
                string datePart = DateTime.Now.ToString("MMddhhmm");

                // Use only the order number
                string orderPart = transactNumber.ToString("D3");

                // Increment the order number for the next order
                transactNumber++;
                string ordersessionNumber = $"A-{datePart}-{orderPart}";

                return ordersessionNumber;
            }
            public static string ShopProdGenerateTransNumberDefault()
            {
                string datePart = DateTime.Now.ToString("MMddhhmm");

                string orderPart = transactNumber.ToString("D3");

                string ordersessionNumber = $"RP-{datePart}-{orderPart}";

                return ordersessionNumber;
            }
            public static string ShopProdGenerateTransNumberInc()
            {
                string datePart = DateTime.Now.ToString("MMddhhmm");

                // Use only the order number
                string orderPart = transactNumber.ToString("D3");

                // Increment the order number for the next order
                transactNumber++;
                string ordersessionNumber = $"RP-{datePart}-{orderPart}";

                return ordersessionNumber;
            }
        }

        private void RecWalkinTransactNumRefresh()
        {
            RecWalkinTransNumText.Text = TransactionNumberGenerator.WalkinGenerateTransNumberInc();

        }
        private void RecHomeBtn_Click(object sender, EventArgs e)
        {
            // Scroll to the Home position (0, 0)
            ScrollToCoordinates(0, 0);
            ReceptionHomePanelReset();
            RecApptAcceptLateDeclineDGV.Rows.Clear();

            //Change color once clicked
            RecHomeBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(177)))), ((int)(((byte)(183)))), ((int)(((byte)(97)))));

        }

        private void RecHomeBtn_MouseHover(object sender, EventArgs e)
        {
            iconToolTip.SetToolTip(RecHomeBtn, "Home");
        }


        private void ReceptionAccBtn_MouseHover(object sender, EventArgs e)
        {
            iconToolTip.SetToolTip(RecAccBtn, "Profile");
        }
        #endregion

        #region Receptionist Walk-in Transaction
        private void RecWalkInExitBtn_Click(object sender, EventArgs e)
        {
            Transaction.PanelShow(RecTransactionPanel);
            RecWalkinTransactionClear();

        }
        private void RecWalkInCatHSBtn_Click(object sender, EventArgs e)
        {
            filterstaffbyservicecategory = "Hair Styling";
            haschosenacategory = true;
            if (RecWalkinPreferredStaffToggleSwitch.Checked == true)
            {
                RecWalkinAttendingStaffSelectedComboBox.Items.Clear();
                LoadPreferredStaffComboBox();
            }
            RecWalkinHairStyle();

        }

        private void RecWalkInCatFSBtn_Click(object sender, EventArgs e)
        {
            filterstaffbyservicecategory = "Face & Skin";
            haschosenacategory = true;
            if (RecWalkinPreferredStaffToggleSwitch.Checked == true)
            {
                RecWalkinAttendingStaffSelectedComboBox.Items.Clear();
                LoadPreferredStaffComboBox();

            }
            RecWalkinFace();

        }

        private void RecWalkInCatNCBtn_Click(object sender, EventArgs e)
        {
            filterstaffbyservicecategory = "Nail Care";
            haschosenacategory = true;
            if (RecWalkinPreferredStaffToggleSwitch.Checked == true)
            {
                RecWalkinAttendingStaffSelectedComboBox.Items.Clear();
                LoadPreferredStaffComboBox();
            }
            RecWalkinNail();

        }

        private void RecWalkInCatSpaBtn_Click(object sender, EventArgs e)
        {
            filterstaffbyservicecategory = "Spa";
            haschosenacategory = true;
            if (RecWalkinPreferredStaffToggleSwitch.Checked == true)
            {
                RecWalkinAttendingStaffSelectedComboBox.Items.Clear();
                LoadPreferredStaffComboBox();
            }
            RecWalkinSpa();

        }

        private void RecWalkInCatMassageBtn_Click(object sender, EventArgs e)
        {
            filterstaffbyservicecategory = "Massage";
            haschosenacategory = true;
            if (RecWalkinPreferredStaffToggleSwitch.Checked == true)
            {
                RecWalkinAttendingStaffSelectedComboBox.Items.Clear();
                LoadPreferredStaffComboBox();
            }
            RecWalkinMassage();

        }


        private void RecWalkinHairStyle()
        {
            if (RecWalkinCatHSRB.Checked == false)
            {
                RecWalkinCatHSRB.Visible = true;
                RecWalkinCatHSRB.Checked = true;
                RecWalkinLoadServiceTypeComboBox("Hair Styling");

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
                RecWalkinLoadServiceTypeComboBox("Hair Styling");

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
        private void RecWalkinFace()
        {
            if (RecWalkinCatFSRB.Checked == false)
            {
                RecWalkinCatFSRB.Visible = true;
                RecWalkinCatFSRB.Checked = true;
                RecWalkinLoadServiceTypeComboBox("Face & Skin");

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
        private void RecWalkinNail()
        {
            if (RecWalkinCatNCRB.Checked == false)
            {
                RecWalkinCatNCRB.Visible = true;
                RecWalkinCatNCRB.Checked = true;
                RecWalkinLoadServiceTypeComboBox("Nail Care");

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
        private void RecWalkinSpa()
        {
            if (RecWalkinCatSpaRB.Checked == false)
            {
                RecWalkinCatSpaRB.Visible = true;
                RecWalkinCatSpaRB.Checked = true;
                RecWalkinLoadServiceTypeComboBox("Spa");

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
        private void RecWalkinMassage()
        {
            if (RecWalkinCatMassageRB.Checked == false)
            {
                RecWalkinCatMassageRB.Visible = true;
                RecWalkinCatMassageRB.Checked = true;
                RecWalkinLoadServiceTypeComboBox("Massage");

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
        public void RecWalkinLoadHairStyleType()
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

                        RecWalkInServiceTypeDGV.Columns.Clear();


                        RecWalkInServiceTypeDGV.DataSource = dataTable;

                        RecWalkInServiceTypeDGV.Columns[0].Visible = false; //service category
                        RecWalkInServiceTypeDGV.Columns[1].Visible = false; // service type
                        RecWalkInServiceTypeDGV.Columns[2].Visible = false; // service ID
                        RecWalkInServiceTypeDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        RecWalkInServiceTypeDGV.ClearSelection();
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
        public void RecWalkinFaceSkinType()
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

                        RecWalkInServiceTypeDGV.Columns.Clear();


                        RecWalkInServiceTypeDGV.DataSource = dataTable;

                        RecWalkInServiceTypeDGV.Columns[0].Visible = false;
                        RecWalkInServiceTypeDGV.Columns[1].Visible = false;
                        RecWalkInServiceTypeDGV.Columns[2].Visible = false;
                        RecWalkInServiceTypeDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        RecWalkInServiceTypeDGV.ClearSelection();
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
        public void RecWalkinNailCareType()
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

                        RecWalkInServiceTypeDGV.Columns.Clear();


                        RecWalkInServiceTypeDGV.DataSource = dataTable;

                        RecWalkInServiceTypeDGV.Columns[0].Visible = false;
                        RecWalkInServiceTypeDGV.Columns[1].Visible = false;
                        RecWalkInServiceTypeDGV.Columns[2].Visible = false;
                        RecWalkInServiceTypeDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        RecWalkInServiceTypeDGV.ClearSelection();
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
        public void RecWalkinSpaType()
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

                        RecWalkInServiceTypeDGV.Columns.Clear();


                        RecWalkInServiceTypeDGV.DataSource = dataTable;

                        RecWalkInServiceTypeDGV.Columns[0].Visible = false;
                        RecWalkInServiceTypeDGV.Columns[1].Visible = false;
                        RecWalkInServiceTypeDGV.Columns[2].Visible = false;
                        RecWalkInServiceTypeDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        RecWalkInServiceTypeDGV.ClearSelection();
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
        public void RecWalkinMassageType()
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

                        RecWalkInServiceTypeDGV.Columns.Clear();


                        RecWalkInServiceTypeDGV.DataSource = dataTable;

                        RecWalkInServiceTypeDGV.Columns[0].Visible = false;
                        RecWalkInServiceTypeDGV.Columns[1].Visible = false;
                        RecWalkInServiceTypeDGV.Columns[2].Visible = false;
                        RecWalkInServiceTypeDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        RecWalkInServiceTypeDGV.ClearSelection();
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
        private void RecWalkinLoadServiceTypeComboBox(string selectedCategory)
        {
            // Filter and add the relevant service types based on the selected category
            switch (selectedCategory)
            {
                case "Hair Styling":
                    RecWalkinLoadHairStyleType();
                    break;
                case "Nail Care":
                    RecWalkinNailCareType();
                    break;
                case "Face & Skin":
                    RecWalkinFaceSkinType();
                    break;
                case "Massage":
                    RecWalkinMassageType();
                    break;
                case "Spa":
                    RecWalkinSpaType();
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

                        RecWalkInServiceTypeDGV.Columns.Clear();

                        RecWalkInServiceTypeDGV.DataSource = dataTable;

                        // Adjust column visibility and sizing as needed
                        RecWalkInServiceTypeDGV.Columns[0].Visible = false;
                        RecWalkInServiceTypeDGV.Columns[1].Visible = false;
                        RecWalkInServiceTypeDGV.Columns[2].Visible = false;
                        RecWalkInServiceTypeDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        RecWalkInServiceTypeDGV.ClearSelection();
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

        private void RecSelectServiceAndStaffBtn_Click(object sender, EventArgs e)
        {
            RecWalkinAddService();
        }
        private void RecWalkinAddService()
        {


            if (RecWalkInServiceTypeDGV.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a service.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (string.IsNullOrEmpty(selectedStaffID))
            {
                MessageBox.Show("Please select a prefered staff or toggle anyone ", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DataGridViewRow selectedRow = RecWalkInServiceTypeDGV.SelectedRows[0];

            string SelectedCategory = selectedRow.Cells[0].Value.ToString();
            string ServiceID = selectedRow.Cells[2].Value.ToString();
            string ServiceName = selectedRow.Cells[3].Value.ToString();
            string ServicePrice = selectedRow.Cells[6].Value.ToString();

            string serviceID = selectedRow.Cells[2]?.Value?.ToString(); // Use null-conditional operator to avoid NullReferenceException

            // ... (existing code)

            if (RecWalkInServiceTypeDGV.SelectedRows.Count == 0)
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

            foreach (DataGridViewRow row in RecWalkinSelectedServiceDGV.Rows)
            {
                string existingServiceID = row.Cells["ServiceID"]?.Value?.ToString(); // Use null-conditional operator

                if (serviceID == existingServiceID)
                {
                    MessageBox.Show("This service is already selected.", "Duplicate Service", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }



            DialogResult result = MessageBox.Show("Are you sure you want to add this service?", "Confirm Service Selection", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Add the row
                DataGridViewRow NewSelectedServiceRow = RecWalkinSelectedServiceDGV.Rows[RecWalkinSelectedServiceDGV.Rows.Add()];

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


                RecWalkInServiceTypeDGV.ClearSelection();

            }
        }


        private int GetLargestQueNum(string appointmentDate, string serviceCategory)
        {
            using (MySqlConnection connection = new MySqlConnection(mysqlconn))
            {
                connection.Open();

                using (MySqlCommand command = connection.CreateCommand())
                {
                    string query = "SELECT MAX(CAST(QueNumber AS UNSIGNED)) FROM servicehistory WHERE AppointmentDate = @AppointmentDate AND ServiceCategory = @ServiceCategory";
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

        //ApptMember
        public bool isappointment;
        public void QueTypeIdentifier(DataGridViewCell QueType)
        {


            if (isappointment == true && RecApptAnyStaffToggleSwitch.Checked)
            {
                QueType.Value = "AnyonePriority";
            }
            else if (isappointment == true && RecApptPreferredStaffToggleSwitch.Checked)
            {
                QueType.Value = "PreferredPriority";
            }
            else if (selectedStaffID == "Anyone")
            {
                QueType.Value = "GeneralQue";
            }
            else
            {
                QueType.Value = "Preferred";
            }
        }
        //private string CustomerTimePicked()
        //{
        //    string TimePicked = string.Empty;

        //    if (RecAppPrefferedTimeAMComboBox.SelectedIndex == 0)
        //    {
        //        TimePicked = RecAppPrefferedTimePMComboBox.SelectedItem.ToString();
        //    }
        //    else if (RecAppPrefferedTimePMComboBox.SelectedIndex == 0)
        //    {
        //        TimePicked = RecAppPrefferedTimeAMComboBox.SelectedItem.ToString();
        //    }

        //    return TimePicked;
        //}

        //private string TimeSchedPicked()
        //{
        //    string TimeSched = string.Empty;

        //    if (RecAppPrefferedTimeAMComboBox.SelectedIndex == 0)
        //    {
        //        TimeSched = "PM";
        //    }
        //    else if (RecAppPrefferedTimePMComboBox.SelectedIndex == 0)
        //    {
        //        TimeSched = "AM";
        //    }

        //    return TimeSched;
        //}
        //private void RecWalkinSelectedDateText_TextChanged(object sender, EventArgs e)
        //{
        //    DisbaleTimeSchedIfNoDateIsSelected();
        //}

        private void RecDeleteSelectedServiceAndStaffBtn_Click(object sender, EventArgs e)
        {
            if (RecWalkinSelectedServiceDGV.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this row?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    DataGridViewRow selectedRow = RecWalkinSelectedServiceDGV.SelectedRows[0];
                    RecWalkinSelectedServiceDGV.Rows.Remove(selectedRow);
                }
            }
        }

        private void RecWalkInServiceTypeTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            RecWalkinAddService();
        }

        private bool IsCardNameValid(string name)
        {
            foreach (char c in name)
            {
                if (!char.IsLetter(c) && c != ' ')
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
            else if (RecWalkinSelectedServiceDGV != null && RecWalkinSelectedServiceDGV.Rows.Count == 0)
            {
                MessageBox.Show("Select a service first to proceed on booking a transaction.", "Ooooops!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                RecWalkinServiceHistoryDB(RecWalkinSelectedServiceDGV); //service history db
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
            RecWalkinSelectedServiceDGV.Rows.Clear();
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
                    foreach (DataGridViewRow row in RecPayServiceAcquiredDGV.Rows)
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
                    decimal netAmount = decimal.Parse(RecPayServiceNetAmountBox.Text);
                    decimal discount = decimal.Parse(RecPayServiceDiscountBox.Text);
                    decimal vat = decimal.Parse(RecPayServiceVATBox.Text);
                    decimal grossAmount = decimal.Parse(RecPayServiceGrossAmountBox.Text);
                    decimal cash = decimal.Parse(RecPayServiceCashBox.Text);
                    decimal change = decimal.Parse(RecPayServiceChangeBox.Text);

                    // Create a new table for the "Total" section
                    PdfPTable totalTable = new PdfPTable(2); // 2 columns for the "Total" table
                    totalTable.SetWidths(new float[] { 5f, 3f }); // Column widths
                    totalTable.DefaultCell.Border = PdfPCell.NO_BORDER;

                    // Add cells to the "Total" table
                    totalTable.AddCell(new Phrase($"Total # of Service ({RecPayServiceAcquiredDGV.Rows.Count})", font));
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
                    MessageBox.Show(errorMessage, "Reception Walkin  Product History", MessageBoxButtons.OK, MessageBoxIcon.Error);
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


            try
            {
                using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                {
                    connection.Open();
                    string insertQuery = "INSERT INTO walk_in_appointment (TransactionNumber, ServiceStatus, AppointmentDate, AppointmentTime, " +
                                        "ClientName, ClientCPNum, ServiceDuration, BookedBy, BookedDate, BookedTime)" +
                                        "VALUES (@Transact, @status, @appointDate, @appointTime, @clientName, @clientCP, @duration, @bookedBy, @bookedDate, @bookedTime)";

                    MySqlCommand cmd = new MySqlCommand(insertQuery, connection);
                    cmd.Parameters.AddWithValue("@Transact", transactionNum);
                    cmd.Parameters.AddWithValue("@status", serviceStatus);
                    cmd.Parameters.AddWithValue("@appointDate", bookedDate);
                    cmd.Parameters.AddWithValue("@appointTime", bookedTime);
                    cmd.Parameters.AddWithValue("@clientName", CustomerName);
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
            string transactionType = "Walk-in Transaction";

            string serviceStatus = "Pending";

            //booked values
            string bookedDate = currentDate.ToString("MM-dd-yyyy dddd"); //bookedDate
            string bookedTime = currentDate.ToString("hh:mm tt"); //bookedTime

            //basic info
            string CustomerName = RecWalkinFNameText.Text + " " + RecWalkinLNameText.Text; //client name

            if (RecWalkinSelectedServiceDGV.Rows.Count > 0)
            {
                try
                {
                    using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                    {
                        connection.Open();

                        foreach (DataGridViewRow row in RecWalkinSelectedServiceDGV.Rows)
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

                                string insertQuery = "INSERT INTO servicehistory (TransactionNumber, TransactionType, ServiceStatus, AppointmentDate, AppointmentTime, ClientName, " +
                                                     "ServiceCategory, ServiceID, SelectedService, ServicePrice, PreferredStaff, QueNumber," +
                                                     "QueType) " +
                                                     "VALUES (@Transact, @type, @status, @appointDate, @appointTime, @name, @serviceCat, @ID, @serviceName, @servicePrice, " +
                                                     "@preferredstaff, @quenumber, @quetype)";

                                MySqlCommand cmd = new MySqlCommand(insertQuery, connection);
                                cmd.Parameters.AddWithValue("@Transact", transactionNum);
                                cmd.Parameters.AddWithValue("@type", transactionType);
                                cmd.Parameters.AddWithValue("@status", serviceStatus);
                                cmd.Parameters.AddWithValue("@appointDate", bookedDate);
                                cmd.Parameters.AddWithValue("@appointTime", bookedTime);
                                cmd.Parameters.AddWithValue("@name", CustomerName);
                                cmd.Parameters.AddWithValue("@serviceCat", serviceCat);
                                cmd.Parameters.AddWithValue("@ID", serviceID);
                                cmd.Parameters.AddWithValue("@serviceName", serviceName);
                                cmd.Parameters.AddWithValue("@servicePrice", servicePrice);
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

        // Receptionist Buy Products
        #region

        private void RecWalkinProductUserControl_Click(object sender, EventArgs e)
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
                    RecWalkinSelectedProdDGV.Rows.Add(itemID, "x", itemName, "-", "1", "+", itemPrice, itemPrice, false);

                }
            }
        }
        private void RecShopProdProductUserControl_Click(object sender, EventArgs e)
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
                foreach (DataGridViewRow row in RecShopProdSelectedProdDGV.Rows)
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
                    string quantityString = RecShopProdSelectedProdDGV.Rows[existingRowIndex].Cells["Qty"].Value?.ToString();
                    if (!string.IsNullOrEmpty(quantityString) && int.TryParse(quantityString, out int quantity))
                    {
                        decimal itemCost = decimal.Parse(RecShopProdSelectedProdDGV.Rows[existingRowIndex].Cells["Total Price"].Value?.ToString());

                        // Calculate the cost per item
                        decimal costPerItem = itemCost / quantity;

                        // Increase quantity
                        quantity++;

                        // Calculate updated item cost
                        decimal updatedCost = costPerItem * quantity;

                        // Update Qty and ItemCost in the DataGridView
                        RecShopProdSelectedProdDGV.Rows[existingRowIndex].Cells["Qty"].Value = quantity.ToString();
                        RecShopProdSelectedProdDGV.Rows[existingRowIndex].Cells["Total Price"].Value = updatedCost.ToString("F2"); // Format to two decimal places
                        RecShopProdCalculateTotalPrice();
                    }
                    else
                    {
                        // Handle the case where quantityString is empty or not a valid integer
                        // For example, show an error message or set a default value
                    }
                }
                else
                {
                    RecShopProdSelectedProdDGV.Rows.Add(itemID, "0.00", "x", itemName, "-", "1", "+", itemPrice, itemPrice, false);
                    RecShopProdCalculateTotalPrice();

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
                Size userControlSize = new Size(295, 275);

                while (reader.Read())
                {
                    string itemID = reader["ItemID"].ToString();
                    string itemName = reader["ItemName"].ToString();
                    string itemStock = reader["ItemStock"].ToString();
                    string itemPrice = reader["ItemPrice"].ToString();
                    string itemStatus = reader["ItemStatus"].ToString();
                    byte[] productPicture = (byte[])reader["ProductPicture"];

                    ProductUserControl recwalkinproductusercontrol = new ProductUserControl();
                    ProductUserControl recshopproductusercontrol = new ProductUserControl();

                    //recwalkin product
                    recwalkinproductusercontrol.ProductItemIDTextBox.Text = itemID;
                    recwalkinproductusercontrol.ProductNameTextBox.Text = itemName;
                    recwalkinproductusercontrol.ProductStockTextBox.Text = itemStock;
                    recwalkinproductusercontrol.ProductPriceTextBox.Text = itemPrice;
                    recwalkinproductusercontrol.ProductStatusTextBox.Text = itemStatus;
                    //recshop product
                    recshopproductusercontrol.Size = userControlSize;
                    recshopproductusercontrol.ProductNameTextBox.Size = new Size(235, 33);
                    recshopproductusercontrol.ProductPriceTextBox.Size = new Size(90, 27);
                    recshopproductusercontrol.ProductPicturePictureBox.Size = new Size(162, 162);
                    recshopproductusercontrol.ProductNameTextBox.Location = new Point(12, 190);
                    recshopproductusercontrol.ProductPriceTextBox.Location = new Point(67, 230);
                    recshopproductusercontrol.PhpSignLbl.Location = new Point(18, 230);
                    recshopproductusercontrol.ProductPicturePictureBox.Location = new Point(72, 12);
                    recshopproductusercontrol.ProductItemIDTextBox.Text = itemID;
                    recshopproductusercontrol.ProductNameTextBox.Text = itemName;
                    recshopproductusercontrol.ProductStockTextBox.Text = itemStock;
                    recshopproductusercontrol.ProductPriceTextBox.Text = itemPrice;
                    recshopproductusercontrol.ProductStatusTextBox.Text = itemStatus;

                    if (itemStatus == "Low Stock")
                    {
                        recwalkinproductusercontrol.ProductOutOfStockPictureBox.Visible = true;
                        recwalkinproductusercontrol.Enabled = false;
                        recshopproductusercontrol.ProductOutOfStockPictureBox.Visible = true;
                        recshopproductusercontrol.Enabled = false;
                    }
                    else
                    {
                        recwalkinproductusercontrol.ProductOutOfStockPictureBox.Visible = false;
                        recwalkinproductusercontrol.Enabled = true;
                        recshopproductusercontrol.ProductOutOfStockPictureBox.Visible = false;
                        recshopproductusercontrol.Enabled = true;
                    }

                    if (productPicture != null && productPicture.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream(productPicture))
                        {
                            System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
                            recwalkinproductusercontrol.ProductPicturePictureBox.Image = image;
                            System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms);
                            recshopproductusercontrol.ProductPicturePictureBox.Image = image1;
                        }
                    }
                    else
                    {
                        recwalkinproductusercontrol.ProductPicturePictureBox.Image = null;
                        recshopproductusercontrol.ProductPicturePictureBox.Image = null;

                    }

                    foreach (System.Windows.Forms.Control control in recwalkinproductusercontrol.Controls)
                    {
                        control.Click += RecWalkinProductControlElement_Click;
                    }
                    foreach (System.Windows.Forms.Control control1 in recshopproductusercontrol.Controls)
                    {
                        control1.Click += RecShopProductControlElement_Click;
                    }

                    recwalkinproductusercontrol.Click += RecWalkinProductUserControl_Click;
                    recshopproductusercontrol.Click += RecShopProdProductUserControl_Click;

                    RecWalkinProductFlowLayoutPanel.Controls.Add(recwalkinproductusercontrol);
                    RecShopProdProductFlowLayoutPanel.Controls.Add(recshopproductusercontrol);
                }
                reader.Close();
            }

        }

        private void RecWalkinProductControlElement_Click(object sender, EventArgs e)
        {
            RecWalkinProductUserControl_Click((ProductUserControl)((System.Windows.Forms.Control)sender).Parent, e);
        }
        private void RecShopProductControlElement_Click(object sender, EventArgs e)
        {
            RecShopProdProductUserControl_Click((ProductUserControl)((System.Windows.Forms.Control)sender).Parent, e);
        }

        private void ProductUserControl_ProductClicked(object sender, EventArgs e)
        {
            // MAY GAMIT TO DONT DELETE UwU
            // sigi 
        }
        private void RecWalkinSelectedProdDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && RecWalkinSelectedProdDGV.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
            {
                // Handle the Bin column
                if (RecWalkinSelectedProdDGV.Columns[e.ColumnIndex].Name == "Void")
                {
                    //input dialog messagebox
                    string enteredPassword = GetPasswordWithAsterisks("Enter Manager Password:", "Void Product Permission");

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
        private void ProductVoidButton_Click(object sender, EventArgs e)
        {
            if (RecWalkinSelectedProdDGV.Rows.Count == 0)
            {
                MessageBox.Show("The product list is empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //input dialog messagebox
            string enteredPassword = GetPasswordWithAsterisks("Enter Manager Password:", "Void Product Permission");

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

                System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox()
                {
                    Left = 20,
                    Top = 100,
                    Width = 420,
                    Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                    ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221))))),
                    BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(136)))), ((int)(((byte)(82))))),
                    PasswordChar = '*'
                };
                System.Windows.Forms.Button button = new System.Windows.Forms.Button()
                {
                    Text = "Void Items",
                    Left = 325,
                    Width = 120,
                    Height = 40,
                    Top = 150,
                    Font = new System.Drawing.Font("Arial Black", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                    ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221))))),
                    BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(89)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))))
                };
                System.Windows.Forms.Button button1 = new System.Windows.Forms.Button()
                {
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
                passwordForm.Text = title;

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
        #endregion

        #endregion
       
        #region Receptionist Payment Service

        private void ReceptionCalculateTotalPrice()
        {
            decimal total1 = 0;
            decimal total2 = 0;
            decimal total3 = 0;

            // Assuming the "ServicePrice" column is of decimal type
            int servicepriceColumnIndex = RecPayServiceAcquiredDGV.Columns["ServicePrice"].Index;

            foreach (DataGridViewRow row in RecPayServiceAcquiredDGV.Rows)
            {
                if (row.Cells[servicepriceColumnIndex].Value != null)
                {
                    decimal price = decimal.Parse(row.Cells[servicepriceColumnIndex].Value.ToString());
                    total1 += price;
                }
            }
            RecPayServiceAcquiredTotalText.Text = total1.ToString("F2");

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
            RecPayServiceCOProdTotalText.Text = total2.ToString("F2");


            total3 = total1 + total2;

            // Display the total price in the GrossAmountBox TextBox
            RecPayServiceGrossAmountBox.Text = total3.ToString("F2"); // Format to two decimal places

            ReceptionCalculateVATAndNetAmount();
        }


        public void ReceptionCalculateVATAndNetAmount()
        {
            // Get the Gross Amount from the TextBox (MngrGrossAmountBox)
            if (decimal.TryParse(RecPayServiceGrossAmountBox.Text, out decimal grossAmount))
            {
                // Fixed VAT rate of 12%
                decimal rate = 12;

                // Calculate the VAT Amount
                decimal netAmount = grossAmount / ((rate / 100) + 1);

                // Calculate the Net Amount
                decimal vatAmount = grossAmount - netAmount;

                // Display the calculated values in TextBoxes
                RecPayServiceVATBox.Text = vatAmount.ToString("0.00");
                RecPayServiceNetAmountBox.Text = netAmount.ToString("0.00");
                RecPayServiceVATBox.Text = vatAmount.ToString("0.00");
                RecPayServiceNetAmountBox.Text = netAmount.ToString("0.00");
            }

        }

        private void DateTimePickerTimer_Tick(object sender, EventArgs e)
        {
            RecDateTimePicker.Value = DateTime.Now;
            DateTime cashierrcurrentDate = RecDateTimePicker.Value;
            string Cashiertoday = cashierrcurrentDate.ToString("MM-dd-yyyy dddd hh:mm tt");
            RecDateTimeText.Text = Cashiertoday;
        }
        private decimal originalGrossAmount; // Store the original value

        private void RecWalkinDiscountSenior_CheckedChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(RecPayServiceGrossAmountBox.Text, out decimal grossAmount))
            {
                if (RecPayServiceDiscountSenior.Checked)
                {
                    // Apply the 20% discount if the checkbox is checked and the discount hasn't been applied before
                    originalGrossAmount = grossAmount; // Store the original value
                    decimal discountPercentage = 20m;
                    decimal discountAmount = grossAmount * (discountPercentage / 100); // Calculate the discount amount
                    decimal discountedAmount = grossAmount - discountAmount; // Subtract the discount amount
                    decimal vatAmount = 0;
                    RecPayServiceGrossAmountBox.Text = discountedAmount.ToString("0.00"); // Format to display as currency
                    RecPayServiceNetAmountBox.Text = discountedAmount.ToString("0.00"); // Format to display as currency
                    RecPayServiceDiscountBox.Text = discountAmount.ToString("0.00"); // Display the discount amount
                    RecPayServiceVATBox.Text = vatAmount.ToString("0.00");
                    RecPayServiceDiscountPWD.Checked = false;
                    RecPayServiceVATExemptChk.Checked = true;
                    RecPayServiceVATExemptChk.Enabled = false;
                    return;
                }
                else
                {
                    // Unchecked, set MngrGrossAmount to the original value if the discount has been applied before
                    RecPayServiceGrossAmountBox.Text = originalGrossAmount.ToString("0.00");
                    RecPayServiceDiscountBox.Text = "0.00"; // Reset the discount amount display
                    ReceptionCalculateVATAndNetAmount();
                    RecPayServiceVATExemptChk.Checked = false;
                    RecPayServiceVATExemptChk.Enabled = true;
                    return;
                }

            }
        }

        private void RecWalkinDiscountPWD_CheckedChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(RecPayServiceGrossAmountBox.Text, out decimal grossAmount))
            {
                if (RecPayServiceDiscountPWD.Checked)
                {
                    // Apply the 20% discount if the checkbox is checked and the discount hasn't been applied before
                    originalGrossAmount = grossAmount; // Store the original value
                    decimal discountPercentage = 20m;
                    decimal discountAmount = grossAmount * (discountPercentage / 100); // Calculate the discount amount
                    decimal discountedAmount = grossAmount - discountAmount; // Subtract the discount amount
                    decimal vatAmount = 0;
                    RecPayServiceGrossAmountBox.Text = discountedAmount.ToString("0.00"); // Format to display as currency
                    RecPayServiceNetAmountBox.Text = discountedAmount.ToString("0.00"); // Format to display as currency
                    RecPayServiceDiscountBox.Text = discountAmount.ToString("0.00"); // Display the discount amount
                    RecPayServiceVATBox.Text = vatAmount.ToString("0.00");
                    RecPayServiceDiscountSenior.Checked = false;
                    RecPayServiceVATExemptChk.Checked = true;
                    RecPayServiceVATExemptChk.Enabled = false;
                    return;
                }
                else
                {
                    // Unchecked, set MngrGrossAmount to the original value if the discount has been applied before
                    RecPayServiceGrossAmountBox.Text = originalGrossAmount.ToString("0.00");
                    RecPayServiceDiscountBox.Text = "0.00"; // Reset the discount amount display
                    ReceptionCalculateVATAndNetAmount();
                    RecPayServiceVATExemptChk.Checked = false;
                    RecPayServiceVATExemptChk.Enabled = true;
                    return;
                }

            }
        }

        private void RecWalkinCashBox_TextChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(RecPayServiceGrossAmountBox.Text, out decimal grossAmount))
            {
                // Get the Cash Amount from the TextBox (MngrCashBox)
                if (decimal.TryParse(RecPayServiceCashBox.Text, out decimal cashAmount))
                {
                    // Calculate the Change
                    decimal change = cashAmount - grossAmount;

                    // Display the calculated change value in the MngrChangeBox
                    RecPayServiceChangeBox.Text = change.ToString("0.00");
                }
                else
                {
                    // Handle invalid input in MngrCashBox, e.g., display an error message
                    RecPayServiceChangeBox.Text = "0.00";
                }
            }
            else
            {
                // Handle invalid input in MngrGrossAmountBox, e.g., display an error message
                RecPayServiceChangeBox.Text = "0.00";
            }
        }
        private void RecWalkinGrossAmountBox_TextChanged(object sender, EventArgs e)
        {
            //ReceptionCalculateVATAndNetAmount();
            if (decimal.TryParse(RecPayServiceGrossAmountBox.Text, out decimal grossAmount))
            {
                // Get the Cash Amount from the TextBox (MngrCashBox)
                if (decimal.TryParse(RecPayServiceCashBox.Text, out decimal cashAmount))
                {
                    // Calculate the Change
                    decimal change = cashAmount - grossAmount;

                    // Display the calculated change value in the MngrChangeBox
                    RecPayServiceChangeBox.Text = change.ToString("0.00");
                }
                else
                {
                    // Handle invalid input in MngrCashBox, e.g., display an error message
                    RecPayServiceChangeBox.Text = "0.00";
                }
            }
            else
            {
                // Handle invalid input in MngrGrossAmountBox, e.g., display an error message
                RecPayServiceChangeBox.Text = "0.00";
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
                RecPayServiceCashLbl.Visible = false;
                RecPayServiceCashBox.Visible = false;
                RecPayServiceCashBox.Text = "0";
                RecPayServiceChangeBox.Text = "0.00";
                RecPayServiceCardNameText.Text = "";
                RecPayServiceCardNumText.Text = "";
                RecPayServiceCVCText.Text = "";
                RecPayServiceCardExpText.Text = "MM/YY";
                RecPayServiceWalletNumText.Text = "";
                RecPayServiceWalletPINText.Text = "";
                RecPayServiceWalletOTPText.Text = "";
                RecPayServiceChangeLbl.Visible = false;
                RecPayServiceChangeBox.Visible = false;

                //disable radio buttons
                RecPayServicePPPaymentRB.Visible = false;
                RecPayServiceCashPaymentRB.Visible = false;
                RecPayServiceGCPaymentRB.Visible = false;
                RecPayServicePMPaymentRB.Visible = false;
                RecPayServicePPPaymentRB.Checked = false;
                RecPayServiceCashPaymentRB.Checked = false;
                RecPayServiceGCPaymentRB.Checked = false;
                RecPayServicePMPaymentRB.Checked = false;
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
                RecPayServiceCashLbl.Visible = false;
                RecPayServiceCashBox.Visible = false;
                RecPayServiceCashBox.Text = "0";
                RecPayServiceChangeBox.Text = "0.00";
                RecPayServiceCardNameText.Text = "";
                RecPayServiceCardNumText.Text = "";
                RecPayServiceCVCText.Text = "";
                RecPayServiceCardExpText.Text = "MM/YY";
                RecPayServiceWalletNumText.Text = "";
                RecPayServiceWalletPINText.Text = "";
                RecPayServiceWalletOTPText.Text = "";
                RecPayServiceChangeLbl.Visible = false;
                RecPayServiceChangeBox.Visible = false;

                //disable radio buttons
                RecPayServiceCCPaymentRB.Visible = false;
                RecPayServiceCashPaymentRB.Visible = false;
                RecPayServiceGCPaymentRB.Visible = false;
                RecPayServicePMPaymentRB.Visible = false;
                RecPayServiceCCPaymentRB.Checked = false;
                RecPayServiceCashPaymentRB.Checked = false;
                RecPayServiceGCPaymentRB.Checked = false;
                RecPayServicePMPaymentRB.Checked = false;
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
                RecPayServiceCashLbl.Visible = true;
                RecPayServiceCashBox.Visible = true;
                RecPayServiceChangeLbl.Visible = true;
                RecPayServiceChangeBox.Visible = true;

                //disable other payment panel
                RecPayServiceBankPaymentPanel.Visible = false;
                RecPayServiceWalletPaymentPanel.Visible = false;

                //disable radio buttons
                RecPayServiceCCPaymentRB.Visible = false;
                RecPayServicePPPaymentRB.Visible = false;
                RecPayServiceGCPaymentRB.Visible = false;
                RecPayServicePMPaymentRB.Visible = false;
                RecPayServiceCCPaymentRB.Checked = false;
                RecPayServicePPPaymentRB.Checked = false;
                RecPayServiceGCPaymentRB.Checked = false;
                RecPayServicePMPaymentRB.Checked = false;
                RecPayServiceCardNameText.Text = "";
                RecPayServiceCardNumText.Text = "";
                RecPayServiceCVCText.Text = "";
                RecPayServiceCardExpText.Text = "MM/YY";
                RecPayServiceWalletNumText.Text = "";
                RecPayServiceWalletPINText.Text = "";
                RecPayServiceWalletOTPText.Text = "";
                RecPayServiceCashBox.Text = "0";
                RecPayServiceChangeBox.Text = "0.00";
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
                RecPayServiceCashLbl.Visible = false;
                RecPayServiceCashBox.Visible = false;
                RecPayServiceCashBox.Text = "0";
                RecPayServiceChangeBox.Text = "0.00";
                RecPayServiceCardNameText.Text = "";
                RecPayServiceCardNumText.Text = "";
                RecPayServiceCVCText.Text = "";
                RecPayServiceCardExpText.Text = "MM/YY";
                RecPayServiceWalletNumText.Text = "";
                RecPayServiceWalletPINText.Text = "";
                RecPayServiceWalletOTPText.Text = "";
                RecPayServiceChangeLbl.Visible = false;
                RecPayServiceChangeBox.Visible = false;

                //disable radio buttons
                RecPayServiceCCPaymentRB.Visible = false;
                RecPayServicePPPaymentRB.Visible = false;
                RecPayServiceCashPaymentRB.Visible = false;
                RecPayServicePMPaymentRB.Visible = false;
                RecPayServiceCCPaymentRB.Checked = false;
                RecPayServicePPPaymentRB.Checked = false;
                RecPayServiceCashPaymentRB.Checked = false;
                RecPayServicePMPaymentRB.Checked = false;
            }
            else
            {
                RecPayServiceGCPaymentRB.Visible = true;
                RecPayServiceGCPaymentRB.Checked = true;
            }
        }

        private void RecWalkinPMPaymentBtn_Click(object sender, EventArgs e)
        {
            if (RecPayServicePMPaymentRB.Checked == false)
            {
                RecPayServicePMPaymentRB.Visible = true;
                RecPayServicePMPaymentRB.Checked = true;
                RecPayServiceTypeText.Text = "Paymaya";
                RecPayServiceWalletPaymentPanel.Visible = true;


                //disable other payment panel
                RecPayServiceBankPaymentPanel.Visible = false;
                RecPayServiceCashLbl.Visible = false;
                RecPayServiceCashBox.Visible = false;
                RecPayServiceCashBox.Text = "0";
                RecPayServiceChangeBox.Text = "0.00";
                RecPayServiceCardNameText.Text = "";
                RecPayServiceCardNumText.Text = "";
                RecPayServiceCVCText.Text = "";
                RecPayServiceCardExpText.Text = "MM/YY";
                RecPayServiceWalletNumText.Text = "";
                RecPayServiceWalletPINText.Text = "";
                RecPayServiceWalletOTPText.Text = "";
                RecPayServiceChangeLbl.Visible = false;
                RecPayServiceChangeBox.Visible = false;

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
                RecPayServicePMPaymentRB.Visible = true;
                RecPayServicePMPaymentRB.Checked = true;
            }
        }

        public void RecLoadServiceHistoryDB(string transactNumber)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                {
                    connection.Open();

                    // Modify the SQL query to filter based on TransactNumber and OrderNumber
                    string sql = "SELECT * FROM `servicehistory` WHERE TransactionNumber = @TransactionNumber AND ServiceStatus = @status";
                    MySqlCommand cmd = new MySqlCommand(sql, connection);

                    // Add parameters to the query
                    cmd.Parameters.AddWithValue("@TransactionNumber", transactNumber);
                    cmd.Parameters.AddWithValue("@status", "Completed");

                    System.Data.DataTable dataTable = new System.Data.DataTable();

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);

                        RecPayServiceAcquiredDGV.DataSource = dataTable;

                        RecPayServiceAcquiredDGV.Columns[0].Visible = false; //transact number
                        RecPayServiceAcquiredDGV.Columns[1].Visible = false; //transact type
                        RecPayServiceAcquiredDGV.Columns[2].Visible = false; //service status
                        RecPayServiceAcquiredDGV.Columns[3].Visible = false; //appointment date
                        RecPayServiceAcquiredDGV.Columns[4].Visible = false; //appointment time
                        RecPayServiceAcquiredDGV.Columns[5].Visible = false; //client name
                        RecPayServiceAcquiredDGV.Columns[6].Visible = false; //service category
                        RecPayServiceAcquiredDGV.Columns[7].Visible = false; // attending staff
                        RecPayServiceAcquiredDGV.Columns[8].Visible = false; //service ID
                        RecPayServiceAcquiredDGV.Columns[11].Visible = false; //service start
                        RecPayServiceAcquiredDGV.Columns[12].Visible = false; //service end 
                        RecPayServiceAcquiredDGV.Columns[13].Visible = false; //service duration
                        RecPayServiceAcquiredDGV.Columns[14].Visible = false; // preferred staff
                        RecPayServiceAcquiredDGV.Columns[15].Visible = false; // que number
                        RecPayServiceAcquiredDGV.Columns[16].Visible = false; // que type
                        RecPayServiceAcquiredDGV.Columns[17].Visible = false; // prio number
                        RecPayServiceAcquiredDGV.Columns[18].Visible = false; // prio number

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

        public void RecLoadOrderProdHistoryDB(string transactNumber)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                {
                    connection.Open();

                    // Modify the SQL query to filter based on TransactNumber and OrderNumber
                    string sql = "SELECT * FROM `orderproducthistory` WHERE TransactionNumber = @TransactionNumber AND ProductStatus = @status";
                    MySqlCommand cmd = new MySqlCommand(sql, connection);

                    // Add parameters to the query
                    cmd.Parameters.AddWithValue("@TransactionNumber", transactNumber);
                    cmd.Parameters.AddWithValue("@status", "Not Paid");

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

        private void RecPayServiceCompleteTransDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if a valid cell is clicked
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Get TransactNumber and OrderNumber from the clicked cell in MngrSalesTable
                string transactNumber = RecPayServiceWalkinCompleteTransDGV.Rows[e.RowIndex].Cells["TransactionNumber"].Value.ToString();
                string clientName = RecPayServiceWalkinCompleteTransDGV.Rows[e.RowIndex].Cells["ClientName"].Value.ToString();

                
                RecPayServiceTransactNumLbl.Text = transactNumber;
                RecPayServiceClientNameLbl.Text = $"Client Name: {clientName}";

                RecLoadServiceHistoryDB(transactNumber);
                RecLoadOrderProdHistoryDB(transactNumber);

                ReceptionCalculateTotalPrice();
                RecPayServiceTransTypeLbl.Text = "Walk-in";

            }
        }

        private void RecPayServiceApptCompleteTransDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string transactNumber1 = RecPayServiceApptCompleteTransDGV.Rows[e.RowIndex].Cells["TransactionNumber"].Value.ToString();
                string clientName1 = RecPayServiceApptCompleteTransDGV.Rows[e.RowIndex].Cells["ClientName"].Value.ToString();

                RecPayServiceTransactNumLbl.Text = transactNumber1;
                RecPayServiceClientNameLbl.Text = $"Client Name: {clientName1}";
                RecLoadServiceHistoryDB(transactNumber1);
                RecLoadOrderProdHistoryDB(transactNumber1);

                ReceptionCalculateTotalPrice();
                RecPayServiceTransTypeLbl.Text = "Appointment";

            }
        }
        public void RecLoadCompletedWalkinTrans()
        {
            string todayDate = DateTime.Today.ToString("MM-dd-yyyy dddd");

            MySqlConnection connection = null;
            try
            {
                using (connection = new MySqlConnection(mysqlconn))
                {
                    connection.Open();

                    // Filter and sort the data by FoodType
                    string sql = "SELECT * FROM `walk_in_appointment` WHERE ServiceStatus = 'Completed' AND AppointmentDate = @todayDate ORDER BY ServiceStatus ";
                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    System.Data.DataTable dataTable = new System.Data.DataTable();
                    cmd.Parameters.AddWithValue("@todayDate", todayDate);

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);

                        RecPayServiceWalkinCompleteTransDGV.Columns.Clear();

                        RecPayServiceWalkinCompleteTransDGV.DataSource = dataTable;

                        if (RecPayServiceWalkinCompleteTransDGV.Columns.Count > 2)
                        {
                            RecPayServiceWalkinCompleteTransDGV.Columns[2].Visible = false; //appointment date
                            RecPayServiceWalkinCompleteTransDGV.Columns[3].Visible = false; //appointment time
                            RecPayServiceWalkinCompleteTransDGV.Columns[5].Visible = false; // client cp num
                            RecPayServiceWalkinCompleteTransDGV.Columns[6].Visible = false; // net price
                            RecPayServiceWalkinCompleteTransDGV.Columns[7].Visible = false; // net price
                            RecPayServiceWalkinCompleteTransDGV.Columns[8].Visible = false; // net price
                            RecPayServiceWalkinCompleteTransDGV.Columns[9].Visible = false; // net price
                            RecPayServiceWalkinCompleteTransDGV.Columns[10].Visible = false; // discount amount
                            RecPayServiceWalkinCompleteTransDGV.Columns[11].Visible = false; // discount amount
                            RecPayServiceWalkinCompleteTransDGV.Columns[12].Visible = false; // cash given
                            RecPayServiceWalkinCompleteTransDGV.Columns[13].Visible = false; // due change
                            RecPayServiceWalkinCompleteTransDGV.Columns[14].Visible = false; // payment method
                            RecPayServiceWalkinCompleteTransDGV.Columns[15].Visible = false; // card name
                            RecPayServiceWalkinCompleteTransDGV.Columns[16].Visible = false; // card num
                            RecPayServiceWalkinCompleteTransDGV.Columns[17].Visible = false; // cvc
                            RecPayServiceWalkinCompleteTransDGV.Columns[18].Visible = false; // card expiration
                            RecPayServiceWalkinCompleteTransDGV.Columns[19].Visible = false; // wallet num
                            RecPayServiceWalkinCompleteTransDGV.Columns[20].Visible = false; // wallet PIN
                            RecPayServiceWalkinCompleteTransDGV.Columns[21].Visible = false; // wallet OTP
                            RecPayServiceWalkinCompleteTransDGV.Columns[22].Visible = false; // service duration
                            RecPayServiceWalkinCompleteTransDGV.Columns[23].Visible = false; // booked by
                            RecPayServiceWalkinCompleteTransDGV.Columns[24].Visible = false; // booked date
                        }

                        RecPayServiceWalkinCompleteTransDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        RecPayServiceWalkinCompleteTransDGV.ClearSelection();
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMessage = "An error occurred: " + ex.Message + "\n\n" + ex.StackTrace;

                try
                {
                    // Try to copy the error message to the clipboard
                    Clipboard.SetText(errorMessage);

                    // Show a MessageBox indicating that the error message has been copied to the clipboard
                    MessageBox.Show("An error occurred. The error message has been copied to the clipboard.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception copyEx)
                {
                    // If copying to clipboard fails, display a MessageBox with the error message without copying to clipboard
                    string copyErrorMessage = "An error occurred while copying the error message to the clipboard:\n" + copyEx.Message;
                    MessageBox.Show(errorMessage + "\n\n" + copyErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }
        public void RecLoadCompletedAppointmentTrans()
        {
            string todayDate = DateTime.Today.ToString("MM-dd-yyyy dddd");

            MySqlConnection connection = null;
            try
            {
                using (connection = new MySqlConnection(mysqlconn))
                {
                    connection.Open();

                    // Filter and sort the data by FoodType
                    string sql = "SELECT * FROM `appointment` WHERE ServiceStatus = 'Completed' AND AppointmentDate = @todayDate ORDER BY ServiceStatus ";
                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    System.Data.DataTable dataTable = new System.Data.DataTable();
                    cmd.Parameters.AddWithValue("@todayDate", todayDate);

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);

                        RecPayServiceApptCompleteTransDGV.Columns.Clear();

                        RecPayServiceApptCompleteTransDGV.DataSource = dataTable;

                        if (RecPayServiceWalkinCompleteTransDGV.Columns.Count > 2)
                        {
                            RecPayServiceApptCompleteTransDGV.Columns[3].Visible = false; //appointment time
                            RecPayServiceApptCompleteTransDGV.Columns[4].Visible = false; //appointment time
                            RecPayServiceApptCompleteTransDGV.Columns[5].Visible = false; // client cp num
                            RecPayServiceApptCompleteTransDGV.Columns[7].Visible = false; // net price
                            RecPayServiceApptCompleteTransDGV.Columns[8].Visible = false; // net price
                            RecPayServiceApptCompleteTransDGV.Columns[9].Visible = false; // net price
                            RecPayServiceApptCompleteTransDGV.Columns[10].Visible = false; // discount amount
                            RecPayServiceApptCompleteTransDGV.Columns[11].Visible = false; // discount amount
                            RecPayServiceApptCompleteTransDGV.Columns[12].Visible = false; // cash given
                            RecPayServiceApptCompleteTransDGV.Columns[13].Visible = false; // due change
                            RecPayServiceApptCompleteTransDGV.Columns[14].Visible = false; // payment method
                            RecPayServiceApptCompleteTransDGV.Columns[15].Visible = false; // card name
                            RecPayServiceApptCompleteTransDGV.Columns[16].Visible = false; // card num
                            RecPayServiceApptCompleteTransDGV.Columns[17].Visible = false; // cvc
                            RecPayServiceApptCompleteTransDGV.Columns[18].Visible = false; // card expiration
                            RecPayServiceApptCompleteTransDGV.Columns[19].Visible = false; // wallet num
                            RecPayServiceApptCompleteTransDGV.Columns[20].Visible = false; // wallet PIN
                            RecPayServiceApptCompleteTransDGV.Columns[21].Visible = false; // wallet OTP
                            RecPayServiceApptCompleteTransDGV.Columns[22].Visible = false; // service duration
                            RecPayServiceApptCompleteTransDGV.Columns[23].Visible = false; // booked by
                            RecPayServiceApptCompleteTransDGV.Columns[24].Visible = false; // booked date
                            RecPayServiceApptCompleteTransDGV.Columns[25].Visible = false; // booked date
                            RecPayServiceApptCompleteTransDGV.Columns[26].Visible = false; // booked date

                        }

                        RecPayServiceApptCompleteTransDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        RecPayServiceApptCompleteTransDGV.ClearSelection();
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMessage = "An error occurred: " + ex.Message + "\n\n" + ex.StackTrace;

                try
                {
                    // Try to copy the error message to the clipboard
                    Clipboard.SetText(errorMessage);

                    // Show a MessageBox indicating that the error message has been copied to the clipboard
                    MessageBox.Show("An error occurred. The error message has been copied to the clipboard.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception copyEx)
                {
                    // If copying to clipboard fails, display a MessageBox with the error message without copying to clipboard
                    string copyErrorMessage = "An error occurred while copying the error message to the clipboard:\n" + copyEx.Message;
                    MessageBox.Show(errorMessage + "\n\n" + copyErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            finally
            {
                if (connection != null)
                {
                    connection.Close();
                }
            }
        }

        private bool RecPayServiceUpdateWalkin_And_ApptDB()
        {
            // cash values
            string netAmount = RecPayServiceNetAmountBox.Text; // net amount
            string vat = RecPayServiceVATBox.Text; // vat 
            string discount = RecPayServiceDiscountBox.Text; // discount
            string grossAmount = RecPayServiceGrossAmountBox.Text; // gross amount
            string cash = RecPayServiceCashBox.Text; // cash given
            string change = RecPayServiceChangeBox.Text; // due change
            string paymentMethod = RecPayServiceTypeText.Text; // payment method
            string mngr = RecNameLbl.Text;
            string transactNum = RecPayServiceTransactNumLbl.Text;

            // bank & wallet details
            string cardName = RecPayServiceCardNameText.Text;
            string cardNum = RecPayServiceCardNumText.Text;
            string CVC = RecPayServiceCVCText.Text;
            string expire = RecPayServiceCardExpText.Text;
            string walletNum = RecPayServiceWalletNumText.Text;
            string walletPIN = RecPayServiceWalletPINText.Text;
            string walletOTP = RecPayServiceWalletOTPText.Text;

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
                        else if (string.IsNullOrWhiteSpace(RecPayServiceCardNameText.Text))
                        {
                            MessageBox.Show("Please enter a cardholder name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        else if (!IsCardNameValid(RecPayServiceCardNameText.Text))
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

                    else if (RecPayServiceGCPaymentRB.Checked || RecPayServicePMPaymentRB.Checked)
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

                    //walk-in transactions
                    string cashPaymentWalkin = "UPDATE walk_in_appointment SET ServiceStatus = @status, NetPrice = @net, VatAmount = @vat, DiscountAmount = @discount, " +
                                        "GrossAmount = @gross, CashGiven = @cash, DueChange = @change, PaymentMethod = @payment, CheckedOutBy = @mngr " +
                                        "WHERE TransactionNumber = @transactNum"; // cash query
                    string bankPaymentWalkin = "UPDATE walk_in_appointment SET ServiceStatus = @status, NetPrice = @net, VatAmount = @vat, DiscountAmount = @discount, " +
                                        "GrossAmount = @gross, PaymentMethod = @payment, CardName = @cardname, CardNumber = @cardNum, " +
                                        "CVC = @cvc, CardExpiration = @expiration, CheckedOutBy = @mngr " +
                                        "WHERE TransactionNumber = @transactNum"; // credit card and paypal query
                    string walletPaymentWalkin = "UPDATE walk_in_appointment SET ServiceStatus = @status, NetPrice = @net, VatAmount = @vat, DiscountAmount = @discount, " +
                                        "GrossAmount = @gross, PaymentMethod = @payment, WalletNumber = @walletNum, WalletPIN = @walletPin, WalletOTP = @walletOTP, CheckedOutBy = @mngr " +
                                        "WHERE TransactionNumber = @transactNum"; // gcash and paymaya query
                    string productPaymentWalkin = "UPDATE orderproducthistory SET ProductStatus = @status WHERE TransactionNumber = @transactNum";

                    //appointment transactions
                    string cashPaymentAppt = "UPDATE appointment SET ServiceStatus = @status, NetPrice = @net, VatAmount = @vat, DiscountAmount = @discount, " +
                                        "GrossAmount = @gross, CashGiven = @cash, DueChange = @change, PaymentMethod = @payment, CheckedOutBy = @mngr " +
                                        "WHERE TransactionNumber = @transactNum"; // cash query
                    string bankPaymentAppt = "UPDATE appointment SET ServiceStatus = @status, NetPrice = @net, VatAmount = @vat, DiscountAmount = @discount, " +
                                        "GrossAmount = @gross, PaymentMethod = @payment, CardName = @cardname, CardNumber = @cardNum, " +
                                        "CVC = @cvc, CardExpiration = @expiration, CheckedOutBy = @mngr " +
                                        "WHERE TransactionNumber = @transactNum"; // credit card and paypal query
                    string walletPaymentAppt = "UPDATE appointment SET ServiceStatus = @status, NetPrice = @net, VatAmount = @vat, DiscountAmount = @discount, " +
                                        "GrossAmount = @gross, PaymentMethod = @payment, WalletNumber = @walletNum, WalletPIN = @walletPin, WalletOTP = @walletOTP, CheckedOutBy = @mngr " +
                                        "WHERE TransactionNumber = @transactNum"; // gcash and paymaya query
                    string productPaymentAppt = "UPDATE orderproducthistory SET ProductStatus = @status WHERE TransactionNumber = @transactNum";
                    
                    
                    if (RecPayServiceCashPaymentRB.Checked == true && RecPayServiceTransTypeLbl.Text == "Walk-in")
                    {
                        MySqlCommand cmd = new MySqlCommand(cashPaymentWalkin, connection);
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
                    }
                    else if ((RecPayServiceCCPaymentRB.Checked == true || RecPayServicePPPaymentRB.Checked == true) && RecPayServiceTransTypeLbl.Text == "Walk-in")
                    {
                        MySqlCommand cmd = new MySqlCommand(bankPaymentWalkin, connection);
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
                    }
                    else if ((RecPayServiceGCPaymentRB.Checked == true || RecPayServicePMPaymentRB.Checked == true) && RecPayServiceTransTypeLbl.Text == "Walk-in")
                    {
                        MySqlCommand cmd = new MySqlCommand(walletPaymentWalkin, connection);
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
                    }
                    else if (RecPayServiceCashPaymentRB.Checked == true && RecPayServiceTransTypeLbl.Text == "Appointment")
                    {
                        MySqlCommand cmd = new MySqlCommand(cashPaymentAppt, connection);
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
                    }
                    else if ((RecPayServiceCCPaymentRB.Checked == true || RecPayServicePPPaymentRB.Checked == true) && RecPayServiceTransTypeLbl.Text == "Appointment")
                    {
                        MySqlCommand cmd = new MySqlCommand(bankPaymentAppt, connection);
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
                    }
                    else if ((RecPayServiceGCPaymentRB.Checked == true || RecPayServicePMPaymentRB.Checked == true) && RecPayServiceTransTypeLbl.Text == "Appointment")
                    {
                        MySqlCommand cmd = new MySqlCommand(walletPaymentAppt, connection);
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

                    if (RecPayServiceCashPaymentRB.Checked == true && RecPayServiceTransTypeLbl.Text == "Walk-in")
                    {
                        MySqlCommand cmd = new MySqlCommand(productPaymentWalkin, connection);
                        cmd.Parameters.AddWithValue("@status", "Paid");
                        cmd.Parameters.AddWithValue("@transactNum", transactNum);


                        cmd.ExecuteNonQuery();
                        // Successful update
                        Inventory.PanelShow(MngrInventoryTypePanel);
                    }
                    else if ((RecPayServiceCCPaymentRB.Checked == true || RecPayServicePPPaymentRB.Checked == true) && RecPayServiceTransTypeLbl.Text == "Walk-in")
                    {
                        MySqlCommand cmd = new MySqlCommand(productPaymentWalkin, connection);
                        cmd.Parameters.AddWithValue("@status", "Paid");
                        cmd.Parameters.AddWithValue("@transactNum", transactNum);

                        cmd.ExecuteNonQuery();
                        // Successful update
                        Inventory.PanelShow(MngrInventoryTypePanel);
                    }
                    else if ((RecPayServiceGCPaymentRB.Checked == true || RecPayServicePMPaymentRB.Checked == true) && RecPayServiceTransTypeLbl.Text == "Walk-in")
                    {
                        MySqlCommand cmd = new MySqlCommand(productPaymentWalkin, connection);
                        cmd.Parameters.AddWithValue("@status", "Paid");
                        cmd.Parameters.AddWithValue("@transactNum", transactNum);

                        cmd.ExecuteNonQuery();
                        // Successful update
                        Inventory.PanelShow(MngrInventoryTypePanel);
                    }
                    else if (RecPayServiceCashPaymentRB.Checked == true && RecPayServiceTransTypeLbl.Text == "Appointment")
                    {
                        MySqlCommand cmd = new MySqlCommand(productPaymentAppt, connection);
                        cmd.Parameters.AddWithValue("@status", "Paid");
                        cmd.Parameters.AddWithValue("@transactNum", transactNum);


                        cmd.ExecuteNonQuery();
                        // Successful update
                        Inventory.PanelShow(MngrInventoryTypePanel);
                    }
                    else if ((RecPayServiceCCPaymentRB.Checked == true || RecPayServicePPPaymentRB.Checked == true) && RecPayServiceTransTypeLbl.Text == "Appointment")
                    {
                        MySqlCommand cmd = new MySqlCommand(productPaymentAppt, connection);
                        cmd.Parameters.AddWithValue("@status", "Paid");
                        cmd.Parameters.AddWithValue("@transactNum", transactNum);

                        cmd.ExecuteNonQuery();
                        // Successful update
                        Inventory.PanelShow(MngrInventoryTypePanel);
                    }
                    else if ((RecPayServiceGCPaymentRB.Checked == true || RecPayServicePMPaymentRB.Checked == true) && RecPayServiceTransTypeLbl.Text == "Appointment")
                    {
                        MySqlCommand cmd = new MySqlCommand(productPaymentAppt, connection);
                        cmd.Parameters.AddWithValue("@status", "Paid");
                        cmd.Parameters.AddWithValue("@transactNum", transactNum);

                        cmd.ExecuteNonQuery();
                        // Successful update
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

        private void RecPayServicePaymentButton_Click(object sender, EventArgs e)
        {
            if (!RecPayServiceCashPaymentRB.Checked &&
                !RecPayServiceCCPaymentRB.Checked &&
                !RecPayServicePPPaymentRB.Checked &&
                !RecPayServiceGCPaymentRB.Checked &&
                !RecPayServicePMPaymentRB.Checked)
            {
                MessageBox.Show("Please select a payment method.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (RecPayServiceUpdateWalkin_And_ApptDB())
            {
                RecPayServiceUpdateQtyInventory(RecPayServiceCOProdDGV);
                RecLoadCompletedWalkinTrans();
                RecLoadCompletedAppointmentTrans();
                RecPayServiceInvoiceReceiptGenerator();
                RecPayServiceClearAllField();
                Transaction.PanelShow(RecTransactionPanel);

            }
        }

        private void RecPayServiceUpdateQtyInventory(DataGridView dgv)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                {
                    connection.Open();
                    string updateQuery = "UPDATE inventory SET ItemStock = ItemStock - @Qty WHERE ItemID = @ItemID";

                    foreach (DataGridViewRow row in RecPayServiceCOProdDGV.Rows)
                    {
                        string itemID = row.Cells["ItemID"].Value.ToString();
                        int qty = Convert.ToInt32(row.Cells["Qty"].Value);

                        MySqlCommand command = new MySqlCommand(updateQuery, connection);
                        command.Parameters.AddWithValue("@Qty", qty);
                        command.Parameters.AddWithValue("@ItemID", itemID);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMessage = "An error occurred: " + ex.Message + "\n\n" + ex.StackTrace;
                MessageBox.Show(errorMessage, "Product Qty Failed Inserting to Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        private void RecPayServiceClearAllField()
        {
            RecPayServiceNetAmountBox.Text = "0.00";
            RecPayServiceVATBox.Text = "0.00";
            RecPayServiceDiscountBox.Text = "0.00";
            RecPayServiceGrossAmountBox.Text = "0.00";
            RecPayServiceCashBox.Text = "0";
            RecPayServiceChangeBox.Text = "0.00";
            RecPayServiceTypeText.Text = "";

            RecPayServiceCardNameText.Text = "";
            RecPayServiceCardNumText.Text = "";
            RecPayServiceCVCText.Text = "";
            RecPayServiceCardExpText.Text = "MM/YY";
            RecPayServiceWalletNumText.Text = "";
            RecPayServiceWalletPINText.Text = "";
            RecPayServiceWalletOTPText.Text = "";

            RecPayServiceClientNameLbl.Text = "";
            RecPayServiceTransTypeLbl.Text = "";
            // Clear rows from RecPayServiceAcquiredDGV
            RecPayServiceAcquiredDGV.DataSource = null; // Set data source to null
            RecPayServiceAcquiredDGV.Rows.Clear(); // Clear any remaining rows

            // Clear rows from RecPayServiceCOProdDGV
            RecPayServiceCOProdDGV.DataSource = null; // Set data source to null
            RecPayServiceCOProdDGV.Rows.Clear(); // Clear any remaining rows

        }

        private void RecPayServiceBtn_Click(object sender, EventArgs e)
        {
            Transaction.PanelShow(RecPayServicePanel);
            RecLoadCompletedWalkinTrans();
            RecLoadCompletedAppointmentTrans();
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

        private void RecPayServiceInvoiceReceiptGenerator()
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
                    iTextSharp.text.Font italic = FontFactory.GetFont("Courier", 10, iTextSharp.text.Font.ITALIC);

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

                    foreach (DataGridViewRow row in RecPayServiceAcquiredDGV.Rows)
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

                            // Add cells to the item table
                            PdfPTable serviceTable = new PdfPTable(4);
                            serviceTable.SetWidths(new float[] { 5f, 5f, 3f, 3f }); // Column widths
                            serviceTable.DefaultCell.Border = PdfPCell.NO_BORDER;
                            serviceTable.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
                            serviceTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

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

                            // Add cells to the item table
                            PdfPTable productTable = new PdfPTable(4);
                            productTable.SetWidths(new float[] { 5f, 5f, 3f, 3f }); // Column widths
                            productTable.DefaultCell.Border = PdfPCell.NO_BORDER;
                            productTable.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
                            productTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

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
                    decimal netAmount = decimal.Parse(RecPayServiceNetAmountBox.Text);
                    decimal discount = decimal.Parse(RecPayServiceDiscountBox.Text);
                    decimal vat = decimal.Parse(RecPayServiceVATBox.Text);
                    decimal grossAmount = decimal.Parse(RecPayServiceGrossAmountBox.Text);
                    decimal cash = decimal.Parse(RecPayServiceCashBox.Text);
                    decimal change = decimal.Parse(RecPayServiceChangeBox.Text);
                    string paymentMethod = RecPayServiceTypeText.Text;

                    // Create a new table for the "Total" section
                    PdfPTable totalTable = new PdfPTable(2); // 2 columns for the "Total" table
                    totalTable.SetWidths(new float[] { 5f, 3f }); // Column widths
                    totalTable.DefaultCell.Border = PdfPCell.NO_BORDER;

                    int totalRowCount = RecPayServiceAcquiredDGV.Rows.Count + RecPayServiceCOProdDGV.Rows.Count;

                    // Add cells to the "Total" table
                    totalTable.AddCell(new Phrase($"Total # of Service/Products ({totalRowCount})", font));
                    totalTable.AddCell(new Phrase($"Php {grossAmount:F2}", font));
                    totalTable.AddCell(new Phrase($"Cash Given", font));
                    totalTable.AddCell(new Phrase($"Php {cash:F2}", font));
                    totalTable.AddCell(new Phrase($"Change", font));
                    totalTable.AddCell(new Phrase($"Php {change:F2}", font));
                    totalTable.AddCell(new Phrase($"Payment Method:", font));
                    totalTable.AddCell(new Phrase($"{paymentMethod:F2}", font));

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
                    doc.Add(new Paragraph($"Served To: {clientName}", italic));
                    doc.Add(new Paragraph("Address:_______________________________", italic));
                    doc.Add(new Paragraph("TIN No.:_______________________________", italic));

                    // Add the legal string with center alignment
                    Paragraph paragraph_footer = new Paragraph($"\n\n{legal}", italic);
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

        //ApptMember
        public void LoadAppointmentPreferredStaffComboBox()
        {
            using (MySqlConnection connection = new MySqlConnection(mysqlconn))
            {
                string bookedtime = RecApptBookingTimeComboBox.SelectedItem.ToString();
                string appointmentDate = RecApptBookingDatePicker.Value.ToString("MM-dd-yyyy dddd");

                connection.Open();

                string query = "SELECT EmployeeID, Gender, LastName, FirstName FROM systemusers WHERE EmployeeCategory = @FilterValue";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@FilterValue", filterstaffbyservicecategory);

                List<string> employeeIDs = new List<string>();
                List<string> genders = new List<string>();
                List<string> lastNames = new List<string>();
                List<string> firstNames = new List<string>();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string employeeID = reader.GetString("EmployeeID");
                        string gender = reader.GetString("Gender");
                        string lastName = reader.GetString("LastName");
                        string firstName = reader.GetString("FirstName");

                        employeeIDs.Add(employeeID);
                        genders.Add(gender);
                        lastNames.Add(lastName);
                        firstNames.Add(firstName);
                    }
                }

                RecApptAvailableAttendingStaffSelectedComboBox.Items.Clear();
                RecApptAvailableAttendingStaffSelectedComboBox.Items.Add("Select a Preferred Staff");

                for (int i = 0; i < employeeIDs.Count; i++)
                {
                    string employeeID = employeeIDs[i];
                    string gender = genders[i];
                    string lastName = lastNames[i];
                    string firstName = firstNames[i];

                    string comboBoxItem = $"{employeeID}-{gender}-{lastName}, {firstName}";

                    string scheduleQuery = "SELECT 1 FROM staffappointmentschedule WHERE EmployeeID = @EmployeeID AND AppointmentDate = @AppointmentDate AND AppointmentTime = @AppointmentTime LIMIT 1";
                    MySqlCommand scheduleCommand = new MySqlCommand(scheduleQuery, connection);
                    scheduleCommand.Parameters.AddWithValue("@EmployeeID", employeeID);
                    scheduleCommand.Parameters.AddWithValue("@AppointmentDate", appointmentDate);
                    scheduleCommand.Parameters.AddWithValue("@AppointmentTime", bookedtime);
                    object result = scheduleCommand.ExecuteScalar();

                    if (result == null)
                    {
                        RecApptAvailableAttendingStaffSelectedComboBox.Items.Add(comboBoxItem);
                    }
                }
            }

            RecApptAvailableAttendingStaffSelectedComboBox.SelectedIndex = 0;
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

        //ApptMember
        private void ShowNoServiceCategoryChosenWarningMessage()
        {
            RecWalkinNoServiceCategoryChosenWarningLbl.Visible = true;
            RecApptNoServiceCategoryChosenWarningLbl.Visible = true;
            AnimateShakeEffect(RecWalkinNoServiceCategoryChosenWarningLbl);
            AnimateShakeEffect(RecApptNoServiceCategoryChosenWarningLbl);

            Timer timer = new Timer();
            timer.Interval = 1500; // 1 seconds
            timer.Tick += (s, e) =>
            {
                RecWalkinNoServiceCategoryChosenWarningLbl.Visible = false;
                RecApptNoServiceCategoryChosenWarningLbl.Visible = false;

                timer.Stop();
            };
            timer.Start();
        }


        //ApptMember
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

        //ApptMember
        private void RecApptAvailableAttendingStaffSelectedComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RecApptAvailableAttendingStaffSelectedComboBox.SelectedItem != null)
            {
                string selectedValue = RecApptAvailableAttendingStaffSelectedComboBox.SelectedItem.ToString();
                selectedStaffID = selectedValue.Substring(0, 11);
            }
        }
            private void RecPayServiceVATExemptChk_CheckedChanged(object sender, EventArgs e)
        {
            if (RecPayServiceVATExemptChk.Checked)
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
            if (decimal.TryParse(RecPayServiceNetAmountBox.Text, out decimal netAmount))
            {
                // For VAT exemption, set VAT Amount to zero
                decimal vatAmount = 0;

                // Set the Net Amount as the new Gross Amount
                decimal grossAmount = netAmount;

                // Display the calculated values in TextBoxes
                RecPayServiceVATBox.Text = vatAmount.ToString("0.00");
                RecPayServiceVATBox.Text = vatAmount.ToString("0.00");

            }
        }
        #endregion

        #region Receptionist Queue Window
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
                        RecQueWinNextCustomerDGV.Columns[15].Visible = false; //customization
                        RecQueWinNextCustomerDGV.Columns[16].Visible = false; //customization

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
                        RecQueWinNextCustomerDGV.Columns[15].Visible = false; // preferred staff
                        RecQueWinNextCustomerDGV.Columns[16].Visible = false; // preferred staff
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
                        RecQueWinNextCustomerDGV.Columns[15].Visible = false; // que type
                        RecQueWinNextCustomerDGV.Columns[16].Visible = false; // que type


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
        #endregion

        #region Receptionsit Walk-in Appointment

        //ApptMember
        private void RecApptPanelExitBtn_Click(object sender, EventArgs e)
        {
            Transaction.PanelShow(RecTransactionPanel);
        }

        //ApptMember
        private void RecApptClientBdayPicker_ValueChanged(object sender, EventArgs e)
        {
            DateTime selectedDate = RecApptClientBdayPicker.Value;
            int age = DateTime.Now.Year - selectedDate.Year;

            if (DateTime.Now < selectedDate.AddYears(age))
            {
                age--; // Subtract 1 if the birthday hasn't occurred yet this year
            }
            RecApptClientAgeText.Text = age.ToString();
            if (age < 18)
            {
                RecApptClientAgeErrorLbl.Visible = true;
                RecApptClientAgeErrorLbl.Text = "Must be 18yrs old\nand above";
                return;
            }
            else
            {
                RecApptClientAgeErrorLbl.Visible = false;

            }
        }

        //ApptMember
        string[] bookingTimes = new string[]
        {
            "Select a booking time", "08:00 am", "08:30 am", "09:00 am",
            "09:30 am", "10:00 am", "10:30 am", "11:00 am", "11:30 am",
            "01:00 pm", "01:30 pm", "02:00 pm", "02:30 pm", "03:00 pm",
        };

        //ApptMember
        private void RecApptCatHSBtn_Click(object sender, EventArgs e)
        {
            filterstaffbyservicecategory = "Hair Styling";
            haschosenacategory = true;
            if (RecApptPreferredStaffToggleSwitch.Checked == true)
            {
                RecApptAvailableAttendingStaffSelectedComboBox.Items.Clear();
                LoadAppointmentPreferredStaffComboBox();
            }
            LoadBookingTimes();
            RecApptHairStyle();
        }

        //ApptMember
        private void RecApptCatFSBtn_Click(object sender, EventArgs e)
        {
            filterstaffbyservicecategory = "Face & Skin";
            haschosenacategory = true;
            if (RecApptPreferredStaffToggleSwitch.Checked == true)
            {
                RecApptAvailableAttendingStaffSelectedComboBox.Items.Clear();
                LoadAppointmentPreferredStaffComboBox();
            }
            LoadBookingTimes();
            RecApptFace();
        }

        //ApptMember
        private void RecApptCatNCBtn_Click(object sender, EventArgs e)
        {
            filterstaffbyservicecategory = "Nail Care";
            haschosenacategory = true;
            if (RecApptPreferredStaffToggleSwitch.Checked == true)
            {
                RecApptAvailableAttendingStaffSelectedComboBox.Items.Clear();
                LoadAppointmentPreferredStaffComboBox();
            }
            LoadBookingTimes();
            RecApptNail();
        }

        //ApptMember
        private void RecApptCatSpaBtn_Click(object sender, EventArgs e)
        {
            filterstaffbyservicecategory = "Spa";
            haschosenacategory = true;
            if (RecApptPreferredStaffToggleSwitch.Checked == true)
            {
                RecApptAvailableAttendingStaffSelectedComboBox.Items.Clear();
                LoadAppointmentPreferredStaffComboBox();
            }
            LoadBookingTimes();
            RecApptSpa();
        }

        //ApptMember
        private void RecApptCatMassBtn_Click(object sender, EventArgs e)
        {
            filterstaffbyservicecategory = "Massage";
            haschosenacategory = true;
            if (RecApptPreferredStaffToggleSwitch.Checked == true)
            {
                RecApptAvailableAttendingStaffSelectedComboBox.Items.Clear();
                LoadAppointmentPreferredStaffComboBox();
            }
            LoadBookingTimes();
            RecApptMassage();
        }

        //ApptMember
        private void RecApptHairStyle()
        {
            if (RecApptCatHSRB.Checked == false)
            {
                RecApptCatHSRB.Visible = true;
                RecApptCatHSRB.Checked = true;
                RecApptLoadServiceTypeComboBox("Hair Styling");

                RecApptCatFSRB.Visible = false;
                RecApptCatNCRB.Visible = false;
                RecApptCatSpaRB.Visible = false;
                RecApptCatMassRB.Visible = false;
                RecApptCatFSRB.Checked = false;
                RecApptCatNCRB.Checked = false;
                RecApptCatSpaRB.Checked = false;
                RecApptCatMassRB.Checked = false;
                return;
            }
            else if (RecApptCatHSRB.Checked == true)
            {
                RecApptCatHSRB.Visible = true;
                RecApptCatHSRB.Checked = true;
                RecApptLoadServiceTypeComboBox("Hair Styling");

                RecApptCatFSRB.Visible = false;
                RecApptCatNCRB.Visible = false;
                RecApptCatSpaRB.Visible = false;
                RecApptCatMassRB.Visible = false;
                RecApptCatFSRB.Checked = false;
                RecApptCatNCRB.Checked = false;
                RecApptCatSpaRB.Checked = false;
                RecApptCatMassRB.Checked = false;
            }
        }
        //ApptMember
        private void RecApptFace()
        {
            if (RecApptCatFSRB.Checked == false)
            {
                RecApptCatFSRB.Visible = true;
                RecApptCatFSRB.Checked = true;
                RecApptLoadServiceTypeComboBox("Face & Skin");

                RecApptCatHSRB.Visible = false;
                RecApptCatNCRB.Visible = false;
                RecApptCatSpaRB.Visible = false;
                RecApptCatMassRB.Visible = false;
                RecApptCatHSRB.Checked = false;
                RecApptCatNCRB.Checked = false;
                RecApptCatSpaRB.Checked = false;
                RecApptCatMassRB.Checked = false;
                return;
            }
            else if (RecApptCatFSRB.Checked == true)
            {
                RecApptCatFSRB.Visible = true;
                RecApptCatFSRB.Checked = true;
            }
        }
        //ApptMember
        private void RecApptNail()
        {
            if (RecApptCatNCRB.Checked == false)
            {
                RecApptCatNCRB.Visible = true;
                RecApptCatNCRB.Checked = true;
                RecApptLoadServiceTypeComboBox("Nail Care");

                RecApptCatHSRB.Visible = false;
                RecApptCatFSRB.Visible = false;
                RecApptCatSpaRB.Visible = false;
                RecApptCatMassRB.Visible = false;
                RecApptCatHSRB.Checked = false;
                RecApptCatFSRB.Checked = false;
                RecApptCatSpaRB.Checked = false;
                RecApptCatMassRB.Checked = false;
                return;
            }
            else if (RecApptCatNCRB.Checked == true)
            {
                RecApptCatNCRB.Visible = true;
                RecApptCatNCRB.Checked = true;
            }
        }
        //ApptMember
        private void RecApptSpa()
        {
            if (RecApptCatSpaRB.Checked == false)
            {
                RecApptCatSpaRB.Visible = true;
                RecApptCatSpaRB.Checked = true;
                RecApptLoadServiceTypeComboBox("Spa");

                RecApptCatHSRB.Visible = false;
                RecApptCatFSRB.Visible = false;
                RecApptCatNCRB.Visible = false;
                RecApptCatMassRB.Visible = false;
                RecApptCatHSRB.Checked = false;
                RecApptCatFSRB.Checked = false;
                RecApptCatNCRB.Checked = false;
                RecApptCatMassRB.Checked = false;
                return;
            }
            else if (RecApptCatSpaRB.Checked == true)
            {
                RecApptCatSpaRB.Visible = true;
                RecApptCatSpaRB.Checked = true;
            }
        }
        //ApptMember
        private void RecApptMassage()
        {
            if (RecApptCatMassRB.Checked == false)
            {
                RecApptCatMassRB.Visible = true;
                RecApptCatMassRB.Checked = true;
                RecApptLoadServiceTypeComboBox("Massage");

                RecApptCatHSRB.Visible = false;
                RecApptCatFSRB.Visible = false;
                RecApptCatNCRB.Visible = false;
                RecApptCatSpaRB.Visible = false;
                RecApptCatHSRB.Checked = false;
                RecApptCatFSRB.Checked = false;
                RecApptCatNCRB.Checked = false;
                RecApptCatSpaRB.Checked = false;
                return;
            }
            else if (RecApptCatMassRB.Checked == true)
            {
                RecApptCatMassRB.Visible = true;
                RecApptCatMassRB.Checked = true;
            }
        }

        //ApptMember
        public void RecApptLoadHairStyleType()
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

                        RecApptServiceTypeDGV.Columns.Clear();


                        RecApptServiceTypeDGV.DataSource = dataTable;

                        RecApptServiceTypeDGV.Columns[0].Visible = false; //service category
                        RecApptServiceTypeDGV.Columns[1].Visible = false; // service type
                        RecApptServiceTypeDGV.Columns[2].Visible = false; // service ID
                        RecApptServiceTypeDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        RecApptServiceTypeDGV.ClearSelection();
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
        public void RecApptFaceSkinType()
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

                        RecApptServiceTypeDGV.Columns.Clear();


                        RecApptServiceTypeDGV.DataSource = dataTable;

                        RecApptServiceTypeDGV.Columns[0].Visible = false;
                        RecApptServiceTypeDGV.Columns[1].Visible = false;
                        RecApptServiceTypeDGV.Columns[2].Visible = false;
                        RecApptServiceTypeDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        RecApptServiceTypeDGV.ClearSelection();
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

        //ApptMember
        public void RecApptNailCareType()
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

                        RecApptServiceTypeDGV.Columns.Clear();


                        RecApptServiceTypeDGV.DataSource = dataTable;

                        RecApptServiceTypeDGV.Columns[0].Visible = false;
                        RecApptServiceTypeDGV.Columns[1].Visible = false;
                        RecApptServiceTypeDGV.Columns[2].Visible = false;
                        RecApptServiceTypeDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        RecApptServiceTypeDGV.ClearSelection();
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

        //ApptMember
        public void RecApptSpaType()
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

                        RecApptServiceTypeDGV.Columns.Clear();


                        RecApptServiceTypeDGV.DataSource = dataTable;

                        RecApptServiceTypeDGV.Columns[0].Visible = false;
                        RecApptServiceTypeDGV.Columns[1].Visible = false;
                        RecApptServiceTypeDGV.Columns[2].Visible = false;
                        RecApptServiceTypeDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        RecApptServiceTypeDGV.ClearSelection();
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

        //ApptMember
        public void RecApptMassageType()
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

                        RecApptServiceTypeDGV.Columns.Clear();


                        RecApptServiceTypeDGV.DataSource = dataTable;

                        RecApptServiceTypeDGV.Columns[0].Visible = false;
                        RecApptServiceTypeDGV.Columns[1].Visible = false;
                        RecApptServiceTypeDGV.Columns[2].Visible = false;
                        RecApptServiceTypeDGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        RecApptServiceTypeDGV.ClearSelection();
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

        //ApptMember
        private void RecApptLoadServiceTypeComboBox(string selectedCategory)
        {
            // Filter and add the relevant service types based on the selected category
            switch (selectedCategory)
            {
                case "Hair Styling":
                    RecApptLoadHairStyleType();
                    break;
                case "Nail Care":
                    RecApptNailCareType();
                    break;
                case "Face & Skin":
                    RecApptFaceSkinType();
                    break;
                case "Massage":
                    RecApptMassageType();
                    break;
                case "Spa":
                    RecApptSpaType();
                    break;
                default:
                    break;
            }

        }

        //ApptMember
        private void RecApptAddService()
        {


            if (RecApptServiceTypeDGV.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a service.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (string.IsNullOrEmpty(selectedStaffID))
            {
                MessageBox.Show("Please select a prefered staff or toggle anyone ", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (RecApptBookingTimeComboBox.SelectedIndex == 0 || RecApptBookingTimeComboBox.SelectedItem == null || RecApptBookingTimeComboBox.SelectedItem.ToString() == "Cutoff Time")
            {
                MessageBox.Show("Please select a booking time", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DataGridViewRow selectedRow = RecApptServiceTypeDGV.SelectedRows[0];

            string SelectedCategory = selectedRow.Cells[0].Value.ToString();
            string ServiceID = selectedRow.Cells[2].Value.ToString();
            string ServiceName = selectedRow.Cells[3].Value.ToString();
            string ServicePrice = selectedRow.Cells[6].Value.ToString();
            string ServiceTime = RecApptBookingTimeComboBox.SelectedItem.ToString();
            string serviceID = selectedRow.Cells[2]?.Value?.ToString(); // Use null-conditional operator to avoid NullReferenceException

            // ... (existing code)

            if (RecApptServiceTypeDGV.SelectedRows.Count == 0)
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

            if (RecApptAvailableAttendingStaffSelectedComboBox.SelectedItem?.ToString() == "Select a Preferred Staff") // 4942
            {
                MessageBox.Show("Please select a preferred staff or toggle anyone.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (DataGridViewRow row in RecApptSelectedServiceDGV.Rows)
            {
                string existingServiceID = row.Cells["RecApptServiceID"]?.Value?.ToString(); // Use null-conditional operator

                if (serviceID == existingServiceID)
                {
                    MessageBox.Show("This service is already selected.", "Duplicate Service", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
            }



            DialogResult result = MessageBox.Show("Are you sure you want to add this service?", "Confirm Service Selection", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Add the row
                DataGridViewRow NewSelectedServiceRow = RecApptSelectedServiceDGV.Rows[RecApptSelectedServiceDGV.Rows.Add()];

                string appointmentDate = RecApptBookingDatePicker.Value.ToString("MM-dd-yyyy dddd");
                string serviceCategory = SelectedCategory;
                int latestprioritynumber = GetLargestPriorityNum(appointmentDate, serviceCategory);

                NewSelectedServiceRow.Cells["RecApptServicePrice"].Value = ServicePrice;
                NewSelectedServiceRow.Cells["RecApptServiceCategory"].Value = SelectedCategory;
                NewSelectedServiceRow.Cells["RecApptSelectedService"].Value = ServiceName;
                NewSelectedServiceRow.Cells["RecApptServiceID"].Value = ServiceID;
                NewSelectedServiceRow.Cells["RecApptTimeSelected"].Value = ServiceTime;
                NewSelectedServiceRow.Cells["RecApptPriorityNumber"].Value = latestprioritynumber;
                NewSelectedServiceRow.Cells["RecApptStaffSelected"].Value = selectedStaffID;
                QueTypeIdentifier(NewSelectedServiceRow.Cells["RecApptQueType"]);

                RecApptServiceTypeDGV.ClearSelection();

            }
        }

        private void RecApptServiceTypeDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            RecApptAddService();
        }

        //ApptMember
        private void RecApptSelectServiceAndStaffBtn_Click(object sender, EventArgs e)
        {
            RecApptAddService();
        }

        //ApptMember
        private void RecApptAnyStaffToggleSwitch_CheckedChanged(object sender, EventArgs e)
        {
            if (haschosenacategory == false)
            {
                ShowNoServiceCategoryChosenWarningMessage();
                RecApptAnyStaffToggleSwitch.CheckedChanged -= RecApptAnyStaffToggleSwitch_CheckedChanged;
                RecApptAnyStaffToggleSwitch.Checked = false;
                RecApptAttendingStaffLbl.Visible = false;
                RecApptAvailableAttendingStaffSelectedComboBox.Visible = false;
                RecApptAnyStaffToggleSwitch.CheckedChanged += RecApptAnyStaffToggleSwitch_CheckedChanged;
                return;
            }
            else
            {
                if (RecApptAnyStaffToggleSwitch.Checked)
                {
                    RecApptPreferredStaffToggleSwitch.Checked = false;
                    RecApptAvailableAttendingStaffSelectedComboBox.Enabled = false;
                    RecApptAttendingStaffLbl.Visible = false;
                    RecApptAvailableAttendingStaffSelectedComboBox.Visible = false;
                    selectedStaffID = "Anyone";
                    RecApptAvailableAttendingStaffSelectedComboBox.Items.Clear();
                }
            }
        }

        //ApptMember
        private void RecApptPreferredStaffToggleSwitch_CheckedChanged(object sender, EventArgs e)
        {
            if (haschosenacategory == false)
            {
                ShowNoServiceCategoryChosenWarningMessage();
                RecApptPreferredStaffToggleSwitch.CheckedChanged -= RecApptPreferredStaffToggleSwitch_CheckedChanged;
                RecApptPreferredStaffToggleSwitch.Checked = false;
                RecApptAttendingStaffLbl.Visible = false;
                RecApptAvailableAttendingStaffSelectedComboBox.Visible = false;
                RecApptPreferredStaffToggleSwitch.CheckedChanged += RecApptPreferredStaffToggleSwitch_CheckedChanged;
                return;
            }
            else
            {
                if (RecApptPreferredStaffToggleSwitch.Checked && RecApptAvailableAttendingStaffSelectedComboBox.SelectedText != "Select a Preferred Staff")
                {
                    RecApptAnyStaffToggleSwitch.Checked = false;
                    RecApptAvailableAttendingStaffSelectedComboBox.Enabled = true;
                    RecApptAttendingStaffLbl.Visible = true;
                    RecApptAvailableAttendingStaffSelectedComboBox.Visible = true;
                    LoadAppointmentPreferredStaffComboBox();
                }
                else
                {
                    selectedStaffID = "Anyone";
                    RecApptAvailableAttendingStaffSelectedComboBox.Enabled = false;
                    RecApptAttendingStaffLbl.Visible = false;
                    RecApptAvailableAttendingStaffSelectedComboBox.Visible = false;
                    RecApptAvailableAttendingStaffSelectedComboBox.Items.Clear();
                }
            }
        }

        private void RecApptAttendingStaffSelectedComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        //ApptMember
        private void RecApptDeleteSelectedServiceAndStaffBtn_Click(object sender, EventArgs e)
        {
            if (RecApptSelectedServiceDGV.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this row?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    DataGridViewRow selectedRow = RecApptSelectedServiceDGV.SelectedRows[0];
                    RecApptSelectedServiceDGV.Rows.Remove(selectedRow);
                }
            }
        }


        //ApptMember
        private void RecApptBookTransactBtn_Click(object sender, EventArgs e)
        {
            DateTime selectedDate = RecApptBookingDatePicker.Value.Date;
            DateTime currentDate = DateTime.Today;
            if (string.IsNullOrWhiteSpace(RecApptFNameText.Text) || RecApptFNameText.Text == "First Name")
            {
                MessageBox.Show("Please enter a first name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!IsCardNameValid(RecApptFNameText.Text))
            {
                MessageBox.Show("Invalid First Name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (string.IsNullOrWhiteSpace(RecApptLNameText.Text) || RecApptLNameText.Text == "Last Name")
            {
                MessageBox.Show("Please enter a last name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!IsCardNameValid(RecApptLNameText.Text))
            {
                MessageBox.Show("Invalid Last Name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (string.IsNullOrWhiteSpace(RecApptCPNumText.Text) || RecApptCPNumText.Text == "Mobile Number")
            {
                MessageBox.Show("Please enter a contact number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!IsNumeric(RecApptCPNumText.Text))
            {
                MessageBox.Show("Invalid Contact Number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (string.IsNullOrWhiteSpace(RecApptClientAgeText.Text) || RecApptClientAgeText.Text == "Age")
            {
                MessageBox.Show("Please enter birth date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!IsNumeric(RecApptClientAgeText.Text))
            {
                MessageBox.Show("Invalid Age.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (int.TryParse(RecApptClientAgeText.Text, out int age) && age < 18)
            {
                MessageBox.Show("The client's age must be at least 18.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //else if (selectedDate == currentDate)
            //{
            //    MessageBox.Show("The selected date cannot be today.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            else if (RecApptBookingTimeComboBox.SelectedItem == null || RecApptBookingTimeComboBox.SelectedIndex == 0)
            {
                MessageBox.Show("Please select a booking time.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (RecApptSelectedServiceDGV != null && RecApptSelectedServiceDGV.Rows.Count == 0)
            {
                MessageBox.Show("Select a service first to proceed on booking a transaction.", "Ooooops!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                RecApptServiceHistoryDB(RecApptSelectedServiceDGV); //service history db
                ReceptionistAppointmentDB(); //appointment transaction db
                RecApptTransactNumRefresh();
                RecApptTransactionClear();
            }
        }

        //ApptMember
        private void RecApptServiceHistoryDB(DataGridView RecApptSelectedServiceDGV)
        {
            DateTime pickedDate = RecApptBookingDatePicker.Value;
            string transactionNum = RecApptTransNumText.Text;
            string transactionType = "Walk-in Appointment Transaction";
            string serviceStatus = "Pending";

            //booked values
            string bookedDate = pickedDate.ToString("MM-dd-yyyy dddd"); //bookedDate

            //basic info
            string CustomerName = RecApptFNameText.Text + " " + RecApptLNameText.Text; //client name

            if (RecApptSelectedServiceDGV.Rows.Count > 0)
            {
                try
                {
                    using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                    {
                        connection.Open();

                        foreach (DataGridViewRow row in RecApptSelectedServiceDGV.Rows)
                        {
                            if (row.Cells["RecApptSelectedService"].Value != null)
                            {
                                string serviceName = row.Cells["RecApptSelectedService"].Value.ToString();
                                string serviceCat = row.Cells["RecApptServiceCategory"].Value.ToString();
                                string serviceID = row.Cells["RecApptServiceID"].Value.ToString();
                                decimal servicePrice = Convert.ToDecimal(row.Cells["RecApptServicePrice"].Value);
                                string selectedStaff = row.Cells["RecApptStaffSelected"].Value.ToString();
                                string quepriorityNumber = row.Cells["RecApptPriorityNumber"].Value.ToString();
                                string queType = row.Cells["RecApptQueType"].Value.ToString();
                                string bookedTime = row.Cells["RecApptTimeSelected"].Value.ToString();

                                string insertQuery = "INSERT INTO servicehistory (TransactionNumber, TransactionType, ServiceStatus, AppointmentDate, AppointmentTime, ClientName, " +
                                                     "ServiceCategory, ServiceID, SelectedService, ServicePrice, PreferredStaff, PriorityNumber," +
                                                     "QueType" +
                                                     ") VALUES (@Transact, @TransactType, @status, @appointDate, @appointTime, @name, @serviceCat, @ID, @serviceName, @servicePrice, " +
                                                     "@preferredstaff, @queprioritynumber, @quetype)";

                                MySqlCommand cmd = new MySqlCommand(insertQuery, connection);
                                cmd.Parameters.AddWithValue("@Transact", transactionNum);
                                cmd.Parameters.AddWithValue("@TransactType", transactionType);
                                cmd.Parameters.AddWithValue("@status", serviceStatus);
                                cmd.Parameters.AddWithValue("@appointDate", bookedDate);
                                cmd.Parameters.AddWithValue("@appointTime", bookedTime);
                                cmd.Parameters.AddWithValue("@name", CustomerName);
                                cmd.Parameters.AddWithValue("@serviceCat", serviceCat);
                                cmd.Parameters.AddWithValue("@ID", serviceID);
                                cmd.Parameters.AddWithValue("@serviceName", serviceName);
                                cmd.Parameters.AddWithValue("@servicePrice", servicePrice);
                                cmd.Parameters.AddWithValue("@preferredstaff", selectedStaff);
                                cmd.Parameters.AddWithValue("@queprioritynumber", quepriorityNumber);
                                cmd.Parameters.AddWithValue("@quetype", queType);

                                cmd.ExecuteNonQuery();

                                if (selectedStaff != "Anyone")
                                {
                                    string insertScheduleQuery = "INSERT INTO staffappointmentschedule (EmployeeID, AppointmentDate, AppointmentTime,TransactionNumber,ServiceName,ServiceCategory,ServiceID) VALUES (@EmployeeID, @AppointmentDate, @AppointmentTime, @Transact, " +
                                                                 "@serviceName, @serviceCat, @ID )";
                                    MySqlCommand insertScheduleCommand = new MySqlCommand(insertScheduleQuery, connection);
                                    insertScheduleCommand.Parameters.AddWithValue("@EmployeeID", selectedStaff);
                                    insertScheduleCommand.Parameters.AddWithValue("@AppointmentDate", bookedDate);
                                    insertScheduleCommand.Parameters.AddWithValue("@AppointmentTime", bookedTime);
                                    insertScheduleCommand.Parameters.AddWithValue("@Transact", transactionNum);
                                    insertScheduleCommand.Parameters.AddWithValue("@serviceName", serviceName);
                                    insertScheduleCommand.Parameters.AddWithValue("@serviceCat", serviceCat);
                                    insertScheduleCommand.Parameters.AddWithValue("@ID", serviceID);
                                    insertScheduleCommand.ExecuteNonQuery();
                                }
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

        //ApptMember
        private void ReceptionistAppointmentDB()
        {
            DateTime appointmentdate = RecApptBookingDatePicker.Value;
            string transactionNum = RecApptTransNumText.Text;
            DateTime currentDate = DateTime.Today;
            string serviceStatus = "Pending";
            string transactType = "Walk-in Appointment";
            string appointmentStatus = "Unconfirmed";

            //basic info
            string CustomerName = RecApptFNameText.Text + " " + RecApptLNameText.Text; //client name
            string CustomerMobileNumber = RecApptCPNumText.Text; //client cp num

            //booked values
            string appointmentbookedDate = appointmentdate.ToString("MM-dd-yyyy dddd"); //bookedDate
            string appointmentbookedTime = RecApptBookingTimeComboBox.SelectedItem?.ToString(); //bookedTime
            string bookedDate = currentDate.ToString("MM-dd-yyyy dddd"); //bookedDate
            string bookedTime = currentDate.ToString("hh:mm tt"); //bookedTime
            string bookedBy = RecNameLbl.Text; //booked by


            try
            {
                using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                {
                    connection.Open();
                    string insertQuery = "INSERT INTO appointment (TransactionNumber, TransactionType, ServiceStatus, AppointmentDate, AppointmentTime, AppointmentStatus, " +
                                        "ClientName, ClientCPNum, ServiceDuration, BookedBy, BookedDate, BookedTime)" +
                                        "VALUES (@Transact, @TransactType, @status, @appointDate, @appointTime, @appointStatus, @clientName, @clientCP, @duration, @bookedBy, @bookedDate, @bookedTime)";

                    MySqlCommand cmd = new MySqlCommand(insertQuery, connection);
                    cmd.Parameters.AddWithValue("@Transact", transactionNum);
                    cmd.Parameters.AddWithValue("@TransactType", transactType);
                    cmd.Parameters.AddWithValue("@status", serviceStatus);
                    cmd.Parameters.AddWithValue("@appointDate", appointmentbookedDate);
                    cmd.Parameters.AddWithValue("@appointTime", appointmentbookedTime);
                    cmd.Parameters.AddWithValue("@appointStatus", appointmentStatus);
                    cmd.Parameters.AddWithValue("@clientName", CustomerName);
                    cmd.Parameters.AddWithValue("@clientCP", CustomerMobileNumber);
                    cmd.Parameters.AddWithValue("@duration", "00:00:00");
                    cmd.Parameters.AddWithValue("@bookedBy", bookedBy);
                    cmd.Parameters.AddWithValue("@bookedDate", bookedDate);
                    cmd.Parameters.AddWithValue("@bookedTime", bookedTime);


                    cmd.ExecuteNonQuery();
                }
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

        //ApptMember
        private int GetLargestPriorityNum(string appointmentDate, string serviceCategory)
        {
            using (MySqlConnection connection = new MySqlConnection(mysqlconn))
            {
                connection.Open();

                using (MySqlCommand command = connection.CreateCommand())
                {
                    string query = "SELECT MAX(CAST(PriorityNumber AS UNSIGNED)) FROM servicehistory WHERE AppointmentDate = @AppointmentDate AND ServiceCategory = @ServiceCategory";
                    command.CommandText = query;

                    command.Parameters.AddWithValue("@AppointmentDate", appointmentDate);
                    command.Parameters.AddWithValue("@ServiceCategory", serviceCategory);

                    object result = command.ExecuteScalar();
                    int latestprioritynumber = result != DBNull.Value ? Convert.ToInt32(result) : 0;

                    if (latestprioritynumber > 0)
                    {
                        latestprioritynumber++;
                    }
                    else
                    {
                        latestprioritynumber = 1;
                    }

                    return latestprioritynumber;
                }
            }
        }


        //ApptMember
        private void RecApptTransactNumRefresh()
        {
            RecApptTransNumText.Text = TransactionNumberGenerator.AppointGenerateTransNumberInc();
        }

        //ApptMember
        private void RecApptTransactionClear()
        {
            RecApptFNameText.Text = "";
            RecApptLNameText.Text = "";
            RecApptCPNumText.Text = "";
            RecApptCatHSRB.Checked = false;
            RecApptCatFSRB.Checked = false;
            RecApptCatNCRB.Checked = false;
            RecApptCatSpaRB.Checked = false;
            RecApptCatMassRB.Checked = false;
            RecApptSelectedServiceDGV.Rows.Clear();
            RecApptBookingTimeComboBox.Items.Clear();
            RecApptClientBdayPicker.Value = DateTime.Today;
            RecApptClientAgeText.Text = "Age";
            RecApptBookingDatePicker.Value = DateTime.Today;
            RecApptPreferredStaffToggleSwitch.Checked = false;
            RecApptAnyStaffToggleSwitch.Checked = false;
            isappointment = false;
        }

        //ApptMember
        private void RecApptBookingDatePicker_ValueChanged(object sender, EventArgs e)
        {
            LoadBookingTimes();
        }

        //ApptMember
        private void LoadBookingTimes()
        {
            DateTime selectedDate = RecApptBookingDatePicker.Value.Date;
            string selectedDateString = selectedDate.ToString("MM-dd-yyyy dddd");
            string serviceCategory = filterstaffbyservicecategory;

            // Retrieve matching appointment times based on selected date and service category
            List<string> matchingTimes = RetrieveMatchingAppointmentTimes(selectedDateString, serviceCategory);

            // Clear existing items in the ComboBox
            RecApptBookingTimeComboBox.Items.Clear();

            // Check if the selected date is today and if it's past 3 PM
            if (selectedDate == DateTime.Today && DateTime.Now.TimeOfDay > new TimeSpan(15, 0, 0))
            {
                // Add "Cutoff Time" to ComboBox and disable it
                RecApptBookingTimeComboBox.Items.Add("Cutoff Time");
                RecApptBookingTimeComboBox.SelectedIndex = 0;
                RecApptBookingTimeComboBox.Enabled = false;
            }
            else
            {
                // Add regular booking times for the selected date and service category
                foreach (string time in bookingTimes)
                {
                    // Add the time to the ComboBox
                    RecApptBookingTimeComboBox.Items.Add(time);
                    RecApptBookingTimeComboBox.SelectedIndex = 0;

                }

                // Remove booked times beyond the limit
                Dictionary<string, int> timeCount = new Dictionary<string, int>();
                foreach (string time in matchingTimes)
                {
                    if (!timeCount.ContainsKey(time))
                    {
                        timeCount[time] = 0;
                    }
                    timeCount[time]++;
                }

                foreach (var pair in timeCount)
                {
                    if (pair.Value >= 3)
                    {
                        RecApptBookingTimeComboBox.Items.Remove(pair.Key);
                    }
                }

                RecApptBookingTimeComboBox.Enabled = true;
            }
        }

        //ApptMember
        private List<string> RetrieveMatchingAppointmentTimes(string selectedDate, string serviceCategory)
        {
            List<string> matchingTimes = new List<string>();

            string query = "SELECT AppointmentTime FROM servicehistory WHERE AppointmentDate = @SelectedDate AND ServiceCategory = @ServiceCategory AND (QueType = 'AnyonePriority' OR QueType = 'PreferredPriority')";

            using (MySqlConnection connection = new MySqlConnection(mysqlconn))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SelectedDate", selectedDate);
                    command.Parameters.AddWithValue("@ServiceCategory", serviceCategory);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string appointmentTime = reader.GetString("AppointmentTime");
                            matchingTimes.Add(appointmentTime);
                        }
                    }
                }
            }
            return matchingTimes;
        }



        public void InitializeAppointmentDataGrid()
        {
            using (MySqlConnection connection = new MySqlConnection(mysqlconn))
            {
                connection.Open();

                string currentDate = DateTime.Now.ToString("MM-dd-yyyy");

                string query = "SELECT a.TransactionNumber AS TransactionID, a.AppointmentDate, GROUP_CONCAT(DISTINCT sh.AppointmentTime SEPARATOR ', ') AS AppointmentTime " +
                               "FROM appointment a " +
                               "LEFT JOIN servicehistory sh ON a.TransactionNumber = sh.TransactionNumber " +
                               "WHERE a.ServiceStatus = 'Pending' AND a.AppointmentStatus = 'Unconfirmed' AND " +
                               "STR_TO_DATE(a.AppointmentDate, '%m-%d-%Y') >= STR_TO_DATE(@currentDate, '%m-%d-%Y') " +
                               "GROUP BY a.TransactionNumber, a.AppointmentDate";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@currentDate", currentDate);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        RecApptAcceptLateDeclineDGV.Rows.Add(row["TransactionID"], row["AppointmentDate"], row["AppointmentTime"]);
                    }
                }
            }
        }

        private void RecAcceptApptTransactionBtn_Click(object sender, EventArgs e)
        {
            if (RecApptAcceptLateDeclineDGV.SelectedRows.Count > 0)
            {
                string transactionID = RecApptAcceptLateDeclineDGV.SelectedRows[0].Cells["TransactionID"].Value.ToString();

                DateTime currentDate = DateTime.Now;

                string appointmentTime = string.Empty;
                string serviceCategory = string.Empty;
                string queType = string.Empty;

                using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                {
                    connection.Open();

                    string query = $"SELECT AppointmentTime, ServiceCategory, QueType FROM servicehistory WHERE TransactionNumber = '{transactionID}' AND ServiceStatus = 'Pending'";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        appointmentTime = reader["AppointmentTime"].ToString();
                        serviceCategory = reader["ServiceCategory"].ToString();
                        queType = reader["QueType"].ToString();
                    }

                    reader.Close();

                    if (!string.IsNullOrEmpty(appointmentTime))
                    {
                        DateTime appointmentDateTime;

                        if (DateTime.TryParse(appointmentTime, out appointmentDateTime))
                        {
                            if (appointmentDateTime < currentDate)
                            {
                                if (queType == "AnyonePriority")
                                {
                                    string updateQuery = $"UPDATE servicehistory SET QueType = 'Anyone' WHERE TransactionNumber = '{transactionID}' AND ServiceStatus = 'Pending'";
                                    ExecuteQuery(updateQuery);
                                }
                                else if (queType == "PreferredPriority")
                                {
                                    string updateQuery = $"UPDATE servicehistory SET QueType = 'Preferred' WHERE TransactionNumber= '{transactionID}' AND ServiceStatus = 'Pending'";
                                    ExecuteQuery(updateQuery);
                                }

                                int queNumber = GetLargestQueNumberFromDatabase(serviceCategory);
                                queNumber++;
                                string updateQueNumberQuery = $"UPDATE servicehistory SET QueNumber = {queNumber} WHERE TransactionNumber = '{transactionID}' AND ServiceStatus = 'Pending' AND ServiceCategory = '{serviceCategory}'";
                                ExecuteQuery(updateQueNumberQuery);

                                // Ask for confirmation before confirming the appointment
                                DialogResult result = MessageBox.Show("Are you sure you want to confirm the appointment?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                                if (result == DialogResult.Yes)
                                {
                                    // User confirmed, proceed with appointment confirmation
                                    string updateAppointmentStatusQuery = $"UPDATE appointment SET AppointmentStatus = 'Confirmed' WHERE TransactionNumber = '{transactionID}'";
                                    ExecuteQuery(updateAppointmentStatusQuery);

                                    MessageBox.Show("Appointment Confirmed");

                                    RecApptAcceptLateDeclineDGV.Rows.Clear();
                                    InitializeAppointmentDataGrid();
                                }
                                else
                                {
                                    // User cancelled the operation
                                    MessageBox.Show("Appointment confirmation cancelled");
                                }
                            }
                            else
                            {
                                int queNumber = GetLargestQueNumberFromDatabase(serviceCategory);
                                queNumber++;
                                string updateQueNumberQuery = $"UPDATE servicehistory SET QueNumber = {queNumber} WHERE TransactionNumber = '{transactionID}' AND ServiceStatus = 'Pending' AND ServiceCategory = '{serviceCategory}'";
                                ExecuteQuery(updateQueNumberQuery);

                                // Ask for confirmation before confirming the appointment
                                DialogResult result = MessageBox.Show("Are you sure you want to confirm the appointment?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                                if (result == DialogResult.Yes)
                                {
                                    // User confirmed, proceed with appointment confirmation
                                    string updateAppointmentStatusQuery = $"UPDATE appointment SET AppointmentStatus = 'Confirmed' WHERE TransactionNumber = '{transactionID}'";
                                    ExecuteQuery(updateAppointmentStatusQuery);

                                    MessageBox.Show("Appointment Confirmed");

                                    RecApptAcceptLateDeclineDGV.Rows.Clear();
                                    InitializeAppointmentDataGrid();
                                }
                                else
                                {
                                    // User cancelled the operation
                                    MessageBox.Show("Appointment confirmation cancelled by user.");
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a transaction number.");
            }
        }


        private int GetLargestQueNumberFromDatabase(string serviceCategory)
        {
            int largestQueNumber = 0;

            using (MySqlConnection connection = new MySqlConnection(mysqlconn))
            {
                connection.Open();

                string query = $"SELECT MAX(QueNumber) FROM servicehistory WHERE ServiceCategory = '{serviceCategory}'";
                MySqlCommand command = new MySqlCommand(query, connection);
                object result = command.ExecuteScalar();

                if (result != null && !DBNull.Value.Equals(result))
                {
                    int.TryParse(result.ToString(), out largestQueNumber);
                }
            }

            return largestQueNumber;
        }

        private void ExecuteQuery(string query)
        {
            using (MySqlConnection connection = new MySqlConnection(mysqlconn))
            {
                connection.Open();

                MySqlCommand command = new MySqlCommand(query, connection);
                command.ExecuteNonQuery();
            }
        }





        #endregion

        #region Reception Walk-in Shop
        private void RecAppointmentExitBtn_Click(object sender, EventArgs e)
        {

        }
        private void RecWalkinSelectedServiceDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //di ko alam kung ituloy ko pa
            //selected discount per service itey
            //walkin itey pang discount ng selected service
        }
        private void RecApptConfirmBtn_Click(object sender, EventArgs e)
        {
            Transaction.PanelShow(RecApptConfirmPanel);
            RecApptAcceptLateDeclineDGV.Rows.Clear();
            InitializeAppointmentDataGrid();
        }
        private void RecApptConfirmExitBtn_Click(object sender, EventArgs e)
        {
            Transaction.PanelShow(RecTransactionPanel);

        }
        private void RecShopProdBtn_Click(object sender, EventArgs e)
        {
            Transaction.PanelShow(RecShopProdPanel);
            RecShopProdTransNumText.Text = TransactionNumberGenerator.ShopProdGenerateTransNumberDefault();

        }

        private void RecShopProdExitBtn_Click(object sender, EventArgs e)
        {
            Transaction.PanelShow(RecTransactionPanel);

        }
        private void RecShopProdTransactNumRefresh()
        {
            RecShopProdTransNumText.Text = TransactionNumberGenerator.ShopProdGenerateTransNumberInc();
        }

        private void RecShopProdSelectedProdDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0 && RecShopProdSelectedProdDGV.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
                {
                    DataGridView dgv = (DataGridView)sender;

                    if (dgv.Columns[e.ColumnIndex] is DataGridViewButtonColumn)
                    {
                        if (RecShopProdSelectedProdDGV.Columns[e.ColumnIndex].Name == "Void")
                        {
                            //input dialog messagebox
                            string enteredPassword = GetPasswordWithAsterisks("Enter Manager Password:", "Void Product Permission");

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
                                                    RecShopProdSelectedProdDGV.Rows.RemoveAt(e.RowIndex);
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
                        else if (RecShopProdSelectedProdDGV.Columns[e.ColumnIndex].Name == "-")
                        {
                            string quantityString = RecShopProdSelectedProdDGV.Rows[e.RowIndex].Cells["Qty"].Value?.ToString();
                            if (!string.IsNullOrEmpty(quantityString) && int.TryParse(quantityString, out int quantity))
                            {
                                decimal itemCost = decimal.Parse(RecShopProdSelectedProdDGV.Rows[e.RowIndex].Cells["Total Price"].Value?.ToString());

                                // Calculate the cost per item
                                decimal costPerItem = itemCost / quantity;

                                // Decrease quantity
                                if (quantity > 1)
                                {
                                    quantity--;

                                    // Calculate updated item cost (reset to original price)
                                    decimal updatedCost = costPerItem * quantity;

                                    // Update Qty and ItemCost in the DataGridView
                                    RecShopProdSelectedProdDGV.Rows[e.RowIndex].Cells["Qty"].Value = quantity.ToString();
                                    RecShopProdSelectedProdDGV.Rows[e.RowIndex].Cells["Total Price"].Value = updatedCost.ToString("F2"); // Format to two decimal places

                                }
                            }
                            else
                            {
                                // Handle the case where quantityString is empty or not a valid integer
                                // For example, show an error message or set a default value
                            }
                        }
                        else if (RecShopProdSelectedProdDGV.Columns[e.ColumnIndex].Name == "+")
                        {
                            string quantityString = RecShopProdSelectedProdDGV.Rows[e.RowIndex].Cells["Qty"].Value?.ToString();
                            if (!string.IsNullOrEmpty(quantityString) && int.TryParse(quantityString, out int quantity))
                            {
                                decimal itemCost = decimal.Parse(RecShopProdSelectedProdDGV.Rows[e.RowIndex].Cells["Total Price"].Value?.ToString());

                                // Calculate the cost per item
                                decimal costPerItem = itemCost / quantity;

                                // Increase quantity
                                quantity++;

                                // Calculate updated item cost
                                decimal updatedCost = costPerItem * quantity;

                                // Update Qty and ItemCost in the DataGridView
                                RecShopProdSelectedProdDGV.Rows[e.RowIndex].Cells["Qty"].Value = quantity.ToString();
                                RecShopProdSelectedProdDGV.Rows[e.RowIndex].Cells["Total Price"].Value = updatedCost.ToString("F2"); // Format to two decimal places

                            }
                            else
                            {
                                // Handle the case where quantityString is empty or not a valid integer
                                // For example, show an error message or set a default value
                            }
                        }
                    }

                    else
                    {

                    }
                }
                else if (RecShopProdSelectedProdDGV.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn &&
                RecShopProdSelectedProdDGV.Columns[e.ColumnIndex].Name == "CheckBoxColumn")
                {
                    // Dictionary to store the discounted amounts for each row
                    Dictionary<int, decimal> discountedAmounts = new Dictionary<int, decimal>();
                    // Get the checkbox cell value
                    DataGridViewCheckBoxCell cell = RecShopProdSelectedProdDGV[e.ColumnIndex, e.RowIndex] as DataGridViewCheckBoxCell;
                    RecShopProdSelectedProdDGV.CurrentCell = null;
                    // Check if the checkbox is checked
                    bool isChecked = (bool)cell.Value;

                    // Calculate total amount and apply discount based on checkbox state
                    if (isChecked)
                    {
                        // Get the quantity and amount from the corresponding cells
                        int quantity = Convert.ToInt32(RecShopProdSelectedProdDGV.Rows[e.RowIndex].Cells["Qty"].Value);
                        decimal amount = Convert.ToDecimal(RecShopProdSelectedProdDGV.Rows[e.RowIndex].Cells["Unit Price"].Value);

                        // Calculate the total amount
                        decimal total = quantity * amount;

                        // Apply discount (for example, 20% discount)
                        decimal discount = 0.2m; // 20% discount
                        decimal discountedTotal = total * (1 - discount);

                        // Add or update the discounted amount in the dictionary
                        RecShopProdSelectedProdDGV.Rows[e.RowIndex].Cells["RecShopProdDiscountAmount"].Value = total - discountedTotal;

                        // Update the total cell with the discounted total
                        RecShopProdSelectedProdDGV.Rows[e.RowIndex].Cells["Total Price"].Value = discountedTotal.ToString();
                        RecShopProdSelectedDiscount();

                    }
                    else
                    {
                        // Clear the discounted amount and update the amount cell with the original value
                        RecShopProdSelectedProdDGV.Rows[e.RowIndex].Cells["Total Price"].Value = (Convert.ToDecimal(RecShopProdSelectedProdDGV.Rows[e.RowIndex].Cells["Unit Price"].Value) * Convert.ToInt32(RecShopProdSelectedProdDGV.Rows[e.RowIndex].Cells["Qty"].Value)).ToString();
                        RecShopProdSelectedProdDGV.Rows[e.RowIndex].Cells["RecShopProdDiscountAmount"].Value = "0.00";
                        RecShopProdSelectedDiscount();

                        //int discountpriceColumnIndex = RecShopProdSelectedProdDGV.Columns["CheckBoxColumn"].Index;

                        //foreach (DataGridViewRow row in RecShopProdSelectedProdDGV.Rows)
                        //{
                        //    if (row.Cells[discountpriceColumnIndex].Value == null)
                        //    {
                        //        RecShopProdCalculateTotalPrice();
                        //    }
                        //}
                    }



                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "ShopProdSelectedDGV Cell Content Click Error");
            }
        }

        private void RecShopProdSelectedDiscount()
        {
            decimal totalDiscountedAmount = 0;
            decimal total2 = 0;

            decimal price1;
            decimal price2;

            int discountpriceColumnIndex = RecShopProdSelectedProdDGV.Columns["RecShopProdDiscountAmount"].Index;
            int totalpriceColumnIndex = RecShopProdSelectedProdDGV.Columns["Total Price"].Index;

            foreach (DataGridViewRow row in RecShopProdSelectedProdDGV.Rows)
            {
                if (row.Cells[discountpriceColumnIndex].Value != null)
                {
                    if (decimal.TryParse(row.Cells[discountpriceColumnIndex].Value.ToString(), out price1))
                    {
                        totalDiscountedAmount += price1;
                    }
                    else
                    {
                        // Handle invalid numeric value
                        // For example, you can skip this row or display an error message
                    }
                }

            }
            foreach (DataGridViewRow row in RecShopProdSelectedProdDGV.Rows)
            {
                if (row.Cells[totalpriceColumnIndex].Value != null)
                {
                    if (decimal.TryParse(row.Cells[totalpriceColumnIndex].Value.ToString(), out price2))
                    {
                        total2 += price2;
                    }
                    else
                    {
                        // Handle invalid numeric value
                        // For example, you can skip this row or display an error message
                    }
                }
            }
            RecShopProdGrossAmountBox.Text = total2.ToString("0.00");
            RecShopProdDiscountBox.Text = totalDiscountedAmount.ToString("0.00");

            if (decimal.TryParse(RecShopProdGrossAmountBox.Text, out decimal grossAmount))
            {
                originalGrossAmount = grossAmount; // Store the original value
                decimal discountAmount = Convert.ToDecimal(RecShopProdDiscountBox.Text); // Calculate the discount amount
                decimal vatAmount = 0;
                RecShopProdNetAmountBox.Text = grossAmount.ToString("0.00"); // Format to display as currency
                RecShopProdVATBox.Text = vatAmount.ToString("0.00");

            }

        }

        private void RecShopProdCalculateTotalPrice()
        {
            decimal total1 = 0;

            int servicepriceColumnIndex = RecShopProdSelectedProdDGV.Columns["Total Price"].Index;

            foreach (DataGridViewRow row in RecShopProdSelectedProdDGV.Rows)
            {
                if (row.Cells[servicepriceColumnIndex].Value != null)
                {
                    decimal price;
                    if (decimal.TryParse(row.Cells[servicepriceColumnIndex].Value.ToString(), out price))
                    {
                        total1 += price;
                    }
                    else
                    {
                        // Handle invalid numeric value
                        // For example, you can skip this row or display an error message
                    }
                }
            }
            RecShopProdGrossAmountBox.Text = total1.ToString("F2");

            RecShopProdCalculateVATAndNetAmount();
        }

        public void RecShopProdCalculateVATAndNetAmount()
        {
            // Get the Gross Amount from the TextBox (MngrGrossAmountBox)
            if (decimal.TryParse(RecShopProdGrossAmountBox.Text, out decimal grossAmount))
            {
                // Fixed VAT rate of 12%
                decimal rate = 12;

                // Calculate the VAT Amount
                decimal netAmount = grossAmount / ((rate / 100) + 1);

                // Calculate the Net Amount 
                decimal vatAmount = grossAmount - netAmount;

                // Display the calculated values in TextBoxes
                RecShopProdVATBox.Text = vatAmount.ToString("0.00");
                RecShopProdNetAmountBox.Text = netAmount.ToString("0.00");
                RecShopProdVATBox.Text = vatAmount.ToString("0.00");
                RecShopProdNetAmountBox.Text = netAmount.ToString("0.00");
            }

        }
        private void RecShopProdCashPaymentChk_CheckedChanged(object sender, EventArgs e)
        {
            if (RecShopProdCashPaymentChk.Checked)
            {
                RecShopProdCashPaymentChk.Checked = true;
                RecShopProdTypeText.Text = "Cash";

                RecShopProdCashLbl.Visible = true;
                RecShopProdCashBox.Visible = true;
                RecShopProdChangeLbl.Visible = true;
                RecShopProdChangeBox.Visible = true;

                //disable other payment panel
                RecShopProdBankPaymentPanel.Visible = false;
                RecShopProdWalletPaymentPanel.Visible = false;

                RecShopProdCCPaymentChk.Checked = false;
                RecShopProdPPPaymentChk.Checked = false;
                RecShopProdGCPaymentChk.Checked = false;
                RecShopProdPMPaymentChk.Checked = false;

                RecShopProdCardNameText.Text = "";
                RecShopProdCardNumText.Text = "";
                RecShopProdCVCText.Text = "";
                RecShopProdCardExpText.Text = "MM/YY";
                RecShopProdWalletNumText.Text = "";
                RecShopProdWalletPINText.Text = "";
                RecShopProdWalletOTPText.Text = "";
                RecShopProdCashBox.Text = "0";
                RecShopProdChangeBox.Text = "0.00";
            }
            else
            {
                RecShopProdCashPaymentChk.Checked = false;
                RecShopProdCashLbl.Visible = false;
                RecShopProdCashBox.Visible = false;
                RecShopProdChangeLbl.Visible = false;
                RecShopProdChangeBox.Visible = false;
            }
        }

        private void RecShopProdCCPaymentChk_CheckedChanged(object sender, EventArgs e)
        {
            if (RecShopProdCCPaymentChk.Checked)
            {
                RecShopProdCCPaymentChk.Checked = true;
                RecShopProdTypeText.Text = "Credit Card";

                RecShopProdCashLbl.Visible = false;
                RecShopProdCashBox.Visible = false;
                RecShopProdChangeLbl.Visible = false;
                RecShopProdChangeBox.Visible = false;

                //disable other payment panel
                RecShopProdBankPaymentPanel.Visible = true;
                RecShopProdWalletPaymentPanel.Visible = false;

                RecShopProdCashPaymentChk.Checked = false;
                RecShopProdPPPaymentChk.Checked = false;
                RecShopProdGCPaymentChk.Checked = false;
                RecShopProdPMPaymentChk.Checked = false;

                RecShopProdCardNameText.Text = "";
                RecShopProdCardNumText.Text = "";
                RecShopProdCVCText.Text = "";
                RecShopProdCardExpText.Text = "MM/YY";
                RecShopProdWalletNumText.Text = "";
                RecShopProdWalletPINText.Text = "";
                RecShopProdWalletOTPText.Text = "";
                RecShopProdCashBox.Text = "0";
                RecShopProdChangeBox.Text = "0.00";
            }
            else if (RecShopProdCCPaymentChk.Checked || RecShopProdPPPaymentChk.Checked)
            {
                RecShopProdBankPaymentPanel.Visible = true;
                RecShopProdWalletPaymentPanel.Visible = false;

            }
            else
            {
                RecShopProdCCPaymentChk.Checked = false;
                RecShopProdBankPaymentPanel.Visible = false;
                RecShopProdCardNameText.Text = "";
                RecShopProdCardNumText.Text = "";
                RecShopProdCVCText.Text = "";
                RecShopProdCardExpText.Text = "MM/YY";
            }

        }

        private void RecShopProdPPPaymentChk_CheckedChanged(object sender, EventArgs e)
        {
            if (RecShopProdPPPaymentChk.Checked)
            {
                RecShopProdPPPaymentChk.Checked = true;
                RecShopProdTypeText.Text = "Paypal";

                RecShopProdCashLbl.Visible = false;
                RecShopProdCashBox.Visible = false;
                RecShopProdChangeLbl.Visible = false;
                RecShopProdChangeBox.Visible = false;

                //disable other payment panel
                RecShopProdBankPaymentPanel.Visible = true;
                RecShopProdWalletPaymentPanel.Visible = false;

                RecShopProdCashPaymentChk.Checked = false;
                RecShopProdCCPaymentChk.Checked = false;
                RecShopProdGCPaymentChk.Checked = false;
                RecShopProdPMPaymentChk.Checked = false;

                RecShopProdCardNameText.Text = "";
                RecShopProdCardNumText.Text = "";
                RecShopProdCVCText.Text = "";
                RecShopProdCardExpText.Text = "MM/YY";
                RecShopProdWalletNumText.Text = "";
                RecShopProdWalletPINText.Text = "";
                RecShopProdWalletOTPText.Text = "";
                RecShopProdCashBox.Text = "0";
                RecShopProdChangeBox.Text = "0.00";
            }
            else if (RecShopProdCCPaymentChk.Checked || RecShopProdPPPaymentChk.Checked)
            {
                RecShopProdBankPaymentPanel.Visible = true;
                RecShopProdWalletPaymentPanel.Visible = false;

            }
            else
            {
                RecShopProdPPPaymentChk.Checked = false;
                RecShopProdBankPaymentPanel.Visible = false;
                RecShopProdCardNameText.Text = "";
                RecShopProdCardNumText.Text = "";
                RecShopProdCVCText.Text = "";
                RecShopProdCardExpText.Text = "MM/YY";
            }

        }

        private void RecShopProdGCPaymentChk_CheckedChanged(object sender, EventArgs e)
        {
            if (RecShopProdGCPaymentChk.Checked)
            {
                RecShopProdGCPaymentChk.Checked = true;
                RecShopProdTypeText.Text = "GCash";

                RecShopProdCashLbl.Visible = false;
                RecShopProdCashBox.Visible = false;
                RecShopProdChangeLbl.Visible = false;
                RecShopProdChangeBox.Visible = false;

                //disable other payment panel
                RecShopProdBankPaymentPanel.Visible = false;
                RecShopProdWalletPaymentPanel.Visible = true;

                RecShopProdCashPaymentChk.Checked = false;
                RecShopProdCCPaymentChk.Checked = false;
                RecShopProdPPPaymentChk.Checked = false;
                RecShopProdPMPaymentChk.Checked = false;

                RecShopProdCardNameText.Text = "";
                RecShopProdCardNumText.Text = "";
                RecShopProdCVCText.Text = "";
                RecShopProdCardExpText.Text = "MM/YY";
                RecShopProdWalletNumText.Text = "";
                RecShopProdWalletPINText.Text = "";
                RecShopProdWalletOTPText.Text = "";
                RecShopProdCashBox.Text = "0";
                RecShopProdChangeBox.Text = "0.00";
            }
            else if (RecShopProdGCPaymentChk.Checked || RecShopProdPMPaymentChk.Checked)
            {
                RecShopProdBankPaymentPanel.Visible = false;
                RecShopProdWalletPaymentPanel.Visible = true;

            }
            else
            {
                RecShopProdGCPaymentChk.Checked = false;
                RecShopProdWalletPaymentPanel.Visible = false;
                RecShopProdWalletNumText.Text = "";
                RecShopProdWalletPINText.Text = "";
                RecShopProdWalletOTPText.Text = "";
            }
        }

        private void RecShopProdPMPaymentChk_CheckedChanged(object sender, EventArgs e)
        {
            if (RecShopProdPMPaymentChk.Checked)
            {
                RecShopProdPMPaymentChk.Checked = true;
                RecShopProdTypeText.Text = "Paymaya";

                RecShopProdCashLbl.Visible = false;
                RecShopProdCashBox.Visible = false;
                RecShopProdChangeLbl.Visible = false;
                RecShopProdChangeBox.Visible = false;

                //disable other payment panel
                RecShopProdBankPaymentPanel.Visible = false;
                RecShopProdWalletPaymentPanel.Visible = true;

                RecShopProdCashPaymentChk.Checked = false;
                RecShopProdCCPaymentChk.Checked = false;
                RecShopProdPPPaymentChk.Checked = false;
                RecShopProdGCPaymentChk.Checked = false;

                RecShopProdCardNameText.Text = "";
                RecShopProdCardNumText.Text = "";
                RecShopProdCVCText.Text = "";
                RecShopProdCardExpText.Text = "MM/YY";
                RecShopProdWalletNumText.Text = "";
                RecShopProdWalletPINText.Text = "";
                RecShopProdWalletOTPText.Text = "";
                RecShopProdCashBox.Text = "0";
                RecShopProdChangeBox.Text = "0.00";
            }
            else if (RecShopProdGCPaymentChk.Checked || RecShopProdPMPaymentChk.Checked)
            {
                RecShopProdBankPaymentPanel.Visible = false;
                RecShopProdWalletPaymentPanel.Visible = true;

            }
            else
            {
                RecShopProdPMPaymentChk.Checked = false;
                RecShopProdWalletPaymentPanel.Visible = false;
                RecShopProdWalletNumText.Text = "";
                RecShopProdWalletPINText.Text = "";
                RecShopProdWalletOTPText.Text = "";
            }
        }

        private void RecShopProdDiscountPWD_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void RecShopProdVATExemptChk_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void RecShopProdPaymentButton_Click(object sender, EventArgs e)
        {
            if (!RecShopProdCashPaymentChk.Checked &&
                !RecShopProdCCPaymentChk.Checked &&
                !RecShopProdPPPaymentChk.Checked &&
                !RecShopProdGCPaymentChk.Checked &&
                !RecShopProdPMPaymentChk.Checked)
            {
                MessageBox.Show("Please select a payment method.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (RecShopProdInsertOrderDB())
            {
                RecShopProdUpdateQtyInventory(RecShopProdSelectedProdDGV);
                RecShopProdOrderProdHistoryDB(RecShopProdSelectedProdDGV);
                RecShopProdInvoiceReceiptGenerator();
                RecShopProdClearAllField();
                Transaction.PanelShow(RecTransactionPanel);
            }
        }
        private void RecShopProdClearAllField()
        {

            RecShopProdNetAmountBox.Text = "0.00";
            RecShopProdVATBox.Text = "0.00";
            RecShopProdDiscountBox.Text = "0.00";
            RecShopProdGrossAmountBox.Text = "0.00";
            RecShopProdCashBox.Text = "0";
            RecShopProdChangeBox.Text = "0.00";
            RecShopProdTypeText.Text = "";

            RecShopProdCardNameText.Text = "";
            RecShopProdCardNumText.Text = "";
            RecShopProdCVCText.Text = "";
            RecShopProdCardExpText.Text = "MM/YY";
            RecShopProdWalletNumText.Text = "";
            RecShopProdWalletPINText.Text = "";
            RecShopProdWalletOTPText.Text = "";
            RecShopProdSelectedProdDGV.Rows.Clear();
            RecShopProdClientNameText.Text = "";
            RecShopProdClientCPNumText.Text = "";

            RecShopProdCashPaymentChk.Checked = false;
            RecShopProdCCPaymentChk.Checked = false;
            RecShopProdPPPaymentChk.Checked = false;
            RecShopProdGCPaymentChk.Checked = false;
            RecShopProdPMPaymentChk.Checked = false;

        }
        private void RecShopProdUpdateQtyInventory(DataGridView dgv)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                {
                    connection.Open();
                    string updateQuery = "UPDATE inventory SET ItemStock = ItemStock - @Qty WHERE ItemID = @ItemID";

                    foreach (DataGridViewRow row in RecShopProdSelectedProdDGV.Rows)
                    {
                        string itemID = row.Cells["RecShopProdItemID"].Value.ToString();
                        int qty = Convert.ToInt32(row.Cells["Qty"].Value);

                        MySqlCommand command = new MySqlCommand(updateQuery, connection);
                        command.Parameters.AddWithValue("@Qty", qty);
                        command.Parameters.AddWithValue("@ItemID", itemID);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                string errorMessage = "An error occurred: " + ex.Message + "\n\n" + ex.StackTrace;
                MessageBox.Show(errorMessage, "Product Qty Failed Inserting to Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        private bool RecShopProdInsertOrderDB()
        {
            DateTime currentDate = RecDateTimePicker.Value;
            string clientName = RecShopProdClientNameText.Text;
            string clientCPNum = RecShopProdClientCPNumText.Text;

            // cash values
            string netAmount = RecShopProdNetAmountBox.Text; // net amount
            string vat = RecShopProdVATBox.Text; // vat 
            string discount = RecShopProdDiscountBox.Text; // discount
            string grossAmount = RecShopProdGrossAmountBox.Text; // gross amount
            string cash = RecShopProdCashBox.Text; // cash given
            string change = RecShopProdChangeBox.Text; // due change
            string paymentMethod = RecShopProdTypeText.Text; // payment method
            string rec = RecNameLbl.Text;
            string transactNum = RecShopProdTransNumText.Text;
            //booked values
            string Date = currentDate.ToString("MM-dd-yyyy dddd"); //bookedDate
            string Time = currentDate.ToString("hh:mm tt"); //bookedTime
            // bank & wallet details
            string cardName = RecShopProdCardNameText.Text;
            string cardNum = RecShopProdCardNumText.Text;
            string CVC = RecShopProdCVCText.Text;
            string expire = RecShopProdCardExpText.Text;
            string walletNum = RecShopProdWalletNumText.Text;
            string walletPIN = RecShopProdWalletPINText.Text;
            string walletOTP = RecShopProdWalletOTPText.Text;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                {
                    connection.Open();


                    if (RecShopProdCashPaymentChk.Checked)
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
                    else if (RecShopProdCCPaymentChk.Checked || RecShopProdPPPaymentChk.Checked)
                    {
                        if (grossAmount == "0.00")
                        {
                            MessageBox.Show("Please select a transaction to pay.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        else if (string.IsNullOrWhiteSpace(RecShopProdCardNameText.Text))
                        {
                            MessageBox.Show("Please enter a cardholder name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        else if (!IsCardNameValid(RecShopProdCardNameText.Text))
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

                    else if (RecShopProdGCPaymentChk.Checked || RecShopProdPMPaymentChk.Checked)
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
                    string cashPayment = "INSERT INTO orders (TransactionNumber, TransactionType, ProductStatus, Date, Time, CheckedOutBy, ClientName, ClientCPNum, NetPrice, VatAmount, DiscountAmount, GrossAmount, CashGiven, DueChange, PaymentMethod) " +
                                        "VALUES (@transactNum, @transactType, @status, @date, @time, @rec, @name, @cpNum, @net, @vat, @discount, @gross, @cash, @change, @payment)";


                    string bankPayment = "INSERT INTO orders (TransactionNumber, TransactionType, ProductStatus, Date, Time, CheckedOutBy, ClientName, ClientCPNum, NetPrice, VatAmount, DiscountAmount, GrossAmount, PaymentMethod, CardName, CardNumber, CVC, CardExpiration) " +
                                        "VALUES (@transactNum, @transactType, @status, @date, @time, @rec, @name, @cpNum, @net, @vat, @discount, @gross, @payment, @cardname, @cardNum, @cvc, @expiration)";


                    string walletPayment = "INSERT INTO orders (TransactionNumber, TransactionType, ProductStatus, Date, Time, CheckedOutBy, ClientName, ClientCPNum, NetPrice, VatAmount, DiscountAmount, GrossAmount, PaymentMethod, WalletNumber, WalletPIN, WalletOTP) " +
                                        "VALUES (@transactNum, @transactType, @status, @date, @time, @rec, @name, @cpNum, @net, @vat, @discount, @gross, @payment, @walletNum, @walletPin, @walletOTP)";

                    if (RecShopProdCashPaymentChk.Checked == true)
                    {
                        MySqlCommand cmd = new MySqlCommand(cashPayment, connection);
                        cmd.Parameters.AddWithValue("@transactNum", transactNum);
                        cmd.Parameters.AddWithValue("@transactType", "Walk-in Checked Out");
                        cmd.Parameters.AddWithValue("@status", "Paid");
                        cmd.Parameters.AddWithValue("@date", Date);
                        cmd.Parameters.AddWithValue("@time", Time);
                        cmd.Parameters.AddWithValue("@rec", rec);
                        cmd.Parameters.AddWithValue("@name", clientName);
                        cmd.Parameters.AddWithValue("@cpNum", clientCPNum);
                        cmd.Parameters.AddWithValue("@net", netAmount);
                        cmd.Parameters.AddWithValue("@vat", vat);
                        cmd.Parameters.AddWithValue("@discount", discount);
                        cmd.Parameters.AddWithValue("@gross", grossAmount);
                        cmd.Parameters.AddWithValue("@cash", cash);
                        cmd.Parameters.AddWithValue("@change", change);
                        cmd.Parameters.AddWithValue("@payment", paymentMethod);

                        cmd.ExecuteNonQuery();
                        // Successful update
                        MessageBox.Show("Service successfully been paid through cash.", "Hooray!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (RecShopProdCCPaymentChk.Checked == true || RecShopProdPPPaymentChk.Checked == true)
                    {
                        MySqlCommand cmd = new MySqlCommand(bankPayment, connection);
                        cmd.Parameters.AddWithValue("@transactNum", transactNum);
                        cmd.Parameters.AddWithValue("@transactType", "Walk-in Checked Out");
                        cmd.Parameters.AddWithValue("@status", "Paid");
                        cmd.Parameters.AddWithValue("@date", Date);
                        cmd.Parameters.AddWithValue("@time", Time);
                        cmd.Parameters.AddWithValue("@rec", rec);
                        cmd.Parameters.AddWithValue("@name", clientName);
                        cmd.Parameters.AddWithValue("@cpNum", clientCPNum);
                        cmd.Parameters.AddWithValue("@net", netAmount);
                        cmd.Parameters.AddWithValue("@vat", vat);
                        cmd.Parameters.AddWithValue("@discount", discount);
                        cmd.Parameters.AddWithValue("@gross", grossAmount);
                        cmd.Parameters.AddWithValue("@payment", paymentMethod);
                        cmd.Parameters.AddWithValue("@cardname", cardName);
                        cmd.Parameters.AddWithValue("@cardNum", cardNum);
                        cmd.Parameters.AddWithValue("@cvc", CVC);
                        cmd.Parameters.AddWithValue("@expiration", expire);

                        cmd.ExecuteNonQuery();
                        // Successful update
                        MessageBox.Show("Service successfully been paid through bank.", "Hooray!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (RecShopProdGCPaymentChk.Checked == true || RecShopProdPMPaymentChk.Checked == true)
                    {
                        MySqlCommand cmd = new MySqlCommand(walletPayment, connection);
                        cmd.Parameters.AddWithValue("@transactNum", transactNum);
                        cmd.Parameters.AddWithValue("@transactType", "Walk-in Checked Out");
                        cmd.Parameters.AddWithValue("@status", "Paid");
                        cmd.Parameters.AddWithValue("@date", Date);
                        cmd.Parameters.AddWithValue("@time", Time);
                        cmd.Parameters.AddWithValue("@rec", rec);
                        cmd.Parameters.AddWithValue("@name", clientName);
                        cmd.Parameters.AddWithValue("@cpNum", clientCPNum);
                        cmd.Parameters.AddWithValue("@net", netAmount);
                        cmd.Parameters.AddWithValue("@vat", vat);
                        cmd.Parameters.AddWithValue("@discount", discount);
                        cmd.Parameters.AddWithValue("@gross", grossAmount);
                        cmd.Parameters.AddWithValue("@payment", paymentMethod);
                        cmd.Parameters.AddWithValue("@walletNum", walletNum);
                        cmd.Parameters.AddWithValue("@walletPin", walletPIN);
                        cmd.Parameters.AddWithValue("@walletOTP", walletOTP);

                        cmd.ExecuteNonQuery();
                        // Successful update
                        MessageBox.Show("Service successfully been paid through online wallet.", "Hooray!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
            }
            catch (MySqlException ex)
            {
                // Handle MySQL database exception
                string errorMessage = "An error occurred: " + ex.Message + "\n\n" + ex.StackTrace;
                MessageBox.Show("An error occurred: " + errorMessage, "Shop Product Payment Transaction Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false; // Return false in case of an exception
            }
            finally
            {
                // Make sure to close the connection
                connection.Close();
            }
            return true;
        }
        private void RecShopProdOrderProdHistoryDB(DataGridView RecShopProdSelectedProdDGV)
        {
            DateTime currentDate = RecDateTimePicker.Value;
            string transactionNum = RecShopProdTransNumText.Text;
            string status = "Paid";

            //basic info
            string clientName = RecShopProdClientNameText.Text;
            string clientCPNum = RecShopProdClientCPNumText.Text;

            //booked values
            string bookedDate = currentDate.ToString("MM-dd-yyyy dddd"); //bookedDate
            string bookedTime = currentDate.ToString("hh:mm tt"); //bookedTime
            string bookedBy = RecNameLbl.Text; //booked by

            string yes = "Yes";
            string no = "No";
            if (RecShopProdSelectedProdDGV.Rows.Count > 0)
            {
                try
                {
                    using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                    {
                        connection.Open();

                        foreach (DataGridViewRow row in RecShopProdSelectedProdDGV.Rows)
                        {
                            if (row.Cells["Item Name"].Value != null)
                            {
                                string itemName = row.Cells["Item Name"].Value.ToString();
                                int qty = Convert.ToInt32(row.Cells["Qty"].Value);
                                decimal itemPrice = Convert.ToDecimal(row.Cells["Unit Price"].Value);
                                decimal itemTotalPrice = Convert.ToDecimal(row.Cells["Total Price"].Value);
                                string itemID = row.Cells["RecShopProdItemID"].Value.ToString();


                                string query = "INSERT INTO orderproducthistory (TransactionNumber, ProductStatus, CheckedOutDate, CheckedOutTime, CheckedOutBy, ClientName, ItemID, ItemName, Qty, ItemPrice, ItemTotalPrice, CheckedOut, Voided) " +
                                                 "VALUES (@Transact, @status, @date, @time, @OrderedBy, @client, @ID, @ItemName, @Qty, @ItemPrice, @ItemTotalPrice, @Yes, @No)";

                                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                                {
                                    cmd.Parameters.AddWithValue("@Transact", transactionNum);
                                    cmd.Parameters.AddWithValue("@status", status);
                                    cmd.Parameters.AddWithValue("@date", bookedDate);
                                    cmd.Parameters.AddWithValue("@time", bookedTime);
                                    cmd.Parameters.AddWithValue("@OrderedBy", bookedBy);
                                    cmd.Parameters.AddWithValue("@client", clientName);
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
                    MessageBox.Show(errorMessage, "Product Data Failed Inserting to Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private void RecShopProdInvoiceReceiptGenerator()
        {
            DateTime currentDate = RecDateTimePicker.Value;
            string datetoday = currentDate.ToString("MM-dd-yyyy dddd");
            string timePrinted = currentDate.ToString("hh:mm tt");
            string timePrintedFile = currentDate.ToString("hh-mm-ss");
            string transactNum = RecShopProdTransNumText.Text;
            string clientName = RecShopProdClientNameText.Text;
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
                    iTextSharp.text.Font italic = FontFactory.GetFont("Courier", 10, iTextSharp.text.Font.ITALIC);

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
                    PdfPTable columnHeaderTable = new PdfPTable(5);
                    columnHeaderTable.SetWidths(new float[] { 10f, 10f, 5f, 5f, 5f }); // Column widths
                    columnHeaderTable.DefaultCell.Border = PdfPCell.NO_BORDER;
                    columnHeaderTable.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
                    columnHeaderTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    columnHeaderTable.AddCell(new Phrase("Product ID", boldfont));
                    columnHeaderTable.AddCell(new Phrase("Product Name", boldfont));
                    columnHeaderTable.AddCell(new Phrase("Qty.", boldfont));
                    columnHeaderTable.AddCell(new Phrase("Unit Price", boldfont));
                    columnHeaderTable.AddCell(new Phrase("Total Price", boldfont));
                    doc.Add(columnHeaderTable);
                    doc.Add(new LineSeparator()); // Dotted line
                    // Iterate through the rows of your 


                    foreach (DataGridViewRow row in RecShopProdSelectedProdDGV.Rows)
                    {
                        try
                        {
                            string itemName = row.Cells["Item Name"].Value?.ToString();
                            if (string.IsNullOrEmpty(itemName))
                            {
                                continue; // Skip empty rows
                            }
                            string itemID = row.Cells["RecShopProdItemID"].Value?.ToString();
                            string qty = row.Cells["Qty"].Value?.ToString();
                            string itemCost = row.Cells["Unit Price"].Value?.ToString();
                            string itemTotalcost = row.Cells["Total Price"].Value?.ToString();

                            // Add cells to the item table
                            PdfPTable productTable = new PdfPTable(5);
                            productTable.SetWidths(new float[] { 5f, 5f, 3f, 3f, 3f }); // Column widths
                            productTable.DefaultCell.Border = PdfPCell.NO_BORDER;
                            productTable.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
                            productTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

                            productTable.AddCell(new Phrase(itemID, font));
                            productTable.AddCell(new Phrase(itemName, font));
                            productTable.AddCell(new Phrase(qty, font));
                            productTable.AddCell(new Phrase(itemCost, font));
                            productTable.AddCell(new Phrase(itemTotalcost, font));

                            // Add the item table to the document
                            doc.Add(productTable);
                        }
                        catch (Exception ex)
                        {
                            // Handle or log any exceptions that occur while processing DataGridView data
                            MessageBox.Show("An error occurred: " + ex.Message, "Shop Product Receipt Generator Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }



                    doc.Add(new Chunk("\n")); // New line
                    doc.Add(new LineSeparator()); // Dotted line
                    doc.Add(new Chunk("\n")); // New line

                    // Total from your textboxes as decimal
                    decimal netAmount = decimal.Parse(RecShopProdNetAmountBox.Text);
                    decimal discount = decimal.Parse(RecShopProdDiscountBox.Text);
                    decimal vat = decimal.Parse(RecShopProdVATBox.Text);
                    decimal grossAmount = decimal.Parse(RecShopProdGrossAmountBox.Text);
                    decimal cash = decimal.Parse(RecShopProdCashBox.Text);
                    decimal change = decimal.Parse(RecShopProdChangeBox.Text);
                    string paymentMethod = RecShopProdTypeText.Text;

                    // Create a new table for the "Total" section
                    PdfPTable totalTable = new PdfPTable(2); // 2 columns for the "Total" table
                    totalTable.SetWidths(new float[] { 5f, 3f }); // Column widths
                    totalTable.DefaultCell.Border = PdfPCell.NO_BORDER;

                    int totalRowCount = RecShopProdSelectedProdDGV.Rows.Count;

                    // Add cells to the "Total" table
                    totalTable.AddCell(new Phrase($"Total # of Products ({totalRowCount})", font));
                    totalTable.AddCell(new Phrase($"Php {grossAmount:F2}", font));
                    totalTable.AddCell(new Phrase($"Cash Given", font));
                    totalTable.AddCell(new Phrase($"Php {cash:F2}", font));
                    totalTable.AddCell(new Phrase($"Change", font));
                    totalTable.AddCell(new Phrase($"Php {change:F2}", font));
                    totalTable.AddCell(new Phrase($"Payment Method:", font));
                    totalTable.AddCell(new Phrase($"{paymentMethod:F2}", font));

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
                    doc.Add(new Paragraph($"Served To: {clientName}", italic));
                    doc.Add(new Paragraph("Address:_______________________________", italic));
                    doc.Add(new Paragraph("TIN No.:_______________________________", italic));

                    // Add the legal string with center alignment
                    Paragraph paragraph_footer = new Paragraph($"\n\n{legal}", italic);
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

        private void RecShopProdGrossAmountBox_TextChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(RecShopProdGrossAmountBox.Text, out decimal grossAmount))
            {
                // Get the Cash Amount from the TextBox (MngrCashBox)
                if (decimal.TryParse(RecShopProdCashBox.Text, out decimal cashAmount))
                {
                    // Calculate the Change
                    decimal change = cashAmount - grossAmount;

                    // Display the calculated change value in the MngrChangeBox
                    RecShopProdChangeBox.Text = change.ToString("0.00");
                }
                else
                {
                    // Handle invalid input in MngrCashBox, e.g., display an error message
                    RecShopProdChangeBox.Text = "0.00";
                }
            }
            else
            {
                // Handle invalid input in MngrGrossAmountBox, e.g., display an error message
                RecShopProdChangeBox.Text = "0.00";
            }
        }

        private void RecShopProdCashBox_TextChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(RecShopProdGrossAmountBox.Text, out decimal grossAmount))
            {
                // Get the Cash Amount from the TextBox (MngrCashBox)
                if (decimal.TryParse(RecShopProdCashBox.Text, out decimal cashAmount))
                {
                    // Calculate the Change
                    decimal change = cashAmount - grossAmount;

                    // Display the calculated change value in the MngrChangeBox
                    RecShopProdChangeBox.Text = change.ToString("0.00");
                }
                else
                {
                    // Handle invalid input in MngrCashBox, e.g., display an error message
                    RecShopProdChangeBox.Text = "0.00";
                }
            }
            else
            {
                // Handle invalid input in MngrGrossAmountBox, e.g., display an error message
                RecShopProdChangeBox.Text = "0.00";
            }
        }

        private void RecShopProdSelectedProdVoidBtn_Click(object sender, EventArgs e)
        {
            if (RecShopProdSelectedProdDGV.Rows.Count == 0)
            {
                MessageBox.Show("The product list is empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //input dialog messagebox
            string enteredPassword = GetPasswordWithAsterisks("Enter Manager Password:", "Void Product Permission");

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

                                    RecShopProdSelectedProdDGV.Rows.Clear();


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

        #endregion

        #endregion

        #region Manager dashboard starts here
        #region Manager Misc. Functions
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
        private void RecInventoryMembershipBtn_Click(object sender, EventArgs e)
        {
            Inventory.PanelShow(MngrInventoryMembershipPanel);

        }

        private void RecInventoryProductsBtn_Click(object sender, EventArgs e)
        {
            Inventory.PanelShow(MngrInventoryProductsPanel);
            MngrInventoryProductData();
        }
        private void MngrSchedExitBtn_Click(object sender, EventArgs e)
        {
            Inventory.PanelShow(MngrInventoryTypePanel);
        }

        private void MngrInventoryProductHistoryExitBtn_Click(object sender, EventArgs e)
        {
            Inventory.PanelShow(MngrInventoryTypePanel);
        }

        private void RecPayServiceExitBtn_Click(object sender, EventArgs e)
        {
            Transaction.PanelShow(RecTransactionPanel);
            RecPayServiceClearAllField();
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
        private void MngrInventoryInDemandBtn_Click(object sender, EventArgs e)
        {
            Inventory.PanelShow(MngrIndemandPanel);
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
            MngrProductSalesTransRepDGV.DataSource = null;
        }

        private void MngrWalkinProdSalesExitBtn_Click(object sender, EventArgs e)
        {

            trydata.Visible = false;
            MngrProductSalesTransRepDGV.DataSource = null;
            trydata.DataSource = null;
            MngrProductSalesSelectedPeriodLbl.Visible = true;
            MngrProductSalesSelectedPeriodText.Visible = true;
            MngrProductSalesFromLbl.Visible = false;
            MngrProductSalesFromDatePicker.Visible = false;
            MngrProductSalesToLbl.Visible = false;
            MngrProductSalesToDatePicker.Visible = false;
            MngrProductSalesPeriodCalendar.Visible = false;
            MngrProductSalesPeriod.SelectedItem = null;
            MngrProductSalesSelectCatBox.SelectedItem = null;
            MngrProductSalesSelectedPeriodText.Text = "";
            MngrProductSalesLineGraph.Series.Clear();
            MngrProductSalesGraph.Series.Clear();
            Inventory.PanelShow(MngrInventoryTypePanel);
        }
        #endregion

        //
        #region Mngr Services Data
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
            if (MngrServicesCategoryComboText.SelectedItem != null)
            {
                MngrServicesCategoryComboText.Text = MngrServicesCategoryComboText.SelectedItem.ToString();
                UpdateServiceTypeComboBox();
                GenerateServiceID();
            }
        }

        private void UpdateServiceTypeComboBox()
        {
            MngrServicesTypeComboText.Items.Clear();

            // Get the selected category
            string selectedCategory = MngrServicesCategoryComboText.SelectedItem.ToString();

            // Filter and add the relevant service types based on the selected category
            switch (selectedCategory)
            {
                case "Hair Styling":
                    MngrServicesTypeComboText.Items.AddRange(new string[] { "Hair Cut", "Hair Blowout", "Hair Color", "Hair Extension", "Package" });
                    break;
                case "Nail Care":
                    MngrServicesTypeComboText.Items.AddRange(new string[] { "Manicure", "Pedicure", "Nail Extension", "Nail Art", "Nail Treatment", "Nail Repair", "Package" });
                    break;
                case "Face & Skin":
                    // Add relevant face and skin service types here
                    MngrServicesTypeComboText.Items.AddRange(new string[] { "Skin Whitening", "Exfoliation Treatment", "Chemical Peel", "Hydration Treatment", "Acne Treatment", "Anti-Aging Treatment", "Package" });
                    break;
                case "Massage":
                    // Add relevant massage service types here
                    MngrServicesTypeComboText.Items.AddRange(new string[] { "Soft Massage", "Moderate Massage", "Hard Massage", "Package" });

                    break;
                case "Spa":
                    // Add relevant spa service types here
                    MngrServicesTypeComboText.Items.AddRange(new string[] { "Herbal Pool", "Sauna", "Package" });
                    break;
                default:
                    break;
            }

            // Select the first item in the list
            if (MngrServicesTypeComboText.Items.Count > 0)
            {
                MngrServicesTypeComboText.SelectedIndex = 0;
            }
        }

        private void RecServicesTypeComboText_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MngrServicesTypeComboText.SelectedItem != null)
            {
                MngrServicesTypeComboText.Text = MngrServicesTypeComboText.SelectedItem.ToString();
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
            if (MngrServicesCategoryComboText.SelectedIndex >= 0 && MngrServicesTypeComboText.SelectedIndex >= 0)
            {
                // Get the selected items from both combo boxes
                string selectedCategory = MngrServicesCategoryComboText.SelectedItem.ToString();
                string selectedType = MngrServicesTypeComboText.SelectedItem.ToString();

                // Call the GenerateServiceID method
                string generatedServiceID = DynamicIDGenerator.GenerateServiceID(selectedCategory, selectedType);

                // Update your UI element with the generated ID
                MngrServicesIDNumText.Text = generatedServiceID;
            }
        }

        private void RecServicesCreateBtn_Click(object sender, EventArgs e)
        {
            string category = MngrServicesCategoryComboText.Text;
            string type = MngrServicesTypeComboText.Text;
            string name = MngrServicesNameText.Text;
            string describe = MngrServicesDescriptionText.Text;
            string duration = MngrServicesDurationText.Text;
            string price = MngrServicesPriceText.Text;
            string ID = MngrServicesIDNumText.Text;

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
            MngrServicesCreateBtn.Visible = true;
            MngrServicesUpdateBtn.Visible = false;
            MngrServicesCategoryComboText.Enabled = true;
            MngrServicesTypeComboText.Enabled = true;
            MngrServicesCategoryComboText.SelectedIndex = -1;
            MngrServicesTypeComboText.SelectedIndex = -1;
            MngrServicesCategoryComboText.Text = "";
            MngrServicesTypeComboText.Text = "";
            MngrServicesNameText.Text = "";
            MngrServicesDescriptionText.Text = "";
            MngrServicesDurationText.Text = "";
            MngrServicesPriceText.Text = "";
            MngrServicesIDNumText.Text = "";

        }

        private void RecServicesUpdateInfoBtn_Click(object sender, EventArgs e)
        {
            if (MngrInventoryServicesTable.SelectedRows.Count > 0)
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to edit the selected data?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialogResult == DialogResult.Yes)
                {
                    // Iterate through selected rows in PendingTable
                    foreach (DataGridViewRow selectedRow in MngrInventoryServicesTable.SelectedRows)
                    {
                        try
                        {
                            //// Re data into the database
                            RetrieveServiceDataFromDB(selectedRow);
                            MngrServicesUpdateBtn.Visible = true;
                            MngrServicesCreateBtn.Visible = false;
                            MngrServicesCategoryComboText.Enabled = false;
                            MngrServicesTypeComboText.Enabled = false;
                        }
                        catch (Exception ex)
                        {
                            // Handle any database-related errors here
                            MessageBox.Show("Error: " + ex.Message, "Service Info Edit Failed");
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

                            MngrServicesCategoryComboText.Text = serviceCategory;
                            MngrServicesTypeComboText.Text = serviceType;
                            MngrServicesIDNumText.Text = serviceID;
                            MngrServicesNameText.Text = serviceName;
                            MngrServicesDescriptionText.Text = serviceDescribe;
                            MngrServicesDurationText.Text = serviceDuration;
                            MngrServicesPriceText.Text = servicePrice;
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
            string category = MngrServicesCategoryComboText.Text;
            string type = MngrServicesTypeComboText.Text;
            string name = MngrServicesNameText.Text;
            string describe = MngrServicesDescriptionText.Text;
            string duration = MngrServicesDurationText.Text;
            string price = MngrServicesPriceText.Text;
            string ID = MngrServicesIDNumText.Text;

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
        #endregion

        #region Mngr. Product Data

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
                    MessageBox.Show("Error: " + ex.Message, "Product Creation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                            MessageBox.Show("Error: " + ex.Message, "Product Information Retrieve Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                                    if (MngrInventoryProductsTypeComboText.SelectedItem.ToString() == "Retail Product")
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
                        MessageBox.Show("Error: " + ex.Message, "Product Information Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
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



        #endregion

        #region Mngr. Staff Schedule 

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
        #endregion

        #region Mngr. PANEL OF WALK-IN Services REVENUE
        private void IncomeBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(MngrWalkinSalesPeriod.Text))
            {
                MessageBox.Show("Please select a sale period.", "Walk-in Services Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        MessageBox.Show("Please choose a day in the calendar.", "Walk-in Services Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    toDate = fromDate;
                    break;
                case "Week":
                    if (MngrWalkinSalesSelectedPeriodText.Text.Length < 23 ||
                        !DateTime.TryParseExact(MngrWalkinSalesSelectedPeriodText.Text.Substring(0, 10), "MM-dd-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fromDate) ||
                        !DateTime.TryParseExact(MngrWalkinSalesSelectedPeriodText.Text.Substring(14), "MM-dd-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out toDate))
                    {
                        MessageBox.Show("Please choose a week in the calendar.", "Walk-in Services Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    break;
                case "Month":
                    if (!DateTime.TryParseExact(MngrWalkinSalesSelectedPeriodText.Text, "MMMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out fromDate))
                    {
                        MessageBox.Show("Please choose a month in the calendar.", "Walk-in Services Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    toDate = fromDate.AddMonths(1).AddDays(-1);
                    break;
                case "Specific Date Range":
                    if (MngrWalkinSalesFromDatePicker.Value > MngrWalkinSalesToDatePicker.Value)
                    {
                        MessageBox.Show("From Date cannot be ahead of To Date.", "Walk-in Services Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (MngrWalkinSalesFromDatePicker.Value.Date == MngrWalkinSalesToDatePicker.Value.Date)
                    {
                        MessageBox.Show("From date and to date cannot be the same.", "Walk-in Services Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    fromDate = MngrWalkinSalesFromDatePicker.Value;
                    toDate = MngrWalkinSalesToDatePicker.Value;
                    break;
                default:
                    MessageBox.Show("Invalid Sale Period selection.", "Walk-in Services Error Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }

            if (string.IsNullOrEmpty(selectedCategory))
            {
                MessageBox.Show("Please select a category.", "Walk-in Services Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                    query += " AND TransactionType = 'Walk-in Transaction'";
                    query += " GROUP BY AppointmentDay, ServiceCategory";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@FromDate", fromDate.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@ToDate", toDate.ToString("yyyy-MM-dd"));

                    if (selectedCategory != "All Categories")
                    {
                        command.Parameters.AddWithValue("@SelectedCategory", selectedCategory);
                    }

                    MySqlDataReader reader = command.ExecuteReader();

                    if (!reader.HasRows)
                    {
                        MngrWalkinSalesGraph.Series.Clear();
                        MngrWalkinSalesGraph.Legends.Clear();
                        MngrWalkinSalesTransRepDGV.DataSource = null;
                        MngrWalkinSalesTransServiceHisDGV.DataSource = null;
                        MessageBox.Show("No data available for the selected date range.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

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

                    transNumQuery += " AND TransactionType = 'Walk-in Transaction'";
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
                    MessageBox.Show("Error: " + ex.Message, "Walk-in Service Sales Graph Failed");
                }
            }
        }

        private void SalePeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            MngrWalkinSalesSelectedPeriodText.Text = "";

            string selectedItem = MngrWalkinSalesPeriod.SelectedItem?.ToString();

            if (selectedItem != null)
            {

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

                    MngrWalkinSalesTransIDShow.Text = transactionNumber;
                }
                connection.Close();
            }
        }

        private void MngrWalkinSalesTransRepDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            ViewWalkinSales();
        }

        private void MngrWalkinSalesExitBtn_Click(object sender, EventArgs e)
        {
            MngrWalkinSalesSelectedPeriodLbl.Visible = true;
            MngrWalkinSalesSelectedPeriodText.Visible = true;
            MngrWalkinSalesFromLbl.Visible = false;
            MngrWalkinSalesFromDatePicker.Visible = false;
            MngrWalkinSalesToLbl.Visible = false;
            MngrWalkinSalesToDatePicker.Visible = false;
            MngrWalkinSalesPeriodCalendar.Visible = false;
            MngrWalkinSalesPeriod.SelectedItem = null;
            MngrWalkinSalesSelectCatBox.SelectedItem = null;
            MngrWalkinSalesSelectedPeriodText.Text = "";
            MngrWalkinSalesTransIDShow.Text = "";
            MngrWalkinSalesTransRepDGV.DataSource = null;
            MngrWalkinSalesTransServiceHisDGV.DataSource = null;
            MngrWalkinSalesGraph.Series.Clear();
            MngrWalkinSalesGraph.Legends.Clear();
            Inventory.PanelShow(MngrInventoryTypePanel);
        }

        #endregion

        #region Mngr. PANEL OF SERVICE DEMAND
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
                    if (MngrIndemandDatePickerFrom.Value > MngrIndemandDatePickerTo.Value)
                    {
                        MessageBox.Show("Invalid date range. Please make sure the From date is before the To date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (MngrIndemandDatePickerFrom.Value.Date == MngrIndemandDatePickerTo.Value.Date)
                    {
                        MessageBox.Show("From date and to date cannot be the same.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
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
                            StarRating,
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
                            SelectedService,
                            StarRating
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
                        Dictionary<string, int> staffRatings = new Dictionary<string, int>();
                        Dictionary<string, double> staffFinalRatings = new Dictionary<string, double>();

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                MngrIndemandServiceGraph.Series.Clear();
                                MngrIndemandServiceSelection.DataSource = null;
                                MngrIndemandBestEmployee.DataSource = null;
                                MessageBox.Show("No data available for the selected date range.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }

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

                        using (MySqlCommand ratingCommand = new MySqlCommand(query, connection))
                        {
                            ratingCommand.Parameters.AddWithValue("@FromDate", fromDate.ToString("yyyy-MM-dd"));
                            ratingCommand.Parameters.AddWithValue("@ToDate", toDate.ToString("yyyy-MM-dd"));
                            ratingCommand.Parameters.AddWithValue("@SelectedCategory", selectedCategory);

                            using (MySqlDataReader staffReader = ratingCommand.ExecuteReader())
                            {
                                while (staffReader.Read())
                                {
                                    string attendingStaff = staffReader.GetString("AttendingStaff");
                                    int starRating = staffReader.GetInt32("StarRating");

                                    if (selectedCategory == "Top Service Category")
                                    {
                                        if (staffRatings.ContainsKey(attendingStaff))
                                        {
                                            staffRatings[attendingStaff] += starRating;
                                        }
                                        else
                                        {
                                            staffRatings[attendingStaff] = starRating;
                                        }
                                    }
                                    else
                                    {                                       
                                        if (staffRatings.ContainsKey(attendingStaff))
                                        {
                                            staffRatings[attendingStaff] += starRating;
                                        }
                                        else
                                        {
                                            staffRatings[attendingStaff] = starRating;
                                        }
                                    }
                                }
                            }
                        }

                        foreach (var staffName in staffCounts.Keys)
                        {
                            if (staffCounts.ContainsKey(staffName) && staffCounts[staffName] != 0)
                            {
                                double finalRating = (double)staffRatings[staffName] / staffCounts[staffName];
                                staffFinalRatings[staffName] = finalRating;
                            }
                            else
                            {
                                staffFinalRatings[staffName] = 0;
                            }
                        }

                        if (selectedCategory == "Top Service Category")
                        {
                            MngrIndemandServiceGraph.Series.Clear();
                            var series = MngrIndemandServiceGraph.Series.Add("ServiceCount");
                            series.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;

                            series["PieLabelStyle"] = "Inside";
                            series["PieLineColor"] = "Black";
                            series["PieDrawingStyle"] = "Concave";

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

                            PopulateServiceSelectionGrid(fromDate.ToString("yyyy-MM-dd"), toDate.ToString("yyyy-MM-dd"));

                            DataTable staffTable = new DataTable();
                            staffTable.Columns.Add("Rank");
                            staffTable.Columns.Add("ID");
                            staffTable.Columns.Add("First Name");
                            staffTable.Columns.Add("Last Name");
                            staffTable.Columns.Add("Services Done");
                            //staffTable.Columns.Add("Rating");

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

                                            //double rating = staffFinalRatings.ContainsKey(employeeID) ? staffFinalRatings[employeeID] : 0;

                                            staffTable.Rows.Add(rank, employeeID, firstName, lastName, kvp.Value); //rating);
                                            rank++;
                                        }
                                    }
                                }
                            }
                            MngrIndemandBestEmployee.DataSource = staffTable;
                            MngrIndemandBestEmployee.Columns["Rank"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            MngrIndemandBestEmployee.Columns["Services Done"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            //MngrIndemandBestEmployee.Columns["Rating"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
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
                            staffTable.Columns.Add("Services Done");
                            staffTable.Columns.Add("Rating");

                            List<KeyValuePair<string, double>> sortedStaffRatings = staffFinalRatings.ToList();
                            sortedStaffRatings.Sort((x, y) => y.Value.CompareTo(x.Value));

                            int rank = 1;

                            foreach (var kvp in sortedStaffRatings)
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

                                            int servicesDone = staffCounts.ContainsKey(employeeID) ? staffCounts[employeeID] : 0;
                                            string formattedRating = kvp.Value.ToString("0.0");

                                            staffTable.Rows.Add(rank, employeeID, firstName, lastName, servicesDone, formattedRating);
                                            rank++;
                                        }
                                    }
                                }
                            }

                            DataView dv = staffTable.DefaultView;
                            dv.Sort = "Rating DESC";
                            MngrIndemandBestEmployee.DataSource = dv.ToTable();
                            MngrIndemandBestEmployee.Columns["Rank"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            MngrIndemandBestEmployee.Columns["Services Done"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            MngrIndemandBestEmployee.Columns["Rating"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                            MngrIndemandServiceGraph.Series.Clear();
                            var pieSeries = MngrIndemandServiceGraph.Series.Add("ServiceCount");
                            pieSeries.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;

                            pieSeries["PieLabelStyle"] = "Inside";
                            pieSeries["PieLineColor"] = "Black";
                            pieSeries["PieDrawingStyle"] = "Concave";

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
                MessageBox.Show("Error: " + ex.Message, "In Demand Services Graph Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateServiceSelectionGrid(string fromDate, string toDate)
        {
            string query = @"
        SELECT 
            ServiceCategory,
            COUNT(*) AS CategoryCount
        FROM 
            servicehistory 
        WHERE 
            ServiceStatus = 'Completed' 
            AND STR_TO_DATE(AppointmentDate, '%m-%d-%Y') BETWEEN @FromDate AND @ToDate
        GROUP BY
            ServiceCategory";

            Dictionary<string, int> serviceCounts = new Dictionary<string, int>();

            using (MySqlConnection connection = new MySqlConnection("Server=localhost;Database=enchante;Uid=root;Pwd=;"))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FromDate", fromDate);
                    command.Parameters.AddWithValue("@ToDate", toDate);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string serviceCategory = reader.GetString("ServiceCategory");
                            int categoryCount = reader.GetInt32("CategoryCount");
                            serviceCounts[serviceCategory] = categoryCount;
                        }
                    }
                }
            }

            DataTable serviceTable = new DataTable();
            serviceTable.Columns.Add("Service Category");
            serviceTable.Columns.Add("Service Count");

            foreach (string category in new List<string> { "Hair Styling", "Massage", "Nail Care", "Face & Skin", "Spa" })
            {
                int count = serviceCounts.ContainsKey(category) ? serviceCounts[category] : 0;
                serviceTable.Rows.Add(category, count);
            }
            serviceTable.DefaultView.Sort = "Service Count DESC";
            serviceTable = serviceTable.DefaultView.ToTable();

            MngrIndemandServiceSelection.DataSource = serviceTable;
        }

        private void ServiceHistoryPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            MngrIndemandSelectPeriod.Text = "";
            string selectedItem = MngrIndemandServiceHistoryPeriod.SelectedItem?.ToString();

            if (selectedItem != null)
            {

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
            MngrIndemandSelectPeriodLbl.Visible = true;
            MngrIndemandSelectPeriod.Visible = true;
            MngrIndemandFromLbl.Visible = false;
            MngrIndemandDatePickerFrom.Visible = false;
            MngrIndemandToLbl.Visible = false;
            MngrIndemandDatePickerTo.Visible = false;
            MngrIndemandServicePeriodCalendar.Visible = false;
            MngrIndemandServiceHistoryPeriod.SelectedItem = null;
            MngrIndemandSelectCatBox.SelectedItem = null;
            MngrIndemandSelectPeriod.Text = "";
            MngrIndemandServiceGraph.Series.Clear();
            MngrIndemandServiceSelection.DataSource = null;
            MngrIndemandBestEmployee.DataSource = null;
            Inventory.PanelShow(MngrInventoryTypePanel);
        }

        #endregion

        #region Mngr. PANEL OF WALK-IN PRODUCT SALES
        private void MngrProductSalesIncomeBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(MngrProductSalesPeriod.Text))
            {
                MessageBox.Show("Please select a sale period.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MngrProductSalesSelectCatBox.SelectedItem == null || string.IsNullOrEmpty(MngrProductSalesSelectCatBox.SelectedItem.ToString()))
            {
                MessageBox.Show("Please select a category.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string fromDate = "";
            string toDate = "";
            string categoryPrefix = "";

            switch (MngrProductSalesPeriod.Text)
            {
                case "Day":
                    if (string.IsNullOrEmpty(MngrProductSalesSelectedPeriodText.Text))
                    {
                        MessageBox.Show("Please select a valid date for the day period.", "Walk-in Products Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    string inputValue = MngrProductSalesSelectedPeriodText.Text;
                    fromDate = inputValue;
                    toDate = inputValue;

                    break;

                case "Week":
                    if (string.IsNullOrEmpty(MngrProductSalesSelectedPeriodText.Text))
                    {
                        MessageBox.Show("Please select a date range for the week period.", "Walk-in Products Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string[] weekDates = MngrProductSalesSelectedPeriodText.Text.Split(new char[] { ' ', 't', 'o', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    fromDate = weekDates[0];
                    toDate = weekDates[1];
                    break;

                case "Month":
                    if (string.IsNullOrEmpty(MngrProductSalesSelectedPeriodText.Text))
                    {
                        MessageBox.Show("Please select a month for the month period.", "Walk-in Products Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string[] monthYear = MngrProductSalesSelectedPeriodText.Text.Split('-');

                    int month = DateTime.ParseExact(monthYear[0], "MMMM", CultureInfo.InvariantCulture).Month;
                    int year = int.Parse(monthYear[1]);
                    fromDate = new DateTime(year, month, 1).ToString("MM-dd-yyyy");
                    toDate = new DateTime(year, month, DateTime.DaysInMonth(year, month)).ToString("MM-dd-yyyy");
                    break;

                case "Specific Date Range":
                    if (MngrProductSalesFromDatePicker.Value > MngrProductSalesToDatePicker.Value)
                    {
                        MessageBox.Show("Invalid date range. Please make sure the From date is before the To date.", "Walk-in Products Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (MngrProductSalesFromDatePicker.Value.Date == MngrProductSalesToDatePicker.Value.Date)
                    {
                        MessageBox.Show("From date and to date cannot be the same.", "Walk-in Products Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    fromDate = MngrProductSalesFromDatePicker.Value.ToString("MM-dd-yyyy");
                    toDate = MngrProductSalesToDatePicker.Value.ToString("MM-dd-yyyy");
                    break;

                default:
                    MessageBox.Show("Invalid selection.", "Walk-in Products Invalid Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }

            switch (MngrProductSalesSelectCatBox.Text)
            {
                case "Hair Styling":
                    categoryPrefix = "HS-";
                    break;
                case "Face & Skin":
                    categoryPrefix = "FS-";
                    break;
                case "Nail Care":
                    categoryPrefix = "NC-";
                    break;
                case "Massage":
                    categoryPrefix = "MS-";
                    break;
                case "Spa":
                    categoryPrefix = "SP-";
                    break;
                case "All Categories":
                    categoryPrefix = "";
                    break;
                default:
                    MessageBox.Show("Please select a category.", "Walk-in Products Missing Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }

            string categoryFilter = "";
            if (MngrProductSalesSelectCatBox.Text != "All Categories")
            {
                categoryFilter = $"AND ItemID LIKE '{categoryPrefix}%'";
            }
            string statusFilter = "ProductStatus = 'Paid'";
            string connectionString = "Server=localhost;Database=enchante;Uid=root;Pwd=;";
            string query = $@"
                        SELECT  
                            LEFT(CheckedOutDate, 10) AS CheckedOutDate,
                            {(MngrProductSalesSelectCatBox.Text == "All Categories" ? "ItemID" : "ItemName")}, 
                            ItemName,
                            ItemID,        
                            ItemPrice,
                            SUM(Qty) AS Qty,                                                 
                            SUM(ItemTotalPrice) AS ItemTotalPrice
                        FROM 
                            orderproducthistory 
                        WHERE 
                            LEFT(CheckedOutDate, 10) >= '{fromDate}'
                            AND LEFT(CheckedOutDate, 10) <= '{toDate}'
                            {categoryFilter}
                            AND {statusFilter}
                        GROUP BY 
                            {(MngrProductSalesSelectCatBox.Text == "All Categories" ? "ItemID" : "ItemName")}, 
                            ItemPrice, 
                            LEFT(CheckedOutDate, 10)";

            try
            {
                DataTable filteredData = FetchFilteredData(query, connectionString);
                DisplayFilteredDataInGrid(filteredData);
                DisplayDataInDataGridView(filteredData);

                if (filteredData.Rows.Count == 0)
                {
                    MessageBox.Show("No data available for the selected date range.", "Walk-in Products Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MngrProductSalesGraph.Series[0].Points.Clear();
                    MngrProductSalesLineGraph.Series.Clear();
                    MngrProductSalesLineGraph.Legends.Clear();
                    return;
                }

                DisplayPieChart(query, connectionString);
                DisplayLineChart(query, connectionString);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Walk-in Products Graph Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayPieChart(string query, string connectionString)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    var groupedRows = from row in dataTable.AsEnumerable()
                                      group row by row.Field<string>("ItemName") into grp
                                      select new
                                      {
                                          ItemName = grp.Key,
                                          TotalQty = grp.Sum(r => r.Field<double>("Qty"))
                                      };

                    if (MngrProductSalesSelectCatBox.Text == "All Categories")
                    {

                        groupedRows = from row in dataTable.AsEnumerable()
                                      group row by GetCategoryPrefix(row.Field<string>("ItemID")) into grp
                                      select new
                                      {
                                          ItemName = grp.Key,
                                          TotalQty = grp.Sum(r => r.Field<double>("Qty"))
                                      };
                    }

                    if (!MngrProductSalesGraph.Series.Any())
                    {
                        MngrProductSalesGraph.Series.Add(new Series());
                    }

                    MngrProductSalesGraph.Series[0].Points.Clear();

                    foreach (var group in groupedRows)
                    {
                        DataPoint dataPoint = new DataPoint();
                        dataPoint.SetValueY(group.TotalQty);
                        dataPoint.LegendText = (MngrProductSalesSelectCatBox.Text == "All Categories") ? GetCategoryName(group.ItemName) : group.ItemName;
                        MngrProductSalesGraph.Series[0].Points.Add(dataPoint);
                    }

                    MngrProductSalesGraph.Series[0].ChartType = SeriesChartType.Pie;
                    MngrProductSalesGraph.Series[0]["PieLabelStyle"] = "Inside";
                    MngrProductSalesGraph.Series[0]["PieLineColor"] = "Black";
                    MngrProductSalesGraph.Series[0]["PieDrawingStyle"] = "Concave";

                    MngrProductSalesGraph.Titles.Clear();
                    MngrProductSalesGraph.Titles.Add("Quantity Sold Distribution").Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold | FontStyle.Italic);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayLineChart(string query, string connectionString)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    if (!MngrProductSalesLineGraph.ChartAreas.Any(ca => ca.Name == "MainChartArea"))
                    {
                        MngrProductSalesLineGraph.ChartAreas.Add("MainChartArea");
                    }
                    MngrProductSalesLineGraph.Series.Clear();
                    MngrProductSalesLineGraph.Legends.Clear();
                    bool groupByCategory = MngrProductSalesSelectCatBox.Text == "All Categories";
                    trydata.Visible = true;
                    if (groupByCategory)
                    {
                        var distinctItems = dataTable.AsEnumerable()
                            .Select(row => GetCategoryPrefix(row.Field<string>("ItemID")))
                            .Distinct();
                        var distinctDates = dataTable.AsEnumerable()
                            .Select(row => DateTime.ParseExact(row.Field<string>("CheckedOutDate").Substring(0, 10), "MM-dd-yyyy", CultureInfo.InvariantCulture))
                            .Distinct();
                        List<DateTime> selectedDates = distinctDates.ToList();
                        Dictionary<string, Series> categorySeries = new Dictionary<string, Series>();
                        foreach (var item in distinctItems)
                        {
                            Series series = new Series(GetCategoryName(item));
                            series.ChartType = SeriesChartType.Line;
                            series.XValueType = ChartValueType.DateTime;
                            series.BorderWidth = 3;
                            series.MarkerStyle = MarkerStyle.Circle;
                            series.MarkerSize = 8;
                            var dataForItem = dataTable.AsEnumerable()
                                .Where(row => GetCategoryPrefix(row.Field<string>("ItemID")) == item)
                                .OrderBy(row => DateTime.ParseExact(row.Field<string>("CheckedOutDate").Substring(0, 10), "MM-dd-yyyy", CultureInfo.InvariantCulture));
                            Dictionary<DateTime, double> dateDataPoints = new Dictionary<DateTime, double>();
                            foreach (var date in selectedDates)
                            {
                                dateDataPoints[date] = 0;
                            }
                            foreach (DataRow row in dataForItem)
                            {
                                string dateString = row["CheckedOutDate"].ToString().Substring(0, 10);
                                DateTime date = DateTime.ParseExact(dateString, "MM-dd-yyyy", CultureInfo.InvariantCulture);
                                double totalPrice = Convert.ToDouble(row["ItemTotalPrice"]);
                                dateDataPoints[date] = totalPrice;
                            }
                            foreach (var date in selectedDates)
                            {
                                series.Points.AddXY(date, dateDataPoints[date]);
                            }
                            MngrProductSalesLineGraph.Series.Add(series);
                        }
                    }
                    else
                    {
                        trydata.Visible = false;
                        var distinctItemNames = dataTable.AsEnumerable()
                            .Select(row => row.Field<string>("ItemName"))
                            .Distinct();
                        foreach (var itemName in distinctItemNames)
                        {
                            var dataForItem = dataTable.AsEnumerable()
                                .Where(row => row.Field<string>("ItemName") == itemName);
                            Series series = new Series(itemName);
                            series.ChartType = SeriesChartType.Line;
                            series.XValueType = ChartValueType.DateTime;
                            series.BorderWidth = 3;
                            series.MarkerStyle = MarkerStyle.Circle;
                            series.MarkerSize = 8;
                            foreach (DataRow row in dataForItem)
                            {
                                string dateString = row["CheckedOutDate"].ToString().Substring(0, 10);
                                if (!DateTime.TryParseExact(dateString, "MM-dd-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
                                {
                                    MessageBox.Show($"Error parsing date: {dateString}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    continue;
                                }
                                double totalPrice = Convert.ToDouble(row["ItemTotalPrice"]);
                                series.Points.AddXY(date, totalPrice);
                            }
                            MngrProductSalesLineGraph.Series.Add(series);
                        }
                    }
                    MngrProductSalesLineGraph.ChartAreas[0].AxisX.Title = "Dates";
                    MngrProductSalesLineGraph.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
                    MngrProductSalesLineGraph.ChartAreas[0].AxisY.Title = "Revenue";
                    MngrProductSalesLineGraph.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
                    MngrProductSalesLineGraph.ChartAreas["MainChartArea"].Position = new ElementPosition(5, 5, 90, 70);
                    MngrProductSalesLineGraph.ChartAreas["MainChartArea"].InnerPlotPosition.Auto = false;
                    MngrProductSalesLineGraph.Titles.Clear();
                    MngrProductSalesLineGraph.Titles.Add("Sales Revenue").Font = new System.Drawing.Font("Arial", 12, FontStyle.Bold | FontStyle.Italic);
                    MngrProductSalesLineGraph.Legends.Add(new Legend("MainLegend"));
                    MngrProductSalesLineGraph.Legends["MainLegend"].Docking = Docking.Bottom;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetCategoryPrefix(string itemId)
        {
            if (itemId.Length >= 3) return itemId.Substring(0, 3);
            else return "Other";
        }

        private string GetCategoryName(string categoryPrefix)
        {
            switch (categoryPrefix)
            {
                case "HS-":
                    return "Hair Styling Category";
                case "FS-":
                    return "Face & Skin Category";
                case "NC-":
                    return "Nail Care Category";
                case "MS-":
                    return "Massage Category";
                case "SP-":
                    return "Spa Category";
                default:
                    return "Other";
            }
        }

        private void DisplayDataInDataGridView(DataTable data)
        {
            DataTable aggregatedData = new DataTable();

            aggregatedData.Columns.Add("Date", typeof(string));
            aggregatedData.Columns.Add("Category", typeof(string));
            aggregatedData.Columns.Add("TotalRevenue", typeof(double));

            var groupedData = data.AsEnumerable()
                                  .GroupBy(row => new { Date = row.Field<string>("CheckedOutDate").Substring(0, 10), CategoryPrefix = GetCategoryPrefix(row.Field<string>("ItemID")) })
                                  .Select(group => new
                                  {
                                      Date = group.Key.Date,
                                      Category = GetCategoryName(group.Key.CategoryPrefix),
                                      TotalRevenue = group.Sum(row => row.Field<double>("ItemTotalPrice"))
                                  })
                                  .OrderBy(group => group.Category)
                                  .ThenBy(group => group.Date);

            foreach (var group in groupedData)
            {
                aggregatedData.Rows.Add(group.Date, group.Category, group.TotalRevenue);
            }
            trydata.DataSource = aggregatedData;
        }


        private DataTable FetchFilteredData(string query, string connectionString)
        {
            DataTable filteredData = new DataTable();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    adapter.Fill(filteredData);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return filteredData;
        }

        private void DisplayFilteredDataInGrid(DataTable filteredData)
        {
            MngrProductSalesTransRepDGV.Rows.Clear();
            MngrProductSalesTransRepDGV.Columns.Clear();

            if (MngrProductSalesSelectCatBox.Text == "All Categories")
            {
                MngrProductSalesTransRepDGV.Columns.Add("Category", "Category");
                MngrProductSalesTransRepDGV.Columns.Add("Quantity Sold", "Quantity Sold");
                MngrProductSalesTransRepDGV.Columns.Add("Overall Revenue", "Overall Revenue");

                Dictionary<string, int> categoryQuantities = new Dictionary<string, int>();
                Dictionary<string, double> categoryRevenues = new Dictionary<string, double>();

                foreach (DataRow row in filteredData.Rows)
                {
                    string categoryPrefix = GetCategoryPrefix(row.Field<string>("ItemID"));
                    int qty = Convert.ToInt32(row["Qty"]);
                    double itemTotalPrice = Convert.ToDouble(row["ItemTotalPrice"]);

                    if (!categoryQuantities.ContainsKey(categoryPrefix))
                    {
                        categoryQuantities[categoryPrefix] = qty;
                        categoryRevenues[categoryPrefix] = itemTotalPrice;
                    }
                    else
                    {
                        categoryQuantities[categoryPrefix] += qty;
                        categoryRevenues[categoryPrefix] += itemTotalPrice;
                    }
                }

                foreach (var kvp in categoryQuantities)
                {
                    string categoryName = GetCategoryName(kvp.Key);
                    MngrProductSalesTransRepDGV.Rows.Add(
                        categoryName,
                        kvp.Value,
                        categoryRevenues[kvp.Key]
                    );
                }
            }
            else
            {
                DataView dv = filteredData.DefaultView;
                dv.Sort = "CheckedOutDate ASC";
                DataTable sortedData = dv.ToTable();

                MngrProductSalesTransRepDGV.Columns.Add("CheckedOutDate", "Date");
                MngrProductSalesTransRepDGV.Columns.Add("ItemID", "Item ID");
                MngrProductSalesTransRepDGV.Columns.Add("ItemName", "Item Name");
                MngrProductSalesTransRepDGV.Columns.Add("Qty", "Quantity");
                MngrProductSalesTransRepDGV.Columns.Add("ItemPrice", "Price");
                MngrProductSalesTransRepDGV.Columns.Add("ItemTotalPrice", "Total Price");

                foreach (DataRow row in sortedData.Rows)
                {
                    MngrProductSalesTransRepDGV.Rows.Add(
                        row["CheckedOutDate"],
                        row["ItemID"],
                        row["ItemName"],
                        row["Qty"],
                        row["ItemPrice"],
                        row["ItemTotalPrice"]
                    );
                }
            }
        }

        private void MngrProductSalesPeriod_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            MngrProductSalesSelectedPeriodText.Text = "";
            string selectedItem = MngrProductSalesPeriod.SelectedItem?.ToString();

            if (selectedItem != null)
            {

                if (selectedItem == "Day" || selectedItem == "Week" || selectedItem == "Month")
                {
                    MngrProductSalesPeriodCalendar.Visible = true;
                    MngrProductSalesFromLbl.Visible = false;
                    MngrProductSalesToLbl.Visible = false;
                    MngrProductSalesFromDatePicker.Visible = false;
                    MngrProductSalesToDatePicker.Visible = false;
                    MngrProductSalesSelectedPeriodLbl.Visible = true;
                    MngrProductSalesSelectedPeriodText.Visible = true;
                }
                else if (selectedItem == "Specific Date Range")
                {
                    MngrProductSalesPeriodCalendar.Visible = false;
                    MngrProductSalesFromLbl.Visible = true;
                    MngrProductSalesToLbl.Visible = true;
                    MngrProductSalesFromDatePicker.Visible = true;
                    MngrProductSalesToDatePicker.Visible = true;
                    MngrProductSalesSelectedPeriodLbl.Visible = false;
                    MngrProductSalesSelectedPeriodText.Visible = false;
                }
            }
        }

        private void MngrProductSalesPeriodCalendar_DateChanged_1(object sender, DateRangeEventArgs e)
        {
            DateTime selectedDate = MngrProductSalesPeriodCalendar.SelectionStart;
            string selectedPeriod = "";
            string salePeriod = MngrProductSalesPeriod.SelectedItem.ToString();

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
            MngrProductSalesSelectedPeriodText.Text = selectedPeriod;
        }

        #endregion

        #region Mngr. PANEL OF APPOINTMENT Services REVENUE
        private void MngrAppSalesIncomeBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(MngrAppSalesPeriod.Text))
            {
                MessageBox.Show("Please select a sale period.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MngrAppSalesSelectCatBox.SelectedItem == null || string.IsNullOrEmpty(MngrAppSalesSelectCatBox.SelectedItem.ToString()))
            {
                MessageBox.Show("Please select a category.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string fromDate = "";
            string toDate = "";
            string selectedCategory = MngrAppSalesSelectCatBox.SelectedItem?.ToString();
            string salePeriod = MngrAppSalesPeriod.SelectedItem.ToString();

            switch (MngrAppSalesPeriod.Text)
            {
                case "Day":

                    if (string.IsNullOrEmpty(MngrAppSalesSelectedPeriodText.Text))
                    {
                        MessageBox.Show("Please select a valid date for the day period.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string inputValue = MngrAppSalesSelectedPeriodText.Text;
                    fromDate = inputValue;
                    toDate = inputValue;
                    break;

                case "Week":

                    if (string.IsNullOrEmpty(MngrAppSalesSelectedPeriodText.Text))
                    {
                        MessageBox.Show("Please select a date range for the week period.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string[] weekDates = MngrAppSalesSelectedPeriodText.Text.Split(new char[] { ' ', 't', 'o', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    fromDate = weekDates[0];
                    toDate = weekDates[1];
                    break;

                case "Month":

                    if (string.IsNullOrEmpty(MngrAppSalesSelectedPeriodText.Text))
                    {
                        MessageBox.Show("Please select a month for the month period.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    string[] monthYear = MngrAppSalesSelectedPeriodText.Text.Split('-');
                    int month = DateTime.ParseExact(monthYear[0], "MMMM", CultureInfo.InvariantCulture).Month;
                    int year = int.Parse(monthYear[1]);
                    fromDate = new DateTime(year, month, 1).ToString("MM-dd-yyyy");
                    toDate = new DateTime(year, month, DateTime.DaysInMonth(year, month)).ToString("MM-dd-yyyy");
                    break;

                case "Specific Date Range":

                    if (MngrAppSalesFromDatePicker.Value > MngrAppSalesToDatePicker.Value)
                    {
                        MessageBox.Show("Invalid date range. Please make sure the From date is before the To date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (MngrAppSalesFromDatePicker.Value.Date == MngrAppSalesToDatePicker.Value.Date)
                    {
                        MessageBox.Show("From date and to date cannot be the same.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    fromDate = MngrAppSalesFromDatePicker.Value.ToString("MM-dd-yyyy");
                    toDate = MngrAppSalesToDatePicker.Value.ToString("MM-dd-yyyy");
                    break;

                default:
                    MessageBox.Show("Invalid selection.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                    string query = @"
                        SELECT 
                            LEFT(AppointmentDate, 10) AS AppointmentDay, 
                            ServiceCategory,
                            SUM(CAST(ServicePrice AS DECIMAL(10, 2))) AS TotalRevenue 
                        FROM 
                            servicehistory 
                        WHERE 
                            ServiceStatus = 'Completed' 
                            AND LEFT(AppointmentDate, 10) BETWEEN @FromDate AND @ToDate ";

                    if (selectedCategory != "All Categories")
                    {
                        query += " AND ServiceCategory = @SelectedCategory";
                    }

                    query += " AND TransactionType = 'Walk-in Appointment Transaction'";
                    query += " GROUP BY LEFT(AppointmentDate, 10), ServiceCategory";

                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@FromDate", fromDate);
                    command.Parameters.AddWithValue("@ToDate", toDate);

                    if (selectedCategory != "All Categories")
                    {
                        command.Parameters.AddWithValue("@SelectedCategory", selectedCategory);
                    }

                    MySqlDataReader reader = command.ExecuteReader();

                    if (!reader.HasRows)
                    {
                        MessageBox.Show("No data available for the selected date range.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        MngrAppSalesGraph.Series.Clear();
                        MngrAppSalesGraph.Legends.Clear();
                        MngrAppSalesTransRepDGV.DataSource = null;
                        MngrAppSalesTransServiceHisDGV.DataSource = null;
                        MngrAppSalesTransIDShow.Text = "";
                        return;
                    }

                    while (reader.Read())
                    {
                        string appointmentDayString = reader["AppointmentDay"].ToString().Substring(0, 10);
                        DateTime appointmentDay;
                        if (!DateTime.TryParseExact(appointmentDayString, "MM-dd-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out appointmentDay))
                        {
                            MessageBox.Show($"Error parsing date: {appointmentDayString}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            continue;
                        }

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

                    AppointmentServiceBreakdown(selectedCategory, fromDate, toDate, connection);
                    DisplayAppointmentLineChart(query, connectionString, categoryRevenues, dates);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private void DisplayAppointmentLineChart(string query, string connectionString, Dictionary<string, List<decimal>> categoryRevenues, List<DateTime> dates)
        {
            MngrAppSalesGraph.Series.Clear();
            MngrAppSalesGraph.Legends.Clear();

            foreach (var category in categoryRevenues.Keys)
            {
                Series series = MngrAppSalesGraph.Series.Add($"{category} Revenue");
                series.ChartType = SeriesChartType.Line;
                series.BorderWidth = 3;

                for (int i = 0; i < dates.Count; i++)
                {
                    string dateString = dates[i].ToShortDateString();
                    if (categoryRevenues[category].Count > i)
                    {
                        series.Points.AddXY(dateString, categoryRevenues[category][i]);
                        series.Points[i].MarkerStyle = MarkerStyle.Circle;
                        series.Points[i].MarkerSize = 8;
                    }
                    else
                    {
                        series.Points.AddXY(dateString, 0);
                    }
                }
            }

            MngrAppSalesGraph.ChartAreas[0].AxisX.Title = "Dates";
            MngrAppSalesGraph.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);
            MngrAppSalesGraph.ChartAreas[0].AxisY.Title = "Revenue";
            MngrAppSalesGraph.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Arial", 10, System.Drawing.FontStyle.Bold);

            MngrAppSalesGraph.Legends.Add("Legend1");
            MngrAppSalesGraph.Legends[0].Enabled = true;
            MngrAppSalesGraph.Legends[0].Docking = Docking.Bottom;
        }

        private void AppointmentServiceBreakdown(string selectedCategory, string fromDate, string toDate, MySqlConnection connection)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("TransactionNumber");
            dt.Columns.Add("AppointmentDate");
            dt.Columns.Add("TotalServicePrice", typeof(decimal));

            string transNumQuery = @"
                    SELECT TransactionNumber, AppointmentDate, SUM(CAST(ServicePrice AS DECIMAL(10, 2))) AS TotalServicePrice 
                    FROM servicehistory 
                    WHERE ServiceStatus = 'Completed' 
                    AND LEFT(AppointmentDate, 10) BETWEEN @FromDate AND @ToDate ";

            if (selectedCategory != "All Categories")
            {
                transNumQuery += " AND ServiceCategory = @SelectedCategory";
            }

            transNumQuery += " AND TransactionType = 'Walk-in Appointment Transaction'";
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
                    decimal totalServicePrice = transNumReader.GetDecimal("TotalServicePrice");

                    dt.Rows.Add(transactionNumber, appointmentDate, totalServicePrice);
                }
            }

            MngrAppSalesTransRepDGV.DataSource = dt;
        }

        private void MngrAppSalesPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            MngrAppSalesSelectedPeriodText.Text = "";
            string selectedItem = MngrAppSalesPeriod.SelectedItem?.ToString();

            if (selectedItem != null)
            {
                if (selectedItem == "Day" || selectedItem == "Week" || selectedItem == "Month")
                {
                    MngrAppSalesPeriodCalendar.Visible = true;
                    MngrAppSalesFromLbl.Visible = false;
                    MngrAppSalesToLbl.Visible = false;
                    MngrAppSalesFromDatePicker.Visible = false;
                    MngrAppSalesToDatePicker.Visible = false;
                    MngrAppSalesSelectedPeriodLbl.Visible = true;
                    MngrAppSalesSelectedPeriodText.Visible = true;
                }

                else if (selectedItem == "Specific Date Range")
                {
                    MngrAppSalesPeriodCalendar.Visible = false;
                    MngrAppSalesFromLbl.Visible = true;
                    MngrAppSalesToLbl.Visible = true;
                    MngrAppSalesFromDatePicker.Visible = true;
                    MngrAppSalesToDatePicker.Visible = true;
                    MngrAppSalesSelectedPeriodLbl.Visible = false;
                    MngrAppSalesSelectedPeriodText.Visible = false;
                }
            }
        }

        private void MngrAppSalesPeriodCalendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            DateTime selectedDate = MngrAppSalesPeriodCalendar.SelectionStart;
            string selectedPeriod = "";
            string salePeriod = MngrAppSalesPeriod.SelectedItem.ToString();

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
            MngrAppSalesSelectedPeriodText.Text = selectedPeriod;
        }

        private void MngrAppSalesTransRepDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string connectionString = "Server=localhost;Database=enchante;Uid=root;Pwd=;";

            if (MngrAppSalesTransRepDGV == null || MngrAppSalesTransRepDGV.SelectedRows.Count == 0 || MngrAppSalesTransRepDGV.SelectedRows[0].Cells["TransactionNumber"] == null)
            {
                MessageBox.Show("Please select a row to view.", "No Row Selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string transactionNumber = MngrAppSalesTransRepDGV.SelectedRows[0].Cells["TransactionNumber"].Value?.ToString();

            if (string.IsNullOrEmpty(transactionNumber))
            {
                MessageBox.Show("TransactionNumber is null or empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string categoryFilter = "";
            if (MngrAppSalesSelectCatBox.SelectedItem?.ToString() != "All Categories")
            {
                categoryFilter = "AND ServiceCategory = @ServiceCategory";
            }

            string query = @"
                    SELECT ServiceCategory, SelectedService, ServicePrice 
                    FROM servicehistory 
                    WHERE TransactionNumber = @TransactionNumber 
                    AND ServiceStatus = 'Completed' 
                    AND TransactionType = 'Walk-in Appointment Transaction' " + categoryFilter;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TransactionNumber", transactionNumber);

                    if (MngrAppSalesSelectCatBox.SelectedItem?.ToString() != "All Categories")
                    {
                        command.Parameters.AddWithValue("@ServiceCategory", MngrAppSalesSelectCatBox.SelectedItem?.ToString());
                    }

                    DataTable dataTable = new DataTable();

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                    MngrAppSalesTransServiceHisDGV.DataSource = dataTable;
                    MngrAppSalesTransIDShow.Text = transactionNumber;
                }
                connection.Close();
            }
        }
        #endregion

        #endregion


        //Admin Dashboard Starts Here
        #region
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
            if (age == 0)
            {
                AdminAgeErrorLbl.Visible = false;
                AdminAgeErrorLbl.Text = "Must be 18 years old and above";
                return;
            }
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
                AdminCancelEditBtn.Visible = true;

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

        private void AdminCancelEditBtn_Click(object sender, EventArgs e)
        {
            AdminCreateAccBtn.Visible = true;
            AdminUpdateAccBtn.Visible = false;
            AdminCancelEditBtn.Visible = false;
            AdminEmplTypeComboText.Enabled = true;
            AdminEmplCatComboText.Enabled = true;
            AdminClearFields();
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
            AdminAgeText.Text = "";
            AdminCPNumText.Text = "";
            AdminEmplIDText.Text = "";
            AdminEmailText.Text = "";
            AdminPassText.Text = "";
            AdminConfirmPassText.Text = "";
            AdminEmplTypeComboText.SelectedItem = null;
            AdminEmplCatComboText.SelectedItem = null;
            AdminEmplCatLvlComboText.SelectedItem = null;
            AdminGenderComboText.SelectedItem = null;
            AdminBdayPicker.Value = DateTime.Now;
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
                                    AdminCancelEditBtn.Visible = false;
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
        #endregion


        
        #region Staff Dashboard Starts Here
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
        
        #region general and preferred queue
        public class PendingCustomers
        {
            public string TransactionNumber { get; set; }
            public string ClientName { get; set; }
            public string ServiceID { get; set; }
            public string ServiceName { get; set; }
            public string ServiceStatus { get; set; }
            public string QueNumber { get; set; }
        }
        public int generalsmallestquenumber;

        protected void InitializeGeneralCuePendingCustomersForStaff()
        {
            List<PendingCustomers> generalquependingcustomers = RetrieveGeneralQuePendingCustomersFromDB();

            if (generalquependingcustomers.Count == 0)
            {
                NoCustomerInQueueUserControl nocustomerusercontrol = new NoCustomerInQueueUserControl();
                StaffGeneralCueCurrentCustomersStatusFlowLayoutPanel.Controls.Add(nocustomerusercontrol);

                List<PendingCustomers> preferredquependingcustomers = RetrievePreferredQuePendingCustomersFromDB();
                int smallestQueNumber = int.MaxValue;

                foreach (PendingCustomers customer in preferredquependingcustomers)
                {
                    StaffCurrentAvailableCustomersUserControl customer2 = new StaffCurrentAvailableCustomersUserControl(this);
                    string queNumberText = customer2.StaffQueNumberTextBox.Text;
                    if (int.TryParse(queNumberText, out int queNumber))
                    {
                        if (queNumber < smallestQueNumber)
                        {
                            preferredsmallestquenumber = queNumber;
                        }
                    }
                }
                return;
            }

            int smallestQueNumber2 = int.MaxValue;

            foreach (PendingCustomers customer in generalquependingcustomers)
            {
                StaffCurrentAvailableCustomersUserControl availablecustomersusercontrol = new StaffCurrentAvailableCustomersUserControl(this);
                availablecustomersusercontrol.AvailableCustomerSetData(customer);
                availablecustomersusercontrol.ExpandUserControlButtonClicked += AvailableCustomersUserControl_ExpandCollapseButtonClicked;
                availablecustomersusercontrol.StartServiceButtonClicked += AvailableCustomersUserControl_StartServiceButtonClicked;
                availablecustomersusercontrol.StaffEndServiceBtnClicked += AvailableCustomersUserControl_EndServiceButtonClicked;
                availablecustomersusercontrol.StaffQueTypeTextBox.Visible = false;
                StaffGeneralCueCurrentCustomersStatusFlowLayoutPanel.Controls.Add(availablecustomersusercontrol);
                availablecustomersusercontrol.CurrentStaffID = StaffIDNumLbl.Text;

                string queNumberText = availablecustomersusercontrol.StaffQueNumberTextBox.Text;
                if (int.TryParse(queNumberText, out int queNumber))
                {
                    if (queNumber < smallestQueNumber2)
                    {
                        smallestQueNumber2 = queNumber;
                    }
                }
            }

            UpdateStartServiceButtonStatusGeneral(generalquependingcustomers, smallestQueNumber2);
            generalsmallestquenumber = smallestQueNumber2;
        }
        private void UpdateStartServiceButtonStatusGeneral(List<PendingCustomers> generalquependingcustomers, int smallestQueNumber)
        {
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
            if (StaffPriorityQueueCurrentCustomersStatusFlowLayoutPanel.Controls.Count > 0 && !StaffPriorityQueueCurrentCustomersStatusFlowLayoutPanel.Controls.OfType<NoCustomerInQueueUserControl>().Any())
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

                    string generalquependingcustomersquery = $@"SELECT sh.TransactionNumber, sh.ClientName, sh.ServiceStatus, sh.SelectedService, sh.ServiceID, sh.QueNumber 
                                                     FROM servicehistory sh 
                                                     INNER JOIN walk_in_appointment wa ON sh.TransactionNumber = wa.TransactionNumber 
                                                     WHERE (sh.ServiceStatus = 'Pending' OR sh.ServiceStatus = 'Pending Paid')
                                                     AND sh.ServiceCategory = @membercategory 
                                                     AND sh.QueType = 'GeneralQue' 
                                                     AND (wa.ServiceStatus = 'Pending' OR wa.ServiceStatus = 'Pending Paid')
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
                                ServiceID = reader["ServiceID"] as string,
                                QueNumber = reader["QueNumber"] as string
                            };

                            result.Add(generalquependingcustomers);
                        }
                    }
                    if (result.Count == 0)
                    {
                        //MessageBox.Show("No customers in the queue.", "Empty Queue", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            StaffPriorityQueueCurrentCustomersStatusFlowLayoutPanel.Controls.Clear();
            InitializePriorityPendingCustomersForStaff();
            InitializeGeneralCuePendingCustomersForStaff();
            InitializePreferredCuePendingCustomersForStaff();


            bool hasNoCustomerControl = StaffGeneralCueCurrentCustomersStatusFlowLayoutPanel.Controls.OfType<NoCustomerInQueueUserControl>().Any()
               || StaffPersonalCueCurrentCustomersStatusFlowLayoutPanel.Controls.OfType<NoCustomerInQueueUserControl>().Any();

            if (!hasNoCustomerControl)
            {
                List<PendingCustomers> generalquependingcustomers = RetrieveGeneralQuePendingCustomersFromDB();
                int smallestQueNumber2 = generalsmallestquenumber;
                UpdateStartServiceButtonStatusGeneral(generalquependingcustomers, smallestQueNumber2);

                List<PendingCustomers> preferredquependingcustomers = RetrievePreferredQuePendingCustomersFromDB();
                int smallestQueNumber = preferredsmallestquenumber;
                UpdateStartServiceButtonStatusPreferred(preferredquependingcustomers, smallestQueNumber);
            }

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
            foreach (System.Windows.Forms.Control control in StaffPriorityQueueCurrentCustomersStatusFlowLayoutPanel.Controls.OfType<StaffCurrentAvailableCustomersUserControl>().ToList())
            {
                if (control != selectedControl)
                {
                    StaffPriorityQueueCurrentCustomersStatusFlowLayoutPanel.Controls.Remove(control);
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

                    string preferredquependingcustomersquery = $@"SELECT sh.TransactionNumber, sh.ClientName, sh.ServiceStatus, sh.SelectedService, sh.ServiceID, sh.QueNumber
                       FROM servicehistory sh INNER JOIN walk_in_appointment wa ON sh.TransactionNumber = wa.TransactionNumber
                       WHERE (sh.ServiceStatus = 'Pending' OR sh.ServiceStatus = 'Pending Paid') AND sh.ServiceCategory = @membercategory AND sh.PreferredStaff = @preferredstaff AND (wa.ServiceStatus = 'Pending' OR wa.ServiceStatus = 'Pending Paid') AND sh.AppointmentDate = @datetoday";

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
                                ServiceID = reader.IsDBNull(reader.GetOrdinal("ServiceID")) ? string.Empty : reader.GetString("ServiceID"),
                                QueNumber = reader.IsDBNull(reader.GetOrdinal("QueNumber")) ? string.Empty : reader.GetString("QueNumber")
                            };

                            result.Add(preferredquependingcustomers);
                        }
                    }

                    if (result.Count == 0)
                    {
                        //MessageBox.Show("No customers in the preferred queue.", "Empty Queue", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }

            return result;
        }

        public int preferredsmallestquenumber;

        protected void InitializePreferredCuePendingCustomersForStaff()
        {
            List<PendingCustomers> preferredquependingcustomers = RetrievePreferredQuePendingCustomersFromDB();

            if (preferredquependingcustomers.Count == 0)
            {
                NoCustomerInQueueUserControl nocustomerusercontrol = new NoCustomerInQueueUserControl();
                StaffPersonalCueCurrentCustomersStatusFlowLayoutPanel.Controls.Add(nocustomerusercontrol);
                generalsmallestquenumber = preferredsmallestquenumber;

                List<PendingCustomers> generalquependingcustomers = RetrievePreferredQuePendingCustomersFromDB();
                int smallestQueNumber3 = int.MaxValue;

                foreach (PendingCustomers customer in generalquependingcustomers)
                {
                    StaffCurrentAvailableCustomersUserControl customer2 = new StaffCurrentAvailableCustomersUserControl(this);
                    string queNumberText2 = customer2.StaffQueNumberTextBox.Text;
                    if (int.TryParse(queNumberText2, out int queNumber2))
                    {
                        if (queNumber2 < smallestQueNumber3)
                        {
                            generalsmallestquenumber = queNumber2;
                        }
                    }
                }
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
                availablecustomersusercontrol.StaffQueTypeTextBox.Visible = false;
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
            UpdateStartServiceButtonStatusPreferred(preferredquependingcustomers, smallestQueNumber);
            preferredsmallestquenumber = smallestQueNumber;
        }

        private void UpdateStartServiceButtonStatusPreferred(List<PendingCustomers> preferredquependingcustomers, int smallestQueNumber)
        {
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
            if (StaffPriorityQueueCurrentCustomersStatusFlowLayoutPanel.Controls.Count > 0 && !StaffPriorityQueueCurrentCustomersStatusFlowLayoutPanel.Controls.OfType<NoCustomerInQueueUserControl>().Any())
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




        #endregion


        #region staff inventory
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
        #endregion

        #region Paid Appointment Queue 
        public class PriorityPendingCustomers
        {
            public string TransactionNumber { get; set; }
            public string ClientName { get; set; }
            public string ServiceID { get; set; }
            public string ServiceName { get; set; }
            public string ServiceStatus { get; set; }
            public string QueType { get; set; }
            public string QueNumber { get; set; }
        }

        private List<PriorityPendingCustomers> RetrievePriorityGeneralAndPreferredQuePendingCustomersFromDB()
        {
            string staffID = StaffIDNumLbl.Text;
            DateTime currentDate = DateTime.Today;
            string datetoday = currentDate.ToString("MM-dd-yyyy dddd");
            List<PriorityPendingCustomers> result = new List<PriorityPendingCustomers>();

            using (MySqlConnection connection = new MySqlConnection(mysqlconn))
            {
                try
                {
                    connection.Open();

                    string priorityquependingcustomersquery = $@"SELECT sh.TransactionNumber, sh.ClientName, sh.ServiceStatus, sh.SelectedService, sh.ServiceID, sh.QueNumber, sh.QueType 
                                 FROM servicehistory sh 
                                 INNER JOIN appointment app ON sh.TransactionNumber = app.TransactionNumber 
                                 WHERE (sh.ServiceStatus = 'Pending' OR sh.ServiceStatus = 'Pending Paid')
                                 AND sh.ServiceCategory = @membercategory 
                                 AND (sh.QueType = 'AnyonePriority' OR sh.PreferredStaff = @preferredstaff)
                                 AND (app.ServiceStatus = 'Pending' OR app.ServiceStatus = 'Pending Paid')
                                 AND app.AppointmentStatus = 'Confirmed'
                                 AND sh.AppointmentDate = @datetoday";

                    MySqlCommand command = new MySqlCommand(priorityquependingcustomersquery, connection);
                    command.Parameters.AddWithValue("@membercategory", membercategory);
                    command.Parameters.AddWithValue("@preferredstaff", staffID);
                    command.Parameters.AddWithValue("@datetoday", datetoday);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PriorityPendingCustomers priorityquependingcustomers = new PriorityPendingCustomers
                            {
                                TransactionNumber = reader["TransactionNumber"] as string,
                                ClientName = reader["ClientName"] as string,
                                ServiceStatus = reader["ServiceStatus"] as string,
                                ServiceName = reader["SelectedService"] as string,
                                ServiceID = reader["ServiceID"] as string,
                                QueType = reader["QueType"] as string,
                                QueNumber = reader["QueNumber"] as string
                            };

                            result.Add(priorityquependingcustomers);
                        }
                    }
                    if (result.Count == 0)
                    {
                        //MessageBox.Show("No customers in the queue.", "Empty Queue", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                }
            }

            return result;
        }

        protected void InitializePriorityPendingCustomersForStaff()
        {
            List<PriorityPendingCustomers> priorityqueuependingcustomers = RetrievePriorityGeneralAndPreferredQuePendingCustomersFromDB();

            if (priorityqueuependingcustomers.Count == 0)
            {
                NoCustomerInQueueUserControl nocustomerusercontrol = new NoCustomerInQueueUserControl();
                StaffPriorityQueueCurrentCustomersStatusFlowLayoutPanel.Controls.Add(nocustomerusercontrol);

            }

            int smallestQueNumber2 = int.MaxValue;

            foreach (PriorityPendingCustomers customer in priorityqueuependingcustomers)
            {
                StaffCurrentAvailableCustomersUserControl availablecustomersusercontrol = new StaffCurrentAvailableCustomersUserControl(this);
                availablecustomersusercontrol.AvailablePriorityCustomerSetData(customer);
                availablecustomersusercontrol.ExpandUserControlButtonClicked += AvailableCustomersUserControl_ExpandCollapseButtonClicked;
                availablecustomersusercontrol.StartServiceButtonClicked += AvailableCustomersUserControl_StartServiceButtonClicked;
                availablecustomersusercontrol.StaffEndServiceBtnClicked += AvailableCustomersUserControl_EndServiceButtonClicked;
                StaffPriorityQueueCurrentCustomersStatusFlowLayoutPanel.Controls.Add(availablecustomersusercontrol);
                availablecustomersusercontrol.CurrentStaffID = StaffIDNumLbl.Text;

                string queNumberText = availablecustomersusercontrol.StaffQueNumberTextBox.Text;
                if (int.TryParse(queNumberText, out int queNumber))
                {
                    if (queNumber < smallestQueNumber2)
                    {
                        smallestQueNumber2 = queNumber;
                    }
                }
            }

            UpdateStartServiceButtonStatusPriority(priorityqueuependingcustomers, smallestQueNumber2);
        }

        private void UpdateStartServiceButtonStatusPriority(List<PriorityPendingCustomers> priorityqueuependingcustomers, int smallestQueNumber)
        {
            foreach (System.Windows.Forms.Control control in StaffPriorityQueueCurrentCustomersStatusFlowLayoutPanel.Controls)
            {
                if (control is StaffCurrentAvailableCustomersUserControl userControl)
                {
                    int queNumber;
                    int.TryParse(userControl.StaffQueNumberTextBox.Text, out queNumber);
                    userControl.StaffStartServiceBtn.Enabled = (queNumber == smallestQueNumber);
                }
            }
        }
        #endregion





        #endregion




        private void StaffServiceRateTestBtn_Click(object sender, EventArgs e)
        {

        }

        private void MngrApptServiceBtn_Click(object sender, EventArgs e)
        {
            Inventory.PanelShow(MngrApptServicePanel);
        }

        private void MngrApptServiceExitBtn_Click(object sender, EventArgs e)
        {
            MngrAppSalesSelectedPeriodLbl.Visible = true;
            MngrAppSalesSelectedPeriodText.Visible = true;
            MngrAppSalesFromLbl.Visible = false;
            MngrAppSalesFromDatePicker.Visible = false;
            MngrAppSalesToLbl.Visible = false;
            MngrAppSalesToDatePicker.Visible = false;
            MngrAppSalesPeriodCalendar.Visible = false;
            MngrAppSalesPeriod.SelectedItem = null;
            MngrAppSalesSelectCatBox.SelectedItem = null;
            MngrAppSalesSelectedPeriodText.Text = "";
            MngrAppSalesTransIDShow.Text = "";
            MngrAppSalesTransRepDGV.DataSource = null;
            MngrAppSalesTransServiceHisDGV.DataSource = null;
            MngrAppSalesGraph.Series.Clear();
            MngrAppSalesGraph.Legends.Clear();
            Inventory.PanelShow(MngrInventoryTypePanel);

        }

        private void RecCanceltApptTransactionBtn_Click(object sender, EventArgs e)
        {
            if (RecApptAcceptLateDeclineDGV.SelectedRows.Count > 0)
            {
                // Get the selected transaction ID from the DataGridView
                string transactionID = RecApptAcceptLateDeclineDGV.SelectedRows[0].Cells["TransactionID"].Value.ToString();

                // Ask for confirmation before canceling the appointment
                DialogResult result = MessageBox.Show("Are you sure you want to cancel the appointment?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // User confirmed, proceed with cancellation
                    using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                    {
                        connection.Open();

                        string updateQuery = $"UPDATE appointment SET AppointmentStatus = 'Cancelled' WHERE TransactionNumber = '{transactionID}'";
                        MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection);
                        int rowsAffected = updateCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            RecApptAcceptLateDeclineDGV.Rows.Clear();
                            InitializeAppointmentDataGrid();
                            MessageBox.Show("Appointment successfully cancelled.");
                        }
                        else
                        {
                            MessageBox.Show("Failed to cancel appointment. Please try again.");
                        }
                    }
                }
                else
                {
                    // User cancelled the operation
                    MessageBox.Show("Appointment cancellation cancelled.");
                }
            }
            else
            {
                MessageBox.Show("Please select a transaction number.");
            }
        }

        
    }
}