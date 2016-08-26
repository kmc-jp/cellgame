using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonPart
{
    class ProductBox : WindowBox
    {
        #region Variable
        #endregion
        #region Method
        public ProductBox()
            : base(DataBase.BarPos[5], DataBase.BarWidth[5], DataBase.BarHeight[5]) {
            leftHide = false;
            showing = false;
        }
        #endregion
    }// class end
}// namespace end
