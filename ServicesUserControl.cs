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
    public partial class ServicesUserControl : UserControl
    {
        private Enchante EnchanteForm;

        public event EventHandler ServiceUserControl_Clicked;
        public event EventHandler RecServiceNameTextBox_Clicked;
        public event EventHandler RecServiceDurationTextBox_Clicked;
        public event EventHandler RecServicePriceTextBox_Clicked;

        public ServicesUserControl(Enchante EnchanteForm)
        {
            InitializeComponent();
            this.EnchanteForm = EnchanteForm;
        }

        public void SetServicesData(Enchante.Services services)
        {
            RecServiceNameTextBox.Text = services.ServiceName;
            RecServicePriceTextBox.Text = services.ServicePrice;
            RecServiceDurationTextBox.Text = services.ServiceDuration;
            RecServiceCategoryTextBox.Text = services.ServiceCategory;
            RecServiceIDTextBox.Text = services.ServiceID;
        }

        public void AddService()
        {
            EnchanteForm.serviceName = RecServiceNameTextBox.Text;
            EnchanteForm.servicePrice = RecServicePriceTextBox.Text;
            EnchanteForm.serviceDuration = RecServiceDurationTextBox.Text;
            EnchanteForm.serviceCategory = RecServiceCategoryTextBox.Text;
            EnchanteForm.serviceID2 = RecServiceIDTextBox.Text;
            if (EnchanteForm.serviceappointment == true)
            {
                EnchanteForm.RecApptAddService();
            }
            else
            {
                EnchanteForm.RecWalkinAddService();
            }
        }

        private void ServicesUserControl_Click(object sender, EventArgs e)
        {
            AddService();
        }

        private void RecServiceNameTextBox_Click(object sender, EventArgs e)
        {
            AddService();
        }

        private void RecServiceDurationTextBox_Click(object sender, EventArgs e)
        {
            AddService();
        }

        private void RecServicePriceTextBox_Click(object sender, EventArgs e)
        {
            AddService();
        }
    }
}
