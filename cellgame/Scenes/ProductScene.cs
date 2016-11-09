using System;
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
        public static bool[] productable = {
            true, true, true, true, true, false, false, false
        };
        #endregion

        #region Method
        public ProductScene(SceneManager s, ProductArrangeBar pab_) : base(s)
        {
            s.BackSceneNumber++;
            backGround = new WindowBar(new Vector(272, 160), 50, 35);
            pab = pab_;
            pstate = Mouse.GetState();

            start = new Button(backGround.windowPosition + new Vector(395, 480), 120, new Color(255, 162, 0), new Color(200, 120, 0), "生産開始");
            cancel = new Button(backGround.windowPosition + new Vector(555, 480), 120, new Color(255, 162, 0), new Color(200, 120, 0), "キャンセル");
            fb = new FilledBox(new Vector2(240, 520), Color.White);
            bf = new BoxFrame(new Vector2(230, 25), Color.Black);
            select = 0;
        }
        public override void SceneDraw(Drawing d)
        {
            backGround.Draw(d);
            fb.Draw(d, new Vector(backGround.windowPosition.X + 20, backGround.windowPosition.Y + 20), DepthID.Message);
            base.Draw(d);
            for (int i = 0; i < 8; i++)
            {
                new TextAndFont(DataBase.MyUnitName[i], productable[i] ? Color.Black : new Color(180, 180, 180)).Draw(d, backGround.windowPosition + new Vector(30, 30 + 25 * i), DepthID.Status);
            }
            bf.Draw(d, backGround.windowPosition + new Vector(25, 30 + 25 * select), DepthID.Effect);
            d.Draw(backGround.windowPosition + new Vector(316, 40), select < 9 ? DataBase.myUnit_tex[select] : DataBase.enemyUnit_tex[select - 9], DepthID.Status);
            new TextAndFont(select < 9 ? DataBase.MyUnitName[select] : DataBase.EnemyUnitName[select - 9], Color.Black).Draw(d, backGround.windowPosition + new Vector(380 - (select < 9 ? DataBase.MyUnitName[select] : DataBase.EnemyUnitName[select - 9]).Length * 10, 190), DepthID.Status);
            new TextAndFont("戦闘力", Color.Black).Draw(d, backGround.windowPosition + new Vector(520, 50), DepthID.Status);
            new TextAndFont("移動力", Color.Black).Draw(d, backGround.windowPosition + new Vector(520, 80), DepthID.Status);
            new TextAndFont("HP", Color.Black).Draw(d, backGround.windowPosition + new Vector(520, 110), DepthID.Status);
            new TextAndFont("LP", Color.Black).Draw(d, backGround.windowPosition + new Vector(520, 140), DepthID.Status);
            new TextAndFont(string.Format(": {0}", select < 9 ? DataBase.MyUnitStrength[select] : DataBase.EnemyUnitStrength[select - 9]), Color.Black).Draw(d, backGround.windowPosition + new Vector(640, 50), DepthID.Status);
            new TextAndFont(string.Format(": {0}", select < 9 ? DataBase.MyUnitMoveRange[select] : DataBase.EnemyUnitMoveRange[select - 9]), Color.Black).Draw(d, backGround.windowPosition + new Vector(640, 80), DepthID.Status);
            new TextAndFont(string.Format(": {0}", select < 9 ? DataBase.MyUnitMAX_HP[select] : DataBase.EnemyUnitMAX_HP[select - 9]), Color.Black).Draw(d, backGround.windowPosition + new Vector(640, 110), DepthID.Status);
            new TextAndFont(string.Format(": {0}", select < 9 ? DataBase.MyUnitMAX_LP[select] : DataBase.EnemyUnitMAX_LP[select - 9]), Color.Black).Draw(d, backGround.windowPosition + new Vector(640, 140), DepthID.Status);
            new TextAndFont(string.Format(": {0}", DataBase.maxProductPower[select]), Color.Black).Draw(d, backGround.windowPosition + new Vector(640, 170), DepthID.Status);
            new TextAndFont(string.Format("必要生産力\n生産まで最短 {0} ターン", (DataBase.sumProductPower[select] + DataBase.maxProductPower[select] - 1) / DataBase.maxProductPower[select]), Color.Black).Draw(d, backGround.windowPosition + new Vector(520, 170), DepthID.Status);
            new TextAndFont(select < 9 ? DataBase.MyUnitExpl[select] : DataBase.EnemyUnitExpl[select - 9], Color.Black).Draw(d, backGround.windowPosition + new Vector(290, 280), DepthID.Status);

            cancel.Draw(d);
            start.Draw(d);
        }
        public override void SceneUpdate()
        {
            MouseState state = Mouse.GetState();
            if (pstate.LeftButton == ButtonState.Released && state.LeftButton == ButtonState.Pressed)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (state.X >= backGround.windowPosition.X + 30 && state.X <= backGround.windowPosition.X + 260 && state.Y >= backGround.windowPosition.Y + 30 + 25 * i && state.Y <= backGround.windowPosition.Y + 55 + 25 * i)
                    {
                        select = i;
                    }
                }
            }
            start.Update(pstate, state, productable[select]);
            start.ChangeColor(productable[select] ? new Color(255, 162, 0) : Color.White);
            if (start.Clicked() && productable[select])
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

            for (int i = 0; i < 8; i++)
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
