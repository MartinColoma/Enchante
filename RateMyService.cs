using FontAwesome.Sharp;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Enchante
{

    public partial class RateMyService : Form
    {
        private string[] stars = { "5-star (Exceptional)", "4-star (Great)", "3-star (Satisfactory)", "2-star (Mediocre)", "1-star (Awful)", "Select A Star" };
        public static string mysqlconn = "server=localhost;user=root;database=enchante;password=";
        private Enchante EnchanteForm;
        public string TransactionID { get; set; }
        public string StaffID { get; set; }

        public RateMyService(Enchante EnchanteForm, string transactionID, string staffID)
        {
            InitializeComponent();
            this.EnchanteForm = EnchanteForm;
            TransactionID = transactionID;
            StaffID = staffID;
            foreach (string star in stars)
            {
                RateMeStarBox.Items.Add(star);
            }
            RateMeStarBox.SelectedIndex = 5;
        }
        protected override void OnFormClosing(FormClosingEventArgs e)  //dont delete may function ito
        {
            base.OnFormClosing(e);
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true; // Cancel the close operation
                MessageBox.Show("Please rate the staff attending to you based on your experience.", "Rating Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void RateMeOne_Click(object sender, EventArgs e)
        {
            if (this.RateMeOne.IconFont == FontAwesome.Sharp.IconFont.Solid)
            {
                zero();
            }
            else
            {
                uno();
            }
        }

        private void uno()
        {
            this.RateMeOne.IconChar = FontAwesome.Sharp.IconChar.Star;
            this.RateMeOne.IconFont = FontAwesome.Sharp.IconFont.Solid;
            RateMeStarBox.SelectedIndex = 4;
            RateMeNumStarsLbl.Text = "1";

            this.RateMeTwo.IconChar = FontAwesome.Sharp.IconChar.Star;
            this.RateMeTwo.IconFont = FontAwesome.Sharp.IconFont.Regular;
            this.RateMeThree.IconChar = FontAwesome.Sharp.IconChar.Star;
            this.RateMeThree.IconFont = FontAwesome.Sharp.IconFont.Regular;
            this.RateMeFour.IconChar = FontAwesome.Sharp.IconChar.Star;
            this.RateMeFour.IconFont = FontAwesome.Sharp.IconFont.Regular;
            this.RateMeFive.IconChar = FontAwesome.Sharp.IconChar.Star;
            this.RateMeFive.IconFont = FontAwesome.Sharp.IconFont.Regular;
        }

        private void RateMeTwo_Click(object sender, EventArgs e)
        {
            if (this.RateMeTwo.IconFont == FontAwesome.Sharp.IconFont.Solid)
            {
                zero();
            }
            else
            {
                dos();
            }
        }
        private void dos()
        {
            this.RateMeOne.IconChar = FontAwesome.Sharp.IconChar.Star;
            this.RateMeOne.IconFont = FontAwesome.Sharp.IconFont.Solid;
            this.RateMeTwo.IconChar = FontAwesome.Sharp.IconChar.Star;
            this.RateMeTwo.IconFont = FontAwesome.Sharp.IconFont.Solid;
            RateMeStarBox.SelectedIndex = 3;
            RateMeNumStarsLbl.Text = "2";

            this.RateMeThree.IconChar = FontAwesome.Sharp.IconChar.Star;
            this.RateMeThree.IconFont = FontAwesome.Sharp.IconFont.Regular;
            this.RateMeFour.IconChar = FontAwesome.Sharp.IconChar.Star;
            this.RateMeFour.IconFont = FontAwesome.Sharp.IconFont.Regular;
            this.RateMeFive.IconChar = FontAwesome.Sharp.IconChar.Star;
            this.RateMeFive.IconFont = FontAwesome.Sharp.IconFont.Regular;
        }

        private void RateMeThree_Click(object sender, EventArgs e)
        {
            if (this.RateMeThree.IconFont == FontAwesome.Sharp.IconFont.Solid)
            {
                zero();
            }
            else
            {
                tres();
            }
        }

        private void tres()
        {
            this.RateMeOne.IconChar = FontAwesome.Sharp.IconChar.Star;
            this.RateMeOne.IconFont = FontAwesome.Sharp.IconFont.Solid;
            this.RateMeTwo.IconChar = FontAwesome.Sharp.IconChar.Star;
            this.RateMeTwo.IconFont = FontAwesome.Sharp.IconFont.Solid;
            this.RateMeThree.IconChar = FontAwesome.Sharp.IconChar.Star;
            this.RateMeThree.IconFont = FontAwesome.Sharp.IconFont.Solid;
            RateMeStarBox.SelectedIndex = 2;
            RateMeNumStarsLbl.Text = "3";

            this.RateMeFour.IconChar = FontAwesome.Sharp.IconChar.Star;
            this.RateMeFour.IconFont = FontAwesome.Sharp.IconFont.Regular;
            this.RateMeFive.IconChar = FontAwesome.Sharp.IconChar.Star;
            this.RateMeFive.IconFont = FontAwesome.Sharp.IconFont.Regular;
        }

        private void RateMeFour_Click(object sender, EventArgs e)
        {
            if (this.RateMeFour.IconFont == FontAwesome.Sharp.IconFont.Solid)
            {
                zero();
            }
            else
            {
                kwatro();
            }
        }
        private void kwatro()
        {
            this.RateMeOne.IconChar = FontAwesome.Sharp.IconChar.Star;
            this.RateMeOne.IconFont = FontAwesome.Sharp.IconFont.Solid;
            this.RateMeTwo.IconChar = FontAwesome.Sharp.IconChar.Star;
            this.RateMeTwo.IconFont = FontAwesome.Sharp.IconFont.Solid;
            this.RateMeThree.IconChar = FontAwesome.Sharp.IconChar.Star;
            this.RateMeThree.IconFont = FontAwesome.Sharp.IconFont.Solid;
            this.RateMeFour.IconChar = FontAwesome.Sharp.IconChar.Star;
            this.RateMeFour.IconFont = FontAwesome.Sharp.IconFont.Solid;
            RateMeStarBox.SelectedIndex = 1;
            RateMeNumStarsLbl.Text = "4";


            this.RateMeFive.IconChar = FontAwesome.Sharp.IconChar.Star;
            this.RateMeFive.IconFont = FontAwesome.Sharp.IconFont.Regular;
        }

        private void RateMeFive_Click(object sender, EventArgs e)
        {
            if (this.RateMeFive.IconFont == FontAwesome.Sharp.IconFont.Solid)
            {
                zero();
            }
            else
            {
                singko();
            }
        }
        private void singko()
        {
            this.RateMeOne.IconChar = FontAwesome.Sharp.IconChar.Star;
            this.RateMeOne.IconFont = FontAwesome.Sharp.IconFont.Solid;
            this.RateMeTwo.IconChar = FontAwesome.Sharp.IconChar.Star;
            this.RateMeTwo.IconFont = FontAwesome.Sharp.IconFont.Solid;
            this.RateMeThree.IconChar = FontAwesome.Sharp.IconChar.Star;
            this.RateMeThree.IconFont = FontAwesome.Sharp.IconFont.Solid;
            this.RateMeFour.IconChar = FontAwesome.Sharp.IconChar.Star;
            this.RateMeFour.IconFont = FontAwesome.Sharp.IconFont.Solid;
            this.RateMeFive.IconChar = FontAwesome.Sharp.IconChar.Star;
            this.RateMeFive.IconFont = FontAwesome.Sharp.IconFont.Solid;
            RateMeStarBox.SelectedIndex = 0;
            RateMeNumStarsLbl.Text = "5";
        }
        private void zero()
        {
            this.RateMeOne.IconChar = FontAwesome.Sharp.IconChar.Star;
            this.RateMeOne.IconFont = FontAwesome.Sharp.IconFont.Regular;
            this.RateMeTwo.IconChar = FontAwesome.Sharp.IconChar.Star;
            this.RateMeTwo.IconFont = FontAwesome.Sharp.IconFont.Regular;
            this.RateMeThree.IconChar = FontAwesome.Sharp.IconChar.Star;
            this.RateMeThree.IconFont = FontAwesome.Sharp.IconFont.Regular;
            this.RateMeFour.IconChar = FontAwesome.Sharp.IconChar.Star;
            this.RateMeFour.IconFont = FontAwesome.Sharp.IconFont.Regular;
            this.RateMeFive.IconChar = FontAwesome.Sharp.IconChar.Star;
            this.RateMeFive.IconFont = FontAwesome.Sharp.IconFont.Regular;
            RateMeNumStarsLbl.Text = "0";
            RateMeStarBox.SelectedIndex = 5;

        }

        private void RateMeStarBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(RateMeStarBox.SelectedIndex == 0)
            {
                singko();
                return;
            }
            else if (RateMeStarBox.SelectedIndex == 1)
            {
                kwatro();
                return;
            }
            else if (RateMeStarBox.SelectedIndex == 2)
            {
                tres();
                return;
            }
            else if (RateMeStarBox.SelectedIndex == 3)
            {
                dos();
                return;
            }
            else if (RateMeStarBox.SelectedIndex == 4)
            {
                uno();
                return;
            }
            else if (RateMeStarBox.SelectedIndex == 5)
            {
                zero();
                return;
            }
        }

        private void RateMeSubmitBtn_Click(object sender, EventArgs e)
        {
            // Assuming transactionID and staffID are properties or fields of your class
            string transactionID = TransactionID; // Ensure transactionID is properly assigned
            string attendingStaff = StaffID; // Ensure staffID is properly assigned

            string dateToday = DateTime.Now.ToString("MM-dd-yy dddd");

            string rating = RateMeNumStarsLbl.Text;

            try
            {
                using (MySqlConnection connection = new MySqlConnection(mysqlconn))
                {
                    connection.Open();

                    string query = "INSERT INTO staffrating (EmployeeID, TransactionID, Date, Rating) " +
                                   "VALUES (@EmployeeID, @TransactionID, @Date, @Rating)";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Set the parameter values
                        command.Parameters.AddWithValue("@EmployeeID", attendingStaff);
                        command.Parameters.AddWithValue("@TransactionID", transactionID);
                        command.Parameters.AddWithValue("@Date", dateToday);
                        command.Parameters.AddWithValue("@Rating", rating);

                        // Execute the query
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur during the database operation
                MessageBox.Show("An error occurred while inserting the rating: " + ex.Message);
            }

            this.Hide();
            RateMeStarBox.SelectedIndex = 5;
        }

    }
}
