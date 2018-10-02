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

namespace Parser
{
    public partial class Form1 : Form
    {
        public Form1()
        {
           InitializeComponent();
        }

        CFragment Plik;

        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            StreamReader objReader;
            string textLine;


            textBox1.Clear();
            foreach (string file in files)
            {
                objReader = new StreamReader(file);
                do
                {
                    textLine = objReader.ReadLine();
                    textLine = textLine.TrimStart(null);
                    textLine = textLine.TrimEnd(null);
                    if (textLine != "")
                        textBox1.Text += textLine + "\r\n"; ;
                } while (objReader.Peek() != -1);
                objReader.Close();
            }
        }

        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void textBox2_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            textBox2.Clear();
            foreach (string file in files)
            {
                textBox2.Text += File.ReadAllText(file);
            }
        }

        private void textBox2_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void B_Load_Click(object sender, EventArgs e)
        {
            string file = "driver_5110_lcd.c";
            StreamReader objReader;
            string textLine;


            textBox1.Clear();
            objReader = new StreamReader(file);
            do
            {
                textLine = objReader.ReadLine();
                textLine = textLine.TrimStart(null);
                textLine = textLine.TrimEnd(null);
                if (textLine != "")
                    textBox1.Text += textLine + "\r\n";
            } while (objReader.Peek() != -1);
            objReader.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Plik = new CFragment(textBox1.Text);

            textBox2.Clear();

            Plik.PrintAll(textBox2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string text;
            string[] fragmenty;
            string[] separator = new string[] { "//"};

            text = textBox3.Text;
            textBox6.Text = separator[0];
            fragmenty = text.Split(separator, 2, System.StringSplitOptions.None);
            //[0] istnieje zawsze i zawiera to co przed separatorem
            //[1] istnieje tylko gdy jest separator
            if (fragmenty.Count() > 0) textBox4.Text = fragmenty[0]; else textBox4.Text = "---";
            if (fragmenty.Count() > 1) textBox5.Text = fragmenty[1]; else textBox5.Text = "---";
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
    }
}