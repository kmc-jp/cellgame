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
            {
                for (int j = 0; j < DataBase.MAP_MAX; j++)
                {
                    Data[i,j] = 1;
                }
            }
        }
        // へクスの取得
        public int GetState(int x, int y)
        {
            if (x >= 0 && x < DataBase.MAP_MAX && y >= 0 && y < DataBase.MAP_MAX) return Data[x,y];
            else return 0;
        }
        // へクスの変更
        public void ChangeState(int x, int y, int state)
        {
            if (x >= 0 && x < DataBase.MAP_MAX && y >= 0 && y < DataBase.MAP_MAX && state >= 0 && state < DataBase.hex.Count) Data[x,y] = state;
        }
        // 描画
        public void Draw(Drawing d, Vector camera, int Scale, Depth depth, int sizeX, int sizeY, Vector position) {
            for (int i = Math.Max(0, (int)(camera.X-DataBase.HexWidth*3/2*DataBase.MapScale[Scale])/DataBase.HexWidth); i < DataBase.MAP_MAX; i++)
            {
                double drawx = (DataBase.HexWidth * i - camera.X) * DataBase.MapScale[Scale];
                for (int j = Math.Max(0, (int)(camera.Y - DataBase.HexHeight*DataBase.MapScale[Scale]) / (DataBase.HexHeight*3/4)); j < DataBase.MAP_MAX; j++)
                {
                    double drawy = (DataBase.HexHeight * 3 / 4 * j - camera.Y) * DataBase.MapScale[Scale];
                    if (drawy < sizeY && drawy >= -DataBase.HexHeight*DataBase.MapScale[Scale]
                        && drawx < sizeX && drawx >= -DataBase.HexWidth * 3 / 2 * DataBase.MapScale[Scale])
                    {
                        if (j % 2 == 1)
                        {
                            d.Draw(new Vector(position.X + drawx + DataBase.HexWidth / 2 * DataBase.MapScale[Scale], position.Y + drawy), DataBase.hex[Data[i,j]], depth, (float)DataBase.MapScale[Scale]);
                        }
                        else {
                            d.Draw(new Vector(position.X + drawx, position.Y + drawy), DataBase.hex[Data[i,j]], depth, (float)DataBase.MapScale[Scale]);
                        }
                    }
                    else if(drawy > sizeY)
                    {
                        break;
                    }
                }
                if (drawx - DataBase.HexWidth * 3 / 2 * DataBase.MapScale[Scale] > sizeX)
                {
                    break;
                }
            }
        }
        #endregion
    }// class end
}// namespace end
