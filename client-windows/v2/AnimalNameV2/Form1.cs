using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace AnimalNameV2
{
    public partial class Form1 : MaterialForm
    {
        public Form1()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey700, Primary.BlueGrey800, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
        }
        private int[] widths = new int[3];
        private List<Animal> animals = new List<Animal>();
        public static string path = string.Empty;
        private int selectedAnimalIndex = -1;
        private int loading = -1;
        private const int LOAD_TIME = 50;
        private string say = string.Empty;
        private void Form1_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < materialListView1.Columns.Count; i++)
                widths[i] = materialListView1.Columns[i].Width;
            materialFlatButton1.Font = new Font("Roboto", 25f);
            materialLabel3.Font = new Font("Roboto", 9f, FontStyle.Bold);
            materialLabel3.ForeColor = Color.FromArgb(Primary.Red700.GetHashCode());
            materialLabel3.Visible = false;
            panel1.ForeColor = Color.FromArgb(Primary.BlueGrey900.GetHashCode());
            path = Directory.GetCurrentDirectory();
            string[] files = Directory.GetFiles(path, "*.txt");
            foreach (var file in files)
            {
                var newAnimal = new Animal(file);
                animals.Add(newAnimal);
                materialListView1.Items.Add(new ListViewItem(new string[] {
                    newAnimal.PrimaryName,
                    newAnimal.TotalNames.ToString(),
                    newAnimal.TotalPotentialNames.ToString()
                }));
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            bool flag = false;
            for (int i = 0; i < materialListView1.Columns.Count; i++)
                if (materialListView1.Columns[i].Width != widths[i])
                {
                    materialListView1.Columns[i].Width = widths[i];
                    if (!flag)
                        flag = true;
                }
            if (flag)
                materialLabel3.Visible = true;
            else if (materialLabel3.Visible)
                materialLabel3.Visible = false;
        }
        public void UpdateAnimalText(bool smooth)
        {
            if (smooth)
            {
                loading = 0;
                materialProgressBar1.Value = 0;
                materialProgressBar1.Maximum = LOAD_TIME;
            }
            else
            {
                materialLabel1.Text =
                    "Animal: " + animals[selectedAnimalIndex].PrimaryName + "  •  " +
                    "Total names: " + animals[selectedAnimalIndex].TotalNames.ToString("N0") + "  •  " +
                    "Potential: " + animals[selectedAnimalIndex].TotalPotentialNames.ToString("N0") + "  •  " +
                    "Names left to say: " + animals[selectedAnimalIndex].NamesLeftToSay.ToString("N0");
                materialProgressBar1.Value = materialProgressBar1.Maximum;
            }
        }
        private void materialFlatButton1_Click(object sender, EventArgs e)
        {
            if (selectedAnimalIndex == -1)
            {
                MessageBox.Show("*Aphro* VVN - No.");
                return;
            }
            say = materialCheckBox1.Checked ? animals[selectedAnimalIndex].SayFromFullList() : animals[selectedAnimalIndex].Say();
            materialFlatButton1.Text = say.Length == 0 ? "זהו נגמר" : say;
            UpdateAnimalText(false);
        }
        private void materialListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (materialListView1.SelectedItems.Count == 0)
                return;
            selectedAnimalIndex = materialListView1.SelectedItems[0].Index;
            textBox1.Enabled = true;
            label1.Enabled = true;
            UpdateAnimalText(true);
            materialFlatButton1.Text = "";
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (loading != -1)
            {
                if (++loading == LOAD_TIME)
                {
                    UpdateAnimalText(false);
                    loading = -1;
                }
                else
                {
                    materialProgressBar1.Value = loading;
                    materialProgressBar1.Refresh();
                    materialLabel1.Text =
                        "Animal: " + animals[selectedAnimalIndex].PrimaryName + "  •  " +
                        "Total names: " + (animals[selectedAnimalIndex].TotalNames / LOAD_TIME * loading).ToString("N0") + "  •  " +
                        "Potential: " + (animals[selectedAnimalIndex].TotalPotentialNames / LOAD_TIME * loading).ToString("N0") + "  •  " +
                        "Names left to say: " + (animals[selectedAnimalIndex].NamesLeftToSay / LOAD_TIME * loading).ToString("N0");
                }
            }
        }
        private void materialCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (selectedAnimalIndex == -1)
            {
                MessageBox.Show("*Aphro* VVN - No.");
                return;
            }
            animals[selectedAnimalIndex].UpdatedNamesToSay(materialCheckBox1.Checked);
            UpdateAnimalText(false);
        }
        private void materialFlatButton4_Click(object sender, EventArgs e)
        {
            if (selectedAnimalIndex == -1)
            {
                MessageBox.Show("*Aphro* VVN - No.");
                return;
            }
            animals[selectedAnimalIndex].ResetNamesToSay(materialCheckBox1.Checked);
            UpdateAnimalText(false);
        }
        private void materialFlatButton2_Click(object sender, EventArgs e)
        {
            if (selectedAnimalIndex == -1)
            {
                MessageBox.Show("*Aphro* VVN - No.");
                return;
            }
            string all = string.Empty;
            foreach (var i in animals[selectedAnimalIndex].Names)
                all += (all.Length == 0 ? "" : Environment.NewLine) + i;
            Clipboard.SetText(all);
            MessageBox.Show("Check your clipboard text!");
        }
        private void materialFlatButton3_Click(object sender, EventArgs e)
        {
            if (selectedAnimalIndex == -1)
            {
                MessageBox.Show("*Aphro* VVN - No.");
                return;
            }
            string all = string.Empty;
            foreach (var i in animals[selectedAnimalIndex].Names)
            {
                all += (all.Length == 0 ? "" : Environment.NewLine) + " + " + i;
                if (animals[selectedAnimalIndex].PotentialNames.ContainsKey(i))
                {
                    all += " (" + animals[selectedAnimalIndex].PotentialNames[i].Count + "):";
                    foreach (var j in animals[selectedAnimalIndex].PotentialNames[i])
                        all += Environment.NewLine + " - - " + j;
                }
            }
            Clipboard.SetText(all);
            MessageBox.Show("Check your clipboard text!");
        }
        private void materialFlatButton5_Click(object sender, EventArgs e)
        {
            if (webBrowser1.Url != null && webBrowser1.Url.AbsoluteUri.Length > 0)
                Process.Start(webBrowser1.Url.AbsoluteUri);
        }
        private void materialTabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (say.Length == 0 || say == "זהו נגמר" || e.TabPageIndex != 2)
                return;
            webBrowser1.Url = new Uri("http://www.google.com/search?q=" + say);
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (selectedAnimalIndex != -1)
            {
                var list = animals[selectedAnimalIndex].Names.FindAll(x => x.Contains(textBox1.Text));
                if (list.Count > 0)
                    label1.Text = string.Join(", ", list);
                else
                    label1.ResetText();
            }
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            if (textBox1.Enabled && textBox1.Text == "Search...")
                textBox1.SelectAll();
        }
    }
    public class Animal
    {
        public string PrimaryName = string.Empty;
        public List<string> Names = new List<string>();
        public List<string> FullList = new List<string>();
        public Dictionary<string, List<string>> PotentialNames = new Dictionary<string, List<string>>();
        private Dictionary<string, List<string>> PotentialNameParts = new Dictionary<string, List<string>>();
        public int TotalNames = 0;
        public int TotalPotentialNames = 0;
        public int NamesLeftToSay = 0;
        private Random RandomVar = new Random();
        private List<string> NamesToSay = new List<string>();
        private List<string> FullListToSay = new List<string>();
        public Animal(string file)
        {
            PrimaryName = file.Replace(Form1.path + "\\", "").Replace(".txt", "");
            string[] lines = File.ReadAllLines(file, Encoding.Default);
            int tmp = 0;
            string tmps = string.Empty;
            foreach (string line in lines)
            {
                Names.Add(line);
                TotalNames++;
                TotalPotentialNames++;
                if ((tmp = line.Count(c => c == '(')) > 0)
                {
                    TotalPotentialNames += (int)Math.Pow(tmp, tmp);
                    PotentialNames.Add(line, new List<string>());
                    PotentialNameParts.Add(line, line.Split(new char[] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries).ToList());
                    PotentialNames[line].Add(PotentialNameParts[line][0]);
                    for (int i = 1; i < PotentialNameParts[line].Count; i++)
                    {
                        tmps = PotentialNameParts[line][0] + PotentialNameParts[line][i];
                        if (!PotentialNames[line].Contains(tmps))
                            PotentialNames[line].Add(tmps);
                        for (int j = i + 1; j < PotentialNameParts[line].Count; j++)
                        {
                            tmps = tmps + PotentialNameParts[line][j];
                            if (!PotentialNames[line].Contains(tmps))
                                PotentialNames[line].Add(tmps);
                        }
                    }
                    FullList.AddRange(PotentialNames[line]);
                }
                else
                    FullList.Add(line);
            }
            NamesLeftToSay = TotalNames;
            NamesToSay = new List<string>(Names);
            FullListToSay = new List<string>(FullList);
        }
        public string Say()
        {
            if (NamesToSay.Count == 0)
                return string.Empty;
            int say = RandomVar.Next(NamesToSay.Count);
            string result = NamesToSay[say];
            NamesToSay.RemoveAt(say);
            NamesLeftToSay = NamesToSay.Count;
            return result;
        }
        public string SayFromFullList()
        {
            if (FullListToSay.Count == 0)
                return string.Empty;
            int say = RandomVar.Next(FullListToSay.Count);
            string result = FullListToSay[say];
            FullListToSay.RemoveAt(say);
            NamesLeftToSay = FullListToSay.Count;
            return result;
        }
        public int UpdatedNamesToSay(bool full)
        {
            if (full)
                return NamesLeftToSay = FullListToSay.Count;
            else
                return NamesLeftToSay = NamesToSay.Count;
        }
        public void ResetNamesToSay(bool full)
        {
            NamesToSay = new List<string>(Names);
            FullListToSay = new List<string>(FullList);
            UpdatedNamesToSay(full);
        }
    }
}