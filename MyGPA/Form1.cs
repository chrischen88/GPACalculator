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
                if(Application.OpenForms.OfType<Form2>().Count() == 0 && Application.OpenForms.OfType<RemoveStudent>().Count() == 0)
                {
                    Form2 addStudent = new Form2();
                    addStudent.Show();
                }
            }
            catch (Exception e1) { }
        }

        public void addStudent(String fs, String ls)
        {
            try
            {
                sql_con.Open();
                command = new SQLiteCommand("insert into students(lastName, firstName, GPA, credits) values ('" +
                    ls + "', '" + fs + "', '0.000', 0)", sql_con);
                //Debug.WriteLine(fs + " " + ls);
                command.ExecuteNonQuery();
                sql_con.Close();
                //Debug.WriteLine("addStudent ran.");
            }
            catch (Exception e1) { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (Application.OpenForms.OfType<RemoveStudent>().Count() == 0 && Application.OpenForms.OfType<Form2>().Count() == 0)
                {
                    RemoveStudent rs = new RemoveStudent();
                    rs.Show();
                }
            }
            catch (Exception e1) { }
        }

        public void removeStudent()
        {
            
        }
    }
}
