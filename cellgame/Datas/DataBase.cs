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
        // ユニットの説明文（表示しないものも存在する）
        public static readonly string[] MyUnitExpl = 
        {
            "好中球は強力な殺菌作用を持つ食細胞で、主に免疫の\n初動段階に活躍します。\n短時間で生産することが可能ですが、寿命が短いとい\nう欠点があります。",
            "マクロファージは大食細胞とも呼ばれ、体内に侵入し\nた異物を次々に飲み込みます。\n寿命が長く、中盤から後半にかけて活躍します。\nまた、倒した敵の戦闘力の一部を研究力に加算します。",
            "樹状細胞は侵入した敵の正体を明らかにするのに役立\nつ細胞です。\nこのユニットで敵を倒すと、多くの研究力を得ること\nができます。",
            "好酸球は白血球の一種で、炎症物質を体内に溜め込ん\nでいます。\nこの物質を外敵に向かって放出することで攻撃を行い、\n特に寄生虫に対しての攻撃力は倍になります。",
            "NK細胞は異常のある細胞を独自に検出して破壊する\n働きを持った細胞です。\n具体的には、細胞に定着したウイルスとガンに対して\nの攻撃が可能です。",
            "ヘルパーT細胞は戦闘を行いませんが、自身から放出\nする物質によって周囲のユニットを活性化します。\nこのユニットに隣接するユニットは戦闘力が20%増強\nされます。",
            "キラーT細胞は異常のある細胞に対して自然死を起こ\nさせる細胞です。\nNK細胞と同等の能力を持ち、こちらのほうが運用が楽\nです。",
            "B細胞は敵を取り込むとプラズマ細胞として生まれ\n変わり、抗体を生産するようになります。\nプラズマ細胞は、B細胞の時に倒したのと同じ種類\nの敵が2ヘクス以内の距離にいるとき、その戦闘力\nを10%減少させます。",
            "B細胞の時に倒した敵と同じ種類の敵を弱体化させる\nユニットです。\n周囲2ヘクス以内にいる敵ユニットの戦闘力は-10%されます。"
        };
        public static readonly string[] EnemyUnitExpl = 
        {
            "最も基本的でありふれた敵です。",
            "菌とほとんど同じです。",
            "一定の確率で細胞ヘクスに定着します。定着した状態で放置すると自己複製して増えていきます。",
            "このユニットは、傷口ヘクスからではなく通常の細胞ヘクスからわずかな確率で発生します。\nこのユニットは移動しません。",
            "強力なユニットです。好酸球などのアンチユニットを利用しなければ倒すことは難しいでしょう。"
        };
        // ユニット各種類ごとの固有値
        public static int[] defMyUnitStrength = new[] { 8, 5, 5, 5, 6, 4, 6, 4, 4 };
        public static readonly int[] MyUnitMAX_HP = new[] { 100, 100, 100, 100, 100, 100, 100, 100 ,100 };
        public static readonly int[] MyUnitMAX_LP = new[] { 7, 30, 10, 10, 20, 30, 30, 10, 20 };
        public static int[] MyUnitStrength = new[] { 8, 5, 5, 5, 6, 4, 6, 4, 4 };
        public static readonly int[] MyUnitMoveRange = new[] { 5, 5, 5, 5, 5, 5, 5, 5, 5 };

        public static int[] defEnemyUnitStrength = new[] { 5, 5, 4, 5, 15 };
        public static readonly int[] EnemyUnitMAX_HP = new[] { 100, 100, 100, 100, 100 };
        public static readonly int[] EnemyUnitMAX_LP = new[] { 20, 20, 20, 10000, 30 };
        public static int[] EnemyUnitStrength = new[] { 5, 5, 4, 5, 15 };
        public static readonly int[] EnemyUnitMoveRange = new[] { 5, 5, 5, 0, 5 };

        #endregion

        #region Status
        // アイコン画像
        public static Texture2D studyIcon;
        public static Texture2D productIcon;
        public static Texture2D temperIcon;
        #endregion

        #region Study
        public static Texture2D tree_tex;
        public static readonly Vector studyTreePos = new Vector(104, 120);
        public static readonly Vector studyTreeSize = new Vector(1072, 528);
        // 研究ツリー
        /*public enum Study
        {
            Kaku, Saito, Inter, Kemo, Cross, Kou, Class, Shinwa, Opuso, Chuwa, Masuto
        }*/
        // 研究名
        public static readonly string[] StudyName = {
            "獲得免疫", "サイトカイン", "インターフェロン", "ケモカイン", "クロスプレゼンテーション", "効率的アポトーシス", "クラススイッチ", "親和性成熟", "オプソニン化", "中和", "マスト細胞"
        };
        // 研究の説明文
        public static readonly string[] StudyExpl = {
            "ヘルパーT細胞が生産可能になる。\n生産力が+5される。\nマクロファージの戦闘力が+20%される。",
            "ヘルパーT細胞が味方ユニットを強化する効率が+100%される。\nB細胞が生産可能になる。\n生産力が+5される。\nマクロファージの戦闘力が+20%される。\n樹状細胞の戦闘力が+20%される。\n好酸球の戦闘力が+20%される。",
            "全ての研究項目で必要研究力が-20%される。\n生産力が+5される。",
            "全てのユニットの生産時間が-1ターンされる。\n生産力が+5される。",
            "キラーT細胞が生産可能になる。\nマクロファージの戦闘力が+20%される。\n樹状細胞の戦闘力が+20%される。",
            "キラーT細胞、NK細胞の戦闘力が+33%される。",
            "プラズマ細胞が敵を弱体化する効率が+33%される。\n生産力が+5される。",
            "B細胞の戦闘力が+100%される。\n生産力が+5される。",
            "プラズマ細胞が弱体化しているユニットを攻撃する味方ユニットは反撃を受けない。\n（自分から攻撃した場合、敵ユニットのみがダメージを受ける）\nマクロファージの戦闘力が+20%される。\n樹状細胞の戦闘力が+10%される。\n好酸球の戦闘力が+10%される。",
            "プラズマ細胞によって弱体化されているユニットは自身の複製を行わない。\nマクロファージの戦闘力が+20%される。\n樹状細胞の戦闘力が+10%される。\n好酸球の戦闘力が+10%される。",
            "寄生虫の戦闘力が-25%される。\n好酸球の戦闘力が+20%される。"
        };
        // その研究をするために完了しておく必要のある研究
        public static readonly int[,] StudyParent = {
            { -1, -1 }, { 0, -1 }, { 1, -1 }, { 1, -1 }, { 0, -1 }, { 2, 4 }, { 1, -1 }, { 1, -1 }, { 6, 7 }, { 6, 7 }, { 6, -1 }
        };
        // 必要研究力
        public static readonly int[] defmaxStudyPower = {
            100, 200, 320, 350, 360, 500, 300, 300, 520, 520, 480
        };
        public static int[] maxStudyPower = {
            100, 200, 320, 350, 360, 500, 300, 300, 520, 520, 480
        };
        // 研究力の初期値
        public static readonly int DefaultStudyPower = 10;
        #endregion

        #region Product
        // 生産力の初期値
        public static readonly int DefaultProductPower = 10;
        // それぞれの味方ユニットの一ターン毎に割り当てられる生産力の最大値
        public static readonly int[] maxProductPower = {
            4, 4, 4, 4, 5, 5, 6, 5, -1
        };
        // それぞれの味方ユニットの生産するために必要な合計生産力
        public static readonly int[] defsumProductPower = {
            12, 20, 16, 16, 25, 25, 20, 20, -1
        };
        public static int[] sumProductPower = {
            12, 20, 16, 16, 25, 25, 20, 20, -1
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
            double s = Math.Min(Math.Max((Math.Pow(2.0, r + 1.0) + 3.0) / 7.0, 0.01), 100.0);

            Da = (int)(a >= b ? 20.0 / s : 20.0 * s);
            Db = (int)(a >= b ? 30.0 * s : 30.0 / s);
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
