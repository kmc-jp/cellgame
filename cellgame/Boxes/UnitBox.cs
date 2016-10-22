﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CommonPart
{
    class UnitBox : WindowBar
    {
        #region Variable
        public int x_index = -1, y_index = 0;
        public Unit u;
        #endregion
        #region Method
        public UnitBox()
            : base(DataBase.BarPos[1], DataBase.BarWidth[1], DataBase.BarHeight[1]) {
            u = new Unit(UnitType.NULL);
        }
        public override void Draw(Drawing d)
        {
            if (x_index != -1)
            {
                base.Draw(d);
                // コマンドボタン表示
                if(u.type > 0)
                {
                    d.Draw(windowPosition + new Vector(10, 240), DataBase.command_tex[0], DepthID.Message);
                    d.Draw(windowPosition + new Vector(60, 240), DataBase.command_tex[1], DepthID.Message);
                    d.Draw(windowPosition + new Vector(110, 240), DataBase.command_tex[2], DepthID.Message);
                    d.Draw(windowPosition + new Vector(160, 240), DataBase.command_tex[3], DepthID.Message);
                }

                if (u.type > 0)
                {
                    d.Draw(windowPosition + new Vector(20, 20), DataBase.myUnit_tex[(int)u.type - 1], DepthID.Message);
                }
                else if(u.type < 0)
                {
                    d.Draw(windowPosition + new Vector(20, 20), DataBase.enemyUnit_tex[(int)u.type + 5], DepthID.Message);
                }
                new TextAndFont(u.unitName, FontID.Medium, Color.Black).Draw(d, windowPosition + new Vector(150, 100), DepthID.Message);

                new TextAndFont("HP", FontID.Medium, Color.Black).Draw(d, windowPosition + new Vector(20, 160), DepthID.Message);
                new TextAndFont("LP", FontID.Medium, Color.Black).Draw(d, windowPosition + new Vector(20, 180), DepthID.Message);
                new TextAndFont("EXP", FontID.Medium, Color.Black).Draw(d, windowPosition + new Vector(20, 200), DepthID.Message);

                new Gauge(new Vector2(100, 10), Color.Red, 0, u.MAX_HP, u.HP, new Color(255, 192, 192)).Draw(
                    d, windowPosition + new Vector(60, 165), DepthID.Message);
                new Gauge(new Vector2(100, 10), new Color(0, 255, 0), 0, u.MAX_LP, u.LP, new Color(192, 255, 192)).Draw(
                    d, windowPosition + new Vector(60, 185), DepthID.Message);
                new Gauge(new Vector2(100, 10), Color.Blue, 0, u.MAX_EXP, u.EXP, new Color(192, 192, 255)).Draw(
                    d, windowPosition + new Vector(60, 205), DepthID.Message);

                new TextAndFont(string.Format("{0}/{1}", u.HP, u.MAX_HP), FontID.Medium, Color.Black).Draw(d, windowPosition + new Vector(180, 160), DepthID.Message);
                new TextAndFont(string.Format("{0}/{1}", u.LP, u.MAX_LP), FontID.Medium, Color.Black).Draw(d, windowPosition + new Vector(180, 180), DepthID.Message);
                new TextAndFont(string.Format("{0}/{1}", u.EXP, u.MAX_EXP), FontID.Medium, Color.Black).Draw(d, windowPosition + new Vector(180, 200), DepthID.Message);


                new TextAndFont(string.Format("移動力 {0}/{1}", u.movePower, u.moveRange), FontID.Medium, Color.Black).Draw(d, windowPosition + new Vector(240, 180), DepthID.Message);
            }
        }
        // クリックされた位置を入力としてコマンドを実行
        public void Command(int x, int y, UnitManager um, ref Map nMap)
        {
            if (u.type <= 0 || um.moveAnimation || um.attackAnimation) return;

            if(x >= windowPosition.X + 10 && x <= windowPosition.X + 50 && y >= windowPosition.Y + 240 && y <= windowPosition.Y + 280)
            {
                if (um.moving)
                    um.CancelMoving();
                else
                    um.StartMoving(ref nMap);
            }
            else if (x >= windowPosition.X + 60 && x <= windowPosition.X + 100 && y >= windowPosition.Y + 240 && y <= windowPosition.Y + 280)
            {
                if (um.attacking)
                    um.CancelAttacking();
                else
                    um.StartAttacking();
            }
            else if (x >= windowPosition.X + 110 && x <= windowPosition.X + 150 && y >= windowPosition.Y + 240 && y <= windowPosition.Y + 280)
            {
                um.Skip();
            }
            else if (x >= windowPosition.X + 160 && x <= windowPosition.X + 200 && y >= windowPosition.Y + 240 && y <= windowPosition.Y + 280)
            {
                um.Sleep();
            }
        }
        // 基本的にユニットマネージャから呼び出される
        public void Select(int _x_index, int _y_index, Unit _u)
        {
            x_index = _x_index;
            y_index = _y_index;
            u = _u;
        }
        public void Unselect()
        {
            x_index = -1;
            y_index = 0;
        }
        public override bool IsOn(int x, int y)
        {
            return x_index != -1 && base.IsOn(x, y);
        }
        public override bool IsOnButton(int x, int y)
        {
            return x_index != -1 && u.type > 0 && (
                    (x >= windowPosition.X + 10 && x <= windowPosition.X + 50 && y >= windowPosition.Y + 240 && y <= windowPosition.Y + 280) ||
                    (x >= windowPosition.X + 60 && x <= windowPosition.X + 100 && y >= windowPosition.Y + 240 && y <= windowPosition.Y + 280) ||
                    (x >= windowPosition.X + 110 && x <= windowPosition.X + 150 && y >= windowPosition.Y + 240 && y <= windowPosition.Y + 280) ||
                    (x >= windowPosition.X + 160 && x <= windowPosition.X + 200 && y >= windowPosition.Y + 240 && y <= windowPosition.Y + 280));
        }
        public override void Update() { }
        #endregion
    }// class end
}// namespace end
