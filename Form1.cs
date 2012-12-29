using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace FPSViewer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string xmlfile;
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "FPS题目文件(*.xml)|*.xml";
            op.ValidateNames = true;
            op.CheckFileExists = true;
            op.CheckPathExists = true;
            op.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            if (op.ShowDialog() == DialogResult.OK) xmlfile = op.FileName;
            else return;

            XmlTextReader xmldoc = new XmlTextReader(xmlfile);
            int i = 1;
            try
            {
                while (xmldoc.Read())
                {
                    if(xmldoc.NodeType==XmlNodeType.Element){
                        if (xmldoc.Name == "title")
                        {
                            xmldoc.Read();
                            comboBox1.Items.Add(i.ToString() + ". " + xmldoc.Value);
                            i++;
                        }
                        //textBox1.Text += Environment.NewLine + xmldoc.Name+" " ;
                    }
                }
            }
            catch (XmlException exp)
            {
                MessageBox.Show("文件格式错误!");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = comboBox1.SelectedIndex,i=-1;
            XmlTextReader xmldoc = new XmlTextReader(xmlfile);
            while (xmldoc.Read())
                if (xmldoc.NodeType == XmlNodeType.Element)
                {
                    Application.DoEvents();
                    if (xmldoc.Name == "item") i++;
                    if (i == index) {
                        if (xmldoc.Name == "description")
                        {
                            xmldoc.Read(); textBox1.Text = xmldoc.Value;
                        }
                        else if (xmldoc.Name == "input")
                        {
                            xmldoc.Read(); textBox2.Text = xmldoc.Value;
                        }
                        else if (xmldoc.Name == "output")
                        {
                            xmldoc.Read(); textBox3.Text = xmldoc.Value;
                        }
                        else if (xmldoc.Name == "sample_input")
                        {
                            xmldoc.Read(); textBox4.Text = xmldoc.Value;
                        }
                        else if (xmldoc.Name == "sample_output")
                        {
                            xmldoc.Read(); textBox5.Text = xmldoc.Value;
                        }
                        else if (xmldoc.Name == "hint")
                        {
                            xmldoc.Read(); textBox6.Text = xmldoc.Value;
                        }
                    }
                    else if (i > index) break;
                }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string path = "C:";

            FolderBrowserDialog dlg = new FolderBrowserDialog();
            if (dlg.ShowDialog() == DialogResult.OK) path = dlg.SelectedPath;
            else return;

            //Char[] buf=new Char[10000000];
            int index = comboBox1.SelectedIndex, i = -1,c=0;
            if (index < 0) { MessageBox.Show("请先选择题目!"); return; }
            XmlTextReader xmldoc = new XmlTextReader(xmlfile);

            while (xmldoc.Read())
                if (xmldoc.NodeType == XmlNodeType.Element)
                {
                    Application.DoEvents();
                    if (xmldoc.Name == "item") i++;
                    if (i == index)
                    {
                        if (xmldoc.Name == "test_input")
                        {
                            c++;
                            //int len=xmldoc.ReadChars(buf,0,10000000);
                            //StreamWriter sw = new StreamWriter(path + "\\data" + c.ToString() + ".in", false, Encoding.ASCII);
                            xmldoc.Read();
                            File.WriteAllText(path + "\\data" + c.ToString() + ".in", xmldoc.Value, Encoding.ASCII);
                            
                        }
                        else if (xmldoc.Name == "test_output")
                        {
                            xmldoc.Read();
                            File.WriteAllText(path + "\\data" + c.ToString() + ".out", xmldoc.Value, Encoding.ASCII);
                        }
                    }
                    else if (i > index) break;
                }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
