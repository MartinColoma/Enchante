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
        public static string mysqlconn = "server=localhost;user=root;database=enchante;password=";
        public event EventHandler StartServiceButtonClicked;
        public event EventHandler ExpandUserControlButtonClicked;
        public event EventHandler StaffEndServiceBtnClicked;
        private System.Windows.Forms.Timer timer;
        private TimeSpan elapsedTime = TimeSpan.Zero;
        private TimeSpan lastElapsedTime;
        private System.Diagnostics.Stopwatch stopwatch;
        private bool viewing = false;
        private Enchante EnchanteForm;

        public StaffCurrentAvailableCustomersUserControl(Enchante EnchanteForm)
        {
            InitializeComponent();
            StaffEndServiceBtn.Enabled = false;
            this.EnchanteForm = EnchanteForm;
        }
        

        public bool Viewing
        {
            get { return viewing; }
            set { viewing = value; }
        }

        public TimeSpan GetElapsedTime()
        {
            return elapsedTime;
        }

        public void AvailableCustomerSetData(Enchante.PendingCustomers customer)
        {
            StaffTransactionIDTextBox.Text = customer.TransactionNumber;
            StaffCustomerServiceNameSelectedTextBox.Text = customer.ServiceName;
            StaffCustomerServiceStatusTextBox.Text = customer.ServiceStatus;
            StaffCustomerNameTextBox.Text = "Client Name: " + customer.ClientName;

            StaffServiceIDTextBox.Text = customer.ServiceID;
            StaffQueNumberTextBox.Text = customer.QueNumber;
        }

        public void AvailablePriorityCustomerSetData(Enchante.PriorityPendingCustomers customer)
        {
            StaffTransactionIDTextBox.Text = customer.TransactionNumber;
            StaffCustomerServiceNameSelectedTextBox.Text = customer.ServiceName;
            StaffCustomerServiceStatusTextBox.Text = customer.ServiceStatus;
            StaffCustomerNameTextBox.Text = "Client Name: " + customer.ClientName;
            StaffQueTypeTextBox.Text = customer.QueType;
            StaffServiceIDTextBox.Text = customer.ServiceID;
            StaffQueNumberTextBox.Text = customer.QueNumber;
        }

        public string CurrentStaffID { get; set; }

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
            string attenidingStaff = CurrentStaffID;
            string serviceID = StaffServiceIDTextBox.Text;
            string timeElapsed = StaffElapsedTimeTextBox.Text;
            string customerName = StaffCustomerNameTextBox.Text;
            string customerQueNumber = StaffQueNumberTextBox.Text;

            using (MySqlConnection connection = new MySqlConnection(mysqlconn))

            {
                connection.Open();

                if (UpdatedServiceStatus == "In Session")
                {
                    string updateQuery = "UPDATE walk_in_appointment SET ServiceStatus = @ServiceStatus WHERE TransactionNumber = @TransactionNumber";
                    string updateQuery2 = "UPDATE servicehistory SET ServiceStatus = @ServiceStatus, AttendingStaff = @AttendingStaff, ServiceStart = @ServiceStart WHERE TransactionNumber = @TransactionNumber AND ServiceID = @ServiceID";
                    string updateQuery3 = "UPDATE systemusers SET Availability = 'Unavailable', CurrentCustomerName = @CurrentCustomerName, CurrentCustomerQueNumber = @CurrentCustomerQueNumber WHERE EmployeeID = @EmployeeID";

                    using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@ServiceStatus", UpdatedServiceStatus);
                        command.Parameters.AddWithValue("@TransactionNumber", transactionID);
                        command.ExecuteNonQuery();
                    }
                    using (MySqlCommand command = new MySqlCommand(updateQuery2, connection))
                    {
                        command.Parameters.AddWithValue("@ServiceStatus", UpdatedServiceStatus);
                        command.Parameters.AddWithValue("@TransactionNumber", transactionID);
                        command.Parameters.AddWithValue("@AttendingStaff", attenidingStaff);
                        command.Parameters.AddWithValue("@ServiceID", serviceID);
                        command.Parameters.AddWithValue("@ServiceStart", DateTime.Now.ToString("HH:mm:ss"));
                        command.ExecuteNonQuery();
                    }
                    using (MySqlCommand command = new MySqlCommand(updateQuery3, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeID", attenidingStaff);
                        command.Parameters.AddWithValue("@CurrentCustomerName", customerName);
                        command.Parameters.AddWithValue("@CurrentCustomerQueNumber", customerQueNumber);
                        command.ExecuteNonQuery();
                    }
                }
                else if (UpdatedServiceStatus == "Completed")
                {
                    string updateQuery1 = "UPDATE servicehistory SET ServiceStatus = @ServiceStatus, ServiceEnd = @ServiceEnd, ServiceDuration = @ServiceDuration WHERE TransactionNumber = @TransactionNumber AND ServiceID = @ServiceID";
                    string updateQuery2 = "UPDATE systemusers SET Availability = 'Available', CurrentCustomerName = '', CurrentCustomerQueNumber = '' WHERE EmployeeID = @EmployeeID";
                    string updateQuery3 = "UPDATE walk_in_appointment SET ServiceStatus = @ServiceStatus, ServiceDuration = @ServiceDuration WHERE TransactionNumber = @TransactionNumber";


                    using (MySqlCommand command = new MySqlCommand(updateQuery1, connection))
                    {
                        command.Parameters.AddWithValue("@ServiceStatus", UpdatedServiceStatus);
                        command.Parameters.AddWithValue("@TransactionNumber", transactionID);
                        command.Parameters.AddWithValue("@ServiceID", serviceID);
                        command.Parameters.AddWithValue("@ServiceEnd", DateTime.Now.ToString("HH:mm:ss"));
                        command.Parameters.AddWithValue("@ServiceDuration", timeElapsed);

                        command.ExecuteNonQuery();
                    }

                    string countQuery = "SELECT COUNT(*) FROM servicehistory WHERE TransactionNumber = @TransactionNumber AND ServiceStatus = 'Pending' ";
                    int matchCount;

                    using (MySqlCommand command = new MySqlCommand(countQuery, connection))
                    {
                        command.Parameters.AddWithValue("@TransactionNumber", transactionID);
                        matchCount = Convert.ToInt32(command.ExecuteScalar());
                    }

                    string serviceStatus = (matchCount == 0) ? "Completed" : "Pending";

                    using (MySqlCommand command = new MySqlCommand(updateQuery3, connection))
                    {
                        command.Parameters.AddWithValue("@ServiceStatus", serviceStatus);
                        command.Parameters.AddWithValue("@TransactionNumber", transactionID);
                        command.Parameters.AddWithValue("@ServiceDuration", timeElapsed);

                        command.ExecuteNonQuery();
                    }

                    
                    using (MySqlCommand command = new MySqlCommand(updateQuery2, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeID", attenidingStaff);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        private void StaffStartServiceBtn_Click(object sender, EventArgs e)
        {
            StartServiceButtonClicked?.Invoke(this, EventArgs.Empty);
            StaffEndServiceBtn.Enabled = true;
            EnchanteForm.RemovePendingUserControls(this);
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
            if (Parent != null)
            {
                Parent.Controls.Remove(this);
            }
            EnchanteForm.RefreshFlowLayoutPanel();
        }

    }
}
