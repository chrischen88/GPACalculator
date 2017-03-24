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
            SQLiteDataAdapter sqlData = new SQLiteDataAdapter("SELECT className AND tier FROM ClassWeights", sql_con);
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
            
        }
    }
}
