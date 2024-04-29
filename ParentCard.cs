using System.Windows.Forms;

namespace Enchante
{
    internal class ParentCard
    {
        private Panel HomePage;
        private Panel ReceptionDash;
        private Panel Admin;
        private Panel MngrDash;

        public ParentCard(Panel home, Panel recept, Panel admin, Panel mgnr)
        {
            HomePage = home;
            ReceptionDash = recept;
            Admin = admin;
            MngrDash = mgnr;
        }

        public void PanelShow(Panel panelToShow)
        {
            HomePage.Hide();
            ReceptionDash.Hide();
            Admin.Hide();
            MngrDash.Hide();
            panelToShow.Show();
        }
    }
}
