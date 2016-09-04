using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace CommonPart
{
    class UnitBox : WindowBar
    {
        #region Variable
        public int x_index = -1, y_index = 0;
        Unit u;
        #endregion
        #region Method
        public UnitBox()
            : base(DataBase.BarPos[1], DataBase.BarWidth[1], DataBase.BarHeight[1]) {
        }
        public override void Draw(Drawing d)
        {
            if (x_index != -1)
            {
                base.Draw(d);
                if (u.type > 0)
                {
                    d.Draw(windowPosition + new Vector(20, 20), DataBase.myUnit_tex[(int)u.type - 1], DepthID.Message);
                }
                else if(u.type < 0)
                {
                    d.Draw(windowPosition + new Vector(20, 20), DataBase.enemyUnit_tex[(int)u.type + 5], DepthID.Message);
                }
                new RichText(u.unitName).Draw(d, windowPosition + new Vector(150, 100), DepthID.Message);

                new RichText("HP").Draw(d, windowPosition + new Vector(20, 160), DepthID.Message);
                new RichText("LP").Draw(d, windowPosition + new Vector(20, 180), DepthID.Message);
                new RichText("EXP").Draw(d, windowPosition + new Vector(20, 200), DepthID.Message);

                new Gauge(new Vector2(100, 10), Color.Red, 0, u.MAX_HP, u.HP, new Color(255, 128, 128)).Draw(
                    d, windowPosition + new Vector(60, 165), DepthID.Message);
                new Gauge(new Vector2(100, 10), new Color(0, 255, 0), 0, u.MAX_LP, u.LP, new Color(128, 255, 128)).Draw(
                    d, windowPosition + new Vector(60, 185), DepthID.Message);
                new Gauge(new Vector2(100, 10), Color.Blue, 0, u.MAX_EXP, u.EXP, new Color(128, 128, 255)).Draw(
                    d, windowPosition + new Vector(60, 205), DepthID.Message);

                new RichText(string.Format("{0}/{1}", u.MAX_HP, u.HP)).Draw(d, windowPosition + new Vector(180, 160), DepthID.Message);
                new RichText(string.Format("{0}/{1}", u.MAX_LP, u.LP)).Draw(d, windowPosition + new Vector(180, 180), DepthID.Message);
                new RichText(string.Format("{0}/{1}", u.MAX_EXP, u.EXP)).Draw(d, windowPosition + new Vector(180, 200), DepthID.Message);
            }
        }
        // クリックされた位置を入力としてコマンドを実行
        public void Command(int x, int y, UnitManager um, Map nMap)
        {
            if(x >= windowPosition.X + 10 && x <= windowPosition.X + 50 && y >= windowPosition.Y + 240 && y <= windowPosition.Y + 280)
            {
                um.StartMoving(nMap);
            }
            else if (x >= windowPosition.X + 60 && x <= windowPosition.X + 100 && y >= windowPosition.Y + 240 && y <= windowPosition.Y + 280)
            {

            }
            else if (x >= windowPosition.X + 110 && x <= windowPosition.X + 150 && y >= windowPosition.Y + 240 && y <= windowPosition.Y + 280)
            {

            }
            else if (x >= windowPosition.X + 160 && x <= windowPosition.X + 200 && y >= windowPosition.Y + 240 && y <= windowPosition.Y + 280)
            {

            }
        }
        // 基本的にユニットマネージャから呼び出される
        public void Select(int _x_index, int _y_index, Unit _u)
        {
            x_index = _x_index;
            y_index = _y_index;
            u = _u;
        }
        public bool IsOn(int x, int y)
        {
            return x_index != -1 && base.IsOn(x, y);
        }
        public bool IsOnButton(int x, int y)
        {
            return x_index != -1 && (
                    (x >= windowPosition.X + 10 && x <= windowPosition.X + 50 && y >= windowPosition.Y + 240 && y <= windowPosition.Y + 280) ||
                    (x >= windowPosition.X + 60 && x <= windowPosition.X + 100 && y >= windowPosition.Y + 240 && y <= windowPosition.Y + 280) ||
                    (x >= windowPosition.X + 110 && x <= windowPosition.X + 150 && y >= windowPosition.Y + 240 && y <= windowPosition.Y + 280) ||
                    (x >= windowPosition.X + 160 && x <= windowPosition.X + 200 && y >= windowPosition.Y + 240 && y <= windowPosition.Y + 280) );
        }
        public void Update(UnitManager um)
        {
            if(x_index != -1)
                u = um.Find(x_index, y_index);
        }
        #endregion
    }// class end
}// namespace end
