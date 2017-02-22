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
        private SQLiteCommand command;

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
            try
            {
                if (dataGridView1.SelectedRows.Count != 0)
                {
                    sql_con.Open();
                    foreach (DataGridViewRow r in dataGridView1.SelectedRows)
                    {
                        command = new SQLiteCommand("DROP TABLE grades" + r.Cells["lastName"].Value
                            + r.Cells["firstName"].Value, sql_con);
                        command.ExecuteNonQuery();
                        command = new SQLiteCommand("DELETE FROM students WHERE firstName = '" + r.Cells["firstName"].Value + "' AND lastName = '"
                            + r.Cells["lastName"].Value + "'", sql_con);
                        command.ExecuteNonQuery(); 
                    }
                    sql_con.Close();
                    Form1 f = (Form1)System.Windows.Forms.Application.OpenForms["Form1"];
                    f.refreshStudentsTable();
                    this.Close();
                }
            }
            catch (Exception e1)
            { }
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
