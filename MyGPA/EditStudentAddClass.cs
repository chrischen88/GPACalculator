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
    public partial class EditStudentAddClass : Form
    {
        private SQLiteConnection sql_con;
        private SQLiteCommand command;
        private SQLiteConnection connect;
        private String firstName;
        private String lastName;

        public EditStudentAddClass(String ln, String fn)
        {
            InitializeComponent();
            connect = new SQLiteConnection("Data Source = studentsGPA.sqlite");
            sql_con = new SQLiteConnection("Data Source = ClassWeight.db");
            firstName = fn;
            lastName = ln;
            sql_con.Open();
            SQLiteDataAdapter sqlData = new SQLiteDataAdapter("SELECT * FROM ClassWeights", sql_con);
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
            sql_con.Open();
            
            sql_con.Close();
            connect.Open();
            
            connect.Close();
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                sql_con.Open();
                SQLiteDataAdapter sqlData = new SQLiteDataAdapter("select * from ClassWeights WHERE className LIKE '%" + textBox1.Text + "%'", sql_con);
                DataTable dt = new DataTable();
                sqlData.Fill(dt);
                this.dataGridView1.DataSource = dt;
                sql_con.Close();
            }
            catch (Exception e1)
            { }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(Convert.ToDouble(dataGridView1.SelectedRows[0].Cells["credit"].Value.ToString() )> 0.5)
            {
                label3.Visible = true;
                textBox4.Visible = true;
                this.ClientSize = new System.Drawing.Size(382, 303);
                this.button1.Location = new System.Drawing.Point(12, 275);
                this.button2.Location = new System.Drawing.Point(298, 275);
            }
            else
            {
                label3.Visible = false;
                textBox4.Visible = false;
                this.ClientSize = new System.Drawing.Size(382, 280);
                this.button1.Location = new System.Drawing.Point(12, 254);
                this.button2.Location = new System.Drawing.Point(298, 254);
            }
        }
    }
}
