﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CommonPart
{
    class ProductScene : Scene
    {
        #region Variable
        // 背景の枠
        WindowBar backGround;
        // 必要なPlayScene内の変数
        ProductArrangeBar pab;

        MouseState pstate;

        int select;
        FilledBox fb;
        BoxFrame bf;
        Button start;
        Button cancel;
        public static int productable = 5;
        #endregion

        #region Method
        public ProductScene(SceneManager s, ProductArrangeBar pab_) : base(s)
        {
            s.BackSceneNumber++;
            backGround = new WindowBar(new Vector(352, 160), 40, 25);
            pab = pab_;
            pstate = Mouse.GetState();

            start = new Button(backGround.windowPosition + new Vector(315, 320), 120, new Color(255, 162, 0), new Color(200, 120, 0), "生産開始");
            cancel = new Button(backGround.windowPosition + new Vector(465, 320), 120, new Color(255, 162, 0), new Color(200, 120, 0), "キャンセル");
            fb = new FilledBox(new Vector2(240, 360), Color.White);
            bf = new BoxFrame(new Vector2(230, 25), Color.Black);
            select = 0;
        }
        public override void SceneDraw(Drawing d)
        {
            backGround.Draw(d);
            fb.Draw(d, new Vector(backGround.windowPosition.X + 20, backGround.windowPosition.Y + 20), DepthID.Message);
            base.Draw(d);
            for (int i = 0; i < productable; i++)
            {
                new TextAndFont(DataBase.MyUnitName[i], Color.Black).Draw(d, backGround.windowPosition + new Vector(30, 30 + 25 * i), DepthID.Status);
            }
            bf.Draw(d, backGround.windowPosition + new Vector(25, 30 + 25 * select), DepthID.Effect);
            d.Draw(backGround.windowPosition + new Vector(386, 20), select < 9 ? DataBase.myUnit_tex[select] : DataBase.enemyUnit_tex[select - 9], DepthID.Status);
            new TextAndFont(select < 9 ? DataBase.MyUnitName[select] : DataBase.EnemyUnitName[select - 9], Color.Black).Draw(d, backGround.windowPosition + new Vector(450 - (select < 9 ? DataBase.MyUnitName[select] : DataBase.EnemyUnitName[select - 9]).Length * 10, 150), DepthID.Status);
            new TextAndFont(string.Format("戦闘力　{0}", select < 9 ? DataBase.MyUnitStrength[select] : DataBase.EnemyUnitStrength[select - 9]), Color.Black).Draw(d, backGround.windowPosition + new Vector(300, 180), DepthID.Status);
            new TextAndFont(string.Format("移動力　{0}", select < 9 ? DataBase.MyUnitMoveRange[select] : DataBase.EnemyUnitMoveRange[select - 9]), Color.Black).Draw(d, backGround.windowPosition + new Vector(450, 180), DepthID.Status);
            new TextAndFont(string.Format("HP　　　{0}", select < 9 ? DataBase.MyUnitMAX_HP[select] : DataBase.EnemyUnitMAX_HP[select - 9]), Color.Black).Draw(d, backGround.windowPosition + new Vector(300, 210), DepthID.Status);
            new TextAndFont(string.Format("LP　　　{0}", select < 9 ? DataBase.MyUnitMAX_LP[select] : DataBase.EnemyUnitMAX_LP[select - 9]), Color.Black).Draw(d, backGround.windowPosition + new Vector(450, 210), DepthID.Status);

            cancel.Draw(d);
            start.Draw(d);
        }
        public override void SceneUpdate()
        {
            MouseState state = Mouse.GetState();
            if (pstate.LeftButton == ButtonState.Released && state.LeftButton == ButtonState.Pressed)
            {
                for (int i = 0; i < productable; i++)
                {
                    if (state.X >= backGround.windowPosition.X + 30 && state.X <= backGround.windowPosition.X + 260 && state.Y >= backGround.windowPosition.Y + 30 + 25 * i && state.Y <= backGround.windowPosition.Y + 55 + 25 * i)
                    {
                        select = i;
                    }
                }
            }
            start.Update(pstate, state);
            if (start.Clicked())
            {
                pab.productBox.Add(select < 9 ? (UnitType)(select + 1) : (UnitType)(select - 14));
                Delete = true;
            }
            cancel.Update(pstate, state);
            if (cancel.Clicked())
            {
                Delete = true;
            }
            pstate = state;

            for (int i = 0; i < productable; i++)
            {
                if (state.X >= backGround.windowPosition.X + 30 && state.X <= backGround.windowPosition.X + 260 && state.Y >= backGround.windowPosition.Y + 30 + 25 * i && state.Y <= backGround.windowPosition.Y + 55 + 25 * i)
                {
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Hand;
                }
            }
        }
        #endregion
    }
}