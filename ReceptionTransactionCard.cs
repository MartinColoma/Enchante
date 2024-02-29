using System.Windows.Forms;


namespace Enchante
{
    internal class ReceptionTransactionCard
    {
        private Panel Transaction;
        private Panel WalkIn;
        private Panel Appointment;
        private Panel Payment;

        public ReceptionTransactionCard(Panel transact, Panel walk, Panel appoint, Panel pay)
        {
            Transaction = transact;
            WalkIn = walk;
            Appointment = appoint;
            Payment = pay;
        }

        public void PanelShow(Panel panelToShow)
        {
            Transaction.Hide();
            WalkIn.Hide();
            Appointment.Hide();
            Payment.Hide(); 

            panelToShow.Show(); // do not delete
        }
    }
}
