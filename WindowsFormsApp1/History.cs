using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace WindowsFormsApp1
{
    public partial class History : Form
    {
        int PartnersID;
        string conStr = "host=localhost;user=root;pwd=;database=my4;";
        public History(int id)
        {
            InitializeComponent();
            this.PartnersID = id;
        }

        private void History_Load(object sender, EventArgs e)
        {
            MySqlConnection con = new MySqlConnection(conStr);
            con.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT Partners.PartnersName AS 'Партнеры', Products.ProductsName AS 'Продукция', " +
                "Partner_Product_Count AS 'Количество',Partner_Product_Date AS 'Дата' " +
                "FROM Partner_Product " +
                "INNER JOIN Partners ON Partners.PartnersID = Partner_Product.Partner_Product_PArtner " +
                "INNER JOIN  Products ON Products.ProductsArticul = Partner_Product.Partner_Product_Product " +
                $"WHERE Partners.PartnersID = {PartnersID};", con);
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.Columns.Add("PartnersName", "Партнеры");
            dataGridView1.Columns["PartnersName"].Visible=false;
            dataGridView1.Columns.Add("ProductsName", "Продукция");
            dataGridView1.Columns.Add("Partner_Product_Count", "Количество");
            dataGridView1.Columns.Add("Partner_Product_Date", "Дата");
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(dataGridView1, rdr[0].ToString(), rdr[1].ToString(), rdr[2].ToString(), rdr[3].ToString());
                dataGridView1.Rows.Add(row);
            }
            con.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ViewPart ae = new ViewPart();
            this.Hide();
            ae.Show();
        }
    }
}
