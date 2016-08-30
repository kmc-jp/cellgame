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

        #endregion

        #region Method
        // コンストラクタ
        public UnitManager()
        {
            kochuUnits = new List<KochuUnit>();
            macroUnits = new List<MacroUnit>();
            jujoUnits = new List<JujoUnit>();
            kosanUnits = new List<KosanUnit>();
            nkUnits = new List<NKUnit>();
            helperTUnits = new List<HelperTUnit>();
            killerTUnits = new List<KillerTUnit>();
            bUnits = new List<BUnit>();
            plasmaUnits = new List<PlasmaUnit>();

            kinUnits = new List<KinUnit>();
            kabiUnits = new List<KabiUnit>();
            virusUnits = new List<VirusUnit>();
            ganUnits = new List<GanUnit>();
            kiseichuUnits = new List<KiseichuUnit>();
        }
        // 描画
        public void Draw(Drawing d, Vector camera, int scale)
        {
            foreach(KochuUnit u in kochuUnits)
            {
                if (DataBase.IsInDisp(DataBase.WhereDisp(u.x_index, u.y_index, camera, scale), scale))
                    u.Draw(d);
            }
            foreach(MacroUnit u in macroUnits)
            {
                if (DataBase.IsInDisp(DataBase.WhereDisp(u.x_index, u.y_index, camera, scale), scale))
                    u.Draw(d);
            }
            foreach(JujoUnit u in jujoUnits)
            {
                if (DataBase.IsInDisp(DataBase.WhereDisp(u.x_index, u.y_index, camera, scale), scale))
                    u.Draw(d);
            }
            foreach(KosanUnit u in kosanUnits)
            {
                if (DataBase.IsInDisp(DataBase.WhereDisp(u.x_index, u.y_index, camera, scale), scale))
                    u.Draw(d);
            }
            foreach(NKUnit u in nkUnits)
            {
                if (DataBase.IsInDisp(DataBase.WhereDisp(u.x_index, u.y_index, camera, scale), scale))
                    u.Draw(d);
            }
            foreach(HelperTUnit u in helperTUnits)
            {
                if (DataBase.IsInDisp(DataBase.WhereDisp(u.x_index, u.y_index, camera, scale), scale))
                    u.Draw(d);
            }
            foreach(KillerTUnit u in killerTUnits)
            {
                if (DataBase.IsInDisp(DataBase.WhereDisp(u.x_index, u.y_index, camera, scale), scale))
                    u.Draw(d);
            }
            foreach(BUnit u in bUnits)
            {
                if (DataBase.IsInDisp(DataBase.WhereDisp(u.x_index, u.y_index, camera, scale), scale))
                    u.Draw(d);
            }
            foreach(PlasmaUnit u in plasmaUnits)
            {
                if (DataBase.IsInDisp(DataBase.WhereDisp(u.x_index, u.y_index, camera, scale), scale))
                    u.Draw(d);
            }
            foreach (KinUnit u in kinUnits)
            {
                if (DataBase.IsInDisp(DataBase.WhereDisp(u.x_index, u.y_index, camera, scale), scale))
                    u.Draw(d);
            }
            foreach (KabiUnit u in kabiUnits)
            {
                if (DataBase.IsInDisp(DataBase.WhereDisp(u.x_index, u.y_index, camera, scale), scale))
                    u.Draw(d);
            }
            foreach (VirusUnit u in virusUnits)
            {
                if (DataBase.IsInDisp(DataBase.WhereDisp(u.x_index, u.y_index, camera, scale), scale))
                    u.Draw(d);
            }
            foreach (GanUnit u in ganUnits)
            {
                if (DataBase.IsInDisp(DataBase.WhereDisp(u.x_index, u.y_index, camera, scale), scale))
                    u.Draw(d);
            }
            foreach (KiseichuUnit u in kiseichuUnits)
            {
                if (DataBase.IsInDisp(DataBase.WhereDisp(u.x_index, u.y_index, camera, scale), scale))
                    u.Draw(d);
            }
        }
        // 更新
        public void Update()
        {

        }
        #endregion
    }// class end
}// namespace end
