using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCaro
{
    class BanCo
    {
        private int _SoDong;
        private int _SoCot;

        public int SoDong
        {
            get { return _SoDong; }

        }
        public int SoCot
        {
            get { return _SoCot; }

        }
        public BanCo()
        {
            _SoDong = 0;
            _SoCot = 0;
        }
        public BanCo(int soDong, int soCot)
        {
            _SoDong = soDong;
            _SoCot = soCot;
        }
        //Ve Ban Co
        public void VeBanCo(Graphics g)
        {
            for (int i = 0; i <= _SoCot; i++)
            {
                g.DrawLine(CaroChess.pen, i * OCo._ChieuRong, 0, i * OCo._ChieuRong, _SoDong * OCo._ChieuCao);
            }
            for (int j = 0; j <= _SoDong; j++)
            {
                g.DrawLine(CaroChess.pen, 0, j * OCo._ChieuCao, _SoCot * OCo._ChieuRong, j * OCo._ChieuCao);
            }
        }
        public void VeQuanCo(Graphics g, Point point, Image img)
        {
            //g.FillEllipse(sb, point.X + 1, point.Y + 1, OCo._ChieuRong - 2, OCo._ChieuCao - 2);
            g.DrawImage(img, point.X + 1, point.Y + 1, OCo._ChieuRong - 2, OCo._ChieuCao - 2);
        }
        public void XoaQuanCo(Graphics g, Point point, SolidBrush sb)
        {
            // vẽ đè len quan co cho Redo
            g.FillRectangle(sb, point.X + 1, point.Y + 1, OCo._ChieuRong - 2, OCo._ChieuCao - 2);
        }
    }
}
