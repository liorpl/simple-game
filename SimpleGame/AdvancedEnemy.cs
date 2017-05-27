using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SimpleGame
{
    public class AdvancedEnemy : Enemy
    {
        public AdvancedEnemy(Point startpoint) : base(startpoint) { }

        public override void Step(ulong ticknum)
        {
            base.Step(ticknum);
            if (ticknum % 10 == 0)
            {

            }
        }
    }
}
