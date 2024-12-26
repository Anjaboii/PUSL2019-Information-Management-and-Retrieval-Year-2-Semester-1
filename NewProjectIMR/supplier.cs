using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NewProjectIMR
{
    public partial class supplier : UserControl
    {
        public supplier()
        {
            InitializeComponent();
        }

        private string connectionString = "Server=AnjanaHerath\\MSSQLSERVER01;Database=imr;Integrated Security=True;";

        private void supplier_Load(object sender, EventArgs e)
        {
            LoadSupplierData();
        }

        private void LoadSupplierData()
        {
            string query = "SELECT * FROM Supplier";

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

                        dataGridView1.Columns["SupplierID"].HeaderText = "ID";
                        dataGridView1.Columns["SupplierName"].HeaderText = "Supplier Name";
                        dataGridView1.Columns["ProductName"].HeaderText = "Product Name";
                        dataGridView1.Columns["Amount"].HeaderText = "Amount";
                        dataGridView1.Columns["Size"].HeaderText = "Size";

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
                MessageBox.Show($"Error loading suppliers: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                textBox1.Text = row.Cells["SupplierName"].Value.ToString();
                textBox2.Text = row.Cells["ProductName"].Value.ToString();
                textBox4.Text = row.Cells["Amount"].Value.ToString();
                textBox3.Text = row.Cells["Size"].Value.ToString();

                textBox1.Tag = row.Cells["SupplierID"].Value; // Set the tag for updates

                textBox1.Enabled = false;
                textBox2.Enabled = false;
                textBox3.Enabled = false;
                textBox4.Enabled = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string supplierName = textBox1.Text;
            string productName = textBox2.Text;
            int amount;
            string size = textBox3.Text;

            if (string.IsNullOrEmpty(supplierName) || string.IsNullOrEmpty(productName) || !int.TryParse(textBox4.Text, out amount) || string.IsNullOrEmpty(size))
            {
                MessageBox.Show("Please fill all fields correctly.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (textBox1.Tag == null) // Adding a new supplier
            {
                string query = "INSERT INTO Supplier (SupplierName, ProductName, Amount, Size) VALUES (@SupplierName, @ProductName, @Amount, @Size)";

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@SupplierName", supplierName);
                            command.Parameters.AddWithValue("@ProductName", productName);
                            command.Parameters.AddWithValue("@Amount", amount);
                            command.Parameters.AddWithValue("@Size", size);
                            command.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("Supplier added successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error adding supplier: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else // Updating an existing supplier
            {
                if (!int.TryParse(textBox1.Tag.ToString(), out int supplierId))
                {
                    MessageBox.Show("No supplier selected for update.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string query = "UPDATE Supplier SET SupplierName = @SupplierName, ProductName = @ProductName, Amount = @Amount, Size = @Size WHERE SupplierID = @SupplierID";

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@SupplierID", supplierId);
                            command.Parameters.AddWithValue("@SupplierName", supplierName);
                            command.Parameters.AddWithValue("@ProductName", productName);
                            command.Parameters.AddWithValue("@Amount", amount);
                            command.Parameters.AddWithValue("@Size", size);
                            command.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("Supplier updated successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating supplier: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            LoadSupplierData();
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

            textBox1.Tag = null;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox1.Tag?.ToString(), out int supplierId))
            {
                MessageBox.Show("No supplier selected for deletion.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string query = "DELETE FROM Supplier WHERE SupplierID = @SupplierID";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SupplierID", supplierId);
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Supplier deleted successfully.");
                LoadSupplierData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting supplier: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
        }

        private void label4_Click(object sender, EventArgs e)
        {
            // Placeholder for label click event
        }
    }
}
