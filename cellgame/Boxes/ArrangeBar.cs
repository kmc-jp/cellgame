using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonPart
{
    class ArrangeBar : WindowBar
    {
        #region Variable
        #endregion
        #region Method
        public ArrangeBar()
            : base(DataBase.BarPos[4], DataBase.BarWidth[4], DataBase.BarHeight[4]) { }
        public override void Draw(Drawing d)
        {
            base.Draw(d);

        }
        #endregion
    }// class end
}// namespace end
