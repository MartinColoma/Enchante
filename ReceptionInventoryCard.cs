﻿using System.Windows.Forms;


namespace Enchante
{
    internal class ReceptionInventoryCard
    {
        private Panel Type;
        private Panel Services;
        private Panel Membership;
        private Panel Products;

        public ReceptionInventoryCard(Panel type, Panel service, Panel member, Panel product)
        {
            Type = type;
            Services = service;
            Membership = member;
            Products = product;

        }

        public void PanelShow(Panel panelToShow)
        {
            Type.Hide();
            Services.Hide();
            Membership.Hide();
            Products.Hide();


            panelToShow.Show(); // do not delete
        }
    }
}