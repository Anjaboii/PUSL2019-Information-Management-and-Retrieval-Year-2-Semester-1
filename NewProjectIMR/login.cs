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
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Define the allowed username and password
            string allowedUsername = "admin";
            string allowedPassword = "1234";

            // Get the entered username and password
            string enteredUsername = textBox1.Text;
            string enteredPassword = textBox2.Text;

            // Validate the credentials
            if (enteredUsername == allowedUsername && enteredPassword == allowedPassword)
            {
                // Hide the login form
                this.Hide();

                // Open Form1
                Form1 form1 = new Form1();
                form1.ShowDialog();
            }
            else
            {
                // Show an error message if the credentials are invalid
                MessageBox.Show("Invalid username or password. Please try again.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
