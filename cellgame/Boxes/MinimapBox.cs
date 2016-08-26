using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonPart
{
    class MinimapBox : WindowBox
    {
        #region Variable
        #endregion
        #region Method
        public MinimapBox()
            : base(DataBase.BarPos[2], DataBase.BarWidth[2], DataBase.BarHeight[2]) { }
        public override void Update()
        {
            base.Update();
            if (Settings.WindowStyle == 1 && windowPosition.Y != DataBase.BarPos[2].Y)
                windowPosition = DataBase.BarPos[2];
            else if (Settings.WindowStyle == 0 && windowPosition.Y == DataBase.BarPos[2].Y)
                windowPosition = new Vector(DataBase.BarPos[2].X, DataBase.BarPos[2].Y - (DataBase.WindowDefaultSizeY - DataBase.WindowSlimSizeY));
        }
        #endregion
    }// class end
}// namespace end
