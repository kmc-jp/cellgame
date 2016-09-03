using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonPart
{
    class UnitManager
    {
        #region Variable
        // 味方ユニット
        List<KochuUnit> kochuUnits;
        List<MacroUnit> macroUnits;
        List<JujoUnit> jujoUnits;
        List<KosanUnit> kosanUnits;
        List<NKUnit> nkUnits;
        List<HelperTUnit> helperTUnits;
        List<KillerTUnit> killerTUnits;
        List<BUnit> bUnits;
        List<PlasmaUnit> plasmaUnits;
        // 敵ユニット
        List<KinUnit> kinUnits;
        List<KabiUnit> kabiUnits;
        List<VirusUnit> virusUnits;
        List<GanUnit> ganUnits;
        List<KiseichuUnit> kiseichuUnits;

        public enum UnitType
        {
            NULL, Kochu, Macro, Jujo, Kosan, NK, HelperT, KillerT, B, Plasma, Kin = -5, Kabi, Virus, Gan, Kiseichu
        }

        UnitType[,] unitMap;

        #endregion

        #region Method
        // コンストラクタ
        public UnitManager()
        {
            // 味方ユニット
            kochuUnits = new List<KochuUnit>();
            macroUnits = new List<MacroUnit>();
            jujoUnits = new List<JujoUnit>();
            kosanUnits = new List<KosanUnit>();
            nkUnits = new List<NKUnit>();
            helperTUnits = new List<HelperTUnit>();
            killerTUnits = new List<KillerTUnit>();
            bUnits = new List<BUnit>();
            plasmaUnits = new List<PlasmaUnit>();

            // 敵ユニット
            kinUnits = new List<KinUnit>();
            kabiUnits = new List<KabiUnit>();
            virusUnits = new List<VirusUnit>();
            ganUnits = new List<GanUnit>();
            kiseichuUnits = new List<KiseichuUnit>();

            // ユニットのマップ情報の初期化
            unitMap = new UnitType[DataBase.MAP_MAX + (DataBase.MAP_MAX + 1) / 2, DataBase.MAP_MAX];
            for(int i = 0; i < DataBase.MAP_MAX + (DataBase.MAP_MAX + 1) / 2; i++)
            {
                for(int j = 0; j < DataBase.MAP_MAX; j++)
                {
                    unitMap[i, j] = UnitType.NULL;
                }
            }
        }


        // 描画
        public void Draw(Drawing d, Vector camera, int scale)
        {
            for (int i = 0; i < DataBase.MAP_MAX + (DataBase.MAP_MAX + 1) / 2; i++)
            {
                for (int j = 0; j < DataBase.MAP_MAX; j++)
                {
                    if(unitMap[i, j] != UnitType.NULL)
                    {
                        int i_ = i - (j + 1) / 2, j_ = j;
                        Vector pos = DataBase.WhereDisp(i_, j_, camera, scale);
                        if (DataBase.IsInDisp(pos, scale) && i_ >= 0 && i_ < DataBase.MAP_MAX && j_ >= 0 && j_ < DataBase.MAP_MAX)
                        {
                            pos += new Vector(26 * DataBase.MapScale[scale], 36 * DataBase.MapScale[scale]);
                            if(unitMap[i, j] > 0)
                            {
                                d.Draw(pos, DataBase.myUnit_tex[(int)unitMap[i, j] - 1], DepthID.Player, (float)DataBase.MapScale[scale]);
                            }
                            else
                            {
                                d.Draw(pos, DataBase.enemyUnit_tex[(int)unitMap[i, j] + 5], DepthID.Player, (float)DataBase.MapScale[scale]);
                            }
                        }
                    }
                }
            }
        }
        // 更新
        public void Update(bool changeTurn = false)
        {
            if (changeTurn)
                TurnUpdate();

            // HP か LP が 0 以下になったユニットを削除
            kochuUnits.RemoveAll(u => u.HP <= 0 || u.LP <= 0);
            macroUnits.RemoveAll(u => u.HP <= 0 || u.LP <= 0);
            jujoUnits.RemoveAll(u => u.HP <= 0 || u.LP <= 0);
            kosanUnits.RemoveAll(u => u.HP <= 0 || u.LP <= 0);
            nkUnits.RemoveAll(u => u.HP <= 0 || u.LP <= 0);
            helperTUnits.RemoveAll(u => u.HP <= 0 || u.LP <= 0);
            killerTUnits.RemoveAll(u => u.HP <= 0 || u.LP <= 0);
            bUnits.RemoveAll(u => u.HP <= 0 || u.LP <= 0);
            plasmaUnits.RemoveAll(u => u.HP <= 0 || u.LP <= 0);
            kinUnits.RemoveAll(u => u.HP <= 0 || u.LP <= 0);
            kabiUnits.RemoveAll(u => u.HP <= 0 || u.LP <= 0);
            virusUnits.RemoveAll(u => u.HP <= 0 || u.LP <= 0);
            ganUnits.RemoveAll(u => u.HP <= 0 || u.LP <= 0);
            kiseichuUnits.RemoveAll(u => u.HP <= 0 || u.LP <= 0);
        }
        // ターンの更新
        public void TurnUpdate()
        {
            
        }
        #endregion
    }// class end
}// namespace end
