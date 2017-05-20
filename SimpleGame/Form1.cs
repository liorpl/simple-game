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

    class DBPanel : Panel
    {
        public DBPanel()
        {
            DoubleBuffered = true;
        }        
    }

    public partial class Form1 : Form
    {        
        readonly Font dispfont = new Font("Bauhaus 93", 22);

        public static Rectangle BoundsRect;               

        ulong ticknum = 0;
        
        int speed = 10;
        int bulletradius = 16;
        
        int collectedcoins = 0;

        Point pointdown = Point.Empty;

        //Bitmap cointex;
        public Form1()
        {
            InitializeComponent();
            BoundsRect = Bounds;
            blocks.Add(new Rectangle(0, 200, 300, 20));
            blocks.Add(new Rectangle(100, 400, 300, 20));            
            blocks.Add(new Block(new Rectangle(180, 395, 20, 5), true));
            blocks.Add(new Block(new Rectangle(350, 395, 20, 5), true));
            blocks.Add(new Block(new Rectangle(550, 90, 40, 80), true));
            coins.Add(new Point(200, 180));
            coins.Add(new Point(400, 390));
            coins.Add(new Point(450, 340));
            enemies.Add(new Enemy(new Point(310, 220)));            
        }

        private void load(object sender, EventArgs e)
        {
            //cointex = new Bitmap(@"Images\coin.png");
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
            foreach (var b in blocks) g.FillRectangle(b.Fragile ? Brushes.SlateGray : Brushes.Brown, b.Rect);            
            player.Draw(g);
            g.DrawString("COINS: " + collectedcoins.ToString(), dispfont, Brushes.Green, 550, 0);
            if (gameover) g.DrawString("Gameover", dispfont, Brushes.Crimson, 300, 100);
            //g.DrawString(player.OnGround.ToString(), dispfont, Brushes.Black, 550, 20);
        }

        private void keypress(object sender, KeyEventArgs e)
        {
            byte[] keys = new byte[256];
            GetKeyboardState(keys);
            if ((keys[(int)Keys.Right] & 128) == 128)
            {
                //if (!player.crouch) player.TryMove((ref Rectangle r) => r.X += speed);
                if (!player.crouch) player.TryMove(p => new Point(p.X + speed, p.Y));
                player.flip = 0;
            }
            if ((keys[(int)Keys.Left] & 128) == 128)
            {
                //if(!player.crouch) player.TryMove((ref Rectangle r) => r.X -= speed);
                if (!player.crouch) player.TryMove(p => new Point(p.X - speed, p.Y));
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
            if((keys[(int)Keys.Z] & 128) == 128)
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
            Rectangle mouserect = pointdown.MakeRect(e.Location);
            if (mouserect.Width < 20 || mouserect.Height < 20) return;

            blocks.Add(new Block(mouserect, shift));
        }

        private void Step(object s, EventArgs e)
        {
            player.Step(ticknum);                 

            foreach(var bu in bullets)
            {
                bu.Step();
            }
            //foreach (var en in enemies) en.Step(ticknum);
            for (int i = 0; i < enemies.Count; i++) enemies[i].Step(ticknum);
            for (int i = 0; i < coins.Count; i++)
            {
                if (player.ActorRect.Contains(coins[i]) && !collected[i])
                {
                    collected[i] = true;
                    collectedcoins++;
                }
            }

            int c = bullets.Count;            

            foreach (var dl in DeleteLists) dl.ClearList();
            
            
            
            panel1.Invalidate();
            ticknum++;                        
        }

        
    }
}
