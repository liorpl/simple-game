using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using static SimpleGame.Values;

namespace SimpleGame
{
    public class Player
    {
        Bitmap character = new Bitmap("char.png");

        public Rectangle rect = new Rectangle(10, 10, 64, 64);

        public bool flip, previousflip = false;
        bool gravity = false;
        public int downspeed = 0;

        public void Step(ulong ticknum)
        {
            if (gravity)
            {
                if (ticknum % 25 == 0) downspeed++;
                //rect.Y = testloc;
            }

            if (rect.Bottom > Form1.BoundsRect.Bottom) { rect.Y = 0; }
            previousflip = flip;
            trymovedown();
        }

        public void trymove(RectAction a)
        {
            Rectangle testr = rect;
            a(ref testr);
            foreach (var bl in blocks)
                if (testr.IntersectsWith(bl)) return;
            a(ref rect);
        }

        private void trymovedown()
        {
            Rectangle testr = rect;
            testr.Y++;
            foreach (var b in blocks)
                if (b.IntersectsWith(testr)) { gravity = false; downspeed = 0; return; }
            gravity = true;
            rect.Y += downspeed;
        }

        public void createbullet()
        {
            PointClass p = new PointClass(rect.X + rect.Width, rect.Y + (rect.Height / 2));
            p.Direction = 1 - Convert.ToInt32(flip) * 2;
            bullets.Add(p);
        }

        public void Flip() { character.RotateFlip(RotateFlipType.RotateNoneFlipX); }

        public void draw(Graphics g) { g.DrawImage(character, rect); }
    }
}
