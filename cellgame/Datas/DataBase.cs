using System;
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

        #region Variable
        // ウィンドウの元のサイズ(4：3)
        public static readonly int WindowDefaultSizeX = Game1.WindowSizeX;
        public static readonly int WindowDefaultSizeY = Game1.WindowSizeY;
        // 16：9にした時のウィンドウの縦のサイズ
        public static readonly int WindowSlimSizeY = 720;
        // バーの個数
        public static readonly int BarIndexNum = 5;
        // それぞれのバーの横幅の個数と縦幅の個数リスト
        public static readonly int[] BarWidth = new[] { 22, 22, 22, 40, 18, 18 };
        public static readonly int[] BarHeight = new[] { 6, 23, 16, 4, 6, 23 };
        // マップの倍率の配列
        public static readonly double[] MapScale = new[] { 0.15d, 0.4d, 0.5d, 0.6d, 0.7d, 0.8d, 0.9d, 1.0d, 1.2d, 1.5d, 2.0d, 3.0d, 4.0d };
        // デフォルトのマップの倍率
        public static readonly int DefaultMapScale = 7;
        // へクス画像の横幅と縦幅
        public static readonly int HexWidth = 180;
        public static readonly int HexHeight = 200;
        // へクス画像のリスト
        public static List<Texture2D> hex;
        // バー画像のリスト
        public static List<Texture2D> bar_flame;
        // ボックス画像のリスト
        public static List<Texture2D> box_flame;

        public enum BarIndex
        {
            Study, Unit, Minimap, Status, Arrange, Product
        }
        public static readonly Vector[] BarPos = new[] {
            new Vector(0d, 0d), new Vector(0d, 96d), new Vector(0d, 704d), new Vector(352d, 0d), new Vector(992d, 0d), new Vector(992d, 96d)
        };
        public static readonly int MAP_MAX = 20;
        
        #endregion
        #region singleton
        static DataBase database_singleton = new DataBase();

        static DataBase() {

        }
        private DataBase() { }
        #endregion

        #region Method
        #endregion
    }// DataBase end
}// namespace end
