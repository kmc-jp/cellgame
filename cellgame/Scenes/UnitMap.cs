using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonPart
{
    class UnitMap
    {
        public Unit[,] data;

        // コンストラクタ
        public UnitMap()
        {
            // ユニットのマップ情報の初期化
            data = new Unit[DataBase.MAP_MAX + (DataBase.MAP_MAX + 1) / 2, DataBase.MAP_MAX];
            for (int i = 0; i < DataBase.MAP_MAX + (DataBase.MAP_MAX + 1) / 2; i++)
            {
                for (int j = 0; j < DataBase.MAP_MAX; j++)
                {
                    data[i, j] = new Unit(UnitType.NULL);
                }
            }
        }
        public UnitMap(UnitMap _uMap)
        {
            // ユニットのマップ情報の初期化
            data = new Unit[DataBase.MAP_MAX + (DataBase.MAP_MAX + 1) / 2, DataBase.MAP_MAX];
            for (int i = 0; i < DataBase.MAP_MAX + (DataBase.MAP_MAX + 1) / 2; i++)
            {
                for (int j = 0; j < DataBase.MAP_MAX; j++)
                {
                    data[i, j] = _uMap.data[i, j];
                }
            }
        }
        public void Draw(Drawing d, Vector camera, int Scale, int select_x, int select_y)
        {
            for (int i = Math.Max(0, (int)(camera.X - DataBase.HexWidth * 3 / 2 * DataBase.MapScale[Scale]) / DataBase.HexWidth); i < DataBase.MAP_MAX && ((DataBase.HexWidth * i - camera.X) * DataBase.MapScale[Scale] - DataBase.HexWidth * 3 / 2 * DataBase.MapScale[Scale] <= Game1._WindowSizeX); i++)
            {
                for (int j = Math.Max(0, (int)(camera.Y - DataBase.HexHeight * DataBase.MapScale[Scale]) / (DataBase.HexHeight * 3 / 4)); j < DataBase.MAP_MAX && (DataBase.HexHeight * 3 / 4 * j - camera.Y) * DataBase.MapScale[Scale] <= Game1._WindowSizeY; j++)
                {
                    Vector drawp = DataBase.WhereDisp(i, j, camera, Scale);
                    if (drawp.Y <= Game1._WindowSizeY && drawp.Y >= -DataBase.HexHeight * DataBase.MapScale[Scale]
                        && drawp.X <= Game1._WindowSizeX && drawp.X >= -DataBase.HexWidth * 3 / 2 * DataBase.MapScale[Scale] && data[i + (j + 1) /2, j].type != UnitType.NULL && !(i == select_x && j == select_y))
                    {
                        d.Draw(new Vector(drawp.X, drawp.Y) + new Vector(26 * DataBase.MapScale[Scale], 36 * DataBase.MapScale[Scale]), data[i + (j + 1) /2, j].type > 0 ? DataBase.myUnit_tex[(int)data[i + (j + 1) /2, j].type - 1] : DataBase.enemyUnit_tex[(int)data[i + (j + 1) /2, j].type + 5], DepthID.Player, (float)DataBase.MapScale[Scale]);
                    }
                }
            }
        }
        public void Draw(Drawing d, Vector camera, int Scale)
        {
            for (int i = Math.Max(0, (int)(camera.X - DataBase.HexWidth * 3 / 2 * DataBase.MapScale[Scale]) / DataBase.HexWidth); i < DataBase.MAP_MAX && ((DataBase.HexWidth * i - camera.X) * DataBase.MapScale[Scale] - DataBase.HexWidth * 3 / 2 * DataBase.MapScale[Scale] <= Game1._WindowSizeX); i++)
            {
                for (int j = Math.Max(0, (int)(camera.Y - DataBase.HexHeight * DataBase.MapScale[Scale]) / (DataBase.HexHeight * 3 / 4)); j < DataBase.MAP_MAX && (DataBase.HexHeight * 3 / 4 * j - camera.Y) * DataBase.MapScale[Scale] <= Game1._WindowSizeY; j++)
                {
                    Vector drawp = DataBase.WhereDisp(i, j, camera, Scale);
                    if (drawp.Y <= Game1._WindowSizeY && drawp.Y >= -DataBase.HexHeight * DataBase.MapScale[Scale]
                        && drawp.X <= Game1._WindowSizeX && drawp.X >= -DataBase.HexWidth * 3 / 2 * DataBase.MapScale[Scale] && data[i + (j + 1) /2, j].type != UnitType.NULL)
                    {
                        d.Draw(new Vector(drawp.X, drawp.Y) + new Vector(26 * DataBase.MapScale[Scale], 36 * DataBase.MapScale[Scale]), data[i + (j + 1) /2, j].type > 0 ? DataBase.myUnit_tex[(int)data[i + (j + 1) /2, j].type - 1] : DataBase.enemyUnit_tex[(int)data[i + (j + 1) /2, j].type + 5], DepthID.Player, (float)DataBase.MapScale[Scale]);
                    }
                }
            }
        }
        public void ChangeType(int x_index, int y_index, UnitType ut)
        {
            data[x_index + (y_index + 1) / 2, y_index] = new Unit(ut);
        }
        public UnitType GetType(int x_index, int y_index)
        {
            return data[x_index + (y_index + 1) / 2, y_index].type;
        }
    }
}
