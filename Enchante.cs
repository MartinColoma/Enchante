using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

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


        //tool tip
        private System.Windows.Forms.ToolTip iconToolTip;


        //gender combo box
        private string[] genders = { "Male", "Female", "Prefer Not to Say" };



        public Enchante()
        {
            InitializeComponent();
            
            // Exit MessageBox 
            this.FormClosing += new FormClosingEventHandler(MainForm_FormClosing);



            //Landing Pages Cardlayout Panel Manager
            ParentPanelShow = new ParentCard(EnchanteHomePage, EnchanteStaffPage, EnchanteMngrPage, EnchanteMemberPage,EnchanteAdminPage);
            Registration = new Registration(MembershipPlanPanel, RegularPlanPanel, PremiumPlanPanel, SVIPPlanPanel);
            Service = new ServiceCard(ServiceType, ServiceHairStyling, ServiceFaceSkin, ServiceNailCare, ServiceSpa, ServiceMassage);


            //icon tool tip
            iconToolTip = new System.Windows.Forms.ToolTip();


            //icon tool tip
            iconToolTip = new System.Windows.Forms.ToolTip();

            //gender combobox
            RegularGenderComboText.Items.AddRange(genders);
            RegularGenderComboText.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        private void Enchante_Load(object sender, EventArgs e)
        {
            //Reset Panel to Show Default
            HomePanelReset();
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
            if(LoginPassText.UseSystemPasswordChar == true)
            {
                LoginPassText.UseSystemPasswordChar = false;
                ShowHidePassBtn.IconChar = FontAwesome.Sharp.IconChar.EyeSlash;
            }
            else if(LoginPassText.UseSystemPasswordChar == false)
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
                ParentPanelShow.PanelShow(EnchanteAdminPage);
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
                ParentPanelShow.PanelShow(EnchanteMngrPage);
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
            else if (LoginEmailAddText.Text == "Staff" && LoginPassText.Text == "Staff123")
            {
                //Test Staff
                MessageBox.Show("Welcome back, Staff.", "Login Verified", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ParentPanelShow.PanelShow(EnchanteStaffPage);
                LoginEmailAddErrorLbl.Visible = false;
                LoginPassErrorLbl.Visible = false;
                logincredclear();

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
                ParentPanelShow.PanelShow(EnchanteMemberPage);

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
            }
        }

        private void logincredclear()
        {
            LoginEmailAddText.Text = "";
            LoginPassText.Text = "";

        }

        private void MemberSignOut_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to sign out user?", "Sign Out Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                EnchanteLoginForm.Visible = false;
                ParentPanelShow.PanelShow(EnchanteHomePage);
            }
        }

        private void MngrSignOutBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to sign out user?", "Sign Out Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                EnchanteLoginForm.Visible = false;
                ParentPanelShow.PanelShow(EnchanteHomePage);
            }
        }

        private void StaffSignOutBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to sign out user?", "Sign Out Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                EnchanteLoginForm.Visible = false;
                ParentPanelShow.PanelShow(EnchanteHomePage);
            }
        }

        private void AdminSignOutBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to sign out user?", "Sign Out Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                EnchanteLoginForm.Visible = false;
                ParentPanelShow.PanelShow(EnchanteHomePage);
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
        }

        private void SVIPMemberCreateAccBtn_Click(object sender, EventArgs e)
        {
            Registration.PanelShow(SVIPPlanPanel);
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
        }

        private void RegularGenderComboText_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RegularGenderComboText.SelectedItem != null)
            {
                RegularGenderComboText.Text = RegularGenderComboText.SelectedItem.ToString();
            }
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
            RegularAccIDNumberText.Text = "";

            // Call the GenerateClientID method using the type name
            string generatedClientID = RegularClientIDGenerator.GenerateClientID();

            RegularAccIDNumberText.Text = generatedClientID;
        }

        private void RegularCreateAccBtn_Click(object sender, EventArgs e)
        {
            DateTime selectedDate = RegularBdayPicker.Value;
            DateTime currentDate = DateTime.Now;

            string rCreated = currentDate.ToString("MM-dd-yyyy");
            string rStatus = "Active";
            string rType = "Regular";
            string rFirstname = RegularFirstNameText.Text;
            string rLastname = RegularLastNameText.Text;
            string rBday = selectedDate.ToString("MM-dd-yyyy");
            string rAge = RegularAgeText.Text;
            string rGender = RegularGenderComboText.Text;
            string rNumber = RegularMobileNumText.Text;
            string rEmailAdd = RegularEmailText.Text;
            string rMemberID = RegularAccIDNumberText.Text;
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
            else if (rPass!=rConfirmPass)
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

                        string insertQuery = "INSERT INTO membershipaccount (MembershipType, MemberIDNumber, AccountStatus, FirstName, " +
                            "LastName, Birthday, Age, CPNumber, EmailAdd, HashedPass, SaltedPass, UserSaltedPass, AccountCreated) " +
                            "VALUES (@type, @ID, @status, @firstName, @lastName, @bday, @age, @cpnum, @email, @hashedpass, @saltedpass, @usersaltedpass, @created)"; 



                        //string insertQuery = "INSERT INTO membershipaccount (MembershipType, MemberIDNumber, AccountStatus, FirstName, " +
                        //    "LastName, Birthday, Age, CPNumber, EmailAdd, HashedPass, SaltedPass, UserSaltedPass, PlanPeriod, " +
                        //    "PaymentType, Cardholder Name, CardNumber, CardExpiration, CVCCode, AccountCreated, PlanExpiration, PlanRenewal) " +
                        //    "VALUES (@type, @ID, @status, @firstName, @lastName, @bday, @age, @cpnum, @email, @hashedpass, @saltedpass, @usersaltedpass, " +
                        //    "@period, @payment, @cardname, @cardnumber, @cardexpiration, @cvc, @created, @planExpiration, @planRenew)";

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
            RegularAccIDNumberText.Text = "";
            RegularPassText.Text = "";
            RegularConfirmPassText.Text = "";


        }


              
    }
}
