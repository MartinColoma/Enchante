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
        private Panel Shop;
        private Panel ApptCon;

        public ReceptionTransactionCard(Panel transact, Panel walk, Panel appoint, Panel pay, Panel que, Panel shop, Panel confirm)
        {
            Transaction = transact;
            WalkIn = walk;
            Appointment = appoint;
            Payment = pay;
            Que = que;
            Shop = shop;
            ApptCon = confirm;

        }

        public void PanelShow(Panel panelToShow)
        {
            Transaction.Hide();
            WalkIn.Hide();
            Appointment.Hide();
            Payment.Hide();
            Que.Hide();
            Shop.Hide();
            ApptCon.Hide();

            panelToShow.Show(); // do not delete
        }
    }
}
