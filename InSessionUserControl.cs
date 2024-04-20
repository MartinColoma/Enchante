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
    public partial class InSessionUserControl : UserControl
    {
        public static string mysqlconn = "server=localhost;user=root;database=enchante;password=";

        public event EventHandler StaffEndServiceBtnClicked;
        public event EventHandler StaffCancelServiceBtnClicked;

        public event EventHandler QueueUserControlEnd_Clicked;
        public event EventHandler StaffQueNumberTextBoxEnd_Clicked;
        public event EventHandler StaffCustomerNameTextBoxEnd_Clicked;
        public event EventHandler StaffElapsedTimeTextBoxEnd_Clicked;
        public event EventHandler StaffTransactionIDTextBoxEnd_Clicked;

        private System.Windows.Forms.Timer timer;
        private TimeSpan elapsedTime = TimeSpan.Zero;
        private TimeSpan lastElapsedTime;
        private System.Diagnostics.Stopwatch stopwatch;
        private bool viewing = false;
        private Enchante EnchanteForm;
        public string CurrentStaffID { get; set; }
        public string ControlID { get; set; }
        public TimeSpan GetElapsedTime()
        {
            return elapsedTime;
        }


        public InSessionUserControl(Enchante EnchanteForm)
        {
            InitializeComponent();
            this.EnchanteForm = EnchanteForm;
        }


        public void AvailableCustomerSetData(Enchante.InSessionCustomers customer)
        {
            StaffTransactionIDTextBox.Text = customer.TransactionNumber;
            StaffCustomerServiceNameSelectedTextBox.Text = customer.ServiceName;
            StaffCustomerServiceStatusTextBox.Text = customer.ServiceStatus;
            StaffCustomerNameTextBox.Text = "Client Name: " + customer.ClientName;
            StaffQueTypeTextBox.Text = customer.QueType;
            StaffServiceIDTextBox.Text = customer.ServiceID;
            StaffQueNumberTextBox.Text = customer.QueNumber;
            StaffCustomerAttendingStaffTextBox.Text = customer.AttendingStaff;
            CurrentStaffID = customer.AttendingStaff;
        }


        public void StartTimer()
        {
            stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
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
            string attenidingStaff = StaffCustomerAttendingStaffTextBox.Text;
            string serviceID = StaffServiceIDTextBox.Text;
            string timeElapsed = StaffElapsedTimeTextBox.Text;

            using (MySqlConnection connection = new MySqlConnection(mysqlconn))

            {
                connection.Open();


                if (UpdatedServiceStatus == "Completed")
                {
                    string updateQuery1 = "UPDATE servicehistory SET ServiceStatus = @ServiceStatus, ServiceEnd = @ServiceEnd, ServiceDuration = @ServiceDuration WHERE TransactionNumber = @TransactionNumber AND ServiceID = @ServiceID";
                    string updateQuery2 = "UPDATE systemusers SET Availability = 'Available', CurrentCustomerName = '', CurrentCustomerQueNumber = '' WHERE EmployeeID = @EmployeeID";
                    string updateQuery3 = "UPDATE walk_in_appointment SET ServiceStatus = @ServiceStatus, ServiceDuration = @ServiceDuration WHERE TransactionNumber = @TransactionNumber";
                    string updateQuery4 = "UPDATE appointment SET ServiceStatus = @ServiceStatus, ServiceDuration = @ServiceDuration WHERE TransactionNumber = @TransactionNumber";

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

                    using (MySqlCommand command = new MySqlCommand(updateQuery4, connection))
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
                else if (UpdatedServiceStatus == "Completed Paid")
                {
                    string updateQuery1 = "UPDATE servicehistory SET ServiceStatus = @ServiceStatus, ServiceEnd = @ServiceEnd, ServiceDuration = @ServiceDuration WHERE TransactionNumber = @TransactionNumber AND ServiceID = @ServiceID";
                    string updateQuery2 = "UPDATE systemusers SET Availability = 'Available', CurrentCustomerName = '', CurrentCustomerQueNumber = '' WHERE EmployeeID = @EmployeeID";
                    string updateQuery3 = "UPDATE walk_in_appointment SET ServiceStatus = @ServiceStatus, ServiceDuration = @ServiceDuration WHERE TransactionNumber = @TransactionNumber";
                    string updateQuery4 = "UPDATE appointment SET ServiceStatus = @ServiceStatus, ServiceDuration = @ServiceDuration WHERE TransactionNumber = @TransactionNumber";

                    using (MySqlCommand command = new MySqlCommand(updateQuery1, connection))
                    {
                        command.Parameters.AddWithValue("@ServiceStatus", UpdatedServiceStatus);
                        command.Parameters.AddWithValue("@TransactionNumber", transactionID);
                        command.Parameters.AddWithValue("@ServiceID", serviceID);
                        command.Parameters.AddWithValue("@ServiceEnd", DateTime.Now.ToString("HH:mm:ss"));
                        command.Parameters.AddWithValue("@ServiceDuration", timeElapsed);

                        command.ExecuteNonQuery();
                    }

                    string countQuery = "SELECT COUNT(*) FROM servicehistory WHERE TransactionNumber = @TransactionNumber AND ServiceStatus = 'Pending Paid' ";
                    int matchCount;

                    using (MySqlCommand command = new MySqlCommand(countQuery, connection))
                    {
                        command.Parameters.AddWithValue("@TransactionNumber", transactionID);
                        matchCount = Convert.ToInt32(command.ExecuteScalar());
                    }

                    string serviceStatus = (matchCount == 0) ? "Paid" : "Pending Paid";

                    using (MySqlCommand command = new MySqlCommand(updateQuery3, connection))
                    {
                        command.Parameters.AddWithValue("@ServiceStatus", serviceStatus);
                        command.Parameters.AddWithValue("@TransactionNumber", transactionID);
                        command.Parameters.AddWithValue("@ServiceDuration", timeElapsed);

                        command.ExecuteNonQuery();
                    }

                    using (MySqlCommand command = new MySqlCommand(updateQuery4, connection))
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
                else if (UpdatedServiceStatus == "Cancelled")
                {
                    string updateQuery1 = "UPDATE servicehistory SET ServiceStatus = @ServiceStatus, ServiceEnd = @ServiceEnd, ServiceDuration = @ServiceDuration WHERE TransactionNumber = @TransactionNumber AND ServiceID = @ServiceID";
                    string updateQuery2 = "UPDATE systemusers SET Availability = 'Available', CurrentCustomerName = '', CurrentCustomerQueNumber = '' WHERE EmployeeID = @EmployeeID";
                    string updateQuery3 = "UPDATE walk_in_appointment SET ServiceStatus = @ServiceStatus, ServiceDuration = @ServiceDuration WHERE TransactionNumber = @TransactionNumber";
                    string updateQuery4 = "UPDATE appointment SET ServiceStatus = @ServiceStatus, ServiceDuration = @ServiceDuration WHERE TransactionNumber = @TransactionNumber";

                    using (MySqlCommand command = new MySqlCommand(updateQuery1, connection))
                    {
                        command.Parameters.AddWithValue("@ServiceStatus", UpdatedServiceStatus);
                        command.Parameters.AddWithValue("@TransactionNumber", transactionID);
                        command.Parameters.AddWithValue("@ServiceID", serviceID);
                        command.Parameters.AddWithValue("@ServiceEnd", DateTime.Now.ToString("HH:mm:ss"));
                        command.Parameters.AddWithValue("@ServiceDuration", timeElapsed);

                        command.ExecuteNonQuery();
                    }


                    string countQuery = "SELECT COUNT(*) FROM servicehistory WHERE TransactionNumber = @TransactionNumber AND (ServiceStatus = 'Pending Paid' OR ServiceStatus = 'Pending') ";
                    int matchCount;
                    string serviceStatus = null;

                    using (MySqlCommand command = new MySqlCommand(countQuery, connection))
                    {
                        command.Parameters.AddWithValue("@TransactionNumber", transactionID);
                        matchCount = Convert.ToInt32(command.ExecuteScalar());

                        if (matchCount == 0)
                        {
                            string completedStatusQuery = "SELECT ServiceStatus FROM servicehistory WHERE TransactionNumber = @TransactionNumber AND (ServiceStatus = 'Completed' OR ServiceStatus = 'Completed Paid')";

                            using (MySqlCommand completedStatusCommand = new MySqlCommand(completedStatusQuery, connection))
                            {
                                completedStatusCommand.Parameters.AddWithValue("@TransactionNumber", transactionID);
                                object completedStatusResult = completedStatusCommand.ExecuteScalar();

                                if (completedStatusResult != null)
                                {
                                    string statusQuery = "SELECT ServiceStatus FROM servicehistory WHERE TransactionNumber = @TransactionNumber AND (ServiceStatus = 'Completed' OR ServiceStatus = 'Completed Paid')";

                                    using (MySqlCommand statusCommand = new MySqlCommand(statusQuery, connection))
                                    {
                                        statusCommand.Parameters.AddWithValue("@TransactionNumber", transactionID);
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
                                statusCommand.Parameters.AddWithValue("@TransactionNumber", transactionID);
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
                        command.Parameters.AddWithValue("@TransactionNumber", transactionID);
                        command.Parameters.AddWithValue("@ServiceDuration", timeElapsed);

                        command.ExecuteNonQuery();
                    }

                    using (MySqlCommand command = new MySqlCommand(updateQuery4, connection))
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
            if (StaffCustomerServiceStatusTextBox.Text == "In Session Paid")
            {
                StaffCustomerServiceStatusTextBox.Text = "Completed Paid";
            }
            else if (StaffCustomerServiceStatusTextBox.Text == "In Session")
            {
                StaffCustomerServiceStatusTextBox.Text = "Completed";
            }
            StaffUpdateServiceStatusOfCustomerinDB(StaffCustomerServiceStatusTextBox.Text);
            if (Parent != null)
            {
                Parent.Controls.Remove(this);
            }

            EnchanteForm.RefreshFlowLayoutPanel();
        }



        private void StaffCancelServiceBtn_Click(object sender, EventArgs e)
        {
            StaffUpdateServiceStatusOfCustomerinDB("Cancelled");
            if (Parent != null)
            {
                Parent.Controls.Remove(this);
            }
            EnchanteForm.RefreshFlowLayoutPanel();
        }

        private void StaffElapsedTimeTextBox_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to end this service?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                StopTimer();
                StaffElapsedTimeTextBox.Text = lastElapsedTime.ToString(@"hh\:mm\:ss");
                if (StaffCustomerServiceStatusTextBox.Text == "In Session Paid")
                {
                    StaffCustomerServiceStatusTextBox.Text = "Completed Paid";
                }
                else if (StaffCustomerServiceStatusTextBox.Text == "In Session")
                {
                    StaffCustomerServiceStatusTextBox.Text = "Completed";
                }
                StaffUpdateServiceStatusOfCustomerinDB(StaffCustomerServiceStatusTextBox.Text);
                if (Parent != null)
                {
                    Parent.Controls.Remove(this);
                }
                EnchanteForm.InitializeMainInventory();
                EnchanteForm.RefreshFlowLayoutPanel();
                EnchanteForm.RefreshAvailableStaff();
            }
            else
            {
                return;
            }

        }

        private void StaffQueNumberTextBox_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to end this service?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                StopTimer();
                StaffElapsedTimeTextBox.Text = lastElapsedTime.ToString(@"hh\:mm\:ss");
                if (StaffCustomerServiceStatusTextBox.Text == "In Session Paid")
                {
                    StaffCustomerServiceStatusTextBox.Text = "Completed Paid";
                }
                else if (StaffCustomerServiceStatusTextBox.Text == "In Session")
                {
                    StaffCustomerServiceStatusTextBox.Text = "Completed";
                }
                StaffUpdateServiceStatusOfCustomerinDB(StaffCustomerServiceStatusTextBox.Text);
                if (Parent != null)
                {
                    Parent.Controls.Remove(this);
                }
                EnchanteForm.InitializeMainInventory();
                EnchanteForm.RefreshFlowLayoutPanel();
                EnchanteForm.RefreshAvailableStaff();
            }
            else
            {
                return;
            }
        }

        private void StaffCancelServiceBtn_Click_1(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to cancel this service?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                StaffUpdateServiceStatusOfCustomerinDB("Cancelled");
                if (Parent != null)
                {
                    Parent.Controls.Remove(this);
                }
                EnchanteForm.RefreshFlowLayoutPanel();
                EnchanteForm.RefreshAvailableStaff();
            }
            else
            {
                return;
            }
        }

        private void StaffCustomerNameTextBox_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to end this service?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                StopTimer();
                StaffElapsedTimeTextBox.Text = lastElapsedTime.ToString(@"hh\:mm\:ss");
                if (StaffCustomerServiceStatusTextBox.Text == "In Session Paid")
                {
                    StaffCustomerServiceStatusTextBox.Text = "Completed Paid";
                }
                else if (StaffCustomerServiceStatusTextBox.Text == "In Session")
                {
                    StaffCustomerServiceStatusTextBox.Text = "Completed";
                }
                StaffUpdateServiceStatusOfCustomerinDB(StaffCustomerServiceStatusTextBox.Text);
                if (Parent != null)
                {
                    Parent.Controls.Remove(this);
                }
                EnchanteForm.InitializeMainInventory();
                EnchanteForm.RefreshFlowLayoutPanel();
                EnchanteForm.RefreshAvailableStaff();
            }
            else
            {
                return;
            }
        }

        private void StaffTransactionIDTextBox_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to end this service?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                StopTimer();
                StaffElapsedTimeTextBox.Text = lastElapsedTime.ToString(@"hh\:mm\:ss");
                if (StaffCustomerServiceStatusTextBox.Text == "In Session Paid")
                {
                    StaffCustomerServiceStatusTextBox.Text = "Completed Paid";
                }
                else if (StaffCustomerServiceStatusTextBox.Text == "In Session")
                {
                    StaffCustomerServiceStatusTextBox.Text = "Completed";
                }
                StaffUpdateServiceStatusOfCustomerinDB(StaffCustomerServiceStatusTextBox.Text);
                if (Parent != null)
                {
                    Parent.Controls.Remove(this);
                }
                EnchanteForm.InitializeMainInventory();
                EnchanteForm.RefreshFlowLayoutPanel();
                EnchanteForm.RefreshAvailableStaff();
            }
            else
            {
                return;
            }
        }

        private void StaffQueTypeTextBox_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to end this service?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                StopTimer();
                StaffElapsedTimeTextBox.Text = lastElapsedTime.ToString(@"hh\:mm\:ss");
                if (StaffCustomerServiceStatusTextBox.Text == "In Session Paid")
                {
                    StaffCustomerServiceStatusTextBox.Text = "Completed Paid";
                }
                else if (StaffCustomerServiceStatusTextBox.Text == "In Session")
                {
                    StaffCustomerServiceStatusTextBox.Text = "Completed";
                }
                StaffUpdateServiceStatusOfCustomerinDB(StaffCustomerServiceStatusTextBox.Text);
                if (Parent != null)
                {
                    Parent.Controls.Remove(this);
                }
                EnchanteForm.InitializeMainInventory();
                EnchanteForm.RefreshFlowLayoutPanel();
                EnchanteForm.RefreshAvailableStaff();
            }
            else
            {
                return;
            }
        }

        private void InSessionUserControl_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to end this service?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                StopTimer();
                StaffElapsedTimeTextBox.Text = lastElapsedTime.ToString(@"hh\:mm\:ss");
                if (StaffCustomerServiceStatusTextBox.Text == "In Session Paid")
                {
                    StaffCustomerServiceStatusTextBox.Text = "Completed Paid";
                }
                else if (StaffCustomerServiceStatusTextBox.Text == "In Session")
                {
                    StaffCustomerServiceStatusTextBox.Text = "Completed";
                }
                StaffUpdateServiceStatusOfCustomerinDB(StaffCustomerServiceStatusTextBox.Text);
                if (Parent != null)
                {
                    Parent.Controls.Remove(this);
                }
                EnchanteForm.InitializeMainInventory();
                EnchanteForm.RefreshFlowLayoutPanel();
                EnchanteForm.RefreshAvailableStaff();
            }
            else
            {
                return;
            }
        }


    }
}
