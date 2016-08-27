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
        public void Draw(Drawing d, Map nMap, Vector Camera, int Scale)
        {
            // ボックスの背景の表示
            base.Draw(d);

            // ボックスの中身の表示
            if (showing)
            {
                int sizeX = width * 16 - 90, sizeY = height * 16 - 60;
                Vector dposition = new Vector(windowPosition.X + 50, windowPosition.Y + 30);
                Vector dcamera = new Vector(Camera.X + Game1._WindowSizeX / DataBase.MapScale[Scale] / 2 - sizeX / DataBase.MapScale[0] / 2,
                                         Camera.Y + Game1._WindowSizeY / DataBase.MapScale[Scale] / 2 - sizeY / DataBase.MapScale[0] / 2);
                nMap.Draw(d, dcamera, 0, DepthID.StateFront, sizeX, sizeY, dposition);
            }
        }
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
