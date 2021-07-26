using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameCaro
{
    public partial class DetailSetting : Form
    {
        private int value;
        private const int APPCOMMAND_VOLUME_MUTE = 0x80000;
        private const int APPCOMMAND_VOLUME_UP = 0xA0000;
        private const int APPCOMMAND_VOLUME_DOWN = 0x90000;
        private const int WM_APPCOMMAND = 0x319;

        
        public DetailSetting()
        {
            InitializeComponent();
            value = trackBar1.Value;
        }
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg,
         IntPtr wParam, IntPtr lParam);
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (trackBar1.Value > value)
            {
                VolUp();
            }
            else if (trackBar1.Value < value)
            {
                VolDown();
            }
            value = trackBar1.Value;
        }

        private void DetailSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            MenuGame frm = new MenuGame();
            frm.Show();
        }

        private void trackBar1_MouseLeave(object sender, EventArgs e)
        {
            
        }
       
     
        
        private void Mute()
        {
            SendMessageW(this.Handle, WM_APPCOMMAND, this.Handle,
                (IntPtr)APPCOMMAND_VOLUME_MUTE);
        }

        private void VolDown()
        {
            SendMessageW(this.Handle, WM_APPCOMMAND, this.Handle,
                (IntPtr)APPCOMMAND_VOLUME_DOWN);
        }

        private void VolUp()
        {
            SendMessageW(this.Handle, WM_APPCOMMAND, this.Handle,
                (IntPtr)APPCOMMAND_VOLUME_UP);
        }

        private void btnMute_Click(object sender, EventArgs e)
        {
            Mute();
        }
       
    }
}
