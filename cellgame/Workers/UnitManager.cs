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
        }
        // 描画
        public void Draw(Drawing d, Vector camera, int scale)
        {
            foreach(KochuUnit u in kochuUnits){
                Vector pos = DataBase.WhereDisp(u.x_index, u.y_index, camera, scale);
                if (DataBase.IsInDisp(pos, scale))
                    u.Draw(d);
            }
            foreach(MacroUnit u in macroUnits){
                Vector pos = DataBase.WhereDisp(u.x_index, u.y_index, camera, scale);
                if (DataBase.IsInDisp(pos, scale))
                    u.Draw(d);
            }
            foreach(JujoUnit u in jujoUnits){
                Vector pos = DataBase.WhereDisp(u.x_index, u.y_index, camera, scale);
                if (DataBase.IsInDisp(pos, scale))
                    u.Draw(d);
            }
            foreach(KosanUnit u in kosanUnits){
                Vector pos = DataBase.WhereDisp(u.x_index, u.y_index, camera, scale);
                if (DataBase.IsInDisp(pos, scale))
                    u.Draw(d);
            }
            foreach(NKUnit u in nkUnits){
                Vector pos = DataBase.WhereDisp(u.x_index, u.y_index, camera, scale);
                if (DataBase.IsInDisp(pos, scale))
                    u.Draw(d);
            }
            foreach(HelperTUnit u in helperTUnits){
                Vector pos = DataBase.WhereDisp(u.x_index, u.y_index, camera, scale);
                if (DataBase.IsInDisp(pos, scale))
                    u.Draw(d);
            }
            foreach(KillerTUnit u in killerTUnits){
                Vector pos = DataBase.WhereDisp(u.x_index, u.y_index, camera, scale);
                if (DataBase.IsInDisp(pos, scale))
                    u.Draw(d);
            }
            foreach(BUnit u in bUnits){
                Vector pos = DataBase.WhereDisp(u.x_index, u.y_index, camera, scale);
                if (DataBase.IsInDisp(pos, scale))
                    u.Draw(d);
            }
            foreach(PlasmaUnit u in plasmaUnits){
                Vector pos = DataBase.WhereDisp(u.x_index, u.y_index, camera, scale);
                if (DataBase.IsInDisp(pos, scale))
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
