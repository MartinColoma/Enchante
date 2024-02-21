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
    public partial class StaffCurrentAvailableCustomersUserControl : UserControl
    {
        public StaffCurrentAvailableCustomersUserControl()
        {
            InitializeComponent();
        }
        public static string mysqlconn = "server=localhost;user=root;database=enchante;password=";
        public event EventHandler StartServiceButtonClicked;
        public event EventHandler ExpandUserControlButtonClicked;
        public event EventHandler StaffEndServiceBtnClicked;
        private System.Windows.Forms.Timer timer;
        private TimeSpan elapsedTime;
        private TimeSpan lastElapsedTime;
        private System.Diagnostics.Stopwatch stopwatch;
        private bool viewing = false;

        public bool Viewing
        {
            get { return viewing; }
            set { viewing = value; }
        }

        public TimeSpan GetElapsedTime()
        {
            return elapsedTime;
        }

        private void StaffCurrentServicesDropDownBtn_Click(object sender, EventArgs e)
        {
            viewing = !viewing;

            if (ExpandUserControlButtonClicked != null)
            {
                ExpandUserControlButtonClicked(this, EventArgs.Empty);
            }
        }

        public void AvailableCustomerSetData(Enchante.PendingCustomers customer)
        {
            StaffTransactionIDTextBox.Text = customer.TransactionNumber;
            StaffCustomerServiceNameSelectedTextBox.Text = customer.ServiceName;
            StaffCustomerServiceStatusTextBox.Text = customer.ServiceStatus;
            StaffCustomerNameTextBox.Text = customer.ClientName;
            StaffCustomerCustomizationsTextBox.Text = customer.CustomerCustomizations;
            StaffAdditionalNotesTextBox.Text = customer.AdditionalNotes;
        }


        public void StartTimer()
        {
            stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            StaffCustomerServiceStatusTextBox.Text = "In Session";
            StaffStartServiceBtn.Enabled = false;
            StaffUpdateServiceStatusOfCustomerinDB("In Session");
            timer.Start();
        }
        private void StopTimer()
        {
            if (timer != null && timer.Enabled)
            {
                timer.Tick -= Timer_Tick;
                timer.Stop();
                timer.Dispose();
                stopwatch.Stop();
                lastElapsedTime = elapsedTime;
                timer = null;
                stopwatch = null;
            }
        }

        private void StaffUpdateServiceStatusOfCustomerinDB(string UpdatedServiceStatus)
        {
            string transactionID = StaffTransactionIDTextBox.Text;

            using (MySqlConnection connection = new MySqlConnection(mysqlconn))
            {
                connection.Open();

                string updateQuery = "UPDATE walk_in_appointment SET ServiceStatus = @ServiceStatus WHERE TransactionNumber = @TransactionNumber";

                using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@ServiceStatus", UpdatedServiceStatus);
                    command.Parameters.AddWithValue("@TransactionNumber", transactionID);
                    command.ExecuteNonQuery();
                }
            }
        }
        private void StaffStartServiceBtn_Click(object sender, EventArgs e)
        {
            StartTimer();
            StartServiceButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (timer == null || timer.Enabled == false)
            {
                return; // Timer is already disposed or stopped
            }

            elapsedTime = elapsedTime.Add(TimeSpan.FromSeconds(1));

            // Display the updated elapsed time in the textbox
            if (StaffElapsedTimeTextBox.InvokeRequired)
            {
                StaffElapsedTimeTextBox.Invoke(new Action(() =>
                {
                    StaffElapsedTimeTextBox.Text = elapsedTime.ToString(@"hh\:mm\:ss");
                }));
            }
            else
            {
                StaffElapsedTimeTextBox.Text = elapsedTime.ToString(@"hh\:mm\:ss");
            }
        }

        private void StaffEndServiceBtn_Click(object sender, EventArgs e)
        {
            StopTimer();
            StaffElapsedTimeTextBox.Text = lastElapsedTime.ToString(@"hh\:mm\:ss");
            StaffCustomerServiceStatusTextBox.Text = "Completed";
            StaffEndServiceBtn.Enabled = false;
            StaffUpdateServiceStatusOfCustomerinDB("Completed");
        }



    }
}
