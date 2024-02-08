using System.Windows.Forms;


namespace Enchante
{
    internal class Registration
    {
        private Panel Regular;
        private Panel Premium;
        private Panel SuperVIP;


        public Registration(Panel reg, Panel prem, Panel svip)
        {
            Regular = reg;
            Premium = prem;
            SuperVIP = svip;

        }

        public void PanelShow(Panel panelToShow)
        {
            Regular.Hide();
            Premium.Hide();
            SuperVIP.Hide();

            panelToShow.Show();
        }
    }
}
