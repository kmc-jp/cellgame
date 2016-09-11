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
        // カメラ
        Vector Camera { get { return _camera; } }
        double CameraX
        {
            get { return _camera.X; }
            set { _camera.X = Math.Max(-Game1._WindowSizeX / 2 / DataBase.MapScale[Scale], Math.Min(DataBase.HexWidth * DataBase.MAP_MAX - Game1._WindowSizeX / 2 / DataBase.MapScale[Scale], value)); }
        }
        double CameraY
        {
            get { return _camera.Y; }
            set { _camera.Y = Math.Max(-Game1._WindowSizeY / 2 / DataBase.MapScale[Scale], Math.Min(DataBase.HexHeight * 3 / 4 * DataBase.MAP_MAX - Game1._WindowSizeY / 2 / DataBase.MapScale[Scale], value)); }
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
        int studyPower = 36;
        int studyPoint = 364;
        int maxStudyPoint = 514;
        int PP = 0;
        int maxPP = 25;
        int leftUnit = 19;
        decimal bodyTemp = 36.0m;

        
        #endregion

        #region Method
        // コンストラクタ
        public PlayScene(SceneManager s)
            : base(s) {
            pstate = Mouse.GetState();
            nMap = new Map();
            um = new UnitManager();
            studyBar = new StudyBar(maxStudyPoint, studyPoint, studyPower, "親和性成熟");
            unitBox = new UnitBox();
            minimapBox = new MinimapBox();
            statusBar = new StatusBar(studyPower, PP, maxPP, leftUnit, bodyTemp);
            proarrBar = new ProductArrangeBar();
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

        // マップデータを実行可能ファイルのあるフォルダから見て /MapData/MapData.csv に保存する（既に存在するときは上書き保存）
        public void SaveMap()
        {
            if (!Directory.Exists("MapData"))
                Directory.CreateDirectory("MapData");
            using (StreamWriter w = new StreamWriter(@"MapData\MapData.csv"))
            {
                for (int i = 0; i < DataBase.MAP_MAX; i++)
                {
                    w.Write("{0}", nMap.GetState(0, i));
                    for (int j = 1; j < DataBase.MAP_MAX; j++)
                    {
                        w.Write(",{0}", nMap.GetState(j, i));
                    }
                    w.Write("\r\n");
                }
            }
        }

        // マップデータを実行可能ファイルのあるフォルダから見て /MapData/MapData.csv から読み込む（/MapData ディレクトリがなければ何もしない）
        // ※ /MapData ディレクトリが存在して /MapData/MapData.csv ファイルが存在しなければ実行時にエラーが出るので注意
        public void ReadMap()
        {
            if (Directory.Exists("MapData"))
            {

                using (StreamReader r = new StreamReader(@"MapData\MapData.csv"))
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

            
            // バー・ボックスの更新
            studyBar.Update();
            unitBox.Update(um, nMap, this, pstate, state);
            minimapBox.Update();
            statusBar.Update(studyPower, PP, maxPP, leftUnit, bodyTemp);
            proarrBar.Update(pstate, state);

            // ユニットの更新
            um.Update(unitBox);

            // カーソルの形状を変化
            if (unitBox.IsOnButton(state.X, state.Y) || proarrBar.IsOnButton(state.X, state.Y))
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Hand;
            else
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Arrow;


            // Rキーが押されるとマップデータの読み込み
            if (Keyboard.GetState().IsKeyDown(Keys.R))　ReadMap();

            // Zキーが押されると終了
            if (Input.GetKeyPressed(KeyID.Select))　Delete = true;

            pstate = state;
            base.SceneUpdate();
        }
        #endregion
    }// class end
}// namespace end
