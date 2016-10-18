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
        enum parameter
        {
            map, difficulty, AI, lymph, wound, life
        }
        readonly string[] paraName = { "マップ", "難易度", "AI", "リンパへクス", "傷口へクス", "ユニットの寿命" };
        readonly string[,] paraStr = { { "マップ１", "マップ2", "マップ３" }, { "普通", "", "" }, { "普通", "", "" }, { "通常", "多め", "少なめ" }, { "通常", "多め", "少なめ" }, { "あり", "なし", "" } };
        readonly int[] paraMaxIndex = { 3, 1, 1, 1, 1, 1 };
        int[] paraIndex = { 0, 0, 0, 0, 0, 0 };
        readonly Vector2[] paraPos = { new Vector2(250, 200), new Vector2(250, 400), new Vector2(600, 400), new Vector2(250, 460), new Vector2(600, 460), new Vector2(250, 520) };
        List<Button> paraButton;
        Button start, cancel;
        MouseState pstate;
        #endregion

        #region Method
        public NewGameScene(SceneManager s)
            : base(s) {
            paraButton = new List<Button>();
            for(int i = 0; i < paraName.Length;i++)
                paraButton.Add(new Button(paraPos[i], 160, new Color(255, 162, 0), new Color(200, 120, 0), paraName[i]));

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
                new PlayScene(scenem, paraIndex[0]);
            }

            for(int i = 0; i < paraButton.Count; i++)
            {
                if (paraButton[i].Clicked())
                {
                    paraIndex[i]++;
                    if (paraIndex[i] == paraMaxIndex[i])
                        paraIndex[i] = 0;
                }
            }

            MouseState state = Mouse.GetState();
            for (int i = 0; i < paraButton.Count; i++) paraButton[i].Update(pstate, state);
            start.Update(pstate, state);
            cancel.Update(pstate, state);

            bool on = false;
            if (start.IsOn(state) || cancel.IsOn(state))
                on = true;
            for(int i = 0; i < paraButton.Count; i++)
            {
                if (paraButton[i].IsOn(state))
                    on = true;
            }

            if(on)
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Hand;

            pstate = state;
            base.SceneUpdate();
        }
        public override void SceneDraw(Drawing d)
        {
            new TextAndFont("ニューゲームオプション", Color.Black).Draw(d, new Vector2(250, 120), DepthID.Message);
            new TextAndFont("ニューゲームオプション", Color.Black).Draw(d, new Vector2(250, 120), DepthID.Message);

            for(int i = 0;i < paraButton.Count; i++)
            {
                new TextAndFont(paraStr[i, paraIndex[i]], Color.Black).Draw(d, paraPos[i] + new Vector2(180, 14), DepthID.Message);
            }

            for (int i = 0; i < paraButton.Count; i++) paraButton[i].Draw(d);
            start.Draw(d);
            cancel.Draw(d);
            base.SceneDraw(d);
        }
        #endregion
    }
}
