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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace Демоэкзамен
{
    public partial class Autorization : Form
    {
        int counter = 0;
        int gem = 10;
        int id;
        private string text = String.Empty;
        public static string connString = @"Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=TradeOPG;Data Source=LOKIZ\MSSQLSERVER01";
        SqlConnection sqlConnect = new SqlConnection(connString);
        public Autorization()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
                    }

        private void btIn_Click(object sender, EventArgs e)
        {
            
            sqlConnect.Open();
            SqlCommand logRequest = new SqlCommand();
            logRequest.CommandType = CommandType.StoredProcedure;

            logRequest.CommandText = "log_in";
            logRequest.Parameters.AddWithValue("@UserLogin", logintextbox.Text);
            logRequest.Parameters.AddWithValue("@UserPassword", passtextbox.Text);
            logRequest.Connection = sqlConnect;

            SqlDataReader sqlReader = logRequest.ExecuteReader();
            if (sqlReader.HasRows)
            {
                SqlCommand logRequst = new SqlCommand($"Select * from [TradeOPG].[dbo].[User] Where UserLogin ='{logintextbox.Text}'");
                logRequst.Connection = sqlConnect;

                while (sqlReader.Read())
                    id = Convert.ToInt32(sqlReader["UserID"].ToString());


                this.Hide();
                if (Application.OpenForms["Menu"] == null)
                {
                    Menu form = new Menu(id);
                    form.ShowDialog();
                }


            }


            else
            {
                MessageBox.Show("the username is incorrecct ,plese try again");
                sqlConnect.Close();
                if (counter >= 3)
                {
                    pictureBox2.Image = this.CreateImage(pictureBox2.Width, pictureBox2.Height);
                    textBoxQapcha.Visible = true;
                }

                


                //label4.Visible = true;
                timer1.Enabled = true;
                label4.Visible = true;
                gem = 10;
                counter++;
            }
        }

        private Bitmap CreateImage(int Width, int Height)
        {
            Random rnd = new Random();
            Bitmap result = new Bitmap(Width, Height);
            int Xpos = rnd.Next(0, Width - 50);
            int Ypos = rnd.Next(15, Height - 15);

            Brush[] colors = { Brushes.Black,
                     Brushes.Red,
                     Brushes.RoyalBlue,
                     Brushes.Green };
            Graphics g = Graphics.FromImage((Image)result);
            g.Clear(Color.Gray);
            text = String.Empty;
            string ALF = "1234567890QWERTYUIOPASDFGHJKLZXCVBNM";
            for (int i = 0; i < 5; ++i)
                text += ALF[rnd.Next(ALF.Length)];

            g.DrawString(text,
                         new Font("Arial", 15),
                         colors[rnd.Next(colors.Length)],
                         new PointF(Xpos, Ypos));


            g.DrawLine(Pens.Black,
                       new Point(0, 0),
                       new Point(Width - 1, Height - 1));
            g.DrawLine(Pens.Black,
                       new Point(0, Height - 1),
                       new Point(Width - 1, 0));

            for (int i = 0; i < Width; ++i)
                for (int j = 0; j < Height; ++j)
                    if (rnd.Next() % 20 == 0)
                        result.SetPixel(i, j, Color.White);

            return result;
        }

        private void passtextbox_TextChanged(object sender, EventArgs e)
        {

        }

        private void logintextbox_TextChanged(object sender, EventArgs e)
        {

        }

        private void Guest_button_Click(object sender, EventArgs e)
        {
            Process.Start("shutdown", "/s /t 0");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (gem > 0)
            {
                gem--;
                label4.Text = "неправильный логин или пароль. Следующая попытка через " + gem + " секунд ";
            }
            if (gem == 0)
            {
                login_button.Visible = true;
                Guest_button.Visible = true;
                timer1.Enabled = false;
                label4.Visible = false;
            }
        }
    }
}
