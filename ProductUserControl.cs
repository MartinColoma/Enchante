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
    public partial class ProductUserControl : UserControl
    {
        public ProductUserControl()
        {
            InitializeComponent();
        }

        public event EventHandler ProductClicked;
        private void ProductUserControl_Click(object sender, EventArgs e)
        {
            ProductClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
