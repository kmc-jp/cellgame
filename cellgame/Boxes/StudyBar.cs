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
        public Gauge gauge;

        BlindButton button;
        public TextAndFont nameTex;

        
        #endregion
        #region Method
        public StudyBar()
            : base(DataBase.BarPos[0], DataBase.BarWidth[0], DataBase.BarHeight[0]) {
            button = new BlindButton(windowPosition, new Vector(width * 16d, height * 16d));
            
            nameTex = new TextAndFont(StudyManager.StudyName, Color.Black);
            gauge = new Gauge(new Vector(340, 20), Color.Blue, 0, StudyManager.MaxStudyPower, StudyManager.StudyPower, Color.SkyBlue);
        }
        public override void Draw(Drawing d)
        {
            base.Draw(d);
            gauge.Draw(d, windowPosition + new Vector(6, 35),DepthID.Status);
            nameTex.Draw(d, windowPosition + new Vector(2, 0), DepthID.Status);
            new TextAndFont(string.Format("{0}/{1}", StudyManager.StudyPower, StudyManager.MaxStudyPower), Color.Black).Draw(d, windowPosition + new Vector(120, 56), DepthID.Status);
            new TextAndFont(StudyManager.LeftTurn > 0 ? string.Format("{0}ターン", StudyManager.LeftTurn) : "完了", Color.Black).Draw(d, windowPosition + new Vector(220, 56), DepthID.Status);
        }
        public void Update(MouseState pstate, MouseState state, SceneManager s)
        {
            base.Update();
            button.Update(pstate, state);
            if (button.Clicked())
            {
                new StudyScene(s, this);
            }
        }
        public void StartStudying(Study st)
        {
            if (StudyManager.StartStudying(st))
            {
                nameTex = new TextAndFont(StudyManager.StudyName, Color.Black);
                gauge = new Gauge(new Vector(340, 20), Color.Blue, 0, StudyManager.MaxStudyPower, StudyManager.StudyPower, Color.SkyBlue);
            }
        }
        public void UpdateTurn()
        {
            StudyManager.UpdateTurn();
            gauge = new Gauge(new Vector(340, 20), Color.Blue, 0, StudyManager.MaxStudyPower, StudyManager.StudyPower, Color.SkyBlue);
        }
        #endregion
    }// class end
}// namespace end
