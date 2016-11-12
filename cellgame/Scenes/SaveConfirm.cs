using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CommonPart
{
    class SaveConfirm : Scene
    {
        #region Variable
        // 背景の枠
        WindowBar backGround;

        MouseState pstate;

        Button ok;
        Button cancel;
        EditorScene es;
        int num;
        #endregion

        #region Method
        public SaveConfirm(SceneManager s, EditorScene _es, int _num) : base(s)
        {
            s.BackSceneNumber++;
            backGround = new WindowBar(new Vector(352, 160), 40, 15);
            pstate = Mouse.GetState();
            es = _es;
            num = _num;

            ok = new Button(backGround.windowPosition + new Vector(315, 160), 120, new Color(255, 162, 0), new Color(200, 120, 0), "はい");
            cancel = new Button(backGround.windowPosition + new Vector(465, 160), 120, new Color(255, 162, 0), new Color(200, 120, 0), "いいえ");
        }
        public override void SceneDraw(Drawing d)
        {
            backGround.Draw(d);
            base.Draw(d);

            new TextAndFont(string.Format("このマップをユーザーマップ{0}に保存しますか？", num), Color.Black).Draw(d, backGround.windowPosition + new Vector(60, 40), DepthID.Message);

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
                es.SaveMap(num);
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
