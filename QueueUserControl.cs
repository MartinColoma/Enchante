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
    public partial class QueueUserControl : UserControl
    {
        public static string mysqlconn = "server=localhost;user=root;database=enchante;password=";
        public event EventHandler StartServiceButtonClicked;

        // startting service
        public event EventHandler QueueUserControl_Clicked;
        public event EventHandler StaffQueNumberTextBox_Clicked;
        public event EventHandler StaffCustomerNameTextBox_Clicked;
        public event EventHandler StaffElapsedTimeTextBox_Clicked;
        public event EventHandler StaffTransactionIDTextBox_Clicked;


        public event EventHandler StaffCancelServiceBtnClicked;
        private bool viewing = false;
        private Enchante EnchanteForm;

        public QueueUserControl(Enchante EnchanteForm)
        {
            InitializeComponent();
            this.EnchanteForm = EnchanteForm;
        }

        public bool Viewing
        {
            get { return viewing; }
            set { viewing = value; }
        }


        public void AvailableCustomerSetData(Enchante.PendingCustomers customer)
        {
            StaffTransactionIDTextBox.Text = customer.TransactionNumber;
            StaffCustomerServiceNameSelectedTextBox.Text = customer.ServiceName;
            StaffCustomerServiceStatusTextBox.Text = customer.ServiceStatus;
            StaffCustomerNameTextBox.Text = customer.ClientName;
            StaffQueTypeTextBox.Text = customer.QueType;
            StaffServiceIDTextBox.Text = customer.ServiceID;
            StaffQueNumberTextBox.Text = customer.QueNumber;
        }

        public void AvailablePriorityCustomerSetData(Enchante.PriorityPendingCustomers customer)
        {
            StaffTransactionIDTextBox.Text = customer.TransactionNumber;
            StaffCustomerServiceNameSelectedTextBox.Text = customer.ServiceName;
            StaffCustomerServiceStatusTextBox.Text = customer.ServiceStatus;
            StaffCustomerNameTextBox.Text = customer.ClientName;
            StaffQueTypeTextBox.Text = customer.QueType;
            StaffServiceIDTextBox.Text = customer.ServiceID;
            StaffQueNumberTextBox.Text = customer.QueNumber;
        }


        public string CurrentStaffID { get; set; }





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

                if (UpdatedServiceStatus == "In Session" || UpdatedServiceStatus == "In Session Paid")
                {
                    string updateQueryWalkInTransaction = "UPDATE walk_in_appointment SET ServiceStatus = @ServiceStatus WHERE TransactionNumber = @TransactionNumber";
                    string updateQueryAppointment = "UPDATE appointment SET ServiceStatus = @ServiceStatus WHERE TransactionNumber = @TransactionNumber";
                    string updateQuery2 = "UPDATE servicehistory SET ServiceStatus = @ServiceStatus, AttendingStaff = @AttendingStaff, ServiceStart = @ServiceStart WHERE TransactionNumber = @TransactionNumber AND ServiceID = @ServiceID";
                    string updateQuery3 = "UPDATE systemusers SET Availability = 'Unavailable', CurrentCustomerName = @CurrentCustomerName, CurrentCustomerQueNumber = @CurrentCustomerQueNumber WHERE EmployeeID = @EmployeeID";

                    using (MySqlCommand command = new MySqlCommand(updateQueryAppointment, connection))
                    {
                        command.Parameters.AddWithValue("@ServiceStatus", UpdatedServiceStatus);
                        command.Parameters.AddWithValue("@TransactionNumber", transactionID);
                        command.ExecuteNonQuery();
                    }
                    using (MySqlCommand command = new MySqlCommand(updateQueryWalkInTransaction, connection))
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


        private void StaffCancelServiceBtn_Click(object sender, EventArgs e)
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


        private void StaffStartServiceBtn_Click(object sender, EventArgs e)
        {


        }

        int matchedRowCount = 0;
        public bool CheckIfInventoryIsEnoughForService(string serviceID, DataGridView RecQueStartInventoryDGV)
        {

            if (RecQueStartInventoryDGV.Rows.Count == 0)
            {
                return false;
            }

            string query = "SELECT RequiredItem, NumOfItems FROM services WHERE ServiceID = @serviceID";


            using (MySqlConnection connection = new MySqlConnection(mysqlconn))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@serviceID", serviceID);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string requiredItemString = reader.GetString("RequiredItem");
                            string numOfItemsString = reader.GetString("NumOfItems");

                            string[] requiredItemsArray = requiredItemString.Split(',');
                            string[] numOfItemsArray = numOfItemsString.Split(',');

                            if (requiredItemsArray.Length != numOfItemsArray.Length)
                            {
                                return false;
                            }

                            var requiredItemsDict = new Dictionary<string, int>();

                            for (int i = 0; i < requiredItemsArray.Length; i++)
                            {
                                string requiredItem = requiredItemsArray[i].Trim();
                                int numOfItem = int.Parse(numOfItemsArray[i].Trim());
                                requiredItemsDict.Add(requiredItem, numOfItem);
                            }
                            bool isEnoughInventory = false; // Flag to track inventory sufficiency

                            foreach (DataGridViewRow row in RecQueStartInventoryDGV.Rows)
                            {
                                string staffItemID = row.Cells["RecStaffItemID"].Value.ToString();
                                int staffItemStock = int.Parse(row.Cells["RecStaffItemStock"].Value.ToString());


                                if (requiredItemsDict.ContainsKey(staffItemID))
                                {
                                    int requiredItemQuantity = requiredItemsDict[staffItemID];

                                    if (requiredItemQuantity <= staffItemStock && staffItemStock != 0)
                                    {
                                        isEnoughInventory = true;
                                        matchedRowCount++;
                                    }
                                }


                            }

                            if (isEnoughInventory && matchedRowCount == requiredItemsDict.Count)
                            {
                                DeductFromStaffInventory(requiredItemsDict);
                                return true;
                            }
                            else
                            {
                                string requiredItemsMessage = "Required Items:\n";
                                foreach (var item in requiredItemsDict)
                                {
                                    requiredItemsMessage += $"{item.Key}: {item.Value}\n";
                                }
                                MessageBox.Show(requiredItemsMessage, "Inventory Insufficient", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return false;
                            }
                        }

                    }
                }
            }

            return false;
        }
        public void DeductFromStaffInventory(Dictionary<string, int> requiredItemsDict)
        {
            string staffID = CurrentStaffID;
            matchedRowCount = 0;
            using (MySqlConnection connection = new MySqlConnection(mysqlconn))
            {
                connection.Open();

                foreach (string staffItemID in requiredItemsDict.Keys)
                {
                    int requiredItemQuantity = requiredItemsDict[staffItemID];

                    string selectQuery = "SELECT ItemStock FROM inventory " +
                                         "WHERE ItemID = @StaffItemID";

                    MySqlCommand selectCommand = new MySqlCommand(selectQuery, connection);
                    selectCommand.Parameters.AddWithValue("@CurrentStaffID", CurrentStaffID);
                    selectCommand.Parameters.AddWithValue("@StaffItemID", staffItemID);

                    string staffItemStock = selectCommand.ExecuteScalar()?.ToString();

                    if (staffItemStock != null)
                    {
                        if (int.TryParse(staffItemStock, out int stock) && requiredItemQuantity <= stock)
                        {
                            string updateQuery = $"UPDATE inventory " +
                                                 $"SET ItemStock = @NewStock " +
                                                 $"WHERE ItemID = @StaffItemID";

                            MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection);
                            updateCommand.Parameters.AddWithValue("@NewStock", (stock - requiredItemQuantity).ToString());
                            updateCommand.Parameters.AddWithValue("@CurrentStaffID", CurrentStaffID);
                            updateCommand.Parameters.AddWithValue("@StaffItemID", staffItemID);

                            int rowsAffected = updateCommand.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show($"Deducted {requiredItemQuantity} items from staff item: {staffItemID}");
                                EnchanteForm.CheckItemStockPersonalStatus(staffItemID, staffID);
                                EnchanteForm.RecQueStartInventoryDGV.Rows.Clear();
                                EnchanteForm.InitializeMainInventory();
                            }
                            else
                            {
                                MessageBox.Show($"Failed to deduct items from staff item: {staffItemID}");
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Inventory is not enough for service. Staff item: {staffItemID}");
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Failed to retrieve staff item stock for: {staffItemID}");
                    }
                }
            }
        }

        public void QueueUserControl_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to start this service?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                string serviceID = StaffServiceIDTextBox.Text;
                if (CheckIfInventoryIsEnoughForService(serviceID, EnchanteForm.RecQueStartInventoryDGV) == true)
                {
                    StartServiceButtonClicked?.Invoke(this, EventArgs.Empty);
                    if (StaffCustomerServiceStatusTextBox.Text == "Pending")
                    {
                        StaffCustomerServiceStatusTextBox.Text = "In Session";
                    }
                    else if (StaffCustomerServiceStatusTextBox.Text == "Pending Paid")
                    {
                        StaffCustomerServiceStatusTextBox.Text = "In Session Paid";
                    }
                    StaffUpdateServiceStatusOfCustomerinDB(StaffCustomerServiceStatusTextBox.Text);
                    if (Parent != null)
                    {
                        Parent.Controls.Remove(this);
                    }
                    EnchanteForm.RefreshFlowLayoutPanel();
                    EnchanteForm.InitializeInSessionCustomers();
                    EnchanteForm.RefreshAvailableStaff();
                }
                else
                {
                    MessageBox.Show("You don't have enough stock to perform this service");
                }
            }
            else
            {
                return;
            }
        }

        private void StaffElapsedTimeTextBox_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to start this service?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                string serviceID = StaffServiceIDTextBox.Text;
                if (CheckIfInventoryIsEnoughForService(serviceID, EnchanteForm.RecQueStartInventoryDGV) == true)
                {
                    StartServiceButtonClicked?.Invoke(this, EventArgs.Empty);
                    if (StaffCustomerServiceStatusTextBox.Text == "Pending")
                    {
                        StaffCustomerServiceStatusTextBox.Text = "In Session";
                    }
                    else if (StaffCustomerServiceStatusTextBox.Text == "Pending Paid")
                    {
                        StaffCustomerServiceStatusTextBox.Text = "In Session Paid";
                    }
                    StaffUpdateServiceStatusOfCustomerinDB(StaffCustomerServiceStatusTextBox.Text);
                    if (Parent != null)
                    {
                        Parent.Controls.Remove(this);
                    }
                    EnchanteForm.RefreshFlowLayoutPanel();
                    EnchanteForm.InitializeInSessionCustomers();
                    EnchanteForm.RefreshAvailableStaff();
                }
                else
                {
                    MessageBox.Show("You don't have enough stock to perform this service");
                }
            }
            else
            {
                return;
            }
        }

        private void StaffQueNumberTextBox_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to start this service?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                string serviceID = StaffServiceIDTextBox.Text;
                if (CheckIfInventoryIsEnoughForService(serviceID, EnchanteForm.RecQueStartInventoryDGV) == true)
                {
                    StartServiceButtonClicked?.Invoke(this, EventArgs.Empty);
                    if (StaffCustomerServiceStatusTextBox.Text == "Pending")
                    {
                        StaffCustomerServiceStatusTextBox.Text = "In Session";
                    }
                    else if (StaffCustomerServiceStatusTextBox.Text == "Pending Paid")
                    {
                        StaffCustomerServiceStatusTextBox.Text = "In Session Paid";
                    }
                    StaffUpdateServiceStatusOfCustomerinDB(StaffCustomerServiceStatusTextBox.Text);
                    if (Parent != null)
                    {
                        Parent.Controls.Remove(this);
                    }
                    EnchanteForm.RefreshFlowLayoutPanel();
                    EnchanteForm.InitializeInSessionCustomers();
                    EnchanteForm.RefreshAvailableStaff();
                }
                else
                {
                    MessageBox.Show("You don't have enough stock to perform this service");
                }
            }
            else
            {
                return;
            }
        }

        private void StaffCustomerNameTextBox_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to start this service?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                string serviceID = StaffServiceIDTextBox.Text;
                if (CheckIfInventoryIsEnoughForService(serviceID, EnchanteForm.RecQueStartInventoryDGV) == true)
                {
                    StartServiceButtonClicked?.Invoke(this, EventArgs.Empty);
                    if (StaffCustomerServiceStatusTextBox.Text == "Pending")
                    {
                        StaffCustomerServiceStatusTextBox.Text = "In Session";
                    }
                    else if (StaffCustomerServiceStatusTextBox.Text == "Pending Paid")
                    {
                        StaffCustomerServiceStatusTextBox.Text = "In Session Paid";
                    }
                    StaffUpdateServiceStatusOfCustomerinDB(StaffCustomerServiceStatusTextBox.Text);
                    if (Parent != null)
                    {
                        Parent.Controls.Remove(this);
                    }
                    EnchanteForm.RefreshFlowLayoutPanel();
                    EnchanteForm.InitializeInSessionCustomers();
                    EnchanteForm.RefreshAvailableStaff();
                }
                else
                {
                    MessageBox.Show("You don't have enough stock to perform this service");
                }
            }
            else
            {
                return;
            }
        }

        private void StaffTransactionIDTextBox_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to start this service?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                string serviceID = StaffServiceIDTextBox.Text;
                if (CheckIfInventoryIsEnoughForService(serviceID, EnchanteForm.RecQueStartInventoryDGV) == true)
                {
                    StartServiceButtonClicked?.Invoke(this, EventArgs.Empty);
                    if (StaffCustomerServiceStatusTextBox.Text == "Pending")
                    {
                        StaffCustomerServiceStatusTextBox.Text = "In Session";
                    }
                    else if (StaffCustomerServiceStatusTextBox.Text == "Pending Paid")
                    {
                        StaffCustomerServiceStatusTextBox.Text = "In Session Paid";
                    }
                    StaffUpdateServiceStatusOfCustomerinDB(StaffCustomerServiceStatusTextBox.Text);
                    if (Parent != null)
                    {
                        Parent.Controls.Remove(this);
                    }
                    EnchanteForm.RefreshFlowLayoutPanel();
                    EnchanteForm.InitializeInSessionCustomers();
                    EnchanteForm.RefreshAvailableStaff();
                }
                else
                {
                    MessageBox.Show("You don't have enough stock to perform this service");
                }
            }
            else
            {
                return;
            }
        }

        private void StaffQueTypeTextBox_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to start this service?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                string serviceID = StaffServiceIDTextBox.Text;
                if (CheckIfInventoryIsEnoughForService(serviceID, EnchanteForm.RecQueStartInventoryDGV) == true)
                {
                    StartServiceButtonClicked?.Invoke(this, EventArgs.Empty);
                    if (StaffCustomerServiceStatusTextBox.Text == "Pending")
                    {
                        StaffCustomerServiceStatusTextBox.Text = "In Session";
                    }
                    else if (StaffCustomerServiceStatusTextBox.Text == "Pending Paid")
                    {
                        StaffCustomerServiceStatusTextBox.Text = "In Session Paid";
                    }
                    StaffUpdateServiceStatusOfCustomerinDB(StaffCustomerServiceStatusTextBox.Text);
                    if (Parent != null)
                    {
                        Parent.Controls.Remove(this);
                    }
                    EnchanteForm.RefreshFlowLayoutPanel();
                    EnchanteForm.InitializeInSessionCustomers();
                    EnchanteForm.RefreshAvailableStaff();
                }
                else
                {
                    MessageBox.Show("You don't have enough stock to perform this service");
                }
            }
            else
            {
                return;
            }
        }

        
    }
}
