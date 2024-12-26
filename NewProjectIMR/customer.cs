using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewProjectIMR
{
    public partial class customer : UserControl
    {
        public customer()
        {
            InitializeComponent();
        }

        
        private string connectionString = "Server=AnjanaHerath\\MSSQLSERVER01;Database=imr;Integrated Security=True;";

        

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void customer_Load(object sender, EventArgs e)
        {
            LoadCustomerData();
        }

        private void LoadCustomerData()
        {
            string query = "SELECT * FROM Customer";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        dataGridView1.DataSource = table;


                        foreach (DataGridViewColumn column in dataGridView1.Columns)
                        {
                            column.ReadOnly = true;
                        }


                        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                        dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;


                        dataGridView1.Columns["CustomerID"].HeaderText = "ID";
                        dataGridView1.Columns["FirstName"].HeaderText = "First Name";
                        dataGridView1.Columns["LastName"].HeaderText = "Last Name";
                        dataGridView1.Columns["ContactNo"].HeaderText = "Contact Number";
                        dataGridView1.Columns["NIC"].HeaderText = "NIC";


                        dataGridView1.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10);
                        dataGridView1.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 12, System.Drawing.FontStyle.Bold);
                        dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.DarkSlateGray;
                        dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;


                        dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;


                        dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
                        dataGridView1.AlternatingRowsDefaultCellStyle.ForeColor = System.Drawing.Color.Black;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading products: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string firstName = textBox1.Text;
            string lastName = textBox2.Text;
            long contactNo;
            long nic;

            // Validate input
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || !long.TryParse(textBox3.Text, out contactNo) || !long.TryParse(textBox4.Text, out nic))
            {
                MessageBox.Show("Please fill all fields correctly.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check if we are adding a new customer or updating an existing one
            if (textBox4.Tag == null) // If adding a new customer
            {
                string query = "INSERT INTO Customer (FirstName, LastName, ContactNo, NIC) VALUES (@FirstName, @LastName, @ContactNo, @NIC)";

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@FirstName", firstName);
                            command.Parameters.AddWithValue("@LastName", lastName);
                            command.Parameters.AddWithValue("@ContactNo", contactNo);
                            command.Parameters.AddWithValue("@NIC", nic);
                            command.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("Customer added successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error adding customer: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else // If updating an existing customer
            {
                int customerId = Convert.ToInt32(textBox4.Tag); // Assuming CustomerID is stored in textBox4.Tag
                string query = "UPDATE Customer SET FirstName = @FirstName, LastName = @LastName, ContactNo = @ContactNo, NIC = @NIC WHERE CustomerID = @CustomerID";

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@CustomerID", customerId);
                            command.Parameters.AddWithValue("@FirstName", firstName);
                            command.Parameters.AddWithValue("@LastName", lastName);
                            command.Parameters.AddWithValue("@ContactNo", contactNo);
                            command.Parameters.AddWithValue("@NIC", nic);
                            command.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("Customer updated successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating customer: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            LoadCustomerData(); // Reload the customer data
        }


        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();


            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;


            textBox4.Tag = null;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                textBox1.Text = row.Cells["FirstName"].Value.ToString();
                textBox2.Text = row.Cells["LastName"].Value.ToString();
                textBox3.Text = row.Cells["ContactNo"].Value.ToString();
                textBox4.Text = row.Cells["NIC"].Value.ToString();

                textBox4.Tag = row.Cells["CustomerID"].Value;

                textBox1.Enabled = false;
                textBox2.Enabled = false;
                textBox3.Enabled = false;
                textBox4.Enabled = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int customerId = Convert.ToInt32(textBox4.Tag);

            if (customerId == 0)
            {
                MessageBox.Show("No customer selected for deletion.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string query = "DELETE FROM Customer WHERE CustomerID = @CustomerID";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CustomerID", customerId);
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Customer deleted successfully.");
                LoadCustomerData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting customer: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
    
