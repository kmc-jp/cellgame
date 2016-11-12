using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CommonPart
{
    class EndConfirm : Scene
    {
        #region Variable
        // 背景の枠
        WindowBar backGround;

        MouseState pstate;

        Button ok;
        Button cancel;
        EditorScene es;
        #endregion

        #region Method
        public EndConfirm(SceneManager s, EditorScene _es) : base(s)
        {
            s.BackSceneNumber++;
            backGround = new WindowBar(new Vector(352, 160), 40, 15);
            pstate = Mouse.GetState();
            es = _es;

            ok = new Button(backGround.windowPosition + new Vector(315, 160), 120, new Color(255, 162, 0), new Color(200, 120, 0), "はい");
            cancel = new Button(backGround.windowPosition + new Vector(465, 160), 120, new Color(255, 162, 0), new Color(200, 120, 0), "いいえ");
        }
        public override void SceneDraw(Drawing d)
        {
            backGround.Draw(d);
            base.Draw(d);

            new TextAndFont("終了しますか？", Color.Black).Draw(d, backGround.windowPosition + new Vector(60, 40), DepthID.Message);

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
                es.Delete = true;
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
