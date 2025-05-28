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
    public partial class ViewPart : Form
    {
        public ViewPart()
        {
            InitializeComponent();
            dataGridView1.Columns.Add("PartnerInfo", "Информация");
            dataGridView1.Columns.Add("Discount", "Скидка");
            dataGridView1.Columns.Add("PartnersID", "id");
            dataGridView1.Columns["PartnersID"].Visible=false;
        }
        string conStr = "host=localhost;user=root;pwd=;database=my4;";
        private void ViewPart_Load(object sender, EventArgs e)
        {
            MySqlConnection con = new MySqlConnection(conStr);
            con.Open();
            MySqlCommand cmd = new MySqlCommand("SELECT PartnersType.PartnersTypeName, PartnersName, PartnersDirector,PartnersPhone,PartnersRating, sum(Partner_Product.Partner_Product_Count), Partners.PartnersID" +
            " FROM Partners"+
            " LEFT JOIN Partner_Product ON Partner_Product.Partner_Product_PArtner = Partners.PartnersID"+
            " INNER JOIN PartnersType ON PartnersType.idPartnersType = Partners.PartnersType"+
            " GROUP by Partners.PartnersID; ", con);
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            MySqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                string ProductsTypeName = rdr["PartnersTypeName"].ToString();
                string PartnersName = rdr["PartnersName"].ToString();
                string PartnersDirector = rdr["PartnersDirector"].ToString();
                string PartnersPhone = rdr["PartnersPhone"].ToString();
                string PartnersRating = rdr["PartnersRating"].ToString();
                string id = rdr["PartnersID"].ToString();
                string vse = $"{ProductsTypeName} | {PartnersName}\n " +
                $"{PartnersDirector}\n" +
                $"{PartnersPhone}\n" +
                $"{PartnersRating}\n";


                if (rdr["sum(Partner_Product.Partner_Product_Count)"] != DBNull.Value)
                {
                    int totalSale = Convert.ToInt32(rdr["sum(Partner_Product.Partner_Product_Count)"]);
                    int discount = Disc(totalSale);
                    dataGridView1.Rows.Add(vse, discount, id);
                }
                else
                {
                    int discount = 0;
                    dataGridView1.Rows.Add(vse, discount, id);
                }
            }
            con.Close();
        }
        public int Disc(int totalSale)
        {
            if (totalSale <= 10000)
            {
                return 0;
            }
            else if (totalSale > 10000 && totalSale <= 50000)
            {
                return 5;
            }
            else if (totalSale > 50000 && totalSale <= 300000)
            {
                return 10;
            }
            else if (totalSale > 300000)
            {
                return 15;
            }
            return totalSale;
        }

        private void историяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ed = dataGridView1.CurrentCell.RowIndex;
            int id = Convert.ToInt32(dataGridView1.Rows[ed].Cells["PartnersID"].Value);
            History ae = new History(id);
            this.Hide();
            ae.Show();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >= 0)
            {
                int ed = dataGridView1.CurrentCell.RowIndex;
                int id = Convert.ToInt32(dataGridView1.Rows[ed].Cells["PartnersID"].Value);
                AddEdit ae = new AddEdit(id);
                this.Hide();
                ae.Show();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddEdit ae = new AddEdit();
            this.Hide();
            ae.Show();
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {

        }
    }
}
