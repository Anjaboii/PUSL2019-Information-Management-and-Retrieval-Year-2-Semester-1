using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO; // For file handling
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iTextSharp.text; // iTextSharp for PDF generation
using iTextSharp.text.pdf;

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

        private void button1_Click(object sender, EventArgs e)
        {
            string fromOrderId = textBox1.Text.Trim();
            string toOrderId = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(fromOrderId) || string.IsNullOrEmpty(toOrderId))
            {
                MessageBox.Show("Please enter both 'From Order ID' and 'To Order ID'.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(fromOrderId, out int fromId) || !int.TryParse(toOrderId, out int toId))
            {
                MessageBox.Show("Order IDs must be valid integers.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            GenerateSalesReport(fromId, toId);
        }

        private void GenerateSalesReport(int fromOrderId, int toOrderId)
        {
            string query = @"
        SELECT OrderID, Products, TotalBillAmount, DiscountPercentage, DiscountAmount, LastAmount
        FROM Sales
        WHERE OrderID BETWEEN @FromOrderID AND @ToOrderID";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FromOrderID", fromOrderId);
                        command.Parameters.AddWithValue("@ToOrderID", toOrderId);

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable salesData = new DataTable();
                        adapter.Fill(salesData);

                        if (salesData.Rows.Count > 0)
                        {
                            SaveFileDialog saveFileDialog = new SaveFileDialog
                            {
                                Filter = "PDF Files (*.pdf)|*.pdf",
                                FileName = "SalesReport.pdf"
                            };

                            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                            {
                                string filePath = saveFileDialog.FileName;

                                // Create the PDF document
                                iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 10f, 10f, 20f, 20f);
                                PdfWriter.GetInstance(pdfDoc, new FileStream(filePath, FileMode.Create));
                                pdfDoc.Open();

                                // Add Title
                                iTextSharp.text.Font titleFont = iTextSharp.text.FontFactory.GetFont(
                                    iTextSharp.text.FontFactory.HELVETICA_BOLD, 16f, iTextSharp.text.BaseColor.BLACK
                                );
                                iTextSharp.text.Paragraph title = new iTextSharp.text.Paragraph("Sales Report\n\n", titleFont)
                                {
                                    Alignment = iTextSharp.text.Element.ALIGN_CENTER
                                };
                                pdfDoc.Add(title);

                                // Add Table
                                PdfPTable table = new PdfPTable(salesData.Columns.Count)
                                {
                                    WidthPercentage = 100
                                };

                                // Add Header Row
                                foreach (DataColumn column in salesData.Columns)
                                {
                                    PdfPCell headerCell = new PdfPCell(new Phrase(column.ColumnName))
                                    {
                                        BackgroundColor = iTextSharp.text.BaseColor.LIGHT_GRAY,
                                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER
                                    };
                                    table.AddCell(headerCell);
                                }

                                // Add Data Rows
                                foreach (DataRow row in salesData.Rows)
                                {
                                    foreach (var cellValue in row.ItemArray)
                                    {
                                        PdfPCell cell = new PdfPCell(new Phrase(cellValue.ToString()))
                                        {
                                            HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER
                                        };
                                        table.AddCell(cell);
                                    }
                                }

                                pdfDoc.Add(table);
                                pdfDoc.Close();

                                MessageBox.Show("Sales report generated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            MessageBox.Show("No sales data found for the specified order range.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
