using System.Windows.Forms;

namespace Enchante
{
    internal class ParentCard
    {
        private Panel HomePage;
        private Panel StaffDash;
        private Panel ReceptionDash;
        private Panel Admin;
        private Panel MngrDash;

        public ParentCard(Panel home, Panel staff, Panel recept, Panel admin, Panel mgnr)
        {
            HomePage = home;
            StaffDash = staff;
            ReceptionDash = recept;
            Admin = admin;
            MngrDash = mgnr;
        }

        public void PanelShow(Panel panelToShow)
        {
            HomePage.Hide();
            StaffDash.Hide();
            ReceptionDash.Hide();
            Admin.Hide();
            MngrDash.Hide();
            panelToShow.Show();
        }
    }
}
