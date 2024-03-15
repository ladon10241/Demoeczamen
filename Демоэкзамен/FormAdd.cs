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
    public partial class FormAdd : Form
    {
        public int id;
        public static string connString = @"Integrated Security=SSPI;Persist Security Info=False;User ID=ladon;Initial Catalog=mephi3.5;Data Source=lokiz";
        public SqlConnection sqlConnect;
        public FormAdd(SqlConnection connect, int id1)
        {
            id = id1;

            sqlConnect = connect;
            InitializeComponent(); 
      }

        private void FormAdd_Load(object sender, EventArgs e)
        {
            sqlConnect.Close();
            SqlCommand logRequst = new SqlCommand();
            sqlConnect.Open();
            logRequst.CommandText = ($"Select * from [TradeOPG].[dbo].[FIO] Where UserID ='{id}'");
            logRequst.Connection = sqlConnect;
            SqlDataReader rd = logRequst.ExecuteReader();
            rd.Read();
            label13.Text = rd.GetString(1) + rd.GetString(2) + rd.GetString(3);
            sqlConnect.Close();
        }

        private void login_button_Click(object sender, EventArgs e)
        {
            sqlConnect.Open();

            SqlCommand logRequst = new SqlCommand();
            logRequst.CommandType = CommandType.StoredProcedure;
            logRequst.CommandText = "ProductAdd";

            logRequst.Connection = sqlConnect;
            logRequst.Parameters.AddWithValue("@ProductArticleNumber", textBox1.Text);
            logRequst.Parameters.AddWithValue("@ProductName", textBox2.Text);
            logRequst.Parameters.AddWithValue("@ProductCost", textBox3.Text);
            logRequst.Parameters.AddWithValue("@ProductDiscountAmount", textBox4.Text);
            logRequst.Parameters.AddWithValue("@ProductManufacturer", textBox5.Text);
            logRequst.Parameters.AddWithValue("@ProductProvider",comboBox1.Text);
            logRequst.Parameters.AddWithValue("@ProductCategory", comboBox2.Text);
            logRequst.Parameters.AddWithValue("@ProductCurrentDiscount", textBox6.Text);
            logRequst.Parameters.AddWithValue("@ProductQuantityInStock", textBox7.Text);
            logRequst.Parameters.AddWithValue("@ProductDescription", textBox8.Text);
            logRequst.Parameters.AddWithValue("@ProductPhoto", textBox9.Text);
            logRequst.ExecuteNonQuery();
            Menu fu = new Menu(id);
            fu.Show();
            sqlConnect.Close();
        }
    }
}
