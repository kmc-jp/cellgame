using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonPart
{
    class UnitBox : WindowBox
    {
        #region Variable
        #endregion
        #region Method
        public UnitBox()
            : base(DataBase.BarPos[1], DataBase.BarWidth[1], DataBase.BarHeight[1]) { }
        #endregion
    }// class end
}// namespace end
