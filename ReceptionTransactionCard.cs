using System.Windows.Forms;


namespace Enchante
{
    internal class ReceptionTransactionCard
    {
        private Panel Transaction;
        private Panel WalkIn;
        private Panel Appointment;
        private Panel Payment;
        private Panel Que;

        public ReceptionTransactionCard(Panel transact, Panel walk, Panel appoint, Panel pay, Panel que)
        {
            Transaction = transact;
            WalkIn = walk;
            Appointment = appoint;
            Payment = pay;
            Que = que;
        }

        public void PanelShow(Panel panelToShow)
        {
            Transaction.Hide();
            WalkIn.Hide();
            Appointment.Hide();
            Payment.Hide();
            Que.Hide();

            panelToShow.Show(); // do not delete
        }
    }
}
