using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.IO;

namespace CommonPart {
    /// <summary>
    /// ゲーム開始後の処理を書いたクラス
    /// </summary>
    class PlayScene : Scene {
        #region Variable
        // ボックスウィンドウ（ユニットボックスとか）
        ProductArrangeBar proarrBar;
        MinimapBox minimapBox;
        StatusBar statusBar;
        StudyBar studyBar;
        UnitBox unitBox;
        Button next;
        // カメラ
        Vector Camera { get { return _camera; } }
        double CameraX
        {
            get { return _camera.X; }
            set { _camera.X = Math.Max(-DataBase.HexWidth * 4, Math.Min(DataBase.HexWidth * DataBase.MAP_MAX - Game1._WindowSizeX / DataBase.MapScale[Scale] + DataBase.HexWidth * 4, value)); }
        }
        double CameraY
        {
            get { return _camera.Y; }
            set { _camera.Y = Math.Max(-DataBase.HexHeight * 3 , Math.Min(DataBase.HexHeight * 3 / 4 * DataBase.MAP_MAX - Game1._WindowSizeY / DataBase.MapScale[Scale] + DataBase.HexHeight * 3 , value)); }
        }
        Vector _camera = new Vector(DataBase.HexWidth * DataBase.MAP_MAX / 2 - Game1._WindowSizeX / 2, DataBase.HexHeight * DataBase.MAP_MAX / 2 - Game1._WindowSizeY / 2);
        // 現在のマップ
        Map nMap;
        // ユニットマネージャ
        UnitManager um;
        // カメラの移動速度
        int defcameraVel = DataBase.cameraV;
        // カメラの倍率
        int _scale = DataBase.DefaultMapScale;
        int Scale {
            get { return _scale; }
            set {
                if (value < 1) _scale = 1;
                else if (value >= DataBase.MapScale.Length) _scale = DataBase.MapScale.Length - 1;
                else _scale = value;
            }
        }
        // 直前のマウスの状態
        MouseState pstate;
        // ゲーム内変数
        public static int studyPower;
        public static int productPower;
        public static int maxProductPower;
        public static decimal bodyTemp;

        AI ai;

        
        #endregion

        #region Method
        // コンストラクタ
        public PlayScene(SceneManager s, int map_n)
            : base(s) {
            pstate = Mouse.GetState();
            nMap = new Map();
            studyBar = new StudyBar();
            unitBox = new UnitBox();
            minimapBox = new MinimapBox();
            statusBar = new StatusBar();
            proarrBar = new ProductArrangeBar();
            um = new UnitManager(ref unitBox);
            ReadMap(map_n + 1);
            next = new Button(new Vector(1160, 912), 120, Color.White, Color.White, "　次のターンへ");
            ai = new AI(ref nMap, ref um);


            
            studyPower = DataBase.DefaultStudyPower;
            productPower = maxProductPower = DataBase.DefaultProductPower;
            bodyTemp = 36.0m;
        }

        // 画面上の座標(x, y)がどのへクスの上にあるか どのへクスの上にもなければ(0, -1)を返す バーの上にある場合は(-1, 0)を返す
        public PAIR WhichHex(int x, int y)
        {
            if(studyBar.IsOn(x,y) || 
                unitBox.IsOn(x, y) ||
                minimapBox.IsOn(x, y) ||
                statusBar.IsOn(x, y) ||
                proarrBar.IsOn(x, y)) return new PAIR(-1, 0);
            
            for (int i = 0; i < DataBase.MAP_MAX; i++)
                for (int j = 0; j < DataBase.MAP_MAX; j++)
                    if (DataBase.IsOnHex(i, j, x, y, Camera, Scale))
                        return new PAIR(i, j);

            return new PAIR(0, -1);
        }
        
