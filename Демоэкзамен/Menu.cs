using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Демоэкзамен
{
    public partial class Menu : Form
    {
        public int id;
        public static string connString = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=TradeOPG;Data Source=LOKIZ\MSSQLSERVER01";
        SqlConnection sqlConnect = new SqlConnection(connString);
        string selected_id;
        public Menu(int id1)
        {
            InitializeComponent();
            id = id1;
        }
        private void Menu_Load(object sender, EventArgs e)
        {
            SqlCommand logRequst = new SqlCommand();
            sqlConnect.Open();
            logRequst.CommandText = $"SELECT * FROM [TradeOPG].[dbo].[Product]";
            logRequst.Connection = sqlConnect;
            SqlDataAdapter adapter = new SqlDataAdapter(logRequst);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);
            dataGridView1.DataSource = dataSet.Tables[0];
            logRequst.CommandText = ($"Select * from [TradeOPG].[dbo].[FIO] Where UserID ='{id}'");
         // logRequst.Connection = sqlConnect;
            SqlDataReader rd = logRequst.ExecuteReader();
            rd.Read();
            label13.Text = rd.GetString(1) + rd.GetString(2) + rd.GetString( 3);
            sqlConnect.Close();

        }
        private void button3_Click(object sender, EventArgs e)
        {
            Process.Start("shutdown", "/s /t 0");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            FormСart form = new FormСart(id);
            form.Show();
            this.Hide(); 
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
            (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $"ProductName LIKE '%{textBox1.Text}%' ";

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0) {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $" ProductDiscountAmount >0 and ProductDiscountAmount  <10  ";
            }
            if (comboBox1.SelectedIndex == 1)
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $" ProductDiscountAmount >=10 and ProductDiscountAmount  <15  ";
            }
            if (comboBox1.SelectedIndex == 2)
            {
                (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = $" ProductDiscountAmount >=15   ";
            }
        }

        

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void измененияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormUpdate fu = new FormUpdate(sqlConnect, selected_id,id);
            fu.Show();
        }

        private void добавлениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            if (Application.OpenForms["Formadd"] == null)
            {
                FormAdd student = new FormAdd(sqlConnect,id);
                student.ShowDialog();
            }
        }

        private void удалениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sqlConnect.Open();
            SqlCommand logRequst = new SqlCommand();
            logRequst.Connection = sqlConnect;
            logRequst.CommandType = CommandType.StoredProcedure;
            logRequst.CommandText = "ProductDelete";



            logRequst.Parameters.AddWithValue("@ProductArticleNumber", selected_id);
            try
            {
                logRequst.ExecuteNonQuery();
                MessageBox.Show("Продукт удален");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка : " + ex.Message);

            }
            finally
            {
                sqlConnect.Close();
            }
            Menu_Load(sender, e);
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            selected_id = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[0].Value); // сбор параметра для удаления продукта
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }     
}
