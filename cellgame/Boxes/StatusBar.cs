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
        private int studyPoint;
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
        public void Update(int _studyPoint, int _PP, int _maxPP, int _leftUnit, decimal _bodyTemp)
        {
            base.Update();
            studyPoint = _studyPoint;
            PP = _PP;
            maxPP = _maxPP;
            leftUnit = _leftUnit;
            bodyTemp = _bodyTemp;
        }
        public override void Draw(Drawing d)
        {
            base.Draw(d);

        }
        #endregion
    }// class end
}// namespace end
