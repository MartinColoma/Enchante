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
    public partial class StaffUserControl : UserControl
    {
        private Enchante EnchanteForm;
        private static StaffUserControl currentlySelectedControl;

        public EventHandler StaffUserControl_Clicked;
        public StaffUserControl(Enchante EnchanteForm)
        {
            InitializeComponent();
            this.EnchanteForm = EnchanteForm;
            this.GotFocus += StaffUserControl_GotFocus;
            this.LostFocus += StaffUserControl_LostFocus;
        }

        public void SetStaffData(Enchante.Staff staff)
        {
            StaffName.Text = staff.StaffName;
            StaffCategory.Text = staff.StaffCategory;
            StaffGender.Text = staff.StaffGender;
            StaffServiceID.Text = staff.StaffID;
        }

        private void StaffUserControl_Click(object sender, EventArgs e)
        {
            EnchanteForm.selectedmembercategory = StaffCategory.Text;
            EnchanteForm.selectedstaffemployeeid = StaffServiceID.Text;
            EnchanteForm.StaffClicked();
        }

        private void StaffUserControl_GotFocus(object sender, EventArgs e)
        {
            if (currentlySelectedControl != null && currentlySelectedControl != this)
            {
                currentlySelectedControl.TriggerLostFocus();
            }

            currentlySelectedControl = this;

            StaffName.DisabledState.FillColor = Color.FromArgb(216, 213, 178);
            StaffGender.DisabledState.FillColor = Color.FromArgb(216, 213, 178);
            StaffName.DisabledState.ForeColor = Color.FromArgb(69, 105, 44);
            StaffGender.DisabledState.ForeColor = Color.FromArgb(69, 105, 44);
            StaffName.DisabledState.BorderColor = Color.FromArgb(177, 183, 97);
            StaffGender.DisabledState.BorderColor = Color.FromArgb(177, 183, 97);
        }

        private void StaffUserControl_LostFocus(object sender, EventArgs e)
        {
            if (currentlySelectedControl == this)
            {
                currentlySelectedControl = null;

                StaffName.DisabledState.FillColor = Color.FromArgb(89, 136, 82);
                StaffGender.DisabledState.FillColor = Color.FromArgb(89, 136, 82);
                StaffName.DisabledState.ForeColor = Color.FromArgb(216, 213, 178);
                StaffGender.DisabledState.ForeColor = Color.FromArgb(216, 213, 178);
                StaffName.DisabledState.BorderColor = Color.FromArgb(216, 213, 178);
                StaffGender.DisabledState.BorderColor = Color.FromArgb(216, 213, 178);
            }
        }

        private void TriggerLostFocus()
        {
            EventArgs emptyArgs = EventArgs.Empty;
            OnLostFocus(emptyArgs);
        }

    }
}
