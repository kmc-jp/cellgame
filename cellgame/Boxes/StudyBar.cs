using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonPart {
    class StudyBar : WindowBar {
        #region Variable
        #endregion
        #region Method
        public StudyBar()
            : base(DataBase.BarPos[0], DataBase.BarWidth[0], DataBase.BarHeight[0]) { }
        #endregion
    }// class end
}// namespace end
