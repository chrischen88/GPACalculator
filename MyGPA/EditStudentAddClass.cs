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
            sql_con = new SQLiteConnection("Data Source = studentsGPA.sqlite");
            connect = new SQLiteConnection("Data Source = ClassWeight.db");
            firstName = fn;
            lastName = ln;
            connect.Open();
            SQLiteDataAdapter sqlData = new SQLiteDataAdapter("SELECT * FROM ClassWeights", connect);
            DataTable dt = new DataTable();
            sqlData.Fill(dt);
            this.dataGridView1.DataSource = dt;
            connect.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sql_con.Open();
            if(dataGridView1.SelectedRows.Count == 1)
            {
                if (Convert.ToDouble(dataGridView1.SelectedRows[0].Cells["credit"].Value.ToString()) > 0.5)
                {
                    try
                    {
                        DataGridViewRow r = dataGridView1.SelectedRows[0];
                        command = new SQLiteCommand("INSERT INTO grades"+lastName+firstName+"(className, average, year) VALUES ('"
                            + r.Cells["className"].Value +" Semester 1', '"+ textBox2.Text +"', "+ textBox3.Text +")",sql_con);
                        command.ExecuteNonQuery();
                        command = new SQLiteCommand("INSERT INTO grades" + lastName + firstName + "(className, average, year) VALUES ('"
                            + r.Cells["className"].Value + " Semester 2', '" + textBox4.Text + "', " + textBox3.Text + ")",sql_con);
                        command.ExecuteNonQuery();
                    }
                    catch (Exception e1)
                    {

                    }
                }
                else
                {

                }
            }
            else
            {
                MessageBox.Show("Selected more than 1 class","ERROR");
            }
            sql_con.Close();
            editStudent es = (editStudent)System.Windows.Forms.Application.OpenForms["editStudent"];
            es.refreshGrades();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                connect.Open();
                SQLiteDataAdapter sqlData = new SQLiteDataAdapter("select * from ClassWeights WHERE className LIKE '%" + textBox1.Text + "%'", connect);
                DataTable dt = new DataTable();
                sqlData.Fill(dt);
                this.dataGridView1.DataSource = dt;
                connect.Close();
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
