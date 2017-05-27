using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using static SimpleGame.Values;

namespace SimpleGame
{
    public class Bullet : IMarkDelete
    {
        const int SPEED = 4;

        public static readonly Brush[] bulletcolors =
        {
            Brushes.Red,
            Brushes.Yellow,
            Brushes.Orange,
            Brushes.Blue,
            Brushes.Cyan,
            Brushes.ForestGreen,
            Brushes.LightGreen,
            Brushes.LightGray,
            Brushes.Magenta
        };

        public bool IsMarkedKilled { get; set; } = false;       
        public Point Pos { get; set; }
        public int Direction { get; set; }
        public int BulletType { get; set; } = 0;
        public bool EnemyBullet { get; set; } = false;

        public void Step()
        {
            //bullets[i].Item1.X += bullets[i].Item2 * 4;
            var blk = trymove(p => new Point(p.X + Direction * SPEED, p.Y));
            if (blk != null)
            {
                IsMarkedKilled = true;
                if (blk.Fragile) blk.Clear();
            }
            else
            {
                for (int j = 0; j < enemies.Count; j++)
                {
                    if (enemies[j].ActorRect.Contains(Pos))
                    {
                        enemies[j].Hit();
                        //bullets.RemoveAt(i);
                        IsMarkedKilled = true;
                    }
                }
            }

            if (!Form1.BoundsRect.Contains(Pos))
                IsMarkedKilled = true;
        }

        private Block trymove(Func<Point, Point> pa)
        {
            var testp = Pos;
            testp = pa(testp);

            foreach (var b in blocks)
                if (b.Rect.Contains(Pos)) return b;
            Pos = pa(Pos);
            return null;
        }

        public static implicit operator Point(Bullet b) => b.Pos;
    }
}
