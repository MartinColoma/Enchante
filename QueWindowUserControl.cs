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
    public partial class QueWindowUserControl : UserControl
    {
        private Enchante EnchanteForm;
        public QueWindowUserControl(Enchante EnchanteForm)
        {
            InitializeComponent();
            this.EnchanteForm = EnchanteForm;
        }

        public string CurrentStaffID { get; set; }
        public string ControlID { get; set; }

        public void AvailableCustomerSetData(Enchante.InSessionCustomers customer)
        {
            StaffTransactionIDTextBox.Text = customer.TransactionNumber;
            StaffCustomerServiceNameSelectedTextBox.Text = customer.ServiceName;
            StaffCustomerServiceStatusTextBox.Text = customer.ServiceStatus;
            StaffCustomerNameTextBox.Text = customer.ClientName;
            StaffQueTypeTextBox.Text = customer.QueType;
            StaffServiceIDTextBox.Text = customer.ServiceID;
            StaffQueNumberTextBox.Text = customer.QueNumber;
            StaffCustomerAttendingStaffTextBox.Text = customer.AttendingStaff;
        }
    }
}