        // マップデータを実行可能ファイルのあるフォルダから見て /MapData/MapData.csv から読み込む（ファイルがなければ何もしない）
        public void ReadMap(int n)
        {
            if (File.Exists(string.Format(@"MapData\MapData{0}.csv",n)))
            {

                using (StreamReader r = new StreamReader(string.Format(@"MapData\MapData{0}.csv", n)))
                {
                    string line;
                    for (int i = 0; (line = r.ReadLine()) != null && i < DataBase.MAP_MAX; i++) // 1行ずつ読み出し。
                    {
                        string[] ss = line.Split(',');
                        for (int j = 0; j < ss.Length && j < DataBase.MAP_MAX; j++)
                        {
                            nMap.ChangeState(j, i, int.Parse(ss[j]));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ゲーム画面の描画メソッド
        /// </summary>
        /// <param name="d"></param>
        public override void SceneDraw(Drawing d) {
            // マップの描画
            nMap.Draw(d, Camera, Scale);
            // ユニットの描画
            um.Draw(d, Camera, Scale);
            // それぞれのバーの描画
            studyBar.Draw(d);
            minimapBox.Draw(d, um, nMap, Camera, Scale);
            statusBar.Draw(d);
            proarrBar.Draw(d);
            unitBox.Draw(d);
            next.Draw(d);
        }
        public override void SceneUpdate() {
            // 現在のマウスの状態を取得
            MouseState state = Mouse.GetState();
            // マウスカーソルがウィンドウの外に出たときにカメラがその方向へ移動
            if (state.X <= 0)                   CameraX -= defcameraVel / DataBase.MapScale[Scale];
            if (state.X >= Game1._WindowSizeX)  CameraX += defcameraVel / DataBase.MapScale[Scale];
            if (state.Y <= 0)                   CameraY -= defcameraVel / DataBase.MapScale[Scale];
            if (state.Y >= Game1._WindowSizeY)  CameraY += defcameraVel / DataBase.MapScale[Scale];

            // マウススクロールするとマップの描画倍率が変化
            int ps = Scale;
            if (state.ScrollWheelValue > pstate.ScrollWheelValue)       Scale++;
            else if (state.ScrollWheelValue < pstate.ScrollWheelValue)  Scale--;
            CameraX = CameraX + Game1._WindowSizeX / DataBase.MapScale[ps] / 2 - Game1._WindowSizeX / DataBase.MapScale[Scale] / 2;
            CameraY = CameraY + Game1._WindowSizeY / DataBase.MapScale[ps] / 2 - Game1._WindowSizeY / DataBase.MapScale[Scale] / 2;

            // 敵のターンの時、マウスの状態を更新しない
            if (ai.turn)
                state = pstate;

            // バー・ボックスの更新
            studyBar.Update(pstate, state, scenem);
            unitBox.Update();
            minimapBox.Update();
            statusBar.Update();
            proarrBar.Update(pstate, state, um, this, ref nMap, scenem);

            // ボタンの更新
            next.Update(pstate, state);

            // ユニットの更新
            um.Update(pstate, state, ref nMap, this);

            // 次のユニットへフェードイン
            if (um.phase)
            {
                if(um.select_i >= 0 && um.select_j >= 0)
                    FadeIn(um.select_i - (um.select_j + 1) / 2, um.select_j);
                um.phase = false;
            }

            // カーソルの形状を変化
            if (!ai.turn && (unitBox.IsOnButton(state.X, state.Y) || proarrBar.IsOnButton(state.X, state.Y) || minimapBox.IsOnButton(state.X, state.Y) || next.IsOn(state)))
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Hand;

            // 次のターンへボタンを押されるとターンを進める
            if (next.Clicked())
            {
                um.turn++;
                studyBar.UpdateTurn();
                ai.turn = true;
            }
            
            // AIの更新
            ai.Update();

            // Zキーが押されると終了
            if (Input.GetKeyPressed(KeyID.Select))　Delete = true;

            pstate = state;
            base.SceneUpdate();
        }
        public void FadeIn(int x_index, int y_index)
        {
            CameraX = (DataBase.HexWidth * x_index + DataBase.HexWidth / 2 * ((y_index % 2) + 1)) - Game1._WindowSizeX / 2 / DataBase.MapScale[Scale];
            CameraY = (DataBase.HexHeight * 3 / 4 * y_index + DataBase.HexHeight / 2) - Game1._WindowSizeY / 2 / DataBase.MapScale[Scale];
        }
        #endregion
    }// class end
}// namespace end
