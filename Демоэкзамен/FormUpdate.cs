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
    public partial class FormUpdate : Form
    {
        public int id;
        private SqlConnection sqlConnect;
        public string selected_id;
        public FormUpdate(SqlConnection connect, string id2, int id1)
        {
            id = id1;
            selected_id = id2;
            sqlConnect = connect;
            InitializeComponent();
        }

        private void login_button_Click(object sender, EventArgs e)
        {
            sqlConnect.Close();
            sqlConnect.Open();
            SqlCommand logRequst = new SqlCommand();
            logRequst.Connection = sqlConnect;
            logRequst.CommandType = CommandType.StoredProcedure;
            logRequst.CommandText = "Productupdate";
            SqlCommand logRequstsd = new SqlCommand();
            logRequstsd.Connection = sqlConnect;
            logRequstsd.CommandType = CommandType.StoredProcedure;
            logRequstsd.CommandText = "Productupdate";
            logRequst.Parameters.AddWithValue("@ProductArticleNumber", selected_id);
            logRequst.Parameters.AddWithValue("@ProductName", textBox2.Text);
            logRequst.Parameters.AddWithValue("@ProductCost", textBox1.Text);
            logRequst.Parameters.AddWithValue("@ProductDiscountAmount", textBox4.Text);
            logRequst.Parameters.AddWithValue("@ProductManufacturer", textBox5.Text);
            logRequst.Parameters.AddWithValue("@ProductProvider", comboBox1.Text);
            logRequst.Parameters.AddWithValue("@ProductCategory", comboBox2.Text);
            logRequst.Parameters.AddWithValue("@ProductCurrentDiscount", textBox6.Text);
            logRequst.Parameters.AddWithValue("@ProductQuantityInStock", textBox7.Text);
            logRequst.Parameters.AddWithValue("@ProductDescription", textBox8.Text);
            logRequst.Parameters.AddWithValue("@ProductPhoto", textBox9.Text);



            try
            {
                logRequst.ExecuteNonQuery();
                MessageBox.Show("Данные продукта изменены");
                Menu fu = new Menu(id);
                fu.Show();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка! : " + ex.Message);
            }
            finally
            {
                sqlConnect.Close();

            }
        }

        private void FormUpdate_Load(object sender, EventArgs e)
        {
            sqlConnect.Close();
            SqlCommand logRequst1 = new SqlCommand();
            sqlConnect.Open();
            logRequst1.CommandText = ($"Select * from [TradeOPG].[dbo].[FIO] Where UserID ='{id}'");
            logRequst1.Connection = sqlConnect;
            SqlDataReader rd1 = logRequst1.ExecuteReader();
            rd1.Read();
            label2.Text = rd1.GetString(1) + rd1.GetString(2) + rd1.GetString(3);
            SqlCommand logRequst = new SqlCommand($"Select * from Product Where ProductArticleNumber = '{selected_id}'");
            logRequst.Connection = sqlConnect;
            sqlConnect.Close();
            sqlConnect.Open();
            SqlDataReader rd = logRequst.ExecuteReader();
            while (rd.Read())
            {

                textBox2.Text = rd["ProductName"].ToString();
                textBox1.Text =  rd["ProductCost"].ToString();
                textBox4.Text = rd["ProductDiscountAmount"].ToString();
                textBox5.Text = rd["ProductManufacturer"].ToString();
                comboBox1.Text = rd["ProductProvider"].ToString();
                comboBox2.Text = rd["ProductCategory"].ToString();
                textBox6.Text = rd["ProductCurrentDiscount"].ToString();
                textBox7.Text = rd["ProductQuantityInStock"].ToString();
                textBox8.Text = rd["ProductDescription"].ToString();
                textBox9.Text = rd["ProductPhoto"].ToString();




            }
            if (sqlConnect.State == ConnectionState.Closed)
                sqlConnect.Open();
        }
    }
}
