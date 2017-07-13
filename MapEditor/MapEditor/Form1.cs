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

namespace MapEditor
{
    public partial class Form1 : Form
    {
        private Map_State select_state;
        private Map_State[,] items = new Map_State[10, 7];
        private int BlockSize = 50;
        private enum Map_State
        {
            None=-1,
            Passageway = 0,
            Red =1,
            Red1=2,
            Red2=3,
            Silver=4,
            Blue=5,
            ColorFul=6,
            Purple=7,
            Wood=8         
        };

        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void Main_Load(object sender, EventArgs e)
        {
            resetMap();
        }

        private void resetMap()
        {
            select_state = Map_State.None;
            for (int i = 0; i < 10; ++i)
            {
                for (int j = 0; j < 7; ++j)
                {
                    items[i, j] = Map_State.Passageway;
                }
            }
            pictureBox1.Width = 10 * BlockSize + 2;
            pictureBox1.Height = 10 * BlockSize + 2;
            drawImg();
        }

        private void drawImg()
        {
            
            Bitmap bit = new Bitmap(this.pictureBox1.Width, this.pictureBox1.Height);
            Graphics g = Graphics.FromImage(bit);
            Image img; 
            for(int i=0; i<10; ++i)
            {
                for(int j=0; j<7; ++j)
                {
                    if (items[i, j] == Map_State.Red)
                    {
                        img = new Bitmap(System.Windows.Forms.Application.StartupPath + "\\red.jpg");
                        g.DrawImage(img, i * 50, j * 50, 50, 50);
                    }
                    else if (items[i, j] == Map_State.Red1)
                    {
                        img = new Bitmap(System.Windows.Forms.Application.StartupPath + "\\red1.jpg");
                        g.DrawImage(img, i * 50, j * 50, 50, 50);
                    }
                    else if (items[i, j] == Map_State.Red2)
                    {
                        img = new Bitmap(System.Windows.Forms.Application.StartupPath + "\\red2.jpg");
                        g.DrawImage(img, i * 50, j * 50, 50, 50);
                    }
                    else if (items[i, j] == Map_State.ColorFul)
                    {
                        img = new Bitmap(System.Windows.Forms.Application.StartupPath + "\\colorful.jpg");
                        g.DrawImage(img, i * 50, j * 50, 50, 50);
                    }
                    else if (items[i, j] == Map_State.Blue)
                    {
                        img = new Bitmap(System.Windows.Forms.Application.StartupPath + "\\blue.jpg");
                        g.DrawImage(img, i * 50, j * 50, 50, 50);
                    }
                    else if (items[i, j] == Map_State.Silver)
                    {
                        img = new Bitmap(System.Windows.Forms.Application.StartupPath + "\\silver.jpg");
                        g.DrawImage(img, i * 50, j * 50, 50, 50);
                    }
                    else if (items[i, j] == Map_State.Wood)
                    {
                        img = new Bitmap(System.Windows.Forms.Application.StartupPath + "\\wood.jpg");
                        g.DrawImage(img, i * 50, j * 50, 50, 50);
                    }
                    else if (items[i, j] == Map_State.Purple)
                    {
                        img = new Bitmap(System.Windows.Forms.Application.StartupPath + "\\purple.jpg");
                        g.DrawImage(img, i * 50, j * 50, 50, 50);
                    }
                    else if (items[i, j] == Map_State.Passageway)
                    {
                        img = new Bitmap(System.Windows.Forms.Application.StartupPath + "\\passageway.gif");
                        g.DrawImage(img, i * 50, j * 50, 50, 50);
                    }
                }
            }

            this.pictureBox1.Image = bit;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            select_state = Map_State.Passageway;
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            select_state = Map_State.ColorFul;
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            select_state = Map_State.Blue;
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            select_state = Map_State.Purple;
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            select_state = Map_State.Red;
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            select_state = Map_State.Red1;
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            select_state = Map_State.Red2;
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            select_state = Map_State.Silver;
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            select_state = Map_State.Wood;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            int x, y;
            x = e.X / 50;
            y = e.Y / 50;

            if(x>=0 && x<10 && y>=0 && y<7)
            {
                items[x, y] = select_state;//修改地图
                drawImg();
            }

        }

        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            resetMap();
        }

        private void toolStripButton13_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "json文件|*.json";
            saveFileDialog1.Title = "save";
            saveFileDialog1.FileName = string.Empty;
            saveFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            int[,] arr = new int[10, 7];

            for(int i=0; i<10; ++i)
            {
                for(int j=0; j<7; ++j)
                {
                    arr[i, j] = (int)items[i, j];
                }
            }

            string str = LitJson.JsonMapper.ToJson(arr);
            File.WriteAllText(saveFileDialog1.FileName, str);
        }
    }
}
