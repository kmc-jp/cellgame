﻿using System;
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
        public ProductArrangeBar proarrBar;
        public MinimapBox minimapBox;
        public StatusBar statusBar;
        public StudyBar studyBar;
        public UnitBox unitBox;
        Button next;
        // カメラ
        public Vector Camera { get { return _camera; } }
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
        public static Map nMap;
        // ユニットマネージャ
        public static UnitManager um;
        // カメラの移動速度
        int defcameraVel = DataBase.cameraV;
        // カメラの倍率
        int _scale = DataBase.DefaultMapScale;
        public int Scale {
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
        static decimal _bodyTemp;
        public static decimal BodyTemp{
            get { return _bodyTemp; }
            set { _bodyTemp = Math.Max(36.0m, value); }
        }

        // 現在のターン
        int pturn = 0;
        public int turn = 0;

        public static bool changeTurn = false;

        AI ai;

        #endregion

        #region Method
        // コンストラクタ
        public PlayScene(SceneManager s, int map_n, string dataName = "")
            : base(s)
        {
            pstate = Mouse.GetState();
            nMap = new Map();
            studyBar = new StudyBar();
            unitBox = new UnitBox();
            minimapBox = new MinimapBox();
            statusBar = new StatusBar();
            proarrBar = new ProductArrangeBar();
            UnitMap _uMap = ReadMap(map_n + 1);
            um = new UnitManager(ref unitBox, _uMap);
            next = new Button(new Vector(1120, 912), 160, new Color(255, 162, 0), Color.Black, "次のターンへ");



            studyPower = DataBase.DefaultStudyPower;
            productPower = maxProductPower = DataBase.DefaultProductPower;
            BodyTemp = 36.0m;
            if (dataName != "")
            {
                ReadData(dataName);
            }

            SoundManager.Music.PlayBGM(BGMID.Normal, true);
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
        public UnitMap ReadMap(int n)
        {
            UnitMap res = new UnitMap();
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
                            int v = int.Parse(ss[j]), hex = (v + 10000) % 100, uni = (v - hex) / 100;
                            nMap.ChangeState(j, i, hex);
                            res.ChangeType(j, i, (UnitType)uni);
                        }
                    }
                }
            }
            return res;
        }

        // セーブする
        public void SaveData(string name = "")
        {
            if (!Directory.Exists("SaveData")) Directory.CreateDirectory("SaveData");

            if (name == "") name = DateTime.Now.ToString("yy_MM_dd_hh：mm") + ".save";

            using (StreamWriter w = new StreamWriter(string.Format(@"SaveData\{0}", name)))
            {
                for (int i = 0; i < DataBase.MAP_MAX; i++)
                {
                    for (int j = 0; j < DataBase.MAP_MAX; j++)
                    {
                        Unit u = um.uMap.data[i + (j + 1) / 2, j];
                        w.Write("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}\r\n", nMap.Data[i, j], (int)u.type, u.HP, u.LP, u.movePower, u.defcommand, u.command, u.defattack, u.attack, (int)u.enemyType, u.virusState);
                    }
                }
            }
        }
        // セーブデータを読み込む(読み込めなければfalseを返す)
        public bool ReadData(string name)
        {
            if (File.Exists(@"SaveData\" + name + ".save"))
            {
                using (StreamReader r = new StreamReader(@"SaveData\" + name + ".save"))
                {
                    string line;
                    for (int i = 0; (line = r.ReadLine()) != null && i < DataBase.MAP_MAX * DataBase.MAP_MAX; i++) // 1行ずつ読み出し。
                    {
                        string[] ss = line.Split(',');
                        nMap.Data[i / DataBase.MAP_MAX, i % DataBase.MAP_MAX] = int.Parse(ss[0]);
                        um.uMap.data[i / DataBase.MAP_MAX + (i % DataBase.MAP_MAX + 1) / 2, i % DataBase.MAP_MAX] =
                            new Unit((UnitType)int.Parse(ss[1]), int.Parse(ss[2]), int.Parse(ss[3]), int.Parse(ss[4]), bool.Parse(ss[5]), bool.Parse(ss[6]), bool.Parse(ss[7]), bool.Parse(ss[8]), (UnitType)int.Parse(ss[9]), int.Parse(ss[10]));
                    }
                    return true;
                }
            }
            return false;
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

            // ウィンドウのサイズが変わったら次のターンボタンを移動
            if (Settings.WindowStyle == 1 && next.pos.Y + next.size.Y != Game1._WindowSizeY)
            {
                next.MoveTo(new Vector2(next.pos.X, next.pos.Y + 240));
            }
            else if (Settings.WindowStyle == 0 && next.pos.Y + next.size.Y != Game1._WindowSizeY)
            {
                next.MoveTo(new Vector2(next.pos.X, next.pos.Y - 240));
            }

            // ボタンの更新
            next.Update(pstate, state);

            // 次のターンへボタンを押されるとターンを進める
            if (next.Clicked())
            {
                bool flag = true;
                foreach (PAIR p in um.myUnits)
                {
                    if (um.uMap.data[p.i, p.j].command)
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    changeTurn = true;
                }
                else
                {
                    new ConfirmationScene(scenem);
                }
            }

            if (changeTurn)
            {
                changeTurn = false;
                turn++;
                studyBar.UpdateTurn();
                ai = new AI(scenem, ref nMap, ref um, this);
            }

            // バー・ボックスの更新
            studyBar.Update(pstate, state, scenem);
            unitBox.Update(pstate, state, scenem);
            minimapBox.Update(pstate, state);
            statusBar.Update();
            proarrBar.Update(pstate, state, um, this, scenem);
            if(pturn < turn)
            {
                proarrBar.productBox.UpdateTurn();
            }


            // ユニットの更新
            um.Update(pstate, state, this, false);
            if (pturn < turn) {
                int wl = um.UpdateTurn();
                if (wl == 1)
                {
                    new GameClearScene(scenem);
                    Delete = true;
                    ai.Delete = true;
                    return;
                }
                if (wl == -1)
                {
                    new GameOverScene(scenem);
                    Delete = true;
                    ai.Delete = true;
                    return;
                }
            }
            // 次のユニットへフェードイン
            if (um.phase)
            {
                if(um.select_i >= 0 && um.select_j >= 0)
                    FadeIn(um.select_i - (um.select_j + 1) / 2, um.select_j);
                um.phase = false;
            }


            // Sキーが押されるとセッティングメニューを開く
            if (Input.GetKeyPressed(KeyID.Setting)) new SettingsScene(scenem);
            
            // Xキーが押されるとセーブ
            if (Input.GetKeyPressed(KeyID.Cancel)) new SaveConfScene(scenem, this);

            pturn = turn;
            pstate = state;

            if (BodyTemp >= 41m && SoundManager.Music.GetPlayingID != BGMID.Pinch) SoundManager.Music.PlayBGM(BGMID.Pinch, true);
            base.SceneUpdate();
        }
        public void UpdateByAI()
        {
            // 現在のマウスの状態を取得
            MouseState state = Mouse.GetState();
            // マウススクロールするとマップの描画倍率が変化
            int ps = Scale;
            if (state.ScrollWheelValue > pstate.ScrollWheelValue) Scale++;
            else if (state.ScrollWheelValue < pstate.ScrollWheelValue) Scale--;
            CameraX = CameraX + Game1._WindowSizeX / DataBase.MapScale[ps] / 2 - Game1._WindowSizeX / DataBase.MapScale[Scale] / 2;
            CameraY = CameraY + Game1._WindowSizeY / DataBase.MapScale[ps] / 2 - Game1._WindowSizeY / DataBase.MapScale[Scale] / 2;

            // ユニットの更新
            um.Update(pstate, state, this, true);
            
            if (um.select_i >= 0 && um.select_j >= 0)
                FadeIn(um.select_i - (um.select_j + 1) / 2, um.select_j);

            pstate = state;
        }
        // 選択されたユニットにフェードイン
        public void FadeIn(int x_index, int y_index)
        {
            CameraX = (DataBase.HexWidth * x_index + DataBase.HexWidth / 2 * ((y_index % 2) + 1)) - Game1._WindowSizeX / 2 / DataBase.MapScale[Scale];
            CameraY = (DataBase.HexHeight * 3 / 4 * y_index + DataBase.HexHeight / 2) - Game1._WindowSizeY / 2 / DataBase.MapScale[Scale];
        }
        #endregion
    }// class end
}// namespace end
