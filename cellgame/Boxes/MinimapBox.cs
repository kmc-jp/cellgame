using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace CommonPart
{
    class MinimapBox : WindowBox
    {
        #region Variable
        #endregion
        #region Method
        public MinimapBox()
            : base(DataBase.BarPos[2], DataBase.BarWidth[2], DataBase.BarHeight[2]) { }
        public void Draw(Drawing d, UnitManager um, Map nMap, Vector Camera, int Scale)
        {
            // ボックスの背景の表示
            base.Draw(d);
            
            // ボックスの中身の表示
            if (showing)
            {
                int sizeX = width * 16 - 19, sizeY = height * 16 - 5;
                Vector dposition = new Vector(windowPosition.X + 17, windowPosition.Y + 3);
                Vector dcamera = new Vector(Camera.X + Game1._WindowSizeX / DataBase.MapScale[Scale] / 2 - sizeX / DataBase.MapScale[0] / 2,
                                         Camera.Y + Game1._WindowSizeY / DataBase.MapScale[Scale] / 2 - sizeY / DataBase.MapScale[0] / 2);
                nMap.DrawMinimap(d, um, dcamera, DepthID.StateFront, DepthID.Message, sizeX, sizeY, dposition);

                BoxFrame bf = new BoxFrame(new Vector(Game1._WindowSizeX / DataBase.MapScale[Scale] * DataBase.MapScale[0], Game1._WindowSizeY / DataBase.MapScale[Scale] * DataBase.MapScale[0]), Color.Black);
                bf.Draw(d, dposition + new Vector((Camera.X - dcamera.X) * DataBase.MapScale[0], (Camera.Y - dcamera.Y) * DataBase.MapScale[0]), DepthID.Effect);
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
