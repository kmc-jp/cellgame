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
        List<NKUnit> NKUnits;
        List<HelperTUnit> helperTUnits;
        List<KillerTUnit> killerTUnits;
        List<BUnit> BUnits;
        List<PlasmaUnit> plasmaUnits;
        // 敵ユニット
        #endregion

        #region Method
        public UnitManager()
        {
            kochuUnits = new List<KochuUnit>();
            macroUnits = new List<MacroUnit>();
            jujoUnits = new List<JujoUnit>();
            kosanUnits = new List<KosanUnit>();
            NKUnits = new List<NKUnit>();
            helperTUnits = new List<HelperTUnit>();
            killerTUnits = new List<KillerTUnit>();
            BUnits = new List<BUnit>();
            plasmaUnits = new List<PlasmaUnit>();
        }
        public void Draw(Drawing d)
        {

        }
        public void Update()
        {

        }
        #endregion
    }// class end
}// namespace end
