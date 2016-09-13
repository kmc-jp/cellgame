using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using System.Reflection;
using MyUpdaterLib;


namespace CommonPart {
    class TitleScene: MenuScene {
        enum TitleIndex {
            Start, Editor, Load, Config, Save, Quit
        }
        static readonly string[] choiceDefault = new[] { "ニューゲーム", "マップエディター", "ロード", "オプション", "記録", "ゲーム終了" };
        readonly string[] choice = (string[])choiceDefault.Clone();
        bool[] enabled = new bool[] { true, true, false, true, false, true };
        Color[] defaultColor = new Color[] { Color.White, Color.White, Color.White, Color.White, Color.Gold, Color.White };
        Animation cursor = TalkWindow.GetCursorAnimation();
        
        public TitleScene(SceneManager s) : base(s, choiceDefault.Length) {

            Focused();
            
            enabled[(int)TitleIndex.Load] = Function.GetEnumLength<BGMID>() > 1;
        }
        public override void Deleted() {
        }
        public override void SceneUpdate() {
            cursor.Update();
            SoundManager.Music.PlayBGM(BGMID.None, true);
            base.SceneUpdate();
        }
        protected override void Choosed(int i) {
            if(!enabled[i]) return;
            switch((TitleIndex)i) {
                case TitleIndex.Start:
                    new PlayScene(scenem);
                    break;
                case TitleIndex.Editor:
                    new EditorScene(scenem);
                    break;
                case TitleIndex.Load:
                    new SoundTest(scenem);
                    break;
                case TitleIndex.Config:
                    new SettingsScene(scenem);
                    break;
                case TitleIndex.Save:
                    break;
                case TitleIndex.Quit:
                    Delete = true;
                    break;
            }
        }
        public override void Focused() {
            JoyPadManager.GetPad();
            JoyPadManager.Update();
        }
        public override void SceneDraw(Drawing d) {
            if(scenem.IsTopScene(this) && Settings.WindowStyleOld != Settings.WindowStyle) d.DrawStyle = Settings.WindowStyle;

            Vector2 basePos = new Vector2(218, 234);
            TalkWindow.DrawMessageBack(d, new Vector2(274, 28 + MaxIndex * 25), basePos, DepthID.Message);
            for(int i = 0; i < MaxIndex; i++) {
                new RichText(choice[i], FontID.Medium, enabled[i] ? defaultColor[i] : Color.Gray).Draw(d, basePos + new Vector(34, 10 + i * 26), DepthID.Message);
            }
            cursor.Draw(d, basePos + new Vector2(12, 16 + Index * 26), DepthID.Message);
            
        }
    }
}
