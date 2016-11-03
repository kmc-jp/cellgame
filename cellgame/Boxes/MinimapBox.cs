using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CommonPart
{
    class MinimapBox : WindowBar
    {
        #region Variable
        bool showing = true;
        BlindButton shButton;
        #endregion
        #region Method
        public MinimapBox()
            : base(DataBase.BarPos[2], DataBase.BarWidth[2], DataBase.BarHeight[2]) {
            shButton = new BlindButton(windowPosition, new Vector2(15, 256));
        }
        public void Draw(Drawing d, UnitManager um, Map nMap, Vector Camera, int Scale)
        {
            d.Draw(windowPosition, DataBase.minimapButton[showing ? 0 : 1], DepthID.Map);

            // ボックスの中身の表示
            if (showing)
            {
                // ボックスの背景の表示
                base.Draw(d);

                int sizeX = width * 16 - 19, sizeY = height * 16 - 5;
                Vector dposition = new Vector(windowPosition.X + 17, windowPosition.Y + 3);
                Vector dcamera = new Vector(Camera.X + Game1._WindowSizeX / DataBase.MapScale[Scale] / 2 - sizeX / DataBase.MapScale[0] / 2,
                                         Camera.Y + Game1._WindowSizeY / DataBase.MapScale[Scale] / 2 - sizeY / DataBase.MapScale[0] / 2);
                nMap.DrawMinimap(d, um, dcamera, DepthID.StateFront, DepthID.Message, sizeX, sizeY, dposition);

                BoxFrame bf = new BoxFrame(new Vector(Game1._WindowSizeX / DataBase.MapScale[Scale] * DataBase.MapScale[0], Game1._WindowSizeY / DataBase.MapScale[Scale] * DataBase.MapScale[0]), Color.Black);
                bf.Draw(d, dposition + new Vector((Camera.X - dcamera.X) * DataBase.MapScale[0], (Camera.Y - dcamera.Y) * DataBase.MapScale[0]), DepthID.Effect);
            }
        }
        public void Update(MouseState pstate, MouseState state)
        {
            base.Update();

            shButton.Update(pstate, state);

            if (shButton.Clicked())
                showing = showing == false;

            if (Settings.WindowStyle == 1 && windowPosition.Y != DataBase.BarPos[2].Y)
                windowPosition = DataBase.BarPos[2];
            else if (Settings.WindowStyle == 0 && windowPosition.Y == DataBase.BarPos[2].Y)
                windowPosition = new Vector(DataBase.BarPos[2].X, DataBase.BarPos[2].Y - (DataBase.WindowDefaultSizeY - DataBase.WindowSlimSizeY));
        }
        public override bool IsOn(int x, int y)
        {
            return (showing && base.IsOn(x, y)) || shButton.IsOn();
        }
        #endregion
    }// class end
}// namespace end
