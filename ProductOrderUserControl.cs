using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Enchante
{
    public partial class ProductOrderUserControl : UserControl
    {
        public ProductOrderUserControl()
        {
            InitializeComponent();
        }
        public event EventHandler ProductOrderVoidClicked;
        public event EventHandler<int> QuantityChanged;
        public event EventHandler VoidClicked;



        public string ProductItemID
        {
            get { return ProductOrderItemIDTextBox.Text; }
            set { ProductOrderItemIDTextBox.Text = value; }
        }

        public string ProductName
        {
            get { return ProductOrderItemNameTextBox.Text; }
            set { ProductOrderItemNameTextBox.Text = value; }
        }

        public string ProductPrice
        {
            get { return ProductOrderItemPriceTextBox.Text; }
            set { ProductOrderItemPriceTextBox.Text = value; }
        }


        private void ProductOrderVoidBtn_Click(object sender, EventArgs e)
        {
            VoidClicked?.Invoke(this, EventArgs.Empty);
            Control parentControl = this.Parent;
            if (parentControl != null)
            {
                parentControl.Controls.Remove(this);
                this.Dispose();

            }
        }

        private void ProductOrderMinusBtn_Click(object sender, EventArgs e)
        {
            int currentQuantity = int.Parse(ProductOrderQuantityTextBox.Text);
            if (currentQuantity > 1)
            {
                ProductOrderQuantityTextBox.Text = (currentQuantity - 1).ToString();
                int newQuantity = currentQuantity - 1;
                QuantityChanged?.Invoke(this, newQuantity);
            }
        }

        private void ProductOrderAddBtn_Click(object sender, EventArgs e)
        {
            int currentQuantity = int.Parse(ProductOrderQuantityTextBox.Text);
            ProductOrderQuantityTextBox.Text = (currentQuantity + 1).ToString();
            int newQuantity = currentQuantity + 1;
            QuantityChanged?.Invoke(this, newQuantity);
        }


    }
}
