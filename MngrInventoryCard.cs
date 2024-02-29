﻿using MySqlX.XDevAPI;
using Org.BouncyCastle.Utilities.Collections;
using System.Windows.Forms;


namespace Enchante
{
    internal class MngrInventoryCard
    {
        private Panel Type;
        private Panel Services;
        private Panel Membership;
        private Panel Products;
        private Panel ProductHistory;
        private Panel StaffSched;


        public MngrInventoryCard(Panel type, Panel service, Panel member, Panel product, Panel history, Panel sched)
        {
            Type = type;
            Services = service;
            Membership = member;
            Products = product;
            ProductHistory = history;
            StaffSched = sched;
        }

        public void PanelShow(Panel panelToShow)
        {
            Type.Hide();
            Services.Hide();
            Membership.Hide();
            Products.Hide();
            ProductHistory.Hide();
            StaffSched.Hide();

            panelToShow.Show(); // do not delete
        }
    }
}
