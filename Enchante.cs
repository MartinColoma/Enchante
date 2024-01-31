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
    public partial class Enchante : Form
    {
        //cardlayout panel classes
        private ParentCard ParentPanelShow;

        public Enchante()
        {
            InitializeComponent();

            //Main Form Panel Manager
            ParentPanelShow = new ParentCard(EnchanteHomePage, EnchanteStaffPage, EnchanteMngrPage, EnchanteVIPPage);

        }

        private void Enchante_Load(object sender, EventArgs e)
        {
            ParentPanelShow.PanelShow(EnchanteHomePage);
        }
    }
}
