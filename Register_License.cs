using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace iSpyApplication
{
    public partial class Register_License : Form
    {
        public Register_License()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string email = textBox1.Text;
            string Nickname = textBox2.Text;
            string ID = textBox3.Text;
            string Serial_key = textBox4.Text;

            if(Serial_key == "BYSEC504") { 
            MessageBox.Show("Account Register", "You have licanse.");
                this.Close();
            }

        }
    }
}
