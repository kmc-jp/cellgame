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
        int studyPoint;
        int productPoint;
        int leftUnit;
        decimal bodyTemp;
        #endregion
        #region Method
        public StatusBar(int _studyPoint, int _productPoint, int _leftUnit, decimal _bodyTemp)
            : base(DataBase.BarPos[3], DataBase.BarWidth[3], DataBase.BarHeight[3]) {
            studyPoint = _studyPoint;
            productPoint = _productPoint;
            leftUnit = _leftUnit;
            bodyTemp = _bodyTemp;
        }
        public void Update(int _studyPoint, int _productPoint, int _leftUnit, decimal _bodyTemp)
        {
            base.Update();
            studyPoint = _studyPoint;
            productPoint = _productPoint;
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
