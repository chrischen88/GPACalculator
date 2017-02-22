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
    public partial class editStudent : Form
    {
        private String firstName;
        private String lastName;
        private SQLiteConnection sql_con;
        private SQLiteCommand command;

        public editStudent(String ln, String fn)
        {
            InitializeComponent();
            firstName = fn;
            lastName = ln;
            sql_con = new SQLiteConnection("Data Source = studentsGPA.sqlite");
            refreshGrades();
        }

        private void refreshGrades()
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
                EditStudentAddClass es = new EditStudentAddClass();
                es.Show();
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
