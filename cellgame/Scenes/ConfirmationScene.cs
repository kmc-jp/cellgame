using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CommonPart
{
    class ConfirmationScene : Scene
    {
        #region Variable
        // 背景の枠
        WindowBar backGround;

        MouseState pstate;

        bool f1, f2, f3;
        Button ok;
        Button cancel;
        #endregion

        #region Method
        public ConfirmationScene(SceneManager s, bool _f1, bool _f2, bool _f3) : base(s)
        {
            s.BackSceneNumber++;
            backGround = new WindowBar(new Vector(352, 160), 40, 15);
            pstate = Mouse.GetState();
            f1 = _f1;
            f2 = _f2;
            f3 = _f3;

            ok = new Button(backGround.windowPosition + new Vector(315, 160), 120, new Color(255, 162, 0), new Color(200, 120, 0), "進める");
            cancel = new Button(backGround.windowPosition + new Vector(465, 160), 120, new Color(255, 162, 0), new Color(200, 120, 0), "キャンセル");
        }
        public override void SceneDraw(Drawing d)
        {
            backGround.Draw(d);
            base.Draw(d);

            new TextAndFont("ターンを進めますか？", Color.Black).Draw(d, backGround.windowPosition + new Vector(40, 60), DepthID.Message);
            if (f1) new TextAndFont("※コマンドの入力を終えていないユニットがあります。", Color.Black).Draw(d, backGround.windowPosition + new Vector(40, 80), DepthID.Message);
            if (f2) new TextAndFont("※研究項目が指定されていません。", Color.Black).Draw(d, backGround.windowPosition + new Vector(40, 100), DepthID.Message);
            if (f3) new TextAndFont("※生産力が余っています。", Color.Black).Draw(d, backGround.windowPosition + new Vector(40, f2 ? 120 : 100), DepthID.Message);

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
                PlayScene.changeTurn = true;
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
