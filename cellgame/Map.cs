using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace CommonPart {
    /// <summary>
    /// ステージごとのマップのオブジェクト
    /// </summary>
    class Map {
        #region Variable
        public int[,] Data = new int[DataBase.MAP_MAX, DataBase.MAP_MAX];
        #endregion
        #region Method
        // コンストラクタ
        public Map() {
            for(int i = 0; i < DataBase.MAP_MAX; i++)
                for (int j = 0; j < DataBase.MAP_MAX; j++)
                    Data[i,j] = 1;
        }
        // へクスの取得
        public int GetState(int x_index, int y_index)
        {
            if (x_index >= 0 && x_index < DataBase.MAP_MAX && y_index >= 0 && y_index < DataBase.MAP_MAX) return Data[x_index, y_index];
            else return 0;
        }
        // へクスの変更
        public void ChangeState(int x_index, int y_index, int state)
        {
            if (x_index >= 0 && x_index < DataBase.MAP_MAX && y_index >= 0 && y_index < DataBase.MAP_MAX && state >= 0 && state < DataBase.hex_tex.Count) Data[x_index, y_index] = state;
        }
        // マップの描画
        public void Draw(Drawing d, Vector camera, int Scale)
        {
            for (int i = Math.Max(0, (int)(camera.X - DataBase.HexWidth * 3 / 2 * DataBase.MapScale[Scale]) / DataBase.HexWidth); i < DataBase.MAP_MAX && ((DataBase.HexWidth * i - camera.X) * DataBase.MapScale[Scale] - DataBase.HexWidth * 3 / 2 * DataBase.MapScale[Scale] <= Game1._WindowSizeX); i++)
            {
                for (int j = Math.Max(0, (int)(camera.Y - DataBase.HexHeight * DataBase.MapScale[Scale]) / (DataBase.HexHeight * 3 / 4)); j < DataBase.MAP_MAX && (DataBase.HexHeight * 3 / 4 * j - camera.Y) * DataBase.MapScale[Scale] <= Game1._WindowSizeY; j++)
                {
                    Vector drawp = DataBase.WhereDisp(i, j, camera, Scale);
                    if (drawp.Y <= Game1._WindowSizeY && drawp.Y >= -DataBase.HexHeight * DataBase.MapScale[Scale]
                        && drawp.X <= Game1._WindowSizeX && drawp.X >= -DataBase.HexWidth * 3 / 2 * DataBase.MapScale[Scale])
                    {
                        d.Draw(new Vector(drawp.X, drawp.Y), DataBase.hex_tex[Data[i, j]], DepthID.BackGroundFloor, (float)DataBase.MapScale[Scale]);
                    }
                }
            }
        }
        // ミニマップの描画
        public void DrawMinimap(Drawing d, UnitManager um, Vector camera, Depth depth, Depth depth2, int sizeX, int sizeY, Vector position)
        {
            const int mini_hexwidth = 18, mini_hexheight = 20;
            for (int i = Math.Max(0, (int)(camera.X - mini_hexwidth * 3 / 2) / DataBase.HexWidth - 1); i < DataBase.MAP_MAX && ((DataBase.HexWidth * i - camera.X) * DataBase.MapScale[0] - mini_hexwidth * 3 / 2 <= sizeX); i++)
            {
                for (int j = Math.Max(0, (int)(camera.Y - mini_hexheight) / (DataBase.HexHeight * 3 / 4)); j < DataBase.MAP_MAX && (DataBase.HexHeight * 3 / 4 * j - camera.Y) * DataBase.MapScale[0] <= sizeY; j++)
                {
                    Vector drawp = DataBase.WhereDisp(i, j, camera, 0);
                    Rectangle rect = new Rectangle(0, 0, mini_hexwidth, mini_hexheight);
                    if (drawp.X < 0)
                    {
                        rect.X = Math.Min((int)(-drawp.X), rect.Width);
                        rect.Width -= rect.X;
                    }
                    if (drawp.X > sizeX - mini_hexwidth)
                    {
                        rect.Width = Math.Max((int)((sizeX - drawp.X)) - rect.X, 0);
                    }
                    drawp.X += rect.X;
                    if (drawp.Y < 0)
                    {
                        rect.Y = Math.Min((int)(-drawp.Y), rect.Height);
                        rect.Height -= rect.Y;
                    }
                    if (drawp.Y > sizeY - mini_hexheight)
                    {
                        rect.Height = Math.Max((int)((sizeY - drawp.Y)) - rect.Y, 0);
                    }
                    drawp.Y += rect.Y;
                    if (rect.Width > 0 && rect.Height > 0)
                    {
                        d.Draw(new Vector(position.X + drawp.X, position.Y + drawp.Y), DataBase.mini_hex_tex[Data[i, j]], rect, depth);
                        if (um.FindType(i, j) > 0)
                        {
                            d.Draw(new Vector(position.X + drawp.X, position.Y + drawp.Y), DataBase.miniUnit_tex[0], rect, depth2);
                        }
                        else if (um.FindType(i, j) < 0)
                        {
                            d.Draw(new Vector(position.X + drawp.X, position.Y + drawp.Y), DataBase.miniUnit_tex[1], rect, depth2);
                        }
                    }
                }
            }
        }
        #endregion
    }// class end
}// namespace end
