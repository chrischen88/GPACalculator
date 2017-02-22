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
using System.Diagnostics;

namespace MyGPA
{
    public partial class RemoveStudent : Form
    {
        private SQLiteConnection sql_con;

        public RemoveStudent()
        {
            InitializeComponent();
            sql_con = new SQLiteConnection("Data Source = studentsGPA.sqlite");
            sql_con.Open();
            SQLiteDataAdapter sqlData = new SQLiteDataAdapter("select lastName, firstName from students", sql_con);
            DataTable dt = new DataTable();
            sqlData.Fill(dt);
            this.dataGridView1.DataSource = dt;
            sql_con.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count!=0)
            {
                foreach(DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    Form1 f = (Form1)System.Windows.Forms.Application.OpenForms["Form1"];
                    String fn = row.Cells[1].Value.ToString();
                    String ln = row.Cells[0].Value.ToString();
                    String gpa = row.Cells[2].Value.ToString();
                    String c = row.Cells[3].Value.ToString();

                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                sql_con.Open();
                SQLiteDataAdapter sqlData = new SQLiteDataAdapter("select lastName, firstName from students WHERE lastName LIKE '%" + nameBox.Text +"%'", sql_con);
                DataTable dt = new DataTable();
                sqlData.Fill(dt);
                this.dataGridView1.DataSource = dt;
                sql_con.Close();
            }
            catch (Exception e1)
            { }
        }
    }
}
