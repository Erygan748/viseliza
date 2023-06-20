using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Hangman_Game
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (!checkTextBox()) {

                MessageBox.Show("Повтори");
                return;
            }

            FileStream fs = new FileStream("text.txt", FileMode.Append, FileAccess.Write);
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.Write("\n" + textBox1.Text); 
            }
        }

        private bool checkTextBox()
        {

            string result = textBox1.Text;
            return result.All(char.IsLetter) && result.Length >= 1; // проыеркв что введена буква 
            
        }
    }
}
