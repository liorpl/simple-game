using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SimpleGame.Values;

namespace SimpleGame
{
    public delegate void RectAction(ref Rectangle r);    

    class DBPanel : Panel
    {
        public DBPanel()
        {
            DoubleBuffered = true;
        }        
    }

    public class PointClass
    {
        public int X;
        public int Y;
        public int Direction; //Used in moving points
        public PointClass(int i1,int i2) { X = i1; Y = i2; }

        public PointClass Copy() { return (PointClass)MemberwiseClone(); }

        public static implicit operator Point(PointClass p) => new Point(p.X, p.Y);
    }    

    public partial class Form1 : Form
    {
        //readonly Font dispfont = new Font("Arial", 22);
        readonly Font dispfont = new Font("Palatino Linotype", 22);

        //Player p = new Player();
        public static Rectangle BoundsRect;
        

        //static readonly Rectangle nullrect = Rectangle.Empty;        

        ulong ticknum = 0;
        
        int speed = 10;
        int bulletradius = 16;
        
        int collectedcoins = 0;

        Point pointdown = Point.Empty;
        Point tempp2 = Point.Empty;        

        public Form1()
        {
            InitializeComponent();
            BoundsRect = Bounds;
            blocks.Add(new Rectangle(0, 200, 300, 20));
            blocks.Add(new Rectangle(100, 400, 300, 20));            
            blocks.Add(new Block(new Rectangle(180, 395, 20, 5), true));
            blocks.Add(new Block(new Rectangle(350, 395, 20, 5), true));
            blocks.Add(new Block(new Rectangle(550, 90, 40, 80), true));
            coins.Add(new PointClass(200, 180));
            coins.Add(new PointClass(400, 390));
            coins.Add(new PointClass(450, 340));
            enemies.Add(new Enemy(new Point(310, 220)));            
        }

        private void load(object sender, EventArgs e)
        {
            timer = new Timer();
            timer.Interval = 1;
            timer.Tick += Step;
            timer.Enabled = true;                      
        }

        private void paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            foreach (var p in bullets)
                g.FillEllipse(Bullet.bulletcolors[p.BulletType], p.Pos.X - bulletradius, p.Pos.Y - bulletradius, bulletradius, bulletradius);
            for (int i = 0; i < coins.Count; i++)
                if (!collected[i]) g.FillEllipse(Brushes.Gold, coins[i].X - bulletradius, coins[i].Y - bulletradius, bulletradius, bulletradius);
            foreach (var en in enemies) en.Draw(g);
            //for (int i = 0; i < blocks.Count; i++)
            //    if (fragile[i]) g.FillRectangle(Brushes.Brown, blocks[i]); else g.FillRectangle(Brushes.SlateGray, blocks[i]);
            foreach (var b in blocks) g.FillRectangle(b.Fragile ? Brushes.SlateGray : Brushes.Brown, b.Rect);
            player.Draw(g);
            g.DrawString("COINS: " + collectedcoins.ToString(), dispfont, Brushes.Green, 550, 0);
            if (gameover) g.DrawString("Gameover", dispfont, Brushes.Crimson, 300, 100);
            g.DrawString(player.OnGround.ToString(), dispfont, Brushes.Black, 550, 20);
        }

        private void keypress(object sender, KeyEventArgs e)
        {
            byte[] keys = new byte[256];
            GetKeyboardState(keys);
            if ((keys[(int)Keys.Right] & 128) == 128)
            {
                if (!player.crouch) player.TryMove((ref Rectangle r) => r.X += speed);
                player.flip = 0;
            }
            if ((keys[(int)Keys.Left] & 128) == 128)
            {
                if(!player.crouch) player.TryMove((ref Rectangle r) => r.X -= speed);
                player.flip = 1;
            }
            if ((keys[(int)Keys.Up] & 128) == 128)
            {
                //player.TryMove((ref Rectangle r) => player.Jump());
                player.Jump();
            }
            if ((keys[(int)Keys.Down] & 128) == 128)
            {
                player.crouch = true;
            }
            if ((keys[(int)Keys.Space] & 128) == 128)
            {
                player.CreateBullet();
            }
            if((keys[(int)Keys.Z]& 128) == 128)
            {
                player.HandleGroundPound();
            }
            for(int i=0;i<9;i++)
            {
                if((keys[i+49] & 128) == 128)
                {
                    bulletstate = i;
                }
            }
            if (e.Shift) shift = true;
        }


        private void keyrelease(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
                player.crouch = false;
            if (!e.Shift) shift = false;
        }

        private void mousedown(object sender, MouseEventArgs e)
        {            
            pointdown = e.Location;
        }

        private void mouseup(object sender, MouseEventArgs e)
        {
            tempp2 = e.Location;
            Rectangle mouserect = pointdown.MakeRect(e.Location);
            if (mouserect.Width < 20 || mouserect.Height < 20) return;

            blocks.Add(new Block(mouserect, shift));
        }

        private void Step(object s, EventArgs e)
        {
            player.Step(ticknum);                 
            //if (p.previousflip != p.flip)
            //    p.Flip();           
            //for(int i=0;i<bullets.Count;i++)
            foreach(var bu in bullets)
            {
                bu.Step();
            }
            //foreach (var en in enemies) en.Step(ticknum);
            for (int i = 0; i < enemies.Count; i++) enemies[i].Step(ticknum);
            for (int i = 0; i < coins.Count; i++)
            {
                if (player.actorrect.Contains(coins[i]) && !collected[i])
                {
                    collected[i] = true;
                    collectedcoins++;
                }
            }

            int c = bullets.Count;
            foreach (var bu in bullets) if (!Form1.BoundsRect.Contains(bu.Pos)) bu.IsMarkedKilled = true;

            foreach (var dl in DeleteLists) dl.ClearList();
            
            
            
            panel1.Invalidate();
            ticknum++;                        
        }

        
    }
}
