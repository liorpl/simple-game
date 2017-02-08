using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using static SimpleGame.Values;

namespace SimpleGame
{
    public abstract class Actor
    {
        public int flip, previousflip = 0;
        public int Direction { get { return flip * 2 - 1; } }
        protected bool gravity = false;
        protected int downspeed = 0;        
        public bool OnGround { get; protected set; }

        protected Bitmap[] images = new Bitmap[8];
        protected int currentchar = 0;
        protected bool flipping = false;

        public Block LastGroundBlock { get; private set; } = Rectangle.Empty;

        public Rectangle actorrect = new Rectangle();        

        public Actor(Point startpoint, Bitmap[] ims)
        {
            images = ims;
            actorrect = new Rectangle(startpoint, Size.Round(images[0].PhysicalDimension));
        }

        public virtual void TryMove(RectAction a)
        {
            Rectangle testr = actorrect;
            a(ref testr);
            foreach (var bl in blocks)
                if (bl.Rect.IntersectsWith(testr)) { if (flipping) flip^=1; return; }
            a(ref actorrect);
        }

        public bool TryMoveDown()
        {
            Rectangle testr = actorrect;
            testr.Y += downspeed + 1;
            foreach (var b in blocks)
                if (b.Rect.IntersectsWith(testr)) { downspeed = 1; OnGround = true; LastGroundBlock = b; return false; }
            actorrect.Y += downspeed;
            OnGround = false;
            return true;
        }

        public void Jump() { if (OnGround) downspeed = -8; }

        public virtual void Step(ulong ticknum)
        {
            if (!OnGround)
            {
                if (ticknum % 5 == 0) downspeed++;
            }

            TryMoveDown();
            if (previousflip != flip) Flip();
            
            if (actorrect.Bottom > Form1.BoundsRect.Bottom) { actorrect.Y = -1 * actorrect.Height; }
            previousflip = flip;
        }

        public void Flip() { foreach (var i in images) i?.RotateFlip(RotateFlipType.RotateNoneFlipX); }

        public void Draw(Graphics g) { g.DrawImage(images[currentchar], actorrect); }
    }
}
