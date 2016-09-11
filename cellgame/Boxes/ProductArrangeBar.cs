using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CommonPart
{
    class ProductArrangeBar : WindowBar
    {
        #region Variable
        ProductBox productBox;
        ArrangeBox arrangeBox;
        #endregion
        #region Method
        public ProductArrangeBar()
            : base(DataBase.BarPos[4], DataBase.BarWidth[4], DataBase.BarHeight[4]) {
            productBox = new ProductBox();
            arrangeBox = new ArrangeBox();
        }
        public void Update(MouseState pstate, MouseState state)
        {
            base.Update();
            if(pstate.LeftButton == ButtonState.Released && state.LeftButton == ButtonState.Pressed)
            {
                if (state.X >= windowPosition.X && state.X <= windowPosition.X +width / 2 * 16 && state.Y >= windowPosition.Y && state.Y <= windowPosition.Y + height * 16)
                {// 左のボタンをクリック
                    productBox.Show();
                    arrangeBox.Hide();
                }
                else if(state.X >= windowPosition.X + width / 2 * 16 && state.X <= windowPosition.X + width * 16 && state.Y >= windowPosition.Y && state.Y <= windowPosition.Y + height * 16)
                {// 右のボタンをクリック
                    arrangeBox.Show();
                    productBox.Hide();
                }
                else if (!IsOn(state.X, state.Y))
                {
                    arrangeBox.Hide();
                    productBox.Hide();
                }
            }
        }
        public override bool IsOn(int x, int y)
        {
            return base.IsOn(x, y) || productBox.IsOn(x, y) || arrangeBox.IsOn(x, y);
        }
        public override void Draw(Drawing d)
        {
            base.Draw(d);
            productBox.Draw(d);
            arrangeBox.Draw(d);
        }
        #endregion
    }// class end
}// namespace end
