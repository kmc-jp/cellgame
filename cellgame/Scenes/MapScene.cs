using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using System.IO;

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

        // WhichHexの返り値用の構造体
        public struct PAIR　{
            public int i, j;
            public PAIR(int _i, int _j) { i = _i; j = _j; }
        }


        #endregion
        #region Method
        // コンストラクタ
        public MapScene(SceneManager s)
            : base(s) {
            pstate = Mouse.GetState();
            nMap = new Map();
            bars = new List<WindowBox>();
            for(int i = 0; i < DataBase.BarIndexNum; i++) {
                bars.Add(new WindowBox(DataBase.BarPos[i],DataBase.BarWidth[i],DataBase.BarHeight[i]));
            }
        }

        // 画面上の座標(x, y)がどのへクスの上にあるか どのへクスの上にもなければ(0, -1)を返す バーの上にある場合は(-1, 0)を返す
        public PAIR WhichHex(int x, int y)
        {
            foreach (WindowBox bar in bars)
                if (bar.IsOn(x, y))
                    return new PAIR(-1, 0);

            double X = CameraX + x / DataBase.MapScale[Scale], Y = CameraY + y / DataBase.MapScale[Scale];
            for (int i = 0; i < DataBase.MAP_MAX; i++)
            {
                for (int j = 0; j < DataBase.MAP_MAX; j++)
                {
                    double dx = X - DataBase.HexWidth * i, dy = Y - DataBase.HexHeight * 3 / 4 * j;
                    if (j % 2 == 1)
                        dx -= DataBase.HexWidth / 2;

                    if (dx >= 0 && dx <= DataBase.HexWidth &&
                        dy + dx * DataBase.HexHeight / DataBase.HexWidth / 2 >= DataBase.HexHeight / 4 &&
                        dy + dx * DataBase.HexHeight / DataBase.HexWidth / 2 <= DataBase.HexHeight / 4 * 5 &&
                        dy + DataBase.HexHeight / 4 >= dx * DataBase.HexHeight / DataBase.HexWidth / 2 &&
                        dy <= DataBase.HexHeight / 4 * 3 + dx * DataBase.HexHeight / DataBase.HexWidth / 2)
                    {
                        return new PAIR(i, j);
                    }
                }
            }
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
            // マウスカーソルがウィンドウの外に出たときにカメラがその方向へ移動
            if (state.X <= 0)                   CameraX -= defcameraVel / DataBase.MapScale[Scale];
            if (state.X >= Game1._WindowSizeX)  CameraX += defcameraVel / DataBase.MapScale[Scale];
            if (state.Y <= 0)                   CameraY -= defcameraVel / DataBase.MapScale[Scale];
            if (state.Y >= Game1._WindowSizeY)  CameraY += defcameraVel / DataBase.MapScale[Scale];

            // マウススクロールするとマップの描画倍率が変化
            if (state.ScrollWheelValue > pstate.ScrollWheelValue)       Scale++;
            else if (state.ScrollWheelValue < pstate.ScrollWheelValue)  Scale--;

            // 左クリックされたときにその座標がウィンドウ上であり、バーの上でなくかつどれかのへクスの上であればそのへクスの値をインクリメント
            if (pstate.LeftButton != ButtonState.Pressed && state.LeftButton == ButtonState.Pressed &&
                state.X >= 0 && state.X <= Game1._WindowSizeX && state.Y >= 0 && state.Y <= Game1._WindowSizeY)
            {
                PAIR p = WhichHex(state.X, state.Y);
                if (p.i >= 0 && p.j >= 0) nMap.ChangeState(p.i, p.j, (nMap.GetState(p.i, p.j) + 1) % DataBase.hex.Count);
            }
            // 右クリックされたときにその座標がウィンドウ上であり、バーの上でなくかつどれかのへクスの上であればそのへクスの値をデクリメント
            if (pstate.RightButton != ButtonState.Pressed && state.RightButton == ButtonState.Pressed &&
                state.X >= 0 && state.X <= Game1._WindowSizeX && state.Y >= 0 && state.Y <= Game1._WindowSizeY)
            {
                PAIR p = WhichHex(state.X, state.Y);
                if (p.i >= 0 && p.j >= 0)　nMap.ChangeState(p.i, p.j, (nMap.GetState(p.i, p.j) + DataBase.hex.Count - 1) % DataBase.hex.Count);
            }

            pstate = state;
            // Sキーが押されるとマップデータの保存
            if (Keyboard.GetState().IsKeyDown(Keys.S))　SaveMap();

            // Rキーが押されるとマップデータの読み込み
            if (Keyboard.GetState().IsKeyDown(Keys.R))　ReadMap();

            // Zキーが押されると終了
            if (Input.GetKeyPressed(KeyID.Select))　Delete = true;
        }
        #endregion
    }// class end
}// namespace end
