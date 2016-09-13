using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace CommonPart {
    class StudyBar : WindowBar {
        #region Variable
        // 研究力
        int studyPower;
        // 研究進度
        int maxStudyPoint;
        int studyPoint;
        // ゲージオブジェクト
        Gauge gauge;
        // 研究名
        TextAndFont st;

        #endregion
        #region Method
        public StudyBar(int _maxStudyPoint, int _studyPoint, int _studyPower, string _studying)
            : base(DataBase.BarPos[0], DataBase.BarWidth[0], DataBase.BarHeight[0]) {
            studyPower = _studyPower;
            maxStudyPoint = _maxStudyPoint;
            studyPoint = _studyPoint;
            gauge = new Gauge(new Vector(340, 20), Color.CornflowerBlue, 0, maxStudyPoint, studyPoint, Color.AliceBlue);
            st = new TextAndFont(_studying, Color.Black);
        }
        public override void Draw(Drawing d)
        {
            base.Draw(d);
            gauge.Draw(d, windowPosition + new Vector(6, 35),DepthID.Status);
            st.Draw(d, windowPosition + new Vector(2, 0), DepthID.Status);
            new TextAndFont(string.Format("{0}/{1}　　{2}ターン",studyPoint ,maxStudyPoint, (maxStudyPoint - studyPoint + studyPower - 1) / studyPower), Color.Black).Draw(d, windowPosition + new Vector(120, 56), DepthID.Status);
        }
        #endregion
    }// class end
}// namespace end
