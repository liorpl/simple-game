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
        public PointClass Pos { get; set; }
        public int BulletType { get; set; } = 0;

        public void Step()
        {
            //bullets[i].Item1.X += bullets[i].Item2 * 4;
            var blk = trymove(po => po.X += po.Direction * 4);
            if (blk != null)
            {
                IsMarkedKilled = true;
                if (blk.Fragile) blk.Clear();
            }
            else
            {
                for (int j = 0; j < enemies.Count; j++)
                {
                    if (enemies[j].actorrect.Contains(Pos))
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

        private Block trymove(Action<PointClass> pa)
        {
            PointClass testp = Pos.Copy();
            //PointClass testp = p;
            pa(testp);
            //for (int i = 0; i < blocks.Count; i++)
            //    if (blocks[i].Contains(p.ToPoint()))
            //        return i;
            foreach (var b in blocks)
                if (b.Rect.Contains(Pos)) return b;
            pa(Pos);
            return null;
        }

        public static implicit operator Point(Bullet b) => b.Pos;
    }
}
