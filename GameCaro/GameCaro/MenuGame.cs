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

namespace GameCaro
{
    public partial class MenuGame : Form
    {
        Graphics g;
        public MenuGame()
        {
            InitializeComponent();
            g = this.CreateGraphics();  
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PlayerList user = new PlayerList(1);
            user.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PlayerList user = new PlayerList(2);
            user.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DetailSetting dt = new DetailSetting();
            dt.Show();
            this.Hide();
        }

        private void MenuGame_FormClosing(object sender, FormClosingEventArgs e)
        {
           //if(MessageBox.Show("Bạn có muốn thoát game không?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
           //{ e.Cancel = true; }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            /* Mục tiêu : Đọc file bàn cờ
             * Nội dung: Đọc 1 mảng các ô bàn cờ, do lưu bàn cờ theo cơ chế stack nên phải duyệt ngược để push vào stack lại.
             * */
            BanCo banco = new BanCo();
            CaroChess cr = new CaroChess();
            bool check = false;
            StreamReader FR;
            int n;
            int cheDo = 1;
            OCo[] mangOCo = null;
            String pl1 = "";
            String pl2 = "";
            OpenFileDialog Open = new OpenFileDialog();
            Open.Filter = "Caro file (*.cro)|*.cro|All file (*.*)|*.*";
            Open.ShowDialog();

            try
            {
                if (Open.FileName.CompareTo("") != 0)
                {

                    FR = new StreamReader(Open.FileName);
                    n = Convert.ToInt32(FR.ReadLine());
                    cheDo = Convert.ToInt32(FR.ReadLine());
                    pl1 = Convert.ToString(FR.ReadLine());
                    pl2 = Convert.ToString(FR.ReadLine());
                    mangOCo = new OCo[n];
                    for (int i = 0; i < n; i++)
                    {
                        int d = Convert.ToInt32(FR.ReadLine());
                        int c = Convert.ToInt32(FR.ReadLine());
                        int sh = Convert.ToInt32(FR.ReadLine());
                        mangOCo[i] = new OCo(d, c, new Point((c * OCo._ChieuRong + 2), (d * OCo._ChieuCao + 2)), sh);
                    }
                    check = true;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi");
                throw;
            }
            // neu doc thanh cong
            if(check)
            {
                Mode.load_game = true;
                Caro aaa = new Caro(pl1, pl2, cheDo, mangOCo);
                aaa.Show();
                this.Hide();
            }
           
        }

        private void buttonLan_Click(object sender, EventArgs e)
        {
            Caro frm = new Caro("Player1", "Player2", 3);
            frm.Show();
            this.Hide();
        }

        private void MenuGame_Load(object sender, EventArgs e)
        {

        }
    }
}
