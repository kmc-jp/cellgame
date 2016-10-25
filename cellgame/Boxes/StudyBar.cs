using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CommonPart {
    class StudyBar : WindowBar {
        #region Variable
        // ゲージオブジェクト
        Gauge gauge;

        BlindButton button;
        TextAndFont nameTex;
        // 研究名
        int pstudying;
        public int studying;

        int _studyPoint;
        int StudyPoint {
            get { return _studyPoint; }
            set { _studyPoint = Math.Max(0, Math.Min(value, DataBase.maxStudyPoint[studying])); }
        }
        
        #endregion
        #region Method
        public StudyBar()
            : base(DataBase.BarPos[0], DataBase.BarWidth[0], DataBase.BarHeight[0]) {
            pstudying = studying = (int)DataBase.Study.Kaku;
            button = new BlindButton(windowPosition, new Vector(width * 16d, height * 16d));
            
            StudyPoint = 0;
            nameTex = new TextAndFont(DataBase.StudyName[studying], Color.Black);
            gauge = new Gauge(new Vector(340, 20), Color.CornflowerBlue, 0, DataBase.maxStudyPoint[studying], StudyPoint, Color.AliceBlue);
        }
        public override void Draw(Drawing d)
        {
            base.Draw(d);
            gauge.Draw(d, windowPosition + new Vector(6, 35),DepthID.Status);
            nameTex.Draw(d, windowPosition + new Vector(2, 0), DepthID.Status);
            new TextAndFont(string.Format("{0}/{1}　　{2}ターン", StudyPoint , DataBase.maxStudyPoint[studying], (DataBase.maxStudyPoint[studying] - StudyPoint + PlayScene.studyPoint - 1) / PlayScene.studyPoint), Color.Black).Draw(d, windowPosition + new Vector(120, 56), DepthID.Status);
        }
        public void Update(MouseState pstate, MouseState state, SceneManager s)
        {
            base.Update();
            button.Update(pstate, state);
            if (button.Clicked())
            {
                new StudyScene(s, this);
            }

            if (pstudying == studying) return;

            pstudying = studying;
            StudyPoint = 0;
            nameTex = new TextAndFont(DataBase.StudyName[studying], Color.Black);
            gauge = new Gauge(new Vector(340, 20), Color.CornflowerBlue, 0, DataBase.maxStudyPoint[studying], StudyPoint, Color.AliceBlue);
        }
        public void UpdateTurn()
        {
            StudyPoint += PlayScene.studyPoint;
            gauge = new Gauge(new Vector(340, 20), Color.CornflowerBlue, 0, DataBase.maxStudyPoint[studying], StudyPoint, Color.AliceBlue);
        }
        #endregion
    }// class end
}// namespace end
