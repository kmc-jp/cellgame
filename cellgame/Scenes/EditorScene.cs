using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace CommonPart
{
    /// <summary>
    /// マップエディターの処理を書いたクラス
    /// </summary>
    class EditorScene : Scene
    {
        #region Variable
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
            set { _camera.Y = Math.Max(-DataBase.HexHeight * 3, Math.Min(DataBase.HexHeight * 3 / 4 * DataBase.MAP_MAX - Game1._WindowSizeY / DataBase.MapScale[Scale] + DataBase.HexHeight * 3, value)); }
        }
        Vector _camera = new Vector(DataBase.HexWidth * DataBase.MAP_MAX / 2 - Game1._WindowSizeX / 2, DataBase.HexHeight * DataBase.MAP_MAX / 2 - Game1._WindowSizeY / 2);
        // 現在のマップ
        Map nMap;
        // カメラの移動速度
        int defcameraVel = DataBase.cameraV;
        // カメラの倍率
        int _scale = DataBase.DefaultMapScale;
        int Scale
        {
            get { return _scale; }
            set
            {
                if (value < 1) _scale = 1;
                else if (value >= DataBase.MapScale.Length) _scale = DataBase.MapScale.Length - 1;
                else _scale = value;
            }
        }
        // 直前のマウスの状態
        MouseState pstate;

        // ユニットマップ
        UnitMap um;

        // WhichHexの返り値用の構造体
        public struct PAIR
        {
            public int i, j;
            public PAIR(int _i, int _j) { i = _i; j = _j; }
        }


        #endregion

        #region Method
        // コンストラクタ
        public EditorScene(SceneManager s)
            :base(s) {
            pstate = Mouse.GetState();
            nMap = new Map();
            um = new UnitMap();
        }

        // 画面上の座標(x, y)がどのへクスの上にあるか どのへクスの上にもなければ(0, -1)を返す
        public PAIR WhichHex(int x, int y)
        {
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
        public void SaveMap(int n)
        {
            if (!Directory.Exists("MapData"))
                Directory.CreateDirectory("MapData");
            using (StreamWriter w = new StreamWriter(string.Format(@"MapData\MapData{0}.csv", n)))
            {
                for (int i = 0; i < DataBase.MAP_MAX; i++)
                {
                    w.Write("{0}", nMap.GetState(0, i) + (int)um.GetType(0, i) * 100);
                    for (int j = 1; j < DataBase.MAP_MAX; j++)
                    {
                        w.Write(",{0}", nMap.GetState(j, i) + (int)um.GetType(j, i) * 100);
                    }
                    w.Write("\r\n");
                }
            }
        }

        // マップデータを実行可能ファイルのあるフォルダから見て /MapData/MapData.csv から読み込む（ファイルがなければ何もしない）
        public void ReadMap(int n)
        {
            if (File.Exists(string.Format(@"MapData\MapData{0}.csv", n)))
            {

                using (StreamReader r = new StreamReader(string.Format(@"MapData\MapData{0}.csv", n)))
                {
                    string line;
                    for (int i = 0; (line = r.ReadLine()) != null && i < DataBase.MAP_MAX; i++) // 1行ずつ読み出し。
                    {
                        string[] ss = line.Split(',');
                        for (int j = 0; j < ss.Length && j < DataBase.MAP_MAX; j++)
                        {
                            int v = int.Parse(ss[j]), hex = (v + 10000) % 100, uni = (v - hex) / 100;
                            nMap.ChangeState(j, i, hex);
                            um.ChangeType(j, i, (UnitType)uni);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ゲーム画面の描画メソッド
        /// </summary>
        /// <param name="d"></param>
        public override void SceneDraw(Drawing d)
        {
            // マップの描画
            nMap.Draw(d, Camera, Scale);
            um.Draw(d, Camera, Scale);
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
            int ps = Scale;
            if (state.ScrollWheelValue > pstate.ScrollWheelValue)       Scale++;
            else if (state.ScrollWheelValue < pstate.ScrollWheelValue)  Scale--;
            CameraX = CameraX + Game1._WindowSizeX / DataBase.MapScale[ps] / 2 - Game1._WindowSizeX / DataBase.MapScale[Scale] / 2;
            CameraY = CameraY + Game1._WindowSizeY / DataBase.MapScale[ps] / 2 - Game1._WindowSizeY / DataBase.MapScale[Scale] / 2;


            // 左クリックされたときにその座標がウィンドウ上であり、バーの上でなくかつどれかのへクスの上であればそのへクスの値をインクリメント
            if (pstate.LeftButton != ButtonState.Pressed && state.LeftButton == ButtonState.Pressed &&
                state.X >= 0 && state.X <= Game1._WindowSizeX && state.Y >= 0 && state.Y <= Game1._WindowSizeY)
            {
                PAIR p = WhichHex(state.X, state.Y);
                if (p.i >= 0 && p.j >= 0) nMap.ChangeState(p.i, p.j, (nMap.GetState(p.i, p.j) + 1) % DataBase.hex_tex.Count);
            }
            // 右クリックされたときにその座標がウィンドウ上であり、バーの上でなくかつどれかのへクスの上であればそのへクスの値をデクリメント
            if (pstate.RightButton != ButtonState.Pressed && state.RightButton == ButtonState.Pressed &&
                state.X >= 0 && state.X <= Game1._WindowSizeX && state.Y >= 0 && state.Y <= Game1._WindowSizeY)
            {
                PAIR p = WhichHex(state.X, state.Y);
                if (p.i >= 0 && p.j >= 0)　um.ChangeType(p.i, p.j, um.GetType(p.i, p.j) != UnitType.NK ? (UnitType)((int)um.GetType(p.i, p.j) + 1) : UnitType.Kin);
            }
            
            pstate = state;
            // シフト＋数字キーが押されるとマップデータの保存
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) || Keyboard.GetState().IsKeyDown(Keys.RightShift))
            {
                if (Keyboard.GetState().IsKeyDown(Keys.D1) || Keyboard.GetState().IsKeyDown(Keys.NumPad1)) SaveMap(1);
                if (Keyboard.GetState().IsKeyDown(Keys.D2) || Keyboard.GetState().IsKeyDown(Keys.NumPad2)) SaveMap(2);
                if (Keyboard.GetState().IsKeyDown(Keys.D3) || Keyboard.GetState().IsKeyDown(Keys.NumPad3)) SaveMap(3);
            }
            // 数字キーのみが押されるとマップデータ読み込み
            else
            {
                if (Keyboard.GetState().IsKeyDown(Keys.D1) || Keyboard.GetState().IsKeyDown(Keys.NumPad1))　ReadMap(1);
                if (Keyboard.GetState().IsKeyDown(Keys.D2) || Keyboard.GetState().IsKeyDown(Keys.NumPad2))　ReadMap(2);
                if (Keyboard.GetState().IsKeyDown(Keys.D3) || Keyboard.GetState().IsKeyDown(Keys.NumPad3))　ReadMap(3);
            }

            // Zキーが押されると終了
            if (Input.GetKeyPressed(KeyID.Select))　Delete = true;
        }
        #endregion
    }// class end
}// namespace end
