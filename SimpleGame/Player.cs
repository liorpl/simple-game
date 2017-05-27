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
        public int Lives { get; private set; } = 5;

        public Player() : base(Point.Empty, new Bitmap[] { new Bitmap(@"Images\char.png"), new Bitmap(@"Images\charcrouch.png"),
            new Bitmap(@"Images\chargp.png") })
        {
        }

        public bool crouch = false;

        public override void Step(ulong ticknum)
        {
            base.Step(ticknum);
            currentchar = GroundPound ? 2 : crouch ? 1 : 0;         
        }                

        public void CreateBullet()
        {
            int yb = ActorRect.Y + (crouch ? ActorRect.Height - 8 : ActorRect.Height / 2 + 16);
            var p = new Point(ActorRect.X + ActorRect.Width, yb);
            bullets.Add(new Bullet { Pos = p, Direction = 1 - flip * 2, BulletType = bulletstate });
        }

        public void Hit()
        {
            Lives--;
            if (Lives == 0) { gameover = true; timer.Enabled = false; }
        }

        public async void HandleGroundPound()
        {
            if (OnGround) return;
            GroundPound = true;
            crouch = true;
            downspeed = 30;
            //Task t = new Task(() => { while (!OnGround) { } });
            //t.Start();
            //await t;
            await Task.Factory.StartNew(() => { while (!OnGround) { } });
            if (LastGroundBlock.Rect != Rectangle.Empty && LastGroundBlock.Fragile)
                LastGroundBlock.Clear();
            crouch = false;
            GroundPound = false;
        } 
        
    }
}
