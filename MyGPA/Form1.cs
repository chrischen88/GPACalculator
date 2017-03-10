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
    public partial class Form1 : Form
    {
        private SQLiteConnection sql_con;
        private SQLiteCommand command;

        public Form1()
        {
            InitializeComponent();
            sql_con = new SQLiteConnection("Data Source = studentsGPA.sqlite");
            refreshStudentsTable();
            updateMultipleGPA();
            refreshStudentsTable();
        }

        public void refreshStudentsTable()
        {
            try
            {
                sql_con.Open();
                SQLiteDataAdapter sqlData = new SQLiteDataAdapter("select * from students", sql_con);
                DataTable table = new DataTable();
                sqlData.Fill(table);
                this.dataGridView1.DataSource = table;
                sql_con.Close();
                //Debug.WriteLine("refreshStudentTable ran.");
            }
            catch (Exception e1) { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if(Application.OpenForms.OfType<Form2>().Count() == 0)
                {
                    Form2 addStudent = new Form2();
                    addStudent.Show();
                }
            }
            catch (Exception e1) { }
        }

        public void addStudent(String fs, String ls, int grade)
        {
            try
            {
                sql_con.Open();
                command = new SQLiteCommand("insert into students(lastName, firstName, grade, GPA, credits) values ('" +
                    ls + "', '" + fs + "', " + grade +", '0.000', 0)", sql_con);
                command.ExecuteNonQuery();
                command = new SQLiteCommand("create table grades" + ls + fs + "(className varchar(20), average int, tier int, exempted text)", sql_con);
                command.ExecuteNonQuery();
                sql_con.Close();
            }
            catch (Exception e1) { }
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
                        command = new SQLiteCommand("DROP TABLE grades" + r.Cells["lastName"].Value
                            + r.Cells["firstName"].Value, sql_con);
                        command.ExecuteNonQuery();
                        command = new SQLiteCommand("DELETE FROM students WHERE firstName = '" + r.Cells["firstName"].Value + "' AND lastName = '"
                            + r.Cells["lastName"].Value + "'", sql_con);
                        command.ExecuteNonQuery();
                    }
                    sql_con.Close();
                    refreshStudentsTable();
                }
            }
            catch (Exception e1)
            { }
        }

        private void editStudent_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms.OfType<editStudent>().Count() == 0)
            {
                sql_con.Open();
                if (dataGridView1.SelectedRows.Count == 1)
                {
                    DataGridViewRow r = dataGridView1.SelectedRows[0];
                    editStudent es = new MyGPA.editStudent((String)r.Cells["lastName"].Value, (String)r.Cells["firstName"].Value);
                    es.Show();
                }
                else
                {
                    MessageBox.Show("Please select 1 student", "ERROR");
                }
                sql_con.Close();
            }
        }

        private void updateMultipleGPA()
        {
            sql_con.Open();
            foreach(DataGridViewRow r in dataGridView1.Rows)
            {
                editStudent es = new MyGPA.editStudent((String)r.Cells["lastName"].Value, (String)r.Cells["firstName"].Value);
                es.updateGPA();
                es.Close();
            }
            sql_con.Close();
        }

    }
}
