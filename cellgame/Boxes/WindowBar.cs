using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonPart;
using Microsoft.Xna.Framework.Input;

namespace CommonPart {
    /// <summary>
    /// プレイ画面上のバーのクラス
    /// </summary>
    class WindowBar {

        #region Variable
        // 描画する位置（左上）の座標
        public Vector windowPosition;
        // 横と縦のマス目(16x16)の数
        protected int width, height;
        
        #endregion

        #region Method

        public WindowBar(Vector _pos, int _w, int _h) {
            windowPosition = _pos;
            width = _w;
            height = _h;
        }

        public virtual void Draw(Drawing d) {
            // バーの背景を表示
            // 左上と右上
            d.Draw(windowPosition,DataBase.bar_frame_tex[0],DepthID.Message);
            d.Draw(windowPosition + new Vector((width - 1) * 16d, 0d), DataBase.bar_frame_tex[2], DepthID.Message);
            // 上下の中央
            for (int i = 1; i < width - 1;i++) {
                d.Draw(windowPosition + new Vector(i * 16d, 0d), DataBase.bar_frame_tex[1], DepthID.Message);
                d.Draw(windowPosition + new Vector(i * 16d, (height - 1) * 16d), DataBase.bar_frame_tex[7], DepthID.Message);
            }
            // 左下と右下
            d.Draw(windowPosition + new Vector(0d, (height - 1) * 16d), DataBase.bar_frame_tex[6], DepthID.Message);
            d.Draw(windowPosition + new Vector((width - 1) * 16d, (height - 1) * 16d), DataBase.bar_frame_tex[8], DepthID.Message);
            // 左右の中央
            for (int i = 1; i < height - 1; i++) {
                d.Draw(windowPosition + new Vector(0, i * 16), DataBase.bar_frame_tex[3], DepthID.Message);
                d.Draw(windowPosition + new Vector((width - 1) * 16d, i * 16d), DataBase.bar_frame_tex[5], DepthID.Message);
            }
            // 真ん中
            for (int i = 1; i < width - 1; i++) {
                for (int j = 1; j < height - 1; j++) {
                    d.Draw(windowPosition + new Vector(i * 16d, j * 16d), DataBase.bar_frame_tex[4], DepthID.Message);
                }
            }
        }

        public virtual void Update() { }

        // 位置がこのWindowBar 上のボタンの上にあるかどうか
        public virtual bool IsOnButton(int x, int y)
        {

            return false;
        }
        // 位置がこの WindowBar の上にあるかどうか
        public virtual bool IsOn(int x, int y) {
            return x >= windowPosition.X && x <= (windowPosition.X + width * 16) && y >= windowPosition.Y && y <= (windowPosition.Y + height * 16);
        }

        #endregion
    }// class end
}// namespace end
