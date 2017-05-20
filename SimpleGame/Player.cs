using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using static SimpleGame.Values;

namespace SimpleGame
{
    public class Player : Actor
    {
        
        public bool GroundPound { get; private set; }

        public Player() : base(Point.Empty, new Bitmap[] { new Bitmap(@"Images\char.png"), new Bitmap(@"Images\charcrouch.png") })
           // ,new Bitmap(@"Images\chargp.png") })
        {
        }

        public bool crouch = false;

        public override void Step(ulong ticknum)
        {
            base.Step(ticknum);
            currentchar = Convert.ToInt32(crouch);          
        }                

        public void CreateBullet()
        {
            int yb = ActorRect.Y + (crouch ? ActorRect.Height - 8 : ActorRect.Height / 2);
            var p = new Point(ActorRect.X + ActorRect.Width, yb);
            bullets.Add(new Bullet { Pos = p, Direction = 1 - flip * 2, BulletType = bulletstate });
        }       
        
        public async void HandleGroundPound()
        {
            if (OnGround) return;
            GroundPound = true;
            crouch = true;
            downspeed = 50;
            await Task.Factory.StartNew(() => { while (!OnGround) { } });
            if (LastGroundBlock.Rect != Rectangle.Empty && LastGroundBlock.Fragile)
                LastGroundBlock.Clear();
            crouch = false;
            GroundPound = false;
        } 
        
    }
}
