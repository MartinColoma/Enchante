using MySqlX.XDevAPI;
using Org.BouncyCastle.Utilities.Collections;
using System.Windows.Forms;


namespace Enchante
{
    internal class MngrInventoryCard
    {
        private Panel Type;
        private Panel Services;
        private Panel ServiceHistory;
        private Panel Membership;
        private Panel Products;
        private Panel ProductHistory;
        private Panel StaffSched;
        private Panel Walk_In_Sales;
        private Panel Walk_In_Prod_sales;
        private Panel ApptTrans_Sales;
        private Panel InDemandService;

        public MngrInventoryCard(Panel type, Panel service, Panel serviceHis, Panel member, Panel product, Panel prodHistory, 
                                Panel sched, Panel walk_in, Panel walkin_prod, Panel apptTrans,Panel indemand)
        {
            Type = type;
            Services = service;
            ServiceHistory = serviceHis;
            Membership = member;
            Products = product;
            ProductHistory = prodHistory;
            StaffSched = sched;
            Walk_In_Sales = walk_in;
            Walk_In_Prod_sales = walkin_prod;
            ApptTrans_Sales = apptTrans;
            InDemandService = indemand;
        }

        public void PanelShow(Panel panelToShow)
        {
            Type.Hide();
            Services.Hide();
            ServiceHistory.Hide();
            Membership.Hide();
            Products.Hide();
            ProductHistory.Hide();
            StaffSched.Hide();
            Walk_In_Sales.Hide();
            Walk_In_Prod_sales.Hide();
            ApptTrans_Sales.Hide();
            InDemandService.Hide();

            panelToShow.Show(); // do not delete
        }
    }
}
