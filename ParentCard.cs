using System.Windows.Forms;

namespace Enchante
{
    internal class ParentCard
    {
        private Panel HomePage;
        private Panel StaffDash;
        private Panel ManagerDash;
        private Panel VIPPage;

        public ParentCard(Panel home, Panel staff, Panel manager, Panel vip)
        {
            HomePage = home;
            StaffDash = staff;
            ManagerDash = manager;
            VIPPage = vip;
        }

        public void PanelShow(Panel panelToShow)
        {
            HomePage.Hide();
            StaffDash.Hide();
            ManagerDash.Hide();
            VIPPage.Hide();

            panelToShow.Show();
        }
    }
}
