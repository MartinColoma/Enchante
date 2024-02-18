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
    }
}
