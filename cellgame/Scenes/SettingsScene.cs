using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CommonPart {
    static class Settings {
        public static int BGMVolume { get { return SoundManager.Music.Volume / 10; } set { SoundManager.Music.Volume = value * 10; } }
        public static int SEVolume { get { return SoundManager.SE.Volume; } set { SoundManager.SE.Volume = value; } }

        public static int WindowR, WindowG, WindowB, WindowA;
        static Settings() { SetDefaultColor(); }

        public static void SetDefaultColor() {
            WindowR = 16;
            WindowG = 48;
            WindowB = 96;
            WindowA = 224;
        }

        public static int WindowStyle = 1;
        //これは保存しなくてよい
        public static int WindowStyleOld = 1;
        
    }
    class SettingsScene : MenuScene {
        readonly static string[] messages;
        readonly static int length;
        static SettingsScene() {
            messages = new string[length = Enum.GetNames(typeof(SettingID)).Length];
            messages[(int)SettingID.BGMVolume] = "BGM音量";
            messages[(int)SettingID.SEVolume] = "SE音量";
            messages[(int)SettingID.WindowSize] = "ウィンドウの大きさ";
            messages[(int)SettingID.KeyConfig] = "キー設定";
            messages[(int)SettingID.Close] = "閉じる";
        }
        Animation cursor = TalkWindow.GetCursorAnimation();
        bool _delete;
        public SettingsScene(SceneManager s) : base(s, length) { s.BackSceneNumber++; Settings.WindowStyleOld = Settings.WindowStyle; }
        public override void SceneUpdate() {
            cursor.Update();
            base.SceneUpdate();
            switch((SettingID)Index) {
                case SettingID.BGMVolume:
                    if(Input.IsPressedForMenu(KeyID.Left, 15, 2)) Settings.BGMVolume--;
                    else if(Input.IsPressedForMenu(KeyID.Right, 15, 2)) Settings.BGMVolume++;
                    break;
                case SettingID.SEVolume:
                    if(Input.IsPressedForMenu(KeyID.Left, 15, 2)) Settings.SEVolume--;
                    else if(Input.IsPressedForMenu(KeyID.Right, 15, 2)) Settings.SEVolume++;
                    break;
                case SettingID.WindowSize:
                    if(Input.IsPressedForMenu(KeyID.Left, 30, 15) || Input.IsPressedForMenu(KeyID.Right, 30, 15))
                        Settings.WindowStyle = 1 - Settings.WindowStyle;
                    break;
            }
            if(Input.GetKeyPressed(KeyID.Cancel)) {
                SoundManager.PlaySE(SoundEffectID.Cursor_Cancel);
                Close();
            }
        }
        void Close() {
            _delete = true;
            //Save.SaveConfig();
        }
        protected override void Choosed(int i) {
            switch((SettingID)i) {
                case SettingID.KeyConfig: new KeyConfig(scenem); break;
                case SettingID.Close: Close(); break;
            }
        }
        public override void SceneDraw(Drawing d) {
            if(_delete) { Delete = true; if (Settings.WindowStyle != Settings.WindowStyleOld) { d.DrawStyle = Settings.WindowStyle; Settings.WindowStyleOld = Settings.WindowStyle; } }
            TalkWindow.DrawMessageBack(d, new Vector2(500, 24 + length * 26), new Vector2(20, 20), DepthID.Message);
            for(int i = 0; i < length; i++) {
                Vector2 pos = new Vector2(52, 30 + 26 * i);
                new RichText(messages[i]).Draw(d, pos, DepthID.Message);
            }
            cursor.Draw(d, new Vector2(30, 34 + Index * 26), DepthID.Message);
            DrawMeter(d, new Vector2(240, 30 + 26 * (int)SettingID.BGMVolume), Settings.BGMVolume, Settings.BGMVolume.ToString(), 0, 100);
            DrawMeter(d, new Vector2(240, 30 + 26 * (int)SettingID.SEVolume), Settings.SEVolume, Settings.SEVolume.ToString(), 0, 100);

            string show;
            if(Settings.WindowStyle == 1)
                show = String.Format("{0}x{1} (4 : 3)", DataBase.WindowDefaultSizeX, DataBase.WindowDefaultSizeY);
            else
                show = String.Format("{0}x{1} (16 : 9)", DataBase.WindowDefaultSizeX, DataBase.WindowSlimSizeY);
            new RichText(show).Draw(d, new Vector2(240, 30 + 26 * (int)SettingID.WindowSize), DepthID.Message);
        }
        public static void DrawMeter(Drawing d, Vector2 pos, int value, string str, int min, int max) {
            //new SimpleTexture(TextureID.ConfigMeterBack).Draw(d, pos + new Vector2(0, 4), DepthID.Message);
            //new SimpleTexture(TextureID.ConfigMeter).Draw(d, pos + new Vector2(2 + (value - min) * 188 / (max - min), 0), DepthID.Message);
            new Gauge(new Vector2(200, 12), Color.Blue, min, max, value, Color.Black).Draw(d, pos + new Vector2(0, 6), DepthID.Message);
            new RichText(str).Draw(d, pos + new Vector2(220, 0), DepthID.Message);
        }
        enum SettingID {
            BGMVolume, SEVolume, WindowSize, KeyConfig, Close
        }
    }
}
