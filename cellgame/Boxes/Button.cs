using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace CommonPart {
    class Button {
        #region Variable
        FilledBox front, back;
        Vector2 pos;
        DepthID depth;
        string str;
        #endregion
        #region Method
        public Button(Vector2 _pos, Vector2 _size, Color _center, Color _frame, string _str = "", DepthID _depth = DepthID.Message)
        {
            pos = _pos;
            front = new FilledBox(_size - new Vector2(10, 10), _center);
            back = new FilledBox(_size, _frame);
            depth = _depth;
            str = _str;
        }
        public void Draw(Drawing d)
        {
            back.Draw(d, pos, depth);
            front.Draw(d, pos - new Vector2(5, 5), depth + 1);
            new RichText(str).Draw(d, pos - new Vector2(5, 5), depth + 2, front.Y / 30);
        }
        public bool IsOn(Vector2 p)
        {
            return p.X >= pos.X && p.X <= pos.X + back.X && p.Y >= pos.Y && p.Y <= pos.Y + back.Y;
        }
        #endregion
    }
}
