using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace CommonPart {
    /// <summary>
    /// ゲーム開始後の処理を書いたクラス
    /// </summary>
    class MapScene : Scene {
        #region Variable
        // ボックスウィンドウ（ユニットボックスとか）のリスト
        List<WindowBox> bars;
        Vector Camera { get { return _camera; } }
        double CameraX {
            get { return _camera.X; }
            set { _camera.X = Math.Max(-Game1._WindowSizeX / 2 / DataBase.MapScale[Scale], Math.Min(DataBase.HexWidth * DataBase.MAP_MAX - Game1._WindowSizeX / 2 / DataBase.MapScale[Scale], value)); }
        }
        double CameraY
        {
            get { return _camera.Y; }
            set { _camera.Y = Math.Max(-Game1._WindowSizeY / 2 / DataBase.MapScale[Scale], Math.Min(DataBase.HexHeight * 3 / 4 * DataBase.MAP_MAX - Game1._WindowSizeY / 2 / DataBase.MapScale[Scale], value)); }
        }
        Vector _camera = new Vector(DataBase.HexWidth * DataBase.MAP_MAX / 2 - Game1._WindowSizeX / 2, DataBase.HexHeight * DataBase.MAP_MAX / 2 - Game1._WindowSizeY / 2);
        Map nMap;
        // カメラの移動速度
        int defcameraVel = 15;
        // カメラの倍率
        int _scale = DataBase.DefaultMapScale;
        int Scale {
            get { return _scale; }
            set {
                if (value < 0) _scale = 0;
                else if (value >= DataBase.MapScale.Length) _scale = DataBase.MapScale.Length - 1;
                else _scale = value;
            }
        }
        // 直前のマウスの状態
        MouseState pstate;
        // ゲーム内変数
        int studypoint = 0;
        int productpoint = 0;
        int leftunit = 0;
        decimal bodytemp = 36;


        #endregion
        #region Method
        public MapScene(SceneManager s)
            : base(s) {
            pstate = Mouse.GetState();
            nMap = new Map();
            bars = new List<WindowBox>();
            for(int i = 0; i < DataBase.BarIndexNum; i++) {
                bars.Add(new WindowBox(DataBase.BarPos[i],DataBase.BarWidth[i],DataBase.BarHeight[i]));
            }
        }

        /// <summary>
        /// ゲーム画面の描画メソッド
        /// </summary>
        /// <param name="d"></param>
        public override void SceneDraw(Drawing d) {
            // マップの描画
            nMap.Draw(d, Camera, Scale);
            // それぞれのバーの描画
            for (int i = 0; i < DataBase.BarIndexNum; i++) {
                switch ((DataBase.BarIndex)i) {
                    case DataBase.BarIndex.Study:
                        bars[i].Draw(d);
                        break;
                    case DataBase.BarIndex.Unit:
                        bars[i].Draw(d);
                        break;
                    case DataBase.BarIndex.Minimap:
                        if (Settings.WindowStyle == 1 && bars[i].windowPosition.Y != DataBase.BarPos[i].Y)
                            bars[i].windowPosition = DataBase.BarPos[i];
                        else if(Settings.WindowStyle == 0 && bars[i].windowPosition.Y == DataBase.BarPos[i].Y)
                            bars[i].windowPosition = new Vector(DataBase.BarPos[i].X, DataBase.BarPos[i].Y - (DataBase.WindowDefaultSizeY - DataBase.WindowSlimSizeY));
                        bars[i].Draw(d);
                        break;
                    case DataBase.BarIndex.Status:
                        bars[i].Draw(d);
                        break;
                    case DataBase.BarIndex.Arrange:
                        bars[i].Draw(d);
                        break;
                }
            }
        }
        public override void SceneUpdate() {
            base.SceneUpdate();
            MouseState state = Mouse.GetState();
            if (state.X <= 0)
                CameraX -= defcameraVel / DataBase.MapScale[Scale];
            if (state.X >= Game1._WindowSizeX)
                CameraX += defcameraVel / DataBase.MapScale[Scale];
            if (state.Y <= 0)
                CameraY -= defcameraVel / DataBase.MapScale[Scale];
            if (state.Y >= Game1._WindowSizeY)
                CameraY += defcameraVel / DataBase.MapScale[Scale];
            if (state.ScrollWheelValue > pstate.ScrollWheelValue)
                Scale++;
            else if (state.ScrollWheelValue < pstate.ScrollWheelValue)
                Scale--;
            pstate = state;

            // Zキーが押されると終了
            if (Input.GetKeyPressed(KeyID.Select)) Delete = true;
        }
        #endregion
    }// class end
}// namespace end
