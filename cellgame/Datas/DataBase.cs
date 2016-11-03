﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace CommonPart {
    /// <summary>
    /// 不変なデータをまとめたクラス
    /// </summary>
    class DataBase　{

        #region Window
        // ウィンドウの元のサイズ(4：3)
        public static readonly int WindowDefaultSizeX = Game1.WindowSizeX; // 1280
        public static readonly int WindowDefaultSizeY = Game1.WindowSizeY; // 960
        // 16：9にした時のウィンドウの縦のサイズ
        public static readonly int WindowSlimSizeY = 720;
        #endregion

        #region Map
        // へクス画像のリスト
        public static List<Texture2D> mini_hex_tex;
        public static List<Texture2D> hex_tex;
        public static Texture2D select_tex;
        // マップのサイズは MAP_MAX × MAP_MAX
        public static readonly int MAP_MAX = 20;
        // マップの倍率の配列
        public static readonly double[] MapScale = { 0.1d, 0.5d, 0.75d, 1.0d };
        // デフォルトのマップの倍率
        public static readonly int DefaultMapScale = 3;
        // へクス画像の横幅と縦幅
        public static readonly int HexWidth = 180;
        public static readonly int HexHeight = 200;
        // カメラの移動速度
        public static readonly int cameraV = 12;
        #endregion

        #region Bar&Box
        // バー画像のリスト
        public static List<Texture2D> bar_frame_tex;
        public static List<Texture2D> minimapButton;
        public static List<Texture2D> productButton;
        public static List<Texture2D> arrangeButton;
        // バー・ボックスの名前　※要らないけど名前と番号のメモ用に
        public enum BarName
        {
            Study, Unit, Minimap, Status, Arrange, Product
        }
        // それぞれのバー・ボックスの横幅の個数と縦幅の個数リスト
        public static readonly int[] BarWidth = { 22, 22, 22, 40, 18, 18 };
        public static readonly int[] BarHeight = { 6, 23, 16, 4, 6, 23 };
        // それぞれのバー・ボックスの左上の画面上での座標のリスト
        public static readonly Vector[] BarPos = {
            new Vector(0d, 0d), new Vector(0d, 96d), new Vector(0d, 704d), new Vector(352d, 0d), new Vector(992d, 0d), new Vector(992d, 96d)
        };
        #endregion

        #region Unit
        // ユニット画像のリスト
        public static List<Texture2D> miniUnit_tex;
        public static List<Texture2D> myUnit_tex;
        public static List<Texture2D> enemyUnit_tex;
        public static List<Texture2D> Plasma_tex;
        public static List<Texture2D> Virus_tex;
        // ユニットの名前
        /*public enum UnitType
        {
            NULL, Kochu, Macro, Jujo, Kosan, NK, HelperT, KillerT, B, Plasma, Kin = -5, Kabi, Virus, Gan, Kiseichu
        }*/
        public static readonly string[] MyUnitName = 
        {
            "好中球", "マクロファージ", "樹状細胞", "好酸球", "NK細胞", "ヘルパーT細胞", "キラーT細胞", "B細胞", "プラズマ細胞"
        };
        public static readonly string[] EnemyUnitName = 
        {
            "菌", "カビ", "ウイルス", "ガン", "寄生虫"
        };
        // ユニット各種類ごとの固有値
        public static readonly int[] MyUnitMAX_HP = new[] { 100, 100, 100, 100, 100, 100, 100, 100 ,100 };
        public static readonly int[] MyUnitMAX_LP = new[] { 15, 60, 20, 20, 30, 60, 60, 20, 40 };
        public static int[] MyUnitStrength = new[] { 8, 5, 5, 5, 5, 4, 5, 4, 4 };
        public static readonly int[] MyUnitMoveRange = new[] { 2, 2, 2, 2, 2, 2, 2, 2, 2 };

        public static readonly int[] EnemyUnitMAX_HP = new[] { 100, 100, 100, 100, 100 };
        public static readonly int[] EnemyUnitMAX_LP = new[] { 30, 30, 30, 10000, 60 };
        public static int[] EnemyUnitStrength = new[] { 5, 5, 4, 5, 15 };
        public static readonly int[] EnemyUnitMoveRange = new[] { 2, 2, 2, 0, 2 };

        #endregion

        #region Status
        // アイコン画像
        public static Texture2D studyIcon;
        public static Texture2D productIcon;
        public static Texture2D temperIcon;
        #endregion

        #region Study
        // 研究ツリー
        /*public enum Study
        {
            Kaku, Saito, Inter, Kemo, Cross, Kou, Class, Shinwa, Opuso, Chuwa, Masuto
        }*/
        // 研究名
        public static readonly string[] StudyName = {
            "獲得免疫", "サイトカイン", "インターフェロン", "ケモカイン", "クロスプレゼンテーション", "効率的アポトーシス", "クラススイッチ", "親和性成熟", "オプソニン化", "中和", "マスト細胞"
        };
        // その研究をするために完了しておく必要のある研究
        public static readonly int[,] StudyParent = {
            { -1, -1 }, { 0, -1 }, { 1, -1 }, { 1, -1 }, { 0, -1 }, { 2, 4 }, { 1, -1 }, { 1, -1 }, { 6, 7 }, { 6, 7 }, { 6, -1 }
        };
        // 必要研究力
        public static readonly int[] maxStudyPower = {
            100, 100, 100, 100, 100, 100, 100, 100, 100, 100, 100
        };
        // 研究力の初期値
        public static readonly int DefaultStudyPower = 10;
        #endregion

        #region Product
        // 生産力の初期値
        public static readonly int DefaultProductPower = 25;
        // それぞれの味方ユニットの一ターン毎に割り当てられる生産力の最大値
        public static readonly int[] maxProductPower = {
            3, 5, 4, 4, 6, 6, 6, 5, -1
        };
        // それぞれの味方ユニットの生産するために必要な合計生産力
        public static int[] sumProductPower = {
            9, 30, 16, 16, 36, 30, 30, 20, -1
        };
        #endregion

        #region Button
        // ユニットボックスのコマンドボタン画像のリスト
        public static List<Texture2D> command_tex;
        #endregion

        #region Method
        // 戦闘時のダメージ計算
        public static void Battle(int a, int b, out int Da, out int Db)
        {
            if(b == 0) {
                Da = 0;
                Db = 100;
                return;
            }
            else if (a == 0) {
                Da = 100;
                Db = 0;
            }

            double r = a >= b ? (double)a / b : (double)b / a;
            double s = Math.Min(Math.Max((Math.Pow(2.0, r + 1.0) + 3.0) / 7.0, 0.0625), 16.0);

            Da = (int)(a >= b ? 10.0 / s : 10.0 * s);
            Db = (int)(a >= b ? 12 * s : 12 / s);
        }
        // マップ上の位置から現在の画面上の座標を求める
        public static Vector WhereDisp(int x_index, int y_index, Vector camera, int scale)
        {
            if (y_index % 2 == 1)
                return new Vector((HexWidth * x_index + HexWidth / 2 - camera.X) * MapScale[scale],
                                (HexHeight * 3 / 4 * y_index - camera.Y) * MapScale[scale]);

            return new Vector((HexWidth * x_index - camera.X) * MapScale[scale],
                            (HexHeight * 3 / 4 * y_index - camera.Y) * MapScale[scale]);
        }
        // 左上の描画位置を与えるとそのユニットが画面に入るかどうかを返す
        public static bool IsInDisp(Vector p, int scale)
        {
            return p.X >= -HexWidth * MapScale[scale] && p.X <= Game1._WindowSizeX && p.Y >= -HexHeight * MapScale[scale] && p.Y <= Game1._WindowSizeY;
        }
        // 画面上の位置 (x,y) がへクスのある位置の上にあるどうかを返す
        public static bool IsOnHex(int x_index, int y_index, int x, int y, Vector camera, int scale)
        {
            double dx = camera.X + x / MapScale[scale] - HexWidth * x_index;
            double dy = camera.Y + y / MapScale[scale] - HexHeight * 3 / 4 * y_index;
            if (y_index % 2 == 1) dx -= HexWidth / 2;

            return dx >= 0 && dx <= HexWidth &&
                dy + dx * HexHeight / HexWidth / 2 >= HexHeight / 4 && dy + dx * HexHeight / HexWidth / 2 <= HexHeight / 4 * 5 &&
                dy + HexHeight / 4 >= dx * HexHeight / HexWidth / 2 && dy <= HexHeight / 4 * 3 + dx * HexHeight / HexWidth / 2;
        }
        #endregion

    }// DataBase end

    // ユニットの名前
    public enum UnitType
    {
        NULL, Kochu, Macro, Jujo, Kosan, NK, HelperT, KillerT, B, Plasma, Kin = -5, Kabi, Virus, Gan, Kiseichu
    }
    public enum Study
    {
        Kaku, Saito, Inter, Kemo, Cross, Kou, Class, Shinwa, Opuso, Chuwa, Masuto
    }
    // x_index と y_index のペアの構造体
    public struct PAIR
    {
        public int i, j;
        public PAIR(int _i, int _j) { i = _i; j = _j; }
        public static bool operator ==(PAIR l, PAIR r)
        {
            return l.i == r.i && l.j == r.j;
        }
        public static bool operator !=(PAIR l, PAIR r)
        {
            return !(l == r);
        }
        public static bool operator >(PAIR l, PAIR r)
        {
            return l.i > r.i || (l.i == r.i && l.j > r.j);
        }
        public static bool operator <(PAIR l, PAIR r)
        {
            return l.i < r.i || (l.i == r.i && l.j < r.j);
        }
    }

}// namespace end
