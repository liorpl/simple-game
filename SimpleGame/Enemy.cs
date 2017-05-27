using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using static SimpleGame.Values;

namespace SimpleGame
{
    public class Enemy : Actor
    {
        const int SPEED = 1;

        private static Size enemysize = new Size(32, 32);

        public Enemy(Point startpoint) : base(startpoint, new Bitmap[]{ new Bitmap(@"Images\enemy1.png") })
        {            
            flipping = true;
        }

        //public bool IsMarkedKilled { get; private set; }

        public void Hit()
        {
            enemies.Remove(this);
            coins.Add(new Point(ActorRect.X + ActorRect.Width / 2, ActorRect.Y + ActorRect.Height / 2));
        }

        public override void Step(ulong ticknum)
        {
            base.Step(ticknum);
            //if (OnGround) TryMove((ref Rectangle r) => r.X += Direction * SPEED);
            if (OnGround) TryMove(p => new Point(p.X + Direction * SPEED, p.Y));
            if (ActorRect.IntersectsWith(player.ActorRect))
            {
                if (player.GroundPound)
                {
                    Hit();
                }
                else
                {
                    //gameover = true;
                    player.Hit();
                    flip ^= 1;
                    //timer.Enabled = false;
                }
            }
            //foreach(var b in bullets)
            //{
            //    if(actorrect.Contains(b))
                    
            //}            
        }

    }
}
