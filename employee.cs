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
    public partial class employee : UserControl
    {
        public employee()
        {
            InitializeComponent();
        }

        private string connectionString = "Server=AnjanaHerath\\MSSQLSERVER01;Database=imr;Integrated Security=True;";

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void employee_Load(object sender, EventArgs e)
        {
            LoadEmployeeData();
        }

        private void LoadEmployeeData()
        {
            string query = "SELECT * FROM Employee";

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


                        dataGridView1.Columns["EmployeeID"].HeaderText = "ID";
                        dataGridView1.Columns["EmployeeName"].HeaderText = "Employee Name";
                        dataGridView1.Columns["Position"].HeaderText = "Position";
                        dataGridView1.Columns["ContactNo"].HeaderText = "ContactNo";
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
            string employeeName = textBox1.Text;
            string position = textBox2.Text;
            long contactNo;
            long nic;

            // Validate input
            if (string.IsNullOrEmpty(employeeName) ||
                string.IsNullOrEmpty(position) ||
                !long.TryParse(textBox3.Text, out contactNo) ||
                !long.TryParse(textBox4.Text, out nic))
            {
                MessageBox.Show("Please fill all fields correctly.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Check if we are adding a new employee or updating an existing one
            if (textBox4.Tag == null) // If adding a new employee
            {
                string query = "INSERT INTO Employee (EmployeeName, Position, ContactNo, NIC) VALUES (@EmployeeName, @Position, @ContactNo, @NIC)";

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@EmployeeName", employeeName);
                            command.Parameters.AddWithValue("@Position", position);
                            command.Parameters.AddWithValue("@ContactNo", contactNo);
                            command.Parameters.AddWithValue("@NIC", nic);
                            command.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("Employee added successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error adding employee: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else // If updating an existing employee
            {
                int employeeId = Convert.ToInt32(textBox4.Tag); // Assuming EmployeeID is stored in textBox4.Tag
                string query = "UPDATE Employee SET EmployeeName = @EmployeeName, Position = @Position, ContactNo = @ContactNo, NIC = @NIC WHERE EmployeeID = @EmployeeID";

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@EmployeeID", employeeId);
                            command.Parameters.AddWithValue("@EmployeeName", employeeName);
                            command.Parameters.AddWithValue("@Position", position);
                            command.Parameters.AddWithValue("@ContactNo", contactNo);
                            command.Parameters.AddWithValue("@NIC", nic);
                            command.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("Employee updated successfully.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating employee: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            LoadEmployeeData(); // Reload the employee data
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


        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            {
                if (e.RowIndex >= 0)
                {
                    DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                    textBox1.Text = row.Cells["EmployeeName"].Value.ToString();
                    textBox2.Text = row.Cells["Position"].Value.ToString();
                    textBox3.Text = row.Cells["ContactNo"].Value.ToString();
                    textBox4.Text = row.Cells["NIC"].Value.ToString();

                    textBox4.Tag = row.Cells["EmployeeID"].Value;

                    textBox1.Enabled = false;
                    textBox2.Enabled = false;
                    textBox3.Enabled = false;
                    textBox4.Enabled = false;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int employeeId = Convert.ToInt32(textBox4.Tag);

            if (employeeId == 0)
            {
                MessageBox.Show("No employee selected for deletion.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string query = "DELETE FROM Employee WHERE EmployeeID = @EmployeeID";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeID", employeeId);
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Employee deleted successfully.");
                LoadEmployeeData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting employee: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            {
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                textBox3.Enabled = true;
                textBox4.Enabled = true;
            }
        }
    }
}