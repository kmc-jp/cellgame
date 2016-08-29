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
            : base(_x_index, _y_index, DataBase.MyUnitMAX_HP[0], DataBase.MyUnitMAX_LP[0], DataBase.MyUnitMAX_EXP[0]) {

        }
        public void Draw(Drawing d)
        {
            d.Draw(new Vector(),DataBase.hex_tex[0],DepthID.Player);
        }
    }
    class MacroUnit : Unit
    {
        public MacroUnit(int _x_index, int _y_index)
            : base(_x_index, _y_index, DataBase.MyUnitMAX_HP[1], DataBase.MyUnitMAX_LP[1], DataBase.MyUnitMAX_EXP[1]) {

        }
        public void Draw(Drawing d)
        {
            d.Draw(new Vector(), DataBase.hex_tex[1], DepthID.Player);
        }
    }
    class JujoUnit : Unit
    {
        public JujoUnit(int _x_index, int _y_index)
            : base(_x_index, _y_index, DataBase.MyUnitMAX_HP[2], DataBase.MyUnitMAX_LP[2], DataBase.MyUnitMAX_EXP[2]) {

        }
        public void Draw(Drawing d)
        {
            d.Draw(new Vector(), DataBase.hex_tex[2], DepthID.Player);
        }
    }
    class KosanUnit : Unit
    {
        public KosanUnit(int _x_index, int _y_index)
            : base(_x_index, _y_index, DataBase.MyUnitMAX_HP[3], DataBase.MyUnitMAX_LP[3], DataBase.MyUnitMAX_EXP[3]) {

        }
        public void Draw(Drawing d)
        {
            d.Draw(new Vector(), DataBase.hex_tex[3], DepthID.Player);
        }
    }
    class NKUnit : Unit
    {
        public NKUnit(int _x_index, int _y_index)
            : base(_x_index, _y_index, DataBase.MyUnitMAX_HP[4], DataBase.MyUnitMAX_LP[4], DataBase.MyUnitMAX_EXP[4]) {

        }
        public void Draw(Drawing d)
        {
            d.Draw(new Vector(), DataBase.hex_tex[4], DepthID.Player);
        }
    }
    class HelperTUnit : Unit
    {
        public HelperTUnit(int _x_index, int _y_index)
            : base(_x_index, _y_index, DataBase.MyUnitMAX_HP[5], DataBase.MyUnitMAX_LP[5], DataBase.MyUnitMAX_EXP[5]) {

        }
        public void Draw(Drawing d)
        {
            d.Draw(new Vector(), DataBase.hex_tex[5], DepthID.Player);
        }
    }
    class KillerTUnit : Unit
    {
        public KillerTUnit(int _x_index, int _y_index)
            : base(_x_index, _y_index, DataBase.MyUnitMAX_HP[6], DataBase.MyUnitMAX_LP[6], DataBase.MyUnitMAX_EXP[6]) {

        }
        public void Draw(Drawing d)
        {
            d.Draw(new Vector(), DataBase.hex_tex[6], DepthID.Player);
        }
    }
    class BUnit : Unit
    {
        public BUnit(int _x_index, int _y_index)
            : base(_x_index, _y_index, DataBase.MyUnitMAX_HP[7], DataBase.MyUnitMAX_LP[7], DataBase.MyUnitMAX_EXP[7]) {

        }
        public void Draw(Drawing d)
        {
            d.Draw(new Vector(), DataBase.hex_tex[7], DepthID.Player);
        }
    }
    class PlasmaUnit : Unit
    {
        public PlasmaUnit(int _x_index, int _y_index)
            : base(_x_index, _y_index, DataBase.MyUnitMAX_HP[8], DataBase.MyUnitMAX_LP[8], DataBase.MyUnitMAX_EXP[8]) {

        }
        public void Draw(Drawing d)
        {
            d.Draw(new Vector(), DataBase.hex_tex[8], DepthID.Player);
        }
    }
    // 敵ユニット

}
