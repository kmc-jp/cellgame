using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonPart;
using Microsoft.Xna.Framework.Input;

namespace CommonPart
{
    /// <summary>
    /// プレイ画面上のボックスのクラス
    /// </summary>
    class WindowBox
    {

        #region Variable
        // 描画する位置（左上）の座標
        public Vector windowPosition;
        // 横と縦のマス目(16x16)の数
        int width, height;
        // ボックスが今表示されているかどうか
        protected bool showing = true;
        // ボックスの非表示ボタンが左についているか右についているか
        protected bool leftHide = true;
        // 直前のマウスの状態
        MouseState pstate;

        #endregion

        #region Method

        public WindowBox(Vector _pos, int _w, int _h)
        {
            windowPosition = _pos;
            width = _w;
            height = _h;
            pstate = Mouse.GetState();
        }

        // 描画
        public void Draw(Drawing d)
        {
            // バーの背景を表示
            // 左上と右上
            if (leftHide)
            {
                if (showing)
                {
                    d.Draw(windowPosition, DataBase.box_flame[0], DepthID.Message);
                    d.Draw(windowPosition + new Vector((width - 1) * 16d, 0d), DataBase.bar_flame[2], DepthID.Message);
                }
                else
                {
                    d.Draw(windowPosition, DataBase.box_flame[5], DepthID.Message);
                }
            }
            else　if (showing)
            {
                d.Draw(windowPosition + new Vector((width - 1) * 16d, 0d), DataBase.box_flame[5], DepthID.Message);
                d.Draw(windowPosition, DataBase.bar_flame[0], DepthID.Message);
            }
            else
            {
                d.Draw(windowPosition + new Vector((width - 1) * 16d, 0d), DataBase.box_flame[0], DepthID.Message);
            }
            // 上下の中央
            if (showing)
            {
                for (int i = 1; i < width - 1; i++)
                {
                    d.Draw(windowPosition + new Vector(i * 16d, 0d), DataBase.bar_flame[1], DepthID.Message);
                    d.Draw(windowPosition + new Vector(i * 16d, (height - 1) * 16d), DataBase.bar_flame[7], DepthID.Message);
                }
            }
            // 左下と右下
            if (leftHide)
            {
                if (showing)
                {
                    d.Draw(windowPosition + new Vector(0d, (height - 1) * 16d), DataBase.box_flame[2], DepthID.Message);
                    d.Draw(windowPosition + new Vector((width - 1) * 16d, (height - 1) * 16d), DataBase.bar_flame[8], DepthID.Message);
                }
                else
                {
                    d.Draw(windowPosition + new Vector(0d, (height - 1) * 16d), DataBase.box_flame[7], DepthID.Message);
                }
            }
            else　if (showing)
            {
                d.Draw(windowPosition + new Vector((width - 1) * 16d, (height - 1) * 16d), DataBase.box_flame[7], DepthID.Message);
                d.Draw(windowPosition + new Vector(0d, (height - 1) * 16d), DataBase.bar_flame[6], DepthID.Message);
            }
            else
            {
                d.Draw(windowPosition + new Vector((width - 1) * 16d, (height - 1) * 16d), DataBase.box_flame[2], DepthID.Message);
            }
            // 左右の中央
            for (int i = 1; i < height - 1; i++)
            {
                if (leftHide)
                {
                    if (showing)
                    {
                        if (i == height / 2)
                            d.Draw(windowPosition + new Vector(0, i * 16), DataBase.box_flame[3], DepthID.Message);
                        else if (i == height / 2 + 1)
                            d.Draw(windowPosition + new Vector(0, i * 16), DataBase.box_flame[4], DepthID.Message);
                        else
                            d.Draw(windowPosition + new Vector(0, i * 16), DataBase.box_flame[1], DepthID.Message);
                        d.Draw(windowPosition + new Vector((width - 1) * 16d, i * 16d), DataBase.bar_flame[5], DepthID.Message);
                    }
                    else if (i == height / 2)
                        d.Draw(windowPosition + new Vector(0, i * 16), DataBase.box_flame[8], DepthID.Message);
                    else if (i == height / 2 + 1)
                        d.Draw(windowPosition + new Vector(0, i * 16), DataBase.box_flame[9], DepthID.Message);
                    else
                        d.Draw(windowPosition + new Vector(0, i * 16), DataBase.box_flame[6], DepthID.Message);
                }
                else
                {
                    if (showing)
                    {
                        if (i == height / 2)
                            d.Draw(windowPosition + new Vector((width - 1) * 16d, i * 16d), DataBase.box_flame[8], DepthID.Message);
                        else if (i == height / 2 + 1)
                            d.Draw(windowPosition + new Vector((width - 1) * 16d, i * 16d), DataBase.box_flame[9], DepthID.Message);
                        else
                            d.Draw(windowPosition + new Vector((width - 1) * 16d, i * 16d), DataBase.box_flame[6], DepthID.Message);
                        d.Draw(windowPosition + new Vector(0, i * 16), DataBase.bar_flame[3], DepthID.Message);
                    }
                    else if (i == height / 2)
                        d.Draw(windowPosition + new Vector((width - 1) * 16d, i * 16d), DataBase.box_flame[3], DepthID.Message);
                    else if (i == height / 2 + 1)
                        d.Draw(windowPosition + new Vector((width - 1) * 16d, i * 16d), DataBase.box_flame[4], DepthID.Message);
                    else
                        d.Draw(windowPosition + new Vector((width - 1) * 16d, i * 16d), DataBase.box_flame[1], DepthID.Message);
                }
            }
            // 真ん中
            if (showing)
            {
                for (int i = 1; i < width - 1; i++)
                {
                    for (int j = 1; j < height - 1; j++)
                    {
                        d.Draw(windowPosition + new Vector(i * 16d, j * 16d), DataBase.bar_flame[4], DepthID.Message);
                    }
                }
            }
        }

        public virtual void Update()
        {
            MouseState state = Mouse.GetState();
            if(state.LeftButton == ButtonState.Pressed && pstate.LeftButton != ButtonState.Pressed)
            {
                if (leftHide && state.X >= windowPosition.X && state.X <= (windowPosition.X + 16) && state.Y >= windowPosition.Y && state.Y <= (windowPosition.Y + height * 16))
                    showing = showing == false;
                else if(!leftHide && state.X >= (windowPosition.X + (width - 1) * 16) && state.X <= (windowPosition.X + width * 16) && state.Y >= windowPosition.Y && state.Y <= (windowPosition.Y + height * 16))
                    showing = showing == false;
            }
            pstate = state;
        }

        // 位置がこの WindowBar の上にあるかどうか
        public bool IsOn(int x, int y)
        {
            if (showing) return x >= windowPosition.X && x <= (windowPosition.X + width * 16) && y >= windowPosition.Y && y <= (windowPosition.Y + height * 16);
            else if (leftHide) return x >= windowPosition.X && x <= (windowPosition.X + 16) && y >= windowPosition.Y && y <= (windowPosition.Y + height * 16);
            else return x >= (windowPosition.X + (width - 1) * 16) && x <= (windowPosition.X + width * 16) && y >= windowPosition.Y && y <= (windowPosition.Y + height * 16);
        }

        #endregion
    }// class end
}// namespace end
