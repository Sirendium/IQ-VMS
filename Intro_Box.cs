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
    public partial class Intro_Box : Form
    {
        public Intro_Box()
        {
            InitializeComponent();
        }

        private void Intro_Box_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
        
            this.Close();
        }
    }
}
