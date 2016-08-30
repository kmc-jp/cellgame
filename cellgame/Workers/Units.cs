using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonPart
{
    // 味方ユニット
    class KochuUnit : Unit
    {
        public KochuUnit(int _x_index, int _y_index)
            : base(_x_index, _y_index, (int)DataBase.MyUnitName.Kochu)
        {

        }
    }
    class MacroUnit : Unit
    {
        public MacroUnit(int _x_index, int _y_index)
            : base(_x_index, _y_index, (int)DataBase.MyUnitName.Macro)
        {

        }
    }
    class JujoUnit : Unit
    {
        public JujoUnit(int _x_index, int _y_index)
            : base(_x_index, _y_index, (int)DataBase.MyUnitName.Jujo)
        {

        }
    }
    class KosanUnit : Unit
    {
        public KosanUnit(int _x_index, int _y_index)
            : base(_x_index, _y_index, (int)DataBase.MyUnitName.Kosan)
        {

        }
    }
    class NKUnit : Unit
    {
        public NKUnit(int _x_index, int _y_index)
            : base(_x_index, _y_index, (int)DataBase.MyUnitName.NK)
        {

        }
    }
    class HelperTUnit : Unit
    {
        public HelperTUnit(int _x_index, int _y_index)
            : base(_x_index, _y_index, (int)DataBase.MyUnitName.HelperT)
        {

        }
    }
    class KillerTUnit : Unit
    {
        public KillerTUnit(int _x_index, int _y_index)
            : base(_x_index, _y_index, (int)DataBase.MyUnitName.KillerT)
        {

        }
    }
    class BUnit : Unit
    {
        public BUnit(int _x_index, int _y_index)
            : base(_x_index, _y_index, (int)DataBase.MyUnitName.B)
        {

        }
    }
    class PlasmaUnit : Unit
    {
        public PlasmaUnit(int _x_index, int _y_index)
            : base(_x_index, _y_index, (int)DataBase.MyUnitName.Plasma)
        {

        }
    }
    // 敵ユニット
    class KinUnit : Unit
    {
        public KinUnit(int _x_index, int _y_index)
            : base(_x_index, _y_index, (int)DataBase.EnemyUnitName.Kin)
        {

        }
    }
    class KabiUnit : Unit
    {
        public KabiUnit(int _x_index, int _y_index)
            : base(_x_index, _y_index, (int)DataBase.EnemyUnitName.Kabi)
        {

        }
    }
    class VirusUnit : Unit
    {
        public VirusUnit(int _x_index, int _y_index)
            : base(_x_index, _y_index, (int)DataBase.EnemyUnitName.Virus)
        {

        }
    }
    class GanUnit : Unit
    {
        public GanUnit(int _x_index, int _y_index)
            : base(_x_index, _y_index, (int)DataBase.EnemyUnitName.Gan)
        {

        }
    }
    class KiseichuUnit : Unit
    {
        public KiseichuUnit(int _x_index, int _y_index)
            : base(_x_index, _y_index, (int)DataBase.EnemyUnitName.Kiseichu)
        {

        }
    }
}
