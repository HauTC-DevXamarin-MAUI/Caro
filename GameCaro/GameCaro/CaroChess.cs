using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameCaro
{
    public enum KETTHUC
    {
        HoaCo,
        Player1,
        Player2,
        COM
    }
    class CaroChess
    {
        public SocketManager socket = new SocketManager();
        public static Pen pen;
        public static Image imgX;
        public static Image imgO;
        public static SolidBrush sbXoaQuanCo;

        private OCo[,] _MangOCo;
        private BanCo _BanCo;

        private Stack<OCo> stkCacNuocDaDi;

        public Stack<OCo> StkCacNuocDaDi
        {
            get { return stkCacNuocDaDi; }
            set { stkCacNuocDaDi = value; }
        }

        public int _LuotDi;
        private bool _SanSang;
        private List<Player> playerDS;
        public Stack<OCo> stkCacNuocUndo;
        private KETTHUC _ketThuc;
        private int _CheDoChoi;

        private ProgressBar proBar;
        private Timer timer;
        private PictureBox pictureBox;
        private TextBox txtName;

        public int CheDoChoi
        {
            get { return _CheDoChoi; }
            set { _CheDoChoi = value; }
        }
        public bool SanSang
        {
            get { return _SanSang; }
            set { _SanSang = value; }
        }

        public List<Player> PlayerDS
        {
            get { return playerDS; }
            set { playerDS = value; }
        }
        public ProgressBar ProBar
        {
            get { return proBar; }
            set { proBar = value; }
        }
        public Timer Timer
        {
            get { return timer; }
            set { timer = value; }
        }



        public PictureBox PictureBox
        {
            get { return pictureBox; }
            set { pictureBox = value; }
        }


        public TextBox TxtName
        {
            get { return txtName; }
            set { txtName = value; }
        }

        private Label diem;

        public Label Diem
        {
            get { return diem; }
            set { diem = value; }
        }
        public CaroChess()
        {

            pen = new Pen(Color.FromArgb(45, 45, 45));
            imgX = new Bitmap(GameCaro.Properties.Resources.x);
            imgO = new Bitmap(GameCaro.Properties.Resources.o);
            sbXoaQuanCo = new SolidBrush(Color.FromArgb(255, 255, 225));

            _BanCo = new BanCo(20, 20);
            _MangOCo = new OCo[_BanCo.SoDong, _BanCo.SoCot];

            stkCacNuocDaDi = new Stack<OCo>();

            stkCacNuocUndo = new Stack<OCo>();

            _LuotDi = 1;

            //playerDS = new List<Player> { new Player( "a", Image.FromFile(Application.StartupPath+ "\\O.png")),
            //    new Player( "b", Image.FromFile(Application.StartupPath+ "\\X.png"))
            //};


        }

        public CaroChess(String player1, String player2, ProgressBar proBar, Timer timer, PictureBox pictureBox, TextBox txtName, Label diem)
        {
            pen = new Pen(Color.FromArgb(0, 0, 0));
            imgX = new Bitmap(GameCaro.Properties.Resources.x);
            imgO = new Bitmap(GameCaro.Properties.Resources.o);
            sbXoaQuanCo = new SolidBrush(Color.FromArgb(255, 255, 225));
            if(Mode.cheDoChoi == 3)
            { 
                _SanSang = true;
            }
            _BanCo = new BanCo(20, 20);
            _MangOCo = new OCo[_BanCo.SoDong, _BanCo.SoCot];

            playerDS = new List<Player> { new Player( player1, Image.FromFile(Application.StartupPath+ "\\x.png")),
                new Player( player2, Image.FromFile(Application.StartupPath+ "\\o.png"))
            };

            stkCacNuocDaDi = new Stack<OCo>();
            stkCacNuocUndo = new Stack<OCo>();

            _LuotDi = 1;
            _CheDoChoi = Mode.cheDoChoi;


            this.proBar = proBar;
            this.timer = timer;
            this.pictureBox = pictureBox;
            this.txtName = txtName;
            this.diem = diem;
        }


        // doc file
        public CaroChess(String player1, String player2, ProgressBar proBar, Timer timer, PictureBox pictureBox, TextBox txtName, Label diem, OCo[] mang, Graphics g)
        {
            pen = new Pen(Color.FromArgb(0, 0, 0));
            imgX = new Bitmap(GameCaro.Properties.Resources.x);
            imgO = new Bitmap(GameCaro.Properties.Resources.o);
            sbXoaQuanCo = new SolidBrush(Color.FromArgb(255, 255, 225));
            _BanCo = new BanCo(20, 20);
            playerDS = new List<Player> { new Player( player1, Image.FromFile(Application.StartupPath+ "\\x.png")),
                new Player( player2, Image.FromFile(Application.StartupPath+ "\\o.png"))
            };

            _MangOCo = new OCo[20, 20];
            KhoiTaoMangOCo();

            
            stkCacNuocDaDi = new Stack<OCo>();
            stkCacNuocUndo = new Stack<OCo>();
            _CheDoChoi = Mode.cheDoChoi;
            this.proBar = proBar;
            this.timer = timer;
            this.pictureBox = pictureBox;
            this.txtName = txtName;
            this.diem = diem;
            int idx = mang.Length - 1;
            for (int i = idx; i >= 0; i--)
            {
                OCo oco = new OCo(mang[i].Dong, mang[i].Cot, mang[i].ViTri, mang[i].SoHuu);
                if (oco == null) continue;
                _MangOCo[mang[i].Dong, mang[i].Cot] = oco;
                if (oco.SoHuu != 0)
                {
                    MessageBox.Show(oco.Dong + "," + oco.Cot + "," + oco.SoHuu + "," + oco.Dong);
                    stkCacNuocDaDi.Push(oco);
                }
            }
            _LuotDi = 1;
            VeLaiQuanCo(g);
        }
        public void VeBanCo(Graphics g)
        {
            _BanCo.VeBanCo(g);
        }

        public void KhoiTaoMangOCo()
        {
            for (int i = 0; i < _BanCo.SoDong; i++)
                for (int j = 0; j < _BanCo.SoCot; j++)
                {
                    _MangOCo[i, j] = new OCo(i, j, new Point(j * OCo._ChieuRong, i * OCo._ChieuCao), 0);

                }
        }
        //void checkSL(OCo[] mang)
        //{
        //    int slX = 0;
        //    int slY = 0;
        //    int slRong = 0;
        //    for (int i = 0; i < mang.Length; i++)
        //    {
        //            OCo oco = new OCo(mang[i].Dong, mang[i].Cot, mang[i].ViTri, mang[i].SoHuu);
        //            if (oco.SoHuu == 1)
        //                slX++;
        //            else if (oco.SoHuu == 2)
        //                slY++;
        //            else
        //                slRong++;
        //    }
        //    MessageBox.Show("X:" + slX + ", Y:" + slY + ",Rong: " + slRong);
        //}
        public bool DanhCo(int MouseX, int MouseY, Graphics g)
        {
            try
            {
                if (MouseX % OCo._ChieuRong == 0 || MouseY % OCo._ChieuCao == 0)
                    return false;

                int Cot = MouseX / OCo._ChieuRong;
                int Dong = MouseY / OCo._ChieuCao;

                //Đánh ô đó rồi không được đánh lại
                if (_MangOCo[Dong, Cot].SoHuu != 0)
                    return false;
                //Đánh cờ
                switch (_LuotDi)
                {

                    case 1:
                        _MangOCo[Dong, Cot].SoHuu = 1;
                        _BanCo.VeQuanCo(g, _MangOCo[Dong, Cot].ViTri, imgX);
                        _LuotDi = 2;
                        pictureBox.Image = playerDS[1].Mark;
                        txtName.Text = playerDS[1].Name;
                        break;
                    case 2:
                        _MangOCo[Dong, Cot].SoHuu = 2;
                        _BanCo.VeQuanCo(g, _MangOCo[Dong, Cot].ViTri, imgO);
                        _LuotDi = 1;
                         pictureBox.Image = playerDS[0].Mark;
                        txtName.Text = playerDS[0].Name;
                        break;
                    default:
                        MessageBox.Show("Có Lỗi!");
                        break;
                }

                //// thay đổi tên và ảnh
                //pictureBox.Image = _MangOCo[Dong, Cot].SoHuu == 1 ? playerDS[1].Mark : playerDS[0].Mark;
                //txtName.Text = _MangOCo[Dong, Cot].SoHuu == 1 ? playerDS[1].Name : playerDS[0].Name;

                //Khi đánh cờ, rồi Udo xong đánh mới thì k Redo lại được các nước đã Undo
                stkCacNuocUndo = new Stack<OCo>();

                OCo oco = new OCo(_MangOCo[Dong, Cot].Dong, _MangOCo[Dong, Cot].Cot, _MangOCo[Dong, Cot].ViTri, _MangOCo[Dong, Cot].SoHuu);
                stkCacNuocDaDi.Push(oco);

              // danh co -> 
                if(Mode.cheDoChoi != 3)
                {
                    if (KTChienThang())
                    {
                        KetThucGame();
                        
                        return false;
                    }
                }
               

                timer.Start();
                proBar.Value = 0;
                Sound();
                return true;
            }

            catch
            {

            }
            return false;
        }
        public bool DanhCoLan(int MouseX, int MouseY, Graphics g)
        {
            try
            {
                if (MouseX % OCo._ChieuRong == 0 || MouseY % OCo._ChieuCao == 0)
                    return false;

                int Cot = MouseX / OCo._ChieuRong;
                int Dong = MouseY / OCo._ChieuCao;

                //Đánh ô đó rồi không được đánh lại
                if (_MangOCo[Dong, Cot].SoHuu != 0)
                    return false;
                //Đánh cờ
                switch (_LuotDi)
                {

                    case 1:
                        _MangOCo[Dong, Cot].SoHuu = 1;
                        _BanCo.VeQuanCo(g, _MangOCo[Dong, Cot].ViTri, imgX);
                        _LuotDi = 2;
                        pictureBox.Image = playerDS[1].Mark;
                        txtName.Text = playerDS[1].Name;
                        break;
                    case 2:
                        _MangOCo[Dong, Cot].SoHuu = 2;
                        _BanCo.VeQuanCo(g, _MangOCo[Dong, Cot].ViTri, imgO);
                        _LuotDi = 1;
                        pictureBox.Image = playerDS[0].Mark;
                        txtName.Text = playerDS[0].Name;
                        break;
                    default:
                        MessageBox.Show("Có Lỗi!");
                        break;
                }
                stkCacNuocUndo = new Stack<OCo>();

                OCo oco = new OCo(_MangOCo[Dong, Cot].Dong, _MangOCo[Dong, Cot].Cot, _MangOCo[Dong, Cot].ViTri, _MangOCo[Dong, Cot].SoHuu);
                stkCacNuocDaDi.Push(oco);

                timer.Start();
                proBar.Value = 0;
                Sound();
                return true;
            }

            catch
            {

            }
            return false;
        }

        public void VeLaiQuanCo(Graphics g)
        {
            foreach (OCo oco in stkCacNuocDaDi)
            {
                if (oco.SoHuu == 1)
                    _BanCo.VeQuanCo(g, oco.ViTri, imgX);
                else if (oco.SoHuu == 2)
                    _BanCo.VeQuanCo(g, oco.ViTri, imgO);
            }
        }

        public int kiemTraLuotDi()
        {
            // cong them 1 luot
           return stkCacNuocDaDi.Count % 2 == 0 ? 1 : 2;
        }

        #region PvsP
        public void StartPlayervsPlayer(Graphics g)
        {
            if(Mode.load_game)
            {
                proBar.Value = 0;
               // timer.Stop();
                _SanSang = true;
                stkCacNuocUndo = new Stack<OCo>();
                _LuotDi = kiemTraLuotDi();
                txtName.Text = _LuotDi == 1 ? playerDS[0].Name : playerDS[1].Name;
                pictureBox.Image = _LuotDi == 1 ? playerDS[0].Mark : playerDS[1].Mark;
            }
            else
            {
                txtName.Text = playerDS[0].Name;
                pictureBox.Image = playerDS[0].Mark;
                proBar.Value = 0;
                _SanSang = true;
                stkCacNuocUndo = new Stack<OCo>();
                stkCacNuocDaDi = new Stack<OCo>();
                _LuotDi = 1;
                KhoiTaoMangOCo();
                VeBanCo(g);
            }
           
        }
        #endregion

        #region PvPLan
        //public void Lan(Graphics g)
        //{
        //        txtName.Text = playerDS[0].Name;
        //        pictureBox.Image = playerDS[0].Mark;
        //        proBar.Value = 0;
        //        _SanSang = true;
        //        stkCacNuocUndo = new Stack<OCo>();
        //        stkCacNuocDaDi = new Stack<OCo>();
        //        _LuotDi = 1;
        //        KhoiTaoMangOCo();
        //        VeBanCo(g);
        //}
        #endregion



        public void StartPvsC(Graphics g)
        {
            txtName.Text = playerDS[0].Name;
            pictureBox.Image = playerDS[0].Mark;
            proBar.Value = 0;
            _SanSang = true;
            stkCacNuocUndo = new Stack<OCo>();
            stkCacNuocDaDi = new Stack<OCo>();
            _LuotDi = 1;
            KhoiTaoMangOCo();
            VeBanCo(g);
            KhoiDongCom(g);
        }



        #region Undo
        public void Undo(Graphics g)
        {
            if (stkCacNuocDaDi.Count != 0)
            {
                if ((_CheDoChoi == 2 && stkCacNuocDaDi.Count <= 1) || (_CheDoChoi == 3 && stkCacNuocDaDi.Count <= 1) || _SanSang == false)
                    return;
                OCo oco = stkCacNuocDaDi.Pop();
                stkCacNuocUndo.Push(new OCo(oco.Dong, oco.Cot, oco.ViTri, oco.SoHuu));
                _MangOCo[oco.Dong, oco.Cot].SoHuu = 0;
                _BanCo.XoaQuanCo(g, oco.ViTri, sbXoaQuanCo);
                if (_LuotDi == 1)
                {
                    _LuotDi = 2;
                    txtName.Text = playerDS[1].Name;
                    pictureBox.Image = playerDS[1].Mark;
                    proBar.Value = 0;
                    timer.Start();
                }
                else
                {
                    _LuotDi = 1;
                    txtName.Text = playerDS[0].Name;
                    pictureBox.Image = playerDS[0].Mark;
                    proBar.Value = 0;
                    timer.Start();
                }

            }
            //VeBanCo(g);
            //VeLaiQuanCo(g);

        }
        #endregion

        #region Redo
        public void Redo(Graphics g)
        {
            if (stkCacNuocUndo.Count != 0)
            {
                OCo oco = stkCacNuocUndo.Pop();
                stkCacNuocDaDi.Push(new OCo(oco.Dong, oco.Cot, oco.ViTri, oco.SoHuu));
                _MangOCo[oco.Dong, oco.Cot].SoHuu = oco.SoHuu;
                _BanCo.VeQuanCo(g, oco.ViTri, oco.SoHuu == 1 ? imgX : imgO);

                if (_LuotDi == 1)
                {
                    _LuotDi = 2;
                    txtName.Text = playerDS[1].Name;
                    pictureBox.Image = playerDS[1].Mark;
                    proBar.Value = 0;
                    timer.Start();
                }
                else
                {
                    _LuotDi = 1;
                    txtName.Text = playerDS[0].Name;
                    pictureBox.Image = playerDS[0].Mark;
                    proBar.Value = 0;
                    timer.Start();
                }
            }

        }
        #endregion


        // Save
        public bool LuuGame(Graphics g)
        {
            SaveFileDialog Save = new SaveFileDialog();

            Save.Filter = "Caro file (*.caro)|*.caro|All file (*.*)|*.*";
            Save.FilterIndex = 2;
            Save.RestoreDirectory = true;

            if (Save.ShowDialog() == DialogResult.OK)
            {
                StreamWriter myStream = new StreamWriter(Save.FileName);
                if (Save.FileName.CompareTo("") != 0)
                {
                    myStream.WriteLine(stkCacNuocDaDi.Count);
                    myStream.WriteLine(_CheDoChoi);
                    myStream.WriteLine(playerDS[0].Name);
                    myStream.WriteLine(playerDS[1].Name);

                    for (int i = stkCacNuocDaDi.Count; i > 0; i--)
                    {
                        OCo oco = stkCacNuocDaDi.Pop();
                        myStream.WriteLine(oco.Dong);
                        myStream.WriteLine(oco.Cot);
                        myStream.WriteLine(oco.SoHuu);
                    }
                    myStream.Close();
                }
            }
            MessageBox.Show("Đã lưu");
            return true;
        }



        public bool OpenGame(Graphics g)
        {
            VeBanCo(g);
            StreamReader FR;
            OpenFileDialog Open = new OpenFileDialog();
            Open.Filter = "Caro file (*.cro)|*.cro|All file (*.*)|*.*";
            Open.ShowDialog();
            if (Open.FileName.CompareTo("") != 0)
            {

                FR = new StreamReader(Open.FileName);
                int n = Convert.ToInt32(FR.ReadLine()); 
                int cd = Convert.ToInt32(FR.ReadLine());
                String pl1 = Convert.ToString(FR.ReadLine());
                String pl2 = Convert.ToString(FR.ReadLine());
                int d = Convert.ToInt32(FR.ReadLine());
                int c = Convert.ToInt32(FR.ReadLine());
                int sh = Convert.ToInt32(FR.ReadLine());
                for (int i = 1; i <= n ; i++)
                {
                    _MangOCo[d, c].SoHuu = sh; //Debug giùm Hau xem sh nay bao nhiu v Nhưtu, cach nay kh dc :)))
                    VeBanCo(g);
                    DanhCo(c,d,g);
                    if(sh==1)
                        _BanCo.VeQuanCo(g, _MangOCo[d, c].ViTri, imgX);
                    else
                        _BanCo.VeQuanCo(g, _MangOCo[d, c].ViTri, imgO);

                }
            }
            return true;
        }



        #region Duyệt Chiến Thắng
        public void KetThucGame()
        {
            String str = "";
            switch (_ketThuc)
            {
                case KETTHUC.HoaCo:
                    timer.Stop();
                    str = "Hòa cờ";
                    _SanSang = false;
                    break;
                case KETTHUC.Player1:
                    timer.Stop();
                    str = playerDS[0].Name + " đã giành chiến Thắng!!!";
                    _SanSang = false;
                    Mode.xWin++;
                    break;
                case KETTHUC.Player2:
                    timer.Stop();
                    str = playerDS[1].Name + " đã giành chiến Thắng!!!";
                    _SanSang = false;
                    Mode.yWin++;
                    break;
                case KETTHUC.COM:
                    timer.Stop();
                    str = "Computer chiến Thắng!!!";
                    _SanSang = false;
                    Mode.xWin++;
                    break;
                default:
                    break;
            }
            //if (_CheDoChoi == 3)
            //{

            //try
            //{
            //    socket.Send(new SocketData((int)SocketCommand.END_GAME, "", new Point()));
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show(e.Message);
            //}
            
            MessageBox.Show(str);
            diem.Text = Mode.xWin + ":" + Mode.yWin;
            Mode.load_game = Mode.load_game == true ? false : false;
        }
       
        public bool KTChienThang()
        {
            if (stkCacNuocDaDi.Count == _BanCo.SoCot * _BanCo.SoDong)
            {
                _ketThuc = KETTHUC.HoaCo;
                return true;
            }
            foreach (OCo oco in stkCacNuocDaDi)
            {
                if (DuyetDoc(oco.Dong, oco.Cot, oco.SoHuu) || DuyetNgang(oco.Dong, oco.Cot, oco.SoHuu) || DuyetCheoXuoi(oco.Dong, oco.Cot, oco.SoHuu) || DuyetCheoNguoc(oco.Dong, oco.Cot, oco.SoHuu))
                {
                    _ketThuc = oco.SoHuu == 1 ? KETTHUC.Player1 : KETTHUC.Player2;
                    return true;
                }
            }

            return false;
        }

        public string getNameWin(string playerLose)
        {
            return playerLose.Equals(playerDS[0].Name) ? playerDS[1].Name : playerDS[0].Name;
        }
        public int getXorO(string playerWin)
        {
            return playerWin.Equals(playerDS[0].Name) ? 1 : 2;
        }

        #region Duyệt Dọc
        private bool DuyetDoc(int currDong, int currCot, int currtSoHuu)
        {
            //Xét ô cờ ở dòng 16(chỉ có 4 chess nên k WIn) 
            if (currDong > _BanCo.SoDong - 5)
                return false;
            int Dem;
            for (Dem = 1; Dem < 5; Dem++)
            {
                if (_MangOCo[currDong + Dem, currCot].SoHuu != currtSoHuu)
                    return false;
            }
            if (currDong == 0 || currDong + Dem == _BanCo.SoDong)
                return true;
            if (_MangOCo[currDong - 1, currCot].SoHuu == 0 || _MangOCo[currDong + Dem, currCot].SoHuu == 0)
                return true;
            return false;
        }
        #endregion
        #region Duyệt Ngang
        private bool DuyetNgang(int currDong, int currCot, int currtSoHuu)
        {
            //Xét ô cờ ở Cột 16(chỉ có 4 chess nên k WIn)
            if (currCot > _BanCo.SoCot - 5)
                return false;
            int Dem;
            for (Dem = 1; Dem < 5; Dem++)
            {
                if (_MangOCo[currDong, currCot + Dem].SoHuu != currtSoHuu)
                    return false;
            }
            if (currCot == 0 || currCot + Dem == _BanCo.SoCot)
                return true;
            if (_MangOCo[currDong, currCot - 1].SoHuu == 0 || _MangOCo[currDong, currCot + Dem].SoHuu == 0)
                return true;
            return false;
        }
        #endregion
        #region Duyệt Chéo Xuôi
        private bool DuyetCheoXuoi(int currDong, int currCot, int currtSoHuu) //Xét từ trên trái đến dưới phải
        {

            if (currDong > _BanCo.SoDong - 5 || currCot > _BanCo.SoCot - 5)
                return false;
            int Dem;
            for (Dem = 1; Dem < 5; Dem++)
            {
                if (_MangOCo[currDong + Dem, currCot + Dem].SoHuu != currtSoHuu)
                    return false;
            }
            if (currDong == 0 || currDong + Dem == _BanCo.SoDong || currCot == 0 || currCot + Dem == _BanCo.SoCot)
                return true;
            if (_MangOCo[currDong - 1, currCot - 1].SoHuu == 0 || _MangOCo[currDong + Dem, currCot + Dem].SoHuu == 0)
                return true;
            return false;
        }
        #endregion
        #region Duyệt Chéo Ngược
        private bool DuyetCheoNguoc(int currDong, int currCot, int currtSoHuu) //Xét từ trên trái đến dưới phải
        {

            if (currDong < 4 || currCot > _BanCo.SoCot - 5)
                return false;
            int Dem;
            for (Dem = 1; Dem < 5; Dem++)
            {
                if (_MangOCo[currDong - Dem, currCot + Dem].SoHuu != currtSoHuu)
                    return false;
            }
            if (currDong == _BanCo.SoDong - 1 || currDong == 4 || currCot == 0 || currCot + Dem == _BanCo.SoCot)
                return true;
            if (_MangOCo[currDong + 1, currCot - 1].SoHuu == 0 || _MangOCo[currDong - Dem, currCot + Dem].SoHuu == 0)
                return true;
            return false;
        }
        #endregion
        #endregion

        #region AI
        //private long[] MangDiemTanCong = new long[6] { 0, 64, 4096, 262144, 16777216, 1073741824 };
        //private long[] MangDiemPhongThu = new long[6] { 0, 8, 512, 32768, 2097152, 134217728 };
        private long[] MangDiemTanCong = new long[7] { 0, 9, 54, 162, 1458, 13122, 118008 };
        private long[] MangDiemPhongThu = new long[7] { 0, 3, 27, 99, 729, 6561, 59049 };
        public void KhoiDongCom(Graphics g)
        {
            if (stkCacNuocDaDi.Count == 0)
            {
                DanhCo(_BanCo.SoCot / 2 * OCo._ChieuCao + 1, _BanCo.SoDong / 2 * OCo._ChieuRong + 1, g);
            }
            else
            {
                OCo oco = TimKiemNuocDi();
                DanhCo(oco.ViTri.X + 1, oco.ViTri.Y + 1, g);
            }
        }
        private OCo TimKiemNuocDi()
        {
            OCo oCoResult = new OCo();
            long DiemMax = 0;
            for (int i = 0; i < _BanCo.SoDong; i++)
            {
                for (int j = 0; j < _BanCo.SoCot; j++)
                {
                    if (_MangOCo[i, j].SoHuu == 0)
                    {
                        long DiemTanCong = DiemTC_DuyetDoc(i, j) + DiemTC_DuyetNgang(i, j) + DiemTC_DuyeCheoXuoi(i, j) + DiemTC_DuyetCheoNguoc(i, j);
                        long DiemPhongThu = DiemPT_DuyetDoc(i, j) + DiemPT_DuyetNgang(i, j) + DiemPT_DuyeCheoXuoi(i, j) + DiemPT_DuyetNguoc(i, j);
                        long DiemTam = DiemTanCong > DiemPhongThu ? DiemTanCong : DiemPhongThu;
                        if (DiemMax < DiemTam)
                        {
                            DiemMax = DiemTam;

                            oCoResult = new OCo(_MangOCo[i, j].Dong, _MangOCo[i, j].Cot, _MangOCo[i, j].ViTri, _MangOCo[i, j].SoHuu);
                        }
                    }
                }
            }
            return oCoResult;
        }
        #region Tấn Công
        private long DiemTC_DuyetDoc(int currDong, int currCot)
        {
            long DiemTong = 0;
            //long DiemTC = 0;
            int SoQuanTa = 0;
            int SoQuanDich = 0;
            for (int Dem = 1; Dem < 6 && currDong + Dem < _BanCo.SoDong; Dem++)
            {
                if (_MangOCo[currDong + Dem, currCot].SoHuu == 1)//tren xuong duoi
                {
                    SoQuanTa++;
                }
                else if (_MangOCo[currDong + Dem, currCot].SoHuu == 2)
                {
                    SoQuanDich++;
                    break;
                }
                else
                    break;
            }
            for (int Dem = 1; Dem < 6 && currDong - Dem >= 0; Dem++)
            {
                if (_MangOCo[currDong - Dem, currCot].SoHuu == 1)
                {
                    SoQuanTa++;

                }
                else if (_MangOCo[currDong - Dem, currCot].SoHuu == 2)
                {
                    SoQuanDich++;
                    break;
                }
                else
                    break;
            }
            if (SoQuanDich == 2)//bi chan 2 dau
                return 0;
            //DiemTong -= MangDiemPhongThu[SoQuanDich + 1] * 2;
            DiemTong -= MangDiemPhongThu[SoQuanDich + 1] * 2;
            DiemTong += MangDiemTanCong[SoQuanTa];
            return DiemTong;
        }
        private long DiemTC_DuyetNgang(int currDong, int currCot)
        {

            long DiemTong = 0;
            //long DiemTC = 0;
            int SoQuanTa = 0;
            int SoQuanDich = 0;
            for (int Dem = 1; Dem < 6 && currCot + Dem < _BanCo.SoCot; Dem++)
            {
                if (_MangOCo[currDong, currCot + Dem].SoHuu == 1)
                {
                    SoQuanTa++;

                }
                else if (_MangOCo[currDong, currCot + Dem].SoHuu == 2)
                {
                    SoQuanDich++;
                    break;
                }
                else
                    break;
            }
            for (int Dem = 1; Dem < 6 && currCot - Dem >= 0; Dem++)
            {
                if (_MangOCo[currDong, currCot - Dem].SoHuu == 1)
                {
                    SoQuanTa++;

                }
                else if (_MangOCo[currDong, currCot - Dem].SoHuu == 2)
                {
                    SoQuanDich++;
                    break;
                }
                else
                    break;
            }
            if (SoQuanDich == 2)
                return 0;
            //DiemTong -= MangDiemPhongThu[SoQuanDich + 1] * 2;
            DiemTong -= MangDiemPhongThu[SoQuanDich + 1] * 2;
            DiemTong += MangDiemTanCong[SoQuanTa];
            return DiemTong;
        }
        private long DiemTC_DuyeCheoXuoi(int currDong, int currCot)
        {
            long DiemTong = 0;
            //long DiemTC = 0;
            int SoQuanTa = 0;
            int SoQuanDich = 0;
            for (int Dem = 1; Dem < 6 && currCot + Dem < _BanCo.SoCot && currDong + Dem < _BanCo.SoDong; Dem++)
            {
                if (_MangOCo[currDong + Dem, currCot + Dem].SoHuu == 1)
                {
                    SoQuanTa++;

                }
                else if (_MangOCo[currDong + Dem, currCot + Dem].SoHuu == 2)
                {
                    SoQuanDich++;
                    break;
                }
                else
                    break;
            }
            for (int Dem = 1; Dem < 6 && currCot - Dem >= 0 && currDong - Dem >= 0; Dem++)
            {
                if (_MangOCo[currDong - Dem, currCot - Dem].SoHuu == 1)
                {
                    SoQuanTa++;

                }
                else if (_MangOCo[currDong - Dem, currCot - Dem].SoHuu == 2)
                {
                    SoQuanDich++;
                    break;
                }
                else
                    break;
            }
            if (SoQuanDich == 2)
                return 0;
            //DiemTong -= MangDiemPhongThu[SoQuanDich + 2] * 2;
            DiemTong -= MangDiemPhongThu[SoQuanDich + 1] * 2;
            DiemTong += MangDiemTanCong[SoQuanTa];
            return DiemTong;
        }
        private long DiemTC_DuyetCheoNguoc(int currDong, int currCot)
        {
            long DiemTong = 0;
            //long DiemTC = 0;
            int SoQuanTa = 0;
            int SoQuanDich = 0;
            for (int Dem = 1; Dem < 6 && currCot + Dem < _BanCo.SoCot && currDong - Dem >= 0; Dem++)//DONG GIAM COT TANG
            {
                if (_MangOCo[currDong - Dem, currCot + Dem].SoHuu == 1)
                {
                    SoQuanTa++;

                }
                else if (_MangOCo[currDong - Dem, currCot + Dem].SoHuu == 2)
                {
                    SoQuanDich++;
                    break;
                }
                else
                    break;
            }
            for (int Dem = 1; Dem < 6 && currCot - Dem >= 0 && currDong + Dem < _BanCo.SoDong; Dem++)
            {
                if (_MangOCo[currDong + Dem, currCot - Dem].SoHuu == 1)
                {
                    SoQuanTa++;

                }
                else if (_MangOCo[currDong + Dem, currCot - Dem].SoHuu == 2)
                {
                    SoQuanDich++;
                    break;
                }
                else
                    break;
            }
            if (SoQuanDich == 2)
                return 0;
            //DiemTong -= MangDiemPhongThu[SoQuanDich + 2] * 2;
            DiemTong -= MangDiemPhongThu[SoQuanDich + 1] * 2;
            DiemTong += MangDiemTanCong[SoQuanTa];
            return DiemTong;
        }
        #endregion
        #region Phòng Thủ
        private long DiemPT_DuyetDoc(int currDong, int currCot)
        {
            long DiemTong = 0;
            //long DiemTC = 0;
            int SoQuanTa = 0;
            int SoQuanDich = 0;
            for (int Dem = 1; Dem < 6 && currDong + Dem < _BanCo.SoDong; Dem++)
            {
                if (_MangOCo[currDong + Dem, currCot].SoHuu == 1)
                {
                    SoQuanTa++;
                    break;
                }
                else if (_MangOCo[currDong + Dem, currCot].SoHuu == 2)
                {
                    SoQuanDich++;
                }
                else
                    break;
            }
            for (int Dem = 1; Dem < 6 && currDong - Dem >= 0; Dem++)
            {
                if (_MangOCo[currDong - Dem, currCot].SoHuu == 1)
                {
                    SoQuanTa++;
                    break;

                }
                else if (_MangOCo[currDong - Dem, currCot].SoHuu == 2)
                {
                    SoQuanDich++;

                }
                else
                    break;
            }
            if (SoQuanTa == 2)
                return 0;
            DiemTong += MangDiemPhongThu[SoQuanDich];
            return DiemTong;
        }
        private long DiemPT_DuyetNgang(int currDong, int currCot)
        {
            long DiemTong = 0;
            //long DiemTC = 0;
            int SoQuanTa = 0;
            int SoQuanDich = 0;
            for (int Dem = 1; Dem < 6 && currCot + Dem < _BanCo.SoCot; Dem++)
            {
                if (_MangOCo[currDong, currCot + Dem].SoHuu == 1)
                {
                    SoQuanTa++;
                    break;
                }
                else if (_MangOCo[currDong, currCot + Dem].SoHuu == 2)
                {
                    SoQuanDich++;

                }
                else
                    break;
            }
            for (int Dem = 1; Dem < 6 && currCot - Dem >= 0; Dem++)
            {
                if (_MangOCo[currDong, currCot - Dem].SoHuu == 1)
                {
                    SoQuanTa++;
                    break;

                }
                else if (_MangOCo[currDong, currCot - Dem].SoHuu == 2)
                {
                    SoQuanDich++;

                }
                else
                    break;
            }
            if (SoQuanTa == 2)
                return 0;
            DiemTong += MangDiemPhongThu[SoQuanDich];
            return DiemTong;
        }
        private long DiemPT_DuyeCheoXuoi(int currDong, int currCot)
        {
            long DiemTong = 0;
            //long DiemTC = 0;
            int SoQuanTa = 0;
            int SoQuanDich = 0;
            for (int Dem = 1; Dem < 6 && currCot + Dem < _BanCo.SoCot && currDong + Dem < _BanCo.SoDong; Dem++)
            {
                if (_MangOCo[currDong + Dem, currCot + Dem].SoHuu == 1)
                {
                    SoQuanTa++;
                    break;
                }
                else if (_MangOCo[currDong + Dem, currCot + Dem].SoHuu == 2)
                {
                    SoQuanDich++;

                }
                else
                    break;
            }
            for (int Dem = 1; Dem < 6 && currCot - Dem >= 0 && currDong - Dem >= 0; Dem++)
            {
                if (_MangOCo[currDong - Dem, currCot - Dem].SoHuu == 1)
                {
                    SoQuanTa++;
                    break;
                }
                else if (_MangOCo[currDong - Dem, currCot - Dem].SoHuu == 2)
                {
                    SoQuanDich++;

                }
                else
                    break;
            }
            if (SoQuanTa == 2)
                return 0;

            DiemTong += MangDiemPhongThu[SoQuanDich];
            return DiemTong;
        }
        private long DiemPT_DuyetNguoc(int currDong, int currCot)
        {
            long DiemTong = 0;
            //long DiemTC = 0;
            int SoQuanTa = 0;
            int SoQuanDich = 0;
            for (int Dem = 1; Dem < 6 && currCot + Dem < _BanCo.SoCot && currDong - Dem >= 0; Dem++)
            {
                if (_MangOCo[currDong - Dem, currCot + Dem].SoHuu == 1)
                {
                    SoQuanTa++;
                    break;
                }
                else if (_MangOCo[currDong - Dem, currCot + Dem].SoHuu == 2)
                {
                    SoQuanDich++;

                }
                else
                    break;
            }
            for (int Dem = 1; Dem < 6 && currCot - Dem >= 0 && currDong + Dem < _BanCo.SoDong; Dem++)
            {
                if (_MangOCo[currDong + Dem, currCot - Dem].SoHuu == 1)
                {
                    SoQuanTa++;
                    break;
                }
                else if (_MangOCo[currDong + Dem, currCot - Dem].SoHuu == 2)
                {
                    SoQuanDich++;
                }
                else
                    break;
            }
            if (SoQuanTa == 2)
                return 0;

            DiemTong += MangDiemPhongThu[SoQuanDich];
            return DiemTong;
        }
        #endregion

        #endregion
        // Âm thanh
        private void Sound()
        {
            System.Media.SoundPlayer bloop = new System.Media.SoundPlayer(Application.StartupPath + "\\Sound\\bloop.wav");
            bloop.Play();
        }

    }
}