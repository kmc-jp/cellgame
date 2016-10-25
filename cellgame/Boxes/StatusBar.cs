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
        #endregion
        #region Method
        public StatusBar()
            : base(DataBase.BarPos[3], DataBase.BarWidth[3], DataBase.BarHeight[3]) {
        }
        public override void Update()
        {
            base.Update();
        }
        public override void Draw(Drawing d)
        {
            base.Draw(d);
            /*
            d.Draw(windowPosition + new Vector(20, 10), DataBase.status_tex[0], DepthID.Message);
            d.Draw(windowPosition + new Vector(170, 10), DataBase.status_tex[1], DepthID.Message);
            d.Draw(windowPosition + new Vector(470, 10), DataBase.status_tex[2], DepthID.Message);
            */
            new TextAndFont(string.Format("{0}",PlayScene.studyPoint), Color.Black).Draw(d, windowPosition + new Vector(80, 20), DepthID.Message);
            new TextAndFont(string.Format("{0}/{1}", PlayScene.PP, PlayScene.maxPP), Color.Black).Draw(d, windowPosition + new Vector(230, 20), DepthID.Message);
            new TextAndFont(string.Format("{0}", PlayScene.bodyTemp), Color.Black).Draw(d, windowPosition + new Vector(530, 20), DepthID.Message);
        }
        #endregion
    }// class end
}// namespace end
