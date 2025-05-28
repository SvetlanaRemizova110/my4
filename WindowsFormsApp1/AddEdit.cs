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
    public partial class AddEdit : Form
    {
        int PartnersID;
        string PartnersTypeID=string.Empty;
        public AddEdit()
        {
            InitializeComponent();
            button1.Text = "Добавить";
            ComboBox();
        }
        public AddEdit(int id)
        {
            InitializeComponent();
            this.PartnersID = id;
            button1.Text = "Редактировать";
            FillLoad(); 
            Combo();
        }
        string conStr = "host=localhost;user=root;pwd=;database=my4;";
        void FillLoad()
        {
            MySqlConnection con = new MySqlConnection(conStr);
            con.Open();
            using (MySqlCommand cmd = new MySqlCommand($"SELECT * FROM Partners WHERE PartnersID = {PartnersID};", con))
            {
                MySqlDataReader rdr = cmd.ExecuteReader();
                while(rdr.Read()){
                    PartnersTypeID = rdr["PartnersType"].ToString();
                    textBoxName.Text = rdr["PartnersName"].ToString();
                    textBoxFIO.Text = rdr["PartnersDirector"].ToString();
                    textBoxAddress.Text = rdr["PartnersAddress"].ToString();
                    textBoxRating.Text = rdr["PartnersRating"].ToString();
                    textBoxTel.Text = rdr["PartnersPhone"].ToString();
                    textBoxEmail.Text = rdr["PartnersEmail"].ToString();
                }
            }
            con.Close();
        }
        void Combo()
        {
            MySqlConnection con = new MySqlConnection(conStr);
            con.Open();
            MySqlDataAdapter ad = new MySqlDataAdapter("SELECT * FROM PartnersType;", con);
            DataTable dt = new DataTable();
            ad.Fill(dt);
            comboBox1.DataSource = dt;
            comboBox1.DisplayMember = "PartnersTypeName";
            comboBox1.ValueMember = "idPartnersType";
            comboBox1.SelectedValue = PartnersTypeID;
        }
        void ComboBox()
        {
            MySqlConnection con = new MySqlConnection(conStr);
            con.Open();
            using (MySqlCommand cmdtype = new MySqlCommand("SELECT PartnersTypeName FROM PartnersType;", con))
            {
                using(MySqlDataReader rdr = cmdtype.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        comboBox1.Items.Add(rdr["PartnersTypeName"]);
                    }
                }
            }
            con.Close();
        }
        private void AddEdit_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Добавить")
            {
                MySqlConnection con = new MySqlConnection(conStr);
                con.Open();
                int typeid;
                using(MySqlCommand cmdtype = new MySqlCommand("SELECT idPartnersType FROM PartnersType WHERE PartnersTypeName = @PartnersTypeName;", con))
                {
                    cmdtype.Parameters.AddWithValue("@PartnersTypeName", comboBox1.Text);
                    object res = cmdtype.ExecuteScalar();
                    typeid = res != null ? Convert.ToInt32(res) : 0;
                }
                MySqlCommand cmd = new MySqlCommand("INSERT Partners (PartnersType,PartnersName,PartnersDirector,PartnersEmail,PartnersPhone,PartnersAddress,PartnersRating) " +
                    "VALUES (@PartnersType,@PartnersName,@PartnersDirector,@PartnersEmail,@PartnersPhone,@PartnersAddress,@PartnersRating);", con);
                cmd.Parameters.AddWithValue("@PartnersType", typeid);
                cmd.Parameters.AddWithValue("@PartnersName", textBoxName.Text);
                cmd.Parameters.AddWithValue("@PartnersDirector", textBoxFIO.Text);
                cmd.Parameters.AddWithValue("@PartnersEmail", textBoxEmail.Text);
                cmd.Parameters.AddWithValue("@PartnersPhone", textBoxTel.Text);
                cmd.Parameters.AddWithValue("@PartnersAddress", textBoxAddress.Text);
                cmd.Parameters.AddWithValue("@PartnersRating", textBoxRating.Text);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Добалена", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ViewPart ae = new ViewPart();
                this.Hide();
                ae.Show();
            }
            if (button1.Text == "Редактировать")
            {
                MySqlConnection con = new MySqlConnection(conStr);
                con.Open();
                int typeid;
                using (MySqlCommand cmdtype = new MySqlCommand("SELECT idPartnersType FROM PartnersType WHERE PartnersTypeName = @PartnersTypeName;", con))
                {
                    cmdtype.Parameters.AddWithValue("@PartnersTypeName", comboBox1.Text);
                    object res = cmdtype.ExecuteScalar();
                    typeid = res != null ? Convert.ToInt32(res) : 0;
                }
                MySqlCommand cmd = new MySqlCommand("UPDATE Partners SET PartnersType = @PartnersType, " +
                    "PartnersName=@PartnersName," +
                    "PartnersDirector=@PartnersDirector," +
                    "PartnersEmail=@PartnersEmail," +
                    "PartnersPhone=@PartnersPhone," +
                    "PartnersAddress=@PartnersAddress," +
                    "PartnersRating=@PartnersRating" +
                    $" WHERE PartnersID ={PartnersID}; ", con);
                cmd.Parameters.AddWithValue("@PartnersType", typeid);
                cmd.Parameters.AddWithValue("@PartnersName", textBoxName.Text);
                cmd.Parameters.AddWithValue("@PartnersDirector", textBoxFIO.Text);
                cmd.Parameters.AddWithValue("@PartnersEmail", textBoxEmail.Text);
                cmd.Parameters.AddWithValue("@PartnersPhone", textBoxTel.Text);
                cmd.Parameters.AddWithValue("@PartnersAddress", textBoxAddress.Text);
                cmd.Parameters.AddWithValue("@PartnersRating", textBoxRating.Text);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Изменено", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ViewPart ae = new ViewPart();
                this.Hide();
                ae.Show();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ViewPart ae = new ViewPart();
            this.Hide();
            ae.Show();
        }
    }
}