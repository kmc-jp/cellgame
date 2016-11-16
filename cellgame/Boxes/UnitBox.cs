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
        // 選択中のユニットの位置
        public int x_index = -1, y_index = 0;
        // 選択中のユニット
        public Unit u;

        // コマンドの数
        public readonly int command_N = 5;
        public enum Commands
        {
            Move, Attack, Skip, Sleep, Delete
        }
        List<BlindButton> commandButton;
        #endregion
        #region Method
        public UnitBox()
            : base(DataBase.BarPos[1], DataBase.BarWidth[1], DataBase.BarHeight[1]) {
            u = new Unit(UnitType.NULL);
            commandButton = new List<BlindButton>();
            for(int i = 0;i < command_N; i++)
            {
                commandButton.Add(new BlindButton(windowPosition + new Vector(10 + 50 * i, 240), new Vector2(40, 40)));
            }
        }
        public override void Draw(Drawing d)
        {
            if (x_index != -1)
            {
                base.Draw(d);
                // コマンドボタン表示
                if(u.type > 0)
                {
                    for (int i = 0; i < command_N; i++)
                    {
                        d.Draw(windowPosition + new Vector(10 + 50 * i, 240), DataBase.command_tex[i], DepthID.Message);
                    }
                }

                if(u.type == UnitType.Plasma)
                {
                    d.Draw(windowPosition + new Vector(20, 20), DataBase.Plasma_tex[(int)u.enemyType + 5], DepthID.Message);
                    new TextAndFont(u.Name + string.Format("\n({0})", DataBase.EnemyUnitName[(int)u.enemyType + 5]), FontID.Medium, Color.Black).Draw(d, windowPosition + new Vector(180, 80), DepthID.Message);
                }
                else if (u.type == UnitType.Virus)
                {
                    d.Draw(windowPosition + new Vector(20, 20), DataBase.Virus_tex[u.virusState], DepthID.Message);
                    new TextAndFont(u.Name + (u.virusState == 0 ? "" : "\n(定着状態)"), FontID.Medium, Color.Black).Draw(d, windowPosition + new Vector(180, 80), DepthID.Message);
                }
                else
                {
                    d.Draw(windowPosition + new Vector(20, 20), u.type > 0 ? DataBase.myUnit_tex[(int)u.type - 1] : DataBase.enemyUnit_tex[(int)u.type + 5], DepthID.Message);
                    new TextAndFont(u.Name, FontID.Medium, Color.Black).Draw(d, windowPosition + new Vector(180, 80), DepthID.Message);
                }

                new TextAndFont("HP", FontID.Medium, Color.Black).Draw(d, windowPosition + new Vector(20, 160), DepthID.Message);
                new TextAndFont("LP", FontID.Medium, Color.Black).Draw(d, windowPosition + new Vector(20, 180), DepthID.Message);

                new Gauge(new Vector2(100, 10), Color.Red, 0, u.MAX_HP, u.HP, new Color(255, 192, 192)).Draw(
                    d, windowPosition + new Vector(60, 165), DepthID.Message
                    );
                new Gauge(new Vector2(100, 10), new Color(0, 255, 0), 0, u.MAX_LP, u.LP, new Color(192, 255, 192)).Draw(
                    d, windowPosition + new Vector(60, 185), DepthID.Message
                    );

                new TextAndFont(string.Format("{0}/{1}", u.HP, u.MAX_HP), FontID.Medium, Color.Black).Draw(d, windowPosition + new Vector(180, 160), DepthID.Message);
                new TextAndFont(string.Format("{0}/{1}", u.LP, u.MAX_LP), FontID.Medium, Color.Black).Draw(d, windowPosition + new Vector(180, 180), DepthID.Message);


                new TextAndFont("戦闘力", FontID.Medium, Color.Black).Draw(d, windowPosition + new Vector(20, 200), DepthID.Message);
                int real = PlayScene.um.RealStrength(x_index + (y_index + 1) / 2, y_index), pre = u.Strength;
                if (real == pre)
                    new TextAndFont(string.Format("{0}", real), FontID.Medium, Color.Black).Draw(d, windowPosition + new Vector(90, 200), DepthID.Message);
                else
                    new TextAndFont(string.Format("{0}({1})", real, pre), FontID.Medium, real > pre ? Color.Red : Color.Blue).Draw(d, windowPosition + new Vector(90, 200), DepthID.Message);
                new TextAndFont(string.Format("移動力 {0}/{1}", u.movePower, u.moveRange), FontID.Medium, Color.Black).Draw(d, windowPosition + new Vector(180, 200), DepthID.Message);
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
            u = new Unit(UnitType.NULL);
        }
        public override bool IsOn(int x, int y)
        {
            return x_index != -1 && base.IsOn(x, y);
        }
        public void Update(MouseState pstate, MouseState state, SceneManager s)
        {
            for(int i = 0;i < command_N; i++)
            {
                commandButton[i].Update(pstate, state, x_index != -1 && u.type > 0);
            }

            if (x_index == -1 || u.type <= 0 || PlayScene.um.moveAnimation || PlayScene.um.attackAnimation) return;

            if (commandButton[(int)Commands.Move].Clicked())
            {
                if (PlayScene.um.moving)
                    PlayScene.um.CancelMoving();
                else
                    PlayScene.um.StartMoving();
            }
            if (commandButton[(int)Commands.Attack].Clicked() && u.type != UnitType.Plasma)
            {
                if (PlayScene.um.attacking)
                    PlayScene.um.CancelAttacking();
                else
                    PlayScene.um.StartAttacking();
            }
            if (commandButton[(int)Commands.Skip].Clicked())
            {
                PlayScene.um.Skip();
            }
            if (commandButton[(int)Commands.Sleep].Clicked())
            {
                PlayScene.um.Sleep();
            }
            if (commandButton[(int)Commands.Delete].Clicked())
            {
                new DeleteUnit(s);
            }
        }
        #endregion
    }// class end
}// namespace end
