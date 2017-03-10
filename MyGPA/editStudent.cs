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
            sql_con.Open();
            command = new SQLiteCommand("SELECT grade FROM students WHERE firstName = '" + firstName + "' AND lastName = '" + lastName + "'", sql_con);
            textBox1.Text = Convert.ToString(command.ExecuteScalar());
            sql_con.Close();
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
            { Debug.WriteLine("Refresh Grades Failed"); }
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
                        Debug.WriteLine("It removed");
                    }
                    sql_con.Close();
                    Form1 f = (Form1)System.Windows.Forms.Application.OpenForms["Form1"];
                    updateGPA();
                    refreshGrades();
                    f.refreshStudentsTable();
                }
            }
            catch (Exception e1)
            { }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            sql_con.Open();
            command = new SQLiteCommand("UPDATE students SET grade = " + textBox1.Text + " WHERE lastName = '" + lastName + "' AND firstName = '" + firstName + "'", sql_con);
            command.ExecuteNonQuery();
            sql_con.Close();
            Form1 f = (Form1)System.Windows.Forms.Application.OpenForms["Form1"];
            f.refreshStudentsTable();
            this.Close();
        }

        public void updateGPA()
        {
            try
            {
                connect.Open();
                sql_con.Open();
                double total = 0;
                int count = 0;
                double totalCredits = 0;
                if (dataGridView1.Rows.Count > 0)
                {
                    foreach (DataGridViewRow r in dataGridView1.Rows)
                    {
                        int tier = Convert.ToInt32(r.Cells["tier"].Value);
                        int average = Convert.ToInt32(r.Cells["average"].Value);
                        String exempted = (String)r.Cells["exempted"].Value;
                        if (average > 70)
                        {
                            if(exempted.Equals("NO"))
                            {
                                if (average >= 97) command = new SQLiteCommand("select g97 from averages where tier = " + tier, connect);
                                else if (average >= 93) command = new SQLiteCommand("SELECT g93 FROM averages WHERE tier = " + tier, connect);
                                else if (average >= 90) command = new SQLiteCommand("SELECT g90 FROM averages WHERE tier = " + tier, connect);
                                else if (average >= 87) command = new SQLiteCommand("SELECT g87 FROM averages WHERE tier = " + tier, connect);
                                else if (average >= 83) command = new SQLiteCommand("SELECT g83 FROM averages WHERE tier = " + tier, connect);
                                else if (average >= 80) command = new SQLiteCommand("SELECT g80 FROM averages WHERE tier = " + tier, connect);
                                else if (average >= 77) command = new SQLiteCommand("SELECT g77 FROM averages WHERE tier = " + tier, connect);
                                else if (average >= 73) command = new SQLiteCommand("SELECT g73 FROM averages WHERE tier = " + tier, connect);
                                else                    command = new SQLiteCommand("SELECT g71 FROM averages WHERE tier = " + tier, connect);
                                total += Convert.ToDouble(command.ExecuteScalar());
                                count++;
                            }
                            totalCredits += 0.5;
                        }
                    }
                    if(count> 0)
                    {
                        total /= count;
                    }
                }
                command = new SQLiteCommand("UPDATE students SET GPA = " + Math.Round(total, 3) + " WHERE firstName = '" + firstName + "' AND lastName = '" + lastName + "'", sql_con);
                command.ExecuteNonQuery();
                command = new SQLiteCommand("UPDATE students SET credits = "+ totalCredits+ " WHERE firstName = '" + firstName + "' AND lastName = '" + lastName + "'", sql_con);
                command.ExecuteNonQuery();
                connect.Close();
                sql_con.Close();
            }
            catch (Exception e1) { Debug.WriteLine("Update failed"); }
        }
    }
}
