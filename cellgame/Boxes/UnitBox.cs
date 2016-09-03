using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonPart
{
    class UnitBox : WindowBar
    {
        #region Variable
        int x_index = -1, y_index = 0;
        #endregion
        #region Method
        public UnitBox()
            : base(DataBase.BarPos[1], DataBase.BarWidth[1], DataBase.BarHeight[1]) { }
        public void Draw(Drawing d)
        {
            if (x_index != -1)
            {
                base.Draw(d);

            }
        }
        public void Update(UnitManager um)
        {

        }
        #endregion
    }// class end
}// namespace end
