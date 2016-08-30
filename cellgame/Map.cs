using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonPart {
    /// <summary>
    /// ステージごとのマップのオブジェクト
    /// </summary>
    class Map {
        #region Variable
        int[,] Data = new int[DataBase.MAP_MAX, DataBase.MAP_MAX];
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
        // 描画
        public void Draw(Drawing d, Vector camera, int Scale, Depth depth, int sizeX, int sizeY, Vector position)
        {
            for (int i = Math.Max(0, (int)(camera.X - DataBase.HexWidth * 3 / 2 * DataBase.MapScale[Scale]) / DataBase.HexWidth); i < DataBase.MAP_MAX && ((DataBase.HexWidth * i - camera.X) * DataBase.MapScale[Scale] - DataBase.HexWidth * 3 / 2 * DataBase.MapScale[Scale] <= sizeX); i++)
            {
                for (int j = Math.Max(0, (int)(camera.Y - DataBase.HexHeight * DataBase.MapScale[Scale]) / (DataBase.HexHeight * 3 / 4)); j < DataBase.MAP_MAX && (DataBase.HexHeight * 3 / 4 * j - camera.Y) * DataBase.MapScale[Scale] <= sizeY; j++)
                {
                    Vector drawp = DataBase.WhereDisp(i, j, camera, Scale);
                    if (drawp.Y < sizeY && drawp.Y >= -DataBase.HexHeight * DataBase.MapScale[Scale]
                        && drawp.X < sizeX && drawp.X >= -DataBase.HexWidth * 3 / 2 * DataBase.MapScale[Scale])
                    {
                        d.Draw(new Vector(position.X + drawp.X, position.Y + drawp.Y), DataBase.hex_tex[Data[i, j]], depth, (float)DataBase.MapScale[Scale]);
                    }
                }
            }
        }
        #endregion
    }// class end
}// namespace end
