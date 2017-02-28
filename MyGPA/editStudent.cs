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
    public partial class editStudent : Form
    {
        private String firstName;
        private String lastName;
        private SQLiteConnection sql_con;
        private SQLiteCommand command;
        private SQLiteConnection connect;

        public editStudent(String ln, String fn)
        {
            InitializeComponent();
            firstName = fn;
            lastName = ln;
            sql_con = new SQLiteConnection("Data Source = studentsGPA.sqlite");
            connect = new SQLiteConnection("Data Source = weightAverage.sqlite");
            refreshGrades();
        }

        public void refreshGrades()
        {
            try
            {
                sql_con.Open();
                SQLiteDataAdapter sqlData = new SQLiteDataAdapter("select * from grades"
                    + lastName + firstName, sql_con);
                DataTable dt = new DataTable();
                sqlData.Fill(dt);
                this.dataGridView1.DataSource = dt;
                sql_con.Close();
            }
            catch (Exception e)
            { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(Application.OpenForms.OfType<EditStudentAddClass>().Count()==0)
            {
                EditStudentAddClass es = new EditStudentAddClass(lastName, firstName);
                es.Show();
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count != 0)
                {
                    sql_con.Open();
                    foreach (DataGridViewRow r in dataGridView1.SelectedRows)
                    {
                        command = new SQLiteCommand("DELETE FROM grades"+ lastName+firstName +" WHERE className LIKE '%" + r.Cells["className"].Value + "%' AND year = "
                            + r.Cells["year"].Value, sql_con);
                        command.ExecuteNonQuery();
                    }
                    sql_con.Close();
                    refreshGrades();
                }
            }
            catch (Exception e1)
            { }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public double updateGPA()
        {
            connect.Open();
            double total = 0;
            if (dataGridView1.Rows.Count > 0)
            {
                foreach (DataGridViewRow r in dataGridView1.Rows)
                {
                    int tier = (int)r.Cells["tier"].Value;
                    int average = (int)r.Cells["average"].Value;
                    double credits = (double)r.Cells["credit"].Value;
                    int count = 0;
                    if(average > 70)
                    {
                        if (average >= 97) command = new SQLiteCommand("select 97 from averages where tier = " + tier, connect);
                        else if (average >= 93) command = new SQLiteCommand("SELECT 93 FROM averages WHERE tier = " + tier, connect);
                        else if (average >= 90) command = new SQLiteCommand("SELECT 90 FROM averages WHERE tier = " + tier, connect);
                        else if (average >= 87) command = new SQLiteCommand("SELECT 87 FROM averages WHERE tier = " + tier, connect);
                        else if (average >= 83) command = new SQLiteCommand("SELECT 83 FROM averages WHERE tier = " + tier, connect);
                        else if (average >= 80) command = new SQLiteCommand("SELECT 80 FROM averages WHERE tier = " + tier, connect);
                        else if (average >= 77) command = new SQLiteCommand("SELECT 77 FROM averages WHERE tier = " + tier, connect);
                        else if (average >= 73) command = new SQLiteCommand("SELECT 73 FROM averages WHERE tier = " + tier, connect);
                        else if (average >= 71) command = new SQLiteCommand("SELECT 71 FROM averages WHERE tier = " + tier, connect);
                        total += (double)command.ExecuteScalar();
                        count++;
                    }
                    
                }
            }
            connect.Close();
            return total;
        }
    }
}
