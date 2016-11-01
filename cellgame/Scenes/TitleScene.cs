using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Reflection;
using MyUpdaterLib;


namespace CommonPart {
    class TitleScene: Scene {
        enum TitleIndex {
            Start, Editor, Load, Config, Save, Quit
        }
        static readonly string[] choiceDefault = new[] { "ニューゲーム", "マップエディター", "ロード", "オプション", "記録", "ゲーム終了" };
        readonly string[] choice = (string[])choiceDefault.Clone();
        List<Button> button;
        MouseState pstate;

        public TitleScene(SceneManager s) : base(s) {
            button = new List<Button>();
            for(int i = 0; i < choice.Length; i++)
            {
                button.Add(new Button(new Vector2(960, 300 + i * 60), 200, new Color(255, 162,0), new Color(200, 120, 0), choice[i]));
            }
        }
        public override void SceneUpdate() {
            if (button[(int)TitleIndex.Start].Clicked())
            {
                new NewGameScene(scenem);
            }
            else if (button[(int)TitleIndex.Editor].Clicked())
            {
                new EditorScene(scenem);
            }
            else if (button[(int)TitleIndex.Load].Clicked())
            {

            }
            else if (button[(int)TitleIndex.Config].Clicked())
            {
                new SettingsScene(scenem);
            }
            else if (button[(int)TitleIndex.Save].Clicked())
            {

            }
            else if (button[(int)TitleIndex.Quit].Clicked())
            {
                Delete = true;
            }
            MouseState state = Mouse.GetState();
            for(int i = 0; i < button.Count; i++)
            {
                button[i].Update(pstate, state);
            }
            
            bool on = false;
            for (int i = 0; i < button.Count; i++)
            {
                if (button[i].IsOn(state))
                    on = true;
            }

            if (on)
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Hand;

            pstate = state;

            if (SoundManager.Music.GetPlayingID != BGMID.Title) SoundManager.Music.PlayBGM(BGMID.Title, true);
            base.SceneUpdate();
        }
        public override void SceneDraw(Drawing d) {
            for(int i = 0;i < button.Count; i++)
            {
                button[i].Draw(d);
            }
        }
    }
}
