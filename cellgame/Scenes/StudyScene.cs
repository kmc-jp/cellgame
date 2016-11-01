using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CommonPart
{
    class StudyScene : Scene
    {
        #region Variable
        // 背景の枠
        WindowBar backGround;
        // 必要なPlayScene内の変数
        StudyBar sb;

        MouseState pstate;
        
        Button start;
        Button cancel;
        #endregion

        #region Method
        public StudyScene(SceneManager s, StudyBar sb_) : base(s)
        {
            s.BackSceneNumber++;
            backGround = new WindowBar(new Vector(352, 160), 40, 25);
            sb = sb_;
            pstate = Mouse.GetState();

            start = new Button(backGround.windowPosition + new Vector(315, 320), 120, new Color(255, 162, 0), new Color(200, 120, 0), "研究開始");
            cancel = new Button(backGround.windowPosition + new Vector(465, 320), 120, new Color(255, 162, 0), new Color(200, 120, 0), "キャンセル");
        }
        public override void SceneDraw(Drawing d)
        {
            backGround.Draw(d);
            base.Draw(d);

            cancel.Draw(d);
            start.Draw(d);
        }
        public override void SceneUpdate()
        {
            MouseState state = Mouse.GetState();
            start.Update(pstate, state);
            cancel.Update(pstate, state);

            if (start.Clicked())
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
