using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace CommonPart
{
    class ProductBox : WindowBar
    {
        #region Variable
        bool showing = false;
        #endregion
        #region Method
        public ProductBox()
            : base(DataBase.BarPos[5], DataBase.BarWidth[5], DataBase.BarHeight[5]) {
        }
        public void Show()
        {
            showing = true;
        }
        public void Hide()
        {
            showing = false;
        }
        public bool IsOn(int x, int y)
        {
            return showing  && base.IsOn(x, y);
        }
        public override void Draw(Drawing d)
        {
            if (showing)
            {
                base.Draw(d);
                for(int i = 0; i < 9; i++)
                {
                    new TextAndFont(DataBase.MyUnitName[i], FontID.Medium, Color.Black).Draw(d, windowPosition + new Vector(10, 10 + 30 * i), DepthID.Message);
                }
            }
        }
        #endregion
    }// class end

    class ArrangeBox : WindowBar
    {
        #region Variable
        bool showing = false;
        #endregion
        #region Method
        public ArrangeBox()
            : base(DataBase.BarPos[5], DataBase.BarWidth[5], DataBase.BarHeight[5])
        {
        }
        public void Show()
        {
            showing = true;
        }
        public void Hide()
        {
            showing = false;
        }
        public bool IsOn(int x, int y)
        {
            return showing && base.IsOn(x, y);
        }
        public override void Draw(Drawing d)
        {
            if (showing)
            {
                base.Draw(d);
            }
        }
        #endregion
    }// class end
}// namespace end
