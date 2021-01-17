using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YilanOyunu
{
    public partial class Form1 : Form
    {

        LinkedList<Label> yilan = new LinkedList<Label>();
        List<string> duvarlar_1 = new List<string>();
        List<string> duvarlar_2 = new List<string>();
        int yon = 0, gecenSure = 0;
        bool sag = false, sol = false;
        public double puan = 0.0;
        string istikamet = "sag", yem = null, sonParca = null, deger = null;
        Random random = new Random();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Baslangic();
            DuvarTanimla();
            sagYon();
        }

        private void DuvarTanimla()
        {
            for (int i = 0; i <= 380; i+=20)
            {    
                duvarlar_1.Add(i.ToString());
            }
            for (int i = 19; i <= 399; i += 20)
            {
                duvarlar_2.Add(i.ToString());
            }
        }

        private void Yem()
        {
        x:
            yem = random.Next(399).ToString();
            foreach (Label item in flowLayoutPanel1.Controls)
            {
                if (item.Name == yem && item.BackColor == Color.Green)
                {
                    goto x;
                }
            }
            foreach (Label item in flowLayoutPanel1.Controls)
            {
                if (item.Name == yem)
                {
                    item.BackColor = Color.Red;
                }
            }
        }

        private void Baslangic()
        {
            gecenSureTimer.Interval = 1000;

            for (int i = 0; i < 400; i++)
            {
                Label label = new Label();
                label.Height = 20;
                label.Width = 20;
                label.Name = i.ToString();
                //label.Text = i.ToString();
                label.BackColor = Color.White;
                label.Margin = new Padding(0, -1, -1, -1);

                flowLayoutPanel1.Controls.Add(label);
            }

            foreach (Label item in flowLayoutPanel1.Controls)
            {
                if (item.Name == "229")
                {
                    yilan.AddLast(item);
                    item.BackColor = Color.Green;
                }
                else if (item.Name == "230" || item.Name == "231")
                {
                    yilan.AddFirst(item);
                    item.BackColor = Color.Green;
                }
                if (item.Name == "229")
                {
                    sonParca = item.Name;
                }
            }
            Yem();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Up:
                    if (istikamet != "asagi") {
                        yukariYon();
                    }
                    break;
                case Keys.Down:
                    if (istikamet != "yukari") 
                    {
                        asagiYon();
                    }
                    break;
                case Keys.Left:
                    if (istikamet != "sag")
                    {
                        solYon();
                    }
                    break;
                case Keys.Right:
                    if(istikamet != "sol"){
                        sagYon();
                    }
                    break;
            }
        }


        private void sagYon() {
            yon = 1;
            istikamet = "sag";
            YonVer();
        }
        private void solYon() {
            yon = -1;
            istikamet = "sol";
            YonVer();
        }
        private void yukariYon() {
            yon = -20;
            istikamet = "yukari";
            YonVer();
        }
        private void asagiYon()
        {
            yon = 20;
            istikamet = "asagi";
            YonVer();
        }

        private void yilanTimer_Tick(object sender, EventArgs e)
        {
            YonVer();
        }

       
        private void YonVer()
        {
            bool durum = false;
            LinkedListNode<Label> dugum = yilan.First;

            deger = (Convert.ToInt32(dugum.Value.Name) + (yon)).ToString();

            if (DuvarKontrol(int.Parse(deger)))
            {
                foreach (Label item in flowLayoutPanel1.Controls)
                {
                    if (item.Name == deger)
                    {

                        yilan.AddFirst(item);
                        if (item.BackColor == Color.Red)
                        {
                            puan += 50;
                            puanLbl.Text = puan.ToString();
                            foreach (Label item_ in flowLayoutPanel1.Controls)
                            {
                                if (item_.Name == sonParca)
                                {
                                    yilan.AddLast(item_);
                                    item_.BackColor = Color.Green;
                                    durum = true;
                                    yilanTimer.Interval -= 1;
                                    intervalLabel.Text = yilanTimer.Interval.ToString();

                                    if(yilanTimer.Interval == 0)
                                    {
                                        OyunBitti();
                                    }

                                    Yem();
                                }
                            }
                        }
                        item.BackColor = Color.Green;

                    }
                }

                if (durum == false)
                {
                    dugum = yilan.Last;

                    foreach (Label item in flowLayoutPanel1.Controls)
                    {
                        if (item.Name == dugum.Value.Name.ToString())
                        {
                            item.BackColor = Color.White;
                            sonParca = item.Name;
                            yilan.RemoveLast();
                        }
                    }
                }
            }
            else
            {
                OyunBitti();
            }

        }


        private void OyunBitti()
        {
            yilanTimer.Enabled = false;
            gecenSureTimer.Enabled = false;
            DialogResult result = MessageBox.Show("Oyun Bitti Puanınız :" + puanLbl.Text.ToString() + " Yeniden Başlamak istiyormusunuz ?", "Bilgi", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (result == DialogResult.Yes)
            {
                Application.Restart();
            }
            else if (result == DialogResult.No)
            {
                Environment.Exit(0);
            }
        }

        private void gecenSureTimer_Tick(object sender, EventArgs e)
        {
            gecenSure++;
            gecenSureLbl.Text = gecenSure.ToString()+" s";
        }
        
        private bool DuvarKontrol(int konum)
        {
            bool durum=true;
            foreach (Label item in flowLayoutPanel1.Controls)
            {
                if(item.Name == konum.ToString())
                {
                    if(item.BackColor == Color.Green)
                    {
                        durum = false;
                    }
                }
            }
            if(konum<0 || konum >399)
            {
                durum = false;
            }

            


            if (istikamet == "sol" && sol)
            {
                durum = false;
            }
            else if (istikamet == "sag" && sag)
            {
                durum = false;
            }


            if (istikamet == "sol")
            {
                sag = false;
                if (duvarlar_1.Contains((Convert.ToInt32(konum)).ToString()))
                {
                    sol = true;
                    
                }
            }
            
            else if(istikamet == "sag")
            {
                sol = false;
                if (duvarlar_2.Contains((Convert.ToInt32(konum)).ToString()))
                {
                    sag = true;
                    
                }
            }
            
            return durum;
        }

    }
}
