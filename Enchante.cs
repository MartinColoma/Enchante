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
using System.Web.Util;
using System.Web.UI;
using System.Runtime.Remoting.Messaging;

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
        private string[] SalesCategories = { "Hair Styling", "Face & Skin", "Nail Care", "Massage", "Spa" };
        private string[] BestCategories = { "Hair Styling", "Face & Skin", "Nail Care", "Massage", "Spa", "Top Service Category" };

        //picture slide landing page
        private int currentIndex = 0;
        private System.Drawing.Image[] images = { Properties.Resources.Enchante_Bldg,  Properties.Resources.Hair,
                                    Properties.Resources.Olga_Collection, Properties.Resources.download,
                                    Properties.Resources.Green___Gold_Collection___Salon_Equipment_Centre}; // Replace with your resource names

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
            ParentPanelShow = new ParentCard(EnchanteHomePage, EnchanteReceptionPage, EnchanteAdminPage, EnchanteMngrPage);
            Transaction = new ReceptionTransactionCard(RecQueStartPanel, RecWalkinPanel, RecApptPanel, RecPayServicePanel, RecQueWinPanel, RecShopProdPanel, RecApptConfirmPanel);
            Inventory = new MngrInventoryCard(MngrInventoryTypePanel, MngrServicesPanel, MngrServiceHistoryPanel, MngrInventoryMembershipPanel,
                                            MngrInventoryProductsPanel, MngrInventoryProductHistoryPanel, MngrPromoPanel, MngrWalkinSalesPanel, MngrIndemandPanel, MngrWalkinProdSalesPanel, MngrApptServicePanel);




            //icon tool tip
            iconToolTip = new System.Windows.Forms.ToolTip();
            iconToolTip.IsBalloon = true;



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



            MngrProductSalesPeriod.Items.AddRange(SalesDatePeriod);
            MngrProductSalesSelectCatBox.Items.AddRange(SalesCategories);

            MngrAppSalesPeriod.Items.AddRange(SalesDatePeriod);
            MngrAppSalesSelectCatBox.Items.AddRange(SalesCategories);


            MngrPDHistoryStatusBox.Items.Add("Paid");
            MngrPDHistoryStatusBox.Items.Add("Not Paid");

            MngrPDHistoryItemCatBox.Items.Add("Hair Styling");
            MngrPDHistoryItemCatBox.Items.Add("Face & Skin");
            MngrPDHistoryItemCatBox.Items.Add("Nail Care");
            MngrPDHistoryItemCatBox.Items.Add("Massage");
            MngrPDHistoryItemCatBox.Items.Add("Spa");

            MngrSVHistoryServiceStatusBox.Items.Add("Completed");
            MngrSVHistoryServiceStatusBox.Items.Add("Pending");
            MngrSVHistoryServiceStatusBox.Items.Add("In Session");
            MngrSVHistoryServiceStatusBox.Items.Add("Cancelled");

            MngrSVHistoryServiceCatBox.Items.Add("Hair Styling");
            MngrSVHistoryServiceCatBox.Items.Add("Face & Skin");
            MngrSVHistoryServiceCatBox.Items.Add("Nail Care");
            MngrSVHistoryServiceCatBox.Items.Add("Massage");
            MngrSVHistoryServiceCatBox.Items.Add("Spa");

            MngrSVHistoryTransTypeBox.Items.Add("Walk-in Transaction");
            MngrSVHistoryTransTypeBox.Items.Add("Walk-in Appointment Transaction");

            MngrMemAccMemTypeBox.Items.Add("Regular");
            MngrMemAccMemTypeBox.Items.Add("PREMIUM");
            MngrMemAccMemTypeBox.Items.Add("SVIP");

            MngrVoucherPromoCategoryComboBox.Items.AddRange(SalesCategories);


            ProductHistoryShow();
            ServiceHistoryShow();
            MemberAccountsShow();
            PopulateRequiredItemsComboBox();
            VouchersShow();
            PromoCodeGenerator();
            ReceptionLoadServices();
            MngrInventoryProductData();
            PopulateUserInfoDataGrid();

            //Tab Header remover
            WalkinTabs.SizeMode = TabSizeMode.Fixed;
            WalkinTabs.ItemSize = new Size(0, 1);
            ApptTabs.SizeMode = TabSizeMode.Fixed;
            ApptTabs.ItemSize = new Size(0, 1);


        }

        private void Enchante_Load(object sender, EventArgs e)
        {
            //Reset Panel to Show Default
            ParentPanelShow.PanelShow(EnchanteHomePage);
            DateTimePickerTimer.Interval = 1000;
            DateTimePickerTimer.Start();

        }

        //database-related methods
        #region
        
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

        //
        #region customized dgv on receptionist dashboard
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

            //DataGridViewCheckBoxColumn checkboxColumn = new DataGridViewCheckBoxColumn();
            //checkboxColumn.HeaderText = "Senior\nPWD\nDiscount";
            //checkboxColumn.Name = "CheckBoxColumn";
            //checkboxColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            //checkboxColumn.Width = 15;
            //RecShopProdSelectedProdDGV.Columns.Add(checkboxColumn);

        }
        #endregion

        // Enchante Home Landing Page Starts Here
        #region
        private void EnchanteHomeScrollPanel_Click(object sender, EventArgs e)
        {
            //Reset Panel to Show Default
        }


        private void MngrHomePanelReset()
        {
            ParentPanelShow.PanelShow(EnchanteMngrPage);
            ReceptionLoadServices();
            MngrDataTimer.Start();
            MngrServiceDataColor();
        }
        private void ReceptionHomePanelReset()
        {
            ParentPanelShow.PanelShow(EnchanteReceptionPage);
            InitialWalkinTransColor();
            RecTransTimer.Start();

        }

        private void AdminHomePanelReset()
        {
            ParentPanelShow.PanelShow(EnchanteAdminPage);

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
        private void PictureSlideTimer_Tick(object sender, EventArgs e)
        {

            DisplayNextImage();

        }

        private void DisplayNextImage()
        {
            // Load the next image
            System.Drawing.Image image = images[currentIndex];
            EDP1.Image = image;

            // Increment the index, looping back to the beginning if necessary
            currentIndex = (currentIndex + 1) % images.Length;
        }
        private void ScrollToCoordinates(int x, int y)
        {
            // Set the AutoScrollPosition to the desired coordinates
            EnchanteHomeScrollPanel.AutoScrollPosition = new Point(x, y);
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
                MngrEmplTypeLbl.Text = "Manager";
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
                RecEmplTypeLbl.Text = "Receptionist";

                RecWalkinBdayMaxDate();
                RecApptBdayMaxDate();
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
                                        MngrNameLbl.Text = name + " " + lastname;
                                        MngrIDNumLbl.Text = ID;
                                        MngrEmplTypeLbl.Text = membertype;

                                        MngrHomePanelReset();
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
                                        RecEmplTypeLbl.Text = membertype;

                                        ReceptionHomePanelReset();
                                        RecWalkinBdayMaxDate();
                                        RecApptBdayMaxDate();
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
            ShowHidePassBtn.IconChar = FontAwesome.Sharp.IconChar.EyeSlash;

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
                ParentPanelShow.PanelShow(EnchanteHomePage);
                RecQueueStartPanel.Controls.Clear();
                RecApptAcceptLateDeclineDGV.Rows.Clear();
                membercategory = "";

                AdminUserAccPanel.Visible = false;

                RecWalkinTransactionClear();
                WalkinTabs.SelectedIndex = 0;
                RecApptTransactionClear();
                ApptTabs.SelectedIndex = 0;
                RecShopProdTransactionClear();


                RecTransTimer.Stop();
                RecQueTimer.Stop();
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
            //if (ReceptionUserAccPanel.Visible == false)
            //{
            //    ReceptionUserAccPanel.Visible = true;

            //}
            //else
            //{
            //    ReceptionUserAccPanel.Visible = false;
            //}
        }

        private void RecWalkInBtn_Click(object sender, EventArgs e)
        {
            WalkinTabs.SelectedIndex = 0;
            RecApptTransactionClear();
            RecShopProdTransactionClear();
            InitialWalkinTransColor();
            RecWalkinBdayMaxDate();
            serviceappointment = false;
            InitializeEmployeeCategory();
            RefreshAvailableStaff();
        }

        private void RecWalkinBdayMaxDate()
        {
            DateTime currentDate = DateTime.Today;
            DateTime maxDate = currentDate.AddYears(-4); // Calculate 2 years ago from today

            // Set the MaxDate property
            RecWalkinBdayPicker.MaxDate = maxDate;

            // Convert maxDate to the desired format and set it as the initial value
            RecWalkinBdayPicker.Value = DateTime.ParseExact(maxDate.ToString("MMMM dd, yyyy"), "MMMM dd, yyyy", null);

            DateTime selectedDate = RecWalkinBdayPicker.Value;
            int age = DateTime.Now.Year - selectedDate.Year;

            if (DateTime.Now < selectedDate.AddYears(age))
            {
                age--; // Subtract 1 if the birthday hasn't occurred yet this year
            }
            RecWalkinAgeBox.Text = age.ToString();
            if (age < 4)
            {
                RecWalkinAgeErrorLbl.Visible = true;
                RecWalkinAgeErrorLbl.Text = "Must be 4yrs old\nand above";
                return;
            }
            else
            {
                RecWalkinAgeErrorLbl.Visible = false;

            }

        }

        private void RecApptBdayMaxDate()
        {
            DateTime currentDate = DateTime.Today;
            DateTime maxDate = currentDate.AddYears(-18); // Calculate 18 years ago from today

            // Set the MaxDate property
            RecApptClientBdayPicker.MaxDate = maxDate;

            // Convert maxDate to the desired format and set it as the initial value
            RecApptClientBdayPicker.Value = DateTime.ParseExact(maxDate.ToString("MMMM dd, yyyy"), "MMMM dd, yyyy", null);

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



        private void InitialWalkinTransColor()
        {
            Transaction.PanelShow(RecWalkinPanel);
            RecWalkinTransNumText.Text = TransactionNumberGenerator.WalkinGenerateTransNumberDefault();

            //light yellow bg, green text and fg
            RecWalkInBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            RecWalkInBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            RecWalkInBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));

            RecAppointmentBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            RecAppointmentBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            RecAppointmentBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            RecApptConfirmBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            RecApptConfirmBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            RecApptConfirmBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            RecPayServiceBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            RecPayServiceBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            RecPayServiceBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            RecShopProdBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            RecShopProdBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            RecShopProdBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            RecQueBtnResetColor();

        } //216, 213, 178 89, 136, 82

        private void RecTransBtnResetColor()
        {
            RecWalkInBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            RecWalkInBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            RecWalkInBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            RecAppointmentBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            RecAppointmentBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            RecAppointmentBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            RecApptConfirmBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            RecApptConfirmBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            RecApptConfirmBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            RecPayServiceBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            RecPayServiceBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            RecPayServiceBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            RecShopProdBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            RecShopProdBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            RecShopProdBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
        }

        private void ApptTransColor()
        {
            Transaction.PanelShow(RecApptPanel);
            RecApptTransNumText.Text = TransactionNumberGenerator.AppointGenerateTransNumberDefault();

            RecAppointmentBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            RecAppointmentBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            RecAppointmentBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));

            RecWalkInBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            RecWalkInBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            RecWalkInBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            RecApptConfirmBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            RecApptConfirmBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            RecApptConfirmBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            RecPayServiceBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            RecPayServiceBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            RecPayServiceBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            RecShopProdBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            RecShopProdBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            RecShopProdBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            RecQueBtnResetColor();
        }
        private void ApptConfirmTransColor()
        {
            Transaction.PanelShow(RecApptConfirmPanel);
            RecWalkinTransNumText.Text = TransactionNumberGenerator.WalkinGenerateTransNumberDefault();

            RecApptConfirmBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            RecApptConfirmBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            RecApptConfirmBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));

            RecWalkInBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            RecWalkInBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            RecWalkInBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            RecAppointmentBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            RecAppointmentBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            RecAppointmentBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            RecPayServiceBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            RecPayServiceBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            RecPayServiceBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            RecShopProdBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            RecShopProdBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            RecShopProdBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            RecQueBtnResetColor();
        }
        private void PaymentTransColor()
        {
            Transaction.PanelShow(RecPayServicePanel);
            RecWalkinTransNumText.Text = TransactionNumberGenerator.WalkinGenerateTransNumberDefault();

            RecPayServiceBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            RecPayServiceBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            RecPayServiceBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));

            RecWalkInBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            RecWalkInBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            RecWalkInBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            RecAppointmentBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            RecAppointmentBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            RecAppointmentBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            RecApptConfirmBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            RecApptConfirmBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            RecApptConfirmBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            RecShopProdBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            RecShopProdBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            RecShopProdBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            RecQueBtnResetColor();
        }
        private void ShopProdTransColor()
        {
            Transaction.PanelShow(RecShopProdPanel);
            RecShopProdTransNumText.Text = TransactionNumberGenerator.ShopProdGenerateTransNumberDefault();

            RecShopProdBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            RecShopProdBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            RecShopProdBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));

            RecWalkInBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            RecWalkInBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            RecWalkInBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            RecAppointmentBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            RecAppointmentBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            RecAppointmentBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            RecApptConfirmBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            RecApptConfirmBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            RecApptConfirmBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            RecPayServiceBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            RecPayServiceBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            RecPayServiceBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            RecQueBtnResetColor();
        }
        //ApptMember
        private void RecAppointmentBtn_Click(object sender, EventArgs e)
        {
            ApptTabs.SelectedIndex = 0;
            RecWalkinTransactionClear();
            RecShopProdTransactionClear();
            ApptTransColor();
            RecApptBookingTimeComboBox.Items.Clear();
            LoadBookingTimes();
            RecApptBookingDatePicker.MinDate = DateTime.Today;
            RecApptBdayMaxDate();
            isappointment = true;
            RecQueBtnResetColor();
            serviceappointment = true;
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

        #endregion

        #region Receptionist Walk-in Transaction
        private void RecWalkInExitBtn_Click(object sender, EventArgs e)
        {
            //RecWalkinTransactionClear();
            WalkinTabs.SelectedIndex = 0;

        }
        private void RecWalkInCatHSBtn_Click(object sender, EventArgs e)
        {
            filterstaffbyservicecategory = "Hair Styling";
            servicecurrentPage = 0;
            servicecurrentPagefake = 1;
            RecWalkinServicesFlowLayoutPanel.Controls.Clear();
            InitializeServices(filterstaffbyservicecategory);
            serviceappointment = false;
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
            servicecurrentPage = 0;
            servicecurrentPagefake = 1;
            RecWalkinServicesFlowLayoutPanel.Controls.Clear();
            InitializeServices(filterstaffbyservicecategory);
            serviceappointment = false;
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
            servicecurrentPage = 0;
            servicecurrentPagefake = 1;
            RecWalkinServicesFlowLayoutPanel.Controls.Clear();
            InitializeServices(filterstaffbyservicecategory);
            serviceappointment = false;
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
            servicecurrentPage = 0;
            servicecurrentPagefake = 1;
            RecWalkinServicesFlowLayoutPanel.Controls.Clear();
            InitializeServices(filterstaffbyservicecategory);
            serviceappointment = false;
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
            servicecurrentPage = 0;
            servicecurrentPagefake = 1;
            RecWalkinServicesFlowLayoutPanel.Controls.Clear();
            InitializeServices(filterstaffbyservicecategory);
            serviceappointment = false;
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

        //ditotayo
        private void SearchAcrossCategories(string searchText, string filterstaffbyservicecategory)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                {
                    connection.Open();

                    string sql = "SELECT ServiceID, Name, Duration, Category, Price FROM `services` WHERE Category = @category AND " +
                                 "(Name LIKE @searchText OR " +
                                 "Duration LIKE @searchText OR " +
                                 "Price LIKE @searchText)";

                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("@searchText", "%" + searchText + "%");
                    cmd.Parameters.AddWithValue("@category", filterstaffbyservicecategory);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        RecWalkinServicesFlowLayoutPanel.Controls.Clear();

                        while (reader.Read())
                        {
                            Services service = new Services();

                            // Retrieve the data from the reader and populate the 'service' object
                            service.ServiceName = reader["Name"].ToString();
                            service.ServiceID = reader["ServiceID"].ToString();
                            service.ServiceDuration = reader["Duration"].ToString();
                            service.ServicePrice = reader["Price"].ToString();
                            service.ServiceCategory = reader["Category"].ToString();

                            ServicesUserControl servicesusercontrol = new ServicesUserControl(this);
                            servicesusercontrol.SetServicesData(service);
                            servicesusercontrol.ServiceUserControl_Clicked += ServiceUserControl_Clicked;
                            servicesusercontrol.RecServicePriceTextBox_Clicked += ServiceUserControl_Clicked;
                            servicesusercontrol.RecServiceDurationTextBox_Clicked += ServiceUserControl_Clicked;
                            servicesusercontrol.RecServiceNameTextBox_Clicked += ServiceUserControl_Clicked;

                            RecWalkinServicesFlowLayoutPanel.Controls.Add(servicesusercontrol);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred: " + e.Message, "Error");
            }
        }


        private void RecWalkInSearchServiceTypeText_TextChanged(object sender, EventArgs e)
        {
            string searchKeyword = RecWalkinSearchServiceTypeText.Text.Trim().ToLower();
            InitializeServices2(filterstaffbyservicecategory,searchKeyword);
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
        public int age;
        public void QueTypeIdentifier(DataGridViewCell QueType)
        {

            if (isappointment)
            {
                string agetext = RecApptClientAgeText.Text.Trim();
                age = Convert.ToInt32(agetext);
            }
            else
            {
                string agetext = RecWalkinAgeBox.Text.Trim();
                age = Convert.ToInt32(agetext);
            }

            if (isappointment == true && RecApptAnyStaffToggleSwitch.Checked)
            {
                if (age >= 60)
                {
                    QueType.Value = "Senior-Anyone-Priority";
                }
                else
                {
                    QueType.Value = "Anyone-Priority";
                }

            }
            else if (isappointment == true && RecApptPreferredStaffToggleSwitch.Checked)
            {
                if (age >= 60)
                {
                    QueType.Value = "Senior-Preferred-Priority";
                }
                else
                {
                    QueType.Value = "Preferred-Priority";
                }

            }
            else if (selectedStaffID == "Anyone")
            {
                if (age >= 60)
                {
                    QueType.Value = "Senior-Anyone";
                }
                else
                {
                    QueType.Value = "Anyone";
                }
            }
            else
            {
                if (age >= 60)
                {
                    QueType.Value = "Senior-Preferred";
                }
                else
                {
                    QueType.Value = "Preferred";
                }

            }
        }

        private void RecDeleteSelectedServiceAndStaffBtn_Click(object sender, EventArgs e)
        {
            if (RecWalkinSelectedServiceDGV.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to void these services?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    RecWalkinSelectedServiceDGV.Rows.Clear();
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

            if (RecWalkinSelectedServiceDGV != null && RecWalkinSelectedServiceDGV.Rows.Count == 0)
            {
                MessageBox.Show("Select a service first to proceed on booking a transaction.", "Ooooops!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                RecWalkinOrdersDB(RecWalkinSelectedProdDGV);
                RecWalkinOrderProdHistoryDB(RecWalkinSelectedProdDGV);
                RecWalkinServiceHistoryDB(RecWalkinSelectedServiceDGV); //service history db
                ReceptionistWalk_in_AppointmentDB(); //walk-in transaction db
                RecWalkinTransactNumRefresh();
                WalkinTabs.SelectedIndex = 0;
                RecWalkinTransactionClear();
            }
        }

        private void RecWalkinTransactionClear()
        {
            WalkinTabs.SelectedIndex = 0;
            RecWalkinFNameText.Text = "";
            RecWalkinLNameText.Text = "";
            RecWalkinCPNumText.Text = "";
            RecWalkinAgeBox.Text = "";
            RecWalkinCatHSRB.Visible = false;
            RecWalkinCatHSRB.Checked = false;
            RecWalkinCatFSRB.Visible = false;
            RecWalkinCatNCRB.Visible = false;
            RecWalkinCatSpaRB.Visible = false;
            RecWalkinCatMassageRB.Visible = false;
            RecWalkinCatFSRB.Checked = false;
            RecWalkinCatNCRB.Checked = false;
            RecWalkinCatSpaRB.Checked = false;
            RecWalkinCatMassageRB.Checked = false;
            RecWalkinBdayMaxDate();
            RecWalkinAgeBox.Text = "Age";

            RecWalkinSelectedServiceDGV.Rows.Clear();
            RecWalkinSelectedProdDGV.Rows.Clear();
            RecWalkinAnyStaffToggleSwitch.Checked = false;
            RecWalkinPreferredStaffToggleSwitch.Checked = false;

        }


        private void QueueNumReceiptGenerator() // to be discarded
        {
            DateTime currentDate = RecDateTimePicker.Value;
            string datetoday = currentDate.ToString("MM-dd-yyyy dddd");
            string timePrinted = currentDate.ToString("hh:mm tt");
            string timePrintedFile = currentDate.ToString("hh-mm-ss");
            string transactNum = RecPayServiceWalkinTransactNumLbl.Text;
            string clientName = RecPayServiceWalkinClientNameLbl.Text;
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
                    centerAligned.Add(new Chunk("\n69th flr. Enchanté Bldg. Ortigas Ave. Ext.\nManggahan, Pasig City 1611 Philippines", font));
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
                    foreach (DataGridViewRow row in RecPayServiceWalkinAcquiredDGV.Rows)
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
                    decimal netAmount = decimal.Parse(RecPayServiceWalkinNetAmountBox.Text);
                    decimal discount = decimal.Parse(RecPayServiceWalkinDiscountBox.Text);
                    decimal vat = decimal.Parse(RecPayServiceWalkinVATBox.Text);
                    decimal grossAmount = decimal.Parse(RecPayServiceWalkinGrossAmountBox.Text);
                    decimal cash = decimal.Parse(RecPayServiceWalkinCashBox.Text);
                    decimal change = decimal.Parse(RecPayServiceWalkinChangeBox.Text);

                    // Create a new table for the "Total" section
                    PdfPTable totalTable = new PdfPTable(2); // 2 columns for the "Total" table
                    totalTable.SetWidths(new float[] { 5f, 3f }); // Column widths
                    totalTable.DefaultCell.Border = PdfPCell.NO_BORDER;

                    // Add cells to the "Total" table
                    totalTable.AddCell(new Phrase($"Total # of Service ({RecPayServiceWalkinAcquiredDGV.Rows.Count})", font));
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
            //else
            //{
            //    MessageBox.Show("No products bought.", "Product");
            //}

        }
        private void RecWalkinOrdersDB(DataGridView RecWalkinOrderHistoryView)
        {
            DateTime currentDate = RecDateTimePicker.Value;
            string transactionNum = RecWalkinTransNumText.Text;
            string status = "Not Paid";
            string type = "Walk-in Transaction Checked Out";
            //booked values
            string bookedDate = currentDate.ToString("MM-dd-yyyy dddd"); //bookedDate
            string bookedTime = currentDate.ToString("hh:mm tt"); //bookedTime
            string bookedBy = RecNameLbl.Text; //booked by

            //basic info
            string CustomerName = RecWalkinFNameText.Text + " " + RecWalkinLNameText.Text; //client name
            string cpnum = RecWalkinCPNumText.Text; //client name

            if (RecWalkinSelectedProdDGV.Rows.Count > 0)
            {
                try
                {
                    using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                    {
                        connection.Open();
                        string query = "INSERT INTO orders (TransactionNumber, TransactionType, ProductStatus, Date, Time, CheckedOutBy, ClientName, ClientCPNum) " +
                                                "VALUES (@Transact, @type, @status, @date, @time, @OrderedBy, @client, @clientCPnum)";

                        using (MySqlCommand cmd = new MySqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@Transact", transactionNum);
                            cmd.Parameters.AddWithValue("@type", type);
                            cmd.Parameters.AddWithValue("@status", status);
                            cmd.Parameters.AddWithValue("@date", bookedDate);
                            cmd.Parameters.AddWithValue("@time", bookedTime);
                            cmd.Parameters.AddWithValue("@OrderedBy", bookedBy);
                            cmd.Parameters.AddWithValue("@client", CustomerName);
                            cmd.Parameters.AddWithValue("@clientCPnum", cpnum);

                            cmd.ExecuteNonQuery();
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
            //else
            //{
            //    MessageBox.Show("No products bought.", "Product");
            //}

        }

        private void ReceptionistWalk_in_AppointmentDB()
        {
            DateTime currentDate = RecDateTimePicker.Value;
            string transactionNum = RecWalkinTransNumText.Text;
            string serviceStatus = "Pending";

            //basic info
            string CustomerName = RecWalkinFNameText.Text + " " + RecWalkinLNameText.Text; //client name
            string CustomerMobileNumber = RecWalkinCPNumText.Text; //client cp num
            string bday = RecWalkinBdayPicker.Value.ToString("MMMM dd, yyyy");
            string age = RecWalkinAgeBox.Text;
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
                                        "ClientName, ClientCPNum, ClientBday, ClientAge, ServiceDuration, BookedBy, BookedDate, BookedTime)" +
                                        "VALUES (@Transact, @status, @appointDate, @appointTime, @clientName, @clientCP, @clientBday, @clientAge, @duration, @bookedBy, @bookedDate, @bookedTime)";

                    MySqlCommand cmd = new MySqlCommand(insertQuery, connection);
                    cmd.Parameters.AddWithValue("@Transact", transactionNum);
                    cmd.Parameters.AddWithValue("@status", serviceStatus);
                    cmd.Parameters.AddWithValue("@appointDate", bookedDate);
                    cmd.Parameters.AddWithValue("@appointTime", bookedTime);
                    cmd.Parameters.AddWithValue("@clientName", CustomerName);
                    cmd.Parameters.AddWithValue("@clientCP", CustomerMobileNumber);
                    cmd.Parameters.AddWithValue("@clientBday", bday);
                    cmd.Parameters.AddWithValue("@clientAge", age);
                    cmd.Parameters.AddWithValue("@duration", "00:00:00");
                    cmd.Parameters.AddWithValue("@bookedBy", bookedBy);
                    cmd.Parameters.AddWithValue("@bookedDate", bookedDate);
                    cmd.Parameters.AddWithValue("@bookedTime", bookedTime);


                    cmd.ExecuteNonQuery();
                }

                // Successful insertion
                MessageBox.Show("Service successfully booked.", "Hooray!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                WalkinTabs.SelectedIndex = 0;
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
                                string serviceCat = row.Cells["ServiceCategories"].Value.ToString();
                                string serviceID = row.Cells["ServiceID"].Value.ToString();
                                decimal servicePrice = Convert.ToDecimal(row.Cells["ServicePrices"].Value);
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
                        RecWalkinProdCalculateTotalPrice();
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
                    RecWalkinProdCalculateTotalPrice();

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

        private int currentPage = 0;
        private int currentPagefake = 1;
        private int itemsPerPage = 10;
        private int totalItems = 0;
        private int totalPages = 0;

        public bool product;
        public void InitializeProducts()
        {
            RecShopProdProductFlowLayoutPanel.Controls.Clear();
            RecWalkinProductFlowLayoutPanel.Controls.Clear();

            using (MySqlConnection connection = new MySqlConnection(mysqlconn))
            {
                connection.Open();

                string countQuery = "SELECT COUNT(*) FROM inventory WHERE ProductType = 'Retail Product'";
                MySqlCommand countCommand = new MySqlCommand(countQuery, connection);
                totalItems = Convert.ToInt32(countCommand.ExecuteScalar());

                totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

                int offset = currentPage * itemsPerPage;
                ProductPageLbl.Text = $"{currentPagefake} / {totalPages}";
                WalkinProductPageLbl.Text = $"{currentPagefake} / {totalPages}";

                string query = $@"SELECT ItemID, ItemName, ItemStock, ItemPrice, ItemStatus, ProductPicture 
                  FROM inventory 
                  WHERE ProductType = 'Retail Product' 
                  LIMIT {offset}, {itemsPerPage}";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();
                Size userControlSize = new Size(419, 90);
                //by three 278,56

                while (reader.Read())
                {
                    string itemID = reader["ItemID"].ToString();
                    string itemName = reader["ItemName"].ToString();
                    string itemStock = reader["ItemStock"].ToString();
                    string itemPrice = reader["ItemPrice"].ToString();
                    string itemStatus = reader["ItemStatus"].ToString();
                    byte[] productPicture = (byte[])reader["ProductPicture"];

                    if (product)
                    {
                        ProductUserControl recshopproductusercontrol = new ProductUserControl();

                        //recshop product
                        recshopproductusercontrol.Size = userControlSize;
                        recshopproductusercontrol.ProductNameTextBox.Size = new Size(235, 33);
                        recshopproductusercontrol.ProductPriceTextBox.Size = new Size(90, 27);
                        recshopproductusercontrol.ProductPicturePictureBox.Size = new Size(72, 72);
                        recshopproductusercontrol.ProductNameTextBox.Location = new Point(90, 29);
                        recshopproductusercontrol.ProductPriceTextBox.Location = new Point(318, 32);
                        recshopproductusercontrol.PhpSignLbl.Location = new Point(280, 31);
                        recshopproductusercontrol.ProductPicturePictureBox.Location = new Point(16, 9);
                        //Border
                        recshopproductusercontrol.LeftBorder.Size = new Size(5, 100);
                        recshopproductusercontrol.LeftBorder.Location = new Point(-3, 0);
                        recshopproductusercontrol.TopBorder.Size = new Size(425, 5);
                        recshopproductusercontrol.TopBorder.Location = new Point(0, -3);
                        recshopproductusercontrol.RightBorder.Size = new Size(5, 100);
                        recshopproductusercontrol.RightBorder.Location = new Point(417, 0);
                        recshopproductusercontrol.DownBorder.Size = new Size(425, 10);
                        recshopproductusercontrol.DownBorder.Location = new Point(0, 88);

                        recshopproductusercontrol.ProductItemIDTextBox.Text = itemID;
                        recshopproductusercontrol.ProductNameTextBox.Text = itemName;
                        recshopproductusercontrol.ProductStockTextBox.Text = itemStock;
                        recshopproductusercontrol.ProductPriceTextBox.Text = itemPrice;
                        recshopproductusercontrol.ProductStatusTextBox.Text = itemStatus;

                        if (itemStatus == "Low Stock")
                        {
                            recshopproductusercontrol.ProductOutOfStockPictureBox.Visible = true;
                            recshopproductusercontrol.Enabled = false;
                        }
                        else
                        {
                            recshopproductusercontrol.ProductOutOfStockPictureBox.Visible = false;
                            recshopproductusercontrol.Enabled = true;
                        }

                        if (productPicture != null && productPicture.Length > 0)
                        {
                            using (MemoryStream ms = new MemoryStream(productPicture))
                            {
                                System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms);
                                recshopproductusercontrol.ProductPicturePictureBox.Image = image1;
                            }
                        }
                        else
                        {
                            recshopproductusercontrol.ProductPicturePictureBox.Image = null;

                        }
                        foreach (System.Windows.Forms.Control control1 in recshopproductusercontrol.Controls)
                        {
                            control1.Click += RecShopProductControlElement_Click;
                        }

                        recshopproductusercontrol.Click += RecShopProdProductUserControl_Click;

                        RecShopProdProductFlowLayoutPanel.Controls.Add(recshopproductusercontrol);
                    }


                    else
                    {
                        ProductUserControl recwalkinproductusercontrol = new ProductUserControl();
                        //recwalkin product
                        recwalkinproductusercontrol.ProductItemIDTextBox.Text = itemID;
                        recwalkinproductusercontrol.ProductNameTextBox.Text = itemName;
                        recwalkinproductusercontrol.ProductStockTextBox.Text = itemStock;
                        recwalkinproductusercontrol.ProductPriceTextBox.Text = itemPrice;
                        recwalkinproductusercontrol.ProductStatusTextBox.Text = itemStatus;


                        if (itemStatus == "Low Stock")
                        {
                            recwalkinproductusercontrol.ProductOutOfStockPictureBox.Visible = true;
                            recwalkinproductusercontrol.Enabled = false;
                        }
                        else
                        {
                            recwalkinproductusercontrol.ProductOutOfStockPictureBox.Visible = false;
                            recwalkinproductusercontrol.Enabled = true;
                        }

                        if (productPicture != null && productPicture.Length > 0)
                        {
                            using (MemoryStream ms = new MemoryStream(productPicture))
                            {
                                System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
                                recwalkinproductusercontrol.ProductPicturePictureBox.Image = image;
                            }
                        }
                        else
                        {
                            recwalkinproductusercontrol.ProductPicturePictureBox.Image = null;

                        }

                        foreach (System.Windows.Forms.Control control in recwalkinproductusercontrol.Controls)
                        {
                            control.Click += RecWalkinProductControlElement_Click;
                        }


                        recwalkinproductusercontrol.Click += RecWalkinProductUserControl_Click;


                        RecWalkinProductFlowLayoutPanel.Controls.Add(recwalkinproductusercontrol);
                    }


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
                    DialogResult result;
                    result = MessageBox.Show("Do you want to remove this item?", "Remove Item", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        // Remove the selected row
                        RecWalkinSelectedProdDGV.Rows.RemoveAt(e.RowIndex);
                        MessageBox.Show("Item removed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RecWalkinProdCalculateTotalPrice();
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
                            RecWalkinProdCalculateTotalPrice();

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
                        RecWalkinProdCalculateTotalPrice();

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

        private void RecPayServiceWalkinCalculateTotalPrice()
        {
            decimal total1 = 0;
            decimal total2 = 0;
            decimal total3 = 0;

            // Assuming the "ServicePrice" column is of decimal type
            int servicepriceColumnIndex = RecPayServiceWalkinAcquiredDGV.Columns["WalkinServicePrice"].Index;

            foreach (DataGridViewRow row in RecPayServiceWalkinAcquiredDGV.Rows)
            {
                if (row.Cells[servicepriceColumnIndex].Value != null)
                {
                    decimal price = decimal.Parse(row.Cells[servicepriceColumnIndex].Value.ToString());
                    total1 += price;
                }
            }
            RecPayServiceWalkinAcquiredTotalText.Text = total1.ToString("F2");

            // Assuming the "ItemTotalPrice" column is of decimal type
            int productpriceColumnIndex = RecPayServiceWalkinCOProdDGV.Columns["WalkinTotalPrice"].Index;

            foreach (DataGridViewRow row in RecPayServiceWalkinCOProdDGV.Rows)
            {
                if (row.Cells[productpriceColumnIndex].Value != null)
                {
                    decimal price = decimal.Parse(row.Cells[productpriceColumnIndex].Value.ToString());
                    total2 += price;
                }
            }
            RecPayServiceWalkinCOProdTotalText.Text = total2.ToString("F2");


            total3 = total1 + total2;

            // Display the total price in the GrossAmountBox TextBox
            RecPayServiceWalkinGrossAmountBox.Text = total3.ToString("F2"); // Format to two decimal places

            RecWalkinCalculateTotalVATAndNetAmountDB();
            RecWalkinProdCalculateTotalVATAndNetAmountDB();
            RecWalkinServiceCalculateTotalVATAndNetAmountDB();
        }


        public void RecWalkinCalculateTotalVATAndNetAmountDB()
        {
            // Get the Gross Amount from the TextBox (MngrGrossAmountBox)
            if (decimal.TryParse(RecPayServiceWalkinGrossAmountBox.Text, out decimal grossAmount))
            {
                // Fixed VAT rate of 12%
                decimal rate = 12;

                // Calculate the VAT Amount
                decimal netAmount = grossAmount / ((rate / 100) + 1);

                // Calculate the Net Amount
                decimal vatAmount = grossAmount - netAmount;

                // Display the calculated values in TextBoxes
                RecPayServiceWalkinVATBox.Text = vatAmount.ToString("0.00");
                RecPayServiceWalkinNetAmountBox.Text = netAmount.ToString("0.00");
            }

        }
        public void RecWalkinProdCalculateTotalVATAndNetAmountDB()
        {
            // Get the Gross Amount from the TextBox (MngrGrossAmountBox)
            if (decimal.TryParse(RecPayServiceWalkinCOProdTotalText.Text, out decimal grossAmount))
            {
                // Fixed VAT rate of 12%
                decimal rate = 12;

                // Calculate the VAT Amount
                decimal netAmount = grossAmount / ((rate / 100) + 1);

                // Calculate the Net Amount
                decimal vatAmount = grossAmount - netAmount;

                // Display the calculated values in TextBoxes
                RecPayServiceWalkinCOProdVATText.Text = vatAmount.ToString("0.00");
                RecPayServiceWalkinCOProdNetText.Text = netAmount.ToString("0.00");
            }

        }
        public void RecWalkinServiceCalculateTotalVATAndNetAmountDB()
        {
            // Get the Gross Amount from the TextBox (MngrGrossAmountBox)
            if (decimal.TryParse(RecPayServiceWalkinAcquiredTotalText.Text, out decimal grossAmount))
            {
                // Fixed VAT rate of 12%
                decimal rate = 12;

                // Calculate the VAT Amount
                decimal netAmount = grossAmount / ((rate / 100) + 1);

                // Calculate the Net Amount
                decimal vatAmount = grossAmount - netAmount;

                // Display the calculated values in TextBoxes
                RecPayServiceWalkinAcquiredVATText.Text = vatAmount.ToString("0.00");
                RecPayServiceWalkinAcquiredNetText.Text = netAmount.ToString("0.00");
            }

        }

        private void DateTimePickerTimer_Tick(object sender, EventArgs e)
        {
            RecDateTimePicker.Value = DateTime.Now;
            DateTime cashierrcurrentDate = RecDateTimePicker.Value;
            string Cashiertoday = cashierrcurrentDate.ToString("MMMM dd, yyyy dddd hh:mm tt");
            RecDateTimeText.Text = Cashiertoday;
            MngrDateTimeText.Text = Cashiertoday;
        }

        private void RecWalkinCashBox_TextChanged(object sender, EventArgs e)
        {
            if (decimal.TryParse(RecPayServiceWalkinGrossAmountBox.Text, out decimal grossAmount))
            {
                // Get the Cash Amount from the TextBox (MngrCashBox)
                if (decimal.TryParse(RecPayServiceWalkinCashBox.Text, out decimal cashAmount))
                {
                    // Calculate the Change
                    decimal change = cashAmount - grossAmount;
                    decimal walkChange = cashAmount - decimal.Parse(RecPayServiceWalkinAcquiredTotalText.Text);
                    decimal prodChange = walkChange - decimal.Parse(RecPayServiceWalkinCOProdTotalText.Text);
                    // Display the calculated change value in the MngrChangeBox
                    RecPayServiceWalkinChangeBox.Text = change.ToString("0.00");
                    RecPayServiceWalkinAcquiredChangeText.Text = walkChange.ToString("0.00");
                    RecPayServiceWalkinCOProdChangeText.Text = prodChange.ToString("0.00");
                }
                else
                {
                    // Handle invalid input in MngrCashBox, e.g., display an error message
                    RecPayServiceWalkinChangeBox.Text = "Invalid Cash Input";
                    RecPayServiceWalkinAcquiredChangeText.Text = "Invalid Cash Input";
                    RecPayServiceWalkinCOProdChangeText.Text = "Invalid Cash Input";
                }
            }
            else
            {
                // Handle invalid input in MngrGrossAmountBox, e.g., display an error message
                RecPayServiceWalkinChangeBox.Text = "0.00";
            }
        }
        private void RecWalkinGrossAmountBox_TextChanged(object sender, EventArgs e)
        {
            //RecWalkinCalculateTotalVATAndNetAmountDB();
            if (decimal.TryParse(RecPayServiceWalkinGrossAmountBox.Text, out decimal grossAmount))
            {
                // Get the Cash Amount from the TextBox (MngrCashBox)
                if (decimal.TryParse(RecPayServiceWalkinCashBox.Text, out decimal cashAmount))
                {
                    // Calculate the Change
                    decimal change = cashAmount - grossAmount;

                    // Display the calculated change value in the MngrChangeBox
                    RecPayServiceWalkinChangeBox.Text = change.ToString("0.00");
                }
                else
                {
                    // Handle invalid input in MngrCashBox, e.g., display an error message
                    RecPayServiceWalkinChangeBox.Text = "0.00";
                }
            }
            else
            {
                // Handle invalid input in MngrGrossAmountBox, e.g., display an error message
                RecPayServiceWalkinChangeBox.Text = "0.00";
            }
        }


        public void RecWalkinLoadServiceHistoryDB(string transactNumber)
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
                        RecPayServiceWalkinAcquiredDGV.Rows.Clear();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            string service = row["SelectedService"].ToString();
                            string price = row["ServicePrice"].ToString();
                            string staff = row["AttendingStaff"].ToString();



                            // Add a new row to the DataGridView
                            int rowIndex = RecPayServiceWalkinAcquiredDGV.Rows.Add();

                            // Set the values of cells in the DataGridView
                            RecPayServiceWalkinAcquiredDGV.Rows[rowIndex].Cells["WalkinSelectedService"].Value = service;
                            RecPayServiceWalkinAcquiredDGV.Rows[rowIndex].Cells["WalkinServicePrice"].Value = price;
                            RecPayServiceWalkinAcquiredDGV.Rows[rowIndex].Cells["WalkinAttendingStaff"].Value = staff;


                        }


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
                    string sql = "SELECT ItemName, ItemID, Qty, ItemPrice, ItemTotalPrice FROM `orderproducthistory` WHERE TransactionNumber = @TransactionNumber AND ProductStatus = @status";
                    MySqlCommand cmd = new MySqlCommand(sql, connection);

                    // Add parameters to the query
                    cmd.Parameters.AddWithValue("@TransactionNumber", transactNumber);
                    cmd.Parameters.AddWithValue("@status", "Not Paid");

                    System.Data.DataTable dataTable = new System.Data.DataTable();

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                        RecPayServiceWalkinCOProdDGV.Rows.Clear();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            string name = row["ItemName"].ToString();
                            string id = row["ItemID"].ToString();
                            string qty = row["Qty"].ToString();
                            string price = row["ItemPrice"].ToString();
                            string totalPrice = row["ItemTotalPrice"].ToString();


                            // Add a new row to the DataGridView
                            int rowIndex = RecPayServiceWalkinCOProdDGV.Rows.Add();

                            // Set the values of cells in the DataGridView
                            RecPayServiceWalkinCOProdDGV.Rows[rowIndex].Cells["WalkinItemName"].Value = name;
                            RecPayServiceWalkinCOProdDGV.Rows[rowIndex].Cells["WalkinItemID"].Value = id;
                            RecPayServiceWalkinCOProdDGV.Rows[rowIndex].Cells["WalkinQTY"].Value = qty;
                            RecPayServiceWalkinCOProdDGV.Rows[rowIndex].Cells["WalkinPrice"].Value = price;
                            RecPayServiceWalkinCOProdDGV.Rows[rowIndex].Cells["WalkinTotalPrice"].Value = totalPrice;


                        }
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
        private bool serviceHistoryLoaded = false;

        private void RecPayServiceCompleteTransDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if a valid cell is clicked
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Get TransactNumber and OrderNumber from the clicked cell in MngrSalesTable
                string transactNumber = RecPayServiceWalkinCompleteTransDGV.Rows[e.RowIndex].Cells["WalkinTransNum"].Value.ToString();
                string clientName = RecPayServiceWalkinCompleteTransDGV.Rows[e.RowIndex].Cells["WalkinClientName"].Value.ToString();

                RecPayServiceWalkinTransactNumLbl.Text = transactNumber;
                RecPayServiceWalkinClientNameLbl.Text = $"{clientName}";

                // Clear existing data in DGVs before loading new data
                RecPayServiceWalkinAcquiredDGV.Rows.Clear();
                RecPayServiceWalkinCOProdDGV.Rows.Clear();

                // Load service history and order product history based on the clicked transaction number
                RecWalkinLoadServiceHistoryDB(transactNumber);
                RecLoadOrderProdHistoryDB(transactNumber);

                RecPayServiceWalkinCalculateTotalPrice();
                serviceHistoryLoaded = true; // Set the flag to true to indicate that service history is loaded
                RecPayServiceApptTransTypeLbl.Text = "Walk-in";
            }
        }


        private bool ApptserviceHistoryLoaded = false;

        private void RecPayServiceApptCompleteTransDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                string transactNumber1 = RecPayServiceApptCompleteTransDGV.Rows[e.RowIndex].Cells["ApptTransNum"].Value.ToString();
                string clientName1 = RecPayServiceApptCompleteTransDGV.Rows[e.RowIndex].Cells["ApptCustomerName"].Value.ToString();

                RecPayServiceApptTransactNumLbl.Text = transactNumber1;
                RecPayServiceApptClientNameLbl.Text = $"{clientName1}";
                RecPayServiceApptTransTypeLbl.Text = "Appointment";
                RecPayServiceApptAcquiredDGV.Rows.Clear();

                // Load service history only if it hasn't been loaded before
                RecApptLoadServiceHistoryDB(transactNumber1);
                RecPayServiceApptCalculateTotalPrice();
                ApptserviceHistoryLoaded = true; // Set the flag to true to indicate that service history is loaded

            }
        }
        public void RecApptLoadServiceHistoryDB(string transactNumber)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                {
                    connection.Open();

                    // Modify the SQL query to filter based on TransactNumber and OrderNumber
                    string sql = "SELECT SelectedService, AttendingStaff, ServicePrice FROM `servicehistory` WHERE TransactionNumber = @TransactionNumber AND ServiceStatus = @status";
                    MySqlCommand cmd = new MySqlCommand(sql, connection);

                    // Add parameters to the query
                    cmd.Parameters.AddWithValue("@TransactionNumber", transactNumber);
                    cmd.Parameters.AddWithValue("@status", "Completed");

                    System.Data.DataTable dataTable = new System.Data.DataTable();

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);

                        RecPayServiceApptAcquiredDGV.Rows.Clear();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            string service = row["SelectedService"].ToString();
                            string staff = row["AttendingStaff"].ToString();
                            string price = row["ServicePrice"].ToString();


                            // Add a new row to the DataGridView
                            int rowIndex = RecPayServiceApptAcquiredDGV.Rows.Add();

                            // Set the values of cells in the DataGridView
                            RecPayServiceApptAcquiredDGV.Rows[rowIndex].Cells["ApptSelectedService"].Value = service;
                            RecPayServiceApptAcquiredDGV.Rows[rowIndex].Cells["ApptStaffSelected"].Value = staff;
                            RecPayServiceApptAcquiredDGV.Rows[rowIndex].Cells["ApptServicePrice"].Value = price;

                        }

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
        private void RecPayServiceApptCalculateTotalPrice()
        {
            decimal total1 = 0;

            int servicepriceColumnIndex = RecPayServiceApptAcquiredDGV.Columns["ApptServicePrice"].Index;

            foreach (DataGridViewRow row in RecPayServiceApptAcquiredDGV.Rows)
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
            RecPayServiceApptGrossAmountText.Text = total1.ToString("F2");
            // Apply discount (for example, 20% discount)
            decimal initialFee = 0.6m; // 20% discount
            decimal inititalFeeTotal = total1 * (1 - initialFee);
            RecPayServiceApptInitialFeeText.Text = inititalFeeTotal.ToString("F2");
            decimal balance = total1 - inititalFeeTotal;
            RecPayServiceApptRemainingBalText.Text = balance.ToString("F2");
            RecPayServiceApptCalculateTotalVATAndNetAmountDB();

        }

        public void RecPayServiceApptCalculateTotalVATAndNetAmountDB()
        {
            // Get the Gross Amount from the TextBox (MngrGrossAmountBox)
            if (decimal.TryParse(RecPayServiceApptGrossAmountText.Text, out decimal grossAmount))
            {
                // Fixed VAT rate of 12%
                decimal rate = 12;

                // Calculate the VAT Amount
                decimal netAmount = grossAmount / ((rate / 100) + 1);

                // Calculate the Net Amount
                decimal vatAmount = grossAmount - netAmount;

                // Display the calculated values in TextBoxes
                RecPayServiceApptVATText.Text = vatAmount.ToString("0.00");
                RecPayServiceApptNetAmountText.Text = netAmount.ToString("0.00");
            }

        }
        private void RecPayServiceApptChangeCalculateAmount()
        {
            if (decimal.TryParse(RecPayServiceApptRemainingBalText.Text, out decimal grossAmount))
            {
                // Get the Cash Amount from the TextBox (MngrCashBox)
                if (decimal.TryParse(RecPayServiceApptCashText.Text, out decimal cashAmount))
                {
                    // Calculate the Change
                    decimal change = cashAmount - grossAmount;

                    // Display the calculated change value in the MngrChangeBox
                    RecPayServiceApptChangeText.Text = change.ToString("0.00");
                }
                else
                {
                    // Handle invalid input in MngrCashBox, e.g., display an error message
                    RecPayServiceApptChangeText.Text = "Invalid Cash Input";
                }
            }
            else
            {
                // Handle invalid input in MngrGrossAmountBox, e.g., display an error message
                RecPayServiceApptChangeText.Text = "0.00";
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
                    string sql = "SELECT TransactionNumber, ServiceStatus, ClientName, ClientCPNum FROM `walk_in_appointment` " +
                                 "WHERE ServiceStatus = 'Completed' AND AppointmentDate = @todayDate ORDER BY ServiceStatus ";
                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    System.Data.DataTable dataTable = new System.Data.DataTable();
                    cmd.Parameters.AddWithValue("@todayDate", todayDate);

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                        RecPayServiceWalkinCompleteTransDGV.Rows.Clear();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            string transNum = row["TransactionNumber"].ToString();
                            string status = row["ServiceStatus"].ToString();
                            string name = row["ClientName"].ToString();
                            string cpNum = row["ClientCPNum"].ToString();



                            // Add a new row to the DataGridView
                            int rowIndex = RecPayServiceWalkinCompleteTransDGV.Rows.Add();

                            // Set the values of cells in the DataGridView
                            RecPayServiceWalkinCompleteTransDGV.Rows[rowIndex].Cells["WalkinTransNum"].Value = transNum;
                            RecPayServiceWalkinCompleteTransDGV.Rows[rowIndex].Cells["WalkinServiceStatus"].Value = status;
                            RecPayServiceWalkinCompleteTransDGV.Rows[rowIndex].Cells["WalkinClientName"].Value = name;
                            RecPayServiceWalkinCompleteTransDGV.Rows[rowIndex].Cells["WalkinClientCPNum"].Value = cpNum;


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
                    string sql = "SELECT TransactionNumber, ServiceStatus, ClientName, ClientCPNum, AppointmentDate, AppointmentTime " +
                                "FROM `appointment` WHERE ServiceStatus = 'Completed' AND AppointmentDate = @todayDate ORDER BY ServiceStatus ";
                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    System.Data.DataTable dataTable = new System.Data.DataTable();
                    cmd.Parameters.AddWithValue("@todayDate", todayDate);

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);

                        RecPayServiceApptCompleteTransDGV.Rows.Clear();

                        foreach (DataRow row in dataTable.Rows)
                        {
                            string transNum = row["TransactionNumber"].ToString();
                            string status = row["ServiceStatus"].ToString();
                            string name = row["ClientName"].ToString();
                            string cpNum = row["ClientCPNum"].ToString();
                            string apptDate = row["AppointmentDate"].ToString();
                            string appTime = row["AppointmentTime"].ToString();


                            // Add a new row to the DataGridView
                            int rowIndex = RecPayServiceApptCompleteTransDGV.Rows.Add();

                            // Set the values of cells in the DataGridView
                            RecPayServiceApptCompleteTransDGV.Rows[rowIndex].Cells["ApptTransNum"].Value = transNum;
                            RecPayServiceApptCompleteTransDGV.Rows[rowIndex].Cells["ApptServiceStatus"].Value = status;
                            RecPayServiceApptCompleteTransDGV.Rows[rowIndex].Cells["ApptCustomerName"].Value = name;
                            RecPayServiceApptCompleteTransDGV.Rows[rowIndex].Cells["ApptCustomerCPNum"].Value = cpNum;
                            RecPayServiceApptCompleteTransDGV.Rows[rowIndex].Cells["ApptDate"].Value = apptDate;
                            RecPayServiceApptCompleteTransDGV.Rows[rowIndex].Cells["ApptTime"].Value = appTime;

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

        private bool RecPayServiceUpdateWalkinAndOrdersDB()
        {
            // walk-in cash values
            string walkNetAmount = RecPayServiceWalkinAcquiredNetText.Text; // net amount
            string walkVat = RecPayServiceWalkinAcquiredVATText.Text; // vat 
            string walkGrossAmount = RecPayServiceWalkinAcquiredTotalText.Text; // gross amount
            string walkDiscount = ""; // gross amount
            string walkChange = RecPayServiceWalkinAcquiredChangeText.Text;

            //product cash values
            string prodNetAmount = RecPayServiceWalkinCOProdNetText.Text; // net amount
            string prodVat = RecPayServiceWalkinCOProdVATText.Text; // vat 
            string prodGrossAmount = RecPayServiceWalkinCOProdTotalText.Text; // gross amount
            string prodDiscount = ""; // gross amount
            string prodChange = RecPayServiceWalkinCOProdChangeText.Text;

            //string walkNetAmount = RecPayServiceNetAmountBox.Text; // net amount
            //string walkVat = RecPayServiceVATBox.Text; // vat 
            string grossAmount = RecPayServiceWalkinGrossAmountBox.Text; // gross amount

            string discount = RecPayServiceWalkinDiscountBox.Text; // discount
            string cash = RecPayServiceWalkinCashBox.Text; // cash given
            string change = RecPayServiceWalkinChangeBox.Text; // due change
            string paymentMethod = "Cash"; // payment method
            string mngr = RecNameLbl.Text;
            string transactNum = RecPayServiceWalkinTransactNumLbl.Text;

            // bank & wallet details


            try
            {
                using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                {
                    connection.Open();
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
                    else
                    {
                        //walk-in transactions
                        string cashPaymentWalkinTrans = "UPDATE walk_in_appointment SET ServiceStatus = @status, NetPrice = @net, VatAmount = @vat, DiscountAmount = @discount, " +
                                            "GrossAmount = @gross, CashGiven = @cash, DueChange = @change, PaymentMethod = @payment, CheckedOutBy = @mngr " +
                                            "WHERE TransactionNumber = @transactNum"; // cash query
                        MySqlCommand cmd = new MySqlCommand(cashPaymentWalkinTrans, connection);
                        cmd.Parameters.AddWithValue("@status", "Paid");
                        cmd.Parameters.AddWithValue("@net", walkNetAmount);
                        cmd.Parameters.AddWithValue("@vat", walkVat);
                        cmd.Parameters.AddWithValue("@discount", walkDiscount);
                        cmd.Parameters.AddWithValue("@gross", walkGrossAmount);
                        cmd.Parameters.AddWithValue("@cash", cash);
                        cmd.Parameters.AddWithValue("@change", walkChange);
                        cmd.Parameters.AddWithValue("@payment", paymentMethod);
                        cmd.Parameters.AddWithValue("@mngr", mngr);
                        cmd.Parameters.AddWithValue("@transactNum", transactNum);

                        cmd.ExecuteNonQuery();

                        //product walk-in transactions
                        string cashPaymentWalkinProd = "UPDATE orders SET ProductStatus = @status, NetPrice = @net, VatAmount = @vat, DiscountAmount = @discount, " +
                                            "GrossAmount = @gross, CashGiven = @cash, DueChange = @change, PaymentMethod = @payment, CheckedOutBy = @mngr " +
                                            "WHERE TransactionNumber = @transactNum"; // cash query
                        MySqlCommand cmd1 = new MySqlCommand(cashPaymentWalkinProd, connection);
                        cmd1.Parameters.AddWithValue("@status", "Paid");
                        cmd1.Parameters.AddWithValue("@net", prodNetAmount);
                        cmd1.Parameters.AddWithValue("@vat", prodVat);
                        cmd1.Parameters.AddWithValue("@discount", prodDiscount);
                        cmd1.Parameters.AddWithValue("@gross", prodGrossAmount);
                        cmd1.Parameters.AddWithValue("@cash", walkChange);
                        cmd1.Parameters.AddWithValue("@change", prodChange);
                        cmd1.Parameters.AddWithValue("@payment", paymentMethod);
                        cmd1.Parameters.AddWithValue("@mngr", mngr);
                        cmd1.Parameters.AddWithValue("@transactNum", transactNum);

                        cmd1.ExecuteNonQuery();

                        string productPaymentWalkin = "UPDATE orderproducthistory SET ProductStatus = @status WHERE TransactionNumber = @transactNum";

                        MySqlCommand cmd2 = new MySqlCommand(productPaymentWalkin, connection);
                        cmd2.Parameters.AddWithValue("@status", "Paid");
                        cmd2.Parameters.AddWithValue("@transactNum", transactNum);
                        cmd2.ExecuteNonQuery();


                        MessageBox.Show("Service successfully been paid through cash.", "Hooray!", MessageBoxButtons.OK, MessageBoxIcon.Information);


                        ////appointment transactions
                        //string cashPaymentAppt = "UPDATE appointment SET ServiceStatus = @status, NetPrice = @net, VatAmount = @vat, DiscountAmount = @discount, " +
                        //                    "GrossAmount = @gross, CashGiven = @cash, DueChange = @change, PaymentMethod = @payment, CheckedOutBy = @mngr " +
                        //                    "WHERE TransactionNumber = @transactNum"; // cash query
                        //MySqlCommand cmd2 = new MySqlCommand(cashPaymentAppt, connection);
                        //cmd2.Parameters.AddWithValue("@status", "Paid");
                        //cmd2.Parameters.AddWithValue("@net", netAmount);
                        //cmd2.Parameters.AddWithValue("@vat", vat);
                        //cmd2.Parameters.AddWithValue("@discount", discount);
                        //cmd2.Parameters.AddWithValue("@gross", grossAmount);
                        //cmd2.Parameters.AddWithValue("@cash", cash);
                        //cmd2.Parameters.AddWithValue("@change", change);
                        //cmd2.Parameters.AddWithValue("@payment", paymentMethod);
                        //cmd2.Parameters.AddWithValue("@mngr", mngr);
                        //cmd2.Parameters.AddWithValue("@transactNum", transactNum);

                        //cmd2.ExecuteNonQuery();
                        //// Successful update
                        //MessageBox.Show("Service successfully been paid through cash.", "Hooray!", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
            if (RecPayServiceUpdateWalkinAndOrdersDB())
            {
                RecPayServiceUpdateQtyInventory(RecPayServiceWalkinCOProdDGV);
                //RecPayServiceWalkinUpdateOrderProdHistory(RecPayServiceWalkinCOProdDGV);
                RecLoadCompletedWalkinTrans();
                RecLoadCompletedAppointmentTrans();
                RecPayServiceWalkinInvoiceGenerator();
                RecPayServiceClearAllField();
                RecLoadCompletedWalkinTrans();

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

                    foreach (DataGridViewRow row in RecPayServiceWalkinCOProdDGV.Rows)
                    {
                        string itemID = row.Cells["WalkinItemID"].Value.ToString();
                        int qty = Convert.ToInt32(row.Cells["WalkinQTY"].Value);

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
        private void RecPayServiceWalkinUpdateOrderProdHistory(DataGridView dgv)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                {
                    connection.Open();
                    string updateQuery = "UPDATE orderproducthistory SET ProductStatus = @status WHERE ItemID = @ItemID";

                    foreach (DataGridViewRow row in RecPayServiceWalkinCOProdDGV.Rows)
                    {
                        string itemID = row.Cells["ItemID"].Value.ToString();

                        MySqlCommand command = new MySqlCommand(updateQuery, connection);
                        command.Parameters.AddWithValue("@status", "Paid");
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
            RecPayServiceWalkinNetAmountBox.Text = "0.00";
            RecPayServiceWalkinVATBox.Text = "0.00";
            RecPayServiceWalkinDiscountBox.Text = "0.00";
            RecPayServiceWalkinGrossAmountBox.Text = "0.00";
            RecPayServiceWalkinCashBox.Text = "0";
            RecPayServiceWalkinChangeBox.Text = "0.00";




            RecPayServiceWalkinClientNameLbl.Text = "";
            RecPayServiceApptTransTypeLbl.Text = "";
            // Clear rows from RecPayServiceAcquiredDGV
            RecPayServiceWalkinAcquiredDGV.DataSource = null; // Set data source to null
            RecPayServiceWalkinAcquiredDGV.Rows.Clear(); // Clear any remaining rows

            // Clear rows from RecPayServiceCOProdDGV
            RecPayServiceWalkinCOProdDGV.DataSource = null; // Set data source to null
            RecPayServiceWalkinCOProdDGV.Rows.Clear(); // Clear any remaining rows

            RecPayServiceWalkinTransactNumLbl.Text = "Transaction Number";
            RecPayServiceWalkinClientNameLbl.Text = "Client Name";

            RecPayServiceApptClientNameLbl.Text = "";
            RecPayServiceApptTransTypeLbl.Text = "";
            // Clear rows from RecPayServiceAcquiredDGV
            RecPayServiceApptAcquiredDGV.DataSource = null; // Set data source to null
            RecPayServiceApptAcquiredDGV.Rows.Clear(); // Clear any remaining rows



            RecPayServiceApptTransactNumLbl.Text = "Transaction Number";
            RecPayServiceApptClientNameLbl.Text = "Client Name";
        }

        private bool CompletedTransLoad = false;
        private void RecPayServiceBtn_Click(object sender, EventArgs e)
        {
            PaymentTabs.SelectedIndex = 0;
            PaymentTransColor();

            if (!CompletedTransLoad)
            {
                RecLoadCompletedWalkinTrans();
                RecLoadCompletedAppointmentTrans();
                CompletedTransLoad = true; // Set the flag to true to indicate that service history is loaded
            }
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

        private void RecPayServiceWalkinInvoiceGenerator()
        {
            DateTime currentDate = RecDateTimePicker.Value;
            string datetoday = currentDate.ToString("MM-dd-yyyy dddd");
            string timePrinted = currentDate.ToString("hh:mm tt");
            string timePrintedFile = currentDate.ToString("hh-mm-ss");
            string transactNum = RecPayServiceWalkinTransactNumLbl.Text;
            string clientName = RecPayServiceWalkinClientNameLbl.Text;
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
                    Bitmap imagepath = Properties.Resources.Enchante_Logo__200_x_200_px__Green;
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(imagepath, System.Drawing.Imaging.ImageFormat.Png);
                    logo.Alignment = Element.ALIGN_CENTER;
                    logo.ScaleAbsolute(100f, 100f);
                    doc.Add(logo);

                    iTextSharp.text.Font headerFont = FontFactory.GetFont("Courier", 16, iTextSharp.text.Font.BOLD);
                    iTextSharp.text.Font boldfont = FontFactory.GetFont("Courier", 10, iTextSharp.text.Font.BOLD);
                    iTextSharp.text.Font font = FontFactory.GetFont("Courier", 10, iTextSharp.text.Font.NORMAL);
                    iTextSharp.text.Font italic = FontFactory.GetFont("Courier", 10, iTextSharp.text.Font.ITALIC);

                    // Create a centered alignment for text
                    iTextSharp.text.Paragraph centerAligned = new Paragraph();
                    centerAligned.Alignment = Element.ALIGN_CENTER;

                    // Add centered content to the centerAligned Paragraph
                    //centerAligned.Add(new Chunk("Enchanté Salon", headerFont));
                    centerAligned.Add(new Chunk("\n69th flr. Enchanté Bldg. Ortigas Ave. Ext.\nManggahan, Pasig City 1611 Philippines", font));
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

                    foreach (DataGridViewRow row in RecPayServiceWalkinAcquiredDGV.Rows)
                    {
                        try
                        {
                            string serviceName = row.Cells["WalkinSelectedService"].Value?.ToString();
                            if (string.IsNullOrEmpty(serviceName))
                            {
                                continue; // Skip empty rows
                            }

                            string staffID = row.Cells["WalkinAttendingStaff"].Value?.ToString();
                            string itemTotalcost = row.Cells["WalkinServicePrice"].Value?.ToString();

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

                    foreach (DataGridViewRow row in RecPayServiceWalkinCOProdDGV.Rows)
                    {
                        try
                        {
                            string itemName = row.Cells["WalkinItemName"].Value?.ToString();
                            if (string.IsNullOrEmpty(itemName))
                            {
                                continue; // Skip empty rows
                            }
                            string itemID = row.Cells["WalkinItemID"].Value?.ToString();
                            string qty = row.Cells["WalkinQTY"].Value?.ToString();
                            string itemTotalcost = row.Cells["WalkinTotalPrice"].Value?.ToString();

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
                    decimal netAmount = decimal.Parse(RecPayServiceWalkinNetAmountBox.Text);
                    decimal discount = decimal.Parse(RecPayServiceWalkinDiscountBox.Text);
                    decimal vat = decimal.Parse(RecPayServiceWalkinVATBox.Text);
                    decimal grossAmount = decimal.Parse(RecPayServiceWalkinGrossAmountBox.Text);
                    decimal cash = decimal.Parse(RecPayServiceWalkinCashBox.Text);
                    decimal change = decimal.Parse(RecPayServiceWalkinChangeBox.Text);
                    string paymentMethod = "Cash";

                    // Create a new table for the "Total" section
                    PdfPTable totalTable = new PdfPTable(2); // 2 columns for the "Total" table
                    totalTable.SetWidths(new float[] { 5f, 3f }); // Column widths
                    totalTable.DefaultCell.Border = PdfPCell.NO_BORDER;

                    int totalRowCount = RecPayServiceWalkinAcquiredDGV.Rows.Count + RecPayServiceWalkinCOProdDGV.Rows.Count;

                    // Add cells to the "Total" table
                    totalTable.AddCell(new Phrase($"Total ({totalRowCount})", font));
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
                    //vatTable.AddCell(new Phrase("Discount (20%)", font));
                    //vatTable.AddCell(new Phrase($"Php {discount:F2}", font));

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
                RecWalkinAttendingStaffSelectedComboBox.Enabled = false;
                RecWalkinAnyStaffToggleSwitch.CheckedChanged += RecWalkinAnyStaffToggleSwitch_CheckedChanged;
                return;
            }
            else
            {
                if (RecWalkinAnyStaffToggleSwitch.Checked)
                {
                    RecWalkinPreferredStaffToggleSwitch.Checked = false;
                    RecWalkinAttendingStaffSelectedComboBox.Enabled = false;
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
                    LoadPreferredStaffComboBox();
                }
                else
                {
                    selectedStaffID = "Anyone";
                    RecWalkinAttendingStaffSelectedComboBox.Enabled = false;
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
        //private void RecPayServiceVATExemptChk_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (RecPayServiceVATExemptChk.Checked)
        //    {
        //        ReceptionCalculateVATExemption();
        //    }
        //    else
        //    {
        //        RecPayServiceWalkinCalculateTotalPrice();
        //    }
        //}
        public void ReceptionCalculateVATExemption()
        {
            // Get the Gross Amount from the TextBox (MngrGrossAmountBox)
            if (decimal.TryParse(RecPayServiceWalkinNetAmountBox.Text, out decimal netAmount))
            {
                // For VAT exemption, set VAT Amount to zero
                decimal vatAmount = 0;

                // Set the Net Amount as the new Gross Amount
                decimal grossAmount = netAmount;

                // Display the calculated values in TextBoxes
                RecPayServiceWalkinVATBox.Text = vatAmount.ToString("0.00");
                RecPayServiceWalkinVATBox.Text = vatAmount.ToString("0.00");

            }
        }
        #endregion

        #region Receptionist Queue Window
        private void RecQueWinBtn_Click(object sender, EventArgs e)
        {
            RecQueWinColor();
            RecQuePreferredStaffLoadData();
            RecQueGeneralStaffLoadData();
            RecQueWinNextCustomerLbl.Text = "| NEXT IN LINE [GENERAL QUEUE]";
            RecQueWinGenCatComboText.Visible = true;
            RecQueWinGenCatComboText.SelectedIndex = 5;
            RecQueWinStaffCatComboText.SelectedIndex = 5;

        }

        private void RecQueWinExitBtn_Click(object sender, EventArgs e)
        {
            Transaction.PanelShow(RecQueStartPanel);
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
                        RecQueWinStaffListDGV.Columns[0].Visible = false;
                        RecQueWinStaffListDGV.Columns[1].Visible = false;
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
                        RecQueWinStaffListDGV.Columns[0].Visible = false;
                        RecQueWinStaffListDGV.Columns[1].Visible = false;
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

                    string sql = "SELECT * FROM `servicehistory` WHERE (QueType = 'GeneralQue' OR QueType = 'AnyonePriority') AND ServiceStatus = 'Pending' AND AppointmentDate = @todayDate";
                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    System.Data.DataTable dataTable = new System.Data.DataTable();
                    cmd.Parameters.AddWithValue("@todayDate", todayDate);

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);


                        RecQueWinNextCustomerDGV.DataSource = dataTable;

                        RecQueWinNextCustomerDGV.Columns[1].Visible = false; //appointment time
                        RecQueWinNextCustomerDGV.Columns[3].Visible = false; //appointment time
                        RecQueWinNextCustomerDGV.Columns[5].Visible = false; //service category
                        RecQueWinNextCustomerDGV.Columns[6].Visible = false; //attending staff
                        RecQueWinNextCustomerDGV.Columns[7].Visible = false; //attending staff
                        RecQueWinNextCustomerDGV.Columns[8].Visible = false; //attending staff
                        RecQueWinNextCustomerDGV.Columns[10].Visible = false; //service start
                        RecQueWinNextCustomerDGV.Columns[11].Visible = false; //service end
                        RecQueWinNextCustomerDGV.Columns[12].Visible = false; //service duration
                        RecQueWinNextCustomerDGV.Columns[13].Visible = false; //service duration
                        RecQueWinNextCustomerDGV.Columns[14].Visible = false; //service duration
                        RecQueWinNextCustomerDGV.Columns[18].Visible = false; // que type

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

                    string sql = "SELECT * FROM `servicehistory` WHERE (QueType = 'GeneralQue' OR QueType = 'AnyonePriority') AND ServiceStatus = 'Pending' AND AppointmentDate = @todayDate AND ServiceCategory = @category";
                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    System.Data.DataTable dataTable = new System.Data.DataTable();
                    cmd.Parameters.AddWithValue("@todayDate", todayDate);
                    cmd.Parameters.AddWithValue("@category", category);

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);


                        RecQueWinNextCustomerDGV.DataSource = dataTable;

                        RecQueWinNextCustomerDGV.Columns[1].Visible = false; //appointment time
                        RecQueWinNextCustomerDGV.Columns[3].Visible = false; //appointment time
                        RecQueWinNextCustomerDGV.Columns[5].Visible = false; //service category
                        RecQueWinNextCustomerDGV.Columns[6].Visible = false; //attending staff
                        RecQueWinNextCustomerDGV.Columns[7].Visible = false; //attending staff
                        RecQueWinNextCustomerDGV.Columns[8].Visible = false; //attending staff
                        RecQueWinNextCustomerDGV.Columns[10].Visible = false; //service start
                        RecQueWinNextCustomerDGV.Columns[11].Visible = false; //service end
                        RecQueWinNextCustomerDGV.Columns[12].Visible = false; //service duration
                        RecQueWinNextCustomerDGV.Columns[13].Visible = false; //service duration
                        RecQueWinNextCustomerDGV.Columns[14].Visible = false; //service duration
                        RecQueWinNextCustomerDGV.Columns[18].Visible = false; // que type
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
                RecQueWinNextCustomerLbl.Text = $"| Queue Line for {emplFName} {emplLName}";
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

                    string sql = "SELECT * FROM `servicehistory` WHERE PreferredStaff = @emplID AND ServiceStatus = 'Pending' AND " +
                        "(QueType = 'Preferred' OR QueType = 'PreferredPriority') AND AppointmentDate = @todayDate";
                    MySqlCommand cmd = new MySqlCommand(sql, connection);

                    // Add parameters to the query
                    cmd.Parameters.AddWithValue("@emplID", ID);
                    cmd.Parameters.AddWithValue("@todayDate", todayDate);


                    System.Data.DataTable dataTable = new System.Data.DataTable();

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);

                        RecQueWinPrefNextCustomerDGV.DataSource = dataTable;
                        RecQueWinPrefNextCustomerDGV.Columns[1].Visible = false; //appointment time
                        RecQueWinPrefNextCustomerDGV.Columns[3].Visible = false; //appointment time
                        RecQueWinPrefNextCustomerDGV.Columns[5].Visible = false; //service category
                        RecQueWinPrefNextCustomerDGV.Columns[6].Visible = false; //attending staff
                        RecQueWinPrefNextCustomerDGV.Columns[7].Visible = false; //attending staff
                        RecQueWinPrefNextCustomerDGV.Columns[8].Visible = false; //attending staff
                        RecQueWinPrefNextCustomerDGV.Columns[10].Visible = false; //service start
                        RecQueWinPrefNextCustomerDGV.Columns[11].Visible = false; //service end
                        RecQueWinPrefNextCustomerDGV.Columns[12].Visible = false; //service duration
                        RecQueWinPrefNextCustomerDGV.Columns[13].Visible = false; //service duration
                        RecQueWinPrefNextCustomerDGV.Columns[14].Visible = false; //service duration
                        RecQueWinPrefNextCustomerDGV.Columns[18].Visible = false; // que type


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
            Transaction.PanelShow(RecQueStartPanel);
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


        public bool serviceappointment;
        //ApptMember
        private void RecApptCatHSBtn_Click(object sender, EventArgs e)
        {
            filterstaffbyservicecategory = "Hair Styling";
            haschosenacategory = true;
            RecApptServicesFLP.Controls.Clear();
            servicecurrentPage = 0;
            servicecurrentPagefake = 1;
            InitializeServices(filterstaffbyservicecategory);
            serviceappointment = true;
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
            RecApptServicesFLP.Controls.Clear();
            servicecurrentPage = 0;
            servicecurrentPagefake = 1;
            InitializeServices(filterstaffbyservicecategory);
            serviceappointment = true;
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
            RecApptServicesFLP.Controls.Clear();
            servicecurrentPage = 0;
            servicecurrentPagefake = 1;
            InitializeServices(filterstaffbyservicecategory);
            serviceappointment = true;
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
            RecApptServicesFLP.Controls.Clear();
            servicecurrentPage = 0;
            servicecurrentPagefake = 1;
            InitializeServices(filterstaffbyservicecategory);
            serviceappointment = true;
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
            RecApptServicesFLP.Controls.Clear();
            servicecurrentPage = 0;
            servicecurrentPagefake = 1;
            InitializeServices(filterstaffbyservicecategory);
            serviceappointment = true;
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
                RecApptCatHSRB.Visible = false;
                RecApptCatHSRB.Checked = true;

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
        public void RecApptAddService()
        {
            selectedStaffID = "";

            if (RecApptAnyStaffToggleSwitch.Checked == false && RecApptPreferredStaffToggleSwitch.Checked == false)
            {
                MessageBox.Show("Please select a prefered staff or toggle anyone ", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (RecApptAnyStaffToggleSwitch.Checked)
            {
                selectedStaffID = "Anyone";
            }
            if (RecApptPreferredStaffToggleSwitch.Checked)
            {
                string selectedstaff = RecApptAvailableAttendingStaffSelectedComboBox.SelectedItem.ToString();
                selectedStaffID = selectedstaff.Substring(0, 11);
            }


            if (string.IsNullOrEmpty(selectedStaffID))
            {
                MessageBox.Show("Please select a prefered staff or toggle anyone ", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //if (RecApptBookingTimeComboBox.SelectedItem == null || RecApptBookingTimeComboBox.SelectedItem.ToString() == "Cutoff Time"
            //    || RecApptBookingTimeComboBox.SelectedItem.ToString() == "Select a booking time")
            //{
            //    MessageBox.Show("Please select a booking time", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    return;
            //}



            string SelectedCategory = serviceCategory;
            string ServiceID = serviceID2;
            string ServiceName = serviceName;
            string ServicePrice = servicePrice;
            string ServiceTime = RecApptBookingTimeComboBox.SelectedItem.ToString();
            string serviceID = serviceID2;


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
                RecApptServiceCalculateTotalPrice();

            }
        }

        private void RecApptServiceTypeDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            RecApptAddService();
        }
        private void RecApptServiceCalculateTotalPrice()
        {
            decimal total1 = 0;

            int servicepriceColumnIndex = RecApptSelectedServiceDGV.Columns["RecApptServicePrice"].Index;

            foreach (DataGridViewRow row in RecApptSelectedServiceDGV.Rows)
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
            RecAppTotalText.Text = total1.ToString("F2");
            // Apply discount (for example, 20% discount)
            decimal initialFee = 0.6m; // 20% discount
            decimal inititalFeeTotal = total1 * (1 - initialFee);
            RecApptInitialFeeText.Text = inititalFeeTotal.ToString("F2");
            decimal balance = total1 - inititalFeeTotal;
            RecApptBalanceText.Text = balance.ToString("F2");
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
                RecApptAvailableAttendingStaffSelectedComboBox.Enabled = false;

                RecApptAnyStaffToggleSwitch.CheckedChanged += RecApptAnyStaffToggleSwitch_CheckedChanged;
                return;
            }
            else
            {
                if (RecApptAnyStaffToggleSwitch.Checked)
                {
                    RecApptPreferredStaffToggleSwitch.Checked = false;
                    RecApptAvailableAttendingStaffSelectedComboBox.Enabled = false;
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
                RecApptAvailableAttendingStaffSelectedComboBox.Enabled = false;
                RecApptPreferredStaffToggleSwitch.CheckedChanged += RecApptPreferredStaffToggleSwitch_CheckedChanged;
                return;
            }
            else
            {
                if (RecApptPreferredStaffToggleSwitch.Checked && RecApptAvailableAttendingStaffSelectedComboBox.SelectedText != "Select a Preferred Staff")
                {
                    RecApptAnyStaffToggleSwitch.Checked = false;
                    RecApptAvailableAttendingStaffSelectedComboBox.Enabled = true;
                    LoadAppointmentPreferredStaffComboBox();
                }
                else
                {
                    selectedStaffID = "Anyone";
                    RecApptAvailableAttendingStaffSelectedComboBox.Enabled = false;
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

            if (RecApptSelectedServiceDGV != null && RecApptSelectedServiceDGV.Rows.Count == 0)
            {
                MessageBox.Show("Select a service first to proceed on booking a transaction.", "Ooooops!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else if (RecApptSelectedServiceDGV != null && RecApptSelectedServiceDGV.Rows.Count == 0)
            {
                MessageBox.Show("Select a service first to proceed on booking a transaction.", "Ooooops!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else if (ReceptionistAppointmentDB())
            {
                RecApptServiceHistoryDB(RecApptSelectedServiceDGV); //service history 
                RecApptFormGenerator();
                RecApptTransactNumRefresh();
                ApptTabs.SelectedIndex = 0;
                RecApptTransactionClear();
            }
        }


        private void RecApptFormGenerator()
        {
            DateTime currentDate = RecDateTimePicker.Value;
            string datetoday = currentDate.ToString("MM-dd-yyyy dddd");
            string timePrinted = currentDate.ToString("hh:mm tt");
            string timePrintedFile = currentDate.ToString("hh-mm-ss");
            string transactNum = RecApptTransNumText.Text;
            string clientName = $"{RecApptFNameText.Text} {RecApptLNameText.Text}";
            string recName = RecNameLbl.Text;
            string apptNote = "This form will serves as your proof of appointment with Enchanté Salon. " +
                                "Kindly present this form and one (1) Valid ID in our frontdesk and our " +
                                "receptionist shall attend to your needs right away.";
            string total = RecAppTotalText.Text.ToString();
            string downpayment = RecApptInitialFeeText.Text;
            string cash = RecApptCashText.Text;
            string change = RecApptChangeText.Text;

            DateTime bookeddate = RecApptBookingDatePicker.Value;
            string apptdate = bookeddate.ToString("MM-dd-yyyy dddd");
            string appttime = RecApptBookingTimeComboBox.Text;

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
                Document doc = new Document(new iTextSharp.text.Rectangle(Utilities.MillimetersToPoints(133.35f), Utilities.MillimetersToPoints(215.9f)));

                try
                {
                    // Create a PdfWriter instance
                    PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));

                    // Open the document for writing
                    doc.Open();

                    Bitmap imagepath = Properties.Resources.Enchante_Logo__200_x_200_px__Green;
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(imagepath, System.Drawing.Imaging.ImageFormat.Png);
                    logo.Alignment = Element.ALIGN_CENTER;
                    logo.ScaleAbsolute(100f, 100f);
                    doc.Add(logo);

                    iTextSharp.text.Font headerFont = FontFactory.GetFont("Courier", 16, iTextSharp.text.Font.BOLD);
                    iTextSharp.text.Font boldfont = FontFactory.GetFont("Courier", 10, iTextSharp.text.Font.BOLD);
                    iTextSharp.text.Font font = FontFactory.GetFont("Courier", 10, iTextSharp.text.Font.NORMAL);
                    iTextSharp.text.Font italic = FontFactory.GetFont("Courier", 10, iTextSharp.text.Font.ITALIC);
                    iTextSharp.text.Font right = FontFactory.GetFont("Courier", 10, Element.ALIGN_CENTER);

                    // Create a centered alignment for text
                    iTextSharp.text.Paragraph centerAligned = new Paragraph();
                    centerAligned.Alignment = Element.ALIGN_CENTER;

                    // Add centered content to the centerAligned Paragraph
                    //centerAligned.Add(new Chunk("Enchanté Salon", headerFont));
                    centerAligned.Add(new Chunk("\n69th flr. Enchanté Bldg. Ortigas Ave. Ext.\nManggahan, Pasig City 1611 Philippines", font));
                    centerAligned.Add(new Chunk("\nTel. No.: (1101) 111-1010", font));
                    centerAligned.Add(new Chunk($"\nDate: {datetoday} Time: {timePrinted}", font));

                    // Add the centered content to the document
                    doc.Add(centerAligned);

                    int totalRowCount = RecApptSelectedServiceDGV.Rows.Count;
                    doc.Add(new Chunk("\n")); // New line

                    doc.Add(new Paragraph($"Transaction No.: {transactNum}", font));
                    doc.Add(new Paragraph($"Booked For: {clientName}", font));
                    doc.Add(new Paragraph($"Booked By: {recName}", font));

                    doc.Add(new Chunk("\n")); // New line

                    doc.Add(new LineSeparator()); // Dotted line

                    PdfPTable columnHeaderTable = new PdfPTable(4);
                    columnHeaderTable.SetWidths(new float[] { 30f, 40f, 30f, 30f }); // Column widths
                    columnHeaderTable.DefaultCell.Border = PdfPCell.NO_BORDER;
                    columnHeaderTable.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
                    columnHeaderTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

                    columnHeaderTable.AddCell(new Phrase("Staff ID", boldfont));
                    columnHeaderTable.AddCell(new Phrase("Services", boldfont));
                    columnHeaderTable.AddCell(new Phrase("Total Price", boldfont));
                    columnHeaderTable.AddCell(new Phrase("Time", boldfont));

                    doc.Add(columnHeaderTable);

                    doc.Add(new LineSeparator()); // Dotted line
                    // Iterate through the rows of your 

                    foreach (DataGridViewRow row in RecApptSelectedServiceDGV.Rows)
                    {
                        try
                        {
                            string serviceName = row.Cells["RecApptSelectedService"].Value?.ToString();
                            if (string.IsNullOrEmpty(serviceName))
                            {
                                continue; // Skip empty rows
                            }

                            string staffID = row.Cells["RecApptStaffSelected"].Value?.ToString();
                            string itemTotalcost = row.Cells["RecApptServicePrice"].Value?.ToString();
                            string selectedTime = row.Cells["RecApptTimeSelected"].Value?.ToString();

                            // Add cells to the item table
                            PdfPTable serviceTable = new PdfPTable(4);
                            serviceTable.SetWidths(new float[] { 30f, 40f, 30f, 30f }); // Column widths
                            serviceTable.DefaultCell.Border = PdfPCell.NO_BORDER;
                            serviceTable.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
                            serviceTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

                            serviceTable.AddCell(new Phrase(staffID, font));
                            serviceTable.AddCell(new Phrase(serviceName, font));
                            serviceTable.AddCell(new Phrase(itemTotalcost, font));
                            serviceTable.AddCell(new Phrase(selectedTime, font));

                            doc.Add(serviceTable);

                        }
                        catch (Exception ex)
                        {
                            // Handle or log any exceptions that occur while processing DataGridView data
                            MessageBox.Show("An error occurred: " + ex.Message, "Appoint Form Generator Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    doc.Add(new Chunk("\n")); // New line
                    doc.Add(new LineSeparator()); // Dotted line
                    doc.Add(new Chunk("\n")); // New line

                    PdfPTable ApptDetails = new PdfPTable(2);

                    ApptDetails.HorizontalAlignment = Element.ALIGN_CENTER; // Center the table

                    ApptDetails.SetWidths(new float[] { 60f, 40f }); // Column widths as percentage of the total width

                    ApptDetails.DefaultCell.Border = PdfPCell.NO_BORDER;
                    ApptDetails.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
                    ApptDetails.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT; // Align cell content justified

                    ApptDetails.AddCell(new Phrase($"Appointment Date: ", font));
                    PdfPCell ApptdateCell = new PdfPCell(new Phrase($"{apptdate}", font));
                    ApptdateCell.Border = PdfPCell.NO_BORDER; // Remove border from this cell
                    ApptDetails.AddCell(ApptdateCell);

                    ApptDetails.AddCell(new Phrase($"Appointment Time: ", font));
                    PdfPCell ApptTimeCell = new PdfPCell(new Phrase($"{appttime}", font));
                    ApptTimeCell.Border = PdfPCell.NO_BORDER; // Remove border from this cell
                    ApptDetails.AddCell(ApptTimeCell);
                    doc.Add(ApptDetails); // Add the table to the document

                    doc.Add(new Chunk("\n")); // New line

                    // Add cells to the INFO table
                    PdfPTable amount = new PdfPTable(2);

                    amount.HorizontalAlignment = Element.ALIGN_CENTER; // Center the table

                    amount.SetWidths(new float[] { 60f, 40f }); // Column widths as percentage of the total width

                    amount.DefaultCell.Border = PdfPCell.NO_BORDER;
                    amount.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
                    amount.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT; // Align cell content justified

                    amount.AddCell(new Phrase($"Total ({totalRowCount}): ", font));
                    PdfPCell totalCell = new PdfPCell(new Phrase($"Php. {total}", font));
                    totalCell.Border = PdfPCell.NO_BORDER; // Remove border from this cell
                    amount.AddCell(totalCell);

                    amount.AddCell(new Phrase($"Initital Payment (40%): ", font));
                    PdfPCell dpCell = new PdfPCell(new Phrase($"Php. {downpayment}", font));
                    dpCell.Border = PdfPCell.NO_BORDER; // Remove border from this cell
                    amount.AddCell(dpCell);

                    amount.AddCell(new Phrase($"Cash Given: ", font));
                    PdfPCell cashCell = new PdfPCell(new Phrase($"Php. {cash}", font));
                    cashCell.Border = PdfPCell.NO_BORDER; // Remove border from this cell
                    amount.AddCell(cashCell);

                    amount.AddCell(new Phrase($"Change: ", font));
                    PdfPCell changeCell = new PdfPCell(new Phrase($"Php. {change}", font));
                    changeCell.Border = PdfPCell.NO_BORDER; // Remove border from this cell
                    amount.AddCell(changeCell);

                    doc.Add(amount); // Add the table to the document


                    doc.Add(new Chunk("\n")); // New line


                    // Add the legal string with center alignment
                    Paragraph paragraph_footer = new Paragraph($"\n{apptNote}", italic);
                    paragraph_footer.Alignment = Element.ALIGN_CENTER;
                    doc.Add(paragraph_footer);
                }
                catch (DocumentException de)
                {
                    MessageBox.Show("An error occurred: " + de.Message, "Appoint Form Generator Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (IOException ioe)
                {
                    MessageBox.Show("An error occurred: " + ioe.Message, "Appoint Form Generator Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // Close the document
                    doc.Close();
                }

                //MessageBox.Show($"Receipt saved as {filePath}", "Receipt Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        private bool ReceptionistAppointmentDB()
        {
            DateTime appointmentdate = RecApptBookingDatePicker.Value;
            string transactionNum = RecApptTransNumText.Text;
            DateTime currentDate = RecDateTimePicker.Value;
            string serviceStatus = "Pending";
            string transactType = "Walk-in Appointment";
            string appointmentStatus = "Unconfirmed";

            //basic info
            string CustomerName = RecApptFNameText.Text + " " + RecApptLNameText.Text; //client name
            string CustomerMobileNumber = RecApptCPNumText.Text; //client cp num
            string bday = RecApptClientBdayPicker.Value.ToString("MMMM dd, yyyy");
            string age = RecApptClientAgeText.Text;

            //booked values
            string appointmentbookedDate = appointmentdate.ToString("MM-dd-yyyy dddd"); //bookedDate
            string appointmentbookedTime = RecApptBookingTimeComboBox.SelectedItem?.ToString(); //bookedTime
            string bookedDate = currentDate.ToString("MM-dd-yyyy dddd"); //bookedDate
            string bookedTime = currentDate.ToString("hh:mm tt"); //bookedTime
            string bookedBy = RecNameLbl.Text; //booked by

            //cash values
            string total = RecAppTotalText.Text;
            string downpayment = RecApptInitialFeeText.Text;
            string cash = RecApptCashText.Text;
            string change = RecApptChangeText.Text;
            string bal = RecApptBalanceText.Text;

            // Assuming dgv is your DataGridView object
            // Assuming columnIndex is the index of the column you want to retrieve

            if (RecApptSelectedServiceDGV.Rows.Count > 0) // Check if there are any rows in the DataGridView
            {
                // Access the cell value of the first row and specified column
                object cellValue = RecApptSelectedServiceDGV.Rows[0].Cells["RecApptTimeSelected"].Value;

                if (cellValue != null)
                {
                    // Do something with the cell value
                    string cellContent = cellValue.ToString();
                }

            }





            try
            {
                using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                {
                    connection.Open();
                    if (downpayment == "0.00")
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
                    else if (Convert.ToDecimal(cash) < Convert.ToDecimal(downpayment))
                    {
                        MessageBox.Show("Insufficient amount. Please provide enough cash to cover the transaction.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else
                    {
                        string insertQuery = "INSERT INTO appointment (TransactionNumber, TransactionType, ServiceStatus, AppointmentDate, AppointmentTime, AppointmentStatus, " +
                                        "ClientName, ClientCPNum, ClientBday, ClientAge, GrossAmount, Downpayment, RemainingBal, CashGiven, DueChange, PaymentMethod, BookedBy, BookedDate, BookedTime)" +
                                        "VALUES (@Transact, @TransactType, @status, @appointDate, @appointTime, @appointStatus, @clientName, @clientCP, @clientBday, @clientAge, @total,  " +
                                        "@dp, @bal, @cash, @change, @method, @bookedBy, @bookedDate, @bookedTime)";

                        MySqlCommand cmd = new MySqlCommand(insertQuery, connection);
                        cmd.Parameters.AddWithValue("@Transact", transactionNum);
                        cmd.Parameters.AddWithValue("@TransactType", transactType);
                        cmd.Parameters.AddWithValue("@status", serviceStatus);
                        cmd.Parameters.AddWithValue("@appointDate", appointmentbookedDate);
                        if (RecApptSelectedServiceDGV.Rows.Count > 0) // Check if there are any rows in the DataGridView
                        {
                            object cellValue = RecApptSelectedServiceDGV.Rows[0].Cells["RecApptTimeSelected"].Value;
                            if (cellValue != null)
                            {
                                // Convert cell value to string
                                string cellContent = cellValue.ToString();
                                cmd.Parameters.AddWithValue("@appointTime", cellContent);
                            }

                        }
                        cmd.Parameters.AddWithValue("@appointStatus", appointmentStatus);
                        cmd.Parameters.AddWithValue("@clientName", CustomerName);
                        cmd.Parameters.AddWithValue("@clientCP", CustomerMobileNumber);
                        cmd.Parameters.AddWithValue("@clientBday", bday);
                        cmd.Parameters.AddWithValue("@clientAge", age);
                        cmd.Parameters.AddWithValue("@total", total);
                        cmd.Parameters.AddWithValue("@dp", downpayment);
                        cmd.Parameters.AddWithValue("@bal", bal);
                        cmd.Parameters.AddWithValue("@cash", cash);
                        cmd.Parameters.AddWithValue("@change", change);
                        cmd.Parameters.AddWithValue("@method", "Cash");

                        cmd.Parameters.AddWithValue("@bookedBy", bookedBy);
                        cmd.Parameters.AddWithValue("@bookedDate", bookedDate);
                        cmd.Parameters.AddWithValue("@bookedTime", bookedTime);




                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Service successfully booked.", "Hooray!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
            }
            catch (MySqlException ex)
            {
                // Handle MySQL database exception
                MessageBox.Show("An error occurred: " + ex.Message, "Appointment booking transaction failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                // Make sure to close the connection
                connection.Close();
            }
            return true;
        }
        private bool RecApptDownpayment()
        {
            // cash values
            string netAmount = RecPayServiceWalkinNetAmountBox.Text; // net amount
            string vat = RecPayServiceWalkinVATBox.Text; // vat 
            string discount = RecPayServiceWalkinDiscountBox.Text; // discount
            string grossAmount = RecPayServiceWalkinGrossAmountBox.Text; // gross amount
            string cash = RecPayServiceWalkinCashBox.Text; // cash given
            string change = RecPayServiceWalkinChangeBox.Text; // due change
            string paymentMethod = "Cash"; // payment method
            string mngr = RecNameLbl.Text;
            string transactNum = RecPayServiceWalkinTransactNumLbl.Text;


            try
            {
                using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                {
                    connection.Open();
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
                    else
                    {
                        string cashPaymentAppt = "UPDATE appointment SET ServiceStatus = @status, NetPrice = @net, VatAmount = @vat, DiscountAmount = @discount, " +
                                        "GrossAmount = @gross, CashGiven = @cash, DueChange = @change, PaymentMethod = @payment, CheckedOutBy = @mngr " +
                                        "WHERE TransactionNumber = @transactNum"; // cash query

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
            RecApptCatHSRB.Visible = false;
            RecApptCatFSRB.Visible = false;
            RecApptCatNCRB.Visible = false;
            RecApptCatSpaRB.Visible = false;
            RecApptCatMassRB.Visible = false;
            RecApptCatHSRB.Checked = false;
            RecApptCatFSRB.Checked = false;
            RecApptCatNCRB.Checked = false;
            RecApptCatSpaRB.Checked = false;
            RecApptCatMassRB.Checked = false;
            RecApptSelectedServiceDGV.Rows.Clear();
            RecApptBookingTimeComboBox.Items.Clear();
            RecApptBdayMaxDate();
            RecApptClientAgeText.Text = "Age";
            RecApptBookingDatePicker.Value = DateTime.Today;
            RecApptPreferredStaffToggleSwitch.Checked = false;
            RecApptAnyStaffToggleSwitch.Checked = false;
            isappointment = false;
            RecAppTotalText.Text = "0.00";
            RecApptInitialFeeText.Text = "0.00";
            RecApptCashText.Text = "0.00";
            RecApptChangeText.Text = "0.00";
        }

        //ApptMember
        private void RecApptBookingDatePicker_ValueChanged(object sender, EventArgs e)
        {
            LoadBookingTimes();
        }
        //ApptMember
        string[] bookingTimes = new string[]
        {
            "Select a booking time", "08:00 am", "08:30 am", "09:00 am",
            "09:30 am", "10:00 am", "10:30 am", "11:00 am", "11:30 am",
            "01:00 pm", "01:30 pm", "02:00 pm", "02:30 pm",
        };
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
            RecApptBookingTimeComboBox.Items.Add("Select a booking time");
            RecApptBookingTimeComboBox.SelectedIndex = 0;

            bool cutoffTimeAdded = false; // Flag to track if "Cutoff Time" has been added

            if (selectedDate == DateTime.Today && DateTime.Now.TimeOfDay > new TimeSpan(14, 30, 0))  // Check if the selected date is today and if it's past 2:30 PM
            {
                // Add "Cutoff Time" to ComboBox
                RecApptBookingTimeComboBox.Items.Add("Cutoff Time");
                cutoffTimeAdded = true;
            }

            // Add regular booking times for the selected date and service category
            foreach (string time in bookingTimes) // Skip the first item "Select a booking time"  bookingTimes.Skip(1)
            {
                DateTime bookingDateTime;
                if (DateTime.TryParseExact(time, "hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out bookingDateTime))
                {
                    // Combine the date part from selectedDate with the time part from bookingDateTime
                    DateTime combinedDateTime = selectedDate.Date.Add(bookingDateTime.TimeOfDay);

                    // Check if the combinedDateTime is in the past
                    if (DateTime.Now >= combinedDateTime)
                    {
                        continue; // Skip this time if it's in the past
                    }
                }
                else
                {
                    // If parsing fails, log an error or handle it accordingly
                    Console.WriteLine($"Failed to parse time: {time}");
                    continue;
                }
                RecApptBookingTimeComboBox.Items.Add(time);
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

            // Disable the ComboBox if "Cutoff Time" is added
            RecApptBookingTimeComboBox.Enabled = !cutoffTimeAdded;
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

                string currentDate = DateTime.Now.ToString("MM-dd-yyyy dddd");

                string query = @"SELECT a.TransactionNumber AS TransactionID, a.AppointmentDate, a.ClientName, 
                                GROUP_CONCAT(sh.AppointmentTime SEPARATOR ', ') AS AppointmentTime
                                 FROM appointment a
                                 LEFT JOIN servicehistory sh ON sh.TransactionNumber = a.TransactionNumber
                                 WHERE a.ServiceStatus = 'Pending' AND a.AppointmentStatus = 'Unconfirmed'
                                    AND a.AppointmentDate = @currentDate
                                 GROUP BY a.TransactionNumber, a.AppointmentDate, a.ClientName";


                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@currentDate", currentDate);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                if (dataTable.Rows.Count > 0)
                {
                    foreach (DataRow row in dataTable.Rows)
                    {
                        RecApptAcceptLateDeclineDGV.Rows.Add(row["TransactionID"], row["AppointmentDate"], row["ClientName"], row["AppointmentTime"]);

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
                                    RecCancelServicesDGV.Rows.Clear();
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
                                    RecCancelServicesDGV.Rows.Clear();
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
            if (RecWalkinSelectedServiceDGV.Columns[e.ColumnIndex].Name == "WalkinServiceVoid")
            {
                DialogResult result;

                result = MessageBox.Show("Do you want to remove this item?", "Remove Item", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Remove the selected row
                    RecWalkinSelectedServiceDGV.Rows.RemoveAt(e.RowIndex);
                    MessageBox.Show("Item removed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }


        }
        private void RecApptConfirmBtn_Click(object sender, EventArgs e)
        {
            ApptConfirmTransColor();
            RecApptAcceptLateDeclineDGV.Rows.Clear();
            InitializeAppointmentDataGrid();
        }
        private void RecApptConfirmExitBtn_Click(object sender, EventArgs e)
        {
            Transaction.PanelShow(RecQueStartPanel);

        }
        private void RecShopProdBtn_Click(object sender, EventArgs e)
        {
            RecWalkinTransactionClear();
            RecApptTransactionClear();
            ShopProdTransColor();
            RecShopProdProductFlowLayoutPanel.Controls.Clear();
            product = true;
            InitializeProducts();
            walkinproductsearch = false;
        }

        private void RecShopProdExitBtn_Click(object sender, EventArgs e)
        {
            Transaction.PanelShow(RecQueStartPanel);

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

                            DialogResult result = MessageBox.Show("Do you want to remove this item?", "Remove Item", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                            if (result == DialogResult.Yes)
                            {
                                // Remove the selected row
                                RecShopProdSelectedProdDGV.Rows.RemoveAt(e.RowIndex);
                                MessageBox.Show("Item removed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                RecShopProdCalculateTotalPrice();
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
                                    RecShopProdCalculateTotalPrice();

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
                                RecShopProdCalculateTotalPrice();

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
        private decimal originalGrossAmount;
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
        private void RecShopProdPaymentButton_Click(object sender, EventArgs e)
        {
            if (RecShopProdInsertOrderDB())
            {
                RecShopProdUpdateQtyInventory(RecShopProdSelectedProdDGV);
                RecShopProdOrderProdHistoryDB(RecShopProdSelectedProdDGV);
                RecShopProdInvoiceReceiptGenerator();
                RecShopProdTransactionClear();
            }
        }
        private void RecShopProdTransactionClear()
        {

            RecShopProdNetAmountBox.Text = "0.00";
            RecShopProdVATBox.Text = "0.00";
            RecShopProdDiscountBox.Text = "0.00";
            RecShopProdGrossAmountBox.Text = "0.00";
            RecShopProdCashBox.Text = "0";
            RecShopProdChangeBox.Text = "0.00";
            RecShopProdSelectedProdDGV.Rows.Clear();

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


            // cash values
            string netAmount = RecShopProdNetAmountBox.Text; // net amount
            string vat = RecShopProdVATBox.Text; // vat 
            string discount = RecShopProdDiscountBox.Text; // discount
            string grossAmount = RecShopProdGrossAmountBox.Text; // gross amount
            string cash = RecShopProdCashBox.Text; // cash given
            string change = RecShopProdChangeBox.Text; // due change
            string rec = RecNameLbl.Text;
            string transactNum = RecShopProdTransNumText.Text;
            //booked values
            string Date = currentDate.ToString("MM-dd-yyyy dddd"); //bookedDate
            string Time = currentDate.ToString("hh:mm tt"); //bookedTime
            // bank & wallet details


            try
            {
                using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                {
                    connection.Open();
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
                    else
                    {
                        string cashPayment = "INSERT INTO orders (TransactionNumber, TransactionType, ProductStatus, Date, Time, CheckedOutBy, NetPrice, VatAmount, DiscountAmount, GrossAmount, CashGiven, DueChange) " +
                                        "VALUES (@transactNum, @transactType, @status, @date, @time, @rec, @net, @vat, @discount, @gross, @cash, @change)";

                        MySqlCommand cmd = new MySqlCommand(cashPayment, connection);
                        cmd.Parameters.AddWithValue("@transactNum", transactNum);
                        cmd.Parameters.AddWithValue("@transactType", "Walk-in Checked Out");
                        cmd.Parameters.AddWithValue("@status", "Paid");
                        cmd.Parameters.AddWithValue("@date", Date);
                        cmd.Parameters.AddWithValue("@time", Time);
                        cmd.Parameters.AddWithValue("@rec", rec);

                        cmd.Parameters.AddWithValue("@net", netAmount);
                        cmd.Parameters.AddWithValue("@vat", vat);
                        cmd.Parameters.AddWithValue("@discount", discount);
                        cmd.Parameters.AddWithValue("@gross", grossAmount);
                        cmd.Parameters.AddWithValue("@cash", cash);
                        cmd.Parameters.AddWithValue("@change", change);

                        cmd.ExecuteNonQuery();
                        // Successful update
                        MessageBox.Show("Products successfully been paid through cash.", "Hooray!", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

            ////basic info
            //string clientName = RecShopProdClientNameText.Text;
            //string clientCPNum = RecShopProdClientCPNumText.Text;

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


                                string query = "INSERT INTO orderproducthistory (TransactionNumber, ProductStatus, CheckedOutDate, CheckedOutTime, CheckedOutBy, ItemID, ItemName, Qty, ItemPrice, ItemTotalPrice, CheckedOut, Voided) " +
                                                 "VALUES (@Transact, @status, @date, @time, @OrderedBy, @ID, @ItemName, @Qty, @ItemPrice, @ItemTotalPrice, @Yes, @No)";

                                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                                {
                                    cmd.Parameters.AddWithValue("@Transact", transactionNum);
                                    cmd.Parameters.AddWithValue("@status", status);
                                    cmd.Parameters.AddWithValue("@date", bookedDate);
                                    cmd.Parameters.AddWithValue("@time", bookedTime);
                                    cmd.Parameters.AddWithValue("@OrderedBy", bookedBy);
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
            //string clientName = RecShopProdClientNameText.Text;
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
                    Bitmap imagepath = Properties.Resources.Enchante_Logo__200_x_200_px__Green;
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(imagepath, System.Drawing.Imaging.ImageFormat.Png);
                    logo.Alignment = Element.ALIGN_CENTER;
                    logo.ScaleAbsolute(100f, 100f);
                    logo.Alignment = Element.ALIGN_CENTER;
                    doc.Add(logo);

                    iTextSharp.text.Font headerFont = FontFactory.GetFont("Courier", 16, iTextSharp.text.Font.BOLD);
                    iTextSharp.text.Font boldfont = FontFactory.GetFont("Courier", 10, iTextSharp.text.Font.BOLD);
                    iTextSharp.text.Font font = FontFactory.GetFont("Courier", 10, iTextSharp.text.Font.NORMAL);
                    iTextSharp.text.Font italic = FontFactory.GetFont("Courier", 10, iTextSharp.text.Font.ITALIC);

                    // Create a centered alignment for text
                    iTextSharp.text.Paragraph centerAligned = new Paragraph();
                    centerAligned.Alignment = Element.ALIGN_CENTER;

                    // Add centered content to the centerAligned Paragraph
                    //centerAligned.Add(new Chunk("Enchanté Salon", headerFont));
                    centerAligned.Add(new Chunk("\n69th flr. Enchanté Bldg. Ortigas Ave. Ext.\nManggahan, Pasig City 1611 Philippines", font));
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
                    string paymentMethod = "Cash";

                    // Create a new table for the "Total" section
                    PdfPTable totalTable = new PdfPTable(2); // 2 columns for the "Total" table
                    totalTable.SetWidths(new float[] { 5f, 3f }); // Column widths
                    totalTable.DefaultCell.Border = PdfPCell.NO_BORDER;

                    int totalRowCount = RecShopProdSelectedProdDGV.Rows.Count;

                    // Add cells to the "Total" table
                    totalTable.AddCell(new Phrase($"Total ({totalRowCount})", font));
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
                    //vatTable.AddCell(new Phrase("Discount (20%)", font));
                    //vatTable.AddCell(new Phrase($"Php {discount:F2}", font));

                    // Add the "VATable" table to the document
                    doc.Add(vatTable);


                    // Add the "Served To" section
                    doc.Add(new Chunk("\n")); // New line
                    doc.Add(new Paragraph($"Served To: ", italic));
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

            DialogResult result = MessageBox.Show("Do you want to remove this item?", "Remove Item", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {

                RecShopProdSelectedProdDGV.Rows.Clear();


                MessageBox.Show("Item removed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void RecInventoryMembershipBtn_Click(object sender, EventArgs e)
        {
            MngrMembershipDataColor();
        }

        private void RecInventoryProductsBtn_Click(object sender, EventArgs e)
        {
            MngrProductDataColor();
            MngrInventoryProductData();
            ExitFunction();
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
            Transaction.PanelShow(RecQueStartPanel);
            RecPayServiceClearAllField();
        }

        private void MngrInventoryWalkinSalesBtn_Click(object sender, EventArgs e)
        {
            MngrWalkinSalesColor();
            ExitFunction();
        }

        private void MngrInventoryProductsHistoryBtn_Click(object sender, EventArgs e)
        {
            MngrProductHistoryColor();       
            ExitFunction();
            ApplyRowAlternatingColors(MngrPDHistoryDGV);
        }

        private void MngrInventoryStaffSchedBtn_Click(object sender, EventArgs e)
        {
            MngrPromoDataColor();
            ApplyRowAlternatingColors(MngrVoucherDGV);
            ExitFunction();
        }

        private void MngrInventoryMembershipExitBtn_Click(object sender, EventArgs e)
        {
            Inventory.PanelShow(MngrInventoryTypePanel);
        }
        private void MngrInventoryInDemandBtn_Click(object sender, EventArgs e)
        {
            MngrInDemandColor();
            ExitFunction();
        }
        private void MngrServicesHistoryBtn_Click(object sender, EventArgs e)
        {        
            MngrServiceHistoryColor();         
            ExitFunction();
            ApplyRowAlternatingColors(MngrSVHistoryDGV);
        }
        private void MngrServiceHistoryExitBtn_Click(object sender, EventArgs e)
        {
            Inventory.PanelShow(MngrInventoryTypePanel);

        }

        private void MngrWalkinProdSalesBtn_Click(object sender, EventArgs e)
        {
            MngrProdSalesColor();
            ExitFunction();
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
            MngrProductSalesTotalRevBox.Text = "";
            MngrProductSalesLineGraph.Series.Clear();
            MngrProductSalesGraph.Series.Clear();
            Inventory.PanelShow(MngrInventoryTypePanel);
        }
        #endregion

        #region Mngr Services Data
        public void ReceptionLoadServices()
        {
            MySqlConnection connection = null;
            try
            {
                connection = new MySqlConnection(mysqlconn);
                connection.Open();
                string countQuery = "SELECT COUNT(*) FROM services";
                MySqlCommand countCommand = new MySqlCommand(countQuery, connection);
                int totalRows = Convert.ToInt32(countCommand.ExecuteScalar());
                string sql = "SELECT * FROM `services` LIMIT 10";
                MySqlCommand cmd = new MySqlCommand(sql, connection);
                System.Data.DataTable dataTable = new System.Data.DataTable();

                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                {
                    adapter.Fill(dataTable);

                    MngrInventoryServicesTable.DataSource = dataTable;
                    ApplyRowAlternatingColors(MngrInventoryServicesTable);
                    MngrInventoryServicesTable.RowTemplate.Height = 41;
                    MngrInventoryServicesTable.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    MngrInventoryServicesTable.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                }

                int currentBatch = totalRows > 0 ? 1 : 0;
                int totalBatches = (int)Math.Ceiling((double)totalRows / 10);

                MngrServicesCurrentRecordLbl.Text = $"{currentBatch} of {totalBatches}";
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred: " + e.Message, "Inventory Service List");
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private int currentBatchServices = 1;

        private void MngrServicesNextBtn_Click(object sender, EventArgs e)
        {
            int totalBatches = string.IsNullOrEmpty(MngrServicesSearchTextBox.Text.Trim())
                        ? (int)Math.Ceiling((double)GetTotalRowsServices() / 10)
                        : (int)Math.Ceiling((double)GetFilteredTotalRowsServices() / 10);

            if (currentBatchServices >= totalBatches)
            {
                MessageBox.Show("No more data to show.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            currentBatchServices++;

            UpdateDataGridViewAndLabelServices();
            ApplyRowAlternatingColors(MngrInventoryServicesTable);
        }

        private void MngrServicesPreviousBtn_Click(object sender, EventArgs e)
        {
            if (currentBatchServices <= 1)
            {
                MessageBox.Show("No more previous data to show.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            currentBatchServices--;

            UpdateDataGridViewAndLabelServices();
            ApplyRowAlternatingColors(MngrInventoryServicesTable);

        }

        private void UpdateDataGridViewAndLabelServices()
        {
            string connectionString = "Server=localhost;Database=enchante;Uid=root;Pwd=;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string countQuery = string.IsNullOrEmpty(MngrServicesSearchTextBox.Text.Trim())
                                        ? "SELECT COUNT(*) FROM services"
                                        : $"SELECT COUNT(*) FROM services WHERE {GetFilterExpressionServices()}";

                    MySqlCommand countCommand = new MySqlCommand(countQuery, connection);
                    int totalRows = Convert.ToInt32(countCommand.ExecuteScalar());

                    if (totalRows == 0)
                    {
                        MessageBox.Show("No matching data found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        MngrServicesCurrentRecordLbl.Text = "0 of 0";
                        return;
                    }

                    int totalBatches = (int)Math.Ceiling((double)totalRows / 10);

                    currentBatchServices = Math.Min(currentBatchServices, totalBatches);

                    string query = string.IsNullOrEmpty(MngrServicesSearchTextBox.Text.Trim())
                                    ? GetRegularQueryServices()
                                    : GetFilteredQueryServices();

                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    System.Data.DataTable dataTable = new System.Data.DataTable();
                    adapter.Fill(dataTable);

                    foreach (DataColumn column in dataTable.Columns)
                    {
                        if (!MngrInventoryServicesTable.Columns.Contains(column.ColumnName))
                        {
                            MngrInventoryServicesTable.Columns.Add(column.ColumnName, column.ColumnName);
                        }
                    }

                    MngrInventoryServicesTable.DataSource = dataTable;

                    MngrServicesCurrentRecordLbl.Text = $"{currentBatchServices} of {totalBatches}";

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private string GetRegularQueryServices()
        {
            int startIndex = (currentBatchServices - 1) * 10;
            return $"SELECT Category, Type, ServiceID, Name, Description, " +
                    $"Duration, Price, RequiredItem, NumOfItems FROM services " +
                    $"LIMIT {startIndex}, 10";
        }

        private string GetFilteredQueryServices()
        {
            string filterExpression = GetFilterExpressionServices();
            int startIndex = (currentBatchServices - 1) * 10;
            return $"SELECT Category, Type, ServiceID, Name, Description, " +
                    $"Duration, Price, RequiredItem, NumOfItems FROM services " +
                    $"WHERE {filterExpression} " +
                    $"LIMIT {startIndex}, 10";
        }

        private string GetFilterExpressionServices()
        {
            string searchText = MngrServicesSearchTextBox.Text.Trim();
            return string.Join(" OR ", ((DataTable)MngrInventoryServicesTable.DataSource).Columns.Cast<DataColumn>()
                                .Select(col => $"{col.ColumnName} LIKE '%{searchText}%'"));
        }

        private int GetTotalRowsServices()
        {
            string connectionString = "Server=localhost;Database=enchante;Uid=root;Pwd=;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string countQuery = "SELECT COUNT(*) FROM services";
                    MySqlCommand countCommand = new MySqlCommand(countQuery, connection);
                    int totalRows = Convert.ToInt32(countCommand.ExecuteScalar());
                    connection.Close();
                    return totalRows;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return 0;
            }
        }

        private int GetFilteredTotalRowsServices()
        {
            string connectionString = "Server=localhost;Database=enchante;Uid=root;Pwd=;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = $"SELECT COUNT(*) FROM services WHERE {GetFilterExpressionServices()}";
                    MySqlCommand countCommand = new MySqlCommand(query, connection);
                    int totalRows = Convert.ToInt32(countCommand.ExecuteScalar());
                    connection.Close();
                    return totalRows;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return 0;
            }
        }


        private void MngrServicesSearchTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchText = MngrServicesSearchTextBox.Text.Trim();

            DataView dv = ((DataTable)MngrInventoryServicesTable.DataSource).DefaultView;

            if (string.IsNullOrEmpty(searchText))
            {
                dv.RowFilter = string.Empty;
            }
            else
            {
                string filterExpression = string.Join(" OR ", ((DataTable)MngrInventoryServicesTable.DataSource).Columns.Cast<DataColumn>()
                                                    .Select(col => $"{col.ColumnName} LIKE '{searchText}%'"));
                dv.RowFilter = filterExpression;
            }

            UpdateDataGridViewAndLabelServices();
            ApplyRowAlternatingColors(MngrInventoryServicesTable);
        }

        private void RecInventoryServicesBtn_Click_1(object sender, EventArgs e)
        {
            MngrServiceDataColor();
            ReceptionLoadServices();
            ExitFunction();
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
            if (MngrServicesCategoryComboText.SelectedItem != null)
            {
                PopulateRequiredItemsComboBox();
            }
        }

        private void UpdateServiceTypeComboBox()
        {
            MngrServicesTypeComboText.Items.Clear();

            string selectedCategory = MngrServicesCategoryComboText.SelectedItem.ToString();

            switch (selectedCategory)
            {
                case "Hair Styling":
                    MngrServicesTypeComboText.Items.AddRange(new string[] { "Hair Cut", "Hair Blowout", "Hair Color", "Hair Extension", "Package" });
                    break;
                case "Nail Care":
                    MngrServicesTypeComboText.Items.AddRange(new string[] { "Manicure", "Pedicure", "Nail Extension", "Nail Art", "Nail Treatment", "Nail Repair", "Package" });
                    break;
                case "Face & Skin":
                    MngrServicesTypeComboText.Items.AddRange(new string[] { "Skin Whitening", "Exfoliation Treatment", "Chemical Peel", "Hydration Treatment", "Acne Treatment", "Anti-Aging Treatment", "Package" });
                    break;
                case "Massage":
                    MngrServicesTypeComboText.Items.AddRange(new string[] { "Soft Massage", "Moderate Massage", "Hard Massage", "Package" });
                    break;
                case "Spa":
                    MngrServicesTypeComboText.Items.AddRange(new string[] { "Herbal Pool", "Sauna", "Package" });
                    break;
                default:
                    break;
            }

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
                string categoryCode = selectedCategory.Substring(0, 2).ToUpper();
                char typeCode = selectedType[0];
                string randomPart = GenerateRandomNumber();
                string serviceID = $"{categoryCode}-{typeCode}-{randomPart:D6}";
                return serviceID;
            }

            private static string GenerateRandomNumber()
            {
                int randomNumber = random.Next(100000, 999999);
                return randomNumber.ToString();
            }
        }

        private void GenerateServiceID()
        {
            if (MngrServicesCategoryComboText.SelectedIndex >= 0 && MngrServicesTypeComboText.SelectedIndex >= 0)
            {
                string selectedCategory = MngrServicesCategoryComboText.SelectedItem.ToString();
                string selectedType = MngrServicesTypeComboText.SelectedItem.ToString();
                string generatedServiceID = DynamicIDGenerator.GenerateServiceID(selectedCategory, selectedType);

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
            string reqitem = MngrServicesSelectedReqItemText.Text;
            string numofitem = MngrServicesNumOfItems.Text;

            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(type) && string.IsNullOrEmpty(category) && string.IsNullOrEmpty(describe)
                && string.IsNullOrEmpty(duration) && string.IsNullOrEmpty(price) && string.IsNullOrEmpty(reqitem) && string.IsNullOrEmpty(numofitem))
            {
                MessageBox.Show("Missing text on required fields.", "Missing Text", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(type) || string.IsNullOrEmpty(category) || string.IsNullOrEmpty(describe)
                || string.IsNullOrEmpty(duration) || string.IsNullOrEmpty(price) || string.IsNullOrEmpty(reqitem) || string.IsNullOrEmpty(numofitem))
            {
                MessageBox.Show("Missing text on required fields.", "Missing Text", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (!IsNumericTwo(MngrServicesPriceText.Text))
            {
                MessageBox.Show("Invalid Price Number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (!IsValidFormat(numofitem))
            {
                MessageBox.Show("Value in 'Number of Items' must be a single number or in the format 'num,num,num,...' based on how many selected items on Selected Required Item Field.",
                    "Invalid Format", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            else
            {

                try
                {
                    using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                    {
                        connection.Open();
                        string checkIDQuery = "SELECT COUNT(*) FROM services WHERE ServiceID = @ID";
                        MySqlCommand checkIDCmd = new MySqlCommand(checkIDQuery, connection);
                        checkIDCmd.Parameters.AddWithValue("@ID", ID);

                        int ID_Count = Convert.ToInt32(checkIDCmd.ExecuteScalar());

                        if (ID_Count > 0)
                        {
                            MessageBox.Show("Service ID already exists. Please use a different ID Number.", "Salon Service Exists", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        string insertQuery = "INSERT INTO services (Category, Type, ServiceID, Name, Description, Duration, Price, RequiredItem, NumOfItems)" +
                            "VALUES (@category, @type, @ID, @name, @describe, @duration, @price, @reqitem, @numofitem)";

                        MySqlCommand cmd = new MySqlCommand(insertQuery, connection);
                        cmd.Parameters.AddWithValue("@category", category);
                        cmd.Parameters.AddWithValue("@type", type);
                        cmd.Parameters.AddWithValue("@ID", ID);
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@describe", describe);
                        cmd.Parameters.AddWithValue("@duration", duration);
                        cmd.Parameters.AddWithValue("@price", price);
                        cmd.Parameters.AddWithValue("@reqitem", reqitem);
                        cmd.Parameters.AddWithValue("@numofitem", numofitem);

                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Salon service is successfully created.", "Enchanté Service", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ServiceBoxClear();
                    ReceptionLoadServices();
                    GenerateServiceID();
                    selectedItems.Clear();

                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("MySQL Error: " + ex.Message, "Creating Enchanté Service Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private bool IsValidFormat(string input)
        {
            string[] parts = input.Split(',');

            foreach (string part in parts)
            {
                if (!IsNumeric(part.Trim()))
                {
                    return false;
                }
            }
            return true;
        }

        private void ServiceBoxClear()
        {
            MngrServicesCreateBtn.Visible = true;
            MngrServicesUpdateBtn.Visible = false;
            MngrServicesCategoryComboText.Enabled = true;
            MngrServicesTypeComboText.Enabled = true;
            MngrServicesCategoryComboText.SelectedIndex = -1;
            MngrServicesTypeComboText.SelectedIndex = -1;
            MngrServicesRequiredItemBox.SelectedIndex = -1;
            MngrServicesCategoryComboText.Text = "";
            MngrServicesTypeComboText.Text = "";
            MngrServicesNameText.Text = "";
            MngrServicesDescriptionText.Text = "";
            MngrServicesDurationText.Text = "";
            MngrServicesPriceText.Text = "";
            MngrServicesIDNumText.Text = "";
            MngrServicesSelectedReqItemText.Text = "";
            MngrServicesNumOfItems.Text = "";
        }

        private void RecServicesUpdateInfoBtn_Click(object sender, EventArgs e)
        {
            if (MngrInventoryServicesTable.SelectedRows.Count > 0)
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to edit the selected data?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialogResult == DialogResult.Yes)
                {
                    foreach (DataGridViewRow selectedRow in MngrInventoryServicesTable.SelectedRows)
                    {
                        try
                        {
                            RetrieveServiceDataFromDB(selectedRow);
                            MngrServicesUpdateBtn.Visible = true;
                            MngrServicesCancelButton.Visible = true;
                            MngrServicesCreateBtn.Visible = false;
                            MngrServicesCategoryComboText.Enabled = false;
                            MngrServicesTypeComboText.Enabled = false;
                            selectedItems.Clear();
                        }
                        catch (Exception ex)
                        {
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
                            string reqItem = reader["RequiredItem"].ToString();
                            string numofItems = reader["NumOfItems"].ToString();

                            MngrServicesCategoryComboText.Text = serviceCategory;
                            MngrServicesTypeComboText.Text = serviceType;
                            MngrServicesIDNumText.Text = serviceID;
                            MngrServicesNameText.Text = serviceName;
                            MngrServicesDescriptionText.Text = serviceDescribe;
                            MngrServicesDurationText.Text = serviceDuration;
                            MngrServicesPriceText.Text = servicePrice;
                            MngrServicesSelectedReqItemText.Text = reqItem;
                            MngrServicesNumOfItems.Text = numofItems;
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
            string reqitem = MngrServicesSelectedReqItemText.Text;
            string numofitem = MngrServicesNumOfItems.Text;

            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(type) && string.IsNullOrEmpty(category) && string.IsNullOrEmpty(describe)
                && string.IsNullOrEmpty(duration) && string.IsNullOrEmpty(price) && string.IsNullOrEmpty(ID) && string.IsNullOrEmpty(reqitem)
                && string.IsNullOrEmpty(numofitem))
            {
                MessageBox.Show("Missing text on required fields.", "Missing Text", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(type) || string.IsNullOrEmpty(category) || string.IsNullOrEmpty(describe)
                || string.IsNullOrEmpty(duration) || string.IsNullOrEmpty(price) || string.IsNullOrEmpty(ID) || string.IsNullOrEmpty(reqitem)
                || string.IsNullOrEmpty(numofitem))
            {
                MessageBox.Show("Missing text on required fields.", "Missing Text", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (!IsNumericTwo(MngrServicesPriceText.Text))
            {
                MessageBox.Show("Invalid Price Number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (!IsValidFormat(numofitem))
            {
                MessageBox.Show("Value in 'Number of Items' must be a single number or in the format 'num,num,num,...' based on how many selected items on Selected Required Item Field.",
                    "Invalid Format", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            else
            {
                try
                {
                    using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                    {
                        connection.Open();

                        string checkExistQuery = "SELECT COUNT(*) FROM services WHERE ServiceID = @ID";
                        MySqlCommand checkExistCmd = new MySqlCommand(checkExistQuery, connection);
                        checkExistCmd.Parameters.AddWithValue("@ID", ID);
                        int serviceCount = Convert.ToInt32(checkExistCmd.ExecuteScalar());

                        if (serviceCount == 0)
                        {
                            MessageBox.Show("Service with the provided ID does not exist in the database.", "Service Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        string updateQuery = "UPDATE services SET Category = @category, Type = @type, Name = @name, Description = @describe, Duration = @duration, Price = @price, " +
                            "RequiredItem = @reqitem, NumOfItems = @numofitem " +
                            "WHERE ServiceID = @ID";

                        MySqlCommand updateCmd = new MySqlCommand(updateQuery, connection);
                        updateCmd.Parameters.AddWithValue("@category", category);
                        updateCmd.Parameters.AddWithValue("@type", type);
                        updateCmd.Parameters.AddWithValue("@ID", ID);
                        updateCmd.Parameters.AddWithValue("@name", name);
                        updateCmd.Parameters.AddWithValue("@describe", describe);
                        updateCmd.Parameters.AddWithValue("@duration", duration);
                        updateCmd.Parameters.AddWithValue("@price", price);
                        updateCmd.Parameters.AddWithValue("@reqitem", reqitem);
                        updateCmd.Parameters.AddWithValue("@numofitem", numofitem);

                        updateCmd.ExecuteNonQuery();

                    }
                    MessageBox.Show("Service information has been successfully updated.", "Service Info Update", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MngrServicesCancelButton.Visible = false;
                    ServiceBoxClear();
                    ReceptionLoadServices();
                    selectedItems.Clear();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("MySQL Error: " + ex.Message, "Updating Service Information Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
        }


        private void MngrServicesCancelButton_Click(object sender, EventArgs e)
        {
            ServiceBoxClear();
            selectedItems.Clear();
            MngrServicesCancelButton.Visible = false;
        }

        private void PopulateRequiredItemsComboBox()
        {
            MngrServicesRequiredItemBox.Items.Clear();

            string selectedCategory = MngrServicesCategoryComboText.SelectedItem?.ToString();

            string connectionString = "Server=localhost;Database=enchante;User=root;Password=;";
            string query = "SELECT ItemName FROM inventory WHERE ProductType = 'Service Product' AND ProductCategory = @Category";

            if (selectedCategory != null)
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))

                    try
                    {
                        connection.Open();
                        MySqlCommand command = new MySqlCommand(query, connection);
                        command.Parameters.AddWithValue("@Category", selectedCategory);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string itemName = reader["ItemName"].ToString();
                                MngrServicesRequiredItemBox.Items.Add(itemName);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
            }
        }

        private HashSet<string> selectedItems = new HashSet<string>();

        private void MngrServicesRequiredItemBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MngrServicesRequiredItemBox.SelectedItem != null)
            {
                string selectedCategory = MngrServicesCategoryComboText.SelectedItem?.ToString();
                string selectedItemName = MngrServicesRequiredItemBox.SelectedItem.ToString();

                if (selectedItems.Contains(selectedItemName))
                {
                    MessageBox.Show("You have already selected this item.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string connectionString = "Server=localhost;Database=enchante;User=root;Password=;";
                string query = "SELECT ItemID FROM inventory WHERE ProductType = 'Service Product' AND ProductCategory = @Category AND ItemName = @ItemName";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        MySqlCommand command = new MySqlCommand(query, connection);
                        command.Parameters.AddWithValue("@Category", selectedCategory);
                        command.Parameters.AddWithValue("@ItemName", selectedItemName);

                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            if (!string.IsNullOrEmpty(MngrServicesSelectedReqItemText.Text))
                            {
                                MngrServicesSelectedReqItemText.Text += "," + result.ToString();
                            }
                            else
                            {
                                MngrServicesSelectedReqItemText.Text = result.ToString();
                            }
                            selectedItems.Add(selectedItemName);
                            MngrServicesRequiredItemBox.SelectedIndex = -1;
                        }
                        else
                        {
                            MessageBox.Show("ItemID not found for the selected item.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void MngrServicesDeleteBtn_Click(object sender, EventArgs e)
        {
            MngrServicesSelectedReqItemText.Text = "";
            selectedItems.Clear();
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
                    string countQuery = "SELECT COUNT(*) FROM inventory";
                    MySqlCommand countCommand = new MySqlCommand(countQuery, connection);
                    int totalRows = Convert.ToInt32(countCommand.ExecuteScalar());
                    string query = "SELECT ItemID, ProductCategory, ItemName, ItemStock, ItemPrice, ProductType, " +
                        "ItemStatus FROM inventory LIMIT 10";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            int currentBatch = totalRows > 0 ? 1 : 0;
                            int totalBatches = (int)Math.Ceiling((double)totalRows / 10);                         
                            MngrInventoryProductsCurrentRecordLbl.Text = $"{currentBatch} of {totalBatches}";
                            MngrInventoryProductsTable.DataSource = dataTable;
                            ApplyRowAlternatingColors(MngrInventoryProductsTable);
                            MngrInventoryProductsTable.RowTemplate.Height = 41;
                            MngrInventoryProductsTable.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                            MngrInventoryProductsTable.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private int currentBatchProducts = 1;

        private void MngrInventoryProductsNextBtn_Click(object sender, EventArgs e)
        {
            int totalBatches = string.IsNullOrEmpty(MngrInventoryProductsSearchTextBox.Text.Trim())
                ? (int)Math.Ceiling((double)GetTotalRowsProducts() / 10)
                : (int)Math.Ceiling((double)GetFilteredTotalRowsProducts() / 10);

            if (currentBatchProducts >= totalBatches)
            {
                MessageBox.Show("No more data to show.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            currentBatchProducts++;

            UpdateDataGridViewAndLabelProducts();
            ApplyRowAlternatingColors(MngrInventoryProductsTable);
        }

        private void MngrInventoryProductsPreviousBtn_Click(object sender, EventArgs e)
        {
            if (currentBatchProducts <= 1)
            {
                MessageBox.Show("No more previous data to show.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            currentBatchProducts--;

            UpdateDataGridViewAndLabelProducts();
            ApplyRowAlternatingColors(MngrInventoryProductsTable);
        }

        private void UpdateDataGridViewAndLabelProducts()
        {
            string connectionString = "Server=localhost;Database=enchante;Uid=root;Pwd=;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string countQuery = string.IsNullOrEmpty(MngrInventoryProductsSearchTextBox.Text.Trim())
                                        ? "SELECT COUNT(*) FROM inventory"
                                        : $"SELECT COUNT(*) FROM inventory WHERE {GetFilterExpressionProducts()}";

                    MySqlCommand countCommand = new MySqlCommand(countQuery, connection);
                    int totalRows = Convert.ToInt32(countCommand.ExecuteScalar());

                    if (totalRows == 0)
                    {
                        MessageBox.Show("No matching data found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        MngrInventoryProductsCurrentRecordLbl.Text = "0 of 0";
                        return;
                    }

                    int totalBatches = (int)Math.Ceiling((double)totalRows / 10);

                    currentBatchProducts = Math.Min(currentBatchProducts, totalBatches);

                    string query = string.IsNullOrEmpty(MngrInventoryProductsSearchTextBox.Text.Trim())
                                    ? GetRegularQueryProducts()
                                    : GetFilteredQueryProducts();

                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    System.Data.DataTable dataTable = new System.Data.DataTable();
                    adapter.Fill(dataTable);

                    MngrInventoryProductsTable.Columns.Clear();
                    MngrInventoryProductsTable.DataSource = dataTable;
                    MngrInventoryProductsCurrentRecordLbl.Text = $"{currentBatchProducts} of {totalBatches}";

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private string GetRegularQueryProducts()
        {
            int startIndex = (currentBatchProducts - 1) * 10;
            return $"SELECT ItemID, ProductCategory, ItemName, ItemStock, ItemPrice, ProductType, " +
                    $"ItemStatus FROM inventory " +
                    $"LIMIT {startIndex}, 10";
        }

        private string GetFilteredQueryProducts()
        {
            string filterExpression = GetFilterExpressionProducts();
            int startIndex = (currentBatchProducts - 1) * 10;
            return $"SELECT ItemID, ProductCategory, ItemName, ItemStock, ItemPrice, ProductType, " +
                    $"ItemStatus FROM inventory " +
                    $"WHERE {filterExpression} " +
                    $"LIMIT {startIndex}, 10";
        }

        private string GetFilterExpressionProducts()
        {
            string searchText = MngrInventoryProductsSearchTextBox.Text.Trim();
            return string.Join(" OR ", ((DataTable)MngrInventoryProductsTable.DataSource).Columns.Cast<DataColumn>()
                                .Select(col => $"{col.ColumnName} LIKE '%{searchText}%'"));
        }

        private int GetTotalRowsProducts()
        {
            string connectionString = "Server=localhost;Database=enchante;Uid=root;Pwd=;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string countQuery = "SELECT COUNT(*) FROM inventory";
                    MySqlCommand countCommand = new MySqlCommand(countQuery, connection);
                    int totalRows = Convert.ToInt32(countCommand.ExecuteScalar());
                    connection.Close();
                    return totalRows;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return 0;
            }
        }

        private int GetFilteredTotalRowsProducts()
        {
            string connectionString = "Server=localhost;Database=enchante;Uid=root;Pwd=;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = $"SELECT COUNT(*) FROM inventory WHERE {GetFilterExpressionProducts()}";
                    MySqlCommand countCommand = new MySqlCommand(query, connection);
                    int totalRows = Convert.ToInt32(countCommand.ExecuteScalar());
                    connection.Close();
                    return totalRows;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return 0;
            }
        }

        private void MngrInventoryProductsSearchTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchText = MngrInventoryProductsSearchTextBox.Text.Trim();

            DataView dv = ((DataTable)MngrInventoryProductsTable.DataSource).DefaultView;

            if (string.IsNullOrEmpty(searchText))
            {
                dv.RowFilter = string.Empty;
            }
            else
            {
                string filterExpression = string.Join(" OR ", ((DataTable)MngrInventoryProductsTable.DataSource).Columns.Cast<DataColumn>()
                                                    .Select(col => $"{col.ColumnName} LIKE '{searchText}%'"));
                dv.RowFilter = filterExpression;
            }

            UpdateDataGridViewAndLabelProducts();
            ApplyRowAlternatingColors(MngrInventoryProductsTable);
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

        public System.Drawing.Image firststoredImage;

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

                            object result = command.ExecuteScalar();

                            if (result != DBNull.Value && result != null)
                            {
                                byte[] imageData = (byte[])result;
                                if (imageData != null && imageData.Length > 0)
                                {
                                    using (MemoryStream ms = new MemoryStream(imageData))
                                    {
                                        ProductImagePictureBox.Image = System.Drawing.Image.FromStream(ms);
                                        firststoredImage = ProductImagePictureBox.Image;
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

            System.Drawing.Image storedImage = firststoredImage;

            System.Drawing.Image currentImage = null;
            if (ProductImagePictureBox.Image != null)
            {
                currentImage = (System.Drawing.Image)ProductImagePictureBox.Image.Clone();
            }

            bool imagesAreEqual = ImagesAreEqual(storedImage, currentImage);

            bool imagewillnotupdate = true;
            if (imagesAreEqual)
            {
                imagewillnotupdate = true;
            }
            else
            {
                imagewillnotupdate = false;
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
            string query;
            if (!imagewillnotupdate)
            {
                query = @"UPDATE inventory 
                                SET ItemName = @ItemName, 
                                ItemPrice = @ItemPrice, 
                                ItemStock = @ItemStock, 
                                ProductCategory = @ProductCategory, 
                                ProductType = @ProductType, 
                                ItemStatus = @ItemStatus,
                                ProductPicture = @ProductPicture
                                WHERE ItemID = @ItemID";
            }
            else
            {
                query = @"UPDATE inventory 
                                SET ItemName = @ItemName, 
                                ItemPrice = @ItemPrice, 
                                ItemStock = @ItemStock, 
                                ProductCategory = @ProductCategory, 
                                ProductType = @ProductType, 
                                ItemStatus = @ItemStatus
                                WHERE ItemID = @ItemID";
            }

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

                    if (MngrInventoryProductsTypeComboText.SelectedItem.ToString() == "Retail Product" && ProductImagePictureBox.Image != null && imagewillnotupdate == false)
                    {
                        MemoryStream ms = new MemoryStream();
                        ProductImagePictureBox.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        byte[] imageData = ms.ToArray();
                        command.Parameters.AddWithValue("@ProductPicture", imageData);
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
                                    if (reader["ItemName"].ToString() != MngrInventoryProductsNameText.Text ||
                                        reader["ItemPrice"].ToString() != MngrInventoryProductsPriceText.Text ||
                                        reader["ItemStock"].ToString() != MngrInventoryProductsStockText.Text ||
                                        reader["ProductCategory"].ToString() != MngrInventoryProductsCatComboText.SelectedItem.ToString() ||
                                        reader["ProductType"].ToString() != MngrInventoryProductsTypeComboText.SelectedItem.ToString() ||
                                        reader["ItemStatus"].ToString() != MngrInventoryProductsStatusComboText.SelectedItem.ToString())
                                    {
                                        fieldsChanged = true;
                                    }

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

        private bool ImagesAreEqual(System.Drawing.Image image1, System.Drawing.Image image2)
        {
            if (image1 == null && image2 == null)
            {
                return true;
            }
            else if (image1 == null || image2 == null)
            {
                return false;
            }

            if (image1.Width != image2.Width || image1.Height != image2.Height)
            {
                return false;
            }

            Bitmap bitmap1 = new Bitmap(image1);
            Bitmap bitmap2 = new Bitmap(image2);

            for (int x = 0; x < bitmap1.Width; x++)
            {
                for (int y = 0; y < bitmap1.Height; y++)
                {
                    Color color1 = bitmap1.GetPixel(x, y);
                    Color color2 = bitmap2.GetPixel(x, y);

                    if (color1 != color2)
                    {
                        return false;
                    }
                }
            }

            return true;
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
            ProductImagePictureBox.Image = null;

        }

        private void SelectImage_Click(object sender, EventArgs e)
        {
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

        #region Mngr. PANEL OF WALK-IN Services REVENUE
        private DataTable dt = new DataTable();
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
                    fromDate = MngrWalkinSalesFromDatePicker.Value.Date;
                    toDate = MngrWalkinSalesToDatePicker.Value.Date.AddDays(1).AddTicks(-1);
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
                        MngrWalkinSalesRevenueTextbox.Text = "";
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
                    MngrWalkinSalesGraph.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Arial", 16, FontStyle.Bold);
                    MngrWalkinSalesGraph.ChartAreas[0].AxisY.Title = "Revenue";
                    MngrWalkinSalesGraph.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Arial", 16, FontStyle.Bold);

                    MngrWalkinSalesGraph.Legends.Add("Legend1");
                    MngrWalkinSalesGraph.Legends[0].Enabled = true;
                    MngrWalkinSalesGraph.Legends[0].Docking = Docking.Bottom;
                    MngrWalkinSalesGraph.Legends[0].Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);

                    DataTable dt = new DataTable();
                    dt.Columns.Add("TransactionNumber");
                    dt.Columns.Add("AppointmentDate");
                    dt.Columns.Add("TotalServicePrice", typeof(decimal));

                    string transNumQuery = @"
                            SELECT TransactionNumber, AppointmentDate, SUM(CAST(ServicePrice AS DECIMAL(10, 2))) AS TotalServicePrice";

                    if (selectedCategory == "All Categories")
                    {
                        dt.Columns.Add("ServiceCategory");
                        transNumQuery += ", ServiceCategory";
                    }

                    transNumQuery += @"
                                FROM servicehistory 
                                WHERE ServiceStatus = 'Completed' 
                                AND STR_TO_DATE(AppointmentDate, '%m-%d-%Y %W') BETWEEN @FromDate AND @ToDate";

                    if (selectedCategory != "All Categories")
                    {
                        transNumQuery += " AND ServiceCategory = @SelectedCategory";
                    }

                    transNumQuery += @"
                                AND TransactionType = 'Walk-in Transaction'
                                GROUP BY TransactionNumber, AppointmentDate 
                                ORDER BY AppointmentDate DESC";

                    MySqlCommand transNumCommand = new MySqlCommand(transNumQuery, connection);
                    transNumCommand.Parameters.AddWithValue("@FromDate", fromDate);
                    transNumCommand.Parameters.AddWithValue("@ToDate", toDate);

                    if (selectedCategory != "All Categories")
                    {
                        transNumCommand.Parameters.AddWithValue("@SelectedCategory", selectedCategory);
                    }

                    using (MySqlDataReader transNumReader = transNumCommand.ExecuteReader())
                    {
                        int rowCount = 0;
                        while (transNumReader.Read())
                        {
                            string transactionNumber = transNumReader["TransactionNumber"].ToString();
                            string appointmentDate = transNumReader["AppointmentDate"].ToString();
                            decimal totalServicePrice = (decimal)transNumReader["TotalServicePrice"];

                            DataRow row = dt.Rows.Add(transactionNumber, appointmentDate, totalServicePrice);
                            if (selectedCategory == "All Categories")
                            {
                                if (transNumReader.FieldCount > 3 && !transNumReader.IsDBNull(3))
                                {
                                    string serviceCategory = transNumReader["ServiceCategory"].ToString();
                                    row["ServiceCategory"] = serviceCategory;
                                }
                            }

                            rowCount++;
                        }
                        int totalRows = dt.Rows.Count;
                        int pageSize = 5;
                        int totalBatches = (int)Math.Ceiling((double)totalRows / pageSize);
                        int currentBatch = (rowCount - 1) / pageSize + 1;

                        MngrWalkinSalesCurrentRecordLbl.Text = $"{1} of {totalBatches}";
                        DataTable limitedDataTable = dt.AsEnumerable().Take(5).CopyToDataTable();
                        MngrWalkinSalesTransRepDGV.RowTemplate.Height = 37;
                        MngrWalkinSalesTransRepDGV.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                        MngrWalkinSalesTransRepDGV.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                        MngrWalkinSalesTransRepDGV.DataSource = limitedDataTable;
                        MngrWalkinSalesTransRepDGVTwo.DataSource = dt;
                        ApplyRowAlternatingColors(MngrWalkinSalesTransRepDGV);
                        
                        decimal totalServicePriceSum = 0;

                        foreach (DataRow row in dt.Rows)
                        {
                            totalServicePriceSum += Convert.ToDecimal(row["TotalServicePrice"]);
                        }
                        string formattedTotalServicePrice = "₱" + totalServicePriceSum.ToString("#,##0.00");
                        MngrWalkinSalesRevenueTextbox.Text = formattedTotalServicePrice;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }

        private int currentPageWalkIn = 1;

        private void MngrWalkinSalesNextBtn_Click(object sender, EventArgs e)
        {
            ShowNextPage();
            ApplyRowAlternatingColors(MngrWalkinSalesTransRepDGV);
        }

        private void MngrWalkinSalesPreviousBtn_Click(object sender, EventArgs e)
        {
            ShowPreviousPage();
            ApplyRowAlternatingColors(MngrWalkinSalesTransRepDGV);
        }

        private void ShowNextPage()
        {
            int pageSize = 5;
            DataView filteredDataView = ((DataTable)MngrWalkinSalesTransRepDGVTwo.DataSource).DefaultView;
            if (!string.IsNullOrEmpty(filteredDataView.RowFilter))
            {
                int totalFilteredRows = filteredDataView.Count;
                int totalFilteredBatches = (int)Math.Ceiling((double)totalFilteredRows / pageSize);
                int currentFilteredBatch = currentPageWalkIn;

                if (currentFilteredBatch < totalFilteredBatches)
                {
                    int startIndex = currentFilteredBatch * pageSize;
                    int endIndex = Math.Min(startIndex + pageSize, totalFilteredRows);

                    MngrWalkinSalesTransRepDGV.DataSource = null;
                    DataTable pageDataTable = ((DataTable)MngrWalkinSalesTransRepDGVTwo.DataSource).Clone();

                    for (int i = startIndex; i < endIndex; i++)
                    {
                        DataRow newRow = filteredDataView[i].Row;
                        pageDataTable.ImportRow(newRow);
                    }
                    MngrWalkinSalesTransRepDGV.DataSource = pageDataTable;
                    currentPageWalkIn++;
                    MngrWalkinSalesCurrentRecordLbl.Text = $"{currentPageWalkIn} of {totalFilteredBatches}";
                }
                else
                {
                    MessageBox.Show("No more pages to display.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                int totalRows = MngrWalkinSalesTransRepDGVTwo.Rows.Count;
                int currentBatch = currentPageWalkIn;
                int totalBatches = (int)Math.Ceiling((double)totalRows / pageSize);

                if (currentBatch < totalBatches)
                {
                    int startIndex = currentBatch * pageSize;
                    int endIndex = Math.Min(startIndex + pageSize, totalRows);

                    MngrWalkinSalesTransRepDGV.DataSource = null;
                    DataTable pageDataTable = ((DataTable)MngrWalkinSalesTransRepDGVTwo.DataSource).Clone();

                    for (int i = startIndex; i < endIndex; i++)
                    {
                        DataRow newRow = ((DataTable)MngrWalkinSalesTransRepDGVTwo.DataSource).Rows[i];
                        pageDataTable.ImportRow(newRow);
                    }

                    MngrWalkinSalesTransRepDGV.DataSource = pageDataTable;
                    currentPageWalkIn++;
                    MngrWalkinSalesCurrentRecordLbl.Text = $"{currentPageWalkIn} of {totalBatches}";
                }
                else
                {
                    MessageBox.Show("No more pages to display.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void ShowPreviousPage()
        {
            if (currentPageWalkIn > 1)
            {
                currentPageWalkIn--;

                int pageSize = 5;
                int startIndex = (currentPageWalkIn - 1) * pageSize;

                MngrWalkinSalesTransRepDGV.DataSource = null;

                DataTable pageDataTable = ((DataTable)MngrWalkinSalesTransRepDGVTwo.DataSource).Clone();
                DataView filteredDataView = ((DataTable)MngrWalkinSalesTransRepDGVTwo.DataSource).DefaultView;

                for (int i = startIndex; i < startIndex + pageSize; i++)
                {
                    if (i < filteredDataView.Count)
                    {
                        DataRow newRow = filteredDataView[i].Row;
                        pageDataTable.ImportRow(newRow);
                    }
                    else
                    {
                        break;
                    }
                }
                MngrWalkinSalesTransRepDGV.DataSource = pageDataTable;

                int totalFilteredRows = filteredDataView.Count;
                int totalFilteredBatches = (int)Math.Ceiling((double)totalFilteredRows / pageSize);

                MngrWalkinSalesCurrentRecordLbl.Text = $"{currentPageWalkIn} of {totalFilteredBatches}";
            }
            else
            {
                MessageBox.Show("Already on the first page.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void MngrWalkinSalesSearchTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchText = MngrWalkinSalesSearchTextBox.Text.Trim();

            DataView dv = ((DataTable)MngrWalkinSalesTransRepDGVTwo.DataSource).DefaultView;

            if (string.IsNullOrEmpty(searchText))
            {
                dv.RowFilter = string.Empty;
            }
            else
            {
                string filterExpression = string.Join(" OR ", ((DataTable)MngrWalkinSalesTransRepDGVTwo.DataSource).Columns.Cast<DataColumn>()
                                                    .Select(col => col.DataType != typeof(decimal)
                                                        ? $"{col.ColumnName} LIKE '%{searchText}%'"
                                                        : $"CONVERT([{col.ColumnName}], 'System.String') LIKE '%{searchText}%'"));
                dv.RowFilter = filterExpression;
            }

            int totalBatches = (int)Math.Ceiling((double)dv.Count / 5);

            if (totalBatches > 0)
            {
                MngrWalkinSalesCurrentRecordLbl.Text = $"1 of {totalBatches}";
            }
            else
            {
                MngrWalkinSalesCurrentRecordLbl.Text = "0 of 0";
                MngrWalkinSalesTransRepDGV.DataSource = null;
            }

            if (dv.Count == 0)
            {
                MessageBox.Show("No matching data found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                DataTable limitedDataTable = dv.ToTable().AsEnumerable().Take(5).CopyToDataTable();
                MngrWalkinSalesTransRepDGV.DataSource = limitedDataTable;
                ApplyRowAlternatingColors(MngrWalkinSalesTransRepDGVTwo);
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
            MngrWalkinSalesRevenueTextbox.Text = "";
            MngrWalkinSalesCurrentRecordLbl.Text = "0 of 0";
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
                            title.Font = new System.Drawing.Font("Arial", 14f, System.Drawing.FontStyle.Bold);
                            MngrIndemandServiceGraph.Legends[0].Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
                            MngrIndemandServiceGraph.Legends[0].Font = new System.Drawing.Font("Arial", 10f, System.Drawing.FontStyle.Bold);

                            PopulateServiceSelectionGrid(fromDate.ToString("yyyy-MM-dd"), toDate.ToString("yyyy-MM-dd"));
                            ApplyRowAlternatingColors(MngrIndemandServiceSelection);

                            DataTable staffTable = new DataTable();
                            staffTable.Columns.Add("Rank");
                            staffTable.Columns.Add("ID");
                            staffTable.Columns.Add("First Name");
                            staffTable.Columns.Add("Last Name");
                            staffTable.Columns.Add("Services Done");

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
                            MngrIndemandBestEmployee.Columns["Rank"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            MngrIndemandBestEmployee.Columns["Services Done"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
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

                            int rowCount = 0;
                            rowCount++;
                            int totalRows = serviceTable.Rows.Count;
                            int pageSize = 10;
                            int totalBatches = (int)Math.Ceiling((double)totalRows / pageSize);
                            int currentBatch = (rowCount - 1) / pageSize + 1;

                            MngrIndemandCurrentRecordLbl.Text = $"{1} of {totalBatches}";
                            DataTable limitedDataTable = serviceTable.AsEnumerable().Take(10).CopyToDataTable();
                            MngrIndemandServiceSelection.RowTemplate.Height = 38;
                            MngrIndemandServiceSelection.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                            MngrIndemandServiceSelection.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                            MngrIndemandServiceSelection.DataSource = limitedDataTable;       
                            MngrIndemandServiceSelectionTwo.DataSource = serviceTable;
                            serviceTable.DefaultView.Sort = "Service Selection Counts DESC";
                            limitedDataTable.DefaultView.Sort = "Service Selection Counts DESC";
                            ApplyRowAlternatingColors(MngrIndemandServiceSelection);

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
                            chartTitle.Font = new System.Drawing.Font("Arial", 14f, System.Drawing.FontStyle.Bold);
                            MngrIndemandServiceGraph.Legends[0].Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
                            MngrIndemandServiceGraph.Legends[0].Font = new System.Drawing.Font("Arial", 10f, System.Drawing.FontStyle.Bold);
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
            int totalRows = serviceTable.Rows.Count;
            int pageSize = 10;
            int totalBatches = (int)Math.Ceiling((double)totalRows / pageSize);

            MngrIndemandCurrentRecordLbl.Text = $"{1} of {totalBatches}";

            DataTable limitedDataTable = serviceTable.AsEnumerable().Take(10).CopyToDataTable();
            MngrIndemandServiceSelection.RowTemplate.Height = 38;
            MngrIndemandServiceSelection.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            MngrIndemandServiceSelection.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            MngrIndemandServiceSelection.DataSource = limitedDataTable;
            MngrIndemandServiceSelectionTwo.DataSource = serviceTable;      

            serviceTable.DefaultView.Sort = "Service Count DESC";
            limitedDataTable.DefaultView.Sort = "Service Count DESC";
        }

        private int currentPageFourSales = 1;

        private void MngrIndemandNextBtn_Click(object sender, EventArgs e)
        {
            ShowNextPageFour();
            ApplyRowAlternatingColors(MngrIndemandServiceSelection);
        }

        private void MngrIndemandPreviousBtn_Click(object sender, EventArgs e)
        {
            ShowPreviousPageFour();
            ApplyRowAlternatingColors(MngrIndemandServiceSelection);
        }

        private void ShowNextPageFour()
        {
            int pageSize = 10;
            DataView filteredDataView = ((DataTable)MngrIndemandServiceSelectionTwo.DataSource).DefaultView;
            if (!string.IsNullOrEmpty(filteredDataView.RowFilter))
            {
                int totalFilteredRows = filteredDataView.Count;
                int totalFilteredBatches = (int)Math.Ceiling((double)totalFilteredRows / pageSize);
                int currentFilteredBatch = currentPageFourSales;

                if (currentFilteredBatch < totalFilteredBatches)
                {
                    int startIndex = currentFilteredBatch * pageSize;
                    int endIndex = Math.Min(startIndex + pageSize, totalFilteredRows);

                    MngrIndemandServiceSelection.DataSource = null;
                    DataTable pageDataTable = ((DataTable)MngrIndemandServiceSelectionTwo.DataSource).Clone();

                    for (int i = startIndex; i < endIndex; i++)
                    {
                        DataRow newRow = filteredDataView[i].Row;
                        pageDataTable.ImportRow(newRow);
                    }
                    MngrIndemandServiceSelection.DataSource = pageDataTable;
                    currentPageFourSales++;
                    MngrIndemandCurrentRecordLbl.Text = $"{currentPageFourSales} of {totalFilteredBatches}";
                }
                else
                {
                    MessageBox.Show("No more pages to display.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                int totalRows = MngrIndemandServiceSelectionTwo.Rows.Count;
                int currentBatch = currentPageFourSales;
                int totalBatches = (int)Math.Ceiling((double)totalRows / pageSize);

                if (currentBatch < totalBatches)
                {
                    int startIndex = currentBatch * pageSize;
                    int endIndex = Math.Min(startIndex + pageSize, totalRows);

                    MngrIndemandServiceSelection.DataSource = null;
                    DataTable pageDataTable = ((DataTable)MngrIndemandServiceSelectionTwo.DataSource).Clone();

                    for (int i = startIndex; i < endIndex; i++)
                    {
                        DataRow newRow = ((DataTable)MngrIndemandServiceSelectionTwo.DataSource).Rows[i];
                        pageDataTable.ImportRow(newRow);
                    }

                    MngrIndemandServiceSelection.DataSource = pageDataTable;
                    currentPageFourSales++;
                    MngrIndemandCurrentRecordLbl.Text = $"{currentPageFourSales} of {totalBatches}";
                }
                else
                {
                    MessageBox.Show("No more pages to display.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void ShowPreviousPageFour()
        {
            if (currentPageFourSales > 1)
            {
                currentPageFourSales--;

                int pageSize = 10;
                int startIndex = (currentPageFourSales - 1) * pageSize;

                MngrIndemandServiceSelection.DataSource = null;

                DataTable pageDataTable = ((DataTable)MngrIndemandServiceSelectionTwo.DataSource).Clone();
                DataView filteredDataView = ((DataTable)MngrIndemandServiceSelectionTwo.DataSource).DefaultView;

                for (int i = startIndex; i < startIndex + pageSize; i++)
                {
                    if (i < filteredDataView.Count)
                    {
                        DataRow newRow = filteredDataView[i].Row;
                        pageDataTable.ImportRow(newRow);
                    }
                    else
                    {
                        break;
                    }
                }
                MngrIndemandServiceSelection.DataSource = pageDataTable;

                int totalFilteredRows = filteredDataView.Count;
                int totalFilteredBatches = (int)Math.Ceiling((double)totalFilteredRows / pageSize);

                MngrIndemandCurrentRecordLbl.Text = $"{currentPageFourSales} of {totalFilteredBatches}";
            }
            else
            {
                MessageBox.Show("Already on the first page.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void MngrIndemandSearchTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchText = MngrIndemandSearchTextBox.Text.Trim();

            DataView dv = ((DataTable)MngrIndemandServiceSelectionTwo.DataSource).DefaultView;

            if (string.IsNullOrEmpty(searchText))
            {
                dv.RowFilter = null;
            }
            else
            {
                StringBuilder filterExpressionBuilder = new StringBuilder();
                foreach (DataColumn col in ((DataTable)MngrIndemandServiceSelectionTwo.DataSource).Columns)
                {
                    if (col.DataType != typeof(decimal))
                    {
                        Console.WriteLine($"Column Name: {col.ColumnName}");

                        if (filterExpressionBuilder.Length > 0)
                        {
                            filterExpressionBuilder.Append(" OR ");
                        }
                        filterExpressionBuilder.Append($"[{col.ColumnName}] LIKE '%{searchText}%'");
                    }
                }
                string filterExpression = filterExpressionBuilder.ToString();
                Console.WriteLine($"Combined Filter Expression: {filterExpression}");
                dv.RowFilter = string.IsNullOrEmpty(filterExpression) ? null : filterExpression;
                ApplyRowAlternatingColors(MngrIndemandServiceSelection);
            }
            int totalBatches = (int)Math.Ceiling((double)dv.Count / 10);
            if (totalBatches > 0)
            {
                MngrIndemandCurrentRecordLbl.Text = $"1 of {totalBatches}";
            }
            else
            {
                MngrIndemandCurrentRecordLbl.Text = "0 of 0";
                MngrIndemandServiceSelection.DataSource = null;
            }

            if (dv.Count == 0)
            {
                MessageBox.Show("No matching data found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                DataTable limitedDataTable = dv.ToTable().AsEnumerable().Take(10).CopyToDataTable();
                MngrIndemandServiceSelection.DataSource = limitedDataTable;
                ApplyRowAlternatingColors(MngrIndemandServiceSelection);
            }
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
            MngrIndemandCurrentRecordLbl.Text = "0 of 0";
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
                    MngrProductSalesTotalRevBox.Text = "";
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
                    MngrProductSalesGraph.Titles.Add("Quantity Sold Distribution").Font = new System.Drawing.Font("Arial", 14, FontStyle.Bold | FontStyle.Italic);
                    MngrProductSalesGraph.Legends[0].Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
                    MngrProductSalesGraph.Legends[0].Font = new System.Drawing.Font("Arial", 10f, System.Drawing.FontStyle.Bold);
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
                    MngrProductSalesLineGraph.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Arial", 14, FontStyle.Bold);
                    MngrProductSalesLineGraph.ChartAreas[0].AxisY.Title = "Revenue";
                    MngrProductSalesLineGraph.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Arial", 14, FontStyle.Bold);
                    MngrProductSalesLineGraph.ChartAreas["MainChartArea"].Position = new ElementPosition(5, 5, 90, 70);
                    MngrProductSalesLineGraph.ChartAreas["MainChartArea"].InnerPlotPosition.Auto = false;
                    MngrProductSalesLineGraph.Titles.Clear();
                    MngrProductSalesLineGraph.Titles.Add("Sales Revenue").Font = new System.Drawing.Font("Arial", 14, FontStyle.Bold | FontStyle.Italic);
                    MngrProductSalesLineGraph.Legends.Add(new Legend("MainLegend"));
                    MngrProductSalesLineGraph.Legends["MainLegend"].Docking = Docking.Bottom;
                    MngrProductSalesLineGraph.Legends[0].Font = new System.Drawing.Font("Arial", 10f, System.Drawing.FontStyle.Bold);
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
            if (MngrProductSalesSelectCatBox.Text == "All Categories")
            {
                DisplayAllCategoriesData(filteredData);
            }
            else
            {
                DisplayFilteredCategoriesData(filteredData);
            }
        }

        private void DisplayAllCategoriesData(DataTable filteredData)
        {
            MngrProductSalesTransRepDGV.Columns.Clear();
            MngrProductSalesTransRepDGV.DataSource = null;

            DataTable dt = new DataTable();
            dt.Columns.Add("Category");
            dt.Columns.Add("Quantity Sold");
            dt.Columns.Add("Overall Revenue");

            Dictionary<string, int> categoryQuantities = new Dictionary<string, int>();
            Dictionary<string, double> categoryRevenues = new Dictionary<string, double>();
            double totalRevenue = 0;

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
                totalRevenue += itemTotalPrice;
            }

            MngrProductSalesCurrentRecordLbl.Text = "1 of 1";

            foreach (var kvp in categoryQuantities)
            {
                string categoryName = GetCategoryName(kvp.Key);
                string formattedOverallRevenue = "₱" + categoryRevenues[kvp.Key].ToString("#,##0.00");
                dt.Rows.Add(categoryName, kvp.Value, formattedOverallRevenue);
            }

            string formattedTotalRevenue = "₱" + totalRevenue.ToString("#,##0.00");
            MngrProductSalesTotalRevBox.Text = formattedTotalRevenue;
            MngrProductSalesTransRepDGVTwo.DataSource = dt;
            MngrProductSalesTransRepDGV.DataSource = dt;
        }

        private void DisplayFilteredCategoriesData(DataTable filteredData)
        {
            MngrProductSalesTransRepDGV.Columns.Clear();

            DataTable dt = new DataTable();
            dt.Columns.Add("CheckedOutDate");
            dt.Columns.Add("ItemID");
            dt.Columns.Add("ItemName");
            dt.Columns.Add("Qty");
            dt.Columns.Add("ItemPrice");
            dt.Columns.Add("TotalServicePrice");

            DataView dv = filteredData.DefaultView;
            dv.Sort = "CheckedOutDate DESC";
            DataTable sortedData = dv.ToTable();

            decimal totalRevenue = 0;

            foreach (DataRow row in sortedData.Rows)
            {
                dt.Rows.Add(
                    row["CheckedOutDate"],
                    row["ItemID"],
                    row["ItemName"],
                    row["Qty"],
                    row["ItemPrice"],
                    row["ItemTotalPrice"]
                );
                if (row["ItemTotalPrice"] != DBNull.Value)
                {
                    totalRevenue += Convert.ToDecimal(row["ItemTotalPrice"]);
                }
            }

            int totalRows = sortedData.Rows.Count;
            int pageSize = 5;
            int totalBatches = (int)Math.Ceiling((double)totalRows / pageSize);

            MngrProductSalesCurrentRecordLbl.Text = $"{1} of {totalBatches}";

            DataTable limitedDataTable = dt.AsEnumerable().Take(10).CopyToDataTable();
            MngrProductSalesTransRepDGV.RowTemplate.Height = 38;
            MngrProductSalesTransRepDGV.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            MngrProductSalesTransRepDGV.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            MngrProductSalesTransRepDGV.DataSource = limitedDataTable;
            ApplyRowAlternatingColors(MngrProductSalesTransRepDGV);

            MngrProductSalesTransRepDGVTwo.DataSource = null;
            MngrProductSalesTransRepDGVTwo.DataSource = dt;

            string formattedTotalRevenue = "₱" + totalRevenue.ToString("#,##0.00");
            MngrProductSalesTotalRevBox.Text = formattedTotalRevenue;
        }

        private int currentPageTwoSales = 1;

        private void MngrProductSalesNextBtn_Click(object sender, EventArgs e)
        {
            ShowNextPageTwo();
            ApplyRowAlternatingColors(MngrProductSalesTransRepDGV);
        }

        private void MngrProductSalesPreviousBtn_Click(object sender, EventArgs e)
        {
            ShowPreviousPageTwo();
            ApplyRowAlternatingColors(MngrProductSalesTransRepDGV);
        }

        private void ShowNextPageTwo()
        {
            int pageSize = 10;
            DataView filteredDataView = ((DataTable)MngrProductSalesTransRepDGVTwo.DataSource).DefaultView;
            if (!string.IsNullOrEmpty(filteredDataView.RowFilter))
            {
                int totalFilteredRows = filteredDataView.Count;
                int totalFilteredBatches = (int)Math.Ceiling((double)totalFilteredRows / pageSize);
                int currentFilteredBatch = currentPageTwoSales;

                if (currentFilteredBatch < totalFilteredBatches)
                {
                    int startIndex = currentFilteredBatch * pageSize;
                    int endIndex = Math.Min(startIndex + pageSize, totalFilteredRows);

                    MngrProductSalesTransRepDGV.DataSource = null;
                    DataTable pageDataTable = ((DataTable)MngrProductSalesTransRepDGVTwo.DataSource).Clone();

                    for (int i = startIndex; i < endIndex; i++)
                    {
                        DataRow newRow = filteredDataView[i].Row;
                        pageDataTable.ImportRow(newRow);
                    }
                    MngrProductSalesTransRepDGV.DataSource = pageDataTable;
                    currentPageTwoSales++;
                    MngrProductSalesCurrentRecordLbl.Text = $"{currentPageTwoSales} of {totalFilteredBatches}";
                }
                else
                {
                    MessageBox.Show("No more pages to display.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                int totalRows = MngrProductSalesTransRepDGVTwo.Rows.Count;
                int currentBatch = currentPageTwoSales;
                int totalBatches = (int)Math.Ceiling((double)totalRows / pageSize);

                if (currentBatch < totalBatches)
                {
                    int startIndex = currentBatch * pageSize;
                    int endIndex = Math.Min(startIndex + pageSize, totalRows);

                    MngrProductSalesTransRepDGV.DataSource = null;
                    DataTable pageDataTable = ((DataTable)MngrProductSalesTransRepDGVTwo.DataSource).Clone();

                    for (int i = startIndex; i < endIndex; i++)
                    {
                        DataRow newRow = ((DataTable)MngrProductSalesTransRepDGVTwo.DataSource).Rows[i];
                        pageDataTable.ImportRow(newRow);
                    }

                    MngrProductSalesTransRepDGV.DataSource = pageDataTable;
                    currentPageTwoSales++;
                    MngrProductSalesCurrentRecordLbl.Text = $"{currentPageTwoSales} of {totalBatches}";
                }
                else
                {
                    MessageBox.Show("No more pages to display.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void ShowPreviousPageTwo()
        {
            if (currentPageTwoSales > 1)
            {
                currentPageTwoSales--;

                int pageSize = 10;
                int startIndex = (currentPageTwoSales - 1) * pageSize;

                MngrProductSalesTransRepDGV.DataSource = null;

                DataTable pageDataTable = ((DataTable)MngrProductSalesTransRepDGVTwo.DataSource).Clone();
                DataView filteredDataView = ((DataTable)MngrProductSalesTransRepDGVTwo.DataSource).DefaultView;

                for (int i = startIndex; i < startIndex + pageSize; i++)
                {
                    if (i < filteredDataView.Count)
                    {
                        DataRow newRow = filteredDataView[i].Row;
                        pageDataTable.ImportRow(newRow);
                    }
                    else
                    {
                        break;
                    }
                }
                MngrProductSalesTransRepDGV.DataSource = pageDataTable;

                int totalFilteredRows = filteredDataView.Count;
                int totalFilteredBatches = (int)Math.Ceiling((double)totalFilteredRows / pageSize);

                MngrProductSalesCurrentRecordLbl.Text = $"{currentPageTwoSales} of {totalFilteredBatches}";
            }
            else
            {
                MessageBox.Show("Already on the first page.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void MngrProductSalesSearchTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchText = MngrProductSalesSearchTextBox.Text.Trim();

            DataView dv = ((DataTable)MngrProductSalesTransRepDGVTwo.DataSource).DefaultView;

            if (string.IsNullOrEmpty(searchText))
            {
                dv.RowFilter = string.Empty;
            }
            else
            {
                List<string> filterExpressions = new List<string>();
                foreach (DataColumn col in ((DataTable)MngrProductSalesTransRepDGVTwo.DataSource).Columns)
                {
                    if (col.DataType != typeof(decimal))
                    {
                        string columnName = col.ColumnName.Contains(" ") ? $"[{col.ColumnName}]" : col.ColumnName;
                        filterExpressions.Add($"{columnName} LIKE '%{searchText}%'");
                    }
                }
                string combinedFilterExpression = string.Join(" OR ", filterExpressions);
                dv.RowFilter = combinedFilterExpression;
                ApplyRowAlternatingColors(MngrProductSalesTransRepDGV);
            }
            int totalBatches = (int)Math.Ceiling((double)dv.Count / 10);

            if (totalBatches > 0)
            {
                MngrProductSalesCurrentRecordLbl.Text = $"1 of {totalBatches}";
            }
            else
            {
                MngrProductSalesCurrentRecordLbl.Text = "0 of 0";
                MngrProductSalesTransRepDGV.DataSource = null;
            }
            if (dv.Count == 0)
            {
                MessageBox.Show("No matching data found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                DataTable limitedDataTable = dv.ToTable().AsEnumerable().Take(10).CopyToDataTable();
                MngrProductSalesTransRepDGV.DataSource = limitedDataTable;
                ApplyRowAlternatingColors(MngrProductSalesTransRepDGV);
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
                    AND TransactionType = 'Walk-in Appointment Transaction'
                    AND LEFT(AppointmentDate, 10) BETWEEN @FromDate AND @ToDate ";

                    if (selectedCategory != "All Categories")
                    {
                        query += " AND ServiceCategory = @SelectedCategory";
                    }

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
                        MngrAppSalesTotalRevBox.Text = "";
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
            MngrAppSalesGraph.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Arial", 16, System.Drawing.FontStyle.Bold);
            MngrAppSalesGraph.ChartAreas[0].AxisY.Title = "Revenue";
            MngrAppSalesGraph.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Arial", 16, System.Drawing.FontStyle.Bold);

            MngrAppSalesGraph.Legends.Add("Legend1");
            MngrAppSalesGraph.Legends[0].Enabled = true;
            MngrAppSalesGraph.Legends[0].Docking = Docking.Bottom;
            MngrAppSalesGraph.Legends[0].Font = new System.Drawing.Font("Arial", 10, FontStyle.Bold);
        }

        private void AppointmentServiceBreakdown(string selectedCategory, string fromDate, string toDate, MySqlConnection connection)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("TransactionNumber");
            dt.Columns.Add("AppointmentDate");
            dt.Columns.Add("TotalServicePrice", typeof(decimal));

            if (selectedCategory == "All Categories")
            {
                dt.Columns.Add("ServiceCategory");
            }

            string transNumQuery = @"
                            SELECT TransactionNumber, AppointmentDate, ServiceCategory, SUM(CAST(ServicePrice AS DECIMAL(10, 2))) AS TotalServicePrice 
                            FROM servicehistory 
                            WHERE ServiceStatus = 'Completed' 
                            AND TransactionType = 'Walk-in Appointment Transaction'
                            AND LEFT(AppointmentDate, 10) BETWEEN @FromDate AND @ToDate ";

            if (selectedCategory != "All Categories")
            {
                transNumQuery += " AND ServiceCategory = @SelectedCategory";
            }

            transNumQuery += " GROUP BY TransactionNumber ORDER BY AppointmentDate DESC";

            MySqlCommand transNumCommand = new MySqlCommand(transNumQuery, connection);
            transNumCommand.Parameters.AddWithValue("@FromDate", fromDate);
            transNumCommand.Parameters.AddWithValue("@ToDate", toDate);

            if (selectedCategory != "All Categories")
            {
                transNumCommand.Parameters.AddWithValue("@SelectedCategory", selectedCategory);
            }

            int rowCount = 0;

            using (MySqlDataReader transNumReader = transNumCommand.ExecuteReader())
            {
                while (transNumReader.Read())
                {
                    string transactionNumber = transNumReader["TransactionNumber"].ToString();
                    string appointmentDate = transNumReader["AppointmentDate"].ToString();
                    decimal totalServicePrice = transNumReader.GetDecimal("TotalServicePrice");
                    string serviceCategory = selectedCategory == "All Categories" ? transNumReader["ServiceCategory"].ToString() : "";

                    if (selectedCategory == "All Categories")
                    {
                        dt.Rows.Add(transactionNumber, appointmentDate, totalServicePrice, serviceCategory);
                    }
                    else
                    {
                        dt.Rows.Add(transactionNumber, appointmentDate, totalServicePrice);
                    }
                }
                rowCount++;
            }

            int totalRows = dt.Rows.Count;
            int pageSize = 5;
            int totalBatches = (int)Math.Ceiling((double)totalRows / pageSize);
            int currentBatch = (rowCount - 1) / pageSize + 1;

            MngrAppSalesCurrentRecordLbl.Text = $"{1} of {totalBatches}";
            DataTable limitedDataTable = dt.AsEnumerable().Take(5).CopyToDataTable();
            MngrAppSalesTransRepDGV.RowTemplate.Height = 37;
            MngrAppSalesTransRepDGV.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            MngrAppSalesTransRepDGV.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            MngrAppSalesTransRepDGV.DataSource = limitedDataTable;
            MngrAppSalesTransRepDGVTwo.DataSource = dt;
            ApplyRowAlternatingColors(MngrAppSalesTransRepDGV);

            decimal totalServicePriceSum = 0;
            foreach (DataRow row in dt.Rows)
            {
                totalServicePriceSum += (decimal)row["TotalServicePrice"];
            }
            string formattedTotalServicePrice = "₱" + totalServicePriceSum.ToString("#,##0.00");
            MngrAppSalesTotalRevBox.Text = formattedTotalServicePrice;
        }

        private int currentPageThreeSales = 1;

        private void MngrAppSalesNextBtn_Click(object sender, EventArgs e)
        {
            ShowNextPageThree();
            ApplyRowAlternatingColors(MngrAppSalesTransRepDGV);
        }

        private void MngrAppSalesPreviousBtn_Click(object sender, EventArgs e)
        {
            ShowPreviousPageThree();
            ApplyRowAlternatingColors(MngrAppSalesTransRepDGV);
        }

        private void ShowNextPageThree()
        {
            int pageSize = 5;
            DataView filteredDataView = ((DataTable)MngrAppSalesTransRepDGVTwo.DataSource).DefaultView;
            if (!string.IsNullOrEmpty(filteredDataView.RowFilter))
            {
                int totalFilteredRows = filteredDataView.Count;
                int totalFilteredBatches = (int)Math.Ceiling((double)totalFilteredRows / pageSize);
                int currentFilteredBatch = currentPageThreeSales;

                if (currentFilteredBatch < totalFilteredBatches)
                {
                    int startIndex = currentFilteredBatch * pageSize;
                    int endIndex = Math.Min(startIndex + pageSize, totalFilteredRows);

                    MngrAppSalesTransRepDGV.DataSource = null;
                    DataTable pageDataTable = ((DataTable)MngrAppSalesTransRepDGVTwo.DataSource).Clone();

                    for (int i = startIndex; i < endIndex; i++)
                    {
                        DataRow newRow = filteredDataView[i].Row;
                        pageDataTable.ImportRow(newRow);
                    }
                    MngrAppSalesTransRepDGVTwo.DataSource = pageDataTable;
                    currentPageThreeSales++;
                    MngrAppSalesCurrentRecordLbl.Text = $"{currentPageThreeSales} of {totalFilteredBatches}";
                }
                else
                {
                    MessageBox.Show("No more pages to display.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                int totalRows = MngrAppSalesTransRepDGVTwo.Rows.Count;
                int currentBatch = currentPageThreeSales;
                int totalBatches = (int)Math.Ceiling((double)totalRows / pageSize);

                if (currentBatch < totalBatches)
                {
                    int startIndex = currentBatch * pageSize;
                    int endIndex = Math.Min(startIndex + pageSize, totalRows);

                    MngrAppSalesTransRepDGV.DataSource = null;
                    DataTable pageDataTable = ((DataTable)MngrAppSalesTransRepDGVTwo.DataSource).Clone();

                    for (int i = startIndex; i < endIndex; i++)
                    {
                        DataRow newRow = ((DataTable)MngrAppSalesTransRepDGVTwo.DataSource).Rows[i];
                        pageDataTable.ImportRow(newRow);
                    }

                    MngrAppSalesTransRepDGV.DataSource = pageDataTable;
                    currentPageThreeSales++;
                    MngrAppSalesCurrentRecordLbl.Text = $"{currentPageThreeSales} of {totalBatches}";
                }
                else
                {
                    MessageBox.Show("No more pages to display.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void ShowPreviousPageThree()
        {
            if (currentPageThreeSales > 1)
            {
                currentPageThreeSales--;

                int pageSize = 5;
                int startIndex = (currentPageThreeSales - 1) * pageSize;

                MngrAppSalesTransRepDGV.DataSource = null;

                DataTable pageDataTable = ((DataTable)MngrAppSalesTransRepDGVTwo.DataSource).Clone();
                DataView filteredDataView = ((DataTable)MngrAppSalesTransRepDGVTwo.DataSource).DefaultView;

                for (int i = startIndex; i < startIndex + pageSize; i++)
                {
                    if (i < filteredDataView.Count)
                    {
                        DataRow newRow = filteredDataView[i].Row;
                        pageDataTable.ImportRow(newRow);
                    }
                    else
                    {
                        break;
                    }
                }
                MngrAppSalesTransRepDGV.DataSource = pageDataTable;

                int totalFilteredRows = filteredDataView.Count;
                int totalFilteredBatches = (int)Math.Ceiling((double)totalFilteredRows / pageSize);

                MngrAppSalesCurrentRecordLbl.Text = $"{currentPageThreeSales} of {totalFilteredBatches}";
            }
            else
            {
                MessageBox.Show("Already on the first page.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void MngrAppSalesSearchTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchText = MngrAppSalesSearchTextBox.Text.Trim();

            DataView dv = ((DataTable)MngrAppSalesTransRepDGVTwo.DataSource).DefaultView;

            if (string.IsNullOrEmpty(searchText))
            {
                dv.RowFilter = string.Empty;
            }
            else
            {
                List<string> filterExpressions = new List<string>();
                foreach (DataColumn col in ((DataTable)MngrAppSalesTransRepDGVTwo.DataSource).Columns)
                {
                    if (col.DataType == typeof(decimal))
                    {
                        filterExpressions.Add($"CONVERT([{col.ColumnName}], 'System.String') LIKE '%{searchText}%'");
                    }
                    else
                    {
                        filterExpressions.Add($"[{col.ColumnName}] LIKE '%{searchText}%'");
                    }
                }

                string combinedFilterExpression = string.Join(" OR ", filterExpressions);

                dv.RowFilter = combinedFilterExpression;
            }
            int totalBatches = (int)Math.Ceiling((double)dv.Count / 5);

            if (totalBatches > 0)
            {
                MngrAppSalesCurrentRecordLbl.Text = $"1 of {totalBatches}";
            }
            else
            {
                MngrAppSalesCurrentRecordLbl.Text = "0 of 0";
                MngrAppSalesTransRepDGV.DataSource = null;
            }
            if (dv.Count == 0)
            {
                MessageBox.Show("No matching data found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                DataTable limitedDataTable = dv.ToTable().AsEnumerable().Take(5).CopyToDataTable();
                MngrAppSalesTransRepDGV.DataSource = limitedDataTable;
                ApplyRowAlternatingColors(MngrAppSalesTransRepDGV);
            }
        }

        private void MngrAppSalesPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MngrAppSalesPeriod != null && MngrAppSalesPeriod.SelectedItem != null)
            {
                MngrAppSalesSelectedPeriodText.Text = "";
                string selectedItem = MngrAppSalesPeriod.SelectedItem.ToString();

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
                        AND ServiceStatus = 'Completed'" + categoryFilter;

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

        #region Mngr. PANEL OF PRODUCT HISTORY
        private void ProductHistoryShow()
        {
            string connectionString = "Server=localhost;Database=enchante;Uid=root;Pwd=;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string queryCount = "SELECT COUNT(*) FROM orderproducthistory";
                    MySqlCommand commandCount = new MySqlCommand(queryCount, connection);
                    int totalRows = Convert.ToInt32(commandCount.ExecuteScalar());
                    string queryAllRows = "SELECT TransactionNumber, ProductStatus, CheckedOutDate, ClientName, ItemName, " +
                                    "ItemID, Qty, ItemPrice, ItemTotalPrice FROM orderproducthistory " +
                                    "ORDER BY CheckedOutDate DESC";
                    string queryFirstTenRows = "SELECT TransactionNumber, ProductStatus, CheckedOutDate, ClientName, ItemName, " +
                                    "ItemID, Qty, ItemPrice, ItemTotalPrice FROM orderproducthistory " +
                                    "ORDER BY CheckedOutDate DESC LIMIT 10";

                    System.Data.DataTable dataTableAll = new System.Data.DataTable();
                    MySqlDataAdapter adapterAll = new MySqlDataAdapter(queryAllRows, connection);
                    adapterAll.Fill(dataTableAll);
                    System.Data.DataTable dataTableFirstTen = new System.Data.DataTable();
                    MySqlDataAdapter adapterFirstTen = new MySqlDataAdapter(queryFirstTenRows, connection);
                    adapterFirstTen.Fill(dataTableFirstTen);

                    MngrPDHistoryDGV.DataSource = dataTableFirstTen;
                    MngrPDHistoryDGVTwo.DataSource = dataTableAll;
                    ApplyRowAlternatingColors(MngrPDHistoryDGV);
                    MngrPDHistoryDGV.RowTemplate.Height = 45;
                    MngrPDHistoryDGV.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    MngrPDHistoryDGV.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

                    int currentBatch = totalRows > 0 ? 1 : 0;
                    int totalBatches = (int)Math.Ceiling((double)totalRows / 10);

                    MngrPDCurrentRecordLbl.Text = $"{currentBatch} of {totalBatches}";

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private int currentBatchThree = 1;

        private void MngrPDNextBtn_Click(object sender, EventArgs e)
        {
            string searchText = MngrPDSearchTextBox.Text.Trim();
            int totalBatches = string.IsNullOrEmpty(MngrPDSearchTextBox.Text.Trim())
                        ? (int)Math.Ceiling((double)GetTotalRowsThree() / 10)
                        : (int)Math.Ceiling((double)GetFilteredTotalRowsThree() / 10);

            if ((MngrPDHistoryStatusBox.SelectedItem != null || considerDateFilter == true) && !string.IsNullOrEmpty(searchText))
            {
                FetchNextBatchTwo();
                ApplyRowAlternatingColors(MngrPDHistoryDGV);
            }
            else
            {
                if (currentBatchThree >= totalBatches)
                {
                    MessageBox.Show("No more data to show.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    string[] parts = MngrPDCurrentRecordLbl.Text.Split(' ');
                    if (parts.Length >= 2 && parts[0] == parts[2])
                    {
                        MessageBox.Show("No more data to show.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else
                    {
                        currentBatchThree++;

                        UpdateDataGridViewAndLabelThree();
                        ApplyCombinedFilter();
                        ApplyRowAlternatingColors(MngrPDHistoryDGV);
                    }
                }

            }
        }

        private void MngrPDPreviousBtn_Click(object sender, EventArgs e)
        {
            string searchText = MngrPDSearchTextBox.Text.Trim();

            if ((MngrPDHistoryStatusBox.SelectedItem != null || considerDateFilter == true) && !string.IsNullOrEmpty(searchText))
            {
                FetchPreviousBatchTwo();
                ApplyRowAlternatingColors(MngrPDHistoryDGV);
            }
            else
            {
                if (currentBatchThree <= 1)
                {
                    MessageBox.Show("No more previous data to show.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (MngrPDCurrentRecordLbl.Text.StartsWith("1 of "))
                {
                    MessageBox.Show("No more previous data to show.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    currentBatchThree--;

                    UpdateDataGridViewAndLabelThree();
                    ApplyCombinedFilter();
                    ApplyRowAlternatingColors(MngrPDHistoryDGV);
                }
            }
        }

        private void FetchNextBatchTwo()
        {
            int totalRows = ((DataTable)MngrPDHistoryDGVTwo.DataSource).Rows.Count;
            int totalBatches = (int)Math.Ceiling((double)totalRows / 10);
            int startIndex = currentBatchThree * 10;

            if (currentBatchThree < totalBatches)
            {
                DataTable filteredTable = ((DataTable)MngrPDHistoryDGVTwo.DataSource).Clone();
                for (int i = startIndex; i < Math.Min(startIndex + 10, totalRows); i++)
                {
                    DataRow newRow = ((DataTable)MngrPDHistoryDGVTwo.DataSource).Rows[i];
                    filteredTable.ImportRow(newRow);
                }
                MngrPDHistoryDGV.DataSource = filteredTable;

                currentBatchThree++;
                MngrPDCurrentRecordLbl.Text = $"{currentBatchThree} of {totalBatches}";
            }
            else
            {
                MessageBox.Show("No more pages to display.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void FetchPreviousBatchTwo()
        {
            int totalRows = ((DataTable)MngrPDHistoryDGVTwo.DataSource).Rows.Count;
            int totalBatches = (int)Math.Ceiling((double)totalRows / 10);
            int startIndex = (currentBatchThree - 2) * 10;

            if (currentBatchThree > 1)
            {
                DataTable filteredTable = ((DataTable)MngrPDHistoryDGVTwo.DataSource).Clone();
                for (int i = startIndex; i < Math.Min(startIndex + 10, totalRows); i++)
                {
                    DataRow newRow = ((DataTable)MngrPDHistoryDGVTwo.DataSource).Rows[i];
                    filteredTable.ImportRow(newRow);
                }
                MngrPDHistoryDGV.DataSource = filteredTable;

                currentBatchThree--;
                MngrPDCurrentRecordLbl.Text = $"{currentBatchThree} of {totalBatches}";
            }
            else
            {
                MessageBox.Show("No more previous data to show.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void UpdateDataGridViewAndLabelThree()
        {
            string connectionString = "Server=localhost;Database=enchante;Uid=root;Pwd=;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string countQuery = string.IsNullOrEmpty(MngrPDSearchTextBox.Text.Trim())
                                        ? "SELECT COUNT(*) FROM orderproducthistory"
                                        : $"SELECT COUNT(*) FROM orderproducthistory WHERE {GetFilterExpressionThree()}";

                    MySqlCommand countCommand = new MySqlCommand(countQuery, connection);
                    int totalRows = Convert.ToInt32(countCommand.ExecuteScalar());

                    if (totalRows == 0)
                    {
                        MessageBox.Show("No matching data found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        MngrPDCurrentRecordLbl.Text = "0 of 0";
                        return;
                    }

                    int totalBatches = (int)Math.Ceiling((double)totalRows / 10);

                    currentBatchThree = Math.Min(currentBatchThree, totalBatches);

                    string query = string.IsNullOrEmpty(MngrPDSearchTextBox.Text.Trim())
                                    ? GetRegularQueryThree()
                                    : GetFilteredQueryThree();

                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    System.Data.DataTable dataTable = new System.Data.DataTable();
                    adapter.Fill(dataTable);

                    MngrPDHistoryDGV.DataSource = dataTable;

                    MngrPDCurrentRecordLbl.Text = $"{currentBatchThree} of {totalBatches}";

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private string GetRegularQueryThree()
        {
            int startIndex = (currentBatchThree - 1) * 10;
            return $"SELECT TransactionNumber, ProductStatus, CheckedOutDate, ClientName, ItemName, " +
                    $"ItemID, Qty, ItemPrice, ItemTotalPrice FROM orderproducthistory " +
                    $"ORDER BY CheckedOutDate DESC LIMIT {startIndex}, 10";
        }

        private string GetFilteredQueryThree()
        {
            string filterExpression = GetFilterExpressionThree();
            int startIndex = (currentBatchThree - 1) * 10;
            return $"SELECT TransactionNumber, ProductStatus, CheckedOutDate, ClientName, ItemName, " +
                   $"ItemID, Qty, ItemPrice, ItemTotalPrice FROM orderproducthistory " +
                   $"{(string.IsNullOrEmpty(filterExpression) ? "" : $"WHERE {filterExpression} ")}" +
                   $"ORDER BY CheckedOutDate DESC LIMIT {startIndex}, 10";
        }

        private string GetFilterExpressionThree()
        {
            string searchText = MngrPDSearchTextBox.Text.Trim();
            return string.Join(" OR ", ((DataTable)MngrPDHistoryDGV.DataSource).Columns.Cast<DataColumn>()
                         .Select(col => $"{col.ColumnName} LIKE '%{searchText}%'"));
        }

        private int GetTotalRowsThree()
        {
            string connectionString = "Server=localhost;Database=enchante;Uid=root;Pwd=;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string countQuery = "SELECT COUNT(*) FROM orderproducthistory";
                    MySqlCommand countCommand = new MySqlCommand(countQuery, connection);
                    int totalRows = Convert.ToInt32(countCommand.ExecuteScalar());
                    connection.Close();
                    return totalRows;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return 0;
            }
        }

        private int GetFilteredTotalRowsThree()
        {
            string connectionString = "Server=localhost;Database=enchante;Uid=root;Pwd=;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = $"SELECT COUNT(*) FROM orderproducthistory WHERE {GetFilterExpressionThree()}";
                    MySqlCommand countCommand = new MySqlCommand(query, connection);
                    int totalRows = Convert.ToInt32(countCommand.ExecuteScalar());
                    connection.Close();
                    return totalRows;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return 0;
            }
        }

        private void MngrPDSearchTextBox_TextChanged(object sender, EventArgs e)
        {
            string query = "SELECT TransactionNumber, ProductStatus, CheckedOutDate, ClientName, ItemName, " +
                                    "ItemID, Qty, ItemPrice, ItemTotalPrice FROM orderproducthistory";
            string searchText = MngrPDSearchTextBox.Text.Trim();

            if ((MngrPDHistoryStatusBox.SelectedItem != null || MngrPDHistoryItemCatBox.SelectedItem != null || considerDateFilter == true)
                && !string.IsNullOrEmpty(searchText))
            {
                string combinedFilter = GetCombinedFilter();
                string searchFilter = GetSearchFilterThree();

                string filterExpression = searchFilter;

                if (!string.IsNullOrEmpty(combinedFilter))
                {
                    DataView dv = ((DataTable)MngrPDHistoryDGVTwo.DataSource).DefaultView;
                    filterExpression = string.Join(" OR ", ((DataTable)MngrPDHistoryDGVTwo.DataSource).Columns.Cast<DataColumn>()
                                                .Select(col => $"{col.ColumnName} LIKE '%{searchText}%'"));
                    dv.RowFilter = filterExpression;
                    filterExpression = string.IsNullOrEmpty(searchFilter) ? combinedFilter : $"({combinedFilter}) AND ({searchFilter})";
                }

                DataView dataView = new DataView(((DataTable)MngrPDHistoryDGVTwo.DataSource), filterExpression, "", DataViewRowState.CurrentRows);
                MngrPDHistoryDGVTwo.DataSource = dataView.ToTable();
                ApplyRowAlternatingColors(MngrPDHistoryDGV);

                if (!string.IsNullOrEmpty(searchText))
                {
                    int totalRows = dataView.Count;
                    int pageNumber = (int)Math.Ceiling((double)totalRows / 10);
                    MngrPDCurrentRecordLbl.Text = $"1 of {pageNumber}";
                }
                DataTable filteredTable = dataView.ToTable();
                DataTable limitedRowsTable = filteredTable.Clone();
                int rowsToShow = Math.Min(filteredTable.Rows.Count, 10);
                for (int i = 0; i < rowsToShow; i++)
                {
                    limitedRowsTable.ImportRow(filteredTable.Rows[i]);
                }
                MngrPDHistoryDGV.DataSource = limitedRowsTable;

                if (dataView.Count == 0)
                {
                    MessageBox.Show("No matching data found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MngrPDSearchTextBox.Text = "";
                    GetCombinedFilter();
                }
            }
            else if (!string.IsNullOrEmpty(searchText))
            {
                DataView dv = ((DataTable)MngrPDHistoryDGV.DataSource).DefaultView;

                if (string.IsNullOrEmpty(searchText))
                {
                    dv.RowFilter = string.Empty;
                }
                else
                {
                    string filterExpression = string.Join(" OR ", ((DataTable)MngrPDHistoryDGV.DataSource).Columns.Cast<DataColumn>()
                                                    .Select(col => $"{col.ColumnName} LIKE '{searchText}%'"));
                    dv.RowFilter = filterExpression;
                }

                UpdateDataGridViewAndLabelThree();
                ApplyRowAlternatingColors(MngrPDHistoryDGV);
            }
            else if ((MngrPDHistoryStatusBox.SelectedItem != null || considerDateFilter == true))
            {
                ApplyCombinedFilter();
                ApplyRowAlternatingColors(MngrPDHistoryDGV);
            }
            else
            {
                ProductHistoryShow();
                ApplyRowAlternatingColors(MngrPDHistoryDGV);
            }
        }

        private bool considerDateFilter = false;

        private string GetCombinedFilter()
        {
            string statusFilter = GetStatusFilter();
            string categoryFilter = GetCategoryFilter();
            string dateFilter = considerDateFilter ? GetDateFilter() : string.Empty;
            string searchFilter = GetSearchFilterThree();

            List<string> filters = new List<string>();

            if (!string.IsNullOrEmpty(statusFilter))
                filters.Add(statusFilter);

            if (!string.IsNullOrEmpty(categoryFilter))
                filters.Add(categoryFilter);

            if (!string.IsNullOrEmpty(dateFilter))
                filters.Add(dateFilter);

            if (!string.IsNullOrEmpty(searchFilter))
                filters.Add(searchFilter);

            if (filters.Count == 1)
                return filters[0];

            if (filters.Count > 0)
            {
                string combinedFilter = string.Join(" AND ", filters.Select(filter => $"({filter})"));
                return combinedFilter;
            }
            else
            {
                return string.Empty;
            }
        }

        private string GetSearchFilterThree()
        {
            string searchText = MngrPDSearchTextBox.Text.Trim();
            if (string.IsNullOrEmpty(searchText))
                return null;

            string filterExpression = string.Join(" OR ", ((DataTable)MngrPDHistoryDGV.DataSource).Columns.Cast<DataColumn>()
                                                .Select(col => $"{col.ColumnName} LIKE '%{searchText}%'"));

            return filterExpression;
        }

        private string GetStatusFilter()
        {
            if (MngrPDHistoryStatusBox.SelectedItem != null)
            {
                string selectedStatus = MngrPDHistoryStatusBox.SelectedItem.ToString();
                if (selectedStatus == "Paid")
                {
                    return "ProductStatus = 'Paid'";
                }
                else if (selectedStatus == "Not Paid")
                {
                    return "ProductStatus = 'Not Paid'";
                }
            }
            return string.Empty;
        }

        private string GetCategoryFilter()
        {
            if (MngrPDHistoryItemCatBox.SelectedItem != null)
            {
                string selectedCategory = MngrPDHistoryItemCatBox.SelectedItem.ToString();
                switch (selectedCategory)
                {
                    case "Hair Styling":
                        return "SUBSTRING(ItemID, 1, 2) = 'HS'";
                    case "Face & Skin":
                        return "SUBSTRING(ItemID, 1, 2) = 'FS'";
                    case "Nail Care":
                        return "SUBSTRING(ItemID, 1, 2) = 'NC'";
                    case "Massage":
                        return "SUBSTRING(ItemID, 1, 2) = 'MS'";
                    case "Spa":
                        return "SUBSTRING(ItemID, 1, 2) = 'SP'";
                }
            }
            return string.Empty;
        }

        private string GetDateFilter()
        {
            DateTime fromDate = MngrPDHistoryDatePickFrom.Value.Date;
            DateTime toDate = MngrPDHistoryDatePickTo.Value.Date;

            if (fromDate > toDate && fromDate != DateTime.Now.Date && toDate != DateTime.Now.Date)
            {
                MessageBox.Show("From date should not be ahead of To date.", "Invalid Date Range", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            toDate = toDate.AddDays(1);

            string fromDateString = fromDate.ToString("MM-dd-yyyy dddd");
            string toDateString = toDate.ToString("MM-dd-yyyy dddd");

            if (fromDate == toDate)
            {
                return $"CheckedOutDate LIKE '%{fromDateString}%'";
            }
            else
            {
                return $"CheckedOutDate >= '{fromDateString}' AND CheckedOutDate < '{toDateString}'";
            }
        }

        private void ApplyCombinedFilter()
        {
            string combinedFilter = GetCombinedFilter();

            string query = "SELECT TransactionNumber, ProductStatus, CheckedOutDate, ClientName, ItemName, " +
                                    "ItemID, Qty, ItemPrice, ItemTotalPrice FROM orderproducthistory";

            if (!string.IsNullOrEmpty(combinedFilter))
            {
                query += $" WHERE {combinedFilter}";
            }

            query += " ORDER BY CheckedOutDate DESC";

            FetchDataFromDatabaseThree(query, combinedFilter);
        }

        private void FetchDataFromDatabaseThree(string query, string combinedFilter)
        {
            string connectionString = "Server=localhost;Database=enchante;Uid=root;Pwd=;";
            string searchText = MngrPDSearchTextBox.Text.Trim();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    DataTable dataTable = new DataTable();
                    DataTable batchDataTable = new DataTable();
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    adapter.Fill(dataTable);

                    int totalRows = dataTable.Rows.Count;
                    int totalBatches = (int)Math.Ceiling((double)totalRows / 10);
                    int startIndex = (currentBatchThree - 1) * 10;

                    foreach (DataColumn column in dataTable.Columns)
                    {
                        batchDataTable.Columns.Add(column.ColumnName);
                    }

                    for (int i = startIndex; i < Math.Min(startIndex + 10, totalRows); i++)
                    {
                        batchDataTable.ImportRow(dataTable.Rows[i]);
                    }

                    MngrPDHistoryDGV.DataSource = batchDataTable;
                    MngrPDHistoryDGVTwo.DataSource = dataTable;

                    if (string.IsNullOrEmpty(searchText))
                    {
                        MngrPDCurrentRecordLbl.Text = $"{currentBatchThree} of {totalBatches}";
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void MngrPDHistoryStatusBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyCombinedFilter();
        }

        private void MngrPDHistoryItemCatBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyCombinedFilter();
        }

        private void MngrPDHistoryDatePickFrom_ValueChanged(object sender, EventArgs e)
        {
            considerDateFilter = true;
            ApplyCombinedFilter();
        }

        private void MngrPDHistoryDatePickTo_ValueChanged(object sender, EventArgs e)
        {
            considerDateFilter = true;
            ApplyCombinedFilter();
        }

        private void MngrPDHistoryResetBtn_Click(object sender, EventArgs e)
        {
            MngrPDHistoryStatusBox.SelectedIndex = -1;
            MngrPDHistoryItemCatBox.SelectedIndex = -1;
            MngrPDHistoryDatePickFrom.Value = DateTime.Now;
            MngrPDHistoryDatePickTo.Value = DateTime.Now;
            considerDateFilter = false;
            MngrPDSearchTextBox.Text = "";
            currentBatchThree = 1;
            UpdateDataGridViewAndLabelThree();
        }
        #endregion

        #region Mngr. PANEL OF SERVICE HISTORY
        private void ServiceHistoryShow()
        {
            string connectionString = "Server=localhost;Database=enchante;Uid=root;Pwd=;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string queryCount = "SELECT COUNT(*) FROM servicehistory";
                    MySqlCommand commandCount = new MySqlCommand(queryCount, connection);
                    int totalRows = Convert.ToInt32(commandCount.ExecuteScalar());

                    string queryAllRows = "SELECT TransactionNumber, TransactionType, ServiceStatus, AppointmentDate, ClientName, " +
                                    "ServiceCategory, AttendingStaff, ServiceID, SelectedService, ServicePrice FROM servicehistory " +
                                    "ORDER BY AppointmentDate DESC";

                    string queryFirstTenRows = "SELECT TransactionNumber, TransactionType, ServiceStatus, AppointmentDate, ClientName, " +
                                    "ServiceCategory, AttendingStaff, ServiceID, SelectedService, ServicePrice FROM servicehistory " +
                                    "ORDER BY AppointmentDate DESC LIMIT 10";

                    System.Data.DataTable dataTableAll = new System.Data.DataTable();
                    MySqlDataAdapter adapterAll = new MySqlDataAdapter(queryAllRows, connection);
                    adapterAll.Fill(dataTableAll);
                    System.Data.DataTable dataTableFirstTen = new System.Data.DataTable();
                    MySqlDataAdapter adapterFirstTen = new MySqlDataAdapter(queryFirstTenRows, connection);
                    adapterFirstTen.Fill(dataTableFirstTen);
                    MngrSVHistoryDGV.DataSource = dataTableFirstTen;
                    MngrSVHistoryDGVTwo.DataSource = dataTableAll;
                    ApplyRowAlternatingColors(MngrSVHistoryDGV);
                    MngrSVHistoryDGV.RowTemplate.Height = 45;
                    MngrSVHistoryDGV.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    MngrSVHistoryDGV.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

                    int currentBatch = totalRows > 0 ? 1 : 0;
                    int totalBatches = (int)Math.Ceiling((double)totalRows / 10);

                    MngrSVHistoryCurrentRecordLbl.Text = $"{currentBatch} of {totalBatches}";

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private int currentBatchFour = 1;

        private void MngrSVHistoryNextBtn_Click(object sender, EventArgs e)
        {
            string searchText = MngrSVHistorySearchTextBox.Text.Trim();
            int totalBatches = string.IsNullOrEmpty(MngrSVHistorySearchTextBox.Text.Trim())
                            ? (int)Math.Ceiling((double)GetTotalRowsFour() / 10)
                            : (int)Math.Ceiling((double)GetFilteredTotalRowsFour() / 10);

            if ((MngrSVHistoryTransTypeBox.SelectedItem != null || MngrSVHistoryServiceStatusBox.SelectedItem != null ||
                MngrSVHistoryServiceCatBox.SelectedItem != null || ConsiderDateFilter == true) && !string.IsNullOrEmpty(searchText))
            {
                FetchNextBatch();
                ApplyRowAlternatingColors(MngrSVHistoryDGV);
            }
            else
            {
                if (currentBatchFour >= totalBatches)
                {
                    MessageBox.Show("No more data to show.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    string[] parts = MngrSVHistoryCurrentRecordLbl.Text.Split(' ');
                    if (parts.Length >= 2 && parts[0] == parts[2])
                    {
                        MessageBox.Show("No more data to show.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else
                    {
                        currentBatchFour++;

                        UpdateDataGridViewAndLabelFour();
                        Apply_CombinedFilter();
                        ApplyRowAlternatingColors(MngrSVHistoryDGV);
                    }
                }
            }
        }

        private void MngrSVHistoryPreviousBtn_Click(object sender, EventArgs e)
        {
            string searchText = MngrSVHistorySearchTextBox.Text.Trim();
            if ((MngrSVHistoryTransTypeBox.SelectedItem != null || MngrSVHistoryServiceStatusBox.SelectedItem != null ||
                MngrSVHistoryServiceCatBox.SelectedItem != null || ConsiderDateFilter == true) && !string.IsNullOrEmpty(searchText))
            {
                FetchPreviousBatch();
                ApplyRowAlternatingColors(MngrSVHistoryDGV);
            }
            else
            {
                if (currentBatchFour <= 1)
                {
                    MessageBox.Show("No more previous data to show.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (MngrSVHistoryCurrentRecordLbl.Text.StartsWith("1 of "))
                {
                    MessageBox.Show("No more previous data to show.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    currentBatchFour--;

                    UpdateDataGridViewAndLabelFour();
                    Apply_CombinedFilter();
                    ApplyRowAlternatingColors(MngrSVHistoryDGV);
                }
            }
        }

        private void UpdateDataGridViewAndLabelFour()
        {
            string connectionString = "Server=localhost;Database=enchante;Uid=root;Pwd=;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string countQuery = string.IsNullOrEmpty(MngrSVHistorySearchTextBox.Text.Trim())
                                        ? "SELECT COUNT(*) FROM servicehistory"
                                        : $"SELECT COUNT(*) FROM servicehistory WHERE {GetFilterExpressionFour()}";

                    MySqlCommand countCommand = new MySqlCommand(countQuery, connection);
                    int totalRows = Convert.ToInt32(countCommand.ExecuteScalar());

                    if (totalRows == 0)
                    {
                        MessageBox.Show("No matching data found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        MngrSVHistoryCurrentRecordLbl.Text = "0 of 0";
                        return;
                    }

                    int totalBatches = (int)Math.Ceiling((double)totalRows / 10);

                    currentBatchFour = Math.Min(currentBatchFour, totalBatches);

                    string query = string.IsNullOrEmpty(MngrSVHistorySearchTextBox.Text.Trim())
                                    ? GetRegularQueryFour()
                                    : GetFilteredQueryFour();

                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    System.Data.DataTable dataTable = new System.Data.DataTable();
                    adapter.Fill(dataTable);

                    MngrSVHistoryDGV.DataSource = dataTable;

                    MngrSVHistoryCurrentRecordLbl.Text = $"{currentBatchFour} of {totalBatches}";

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void FetchNextBatch()
        {
            int totalRows = ((DataTable)MngrSVHistoryDGVTwo.DataSource).Rows.Count;
            int totalBatches = (int)Math.Ceiling((double)totalRows / 10);
            int startIndex = currentBatchFour * 10;

            if (currentBatchFour < totalBatches)
            {
                DataTable filteredTable = ((DataTable)MngrSVHistoryDGVTwo.DataSource).Clone();
                for (int i = startIndex; i < Math.Min(startIndex + 10, totalRows); i++)
                {
                    DataRow newRow = ((DataTable)MngrSVHistoryDGVTwo.DataSource).Rows[i];
                    filteredTable.ImportRow(newRow);
                }
                MngrSVHistoryDGV.DataSource = filteredTable;

                currentBatchFour++;
                MngrSVHistoryCurrentRecordLbl.Text = $"{currentBatchFour} of {totalBatches}";
            }
            else
            {
                MessageBox.Show("No more pages to display.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void FetchPreviousBatch()
        {
            int totalRows = ((DataTable)MngrSVHistoryDGVTwo.DataSource).Rows.Count;
            int totalBatches = (int)Math.Ceiling((double)totalRows / 10);
            int startIndex = (currentBatchFour - 2) * 10;

            if (currentBatchFour > 1)
            {
                DataTable filteredTable = ((DataTable)MngrSVHistoryDGVTwo.DataSource).Clone();
                for (int i = startIndex; i < Math.Min(startIndex + 10, totalRows); i++)
                {
                    DataRow newRow = ((DataTable)MngrSVHistoryDGVTwo.DataSource).Rows[i];
                    filteredTable.ImportRow(newRow);
                }
                MngrSVHistoryDGV.DataSource = filteredTable;

                currentBatchFour--;
                MngrSVHistoryCurrentRecordLbl.Text = $"{currentBatchFour} of {totalBatches}";
            }
            else
            {
                MessageBox.Show("No more previous data to show.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private string GetRegularQueryFour()
        {
            int startIndex = (currentBatchFour - 1) * 10;
            return $"SELECT TransactionNumber, TransactionType, ServiceStatus, AppointmentDate, ClientName, " +
                   $"ServiceCategory, AttendingStaff, ServiceID, SelectedService, ServicePrice FROM servicehistory " +
                   $"ORDER BY AppointmentDate DESC LIMIT {startIndex}, 10";
        }

        private string GetFilteredQueryFour()
        {
            string filterExpression = GetFilterExpressionFour();
            int startIndex = (currentBatchFour - 1) * 10;
            return $"SELECT TransactionNumber, TransactionType, ServiceStatus, AppointmentDate, ClientName, " +
                   $"ServiceCategory, AttendingStaff, ServiceID, SelectedService, ServicePrice FROM servicehistory " +
                   $"{(string.IsNullOrEmpty(filterExpression) ? "" : $"WHERE {filterExpression} ")}" +
                   $"ORDER BY AppointmentDate DESC LIMIT {startIndex}, 10";
        }

        private string GetFilterExpressionFour()
        {
            string searchText = MngrSVHistorySearchTextBox.Text.Trim();
            return string.Join(" OR ", ((DataTable)MngrSVHistoryDGV.DataSource).Columns.Cast<DataColumn>()
                            .Select(col => $"{col.ColumnName} LIKE '%{searchText}%'"));
        }

        private int GetTotalRowsFour()
        {
            string connectionString = "Server=localhost;Database=enchante;Uid=root;Pwd=;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string countQuery = "SELECT COUNT(*) FROM servicehistory";
                    MySqlCommand countCommand = new MySqlCommand(countQuery, connection);
                    int totalRows = Convert.ToInt32(countCommand.ExecuteScalar());
                    connection.Close();
                    return totalRows;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return 0;
            }
        }

        private int GetFilteredTotalRowsFour()
        {
            string connectionString = "Server=localhost;Database=enchante;Uid=root;Pwd=;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = $"SELECT COUNT(*) FROM servicehistory WHERE {GetFilterExpressionFour()}";
                    MySqlCommand countCommand = new MySqlCommand(query, connection);
                    int totalRows = Convert.ToInt32(countCommand.ExecuteScalar());
                    connection.Close();
                    return totalRows;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return 0;
            }
        }

        private void MngrSVHistorySearchTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchText = MngrSVHistorySearchTextBox.Text.Trim();

            if ((MngrSVHistoryTransTypeBox.SelectedItem != null || MngrSVHistoryServiceStatusBox.SelectedItem != null ||
                 MngrSVHistoryServiceCatBox.SelectedItem != null || ConsiderDateFilter == true) && !string.IsNullOrEmpty(searchText))
            {
                string combinedFilter = GetCombined_Filter();
                string searchFilter = GetSearchFilterFour();

                string filterExpression = searchFilter;

                if (!string.IsNullOrEmpty(combinedFilter))
                {
                    DataView dv = ((DataTable)MngrSVHistoryDGVTwo.DataSource).DefaultView;
                    filterExpression = string.Join(" OR ", ((DataTable)MngrSVHistoryDGVTwo.DataSource).Columns.Cast<DataColumn>()
                                                .Select(col => $"{col.ColumnName} LIKE '%{searchText}%'"));
                    dv.RowFilter = filterExpression;
                    filterExpression = string.IsNullOrEmpty(searchFilter) ? combinedFilter : $"({combinedFilter}) AND ({searchFilter})";
                }

                DataView dataView = new DataView(((DataTable)MngrSVHistoryDGVTwo.DataSource), filterExpression, "", DataViewRowState.CurrentRows);
                MngrSVHistoryDGVTwo.DataSource = dataView.ToTable();
                ApplyRowAlternatingColors(MngrSVHistoryDGV);

                if (!string.IsNullOrEmpty(searchText))
                {
                    int totalRows = dataView.Count;
                    int pageNumber = (int)Math.Ceiling((double)totalRows / 10);
                    MngrSVHistoryCurrentRecordLbl.Text = $"1 of {pageNumber}";
                }
                DataTable filteredTable = dataView.ToTable();
                DataTable limitedRowsTable = filteredTable.Clone();
                int rowsToShow = Math.Min(filteredTable.Rows.Count, 10);
                for (int i = 0; i < rowsToShow; i++)
                {
                    limitedRowsTable.ImportRow(filteredTable.Rows[i]);
                }
                MngrSVHistoryDGV.DataSource = limitedRowsTable;

                if (dataView.Count == 0)
                {
                    MessageBox.Show("No matching data found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MngrSVHistorySearchTextBox.Text = "";
                    GetCombined_Filter();
                }
            }
            else if (!string.IsNullOrEmpty(searchText))
            {
                DataView dv = ((DataTable)MngrSVHistoryDGV.DataSource).DefaultView;

                if (string.IsNullOrEmpty(searchText))
                {
                    dv.RowFilter = string.Empty;
                }
                else
                {
                    string filterExpression = string.Join(" OR ", ((DataTable)MngrSVHistoryDGV.DataSource).Columns.Cast<DataColumn>()
                                                    .Select(col => $"{col.ColumnName} LIKE '{searchText}%'"));
                    dv.RowFilter = filterExpression;
                }

                UpdateDataGridViewAndLabelFour();
                ApplyRowAlternatingColors(MngrSVHistoryDGV);
            }
            else if (MngrSVHistoryTransTypeBox.SelectedItem != null || MngrSVHistoryServiceStatusBox.SelectedItem != null ||
                    MngrSVHistoryServiceCatBox.SelectedItem != null || ConsiderDateFilter == true)
            {
                Apply_CombinedFilter();
                ApplyRowAlternatingColors(MngrSVHistoryDGV);
            }
            else
            {
                ServiceHistoryShow();
                ApplyRowAlternatingColors(MngrSVHistoryDGV);
            }
        }

        private bool ConsiderDateFilter = false;

        private string GetCombined_Filter()
        {
            string transactionTypeFilter = GetTransactionTypeFilter();
            string serviceStatusFilter = GetServiceStatusFilter();
            string serviceCategoryFilter = GetServiceCategoryFilter();
            string dateFilter = ConsiderDateFilter ? FilterRowByDateRange() : string.Empty;
            string searchFilter = GetSearchFilterFour();

            List<string> filters = new List<string>();

            if (!string.IsNullOrEmpty(transactionTypeFilter))
                filters.Add(transactionTypeFilter);

            if (!string.IsNullOrEmpty(serviceStatusFilter))
                filters.Add(serviceStatusFilter);

            if (!string.IsNullOrEmpty(serviceCategoryFilter))
                filters.Add(serviceCategoryFilter);

            if (!string.IsNullOrEmpty(dateFilter))
                filters.Add(dateFilter);

            if (!string.IsNullOrEmpty(searchFilter))
                filters.Add(searchFilter);

            if (filters.Count == 1)
                return filters[0];

            if (filters.Count > 0)
            {
                string combinedFilter = string.Join(" AND ", filters.Select(filter => $"({filter})"));
                return combinedFilter;
            }
            else
            {
                return string.Empty;
            }
        }

        private string GetSearchFilterFour()
        {
            string searchText = MngrSVHistorySearchTextBox.Text.Trim();
            if (string.IsNullOrEmpty(searchText))
                return null;

            string filterExpression = string.Join(" OR ", ((DataTable)MngrSVHistoryDGV.DataSource).Columns.Cast<DataColumn>()
                                                .Select(col => $"{col.ColumnName} LIKE '%{searchText}%'"));

            return filterExpression;
        }

        private string GetTransactionTypeFilter()
        {
            if (MngrSVHistoryTransTypeBox.SelectedItem != null)
            {
                string selectedTransactionType = MngrSVHistoryTransTypeBox.SelectedItem.ToString();
                return $"TransactionType = '{selectedTransactionType}'";
            }
            return string.Empty;
        }

        private string GetServiceStatusFilter()
        {
            if (MngrSVHistoryServiceStatusBox.SelectedItem != null)
            {
                string selectedServiceStatus = MngrSVHistoryServiceStatusBox.SelectedItem.ToString();
                return $"ServiceStatus = '{selectedServiceStatus}'";
            }
            return string.Empty;
        }

        private string GetServiceCategoryFilter()
        {
            if (MngrSVHistoryServiceCatBox.SelectedItem != null)
            {
                string selectedServiceCategory = MngrSVHistoryServiceCatBox.SelectedItem.ToString();
                return $"ServiceCategory = '{selectedServiceCategory}'";
            }
            return string.Empty;
        }

        private void Apply_CombinedFilter()
        {
            string combinedFilter = GetCombined_Filter();

            string query = "SELECT TransactionNumber, TransactionType, ServiceStatus, AppointmentDate, ClientName, " +
                           "ServiceCategory, AttendingStaff, ServiceID, SelectedService, ServicePrice FROM servicehistory";

            if (!string.IsNullOrEmpty(combinedFilter))
            {
                query += $" WHERE {combinedFilter}";
            }

            query += " ORDER BY AppointmentDate DESC";

            FetchDataFromDatabaseFour(query, combinedFilter);
        }

        private void FetchDataFromDatabaseFour(string query, string combinedFilter)
        {
            string connectionString = "Server=localhost;Database=enchante;Uid=root;Pwd=;";
            string searchText = MngrSVHistorySearchTextBox.Text.Trim();
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    DataTable dataTable = new DataTable();
                    DataTable batchDataTable = new DataTable();
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    adapter.Fill(dataTable);

                    int totalRows = dataTable.Rows.Count;
                    int totalBatches = (int)Math.Ceiling((double)totalRows / 10);
                    int startIndex = (currentBatchFour - 1) * 10;

                    foreach (DataColumn column in dataTable.Columns)
                    {
                        batchDataTable.Columns.Add(column.ColumnName);
                    }

                    for (int i = startIndex; i < Math.Min(startIndex + 10, totalRows); i++)
                    {
                        batchDataTable.ImportRow(dataTable.Rows[i]);
                    }

                    MngrSVHistoryDGV.DataSource = batchDataTable;
                    MngrSVHistoryDGVTwo.DataSource = dataTable;

                    if (string.IsNullOrEmpty(searchText))
                    {
                        MngrSVHistoryCurrentRecordLbl.Text = $"{currentBatchFour} of {totalBatches}";
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private string FilterRowByDateRange()
        {
            DateTime fromDate = MngrSVHistoryDatePickFrom.Value.Date;
            DateTime toDate = MngrSVHistoryDatePickTo.Value.Date;

            if (fromDate > toDate && fromDate != DateTime.Now.Date && toDate != DateTime.Now.Date)
            {
                MessageBox.Show("From date should not be ahead of To date.", "Invalid Date Range", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            toDate = toDate.AddDays(1);

            string fromDateString = fromDate.ToString("MM-dd-yyyy dddd");
            string toDateString = toDate.ToString("MM-dd-yyyy dddd");
            if (fromDate == toDate)
            {
                return $"AppointmentDate LIKE '%{fromDateString}%'";
            }
            else
            {
                return $"AppointmentDate >= '{fromDateString}' AND AppointmentDate < '{toDateString}'";
            }
        }

        private void MngrSVHistoryTransTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Apply_CombinedFilter();
        }

        private void MngrSVHistoryServiceStatusBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Apply_CombinedFilter();
        }

        private void MngrSVHistoryServiceCatBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Apply_CombinedFilter();
        }

        private void MngrSVHistoryDatePickFrom_ValueChanged(object sender, EventArgs e)
        {
            ConsiderDateFilter = true;
            Apply_CombinedFilter();
        }

        private void MngrSVHistoryDatePickTo_ValueChanged(object sender, EventArgs e)
        {
            ConsiderDateFilter = true;
            Apply_CombinedFilter();
        }

        private void MngrSVHistoryResetBtn_Click(object sender, EventArgs e)
        {
            MngrSVHistoryTransTypeBox.SelectedIndex = -1;
            MngrSVHistoryServiceStatusBox.SelectedIndex = -1;
            MngrSVHistoryServiceCatBox.SelectedIndex = -1;
            MngrSVHistoryDatePickFrom.Value = DateTime.Now;
            MngrSVHistoryDatePickTo.Value = DateTime.Now;
            ConsiderDateFilter = false;
            MngrSVHistorySearchTextBox.Text = "";
            currentBatchFour = 1;
            UpdateDataGridViewAndLabelFour();
        }
        #endregion

        #region Mngr. PANEL OF VOUCHERS
        private void VouchersShow()
        {
            string connectionString = "Server=localhost;Database=enchante;Uid=root;Pwd=;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string countQuery = "SELECT COUNT(*) FROM voucher";
                    MySqlCommand countCommand = new MySqlCommand(countQuery, connection);
                    int totalRows = Convert.ToInt32(countCommand.ExecuteScalar());

                    string query = "SELECT DateStart, DateEnd, PromoName, PromoCategory, PromoCode, PromoDiscount, " +
                                    "AvailableNumber, PromoCreated FROM voucher " +
                                    "ORDER BY PromoCreated DESC " +
                                    "LIMIT 10";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    System.Data.DataTable dataTable = new System.Data.DataTable();
                    adapter.Fill(dataTable);

                    if (!dataTable.Columns.Contains("Status"))
                    {
                        dataTable.Columns.Add("Status", typeof(string));
                    }

                    foreach (DataRow row in dataTable.Rows)
                    {
                        string promoCode = row["PromoCode"].ToString();
                        DateTime dateStart = Convert.ToDateTime(row["DateStart"]);
                        DateTime dateEnd = Convert.ToDateTime(row["DateEnd"]);
                        int availableNumber = Convert.ToInt32(row["AvailableNumber"]);

                        DateTime currentDate = DateTime.Now;
                        string status = "";

                        if (availableNumber == 0)
                        {
                            status = "Fully Claimed";
                        }
                        else if (currentDate > dateEnd)
                        {
                            status = "Expired";
                        }
                        else if (currentDate >= dateStart && currentDate <= dateEnd)
                        {
                            status = "Ongoing";
                        }
                        else
                        {
                            status = "Pending";
                        }

                        string updateStatusQuery = "UPDATE voucher SET Status = @Status WHERE PromoCode = @PromoCode";
                        MySqlCommand updateStatusCommand = new MySqlCommand(updateStatusQuery, connection);
                        updateStatusCommand.Parameters.AddWithValue("@Status", status);
                        updateStatusCommand.Parameters.AddWithValue("@PromoCode", promoCode);
                        updateStatusCommand.ExecuteNonQuery();

                        row["Status"] = status;
                    }

                    MngrVoucherDGV.DataSource = dataTable;
                    ApplyRowAlternatingColors(MngrVoucherDGV);
                    MngrVoucherDGV.RowTemplate.Height = 41; // Adjust the row height
                    MngrVoucherDGV.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                    MngrVoucherDGV.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

                    int currentBatch = totalRows > 0 ? 1 : 0;
                    int totalBatches = (int)Math.Ceiling((double)totalRows / 10);

                    MngrVoucherCurrentRecordLbl.Text = $"{currentBatch} of {totalBatches}";

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void MngrVoucherInsertBtn_Click(object sender, EventArgs e)
        {
            DateTime dateStart = MngrVoucherDatePickerStart.Value;
            DateTime dateEnd = MngrVoucherDatePickerEnd.Value;
            string promoName = MngrVoucherPromoNameTextBox.Text;
            string promoCode = MngrVoucherPromoCodeTextBox.Text;
            string promoDiscount = MngrVoucherPromoDiscTextBox.Text;
            string promoCategory = MngrVoucherSelectCatTextBox.Text;
            string availableNumber = MngrVoucherAvailNumTextBox.Text;
            DateTime promoCreated = DateTime.Now;

            if (string.IsNullOrWhiteSpace(promoName) || string.IsNullOrWhiteSpace(promoDiscount)
                || string.IsNullOrWhiteSpace(availableNumber) || string.IsNullOrWhiteSpace(promoCategory))
            {
                MessageBox.Show("Please fill in all required fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (dateStart > dateEnd && dateStart.Date != dateEnd.Date)
            {
                MessageBox.Show("Start date cannot be ahead of end date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!promoDiscount.EndsWith("%"))
            {
                MessageBox.Show("Promo discount must end with '%'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(availableNumber, out _))
            {
                MessageBox.Show("Please enter a valid number for Available Number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string formattedDateStart = dateStart.ToString("MMMM d, yyyy");
            string formattedDateEnd = dateEnd.ToString("MMMM d, yyyy");
            string formattedPromoCreated = promoCreated.ToString("MMMM d, yyyy HH:mm:ss");

            string connectionString = "Server=localhost;Database=enchante;Uid=root;Pwd=;";
            string query = "INSERT INTO voucher (DateStart, DateEnd, PromoName, PromoCategory, PromoCode, PromoDiscount, AvailableNumber, PromoCreated) " +
                           "VALUES (@DateStart, @DateEnd, @PromoName, @PromoCategory, @PromoCode, @PromoDiscount, @AvailableNumber, @PromoCreated)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@DateStart", formattedDateStart);
                    command.Parameters.AddWithValue("@DateEnd", formattedDateEnd);
                    command.Parameters.AddWithValue("@PromoName", promoName);
                    command.Parameters.AddWithValue("@PromoCategory", promoCategory);
                    command.Parameters.AddWithValue("@PromoCode", promoCode);
                    command.Parameters.AddWithValue("@PromoDiscount", promoDiscount);
                    command.Parameters.AddWithValue("@AvailableNumber", availableNumber);
                    command.Parameters.AddWithValue("@PromoCreated", formattedPromoCreated);

                    command.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Voucher inserted successfully!", "Information");
            VouchersShow();
            ClearFields();
            PromoCodeGenerator();
        }

        private void PromoCodeGenerator()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder codeBuilder = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < 10; i++)
            {
                codeBuilder.Append(chars[random.Next(chars.Length)]);
            }

            MngrVoucherPromoCodeTextBox.Text = codeBuilder.ToString();
        }

        private void ClearFields()
        {
            MngrVoucherDatePickerStart.Value = DateTime.Today;
            MngrVoucherDatePickerEnd.Value = DateTime.Today;
            MngrVoucherPromoNameTextBox.Text = string.Empty;
            MngrVoucherPromoCodeTextBox.Text = string.Empty;
            MngrVoucherPromoDiscTextBox.Text = string.Empty;
            MngrVoucherAvailNumTextBox.Text = string.Empty;
            MngrVoucherSelectCatTextBox.Text = string.Empty;
        }

        private void MngrVoucherEditBtn_Click(object sender, EventArgs e)
        {
            if (MngrVoucherDGV.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = MngrVoucherDGV.SelectedRows[0];

                DateTime dateStart = Convert.ToDateTime(selectedRow.Cells["DateStart"].Value);
                DateTime dateEnd = Convert.ToDateTime(selectedRow.Cells["DateEnd"].Value);
                string promoName = Convert.ToString(selectedRow.Cells["PromoName"].Value);
                string promoCategory = Convert.ToString(selectedRow.Cells["PromoCategory"].Value);
                string promoCode = Convert.ToString(selectedRow.Cells["PromoCode"].Value);
                string promoDiscount = Convert.ToString(selectedRow.Cells["PromoDiscount"].Value);
                int availableNumber = Convert.ToInt32(selectedRow.Cells["AvailableNumber"].Value);

                MngrVoucherDatePickerStart.Value = dateStart;
                MngrVoucherDatePickerEnd.Value = dateEnd;
                MngrVoucherPromoNameTextBox.Text = promoName;
                MngrVoucherPromoCodeTextBox.Text = promoCode;
                MngrVoucherPromoDiscTextBox.Text = promoDiscount;
                MngrVoucherSelectCatTextBox.Text = promoCategory;
                MngrVoucherAvailNumTextBox.Text = availableNumber.ToString();

                MngrVoucherDGV.ClearSelection();
                MngrVoucherEditBtn.Visible = false;
                MngrVoucherInsertBtn.Visible = false;
                MngrVoucherUpdateBtn.Visible = true;
                MngrVoucherCancelBtn.Visible = true;
            }
            else
            {
                MessageBox.Show("Please select a row to edit.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void MngrVoucherCancelBtn_Click(object sender, EventArgs e)
        {
            ClearFields();
            PromoCodeGenerator();
            MngrVoucherEditBtn.Visible = true;
            MngrVoucherInsertBtn.Visible = true;
            MngrVoucherUpdateBtn.Visible = false;
            MngrVoucherCancelBtn.Visible = false;
        }

        private void MngrVoucherUpdateBtn_Click(object sender, EventArgs e)
        {
            DateTime updatedDateStart = MngrVoucherDatePickerStart.Value;
            DateTime updatedDateEnd = MngrVoucherDatePickerEnd.Value;
            string updatedPromoName = MngrVoucherPromoNameTextBox.Text;
            string updatedPromoCode = MngrVoucherPromoCodeTextBox.Text;
            string updatedPromoDiscount = MngrVoucherPromoDiscTextBox.Text;
            string updatedAvailableNumber = MngrVoucherAvailNumTextBox.Text;
            string updatedPromoCategory = MngrVoucherSelectCatTextBox.Text;

            if (string.IsNullOrWhiteSpace(updatedPromoName) || string.IsNullOrWhiteSpace(updatedPromoDiscount)
                || string.IsNullOrWhiteSpace(updatedAvailableNumber) || string.IsNullOrEmpty(updatedPromoCategory))
            {
                MessageBox.Show("Please fill in all required fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (updatedDateStart > updatedDateEnd && updatedDateStart.Date != updatedDateEnd.Date)
            {
                MessageBox.Show("Start date cannot be ahead of end date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!updatedPromoDiscount.EndsWith("%"))
            {
                MessageBox.Show("Promo discount must end with '%'.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(updatedAvailableNumber, out _))
            {
                MessageBox.Show("Please enter a valid number for Available Number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string formattedDateStart = updatedDateStart.ToString("MMMM d, yyyy");
            string formattedDateEnd = updatedDateEnd.ToString("MMMM d, yyyy");

            string connectionString = "Server=localhost;Database=enchante;Uid=root;Pwd=;";
            string updateQuery = "UPDATE voucher SET DateStart = @DateStart, DateEnd = @DateEnd, " +
                                 "PromoName = @PromoName, PromoCategory = @PromoCategory, PromoDiscount = @PromoDiscount, " +
                                 "AvailableNumber = @AvailableNumber " +
                                 "WHERE PromoCode = @PromoCode";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@DateStart", formattedDateStart);
                    command.Parameters.AddWithValue("@DateEnd", formattedDateEnd);
                    command.Parameters.AddWithValue("@PromoName", updatedPromoName);
                    command.Parameters.AddWithValue("@PromoCategory", updatedPromoCategory);
                    command.Parameters.AddWithValue("@PromoDiscount", updatedPromoDiscount);
                    command.Parameters.AddWithValue("@AvailableNumber", updatedAvailableNumber);
                    command.Parameters.AddWithValue("@PromoCode", updatedPromoCode);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Update successful.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            VouchersShow();
                            ClearFields();
                            PromoCodeGenerator();
                            UpdateDataGridViewAndLabel();
                            MngrVoucherEditBtn.Visible = true;
                            MngrVoucherInsertBtn.Visible = true;
                            MngrVoucherUpdateBtn.Visible = false;
                            MngrVoucherCancelBtn.Visible = false;
                        }
                        else
                        {
                            MessageBox.Show("Promo code not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error updating voucher: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private int currentBatch = 1;

        private void MngrVoucherNextBtn_Click(object sender, EventArgs e)
        {
            int totalBatches = string.IsNullOrEmpty(MngrVoucherSearchTextBox.Text.Trim())
                                ? (int)Math.Ceiling((double)GetTotalRows() / 10)
                                : (int)Math.Ceiling((double)GetFilteredTotalRows() / 10);

            if (currentBatch >= totalBatches)
            {
                MessageBox.Show("No more data to show.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            currentBatch++;

            UpdateDataGridViewAndLabel();
            ApplyRowAlternatingColors(MngrVoucherDGV);
        }

        private void MngrVoucherPreviousBtn_Click(object sender, EventArgs e)
        {
            if (currentBatch <= 1)
            {
                MessageBox.Show("No more previous data to show.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            currentBatch--;

            UpdateDataGridViewAndLabel();
            ApplyRowAlternatingColors(MngrVoucherDGV);
        }

        private void UpdateDataGridViewAndLabel()
        {
            string connectionString = "Server=localhost;Database=enchante;Uid=root;Pwd=;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string countQuery = string.IsNullOrEmpty(MngrVoucherSearchTextBox.Text.Trim())
                                        ? "SELECT COUNT(*) FROM voucher"
                                        : $"SELECT COUNT(*) FROM voucher WHERE {GetFilterExpression()}";

                    MySqlCommand countCommand = new MySqlCommand(countQuery, connection);
                    int totalRows = Convert.ToInt32(countCommand.ExecuteScalar());

                    if (totalRows == 0)
                    {
                        MessageBox.Show("No matching data found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        MngrVoucherCurrentRecordLbl.Text = "0 of 0";
                        return;
                    }

                    int totalBatches = (int)Math.Ceiling((double)totalRows / 10);

                    currentBatch = Math.Min(currentBatch, totalBatches);

                    string query = string.IsNullOrEmpty(MngrVoucherSearchTextBox.Text.Trim())
                                    ? GetRegularQuery()
                                    : GetFilteredQuery();

                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    System.Data.DataTable dataTable = new System.Data.DataTable();
                    adapter.Fill(dataTable);

                    if (!dataTable.Columns.Contains("Status"))
                    {
                        dataTable.Columns.Add("Status", typeof(string));
                    }

                    if (!dataTable.Columns.Contains("PromoCategory"))
                    {
                        dataTable.Columns.Add("PromoCategory", typeof(string));
                    }

                    foreach (DataRow row in dataTable.Rows)
                    {
                        string promoCode = row["PromoCode"].ToString();
                        DateTime dateStart = Convert.ToDateTime(row["DateStart"]);
                        DateTime dateEnd = Convert.ToDateTime(row["DateEnd"]);
                        int availableNumber = Convert.ToInt32(row["AvailableNumber"]);

                        DateTime currentDate = DateTime.Now;
                        string status = "";

                        if (availableNumber == 0)
                        {
                            status = "Fully Claimed";
                        }
                        else if (currentDate > dateEnd)
                        {
                            status = "Expired";
                        }
                        else if (currentDate >= dateStart && currentDate <= dateEnd)
                        {
                            status = "Ongoing";
                        }
                        else
                        {
                            status = "Pending";
                        }

                        row["Status"] = status;
                    }

                    foreach (DataColumn column in dataTable.Columns)
                    {
                        if (!MngrVoucherDGV.Columns.Contains(column.ColumnName))
                        {
                            MngrVoucherDGV.Columns.Add(column.ColumnName, column.ColumnName);
                        }
                    }

                    foreach (DataRow row in dataTable.Rows)
                    {
                        string promoCode = row["PromoCode"].ToString();

                        string promoCategory = GetPromoCategoryFromDatabase(promoCode);

                        row["PromoCategory"] = promoCategory;
                    }

                    MngrVoucherDGV.DataSource = dataTable;

                    MngrVoucherCurrentRecordLbl.Text = $"{currentBatch} of {totalBatches}";

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private string GetPromoCategoryFromDatabase(string promoCode)
        {
            string promoCategory = "";
            string connectionString = "Server=localhost;Database=enchante;Uid=root;Pwd=;";
            string query = "SELECT PromoCategory FROM voucher WHERE PromoCode = @PromoCode";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PromoCode", promoCode);
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            promoCategory = reader["PromoCategory"].ToString();
                        }
                    }
                }

                connection.Close();
            }

            return promoCategory;
        }

        private string GetRegularQuery()
        {
            int startIndex = (currentBatch - 1) * 10;
            return $"SELECT DateStart, DateEnd, PromoName, PromoCode, PromoDiscount, " +
                    $"AvailableNumber, PromoCreated FROM voucher " +
                    $"ORDER BY PromoCreated DESC LIMIT {startIndex}, 10";
        }

        private string GetFilteredQuery()
        {
            string filterExpression = GetFilterExpression();
            int startIndex = (currentBatch - 1) * 10;
            return $"SELECT DateStart, DateEnd, PromoName, PromoCode, PromoDiscount, " +
                    $"AvailableNumber, PromoCreated FROM voucher " +
                    $"WHERE {filterExpression} " +
                    $"ORDER BY PromoCreated DESC LIMIT {startIndex}, 10";
        }

        private string GetFilterExpression()
        {
            string searchText = MngrVoucherSearchTextBox.Text.Trim();
            return string.Join(" OR ", ((DataTable)MngrVoucherDGV.DataSource).Columns.Cast<DataColumn>()
                                .Select(col => $"{col.ColumnName} LIKE '%{searchText}%'"));
        }

        private int GetTotalRows()
        {
            string connectionString = "Server=localhost;Database=enchante;Uid=root;Pwd=;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string countQuery = "SELECT COUNT(*) FROM voucher";
                    MySqlCommand countCommand = new MySqlCommand(countQuery, connection);
                    int totalRows = Convert.ToInt32(countCommand.ExecuteScalar());
                    connection.Close();
                    return totalRows;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return 0;
            }
        }

        private int GetFilteredTotalRows()
        {
            string connectionString = "Server=localhost;Database=enchante;Uid=root;Pwd=;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = $"SELECT COUNT(*) FROM voucher WHERE {GetFilterExpression()}";
                    MySqlCommand countCommand = new MySqlCommand(query, connection);
                    int totalRows = Convert.ToInt32(countCommand.ExecuteScalar());
                    connection.Close();
                    return totalRows;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return 0;
            }
        }

        private void MngrVoucherSearchTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchText = MngrVoucherSearchTextBox.Text.Trim();

            DataView dv = ((DataTable)MngrVoucherDGV.DataSource).DefaultView;

            if (string.IsNullOrEmpty(searchText))
            {
                dv.RowFilter = string.Empty;
            }
            else
            {
                string filterExpression = string.Join(" OR ", ((DataTable)MngrVoucherDGV.DataSource).Columns.Cast<DataColumn>()
                                                    .Select(col => $"{col.ColumnName} LIKE '{searchText}%'"));
                dv.RowFilter = filterExpression;
            }

            UpdateDataGridViewAndLabel();
            ApplyRowAlternatingColors(MngrVoucherDGV);
        }

        private void MngrVoucherPromoCategoryComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MngrVoucherPromoCategoryComboBox.SelectedItem != null)
            {
                string selectedItem = MngrVoucherPromoCategoryComboBox.SelectedItem.ToString();

                if (selectedItem == "All Categories")
                {
                    MngrVoucherSelectCatTextBox.Text = "All Categories";
                }
                else
                {
                    if (MngrVoucherSelectCatTextBox.Text.Contains(selectedItem))
                    {
                        MessageBox.Show("This category has already been selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (MngrVoucherSelectCatTextBox.Text == "All Categories")
                    {
                        MessageBox.Show("Cannot add other categories when 'All Categories' is selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(MngrVoucherSelectCatTextBox.Text))
                        {
                            MngrVoucherSelectCatTextBox.Text += "," + selectedItem;
                        }
                        else
                        {
                            MngrVoucherSelectCatTextBox.Text = selectedItem;
                        }
                    }
                }

                MngrVoucherPromoCategoryComboBox.SelectedIndex = -1;
            }
        }

        private void MngrVoucherXBtn_Click(object sender, EventArgs e)
        {
            MngrVoucherSelectCatTextBox.Text = "";
        }

        private void MngrVoucherPromoNameTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ' && MngrVoucherPromoNameTextBox.Text.Length == 0)
            {
                e.Handled = true;
                return;
            }

            if (MngrVoucherPromoNameTextBox.Text.Length >= 100)
            {
                e.Handled = true;
                return;
            }
        }

        private void MngrVoucherPromoDiscTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ' && MngrVoucherPromoDiscTextBox.Text.Length == 0)
            {
                e.Handled = true;
                return;
            }

            if (MngrVoucherPromoDiscTextBox.Text.Length >= 3 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                return;
            }

            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '%' && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                return;
            }

            if (e.KeyChar == '%' && MngrVoucherPromoDiscTextBox.Text.Length > 0 && MngrVoucherPromoDiscTextBox.Text.Length < 2)
            {
                return;
            }

            e.Handled = false;
        }

        private void MngrVoucherAvailNumTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ' && MngrVoucherAvailNumTextBox.Text.Length == 0)
            {
                e.Handled = true;
                return;
            }

            if (MngrVoucherAvailNumTextBox.Text.Length >= 4 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
                return;
            }

            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        #endregion

        #region Mngr. PANEL OF MEMBER ACCOUNTS
        private void MemberAccountsShow()
        {
            string connectionString = "Server=localhost;Database=enchante;Uid=root;Pwd=;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT MembershipType, MemberIDNumber, AccountStatus, FirstName, LastName, " +
                                    "Birthday, CPNumber, EmailAdd, AccountCreated FROM membershipaccount";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    System.Data.DataTable dataTable = new System.Data.DataTable();
                    adapter.Fill(dataTable);

                    MngrMemAccDGV.DataSource = dataTable;

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private bool considerDateFilter_MngrMemAcc = false;

        private string GetCombinedFilter_MngrMemAcc()
        {
            string membershipTypeFilter = GetMembershipTypeFilter();
            string dateFilter = considerDateFilter_MngrMemAcc ? FilterRowByDateCreated() : string.Empty;

            List<string> filters = new List<string>();

            if (!string.IsNullOrEmpty(membershipTypeFilter))
                filters.Add(membershipTypeFilter);

            if (!string.IsNullOrEmpty(dateFilter))
                filters.Add(dateFilter);

            if (filters.Count == 1)
                return filters[0];

            string combinedFilter = string.Join(" AND ", filters);

            return combinedFilter;
        }

        private string GetMembershipTypeFilter()
        {
            if (MngrMemAccMemTypeBox.SelectedItem != null)
            {
                string selectedMembershipType = MngrMemAccMemTypeBox.SelectedItem.ToString();
                return $"MembershipType = '{selectedMembershipType}'";
            }
            return string.Empty;
        }

        private void ApplyCombinedFilter_MngrMemAcc()
        {
            string combinedFilter = GetCombinedFilter_MngrMemAcc();

            DataView dv = ((DataTable)MngrMemAccDGV.DataSource).DefaultView;
            dv.RowFilter = combinedFilter;
        }

        private string FilterRowByDateCreated()
        {
            DateTime fromDate = MngrMemAccDatePickFrom.Value.Date;
            DateTime toDate = MngrMemAccDatePickTo.Value.Date;

            if (fromDate > toDate && fromDate != DateTime.Now.Date && toDate != DateTime.Now.Date)
            {
                MessageBox.Show("From date should not be ahead of To date.", "Invalid Date Range", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            toDate = toDate.AddDays(1);

            if (fromDate == toDate)
            {
                string dateString = fromDate.ToString("MM-dd-yyyy");
                return $"CONVERT(AccountCreated, 'System.String') LIKE '{dateString}%'";
            }
            else
            {
                string fromDateString = fromDate.ToString("MM-dd-yyyy");
                string toDateString = toDate.ToString("MM-dd-yyyy");

                return $"CONVERT(AccountCreated, 'System.String') >= '{fromDateString}' AND CONVERT(AccountCreated, 'System.String') <= '{toDateString}'";
            }
        }

        private void MngrMemAccMemTypeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyCombinedFilter_MngrMemAcc();
        }

        private void MngrMemAccDatePickFrom_ValueChanged(object sender, EventArgs e)
        {
            considerDateFilter_MngrMemAcc = true;
            ApplyCombinedFilter_MngrMemAcc();
        }

        private void MngrMemAccDatePickTo_ValueChanged(object sender, EventArgs e)
        {
            considerDateFilter_MngrMemAcc = true;
            ApplyCombinedFilter_MngrMemAcc();
        }

        private void MngrMemAccResetBtn_Click(object sender, EventArgs e)
        {
            MngrMemAccMemTypeBox.SelectedIndex = -1;
            MngrMemAccDatePickFrom.Value = DateTime.Now;
            MngrMemAccDatePickTo.Value = DateTime.Now;
            considerDateFilter_MngrMemAcc = false;
            MemberAccountsShow();

            DataView dv = ((DataTable)MngrMemAccDGV.DataSource).DefaultView;
            dv.RowFilter = string.Empty;
        }
        #endregion

        private void ExitFunction()
        {
            MngrServicesCreateBtn.Visible = true;
            MngrServicesUpdateBtn.Visible = false;
            MngrServicesCategoryComboText.Enabled = true;
            MngrServicesTypeComboText.Enabled = true;
            MngrServicesCategoryComboText.SelectedIndex = -1;
            MngrServicesTypeComboText.SelectedIndex = -1;
            MngrServicesRequiredItemBox.SelectedIndex = -1;
            MngrServicesCurrentRecordLbl.Text = "0 of 0";
            MngrServicesCategoryComboText.Text = "";
            MngrServicesTypeComboText.Text = "";
            MngrServicesNameText.Text = "";
            MngrServicesDescriptionText.Text = "";
            MngrServicesDurationText.Text = "";
            MngrServicesPriceText.Text = "";
            MngrServicesIDNumText.Text = "";
            MngrServicesSelectedReqItemText.Text = "";
            MngrServicesNumOfItems.Text = "";
            MngrServicesSearchTextBox.Text = "";

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
            MngrInventoryProductsCurrentRecordLbl.Text = "0 of 0";
            MngrInventoryProductsIDText.Text = "";
            MngrInventoryProductsNameText.Text = "";
            MngrInventoryProductsPriceText.Text = "";
            MngrInventoryProductsStockText.Text = "";
            MngrInventoryProductsSearchTextBox.Text = "";
            ProductImagePictureBox.Image = null;

            MngrVoucherEditBtn.Visible = true;
            MngrVoucherInsertBtn.Visible = true;
            MngrVoucherUpdateBtn.Visible = false;
            MngrVoucherCancelBtn.Visible = false;
            MngrVoucherDatePickerStart.Value = DateTime.Today;
            MngrVoucherDatePickerEnd.Value = DateTime.Today;
            MngrVoucherPromoNameTextBox.Text = string.Empty;
            MngrVoucherPromoDiscTextBox.Text = string.Empty;
            MngrVoucherAvailNumTextBox.Text = string.Empty;
            MngrVoucherSelectCatTextBox.Text = string.Empty;
            MngrVoucherSearchTextBox.Text = string.Empty;
            MngrVoucherCurrentRecordLbl.Text = "0 of 0";

            MngrWalkinSalesSelectedPeriodLbl.Visible = true;
            MngrWalkinSalesSelectedPeriodText.Visible = true;
            MngrWalkinSalesFromLbl.Visible = false;
            MngrWalkinSalesFromDatePicker.Visible = false;
            MngrWalkinSalesToLbl.Visible = false;
            MngrWalkinSalesToDatePicker.Visible = false;
            MngrWalkinSalesPeriod.SelectedItem = null;
            MngrWalkinSalesSelectCatBox.SelectedItem = null;
            MngrWalkinSalesSelectedPeriodText.Text = "";
            MngrWalkinSalesTransIDShow.Text = "";
            MngrWalkinSalesRevenueTextbox.Text = "";
            MngrWalkinSalesSearchTextBox.Text = "";
            MngrWalkinSalesCurrentRecordLbl.Text = "0 of 0";
            MngrWalkinSalesTransRepDGV.DataSource = null;
            MngrWalkinSalesTransRepDGVTwo.DataSource = null;
            MngrWalkinSalesTransServiceHisDGV.DataSource = null;
            MngrWalkinSalesGraph.Series.Clear();
            MngrWalkinSalesGraph.Legends.Clear();

            MngrAppSalesSelectedPeriodLbl.Visible = true;
            MngrAppSalesSelectedPeriodText.Visible = true;
            MngrAppSalesFromLbl.Visible = false;
            MngrAppSalesFromDatePicker.Visible = false;
            MngrAppSalesToLbl.Visible = false;
            MngrAppSalesToDatePicker.Visible = false;
            MngrAppSalesPeriod.SelectedItem = null;
            MngrAppSalesSelectCatBox.SelectedItem = null;
            MngrAppSalesSelectedPeriodText.Text = "";
            MngrAppSalesTransIDShow.Text = "";
            MngrAppSalesTotalRevBox.Text = "";
            MngrAppSalesSearchTextBox.Text = "";
            MngrAppSalesCurrentRecordLbl.Text = "0 of 0";
            MngrAppSalesTransRepDGV.DataSource = null;
            MngrAppSalesTransRepDGVTwo.DataSource = null;
            MngrAppSalesTransServiceHisDGV.DataSource = null;
            MngrAppSalesGraph.Series.Clear();
            MngrAppSalesGraph.Legends.Clear();

            trydata.Visible = false;
            MngrProductSalesTransRepDGV.DataSource = null;
            MngrProductSalesTransRepDGVTwo.DataSource = null;
            trydata.DataSource = null;
            MngrProductSalesSelectedPeriodLbl.Visible = true;
            MngrProductSalesSelectedPeriodText.Visible = true;
            MngrProductSalesFromLbl.Visible = false;
            MngrProductSalesFromDatePicker.Visible = false;
            MngrProductSalesToLbl.Visible = false;
            MngrProductSalesToDatePicker.Visible = false;
            MngrProductSalesPeriod.SelectedItem = null;
            MngrProductSalesSelectCatBox.SelectedItem = null;
            MngrProductSalesSelectedPeriodText.Text = "";
            MngrProductSalesTotalRevBox.Text = "";
            MngrProductSalesSearchTextBox.Text = "";
            MngrProductSalesCurrentRecordLbl.Text = "0 of 0";
            MngrProductSalesLineGraph.Series.Clear();
            MngrProductSalesGraph.Series.Clear();

            MngrIndemandSelectPeriodLbl.Visible = true;
            MngrIndemandSelectPeriod.Visible = true;
            MngrIndemandFromLbl.Visible = false;
            MngrIndemandDatePickerFrom.Visible = false;
            MngrIndemandToLbl.Visible = false;
            MngrIndemandDatePickerTo.Visible = false;
            MngrIndemandServiceHistoryPeriod.SelectedItem = null;
            MngrIndemandSelectCatBox.SelectedItem = null;
            MngrIndemandSelectPeriod.Text = "";
            MngrIndemandSearchTextBox.Text = "";
            MngrIndemandCurrentRecordLbl.Text = "0 of 0";
            MngrIndemandServiceGraph.Series.Clear();
            MngrIndemandServiceSelection.DataSource = null;
            MngrIndemandServiceSelectionTwo.DataSource = null;
            MngrIndemandBestEmployee.DataSource = null;

            MngrSVHistoryTransTypeBox.SelectedIndex = -1;
            MngrSVHistoryServiceStatusBox.SelectedIndex = -1;
            MngrSVHistoryServiceCatBox.SelectedIndex = -1;
            MngrSVHistoryDatePickFrom.Value = DateTime.Now;
            MngrSVHistoryDatePickTo.Value = DateTime.Now;
            ConsiderDateFilter = false;
            MngrSVHistorySearchTextBox.Text = "";
            currentBatchFour = 1;
            UpdateDataGridViewAndLabelFour();

            MngrPDHistoryStatusBox.SelectedIndex = -1;
            MngrPDHistoryItemCatBox.SelectedIndex = -1;
            MngrPDHistoryDatePickFrom.Value = DateTime.Now;
            MngrPDHistoryDatePickTo.Value = DateTime.Now;
            considerDateFilter = false;
            MngrPDSearchTextBox.Text = "";
            currentBatchThree = 1;
            UpdateDataGridViewAndLabelThree();
        }

        private void DrawRoundedBorderPanel(System.Windows.Forms.Panel panel, PaintEventArgs e, Color borderColor, int borderWidth, int radius)
        {
            System.Drawing.Drawing2D.GraphicsPath borderPath = new System.Drawing.Drawing2D.GraphicsPath();
            System.Drawing.Rectangle borderRectangle = panel.ClientRectangle;
            borderRectangle.Width -= borderWidth;
            borderRectangle.Height -= borderWidth;

            borderPath.AddArc(borderRectangle.X, borderRectangle.Y, radius, radius, 180, 90);
            borderPath.AddArc(borderRectangle.Right - radius, borderRectangle.Y, radius, radius, 270, 90);
            borderPath.AddArc(borderRectangle.Right - radius, borderRectangle.Bottom - radius, radius, radius, 0, 90);
            borderPath.AddArc(borderRectangle.X, borderRectangle.Bottom - radius, radius, radius, 90, 90);
            borderPath.CloseFigure();

            e.Graphics.DrawPath(new System.Drawing.Pen(borderColor, borderWidth), borderPath);
        }

        private void trypanel_Paint(object sender, PaintEventArgs e)
        {
            System.Drawing.Color borderColor = System.Drawing.Color.FromArgb(216, 213, 178);
            int borderWidth = 4;
            int radius = 20;

            DrawRoundedBorderPanel((System.Windows.Forms.Panel)sender, e, borderColor, borderWidth, radius);
        }

        private void ApplyRowAlternatingColors(DataGridView dataGridView)
        {
            Color evenRowColor = Color.FromArgb(106, 155, 91);
            Color oddRowColor = Color.FromArgb(89, 136, 82);

            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.Index % 2 == 0)
                {
                    row.DefaultCellStyle.BackColor = evenRowColor;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = oddRowColor;
                }
            }
        }       
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
            DateTime selectedDate = AdminBdayPicker.Value;
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

                selectedHashedPerUser = selectedRow.Cells["HashedPerUser"].Value?.ToString();
                AdminEmplTypeComboText.Enabled = false;
                AdminEmplCatComboText.Enabled = false;
                AdminEmplIDText.Enabled = false;
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
            AdminEmplIDText.Enabled = true;
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

        private bool IsNumericTwo(string input)
        {
            bool hasDecimalPoint = false;

            foreach (char c in input)
            {
                if (!char.IsDigit(c))
                {
                    if (c == '.' && !hasDecimalPoint)
                    {
                        hasDecimalPoint = true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }


        private void AdminCreateAccBtn_Click(object sender, EventArgs e)
        {
            DateTime selectedDate = AdminBdayPicker.Value;
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
                    string countQuery = "SELECT COUNT(*) FROM systemusers";
                    MySqlCommand countCommand = new MySqlCommand(countQuery, connection);
                    int totalRows = Convert.ToInt32(countCommand.ExecuteScalar());
                    string query = "SELECT FirstName, LastName, Email, DATE(Birthday) AS Birthday, Age, Gender, PhoneNumber, EmployeeType, " +
                        "EmployeeCategory, EmployeeCategoryLevel, EmployeeID, HashedPass, HashedFixedSalt, HashedPerUser FROM systemusers LIMIT 10";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            DataTable dataTable = new DataTable();
                            adapter.Fill(dataTable);

                            // Bind the DataTable to the DataGridView
                            AdminAccountTable.DataSource = dataTable;

                            AdminAccountTable.RowTemplate.Height = 53;
                            AdminAccountTable.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                            AdminAccountTable.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                            ApplyRowAlternatingColors(AdminAccountTable);

                            int currentBatch = totalRows > 0 ? 1 : 0;
                            int totalBatches = (int)Math.Ceiling((double)totalRows / 10);

                            AdminCurrentRecordLbl.Text = $"{currentBatch} of {totalBatches}";

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private int currentBatchAdmin = 1;

        private void AdminNextBtn_Click(object sender, EventArgs e)
        {
            int totalBatches = string.IsNullOrEmpty(AdminSearchTextBox.Text.Trim())
                        ? (int)Math.Ceiling((double)GetTotalRowsAdmin() / 10)
                        : (int)Math.Ceiling((double)GetFilteredTotalRowsAdmin() / 10);

            if (currentBatchAdmin >= totalBatches)
            {
                MessageBox.Show("No more data to show.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            currentBatchAdmin++;

            UpdateDataGridViewAndLabelAdmin();
            ApplyRowAlternatingColors(AdminAccountTable);
        }

        private void AdminPreviousBtn_Click(object sender, EventArgs e)
        {
            if (currentBatchAdmin <= 1)
            {
                MessageBox.Show("No more previous data to show.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            currentBatchAdmin--;

            UpdateDataGridViewAndLabelAdmin();
            ApplyRowAlternatingColors(AdminAccountTable);
        }

        private void UpdateDataGridViewAndLabelAdmin()
        {
            string connectionString = "Server=localhost;Database=enchante;Uid=root;Pwd=;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string countQuery = string.IsNullOrEmpty(AdminSearchTextBox.Text.Trim())
                                        ? "SELECT COUNT(*) FROM systemusers"
                                        : $"SELECT COUNT(*) FROM systemusers WHERE {GetFilterExpressionAdmin()}";

                    MySqlCommand countCommand = new MySqlCommand(countQuery, connection);
                    int totalRows = Convert.ToInt32(countCommand.ExecuteScalar());

                    if (totalRows == 0)
                    {
                        MessageBox.Show("No matching data found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        AdminCurrentRecordLbl.Text = "0 of 0";
                        return;
                    }

                    int totalBatches = (int)Math.Ceiling((double)totalRows / 10);

                    currentBatchAdmin = Math.Min(currentBatchAdmin, totalBatches);

                    string query = string.IsNullOrEmpty(AdminSearchTextBox.Text.Trim())
                                    ? GetRegularQueryAdmin()
                                    : GetFilteredQueryAdmin();

                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    System.Data.DataTable dataTable = new System.Data.DataTable();
                    adapter.Fill(dataTable);

                    AdminAccountTable.DataSource = dataTable;
                    AdminCurrentRecordLbl.Text = $"{currentBatchAdmin} of {totalBatches}";

                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private string GetRegularQueryAdmin()
        {
            int startIndex = (currentBatchAdmin - 1) * 10;
            return $"SELECT FirstName, LastName, Email, DATE(Birthday) AS Birthday, Age, Gender, PhoneNumber, EmployeeType, " +
                    $"EmployeeCategory, EmployeeCategoryLevel, EmployeeID, HashedPass, HashedFixedSalt, HashedPerUser FROM systemusers " +
                    $"LIMIT {startIndex}, 10";
        }

        private string GetFilteredQueryAdmin()
        {
            string filterExpression = GetFilterExpressionAdmin();
            int startIndex = (currentBatchAdmin - 1) * 10;
            return $"SELECT FirstName, LastName, Email, DATE(Birthday) AS Birthday, Age, Gender, PhoneNumber, EmployeeType, " +
                    $"EmployeeCategory, EmployeeCategoryLevel, EmployeeID, HashedPass, HashedFixedSalt, HashedPerUser FROM systemusers " +
                    $"WHERE {filterExpression} " +
                    $"LIMIT {startIndex}, 10";
        }

        private string GetFilterExpressionAdmin()
        {
            string searchText = AdminSearchTextBox.Text.Trim();
            return string.Join(" OR ", ((DataTable)AdminAccountTable.DataSource).Columns.Cast<DataColumn>()
                                .Select(col => $"{col.ColumnName} LIKE '%{searchText}%'"));
        }

        private int GetTotalRowsAdmin()
        {
            string connectionString = "Server=localhost;Database=enchante;Uid=root;Pwd=;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string countQuery = "SELECT COUNT(*) FROM systemusers";
                    MySqlCommand countCommand = new MySqlCommand(countQuery, connection);
                    int totalRows = Convert.ToInt32(countCommand.ExecuteScalar());
                    connection.Close();
                    return totalRows;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return 0;
            }
        }

        private int GetFilteredTotalRowsAdmin()
        {
            string connectionString = "Server=localhost;Database=enchante;Uid=root;Pwd=;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string query = $"SELECT COUNT(*) FROM systemusers WHERE {GetFilterExpressionAdmin()}";
                    MySqlCommand countCommand = new MySqlCommand(query, connection);
                    int totalRows = Convert.ToInt32(countCommand.ExecuteScalar());
                    connection.Close();
                    return totalRows;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return 0;
            }
        }

        private void AdminSearchTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchText = AdminSearchTextBox.Text.Trim();

            DataView dv = ((DataTable)AdminAccountTable.DataSource).DefaultView;

            if (string.IsNullOrEmpty(searchText))
            {
                dv.RowFilter = string.Empty;
            }
            else
            {
                string filterExpression = string.Join(" OR ", ((DataTable)AdminAccountTable.DataSource).Columns.Cast<DataColumn>()
                                                    .Select(col => $"CONVERT([{col.ColumnName}], 'System.String') LIKE '%{searchText}%'"));
                dv.RowFilter = filterExpression;
            }
            UpdateDataGridViewAndLabelAdmin();
            ApplyRowAlternatingColors(AdminAccountTable);
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

        private void StaffServiceRateTestBtn_Click(object sender, EventArgs e)
        {

        }

        private void MngrApptServiceBtn_Click(object sender, EventArgs e)
        {
            MngrApptSalesColor();
            ExitFunction();
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
            MngrAppSalesTotalRevBox.Text = "";
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

                        string updateQuery = $"UPDATE appointment SET AppointmentStatus = 'Cancelled', ServiceStatus = 'Cancelled' WHERE TransactionNumber = '{transactionID}'";
                        string updateQuery2 = $"UPDATE servicehistory SET ServiceStatus = 'Cancelled' WHERE TransactionNumber = '{transactionID}'";
                        MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection);
                        int rowsAffected = updateCommand.ExecuteNonQuery();
                        MySqlCommand updateCommand2 = new MySqlCommand(updateQuery2, connection);
                        int rowsAffected2 = updateCommand2.ExecuteNonQuery();

                        if (rowsAffected > 0 && rowsAffected2 > 0)
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
                    MessageBox.Show("Appointment cancellation cancelled.");
                }
            }
            else
            {
                MessageBox.Show("Please select a transaction number.");
            }
        }


        private void RecApptAcceptLateDeclineDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = RecApptAcceptLateDeclineDGV.Rows[e.RowIndex];
                string transactionNumber = selectedRow.Cells["TransactionID"].Value.ToString();
                string serviceHistoryQuery = "SELECT TransactionNumber, ServiceCategory, ServiceID, SelectedService " +
                                             "FROM servicehistory " +
                                             "WHERE TransactionNumber = @transactionNumber AND (ServiceStatus = 'Pending' OR ServiceStatus = 'PendingPaid')";


                using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                {
                    connection.Open();

                    MySqlCommand command = new MySqlCommand(serviceHistoryQuery, connection);
                    command.Parameters.AddWithValue("@transactionNumber", transactionNumber);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    DataTable serviceHistoryTable = new DataTable();
                    adapter.Fill(serviceHistoryTable);

                    RecCancelServicesDGV.Rows.Clear();
                    if (serviceHistoryTable.Rows.Count > 0)
                    {
                        foreach (DataRow serviceHistoryRow in serviceHistoryTable.Rows)
                        {
                            RecCancelServicesDGV.Rows.Add(
                                serviceHistoryRow["TransactionNumber"],
                                serviceHistoryRow["ServiceCategory"],
                                serviceHistoryRow["ServiceID"],
                                serviceHistoryRow["SelectedService"]
                            );
                        }
                    }
                }

            }
        }

        private void RecCancelServicesDGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (RecCancelServicesDGV.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = RecCancelServicesDGV.SelectedRows[0];

                string transactionNumber = selectedRow.Cells["RecServiceTransactionID"].Value.ToString();
                string serviceID = selectedRow.Cells["RecServiceServiceID"].Value.ToString();

                DialogResult confirmationResult = MessageBox.Show("Are you sure you want to cancel the selected service?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirmationResult == DialogResult.Yes)
                {
                    using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                    {

                        connection.Open();

                        string updateQuery1 = "UPDATE servicehistory SET ServiceStatus = 'Cancelled' WHERE TransactionNumber = @TransactionNumber AND ServiceID = @ServiceID";
                        string updateQuery3 = "UPDATE walk_in_appointment SET ServiceStatus = @ServiceStatus WHERE TransactionNumber = @TransactionNumber";
                        string updateQuery4 = "UPDATE appointment SET ServiceStatus = @ServiceStatus WHERE TransactionNumber = @TransactionNumber";

                        using (MySqlCommand command = new MySqlCommand(updateQuery1, connection))
                        {
                            command.Parameters.AddWithValue("@TransactionNumber", transactionNumber);
                            command.Parameters.AddWithValue("@ServiceID", serviceID);
                            command.ExecuteNonQuery();
                        }

                        string countQuery = "SELECT COUNT(*) FROM servicehistory WHERE TransactionNumber = @TransactionNumber AND (ServiceStatus = 'Pending Paid' OR ServiceStatus = 'Pending') ";
                        int matchCount;
                        string serviceStatus = null;

                        using (MySqlCommand command = new MySqlCommand(countQuery, connection))
                        {
                            command.Parameters.AddWithValue("@TransactionNumber", transactionNumber);
                            matchCount = Convert.ToInt32(command.ExecuteScalar());

                            if (matchCount == 0)
                            {
                                string completedStatusQuery = "SELECT ServiceStatus FROM servicehistory WHERE TransactionNumber = @TransactionNumber AND (ServiceStatus = 'Completed' OR ServiceStatus = 'Completed Paid')";

                                using (MySqlCommand completedStatusCommand = new MySqlCommand(completedStatusQuery, connection))
                                {
                                    completedStatusCommand.Parameters.AddWithValue("@TransactionNumber", transactionNumber);
                                    object completedStatusResult = completedStatusCommand.ExecuteScalar();

                                    if (completedStatusResult != null)
                                    {
                                        string statusQuery = "SELECT ServiceStatus FROM servicehistory WHERE TransactionNumber = @TransactionNumber AND (ServiceStatus = 'Completed' OR ServiceStatus = 'Completed Paid')";

                                        using (MySqlCommand statusCommand = new MySqlCommand(statusQuery, connection))
                                        {
                                            statusCommand.Parameters.AddWithValue("@TransactionNumber", transactionNumber);
                                            object result = statusCommand.ExecuteScalar();

                                            if (result != null)
                                            {
                                                serviceStatus = result.ToString();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        serviceStatus = "Cancelled";
                                    }
                                }
                            }
                            else
                            {
                                string statusQuery = "SELECT ServiceStatus FROM servicehistory WHERE TransactionNumber = @TransactionNumber AND (ServiceStatus = 'Pending' OR ServiceStatus = 'Pending Paid')";

                                using (MySqlCommand statusCommand = new MySqlCommand(statusQuery, connection))
                                {
                                    statusCommand.Parameters.AddWithValue("@TransactionNumber", transactionNumber);
                                    object result = statusCommand.ExecuteScalar();

                                    if (result != null)
                                    {
                                        serviceStatus = result.ToString();
                                    }
                                }
                            }
                        }
                        using (MySqlCommand command = new MySqlCommand(updateQuery3, connection))
                        {
                            command.Parameters.AddWithValue("@ServiceStatus", serviceStatus);
                            command.Parameters.AddWithValue("@TransactionNumber", transactionNumber);
                            command.ExecuteNonQuery();
                        }

                        using (MySqlCommand command = new MySqlCommand(updateQuery4, connection))
                        {
                            command.Parameters.AddWithValue("@ServiceStatus", serviceStatus);
                            command.Parameters.AddWithValue("@TransactionNumber", transactionNumber);
                            command.ExecuteNonQuery();
                        }

                        MessageBox.Show("Service has been cancelled successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RecCancelServicesDGV.Rows.Clear();
                    }
                }
            }
        }

        private void RecCancelServiceBtn_Click(object sender, EventArgs e)
        {
            if (RecCancelServicesDGV.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = RecCancelServicesDGV.SelectedRows[0];

                string transactionNumber = selectedRow.Cells["RecServiceTransactionID"].Value.ToString();
                string serviceID = selectedRow.Cells["RecServiceServiceID"].Value.ToString();

                DialogResult confirmationResult = MessageBox.Show("Are you sure you want to cancel the selected service?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirmationResult == DialogResult.Yes)
                {
                    using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                    {

                        connection.Open();

                        string updateQuery1 = "UPDATE servicehistory SET ServiceStatus = 'Cancelled' WHERE TransactionNumber = @TransactionNumber AND ServiceID = @ServiceID";
                        string updateQuery3 = "UPDATE walk_in_appointment SET ServiceStatus = @ServiceStatus WHERE TransactionNumber = @TransactionNumber";
                        string updateQuery4 = "UPDATE appointment SET ServiceStatus = @ServiceStatus WHERE TransactionNumber = @TransactionNumber";

                        using (MySqlCommand command = new MySqlCommand(updateQuery1, connection))
                        {
                            command.Parameters.AddWithValue("@TransactionNumber", transactionNumber);
                            command.Parameters.AddWithValue("@ServiceID", serviceID);
                            command.ExecuteNonQuery();
                        }

                        string countQuery = "SELECT COUNT(*) FROM servicehistory WHERE TransactionNumber = @TransactionNumber AND (ServiceStatus = 'Pending Paid' OR ServiceStatus = 'Pending') ";
                        int matchCount;
                        string serviceStatus = null;

                        using (MySqlCommand command = new MySqlCommand(countQuery, connection))
                        {
                            command.Parameters.AddWithValue("@TransactionNumber", transactionNumber);
                            matchCount = Convert.ToInt32(command.ExecuteScalar());

                            if (matchCount == 0)
                            {
                                string completedStatusQuery = "SELECT ServiceStatus FROM servicehistory WHERE TransactionNumber = @TransactionNumber AND (ServiceStatus = 'Completed' OR ServiceStatus = 'Completed Paid')";

                                using (MySqlCommand completedStatusCommand = new MySqlCommand(completedStatusQuery, connection))
                                {
                                    completedStatusCommand.Parameters.AddWithValue("@TransactionNumber", transactionNumber);
                                    object completedStatusResult = completedStatusCommand.ExecuteScalar();

                                    if (completedStatusResult != null)
                                    {
                                        string statusQuery = "SELECT ServiceStatus FROM servicehistory WHERE TransactionNumber = @TransactionNumber AND (ServiceStatus = 'Completed' OR ServiceStatus = 'Completed Paid')";

                                        using (MySqlCommand statusCommand = new MySqlCommand(statusQuery, connection))
                                        {
                                            statusCommand.Parameters.AddWithValue("@TransactionNumber", transactionNumber);
                                            object result = statusCommand.ExecuteScalar();

                                            if (result != null)
                                            {
                                                serviceStatus = result.ToString();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        serviceStatus = "Cancelled";
                                    }
                                }
                            }
                            else
                            {
                                string statusQuery = "SELECT ServiceStatus FROM servicehistory WHERE TransactionNumber = @TransactionNumber AND (ServiceStatus = 'Pending' OR ServiceStatus = 'Pending Paid')";

                                using (MySqlCommand statusCommand = new MySqlCommand(statusQuery, connection))
                                {
                                    statusCommand.Parameters.AddWithValue("@TransactionNumber", transactionNumber);
                                    object result = statusCommand.ExecuteScalar();

                                    if (result != null)
                                    {
                                        serviceStatus = result.ToString();
                                    }
                                }
                            }
                        }
                        using (MySqlCommand command = new MySqlCommand(updateQuery3, connection))
                        {
                            command.Parameters.AddWithValue("@ServiceStatus", serviceStatus);
                            command.Parameters.AddWithValue("@TransactionNumber", transactionNumber);
                            command.ExecuteNonQuery();
                        }

                        using (MySqlCommand command = new MySqlCommand(updateQuery4, connection))
                        {
                            command.Parameters.AddWithValue("@ServiceStatus", serviceStatus);
                            command.Parameters.AddWithValue("@TransactionNumber", transactionNumber);
                            command.ExecuteNonQuery();
                        }

                        MessageBox.Show("Service has been cancelled successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RecCancelServicesDGV.Rows.Clear();
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a service to cancel.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void RecApptSearchServiceTypeText_TextChanged(object sender, EventArgs e)
        {
            string searchKeyword = RecApptSearchServiceTypeText.Text.Trim().ToLower();
            InitializeServices2(filterstaffbyservicecategory, searchKeyword);
        }

        private void SearchRecApptAcrossCategories(string searchText, string filterstaffbyservicecategory)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                {
                    connection.Open();

                    string sql = "SELECT ServiceID, Name, Duration, Category, Price FROM `services` WHERE Category = @category AND " +
                                 "(Name LIKE @searchText OR " +
                                 "Duration LIKE @searchText OR " +
                                 "Price LIKE @searchText)";

                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("@searchText", "%" + searchText + "%");
                    cmd.Parameters.AddWithValue("@category", filterstaffbyservicecategory);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        RecWalkinServicesFlowLayoutPanel.Controls.Clear();

                        while (reader.Read())
                        {
                            Services service = new Services();

                            // Retrieve the data from the reader and populate the 'service' object
                            service.ServiceName = reader["Name"].ToString();
                            service.ServiceID = reader["ServiceID"].ToString();
                            service.ServiceDuration = reader["Duration"].ToString();
                            service.ServicePrice = reader["Price"].ToString();
                            service.ServiceCategory = reader["Category"].ToString();

                            ServicesUserControl servicesusercontrol = new ServicesUserControl(this);
                            servicesusercontrol.SetServicesData(service);
                            servicesusercontrol.ServiceUserControl_Clicked += ServiceUserControl_Clicked;
                            servicesusercontrol.RecServicePriceTextBox_Clicked += ServiceUserControl_Clicked;
                            servicesusercontrol.RecServiceDurationTextBox_Clicked += ServiceUserControl_Clicked;
                            servicesusercontrol.RecServiceNameTextBox_Clicked += ServiceUserControl_Clicked;

                            RecApptServicesFLP.Controls.Add(servicesusercontrol);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("An error occurred: " + e.Message, "Error");
            }
        }


        private void RecApptSearchServiceTypeBtn_Click(object sender, EventArgs e)
        {
            RecApptSearchServiceTypeText_TextChanged(sender, e);
        }


        public bool walkinproductsearch;
        //ditokayo
        private void RecWalkinSearchProductTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchKeyword = RecWalkinSearchProductTextBox.Text.Trim().ToLower();
            totalItems = 0;
            currentPage = 0;
            currentPagefake = 1;

            if (string.IsNullOrEmpty(searchKeyword))
            {
                RecWalkinProductFlowLayoutPanel.Controls.Clear();
                RecShopProdProductFlowLayoutPanel.Controls.Clear();
                InitializeProducts();
                return;
            }

            using (MySqlConnection connection = new MySqlConnection(mysqlconn))
            {
                connection.Open();


                string countQuery = "SELECT COUNT(*) FROM inventory WHERE ProductType = 'Retail Product' AND ItemName LIKE @searchKeyword";
                MySqlCommand countCommand = new MySqlCommand(countQuery, connection);
                countCommand.Parameters.AddWithValue("@searchKeyword", "%" + searchKeyword + "%");
                totalItems = Convert.ToInt32(countCommand.ExecuteScalar());

                totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

                int offset = currentPage * itemsPerPage;
                ProductPageLbl.Text = $"{currentPagefake} / {totalPages}";
                WalkinProductPageLbl.Text = $"{currentPagefake} / {totalPages}";

                string query = $@"SELECT ItemID, ItemName, ItemStock, ItemPrice, ItemStatus, ProductPicture 
                  FROM inventory 
                  WHERE ProductType = 'Retail Product' AND ItemName LIKE @searchKeyword 
                  LIMIT {offset}, {itemsPerPage}";


                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@searchKeyword", "%" + searchKeyword + "%");
                MySqlDataReader reader = command.ExecuteReader();
                Size userControlSize = new Size(295, 275);

                RecWalkinProductFlowLayoutPanel.Controls.Clear();
                if (totalItems == 0)
                {
                    WalkinProductNextBtn.Enabled = false;
                    WalkinProductPreviousBtn.Enabled = false;
                    WalkinProductPageLbl.Text = $"0/0";
                }
                else
                {
                    WalkinProductNextBtn.Enabled = true;
                    WalkinProductPreviousBtn.Enabled = true;
                }

                while (reader.Read())
                {
                    string itemID = reader["ItemID"].ToString();
                    string itemName = reader["ItemName"].ToString();
                    string itemStock = reader["ItemStock"].ToString();
                    string itemPrice = reader["ItemPrice"].ToString();
                    string itemStatus = reader["ItemStatus"].ToString();
                    byte[] productPicture = (byte[])reader["ProductPicture"];

                    ProductUserControl recwalkinproductusercontrol = new ProductUserControl();

                    // Set the properties of recwalkinproductusercontrol
                    recwalkinproductusercontrol.ProductItemIDTextBox.Text = itemID;
                    recwalkinproductusercontrol.ProductNameTextBox.Text = itemName;
                    recwalkinproductusercontrol.ProductStockTextBox.Text = itemStock;
                    recwalkinproductusercontrol.ProductPriceTextBox.Text = itemPrice;
                    recwalkinproductusercontrol.ProductStatusTextBox.Text = itemStatus;

                    if (itemStatus == "Low Stock")
                    {
                        recwalkinproductusercontrol.ProductOutOfStockPictureBox.Visible = true;
                        recwalkinproductusercontrol.Enabled = false;
                    }
                    else
                    {
                        recwalkinproductusercontrol.ProductOutOfStockPictureBox.Visible = false;
                        recwalkinproductusercontrol.Enabled = true;
                    }

                    if (productPicture != null && productPicture.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream(productPicture))
                        {
                            System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
                            recwalkinproductusercontrol.ProductPicturePictureBox.Image = image;
                        }
                    }
                    else
                    {
                        recwalkinproductusercontrol.ProductPicturePictureBox.Image = null;
                    }

                    foreach (System.Windows.Forms.Control control in recwalkinproductusercontrol.Controls)
                    {
                        control.Click += RecWalkinProductControlElement_Click;
                    }

                    recwalkinproductusercontrol.Click += RecWalkinProductUserControl_Click;

                    RecWalkinProductFlowLayoutPanel.Controls.Add(recwalkinproductusercontrol);
                }

                reader.Close();
            }
        }

        private void RecWalkinSearchProductNextPrevious()
        {
            string searchKeyword = RecWalkinSearchProductTextBox.Text.Trim().ToLower();
            totalItems = 0;

            if (string.IsNullOrEmpty(searchKeyword))
            {
                RecWalkinProductFlowLayoutPanel.Controls.Clear();
                RecShopProdProductFlowLayoutPanel.Controls.Clear();
                InitializeProducts();
                return;
            }

            using (MySqlConnection connection = new MySqlConnection(mysqlconn))
            {
                connection.Open();


                string countQuery = "SELECT COUNT(*) FROM inventory WHERE ProductType = 'Retail Product' AND ItemName LIKE @searchKeyword";
                MySqlCommand countCommand = new MySqlCommand(countQuery, connection);
                countCommand.Parameters.AddWithValue("@searchKeyword", "%" + searchKeyword + "%");
                totalItems = Convert.ToInt32(countCommand.ExecuteScalar());

                totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

                int offset = currentPage * itemsPerPage;
                ProductPageLbl.Text = $"{currentPagefake} / {totalPages}";
                WalkinProductPageLbl.Text = $"{currentPagefake} / {totalPages}";

                string query = $@"SELECT ItemID, ItemName, ItemStock, ItemPrice, ItemStatus, ProductPicture 
                  FROM inventory 
                  WHERE ProductType = 'Retail Product' AND ItemName LIKE @searchKeyword 
                  LIMIT {offset}, {itemsPerPage}";


                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@searchKeyword", "%" + searchKeyword + "%");
                MySqlDataReader reader = command.ExecuteReader();
                Size userControlSize = new Size(295, 275);

                RecWalkinProductFlowLayoutPanel.Controls.Clear();
                if (totalItems == 0)
                {
                    WalkinProductNextBtn.Enabled = false;
                    WalkinProductPreviousBtn.Enabled = false;
                    WalkinProductPageLbl.Text = $"0/0";
                }
                else
                {
                    WalkinProductNextBtn.Enabled = true;
                    WalkinProductPreviousBtn.Enabled = true;
                }

                while (reader.Read())
                {
                    string itemID = reader["ItemID"].ToString();
                    string itemName = reader["ItemName"].ToString();
                    string itemStock = reader["ItemStock"].ToString();
                    string itemPrice = reader["ItemPrice"].ToString();
                    string itemStatus = reader["ItemStatus"].ToString();
                    byte[] productPicture = (byte[])reader["ProductPicture"];

                    ProductUserControl recwalkinproductusercontrol = new ProductUserControl();

                    // Set the properties of recwalkinproductusercontrol
                    recwalkinproductusercontrol.ProductItemIDTextBox.Text = itemID;
                    recwalkinproductusercontrol.ProductNameTextBox.Text = itemName;
                    recwalkinproductusercontrol.ProductStockTextBox.Text = itemStock;
                    recwalkinproductusercontrol.ProductPriceTextBox.Text = itemPrice;
                    recwalkinproductusercontrol.ProductStatusTextBox.Text = itemStatus;

                    if (itemStatus == "Low Stock")
                    {
                        recwalkinproductusercontrol.ProductOutOfStockPictureBox.Visible = true;
                        recwalkinproductusercontrol.Enabled = false;
                    }
                    else
                    {
                        recwalkinproductusercontrol.ProductOutOfStockPictureBox.Visible = false;
                        recwalkinproductusercontrol.Enabled = true;
                    }

                    if (productPicture != null && productPicture.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream(productPicture))
                        {
                            System.Drawing.Image image = System.Drawing.Image.FromStream(ms);
                            recwalkinproductusercontrol.ProductPicturePictureBox.Image = image;
                        }
                    }
                    else
                    {
                        recwalkinproductusercontrol.ProductPicturePictureBox.Image = null;
                    }

                    foreach (System.Windows.Forms.Control control in recwalkinproductusercontrol.Controls)
                    {
                        control.Click += RecWalkinProductControlElement_Click;
                    }

                    recwalkinproductusercontrol.Click += RecWalkinProductUserControl_Click;

                    RecWalkinProductFlowLayoutPanel.Controls.Add(recwalkinproductusercontrol);
                }

                reader.Close();
            }
        }
        //ditopala
        private void RecSearchProductTextBox_TextChanged(object sender, EventArgs e)
        {
            string searchKeyword = RecSearchProductTextBox.Text.Trim().ToLower();
            totalItems = 0;
            currentPage = 0;
            currentPagefake = 1;

            if (string.IsNullOrEmpty(searchKeyword))
            {
                RecWalkinProductFlowLayoutPanel.Controls.Clear();
                RecShopProdProductFlowLayoutPanel.Controls.Clear();
                InitializeProducts();
                return;
            }

            using (MySqlConnection connection = new MySqlConnection(mysqlconn))
            {
                connection.Open();

                string countQuery = "SELECT COUNT(*) FROM inventory WHERE ProductType = 'Retail Product' AND ItemName LIKE @searchKeyword";
                MySqlCommand countCommand = new MySqlCommand(countQuery, connection);
                countCommand.Parameters.AddWithValue("@searchKeyword", "%" + searchKeyword + "%");
                totalItems = Convert.ToInt32(countCommand.ExecuteScalar());

                totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

                int offset = currentPage * itemsPerPage;
                ProductPageLbl.Text = $"{currentPagefake} / {totalPages}";
                WalkinProductPageLbl.Text = $"{currentPagefake} / {totalPages}";

                string query = $@"SELECT ItemID, ItemName, ItemStock, ItemPrice, ItemStatus, ProductPicture 
                  FROM inventory 
                  WHERE ProductType = 'Retail Product' AND ItemName LIKE @searchKeyword 
                  LIMIT {offset}, {itemsPerPage}";


                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@searchKeyword", "%" + searchKeyword + "%");
                MySqlDataReader reader = command.ExecuteReader();
                Size userControlSize = new Size(419, 90);

                RecShopProdProductFlowLayoutPanel.Controls.Clear();

                if (totalItems == 0)
                {
                    ProductNextBtn.Enabled = false;
                    ProductPreviousBtn.Enabled = false;
                    ProductPageLbl.Text = $"0/0";
                }
                else
                {
                    ProductNextBtn.Enabled = true;
                    ProductPreviousBtn.Enabled = true;
                }

                while (reader.Read())
                {
                    string itemID = reader["ItemID"].ToString();
                    string itemName = reader["ItemName"].ToString();
                    string itemStock = reader["ItemStock"].ToString();
                    string itemPrice = reader["ItemPrice"].ToString();
                    string itemStatus = reader["ItemStatus"].ToString();
                    byte[] productPicture = (byte[])reader["ProductPicture"];

                    ProductUserControl recshopproductusercontrol = new ProductUserControl();



                    // Set the properties of recshopproductusercontrol
                    recshopproductusercontrol.Size = userControlSize;
                    recshopproductusercontrol.ProductNameTextBox.Size = new Size(235, 33);
                    recshopproductusercontrol.ProductPriceTextBox.Size = new Size(90, 27);
                    recshopproductusercontrol.ProductPicturePictureBox.Size = new Size(72, 72);
                    recshopproductusercontrol.ProductNameTextBox.Location = new Point(90, 29);
                    recshopproductusercontrol.ProductPriceTextBox.Location = new Point(318, 32);
                    recshopproductusercontrol.PhpSignLbl.Location = new Point(280, 31);
                    recshopproductusercontrol.ProductPicturePictureBox.Location = new Point(16, 9);
                    //Border
                    recshopproductusercontrol.LeftBorder.Size = new Size(5, 100);
                    recshopproductusercontrol.LeftBorder.Location = new Point(-3, 0);
                    recshopproductusercontrol.TopBorder.Size = new Size(425, 5);
                    recshopproductusercontrol.TopBorder.Location = new Point(0, -3);
                    recshopproductusercontrol.RightBorder.Size = new Size(5, 100);
                    recshopproductusercontrol.RightBorder.Location = new Point(417, 0);
                    recshopproductusercontrol.DownBorder.Size = new Size(425, 10);
                    recshopproductusercontrol.DownBorder.Location = new Point(0, 88);


                    recshopproductusercontrol.ProductItemIDTextBox.Text = itemID;
                    recshopproductusercontrol.ProductNameTextBox.Text = itemName;
                    recshopproductusercontrol.ProductStockTextBox.Text = itemStock;
                    recshopproductusercontrol.ProductPriceTextBox.Text = itemPrice;
                    recshopproductusercontrol.ProductStatusTextBox.Text = itemStatus;

                    if (itemStatus == "Low Stock")
                    {
                        recshopproductusercontrol.ProductOutOfStockPictureBox.Visible = true;
                        recshopproductusercontrol.Enabled = false;
                    }
                    else
                    {
                        recshopproductusercontrol.ProductOutOfStockPictureBox.Visible = false;
                        recshopproductusercontrol.Enabled = true;
                    }

                    if (productPicture != null && productPicture.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream(productPicture))
                        {
                            System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms);
                            recshopproductusercontrol.ProductPicturePictureBox.Image = image1;
                        }
                    }
                    else
                    {
                        recshopproductusercontrol.ProductPicturePictureBox.Image = null;
                    }

                    foreach (System.Windows.Forms.Control control1 in recshopproductusercontrol.Controls)
                    {
                        control1.Click += RecShopProductControlElement_Click;
                    }

                    recshopproductusercontrol.Click += RecShopProdProductUserControl_Click;

                    RecShopProdProductFlowLayoutPanel.Controls.Add(recshopproductusercontrol);
                }

                reader.Close();
            }
        }


        private void RecSearchProductNextPrevious()
        {
            string searchKeyword = RecSearchProductTextBox.Text.Trim().ToLower();
            totalItems = 0;

            if (string.IsNullOrEmpty(searchKeyword))
            {
                RecWalkinProductFlowLayoutPanel.Controls.Clear();
                RecShopProdProductFlowLayoutPanel.Controls.Clear();
                InitializeProducts();
                return;
            }

            using (MySqlConnection connection = new MySqlConnection(mysqlconn))
            {
                connection.Open();

                string countQuery = "SELECT COUNT(*) FROM inventory WHERE ProductType = 'Retail Product' AND ItemName LIKE @searchKeyword";
                MySqlCommand countCommand = new MySqlCommand(countQuery, connection);
                countCommand.Parameters.AddWithValue("@searchKeyword", "%" + searchKeyword + "%");
                totalItems = Convert.ToInt32(countCommand.ExecuteScalar());

                totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);

                int offset = currentPage * itemsPerPage;
                ProductPageLbl.Text = $"{currentPagefake} / {totalPages}";
                WalkinProductPageLbl.Text = $"{currentPagefake} / {totalPages}";

                string query = $@"SELECT ItemID, ItemName, ItemStock, ItemPrice, ItemStatus, ProductPicture 
                  FROM inventory 
                  WHERE ProductType = 'Retail Product' AND ItemName LIKE @searchKeyword 
                  LIMIT {offset}, {itemsPerPage}";


                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@searchKeyword", "%" + searchKeyword + "%");
                MySqlDataReader reader = command.ExecuteReader();
                Size userControlSize = new Size(419, 90);

                RecShopProdProductFlowLayoutPanel.Controls.Clear();

                if (totalItems == 0)
                {
                    ProductNextBtn.Enabled = false;
                    ProductPreviousBtn.Enabled = false;
                    ProductPageLbl.Text = $"0/0";
                }
                else
                {
                    ProductNextBtn.Enabled = true;
                    ProductPreviousBtn.Enabled = true;
                }

                while (reader.Read())
                {
                    string itemID = reader["ItemID"].ToString();
                    string itemName = reader["ItemName"].ToString();
                    string itemStock = reader["ItemStock"].ToString();
                    string itemPrice = reader["ItemPrice"].ToString();
                    string itemStatus = reader["ItemStatus"].ToString();
                    byte[] productPicture = (byte[])reader["ProductPicture"];

                    ProductUserControl recshopproductusercontrol = new ProductUserControl();



                    // Set the properties of recshopproductusercontrol
                    recshopproductusercontrol.Size = userControlSize;
                    recshopproductusercontrol.ProductNameTextBox.Size = new Size(235, 33);
                    recshopproductusercontrol.ProductPriceTextBox.Size = new Size(90, 27);
                    recshopproductusercontrol.ProductPicturePictureBox.Size = new Size(72, 72);
                    recshopproductusercontrol.ProductNameTextBox.Location = new Point(90, 29);
                    recshopproductusercontrol.ProductPriceTextBox.Location = new Point(318, 32);
                    recshopproductusercontrol.PhpSignLbl.Location = new Point(280, 31);
                    recshopproductusercontrol.ProductPicturePictureBox.Location = new Point(16, 9);
                    //Border
                    recshopproductusercontrol.LeftBorder.Size = new Size(5, 100);
                    recshopproductusercontrol.LeftBorder.Location = new Point(-3, 0);
                    recshopproductusercontrol.TopBorder.Size = new Size(425, 5);
                    recshopproductusercontrol.TopBorder.Location = new Point(0, -3);
                    recshopproductusercontrol.RightBorder.Size = new Size(5, 100);
                    recshopproductusercontrol.RightBorder.Location = new Point(417, 0);
                    recshopproductusercontrol.DownBorder.Size = new Size(425, 10);
                    recshopproductusercontrol.DownBorder.Location = new Point(0, 88);


                    recshopproductusercontrol.ProductItemIDTextBox.Text = itemID;
                    recshopproductusercontrol.ProductNameTextBox.Text = itemName;
                    recshopproductusercontrol.ProductStockTextBox.Text = itemStock;
                    recshopproductusercontrol.ProductPriceTextBox.Text = itemPrice;
                    recshopproductusercontrol.ProductStatusTextBox.Text = itemStatus;

                    if (itemStatus == "Low Stock")
                    {
                        recshopproductusercontrol.ProductOutOfStockPictureBox.Visible = true;
                        recshopproductusercontrol.Enabled = false;
                    }
                    else
                    {
                        recshopproductusercontrol.ProductOutOfStockPictureBox.Visible = false;
                        recshopproductusercontrol.Enabled = true;
                    }

                    if (productPicture != null && productPicture.Length > 0)
                    {
                        using (MemoryStream ms = new MemoryStream(productPicture))
                        {
                            System.Drawing.Image image1 = System.Drawing.Image.FromStream(ms);
                            recshopproductusercontrol.ProductPicturePictureBox.Image = image1;
                        }
                    }
                    else
                    {
                        recshopproductusercontrol.ProductPicturePictureBox.Image = null;
                    }

                    foreach (System.Windows.Forms.Control control1 in recshopproductusercontrol.Controls)
                    {
                        control1.Click += RecShopProductControlElement_Click;
                    }

                    recshopproductusercontrol.Click += RecShopProdProductUserControl_Click;

                    RecShopProdProductFlowLayoutPanel.Controls.Add(recshopproductusercontrol);
                }

                reader.Close();
            }
        }

        private void LoginPassText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (LoginPassText.Text.Length >= 100 && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
            if (e.KeyChar == ' ' && string.IsNullOrEmpty(LoginPassText.Text))
            {
                e.Handled = true;
            }
        }

        private void RecSearchProductTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (RecSearchProductTextBox.Text.Length >= 50 && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
            if (e.KeyChar == ' ' && string.IsNullOrEmpty(RecSearchProductTextBox.Text))
            {
                e.Handled = true;
            }
        }

        private void RecShopProdClientNameText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && e.KeyChar != '-' && e.KeyChar != '\'' && e.KeyChar != ' ' && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
            //if (RecShopProdClientNameText.Text.Length >= 100 && e.KeyChar != '\b')
            //{
            //    e.Handled = true;
            //}
            //if (e.KeyChar == ' ' && string.IsNullOrEmpty(RecShopProdClientNameText.Text))
            //{
            //    e.Handled = true;
            //}
        }

        private void RecShopProdClientCPNumText_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (!char.IsDigit(e.KeyChar) && e.KeyChar != '+' && e.KeyChar != '\b' || (RecShopProdClientCPNumText.Text.Contains("+")
            //    && RecShopProdClientCPNumText.Text.Length >= 13 && e.KeyChar != '\b') || (!RecShopProdClientCPNumText.Text.Contains("+")
            //    && RecShopProdClientCPNumText.Text.Length >= 11 && e.KeyChar != '\b'))
            //{
            //    e.Handled = true;
            //}
        }


        private void RecApptCPNumText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '+' && e.KeyChar != '\b' || (RecApptCPNumText.Text.Contains("+")
                && RecApptCPNumText.Text.Length >= 13 && e.KeyChar != '\b') || (!RecApptCPNumText.Text.Contains("+")
                && RecApptCPNumText.Text.Length >= 11 && e.KeyChar != '\b'))
            {
                e.Handled = true;
            }
        }

        private void RecApptLNameText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (RecApptLNameText.Text.Length >= 100 && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
            if (e.KeyChar == ' ' && string.IsNullOrEmpty(RecApptLNameText.Text))
            {
                e.Handled = true;
            }
        }

        private void RecApptFNameText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (RecApptFNameText.Text.Length >= 100 && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
            if (e.KeyChar == ' ' && string.IsNullOrEmpty(RecApptFNameText.Text))
            {
                e.Handled = true;
            }
        }

        private void RecApptSearchServiceTypeText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (RecApptSearchServiceTypeText.Text.Length >= 100 && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
            if (e.KeyChar == ' ' && string.IsNullOrEmpty(RecApptSearchServiceTypeText.Text))
            {
                e.Handled = true;
            }
        }

        private void RecQueWinSearchText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (RecQueWinSearchText.Text.Length >= 100 && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
            if (e.KeyChar == ' ' && string.IsNullOrEmpty(RecQueWinSearchText.Text))
            {
                e.Handled = true;
            }
        }



        private void RecWalkinCPNumText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '+' && e.KeyChar != '\b' || (RecWalkinCPNumText.Text.Contains("+")
                && RecWalkinCPNumText.Text.Length >= 13 && e.KeyChar != '\b') || (!RecWalkinCPNumText.Text.Contains("+")
                && RecWalkinCPNumText.Text.Length >= 11 && e.KeyChar != '\b'))
            {
                e.Handled = true;
            }
        }

        private void RecWalkinLNameText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (RecWalkinLNameText.Text.Length >= 100 && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
            if (e.KeyChar == ' ' && string.IsNullOrEmpty(RecWalkinLNameText.Text))
            {
                e.Handled = true;
            }
        }

        private void RecWalkinFNameText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (RecWalkinFNameText.Text.Length >= 100 && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
            if (e.KeyChar == ' ' && string.IsNullOrEmpty(RecWalkinFNameText.Text))
            {
                e.Handled = true;
            }
        }

        private void RecWalkinSearchServiceTypeText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (RecWalkinSearchServiceTypeText.Text.Length >= 50 && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
            if (e.KeyChar == ' ' && string.IsNullOrEmpty(RecWalkinSearchServiceTypeText.Text))
            {
                e.Handled = true;
            }
        }

        private void RecWalkinSearchProductTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (RecWalkinSearchProductTextBox.Text.Length >= 50 && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
            if (e.KeyChar == ' ' && string.IsNullOrEmpty(RecWalkinSearchProductTextBox.Text))
            {
                e.Handled = true;
            }
        }

        private void MngrServicesNameText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (MngrServicesNameText.Text.Length >= 100 && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
            if (e.KeyChar == ' ' && string.IsNullOrEmpty(MngrServicesNameText.Text))
            {
                e.Handled = true;
            }
        }

        private void MngrServicesIDNumText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (MngrServicesIDNumText.Text.Length >= 100 && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
            if (e.KeyChar == ' ' && string.IsNullOrEmpty(MngrServicesIDNumText.Text))
            {
                e.Handled = true;
            }
        }

        private void MngrServicesDurationText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (MngrServicesDurationText.Text.Length >= 50 && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
            if (e.KeyChar == ' ' && string.IsNullOrEmpty(MngrServicesDurationText.Text))
            {
                e.Handled = true;
            }
        }

        private void MngrInventoryProductsNameText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (MngrInventoryProductsNameText.Text.Length >= 50 && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
            if (e.KeyChar == ' ' && string.IsNullOrEmpty(MngrInventoryProductsNameText.Text))
            {
                e.Handled = true;
            }
        }

        private void MngrInventoryProductsIDText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (MngrInventoryProductsIDText.Text.Length >= 50 && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
            if (e.KeyChar == ' ' && string.IsNullOrEmpty(MngrInventoryProductsIDText.Text))
            {
                e.Handled = true;
            }
        }

        private void MngrInventoryProductsStockText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (MngrInventoryProductsStockText.Text.Length >= 50 && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
            if (e.KeyChar == ' ' && string.IsNullOrEmpty(MngrInventoryProductsStockText.Text))
            {
                e.Handled = true;
            }
        }

        private void MngrInventoryProductsPriceText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (MngrInventoryProductsPriceText.Text.Length >= 50 && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
            if (e.KeyChar == ' ' && string.IsNullOrEmpty(MngrInventoryProductsPriceText.Text))
            {
                e.Handled = true;
            }
        }

        private void MngrServicesPriceText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (MngrServicesPriceText.Text.Length >= 50 && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
            if (e.KeyChar == ' ' && string.IsNullOrEmpty(MngrServicesPriceText.Text))
            {
                e.Handled = true;
            }
        }

        private void MngrServicesDescriptionText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (MngrServicesDescriptionText.Text.Length >= 200 && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
            if (e.KeyChar == ' ' && string.IsNullOrEmpty(MngrServicesDescriptionText.Text))
            {
                e.Handled = true;
            }
        }

        private void AdminLastNameText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && e.KeyChar != '-' && e.KeyChar != '\'' && e.KeyChar != ' ' && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
            if (AdminLastNameText.Text.Length >= 100 && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
            if (e.KeyChar == ' ' && string.IsNullOrEmpty(AdminLastNameText.Text))
            {
                e.Handled = true;
            }
        }

        private void AdminAgeText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) || AdminAgeText.Text.Length >= 3)
            {
                e.Handled = true;
            }
        }

        private void AdminCPNumText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '+' && e.KeyChar != '\b' || (AdminCPNumText.Text.Contains("+")
                && AdminCPNumText.Text.Length >= 13 && e.KeyChar != '\b') || (!AdminCPNumText.Text.Contains("+")
                && AdminCPNumText.Text.Length >= 11 && e.KeyChar != '\b'))
            {
                e.Handled = true;
            }
        }

        private void AdminFirstNameText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && e.KeyChar != '-' && e.KeyChar != '\'' && e.KeyChar != ' ' && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
            if (AdminFirstNameText.Text.Length >= 100 && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
            if (e.KeyChar == ' ' && string.IsNullOrEmpty(AdminFirstNameText.Text))
            {
                e.Handled = true;
            }
        }

        private void AdminEmailText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (AdminEmailText.Text.Length >= 200 && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
            if (e.KeyChar == ' ' && string.IsNullOrEmpty(AdminEmailText.Text))
            {
                e.Handled = true;
            }
        }

        private void AdminConfirmPassText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (AdminConfirmPassText.Text.Length >= 100 && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
            if (e.KeyChar == ' ' && string.IsNullOrEmpty(AdminConfirmPassText.Text))
            {
                e.Handled = true;
            }
        }

        private void AdminPassText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (AdminPassText.Text.Length >= 200 && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
            if (e.KeyChar == ' ' && string.IsNullOrEmpty(AdminPassText.Text))
            {
                e.Handled = true;
            }
        }

        private void RecTransBtn_Click(object sender, EventArgs e)
        {
            //InitialWalkinTransColor();
            RecTransTimer.Start();
        }

        private void RecQueBtn_Click(object sender, EventArgs e)
        {
            //RecQueStartColor();
            RecQueTimer.Start();

            RecQueStartStaffFLP.Controls.Clear();
            RecQueStartInventoryDGV.Rows.Clear();
            InitializeEmployeeCategory();
            InitializeStaffUserControl();
            InitializeMainInventory();
            InitializeInSessionCustomers();

        }

        private void RecQueBtnResetColor()
        {
            RecQueStartBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            RecQueStartBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            RecQueStartBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            RecQueWinBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            RecQueWinBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            RecQueWinBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
        }

        private void RecQueStartBtn_Click(object sender, EventArgs e)
        {
            RecQueStartColor();
        }

        private void RecQueStartColor()
        {
            Transaction.PanelShow(RecQueStartPanel);

            RecQueStartBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            RecQueStartBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            RecQueStartBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));

            RecQueWinBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            RecQueWinBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            RecQueWinBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            RecTransBtnResetColor();
        }

        private void RecQueWinColor()
        {
            Transaction.PanelShow(RecQueWinPanel);

            RecQueWinBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            RecQueWinBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            RecQueWinBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));

            RecQueStartBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            RecQueStartBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            RecQueStartBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            RecTransBtnResetColor();
        }


        

        public string CurrentStaffID;

        public string selectedmembercategory;
        public string selectedstaffemployeeid;


        public void RefreshFlowLayoutPanel()
        {
            RecQueueStartPanel.Controls.Clear();
            InitializeGeneralPendingCustomersForStaff();
        }

        private void AvailableCustomersUserControl_StartServiceButtonClicked(object sender, EventArgs e)
        {
            InSessionUserControl insessioncustomerusercontrol = (InSessionUserControl)sender;

            if (insessioncustomerusercontrol != null)
            {
                insessioncustomerusercontrol.StartTimer();
            }

        }


        private void AvailableCustomerUserControl_CancelServiceButtonClicked(object sender, EventArgs e)
        {
            QueueUserControl clickedUserControl = (QueueUserControl)sender;
        }


        public class GeneralPendingCustomers
        {
            public string TransactionNumber { get; set; }
            public string ClientName { get; set; }
            public string ServiceID { get; set; }
            public string ServiceName { get; set; }
            public string ServiceStatus { get; set; }
            public string QueType { get; set; }
            public string QueNumber { get; set; }
        }

        private List<GeneralPendingCustomers> RetrieveGeneralPendingCustomersFromDB()
        {
            string staffID = selectedstaffemployeeid;
            DateTime currentDate = DateTime.Today;
            string datetoday = currentDate.ToString("MM-dd-yyyy dddd");
            List<GeneralPendingCustomers> result = new List<GeneralPendingCustomers>();

            using (MySqlConnection connection = new MySqlConnection(mysqlconn))
            {
                try
                {
                    connection.Open();

                    string generalpendingcustomersquery = $@"SELECT sh.TransactionNumber, sh.ClientName, sh.ServiceStatus, sh.SelectedService, sh.ServiceID, sh.QueNumber, sh.QueType 
                                                 FROM servicehistory sh 
                                                 LEFT JOIN appointment app ON sh.TransactionNumber = app.TransactionNumber 
                                                 WHERE sh.ServiceStatus = 'Pending'
                                                 AND sh.ServiceCategory = @membercategory 
                                                 AND (sh.QueType = 'Anyone' OR sh.QueType = 'Senior-Anyone' OR sh.QueType = 'Senior-Anyone-Priority' OR sh.PreferredStaff = @preferredstaff)
                                                 AND (app.ServiceStatus IS NULL OR app.ServiceStatus = 'Pending')
                                                 AND (app.AppointmentStatus IS NULL OR app.AppointmentStatus = 'Confirmed')
                                                 AND sh.AppointmentDate = @datetoday";

                    MySqlCommand command = new MySqlCommand(generalpendingcustomersquery, connection);
                    command.Parameters.AddWithValue("@membercategory", selectedmembercategory);
                    command.Parameters.AddWithValue("@preferredstaff", staffID);
                    command.Parameters.AddWithValue("@datetoday", datetoday);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            GeneralPendingCustomers generalpendingcustomers = new GeneralPendingCustomers
                            {
                                TransactionNumber = reader["TransactionNumber"] as string,
                                ClientName = reader["ClientName"] as string,
                                ServiceStatus = reader["ServiceStatus"] as string,
                                ServiceName = reader["SelectedService"] as string,
                                ServiceID = reader["ServiceID"] as string,
                                QueType = reader["QueType"] as string,
                                QueNumber = reader["QueNumber"] as string
                            };

                            result.Add(generalpendingcustomers);
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

        public int seniorcount;
        public bool ThereIsSenior = false;
        public int apptpriocount;
        public bool ThereisPriority = false;
        public void InitializeGeneralPendingCustomersForStaff()
        {
            List<GeneralPendingCustomers> generalpendingcustomers = RetrieveGeneralPendingCustomersFromDB();
            seniorcount = 0;
            apptpriocount = 0;
            ThereisPriority = false;
            ThereIsSenior = false;

            if (generalpendingcustomers.Count == 0)
            {
                NoCustomerInQueueUserControl nocustomerusercontrol = new NoCustomerInQueueUserControl();
                RecQueueStartPanel.Controls.Add(nocustomerusercontrol);
                NextCustomerNumLbl.Text = "No customer in queue";
            }

            int smallestgenqueue = int.MaxValue;
            int smallestapptqueue = int.MaxValue;
            int smallestqueuesenior = int.MaxValue;


            foreach (GeneralPendingCustomers customer in generalpendingcustomers)
            {
                QueueUserControl availablecustomersusercontrol = new QueueUserControl(this);
                availablecustomersusercontrol.AvailableGeneralCustomerSetData(customer);
                // usercontrol click event add
                availablecustomersusercontrol.QueueUserControl_Clicked += AvailableCustomersUserControl_StartServiceButtonClicked;
                // add event on elements
                availablecustomersusercontrol.StaffQueNumberTextBox_Clicked += AvailableCustomersUserControl_StartServiceButtonClicked;
                availablecustomersusercontrol.ExpandUserControlButton_Clicked += AvailableCustomersUserControl_ExpandCollapseButtonClicked;
                availablecustomersusercontrol.StaffCustomerNameTextBox_Clicked += AvailableCustomersUserControl_StartServiceButtonClicked;
                availablecustomersusercontrol.StaffElapsedTimeTextBox_Clicked += AvailableCustomersUserControl_StartServiceButtonClicked;
                availablecustomersusercontrol.StaffTransactionIDTextBox_Clicked += AvailableCustomersUserControl_StartServiceButtonClicked;

                availablecustomersusercontrol.StaffCancelServiceBtnClicked += AvailableCustomerUserControl_CancelServiceButtonClicked;
                RecQueueStartPanel.Controls.Add(availablecustomersusercontrol);
                availablecustomersusercontrol.CurrentStaffID = selectedstaffemployeeid;

                string generalqueuenumber = availablecustomersusercontrol.StaffQueNumberTextBox.Text;
                if (int.TryParse(generalqueuenumber, out int genqueueint))
                {
                    if (genqueueint < smallestgenqueue)
                    {
                        smallestgenqueue = genqueueint;
                    }

                }

                string apptprioquenumber = availablecustomersusercontrol.StaffQueNumberTextBox.Text;
                if (int.TryParse(apptprioquenumber, out int apptprioqueueint))
                {
                    if (customer.QueType == "Anyone-Priority" || customer.QueType == "Preferred-Priority")
                    {
                        if (apptprioqueueint < smallestapptqueue)
                        {
                            smallestapptqueue = apptprioqueueint;
                        }
                        ThereisPriority = true;
                        apptpriocount++;
                    }

                }

                string seniorqueuenumber = availablecustomersusercontrol.StaffQueNumberTextBox.Text;
                if (int.TryParse(seniorqueuenumber, out int seniorquenumberint))
                {
                    if (customer.QueType == "Senior-Anyone-Priority" || customer.QueType == "Senior-Anyone"
                        || customer.QueType == "Senior-Preferred-Priority" || customer.QueType == "Senior-Preferred")
                    {
                        if (seniorquenumberint < smallestqueuesenior)
                        {
                            smallestqueuesenior = seniorquenumberint;
                        }
                        ThereIsSenior = true;
                        seniorcount++;
                    }

                }


            }


            if (!ThereIsSenior && seniorcount == 0)
            {
                ThereIsSenior = false;
            }
            if (!ThereisPriority && apptpriocount == 0)
            {
                ThereisPriority = false;
            }
            UpdateStartServiceButtonStatusPriority(generalpendingcustomers, smallestqueuesenior, smallestapptqueue, smallestgenqueue);


        }

        private void UpdateStartServiceButtonStatusPriority(List<GeneralPendingCustomers> generalpendingcustomers, int smallestqueuesenior, int smallestapptqueue, int smallestgenqueue)
        {
            bool hasAnyoneSPriorityOrPreferredSPriority = generalpendingcustomers
                 .Any(customer => customer.QueType == "Senior-Anyone-Priority" || customer.QueType == "Senior-Anyone"
                 || customer.QueType == "Senior-Preferred-Priority" || customer.QueType == "Senior-Preferred");

            if (ThereIsSenior == true)
            {

                foreach (System.Windows.Forms.Control control in RecQueueStartPanel.Controls)
                {
                    if (control is QueueUserControl userControl)
                    {
                        int currentseniorquenum;
                        int.TryParse(userControl.StaffQueNumberTextBox.Text, out currentseniorquenum);
                        if (userControl.StaffQueTypeTextBox.Text == "Senior-Anyone-Priority" || userControl.StaffQueTypeTextBox.Text == "Senior-Anyone" ||
                            userControl.StaffQueTypeTextBox.Text == "Senior-Preferred-Priority" || userControl.StaffQueTypeTextBox.Text == "Preferred-Anyone")
                        {
                            userControl.Enabled = (currentseniorquenum == smallestqueuesenior);
                        }
                        else
                        {
                            userControl.Enabled = false;
                        }
                    }
                }
            }
            else if (ThereIsSenior == false && ThereisPriority == true)
            {
                foreach (System.Windows.Forms.Control control in RecQueueStartPanel.Controls)
                {
                    if (control is QueueUserControl userControl)
                    {
                        int currentapptqueuenum;
                        int.TryParse(userControl.StaffQueNumberTextBox.Text, out currentapptqueuenum);
                        if (userControl.StaffQueTypeTextBox.Text == "Anyone-Priority" || userControl.StaffQueTypeTextBox.Text == "Preferred-Priority")
                        {
                            userControl.Enabled = (currentapptqueuenum == smallestapptqueue);
                        }
                        else
                        {
                            userControl.Enabled = false;
                        }
                    }
                }
            }
            else
            {
                foreach (System.Windows.Forms.Control control in RecQueueStartPanel.Controls)
                {
                    if (control is QueueUserControl userControl)
                    {
                        int currentgeneralque;
                        int.TryParse(userControl.StaffQueNumberTextBox.Text, out currentgeneralque);
                        userControl.Enabled = (currentgeneralque == smallestgenqueue);

                    }
                }
            }

            foreach (System.Windows.Forms.Control control in RecQueueStartPanel.Controls)
            {
                if (control is QueueUserControl userControl && userControl.Enabled)
                {
                    RecQueueStartPanel.Controls.SetChildIndex(userControl, 0);
                    NextCustomerNumLbl.Text = userControl.StaffQueNumberTextBox.Text;
                    break;
                }
            }
        }

        private void AvailableCustomersUserControl_ExpandCollapseButtonClicked(object sender, EventArgs e)
        {
            QueueUserControl availablecustomersusercontrol = (QueueUserControl)sender;

            if (availablecustomersusercontrol != null)
            {
                if (!availablecustomersusercontrol.Viewing)
                {
                    availablecustomersusercontrol.Size = new System.Drawing.Size(457, 29);
                }
                else
                {
                    availablecustomersusercontrol.Size = new System.Drawing.Size(457, 186);
                }
            }
        }

        public void InitializeMainInventory()
        {
            string query = "SELECT ItemID, ProductCategory, ItemName, ItemStock, ItemStatus FROM inventory " +
                           "WHERE ProductType = 'Service Product'";

            using (MySqlConnection connection = new MySqlConnection(mysqlconn))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        RecQueStartInventoryDGV.Rows.Clear(); // Clear existing rows in the DataGridView

                        foreach (DataRow row in dataTable.Rows)
                        {
                            string itemid = row["ItemID"].ToString();
                            string productcategory = row["ProductCategory"].ToString();
                            string itemname = row["ItemName"].ToString();
                            string itemstock = row["ItemStock"].ToString();
                            string itemstatus = row["ItemStatus"].ToString();

                            // Add a new row to the DataGridView
                            int rowIndex = RecQueStartInventoryDGV.Rows.Add();

                            // Set the values of cells in the DataGridView
                            RecQueStartInventoryDGV.Rows[rowIndex].Cells["RecStaffItemID"].Value = itemid;
                            RecQueStartInventoryDGV.Rows[rowIndex].Cells["RecStaffProductCategory"].Value = productcategory;
                            RecQueStartInventoryDGV.Rows[rowIndex].Cells["RecStaffItemName"].Value = itemname;
                            RecQueStartInventoryDGV.Rows[rowIndex].Cells["RecStaffItemStock"].Value = itemstock;
                            RecQueStartInventoryDGV.Rows[rowIndex].Cells["RecStaffItemStatus"].Value = itemstatus;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred: " + ex.Message);
                    }
                }
            }
        }

        public void CheckItemStockPersonalStatus(string ItemID, string staffID)
        {
            using (MySqlConnection connection = new MySqlConnection(mysqlconn))
            {
                connection.Open();

                string selectQuery = "SELECT ItemStock, ItemStatus, ItemName FROM inventory " +
                                     "WHERE ItemID = @ItemID";

                MySqlCommand selectCommand = new MySqlCommand(selectQuery, connection);
                selectCommand.Parameters.AddWithValue("@staffID", staffID);
                selectCommand.Parameters.AddWithValue("@ItemID", ItemID);

                using (MySqlDataReader reader = selectCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int itemStock = int.Parse(reader["ItemStock"].ToString());
                        string itemStatus = reader["ItemStatus"].ToString();
                        string itemName = reader["ItemName"].ToString();

                        reader.Close(); // Close the data reader before executing the update query

                        if (itemStock >= 40 && itemStatus == "High Stock")
                        {
                            // Don't update
                        }
                        else if (itemStock >= 40 && itemStatus == "Low Stock")
                        {
                            string updateQuery = "UPDATE inventory " +
                                                 "SET ItemStatus = 'High Stock' " +
                                                 "WHERE  ItemID = @ItemID";

                            MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection);
                            updateCommand.Parameters.AddWithValue("@staffID", staffID);
                            updateCommand.Parameters.AddWithValue("@ItemID", ItemID);
                            updateCommand.ExecuteNonQuery();
                        }
                        else if (itemStock < 8 && itemStatus == "High Stock")
                        {
                            string updateQuery = "UPDATE inventory " +
                                                 "SET ItemStatus = 'Low Stock' " +
                                                 "WHERE ItemID = @ItemID";

                            MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection);
                            updateCommand.Parameters.AddWithValue("@staffID", staffID);
                            updateCommand.Parameters.AddWithValue("@ItemID", ItemID);
                            updateCommand.ExecuteNonQuery();

                            MessageBox.Show($"{itemName} is at Low Stock");
                        }
                        else if (itemStatus == "Low Stock")
                        {
                            // Don't update
                        }
                    }
                    else
                    {
                        MessageBox.Show("Item not found in staff inventory");
                    }
                }
            }
        }

        private void AvailableCustomersUserControl_EndServiceButtonClicked(object sender, EventArgs e)
        {
            InSessionUserControl clickedUserControl = (InSessionUserControl)sender;
            TimeSpan elapsedTime = clickedUserControl.GetElapsedTime();

        }

        private void InSessionCustomersUserControl_ExpandCollapseButtonClicked(object sender, EventArgs e)
        {
            InSessionUserControl availablecustomersusercontrol = (InSessionUserControl)sender;

            if (availablecustomersusercontrol != null)
            {
                if (!availablecustomersusercontrol.Viewing)
                {
                    availablecustomersusercontrol.Size = new System.Drawing.Size(368, 67);
                }
                else
                {
                    availablecustomersusercontrol.Size = new System.Drawing.Size(368, 249);
                }
            }
        }

        public class InSessionCustomers
        {
            public string TransactionNumber { get; set; }
            public string ClientName { get; set; }
            public string ServiceID { get; set; }
            public string ServiceName { get; set; }
            public string ServiceStatus { get; set; }
            public string QueType { get; set; }
            public string QueNumber { get; set; }
            public string AttendingStaff { get; set; }
        }
        // Define a dictionary to store the existing user controls
        Dictionary<string, InSessionUserControl> existingUserControls = new Dictionary<string, InSessionUserControl>();

        public void InitializeInSessionCustomers()
        {
            List<InSessionCustomers> insessioncustomers = RetrieveInSessionCustomersFromDB();

            foreach (InSessionCustomers customer in insessioncustomers)
            {
                // Generate a unique identifier for the user control
                string controlID = GenerateControlID(customer);

                // Check if the user control already exists in the FlowLayoutPanel or in the existingUserControls dictionary
                bool userControlExists = RecQueStartCurrentCustPanel.Controls
                    .OfType<InSessionUserControl>()
                    .Any(control => control.ControlID == controlID) || existingUserControls.ContainsKey(controlID);

                if (!userControlExists)
                {
                    InSessionUserControl availableinsessionusercontrol = new InSessionUserControl(this);
                    availableinsessionusercontrol.ControlID = controlID;
                    availableinsessionusercontrol.AvailableCustomerSetData(customer);
                    // Add event handlers
                    availableinsessionusercontrol.QueueUserControlEnd_Clicked += AvailableCustomersUserControl_EndServiceButtonClicked;
                    availableinsessionusercontrol.StaffQueNumberTextBoxEnd_Clicked += AvailableCustomersUserControl_EndServiceButtonClicked;
                    availableinsessionusercontrol.StaffCustomerNameTextBoxEnd_Clicked += AvailableCustomersUserControl_EndServiceButtonClicked;
                    availableinsessionusercontrol.StaffElapsedTimeTextBoxEnd_Clicked += AvailableCustomersUserControl_EndServiceButtonClicked;
                    availableinsessionusercontrol.StaffTransactionIDTextBoxEnd_Clicked += AvailableCustomersUserControl_EndServiceButtonClicked;
                    availableinsessionusercontrol.StaffCancelServiceBtnClicked += AvailableCustomerUserControl_CancelServiceButtonClicked;
                    availableinsessionusercontrol.ExpandUserControlButton_Clicked += InSessionCustomersUserControl_ExpandCollapseButtonClicked;


                    availableinsessionusercontrol.StartTimer();
                    availableinsessionusercontrol.StaffQueTypeTextBox.Visible = true;
                    RecQueStartCurrentCustPanel.Controls.Add(availableinsessionusercontrol);

                    // Add the user control to the existingUserControls dictionary
                    existingUserControls.Add(controlID, availableinsessionusercontrol);
                }
            }
        }

        // Generate a unique identifier for the user control based on the customer information
        private string GenerateControlID(InSessionCustomers customer)
        {
            // Customize the logic to generate a suitable unique identifier based on the customer information
            return $"{customer.TransactionNumber}_{customer.ServiceID}_{customer.ClientName}";
        }


        private List<InSessionCustomers> RetrieveInSessionCustomersFromDB()
        {
            DateTime currentDate = DateTime.Today;
            string datetoday = currentDate.ToString("MM-dd-yyyy dddd");
            List<InSessionCustomers> result = new List<InSessionCustomers>();

            using (MySqlConnection connection = new MySqlConnection(mysqlconn))
            {
                try
                {
                    connection.Open();

                    string insessionquerywalkin = $@"SELECT sh.TransactionNumber, sh.ClientName, sh.ServiceStatus, sh.SelectedService, sh.ServiceID, sh.QueNumber, sh.QueType, sh.AttendingStaff
                                                     FROM servicehistory sh 
                                                     INNER JOIN walk_in_appointment wa ON sh.TransactionNumber = wa.TransactionNumber 
                                                     WHERE (sh.ServiceStatus = 'In Session' OR sh.ServiceStatus = 'In Session Paid')
                                                     AND (wa.ServiceStatus = 'In Session' OR wa.ServiceStatus = 'In Session Paid')
                                                     AND sh.AppointmentDate = @datetoday";

                    string insessionquertappointment = $@"SELECT sh.TransactionNumber, sh.ClientName, sh.ServiceStatus, sh.SelectedService, sh.ServiceID, sh.QueNumber, sh.QueType, sh.AttendingStaff
                                                     FROM servicehistory sh 
                                                     INNER JOIN appointment app ON sh.TransactionNumber = app.TransactionNumber 
                                                     WHERE (sh.ServiceStatus = 'In Session' OR sh.ServiceStatus = 'In Session Paid')
                                                     AND (app.ServiceStatus = 'In Session' OR app.ServiceStatus = 'In Session Paid')
                                                     AND sh.AppointmentDate = @datetoday";

                    MySqlCommand command = new MySqlCommand(insessionquerywalkin, connection);
                    command.Parameters.AddWithValue("@datetoday", datetoday);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            InSessionCustomers insessionustomers = new InSessionCustomers
                            {
                                TransactionNumber = reader["TransactionNumber"] as string,
                                ClientName = reader["ClientName"] as string,
                                ServiceStatus = reader["ServiceStatus"] as string,
                                ServiceName = reader["SelectedService"] as string,
                                ServiceID = reader["ServiceID"] as string,
                                QueType = reader["QueType"] as string,
                                QueNumber = reader["QueNumber"] as string,
                                AttendingStaff = reader["AttendingStaff"] as string
                            };

                            result.Add(insessionustomers);
                        }
                    }

                    MySqlCommand command2 = new MySqlCommand(insessionquertappointment, connection);
                    command2.Parameters.AddWithValue("@datetoday", datetoday);

                    using (MySqlDataReader reader = command2.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            InSessionCustomers insessionustomers = new InSessionCustomers
                            {
                                TransactionNumber = reader["TransactionNumber"] as string,
                                ClientName = reader["ClientName"] as string,
                                ServiceStatus = reader["ServiceStatus"] as string,
                                ServiceName = reader["SelectedService"] as string,
                                ServiceID = reader["ServiceID"] as string,
                                QueType = reader["QueType"] as string,
                                QueNumber = reader["QueNumber"] as string,
                                AttendingStaff = reader["AttendingStaff"] as string
                            };

                            result.Add(insessionustomers);
                        }
                    }
                    if (result.Count == 0)
                    {
                        //MessageBox.Show("No customers in the queue.", "Empty Queue", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }

            return result;
        }

        private void ReceptionScrollPanel_Paint(object sender, PaintEventArgs e)
        {

        }
        bool expand = false;

        private void RecTransTimer_Tick(object sender, EventArgs e)
        {

            if (expand == false)
            {
                RecTransBtnFlowPanel.Height += 15;
                RecTransBtn.IconChar = FontAwesome.Sharp.IconChar.CaretUp;
                RecTransBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
                RecTransBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
                RecTransBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));

                if (RecTransBtnFlowPanel.Height >= RecTransBtnFlowPanel.MaximumSize.Height)
                {
                    RecTransTimer.Stop();
                    expand = true;
                }
            }
            else
            {
                RecTransBtnFlowPanel.Height -= 15;
                RecTransBtn.IconChar = FontAwesome.Sharp.IconChar.CaretDown;
                RecTransBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
                RecTransBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
                RecTransBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
                //RecTransBtnResetColor();

                if (RecTransBtnFlowPanel.Height <= RecTransBtnFlowPanel.MinimumSize.Height)
                {
                    RecTransTimer.Stop();
                    expand = false;
                }
            }
        }

        bool expand1 = false;

        private void RecQueTimer_Tick(object sender, EventArgs e)
        {
            if (expand1 == false)
            {
                RecQueBtnFlowPanel.Height += 15;
                RecQueBtn.IconChar = FontAwesome.Sharp.IconChar.CaretUp;
                RecQueBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
                RecQueBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
                RecQueBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));

                if (RecQueBtnFlowPanel.Height >= RecQueBtnFlowPanel.MaximumSize.Height)
                {
                    RecQueTimer.Stop();
                    expand1 = true;
                }
            }
            else
            {
                RecQueBtnFlowPanel.Height -= 15;
                RecQueBtn.IconChar = FontAwesome.Sharp.IconChar.CaretDown;
                RecQueBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
                RecQueBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
                RecQueBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
                //RecQueBtnResetColor();

                if (RecQueBtnFlowPanel.Height <= RecQueBtnFlowPanel.MinimumSize.Height)
                {
                    RecQueTimer.Stop();
                    expand1 = false;
                }
            }
        }


        private void RecWalkinBasicInfoNxtBtn_Click(object sender, EventArgs e)
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
            else if (string.IsNullOrWhiteSpace(RecWalkinAgeBox.Text) || RecWalkinAgeBox.Text == "Age")
            {
                MessageBox.Show("Please enter birth date.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!IsNumeric(RecWalkinAgeBox.Text))
            {
                MessageBox.Show("Invalid Age.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                filterstaffbyservicecategory = "Hair Styling";
                RecWalkinServicesFlowLayoutPanel.Controls.Clear();
                walkinservices = true;
                InitializeServices(filterstaffbyservicecategory);
                serviceappointment = false;
                haschosenacategory = true;
                if (RecWalkinPreferredStaffToggleSwitch.Checked == true)
                {
                    RecWalkinAttendingStaffSelectedComboBox.Items.Clear();
                    LoadPreferredStaffComboBox();
                }
                RecWalkinHairStyle();
                WalkinTabs.SelectedIndex = 1;
            }
            
        }

        private void RecWalkinProdCOBtn_Click(object sender, EventArgs e)
        {
            WalkinTabs.SelectedIndex = 2;
            RecWalkinProductFlowLayoutPanel.Controls.Clear();
            product = false;
            InitializeProducts();
            walkinproductsearch = true;
            //ditoine
        }

        private void RecWalkinCheckoutBtn_Click(object sender, EventArgs e)
        {
            if (RecWalkinSelectedProdDGV != null && RecWalkinSelectedProdDGV.Rows.Count == 0)
            {
                MessageBox.Show("Select an item to proceed on checking out.", "Ooooops!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                WalkinTabs.SelectedIndex = 1;
            }

        }

        private void RecWalkinProdPrevBtn_Click(object sender, EventArgs e)
        {
            WalkinTabs.SelectedIndex = 1;

        }

        private void RecWalkinBdayPicker_ValueChanged(object sender, EventArgs e)
        {
            DateTime selectedDate = RecWalkinBdayPicker.Value;
            int age = DateTime.Now.Year - selectedDate.Year;

            if (DateTime.Now < selectedDate.AddYears(age))
            {
                age--; // Subtract 1 if the birthday hasn't occurred yet this year
            }
            RecWalkinAgeBox.Text = age.ToString();
            if (age < 4)
            {
                RecWalkinAgeErrorLbl.Visible = true;
                RecWalkinAgeErrorLbl.Text = "Must be 4yrs old\nand above";
                return;
            }
            else
            {
                RecWalkinAgeErrorLbl.Visible = false;

            }
        }
        private void RecWalkinProdCalculateTotalPrice()
        {
            decimal total1 = 0;

            int servicepriceColumnIndex = RecWalkinSelectedProdDGV.Columns["Total Price"].Index;

            foreach (DataGridViewRow row in RecWalkinSelectedProdDGV.Rows)
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
            RecWalkinTotalAmountLblText.Text = total1.ToString("F2");

        }

        private void RecApptSelectedServiceDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (RecApptSelectedServiceDGV.Columns[e.ColumnIndex].Name == "ApptServiceVoid")
            {
                DialogResult result;

                result = MessageBox.Show("Do you want to remove this item?", "Remove Item", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Remove the selected row
                    RecApptSelectedServiceDGV.Rows.RemoveAt(e.RowIndex);
                    MessageBox.Show("Item removed successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RecApptServiceCalculateTotalPrice();
                    RecApptChangeCalculateAmount();
                }

            }
        }

        private void RecApptBasicInfoNextBtn_Click(object sender, EventArgs e)
        {
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
            else
            {
                ApptTabs.SelectedIndex = 1;
                walkinservices = false;
            }
        }

        private void RecApptServicePrevBtn_Click(object sender, EventArgs e)
        {
            ApptTabs.SelectedIndex = 0;
        }

        private void RecApptServiceNextBtn_Click(object sender, EventArgs e)
        {
            if (RecApptBookingTimeComboBox.SelectedIndex == 0 || RecApptBookingTimeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select an appointment time.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else if (RecApptPreferredStaffToggleSwitch.Checked && RecApptAvailableAttendingStaffSelectedComboBox.Text == "Select a Preferred Staff")
            {
                MessageBox.Show("Please select client's preferred staff.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                // Proceed to the next step
                ApptTabs.SelectedIndex = 2;
                RecApptCashText.Text = "";
                RecApptCashText.Focus();
                walkinservices = false;
            }
        }


        private void RecApptAcqServicePrevBtn_Click(object sender, EventArgs e)
        {
            ApptTabs.SelectedIndex = 1;

        }

        private void RecApptCashText_TextChanged(object sender, EventArgs e)
        {
            RecApptChangeCalculateAmount();
        }

        private void RecApptChangeCalculateAmount()
        {
            if (decimal.TryParse(RecApptInitialFeeText.Text, out decimal grossAmount))
            {
                // Get the Cash Amount from the TextBox (MngrCashBox)
                if (decimal.TryParse(RecApptCashText.Text, out decimal cashAmount))
                {
                    // Calculate the Change
                    decimal change = cashAmount - grossAmount;

                    // Display the calculated change value in the MngrChangeBox
                    RecApptChangeText.Text = change.ToString("0.00");
                }
                else
                {
                    // Handle invalid input in MngrCashBox, e.g., display an error message
                    RecApptChangeText.Text = "Invalid Cash Input";
                }
            }
            else
            {
                // Handle invalid input in MngrGrossAmountBox, e.g., display an error message
                RecApptChangeText.Text = "0.00";
            }
        }
        //DINE


        private void RecApptAddMoreServiceBtn_Click(object sender, EventArgs e)
        {
            ApptTabs.SelectedIndex = 1;

        }
        bool DataExpand = false;
        bool ReportExpand = false;
        bool HistoryExpand = false;
        private void MngrDataTimer_Tick(object sender, EventArgs e)
        {
            if (DataExpand == false)
            {
                MngrDataBtnFlowPanel.Height += 15;
                MngrDataBtn.IconChar = FontAwesome.Sharp.IconChar.CaretUp;
                MngrDataBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
                MngrDataBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
                MngrDataBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));

                if (MngrDataBtnFlowPanel.Height >= MngrDataBtnFlowPanel.MaximumSize.Height)
                {
                    MngrDataTimer.Stop();
                    DataExpand = true;
                }
            }
            else
            {
                MngrDataBtnFlowPanel.Height -= 15;
                MngrDataBtn.IconChar = FontAwesome.Sharp.IconChar.CaretDown;
                MngrDataBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
                MngrDataBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
                MngrDataBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

                if (MngrDataBtnFlowPanel.Height <= MngrDataBtnFlowPanel.MinimumSize.Height)
                {
                    MngrDataTimer.Stop();
                    DataExpand = false;
                }
            }
        }

        private void MngrReportsTimer_Tick(object sender, EventArgs e)
        {
            if (ReportExpand == false)
            {
                MngrReportsBtnFlowPanel.Height += 15;
                MngrReportsBtn.IconChar = FontAwesome.Sharp.IconChar.CaretUp;
                MngrReportsBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
                MngrReportsBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
                MngrReportsBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));

                if (MngrReportsBtnFlowPanel.Height >= MngrReportsBtnFlowPanel.MaximumSize.Height)
                {
                    MngrReportsTimer.Stop();
                    ReportExpand = true;
                }
            }
            else
            {
                MngrReportsBtnFlowPanel.Height -= 15;
                MngrReportsBtn.IconChar = FontAwesome.Sharp.IconChar.CaretDown;
                MngrReportsBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
                MngrReportsBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
                MngrReportsBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

                if (MngrReportsBtnFlowPanel.Height <= MngrReportsBtnFlowPanel.MinimumSize.Height)
                {
                    MngrReportsTimer.Stop();
                    ReportExpand = false;
                }
            }
        }

        private void MngrHistoryTimer_Tick(object sender, EventArgs e)
        {
            if (HistoryExpand == false)
            {
                MngrHistoryBtnFlowPanel.Height += 15;
                MngrHistoryBtn.IconChar = FontAwesome.Sharp.IconChar.CaretUp;
                MngrHistoryBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
                MngrHistoryBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
                MngrHistoryBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));

                if (MngrHistoryBtnFlowPanel.Height >= MngrHistoryBtnFlowPanel.MaximumSize.Height)
                {
                    MngrHistoryTimer.Stop();
                    HistoryExpand = true;
                }
            }
            else
            {
                MngrHistoryBtnFlowPanel.Height -= 15;
                MngrHistoryBtn.IconChar = FontAwesome.Sharp.IconChar.CaretDown;
                MngrHistoryBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
                MngrHistoryBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
                MngrHistoryBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

                if (MngrHistoryBtnFlowPanel.Height <= MngrHistoryBtnFlowPanel.MinimumSize.Height)
                {
                    MngrHistoryTimer.Stop();
                    HistoryExpand = false;
                }
            }
        }

        private void MngrDataBtn_Click(object sender, EventArgs e)
        {
            MngrDataTimer.Start();

        }

        private void MngrReportsBtn_Click(object sender, EventArgs e)
        {
            MngrReportsTimer.Start();

        }

        private void MngrHistoryBtn_Click(object sender, EventArgs e)
        {
            MngrHistoryTimer.Start();

        }

        private void MngrRecOverrideBtn_Click(object sender, EventArgs e)
        {
            ReceptionHomePanelReset();
            ExitFunction();
            RecNameLbl.Text = MngrNameLbl.Text;
            RecIDNumLbl.Text = MngrIDNumLbl.Text;
            RecEmplTypeLbl.Text = MngrEmplTypeLbl.Text;
            ReceptionLogoutBtn.Visible = false;
            RecOverrideBackBtn.Visible = true;
        }

        private void MngrServiceDataColor()
        {
            Inventory.PanelShow(MngrServicesPanel);

            //light yellow bg, green text and fg
            MngrServicesDataBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrServicesDataBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            MngrServicesDataBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));

            MngrProductsDataBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            MngrProductsDataBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrProductsDataBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            MngrPromoBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            MngrPromoBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrPromoBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            MngrInventoryMembershipBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            MngrInventoryMembershipBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrInventoryMembershipBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrReportBtnsResetColor();
            MngrHistoryBtnResetColor();
        }
        private void MngrProductDataColor()
        {
            Inventory.PanelShow(MngrInventoryProductsPanel);

            //light yellow bg, green text and fg
            MngrProductsDataBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrProductsDataBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            MngrProductsDataBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));

            MngrServicesDataBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            MngrServicesDataBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrServicesDataBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            MngrPromoBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            MngrPromoBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrPromoBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            MngrInventoryMembershipBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            MngrInventoryMembershipBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrInventoryMembershipBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrReportBtnsResetColor();
            MngrHistoryBtnResetColor();
        }
        private void MngrPromoDataColor()
        {
            Inventory.PanelShow(MngrPromoPanel);

            //light yellow bg, green text and fg
            MngrPromoBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrPromoBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            MngrPromoBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));

            MngrServicesDataBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            MngrServicesDataBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrServicesDataBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            MngrProductsDataBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            MngrProductsDataBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrProductsDataBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            MngrInventoryMembershipBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            MngrInventoryMembershipBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrInventoryMembershipBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrReportBtnsResetColor();
            MngrHistoryBtnResetColor();
        }
        private void MngrMembershipDataColor()
        {
            Inventory.PanelShow(MngrInventoryMembershipPanel);

            //light yellow bg, green text and fg
            MngrInventoryMembershipBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrInventoryMembershipBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            MngrInventoryMembershipBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));

            MngrServicesDataBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            MngrProductsDataBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrProductsDataBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            MngrProductsDataBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            MngrProductsDataBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrProductsDataBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            MngrPromoBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            MngrPromoBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrPromoBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrReportBtnsResetColor();
            MngrHistoryBtnResetColor();
        }

        private void MngrDataBtnsResetColor()
        {
            MngrServicesDataBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            MngrServicesDataBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrServicesDataBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            MngrProductsDataBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            MngrProductsDataBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrProductsDataBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            MngrPromoBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            MngrPromoBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrPromoBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            MngrInventoryMembershipBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            MngrInventoryMembershipBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrInventoryMembershipBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

        }

        private void MngrWalkinSalesColor()
        {
            Inventory.PanelShow(MngrWalkinSalesPanel);

            //light yellow bg, green text and fg
            MngrWalkinServiceSalesBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrWalkinServiceSalesBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            MngrWalkinServiceSalesBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));

            MngrWalkinProdSalesBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            MngrWalkinProdSalesBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrWalkinProdSalesBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            MngrApptServiceBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            MngrApptServiceBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrApptServiceBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            MngrInDemandBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            MngrInDemandBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrInDemandBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrDataBtnsResetColor();
            MngrHistoryBtnResetColor();
        }
        private void MngrApptSalesColor()
        {
            Inventory.PanelShow(MngrApptServicePanel);

            //light yellow bg, green text and fg
            MngrApptServiceBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrApptServiceBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            MngrApptServiceBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));

            MngrWalkinServiceSalesBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            MngrWalkinServiceSalesBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrWalkinServiceSalesBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            MngrWalkinProdSalesBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            MngrWalkinProdSalesBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrWalkinProdSalesBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            MngrInDemandBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            MngrInDemandBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrInDemandBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrDataBtnsResetColor();
            MngrHistoryBtnResetColor();
        }
        private void MngrProdSalesColor()
        {
            Inventory.PanelShow(MngrWalkinProdSalesPanel);

            //light yellow bg, green text and fg
            MngrWalkinProdSalesBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrWalkinProdSalesBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            MngrWalkinProdSalesBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));

            MngrApptServiceBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            MngrApptServiceBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrApptServiceBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            MngrWalkinServiceSalesBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            MngrWalkinServiceSalesBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrWalkinServiceSalesBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            MngrInDemandBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            MngrInDemandBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrInDemandBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrDataBtnsResetColor();
            MngrHistoryBtnResetColor();
        }
        private void MngrInDemandColor()
        {
            Inventory.PanelShow(MngrIndemandPanel);

            //light yellow bg, green text and fg
            MngrInDemandBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrInDemandBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            MngrInDemandBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));

            MngrWalkinServiceSalesBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            MngrWalkinServiceSalesBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrWalkinServiceSalesBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            MngrApptServiceBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            MngrApptServiceBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrApptServiceBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            MngrWalkinProdSalesBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            MngrWalkinProdSalesBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrWalkinProdSalesBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrDataBtnsResetColor();
            MngrHistoryBtnResetColor();
        }
        private void MngrReportBtnsResetColor()
        {
            MngrWalkinServiceSalesBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            MngrWalkinServiceSalesBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrWalkinServiceSalesBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            MngrApptServiceBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            MngrApptServiceBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrApptServiceBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            MngrInDemandBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            MngrInDemandBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrInDemandBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            MngrWalkinProdSalesBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            MngrWalkinProdSalesBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrWalkinProdSalesBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

        }
        private void MngrServiceHistoryColor()
        {
            Inventory.PanelShow(MngrServiceHistoryPanel);

            //light yellow bg, green text and fg
            MngrServicesHistoryBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrServicesHistoryBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            MngrServicesHistoryBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));

            MngrProductsHistoryBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            MngrProductsHistoryBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrProductsHistoryBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));


            MngrDataBtnsResetColor();
            MngrReportBtnsResetColor();

        }
        private void MngrProductHistoryColor()
        {
            Inventory.PanelShow(MngrInventoryProductHistoryPanel);

            //light yellow bg, green text and fg
            MngrProductsHistoryBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrProductsHistoryBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));
            MngrProductsHistoryBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(221)))));

            MngrServicesHistoryBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            MngrServicesHistoryBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrServicesHistoryBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));


            MngrDataBtnsResetColor();
            MngrReportBtnsResetColor();

        }
        private void MngrHistoryBtnResetColor()
        {
            //light yellow bg, green text and fg
            MngrProductsHistoryBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            MngrProductsHistoryBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrProductsHistoryBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));

            MngrServicesHistoryBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(213)))), ((int)(((byte)(178)))));
            MngrServicesHistoryBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
            MngrServicesHistoryBtn.IconColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(136)))), ((int)(((byte)(82)))));
        }

        private void RecOverrideBackBtn_Click(object sender, EventArgs e)
        {
            MngrHomePanelReset();
            RecNameLbl.Text = "";
            RecIDNumLbl.Text = "";
            RecEmplTypeLbl.Text = "";
            ReceptionLogoutBtn.Visible = true;
            RecOverrideBackBtn.Visible = false;
            RecWalkinTransactionClear();
            WalkinTabs.SelectedIndex = 0;
            RecApptTransactionClear();
            ApptTabs.SelectedIndex = 0;
            RecShopProdTransactionClear();


            RecTransTimer.Stop();
            RecQueTimer.Stop();
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void RecPayServiceApptPaymentButton_Click(object sender, EventArgs e)
        {
            if (RecPayServiceUpdateApptDB())
            {
                RecLoadCompletedAppointmentTrans();
                RecPayServiceApptInvoiceGenerator();
                RecPayServiceClearAllField();
                RecLoadCompletedAppointmentTrans();
            }
        }
        private void RecPayServiceApptInvoiceGenerator()
        {
            DateTime currentDate = RecDateTimePicker.Value;
            string datetoday = currentDate.ToString("MM-dd-yyyy dddd");
            string timePrinted = currentDate.ToString("hh:mm tt");
            string timePrintedFile = currentDate.ToString("hh-mm-ss");
            string transactNum = RecPayServiceApptTransactNumLbl.Text;
            string clientName = $"{RecPayServiceApptClientNameLbl}";
            string recName = RecNameLbl.Text;
            string apptNote = "This form will serves as your proof of appointment with Enchanté Salon. " +
                                "Kindly present this form and one (1) Valid ID in our frontdesk and our " +
                                "receptionist shall attend to your needs right away.";
            string legal = "Thank you for trusting Enchanté Salon for your beauty needs." +
                " This receipt will serve as your sales invoice of any services done in Enchanté Salon." +
                " Any concerns about your services please ask and show this receipt in the frontdesk of Enchanté Salon.";

            string total = RecPayServiceApptGrossAmountText.Text.ToString();
            string downpayment = RecPayServiceApptInitialFeeText.Text;
            string cash = RecPayServiceApptCashText.Text;
            string change = RecPayServiceApptChangeText.Text;
            string bal = RecPayServiceApptRemainingBalText.Text;



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
                Document doc = new Document(new iTextSharp.text.Rectangle(Utilities.MillimetersToPoints(133.35f), Utilities.MillimetersToPoints(215.9f)));

                try
                {
                    // Create a PdfWriter instance
                    PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));

                    // Open the document for writing
                    doc.Open();

                    Bitmap imagepath = Properties.Resources.Enchante_Logo__200_x_200_px__Green;
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(imagepath, System.Drawing.Imaging.ImageFormat.Png);
                    logo.Alignment = Element.ALIGN_CENTER;
                    logo.ScaleAbsolute(100f, 100f);
                    doc.Add(logo);

                    iTextSharp.text.Font headerFont = FontFactory.GetFont("Courier", 16, iTextSharp.text.Font.BOLD);
                    iTextSharp.text.Font boldfont = FontFactory.GetFont("Courier", 10, iTextSharp.text.Font.BOLD);
                    iTextSharp.text.Font font = FontFactory.GetFont("Courier", 10, iTextSharp.text.Font.NORMAL);
                    iTextSharp.text.Font italic = FontFactory.GetFont("Courier", 10, iTextSharp.text.Font.ITALIC);
                    iTextSharp.text.Font right = FontFactory.GetFont("Courier", 10, Element.ALIGN_CENTER);

                    // Create a centered alignment for text
                    iTextSharp.text.Paragraph centerAligned = new Paragraph();
                    centerAligned.Alignment = Element.ALIGN_CENTER;

                    // Add centered content to the centerAligned Paragraph
                    //centerAligned.Add(new Chunk("Enchanté Salon", headerFont));
                    centerAligned.Add(new Chunk("\n69th flr. Enchanté Bldg. Ortigas Ave. Ext.\nManggahan, Pasig City 1611 Philippines", font));
                    centerAligned.Add(new Chunk("\nTel. No.: (1101) 111-1010", font));
                    centerAligned.Add(new Chunk($"\nDate: {datetoday} Time: {timePrinted}", font));

                    // Add the centered content to the document
                    doc.Add(centerAligned);

                    int totalRowCount = RecPayServiceApptAcquiredDGV.Rows.Count;
                    doc.Add(new Chunk("\n")); // New line

                    doc.Add(new Paragraph($"Transaction No.: {transactNum}", font));
                    doc.Add(new Paragraph($"Booked For: {clientName}", font));
                    doc.Add(new Paragraph($"Booked By: {recName}", font));

                    doc.Add(new Chunk("\n")); // New line

                    doc.Add(new LineSeparator()); // Dotted line

                    PdfPTable columnHeaderTable = new PdfPTable(3);
                    columnHeaderTable.SetWidths(new float[] { 30f, 40f, 30f }); // Column widths
                    columnHeaderTable.DefaultCell.Border = PdfPCell.NO_BORDER;
                    columnHeaderTable.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
                    columnHeaderTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

                    columnHeaderTable.AddCell(new Phrase("Staff ID", boldfont));
                    columnHeaderTable.AddCell(new Phrase("Services", boldfont));
                    columnHeaderTable.AddCell(new Phrase("Total Price", boldfont));

                    doc.Add(columnHeaderTable);

                    doc.Add(new LineSeparator()); // Dotted line
                    // Iterate through the rows of your 

                    foreach (DataGridViewRow row in RecPayServiceApptAcquiredDGV.Rows)
                    {
                        try
                        {
                            string serviceName = row.Cells["ApptSelectedService"].Value?.ToString();
                            if (string.IsNullOrEmpty(serviceName))
                            {
                                continue; // Skip empty rows
                            }

                            string staffID = row.Cells["ApptStaffSelected"].Value?.ToString();
                            string itemTotalcost = row.Cells["ApptServicePrice"].Value?.ToString();

                            // Add cells to the item table
                            PdfPTable serviceTable = new PdfPTable(3);
                            serviceTable.SetWidths(new float[] { 30f, 40f, 30f }); // Column widths
                            serviceTable.DefaultCell.Border = PdfPCell.NO_BORDER;
                            serviceTable.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
                            serviceTable.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;

                            serviceTable.AddCell(new Phrase(staffID, font));
                            serviceTable.AddCell(new Phrase(serviceName, font));
                            serviceTable.AddCell(new Phrase(itemTotalcost, font));

                            doc.Add(serviceTable);

                        }
                        catch (Exception ex)
                        {
                            // Handle or log any exceptions that occur while processing DataGridView data
                            MessageBox.Show("An error occurred: " + ex.Message, "Appoint Form Generator Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }

                    doc.Add(new Chunk("\n")); // New line
                    doc.Add(new LineSeparator()); // Dotted line
                    doc.Add(new Chunk("\n")); // New line


                    decimal netAmount = decimal.Parse(RecPayServiceApptNetAmountText.Text);
                    decimal discount = decimal.Parse(RecPayServiceApptDiscountText.Text);
                    decimal vat = decimal.Parse(RecPayServiceApptVATText.Text);

                    string paymentMethod = "Cash";

                    // Add cells to the INFO table
                    PdfPTable amount = new PdfPTable(2);

                    amount.HorizontalAlignment = Element.ALIGN_CENTER; // Center the table

                    amount.SetWidths(new float[] { 60f, 40f }); // Column widths as percentage of the total width

                    amount.DefaultCell.Border = PdfPCell.NO_BORDER;
                    amount.DefaultCell.VerticalAlignment = Element.ALIGN_CENTER;
                    amount.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT; // Align cell content justified

                    amount.AddCell(new Phrase($"Total ({totalRowCount}): ", font));
                    PdfPCell totalCell = new PdfPCell(new Phrase($"Php. {total}", font));
                    totalCell.Border = PdfPCell.NO_BORDER; // Remove border from this cell
                    amount.AddCell(totalCell);

                    amount.AddCell(new Phrase($"Initital Payment (40%): ", font));
                    PdfPCell dpCell = new PdfPCell(new Phrase($"Php. {downpayment}", font));
                    dpCell.Border = PdfPCell.NO_BORDER; // Remove border from this cell
                    amount.AddCell(dpCell);

                    amount.AddCell(new Phrase($"Balance Payment: ", font));
                    PdfPCell balCell = new PdfPCell(new Phrase($"Php. {bal}", font));
                    balCell.Border = PdfPCell.NO_BORDER; // Remove border from this cell
                    amount.AddCell(balCell);

                    amount.AddCell(new Phrase($"Cash Given: ", font));
                    PdfPCell cashCell = new PdfPCell(new Phrase($"Php. {cash}", font));
                    cashCell.Border = PdfPCell.NO_BORDER; // Remove border from this cell
                    amount.AddCell(cashCell);

                    amount.AddCell(new Phrase($"Change: ", font));
                    PdfPCell changeCell = new PdfPCell(new Phrase($"Php. {change}", font));
                    changeCell.Border = PdfPCell.NO_BORDER; // Remove border from this cell
                    amount.AddCell(changeCell);

                    doc.Add(amount); // Add the table to the document


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
                    //vatTable.AddCell(new Phrase("Discount (20%)", font));
                    //vatTable.AddCell(new Phrase($"Php {discount:F2}", font));

                    // Add the "VATable" table to the document
                    doc.Add(vatTable);

                    // Add the legal string with center alignment
                    Paragraph paragraph_footer = new Paragraph($"\n{legal}", italic);
                    paragraph_footer.Alignment = Element.ALIGN_CENTER;
                    doc.Add(paragraph_footer);
                }
                catch (DocumentException de)
                {
                    MessageBox.Show("An error occurred: " + de.Message, "Appoint Form Generator Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (IOException ioe)
                {
                    MessageBox.Show("An error occurred: " + ioe.Message, "Appoint Form Generator Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // Close the document
                    doc.Close();
                }

                //MessageBox.Show($"Receipt saved as {filePath}", "Receipt Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private bool RecPayServiceUpdateApptDB()
        {
            // cash values
            string netAmount = RecPayServiceApptNetAmountText.Text; // net amount
            string vat = RecPayServiceApptVATText.Text; // vat 
            string discount = ""; // discount RecPayServiceApptDiscountText.Text
            string grossAmount = RecPayServiceApptGrossAmountText.Text; // gross amount
            string cash = RecPayServiceApptCashText.Text; // cash given
            string change = RecPayServiceApptChangeText.Text; // due change
            string bal = RecPayServiceApptRemainingBalText.Text;
            string dp = RecPayServiceApptInitialFeeText.Text;
            string paymentMethod = "Cash"; // payment method
            string mngr = RecNameLbl.Text;
            string transactNum = RecPayServiceApptTransactNumLbl.Text;

            // bank & wallet details


            try
            {
                using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                {
                    connection.Open();
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
                    else if (Convert.ToDecimal(cash) < Convert.ToDecimal(bal))
                    {
                        MessageBox.Show("Insufficient amount. Please provide enough cash to cover the transaction.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else
                    {
                        //appointment transactions
                        string cashPaymentAppt = "UPDATE appointment SET ServiceStatus = @status, NetPrice = @net, VatAmount = @vat, DiscountAmount = @discount, " +
                                            "GrossAmount = @gross, Downpayment = @dp, RemainingBal = @bal, CashGiven = @cash, DueChange = @change, PaymentMethod = @payment, CheckedOutBy = @mngr " +
                                            "WHERE TransactionNumber = @transactNum"; // cash query
                        MySqlCommand cmd2 = new MySqlCommand(cashPaymentAppt, connection);
                        cmd2.Parameters.AddWithValue("@status", "Paid");
                        cmd2.Parameters.AddWithValue("@net", netAmount);
                        cmd2.Parameters.AddWithValue("@vat", vat);
                        cmd2.Parameters.AddWithValue("@discount", discount);
                        cmd2.Parameters.AddWithValue("@gross", grossAmount);
                        cmd2.Parameters.AddWithValue("@dp", dp);
                        cmd2.Parameters.AddWithValue("@bal", bal);
                        cmd2.Parameters.AddWithValue("@cash", cash);
                        cmd2.Parameters.AddWithValue("@change", change);
                        cmd2.Parameters.AddWithValue("@payment", paymentMethod);
                        cmd2.Parameters.AddWithValue("@mngr", mngr);
                        cmd2.Parameters.AddWithValue("@transactNum", transactNum);

                        cmd2.ExecuteNonQuery();
                        // Successful update
                        MessageBox.Show("Service successfully been paid through cash.", "Hooray!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                }
            }
            catch (MySqlException ex)
            {
                // Handle MySQL database exception
                MessageBox.Show("An error occurred: " + ex.Message, "Receptionist appointment payment transaction failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false; // Return false in case of an exception
            }
            finally
            {
                // Make sure to close the connection
                connection.Close();
            }
            return true;
        }
        private void RecBtnHolderFlowPanel_Paint(object sender, PaintEventArgs e)
        {

        }



        public class Services
        {
            public string ServiceName { get; set; }
            public string ServicePrice { get; set; }
            public string ServiceDuration { get; set; }
            public string ServiceCategory { get; set; }
            public string ServiceID { get; set; }
        }

        public void InitializeServices(string category)
        {
            if (walkinservices)
            {
                serviceitemsPerPage = 5;
                RecWalkinServicesFlowLayoutPanel.Controls.Clear();
            }
            else
            {
                serviceitemsPerPage = 7;
                RecApptServicesFLP.Controls.Clear();
            }
            
            List<Services> services = RetrieveServices(category);

            foreach (Services service in services)
            {

                ServicesUserControl servicesusercontrol = new ServicesUserControl(this);
                servicesusercontrol.SetServicesData(service);
                servicesusercontrol.ServiceUserControl_Clicked += ServiceUserControl_Clicked;
                servicesusercontrol.RecServicePriceTextBox_Clicked += ServiceUserControl_Clicked;
                servicesusercontrol.RecServiceDurationTextBox_Clicked += ServiceUserControl_Clicked;
                servicesusercontrol.RecServiceNameTextBox_Clicked += ServiceUserControl_Clicked;

                if (walkinservices)
                {
                    RecWalkinServicesFlowLayoutPanel.Controls.Add(servicesusercontrol);
                }
                else
                {
                    RecApptServicesFLP.Controls.Add(servicesusercontrol);
                }
                
            }
        }

        public string serviceName;
        public string servicePrice;
        public string serviceDuration;
        public string serviceCategory;
        public string serviceID2;

        private void ServiceUserControl_Clicked(object sender, EventArgs e)
        {
            ServicesUserControl servicesUserControl = (ServicesUserControl)sender;
        }


        private int servicecurrentPage = 0;
        private int servicecurrentPagefake = 1;
        private int serviceitemsPerPage = 5;
        private int servicetotalItems = 0;
        private int servicetotalPages = 0;

        public string servicecat;

        private List<Services> RetrieveServices(string category)
        {
            List<Services> result = new List<Services>();


            servicecat = category;

            using (MySqlConnection connection = new MySqlConnection(mysqlconn))
            {
                try
                {
                    connection.Open();

                    string countQuery = "SELECT COUNT(*) FROM services WHERE Category = @category";
                    MySqlCommand countCommand = new MySqlCommand(countQuery, connection);
                    countCommand.Parameters.AddWithValue("@category", servicecat);
                    servicetotalItems = Convert.ToInt32(countCommand.ExecuteScalar());

                    servicetotalPages = (int)Math.Ceiling((double)servicetotalItems / serviceitemsPerPage);

                    int offset = servicecurrentPage * serviceitemsPerPage;
                    if (walkinservices)
                    {
                        WalkinServicePageLbl.Text = $"{servicecurrentPagefake} / {servicetotalPages}";
                    }
                    else
                    {
                        ApptServicePageLbl.Text = $"{servicecurrentPagefake} / {servicetotalPages}";
                    }

                    string servicesquery = $@"SELECT Category, ServiceID, Name, Duration, Price FROM services WHERE Category = @category 
                                            LIMIT { offset}, { serviceitemsPerPage}";



                    MySqlCommand command = new MySqlCommand(servicesquery, connection);
                    command.Parameters.AddWithValue("@category", servicecat);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Services Services = new Services
                            {
                                ServiceName = reader["Name"].ToString(),
                                ServicePrice = reader["Price"].ToString(),
                                ServiceDuration = reader["Duration"].ToString(),
                                ServiceCategory = reader["Category"].ToString(),
                                ServiceID = reader["ServiceID"].ToString()
                            };

                            result.Add(Services);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }

            return result;
        }

        public void RecWalkinAddService()
        {
            selectedStaffID = "";


            if (RecWalkinAnyStaffToggleSwitch.Checked == false && RecWalkinPreferredStaffToggleSwitch.Checked == false)
            {
                MessageBox.Show("Please select a prefered staff or toggle anyone ", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (RecWalkinAnyStaffToggleSwitch.Checked)
            {
                selectedStaffID = "Anyone";
            }
            if (RecWalkinPreferredStaffToggleSwitch.Checked)
            {
                string selectedstaff = RecWalkinAttendingStaffSelectedComboBox.SelectedItem.ToString();
                selectedStaffID = selectedstaff.Substring(0, 11);
            }

            if (string.IsNullOrEmpty(selectedStaffID))
            {
                MessageBox.Show("Please select a prefered staff or toggle anyone ", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string SelectedCategory = serviceCategory;
            string ServiceID = serviceID2;
            string ServiceName = serviceName;
            string ServicePrice = servicePrice;

            string serviceID = serviceID2;


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

                NewSelectedServiceRow.Cells["ServicePrices"].Value = ServicePrice;
                NewSelectedServiceRow.Cells["ServiceCategories"].Value = SelectedCategory;
                NewSelectedServiceRow.Cells["SelectedService"].Value = ServiceName;
                NewSelectedServiceRow.Cells["ServiceID"].Value = ServiceID;
                NewSelectedServiceRow.Cells["QueNumber"].Value = latestquenumber;
                NewSelectedServiceRow.Cells["StaffSelected"].Value = selectedStaffID;
                QueTypeIdentifier(NewSelectedServiceRow.Cells["QueType"]);



            }
        }

        

        private void ProductPreviousBtn_Click(object sender, EventArgs e)
        {
            if (walkinproductsearch)
            {
                string searchKeyword = RecWalkinSearchProductTextBox.Text.Trim().ToLower();

                if (string.IsNullOrEmpty(searchKeyword))
                {
                    if (currentPagefake == 1)
                    {
                        currentPagefake = totalPages;
                        currentPage = totalPages - 1;
                        InitializeProducts();
                        ProductPageLbl.Text = $"{currentPagefake} / {totalPages}";
                        WalkinProductPageLbl.Text = $"{currentPagefake} / {totalPages}";
                    }
                    else if (currentPagefake <= totalPages)
                    {
                        currentPage--;
                        currentPagefake--;
                        InitializeProducts();
                        ProductPageLbl.Text = $"{currentPagefake} / {totalPages}";
                        WalkinProductPageLbl.Text = $"{currentPagefake} / {totalPages}";
                    }
                }
                else
                {
                    if (currentPagefake == 1)
                    {
                        currentPagefake = totalPages;
                        currentPage = totalPages - 1;
                        RecWalkinSearchProductNextPrevious();
                        ProductPageLbl.Text = $"{currentPagefake} / {totalPages}";
                        WalkinProductPageLbl.Text = $"{currentPagefake} / {totalPages}";
                    }
                    else if (currentPagefake <= totalPages)
                    {
                        currentPage--;
                        currentPagefake--;
                        RecWalkinSearchProductNextPrevious();
                        ProductPageLbl.Text = $"{currentPagefake} / {totalPages}";
                        WalkinProductPageLbl.Text = $"{currentPagefake} / {totalPages}";
                    }
                }
            }
            else
            {
                string searchKeyword = RecSearchProductTextBox.Text.Trim().ToLower();
                if (string.IsNullOrEmpty(searchKeyword))
                {
                    if (currentPagefake == 1)
                    {
                        currentPagefake = totalPages;
                        currentPage = totalPages - 1;
                        InitializeProducts();
                        ProductPageLbl.Text = $"{currentPagefake} / {totalPages}";
                        WalkinProductPageLbl.Text = $"{currentPagefake} / {totalPages}";
                    }
                    else if (currentPagefake <= totalPages)
                    {
                        currentPage--;
                        currentPagefake--;
                        InitializeProducts();
                        ProductPageLbl.Text = $"{currentPagefake} / {totalPages}";
                        WalkinProductPageLbl.Text = $"{currentPagefake} / {totalPages}";
                    }
                }
                else
                {
                    if (currentPagefake == 1)
                    {
                        currentPagefake = totalPages;
                        currentPage = totalPages - 1;
                        RecSearchProductNextPrevious();
                        ProductPageLbl.Text = $"{currentPagefake} / {totalPages}";
                        WalkinProductPageLbl.Text = $"{currentPagefake} / {totalPages}";
                    }
                    else if (currentPagefake <= totalPages)
                    {
                        currentPage--;
                        currentPagefake--;
                        RecSearchProductNextPrevious();
                        ProductPageLbl.Text = $"{currentPagefake} / {totalPages}";
                        WalkinProductPageLbl.Text = $"{currentPagefake} / {totalPages}";
                    }
                }
            }

        }

        private void ProductNextBtn_Click(object sender, EventArgs e)
        {
            if (walkinproductsearch)
            {
                string searchKeyword = RecWalkinSearchProductTextBox.Text.Trim().ToLower();

                if (string.IsNullOrEmpty(searchKeyword))
                {
                    if (currentPagefake == totalPages)
                    {
                        currentPage = 0;
                        currentPagefake = 0;
                        currentPagefake++;
                        InitializeProducts();
                        ProductPageLbl.Text = $"{currentPagefake} / {totalPages}";
                        WalkinProductPageLbl.Text = $"{currentPagefake} / {totalPages}";
                    }
                    else
                    {
                        currentPage++;
                        currentPagefake++;
                        InitializeProducts();
                        ProductPageLbl.Text = $"{currentPagefake} / {totalPages}";
                        WalkinProductPageLbl.Text = $"{currentPagefake} / {totalPages}";
                    }
                }
                else
                {
                    if (currentPagefake == totalPages)
                    {
                        currentPage = 0;
                        currentPagefake = 0;
                        currentPagefake++;
                        RecWalkinSearchProductNextPrevious();
                        ProductPageLbl.Text = $"{currentPagefake} / {totalPages}";
                        WalkinProductPageLbl.Text = $"{currentPagefake} / {totalPages}";
                    }
                    else
                    {
                        currentPage++;
                        currentPagefake++;
                        RecWalkinSearchProductNextPrevious();
                        ProductPageLbl.Text = $"{currentPagefake} / {totalPages}";
                        WalkinProductPageLbl.Text = $"{currentPagefake} / {totalPages}";
                    }

                }

            }
            else
            {
                string searchKeyword = RecSearchProductTextBox.Text.Trim().ToLower();

                if (string.IsNullOrEmpty(searchKeyword))
                {
                    if (currentPagefake == totalPages)
                    {
                        currentPage = 0;
                        currentPagefake = 0;
                        currentPagefake++;
                        InitializeProducts();
                        ProductPageLbl.Text = $"{currentPagefake} / {totalPages}";
                        WalkinProductPageLbl.Text = $"{currentPagefake} / {totalPages}";
                    }
                    else
                    {
                        currentPage++;
                        currentPagefake++;
                        InitializeProducts();
                        ProductPageLbl.Text = $"{currentPagefake} / {totalPages}";
                        WalkinProductPageLbl.Text = $"{currentPagefake} / {totalPages}";
                    }
                }
                else
                {
                    if (currentPagefake == totalPages)
                    {
                        currentPage = 0;
                        currentPagefake = 0;
                        currentPagefake++;
                        RecSearchProductNextPrevious();
                        WalkinProductPageLbl.Text = $"{currentPagefake} / {totalPages}";
                    }
                    else
                    {
                        currentPage++;
                        currentPagefake++;
                        RecSearchProductNextPrevious();
                        ProductPageLbl.Text = $"{currentPagefake} / {totalPages}";
                        WalkinProductPageLbl.Text = $"{currentPagefake} / {totalPages}";
                    }

                }

            }

        }

        private void WalkinProductPreviousBtn_Click(object sender, EventArgs e)
        {
            ProductPreviousBtn_Click(sender, e);
        }

        private void WalkinProductNextBtn_Click(object sender, EventArgs e)
        {
            ProductNextBtn_Click(sender, e);
        }

        private bool TabCompletedTransLoad = false;
        private void PaymentTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PaymentTabs.SelectedIndex == 0)
            {

                RecLoadCompletedWalkinTrans();
                TabCompletedTransLoad = true; // Set the flag to true to indicate that service history is loaded


            }
            else if (PaymentTabs.SelectedIndex == 1)
            {

                RecLoadCompletedAppointmentTrans();
                TabCompletedTransLoad = true; // Set the flag to true to indicate that service history is loaded

            }
        }

        private void RecPayServiceApptCashText_TextChanged(object sender, EventArgs e)
        {
            RecPayServiceApptChangeCalculateAmount();
        }


        public class Staff
        {
            public string StaffName { get; set; }
            public string StaffCategory { get; set; }
            public string StaffGender { get; set; }
            public string StaffID { get; set; }
        }

        public List<Staff> RetriveStaffinDB(string employeecategory)
        {
            List<Staff> result = new List<Staff>();

            using (MySqlConnection connection = new MySqlConnection(mysqlconn))
            {
                try
                {
                    connection.Open();

                    string staffquery = "SELECT FirstName, LastName, Gender, EmployeeCategory, EmployeeID FROM systemusers " +
                                        "WHERE EmployeeType = 'Staff' AND Availability = 'Available' AND EmployeeCategory = @employeecategory";

                    MySqlCommand command = new MySqlCommand(staffquery, connection);
                    command.Parameters.AddWithValue("@employeecategory", employeecategory);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string lastName = reader["LastName"] as string;
                            string firstName = reader["FirstName"] as string;
                            string staffName = $"{lastName}, {firstName}";

                            Staff staff = new Staff
                            {
                                StaffName = staffName,
                                StaffCategory = reader["EmployeeCategory"] as string,
                                StaffGender = reader["Gender"] as string,
                                StaffID = reader["EmployeeID"] as string
                            };

                            result.Add(staff);
                        }
                    }
                    if (result.Count == 0)
                    {
                        //MessageBox.Show("No customers in the queue.", "Empty Queue", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }

            return result;
        }


        public void StaffClicked()
        {
            CurrentStaffID = selectedstaffemployeeid;

            RecQueueStartPanel.Controls.Clear();
            InitializeGeneralPendingCustomersForStaff();
            RefreshFlowLayoutPanel();
        }

        public void RefreshAvailableStaff()
        {
            RecQueStartStaffFLP.Controls.Clear();
            InitializeStaffUserControl();
        }

        public string selectedemployeecategory;
        public void InitializeEmployeeCategory()
        {
            ServiceTypeComboBox.SelectedIndex = 0;
        }

        public void InitializeStaffUserControl()
        {
            selectedemployeecategory = ServiceTypeComboBox.SelectedItem.ToString();
            RecQueStartStaffFLP.Controls.Clear();
            List<Staff> staff = RetriveStaffinDB(selectedemployeecategory);
            foreach (Staff staffs in staff)
            {
                StaffUserControl availablestaffusercontrol = new StaffUserControl(this);
                availablestaffusercontrol.SetStaffData(staffs);

                availablestaffusercontrol.StaffUserControl_Clicked += AvailableStaff_Clicked;
                RecQueStartStaffFLP.Controls.Add(availablestaffusercontrol);
            }
        }

        private void AvailableStaff_Clicked(object sender, EventArgs e)
        {
            StaffUserControl clickestaff = (StaffUserControl)sender;
        }

        private void ServiceTypeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            RecQueueStartPanel.Controls.Clear();
            InitializeStaffUserControl();
        }

        private void RecQueStartCurrentCustPanel_Paint(object sender, PaintEventArgs e)
        {

        }


        public void InitializeServices2(string filterstaffbyservicecategory, string searchKeyword)
        {
            if (walkinservices)
            {
                serviceitemsPerPage = 5;
                RecWalkinServicesFlowLayoutPanel.Controls.Clear();
            }
            else
            {
                serviceitemsPerPage = 7;
                RecApptServicesFLP.Controls.Clear();
            }
            List<Services> services = RetrieveServices2(filterstaffbyservicecategory,searchKeyword);

            foreach (Services service in services)
            {

                ServicesUserControl servicesusercontrol = new ServicesUserControl(this);
                servicesusercontrol.SetServicesData(service);
                servicesusercontrol.ServiceUserControl_Clicked += ServiceUserControl_Clicked;
                servicesusercontrol.RecServicePriceTextBox_Clicked += ServiceUserControl_Clicked;
                servicesusercontrol.RecServiceDurationTextBox_Clicked += ServiceUserControl_Clicked;
                servicesusercontrol.RecServiceNameTextBox_Clicked += ServiceUserControl_Clicked;

                if (walkinservices)
                {
                    RecWalkinServicesFlowLayoutPanel.Controls.Add(servicesusercontrol);
                }
                else
                {
                    RecApptServicesFLP.Controls.Add(servicesusercontrol);
                }
                
            }
        }

        private List<Services> RetrieveServices2(string filterstaffbyservicecategory, string searchKeyword)
        {
            List<Services> result = new List<Services>();
            servicetotalItems = 0;
            servicecurrentPage = 0;
            servicecurrentPagefake = 1;

            using (MySqlConnection connection = new MySqlConnection(mysqlconn))
            {
                try
                {
                    connection.Open();

                    string countQuery = "SELECT COUNT(*) FROM services WHERE Category = @category AND Name LIKE @searchKeyword";
                    MySqlCommand countCommand = new MySqlCommand(countQuery, connection);
                    countCommand.Parameters.AddWithValue("@searchKeyword", "%" + searchKeyword + "%");
                    countCommand.Parameters.AddWithValue("@category", filterstaffbyservicecategory);
                    servicetotalItems = Convert.ToInt32(countCommand.ExecuteScalar());

                    servicetotalPages = (int)Math.Ceiling((double)servicetotalItems / serviceitemsPerPage);

                    int offset = servicecurrentPage * serviceitemsPerPage;
                    if (walkinservices)
                    {
                        WalkinServicePageLbl.Text = $"{servicecurrentPagefake} / {servicetotalPages}";
                    }
                    else
                    {
                        ApptServicePageLbl.Text = $"{servicecurrentPagefake} / {servicetotalPages}";
                    }
                    string servicesquery = $@"SELECT Category, ServiceID, Name, Duration, Price FROM services WHERE Category = @category 
                                            AND Name LIKE @searchKeyword
                                            LIMIT {offset}, {serviceitemsPerPage}";
                    if (walkinservices)
                    {
                        if (servicetotalItems == 0)
                        {
                            WalkinServiceNextButton.Enabled = false;
                            WalkinServicePreviousButton.Enabled = false;
                            WalkinServicePageLbl.Text = $"0/0";
                        }
                        else
                        {
                            WalkinServiceNextButton.Enabled = true;
                            WalkinServicePreviousButton.Enabled = true;
                        }
                    }
                    else
                    {
                        if (servicetotalItems == 0)
                        {
                            ApptServiceNextBtn.Enabled = false;
                            ApptServicePreviousBtn.Enabled = false;
                            ApptServicePageLbl.Text = $"0/0";
                        }
                        else
                        {
                            ApptServiceNextBtn.Enabled = true;
                            ApptServicePreviousBtn.Enabled = true;
                        }
                    }


                    MySqlCommand command = new MySqlCommand(servicesquery, connection);
                    command.Parameters.AddWithValue("@category", servicecat);
                    command.Parameters.AddWithValue("@searchKeyword", "%" + searchKeyword + "%");

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Services Services = new Services
                            {
                                ServiceName = reader["Name"].ToString(),
                                ServicePrice = reader["Price"].ToString(),
                                ServiceDuration = reader["Duration"].ToString(),
                                ServiceCategory = reader["Category"].ToString(),
                                ServiceID = reader["ServiceID"].ToString()
                            };

                            result.Add(Services);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }

            return result;
        }

        public void InitializeServices3(string filterstaffbyservicecategory, string searchKeyword)
        {
            if (string.IsNullOrEmpty(searchKeyword))
            {
                InitializeServices(filterstaffbyservicecategory);
                return;
            }

            if (walkinservices)
            {
                serviceitemsPerPage = 5;
                RecWalkinServicesFlowLayoutPanel.Controls.Clear();
            }
            else
            {
                serviceitemsPerPage = 7;
                RecApptServicesFLP.Controls.Clear();
            }

            List<Services> services = RetrieveServices3(filterstaffbyservicecategory, searchKeyword);

            foreach (Services service in services)
            {

                ServicesUserControl servicesusercontrol = new ServicesUserControl(this);
                servicesusercontrol.SetServicesData(service);
                servicesusercontrol.ServiceUserControl_Clicked += ServiceUserControl_Clicked;
                servicesusercontrol.RecServicePriceTextBox_Clicked += ServiceUserControl_Clicked;
                servicesusercontrol.RecServiceDurationTextBox_Clicked += ServiceUserControl_Clicked;
                servicesusercontrol.RecServiceNameTextBox_Clicked += ServiceUserControl_Clicked;

                if (walkinservices)
                {
                    RecWalkinServicesFlowLayoutPanel.Controls.Add(servicesusercontrol);
                }
                else
                {
                    RecApptServicesFLP.Controls.Add(servicesusercontrol);
                }
                
            }
        }

        private List<Services> RetrieveServices3(string filterstaffbyservicecategory, string searchKeyword)
        {
            List<Services> result = new List<Services>();
            servicetotalItems = 0;


            using (MySqlConnection connection = new MySqlConnection(mysqlconn))
            {
                try
                {
                    connection.Open();

                    string countQuery = "SELECT COUNT(*) FROM services WHERE Category = @category AND Name LIKE @searchKeyword";
                    MySqlCommand countCommand = new MySqlCommand(countQuery, connection);
                    countCommand.Parameters.AddWithValue("@searchKeyword", "%" + searchKeyword + "%");
                    countCommand.Parameters.AddWithValue("@category", filterstaffbyservicecategory);
                    servicetotalItems = Convert.ToInt32(countCommand.ExecuteScalar());

                    servicetotalPages = (int)Math.Ceiling((double)servicetotalItems / serviceitemsPerPage);

                    int offset = servicecurrentPage * serviceitemsPerPage;
                    if (walkinservices)
                    {
                        WalkinServicePageLbl.Text = $"{servicecurrentPagefake} / {servicetotalPages}";
                    }
                    else
                    {
                        ApptServicePageLbl.Text = $"{servicecurrentPagefake} / {servicetotalPages}";
                    }

                    string servicesquery = $@"SELECT Category, ServiceID, Name, Duration, Price FROM services WHERE Category = @category 
                                            AND Name LIKE @searchKeyword
                                            LIMIT {offset}, {serviceitemsPerPage}";

                    MySqlCommand command = new MySqlCommand(servicesquery, connection);
                    command.Parameters.AddWithValue("@category", servicecat);
                    command.Parameters.AddWithValue("@searchKeyword", "%" + searchKeyword + "%");

                    if (walkinservices)
                    {
                        if (servicetotalItems == 0)
                        {
                            WalkinServiceNextButton.Enabled = false;
                            WalkinServicePreviousButton.Enabled = false;
                            WalkinServicePageLbl.Text = $"0/0";
                        }
                        else
                        {
                            WalkinServiceNextButton.Enabled = true;
                            WalkinServicePreviousButton.Enabled = true;
                        }
                    }
                    else
                    {
                        if (servicetotalItems == 0)
                        {
                            ApptServiceNextBtn.Enabled = false;
                            ApptServicePreviousBtn.Enabled = false;
                            ApptServicePageLbl.Text = $"0/0";
                        }
                        else
                        {
                            ApptServiceNextBtn.Enabled = true;
                            ApptServicePreviousBtn.Enabled = true;
                        }
                    }


                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Services Services = new Services
                            {
                                ServiceName = reader["Name"].ToString(),
                                ServicePrice = reader["Price"].ToString(),
                                ServiceDuration = reader["Duration"].ToString(),
                                ServiceCategory = reader["Category"].ToString(),
                                ServiceID = reader["ServiceID"].ToString()
                            };

                            result.Add(Services);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }

            return result;
        }

        public bool walkinservices;

        private void WalkinServicePreviousButton_Click(object sender, EventArgs e)
        {
            if (walkinservices)
            {
                string searchKeyword = RecWalkinSearchServiceTypeText.Text.Trim().ToLower();
                
                if (string.IsNullOrEmpty(searchKeyword))
                {
                    if (servicecurrentPagefake == 1)
                    {
                        servicecurrentPagefake = servicetotalPages;
                        servicecurrentPage = servicetotalPages - 1;
                        InitializeServices(filterstaffbyservicecategory);
                        WalkinServicePageLbl.Text = $"{servicecurrentPagefake} / {servicetotalPages}";
                    }
                    else if (currentPagefake <= servicetotalPages)
                    {
                        servicecurrentPage--;
                        servicecurrentPagefake--;
                        InitializeServices(filterstaffbyservicecategory);
                        WalkinServicePageLbl.Text = $"{servicecurrentPagefake} / {servicetotalPages}";
                    }
                }
                else
                {
                    if (servicecurrentPagefake == 1)
                    {
                        servicecurrentPagefake = servicetotalPages;
                        servicecurrentPage = servicetotalPages - 1;
                        InitializeServices3(filterstaffbyservicecategory, searchKeyword);
                        WalkinServicePageLbl.Text = $"{servicecurrentPagefake} / {servicetotalPages}";
                    }
                    else if (currentPagefake <= servicetotalPages)
                    {
                        servicecurrentPage--;
                        servicecurrentPagefake--;
                        InitializeServices3(filterstaffbyservicecategory, searchKeyword);
                        WalkinServicePageLbl.Text = $"{servicecurrentPagefake} / {servicetotalPages}";
                    }
                }
            }
            else
            {
                string searchKeyword = RecApptSearchServiceTypeText.Text.Trim().ToLower();
                if (string.IsNullOrEmpty(searchKeyword))
                {
                    if (servicecurrentPagefake == 1)
                    {
                        servicecurrentPagefake = servicetotalPages;
                        servicecurrentPage = servicetotalPages - 1;
                        InitializeServices(filterstaffbyservicecategory);
                        ApptServicePageLbl.Text = $"{servicecurrentPagefake} / {servicetotalPages}";
                    }
                    else if (servicecurrentPagefake <= servicetotalPages)
                    {
                        servicecurrentPage--;
                        servicecurrentPagefake--;
                        InitializeServices(filterstaffbyservicecategory);
                        ApptServicePageLbl.Text = $"{servicecurrentPagefake} / {servicetotalPages}";
                    }
                }
                else
                {
                    if (servicecurrentPagefake == 1)
                    {
                        servicecurrentPagefake = servicetotalPages;
                        servicecurrentPage = servicetotalPages - 1;
                        InitializeServices3(filterstaffbyservicecategory, searchKeyword);
                        ApptServicePageLbl.Text = $"{servicecurrentPagefake} / {servicetotalPages}";
                    }
                    else if (servicecurrentPagefake <= servicetotalPages)
                    {
                        servicecurrentPage--;
                        servicecurrentPagefake--;
                        InitializeServices3(filterstaffbyservicecategory, searchKeyword);
                        ApptServicePageLbl.Text = $"{servicecurrentPagefake} / {servicetotalPages}";
                    }
                }
            }
        }

        private void WalkinServiceNextButton_Click(object sender, EventArgs e)
        {
            if (walkinservices)
            {
                string searchKeyword = RecWalkinSearchServiceTypeText.Text.Trim().ToLower();

                if (string.IsNullOrEmpty(searchKeyword))
                {
                    if (servicecurrentPagefake == servicetotalPages)
                    {
                        servicecurrentPage = 0;
                        servicecurrentPagefake = 0;
                        servicecurrentPagefake++;
                        InitializeServices(filterstaffbyservicecategory);
                        WalkinServicePageLbl.Text = $"{servicecurrentPagefake} / {servicetotalPages}";
                    }
                    else
                    {
                        servicecurrentPage++;
                        servicecurrentPagefake++;
                        InitializeServices(filterstaffbyservicecategory);
                        WalkinServicePageLbl.Text = $"{servicecurrentPagefake} / {servicetotalPages}";
                    }
                }
                else
                {
                    if (servicecurrentPagefake == servicetotalPages)
                    {
                        servicecurrentPage = 0;
                        servicecurrentPagefake = 0;
                        servicecurrentPagefake++;
                        InitializeServices3(filterstaffbyservicecategory, searchKeyword);
                        WalkinServicePageLbl.Text = $"{servicecurrentPagefake} / {servicetotalPages}";
                    }
                    else
                    {
                        servicecurrentPage++;
                        servicecurrentPagefake++;
                        InitializeServices3(filterstaffbyservicecategory, searchKeyword);
                        WalkinServicePageLbl.Text = $"{servicecurrentPagefake} / {servicetotalPages}";
                    }

                }

            }
            else
            {
                string searchKeyword = RecApptSearchServiceTypeText.Text.Trim().ToLower();

                if (string.IsNullOrEmpty(searchKeyword))
                {
                    if (servicecurrentPagefake == servicetotalPages)
                    {
                        servicecurrentPage = 0;
                        servicecurrentPagefake = 0;
                        servicecurrentPagefake++;
                        InitializeServices(filterstaffbyservicecategory);
                        ApptServicePageLbl.Text = $"{servicecurrentPagefake} / {servicetotalPages}";
                    }
                    else
                    {
                        servicecurrentPage++;
                        servicecurrentPagefake++;
                        InitializeServices(filterstaffbyservicecategory);
                        ApptServicePageLbl.Text = $"{servicecurrentPagefake} / {servicetotalPages}";
                    }
                }
                else
                {
                    if (servicecurrentPagefake == servicetotalPages)
                    {
                        servicecurrentPage = 0;
                        servicecurrentPagefake = 0;
                        servicecurrentPagefake++;
                        InitializeServices3(filterstaffbyservicecategory, searchKeyword);
                        ApptServicePageLbl.Text = $"{servicecurrentPagefake} / {servicetotalPages}";
                    }
                    else
                    {
                        servicecurrentPage++;
                        servicecurrentPagefake++;
                        InitializeServices3(filterstaffbyservicecategory, searchKeyword);
                        ApptServicePageLbl.Text = $"{servicecurrentPagefake} / {servicetotalPages}";
                    }

                }

            }

        }

        private void ApptServiceNextBtn_Click(object sender, EventArgs e)
        {
            WalkinServiceNextButton_Click(sender, e);
        }

        private void ApptServicePreviousBtn_Click(object sender, EventArgs e)
        {
            WalkinServicePreviousButton_Click(sender, e);
        }
    }
}