using System;
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
        Unit u;
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
                    d.Draw(windowPosition + new Vector(210, 240), DataBase.command_tex[4], DepthID.Message);
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
        public void Command(int x, int y, UnitManager um, Map nMap)
        {
            if (u.type <= 0) return;

            if(x >= windowPosition.X + 10 && x <= windowPosition.X + 50 && y >= windowPosition.Y + 240 && y <= windowPosition.Y + 280)
            {
                um.StartMoving(nMap);
            }
            else if (x >= windowPosition.X + 60 && x <= windowPosition.X + 100 && y >= windowPosition.Y + 240 && y <= windowPosition.Y + 280)
            {
                um.Attack(nMap);
            }
            else if (x >= windowPosition.X + 110 && x <= windowPosition.X + 150 && y >= windowPosition.Y + 240 && y <= windowPosition.Y + 280)
            {
                um.Skip(this);
            }
            else if (x >= windowPosition.X + 160 && x <= windowPosition.X + 200 && y >= windowPosition.Y + 240 && y <= windowPosition.Y + 280)
            {
                um.Sleep(this);
            }
            else if (x >= windowPosition.X + 210 && x <= windowPosition.X + 250 && y >= windowPosition.Y + 240 && y <= windowPosition.Y + 280)
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
        public void Unselect()
        {
            x_index = -1;
            y_index = 0;
        }
        public bool IsOn(int x, int y)
        {
            return x_index != -1 && base.IsOn(x, y);
        }
        public bool IsOnButton(int x, int y)
        {
            return x_index != -1 && u.type > 0 && (
                    (x >= windowPosition.X + 10 && x <= windowPosition.X + 50 && y >= windowPosition.Y + 240 && y <= windowPosition.Y + 280) ||
                    (x >= windowPosition.X + 60 && x <= windowPosition.X + 100 && y >= windowPosition.Y + 240 && y <= windowPosition.Y + 280) ||
                    (x >= windowPosition.X + 110 && x <= windowPosition.X + 150 && y >= windowPosition.Y + 240 && y <= windowPosition.Y + 280) ||
                    (x >= windowPosition.X + 160 && x <= windowPosition.X + 200 && y >= windowPosition.Y + 240 && y <= windowPosition.Y + 280) ||
                    (x >= windowPosition.X + 210 && x <= windowPosition.X + 250 && y >= windowPosition.Y + 240 && y <= windowPosition.Y + 280));
        }
        public void Update(UnitManager um, Map nMap, PlayScene ps, MouseState pstate, MouseState state)
        {
            // 左クリックされたときに移動コマンド中でありその座標が移動可能な位置であればその位置へ選択中のユニットを移動
            if (pstate.LeftButton != ButtonState.Pressed && state.LeftButton == ButtonState.Pressed)
            {
                if (state.X >= 0 && state.X <= Game1._WindowSizeX && state.Y >= 0 && state.Y <= Game1._WindowSizeY)
                {
                    PAIR p = ps.WhichHex(state.X, state.Y);
                    if (p.i >= 0 && p.j >= 0)
                    {
                        if (um.moving)
                        {
                            foreach (PAIR pos in um.movable)
                            {
                                if (p.i == pos.i - (pos.j + 1) / 2 && p.j == pos.j)
                                {
                                    um.Move(pos.i - (pos.j + 1) / 2, pos.j, this);
                                    break;
                                }
                            }
                        }
                        // もしユニットが存在しなければ樹状細胞を生産
                        else if (!um.IsExist(p.i, p.j))
                        {
                            um.Produce(p.i, p.j, UnitType.Jujo);
                        }
                        um.Select(p.i, p.j, this);
                    }
                }

                // クリックされた座標がユニットボックスのコマンドボタンであれば、コマンドを実行
                Command(state.X, state.Y, um, nMap);
            }
            if (x_index != -1)
                u = um.Find(x_index, y_index);
        }
        #endregion
    }// class end
}// namespace end
