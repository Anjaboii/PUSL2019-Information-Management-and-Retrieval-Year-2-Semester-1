using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewProjectIMR
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button6_Click(object sender, EventArgs e)
        {

            panel4.Controls.Clear();


            supplier supplierControl = new supplier
            {
                Dock = DockStyle.Fill
            };


            panel4.Controls.Add(supplierControl);
        }



            private void button1_Click(object sender, EventArgs e)
        {

            panel4.Controls.Clear();


            product productControl = new product
            {
                Dock = DockStyle.Fill
            };


            panel4.Controls.Add(productControl);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        public void LoadControlToPanel4(UserControl control)
        {
            panel4.Controls.Clear();
            control.Dock = DockStyle.Fill;
            panel4.Controls.Add(control);
        }

        private void button2_Click(object sender, EventArgs e)
        {

            panel4.Controls.Clear();


            sales salesControl = new sales
            {
                Dock = DockStyle.Fill
            };


            panel4.Controls.Add(salesControl);
        }

        private void button3_Click(object sender, EventArgs e)
        {

            panel4.Controls.Clear();


            customer customerControl = new customer
            {
                Dock = DockStyle.Fill
            };


            panel4.Controls.Add(customerControl);

        }

        private void button4_Click(object sender, EventArgs e)
        {

            panel4.Controls.Clear();


             employee employeeControl = new employee
            {
                Dock = DockStyle.Fill
            };


            panel4.Controls.Add(employeeControl);

        }

        private void button5_Click(object sender, EventArgs e)
        {

            panel4.Controls.Clear();


            purchases purchasesControl = new purchases
            {
                Dock = DockStyle.Fill
            };


            panel4.Controls.Add(purchasesControl);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            {
                login login = new login();
                login.Show();


                this.Hide();


                login.FormClosed += (s, args) => this.Close();
            }
        }
    }
}
