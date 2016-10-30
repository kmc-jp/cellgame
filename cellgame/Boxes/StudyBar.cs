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

        int _studyPower;
        int StudyPower {
            get { return _studyPower; }
            set { _studyPower = Math.Max(0, Math.Min(value, DataBase.maxStudyPower[studying])); }
        }
        
        #endregion
        #region Method
        public StudyBar()
            : base(DataBase.BarPos[0], DataBase.BarWidth[0], DataBase.BarHeight[0]) {
            pstudying = studying = (int)Study.Kaku;
            button = new BlindButton(windowPosition, new Vector(width * 16d, height * 16d));
            
            StudyPower = 0;
            nameTex = new TextAndFont(DataBase.StudyName[studying], Color.Black);
            gauge = new Gauge(new Vector(340, 20), Color.CornflowerBlue, 0, DataBase.maxStudyPower[studying], StudyPower, Color.AliceBlue);
        }
        public override void Draw(Drawing d)
        {
            base.Draw(d);
            gauge.Draw(d, windowPosition + new Vector(6, 35),DepthID.Status);
            nameTex.Draw(d, windowPosition + new Vector(2, 0), DepthID.Status);
            new TextAndFont(string.Format("{0}/{1}　　{2}ターン", StudyPower , DataBase.maxStudyPower[studying], (DataBase.maxStudyPower[studying] - StudyPower + PlayScene.studyPower - 1) / PlayScene.studyPower), Color.Black).Draw(d, windowPosition + new Vector(120, 56), DepthID.Status);
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
            StudyPower = 0;
            nameTex = new TextAndFont(DataBase.StudyName[studying], Color.Black);
            gauge = new Gauge(new Vector(340, 20), Color.CornflowerBlue, 0, DataBase.maxStudyPower[studying], StudyPower, Color.AliceBlue);
        }
        public void UpdateTurn()
        {
            StudyPower += PlayScene.studyPower;
            if (StudyPower == DataBase.maxStudyPower[studying]) DataBase.StudyState[studying] = true;
            gauge = new Gauge(new Vector(340, 20), Color.CornflowerBlue, 0, DataBase.maxStudyPower[studying], StudyPower, Color.AliceBlue);
        }
        #endregion
    }// class end
}// namespace end
