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

namespace Демоэкзамен
{
    public partial class FormСart : Form
    {
        public int id;
        public static string connString = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=TradeOPG;Data Source=LOKIZ\MSSQLSERVER01";
        SqlConnection sqlConnect = new SqlConnection(connString);
        string selected_id;
        int i = 0;
        int b = 0;
        class pol
        {
            public int OrderPickupPoint { get; set; }
            public string Index { get; set; }
        }

        public FormСart(int id1)
        {
            id = id1;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
               
                    if (i < 2)
                    {
                   
                            
                            dataGridView1.Rows[0].Cells[0].Value = selected_id;
                            dataGridView1.Rows[0].Cells[1].Value = 1;

                            dataGridView1.Rows.Add(1);
                       
                    }
                    else
                    {
                    
                            dataGridView1.Rows[i - 1].Cells[0].Value = selected_id;
                   dataGridView1.Rows[i - 1].Cells[1].Value = 1;
                    dataGridView1.Rows.Add(1);
                      
                }
                
            i++;

        }

        private void FormСart_Load(object sender, EventArgs e)
        {
            SqlCommand logRequst = new SqlCommand();
            sqlConnect.Open();
            logRequst.CommandText = $"SELECT * FROM [TradeOPG].[dbo].[Product]";
            logRequst.Connection = sqlConnect;
            SqlDataAdapter adapter = new SqlDataAdapter(logRequst);
            DataSet dataSet = new DataSet();
            adapter.Fill(dataSet);
            dataGridView2.DataSource = dataSet.Tables[0];
            
            logRequst.CommandText = ($"Select * from [TradeOPG].[dbo].[FIO] Where UserID ='{id}'");
            logRequst.Connection = sqlConnect;
            SqlDataReader rd = logRequst.ExecuteReader();
            rd.Read();
            label13.Text = rd.GetString(1) + rd.GetString(2) + rd.GetString(3);
            sqlConnect.Close();
            sqlConnect.Open();

            List<pol> pol = new List<pol>();
            SqlCommand logRequstdep = new SqlCommand("Select * from OrderPickupPoint", sqlConnect);
            logRequstdep.Connection = sqlConnect;
            sqlConnect.Close();
            
        }

        private void dataGridView2_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            selected_id = Convert.ToString(dataGridView2.Rows[e.RowIndex].Cells[0].Value);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sqlConnect.Open();
            SqlCommand logRequst = new SqlCommand();
            logRequst.CommandType = CommandType.StoredProcedure;
            logRequst.CommandText = "OrderAdd";

            logRequst.Connection = sqlConnect;

            logRequst.Parameters.AddWithValue("@OrderPickupPoint", comboBox2.SelectedIndex);
            logRequst.Parameters.AddWithValue("@UserID", id);

            logRequst.ExecuteNonQuery();
            
            try
            {

                

            }
            catch (Exception )
            {
                MessageBox.Show("Не указана точка самовывоза");
                Menu fu = new Menu(id);
                fu.Show();
            }
            finally
            {
                sqlConnect.Close();

            }

            while (b+1<i){

                sqlConnect.Open();
                SqlCommand logRequst1 = new SqlCommand();
                logRequst1.CommandType = CommandType.StoredProcedure;
            logRequst1.CommandText = "OrderProdictAdd";

            logRequst1.Connection = sqlConnect;
            
            logRequst1.Parameters.AddWithValue("@ProductArticleNumber", dataGridView1[0,b].Value.ToString());
            logRequst1.Parameters.AddWithValue("@ProductQuanity", dataGridView1[1, b].Value);
              
            logRequst1.ExecuteNonQuery();
                sqlConnect.Close();
                b++;
                try
                {

                    

                }
                catch (Exception )
                {
                    
                }
            }
            try
            {
                
                MessageBox.Show("Заказ добавлен");
                this.Hide();
                Menu fu = new Menu(id);
                fu.Show();

            }
            catch (Exception )
            {
                MessageBox.Show("Заказ добавлен");
                Menu fu = new Menu(id);
                fu.Show();
            }
            finally
            {


            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
           
                Menu student = new Menu(id);
                student.ShowDialog();
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
