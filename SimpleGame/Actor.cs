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
        public int Direction => flip * 2 - 1;
        protected bool gravity = false;
        protected int downspeed = 1;        
        public bool OnGround { get; protected set; }

        protected Bitmap[] images = new Bitmap[8];
        protected int currentchar = 0;
        protected bool flipping = false;

        public Block LastGroundBlock { get; private set; } = Rectangle.Empty;

        private Point position;
        public Rectangle ActorRect => new Rectangle(position, images[0].Size);

        public Actor(Point startpoint, Bitmap[] ims)
        {
            images = ims;
            position = startpoint;
        }

        public void TryMove(Func<Point,Point> a)
        {
            Point testp = position;
            testp = a(testp);
            Rectangle testr = new Rectangle(testp, images[currentchar].Size);
            foreach (var bl in blocks)
                if (bl.Rect.IntersectsWith(testr)) { if (flipping) flip ^= 1; return; }
            position = a(position);
        }

        public bool TryMoveDown()
        {
            Rectangle testr = ActorRect;
            testr.Y += downspeed;
            foreach (var b in blocks)
                if (b.Rect.IntersectsWith(testr)) { downspeed = 1; position.Y += downspeed - 1; OnGround = true; LastGroundBlock = b; return false; }
            position.Y += downspeed;
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
            
            if (ActorRect.Bottom > Form1.BoundsRect.Bottom) { position.Y = -1 * ActorRect.Height; }
            previousflip = flip;
        }

        public void Flip() { foreach (var i in images) i?.RotateFlip(RotateFlipType.RotateNoneFlipX); }

        public void Draw(Graphics g) { g.DrawImage(images[currentchar], ActorRect); }
    }
}
