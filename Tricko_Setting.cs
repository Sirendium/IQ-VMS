using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace iSpyApplication
{
    public partial class Tricko_Setting : Form
    {
        public Tricko_Setting()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Tricko_Setting_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string color_track;
            color_track = huePicker1.Min + "|" + huePicker1.Max + "|" + "0|0,071|1.00|0.000|0,063|1.00|0.000|0|true|true|true";
            StreamWriter f1 = new StreamWriter("color_track.txt", false);
            StreamWriter f3 = new StreamWriter("test.txt", false);

            f1.Write(color_track);
            f3.Write("60" + "|" + "150" + "|" + "0|0.408|1.00|0.000|0.212|1.00|0.000|0|true|true|true");
            string name_obj;
            name_obj = textBox1.Text;
            StreamWriter f2 = new StreamWriter("C:/Windows/Name_OBJ.txt", false);
            f2.Write(name_obj);
            StreamWriter f4 = new StreamWriter("Checking_obj.bysec", false);
            f4.Write("1");
            f1.Close();
            f2.Close();
            f3.Close();
        }


    }
}
