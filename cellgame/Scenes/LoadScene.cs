using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CommonPart
{
    class LoadScene : Scene
    {
        #region Variable
        WindowBar wb;
        Button start, cancel;
        MouseState pstate;
        List<string> fileNames;
        int select = -1;
        const int pageMax = 8;
        int pagen = 0;
        int Page {
            get {
                return pagen;
            }
            set {
                pagen = Math.Max(0, Math.Min(Math.Max((fileNames.Count + pageMax - 1) / pageMax - 1, 0), value));
            }
        }
        BlindButton prePage, nexPage;
        BoxFrame bf;
        #endregion

        #region Method
        public LoadScene(SceneManager s)
            : base(s) {
            wb = new WindowBar(new Vector(235, 250), 24, 16);
            start = new Button(new Vector2(700, 600), 160, new Color(255, 155, 79), new Color(255, 111, 0), "ゲームスタート");
            cancel = new Button(new Vector2(900, 600), 160, new Color(255, 155, 79), new Color(255, 111, 0), "キャンセル");
            prePage = new BlindButton(new Vector2(235, 250), new Vector2(15, 350));
            nexPage = new BlindButton(new Vector2(603, 250), new Vector2(15, 350));
            bf = new BoxFrame(new Vector2(348, 30), Color.Black);
            pstate = Mouse.GetState();
            fileNames = new List<string>();

            var x = new System.IO.DirectoryInfo(@"SaveData");
            foreach(string fn in x.GetFiles("*.save").Select(fileinfo => fileinfo.Name.Remove(fileinfo.Name.Length - 5, 5)))
            {
                fileNames.Add(fn);
            }
        }
        public override void SceneUpdate()
        {
            if (cancel.Clicked()) Delete = true;
            if (start.Clicked() && select != -1)
            {
                Delete = true;
                new PlayScene(scenem, 0, fileNames[select]);
            }
            
            MouseState state = Mouse.GetState();
            start.Update(pstate, state);
            cancel.Update(pstate, state);
            prePage.Update(pstate, state, Page > 0);
            nexPage.Update(pstate, state, Page < Math.Max((fileNames.Count + pageMax - 1) / pageMax - 1, 0));

            if (prePage.Clicked())
            {
                Page--;
                select = -1;
            }
            if (nexPage.Clicked())
            {
                Page++;
                select = -1;
            }

            bool on = false;
            for (int i = 0;i < Math.Min(fileNames.Count - Page * pageMax, pageMax);i++)
            {
                if (state.X >= 250 && state.X <= 603 && state.Y >= 250 + i * 30 && state.Y <= 280 + i * 30)
                {
                    on = true;
                    if(pstate.LeftButton == ButtonState.Released && state.LeftButton == ButtonState.Pressed)
                    {
                        select = i + Page * pageMax;
                    }
                    break;
                }
            }

            if (on)
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Hand;

            pstate = state;
            base.SceneUpdate();
        }
        public override void SceneDraw(Drawing d)
        {
            wb.Draw(d);
            new TextAndFont("ロードゲーム", Color.Black).Draw(d, new Vector2(250, 180), DepthID.Message);

            for (int i = Page * pageMax;i < Math.Min((Page + 1) * pageMax, fileNames.Count);i++)
            {
                new TextAndFont(fileNames[i], Color.Black).Draw(d, new Vector(250, 250 + (i - Page * pageMax) * 30), DepthID.Message);
            }
            if (select != -1)
            {
                bf.Draw(d, new Vector2(251, 250 + 30 * (select - Page * pageMax)), DepthID.Message);
            }
            if (Page != 0)
            {
                d.Draw(new Vector2(235, 250), DataBase.minimapButton[0], DepthID.Message);
            }
            if (Page != Math.Max((fileNames.Count + pageMax - 1) / pageMax - 1, 0))
            {
                d.Draw(new Vector2(603, 250), DataBase.minimapButton[1], DepthID.Message);
            }
            start.Draw(d);
            cancel.Draw(d);
            base.SceneDraw(d);
        }
        #endregion
    }
}
