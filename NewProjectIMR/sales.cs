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
    public partial class sales : UserControl
    {
        public sales()
        {
            InitializeComponent();
        }

        private string connectionString = "Server=AnjanaHerath\\MSSQLSERVER01;Database=imr;Integrated Security=True;";

        private void sales_Load(object sender, EventArgs e)
        {
            LoadSalesData();
        }

        private void LoadSalesData()
        {
            string query = "SELECT * FROM Sales";

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


                        dataGridView1.Columns["OrderID"].HeaderText = "ID";
                        dataGridView1.Columns["Products"].HeaderText = "Product Name";
                        dataGridView1.Columns["TotalBillAmount"].HeaderText = "Bill Amount";
                        dataGridView1.Columns["DiscountPercentage"].HeaderText = "Discount Percentage";
                        dataGridView1.Columns["DiscountAmount"].HeaderText = "Discount Amount";
                        dataGridView1.Columns["LastAmount"].HeaderText = "Last Amount";


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
                MessageBox.Show($"Error loading sales: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
