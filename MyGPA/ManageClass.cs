using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace MyGPA
{
    public partial class ManageClass : Form
    {
        private SQLiteConnection sql_con;
        private SQLiteCommand command;

        public ManageClass()
        {
            InitializeComponent();
            sql_con = new SQLiteConnection("Data Source = ClassWeight.db");
            sql_con.Open();
            SQLiteDataAdapter sqlData = new SQLiteDataAdapter("SELECT className, tier FROM ClassWeights", sql_con);
            DataTable dt = new DataTable();
            sqlData.Fill(dt);
            this.dataGridView1.DataSource = dt;
            sql_con.Close();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                sql_con.Open();
                command = new SQLiteCommand("INSERT INTO ClassWeights(className, tier, credit) VALUES ('" + textBox1.Text + "', '" + textBox2.Text + "', '" + textBox3.Text + "')",sql_con);
                command.ExecuteNonQuery();
                sql_con.Close();
                refreshClasses();
            }
            catch(Exception e1)
            {

            }
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            try
            {
                sql_con.Open();
                SQLiteDataAdapter sqlData = new SQLiteDataAdapter("select * from ClassWeights WHERE className LIKE '%" + textBox4.Text + "%'", sql_con);
                DataTable dt = new DataTable();
                sqlData.Fill(dt);
                this.dataGridView1.DataSource = dt;
                sql_con.Close();
            }
            catch (Exception e1)
            { }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                sql_con.Open();
                foreach(DataGridViewRow r in dataGridView1.SelectedRows)
                {
                    command = new SQLiteCommand("DELETE FROM ClassWeights WHERE className LIKE '%" + r.Cells["className"].Value + "%' AND tier = " +
                        r.Cells["tier"].Value, sql_con);
                    command.ExecuteNonQuery();
                }
                sql_con.Close();
                refreshClasses();
            }
            catch(Exception e1)
            {

            }
        }

        private void refreshClasses()
        {
            sql_con.Open();
            SQLiteDataAdapter sqlData = new SQLiteDataAdapter("SELECT className, tier FROM ClassWeights", sql_con);
            DataTable dt = new DataTable();
            sqlData.Fill(dt);
            this.dataGridView1.DataSource = dt;
            sql_con.Close();
        }
    }
}
