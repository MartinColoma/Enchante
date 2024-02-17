using System.Windows.Forms;


namespace Enchante
{
    internal class StaffCard
    {

        private Panel Current;
        private Panel History;

        public StaffCard(Panel current, Panel history)
        {
            Current = current;
            History = history;

        }

        public void PanelShow(Panel panelToShow)
        {
            History.Hide();
            Current.Hide();
            panelToShow.Show();
        }
    }
}
