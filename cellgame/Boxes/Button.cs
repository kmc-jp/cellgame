using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CommonPart {
    class BlindButton
    {
        #region Variable
        Vector2 pos;
        Vector2 size;
        bool pressed = false;
        bool clicked = false;
        #endregion
        #region Method
        public BlindButton(Vector2 _pos, Vector2 _size)
        {
            pos = _pos;
            size = _size;
        }
        public bool IsOn()
        {
            return IsOn(Mouse.GetState());
        }
        public bool IsOn(MouseState s)
        {
            return s.X >= pos.X && s.X <= pos.X + size.X && s.Y >= pos.Y && s.Y <= pos.Y + size.Y;
        }
        public void Update(MouseState pstate, MouseState state, bool flag = true)
        {
            clicked = (pstate.LeftButton == ButtonState.Pressed && state.LeftButton == ButtonState.Released && IsOn(state) && pressed);

            if (pstate.LeftButton == ButtonState.Released && state.LeftButton == ButtonState.Pressed && IsOn(state)) pressed = true;
            else if (state.LeftButton == ButtonState.Released) pressed = false;

            if (flag && IsOn(state)) System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Hand;
        }
        public bool Clicked()
        {
            return clicked;
        }
        public void MoveTo(Vector2 pos2)
        {
            pos = pos2;
        }
        #endregion
    }
    class Button {
        #region Variable
        FilledBox front, back;
        Vector2 pos;
        DepthID depth;
        string str;
        TextAndFont tex;
        Color color, backColor;
        bool pressed = false;
        bool clicked = false;
        #endregion
        #region Method
        public Button(Vector2 _pos, float sizeX, Color _color, Color _backColor, string _str = "", DepthID _depth = DepthID.Message)
        {
            pos = _pos;
            color = _color;
            backColor = _backColor;
            str = _str;
            depth = _depth;
            front = new FilledBox(new Vector2(sizeX - 6, 42), color);
            back = new FilledBox(new Vector2(sizeX, 48), backColor);
            tex = new TextAndFont(str, Color.Black);
        }
        public Button(Vector2 _pos, Vector2 size, Color _color, Color _backColor, string _str = "", DepthID _depth = DepthID.Message)
        {
            pos = _pos;
            color = _color;
            backColor = _backColor;
            str = _str;
            depth = _depth;
            front = new FilledBox(new Vector2(size.X - 6, size.Y - 6), color);
            back = new FilledBox(new Vector2(size.X, size.Y), backColor);
            tex = new TextAndFont(str, Color.Black);
        }
        public void Draw(Drawing d)
        {
            front = new FilledBox(front.Size, pressed ? new Color(color.R * 3 / 4, color.G * 3 / 4, color.B * 3 / 4) : color);

            back.Draw(d, pos, depth);
            front.Draw(d, pos + new Vector2(3, 3), depth + 1);
            tex.Draw(d, pos + new Vector2(back.Size.X / 2 - str.Length * 10, front.Size.Y / 2 - 10), depth + 2);
        }
        public bool IsOn(MouseState s)
        {
            return s.X >= pos.X && s.X <= pos.X + back.X && s.Y >= pos.Y && s.Y <= pos.Y + back.Y;
        }
        public void Update(MouseState pstate, MouseState state, bool flag = true)
        {
            clicked = (pstate.LeftButton == ButtonState.Pressed && state.LeftButton == ButtonState.Released && IsOn(state) && pressed);

            if (pstate.LeftButton == ButtonState.Released && state.LeftButton == ButtonState.Pressed && IsOn(state)) pressed = true;
            else if (state.LeftButton == ButtonState.Released) pressed = false;

            if (flag && IsOn(state)) System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Hand;
        }
        public bool Clicked()
        {
            return clicked;
        }
        public void MoveTo(Vector2 pos2)
        {
            pos = pos2;
        }
        public void ChangeColor(Color _color)
        {
            color = _color;
            front = new FilledBox(front.Size, color);
        }
        public void ChangeBackColor(Color _backColor)
        {
            backColor = _backColor;
            back = new FilledBox(back.Size, backColor);
        }
        public void ChangeColor(Color _color, Color _backColor)
        {
            color = _color;
            backColor = _backColor;
            front = new FilledBox(front.Size, color);
            back = new FilledBox(back.Size, backColor);
        }
        #endregion
    }
}
