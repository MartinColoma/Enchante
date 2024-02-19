using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Enchante.Enchante;

namespace Enchante
{
    public partial class AvailableStaffUserControl : UserControl
    {
        public AvailableStaffUserControl()
        {
            InitializeComponent();
        }

        public void AvailableStaffSetData(Enchante.AvailableStaff staff)
        {
            AvailStaffEmployeeIDTextBox.Text = staff.EmployeeID;
            AvailStaffNameTextBox.Text = $"{staff.EmployeeLastName}, {staff.EmployeeFirstName}";
            AvailStaffCategoryTextBox.Text = staff.EmployeeCategory;
            AvailStaffCategoryLevelTextBox.Text = staff.EmployeeCategoryLevel;
            AvailStaffTimeSchedTextBox.Text = staff.EmployeeSchedule;
        }

        public string GetEmployeeID()
        {
            return AvailStaffEmployeeIDTextBox.Text;
        }

        public string GetStaffName()
        {
            return AvailStaffNameTextBox.Text;
        }
        public string GetStaffCategory()
        {
            return AvailStaffCategoryTextBox.Text;
        }
        public string GetStaffCategoryLevel()
        {
            return AvailStaffCategoryLevelTextBox.Text;
        }
        public string GetStaffTimeSched()
        {
            return AvailStaffTimeSchedTextBox.Text;
        }

        public AvailableStaff GetAvailableStaffData()
        {
            AvailableStaff staff = new AvailableStaff();
            staff.EmployeeID = GetEmployeeID();
            staff.EmployeeName = GetStaffName();
            staff.EmployeeCategory = GetStaffCategory();
            staff.EmployeeCategoryLevel = GetStaffCategoryLevel();
            staff.EmployeeSchedule = GetStaffTimeSched();
            return staff;
        }

        private void AvailableStaffUserControl_Load(object sender, EventArgs e)
        {

        }
    }
}
