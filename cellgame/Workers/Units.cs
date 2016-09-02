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
            : base(_x_index, _y_index, (int)DataBase.MyUnit.Kochu)
        {

        }
    }
    class MacroUnit : Unit
    {
        public MacroUnit(int _x_index, int _y_index)
            : base(_x_index, _y_index, (int)DataBase.MyUnit.Macro)
        {

        }
    }
    class JujoUnit : Unit
    {
        public JujoUnit(int _x_index, int _y_index)
            : base(_x_index, _y_index, (int)DataBase.MyUnit.Jujo)
        {

        }
    }
    class KosanUnit : Unit
    {
        public KosanUnit(int _x_index, int _y_index)
            : base(_x_index, _y_index, (int)DataBase.MyUnit.Kosan)
        {

        }
    }
    class NKUnit : Unit
    {
        public NKUnit(int _x_index, int _y_index)
            : base(_x_index, _y_index, (int)DataBase.MyUnit.NK)
        {

        }
    }
    class HelperTUnit : Unit
    {
        public HelperTUnit(int _x_index, int _y_index)
            : base(_x_index, _y_index, (int)DataBase.MyUnit.HelperT)
        {

        }
    }
    class KillerTUnit : Unit
    {
        public KillerTUnit(int _x_index, int _y_index)
            : base(_x_index, _y_index, (int)DataBase.MyUnit.KillerT)
        {

        }
    }
    class BUnit : Unit
    {
        public BUnit(int _x_index, int _y_index)
            : base(_x_index, _y_index, (int)DataBase.MyUnit.B)
        {

        }
    }
    class PlasmaUnit : Unit
    {
        public PlasmaUnit(int _x_index, int _y_index)
            : base(_x_index, _y_index, (int)DataBase.MyUnit.Plasma)
        {

        }
    }
    // 敵ユニット
    class KinUnit : Unit
    {
        public KinUnit(int _x_index, int _y_index)
            : base(_x_index, _y_index, (int)DataBase.EnemyUnit.Kin)
        {

        }
    }
    class KabiUnit : Unit
    {
        public KabiUnit(int _x_index, int _y_index)
            : base(_x_index, _y_index, (int)DataBase.EnemyUnit.Kabi)
        {

        }
    }
    class VirusUnit : Unit
    {
        public VirusUnit(int _x_index, int _y_index)
            : base(_x_index, _y_index, (int)DataBase.EnemyUnit.Virus)
        {

        }
    }
    class GanUnit : Unit
    {
        public GanUnit(int _x_index, int _y_index)
            : base(_x_index, _y_index, (int)DataBase.EnemyUnit.Gan)
        {

        }
    }
    class KiseichuUnit : Unit
    {
        public KiseichuUnit(int _x_index, int _y_index)
            : base(_x_index, _y_index, (int)DataBase.EnemyUnit.Kiseichu)
        {

        }
    }
}
