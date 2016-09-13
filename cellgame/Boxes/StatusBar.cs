using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonPart
{
    class StatusBar : WindowBar
    {
        #region Variable
        private int studyPower;
        private int PP;
        private int maxPP;
        private int leftUnit;
        private decimal bodyTemp;
        #endregion
        #region Method
        public StatusBar(int _studyPoint, int _PP, int _maxPP, int _leftUnit, decimal _bodyTemp)
            : base(DataBase.BarPos[3], DataBase.BarWidth[3], DataBase.BarHeight[3]) {
            Update(_studyPoint, _PP, _maxPP, _leftUnit, _bodyTemp);
        }
        public void Update(int _studyPower, int _PP, int _maxPP, int _leftUnit, decimal _bodyTemp)
        {
            base.Update();
            studyPower = _studyPower;
            PP = _PP;
            maxPP = _maxPP;
            leftUnit = _leftUnit;
            bodyTemp = _bodyTemp;
        }
        public override void Draw(Drawing d)
        {
            base.Draw(d);
            /*
            d.Draw(windowPosition + new Vector(20, 10), DataBase.status_tex[0], DepthID.Message);
            d.Draw(windowPosition + new Vector(170, 10), DataBase.status_tex[1], DepthID.Message);
            d.Draw(windowPosition + new Vector(320, 10), DataBase.status_tex[2], DepthID.Message);
            d.Draw(windowPosition + new Vector(470, 10), DataBase.status_tex[3], DepthID.Message);
            */
            new TextAndFont(string.Format("{0}",studyPower), Color.Black).Draw(d, windowPosition + new Vector(80, 20), DepthID.Message);
            new TextAndFont(string.Format("{0}/{1}", PP, maxPP), Color.Black).Draw(d, windowPosition + new Vector(230, 20), DepthID.Message);
            new TextAndFont(string.Format("{0}(+0)", leftUnit), Color.Black).Draw(d, windowPosition + new Vector(380, 20), DepthID.Message);
            new TextAndFont(string.Format("{0}", bodyTemp), Color.Black).Draw(d, windowPosition + new Vector(530, 20), DepthID.Message);
        }
        #endregion
    }// class end
}// namespace end
