using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CommonPart
{
    class SaveConfScene : Scene
    {
        #region Variable
        // 背景の枠
        WindowBar backGround;
        // 必要なPlayScene内の変数
        StudyBar sb;

        MouseState pstate;

        Button ok;
        Button cancel;
        PlayScene ps;
        #endregion

        #region Method
        public SaveConfScene(SceneManager s, PlayScene _ps) : base(s)
        {
            ps = _ps;
            s.BackSceneNumber++;
            backGround = new WindowBar(new Vector(352, 160), 40, 15);
            pstate = Mouse.GetState();

            ok = new Button(backGround.windowPosition + new Vector(315, 160), 120, new Color(255, 162, 0), new Color(200, 120, 0), "はい");
            cancel = new Button(backGround.windowPosition + new Vector(465, 160), 120, new Color(255, 162, 0), new Color(200, 120, 0), "いいえ");
        }
        public override void SceneDraw(Drawing d)
        {
            backGround.Draw(d);
            base.Draw(d);

            new TextAndFont("終了します。セーブしますか？", Color.Black).Draw(d, backGround.windowPosition + new Vector(40, 60), DepthID.Message);

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
                ps.SaveData();
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
