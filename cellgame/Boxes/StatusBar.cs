using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonPart
{
    class StatusBar : WindowBar
    {
        #region Variable
        #endregion
        #region Method
        public StatusBar()
            : base(DataBase.BarPos[3], DataBase.BarWidth[3], DataBase.BarHeight[3]) { }
        #endregion
    }// class end
}// namespace end
