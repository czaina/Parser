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
            Plik = new CFragment();
            CFragment temp;

            Plik.text = textBox1.Lines[0];
            temp = Plik.Nowy(textBox1.Lines[1]);
            for (int i=2;i<10;i++)
            {
                  temp = temp.Nowy(textBox1.Lines[i]);
            }

            textBox2.Clear();

            temp = Plik;
            Plik.PrintAll(textBox2);
        }
    }
}