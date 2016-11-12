using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CommonPart
{
    class MapErrorScene : Scene
    {
        #region Variable
        // 背景の枠
        WindowBar backGround;
        // 必要なPlayScene内の変数
        StudyBar sb;

        MouseState pstate;

        Button ok;
        Button cancel;
        #endregion

        #region Method
        public MapErrorScene(SceneManager s) : base(s)
        {
            s.BackSceneNumber++;
            backGround = new WindowBar(new Vector(352, 160), 40, 20);
            pstate = Mouse.GetState();

            ok = new Button(backGround.windowPosition + new Vector(315, 240), 120, new Color(255, 162, 0), new Color(200, 120, 0), "　OK");
            cancel = new Button(backGround.windowPosition + new Vector(465, 240), 120, new Color(255, 162, 0), new Color(200, 120, 0), "キャンセル");
        }
        public override void SceneDraw(Drawing d)
        {
            backGround.Draw(d);
            base.Draw(d);
            string mes = "マップが保存できませんでした。\nマップは以下の条件を満たしていなければ保存できません。\n\n・敵ユニットが乗っていないリンパヘクスが存在する。\n・味方ユニットが乗っていない傷口へクスが存在する。\n・空のへクスの上にユニットが存在しない。";
            new TextAndFont(mes, Color.Black).Draw(d, backGround.windowPosition + new Vector(40, 20), DepthID.Message);

            cancel.Draw(d);
            ok.Draw(d);
        }
        public override void SceneUpdate()
        {
            MouseState state = Mouse.GetState();
            ok.Update(pstate, state);
            cancel.Update(pstate, state);

            if (ok.Clicked())
            {
                Delete = true;
            }
            if (cancel.Clicked())
            {
                Delete = true;
            }
            pstate = state;
        }
        #endregion
    }
}
