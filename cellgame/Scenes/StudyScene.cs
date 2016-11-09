using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CommonPart
{
    class StudyScene : Scene
    {
        #region Variable
        // 必要なPlayScene内の変数
        StudyBar sb;

        MouseState pstate;
        
        Button start;
        Button cancel;
        List<Button> studys;

        readonly Vector[] studyPos = {
            new Vector(9, 18), new Vector(277, 18), new Vector(145, 150), new Vector(413, 150), new Vector(9, 216), new Vector(277, 216), new Vector(545, 84), new Vector(545, 18), new Vector(813, 84), new Vector(813, 18), new Vector(681, 150),
        };
        bool[] studyAble = {
            false, false, false, false, false, false, false, false, false, false, false,
        };


        int select;
        #endregion

        #region Method
        public StudyScene(SceneManager s, StudyBar sb_) : base(s)
        {
            s.BackSceneNumber++;
            sb = sb_;
            pstate = Mouse.GetState();
            select = (int)StudyManager.studying;

            start = new Button(DataBase.studyTreePos + new Vector(795, 460), 120, new Color(255, 162, 0), Color.Black, "研究開始");
            cancel = new Button(DataBase.studyTreePos + new Vector(925, 460), 120, new Color(255, 162, 0), Color.Black, "キャンセル");

            studys = new List<Button>();
            for(int i = 0;i < 11; i++)
            {
                int p1 = DataBase.StudyParent[i, 0], p2 = DataBase.StudyParent[i, 1];
                Color c = new Color(100, 100, 100);
                if(p1 == -1 || StudyManager.IsDone((Study)p1))
                {
                    if (p2 == -1 || StudyManager.IsDone((Study)p2))
                    {
                        c = Color.SkyBlue;
                        if (StudyManager.IsDone((Study)i))
                        {
                            c = new Color(100, 100, 255);
                        }
                        else
                        {
                            studyAble[i] = true;
                            c = Color.SkyBlue;
                        }
                    }
                }
                if (StudyManager.IsDone((Study)i))
                {
                    c = Color.Blue;
                }
                studys.Add(new Button(DataBase.studyTreePos + studyPos[i], new Vector2(250, 30), c, i == select ? Color.Red : Color.Black, DataBase.StudyName[i]));
            }
        }
        public override void SceneDraw(Drawing d)
        {
            d.Draw(DataBase.studyTreePos, DataBase.tree_tex, DepthID.BackGroundFloor);
            base.Draw(d);

            cancel.Draw(d);
            start.Draw(d);
            for (int i = 0; i < 11; i++)
            {
                studys[i].Draw(d);
            }
            new TextAndFont(DataBase.StudyName[select], Color.Green).Draw(d, DataBase.studyTreePos + new Vector2(90, 280), DepthID.Message);
            new TextAndFont(string.Format("必要研究力：{1}", DataBase.StudyName[select], DataBase.maxStudyPower[select]), Color.Black).Draw(d, DataBase.studyTreePos + new Vector2(50, 310), DepthID.Message);
            new TextAndFont(DataBase.StudyExpl[select], Color.Black).Draw(d, DataBase.studyTreePos + new Vector2(50, 350), DepthID.Message);
        }
        public override void SceneUpdate()
        {
            MouseState state = Mouse.GetState();
            start.Update(pstate, state, studyAble[select]);
            cancel.Update(pstate, state);

            if (start.Clicked() && studyAble[select])
            {
                sb.StartStudying((Study)select);
                Delete = true;
            }
            if (cancel.Clicked())
            {
                Delete = true;
            }
            for (int i = 0; i < 11; i++)
            {
                studys[i].Update(pstate, state);
            }
            for (int i = 0; i < 11; i++)
            {
                if (studys[i].Clicked())
                {
                    studys[select].ChangeBackColor(Color.Black);
                    studys[i].ChangeBackColor(Color.Red);
                    if (studyAble[i])
                    {
                        start.ChangeColor(new Color(255, 162, 0));
                    }
                    else
                    {
                        start.ChangeColor(Color.White);
                    }
                    select = i;
                }
            }
            pstate = state;
        }
        #endregion
    }
}
