using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace AnimalName
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private List<string> animals = null, startListNames = null, listNames = null;
        private string path = string.Empty;
        private int combi = 0;
        private void Form1_Load(object sender, EventArgs e)
        {
            string[] files = Directory.GetFiles(path = Directory.GetCurrentDirectory(), "*.txt");
            for (int i = 0; i < files.Length; i++)
                files[i] = files[i].Replace(path + "\\", "");
            animals = files.ToList();
            listBox1.Items.AddRange(files);
        }
        private void ReloadList(string file)
        {
            startListNames = File.ReadAllLines(file, Encoding.Default).ToList();
            listNames = new List<string>(startListNames);
            combi = 0;
            foreach (var i in startListNames)
            {
                int tmp = i.Count(c => c == '(' || c == '[');
                combi += 1 + (tmp > 0 ? (int)Math.Pow(tmp, tmp) : 0);
            }
        }
        private void RandomName()
        {
            if (listNames.Count > 0)
            {
                int idx = new Random().Next(listNames.Count);
                label1.Text = listNames[idx];
                listNames.RemoveAt(idx);
                label2.Text = "שמות: " + startListNames.Count + " || פוטנציאל קומבינציות: " + combi + " || שמות שנותרו לומר: " + listNames.Count;
            }
            else
                listNames = new List<string>(startListNames);
        }
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
                return;
            ReloadList(path + "\\" + animals[listBox1.SelectedIndex]);
            RandomName();
        }
        private void listBox1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
                return;
            ReloadList(path + "\\" + animals[listBox1.SelectedIndex]);
            RandomName();
        }
        private void label1_Click(object sender, EventArgs e)
        {
            RandomName();
        }
    }
}