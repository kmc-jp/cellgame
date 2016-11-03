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
            d.Draw(windowPosition + new Vector(50, 0), DataBase.studyIcon, DepthID.Message);
            d.Draw(windowPosition + new Vector(250, 0), DataBase.productIcon, DepthID.Message);
            d.Draw(windowPosition + new Vector(450, 0), DataBase.temperIcon, DepthID.Message);
            new TextAndFont(string.Format("{0}",PlayScene.studyPower), Color.Black).Draw(d, windowPosition + new Vector(130, 20), DepthID.Message);
            new TextAndFont(string.Format("{0}/{1}", PlayScene.productPower, PlayScene.maxProductPower), Color.Black).Draw(d, windowPosition + new Vector(330, 20), DepthID.Message);
            new TextAndFont(string.Format("{0}", PlayScene.bodyTemp), Color.Black).Draw(d, windowPosition + new Vector(530, 20), DepthID.Message);
        }
        #endregion
    }// class end
}// namespace end
