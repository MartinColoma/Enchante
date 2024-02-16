using System.Windows.Forms;


namespace Enchante
{
    internal class ReceptionTransactionCard
    {
        private Panel Transaction;
        private Panel WalkIn;
        private Panel Appointment;

        public ReceptionTransactionCard(Panel transact, Panel walk, Panel appoint)
        {
            Transaction = transact;
            WalkIn = walk;
            Appointment = appoint;

        }

        public void PanelShow(Panel panelToShow)
        {
            Transaction.Hide();
            WalkIn.Hide();
            Appointment.Hide();


            panelToShow.Show(); // do not delete
        }
    }
}
