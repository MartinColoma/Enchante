using System.Windows.Forms;

namespace Enchante
{
    internal class ParentCard
    {
        private Panel HomePage;
        private Panel StaffDash;
        private Panel ManagerDash;
        private Panel Admin;
        private Panel VIPPage;

        public ParentCard(Panel home, Panel staff, Panel manager, Panel vip, Panel admin)
        {
            HomePage = home;
            StaffDash = staff;
            ManagerDash = manager;
            VIPPage = vip;
            Admin = admin;
        }

        public void PanelShow(Panel panelToShow)
        {
            HomePage.Hide();
            StaffDash.Hide();
            ManagerDash.Hide();
            VIPPage.Hide();
            Admin.Hide();
            panelToShow.Show();
        }
    }
}
