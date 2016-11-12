using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CommonPart
{
    class NewGameScene : Scene
    {
        #region Variable
        int select = 0;
        readonly Vector2[] paraPos = { new Vector2(300, 340), new Vector2(300, 400), new Vector2(300, 460), new Vector2(700, 340), new Vector2(700, 400), new Vector2(700, 460) };
        Button[] paraButton;
        Button start, cancel;
        MouseState pstate;
        #endregion

        #region Method
        public NewGameScene(SceneManager s)
            : base(s) {
            paraButton = new Button[6];
            for(int i = 0;i < 6; i++)
            {
                paraButton[i] = new Button(paraPos[i], 200, new Color(255, 155, 79), Color.Black, i < 3 ? string.Format("マップ{0}", i + 1) : string.Format("ユーザーマップ{0}", i - 2));
            }

            start = new Button(new Vector2(700, 600), 160, new Color(255, 155, 79), new Color(255, 111, 0), "ゲームスタート");
            cancel = new Button(new Vector2(900, 600), 160, new Color(255, 155, 79), new Color(255, 111, 0), "キャンセル");
            pstate = Mouse.GetState();
        }
        public override void SceneUpdate()
        {
            if (cancel.Clicked()) Delete = true;
            if (start.Clicked())
            {
                Delete = true;
                new PlayScene(scenem, select % 3, select >= 3);
            }

            for(int i = 0; i < paraButton.Length; i++)
            {
                if (paraButton[i].Clicked())
                {
                    select = i;
                }
            }

            MouseState state = Mouse.GetState();
            for (int i = 0; i < paraButton.Length; i++) paraButton[i].Update(pstate, state);
            start.Update(pstate, state);
            cancel.Update(pstate, state);
            

            pstate = state;
            base.SceneUpdate();
        }
        public override void SceneDraw(Drawing d)
        {
            new TextAndFont("ニューゲームオプション", Color.Black).Draw(d, new Vector2(250, 180), DepthID.Message);

            for(int i = 0;i < paraButton.Length; i++)
            {
                paraButton[i].ChangeBackColor(i == select ? Color.Red : Color.Black);
            }

            for (int i = 0; i < paraButton.Length; i++) paraButton[i].Draw(d);
            start.Draw(d);
            cancel.Draw(d);
            base.SceneDraw(d);
        }
        #endregion
    }
}
